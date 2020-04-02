using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzySystemGUI
{
    class MembershipPoint : MembershipFunction
    {
        public MembershipPoint(double[] coordinates, string term) : base(coordinates, term)
        {
            N = 1;
        }

        public override double calculateProbability(double param)
        {
            if (param == Coordinates[0])
                return 1.0;
            else
                return 0.0;
        }
    }
}
