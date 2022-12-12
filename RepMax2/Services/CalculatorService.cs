using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMax2.Services
{
    public static class CalculatorService
    {
        public static float Estimate1RM(float weight, int reps)
        {
            var est = weight + (weight * reps * 0.0333f);
            return est;
        }
    }
}
