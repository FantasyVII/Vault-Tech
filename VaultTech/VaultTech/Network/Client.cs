/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 14/April/2014
 * Date Moddified :- 25/November/2015
 * </Copyright>
 */

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Newtonsoft.Json;

namespace VaultTech.Network
{
    public class Client
    {
        Socket MainSocket;
        IPEndPoint ServerIPEndPoint;

        SocketError socketError;

        string ServerIpAddress;

        public string StatusReport;

        public bool Send;
        public bool Received;

        public Client(string ServerIpAddress)
        {
            this.ServerIpAddress = ServerIpAddress;

            Send = false;
            Received = false;
        }

        public void Initialize()
        {
            MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MainSocket.Blocking = false;

            ServerIPEndPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), 50000);
        }

        void Connect()
        {
            while (!MainSocket.Connected)
            {
                Thread.Sleep(100);

                try
                {
                    MainSocket.Connect(ServerIPEndPoint);
                }
                catch (SocketException SocketEx)
                {
                    if (SocketEx.SocketErrorCode != SocketError.WouldBlock)
                        StatusReport = SocketEx.ToString();
                }

                if (MainSocket.Connected)
                    StatusReport = "Game Client Connected to " + ServerIPEndPoint.ToString();
            }
        }

        void SendPacket()
        {
            if (Send)
            {
                MainSocket.Send(Encoding.UTF8.GetBytes(Packet.OutBuffer), 0, Encoding.UTF8.GetBytes(Packet.OutBuffer).Length, 0, out socketError);
                StatusReport = "Sent Packet successfully";

                StatusReport = socketError.ToString();

                Send = false;
            }
        }

        void ReceivePacket()
        {
            byte[] Buffer = new byte[100000];

            int ReceivedDataSize = MainSocket.Receive(Buffer, 0, Buffer.Length, SocketFlags.None, out socketError);

            if (ReceivedDataSize > 0)
            {
                Packet.InBuffer = Encoding.ASCII.GetString(Buffer, 0, ReceivedDataSize);
                Received = true;
            }

            if (socketError != SocketError.WouldBlock)
                StatusReport = socketError.ToString();
        }

        public void MainLoop()
        {
            Initialize();
            Connect();

            while (true)
            {
                SendPacket();
                ReceivePacket();

                Thread.Sleep(50);
            }
        }

        public void CloseConnection()
        {
        }
    }
}