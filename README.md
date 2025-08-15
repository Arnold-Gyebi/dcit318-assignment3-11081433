# AssignmentApp

This project contains solutions to the DCIT 318 – Programming II Assignment 3.  
Each question is implemented in its own C# file and namespace, with a main `Program.cs` that runs them in sequence.

## Project Structure

```
AssignmentApp/
│
├── Program.cs                  // Entry point – runs all question demos
├── Q1_Finance.cs                // Question 1: Finance Management System
├── Q2_Healthcare.cs             // Question 2: Healthcare System
├── Q3_Warehouse.cs              // Question 3: Warehouse Inventory
├── Q4_Grading.cs                // Question 4: Grading System
└── Q5_InventoryRecords.cs       // Question 5: Inventory Records
```

## Requirements
- .NET 6.0 SDK or later
- A C# IDE like Visual Studio, Rider, or VS Code with the C# extension

## How to Run
1. Open the folder in your preferred C# IDE or terminal.
2. If using the terminal:
   ```bash
   dotnet run
   ```
3. The console will display the output for all five questions in sequence.

## Notes
- Some questions create and read files during execution (`q4_io` and `q5_io` directories will appear in your project folder).
- The program is self-contained and uses only standard .NET libraries.
- You can modify sample data inside each question's `Run()` method to test different scenarios.
