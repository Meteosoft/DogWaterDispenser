/*
 Name:		DogWaterDispenser.ino
 Created:	09/05/2017
 Author:	Rob Munn

 Regulates the water level in a dog water dispenser, and flushes the dispenser 
 bowl of saliva and dirt once per day.

 Communicates with external software (the Controller, written in C#) via serial 
 or ethernet. All parameters are programmable via the Controller except for the 
 ethernet IP address, which is hard-coded by default to 10.1.1.200. This can be 
 changed in this file.

 To minimise memory usage in the Arduino, all message strings are tokenised, e.g.
	'!n' = "Flushing has been disabled!"
 The controller will translate the token into an equivalent message before logging
 it.

 All commands to and from the Controller are in the form: COMMAND[:DATA]\n, where
 [:DATA] is optional. The COMMAND is a number between 0 and 20 (see //COMMAND section
 below).
*/

#define INCLUDE_ETHERNET 1

#include <TimeLib.h>
#include "Time.h"
#if INCLUDE_ETHERNET
	#include <SPI.h>
	#include <Ethernet.h>
#endif

#if INCLUDE_ETHERNET
// Enter a MAC address and IP address for your controller below.
// The IP address will be dependent on your local network.
// gateway and subnet are optional:
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
IPAddress ip(10, 1, 1, 200);
IPAddress myDns(10, 1, 1, 1);
IPAddress gateway(10, 1, 1, 1);
IPAddress subnet(255, 255, 0, 0);

EthernetServer m_server(80);
EthernetClient m_client = NULL;
#endif

// PINS
// The push button pin for manual flush cycle
#define MANUAL_FLUSH_BUTTON_PIN 3
// The power relay pin for inlet solenoid
#define INLET_RELAY_PIN 5
// The power relay pin for outlet solenoid
#define OUTLET_RELAY_PIN 6
// The water level sensor power pin
#define POWER_TO_SENSOR_PIN 7
// The inlet solenoid LED pin
#define INLET_LED_PIN 8
// The outlet solenoid LED pin
#define OUTLET_LED_PIN 9
// The water level sensor pin
#define WATER_LEVEL_PIN A3

// COMMANDS
// Arduino->Controller: Initialisation
#define INIT_CONTROLLER 0
// Set/Get the current date/time
#define TIME 1
// Here is a informational log message
#define LOG_INFORMATION 2
// Here is an warning log message
#define LOG_WARNING 3
// Here is an error log message
#define LOG_ERROR 4
// Activate/Deactivate the inlet solenoid power relay
#define INLET_POWER_RELAY 5
// Activate/Deactivate the outlet solenoid power relay
#define OUTLET_POWER_RELAY 6
// Get/set the flush period
#define FLUSH_PERIOD 7
// Start/stop the water level readings
#define WATER_LEVEL_OUTPUT 8
// Send one second level readings (not the usual one minute)
#define SEND_FAST_LEVEL_READINGS 9
// Do a manual flush cycle
#define DO_FLUSH_CYCLE_NOW 10
// Get/Set low level mark
#define LOW_LEVEL_SETTING 11
// Get/Set high level mark
#define HIGH_LEVEL_SETTING 12
// Get/Set auto flush hour
#define AUTO_FLUSH_HOUR_SETTING 13
// Get/Set auto flush minute
#define AUTO_FLUSH_MINUTE_SETTING 14
// Send water level reading now (for network connection)
#define SEND_WATER_LEVEL_NOW 15
// Get/Set the sensor read interval
#define WATER_LEVEL_READ_SENSOR_INTERVAL 16
// Get/Set the maximum sensor value that equates to 1% (higher resistance = lower water level)
#define MAX_SENSOR_VALUE_SETTING 17
// Get/Set the minimum sensor value that equates to 99% (lower resistance = higher water level)
#define MIN_SENSOR_VALUE_SETTING 18
// Get/Set whether we are allowed to do auto or manual flush cycles
#define ALLOW_FLUSHING 19
// Get/Set whether the Arduino should send raw water level values
#define SEND_RAW_VALUES 20
// Ping command to check TCP connection
#define IS_ALIVE 98

// INITIAL DEFAULT CONSTANTS
// The default % below which the inlet solenoid will be switched on
#define LOW_MARK 50
// The default % above which the inlet solenoid will be switched off
#define HIGH_MARK 90
// The default hour to perform the auto flushing
#define AUTO_FLUSH_HOUR 1
// The default minute to perform the auto flushing
#define AUTO_FLUSH_MINUTE 0
// The default period in seconds to perform the auto and manual flushing
#define FLUSH_SECONDS 30
// The default interval (in ms) between sensor readings. The power to the sensor
// is turned on for ~1sec at each interval, to minimise the effects of electrolysis
#define READ_SENSOR_INTERVAL 5000
// The default maximum raw sensor value that equates to 1% (empty bowl)
#define MAX_SENSOR_VALUE 530
// The default minimum raw sensor value that equates to 99% (full bowl)
#define MIN_SENSOR_VALUE 390

// GLOBAL VARIABLES
// Is the inlet solenoid on? Default is initially NO
bool m_inletOn = false;
// Are we currently performing a flush cycle? Default is initially NO
bool m_doingFlushCycle = false;
// Should we send water level readings to any external software (if connected) every minute? Default is YES by default
bool m_sendWaterLevelReadings = true;
// Should we send water level readings to any external software (if connected) every second? Default is NO by default
bool m_sendFastWaterLevelReadings = false;
// Is auto or manual flushing allowed? Default is YES
bool m_doFlush = true;
// Should we send raw sensor readings to any external software (if connected) every second? Default is NO by default
bool m_sendRawValues = false;

// The default flush period
int m_flushPeriod = FLUSH_SECONDS;
// The default low level mark
int m_lowLevelMark = LOW_MARK;
// The default high level mark
int m_highLevelMark = HIGH_MARK;
// The default flush hour
int m_autoFlushHour = AUTO_FLUSH_HOUR;
// The default flush minute
int m_autoFlushMinute = AUTO_FLUSH_MINUTE;
// The default sensor read interval
int m_readInterval = READ_SENSOR_INTERVAL;
// The default minimum sensor value
int m_minSensorReading = MIN_SENSOR_VALUE;
// The default maximum sensor value
int m_maxSensorReading = MAX_SENSOR_VALUE;

// Internal use - to stop multiple auto-flush error messages being emitted
bool m_sentAutoFlushErrorMessage = false;
// Internal use - to time the flush sequence
unsigned long m_timeAutoFlushStarted = 0L;
// Internal use - Will store last time the read loop was run
long m_previousReadLoopMillis = 0L;
// Internal use - Will store last time the fast send (water level) loop was run
long m_previousFastSendMillis = 0L;
// Internal use - Will store last time the fast send (raw water level reading) loop was run
long m_previousRawSendMillis = 0L;
// Internal use - Counter for the number of button presses
int m_buttonPushCounter = 0;
// Internal use - Current state of the button
int m_buttonState = 0;
// Internal use - Previous state of the button
int m_lastButtonState = 0;
// Internal use - The last water level read
int m_waterLevel = 0;

// Forward declaration of function to send messages to Controller
void SendMessageToDispenserController(int, String, bool);

// Required main entry point
void setup() 
{
#if INCLUDE_ETHERNET
	// Initialize the ethernet device
	Ethernet.begin(mac, ip, myDns, gateway, subnet);
	
	// Start listening for network clients
	m_server.begin();
#endif
	// Initialize serial communication at 9600 bits per second:
	Serial.begin(9600);

	// Sets the power to water level sensor digital pin as output (initially off)
	digitalWrite(POWER_TO_SENSOR_PIN, LOW);
	pinMode(POWER_TO_SENSOR_PIN, OUTPUT);

	// Sets the inlet solenoid power relay and LED digital pins as output (initially off)
	digitalWrite(INLET_RELAY_PIN, HIGH);
	pinMode(INLET_RELAY_PIN, OUTPUT);
	digitalWrite(INLET_LED_PIN, LOW);
	pinMode(INLET_LED_PIN, OUTPUT);

	// Sets the outlet valve power relay and LED digital pins as output (initially off)
	digitalWrite(OUTLET_RELAY_PIN, HIGH);
	pinMode(OUTLET_RELAY_PIN, OUTPUT);
	digitalWrite(OUTLET_LED_PIN, LOW);
	pinMode(OUTLET_LED_PIN, OUTPUT);

	// Initialize the button pin as a input
	pinMode(MANUAL_FLUSH_BUTTON_PIN, INPUT);

	// Send ethernet IP address
	Serial.print("Ethernet Server address:");
	Serial.println(Ethernet.localIP());
}

// The required processing loop
void loop()
{
#if INCLUDE_ETHERNET
	// Listen for network messages
	ListenForNetworkMessages();
#endif
	// Listen for serial messages
	ListenForSerialMessages();

	// Check the manual flush cycle button
	CheckManualFlushCycleButton();

	// Send raw water level data if required, every second
	unsigned long currentMillis = millis();
	if (m_sendRawValues)
	{
		if (currentMillis - m_previousRawSendMillis >= 1000)
		{
			m_previousRawSendMillis = currentMillis;
			String msg = "!r"; // '!r' = "Raw Value: "
			msg.concat(String(analogRead(WATER_LEVEL_PIN)));
			SendMessageToDispenserController(LOG_INFORMATION, msg, true);
		}
	}

	// Send water level readings every second if fast, else every minute
	if (m_sendWaterLevelReadings)
	{
		if (m_sendFastWaterLevelReadings)
		{
			// Every second...
			if (currentMillis - m_previousFastSendMillis >= 1000)
			{
				m_previousFastSendMillis = currentMillis;
				SendWaterLevelToController();
			}
		}
		else
		{
			// Every minute...
			if (second() == 0)
			{
				SendWaterLevelToController();
				delay(1000);
			}
		}
	}

	// Do automatic water level checking every second
	if (currentMillis - m_previousReadLoopMillis >= m_readInterval)
	{
		m_previousReadLoopMillis = currentMillis;
		DoProcessing();
	}
}

// Check the manual flush cycle button
void CheckManualFlushCycleButton()
{
	// Check the push button state
	m_buttonState = digitalRead(MANUAL_FLUSH_BUTTON_PIN);

	// Compare the buttonState to its previous state
	if (m_buttonState != m_lastButtonState)
	{
		// If the state has changed, increment the counter
		if (m_buttonState == HIGH)
		{
			// If the current state is HIGH then the button
			// went from off to on
			m_buttonPushCounter++;
			DoFlushCycle(true);
		}
		// Delay a little bit to avoid bouncing
		delay(50);
	}

	// Save the current state as the last state, for next time through the loop
	m_lastButtonState = m_buttonState;
}

// Do automatic water level checking
void DoProcessing()
{
	// Check date/time and request latest from a connected controller if possible every 10 seconds
	if (year() < 2013 && second() % 10 == 0)
		SendMessageToDispenserController(TIME, "", true);

	// Check the water level, filling if necessary
	if (!m_doingFlushCycle)
		CheckWaterLevel();

	// Start (or stop) an automatic flush cycle if it's time
	DoFlushCycle(false);

	// Wait a short while
	delay(100);
}

// Listen for serial messages
void ListenForSerialMessages()
{
	// If there's any serial data available, read it...
	while (Serial.available() > 0)
	{
		// Get the latest command string from the serial port
		String strCommand = Serial.readString();

		// Ensure command is terminated
		if (strCommand.length() >= 1 && strCommand.indexOf('\n') == -1)
			strCommand.concat("\n");

		// Loop through all the messages, each terminated with a '\n'
		while (strCommand.indexOf('\n') != -1)
		{
			// Check first character of the command line for non-numeric command
			String msg = "";
			if (strCommand == "\n")
				break;
			if (strCommand[0] < '0' || strCommand[0] > '9')
			{
				msg = "!A"; // '!A' = "Invalid command. Received: "
				msg.concat(strCommand);
				SendMessageToDispenserController(LOG_WARNING, msg, true);
				break;
			}

			// Parse and execute the message
			ParseMessage(strCommand.substring(0, strCommand.indexOf('\n')));

			// Prepare the next message
			strCommand = strCommand.substring(strCommand.indexOf('\n') + 1);

			// Ensure command is terminated
			if (strCommand.length() >= 1 && strCommand.indexOf('\n') == -1)
				strCommand.concat("\n");
		}
	}
}

// Parse and execute the message
void ParseMessage(String strCommand)
{
	// Parse the command line, expected to be in the form:
	// COMMAND[:DATA]\n
	int command = -1;
	String data = "";
	int colonPos = strCommand.indexOf(':');
	if (colonPos != -1)
	{
		// Extract command from the command line
		command = strCommand.substring(0, colonPos).toInt();
		data = strCommand.substring(colonPos + 1);
		data.trim();
	}
	else
		command = strCommand.toInt();
	int receivedCommand = command;

	// Execute command
	ExecuteCommand(command, data);
}

// Execute command
void ExecuteCommand(int command, String data)
{
	String msg;
	switch (command)
	{
		case INIT_CONTROLLER:
			// Tell the controller what our current state is
			InitialiseController();
			break;
		case TIME:
			// Sets or returns the date/time
			if (data != "")
			{
				SetDateTime(data);
				m_previousReadLoopMillis = 0L;
			}
			else
				SendDateTimeMessage();
			break;
		case INLET_POWER_RELAY:
			// Sets or returns the inlet solenoid state
			ActivateInletPowerRelay(data);
			break;
		case OUTLET_POWER_RELAY:
			// Sets or returns the outlet solenoid state
			ActivateOutletPowerRelay(data);
			break;
		case FLUSH_PERIOD:
			// Sets or returns the flush period
			SetFlushPeriod(data);
			break;
		case WATER_LEVEL_OUTPUT:
			// Sets or returns whether we send water level readings every minute
			SetSendWaterLevelReadings(data);
			break;
		case SEND_FAST_LEVEL_READINGS:
			// Sets or returns whether we send water level readings every second
			SetFastWaterLevelReadings(data);
			break;
		case DO_FLUSH_CYCLE_NOW:
			// Do a manual flush cycle now
			DoFlushCycle(true);
			break;
		case LOW_LEVEL_SETTING:
			// Sets or returns the low level % 
			SetLowLevelMark(data);
			break;
		case HIGH_LEVEL_SETTING:
			// Sets or returns the high level % 
			SetHighLevelMark(data);
			break;
		case AUTO_FLUSH_HOUR_SETTING:
			// Sets or returns the hour to perform the auto flush
			SetAutoFlushHour(data);
			break;
		case AUTO_FLUSH_MINUTE_SETTING:
			// Sets or returns the minute to perform the auto flush
			SetAutoFlushMinute(data);
			break;
		case WATER_LEVEL_READ_SENSOR_INTERVAL:
			// Sets or returns the interval between sensor readings
			SetWaterLevelReadInterval(data);
			break;
		case SEND_WATER_LEVEL_NOW:
			// Send the water level to the Controller now
			SendWaterLevelToController();
			break;
		case MAX_SENSOR_VALUE_SETTING:
			// Sets or returns the maximum high sensor value (1%)
			SetMaxSensorValue(data);
			break;
		case MIN_SENSOR_VALUE_SETTING:
			// Sets or returns the minimum low sensor value (99%)
			SetMinSensorValue(data);
			break;
		case ALLOW_FLUSHING:
			// Sets or returns whether we are allowed to do auto or manual flush cycles
			SetFlushState(data);
			break;
		case SEND_RAW_VALUES:
			// Sets or returns whether we send raw sensor values every second
			SetSendRawValueState(data);
			break;
		case IS_ALIVE:
			// Ping command to check TCP connection, so reply with same
			SendMessageToDispenserController(IS_ALIVE, "1", true);
			break;
		default:
			// Command not understood!
			msg = "!A"; // '!A' = "Invalid command. Received: "
			msg.concat(command);
			msg.concat(":");
			msg.concat(data);
			SendMessageToDispenserController(LOG_ERROR, msg, true);
			break;
	}
}

// Send the water level to the controller
void SendWaterLevelToController()
{
	String msg = String(m_waterLevel);
	msg.concat("@");
	msg.concat(FormatDateTime());
	SendMessageToDispenserController(WATER_LEVEL_OUTPUT, msg, true);
}

// Check the water level, filling if necessary
void CheckWaterLevel()
{
	// Check water level
	m_waterLevel = GetWaterLevel();
	if (m_waterLevel < m_lowLevelMark)
	{
		// Solenoid is on and water is low, so fill
		digitalWrite(INLET_RELAY_PIN, LOW); // Inlet solenoid on
		digitalWrite(INLET_LED_PIN, HIGH); // Inlet LED on
		SendMessageToDispenserController(INLET_POWER_RELAY, "1", true);
		if (!m_inletOn)
		{
			String msg = "!B"; // '!B' = "Water level is low ("
			msg.concat(String(m_waterLevel));
			msg.concat("!C"); // '!C' = "%). Filling the dispenser..."
			SendMessageToDispenserController(LOG_INFORMATION, msg, true);
			SendWaterLevelToController();
			m_inletOn = true;
		}
	}
	else if (m_waterLevel > m_highLevelMark)
	{
		// Solenoid is off and water is now high, so turn off
		digitalWrite(INLET_RELAY_PIN, HIGH); // Inlet solenoid off
		digitalWrite(INLET_LED_PIN, LOW); // Inlet LED off
		SendMessageToDispenserController(INLET_POWER_RELAY, "0", true);
		if (m_inletOn)
		{
			String msg = "!D"; // '!D' = "Water level is high ("
			msg.concat(String(m_waterLevel));
			msg.concat("!E"); // '!E' = "%). Turning off solenoid..."
			SendMessageToDispenserController(LOG_INFORMATION, msg, true);
			SendWaterLevelToController();
			m_inletOn = false;
		}
	}
}

// Read the current water level
int GetWaterLevel()
{
	String msg;

	// Turn on the power to the water level sensor
	digitalWrite(POWER_TO_SENSOR_PIN, HIGH); // On
	delay(500);

	int level = map(analogRead(WATER_LEVEL_PIN), m_minSensorReading, m_maxSensorReading, 100, 0);
	if (level < 1)
		level = 1;
	if (level > 99)
		level = 99;

	// Turn off the power to the water level sensor
	digitalWrite(POWER_TO_SENSOR_PIN, LOW); // Off
	return level;
}

// Do an flush cycle if it's time, or user requested it
void DoFlushCycle(bool manual)
{
	if (!m_doFlush && !m_doingFlushCycle)
	{
		return;
	}
	
	// Check we have a valid date/time
	if (!manual && !m_doingFlushCycle && year() < 2013)
	{
		if (!m_sentAutoFlushErrorMessage && second() % 30 == 0)
		{
			SendMessageToDispenserController(LOG_WARNING, "!F", true); // '!F' = "Invalid DATE/TIME. Cannot perform auto flush cycle..."
			m_sentAutoFlushErrorMessage = true;
			SendMessageToDispenserController(TIME, "", true);
		}
		else if (m_sentAutoFlushErrorMessage && second() % 35 == 0)
		{
			m_sentAutoFlushErrorMessage = false;
		}
		return;
	}

	// Get current time
	unsigned long currentTime = millis();

	// If the flush has already started, check if it's time to stop (after 30 seconds [default])
	if (m_doingFlushCycle)
	{
		// Check if it's time to stop the flush cycle
		if (currentTime - m_timeAutoFlushStarted >= (m_flushPeriod * 1000))
		{
			// Turn outlet solenoid off to stop draining water
			digitalWrite(OUTLET_RELAY_PIN, HIGH); // Outlet solenoid off
			digitalWrite(OUTLET_LED_PIN, LOW); // Outlet LED off
			SendMessageToDispenserController(OUTLET_POWER_RELAY, "0", true);

			// Turn inlet solenoid off to stop flushing
			digitalWrite(INLET_RELAY_PIN, HIGH); // Inlet solenoid off
			digitalWrite(INLET_LED_PIN, LOW); // Inlet LED off
			SendMessageToDispenserController(INLET_POWER_RELAY, "0", true);
			m_doingFlushCycle = false;
			m_timeAutoFlushStarted = 0L;
			String msg = "!I"; // '!I' = "Flush cycle ended: "
			msg.concat(FormatDateTime());
			SendMessageToDispenserController(LOG_INFORMATION, msg, true);

			// The normal water level check will now fill the dispenser
		}
	}
	// Check if it's time to start the flush cycle
	else if (manual || (hour() == m_autoFlushHour && minute() == m_autoFlushMinute && second() > 0 && second() <= (int)(m_flushPeriod / 2)))
	{
		// Flag that we're busy
		m_doingFlushCycle = true;

		// Tell user what we're doing
		String msg = manual ? "!G" : "!H"; // '!G' = "Manual Flush cycle started: ", '!H' = "Auto Flush cycle started: "
		msg.concat(FormatDateTime());
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);

		// Turn outlet solenoid on to start draining water
		digitalWrite(OUTLET_RELAY_PIN, LOW); // Outlet solenoid on
		digitalWrite(OUTLET_LED_PIN, HIGH); // Outlet LED on
		SendMessageToDispenserController(OUTLET_POWER_RELAY, "1", true);
		//SendMessageToDispenserController(LOG_INFORMATION, "!N"); // '!N' = "Activating the outlet solenoid..."

		// Turn inlet solenoid on to start flushing
		digitalWrite(INLET_RELAY_PIN, LOW); // Inlet solenoid on
		digitalWrite(INLET_LED_PIN, HIGH); // Inlet LED on
		SendMessageToDispenserController(INLET_POWER_RELAY, "1", true);
		//SendMessageToDispenserController(LOG_INFORMATION, "!L", true); // '!L' = "Activating the inlet solenoid..."
		m_timeAutoFlushStarted = currentTime;
	}
}

// Send: Inlet Relay Pin State, Outlet Relay Pin State, Send Water Level Readings State, Send Fast Water Level Readings State, Flush Period,
//       Low Level Mark, High Level Mark, Auto Flush Hour, Auto Flush Minute, Read Interval, Max Sensor Reading, Min Sensor Reading, Do Flush State,
//		 Send Raw Values State
void InitialiseController()
{
	int inletRelayPinState = digitalRead(INLET_RELAY_PIN);
	int outletRelayPinState = digitalRead(OUTLET_RELAY_PIN);
	String msg = (inletRelayPinState) ? "0" : "1";
	msg.concat(",");
	msg.concat((outletRelayPinState) ? "0" : "1");
	msg.concat(",");
	msg.concat((m_sendWaterLevelReadings) ? "1" : "0");
	msg.concat(",");
	msg.concat((m_sendFastWaterLevelReadings) ? "1" : "0");
	msg.concat(",");
	msg.concat(m_flushPeriod);
	msg.concat(",");
	msg.concat(m_lowLevelMark);
	msg.concat(",");
	msg.concat(m_highLevelMark);
	msg.concat(",");
	msg.concat(m_autoFlushHour);
	msg.concat(",");
	msg.concat(m_autoFlushMinute);
	msg.concat(",");
	msg.concat(m_readInterval);
	msg.concat(",");
	msg.concat(m_maxSensorReading);
	msg.concat(",");
	msg.concat(m_minSensorReading);
	msg.concat(",");
	msg.concat((m_doFlush) ? "1" : "0");
	msg.concat(",");
	msg.concat((m_sendRawValues) ? "1" : "0");
	SendMessageToDispenserController(INIT_CONTROLLER, msg, true);
}

// Sends a message back to the Dispenser controller
void SendMessageToDispenserController(int logType, String message, bool sendTCP = true)
{
	// Format message
	String msg = String(logType);
	msg.concat(":");
	msg.concat(message);
	msg.concat("\r\n");

	// Send message on the serial port
	Serial.print(msg);
	delay(200);

	// Send the message over the ethernet
	if (sendTCP)
		SendEthernetMessage(msg);
}

// Sets the arduino date and time
void SetDateTime(String data)
{
	String str = "";
	bool error = false;
	int commaPos = data.indexOf(',');
	if (commaPos == -1)
	{
		unsigned long pctime;
		const unsigned long DEFAULT_TIME = 1357041600; // Jan 1 2013

		// Extract the current time from the data
		int dataLength = data.length() + 1;
		char dataAsChars[256];
		data.toCharArray(dataAsChars, dataLength);
		pctime = atol(dataAsChars);

		// Check the integer is a valid time (greater than Jan 1 2013)
		if (pctime >= DEFAULT_TIME)
		{
			// Sync Arduino clock to the time received
			setTime(pctime);
		}
		error = true;
	}
	else
	{
		// Expect something like: "2017,6,5,22,14,10"
		int Y = 0;
		int M = 0;
		int D = 0;
		int h = 0;
		int m = 0;
		int s = 0;
		str = data.substring(0, commaPos); // 2017
		Y = str.toInt();
		data = data.substring(commaPos + 1); // "6,5,22,14,10"
		commaPos = data.indexOf(',');
		if (commaPos == -1)
			error = true;
		else
		{
			str = data.substring(0, commaPos); // 6
			M = str.toInt();
			data = data.substring(commaPos + 1); // "5,22,14,10"
			commaPos = data.indexOf(',');
			if (commaPos == -1)
				error = true;
			else
			{
				str = data.substring(0, commaPos); // 5
				D = str.toInt();
				data = data.substring(commaPos + 1); // "22,14,10"
				commaPos = data.indexOf(',');
				if (commaPos == -1)
					error = true;
				else
				{
					str = data.substring(0, commaPos); // 22
					h = str.toInt();
					data = data.substring(commaPos + 1); // "14,10"
					commaPos = data.indexOf(',');
					if (commaPos == -1)
						error = true;
					else
					{
						str = data.substring(0, commaPos); // 14
						m = str.toInt();
						data = data.substring(commaPos + 1); // "10"
						s = data.toInt(); // 10
					}
				}
			}
		}

		if (!error)
		{
			// Sync Arduino clock to the time received on the serial port
			setTime(h, m, s, D, M, Y);
			delay(300);
			SendDateTimeMessage();
		}
	}
	if (error)
		SendMessageToDispenserController(LOG_WARNING, "!J", true); // '!J' = "Invalid TIME parameter(s)..."
}

// Send the date/time message
void SendDateTimeMessage()
{
	String msg = "!K"; // '!K' = "Date/Time is now: "
	msg.concat(FormatDateTime());
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

// Format the date/time into a string in the form 'yyyy/mm/dd HH:MM:SS'
String FormatDateTime()
{
	String dateTime = String(year());
	dateTime.concat("/");
	if (month() < 10)
		dateTime.concat("0");
	dateTime.concat(String(month()));
	dateTime.concat("/");
	if (day() < 10)
		dateTime.concat("0");
	dateTime.concat(String(day()));
	dateTime.concat(" ");
	if (hour() < 10)
		dateTime.concat("0");
	dateTime.concat(String(hour()));
	dateTime.concat(":");
	if (minute() < 10)
		dateTime.concat("0");
	dateTime.concat(String(minute()));
	dateTime.concat(":");
	if (second() < 10)
		dateTime.concat("0");
	dateTime.concat(String(second()));
	return dateTime;
}

// Activates or deactivates the inlet relay, or returns the inlet relay state
void ActivateInletPowerRelay(String data)
{
	// Activates or deactivates the inlet relay, or returns the inlet relay state
	if (data != "")
	{
		int flag = data.toInt();
		if (flag == 1)
		{
			// Activate the inlet power relay
			SendMessageToDispenserController(LOG_INFORMATION, "!L", true); // '!L' = "Activating the inlet relay..."
			digitalWrite(INLET_RELAY_PIN, LOW); // On
			digitalWrite(INLET_LED_PIN, HIGH); // Inlet LED on
			SendMessageToDispenserController(INLET_POWER_RELAY, "1", true);
		}
		else if (flag == 0)
		{
			// Deactivate the inlet power relay 
			SendMessageToDispenserController(LOG_INFORMATION, "!M", true); // '!M' = "Deactivating the inlet relay..."
			digitalWrite(INLET_RELAY_PIN, HIGH);
			digitalWrite(INLET_LED_PIN, LOW); // Inlet LED off
			SendMessageToDispenserController(INLET_POWER_RELAY, "0", true);
		}
	}
	else
	{
		// Return current relay state
		int relayPinState = digitalRead(INLET_RELAY_PIN);
		String msg = (relayPinState) ? "0" : "1";
		SendMessageToDispenserController(INLET_POWER_RELAY, msg, true);
	}
}

// Activates or deactivates the outlet relay, or returns the outlet relay state
void ActivateOutletPowerRelay(String data)
{
	// Activates or deactivates the outlet relay, or returns the outlet relay state
	if (data != "")
	{
		int flag = data.toInt();
		if (flag == 1)
		{
			// Activate the outlet power relay
			SendMessageToDispenserController(LOG_INFORMATION, "!N", true); // '!N' = "Activating the outlet relay..."
			digitalWrite(OUTLET_RELAY_PIN, LOW); // On
			digitalWrite(OUTLET_LED_PIN, HIGH); // Outlet LED on
			SendMessageToDispenserController(OUTLET_POWER_RELAY, "1", true);
		}
		else if (flag == 0)
		{
			// Deactivate the outlet power relay 
			SendMessageToDispenserController(LOG_INFORMATION, "!O", true); // '!O' = "Deactivating the outlet relay..."
			digitalWrite(OUTLET_RELAY_PIN, HIGH);
			digitalWrite(OUTLET_LED_PIN, LOW); // Outlet LED off
			SendMessageToDispenserController(OUTLET_POWER_RELAY, "0", true);
		}
	}
	else
	{
		// Return current relay state
		int relayPinState = digitalRead(OUTLET_RELAY_PIN);
		String msg = (relayPinState) ? "0" : "1";
		SendMessageToDispenserController(OUTLET_POWER_RELAY, msg, true);
	}
}

// Set the flush period during the flush cycle
void SetFlushPeriod(String data)
{
	String msg = "";

	// Sets or returns the maximum pan limit
	if (data != "")
	{
		// Set the flush period
		m_flushPeriod = data.toInt();
		msg = "!P"; // '!P' = "Flush Period set to "
		msg.concat(m_flushPeriod);
		SendMessageToDispenserController(LOG_INFORMATION, msg);
	}
	else
	{
		// Send the flush period to Controller
		SendMessageToDispenserController(FLUSH_PERIOD, String(m_flushPeriod), true);
	}
}

void SetSendWaterLevelReadings(String data)
{
	String msg = "";

	// Sets or returns the maximum pan limit
	if (data != "")
	{
		// Set whether we send water level readings
		m_sendWaterLevelReadings = data.toInt();
		if (m_sendWaterLevelReadings)
			msg = "!R"; // '!R' = "Water readings set to ON"
		else
			msg = "!S"; // '!S' = "Water readings set to OFF"
	}
	else
	{
		// Send whether we send water level readings
		if (m_sendWaterLevelReadings)
			msg = "!T"; // '!T' = "Water readings is ON"
		else
			msg = "!U"; // '!U' = "Water readings is OFF"
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

// Set fast water level readings
void SetFastWaterLevelReadings(String data)
{
	String msg = "";

	// Sets or returns the maximum pan limit
	if (data != "")
	{
		// Set the water level reading period
		m_sendFastWaterLevelReadings = data.toInt();
		if (m_sendFastWaterLevelReadings)
			msg = "!V"; // '!V' = "Will send water level readings every second..."
		else
			msg = "!W"; // '!W' = "Will send water level readings every minute..."
		m_previousFastSendMillis = 0L;
	}
	else
	{
		// Send the water level reading period to Controler
		if (m_sendFastWaterLevelReadings)
			msg = "!X"; // '!X' = "Water level readings sent every second..."
		else
			msg = "!Y"; // '!Y' = "Water level readings sent every minute..."
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetLowLevelMark(String data)
{
	String msg = "";

	// Sets or returns the low level mark (%)
	if (data != "")
	{
		// Set the low level mark
		m_lowLevelMark = data.toInt();
		msg = "!Z"; // '!Z' = "Low level mark set to: "
		msg.concat(m_lowLevelMark);
	}
	else
	{
		// Send the current low level mark
		msg = "!a"; // '!a' = "Low level mark is: "
		msg.concat(m_lowLevelMark);
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetHighLevelMark(String data)
{
	String msg = "";

	// Sets or returns the high level mark (%)
	if (data != "")
	{
		// Set the high level mark
		m_highLevelMark = data.toInt();
		msg = "!b"; // '!b' = "High level mark set to: "
		msg.concat(m_highLevelMark);
	}
	else
	{
		// Send the current high level mark
		msg = "!c"; // '!c' = "High level mark is: "
		msg.concat(m_highLevelMark);
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetAutoFlushHour(String data)
{
	String msg = "";

	// Sets or returns the auto flush hour
	if (data != "")
	{
		// Set the auto flush hour
		m_autoFlushHour = data.toInt();
		msg = "!d"; // '!d' = "Auto flush hour set to: "
		msg.concat(m_autoFlushHour);
	}
	else
	{
		// Send the auto flush hour
		msg = "!e"; // '!e' = "Auto flush hour is: "
		msg.concat(m_autoFlushHour);
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetAutoFlushMinute(String data)
{
	String msg = "";

	// Sets or returns the auto flush minute
	if (data != "")
	{
		// Set the auto flush minute
		m_autoFlushMinute = data.toInt();
		msg = "!f"; // '!f' = "Auto flush minute set to: "
		msg.concat(m_autoFlushMinute);
	}
	else
	{
		// Send the auto flush minute
		msg = "!g"; // '!g' = "Auto flush minute is: "
		msg.concat(m_autoFlushMinute);
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetWaterLevelReadInterval(String data)
{
	String msg = "";

	// Sets or returns the auto flush minute
	if (data != "")
	{
		// Set the water level read interval
		m_readInterval = data.toInt();
		msg = "!h"; // '!h' = "Water level read interval set to: "
		msg.concat(m_readInterval);
	}
	else
	{
		// Send the water level read interval
		msg = "!i"; // '!i' = "Water level read interval is: "
		msg.concat(m_readInterval);
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetMaxSensorValue(String data)
{
	String msg = "";

	// Sets or returns the maximum sensor value that equates to 1% 
	if (data != "")
	{
		// Set the maximum sensor value
		m_maxSensorReading = data.toInt();
		msg = "!j"; // '!j' = "Max Sensor Value (~1%) set to: "
		msg.concat(m_maxSensorReading);
	}
	else
	{
		// Send the maximum sensor value
		msg = "!k"; // '!k' = "Max Sensor Value (~1%) is: "
		msg.concat(m_maxSensorReading);
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetMinSensorValue(String data)
{
	String msg = "";

	// Sets or returns the minimum sensor value that equates to 99% 
	if (data != "")
	{
		// Set the minimum sensor value
		m_minSensorReading = data.toInt();
		msg = "!l"; // '!l' = "Min Sensor Value (~99%) set to: "
		msg.concat(m_minSensorReading);
	}
	else
	{
		// Send the maximum sensor value
		msg = "!m"; // '!m' = "Min Sensor Value (~99%) is: "
		msg.concat(m_minSensorReading);
	}
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetFlushState(String data)
{
	String msg = "";

	// Sets or returns whether we are allowed to flush
	if (data != "")
	{
		// Sets whether we are allowed to flush
		m_doFlush = data.toInt();
	}
	if (m_doFlush)
		msg = "!o"; // '!o' = "Flushing has been enabled!"
	else
		msg = "!n"; // '!n' = "Flushing has been disabled!"
	SendMessageToDispenserController(LOG_INFORMATION, msg, true);
}

void SetSendRawValueState(String data)
{
	String msg = "";

	// Sets or returns the 'send raw values' state
	if (data != "")
	{
		// Sets the 'send raw values' state
		m_sendRawValues = data.toInt();
	}
	else
	{
		if (m_sendRawValues)
			msg = "!q"; // '!q' = "Send raw data is On..."
		else
			msg = "!p"; // '!p' = "Send raw data is Off..."
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
	}
}

#if INCLUDE_ETHERNET
void ListenForNetworkMessages()
{
	// Listen for incoming client requests
	EthernetClient client = m_server.available();
	if (!client)
		return;
	m_client = client;

	// Get the latest command string from the network client
	String request = ReadRequest(&m_client);
	
	// Check first character of the command line for non-numeric command
	String msg = "";
	if (request != "\n")
	{
		if (request[0] < '0' || request[0] > '9')
		{
			msg = "!A"; // '!A' = "Invalid command. Received: "
			msg.concat(request);
			SendMessageToDispenserController(LOG_WARNING, msg, false);
		}
		else
		{
			// Parse and execute the message
			ParseMessage(request.substring(0, request.indexOf('\n')));
		}
	}
}

// Read the request line
String ReadRequest(EthernetClient* client)
{
	String request = "";

	// Loop while the client is connected
	while (client->connected())
	{
		// Read available bytes
		while (client->available())
		{
			// Read a byte
			char c = client->read();

			// Print the value (for debugging)
			//Serial.write(c);

			// Exit loop if end of line
			if (c == '\n')
			{
				request += c;
				return request;
			}

			// Add byte to request line
			request += c;
		}
	}
	SendMessageToDispenserController(LOG_ERROR, request, false);
	return request;
}
#endif // INCLUDE_ETHERNET

void SendEthernetMessage(String message)
{
#if INCLUDE_ETHERNET
	// Send response to client
	if (!m_client)
		return;
	if (m_client.connected())
	{
		m_client.println(message);
	}
#endif
}