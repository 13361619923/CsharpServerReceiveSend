using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Con_S
{
    internal static class Program
    {
        static Listener l;
        static List<Socket> sockets;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            sockets = new List<Socket>();
            l = new Listener(26950);
            l.SocketAccepted += new Listener.SocketAcceptedHandler(l_SocketAccepted);
            l.Start();
            

            Thread.Sleep(1000000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void l_SocketAccepted(Socket e)
        {
            Console.WriteLine("新的连接：{0}\n{1}\n===============", e.RemoteEndPoint, DateTime.Now);
            sockets.Add(e);
        }
    }
}
