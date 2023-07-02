using System.Net;
using System.Text;

namespace MyPcWatcher
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
          
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
    }
}