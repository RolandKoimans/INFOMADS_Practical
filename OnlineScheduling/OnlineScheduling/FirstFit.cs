using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineScheduling
{
    public class FirstFit
    {
        public int firstDoseTime;
        public int secondDoseTime;
        public int gapTime;

        public FirstFit(int firstDoseTime, int secondDoseTime, int gapTime)
        {
            this.firstDoseTime = firstDoseTime;
            this.secondDoseTime = secondDoseTime;
            this.gapTime = gapTime;
        }

        public void Solve()
        {
            bool done = false;

            // Keep track on the amount of patients encountered.
            int patientCount = 0;

            // Setup output
            List<int[]> output = new List<int[]>();

            Schedule startSchedule = new Schedule(new List<List<int>>(), patientCount, 0);
            startSchedule.schedule.Add(new List<int>());

            // Keep going until "x" is encountered, indicating end of input
            while (!done)
            {
                // Check if there is a new patient
                string input = Console.ReadLine();
                if (input == "x")
                {
                    done = true;
                }
                else
                {
                    //Add new entry to fit patient
                    output.Add(new int[4]);

                    // Handle patient input
                    string[] inputSplit = input.Split(',');


                    foreach (string str in inputSplit)
                    {
                        str.Trim();
                    }

                    Patient pat = new Patient(Int32.Parse(inputSplit[0]), Int32.Parse(inputSplit[1]), Int32.Parse(inputSplit[2]), Int32.Parse(inputSplit[3]), patientCount + 1);
                    patientCount++;

                    int newMaxTime = pat.firstIntervalEnd + gapTime + pat.personalGapTime + pat.secondIntervalLength;

                    // Adjust schedule dimension to a new maximum time
                    if (newMaxTime > startSchedule.maxTime)
                    {
                        startSchedule.maxTime = newMaxTime;

                        for (int i = 0; i < startSchedule.schedule.Count; i++)
                        {
                            for (int j = startSchedule.schedule[i].Count; j <= newMaxTime; j++)
                            {
                                startSchedule.schedule[i].Add(0);
                            }
                        }
                    }

                    bool found = false;
                    for (int k = 0; k <= startSchedule.rooms; k++)
                    {
                        if (found) break;
                        for (int i = 0; i < pat.firstIntervalEnd - pat.firstIntervalStart - firstDoseTime + 2; i++)
                        {
                            if (found) break;
                            bool blockedFirst = false;
                            // start + shift until start + shift + jabtime, this doesn't exceed interval due to previous loop.
                            for (int j = pat.firstIntervalStart + i; j <= pat.firstIntervalStart + i + firstDoseTime - 1; j++)
                            {
                                if (found) break;
                                if (startSchedule.schedule[k][j] != 0)
                                {
                                    blockedFirst = true;
                                }
                            }

                            //First jab fits, now try every second jab
                            if (!blockedFirst)
                            {
                                // a is the room we are trying to fit jab 2 in
                                for (int a = 0; a <= startSchedule.rooms; a++)
                                {
                                    if (found) break;
                                    // b is the shift in position while trying to fit jab 2
                                    for (int b = 0; b < pat.secondIntervalLength - secondDoseTime + 1; b++)
                                    {
                                        if (found) break;

                                        bool blockedSecond = false;
                                        // c is a specific timeslot in which we check whether a patient fits or not
                                        for (int c = pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + b; c < pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + b + secondDoseTime; c++)
                                        {
                                            if (startSchedule.schedule[a][c] != 0)
                                            {
                                                blockedSecond = true;
                                            }
                                        }

                                        //Both first and second jab fit: create a new schedule and add to the stack
                                        if (!blockedSecond)
                                        {

                                            for (int j = pat.firstIntervalStart + i; j <= pat.firstIntervalStart + i + firstDoseTime - 1; j++)
                                            {
                                                startSchedule.schedule[k][j] = pat.id;

                                            }
                                            output[patientCount - 1][0] = pat.firstIntervalStart + i;
                                            output[patientCount - 1][1] = k + 1;
                                            for (int c = pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + b; c < pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + secondDoseTime + b; c++)
                                            {
                                                startSchedule.schedule[a][c] = pat.id;


                                            }
                                            output[patientCount - 1][2] = pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + b;
                                            output[patientCount - 1][3] = a + 1;

                                            if (k == startSchedule.rooms || a == startSchedule.rooms)
                                            {
                                                startSchedule.rooms++;

                                                // Adjust dimensions in case of new room
                                                startSchedule.schedule.Add(new List<int>());
                                                for (int z = 0; z <= startSchedule.maxTime; z++)
                                                {
                                                    startSchedule.schedule[startSchedule.schedule.Count - 1].Add(0);
                                                }
                                            }

                                            found = true;

                                            // Output intermediate solution
                                            Console.WriteLine();
                                            startSchedule.PrettySchedule();


                                        }

                                    }
                                }
                            }

                        }
                    }




                }



            }

            startSchedule.PrettySchedule();

            // Official output
            for (int i = 0; i < output.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(output[i][j]);
                    if (j != 3)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(startSchedule.rooms);
        }


    }




}
