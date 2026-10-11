# Prague Parking v1 Solution

# Prague Parking v1.2

A console-based parking application written in C#. It manages parking for cars and motorcycles in a 100-spot lot.

## Requirements

- .NET 10 SDK
- A terminal that supports console input and output

## Run

From the project directory:





## Using the application

Navigate the menu with the **Up** and **Down** arrow keys, then press **Enter**. Choose **Exit** and confirm with **Y** to close the application.

Available actions:

- **Park vehicle** — choose car (`C`) or motorcycle (`M`), then enter a registration number.
- **Retrieve vehicle** — remove a parked vehicle by registration number.
- **Search for vehicle** — find a vehicle's spot by registration number.
- **Move vehicle** — move a vehicle to another spot with sufficient capacity.
- **Map of parking** — view spot occupancy: green is empty, yellow is partially occupied, and red is full.
- **Show parked vehicle** — list parked vehicles and their spots.

Registration numbers are trimmed and converted to uppercase. They must contain 1–10 characters and cannot contain spaces, `#`, or `|`.

## Parking capacity

The lot contains 100 spots. Each spot has capacity for two units:

- A car uses two units and occupies a spot by itself.
- A motorcycle uses one unit, so up to two motorcycles can share a spot.

## Data and limitations

Parking data is held in memory and is cleared when the application exits. The code does not currently persist vehicle records or use its `ParkingTimeStamps` array; displayed times are generated when actions occur.




classDiagram
    class PragueParking {
        <<static application>>
        -int SpotCount
        -int SpotSize
        -int CarSize
        -int McSize
        -int MapColumns
        -string[] ParkingSpots
        -bool IsRunning
        -DateTime?[] ParkingTimeStamps
        -string[] MenuItems
        +Main(args)
        -ParkVehicle()
        -RetrieveVehicle()
        -SearchForVehicle()
        -MoveVehicle()
        -ShowMapOfParking()
        -ListAllVehicles()
        -ExitProgram()
        -GetVehicles(spot)
        -TypeOf(vehicle)
        -RegOf(vehicle)
        -SizeOf(type)
        -UsedSize(spot)
        -GetVehiclesInSpot(spot, reg)
        -FindVehicle(reg)
        -FindFreeSpot(type)
        -AddToSpot(spot, type, reg)
        -RemoveFromSpot(spot, reg)
        -AskVehicleType()
        -AskRegNr()
        -Success(message)
        -Error(message)
        -GetPragueTime()
