using System;

namespace Assignment
{
    class Program
    {
        static void Main()
        {
            new Q1.FinanceApp().Run();
            new Q2.HealthSystemApp().Run();
            new Q3.WareHouseManager().Run();
            new Q4.GradingDemo().Run();
            new Q5.InventoryApp(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "q5_io", "inventory.json")).Run();

            Console.WriteLine("All demos completed.");
        }
    }
}
