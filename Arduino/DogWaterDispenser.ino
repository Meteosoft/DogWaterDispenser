/*
 Name:		DogWaterDispenser.ino
 Created:	09/05/2017
 Modified:  02/06/2017 - Added watchdog, saving to EEPROM and placing strings in FLASH
 Author:	Rob Munn

 Regulates the water level in a dog water dispenser, and flushes the dispenser 
 bowl of saliva and dirt once per day.

 Communicates with external software (the Controller, written in C#) via serial 
 or ethernet. All parameters are programmable via the Controller including the 
 ethernet IP, DNS and Gateway addresses, which is hard-coded by default to 10.1.1.200,
 10.1.1.1 and 10.1.1.1. The default can be changed in this file.

 To minimise SRAM memory usage in the Arduino, all message strings are placed in FLASH
 memory using the F() syntax, e.g.
	F("Water level is low. Filling the dispenser...")

 Previous parameters are persisted in the EEPROM, which will survive a power cut or 
 reset. If the parameter structure in the EEPROM is changed in the program, please also
 change the version number CONFIG_VER, which will force the old EEPROM data to be
 overridden with defaults.

 All commands to and from the Controller are in the form: COMMAND[:DATA]\n. The COMMAND
 is a number between 0 and 99 (see '// COMMAND' section below).
 */

// If defined, this will use the EEPROM to persist states between resets (required for IP changes)
#define SAVE_CONFIG_EEPROM 1
// If defined, this will use the watchdog to reset if the device freezes, and also allows
// for forced resets (i.e. when the IP changes)
#define USE_WATCHDOG 1

#include <TimeLib.h>
#include "Time.h"
#include <SPI.h>
#include <Ethernet.h>
#if USE_WATCHDOG
	#include <avr/wdt.h>
#endif
#if SAVE_CONFIG_EEPROM
	#include <EEPROM.h>
#endif

// The water level status
enum
{
	// The bowl is full (water level at or over the highest sensor probe)
	FULL,
	// The bowl is adequate (water level under the highest sensor probe, but over the lowest)
	ADEQUATE,
	// The bowl is almost empty (water level under both the highest sensor probe and the lowest)
	EMPTY,
};

// Enter a MAC address and IP address for the controller below.
// The IP address will be dependent on the local network;
// gateway and subnet are optional:
String m_ipStr = "10.1.1.200";
String m_dnsStr = "10.1.1.1";
String m_gatewayStr = "10.1.1.1";

const byte m_mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
IPAddress m_ip;
IPAddress m_dns;
IPAddress m_gateway;
IPAddress m_subnet(255, 255, 0 ,0);
const int PROGMEM m_port = 80; // Kept in flash memory

// This TCP server
EthernetServer m_server(m_port);
// The connected TCP client, if any
EthernetClient m_connectedClient = NULL;

// Structure to hold IP address parts for config
struct IPAddressParts
{
	int part[4] = { 0, 0, 0, 0 };
};

// PINS
// The push button pin for manual flush cycle
#define MANUAL_FLUSH_BUTTON_PIN 3
// The power relay pin for inlet solenoid
#define INLET_RELAY_PIN 5
// The power relay pin for outlet solenoid
#define OUTLET_RELAY_PIN 6
// The water level sensor power pin (switched on only when reading is 
// taken to minimise electrolysis)
#define POWER_TO_SENSOR_PIN 7
// The inlet solenoid LED pin
#define INLET_LED_PIN 8
// The outlet solenoid LED pin
#define OUTLET_LED_PIN 9
// NOTE: Pins 10, 11, 12 and 13 are used by the ethernet shield, and 1 and 2 
// are used by the serial bus

// As we have only one digitial pin left and we need two, we must use the 
// analog pins instead:
// The high water level sensor pin
#define HIGH_WATER_LEVEL_PIN A3
// The low water level sensor pin
#define LOW_WATER_LEVEL_PIN A4

// COMMANDS
// Arduino->Controller: Initialisation
#define INIT_CONTROLLER 0
// Set the current date/time
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
// Set the flush period
#define FLUSH_PERIOD 7
// Start/stop the water level readings
#define WATER_LEVEL_OUTPUT 8
// Send one second level readings (not the usual one minute)
#define SEND_FAST_LEVEL_READINGS 9
// Do a manual flush cycle
#define DO_FLUSH_CYCLE_NOW 10
// Set auto flush hour
#define AUTO_FLUSH_HOUR_SETTING 11
// Set auto flush minute
#define AUTO_FLUSH_MINUTE_SETTING 12
// Send water level reading now (for network connection)
#define SEND_WATER_LEVEL_NOW 13
// Set the sensor read interval
#define WATER_LEVEL_READ_SENSOR_INTERVAL 14
// Set whether the Arduino should send raw water level values
#define SEND_RAW_VALUES 15
// Set the new IP, DNS and Gateway and reset
#define CHANGE_IP_RESET 16
// Manually reset the Arduino now
#define RESET_NOW 17
// Cancel manual reset of the Arduino
#define CANCEL_RESET 18
// Ping command to check TCP connection
#define IS_ALIVE 98

// INITIAL DEFAULT CONSTANTS
// The default hour to perform the auto flushing
#define AUTO_FLUSH_HOUR 1
// The default minute to perform the auto flushing
#define AUTO_FLUSH_MINUTE 0
// The default period in seconds to perform the auto and manual flushing
#define FLUSH_SECONDS 30
// The default interval (in ms) between sensor readings. The power to the sensor
// is turned on for ~1sec at each interval, to minimise the effects of electrolysis
#define READ_SENSOR_INTERVAL 5000

#if SAVE_CONFIG_EEPROM
	// The latest EEPROM config version
	#define CONFIG_VER 3

	// The EEPROM configuration
	struct PersistedValues
	{
		byte configVer = CONFIG_VER;
		byte IP[4] = { 0, 0, 0, 0 };
		byte DNS[4] = { 0, 0, 0, 0 };
		byte GATEWAY[4] = { 0, 0, 0, 0 };
		bool sendWaterLevelReadings;
		bool sendFastWaterLevelReadings;
		int flushPeriod;
		int autoFlushHour;
		int autoFlushMinute;
		int readInterval;
		int sendRawValues;
	};
#endif // SAVE_CONFIG_EEPROM

// GLOBAL VARIABLES
// Is the inlet solenoid on? Default is initially NO
bool m_inletOn = false;
// Are we currently performing a flush cycle? Default is initially NO
bool m_doingFlushCycle = false;
// Should we send water level readings to any external software (if connected) every minute? Default is YES by default
bool m_sendWaterLevelReadings = true;
// Should we send water level readings to any external software (if connected) every second? Default is NO by default
bool m_sendFastWaterLevelReadings = false;
// Should we send raw sensor readings to any external software (if connected) every second? Default is NO by default
bool m_sendRawValues = false;
// Are we counting down to a reset?
bool m_resetInProgress = false;
// Number of seconds to reset
int m_resetSecondsToGo = 8;
// Has a manual reset been cancelled?
bool m_cancelReset = false;

// The default flush period
int m_flushPeriod = FLUSH_SECONDS;
// The default flush hour
int m_autoFlushHour = AUTO_FLUSH_HOUR;
// The default flush minute
int m_autoFlushMinute = AUTO_FLUSH_MINUTE;
// The default sensor read interval
int m_readInterval = READ_SENSOR_INTERVAL;

// Internal use - flags setup success, or otherwise
bool m_setupOkay = false;
// Internal use - to stop multiple auto-flush error messages being emitted
bool m_sentAutoFlushErrorMessage = false;
// Internal use - to time the flush sequence
unsigned long m_timeAutoFlushStarted = 0L;
// Internal use - Will store last time the read loop was run
long m_previousReadLoopMillis = 0L;
// Internal use - Will store last time the fast send (water level) loop was run
long m_previousFastSendMillis = 0L;
// Internal use - Counter for the number of button presses
int m_buttonPushCounter = 0;
// Internal use - Current state of the button
int m_buttonState = 0;
// Internal use - Previous state of the button
int m_lastButtonState = 0;
// Internal use - The last water level read
int m_waterLevel = ADEQUATE;
// Internal use - The sensor probes may be corroded
bool m_corrodedProbesPossible = false;
// Internal use - Incomplete data error message
const String m_noDataErrorMsg PROGMEM = "Invalid - No data given";

// Forward declaration of required functions
void SendMessageToDispenserController(int, String, bool);
IPAddressParts IPStrToArray(String IPStr);
#if SAVE_CONFIG_EEPROM
	void PrintConfigValues(PersistedValues persistedValues);
#endif

// Required main entry point
void setup() 
{
#if USE_WATCHDOG
	// Disable watchdog timer so setup will not get interrupted
	wdt_disable();
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

#if SAVE_CONFIG_EEPROM
	// Get last values used from the EEPROM
	ReadValuesFromEEPROM();
#endif

	InitialiseTCPServer();

#if USE_WATCHDOG
	// Enable the watchdog. If not 'patted' within 8 seconds, a reset will occur
	wdt_enable(WDTO_8S);
#endif

	// If we got here, the setup succeeded
	m_setupOkay = true;
}

// The required processing loop
void loop()
{
#if USE_WATCHDOG
	// Pat the watchdog
	if (!m_resetInProgress)
		wdt_reset();
#endif

	// Don't do anything if setup failed
	if (!m_setupOkay)
		return;

	// Listen for network messages
	ListenForNetworkMessages();

	// Listen for serial messages
	ListenForSerialMessages();

	// Are we in the process of resetting?
	if (m_resetInProgress)
	{
		// Count down...
		if (m_cancelReset)
		{
			m_cancelReset = false;
			SendMessageToDispenserController(LOG_INFORMATION, F("Reset cancelled..."), true);
			m_resetInProgress = false;
			m_resetSecondsToGo = 8;
		}
		else
		{
			SendMessageToDispenserController(LOG_WARNING, String(m_resetSecondsToGo) + " sec", true);
			m_resetSecondsToGo--;
			delay(1000);
		}
	}

	// Check the manual flush cycle button
	CheckManualFlushCycleButton();

	// Send water level readings every second if fast, else every minute
	unsigned long currentMillis = millis();
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
				if (m_corrodedProbesPossible)
					SendMessageToDispenserController(LOG_WARNING, F("Please check probes for corrosion!"), true);
				delay(500);
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
				msg = F("Invalid command. Received: ");
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
	switch (command)
	{
		case INIT_CONTROLLER:
			// Tell the controller what our current state is
			InitialiseController();
			break;
		case TIME:
			// Sets/gets the date/time
			if (data != "")
			{
				SetDateTime(data);
				m_previousReadLoopMillis = 0L;
			}
			SendDateTimeMessage();
			break;
		case INLET_POWER_RELAY:
			// Sets the inlet solenoid state
			ActivateInletPowerRelay(data);
			break;
		case OUTLET_POWER_RELAY:
			// Sets the outlet solenoid state
			ActivateOutletPowerRelay(data);
			break;
		case FLUSH_PERIOD:
			// Sets the flush period
			SetFlushPeriod(data);
			break;
		case WATER_LEVEL_OUTPUT:
			// Sets whether we send water level readings every minute
			SetSendWaterLevelReadings(data);
			break;
		case SEND_FAST_LEVEL_READINGS:
			// Sets whether we send water level readings every second
			SetFastWaterLevelReadings(data);
			break;
		case DO_FLUSH_CYCLE_NOW:
			// Do a manual flush cycle now
			DoFlushCycle(true);
			break;
		case AUTO_FLUSH_HOUR_SETTING:
			// Sets the hour to perform the auto flush
			SetAutoFlushHour(data);
			break;
		case AUTO_FLUSH_MINUTE_SETTING:
			// Sets the minute to perform the auto flush
			SetAutoFlushMinute(data);
			break;
		case WATER_LEVEL_READ_SENSOR_INTERVAL:
			// Sets the interval between sensor readings
			SetWaterLevelReadInterval(data);
			break;
		case SEND_WATER_LEVEL_NOW:
			// Send the water level to the Controller now
			SendWaterLevelToController();
			break;
		case SEND_RAW_VALUES:
			// Sets whether we send raw sensor values every second
			SetSendRawValueState(data);
			break;
		case CHANGE_IP_RESET:
			// Set a new IP, DNS and Gateway, and reset
			SetNewIPAndReset(data);
			break;
		case RESET_NOW:
			ResetNow();
			break;
		case CANCEL_RESET:
			m_cancelReset = true;
			break;
		case IS_ALIVE:
			// Ping command to check TCP connection, so reply with same
			SendMessageToDispenserController(IS_ALIVE, "1", true);
			break;
		default:
			// Command not understood!
			String msg = F("Invalid command. Received: ");
			msg.concat(command);
			msg.concat(":");
			msg.concat(data);
			SendMessageToDispenserController(LOG_ERROR, msg, true);
			break;
	}
}

// Send the current water level to the controller
void SendWaterLevelToController()
{
	String msg = String(m_waterLevel); // 0=FULL, 1=ADEQUATE, 2=EMPTY
	msg.concat("@");
	msg.concat(FormatDateTime());
	SendMessageToDispenserController(WATER_LEVEL_OUTPUT, msg, true);
}

// Check the water level, filling if necessary
void CheckWaterLevel()
{
	// Get water level
	m_waterLevel = GetWaterLevel();

	// Check levels
	if (m_waterLevel == EMPTY)
	{
		// Water level is low, so fill
		digitalWrite(INLET_RELAY_PIN, LOW); // Inlet solenoid on
		digitalWrite(INLET_LED_PIN, HIGH);  // Inlet LED on
		SendMessageToDispenserController(INLET_POWER_RELAY, "1", true);
		if (!m_inletOn)
		{
			String msg = F("Water level is low. Filling the dispenser...");
			SendMessageToDispenserController(LOG_INFORMATION, msg, true);
			SendWaterLevelToController();
			m_inletOn = true;
		}
	}
	else if (m_waterLevel == FULL)
	{
		// Water level is now high, so turn off
		digitalWrite(INLET_RELAY_PIN, HIGH); // Inlet solenoid off
		digitalWrite(INLET_LED_PIN, LOW); // Inlet LED off
		SendMessageToDispenserController(INLET_POWER_RELAY, "0", true);
		if (m_inletOn)
		{
			String msg = F("Water level is high. Turning off solenoid...");
			SendMessageToDispenserController(LOG_INFORMATION, msg, true);
			SendWaterLevelToController();
			m_inletOn = false;
		}
	}
}

// Read the current water level
int GetWaterLevel()
{
	// Turn on the power to the water level sensors
	digitalWrite(POWER_TO_SENSOR_PIN, HIGH); // On
	delay(500);

	// Read the pin values (between 0 [probe in water] and 1024 [probe out of water])
	int highLevel = analogRead(HIGH_WATER_LEVEL_PIN);
	int lowLevel = analogRead(LOW_WATER_LEVEL_PIN);

	// Turn off the power to the water level sensors
	digitalWrite(POWER_TO_SENSOR_PIN, LOW); // Off

	// Send raw values if requested
	if (m_sendRawValues)
	{
		String msg = F("High Raw Value: ");
		msg.concat(String(highLevel));
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
		msg = F("Low Raw Value: ");
		msg.concat(String(lowLevel));
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
	}

	// Compute the sensor probe state. The logic is:
	// If water is touching the sensor probe, a good connection = very low raw value (say 0 to 100) on 
	// the analog pin. If the probe is corroded, the raw value increases (say > 200). When the raw value
	// approaches 512, then we won't be able to tell the water level.
	if ((highLevel < 512 && highLevel > 200) || (lowLevel < 512 && lowLevel > 200))
		m_corrodedProbesPossible = true;
	else
		m_corrodedProbesPossible = false;

	// Compute the water level state
	if (highLevel < 512)
	{
		if (m_sendRawValues)
			SendMessageToDispenserController(LOG_INFORMATION, F("FULL"), true);
		return FULL;
	}
	else if (highLevel > 512 && lowLevel > 512)
	{
		if (m_sendRawValues)
			SendMessageToDispenserController(LOG_INFORMATION, F("EMPTY"), true);
		return EMPTY;
	}
	else // if (highLevel > 512 && lowLevel < 512)
	{
		if (m_sendRawValues)
			SendMessageToDispenserController(LOG_INFORMATION, F("ADEQUATE"), true);
		return ADEQUATE;
	}
}

// Do an flush cycle if it's time, or user requested it
void DoFlushCycle(bool manual)
{
	// Check we have a valid date/time
	if (!manual && !m_doingFlushCycle && year() < 2013)
	{
		if (!m_sentAutoFlushErrorMessage && second() % 30 == 0)
		{
			SendMessageToDispenserController(LOG_WARNING, F("Invalid DATE/TIME. Cannot perform auto flush cycle..."), true);
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
			m_doingFlushCycle = false;
			m_timeAutoFlushStarted = 0L;
			String msg = F("Flush cycle ended: ");
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
		String msg = manual ? F("Manual flush cycle started: ") : F("Auto flush cycle started: ");
		msg.concat(FormatDateTime());
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);

		// Turn outlet solenoid on to start draining water
		digitalWrite(OUTLET_RELAY_PIN, LOW); // Outlet solenoid on
		digitalWrite(OUTLET_LED_PIN, HIGH); // Outlet LED on
		SendMessageToDispenserController(OUTLET_POWER_RELAY, "1", true);
		m_timeAutoFlushStarted = currentTime;
	
		// The normal water level check will now attempt to fill the dispenser, whilst draining, hence flushing the bowl
	}
}

// Send: Inlet Relay Pin State, Outlet Relay Pin State, Send Water Level Readings State, Send Fast Water Level Readings State, Flush Period,
//       Low Level Mark, High Level Mark, Auto Flush Hour, Auto Flush Minute, Read Interval, Do Flush State,
//		 Send Raw Values State, IP, DNS and Gateway addresses
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
	msg.concat(m_autoFlushHour);
	msg.concat(",");
	msg.concat(m_autoFlushMinute);
	msg.concat(",");
	msg.concat(m_readInterval);
	msg.concat(",");
	msg.concat((m_sendRawValues) ? "1" : "0");
	msg.concat(",");
	msg.concat(m_ipStr);
	msg.concat(",");
	msg.concat(m_dnsStr);
	msg.concat(",");
	msg.concat(m_gatewayStr);
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

// Sets the Arduino date and time
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
		String origData = data;
		str = origData.substring(0, commaPos); // 2017
		Y = str.toInt();
		origData = origData.substring(commaPos + 1); // "6,5,22,14,10"
		commaPos = origData.indexOf(',');
		if (commaPos == -1)
			error = true;
		else
		{
			str = origData.substring(0, commaPos); // 6
			M = str.toInt();
			origData = origData.substring(commaPos + 1); // "5,22,14,10"
			commaPos = origData.indexOf(',');
			if (commaPos == -1)
				error = true;
			else
			{
				str = origData.substring(0, commaPos); // 5
				D = str.toInt();
				origData = origData.substring(commaPos + 1); // "22,14,10"
				commaPos = origData.indexOf(',');
				if (commaPos == -1)
					error = true;
				else
				{
					str = origData.substring(0, commaPos); // 22
					h = str.toInt();
					origData = origData.substring(commaPos + 1); // "14,10"
					commaPos = origData.indexOf(',');
					if (commaPos == -1)
						error = true;
					else
					{
						str = origData.substring(0, commaPos); // 14
						m = str.toInt();
						origData = origData.substring(commaPos + 1); // "10"
						s = origData.toInt(); // 10
					}
				}
			}
		}

		if (!error)
		{
			// Sync Arduino clock to the time received on the serial port
			setTime(h, m, s, D, M, Y);
			delay(300);
		}
	}
	if (error)
		SendMessageToDispenserController(LOG_WARNING, F("Invalid TIME parameter(s)..."), true);
}

// Send the date/time message
void SendDateTimeMessage()
{
	String msg = F("Date/Time is now: ");
	String dateTime = FormatDateTime();
	msg.concat(dateTime);
	Serial.print("DateTime = ");
	Serial.println(msg);
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

// Activates or deactivates the inlet relay, the inlet relay state
void ActivateInletPowerRelay(String data)
{
	// Activates or deactivates the inlet relay, the inlet relay state
	if (data != "")
	{
		int flag = data.toInt();
		if (flag == 1)
		{
			// Activate the inlet power relay
			SendMessageToDispenserController(LOG_INFORMATION, F("Activating the inlet solenoid..."), true);
			digitalWrite(INLET_RELAY_PIN, LOW); // On
			digitalWrite(INLET_LED_PIN, HIGH); // Inlet LED on
			SendMessageToDispenserController(INLET_POWER_RELAY, "1", true);
		}
		else if (flag == 0)
		{
			// Deactivate the inlet power relay 
			SendMessageToDispenserController(LOG_INFORMATION, F("Deactivating the inlet solenoid..."), true);
			digitalWrite(INLET_RELAY_PIN, HIGH);
			digitalWrite(INLET_LED_PIN, LOW); // Inlet LED off
			SendMessageToDispenserController(INLET_POWER_RELAY, "0", true);
		}
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Activates or deactivates the outlet relay, the outlet relay state
void ActivateOutletPowerRelay(String data)
{
	// Activates or deactivates the outlet relay, the outlet relay state
	if (data != "")
	{
		int flag = data.toInt();
		if (flag == 1)
		{
			// Activate the outlet power relay
			SendMessageToDispenserController(LOG_INFORMATION, F("Activating the outlet solenoid..."), true);
			digitalWrite(OUTLET_RELAY_PIN, LOW); // On
			digitalWrite(OUTLET_LED_PIN, HIGH); // Outlet LED on
			SendMessageToDispenserController(OUTLET_POWER_RELAY, "1", true);
		}
		else if (flag == 0)
		{
			// Deactivate the outlet power relay 
			SendMessageToDispenserController(LOG_INFORMATION, F("Deactivating the outlet solenoid..."), true);
			digitalWrite(OUTLET_RELAY_PIN, HIGH);
			digitalWrite(OUTLET_LED_PIN, LOW); // Outlet LED off
			SendMessageToDispenserController(OUTLET_POWER_RELAY, "0", true);
		}
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set the flush period during the flush cycle
void SetFlushPeriod(String data)
{
	String msg = "";

	// Sets the flush period
	if (data != "")
	{
		// Set the flush period
		m_flushPeriod = data.toInt();
		msg = F("Flush Period set to ");
		msg.concat(m_flushPeriod);
		SendMessageToDispenserController(LOG_INFORMATION, msg);
#if SAVE_CONFIG_EEPROM
		WriteValuesToEEPROM();
#endif
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set whether we must send water level readings
void SetSendWaterLevelReadings(String data)
{
	String msg = "";

	// Sets whether we send water level readings
	if (data != "")
	{
		// Set whether we send water level readings
		m_sendWaterLevelReadings = data.toInt();
		if (m_sendWaterLevelReadings)
			msg = F("Water readings set to ON");
		else
			msg = F("Water readings set to OFF");
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set whether we must send FAST water level readings (every second)
void SetFastWaterLevelReadings(String data)
{
	String msg = "";

	// Sets the water level reading period
	if (data != "")
	{
		// Set the water level reading period
		m_sendFastWaterLevelReadings = data.toInt();
		if (m_sendFastWaterLevelReadings)
			msg = F("Will send water level readings every second...");
		else
			msg = F("Will send water level readings every minute...");
		m_previousFastSendMillis = 0L;
#if SAVE_CONFIG_EEPROM
		WriteValuesToEEPROM();
#endif
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set the auto flush hour
void SetAutoFlushHour(String data)
{
	String msg = "";

	// Sets the auto flush hour
	if (data != "")
	{
		// Set the auto flush hour
		m_autoFlushHour = data.toInt();
		msg = F("Auto flush hour set to: ");
		msg.concat(m_autoFlushHour);
#if SAVE_CONFIG_EEPROM
		WriteValuesToEEPROM();
#endif
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set the auto flush minute
void SetAutoFlushMinute(String data)
{
	String msg = "";

	// Sets the auto flush minute
	if (data != "")
	{
		// Set the auto flush minute
		m_autoFlushMinute = data.toInt();
		msg = F("Auto flush minute set to: ");
		msg.concat(m_autoFlushMinute);
#if SAVE_CONFIG_EEPROM
		WriteValuesToEEPROM();
#endif
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set the water level read interval
void SetWaterLevelReadInterval(String data)
{
	String msg = "";

	// Sets the auto flush minute
	if (data != "")
	{
		// Set the water level read interval
		m_readInterval = data.toInt();
		msg = F("Water level read interval set to: ");
		msg.concat(m_readInterval);
#if SAVE_CONFIG_EEPROM
		WriteValuesToEEPROM();
#endif
		SendMessageToDispenserController(LOG_INFORMATION, msg, true);
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set whether we should send raw analog pin values
void SetSendRawValueState(String data)
{
	String msg = "";

	// Sets the 'send raw values' state
	if (data != "")
	{
		// Sets the 'send raw values' state
		m_sendRawValues = data.toInt();
#if SAVE_CONFIG_EEPROM
		WriteValuesToEEPROM();
#endif
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Set the new IP, DNS and GATEWAY addresses and reset the Arduino
void SetNewIPAndReset(String data)
{
	// Expect data to be in the form xxx.xxx.xxx.xxx|yyy.yyy.yyy.yyy|zzz.zzz.zzz.zzz
	// where xxx... is the new IP, yyy... is the new DNS, and zzz... is the new gateway
	if (data != "")
	{
		// Sets IP etc
		if (data.indexOf('|') != -1)
		{
			// Remember previous values
			String prevIP = m_ipStr;
			String prevDNS = m_dnsStr;
			String prevGATEWAY = m_gatewayStr;

			// Assign new values
			m_ipStr = GetStringArrayItem(data, '|', 0);
			m_dnsStr = GetStringArrayItem(data, '|', 1);
			m_gatewayStr = GetStringArrayItem(data, '|', 2);

			// Check for bad IP address
			String errMsg = F("Bad IP address sent! IP, DNS or GATEWAY addresses NOT changed!");
			IPAddressParts ipAddressParts = IPStrToArray(m_ipStr);
			if (ipAddressParts.part[0] == 0 || ipAddressParts.part[3] == 0 ||
				ipAddressParts.part[0] > 255 || ipAddressParts.part[1] > 255 ||
				ipAddressParts.part[2] > 255 || ipAddressParts.part[3] > 255)
			{
				m_ipStr = prevIP;
				m_dnsStr = prevDNS;
				m_gatewayStr = prevGATEWAY;
				SendMessageToDispenserController(LOG_ERROR, errMsg, true);
				return;
			}

			// Check for bad DNS address
			IPAddressParts dnsAddressParts = IPStrToArray(m_dnsStr);
			if (dnsAddressParts.part[0] == 0 || dnsAddressParts.part[3] == 0 ||
				dnsAddressParts.part[0] > 255 || dnsAddressParts.part[1] > 255 ||
				dnsAddressParts.part[2] > 255 || dnsAddressParts.part[3] > 255)
			{
				m_ipStr = prevIP;
				m_dnsStr = prevDNS;
				m_gatewayStr = prevGATEWAY;
				SendMessageToDispenserController(LOG_ERROR, errMsg, true);
				return;
			}

			// Check for bad gateway address
			IPAddressParts gateAddressParts = IPStrToArray(m_gatewayStr);
			if (gateAddressParts.part[0] == 0 || gateAddressParts.part[3] == 0 ||
				gateAddressParts.part[0] > 255 || gateAddressParts.part[1] > 255 ||
				gateAddressParts.part[2] > 255 || gateAddressParts.part[3] > 255)
			{
				m_ipStr = prevIP;
				m_dnsStr = prevDNS;
				m_gatewayStr = prevGATEWAY;
				SendMessageToDispenserController(LOG_ERROR, errMsg, true);
				return;
			}

			// Okay let's change IPs and reset
			SendMessageToDispenserController(LOG_WARNING, F("Arduino is about to change the internal server IP address and reset in 8 seconds!"), true);
			SendMessageToDispenserController(LOG_WARNING, F("Please change the controller IP to match NOW. Reconnection should occur in a couple of minutes..."), true);
			Serial.print(F("IP: "));
			Serial.println(m_ipStr);
			Serial.print(F("DNS: "));
			Serial.println(m_dnsStr);
			Serial.print(F("GATEWAY: "));
			Serial.println(m_gatewayStr);
#if SAVE_CONFIG_EEPROM
			WriteValuesToEEPROM();

			// Start reset process
			m_resetInProgress = true;
			m_resetSecondsToGo = 8;
#endif // SAVE_CONFIG_EEPROM
		}
	}
	else
		SendMessageToDispenserController(LOG_ERROR, m_noDataErrorMsg, true);
}

// Reset the Arduino now
void ResetNow()
{
#if USE_WATCHDOG
	// Resetting in 8 seconds
	SendMessageToDispenserController(LOG_WARNING, F("Arduino is about reset in 8 seconds! Reconnection should occur in a couple of minutes..."), true);

	// Start reset process
	m_resetInProgress = true;
	m_resetSecondsToGo = 8;
#else
	SendMessageToDispenserController(LOG_WARNING, F("Resetting is not currently supported..."), true);
#endif
}

// Extract the given (index) separated (by separator) string from a longer string
String GetStringArrayItem(String data, char separator, int index)
{
	int found = 0;
	int strIndex[] = { 0, -1 };
	int maxIndex = data.length() - 1;

	for (int i = 0; i <= maxIndex && found <= index; i++) 
	{
		if (data.charAt(i) == separator || i == maxIndex) 
		{
			found++;
			strIndex[0] = strIndex[1] + 1;
			strIndex[1] = (i == maxIndex) ? i + 1 : i;
		}
	}

	return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}

#if SAVE_CONFIG_EEPROM
// Write parameter values to be persisted to the EEPROM
void WriteValuesToEEPROM()
{
	PersistedValues persistedValues;
	IPAddressParts ipAddressParts = IPStrToArray(m_ipStr);
	for (int i = 0; i < 4; i++)
		persistedValues.IP[i] = ipAddressParts.part[i];
	ipAddressParts = IPStrToArray(m_dnsStr);
	for (int i = 0; i < 4; i++)
		persistedValues.DNS[i] = ipAddressParts.part[i];
	ipAddressParts = IPStrToArray(m_gatewayStr);
	for (int i = 0; i < 4; i++)
		persistedValues.GATEWAY[i] = ipAddressParts.part[i];

	persistedValues.sendWaterLevelReadings = m_sendWaterLevelReadings;
	persistedValues.sendFastWaterLevelReadings = m_sendFastWaterLevelReadings;
	persistedValues.flushPeriod = m_flushPeriod;
	persistedValues.autoFlushHour = m_autoFlushHour;
	persistedValues.autoFlushMinute = m_autoFlushMinute;
	persistedValues.readInterval = m_readInterval;
	persistedValues.sendRawValues = m_sendRawValues;
	PrintConfigValues(persistedValues);
	EEPROM.put(0, persistedValues);
}

// Print out the persisted config values to the Serial port
void PrintConfigValues(PersistedValues persistedValues)
{
	Serial.print(F("    EEPROM Config Version: "));
	Serial.println(String(persistedValues.configVer));
	Serial.print(F("    Server IP: "));
	PrintIPArray(persistedValues.IP);
	Serial.print(F("    Server DNS: "));
	PrintIPArray(persistedValues.DNS);
	Serial.print(F("    Server GATEWAY: "));
	PrintIPArray(persistedValues.GATEWAY);
	Serial.print(F("    Send Water Level Readings?: "));
	Serial.println(String(persistedValues.sendWaterLevelReadings));
	Serial.print(F("    Send Fast Water Level Readings?: "));
	Serial.println(String(persistedValues.sendFastWaterLevelReadings));
	Serial.print(F("    Flush Period: "));
	Serial.println(String(persistedValues.flushPeriod));
	Serial.print(F("    Auto Flush Hour: "));
	Serial.println(String(persistedValues.autoFlushHour));
	Serial.print(F("    Auto Flush Minute: "));
	Serial.println(String(persistedValues.autoFlushMinute));
	Serial.print(F("    Read Interval: "));
	Serial.println(String(persistedValues.readInterval));
	Serial.print(F("    Send Raw Values?: "));
	Serial.println(String(persistedValues.sendRawValues));
}

// Print out the IP address from a byte array to the Serial port
void PrintIPArray(byte ipArray[])
{
	for (int i = 0; i < 4; i++)
	{
		Serial.print(ipArray[i]);
		if (i != 3)
			Serial.print(",");
		else
			Serial.println("");
	}
}

// Read parameter values to be persisted from the EEPROM
void ReadValuesFromEEPROM()
{
	PersistedValues persistedValues;
	EEPROM.get(0, persistedValues);
	Serial.println(F("Reading Config from EEPROM!"));
	PrintConfigValues(persistedValues);
	if ((int)(persistedValues.configVer) == (int)CONFIG_VER)
	{
		m_ipStr = String(persistedValues.IP[0]);
		m_ipStr.concat(".");
		m_ipStr.concat(String(persistedValues.IP[1]));
		m_ipStr.concat(".");
		m_ipStr.concat(String(persistedValues.IP[2]));
		m_ipStr.concat(".");
		m_ipStr.concat(String(persistedValues.IP[3]));

		m_dnsStr = String(persistedValues.DNS[0]);
		m_dnsStr.concat(".");
		m_dnsStr.concat(String(persistedValues.DNS[1]));
		m_dnsStr.concat(".");
		m_dnsStr.concat(String(persistedValues.DNS[2]));
		m_dnsStr.concat(".");
		m_dnsStr.concat(String(persistedValues.DNS[3]));

		m_gatewayStr = String(persistedValues.GATEWAY[0]);
		m_gatewayStr.concat(".");
		m_gatewayStr.concat(String(persistedValues.GATEWAY[1]));
		m_gatewayStr.concat(".");
		m_gatewayStr.concat(String(persistedValues.GATEWAY[2]));
		m_gatewayStr.concat(".");
		m_gatewayStr.concat(String(persistedValues.GATEWAY[3]));

		m_sendWaterLevelReadings = persistedValues.sendWaterLevelReadings;
		m_sendFastWaterLevelReadings = persistedValues.sendFastWaterLevelReadings;
		m_flushPeriod = persistedValues.flushPeriod;
		m_autoFlushHour = persistedValues.autoFlushHour;
		m_autoFlushMinute = persistedValues.autoFlushMinute;
		m_readInterval = persistedValues.readInterval;
		m_sendRawValues = persistedValues.sendRawValues;
	}
	else
	{
		String msg = F("Version Conflict. Expected version ");
		msg.concat(String(CONFIG_VER));
		msg.concat(F(". Overwriting with default values..."));
		SendMessageToDispenserController(LOG_WARNING, msg, true);
		WriteValuesToEEPROM();
	}
}
#endif

// Split an IP address string into its parts
IPAddressParts IPStrToArray(String IPStr)
{
	IPAddressParts ipAddressParts;
	int partIndex = 0;
	for (int i = 0; i < 4; i++)
		ipAddressParts.part[i] = 0;
	for (int i = 0; i < IPStr.length(); i++)
	{
		char c = IPStr[i];
		if (c == ' ' || c == '\n' || c == '\r')
			continue;
		if (c == '.' || c == ',')
		{
			partIndex++;
			continue;
		}
		ipAddressParts.part[partIndex] *= 10;
		ipAddressParts.part[partIndex] += c - '0';
	}
	return ipAddressParts;
}

// Initialise and connect the Arduino TCP server
void InitialiseTCPServer()
{
	// Initialise the IP Addresses
	if (!m_ip.fromString(m_ipStr))
	{
		Serial.print(F("Error, problem with IP: "));
		Serial.println(m_ipStr);
		return;
	}
	else
	{
		Serial.print(F("IP: "));
		m_ip.printTo(Serial);
		Serial.println("");
	}

	if (!m_dns.fromString(m_dnsStr))
	{
		Serial.print(F("Error, problem with DNS: "));
		Serial.println(m_dnsStr);
		return;
	}
	else
	{
		Serial.print(F("DNS: "));
		m_dns.printTo(Serial);
		Serial.println("");
	}
	if (!m_gateway.fromString(m_gatewayStr))
	{
		Serial.print(F("Error, problem with Gateway: "));
		Serial.println(m_gateway);
		return;
	}
	else
	{
		Serial.print(F("Gateway: "));
		m_gateway.printTo(Serial);
		Serial.println("");
	}

	// Initialize the ethernet device
	Ethernet.begin(m_mac, m_ip, m_dns, m_gateway, m_subnet);

	// Start listening for network clients
	m_server.begin();

	// Send ethernet IP address to serial port
	Serial.print(F("Ethernet Server address:"));
	Serial.println(Ethernet.localIP());
}

// Listen for TCP messages
void ListenForNetworkMessages()
{
	// Listen for incoming client requests
	EthernetClient client = m_server.available();
	if (!client)
		return;
	m_connectedClient = client;

	// Get the latest command string from the network client
	String request = ReadRequest(&m_connectedClient);
	
	// Check first character of the command line for non-numeric command
	String msg = "";
	if (request != "\n")
	{
		if (request[0] < '0' || request[0] > '9')
		{
			msg = F("Invalid command. Received: ");
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

// Send a message via TCP
void SendEthernetMessage(String message)
{
	// Send response to client
	if (!m_connectedClient)
		return;
	if (m_connectedClient.connected())
	{
		m_connectedClient.println(message);
	}
}


