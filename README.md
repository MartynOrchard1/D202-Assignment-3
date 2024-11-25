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
- You should see some output in the terminal such as the following:
  - ![image](https://github.com/user-attachments/assets/da100136-df4d-43e0-8792-03a1c6d6a55b)
  - As mentioned above you can monitor anomalies the sensor data is flagged and logged to the console.
    - Anomalies are not added to the sensor log.txt file!
- Data is then logged out to the Log file in the logs folder as can be seen here:
  - ![image](https://github.com/user-attachments/assets/4f92d3d9-4604-4f87-a6d6-215b6ea32f1a)

## Configuration:
To configure the application you want to do the following steps:
1. Open the Project folder in Visual Studio Code
2. On the left hand side navigate to the 'config' folder 
<br />![image](https://github.com/user-attachments/assets/5da75137-77a4-4dd9-9fc8-fbcb62cfebf7)
3. Open appsettings.json
4. Once open you should see something like this: 
<br />![image](https://github.com/user-attachments/assets/72e97a6a-920a-435d-a96d-f04e18f71375)
5. For example: If you would like to edit the minimum value allowed of the sensor you would simply edit the "MinValue" setting to whatever you want it to be. 
<br />![image](https://github.com/user-attachments/assets/48ddc044-7ff0-4ca2-a74f-ecf702b4b06d)
6. If you wish to edit the Log File Path/file name etc make sure that the location of the folder doesn't change. It must **ALWAYS** be located within the root directory of the app `(/TempSensorApp)`

## Testing:
