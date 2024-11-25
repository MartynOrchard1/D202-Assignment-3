# D202 Assignment 3
D202 .NET framework project for temperature sensor data

## Table Of Contents: 

- Project description 
- Features
- Installation 
- Usage 
- Configuration 
- Testing 


## Project Description:
This project implements a .NET Core Application that simulates and processes temperature sensor data. The application includes functionality to initalize sensors, simulate Temperature readings, validate and store the data detect anomalies/spikes/outliers, and logs data to a txt file. This project demonstrates the use of clean architecture principles, unit Testing with XUnit, and integration with Coverage Gutters for code coverage. The project is designed as part of Assignment 3 for D202.

## Features:
- Sensor Initialization
  - Create and config sensors with a name, location and operational range
- Data Simulation
  - Generate random temperature readings with optional fault simulation
- Data Validation
  - Ensure that sensor data falls within acceptable ranges.
- Anomaly Detection
  - Identify significant deviaitons in sensor readings
- Data Logging
  - Log sensor readings and anomalies to a .txt file for auditing
- Unit Testing
  - Comprehensive Unit Tests using xUnit to validate the functionality of core components
- Code Coverage
  - Analyze code coverage using Coverage Gutters

## Installation:
### Prerequisites:
- .NET CORE SDK 7.0 or HIGHER
  - Download link here: [https://dotnet.microsoft.com/en-us/download](url)
 
**2 Options for downloading/cloaning the code:**
1. Download the Github repository as a zip and extract it onto your computer and complete the following steps [https://github.com/MartynOrchard1/D202-Assignment-3/tree/main](url)
- ![image](https://github.com/user-attachments/assets/3e2f02ba-407d-4647-9503-8ea2357182d7)
  - Click on code and click the bottom option 'Download ZIP'
- ![image](https://github.com/user-attachments/assets/c334ce5d-be58-454d-b216-2ff937343f64)
  - Click 'Extract Here' Or extract to where ever you would like to store on your computer
- Open the folder in Visual Studio Code
2. Clone the repository by doing the following
- git clone `https://github.com/MartynOrchard1/D202-Assignment-3.git`
- `cd D202-Assignment-3`
- Move on to Usage

## Usage:
- Once the project is open in visual studio code run the following commands in the terminal:
  - `cd TempSensorApp`
  - `dotnet restore`
  - `dotnet build`
- To run the application you would run the following command:
  - `dotnet run`
- You should see some output in the terminal

## Configuration:


## Testing:
