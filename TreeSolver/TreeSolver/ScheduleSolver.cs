using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    public class ScheduleSolver
    {
        public List<Patient> patients;
        public int firstDoseTime;
        public int secondDoseTime;
        public int gapTime;

        public ScheduleSolver(List<Patient> patients, int firstDoseTime, int secondDoseTime, int gapTime )
        {
            this.patients = patients;
        }

        public Schedule CreateEmpty()
        {
            //Check max time duration
            int maxTime = 0;
            for(int i = 0; i < patients.Count; i++)
            {
                maxTime = Math.Max(patients[i].firstIntervalEnd + gapTime + patients[i].personalGapTime + patients[i].secondIntervalLength, maxTime);
            }

            // Maximum amount of rooms: every patient has a room for themself.
            int rooms = patients.Count;

            int[,] scheduleMatrix = new int[maxTime, rooms];

            for(int i = 0; i < rooms; i++)
            {
                for(int j = 0; j<maxTime; j++)
                {
                    scheduleMatrix[j, i] = 0;
                }
            }

            return new Schedule(scheduleMatrix, 0);

        }

        public void CreateSchedule()
        {

        }

    }
}
