using System;
using System.Collections.Generic;

namespace TreeSolver
{
    class Program
    {
        // THIS IS A SOLUTION FOR THE OFFLINE VARIANT OF THE PRACTICAL
        static void Main(string[] args)
        {
            // Handle all input
            int firstDoseTime = Int32.Parse(Console.ReadLine());
            int secondDoseTime = Int32.Parse(Console.ReadLine());
            int gapTime = Int32.Parse(Console.ReadLine());
            int patientAmount = Int32.Parse(Console.ReadLine());


            // Reading all patients
            List<Patient> patients = new List<Patient>();
            for (int i = 0; i < patientAmount; i++)
            {
                // Separate values
                string[] patientline = Console.ReadLine().Split(',');
                
                // Remove whitespace
                foreach(string str in patientline)
                {
                    str.Trim();
                }

                patients.Add(new Patient(Int32.Parse(patientline[0]), Int32.Parse(patientline[1]), Int32.Parse(patientline[2]), Int32.Parse(patientline[3])));
            }


            // Run algorithm
            ScheduleSolver schedule = new ScheduleSolver(patients, firstDoseTime, secondDoseTime, gapTime);

            int[,] sched = new int[,] { {1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            schedule.PrettySchedule(sched);

        }

    }
}
