# SmartStorageDB
## Smart Storage DB - A furniture inventory system displaying the stock levels, price, and names of a variety of furniture from various warehouses.
A WPF application that allows the user to read, write and save data from the UI. 
The application utilizes a database file created through the use of 'DB Browser'.


# Guide

This is a step by step guide on running this program.

### To run without opening Visual Studio.
1. You must have .NET core installed. To install, follow the instructions in the following link: https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net50
2. Download the zip file from github. 
3. Extract the file and open its directory.
4. Go to \SmartStorageDB\FurnitureInventorySystem, hold shift and right click anywhere in the folder. In the context menu, you will see the option to Open command window here. Clicking on it will open a CMD window.
5. Run the command "dotnet run"
6. The program should run. Select a factory and here you can create, remove and update furniture items from the database.

### To run in Visual Studio.
1. Run the solution "\SmartStorageDB\FurnitureInventorySystem.sln" in Visual Studio
2. Goto Project -> FurnitureInventorySystem Properties -> Debug and change the Working Directory to the project directory. For example, on my computer the project directory is: "G:\Other computers\My Laptop\Uni\SmartStorageDB\FurnitureInventorySystem"
3. Save the changes.
4. Run "FurnitureInventorySystem" at the top of the window.
