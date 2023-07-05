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
    public class HttpServer
    {
        Dictionary<string, string> ResponseText= new Dictionary<string, string>();
        string responseText = "";
        Form1 form = new Form1();
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
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().Contains("192"))
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
                        ResponseText = new Dictionary<string, string>();
                        ResponseText.Add("response", "Timer Tick on:" + TimeToTick.Hour.ToString() + ":" + TimeToTick.Minute.ToString());
                       var js =  JsonConvert.SerializeObject(ResponseText, Formatting.Indented);
                        responseText= js;

                    }
                    else if(s.Command == 2) 
                    { 

                    }
                    else if(s.Command == 3)
                    {
                        ResponseText = new Dictionary<string, string>();
                        ResponseText.Add("response", "Message received:"+s.Parameter);
                        var js = JsonConvert.SerializeObject(ResponseText, Formatting.Indented);
                        responseText = js;
                        form.ShowPopup(s.Parameter);
                    }
                    else if(s.Command == 4)
                    {
                        ResponseText = new Dictionary<string, string>();
                        ResponseText.Add("response", "Watcher Stoped!");
                        var js = JsonConvert.SerializeObject(ResponseText, Formatting.Indented);
                        responseText = js;
                        var response2 = context.Response;

                        byte[] buffer2 = Encoding.UTF8.GetBytes(responseText);
                        // получаем поток ответа и пишем в него ответ
                        response2.ContentLength64 = buffer2.Length;
                        using Stream output2 = response2.OutputStream;
                        // отправляем данные
                        await output2.WriteAsync(buffer2);
                        await output2.FlushAsync();
                        await Task.Delay(1000);
                        form.ApplicationExit();
                    }
                }
                var response = context.Response;
                
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
            form.tickTime= time;
            form.StartTimer();
        }

       

       
    }
}
