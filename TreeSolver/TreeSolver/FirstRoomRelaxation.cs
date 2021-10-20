using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    public class FirstRoomRelaxation
    {
        // This relaxation searches for the minimum number of rooms used if only the first jab has to be scheduled. This is equal
        // or better than the full problem, and gives us a bound to determine whether we should continue in a branch in the
        // main problem.

        List<Patient> patients;
        int firstDoseTime;
        int currentBound;
        Schedule schedule;

        public FirstRoomRelaxation(int firstDoseTime, Schedule schedule, int currentBound)
        {
            this.patients = schedule.availablePatients;
            this.firstDoseTime = firstDoseTime;
            this.schedule = schedule;
            this.currentBound = currentBound;
        }

        public int SolveRelaxation()
        {

            // Given schedule is the best we can do in this instance.
            Schedule bestSchedule = schedule.CopySchedule();


            // Worst solution is when every patient has their own hospital room.
            int bound = currentBound;

            // Using a stack for depth-first.
            Stack<Schedule> schedules = new Stack<Schedule>();

            // Put the empty schedule to start with on the stack
            schedules.Push(schedule.CopySchedule());
            while (schedules.Count > 0)
            {
                Schedule currentSchedule = schedules.Pop();

                // if rooms used > bound, do nothing
                if (currentSchedule.rooms < bound)
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
                            for (int j = curPatient.firstIntervalStart + i; j <= curPatient.firstIntervalStart + i + firstDoseTime - 1; j++)
                            {
                                if (currentSchedule.schedule[k, j] != 0)
                                {
                                    blockedFirst = true;
                                }
                            }

                            //First jab fits, now try every second jab
                            if (!blockedFirst)
                            {


                                //Both first and second jab fit: create a new schedule and add to the stack

                                {
                                    Schedule newSchedule = currentSchedule.CopySchedule();


                                    for (int j = curPatient.firstIntervalStart + i; j <= curPatient.firstIntervalStart + i + firstDoseTime - 1; j++)
                                    {
                                        newSchedule.schedule[k, j] = curPatient.id;
                                    }


                                    if (k == currentSchedule.rooms)
                                    {
                                        newSchedule.rooms++;
                                    }

                                    List<Patient> remainingPatients = new List<Patient>();
                                    if (currentSchedule.availablePatients.Count > 1)
                                    {
                                        for (int p = 1; p < currentSchedule.availablePatients.Count; p++)
                                        {
                                            remainingPatients.Add(currentSchedule.availablePatients[p]);
                                        }

                                        newSchedule.availablePatients = remainingPatients;

                                        schedules.Push(newSchedule);
                                    }
                                    else
                                    {
                                        if (currentSchedule.rooms <= bound)
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
            return bestSchedule.rooms;
        }

    }
}
