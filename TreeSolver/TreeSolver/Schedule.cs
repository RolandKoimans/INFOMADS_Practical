using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    public class Schedule
    {
        int[,] schedule;
        int rooms;

        public Schedule(int[,] schedule, int rooms)
        {
            this.schedule = schedule;
            this.rooms = rooms;
        }
    }
}
