using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using Meteosoft.Common;
// ReSharper disable LocalizableElement

#pragma warning disable IDE1006 // Naming Styles
namespace DispenserController
{
    public partial class Settings : Form
    {
        public bool Initialising { private get; set; }

        private bool m_usingSerial;
        private bool m_resetInProgress;
        public SerialPort ArduinoPort { private get; set; }
        public AsynchronousClient TcpClient { private get; set; }

        /// <summary> The event raised when a log message is to be handled </summary>
        public event EventHandler<LogMsgEventArgs> RaiseLogMessageEvent;
        /// <summary> The event raised when a the COM port changes </summary>
        public event EventHandler<EventArgs> RaiseComPortChanged;
        /// <summary> The event raised when a the local IP </summary>
        public event EventHandler<EventArgs> RaiseIPChanged;

        public Settings(bool usingSerial)
        {
            InitializeComponent();
            m_usingSerial = usingSerial;
        }

        private void Properties_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void LogMessage(string message, LogLevel level)
        {
            RaiseLogMessageEvent?.Invoke(null, new LogMsgEventArgs(message, level));
        }

        private void SendTCPMsgToArduino(string message)
        {
            if (TcpClient == null)
            {
                LogMessage("No ethernet connection available!", LogLevel.Error);
                return;
            }
            try
            {
                TcpClient.SendMessage(message);
                TcpClient.ReceiveMessage();
            }
            catch (Exception)
            {
                if (TcpClient.Client == null || !TcpClient.Client.Connected)
                {
                    string msg = "Could not send/receive messages...";
                    LogMessage(msg, LogLevel.Error);
                }
            }
        }

        private void checkSendReadings_CheckedChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.WATER_LEVEL_OUTPUT}:{(checkSendReadings.Checked ? "1" : "0")}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.WATER_LEVEL_OUTPUT}:{(checkSendReadings.Checked ? "1" : "0")}");
            }
        }

        private void checkFastReadings_CheckedChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.SEND_FAST_LEVEL_READINGS}:{(checkFastReadings.Checked ? "1" : "0")}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.SEND_FAST_LEVEL_READINGS}:{(checkFastReadings.Checked ? "1" : "0")}");
            }
        }

        private void checkSendRawValues_CheckedChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.SEND_RAW_VALUES}:{(checkSendRawValues.Checked ? "1" : "0")}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.SEND_RAW_VALUES}:{(checkSendRawValues.Checked ? "1" : "0")}");
            }
        }

        private void numericReadInterval_ValueChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.WATER_LEVEL_READ_SENSOR_INTERVAL}:{numericReadInterval.Value * 1000}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.WATER_LEVEL_READ_SENSOR_INTERVAL}:{numericReadInterval.Value * 1000}");
            }
        }

        private void numericFlushHour_ValueChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.AUTO_FLUSH_HOUR_SETTING}:{numericFlushHour.Value}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.AUTO_FLUSH_HOUR_SETTING}:{numericFlushHour.Value}");
            }
        }

        private void numericFlushMinute_ValueChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.AUTO_FLUSH_MINUTE_SETTING}:{numericFlushMinute.Value}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.AUTO_FLUSH_MINUTE_SETTING}:{numericFlushMinute.Value}");
            }
        }

        private void numericFlushPeriod_ValueChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.FLUSH_PERIOD}:{numericFlushPeriod.Value}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.FLUSH_PERIOD}:{numericFlushPeriod.Value}");
            }
        }

        private void comboComPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            RaiseComPortChanged?.Invoke(comboComPorts, new EventArgs());
        }

        private void textLocalIP_Leave(object sender, EventArgs e)
        {
            RaiseIPChanged?.Invoke(textLocalIP, new EventArgs());
        }

        private void checkInlet_CheckedChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.INLET_POWER_RELAY}:{(checkInlet.Checked ? "1" : "0")}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.INLET_POWER_RELAY}:{(checkInlet.Checked ? "1" : "0")}");
            }
        }

        private void checkOutlet_CheckedChanged(object sender, EventArgs e)
        {
            if (!Initialising)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.OUTLET_POWER_RELAY}:{(checkOutlet.Checked ? "1" : "0")}");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.OUTLET_POWER_RELAY}:{(checkOutlet.Checked ? "1" : "0")}");
            }
        }

        private void buttonSetTime_Click(object sender, EventArgs e)
        {
            if (m_usingSerial)
                ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.TIME}:{DateTime.Now:yyyy,MM,dd,HH,mm,ss}");
            else
                SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.TIME}:{DateTime.Now:yyyy,MM,dd,HH,mm,ss}");
        }

        private void buttonSyncSettings_Click(object sender, EventArgs e)
        {
            if (m_usingSerial)
                ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.INIT_CONTROLLER}:");
            else
                SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.INIT_CONTROLLER}:");
        }

        private void buttonFlush_Click(object sender, EventArgs e)
        {
            if (m_usingSerial)
                ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.DO_FLUSH_CYCLE_NOW}:");
            else
                SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.DO_FLUSH_CYCLE_NOW}:");
        }

        private void buttonSetArduinoIP_Click(object sender, EventArgs e)
        {
            string msg = textArduinoIP.Text + "|" + textArduinoDNS.Text + "|" + textArduinoGateway.Text;
            if (m_usingSerial)
                ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.CHANGE_IP_RESET}:{msg}");
            else
                SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.CHANGE_IP_RESET}:{msg}");
        }

        private void buttonResetNow_Click(object sender, EventArgs e)
        {
            if (!m_resetInProgress)
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int) DogDispenserController.ArduinoCommands.RESET_NOW}:");
                else
                    SendTCPMsgToArduino($"{(int) DogDispenserController.ArduinoCommands.RESET_NOW}:");
                m_resetInProgress = true;
                buttonResetNow.Text = "Cancel Reset";
                SynchronizationContext syncContext = SynchronizationContext.Current;
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Thread.Sleep(8000);
                    m_resetInProgress = false;
                    syncContext.Post(state =>
                    {
                        buttonResetNow.Text = "Reset Arduino";
                    }, null);
                });
            }
            else
            {
                if (m_usingSerial)
                    ArduinoPort?.WriteLine($"{(int)DogDispenserController.ArduinoCommands.CANCEL_RESET}:");
                else
                    SendTCPMsgToArduino($"{(int)DogDispenserController.ArduinoCommands.CANCEL_RESET}:");
                m_resetInProgress = false;
                buttonResetNow.Text = "Reset Arduino";
            }
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
