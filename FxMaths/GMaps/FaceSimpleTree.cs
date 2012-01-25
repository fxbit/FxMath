using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FxMaths.GMaps
{
    public delegate void FaceFunction(Face face);

    /// <summary>
    /// Simple tree of faces.
    /// It's use link list to store the faces
    /// </summary>
    public class FaceSimpleTree : IFaceTree
    {

        // This can be actual tree but for now it can be list
        // TODO: make actual tree
        private List<Face> FaceList;


        public FaceSimpleTree()
        {
            // allocate face list
            FaceList = new List<Face>();
        }

        /// <summary>
        /// Add new face to the tree
        /// </summary>
        /// <param name="newFace"></param>
        public void Add( Face newFace )
        {
            FaceList.Add( newFace );
        }


        /// <summary>
        /// remove the face from the tree
        /// </summary>
        /// <param name="oldFace"></param>
        public void Remove( Face oldFace )
        {
            // remove the face
            FaceList.Remove( oldFace );
        }

        /// <summary>
        /// Find a face that collide with the given vertex
        /// </summary>
        /// <param name="vertex">The vertex for the test</param>
        /// <returns></returns>
        public Face FindFace( IVertex<float> vertex )
        {
            // if the size of faces exist the 1000 then use threads
            if ( FaceList.Count > 1000 ) {

                Face result;
                ConcurrentStack<Face> results = new ConcurrentStack<Face>();

                Parallel.ForEach<Face>( FaceList, ( fa, loopState ) => {
                    // check if we have hit
                    if ( fa.isHit( vertex ) ) {
                        loopState.Stop();
                        results.Push( fa );
                        return;
                    }
                } );

                // get the result from the stack
                if ( !results.TryPop( out result ) ) {
                    return null;
                } else {
                    return result;
                }

            } else {

                // pass all the faces 
                foreach ( Face fa in FaceList ) {
                    // check if we have hit
                    if ( fa.isHit( vertex ) ) {
                        return fa;
                    }
                }

                return null;
            }
        }



        public List<Face> FindFaceRange( IVertex<float> vertex )
        {
            List<Face> faceList = new List<Face>();
            Half_Edge tmpEdge;
            int count = 3;

            // pass all the faces 
            foreach (Face fa in FaceList)
            {
                tmpEdge = fa.HalfEnd;
                count = 0;

                while (count < 3)
                {

                    // check if we have hit
                    if (tmpEdge.StartVertex.X == vertex.X && tmpEdge.StartVertex.Y == vertex.Y)
                    {
                        faceList.Add(fa);
                        break;
                    }

                    // go to the next edge of the face
                    tmpEdge = tmpEdge.NextEdge;

                    
                    count++;
                }
            }

            return faceList;
        }
        

        /// <summary>
        /// update all the internal structs
        /// </summary>
        public void Update()
        {

            // pass all the faces 
            foreach ( Face fa in FaceList ) {
                // update all the internal face
                fa.Update();
            }
        }

        /// <summary>
        /// update all the internal structs base on the specific struct
        /// </summary>
        public void Update( Face face )
        {
            // update all the internal face
            face.Update();
        }

        /// <summary>
        /// Execute the input function in each face
        /// </summary>
        /// <param name="func"></param>
        public void FunctionExec( FaceFunction func )
        {
            // execute in each face the function
            foreach ( Face fe in FaceList )
                func( fe );
        }

        /// <summary>
        /// Get the statistics of the tree
        /// </summary>
        /// <param name="MaxDepth"></param>
        /// <param name="MinDepth"></param>
        /// <param name="NumNodes"></param>
        /// <param name="NumLeaf"></param>
        /// <param name="NumFaces"></param>
        /// <param name="NumFacesInNodes"></param>
        public void GetStat(out int MaxDepth, out int MinDepth, out int NumNodes, out int NumLeaf, out int NumFaces, out int NumFacesInNodes)
        {

            MaxDepth = 0; MinDepth = 0; NumNodes = 0; NumLeaf = 0; NumFaces = 0; NumFacesInNodes = 0;

            // we have flat tree here
            NumFaces = FaceList.Count;
        }

        /// <summary>
        /// Get the Boundary points in whole mesh of the faces.
        /// </summary>
        /// <param name="level">The level of points that we want to retrieve.</param>
        /// <param name="faceSide">The side that we want to retrieve the points.</param>
        /// <returns></returns>
        public List<IVertex<float>> GetSidePoints( int level, FaceSide faceSide )
        {
            List<IVertex<float>> result = new List<IVertex<float>>();


            return result;

        }
    }
}
