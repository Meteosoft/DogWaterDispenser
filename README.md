# Introduction

An Arduino project to monitor a dog water dispenser, topping up when necessary and performing a daily flush.

## Arduino
The Arduino monitors a water level sensor in a dog water dispenser, and automatically tops up the bowl when the level gets too low (by opening the inlet solenoid). Once a day it will automatically flush the bowl by draining the old water (by opening the outlet solenoid) and allowing fresh water to flush the saliva and dirt (by opening the inlet solenoid).

## Controller
The (.NET C#, Windows 10) controller connects to the Arduino via serial or ethernet, to monitor water levels and set various values:

![Screenshot of Controller](Controller.png?raw=true)

Run DogWaterDispenser.exe with a '-serial' command, i.e.
```sh
DogWaterDispenser.exe -serial
```
when a USB is connected to the Arduino. By default, the program communicates with the Arduino via ethernet, on 10.1.1.200 but you can change this in the DogWaterDispenser.ino file.

## Hardware
The hardware is configured as follows:

![Hardware](Circuit.png?raw=true)

