/*
 C# Assignment: Prague Parking v1
 Class, KYH-DevSecOps 2026
 By Carl-Henrik Alm
*/

using System;

public class PragueParking
{
    //--------------------------------
    // Data
    //--------------------------------
    const int SpotCount = 100;
    const int SpotSize = 2;
    const int CarSize = 2;
    const int McSize = 1;
    const int MapColumns = 25;
    static string[] ParkingSpots = new string[SpotCount];
    static bool IsRunning = true;
    static DateTime?[] ParkingTimeStamps = new DateTime?[SpotCount];
    static readonly string[] MenuItems =
    {
        "Park vehicle",
        "Retrieve vehicle",
        "Search for vehicle",
        "Move vehicle",
        "Map of parking",
        "Show parked vehicle",
        "Exit",
    };

    //--------------------------
    //Main 
    //--------------------------

    static void Main(string[] args)
    {
        Console.Title = "Prague Parking";

        int selected = 0;

        while (IsRunning)
        {
            DrawMenu(selected);
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.DownArrow && selected < MenuItems.Length - 1) selected++;
            else if (key.Key == ConsoleKey.UpArrow && selected > 0) selected--;
            else if (key.Key == ConsoleKey.Enter)
            {
                RunMenuChoice(selected);
            }
        }

        static void DrawMenu(int selected)
        {
            Console.Clear();
            Console.WriteLine("Prague Parking Menu\n");
            for (int i = 0; i < MenuItems.Length; i++)
                Console.WriteLine((i == selected ? "> " : "  ") + MenuItems[i]);
            Console.WriteLine("\nUse arrow keys (Up/Down) and press Enter.");
        }
        static void RunMenuChoice(int choice)
        {
            Console.Clear();
            switch (choice)
            {
                case 0:
                    ParkVehicle();
                    break;
                case 1:
                    RetrieveVehicle();
                    break;
                case 2:
                    SearchForVehicle();
                    break;
                case 3:
                    MoveVehicle();
                    break;
                case 4:
                    ShowMapOfParking();
                    break;
                case 5:
                    ListAllVehicles();
                    break;
                case 6:
                    ExitProgram();
                    return;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    //--------------
    // Menu
    //--------------
    #region Park vehicle 
    static void ParkVehicle()
    {
        Console.WriteLine("Park Vehicle\n");

        string type = AskVehicleType();
        if (type == null)
        {
            return;
        }


        string reg = AskRegNr();
        if (reg == null)
        {
            return;
        }


        if (FindVehicle(reg) != -1)
        {
            Error($"{reg} is already parked");
            return;
        }
        int spot = FindFreeSpot(type);
        if (spot == -1)
        {
            Error("No free space for that vehicle");
            return;
        }



        AddToSpot(spot, type, reg);
        DateTime pragueTime = GetPragueTime();
        Success($"{type} {reg} parked at spot {spot + 1} at {pragueTime:HH:MM}.");
    }
    #endregion

    #region Retrieve vehicle
    static void RetrieveVehicle()
    {
        Console.WriteLine("Retrieve vehicle\n");

        string reg = AskRegNr();

        if (reg == null)
        {
            return;
        }

        int spot = FindVehicle(reg);
        if (spot == -1)
        {
            Error($"{reg} was not found.");
            return;
        }

        RemoveFromSpot(spot, reg);
        DateTime pragueTime = GetPragueTime();
        Success($"{reg} retrieved from spot {spot + 1} at {pragueTime:HH:MM}.");
    }
    #endregion

    #region Search for vehicle
    static void SearchForVehicle()
    {
        Console.WriteLine("Search for vehicle\n");

        string reg = AskRegNr();
        if (reg == null)
        {
            return;
        }

        int spot = FindVehicle(reg);
        if (spot == -1)
        {
            Error($"{reg} could not be found.");
        }
        else
        {
            Success($"{reg}is parked at spot {spot + 1}.");
        }
    }
    #endregion

    #region Move vehicle
    static void MoveVehicle()
    {
        Console.WriteLine("Move vehicle\n");

        string reg = AskRegNr();
        if (reg == null)
            return;

        int from = FindVehicle(reg);
        if (from == -1)
        {
            Error($"{reg} was not found.");
            return;
        }

        Console.WriteLine($"Move to which spot (1-{SpotCount}?");
        if (!int.TryParse(Console.ReadLine(), out int to) || to < 1 || to > SpotCount)
        {
            Error("Invalid spot number.");
            return;
        }

        int target = to - 1;

        if (target == from)
        {
            Error("There is already a vehicle in that spot.");
            return;
        }

        string vehicle = GetVehiclesInSpot(from, reg);
        if (UsedSize(target) + SizeOf(TypeOf(vehicle)) > SpotSize)
        {
            Error($"Spot {to} does not have enough room");
            return;
        }

        RemoveFromSpot(from, reg);
        AddToSpot(target, TypeOf(vehicle), reg);
        Success($"{reg} is moved from spot {from + 1} to spot {to}.");

    }
    #endregion

    #region Show map of parking
    static void ShowMapOfParking()
    {
        Console.WriteLine("Map of parking     (green = empty | Yellow = half full | Red = full) ");
        Console.WriteLine();


        for (int i = 0; i < SpotCount; i++)
        {
            int used = UsedSize(i);
            Console.ForegroundColor = used == 0 ? ConsoleColor.Green
                                    : used < SpotSize ? ConsoleColor.DarkYellow
                                    : ConsoleColor.Red;
            Console.Write(" ■ ");
            Console.ResetColor();

            if ((i + 1) % MapColumns == 0) Console.WriteLine();
        }

    }
    #endregion

    #region List of All Vehicles
    static void ListAllVehicles()
    {
        Console.WriteLine("All parked vehicles\n");

        int count = 0;
        for (int i = 0; i < SpotCount; i++)
        {
            foreach (var v in GetVehicles(i))
            {
                Console.WriteLine($"Spot {i + 1,3}: {TypeOf(v),-4} {RegOf(v)}");
                count++;
            }
        }
        if (count == 0) Console.WriteLine("The parking lot is empty");
        else Console.WriteLine($"\nTotal: {count} vehicles.");
    }
    #endregion

    #region Exit program
    static void ExitProgram()
    {
        Console.WriteLine("Are you sure you want to exit (Y/N)");
        var response = Console.ReadKey(true);
        if (response.Key == ConsoleKey.Y)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Exiting the Program. . .");
            Console.ResetColor();
            IsRunning = false;
        }
    }
    #endregion

    //--------------------
    // Parking logic
    //--------------------
    static string[] GetVehicles(int spot)
    {
        if (string.IsNullOrEmpty(ParkingSpots[spot])) return new string[0];
        return ParkingSpots[spot].Split('|', StringSplitOptions.RemoveEmptyEntries);
    }


    static string TypeOf(string vehicle) => vehicle.Split('#')[0];
    static string RegOf(string vehicle) => vehicle.Split('#')[1];

    static int SizeOf(string type) => type == "CAR" ? CarSize : McSize;


    static int UsedSize(int spot)
    {
        int total = 0;
        foreach (string v in GetVehicles(spot))
            total += SizeOf(TypeOf(v));
        return total;
    }

    static string GetVehiclesInSpot(int spot, string reg)
    {
        foreach (string v in GetVehicles(spot))
            if (RegOf(v) == reg) return v;
        return null;
    }

    static int FindVehicle(string reg)
    {
        for (int i = 0; i < SpotCount; i++)
            if (GetVehiclesInSpot(i, reg) != null) return i;
        return -1;
    }

    static int FindFreeSpot(string type)
    {
        int size = SizeOf(type);

        if (type == "MC")
            for (int i = 0; i < SpotCount; i++)
                if (UsedSize(i) + size <= SpotSize) return i;
        for (int i = 0; i < SpotCount; i++)
            if (UsedSize(i) + size <= SpotSize) return i;
        return -1;
    }

    static void AddToSpot(int spot, string type, string reg)
    {
        string entry = $"{type}#{reg}";
        ParkingSpots[spot] = string.IsNullOrEmpty(ParkingSpots[spot])
            ? entry
            : ParkingSpots[spot] + "|" + entry;
    }

    static void RemoveFromSpot(int spot, string reg)
    {
        string[] vehicles = GetVehicles(spot);
        string[] remaining = new string[vehicles.Length];
        int count = 0;

        foreach (string v in vehicles)
            if (RegOf(v) != reg)
            {
                remaining[count++] = v;
            }
        if (count == 0)
        {
            ParkingSpots[spot] = null;
        }
        else
        {
            ParkingSpots[spot] = string.Join("|", remaining, 0, count);
        }
    }
    //-------------------
    // Input / Output
    //-------------------

    static string AskVehicleType()
    {
        Console.WriteLine("Vehicle type - (C)ar or (M)otorcycle? ");
        var key = Console.ReadKey(true);
        Console.WriteLine();
        if (key.Key == ConsoleKey.C) return "CAR";
        if (key.Key == ConsoleKey.M) return "MC";
        Error("invalid choice");
        return null;
    }

    static string AskRegNr()
    {
        Console.Write("Registration number: ");
        string reg = (Console.ReadLine() ?? "").Trim().ToUpper();

        if (reg.Length == 0 || reg.Length > 10 || reg.Contains('#') || reg.Contains('|') || reg.Contains(' '))
        {
            Error("invalid registration number (1-10 characters, no spaces, '#' or '|'.");
            return null;
        }
        return reg;

    }


    static void Success(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    static void Error(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();

    }
    static DateTime GetPragueTime()
    {
        TimeZoneInfo pragueTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        return TimeZoneInfo.ConvertTime(DateTime.Now, pragueTimeZone);
    }
}