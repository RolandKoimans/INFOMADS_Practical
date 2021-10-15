﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    class LastFit : Heuristic
    {
        public override int Solve(List<Patient> patients, int firstDoseTime, int secondDoseTime, int gapTime, Schedule startSchedule)
        {
            return patients.Count;
        }
    }
}
