﻿using System;
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
            this.firstDoseTime = firstDoseTime;
            this.secondDoseTime = secondDoseTime;
            this.gapTime = gapTime;
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

            int[,] scheduleMatrix = new int[rooms, maxTime];

            for(int i = 0; i < rooms; i++)
            {
                for(int j = 0; j<maxTime; j++)
                {
                    scheduleMatrix[i, j] = 0;
                }
            }

            return new Schedule(scheduleMatrix, 0, patients);

        }

        public Schedule CopySchedule(Schedule sched)
        {

            int[,] copySched = new int[sched.schedule.GetLength(0), sched.schedule.GetLength(1)];
            for(int i = 0; i < sched.schedule.GetLength(0); i++)
            {
                for( int j = 0; j < sched.schedule.GetLength(1); j++)
                {
                    copySched[i, j] = sched.schedule[i, j];
                }
            }

            List<Patient> copyPatients = new List<Patient>();
            for(int i = 0; i < sched.availablePatients.Count; i++)
            {
                copyPatients.Add(sched.availablePatients[i]);
            }

            return new Schedule(copySched, sched.rooms, copyPatients);
        }

        public void CreateOptimalSchedule()
        {
            // Based on Branch and Bound
            // The idea here is to go depth-first, and once a feasible solution has been found,
            // Use that solution as a bound.
            Schedule bestSchedule = CreateEmpty();


            // Worst solution is when every patient has their own hospital room.
            //int bound = patients.Count;
            int bound = patients.Count;

            // Using a stack for depth-first.
            Stack<Schedule> schedules = new Stack<Schedule>();

            // Put the empty schedule to start with on the queue
            schedules.Push(CreateEmpty());

            while(schedules.Count > 0)
            {
                Schedule currentSchedule = schedules.Pop();

                // if rooms used > bound, do nothing
                if (currentSchedule.rooms <= bound)
                {
                    // if all patients are scheduled and rooms used < bound, update bound
                    if (false)
                    {

                    }
                    // else branch
                    else
                    {
                        // Take first patient from list
                        Patient curPatient = currentSchedule.availablePatients[0];

                        // Try to fit first jab on every position in every room + every position in a new room
                        for (int k = 0; k <= currentSchedule.rooms; k++)
                        {
                            for (int i = 0; i <= curPatient.firstIntervalEnd - curPatient.firstIntervalStart - firstDoseTime; i++)
                            {
                                bool blockedFirst = false;
                                // start + shift until start + shift + jabtime, this doesn't exceed interval due to previous loop.
                                for (int j = curPatient.firstIntervalStart + i; j < curPatient.firstIntervalStart + i + firstDoseTime; j++)
                                {
                                    if(currentSchedule.schedule[k,j] != 0)
                                    {
                                        blockedFirst = true;
                                    }
                                }

                                //First jab fits, now try every second jab
                                if (!blockedFirst)
                                {
                                    for (int a = 0; a <= currentSchedule.rooms; a++)
                                    {
                                        for(int b = 0; b <= curPatient.secondIntervalLength - secondDoseTime; b++)
                                        {
                                            bool blockedSecond = false;
                                            for(int c = curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime; c < curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime + secondDoseTime; c++)
                                            {
                                                if (currentSchedule.schedule[a, c] != 0)
                                                {
                                                    blockedSecond = true;
                                                }
                                            }

                                            //Both first and second jab fit: create a new schedule and add to the stack
                                            if (!blockedSecond)
                                            {
                                                Schedule newSchedule = CopySchedule(currentSchedule);

                                                for (int j = curPatient.firstIntervalStart + i; j < curPatient.firstIntervalStart + i + firstDoseTime; j++)
                                                {
                                                    newSchedule.schedule[k, j] = curPatient.id;
                                                }
                                                for (int c = curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime + b; c < curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime + secondDoseTime + b; c++)
                                                {
                                                    newSchedule.schedule[a, c] = curPatient.id;
    
                                                }

                                                if(k == currentSchedule.rooms)
                                                {
                                                    newSchedule.rooms++;
                                                }

                                                List<Patient> remainingPatients = new List<Patient>();
                                                if(currentSchedule.availablePatients.Count > 1)
                                                {
                                                    for(int p = 1; p < currentSchedule.availablePatients.Count; p++)
                                                    {
                                                        remainingPatients.Add(currentSchedule.availablePatients[p]);
                                                    }

                                                    newSchedule.availablePatients = remainingPatients;

                                                    Console.WriteLine("New schedule found");
                                                    PrettySchedule(newSchedule.schedule);

                                                    schedules.Push(newSchedule);
                                                }
                                                else
                                                {
                                                    if(currentSchedule.rooms < bound)
                                                    {
                                                        bound = currentSchedule.rooms;
                                                        bestSchedule = currentSchedule;
                                                    }
                                                }


                                            }

                                        }
                                    }
                                }

                            }
                        }

                    }
                }
                
 
          
                
            }

            PrettySchedule(bestSchedule.schedule);

        }

        public void PrettySchedule(int[,] sched)
        {
            for(int i = 0; i < sched.GetLength(0); i++)
            {
                Console.WriteLine();
                for(int j = 0; j<sched.GetLength(1); j++)
                {
                    Console.Write(sched[i, j] + " ");
                }
            }
            Console.WriteLine();
        }

    }
}
