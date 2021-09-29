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
    }
}
