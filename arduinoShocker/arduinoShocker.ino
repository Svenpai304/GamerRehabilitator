// Arduino sketch to be used with the desktop app available here: https://github.com/Svenpai304/GamerRehabilitator
// RF transmission code adapted from: https://github.com/dpmartee/arduino-shock-collar
// With transmission timings from: https://github.com/smouldery/shock-collar-control/blob/master/Arduino%20Modules/transmitter_vars.ino

byte receivedData;

//=================================================== START OF COLLAR SETUP CODE ======================================================================

//const int shock_min = 0; // Minimum of power a command will be executed at
const int shock_delay = 1000;  // Maximum rate at which the shock function can be used at
//const int cmd_max = 1000; // Maximum of milliseconds which a command can be executed at

// Constant variables
const int pin_led = LED_BUILTIN;         // Pin for indication LED
const int pin_rtx = 12;                  // Pin to transmit over
const String key = "00101100101001010";  // Key of the transmitter, dont touch if you dont know how it works

// Variables which do change
int collar_chan = 0;        // Can be channel 0 or 1
int collar_duration = 500;  // Duration of the command in milliseconds

// Define values for easier recognition
#define COLLAR_LED 1
#define COLLAR_BEEP 2
#define COLLAR_VIB 3
#define COLLAR_ZAP 4

// Strings used for building up the command sequence
String sequence, power, channelnorm, channelinv, modenorm, modeinv;

// Store the last time anything was transmitted to the collar
unsigned long transmit_last = 0;
unsigned long shock_last = 0;

// Command transmission function
void transmit_command(int c, int m, int p = 0) {
  transmit_last = millis();
  switch (c)  // Check the channel
  {
    case 1:  // Channel 1
      channelnorm = "111";
      channelinv = "000";
      break;
    default:  // Channel 0
      channelnorm = "000";
      channelinv = "111";
      break;
  }

  switch (m)  // Check the mode
  {
    case 1:  // Light
      modenorm = "1000";
      modeinv = "1110";
      break;
    case 2:  // Beep
      modenorm = "0100";
      modeinv = "1101";
      break;
    case 4:  // Shock
      modenorm = "0001";
      modeinv = "0111";
      shock_last = millis();
      break;
    default:  // Vibrate
      modenorm = "0010";
      modeinv = "1011";
      //      p = 10; // Set strengh to 10 for the command to be executed properly
      break;
  }

  // Convert power to binary
  int zeros = String(p, BIN).length();

  String power;
  for (int i = 0; i < 7 - zeros; i++) {
    power = power + "0";
  }
  power = power + String(p, BIN);

  String sequence = "1" + channelnorm + modenorm + key + power + modeinv + channelinv + "00";

  digitalWrite(pin_led, LOW);
  //  d = constrain(d, 50, cmd_max); // Clamp duration of the command
  unsigned long cmd_start = millis();
  //  while (millis() - cmd_start < d)
  //  {
  // start bit
  digitalWrite(pin_rtx, HIGH);
  delayMicroseconds(1540);  // chnged to new protocol
  digitalWrite(pin_rtx, LOW);
  delayMicroseconds(800);  // wait 750 uS

  for (int n = 0; n < 41; n++) {
    if (sequence.charAt(n) == '1')  // Transmit a one
    {
      digitalWrite(pin_rtx, HIGH);
      delayMicroseconds(740);  // chnged to new protocol
      digitalWrite(pin_rtx, LOW);
      delayMicroseconds(300);  // chnged to new protocol
    } else                     // Transmit a zero
    {
      digitalWrite(pin_rtx, HIGH);
      delayMicroseconds(220);  // chnged to new protocol
      digitalWrite(pin_rtx, LOW);
      delayMicroseconds(820);  // chnged to new protocol
    }
  }
  delayMicroseconds(9000);  // chnged to new protocol
                            //  }
  digitalWrite(pin_led, HIGH);
}

void collar_keepalive() {
  if (millis() - transmit_last >= 120000)  // Send command to the collar at least every 2 minutes to make it stay on
  {
    Serial.println("Keep-alive:\tCollar");
    transmit_command(collar_chan, COLLAR_LED, 50);
  }
}

//=================================================== END OF COLLAR SETUP CODE ======================================================================

//=================================================== START OF SERIAL RECEIVER CODE =================================================================

// One-byte serial command reader
void receive_command(byte command) {
  if (millis() - transmit_last < shock_delay) {  // Disregards commands during delay
    return;
  }
  int command_mode;
  switch (bitRead(command, 7)) {  // Reads most significant bit to set mode of collar
    case 1: command_mode = COLLAR_ZAP; break;
    default: command_mode = COLLAR_VIB; break;
  }
  bitWrite(command, 7, 0);  // Sets the mode bit to false so the power value can read as essentially a 7-bit integer
  int power = command;

  for (byte i = 0; i < 5; i++) {
    transmit_command(collar_chan, command_mode, power);  // Uses command data to transmit to the collar
  }
}
//=================================================== End OF SERIAL RECEIVER CODE ====================================================================


void setup() {
  //=================================================== START OF COLLAR SETUP CODE ======================================================================
  pinMode(pin_rtx, OUTPUT);  // Set transmitter pin as output
  pinMode(pin_led, OUTPUT);  // Set LED pin as output
  Serial.begin(9600);
  //=================================================== END OF COLLAR SETUP CODE ======================================================================
  digitalWrite(pin_rtx, LOW);
  Serial.println("Setup complete");
}

void loop() {
  collar_keepalive();

  // Reads Serial data if available, sends it back to confirm
  if (Serial.available()) {
    receivedData = Serial.read();
    Serial.write(receivedData);
    receive_command(receivedData);
  }
}