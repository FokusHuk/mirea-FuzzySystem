using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzySystemGUI
{
    class TrapezoidalFunction : MembershipFunction
    {
        public TrapezoidalFunction(double[] coordinates, string term) : base(coordinates, term)
        {
            N = 4;
        }

        public override double calculateProbability(double param)
        {
            double probability = 0.0;


            if (param == Coordinates[0] || param == Coordinates[3])
                return 0.01;

            if (param >= Coordinates[0] && param <= Coordinates[3])
            {
                if (param < Coordinates[1])
                {
                    probability = (param - Coordinates[0]) / (Coordinates[1] - Coordinates[0]);
                }
                else if (param <= Coordinates[2])
                {
                    probability = 1.0;
                }
                else
                {
                    probability = (Coordinates[3] - param) / (Coordinates[3] - Coordinates[2]);
                }
            }

            return probability;
        }
    }
}
