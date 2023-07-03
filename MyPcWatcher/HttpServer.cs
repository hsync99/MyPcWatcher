using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MyPcWatcher
{
    public class HttpServer:Form1
    {
     
        Commands commands = new Commands();
        public enum CommandsType
        {
            SetTime = 1,
            ShutDown = 2,
            Notify = 3,
            Quit = 4,
        }
        string Protocol = "http://";
        string IpAddress = "";
        int Port = 8888;
        string Method = "/connection/";
       // Form1 form = new Form1();
        public HttpServer()
        {
           
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public async void ServerStart()
        {


            HttpListener server = new HttpListener();
            // установка адресов прослушки
            IpAddress = GetLocalIPAddress();
            string fullAddress = Protocol + IpAddress + ":" + Port.ToString() + Method;
            server.Prefixes.Add(fullAddress);
            server.Start();
            while (true)
            {
                var context = await server.GetContextAsync();
                var request = context.Request;

                string text;
                using (var reader = new StreamReader(request.InputStream,
                                                     request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }
                if (!String.IsNullOrEmpty(text) && !String.IsNullOrWhiteSpace(text)) 
                {
                 var  s=  JsonConvert.DeserializeObject<Commands>(text.ToString());
                    if(s.Command == 1)
                    {
                        DateTime TimeToTick = DateTime.Parse(s.Parameter);
                        TimerStart(TimeToTick);
                    }
                    if(s.Command == 2) 
                    { 

                    }
                    if(s.Command == 3)
                    {
                        this.ShowPopup(s.Parameter);
                    }
                    if(s.Command == 4)
                    {
                        this.ApplicationExit();
                    }
                }
                var response = context.Response;
                string responseText =
        @"<!DOCTYPE html>
    <html>
        <head>
            <meta charset='utf8'>
            <title>METANIT.COM</title>
        </head>
        <body>
            <h2>Hello METANIT.COM</h2>
        </body>
    </html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                // получаем поток ответа и пишем в него ответ
                response.ContentLength64 = buffer.Length;
                using Stream output = response.OutputStream;
                // отправляем данные
                await output.WriteAsync(buffer);
                await output.FlushAsync();
                //MessageBox.Show("Привет зайка");
               // this.Show();
               // this.TopMost= true; 
               // form.ShowDialog();
            }


        }

        public void TimerStart(DateTime time)
        {
            this.tickTime= time;
            this.StartTimer();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HttpServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(624, 381);
            this.Name = "HttpServer";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.HttpServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void HttpServer_Load(object sender, EventArgs e)
        {

        }
    }
}
