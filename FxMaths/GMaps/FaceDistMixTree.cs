using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FxMaths.GMaps
{

    /// <summary>
    /// Tree for face storing and searching
    /// Store faces in nodes(if the Face is very big) and leafs.
    /// </summary>
    public class FaceDistMixTree : IFaceTree
    {

        /// <summary>
        /// tree of the faces
        /// </summary>
        public FaceDistTreeNode FaceRoot;

        // the max num of faces before the split
        private int MaxFaces;

        /// <summary>
        /// Store the boundary faces
        /// </summary>
        public List<Face> BoundaryFaces;


        #region Constructor

        /// <summary>
        /// Create a new face tree with specific limit of faces per leaf
        /// </summary>
        /// <param name="maxFacesPerLeaf"></param>
        public FaceDistMixTree(int maxFacesPerLeaf = 100)
        {
            // set the max faces
            MaxFaces = maxFacesPerLeaf;

            // allocate face list
            FaceRoot = new FaceDistTreeNode();

            // allocate boundary list
            BoundaryFaces = new List<Face>();
        }

        #endregion


        #region IFace Func

        /// <summary>
        /// Add new face to the tree
        /// </summary>
        /// <param name="newFace"></param>
        public void Add( Face newFace )
        {
            // add the face 
            FaceTreeAdd(FaceRoot, newFace);

            // check if the face is boundary face 
            if ( newFace.HalfEnd.TwinEdge == null ||
                 newFace.HalfEnd.NextEdge.TwinEdge == null ||
                 newFace.HalfEnd.NextEdge.NextEdge.TwinEdge == null ) {

                // add the face to the boundary
                BoundaryFaces.Add( newFace );

            }
        }


        /// <summary>
        /// remove the face from the tree
        /// </summary>
        /// <param name="oldFace"></param>
        public void Remove( Face oldFace )
        {
            // remove the face
            FaceTreeRemove(FaceRoot, oldFace);

            // check if the face is boundary face 
            if ( oldFace.HalfEnd.TwinEdge == null ||
                 oldFace.HalfEnd.NextEdge.TwinEdge == null ||
                 oldFace.HalfEnd.NextEdge.NextEdge.TwinEdge == null ) {

                // add the face to the boundary
                BoundaryFaces.Remove( oldFace );

            }
        }

        /// <summary>
        /// Find a face that collide with the given vertex
        /// </summary>
        /// <param name="vertex">The vertex for the test</param>
        /// <returns></returns>
        public Face FindFace( IVertex<float> vertex )
        {
            // find in the tree
            return FaceTreeFind(FaceRoot, vertex);
        }

        
        /// <summary>
        /// update all the internal structs
        /// </summary>
        public void Update()
        {
            // pass all the faces 
            FaceTreeExec(FaceRoot, new FaceFunction(Update));
        }

        /// <summary>
        /// update all the internal structs base on the specific struct
        /// </summary>
        public void Update( Face face )
        {
            // update all the internal face
            face.Update();

            /// update the position in the tree

            // find the face in the tree

        }

        /// <summary>
        /// Execute the input function in each face
        /// </summary>
        /// <param name="func"></param>
        public void FunctionExec( FaceFunction func )
        {
            // execute in each face the function
            FaceTreeExec(FaceRoot, func);
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

            // recursive call pass all the tree
            FaceTreeStat(FaceRoot, ref MaxDepth, ref MinDepth, ref NumNodes, ref NumLeaf, ref NumFaces, ref NumFacesInNodes);
        }

        /// <summary>
        /// Find all the faces that have the specific vertex.
        /// The vertex must be exactly the same ( pointer compare );
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public List<Face> FindFaceRange( IVertex<float> vertex )
        {
            List<Face> result = new List<Face>();

            // find recursive all the faces that have the specific face.
            FaceTreeFindRange(FaceRoot, vertex, result);

            return result;

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

            switch ( faceSide ) {
                case FaceSide.Right:


                    break;

                case FaceSide.Left:


                    break;
            }

            return result;

        }

        #endregion


        #region Face node utils

        private void FaceNodeSpliting(FaceDistTreeNode faceNode)
        {
            // check if the face is leaf just in case
            if (faceNode.isLeaf)
            {
                // find the median

                
#if false 
                // sort base on x
                faceNode.FaceList.Sort(delegate(Face fa1, Face fa2)
                {
                    return fa1.Min_X.CompareTo(fa2.Min_X);
                });

                // get the x median value 
                faceNode.X_mid = faceNode.FaceList[(int)Math.Truncate(faceNode.FaceList.Count / 2.0f)].Max_X;

                // sort base on y
                faceNode.FaceList.Sort(delegate(Face fa1, Face fa2)
                {
                    return fa1.Min_Y.CompareTo(fa2.Min_Y);
                });

                // get the y median value 
                faceNode.Y_mid = faceNode.FaceList[(int)Math.Truncate(faceNode.FaceList.Count / 2.0f)].Max_Y;

#else
                float max_X= float.MinValue,min_X= float.MaxValue;
                float max_Y= float.MinValue,min_Y= float.MaxValue;

                foreach (Face fa in faceNode.FaceList)
                {
                    // get the max x
                    if (max_X < fa.Max_X)
                        max_X = fa.Max_X;

                    // get the min x
                    if (min_X > fa.Min_X)
                        min_X = fa.Min_X;

                    // get the max Y
                    if (max_Y < fa.Max_Y)
                        max_Y = fa.Max_Y;

                    // get the min Y
                    if (min_Y > fa.Min_Y)
                        min_Y = fa.Min_Y;
                }

                // get the mid value
                faceNode.X_mid = (min_X + max_X ) / 2;
                faceNode.Y_mid = (min_Y + max_Y) / 2;
#endif

                // create the 4 leaf
                faceNode.UpLeft = new FaceDistTreeNode();
                faceNode.DownLeft = new FaceDistTreeNode();
                faceNode.UpRight = new FaceDistTreeNode();
                faceNode.DownRight = new FaceDistTreeNode();

                // disable the leaf mode of the node
                faceNode.isLeaf = false;

                // the faces that is going to remain to this node
                ConcurrentStack<Face> remain = new ConcurrentStack<Face>();

                // fill the nodes
                foreach (Face fa in faceNode.FaceList)
                {
                    if (fa.Min_X > faceNode.X_mid)
                    {
                        if (fa.Min_Y > faceNode.Y_mid)
                        {
                            faceNode.UpRight.FaceList.Add(fa);
                        }
                        else if (fa.Max_Y < faceNode.Y_mid)
                        {
                            faceNode.DownRight.FaceList.Add(fa);
                        }
                        else
                        {
                            // add in both sides :P
                            remain.Push(fa);
                        }
                    }
                    else if (fa.Max_X < faceNode.X_mid)
                    {
                        if (fa.Min_Y > faceNode.Y_mid)
                        {
                            faceNode.UpLeft.FaceList.Add(fa);
                        }
                        else if (fa.Max_Y < faceNode.Y_mid)
                        {
                            faceNode.DownLeft.FaceList.Add(fa);
                        }
                        else
                        {
                            // add in both sides :P
                            remain.Push(fa);
                        }

                    }
                    else
                    {
                        remain.Push(fa);
                    }
                }

                // clean the list 
                faceNode.FaceList.Clear();

                // add the remain faces to the node list
                faceNode.FaceList.AddRange(remain);

                // clean the stack
                remain.Clear();

            }
        }

        private void FaceTreeAdd(FaceDistTreeNode faceNode, Face newFace)
        {
            // check if we are in leaf
            if (faceNode.isLeaf)
            {
                // check if we are pass the max num of faces per leaf
                if (faceNode.FaceList.Count < MaxFaces)
                {
                    // just add the face to the list
                    faceNode.FaceList.Add(newFace);
                }
                else
                {
                    // split the leaf to 4 nodes
                    FaceNodeSpliting(faceNode);

                    // re-call this function as node
                    FaceTreeAdd(faceNode, newFace);
                }
            }
            else
            {
                Boolean isAdded = false;

                // continue to the correct side
                if (newFace.Min_X > faceNode.X_mid)
                {
                    if (newFace.Min_Y > faceNode.Y_mid)
                    {
                        FaceTreeAdd(faceNode.UpRight, newFace);
                        isAdded = true;
                    }
                    else if (newFace.Max_Y < faceNode.Y_mid)
                    {
                        FaceTreeAdd(faceNode.DownRight, newFace);
                        isAdded = true;
                    }
                }
                else if (newFace.Max_X < faceNode.X_mid)
                {
                    if (newFace.Min_Y > faceNode.Y_mid)
                    {
                        FaceTreeAdd(faceNode.UpLeft, newFace);
                        isAdded = true;
                    }
                    else if (newFace.Max_Y < faceNode.Y_mid)
                    {
                        FaceTreeAdd(faceNode.DownLeft, newFace);
                        isAdded = true;
                    }
                }

                if (!isAdded)
                {
                    // just add the face to the node list
                    faceNode.FaceList.Add(newFace);
                }

            }
        }

        private void FaceTreeRemove(FaceDistTreeNode faceNode, Face newFace)
        {
            // check if we are in leaf
            if (faceNode.isLeaf)
            {

                // just remove the face from the list
                faceNode.FaceList.Remove(newFace);

            }
            else
            {
                Boolean isRemoved = false;

                // continue to the correct side
                if (newFace.Min_X > faceNode.X_mid)
                {
                    if (newFace.Min_Y > faceNode.Y_mid)
                    {
                        FaceTreeRemove(faceNode.UpRight, newFace);
                        isRemoved = true;
                    }
                    else if (newFace.Max_Y < faceNode.Y_mid)
                    {
                        FaceTreeRemove(faceNode.DownRight, newFace);
                        isRemoved = true;
                    }
                }
                else if (newFace.Max_X < faceNode.X_mid)
                {
                    if (newFace.Min_Y > faceNode.Y_mid)
                    {
                        FaceTreeRemove(faceNode.UpLeft, newFace);
                        isRemoved = true;
                    }
                    else if (newFace.Max_Y < faceNode.Y_mid)
                    {
                        FaceTreeRemove(faceNode.DownLeft, newFace);
                        isRemoved = true;
                    }
                }

                if (!isRemoved)
                {
                    // just remove the face from the list
                    faceNode.FaceList.Remove(newFace);
                }
            }
        }

        private Face FaceTreeFind( FaceDistTreeNode faceNode, IVertex<float> vertex )
        {
            // check if we are in leaf
            if (faceNode.isLeaf)
            {
                // pass all the faces 
                foreach (Face fa in faceNode.FaceList)
                {
                    // check if we have hit
                    if (fa.isHit(vertex))
                    {
                        return fa;
                    }
                }
            }
            else
            {
                Face result;

                // continue to the correct side
                if (vertex.X > faceNode.X_mid)
                {
                    if (vertex.Y > faceNode.Y_mid)
                    {
                        result = FaceTreeFind(faceNode.UpRight, vertex);
                    }
                    else
                    {
                        result = FaceTreeFind(faceNode.DownRight, vertex);
                    }
                }
                else
                {
                    if (vertex.Y > faceNode.Y_mid)
                    {
                        result = FaceTreeFind(faceNode.UpLeft, vertex);
                    }
                    else
                    {
                        result = FaceTreeFind(faceNode.DownLeft, vertex);
                    }
                }

                if (result != null)
                {
                    return result;
                }
                else
                {
                    // pass all the faces of the node
                    foreach (Face fa in faceNode.FaceList)
                    {
                        // check if we have hit
                        if (fa.isHit(vertex))
                        {
                            return fa;
                        }
                    }
                }
            }

            return null;
        }

        private void FaceTreeExec(FaceDistTreeNode faceNode, FaceFunction func)
        {
            // check if we are in leaf
            if (faceNode.isLeaf)
            {
                // pass all the faces 
                foreach (Face fa in faceNode.FaceList)
                {
                    func(fa);
                }
            }
            else
            {
                // pass all the faces 
                foreach (Face fa in faceNode.FaceList)
                {
                    func(fa);
                }

                FaceTreeExec(faceNode.DownLeft, func);
                FaceTreeExec(faceNode.UpLeft, func);
                FaceTreeExec(faceNode.DownRight, func);
                FaceTreeExec(faceNode.UpRight, func);
            }
        }

        private void FaceTreeStat(FaceDistTreeNode faceNode, ref int MaxDepth, ref int MinDepth, ref int NumNodes, ref int NumLeaf, ref int NumFaces, ref int NumFacesInNodes)
        {
            if (faceNode.isLeaf)
            {
                // inc the leafs
                NumLeaf++;

                // inc the NumFaces
                NumFaces += faceNode.FaceList.Count;

                MaxDepth++;
                MinDepth++;
            }
            else
            {
                int MaxDepth1 = 0, MaxDepth2 = 0, MaxDepth3 = 0, MaxDepth4 = 0;
                int MinDepth1 = 0, MinDepth2 = 0, MinDepth3 = 0, MinDepth4 = 0;

                // pass the Childs
                FaceTreeStat(faceNode.DownLeft, ref MaxDepth1, ref MinDepth1, ref NumNodes, ref NumLeaf, ref NumFaces, ref NumFacesInNodes);
                FaceTreeStat(faceNode.UpLeft, ref MaxDepth2, ref MinDepth2, ref NumNodes, ref NumLeaf, ref NumFaces, ref NumFacesInNodes);
                FaceTreeStat(faceNode.UpRight, ref MaxDepth3, ref MinDepth3, ref NumNodes, ref NumLeaf, ref NumFaces, ref NumFacesInNodes);
                FaceTreeStat(faceNode.DownRight, ref MaxDepth4, ref MinDepth4, ref NumNodes, ref NumLeaf, ref NumFaces, ref NumFacesInNodes);

                // add the min/max depth
                MaxDepth += Math.Max(Math.Max(MaxDepth1, MaxDepth2), Math.Max(MaxDepth3, MaxDepth4));
                MinDepth += Math.Min(Math.Min(MinDepth1, MinDepth2), Math.Min(MinDepth3, MinDepth4));

                MaxDepth++;
                MinDepth++;

                // inc the num Nodes
                NumNodes++;

                // inc the NumFaces
                NumFaces += faceNode.FaceList.Count;

                // inc the num faces only in the nodes
                NumFacesInNodes += faceNode.FaceList.Count;
            }

        }

        private void FaceTreeFindRange( FaceDistTreeNode faceNode, IVertex<float> vertex, List<Face> results )
        {
            // check if we are in leaf
            if (faceNode.isLeaf)
            {
                Half_Edge tmpEdge;
                int count = 3;

                // pass all the faces 
                foreach (Face fa in faceNode.FaceList)
                {
                    tmpEdge = fa.HalfEnd;
                    count = 0;

                    while (count < 3)
                    {

                        // check if we have hit
                        if (tmpEdge.StartVertex.X == vertex.X && tmpEdge.StartVertex.Y == vertex.Y)
                        {
                            results.Add(fa);
                            break;
                        }

                        // go to the next edge of the face
                        tmpEdge = tmpEdge.NextEdge;


                        count++;
                    }
                }

                return;
            }
            else
            {
                Face result;

                // continue to the correct side
                if (vertex.X == faceNode.X_mid)
                {
                    FaceTreeFindRange(faceNode.UpRight, vertex, results);
                    FaceTreeFindRange(faceNode.DownRight, vertex, results);
                    FaceTreeFindRange(faceNode.UpLeft, vertex, results);
                    FaceTreeFindRange(faceNode.DownLeft, vertex, results);
                }
                else if (vertex.X > faceNode.X_mid)
                {

                    if (vertex.Y == faceNode.Y_mid)
                    {
                        FaceTreeFindRange(faceNode.UpRight, vertex, results);
                        FaceTreeFindRange(faceNode.DownRight, vertex, results);
                    }
                    else if (vertex.Y > faceNode.Y_mid)
                    {
                        FaceTreeFindRange(faceNode.UpRight, vertex, results);
                    }
                    else
                    {
                        FaceTreeFindRange(faceNode.DownRight, vertex, results);
                    }
                }
                else
                {
                    if(vertex.Y == faceNode.Y_mid){
                        FaceTreeFindRange(faceNode.UpLeft, vertex, results);
                        FaceTreeFindRange(faceNode.DownLeft, vertex, results);
                    }
                    else if (vertex.Y > faceNode.Y_mid)
                    {
                        FaceTreeFindRange(faceNode.UpLeft, vertex, results);
                    }
                    else
                    {
                        FaceTreeFindRange(faceNode.DownLeft, vertex, results);
                    }
                }

                Half_Edge tmpEdge;
                int count = 3;

                // pass all the faces 
                foreach (Face fa in faceNode.FaceList)
                {
                    tmpEdge = fa.HalfEnd;
                    count = 0;

                    while (count < 3)
                    {

                        // check if we have hit
                        if (tmpEdge.StartVertex.X == vertex.X && tmpEdge.StartVertex.Y == vertex.Y)
                        {
                            results.Add(fa);
                            break;
                        }

                        // go to the next edge of the face
                        tmpEdge = tmpEdge.NextEdge;


                        count++;
                    }
                }

                return;
                
            }
        }

        #endregion
    }
}
