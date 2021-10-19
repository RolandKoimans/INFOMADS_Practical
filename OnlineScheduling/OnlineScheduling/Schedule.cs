using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineScheduling
{
    class Schedule
    {
        public List<List<int>> schedule;
        public int rooms;
        public int maxTime;
        public Schedule(List<List<int>> schedule, int rooms, int maxTime)
        {
            this.schedule = schedule;
            this.rooms = rooms;
        }


        public void PrettySchedule()
        {
            for(int i = 0; i < schedule.Count; i++)
            {
                for(int j = 0; j < schedule[i].Count; j++)
                {
                    Console.Write(schedule[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

    }

}
