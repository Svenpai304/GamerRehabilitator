
namespace GamerRehabilitator
{
    using NAudio.CoreAudioApi;
    using System;
    using System.IO.Ports;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Windows.Forms;


    public partial class ShockSettings : Form
    {
        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Keys targetKey = Keys.None;
        private bool keyHeld = false;

        private SerialPort serialPort = new SerialPort("COM0", 9600);

        private NAudio.Wave.WaveInEvent waveIn = new NAudio.Wave.WaveInEvent
        {
            DeviceNumber = 0, // indicates which microphone to use
            WaveFormat = new NAudio.Wave.WaveFormat(rate: 44100, bits: 16, channels: 1),
            BufferMilliseconds = 20
        };
        private float currentAudioValue = 0;
        private float triggerAudioValue = 0.0f;

        private GlobalKeyboardHook globalKeyboardHook;

        private enum CollarMode { Vibrate, Shock }

        public ShockSettings()
        {
            InitializeComponent();

            // Set up form elements
            Apply.Click += Apply_Click;
            keyComboBox.DataSource = Enum.GetValues(typeof(Keys));
            portComboBox.DataSource = SerialPort.GetPortNames();
            collarModeBox.DataSource = Enum.GetValues(typeof(CollarMode));

            // Create keyboard hook
            globalKeyboardHook = new GlobalKeyboardHook();
            globalKeyboardHook.KeyboardPressed += OnKeyPressed;

            // Set up audio input
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            serialPort.DataReceived += PrintSerialData;
        }

        private void PrintSerialData(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bytes = new byte[32];
            serialPort.Read(bytes, 0, bytes.Length);
            Console.WriteLine(bytes[0]);
        }

        private void CheckTriggerValues()
        {
            if (keyHeld && currentAudioValue >= triggerAudioValue)
            {
                SendCommand();
            }
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            Keys key = e.KeyboardData.Key;
            if (key == targetKey)
            {
                switch (e.KeyboardState)
                {
                    case GlobalKeyboardHook.KeyboardState.KeyDown: keyHeld = true; Console.WriteLine("Key down"); break;
                    case GlobalKeyboardHook.KeyboardState.KeyUp: keyHeld = false; Console.WriteLine("Key up"); break;
                    default: break;
                }
                CheckTriggerValues();
            }
        }

        private void WaveIn_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            // copy buffer into an array of integers
            Int16[] values = new Int16[e.Buffer.Length / 2];
            Buffer.BlockCopy(e.Buffer, 0, values, 0, e.Buffer.Length);

            // determine the highest value as a fraction of the maximum possible value
            float fraction = (float)values.Max() / 32768;
            currentAudioValue = fraction;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            statusTextBox.Text = "Setting...";
            if (!serialPort.IsOpen)
            {
                serialPort.PortName = portComboBox.Text;
                serialPort.Open();
            }
            if (serialPort.PortName != portComboBox.Text)
            {
                serialPort.Close();
                serialPort.PortName = portComboBox.Text;
                serialPort.Open();
            }
            Task.Delay(1000).Wait();

            triggerAudioValue = (float)numericUpDown1.Value / 100;
            Keys key;
            if (Enum.TryParse(keyComboBox.Text, out key))
            {
                targetKey = key;
                statusTextBox.Text = "Hotkey set";
            }
            else { statusTextBox.Text = "Invalid key"; }
        }

        private void SendCommand()
        {
            byte powerValue = (byte)powerSlider.Value;
            byte mode = 0;
            CollarMode modeEnum;
            if (Enum.TryParse(collarModeBox.Text, out modeEnum))
            {
                switch (modeEnum)
                {
                    case CollarMode.Vibrate: mode = 0; break;
                    case CollarMode.Shock: mode = 1; break;
                }
            }

            // Activation command in one byte, with bits 0-6 for the power value and bit 7 for the mode
            int command = powerValue & ~(1 << 7) | (mode << (byte)7);
            byte[] bytes = new byte[1];
            bytes[0] = (byte)command;

            serialPort.Write(bytes, 0, bytes.Length);
        }
    }
}