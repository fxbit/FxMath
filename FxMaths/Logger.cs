using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FxMaths
{
    public static class Logger
    {
        private static TextWriter TW = null;

        private static String LogFilename = @"log.txt";

        /// <summary>
        /// Write a line to log file
        /// </summary>
        /// <param name="str"></param>
        public static void WriteLine( String str )
        {
            // open the text file if is not exist
            if ( TW == null )
                TW = File.CreateText( LogFilename );

            // write the string 
            TW.WriteLine( " [ " + DateTime.Now.ToShortTimeString() + " ] " + str );
            TW.Flush();
        }
    }
}
