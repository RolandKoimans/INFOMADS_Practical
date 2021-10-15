using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    abstract class Heuristic
    {
        public abstract int Solve(List<Patient> patients, int firstDoseTime, int secondDoseTime, int gapTime);
    }

}
