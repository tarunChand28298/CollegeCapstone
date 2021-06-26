#include <Arduino.h>
#include <TimerOne.h>
#include <Wire.h>
#include "DHT.h"
#include "DHT_U.h"
#include "MAX30100_PulseOximeter.h"
#include <PID_v1.h>

#define TempHumSensorIn 4
#define MosfetOut 3

void ServerSendData();
void ServerRecieveData();
void TemperatureControl();

DHT tempHumSensor(TempHumSensorIn, DHT11);
PulseOximeter pulseOxySensor;

double Setpoint, Input, Output;
double kP = 1, kI = 1, kD = 1;
PID controller(&Input, &Output, &Setpoint, kP, kI, kD, DIRECT);

void setup()
{
    pinMode(MosfetOut, OUTPUT);
    Serial.begin(9600);
    
    Setpoint = 100;
    controller.SetMode(AUTOMATIC);
    
    tempHumSensor.begin();
    if(!pulseOxySensor.begin())
    {
        Serial.println("Pulse Oxymeter failed to initialize");
        while(true);
    }
    
    Timer1.initialize(1000000);
    Timer1.attachInterrupt(ServerSendData);
}

void loop()
{
    pulseOxySensor.update();
    TemperatureControl();
    ServerRecieveData();
}

void ServerSendData()
{
    Serial.print('>');
    Serial.print(tempHumSensor.readTemperature());
    Serial.print(',');

    Serial.print(tempHumSensor.readHumidity());
    Serial.print(',');

    Serial.print(pulseOxySensor.getHeartRate());
    Serial.print(',');

    Serial.print(pulseOxySensor.getSpO2());
    Serial.println();
}

void ServerRecieveData()
{
    if(Serial.available() > 0)
    {
        Timer1.stop();

        float newSetpoint = Serial.parseFloat();

        Timer1.resume();
        Setpoint = newSetpoint;
    }
}

void TemperatureControl()
{
    Input = (double)tempHumSensor.readTemperature();
    controller.Compute();
    analogWrite(MosfetOut,Output);
}
