using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace FxMaths
{
    public static class TimeStatistics
    {
        private static Stopwatch watch = new Stopwatch();

        /// <summary>
        /// Reset the counting Clock
        /// </summary>
        public static void StartClock(){
            watch.Reset();
            watch.Start();
        }

        /// <summary>
        /// Stop The clock and print the time that have pass in the console.
        /// </summary>
        public static float StopClock(float loops = 1, Boolean useMessageBox = false)
        {
            watch.Stop();

            // Console.WriteLine( "Elapsed: {0}", watch.Elapsed );
            Console.WriteLine("In milliseconds: {0}", watch.ElapsedMilliseconds / loops);
            // Console.WriteLine( "In timer ticks: {0}", watch.ElapsedTicks / loops );

            if (useMessageBox)
                MessageBox.Show("In milliseconds: " + (watch.ElapsedMilliseconds / loops).ToString());

            return watch.ElapsedMilliseconds / loops;
        }

        /// <summary>
        /// print in the console the time that have passed
        /// </summary>
        public static float ClockLap()
        {
            Console.WriteLine( "In milliseconds: {0}", watch.ElapsedMilliseconds);

            return watch.ElapsedMilliseconds;
        }

        /// <summary>
        /// print in the console the time that have passed
        /// </summary>
        public static float ClockLap( String Text, Boolean resetClock = false )
        {
            long time = watch.ElapsedMilliseconds;

            Console.WriteLine( "[{0}] In milliseconds: {1}  In FPS: {2}", Text, time, 1.0/(time*0.001) );

            if ( resetClock ) {
                watch.Reset();
                watch.Start();
            }
            return time;
        }
    }
}
