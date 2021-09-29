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

            return new Schedule(scheduleMatrix, 0, patients);

        }

        public void CreateOptimalSchedule()
        {
            // Based on Branch and Bound
            // The idea here is to go depth-first, and once a feasible solution has been found,
            // Use that solution as a bound.


            // Worst solution is when every patient has their own hospital room.
            int bound = patients.Count;

            // Using a stack for depth-first.
            Stack<Schedule> schedules = new Stack<Schedule>();

            // Put the empty schedule to start with on the queue
            schedules.Push(CreateEmpty());

            while(schedules.Count > 0)
            {
                Schedule currentSchedule = schedules.Pop();

                // if rooms used > bound, do nothing
                if (currentSchedule.rooms > bound)
                {

                }
                // if all patients are scheduled and rooms used < bound, update bound
                else if (false){

                }
                // else branch
                else
                {

                }
          
                
            }

        }

    }
}
