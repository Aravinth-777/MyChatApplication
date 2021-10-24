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
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        TcpClient sender1 = null;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            sender1 = new TcpClient();

            try
            {
                sender1.Connect(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text));
                status.Text = "Connected Successfully to Server!";

            }
            catch(Exception ex)
            {
                status.Text = ex.Message;
            }

            Thread incomingMessage = new Thread(waitForData);
            incomingMessage.Start();


        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Byte[] bytes = new Byte[256];
            String data = messageBox.Text;
            NetworkStream stream = sender1.GetStream();

            byte[] msg = Encoding.ASCII.GetBytes(data);

            stream.Write(msg, 0, msg.Length);
            status.Text += Environment.NewLine + "Sent :" + data;
            messageBox.Clear();
        }

        private void waitForData()
        {
            while(true)
            {
                Byte[] bytes = new Byte[256];
                NetworkStream stream = sender1.GetStream();
                String responseData = String.Empty;

                
                Int32 bytesLength = stream.Read(bytes, 0, bytes.Length);
                responseData = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesLength);
                status.Text += Environment.NewLine + "Received : " + responseData;
            }
            

         }
    }
}
