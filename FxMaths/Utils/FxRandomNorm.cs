using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Utils
{
    public class FxRandomNorm
    {
        Random rand;
        double mean;
        double stdDev;

        public FxRandomNorm(double mean, double stdDev)
        {
            rand = new Random();
            this.mean = mean;
            this.stdDev = stdDev;
        }


        public double NextDouble()
        {
            double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles
            double u2 = rand.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        }

    }
}
