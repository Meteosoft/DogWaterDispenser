using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Meteosoft.Common;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable LocalizableElement
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedMember.Global

namespace DispenserController
{
    // State object for receiving data from remote device
    public class StateObject : IDisposable
    {
        // Client socket
        public Socket workSocket;

        // Size of receive buffer.  
        public const int BufferSize = 256;

        // Receive buffer
        public byte[] buffer = new byte[BufferSize];

        // Received data string
        public StringBuilder sb = new StringBuilder();

        ~StateObject()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                workSocket.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class AsynchronousClient
    {
        // The port number for the remote device
        public int Port { get; set; }

        // The response from the remote device
        public string Response { get; set; } = string.Empty;

        // The IP address
        public string IP { get; set; }

        // Any error
        public bool ErrorOccurred { get; set; }

        #region Events
        /// <summary> The event raised when a socket connection is established </summary>
        public event EventHandler<EventArgs> RaiseConnectEvent;

        /// <summary> The event raised when a socket is disconnected </summary>
        public event EventHandler<EventArgs> RaiseDisconnectEvent;

        /// <summary> The event raised when data is received from the socket </summary>
        public event EventHandler<DataTransferEventArgs> RaiseDataReceivedEvent;

        /// <summary> The event raised when a log message is available from the socket class </summary>
        public event EventHandler<LogMsgEventArgs> RaiseLogMessageEvent;
        #endregion

        // ManualResetEvent instances signal completion
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        private Socket m_client;

        public AsynchronousClient(string ip, int port)
        {
            IP = ip;
            Port = port;
        }

        public void SendClientOneMessage(string message)
        {
            // Connect to a remote device
            try
            {
                OpenAndConnect();
                SendMessage(message);
                ReceiveMessage();
                CloseConnection();
            }
            catch (Exception)
            {
                string msg = $"Could not connect to {IP}:{Port}...";
                RaiseLogMessageEvent?.Invoke(null, new LogMsgEventArgs(msg, LogLevel.Error));
                ErrorOccurred = true;
            }
        }

        public void CloseConnection()
        {
            // Release the socket
            m_client.Shutdown(SocketShutdown.Both);
            m_client.Close();
            m_client.Dispose();
            m_client = null;
            RaiseDisconnectEvent?.Invoke(null, new EventArgs());
        }

        public void ReceiveMessage()
        {
            // Receive the response from the remote device
            Receive(m_client);
            receiveDone.WaitOne(2000);
        }

        public void SendMessage(string message)
        {
            // Send data to the remote device
            message += "\n";
            Send(m_client, message);
            sendDone.WaitOne();
        }

        public void OpenAndConnect()
        {
// Establish the remote endpoint for the socket
            // The name of the remote device is "10.1.1.200"  
#pragma warning disable 618
            IPHostEntry ipHostInfo = Dns.GetHostByName(IP);
#pragma warning restore 618
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

            // Create a TCP/IP socket
            m_client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint
            m_client.BeginConnect(remoteEP, ConnectCallback, m_client);
            connectDone.WaitOne(2000);
            RaiseConnectEvent?.Invoke(null, new EventArgs());
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                Socket client = (Socket) ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine($"Socket connected to {0}", client.RemoteEndPoint);

                // Signal that the connection has been made
                connectDone.Set();
            }
            catch (Exception)
            {
                string msg = $"Connected host has failed to respond on {IP}:{Port}";
                RaiseLogMessageEvent?.Invoke(null, new LogMsgEventArgs(msg, LogLevel.Error));
                ErrorOccurred = true;
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject {workSocket = client};

                // Begin receiving the data from the remote device
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                RaiseLogMessageEvent?.Invoke(null, new LogMsgEventArgs(e.Message, LogLevel.Error));
                ErrorOccurred = true;
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object
                StateObject state = (StateObject) ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    Response = state.sb.ToString();
                    receiveDone.Set();
                    RaiseDataReceivedEvent?.Invoke(null, new DataTransferEventArgs(Response, client));

                    // Get the rest of the data
                    // client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
                }
                else
                {
                    // All the data has arrived; put it in response
                    if (state.sb.Length > 1)
                    {
                        Response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received
                    receiveDone.Set();
                }
                //state.workSocket.Dispose();
            }
            catch (Exception e)
            {
                RaiseLogMessageEvent?.Invoke(null, new LogMsgEventArgs(e.Message, LogLevel.Error));
                ErrorOccurred = true;
            }
        }

        private void Send(Socket client, string data)
        {
            // Convert the string data to byte data using ASCII encoding
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device
            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                //Socket client = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device
                //int bytesSent = client.EndSend(ar);

                // Signal that all bytes have been sent
                sendDone.Set();
            }
            catch (Exception e)
            {
                RaiseLogMessageEvent?.Invoke(null, new LogMsgEventArgs(e.Message, LogLevel.Error));
                ErrorOccurred = true;
            }
        }
    }
}

