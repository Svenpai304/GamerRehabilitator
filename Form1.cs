
namespace GamerRehabilitator
{
    using System;
    using System.Diagnostics;
    using System.IO.Ports;
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    public partial class ShockSettings : Form
    {
        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int SHOCK_ID = 1;
        private bool active = false;
        private Keys key;
        private SerialPort serialPort = new SerialPort("COM0", 9600);

        public ShockSettings()
        {
            InitializeComponent();
            Apply.Click += Apply_Click;
            comboBox1.DataSource = Enum.GetValues(typeof(Keys));
            comboBox2.DataSource = SerialPort.GetPortNames();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Setting...";
            if (serialPort.PortName != comboBox2.Text)
            {
                serialPort.Close();
                serialPort.PortName = comboBox2.Text;
                serialPort.Open();
            }
            Task.Delay(1000).Wait();
            if (Enum.TryParse(comboBox1.Text, out key))
            {
                if (active)
                {
                    UnregisterHotKey(Handle, SHOCK_ID);
                }
                RegisterHotKey(Handle, SHOCK_ID, 0, (int)key);
                active = true;
                textBox1.Text = "Hotkey set";
            }
            else { textBox1.Text = "Invalid key"; }
        }

        private void SendShockMsg()
        {
            serialPort.Write("9");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == SHOCK_ID)
            {
                SendShockMsg();
            }
            base.WndProc(ref m);
        }
    }
}