using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                maxTime = Math.Max(patients[i].firstIntervalEnd + gapTime + patients[i].personalGapTime + patients[i].secondIntervalLength + 3, maxTime);
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
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Based on Branch and Bound
            // The idea here is to go depth-first, and once a feasible solution has been found,
            // Use that solution as a bound.
            Schedule bestSchedule = CreateEmpty();


            // Worst solution is when every patient has their own hospital room.
            int bound = patients.Count;

            // Try to improve the bound by running some basic greedy algorithms
            Heuristic[] heuristics = new Heuristic[]
            {
                new FirstFit(), new LastFit(), new RandomFit()
            };
            foreach(Heuristic hr in heuristics)
            {
                bound = Math.Min(hr.Solve(patients, firstDoseTime, secondDoseTime, gapTime), bound);
            }


            // Using a stack for depth-first.
            Stack<Schedule> schedules = new Stack<Schedule>();

            // Put the empty schedule to start with on the stack
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
                            for (int i = 0; i < curPatient.firstIntervalEnd - curPatient.firstIntervalStart - firstDoseTime + 2; i++)
                            {
                                bool blockedFirst = false;
                                // start + shift until start + shift + jabtime, this doesn't exceed interval due to previous loop.
                                for (int j = curPatient.firstIntervalStart + i; j < curPatient.firstIntervalStart + i + firstDoseTime - 1; j++)
                                {
                                    if(currentSchedule.schedule[k,j] != 0)
                                    {
                                        blockedFirst = true;
                                    }
                                }

                                //First jab fits, now try every second jab
                                if (!blockedFirst)
                                {
                                    // a is the room we are trying to fit jab 2 in
                                    for (int a = 0; a <= currentSchedule.rooms; a++)
                                    {
                                        // b is the shift in position while trying to fit jab 2
                                        for(int b = 0; b < curPatient.secondIntervalLength - secondDoseTime + 1; b++)
                                        {
                                            bool blockedSecond = false;
                                            // c is a specific timeslot in which we check whether a patient fits or not
                                            for(int c = curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime + b; c < curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime + b + secondDoseTime; c++)
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
                                                

                                                for (int j = curPatient.firstIntervalStart + i; j <= curPatient.firstIntervalStart + i + firstDoseTime - 1; j++)
                                                {
                                                    newSchedule.schedule[k, j] = curPatient.id;
                                                }
                                                for (int c = curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime + b; c < curPatient.firstIntervalStart + i + firstDoseTime + gapTime + curPatient.personalGapTime + secondDoseTime + b; c++)
                                                {
                                                   newSchedule.schedule[a, c] = curPatient.id;
                                                        
                                                }

                                                if(k == currentSchedule.rooms || a == currentSchedule.rooms)
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

                                                    //Console.WriteLine("New schedule found");
                                                    //PrettySchedule(newSchedule.schedule);

                                                    schedules.Push(newSchedule);
                                                }
                                                else
                                                {
                                                    if(currentSchedule.rooms < bound)
                                                    {
                                                        bound = newSchedule.rooms;
                                                        bestSchedule = newSchedule;

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
            sw.Stop();
            Console.WriteLine("Optimal schedule found. Elapsed time: " + sw.Elapsed);
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
