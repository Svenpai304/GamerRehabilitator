
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
        public Keys targetKey = Keys.None;
        private bool keyHeld = false;

        private SerialPort serialPort = new SerialPort("COM0", 9600); // Serial port to use for the Arduino

        // Instantiate event which triggers every time the standard microphone updates its value
        private NAudio.Wave.WaveInEvent waveIn = new NAudio.Wave.WaveInEvent
        {
            DeviceNumber = 0, // indicates which microphone to use
            WaveFormat = new NAudio.Wave.WaveFormat(rate: 44100, bits: 16, channels: 1),
            BufferMilliseconds = 20
        };
        private float currentAudioValue = 0;
        private float triggerAudioValue = 0;

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

            // Further set up audio input
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            // Set up Serial port printing to console
            serialPort.DataReceived += PrintSerialData;
        }

        // Called whenever data is added to the Serial buffer from the Arduino,
        // and writes it into the console for debugging.
        private void PrintSerialData(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bytes = new byte[32];
            serialPort.Read(bytes, 0, bytes.Length);
            Console.WriteLine(bytes[0]);
        }

        // Calls the command function if both trigger conditions are met.
        private void CheckTriggerValues()
        {
            if (keyHeld && currentAudioValue >= triggerAudioValue)
            {
                SendCommand();
            }
            else if (targetKey == Keys.None && currentAudioValue >= triggerAudioValue && triggerAudioValue != 0) // Case for only using the audio as trigger
            {
                SendCommand();
            }
        }

        // Called whenever a keyboard hook event occurs. 
        // If the targeted key is used, the keyHeld value is changed.
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

        // Called whenever new microphone data is available.
        // Translates this data into a float between 0 and 1 which is
        // then used as the audio value for any trigger check.
        private void WaveIn_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            // copy buffer into an array of integers
            Int16[] values = new Int16[e.Buffer.Length / 2];
            Buffer.BlockCopy(e.Buffer, 0, values, 0, e.Buffer.Length);

            // determine the highest value as a fraction of the maximum possible value
            float fraction = (float)values.Max() / 32768;
            currentAudioValue = fraction;
            this.Invoke((MethodInvoker)delegate () { CheckTriggerValues(); }); // Calls the function on the main thread
        }

        // Called when the Apply button on the form is clicked.
        // This function then sets the Serial port's name and opens it,
        // then sets the target key according to the key set in the form.
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
            Task.Delay(3000).Wait(); // Gives the Arduino some time to restart after the Serial port is opened

            triggerAudioValue = (float)numericUpDown1.Value / 100;
            Keys key;
            if (Enum.TryParse(keyComboBox.Text, out key))
            {
                targetKey = key;
                statusTextBox.Text = "Hotkey set";
            }
            else { statusTextBox.Text = "Invalid key"; }
        }

        // Sends a command over the Serial port using the values input in the form.
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