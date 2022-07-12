using System;
using System.Collections;
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

namespace Multi_Con_S_01
{
    public partial class Main : Form
    {
        Listener listener;
        public Main()
        {
            InitializeComponent();
            listener = new Listener(26950);
            listener.SocketAccepted += new Listener.SocketAcceptedHandler(listener_SocketAccepted);
            Load += new EventHandler(Main_Load);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            listener.Start();
        }

        private void listener_SocketAccepted(Socket e)
        {
            Client client = new Client(e);
            client.Received += new Client.ClientReceivedHandler(client_Received);
            client.Disconnected += new Client.ClientDisconnectedHandler(client_Disconnected);
            
            Invoke((MethodInvoker)delegate
            {
                ListViewItem i = new ListViewItem();
                i.Text = client.EndPoint.ToString();
                i.SubItems.Add(client.ID);
                i.SubItems.Add("XX");
                i.SubItems.Add("XX");
                i.Tag = client;
                lstClients.Items.Add(i);
            });
        }

        public void DataTansmit(string str, string guid)
        {
            //从客户端1发送到客户端2

            Invoke((MethodInvoker)delegate
            {
                for (int i = 0; i < lstClients.Items.Count; i++)
                {
                    Client client = lstClients.Items[i].Tag as Client;
                    if (client.ID == guid)
                    {
                        client.Send(str);
                        break;
                    }
                }
            });
        }

        private void client_Disconnected(Client sender)
        {
            Invoke((MethodInvoker)delegate
            {
                for (int i = 0; i < lstClients.Items.Count; i++)
                {
                    Client client = lstClients.Items[i].Tag as Client;
                    if (client.ID == sender.ID)
                    {
                        lstClients.Items.RemoveAt(i);
                        break;
                    }
                }
            });
        }

        private void client_Received(Client sender, byte[] data)
        {
            Invoke((MethodInvoker)delegate
            {
                for (int i = 0; i < lstClients.Items.Count; i++)
                {
                    Client client = lstClients.Items[i].Tag as Client;
                    if (client.ID == sender.ID)
                    {
                        lstClients.Items[i].SubItems[2].Text = Encoding.UTF8.GetString(data);
                        //这里做消息的转发
                        lstClients.Items[i].SubItems[3].Text = DateTime.Now.ToString();
                        break;
                    }
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Client client1 = lstClients.Items[0].Tag as Client;
                Console.WriteLine(client1.ID);
      

                DataTansmit(sendTextBox.Text, client1.ID);// 向client1发送数据
            }
            catch (Exception)
            {
                Console.WriteLine("并未连接两个socket客户端");
            }
            
        }

    }
}
