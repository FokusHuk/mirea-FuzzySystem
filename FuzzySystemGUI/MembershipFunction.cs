using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzySystemGUI
{
    abstract class MembershipFunction
    {
        public string Term { get; set; }

        public double[] Coordinates { get; set; }

        public int N { get; set; }

        public MembershipFunction(double[] coordinates, string term)
        {
            Coordinates = coordinates;
            Term = term;
        }

        public abstract double calculateProbability(double param);
    }
}
