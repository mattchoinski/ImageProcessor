using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MattChoinski
{
    class ImageProcessor
    {
        public static Bitmap AnalyzeImageDifferences(
            Bitmap originalImage,
            Bitmap newImage )
        {
            int xCoordinate = 0;
            int yCoordinate = 0;

            BitmapData newData =
                newImage.LockBits(
                    new Rectangle(
                        xCoordinate,
                        yCoordinate,
                        newImage.Width,
                        newImage.Height ),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb );
            BitmapData originalData =
                originalImage.LockBits(
                    new Rectangle(
                        xCoordinate,
                        yCoordinate,
                        originalImage.Width,
                        originalImage.Height ),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb );

            PointOfInterestList pointOfInterestList =
                new PointOfInterestList();

            unsafe
            {
                byte* newPointer = ( byte* )newData.Scan0;
                byte* originalPointer = ( byte* )originalData.Scan0;

                for ( yCoordinate = 0;
                        yCoordinate < originalData.Height;
                        ++yCoordinate )
                {
                    for ( xCoordinate = 0;
                            xCoordinate < originalData.Width;
                            ++xCoordinate )
                    {
                        int currentOriginalPixel = (
                                originalPointer[ 0 ] +
                                originalPointer[ 1 ] +
                                originalPointer[ 2 ] );
                        int currentNewPixel = (
                                newPointer[ 0 ] +
                                newPointer[ 1 ] +
                                newPointer[ 2 ] );
                        if ( currentNewPixel > currentOriginalPixel + 140
                            || currentNewPixel < currentOriginalPixel - 140 )
                        {
                            pointOfInterestList.ProcessCoordinates(
                                xCoordinate, yCoordinate );
                        }

                        newPointer += 3;
                        originalPointer += 3;
                    }

                    newPointer +=
                        newData.Stride - ( newData.Width * 3 );
                    originalPointer +=
                        originalData.Stride - ( originalData.Width * 3 );
                }
            }

            originalImage.UnlockBits( originalData );
            newImage.UnlockBits( newData );

            using ( Graphics graphics = Graphics.FromImage( newImage ) )
            {
                graphics.DrawRectangles(
                    new Pen( Color.Red, 1 ),
                    pointOfInterestList.GetRectangles() );
            }

            return newImage;
        }
    }
}
