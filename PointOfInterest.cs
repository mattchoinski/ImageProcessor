using System;
using System.Collections;
using System.Drawing;

namespace MattChoinski
{
    public class PointOfInterestList :
        CollectionBase
    {
        public PointOfInterest this[ int index ]
        {
            get { return ( PointOfInterest )this.List[ index ]; }
        }

        public void Add( PointOfInterest pointOfInterest )
        {
            this.List.Add( pointOfInterest );
        }

        private int FindExistingRectangle(
            int xCoordinate,
            int yCoordinate )
        {
            int indexOfMatchingPointOfInterest = -1;

            for ( int index = 0; index < this.List.Count; ++index )
            {
                PointOfInterest existing = this[ index ];

                if ( ( xCoordinate >= existing.XCoordinate - 40
                    && xCoordinate <= existing.PreviousXCoordinate + 40 )
                    && ( yCoordinate >= existing.YCoordinate - 40
                    && yCoordinate <= existing.PreviousYCoordinate + 40 ) )
                {
                    indexOfMatchingPointOfInterest = index;
                    break;
                }
            }

            return indexOfMatchingPointOfInterest;
        }

        public Rectangle[] GetRectangles()
        {
            RemoveInvalidPointOfInterests();

            Rectangle[] rectangles =
                new Rectangle[ this.List.Count ];

            for ( int i = 0; i <= ( this.List.Count - 1 ); ++i )
            {
                PointOfInterest pointOfInterest = this[ i ];
                rectangles[ i ] =
                    new Rectangle(
                        pointOfInterest.XCoordinate,
                        pointOfInterest.YCoordinate,
                        pointOfInterest.Width,
                        pointOfInterest.Height );
            }

            return rectangles;
        }

        public void ProcessCoordinates(
            int xCoordinate,
            int yCoordinate )
        {
            int indexOfExistingRectangle =
                FindExistingRectangle( xCoordinate, yCoordinate );
            if ( indexOfExistingRectangle == -1 )
            {
                Add( new PointOfInterest( xCoordinate, yCoordinate ) );
            }
            else
            {
                this[ indexOfExistingRectangle ].Adjust(
                    xCoordinate, yCoordinate );
            }
        }

        private void RemoveInvalidPointOfInterests()
        {
            for ( int index = 0; index <= ( this.List.Count - 1 ); ++index )
            {
                if ( this[ index ].Height == 0
                    || this[ index ].Width == 0 )
                {
                    this.RemoveAt( index );
                    --index;
                }
            }
        }

        #region PointOfInterest Inner-Class

        public class PointOfInterest
        {
            public PointOfInterest()
            {
            }

            public PointOfInterest(
                int xCoordinate,
                int yCoordinate )
            {
                PreviousXCoordinate = XCoordinate = xCoordinate;
                PreviousYCoordinate = YCoordinate = yCoordinate;
            }

            public void Adjust( int xCoordinate, int yCoordinate )
            {
                if ( xCoordinate > this.XCoordinate
                    && xCoordinate > this.PreviousXCoordinate )
                {
                    this.PreviousXCoordinate = xCoordinate;
                }
                else if ( xCoordinate < this.XCoordinate )
                {
                    this.XCoordinate = xCoordinate;
                }
                if ( yCoordinate > this.YCoordinate
                    && yCoordinate > this.PreviousYCoordinate )
                {
                    this.PreviousYCoordinate = yCoordinate;
                }
                else if ( yCoordinate < this.YCoordinate )
                {
                    this.YCoordinate = yCoordinate;
                }
            }

            #region Attributes

            public int Height
            {
                get { return PreviousYCoordinate - YCoordinate; }
            }

            public int PreviousXCoordinate
            {
                get { return previousXCoordinate; }
                set { previousXCoordinate = value; }
            }

            public int PreviousYCoordinate
            {
                get { return previousYCoordinate; }
                set { previousYCoordinate = value; }
            }

            public int Width
            {
                get { return PreviousXCoordinate - XCoordinate; }
            }

            public int XCoordinate
            {
                get { return xCoordinate; }
                set { xCoordinate = value; }
            }

            public int YCoordinate
            {
                get { return yCoordinate; }
                set { yCoordinate = value; }
            }

            #endregion

            private int previousXCoordinate = -1;
            private int previousYCoordinate = -1;
            private int xCoordinate = -1;
            private int yCoordinate = -1;
        }

        #endregion
    }
}