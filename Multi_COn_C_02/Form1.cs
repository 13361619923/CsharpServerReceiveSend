using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Multi_COn_C_02
{
    public partial class Form1 : Form
    {
        Socket sck;
        public Form1()
        {
            InitializeComponent();
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            sck.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26950));
            MessageBox.Show("已连接");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int s = sck.Send(Encoding.UTF8.GetBytes(textMsg.Text));
            if (s > 0)
            {
                MessageBox.Show("数据已发送");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            sck?.Close();
            sck?.Dispose();
            Close();
        }

        private void textMsg_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
