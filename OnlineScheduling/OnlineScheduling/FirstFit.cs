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
            int patientCount = 1;

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
                    // Handle patient input
                    string[] inputSplit = input.Split(',');

                    foreach (string str in inputSplit)
                    {
                        str.Trim();
                    }

                    Patient curPatient = new Patient(Int32.Parse(inputSplit[0]), Int32.Parse(inputSplit[1]), Int32.Parse(inputSplit[2]), Int32.Parse(inputSplit[3]), patientCount);
                    patientCount++;

                    //bool found = false;
                    //for (int k = 0; k <= startSchedule.rooms; k++)
                    //{
                    //    if (found) break;
                    //    for (int i = 0; i < pat.firstIntervalEnd - pat.firstIntervalStart - firstDoseTime + 2; i++)
                    //    {
                    //        if (found) break;
                    //        bool blockedFirst = false;
                    //        // start + shift until start + shift + jabtime, this doesn't exceed interval due to previous loop.
                    //        for (int j = pat.firstIntervalStart + i; j < pat.firstIntervalStart + i + firstDoseTime - 1; j++)
                    //        {
                    //            if (found) break;
                    //            if (startSchedule.schedule[k, j] != 0)
                    //            {
                    //                blockedFirst = true;
                    //            }
                    //        }

                    //        //First jab fits, now try every second jab
                    //        if (!blockedFirst)
                    //        {
                    //            // a is the room we are trying to fit jab 2 in
                    //            for (int a = 0; a <= startSchedule.rooms; a++)
                    //            {
                    //                if (found) break;
                    //                // b is the shift in position while trying to fit jab 2
                    //                for (int b = 0; b < pat.secondIntervalLength - secondDoseTime + 1; b++)
                    //                {
                    //                    if (found) break;

                    //                    bool blockedSecond = false;
                    //                    // c is a specific timeslot in which we check whether a patient fits or not
                    //                    for (int c = pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + b; c < pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + b + secondDoseTime; c++)
                    //                    {
                    //                        if (startSchedule.schedule[a, c] != 0)
                    //                        {
                    //                            blockedSecond = true;
                    //                        }
                    //                    }

                    //                    //Both first and second jab fit: create a new schedule and add to the stack
                    //                    if (!blockedSecond)
                    //                    {

                    //                        for (int j = pat.firstIntervalStart + i; j <= pat.firstIntervalStart + i + firstDoseTime - 1; j++)
                    //                        {
                    //                            startSchedule.schedule[k, j] = pat.id;
                    //                        }
                    //                        for (int c = pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + b; c < pat.firstIntervalStart + i + firstDoseTime + gapTime + pat.personalGapTime + secondDoseTime + b; c++)
                    //                        {
                    //                            startSchedule.schedule[a, c] = pat.id;

                    //                        }

                    //                        if (k == startSchedule.rooms || a == startSchedule.rooms)
                    //                        {
                    //                            startSchedule.rooms++;
                    //                        }

                    //                        found = true;

                    //                    }

                    //                }
                    //            }
                    //        }

                    //    }
                    //}




                }



            }
        }

    }




}
