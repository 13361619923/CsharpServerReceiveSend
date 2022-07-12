using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Multi_Con_S_01
{
    class Client
    {
        public string ID
        {
            get;
            private set;
        }

        public IPEndPoint EndPoint
        {
            get;
            private set;
        }

        Socket sck;
        public Client(Socket accepted)
        {
            sck = accepted;
            ID = Guid.NewGuid().ToString();
            EndPoint = (IPEndPoint)sck.RemoteEndPoint;
            sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);// 这个是接收消息的触发
        }

        void callback(IAsyncResult ar)
        {
            try
            {
                sck.EndReceive(ar);

                byte[] buf = new byte[8192];

                int rec = sck.Receive(buf, buf.Length, 0);

                if (rec > 0 && rec < buf.Length)
                {
                    Array.Resize<byte>(ref buf, rec);
                }
                else
                {
                    // 在接收到空数据时关闭连接
                    Close();

                    if (Disconnected != null)
                    {
                        Disconnected(this);
                    }
                }

                if (Received != null)
                {
                    Received(this, buf);
                }

                sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
            }
            catch (Exception)
            {
                Console.WriteLine("在接收消息时遇到错误");
                Close();

                if (Disconnected != null)
                {
                    Disconnected(this);
                }
            }
        }

        public void Close()
        {
            sck.Close();
            sck.Dispose();
        }

        public void Send(string str)
        {
            sck.Send(Encoding.UTF8.GetBytes(str));
        }

        public delegate void ClientReceivedHandler(Client sender, byte[] data);
        public delegate void ClientDisconnectedHandler(Client sender);
        public delegate void DataTransmit(string str, string guid1, string guid2);

        public event ClientReceivedHandler Received;
        public event ClientDisconnectedHandler Disconnected;
        public event DataTransmit Transmit;
    }
}
