using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using Meteosoft.Common;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using ZedGraph;
// ReSharper disable CyclomaticComplexity
// ReSharper disable LocalizableElement
// ReSharper disable FieldCanBeMadeReadOnly.Local
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0019 // Use pattern matching

namespace DispenserController
{
    public partial class DogDispenserController : Form
    {
        /// <summary>The water level status</summary>
        public enum LevelStatus
        {
            /// <summary>Full</summary>
            FULL,
            /// <summary>Adequate</summary>
            ADEQUATE,
            /// <summary>Empty - needs filling</summary>
            EMPTY,
        }

        // MESSAGES/COMMANDS
        public enum ArduinoCommands
        {
            /// <summary>Arduino-&gt;Controller: Initialisation</summary>
            INIT_CONTROLLER,
            /// <summary>Set/Get the current date/time</summary>
            TIME,
            /// <summary>Here is a informational log message</summary>
            LOG_INFORMATION,
            /// <summary>Here is an warning log message</summary>
            LOG_WARNING,
            /// <summary>Here is an error log message</summary>
            LOG_ERROR,
            /// <summary>Activate/Deactivate the inlet solinoid power relay</summary>
            INLET_POWER_RELAY,
            /// <summary>Activate/Deactivate the outlet solinoid power relay</summary>
            OUTLET_POWER_RELAY,
            /// <summary>Get/set the flush period</summary>
            FLUSH_PERIOD,
            /// <summary>Start/stop the water level readings</summary>
            WATER_LEVEL_OUTPUT,
            /// <summary>Send one second level readings (not the usual one minute)</summary>
            SEND_FAST_LEVEL_READINGS,
            /// <summary>Do a manual flush cycle</summary>
            DO_FLUSH_CYCLE_NOW,
            /// <summary>Get/Set auto flush hour</summary>
            AUTO_FLUSH_HOUR_SETTING,
            /// <summary>Get/Set auto flush minute</summary>
            AUTO_FLUSH_MINUTE_SETTING,
            /// <summary>Send water level reading now (for network connection)</summary>
            SEND_WATER_LEVEL_NOW,
            /// <summary>Get/Set the sensor read interval</summary>
            WATER_LEVEL_READ_SENSOR_INTERVAL,
            /// <summary>Get/Set whether the Arduino should send raw water level values</summary>
            SEND_RAW_VALUES,
            /// <summary>Set the new IP, DNS and Gateway and reset</summary>
            CHANGE_IP_RESET,
            /// <summary>Reset the Arduino now</summary>
            RESET_NOW,
            /// <summary>Cancel manual reset of the Arduino</summary>
            CANCEL_RESET,
            /// <summary>Ping command to check TCP connection</summary>
            IS_ALIVE = 98,
            /// <summary>Invalid command</summary>
            INVALID_COMMAND = 99,
        }

        private class ComSettings
        {
            /// <summary>The COM port last used</summary>
            public string ComPort { get; set; } = "";
            /// <summary>The Ethernet IP last used</summary>
            public string IP { get; set; } = "";
            /// <summary>The Ethernet port last used</summary>
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int TCPPort { get; set; }
        }

        private bool m_usingSerial;
        private Settings m_settingsForm;

        private SerialPort m_arduinoPort;
        private static object m_syncLock = new object();
        private bool m_initNeeded = true;
        private DateTime m_lastDatePlotted;
        private IPointListEdit WaterLevelList { get; set; }
        private AsynchronousClient m_tcpClient;
        private System.Windows.Forms.Timer m_timer = new System.Windows.Forms.Timer();

        /// <summary>The COM port last used</summary>
        private string ComPort { get; set; } = "COM9";
        /// <summary>The Ethernet IP last used</summary>
        private string IP { get; set; } = "10.1.1.200";
        /// <summary>The Ethernet port last used</summary>
        private int TCPPort { get; } = 80;

        private bool m_tcpConnecting;

        public DogDispenserController()
        {
            m_usingSerial = Program.SerialMode;

            InitializeComponent();
            SetLogLevels();
            //BuildMessageMap();
            InitialiseGraph();

            m_settingsForm = new Settings(m_usingSerial);
            m_settingsForm.RaiseLogMessageEvent += (sender, args) => LogMessage(args.Message, args.Level);
            m_settingsForm.RaiseComPortChanged += ComPortChanged;
            m_settingsForm.RaiseIPChanged += IPChanged;

            string[] portNames = SerialPort.GetPortNames();
            foreach (string portName in portNames)
                m_settingsForm.comboComPorts.Items.Add(portName);

            toolStripLedBulbInlet.LedBulbControl.Color = Color.Red;
            toolStripLedBulbOutlet.LedBulbControl.Color = Color.Red;
            toolStripLedBulbInlet.LedBulbControl.On = false;
            toolStripLedBulbOutlet.LedBulbControl.On = false;
            if (m_usingSerial)
            {
                m_settingsForm.labelLocalIP.Enabled = false;
                m_settingsForm.textLocalIP.Enabled = false;
            }
            else
            {
                m_settingsForm.labelComPorts.Enabled = false;
                m_settingsForm.comboComPorts.Enabled = false;
            }

            string settingFile = @"C:\Temp\WaterLevelSettings.json";
            if (File.Exists(settingFile))
            {
                string settingData;
                using (StreamReader file = new StreamReader(settingFile))
                    settingData = file.ReadToEnd();
                ComSettings settings = JSONUtilities.DeserialiseJSONToClass<ComSettings>(settingData);
                m_settingsForm.comboComPorts.SelectedItem = settings.ComPort;
                m_settingsForm.textLocalIP.Text = settings.IP;
            }

            toolStripStatusLabelDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void ComPortChanged(object sender, EventArgs e)
        {
            ComPort = (string)m_settingsForm.comboComPorts.SelectedItem;
            bool reconnect = m_arduinoPort != null;
            m_arduinoPort?.Close();
            m_arduinoPort = null;
            toolStripLedBulbInlet.LedBulbControl.On = true;
            toolStripLedBulbOutlet.LedBulbControl.On = true;
            if (m_usingSerial && ConnectAndInitialiseArduino(reconnect))
            {
                Cursor = Cursors.WaitCursor;
                Thread.Sleep(1000);
                m_initNeeded = true;
                Cursor = Cursors.Default;
            }
            m_settingsForm.ArduinoPort = m_arduinoPort;
        }

        private void IPChanged(object sender, EventArgs e)
        {
            // Shut down previous connection
            m_tcpClient?.CloseConnection();
            m_tcpClient = null;

            // Create new connection
            IP = m_settingsForm.textLocalIP.Text;
            ConnectEthernet();
        }

        private void DogDispenserController_Load(object sender, EventArgs e)
        {
            m_timer.Interval = 1000;
            m_timer.Tick += Timer_Tick;
            m_timer.Start();

            if (m_usingSerial)
                Text += " [Using Serial Port]";
            else
                Text += " [Using Ethernet Port]";
            if (!m_usingSerial)
            {
                m_settingsForm.comboComPorts.Enabled = false;
                ConnectEthernet();
            }
        }

        /// <summary>Connect to the Arduino via TCP</summary>
        private void ConnectEthernet()
        {
            // If another thread is already attempting to connect, ignore...
            if (m_tcpConnecting)
                return;

            // Connect to the Arduino via ethernet
            ThreadPool.QueueUserWorkItem(_ =>
            {
                m_tcpConnecting = true;
                m_tcpClient = new AsynchronousClient(IP, TCPPort);
                m_tcpClient.RaiseDataReceivedEvent += TCPDataReceived;
                m_tcpClient.RaiseLogMessageEvent += LogMessageEventReceived;
                m_tcpClient.RaiseExceptionEvent += ExceptionReceived;
                try
                {
                    m_tcpClient.OpenAndConnect();
                    m_initNeeded = true;
                }
                catch (Exception)
                {
                    //string msg = $"Could not connect to {m_tcpClient.IP}:{m_tcpClient.Port}...";
                    //LogMessage(msg, LogLevel.Error);
                    m_tcpClient?.CloseConnection();
                    m_tcpClient = null;
                }
                m_settingsForm.TcpClient = m_tcpClient;
                m_tcpConnecting = false;
            });
        }

        /// <summary>Called to log messages, from another thread, to call LogMessage</summary>
        private delegate void CallLogMessage(string message, LogLevel logLevel, string dateTime);

        /// <summary>Called to log messages (Thread safe!)</summary>
        private void LogMessage(string message, LogLevel logLevel, string dateTime = "")
        {
            if (InvokeRequired)
                BeginInvoke(new CallLogMessage(LogMessage), message, logLevel, dateTime);
            else
            {
                // Log the message
                logMessageView.LogMessage(message, (int) logLevel, dateTime);
            }
        }

        /// <summary>
        /// Connect to and initialise the Arduino's serial connection
        /// </summary>
        private bool ConnectAndInitialiseArduino(bool reconnect = false)
        {
            bool success = true;
            lock (m_syncLock)
            {
                // If reconnecting, cleanup any existing failed connection
                if (reconnect && m_arduinoPort != null)
                {
                    m_arduinoPort.Close();
                    m_arduinoPort = null;
                }

                // Connect the serial port...
                try
                {
                    // Initialise and open serial port
                    if (ComPort != "")
                    {
                        m_arduinoPort = new SerialPort(ComPort, 9600, Parity.None, 8, StopBits.One) {ReadTimeout = 5000};
                        m_arduinoPort.DataReceived += SerialDataReceived;
                        m_arduinoPort.Open();
                    }
                }
                catch
                {
                    if (reconnect)
                        LogMessage("Arduino reconnection attempt failed!", LogLevel.Error);
                    m_arduinoPort?.Close();
                    m_arduinoPort = null;
                }

                if (m_arduinoPort == null)
                {
                    if (reconnect)
                        LogMessage("Could not reconnect to the Arduino on port: " + ComPort, LogLevel.Error);
                    else
                        LogMessage("Could not open the connection to the Arduino on port: " + ComPort, LogLevel.Error);
                    success = false;
                }
                else
                {
                    LogMessage("Connected to Arduino on port: " + ComPort, LogLevel.Information);
                }

            }
            return success;
        }

        /// <summary>
        /// Called when the Arduino interface's TCP port sends a message asynchronously
        /// </summary>
        private void TCPDataReceived(object sender, DataTransferEventArgs e)
        {
            string data = e.JSONMessage.Trim();
            if (string.IsNullOrEmpty(data))
                return;
            data = data.Replace("\r\n\r\n", "\n");
            string[] messages = data.Split('\n');
            foreach (string dataLine in messages)
            {
                try
                {
                    ArduinoCommands command = ArduinoCommands.INVALID_COMMAND;
                    string message = "";
                    int colonPos = dataLine.IndexOf(':');
                    if (colonPos != -1 && colonPos < 4)
                    {
                        int cmd = Convert.ToInt32(dataLine.Substring(0, colonPos));
                        command = (ArduinoCommands) cmd;
                        message = dataLine.Substring(dataLine.IndexOf(':') + 1);
                    }
                    switch (command)
                    {
                        case ArduinoCommands.INIT_CONTROLLER:
                            InitControllerWithArduinoState(message);
                            LogMessage("Initialisation received...", LogLevel.Information);
                            break;
                        case ArduinoCommands.TIME:
                            SendTCPMsgToArduino($"{(int) ArduinoCommands.TIME}:{DateTime.Now:yyyy,MM,dd,HH,mm,ss}");
                            break;
                        case ArduinoCommands.LOG_INFORMATION:
                            LogMessage(message, LogLevel.Information);
                            break;
                        case ArduinoCommands.LOG_WARNING:
                            LogMessage(message, LogLevel.Warning);
                            break;
                        case ArduinoCommands.LOG_ERROR:
                            LogMessage(message, LogLevel.Error);
                            break;
                        case ArduinoCommands.INLET_POWER_RELAY:
                            UpdateInletControls(message == "0");
                            break;
                        case ArduinoCommands.OUTLET_POWER_RELAY:
                            UpdateOutletControls(message == "0");
                            break;
                        case ArduinoCommands.WATER_LEVEL_OUTPUT:
                            UpdateValueAndGraph(message);
                            break;
                        case ArduinoCommands.IS_ALIVE:
                            break;
                        default:
                            LogMessage("Invalid command:" + dataLine, LogLevel.Error);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("TCPDataReceived() reported an exception: " + ex.Message, LogLevel.Error);
                }
            }
        }

        /// <summary>
        /// Called when the Arduino interface's serial port sends a message asynchronously
        /// </summary>
        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            try
            {
                while (m_arduinoPort.BytesToRead != 0)
                {
                    string data = m_arduinoPort.ReadLine();
                    data = data.TrimEnd();
                    if (string.IsNullOrEmpty(data))
                        continue;
                    ArduinoCommands command;
                    string message;
                    if (data.IndexOf(':') != -1)
                    {
                        if (data[0] < '0' || data[0] > '9')
                        {
                            command = ArduinoCommands.LOG_INFORMATION;
                            message = data;
                        }
                        else
                        {
                            int cmd = Convert.ToInt32(data.Substring(0, data.IndexOf(':')));
                            command = (ArduinoCommands) cmd;
                            message = data.Substring(data.IndexOf(':') + 1);
                        }
                    }
                    else
                    {
                        command = ArduinoCommands.LOG_WARNING;
                        message = data;
                    }
                    switch (command)
                    {
                        case ArduinoCommands.INIT_CONTROLLER:
                            InitControllerWithArduinoState(message);
                            LogMessage("Initialisation received...", LogLevel.Information);
                            break;
                        case ArduinoCommands.TIME:
                            m_arduinoPort?.WriteLine($"{(int)ArduinoCommands.TIME}:{DateTime.Now:yyyy,MM,dd,HH,mm,ss}");
                            break;
                        case ArduinoCommands.LOG_INFORMATION:
                            LogMessage(message, LogLevel.Information);
                            break;
                        case ArduinoCommands.LOG_WARNING:
                            LogMessage(message, LogLevel.Warning);
                            break;
                        case ArduinoCommands.LOG_ERROR:
                            LogMessage(message, LogLevel.Error);
                            break;
                        case ArduinoCommands.INLET_POWER_RELAY:
                            UpdateInletControls(message == "0");
                            break;
                        case ArduinoCommands.OUTLET_POWER_RELAY:
                            UpdateOutletControls(message == "0");
                            break;
                        case ArduinoCommands.WATER_LEVEL_OUTPUT:
                            UpdateValueAndGraph(message);
                            break;
                        case ArduinoCommands.IS_ALIVE:
                            break;
                        default:
                            LogMessage("Invalid command:" + data, LogLevel.Error);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                LogMessage("SerialDataReceived() reported an exception: " + e.Message, LogLevel.Error);
            }
        }

        /// <summary>Called to update GUI, from another thread, to call InitControllerWithArduinoState</summary>
        private delegate void CallInitControllerWithArduinoState(string message);

        /// <summary>Called to update GUI</summary>
        private void InitControllerWithArduinoState(string message)
        {
            if (InvokeRequired)
                BeginInvoke(new CallInitControllerWithArduinoState(InitControllerWithArduinoState), message);
            else
            {
                string[] values = message.Split(',');
                if (values.Length == 12)
                {
                    m_settingsForm.Initialising = true;
                    UpdateInletControls(values[0] == "0");
                    UpdateOutletControls(values[1] == "0");
                    m_settingsForm.checkSendReadings.Checked = values[2] == "1";
                    m_settingsForm.checkFastReadings.Checked = values[3] == "1";
                    UpdateOtherControls(Convert.ToInt32(values[4]), Convert.ToInt32(values[5]), Convert.ToInt32(values[6]),  
                        Convert.ToInt32(values[7]), values[9], values[10], values[11]);
                    m_settingsForm.checkSendRawValues.Checked = values[8] == "1";
                    m_settingsForm.Initialising = false;
                }
            }
        }

        /// <summary>Called to update GUI, from another thread, to call UpdateValueAndGraph</summary>
        private delegate void CallUpdateValueAndGraph(string message);

        /// <summary>Called to update GUI</summary>
        private void UpdateValueAndGraph(string message)
        {
            if (InvokeRequired)
                BeginInvoke(new CallUpdateValueAndGraph(UpdateValueAndGraph), message);
            else
            {
                if (message.IndexOf('@') != -1)
                {
                    string value = message.Substring(0, message.IndexOf('@'));
                    LevelStatus status = (LevelStatus)Convert.ToInt32(value);
                    DateTime dateTime = DateTime.Parse(message.Substring(message.IndexOf('@') + 1)); // DateTime.Now;
                    labelCurrentLevel.Text = status + " [" + (dateTime + TimeSpan.FromSeconds(10)).ToString("dd HH:mm") + "]";
                    LogMessage($"Level: {status} @ {dateTime:dd HH:mm:ss}", LogLevel.Information);

                    // Graph the val against time
                    if (dateTime != m_lastDatePlotted)
                    {
                        try
                        {
                            double graphVal = (status == LevelStatus.FULL) ? 99 : (status == LevelStatus.ADEQUATE ? 50 : 1);
                            PointPair element = new PointPair(dateTime.ToOADate(), graphVal);
                            WaterLevelList.Add(element);
                            zedGraphDispenserLevel.GraphPane.XAxis.Scale.Min = (dateTime - TimeSpan.FromHours(24.0)).ToOADate();
                            zedGraphDispenserLevel.GraphPane.XAxis.Scale.Max = dateTime.ToOADate();
                            zedGraphDispenserLevel.AxisChange();
                            zedGraphDispenserLevel.Refresh();
                        }
                        catch (Exception)
                        {
                            //LogMessage("UpdateValueAndGraph() reported an exception: " + e.Message, LogLevel.Error);
                        }
                        m_lastDatePlotted = dateTime;
                    }
                }
            }
        }

        /// <summary>Called to update GUI, from another thread, to call UpdateInletControls</summary>
        private delegate void CallUpdateInletControls(bool off);

        /// <summary>Called to update GUI</summary>
        private void UpdateInletControls(bool off)
        {
            if (InvokeRequired)
                BeginInvoke(new CallUpdateInletControls(UpdateInletControls), off);
            else
            {
                m_settingsForm.Initialising = true;
                m_settingsForm.checkInlet.Checked = !off;
                toolStripLedBulbInlet.LedBulbControl.On = true;
                toolStripLedBulbInlet.LedBulbControl.Color = off ? Color.Red : Color.FromArgb(255, 153, 255, 54);
                m_settingsForm.Initialising = false;
            }
        }

        /// <summary>Called to update GUI, from another thread, to call UpdateOutletControls</summary>
        private delegate void CallUpdateOutletControls(bool off);

        /// <summary>Called to update GUI</summary>
        private void UpdateOutletControls(bool off)
        {
            if (InvokeRequired)
                BeginInvoke(new CallUpdateOutletControls(UpdateOutletControls), off);
            else
            {
                m_settingsForm.Initialising = true;
                m_settingsForm.checkOutlet.Checked = !off;
                toolStripLedBulbOutlet.LedBulbControl.On = true;
                toolStripLedBulbOutlet.LedBulbControl.Color = off ? Color.Red : Color.FromArgb(255, 153, 255, 54);
                m_settingsForm.Initialising = false;
            }
        }

        /// <summary>Called to update GUI, from another thread, to call UpdateOutletControls</summary>
        private delegate void CallUpdateOtherControls(int seconds, int flushHour, int flushMinute, 
            int interval, string ip, string dns, string gateway);

        /// <summary>Called to update GUI</summary>
        private void UpdateOtherControls(int seconds, int flushHour, int flushMinute, 
            int interval, string ip, string dns, string gateway)
        {
            if (InvokeRequired)
                BeginInvoke(new CallUpdateOtherControls(UpdateOtherControls), seconds, flushHour, flushMinute, interval, ip, dns, gateway);
            else
            {
                m_settingsForm.Initialising = true;
                m_settingsForm.numericFlushPeriod.Value = seconds;
                m_settingsForm.numericFlushHour.Value = flushHour;
                m_settingsForm.numericFlushMinute.Value = flushMinute;
                // ReSharper disable once PossibleLossOfFraction
                m_settingsForm.numericReadInterval.Value = interval / 1000;
                m_settingsForm.textArduinoIP.Text = ip;
                m_settingsForm.textArduinoDNS.Text = dns;
                m_settingsForm.textArduinoGateway.Text = gateway;
                m_settingsForm.Initialising = false;
            }
        }

        private void SetLogLevels()
        {
            // Set default log levels from LogLevel enums
            logMessageView.ShowFilterRadioButtons = true;
            logMessageView.ResetLogLevelDefinitions();
            foreach (LogLevel logLevel in Enum.GetValues(typeof(LogLevel)))
            {
                string description = "";
                DisplayAttribute[] descriptionAttributes = logLevel.GetType().GetField(logLevel.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (descriptionAttributes != null)
                    description = descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : logLevel.ToString();
                switch (logLevel)
                {
                    case LogLevel.All:
                        logMessageView.SetNewLogLevelEntry((int) logLevel, logLevel.ToString(),
                            GetLogLevelColour(logLevel), description, (int) logLevel < 8, false, true);
                        break;
                    case LogLevel.Information:
                    case LogLevel.Warning:
                    case LogLevel.Error:
                        logMessageView.SetNewLogLevelEntry((int) logLevel, logLevel.ToString(),
                            GetLogLevelColour(logLevel), description, (int) logLevel < 8, true);
                        break;
                }
            }

            // Set log level parameters
            logMessageView.KeepLogOnFile = true;
            logMessageView.LogFilePath = @"C:\Temp\";
            logMessageView.FileNameAppendage = "Dispenser";
            logMessageView.ProgramTitle = "Dispenser Water Level";
            logMessageView.InitialLogLevelFilter = (int) LogLevel.All;
        }

        /// <summary>Called to get the colour associated with the given log level</summary>
        private static Color GetLogLevelColour(LogLevel level)
        {
            Color colour = Color.Black;
            switch (level)
            {
                case LogLevel.Warning:
                    colour = Color.Brown;
                    break;
                case LogLevel.Error:
                    colour = Color.Red;
                    break;
                case LogLevel.Information:
                    colour = Color.Black;
                    break;
            }
            return colour;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            toolStripStatusLabelDateTime.Text = now.ToString("yyyy-MM-dd HH:mm:ss");

            // Reconnect to the network, if required, every 30 seconds
            if (m_tcpClient == null && now.Second % 30 == 0)
                ConnectEthernet();

            // Send alive messages to the Arduino, every 15 seconds
            if (!m_usingSerial && now.Second % 15 == 0 && m_tcpClient != null)
                SendTCPMsgToArduino($"{(int)ArduinoCommands.IS_ALIVE}:1");

            // Sync the controller with the Arduino settings
            if (m_initNeeded)
            {
                if (m_usingSerial)
                    m_arduinoPort?.WriteLine($"{(int)ArduinoCommands.INIT_CONTROLLER}:");
                else
                {
                    SendTCPMsgToArduino($"{(int)ArduinoCommands.INIT_CONTROLLER}:");
                    SendTCPMsgToArduino($"{(int)ArduinoCommands.TIME}:{DateTime.Now:yyyy,MM,dd,HH,mm,ss}");
                    SendTCPMsgToArduino($"{(int)ArduinoCommands.SEND_WATER_LEVEL_NOW}:");
                }
                m_initNeeded = false;
            }

            // Try to receive a messsage
            if (!m_usingSerial)
            {
                try
                {
                    if (!m_tcpConnecting && m_tcpClient != null)
                    {
                        if (m_tcpClient.Client.Connected)
                            m_tcpClient?.ReceiveMessage();
                        else
                        {
                            m_tcpClient?.CloseConnection();
                            m_tcpClient = null;
                        }
                    }
                }
                catch
                {
                    m_tcpClient?.CloseConnection();
                    m_tcpClient = null;
                }
            }

            // Every 5 minutes save graph data
            if (now.Minute % 5 == 0 && now.Second == 0)
            {
                // Save the graph data
                SaveGraphData();
            }

            // At midnight, set the date/time on the Arduino
            if (now.Hour == 0 && now.Minute == 0 && now.Second == 0)
            {
                // Set the date/time
                SendTCPMsgToArduino($"{(int)ArduinoCommands.TIME}:{DateTime.Now:yyyy,MM,dd,HH,mm,ss}");
            }
        }

        private void SendTCPMsgToArduino(string message)
        {
            if (m_tcpClient == null)
            {
                LogMessage("No ethernet connection available!", LogLevel.Error);
                return;
            }
            try
            {
                m_tcpClient.SendMessage(message);
                m_tcpClient.ReceiveMessage();
            }
            catch (Exception)
            {
                if (m_tcpClient.Client.Connected)
                {
                    string msg = "Could not send/receive messages...";
                    LogMessage(msg, LogLevel.Error);
                }
            }
        }

        private void ExceptionReceived(object sender, EventArgs e)
        {
            // Handle situation when socket experiences a catastrophic exception
            m_tcpClient?.CloseConnection();
            m_tcpClient = null;
        }

        private void LogMessageEventReceived(object sender, LogMsgEventArgs e)
        {
            LogMessage(e.Message, e.Level);
        }

        private void InitialiseGraph()
        {
            // Setup the graph
            zedGraphDispenserLevel.IsEnableHZoom = false;
            zedGraphDispenserLevel.IsEnableVZoom = false;
            zedGraphDispenserLevel.IsEnableHPan = false;
            zedGraphDispenserLevel.IsEnableVPan = false;
            zedGraphDispenserLevel.IsEnableHEdit = false;
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.MajorStepAuto = true;
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.MinorStepAuto = true;
            zedGraphDispenserLevel.GraphPane.Legend.Position = LegendPos.TopFlushLeft;
            zedGraphDispenserLevel.GraphPane.Legend.IsShowLegendSymbols = false;
            zedGraphDispenserLevel.GraphPane.Legend.IsVisible = true;
            zedGraphDispenserLevel.GraphPane.Fill = new Fill(Color.Gainsboro, Color.White, 135.0F);
            zedGraphDispenserLevel.GraphPane.Chart.Fill = new Fill(Color.Gainsboro, Color.White, 315.0F);

            // Setup the X-Axis parameters
            zedGraphDispenserLevel.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.Format = "HH:mm";
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.MinAuto = false;
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.MaxAuto = false;
            zedGraphDispenserLevel.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zedGraphDispenserLevel.GraphPane.XAxis.MajorGrid.DashOn = 2F;
            zedGraphDispenserLevel.GraphPane.XAxis.MajorGrid.Color = Color.Gray;
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.Min = (DateTime.Now - TimeSpan.FromHours(24.0)).ToOADate();
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.Max = DateTime.Now.ToOADate();
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.MajorStep = 1;
            zedGraphDispenserLevel.GraphPane.XAxis.Scale.MinorStep = 0;

            // Setup the Y-Axis parameters
            zedGraphDispenserLevel.GraphPane.YAxis.MajorGrid.IsVisible = true;
            zedGraphDispenserLevel.GraphPane.YAxis.MajorGrid.DashOn = 2F;
            zedGraphDispenserLevel.GraphPane.YAxis.MajorGrid.Color = Color.Gray;
            zedGraphDispenserLevel.GraphPane.YAxis.Scale.Min = 0.0;
            zedGraphDispenserLevel.GraphPane.YAxis.Scale.Max = 100.0;
            zedGraphDispenserLevel.GraphPane.YAxis.Scale.MajorStep = 10;
            zedGraphDispenserLevel.GraphPane.YAxis.Scale.MinorStep = 5;

            // Set the initial graph title and axis labels
            zedGraphDispenserLevel.GraphPane.Title.Text = "Water Level";
            zedGraphDispenserLevel.GraphPane.Title.FontSpec.Size = 20;
            zedGraphDispenserLevel.GraphPane.XAxis.Title.Text = "Time";
            zedGraphDispenserLevel.GraphPane.YAxis.Title.Text = "%";

            // Add a water level curve
            zedGraphDispenserLevel.GraphPane.CurveList.Clear();
            LineItem line = zedGraphDispenserLevel.GraphPane.AddCurve("Water Level", null, null, Color.Red, SymbolType.None);
            line.Label.IsVisible = false;
            line.Line.IsSmooth = true;
            line.Line.SmoothTension = 0.5F;
            line.Line.Width = 2.0F;
            WaterLevelList = line.Points as IPointListEdit;

            // Read in dome temp input values from file
            WaterLevelList?.Clear();
            DateTime graphCutoff = DateTime.Now - TimeSpan.FromHours(24);
            if (WaterLevelList != null && File.Exists(@"C:\Temp\WaterLevel.csv"))
            {
                using (StreamReader file = new StreamReader(@"C:\Temp\WaterLevel.csv"))
                {
                    string fileLine;
                    while ((fileLine = file.ReadLine()) != null)
                    {
                        try
                        {
                            string[] lines = fileLine.Split(',');
                            double dateTime = Convert.ToDouble(lines[0]);
                            if (DateTime.FromOADate(dateTime) >= graphCutoff)
                            {
                                PointPair element = new PointPair(dateTime, Convert.ToDouble(lines[1]));
                                WaterLevelList.Add(element);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMessage("Error reading water levels from file - " + ex.Message, LogLevel.Error);
                        }
                    }
                    TrimGraphData(DateTime.Now, WaterLevelList);
                }
            }
            zedGraphDispenserLevel.AxisChange();
            zedGraphDispenserLevel.Refresh();
        }

        /// <summary>
        /// Save the graph results to file
        /// </summary>
        private void SaveGraphData()
        {
            // Save graph data
            TrimGraphData(DateTime.Now, WaterLevelList);
            using (StreamWriter file = new StreamWriter(@"C:\Temp\WaterLevel.csv"))
            {
                for (int i = 0; i < WaterLevelList.Count; i++)
                {
                    file.WriteLine(WaterLevelList[i].X + "," + WaterLevelList[i].Y);
                }
            }
        }

        private void DogDispenserController_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the socket
            try
            {
                m_tcpClient.CloseConnection();
            }
            catch (Exception)
            {
                if (m_tcpClient != null)
                {
                    string msg = $"Could not close the connection to {m_tcpClient.IP}:{m_tcpClient.Port}...";
                    LogMessage(msg, LogLevel.Error);
                }
            }

            ComSettings settings = new ComSettings
            {
                ComPort = (string)m_settingsForm.comboComPorts.SelectedItem,
                IP = m_settingsForm.textLocalIP.Text,
                TCPPort = 80
            };
            string data = JSONUtilities.SerialiseClassToJSON(settings);
            using (StreamWriter file = new StreamWriter(@"C:\Temp\WaterLevelSettings.json"))
            {
                file.Write(data);
            }

            // Save the graph data
            SaveGraphData();
        }

        /// <summary>
        /// Trims the graph data
        /// </summary>
        /// <param name="now">Now</param>
        /// <param name="data">The data to trim</param>
        private void TrimGraphData(DateTime now, IPointListEdit data)
        {
            DateTime graphCutoff = now - TimeSpan.FromHours(24);
            for (int i = 0; i < data.Count; i++)
            {
                if (DateTime.FromOADate(data[i].X) < graphCutoff)
                    data.RemoveAt(i);
                else
                    break;
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            m_settingsForm.ShowDialog(this);
            Show();
        }
    }
}
#pragma warning restore IDE0019 // Use pattern matching
#pragma warning restore IDE1006 // Naming Styles

