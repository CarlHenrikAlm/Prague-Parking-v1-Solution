Prague Parking v1
C# Console Application
By Carl‑Henrik Alm – KYH DevSecOps 2026

Overview
Prague Parking is a console‑based parking management system written in C#. It simulates a parking facility with 100 parking spots, each capable of holding up to two “size units.” Cars occupy two units, while motorcycles occupy one. Multiple motorcycles can share a single spot as long as the total size does not exceed the spot capacity.

The application provides functionality for parking, retrieving, searching, and moving vehicles, as well as displaying a visual parking map and listing all parked vehicles.

Features
Park Vehicle
User selects vehicle type: CAR or MC.

Registration number is validated (1–10 characters, no spaces, #, or |).

The system checks whether the vehicle is already parked.

The first available spot with enough capacity is selected.

Vehicles are stored in the format:
TYPE#REGNR#TIMESTAMP  
Example from the code:
"CAR#ABC123#2026-10-11T15:22:30.0000000Z"

Arrival timestamps are stored in UTC and displayed in Prague local time.

Retrieve Vehicle
Searches for the vehicle by registration number.

Calculates parking duration using stored arrival time.

Removes the vehicle from its spot.

Displays arrival time, retrieval time, and total duration.

Search for Vehicle
Locates the parking spot of a given registration number.

Returns the spot number or an error if not found.

Move Vehicle
Moves a vehicle from its current spot to a user‑specified target spot.

Validates that the target spot has enough remaining capacity.

Retains the original timestamp and vehicle data.

Parking Map
Displays a 25×4 grid representing all 100 parking spots.

Color‑coded output:

Green: empty

Yellow: partially filled

Red: full

Example from the code:
Console.ForegroundColor = used == 0 ? ConsoleColor.Green : used < SpotSize ? ConsoleColor.DarkYellow : ConsoleColor.Red;

List All Vehicles
Counts all motorcycles and cars.

Displays each parked vehicle with its spot number and registration number.

Example output format:
Spot 12: CAR ABC123

Exit Program
Asks for confirmation before terminating the application.

Technical Architecture
Data Structures
ParkingSpots[100]:
An array of nullable strings. Each string may contain one or more vehicles separated by |.
Example from the code:
ParkingSpots[spot] = ParkingSpots[spot] + "|" + vehicleEntry;

Vehicle format:
TYPE#REGNR#TIMESTAMP

ParkingTimeStamps[]:
Stores arrival timestamps (UTC).

Constants
SpotCount = 100

SpotSize = 2

CarSize = 2

McSize = 1

MapColumns = 25

Menu System
Navigation via Up/Down arrow keys and Enter.

Menu items include: Park, Retrieve, Search, Move, Map, List, Exit.

Time Handling
Arrival timestamps stored in ISO 8601 UTC format:
"O" (Round‑trip format)

Converted to Prague time using:
TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")

Installation & Running
Requirements
.NET 6 or later

Windows terminal (for color output and key handling)

Running the Application
Clone or download the project files.

Navigate to the project directory.

Run the application:
dotnet run
Use arrow keys to navigate the menu.

Code Structure
PragueParking.cs
 ├── Data & constants
 ├── Main() and menu rendering
 ├── ParkVehicle()
 ├── RetrieveVehicle()
 ├── SearchForVehicle()
 ├── MoveVehicle()
 ├── ShowMapOfParking()
 ├── ListAllVehicles()
 ├── ExitProgram()
 ├── Parking logic (GetVehicles, FindVehicle, AddToSpot, RemoveFromSpot)
 └── Input/Output helpers (AskVehicleType, AskRegNr, Success, Error)
Error Handling
The program uses a global flag ErrorOccurred to manage error states.
When an error occurs, the user is prompted to retry the action or return to the main menu.

Example from the code:
Error("invalid registration number (1-10 characters, no spaces, '#' or '|'.");
