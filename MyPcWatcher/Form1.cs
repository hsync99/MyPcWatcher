using System.Diagnostics;
using System.Management;
using System.Net;
using System.Text;

namespace MyPcWatcher
{
    public partial class Form1 : Form
    {
       public DateTime tickTime = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
          
        }

        public void StartTimer()
        {
            timer1.Interval = 1000;
            timer1.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost= true; 
            //this.WindowState = FormWindowState.Maximized;
        }
        

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowDialog();
            this.ShowInTaskbar = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            e.Cancel = true;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Shutdown();
        }

       private void Shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown",
                                               mboShutdownParams, null);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            var timeNow = DateTime.Now;
            if(tickTime.Hour == timeNow.Hour && tickTime.Minute == timeNow.Minute)
            {
                timer1.Stop();
                this.ShowDialog();
            }
           

        }
        public void ShowPopup(string Message)
        {
            MessageBox.Show(Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.None,
     MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            
        }
        public void ApplicationExit()
        {
            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }
    }
}