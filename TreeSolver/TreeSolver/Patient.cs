using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    public class Patient
    {
        public int firstIntervalStart;
        public int firstIntervalEnd;
        public int personalGapTime;
        public int secondIntervalLength;
        public int id;

        public Patient(int firstIntervalStart, int firstIntervalEnd, int personalGapTime, int secondIntervalLength, int id)
        {
            this.firstIntervalStart = firstIntervalStart;
            this.firstIntervalEnd = firstIntervalEnd;
            this.personalGapTime = personalGapTime;
            this.secondIntervalLength = secondIntervalLength;
            this.id = id;
        }
    }
}
