using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    public class Schedule
    {
        public double[,] schedule;
        public int rooms;
        public List<Patient> availablePatients;

        public Schedule(double[,] schedule, int rooms, List<Patient> availablePatients)
        {
            this.schedule = schedule;
            this.rooms = rooms;
            this.availablePatients = availablePatients;
        }

        public Schedule CopySchedule()
        {

            double[,] copySched = new double[schedule.GetLength(0), schedule.GetLength(1)];
            for (int i = 0; i < schedule.GetLength(0); i++)
            {
                for (int j = 0; j < schedule.GetLength(1); j++)
                {
                    copySched[i, j] = schedule[i, j];
                }
            }

            List<Patient> copyPatients = new List<Patient>();
            for (int i = 0; i < availablePatients.Count; i++)
            {
                copyPatients.Add(availablePatients[i]);
            }

            return new Schedule(copySched, rooms, copyPatients);
        }

        public void PrettySchedule()
        {
            for (int i = 0; i < schedule.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < schedule.GetLength(1); j++)
                {
                    Console.Write(schedule[i, j] + " ");
                }
            }
            Console.WriteLine();
        }

        public void OfficialSchedule(int totalPatients, int firstJabTime, int secondJabTime)
        {
            int[,] outputSchedule = new int[totalPatients, 4];

            for(int i = 0; i<schedule.GetLength(0); i++)
            {
                for(int j = 0; j < schedule.GetLength(1); j++)
                {
                    if(schedule[i,j] != 0)
                    {
                        int patientNr = (int)schedule[i, j];

                        // Convert the .1 or .2 to 1 or 2, add 0.01 to avoid problems with double inaccuracy.
                        int jabNr = (int)((schedule[i, j] % patientNr + 0.01) * 10);

                        // Start time of found jab 
                        outputSchedule[patientNr - 1, jabNr == 1 ? 0 : 2] = j;

                        // Room number of found jab
                        outputSchedule[patientNr - 1, (jabNr == 1 ? 0 : 2) + 1] = i+1;

                        // Add either firstJabTime or secondJabTime depending on which jab we found
                        // in order to avoid collision of finding the same jab multiple times
                        j += jabNr == 1 ? firstJabTime-1 : secondJabTime-1;

                    }
                }
            }

            // Output Schedule
            for(int i = 0; i < outputSchedule.GetLength(0); i++)
            {
                for(int j = 0; j<4; j++)
                {
                    
                    Console.Write(outputSchedule[i, j]);
                    if (j != 3)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(rooms);
        }

    }
}
