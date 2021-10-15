using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    public class Schedule
    {
        public int[,] schedule;
        public int rooms;
        public List<Patient> availablePatients;

        public Schedule(int[,] schedule, int rooms, List<Patient> availablePatients)
        {
            this.schedule = schedule;
            this.rooms = rooms;
            this.availablePatients = availablePatients;
        }

        public Schedule CopySchedule()
        {

            int[,] copySched = new int[schedule.GetLength(0), schedule.GetLength(1)];
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
    }
}
