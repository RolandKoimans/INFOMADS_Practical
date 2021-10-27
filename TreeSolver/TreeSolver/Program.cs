using System;
using System.Collections.Generic;
using System.IO;

namespace TreeSolver
{
    class Program
    {
        // THIS IS A SOLUTION FOR THE OFFLINE VARIANT OF THE PRACTICAL

       
        static void Main(string[] args)
        {
             // Toggles console / file input
            bool ENABLECONSOLEINPUT = true;

            if (ENABLECONSOLEINPUT)
            {
                // Handle all input
                int firstDoseTime = Int32.Parse(Console.ReadLine());
                int secondDoseTime = Int32.Parse(Console.ReadLine());
                int gapTime = Int32.Parse(Console.ReadLine());
                int patientAmount = Int32.Parse(Console.ReadLine());


                Console.WriteLine("========= " + "Console Input" + " =========");
                Console.WriteLine(firstDoseTime);
                Console.WriteLine(secondDoseTime);
                Console.WriteLine(gapTime);
                Console.WriteLine(patientAmount);

                if (patientAmount > 0)
                {
                    // Reading all patients
                    List<Patient> patients = new List<Patient>();
                    for (int i = 0; i < patientAmount; i++)
                    {
                        // Separate values
                        string patientInput = Console.ReadLine();
                        Console.WriteLine(patientInput);
                        string[] patientline = patientInput.Split(',');

                        // Remove whitespace
                        foreach (string str in patientline)
                        {
                            str.Trim();
                        }

                        patients.Add(new Patient(Int32.Parse(patientline[0]), Int32.Parse(patientline[1]), Int32.Parse(patientline[2]), Int32.Parse(patientline[3]), i + 1));

                    }

                    Console.WriteLine();

                    // Run algorithm
                    ScheduleSolver schedule = new ScheduleSolver(patients, firstDoseTime, secondDoseTime, gapTime);

                    schedule.CreateOptimalSchedule();
                }
                else
                {
                    Console.WriteLine("No patients to be scheduled");
                }
            }
            else
            {

                foreach (string file in Directory.EnumerateFiles("files/", "*.txt"))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        // Handle all input
                        int firstDoseTime = Int32.Parse(reader.ReadLine());
                        int secondDoseTime = Int32.Parse(reader.ReadLine());
                        int gapTime = Int32.Parse(reader.ReadLine());
                        int patientAmount = Int32.Parse(reader.ReadLine());

                        Console.WriteLine("========= " + file + " =========");
                        Console.WriteLine(firstDoseTime);
                        Console.WriteLine(secondDoseTime);
                        Console.WriteLine(gapTime);
                        Console.WriteLine(patientAmount);

                        if (patientAmount > 0)
                        {
                            // Reading all patients
                            List<Patient> patients = new List<Patient>();
                            for (int i = 0; i < patientAmount; i++)
                            {
                                // Separate values
                                string patientInput = reader.ReadLine();
                                Console.WriteLine(patientInput);
                                string[] patientline = patientInput.Split(',');

                                // Remove whitespace
                                foreach (string str in patientline)
                                {
                                    str.Trim();
                                }

                                patients.Add(new Patient(Int32.Parse(patientline[0]), Int32.Parse(patientline[1]), Int32.Parse(patientline[2]), Int32.Parse(patientline[3]), i + 1));

                            }

                            Console.WriteLine();

                            // Run algorithm
                            ScheduleSolver schedule = new ScheduleSolver(patients, firstDoseTime, secondDoseTime, gapTime);

                            schedule.CreateOptimalSchedule();
                        }
                        else
                        {
                            Console.WriteLine("No patients to be scheduled");
                        }
                    }

                }
            }

        }

    }
}
