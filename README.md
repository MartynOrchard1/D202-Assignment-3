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
  - ![image](https://github.com/user-attachments/assets/d864f6f4-8127-4ab5-adaa-5fb0d19bb478)
    - You may notice that if you press 'P' on your keyboard you get some options:
      - 1. **Continue:** This continues Sensor Data Generation as normal
      - 2. **Exit:** This exits the loop and will stop the application.
    - If you enter a random number/letter on your keyboard the application will continue as normal. 
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
When it came to testing the application uses xUnit and Moq to ensure code quality and functionality. Moq is used to mock certain console functions during testing.
<br />
### Testing Details:
**Unit Tests:**
My Unit Tests are written with xUnit and Moq to ensure each component functions correctly in isolation. Dependancy injection is used to mock file system interactions such as Log data and console interactions.
<br />

### Key Test Scenarios:
**1. Sensor Service:**
- Validates sensor initialization with correct parameters
- Simulates sensor data with noise
- Detects anomalies based on historical data
- Logs data and maintains history
**2. Console Service:**
- Verifies the behaviour of WRiteLine, ReadKey, & KeyAvailable
**3. Mocking:**
- IConsoleService & SensorService methods are mocked using the Moq library to isolate key component for testing purposes. File operations are replaced with in memory streams during tests to avoid any form of conflicts.

### Screenshots of Testing:
**Coverage:**
<br />
![image](https://github.com/user-attachments/assets/f4b67546-9f1d-4716-b586-ec2425aaa423)
<br />
**Important Notes from the above image:**
- The reason for the coverage on program.cs being 0% is because it's a very hard file to test for and is generically ignored when it comes to testing because it serves as the entry point for the application and it typically focuses on setting up and orchestrating the overall application flow rather than implementing business logic.
- Key Points (program.cs):
  - Progam.cs Initializes dependancies
  - Progam.cs Wires up the componenets together, e.g. Services, config, log etc.
  - Progam.cs Starts the applications main loop
  - Progam.cs is a static entry point, which doesn't allow for dependancy injection directly.
- Why it's hard to test (program.cs)?
  - There is little to none business logic to test **directly**. Testing involves verifying behaviours like dependancy initalization or method calls. Which are better handled in integration or functional tests.
- There are ways to overcome the challenges that come with testing program.cs such as:
  - Extract the Logic from Main
  - Use dependancy injection to pass dependancies
  - Test the Behaviours and not Program.cs itself
  - Mock External interactions with program.cs
<br />



The below image showcases the test suite within our .NET project:
<br />
![image](https://github.com/user-attachments/assets/bbd81337-3073-4f84-874b-a29e8b27015b)
<br />
<br />
**Breakdown of The Above Image:**
- Passed:
  - Indicates that all test in the suite passed successfuly
  - In our case we had 34 passes which is great.
- Failed:
  - Indicates which tests failed.
  - If there was to be a failure it would display the error alongside it.
- Skipped:
  - Indicates if any tests were skipped
- Total:
  - The total amount of Passed, Failed, Skipped
- Duration:
  - Indicates the total time it took to execute all the tests
  - In this screenshots instance it was 476 milliseconds.

**How To View The Coverage Report:**
If there is a coverage report in the TestResults folder inside of unit-testing do the following:
1. Make sure you have Coverage Gutters installed as a VSC Extension:
  2. If you don't paste this into VSC Extension search: `ryanluker.vscode-coverage-gutters`
3. In VSC do the following keybind: `Ctrl+Shift+P`
4. Type in the following: `.NET: Open Coverage Report`
5. Locate the Test Results Folder
6. Click on the Coverage Report Folder that's already there: `\5c537a59-d05c-4b2b-a1aa-3115647e2263`
7. At first you may see nothing in the folder navigate to the right of file name in file explorer and click on the file type it will expand a drop down menu and you want to then click .xml file type as seen in the attached image:
![image](https://github.com/user-attachments/assets/2cfd0d8e-f95e-4eed-982c-7483c9ba347c)
8. Now that you have selected xml files you should be able to see the report `coverage.cobertua.xml` click on that and you should be able to see the coverage report in the file explorer on the left hand side of your screen in VSC:
![image](https://github.com/user-attachments/assets/3e58fcde-69e7-48c6-8be3-7634dd6e4c60)
9. Output should look like this:
<br />

![image](https://github.com/user-attachments/assets/f4b67546-9f1d-4716-b586-ec2425aaa423)




