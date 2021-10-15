﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TreeSolver
{
    class FirstFit : Heuristic
    {
        public override int Solve(List<Patient> patients, int firstDoseTime, int secondDoseTime, int gapTime)
        {
            return patients.Count;
        }

    }
}