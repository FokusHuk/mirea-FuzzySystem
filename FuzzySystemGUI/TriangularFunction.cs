using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzySystemGUI
{
    class TriangularFunction : MembershipFunction
    {
        public TriangularFunction(double[] coordinates, string term) : base(coordinates, term)
        {
            N = 3;
        }

        public override double calculateProbability(double param)
        {
            double probability = 0.0;


            if (param == Coordinates[0] || param == Coordinates[2])
                return 0.01;

            if (param >= Coordinates[0] && param <= Coordinates[2])
            {
                if (param <= Coordinates[1])
                {
                    probability = (param - Coordinates[0]) / (Coordinates[1] - Coordinates[0]);
                }
                else
                {
                    probability = (Coordinates[2] - param) / (Coordinates[2] - Coordinates[1]);
                }
            }

            return probability;
        }
    }
}
