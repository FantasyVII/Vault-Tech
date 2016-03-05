/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 14/April/2014
 * Date Moddified :- 25/November/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace VaultTech.Network
{
    public class Server
    {
        Socket MainSocket;
        List<Socket> AcceptSockets;

        public int TotalNumberOfConnectedClients;

        int ReceivedDataSize;

        public string StatusReport;
        public bool ClientConnected;

        bool Send;

        SocketError socketError;

        public Server()
        {
            AcceptSockets = new List<Socket>();

            Send = false;
            ClientConnected = false;
        }

        public void Initialize()
        {
            MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MainSocket.Blocking = false;
            MainSocket.Bind(new IPEndPoint(IPAddress.Any, 50000));

            StatusReport = "Waiting for connection...";
            Console.WriteLine(StatusReport);
        }

        public void Listening()
        {
            MainSocket.Listen(50);
        }

        public void AcceptConnection()
        {
            try
            {
                AcceptSockets.Add(MainSocket.Accept());
                StatusReport = "Client " + AcceptSockets.Count + " Connected !!";
                ClientConnected = true;
                Send = true;
                StatusReport.SerializePacket();
                TotalNumberOfConnectedClients = AcceptSockets.Count;
            }
            catch (SocketException SocketEx)
            {
                if (SocketEx.SocketErrorCode != SocketError.WouldBlock)
                    StatusReport = SocketEx.ToString();
            }
        }

        void SendPacket()
        {
            if (Send)
            {
                for (int i = 0; i < AcceptSockets.Count; i++)
                {
                    AcceptSockets[i].Send(Encoding.UTF8.GetBytes(Packet.OutBuffer), 0, Encoding.UTF8.GetBytes(Packet.OutBuffer).Length, 0, out socketError);
                    StatusReport = socketError.ToString();
                }

                Send = false;
            }
        }

        void ReceivePacket()
        {
            for (int i = 0; i < AcceptSockets.Count; i++)
            {
                byte[] Buffer = new byte[100000];
                ReceivedDataSize = AcceptSockets[i].Receive(Buffer, 0, Buffer.Length, SocketFlags.None, out socketError);

                if (socketError != SocketError.WouldBlock)
                {
                    Packet.InBuffer = Encoding.ASCII.GetString(Buffer, 0, ReceivedDataSize);
                    StatusReport = socketError.ToString();

                    Console.WriteLine("Received" + StatusReport + Packet.InBuffer);

                    Send = true;
                }

                if (socketError == SocketError.ConnectionAborted || socketError == SocketError.ConnectionReset)
                {
                    StatusReport = "Client " + (i + 1) + " Disconnected !";
                    AcceptSockets.RemoveAt(i);
                }
            }
        }

        public void MainLoop()
        {
            Initialize();

            while (true)
            {

                Listening();
                AcceptConnection();

                ReceivePacket();
                SendPacket();
                
                Thread.Sleep(50);
            }
        }

        public void CloseConnection()
        {
        }
    }
}