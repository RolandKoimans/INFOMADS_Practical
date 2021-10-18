using System;

namespace OnlineScheduling
{
    class Program
    {
        // THIS IS A SOLUTION FOR THE ONLINE VARIANT OF THE PRACTICAL
        static void Main(string[] args)
        {
            //Handle initial input
            int firstDoseTime = Int32.Parse(Console.ReadLine());
            int secondDoseTime = Int32.Parse(Console.ReadLine());
            int gapTime = Int32.Parse(Console.ReadLine());

            FirstFit problem = new FirstFit(firstDoseTime, secondDoseTime, gapTime);
            problem.Solve();

        }
    }
}
