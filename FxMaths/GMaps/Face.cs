using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{
    public class Face
    {
        public Half_Edge HalfEnd;

        private float X_Max,X_Min,Y_Max,Y_Min;

        #region properties 

        public float Max_X { get { return X_Max; } }
        public float Max_Y { get { return Y_Max; } }
        public float Min_X { get { return X_Min; } }
        public float Min_Y { get { return Y_Min; } }

        #endregion



        #region Contractor

        public Face( Half_Edge HalfEnd )
        {
            // set the internal pointers
            this.HalfEnd = HalfEnd;

            // link this face to the edge
            HalfEnd.Face = this;
            HalfEnd.NextEdge.Face = this;
            HalfEnd.NextEdge.NextEdge.Face = this;

            // update the internal state 
            this.Update();
        }

        #endregion



        #region Hit Test

        /// <summary>
        /// Test if the vertex is close to this vertex
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public Boolean isHit( IVertex<float> vertex )
        {
            // pretest with the x_max , x_min
            if ( vertex.X >= X_Min && vertex.X <= X_Max &&
                vertex.Y >= Y_Min && vertex.Y <= Y_Max ) {

                // get the vertex of the face
                IVertex<float> ver1, ver2, ver3;
                ver1 = HalfEnd.StartVertex;
                ver2 = HalfEnd.NextEdge.StartVertex;
                ver3 = HalfEnd.NextEdge.NextEdge.StartVertex;

                // Compute vectors   
                double vec0_x = ver2.X - ver1.X;
                double vec0_y = ver2.Y - ver1.Y;
                double vec1_x = ver3.X - ver1.X;
                double vec1_y = ver3.Y - ver1.Y;
                double vec2_x = vertex.X - ver1.X;
                double vec2_y = vertex.Y - ver1.Y;

                // Compute dot products
                double dot00 = vec0_x * vec0_x + vec0_y * vec0_y;
                double dot01 = vec0_x * vec1_x + vec0_y * vec1_y;
                double dot02 = vec0_x * vec2_x + vec0_y * vec2_y;
                double dot11 = vec1_x * vec1_x + vec1_y * vec1_y;
                double dot12 = vec1_x * vec2_x + vec1_y * vec2_y;

                // Compute barycentric coordinates
                double inv = 1 / ( dot00 * dot11 - dot01 * dot01 );
                double z = ( dot11 * dot02 - dot01 * dot12 ) * inv;
                if ( z < 0 )
                    return false;

                double y = ( dot00 * dot12 - dot01 * dot02 ) * inv;
                if ( y < 0 )
                    return false;

                if ( z >= 0 && y >= 0 && y + z <= 1 )
                    return true;

            }

            return false;
        }

        #endregion



        #region update

        /// <summary>
        /// Update the internal half edge and the min/max
        /// </summary>
        /// <param name="newHE"></param>
        public void Update(Half_Edge newHE)
        {
            // set the internal half edge
            HalfEnd = newHE;
            HalfEnd.Face = this;
            HalfEnd.NextEdge.Face = this;
            HalfEnd.NextEdge.NextEdge.Face = this;

            // update the min/max
            this.Update();
        }

        /// <summary>
        /// update the internal state base on the triangle
        /// </summary>
        public void Update()
        {
            // get the vertex of the face
            IVertex<float> ver1,ver2,ver3;
            ver1 = HalfEnd.StartVertex;
            ver2 = HalfEnd.NextEdge.StartVertex;
            ver3 = HalfEnd.NextEdge.NextEdge.StartVertex;

            // set the min X 
            if ( ver1.X < ver2.X ) {
                X_Min = ( ver1.X < ver3.X ) ? ver1.X : ver3.X;
                X_Max = ( ver2.X > ver3.X ) ? ver2.X : ver3.X;
            } else {
                X_Min = ( ver2.X < ver3.X ) ? ver2.X : ver3.X;
                X_Max = ( ver1.X > ver3.X ) ? ver1.X : ver3.X;
            }


            // set the max min Y
            if ( ver1.Y < ver2.Y ) {
                Y_Min = ( ver1.Y < ver3.Y ) ? ver1.Y : ver3.Y;
                Y_Max = ( ver2.Y > ver3.Y ) ? ver2.Y : ver3.Y;
            } else {
                Y_Min = ( ver2.Y < ver3.Y ) ? ver2.Y : ver3.Y;
                Y_Max = ( ver1.Y > ver3.Y ) ? ver1.Y : ver3.Y;
            }
        }

        #endregion



        #region Dist

        /// <summary>
        /// find the min Dist of the vert from all the edges.
        /// Return the edge that have the min dist
        /// </summary>
        /// <param name="vert"></param>
        /// <param name="minEdge"></param>
        /// <returns></returns>
        public double minDistEdge(IVertex<float> vert, out Half_Edge minEdge)
        {
            double dist1, dist2, dist3;

            // get the 3 dist from the edge
            dist1 = HalfEnd.Dist(vert);
            dist2 = HalfEnd.NextEdge.Dist(vert);
            dist3 = HalfEnd.NextEdge.NextEdge.Dist(vert);

            // find the minimum
            if (dist1 < dist2)
                if (dist1 < dist3)
                {
                    minEdge = this.HalfEnd;
                    return dist1;
                }
                else
                {
                    minEdge = this.HalfEnd.NextEdge.NextEdge;
                    return dist3;
                }
            else if (dist2 < dist3)
            {
                minEdge = this.HalfEnd.NextEdge;
                return dist2;
            }
            else
            {
                minEdge = this.HalfEnd.NextEdge.NextEdge;
                return dist3;
            }
        }

        /// <summary>
        /// find the min Dist of the vert from all the edges.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        public double minDistEdge( IVertex<float> vert )
        {
            double dist1,dist2,dist3;

            // get the 3 dist from the edge
            dist1 = HalfEnd.Dist(vert);
            dist2 = HalfEnd.NextEdge.Dist(vert);
            dist3 = HalfEnd.NextEdge.NextEdge.Dist(vert);

            // find the minimum
            return (dist1 < dist2) ? ((dist1 < dist3) ? dist1 : dist3) : ((dist2 < dist3) ? dist2 : dist3);
        }

        #endregion

    }
}
