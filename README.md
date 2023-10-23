# GamerRehabilitator
This program activates a shock collar based on keyboard and microphone input.
The shock collar is activated using an RF transmitter connected to an Arduino - for more information on the hardware setup, see the related Instructable.
Code for RF transmission adapted from [dpmartee's repo.](https://github.com/dpmartee/arduino-shock-collar)

# Warning
This project is to control a shock collar - a device traditionally used in "training" dogs and pets. while I cannot stop you from using it for this purpose, I condemn in the strongest possible terms the use of shock collars, and by extension, the use of this software to control shock collars, that are used on any entity that can feel pain that is unable to (i.e. non-human animals, human children) or does not (i.e. unwilling adults) consent to it's use on them.

This project uses commecial products that are intended for use on non-human animals, and involve electricity. I offer no guarantee that use outside (or within) the manufacturer's intended use are safe and I recommend you seek medical advice on the issue before use.

*Warning copied from [smouldery original repo](https://github.com/smouldery/shock-collar-control)*

# Software setup
To use the program, download the folder containing it and copy the Arduino code to a properly connected device.
Most input fields in the desktop app are fairly straightforward, and you can find your Arduino port using the Device Manager or your Arduino IDE.
Keyboard and microphone inputs can also be used independently - if the volume trigger value is set to zero or the trigger key to None, the other will still trigger the command.
