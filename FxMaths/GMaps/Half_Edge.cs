using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{
    public class Half_Edge
    {
        /// <summary>
        /// The start position of the Node
        /// </summary>
        public IVertex<float> StartVertex;

        /// <summary>
        /// The next Edge in the line
        /// </summary>
        public Half_Edge NextEdge;

        /// <summary>
        /// The Oposite edge of the next triangle
        /// </summary>
        public Half_Edge TwinEdge;

        /// <summary>
        /// The face that this half edge belong
        /// </summary>
        public Face Face;

        #region Utils


        #region Swaping


        /// <summary>
        /// Swap the edge between two triangles.
        /// </summary>
        public void Swap()
        {
            // check if we are in boundary
            if ( TwinEdge != null ) {
                // get the information's for the triangles a and b
                Half_Edge a1 = this;
                Half_Edge a2 = a1.NextEdge;
                Half_Edge a3 = a2.NextEdge;
                Face a = a1.Face;

                Half_Edge b1 = this.TwinEdge;
                Half_Edge b2 = b1.NextEdge;
                Half_Edge b3 = b2.NextEdge;
                Face b = b1.Face;

                // change the next edge
                b3.NextEdge = a2;
                a3.NextEdge = b2;

                b2.NextEdge = a1;
                a2.NextEdge = b1;

                // swap the middle edge
                a1.StartVertex = b3.StartVertex;
                a1.NextEdge = a3;

                b1.StartVertex = a3.StartVertex;
                b1.NextEdge = b3;

                // update the faces
                a.Update( a1 );
                b.Update( b1 );
            } 
        }

        #endregion


        #region Create new triangles with Half_edge

        /// <summary>
        /// Create a new triangle with 3 new edge 
        /// </summary>
        /// <param name="vert1"></param>
        /// <param name="vert2"></param>
        /// <param name="vert3"></param>
        /// <param name="he1"></param>
        /// <param name="he2"></param>
        /// <param name="he3"></param>
        static public void CreateTriangle( IVertex<float> vert1, IVertex<float> vert2, IVertex<float> vert3,
            out Half_Edge he1, out Half_Edge he2, out Half_Edge he3 )
        {
            // create the new triangle
            he1 = new Half_Edge();
            he2 = new Half_Edge();
            he3 = new Half_Edge();

            // CCW test
            if ( ( ( vert2.X - vert1.X ) * ( vert3.Y - vert1.Y ) - ( vert3.X - vert1.X ) * ( vert2.Y - vert1.Y ) ) > 0 ) {
                he1.NextEdge = he2;
                he2.NextEdge = he3;
                he3.NextEdge = he1;
            } else {
                he1.NextEdge = he3;
                he3.NextEdge = he2;
                he2.NextEdge = he1;
            }

            // set the vertex
            he1.StartVertex = vert1;
            he2.StartVertex = vert2;
            he3.StartVertex = vert3;

            // set null the twin edge
            he1.TwinEdge = null;
            he2.TwinEdge = null;
            he3.TwinEdge = null;
        }

        /// <summary>
        /// Create a new triangle with 1 old edge and 2 new edge
        /// </summary>
        /// <param name="he1">The old edge</param>
        /// <param name="vert2"></param>
        /// <param name="vert3"></param>
        /// <param name="he2"></param>
        /// <param name="he3"></param>
        static public void CreateTriangle( Half_Edge he1, IVertex<float> vert2, IVertex<float> vert3,
            out Half_Edge he2, out Half_Edge he3 )
        {
            // create the new triangle
            he2 = new Half_Edge();
            he3 = new Half_Edge();

            // link the triangle
            he1.NextEdge = he2;
            he2.NextEdge = he3;
            he3.NextEdge = he1;

            // set the vertex
            he2.StartVertex = vert2;
            he3.StartVertex = vert3;
        }

        #endregion


        #region Circle testing

        /// <summary>
        /// Test if the given vertex (d) lie inside
        /// the  circle passing from the face (a,b,c).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        static public Boolean InCircle( IVertex<float> a,IVertex<float> b,IVertex<float> c,IVertex<float> d )
        {
            float adx, ady, bdx, bdy, cdx, cdy;
            float abdet, bcdet, cadet;
            float alift, blift, clift;

            // CCW test
            if ( ( ( b.X - a.X ) * ( c.Y - a.Y ) - ( c.X - a.X ) * ( b.Y - a.Y ) ) > 0 ) {
                //swap vertex
                IVertex<float> temp = b;
                b = c;
                c = temp;
            }

            // find the diff in each axes
            adx = a.X - d.X;
            ady = a.Y - d.Y;
            bdx = b.X - d.X;
            bdy = b.Y - d.Y;
            cdx = c.X - d.X;
            cdy = c.Y - d.Y;

            // calc the determinal
            abdet = adx * bdy - bdx * ady;
            bcdet = bdx * cdy - cdx * bdy;
            cadet = cdx * ady - adx * cdy;
            alift = adx * adx + ady * ady;
            blift = bdx * bdx + bdy * bdy;
            clift = cdx * cdx + cdy * cdy;

            return ( alift * bcdet + blift * cadet + clift * abdet < 0 );

        }

        /// <summary>
        /// Test if the given vertex lie inside
        /// the  circle passing from the face 
        /// that belong this half edge.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Boolean InCircle( IVertex<float> d )
        {
            float adx, ady, bdx, bdy, cdx, cdy;
            float abdet, bcdet, cadet;
            float alift, blift, clift;

            // get the 3 counterclockwise vertex
            IVertex<float> a = this.StartVertex;
            IVertex<float> b = this.NextEdge.StartVertex;
            IVertex<float> c = this.NextEdge.NextEdge.StartVertex;

            // CCW test
            if ( ( ( b.X - a.X ) * ( c.Y - a.Y ) - ( c.X - a.X ) * ( b.Y - a.Y ) ) > 0 )  {
                //swap vertex
                IVertex<float> temp = b;
                b = c;
                c = temp;
            }

            // find the diff in each axes
            adx = a.X - d.X;
            ady = a.Y - d.Y;
            bdx = b.X - d.X;
            bdy = b.Y - d.Y;
            cdx = c.X - d.X;
            cdy = c.Y - d.Y;

            // calc the determinal
            abdet = adx * bdy - bdx * ady;
            bcdet = bdx * cdy - cdx * bdy;
            cadet = cdx * ady - adx * cdy;
            alift = adx * adx + ady * ady;
            blift = bdx * bdx + bdy * bdy;
            clift = cdx * cdx + cdy * cdy;

            return ( alift * bcdet + blift * cadet + clift * abdet < 0 );

        }


        /// <summary>
        /// Recursive correction of triangles
        /// the common edge is the edge of the other triangle
        /// </summary>
        /// <param name="CommonEdge"></param>
        /// <param name="faceTree"></param>
        public void DelaunayCorrection( Half_Edge CommonEdge, IFaceTree faceTree )
        {
            if (CommonEdge != null && this.InCircle(CommonEdge.NextEdge.NextEdge.StartVertex))
            {
                // remove the faces from the tree before to change them
                faceTree.Remove(CommonEdge.Face);
                faceTree.Remove(CommonEdge.TwinEdge.Face);

                // swap the common edge
                CommonEdge.Swap();
                
                // add the face again with the new parameters
                faceTree.Add(CommonEdge.Face);
                faceTree.Add(CommonEdge.TwinEdge.Face);

                // run correction for the next 2 triangles
                if(CommonEdge.StartVertex == this.StartVertex){
                    CommonEdge.DelaunayCorrection(CommonEdge.NextEdge.TwinEdge, faceTree);
                    CommonEdge.TwinEdge.NextEdge.DelaunayCorrection(CommonEdge.TwinEdge.NextEdge.NextEdge.TwinEdge, faceTree);
                }else{
                    CommonEdge.NextEdge.DelaunayCorrection(CommonEdge.NextEdge.NextEdge.TwinEdge, faceTree);
                    CommonEdge.TwinEdge.DelaunayCorrection(CommonEdge.TwinEdge.NextEdge.TwinEdge, faceTree);
                }
            }

        }

        #endregion


        #region Line Utils

        /// <summary>
        /// Get the min dist of the given vert 
        /// and the edge.
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        public double Dist( IVertex<float> vert )
        {
            IVertex<float> ver2 = this.NextEdge.StartVertex;

            float dx = ver2.X - this.StartVertex.X;
            float dy = ver2.Y - this.StartVertex.Y;

            return Math.Abs(dx * (this.StartVertex.Y - vert.Y) - dy * (this.StartVertex.X - vert.X)) / Math.Sqrt(dx * dx + dy * dy);
        }


        #region Side Test


        /// <summary>
        /// Set if a point is in the right/left of a vector
        /// return true if the OB is right of OA
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        static public  Boolean SideTest(IVertex<float> orig, IVertex<float> A, IVertex<float> B)
        {

            //// check if the node acceptable
            IVertex<float> a = A.Copy();
            a.Subtract(orig);

            IVertex<float> b = B.Copy();
            b.Subtract(orig);

#if true
            a.Normalize();
            b.Normalize();
            double cross = a.X * b.Y - a.Y * b.X;

#else

            double cross = Math.Atan2(b.Y, b.X) - Math.Atan2(a.Y, a.X);
            cross += (cross + Math.PI <= 0.00001) ? 2 * Math.PI : 0;
            cross -= (cross - Math.PI >= -0.00001) ? 2 * Math.PI : 0;
            
#endif

            return cross < -0.0001;
        }

        #endregion


        #endregion


        #region Circle Utils

        public void CreateCircleFromFace( out IVertex<float> Center, out float radius )
        {
            // get the vertex of the face
            IVertex<float> ver1, ver2, ver3;
            ver1 = this.StartVertex;
            ver2 = this.NextEdge.StartVertex;
            ver3 = this.NextEdge.NextEdge.StartVertex;

            // copy the center base on one of the results
            Center = ver1.Copy();

            double f, g, m;
            double c, d, h, e, k, r, s;

            f = ver3.X * ver3.X - ver3.X * ver2.X - ver1.X * ver3.X + ver1.X * ver2.X + ver3.Y * ver3.Y - ver3.Y * ver2.Y - ver1.Y * ver3.Y + ver1.Y * ver2.Y; //formula
            g = ver3.X * ver1.Y - ver3.X * ver2.Y + ver1.X * ver2.Y - ver1.X * ver3.Y + ver2.X * ver3.Y - ver2.X * ver1.Y;

            if ( g == 0 )
                m = 0;
            else
                m = ( f / g );

            c = ( m * ver2.Y ) - ver2.X - ver1.X - ( m * ver1.Y ); //formula
            d = ( m * ver1.X ) - ver1.Y - ver2.Y - ( ver2.X * m );
            e = ( ver1.X * ver2.X ) + ( ver1.Y * ver2.Y ) - ( m * ver1.X * ver2.Y ) + ( m * ver2.X * ver1.Y );

            h = ( c / 2 ); //formula
            k = ( d / 2 );
            s = ( ( ( h ) * ( h ) ) + ( ( k ) * ( k ) ) - e );
            r = Math.Sqrt( s );

            Center.X = -(float)h;
            Center.Y = -(float)k;

            radius = (float)r;


            Console.Write( "(x" );

            if ( h >= 0 )
                Console.Write( " + " );
            else if ( h < 0 )
                Console.Write( " - " );
            if ( h < 0 )
                h = -h;
            Console.Write(  h .ToString() + ")^2" );
            Console.Write( " + " );
            Console.Write( "(y" );
            if ( k >= 0 )
                Console.Write( " + " );
            else if ( k < 0 )
                Console.Write( " - " );
            if ( k < 0 )
                k = -k;
            Console.Write( k.ToString() + ")^2 = "  + r.ToString() + "^2" );

            Console.Write( "" );

            Console.Write( "x^2 + y^2" );

            if ( c >= 0 ) Console.Write( " + " );
            else if ( c < 0 ) Console.Write( " - " );

            if ( c < 0 ) c = -c;
            Console.Write( c.ToString() + "x" );

            if ( d >= 0 ) Console.Write( " + " );
            else if ( d < 0 ) Console.Write( " - " );

            if ( d < 0 ) d = -d;
            Console.Write( d.ToString() + "y"  );

            if ( e >= 0 ) Console.Write( " + " );
            else if ( e < 0 ) Console.Write( " - " );

            if ( e < 0 ) e = -e;
            Console.Write(  e.ToString() +  " = 0" );
            Console.Write( "" );
        }

        #endregion

        #region link twin Edge


        /// <summary>
        /// Link the half edges
        /// </summary>
        /// <param name="half_Edge"></param>
        public void LinkEdge(Half_Edge half_Edge)
        {
            // link the twin edges :P
            this.TwinEdge = half_Edge;
            half_Edge.TwinEdge = this;
        }
        

        #endregion
        
        
        #endregion


    }
}
