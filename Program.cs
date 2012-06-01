using System;
using System.Diagnostics;
using System.Drawing;

namespace MattChoinski
{
    class Program
    {
        static void Main( string[] args )
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                string currentDirectory = Environment.CurrentDirectory;

                Bitmap firstImage =
                    new Bitmap( currentDirectory + "\\Compare_A.jpg" );
                Bitmap secondImage =
                    new Bitmap( currentDirectory + "\\Compare_B.jpg" );
                Bitmap analyzedImage =
                    ImageProcessor.AnalyzeImageDifferences(
                        firstImage, secondImage );
                analyzedImage.Save( currentDirectory + "\\AnalyzedImage.bmp" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( "Error: " + e.Message );
            }

            stopWatch.Stop();

            TimeSpan timeElapsed = stopWatch.Elapsed;
            string elapsedTime =
                    String.Format(
                        "{0:00}.{1:00}",
                        timeElapsed.Seconds,
                        timeElapsed.Milliseconds / 10 );

            Console.WriteLine( "The images were analyzed in " + elapsedTime + " seconds." );
            Console.ReadLine();
        }
    }
}