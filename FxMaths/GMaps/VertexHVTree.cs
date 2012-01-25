using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{

    #region Tree Node

    public class VertexHTreeNode<T> where T : struct , IComparable<T>
    {
        // Childs
        public VertexHTreeNode<T> Up;
        public VertexHTreeNode<T> Down;

        // root in vertical
        public VertexVTreeNode<T> Root;

        // split planes
        public T MidPoint;

        // set that we are in the bottom of the tree
        // if we are then we have face list
        public Boolean isLeaf;

        /// <summary>
        /// The num of sub-regions.
        /// </summary>
        public int NumSubRegions;

        /// <summary>
        /// The num of sub-regions.
        /// </summary>
        public int NumVertex;

        // list with all the vertex that exist under of this node
        public List<IVertex<T>> VertexList;

        public VertexHTreeNode()
        {
            // we start as leaf
            isLeaf = true;

            // init the vertex list
            VertexList = new List<IVertex<T>>();


            // init the vertical root
            Root = new VertexVTreeNode<T>();

            NumSubRegions = 1;

            NumVertex = 0;
        }

    }


    public class VertexVTreeNode<T> where T : struct , IComparable<T>
    {
        // Childs
        public VertexVTreeNode<T> Left;
        public VertexVTreeNode<T> Right;

        // split planes
        public T MidPoint;

        // set that we are in the bottom of the tree
        // if we are then we have face list
        public Boolean isLeaf;

        /// <summary>
        /// The num of sub-regions.
        /// </summary>
        public int NumSubRegions;

        /// <summary>
        /// The num of sub-regions.
        /// </summary>
        public int NumVertex;

        // faces
        public List<IVertex<T>> VertexList;

        public VertexVTreeNode()
        {
            // allocate list
            VertexList = new List<IVertex<T>>();

            // we start as leaf
            isLeaf = true;

            NumSubRegions = 1;

            NumVertex = 0;
        }


        internal void Dispose()
        {

            if (this.isLeaf)
            {
                // clean the list
                VertexList.Clear();
            }
            else
            {
                // dispoce the sub-nodes
                Left.Dispose();
                Right.Dispose();
            }
        }
    }

    #endregion


    public class VertexHVTree<T> : IVertexTree<T> where T : struct , IComparable<T>
    {

        public VertexHTreeNode<T> TreeRoot;

        /// <summary>
        /// Leaf capacity.
        /// </summary> 
        private int MaxVertexInHorizonLeaf = 300;

        private int MaxVertexInVerticalLeaf = 100;


        private int NumRegions = 1;

        #region Constructor

        /// <summary>
        /// Init the vertex tree.
        /// Specify the leaf capacity.
        /// </summary>
        /// <param name="MaxVertexPerLeaf"></param>
        public VertexHVTree(int MaxVertexInHorizonLeaf, int MaxVertexInVerticalLeaf)
        {
            // set the vertex limit
            this.MaxVertexInHorizonLeaf = MaxVertexInHorizonLeaf;
            this.MaxVertexInVerticalLeaf = MaxVertexInVerticalLeaf;

            // init the root
            TreeRoot = new VertexHTreeNode<T>();

        }

        #endregion


        #region IVertex Func

        public void Add(IVertex<T> vert)
        {
            int incLeafs;
            // start recursive adding
            VertexHAdd(TreeRoot, vert, out incLeafs);
        }

        public void AddRange( IVertex<T>[] vertArray )
        {
            
        }

        public void Remove( IVertex<T> vert )
        {
            //VertexRemove(TreeRoot, vert);
        }

        public int GetRegionNum()
        {
            return NumRegions;
        }

        public List<IVertex<T>> GetRegion( int index, ref IVertex<T> Min, ref IVertex<T> Max )
        {
            if (index < NumRegions)
            {
                return VertexGetRegion(TreeRoot, index, ref Min, ref Max);
            }

            return null;
        }


        public List<VertexQuadTreeRegionAxes> GetRegionAxes()
        {
            List<VertexQuadTreeRegionAxes> vertexQuadTreeRegionAxes = new List<VertexQuadTreeRegionAxes>();
            int regionIndex = 0;
            VertexGetRegionAxes(TreeRoot,ref regionIndex, vertexQuadTreeRegionAxes);

            return vertexQuadTreeRegionAxes;
        }

        public void Generate()
        {

        }
        
        #endregion


        #region Recursive functions

        private void VertexHAdd(VertexHTreeNode<T> Node, IVertex<T> vert, out int LeafInc)
        {
            // set to zero the leaf inc
            LeafInc = 0;

            // check if we are in leaf
            if (Node.isLeaf)
            {
                // add the Vertex to the list
                if (!Node.VertexList.Contains(vert, VertexXYComparer<T>.vc))
                {
                    Node.VertexList.Add(vert);

                    // inc the num Vertex
                    Node.NumVertex++;
                }

                // check if we must split the node
                if (Node.VertexList.Count > MaxVertexInHorizonLeaf)
                {

                    #region split the node to 2 sub-nodes

                    /// split the node to 2 sub-nodes
                    
                    // init the sub-nodes
                    Node.Up = new VertexHTreeNode<T>();
                    Node.Down = new VertexHTreeNode<T>();

                    // sort base on y
                    Node.VertexList.Sort(VertexYComparer<T>.vc);

                    /// calc the split points
                    int y_split_index = (int)Math.Truncate(Node.VertexList.Count / 2.0f);

                    // get the x_mid
                    Node.MidPoint = Node.VertexList[y_split_index].Y;

                    // set that we are not leaf any more
                    Node.isLeaf = false;

                    // remove the exist vertical trees
                    Node.Root.Dispose();

                    // remove the sup regions and add 2 for the up/down
                    int localLeafInc = -Node.Root.NumSubRegions + 2;
                    NumRegions += localLeafInc;

                    // add all the vertex to the new structure
                    foreach (IVertex<T> oldVert in Node.VertexList)
                    {
                        if (oldVert.Y.CompareTo(Node.MidPoint) > 0)
                        {
                            VertexHAdd(Node.Up, oldVert, out LeafInc);
                            localLeafInc += LeafInc;
                        }
                        else
                        {
                            VertexHAdd(Node.Down, oldVert, out LeafInc);
                            localLeafInc += LeafInc;
                        }

                    }

                    #endregion

                    // the num of new regions
                    LeafInc = localLeafInc;

                    // clean the vertex list
                    Node.VertexList.Clear();
                }
                else
                {
                    // add the vertex to the sub vertex
                    VertexVAdd(Node.Root, vert, out LeafInc);
                }

                // increase the num of sub regions
                Node.NumSubRegions += LeafInc;
            }
            else
            {
                #region Call the child for adding

                // check if we are Right/Left
                if (vert.Y.CompareTo(Node.MidPoint) > 0)
                {
                    VertexHAdd(Node.Up, vert, out LeafInc);
                }
                else
                {
                    VertexHAdd(Node.Down, vert, out LeafInc);
                }

                #endregion

                // increase the node regions
                Node.NumSubRegions += LeafInc;

                // increase the node vertex num
                Node.NumVertex++;
            }

        }

        private void VertexVAdd(VertexVTreeNode<T> Node, IVertex<T> vert, out int LeafInc)
        {
            // set to zero the leaf inc
            LeafInc = 0;

            // check if we are in leaf
            if (Node.isLeaf)
            {

                // add the Vertex to the list
                if (!Node.VertexList.Contains(vert, VertexXYComparer<T>.vc))
                {
                    Node.VertexList.Add(vert);

                    // inc the num Vertex
                    Node.NumVertex++;
                }

                // check if we must split the node
                if (Node.VertexList.Count > MaxVertexInVerticalLeaf)
                {

                    #region split the node to 2 sub-nodes

                    /// split the node to 2 sub-nodes

                    // init the sub-nodes
                    Node.Left = new VertexVTreeNode<T>();
                    Node.Right = new VertexVTreeNode<T>();

                    // sort base on x
                    Node.VertexList.Sort(VertexXComparer<T>.vc);

                    /// calc the split points
                    int x_split_index = (int)Math.Truncate(Node.VertexList.Count / 2.0f);

                    // get the x_mid
                    Node.MidPoint = Node.VertexList[x_split_index].X;

                    /// split the vertex to the sub-nodes
                    foreach (IVertex<T> ver in Node.VertexList)
                    {
                        // check if we are Right/Left
                        if (ver.X.CompareTo(Node.MidPoint) > 0)
                        {
                            Node.Right.VertexList.Add(ver);
                            Node.Right.NumVertex++;
                        }
                        else
                        {
                            Node.Left.VertexList.Add(ver);
                            Node.Left.NumVertex++;
                        }
                    }

                    // set that we are not leaf any more
                    Node.isLeaf = false;

                    // clean the list of vertex 
                    Node.VertexList.Clear();

                    #endregion

                    // inc the leafs (2 news - 1 old)
                    NumRegions += 1;

                    // the num of new regions
                    LeafInc = 1;

                    // increase the num of sub regions
                    Node.NumSubRegions += LeafInc;
                }
            }
            else
            {
                #region Call the child for adding

                // check if we are Right/Left
                if (vert.X.CompareTo(Node.MidPoint)>0)
                {
                    VertexVAdd(Node.Right, vert, out LeafInc);
                }
                else
                {
                    VertexVAdd(Node.Left, vert, out LeafInc);
                }

                #endregion

                // increase the node regions
                Node.NumSubRegions += LeafInc;

                // increase the node vertex num
                Node.NumVertex++;
            }

        }

        private void VertexRemove(VertexHTreeNode<T> Node, IVertex<T> vert)
        {
            /*
            // check if we are in leaf
            if (Node.isLeaf)
            {
                // add the Vertex to the list
                Node.VertexList.Remove(vert);
            }
            else
            {
                #region Call the child for adding

                // check if we are Right/Left
                if (vert.X.CompareTo(Node.X_mid)>0)
                {
                    VertexRemove(Node.Right, vert);
                }
                else
                {
                    VertexRemove(Node.Left, vert);
                }

                #endregion

            }
            */
        }

        private List<IVertex<T>> VertexGetRegion( VertexHTreeNode<T> Node, int regionIndex, ref IVertex<T> Min, ref IVertex<T> Max )
        {
            // return the specific region
            if (Node.isLeaf)
            {
                return VertexGetRegion(Node.Root, regionIndex, ref Min, ref Max);
            }
            else
            {
                #region Call the child for searching

                // find if we move to the up Right
                if (regionIndex < Node.Up.NumSubRegions)
                {
                    Min.Y = Node.MidPoint;
                    return VertexGetRegion(Node.Up, regionIndex, ref Min, ref Max);
                }

                // find if we move to the down left
                regionIndex -= Node.Up.NumSubRegions;

                if (regionIndex < Node.Down.NumSubRegions)
                {
                    Max.Y = Node.MidPoint;
                    return VertexGetRegion(Node.Down, regionIndex, ref Min, ref Max);
                }

                #endregion

                return null;
            }
            
        }

        private List<IVertex<T>> VertexGetRegion(VertexVTreeNode<T> Node, int regionIndex, ref IVertex<T> Min, ref IVertex<T> Max)
        {
            // return the specific region
            if (Node.isLeaf)
            {
                return Node.VertexList;
            }
            else
            {
                #region Call the child for searching

                // find if we move to the up Right
                if (regionIndex < Node.Right.NumSubRegions)
                {
                    Min.X = Node.MidPoint;
                    return VertexGetRegion(Node.Right, regionIndex, ref Min, ref Max);
                }

                // find if we move to the down left
                regionIndex -= Node.Right.NumSubRegions;

                if (regionIndex < Node.Left.NumSubRegions)
                {
                    Max.X = Node.MidPoint;
                    return VertexGetRegion(Node.Left, regionIndex, ref Min, ref Max);
                }

                #endregion

                return null;
            }

        }


        #region Axes region 

        
        private void VertexGetRegionAxes(VertexHTreeNode<T> Node, ref int regionIndex , List<VertexQuadTreeRegionAxes> AxesList)
        {
            /*
            if (!Node.isLeaf)
            {
                // create a new axes for the level that i am now
                VertexQuadTreeRegionAxes newVQTRA = new VertexQuadTreeRegionAxes();
                AxesList.Add(newVQTRA);
                int dummyRegionIndex = regionIndex;

                // fill the X axis
                VertexGetRegionXAxes(Node.Right, ref dummyRegionIndex, newVQTRA,true,true);

                VertexGetRegionXAxes(Node.Left, ref dummyRegionIndex, newVQTRA, false, true);

                // reset the region index
                dummyRegionIndex = regionIndex;

                // go to create all the others axis in the other paths
                dummyRegionIndex = regionIndex;
                VertexGetRegionAxes( Node.Right, ref dummyRegionIndex, AxesList );

                dummyRegionIndex = Node.Right.NumSubRegions + regionIndex;
                VertexGetRegionAxes( Node.Left, ref dummyRegionIndex, AxesList );
            }
             */
        }

        private void VertexGetRegionXAxes(VertexHTreeNode<T> Node, ref int regionIndex, VertexQuadTreeRegionAxes CurrentAxes, Boolean isRight,Boolean isRoot)
        {
            /*
            if (Node.isLeaf)
            {
                // xor the root and right to be able to have only one if :P
                if (isRight)
                {
                    // add the region to the specific axes
                    CurrentAxes.rightSide.Add(regionIndex);
                }
                else
                {
                    // add the region to the specific axes
                    CurrentAxes.leftSide.Add(regionIndex);
                }

                // inc the region index
                regionIndex++;
            }
            else
            {
                //  goto to lower level 
                // but filter the sides base on the left and right
                if(isRight)
                {
                    // inc region because we bye bass the region
                    regionIndex += Node.Right.NumSubRegions;

                    VertexGetRegionXAxes(Node.Left, ref regionIndex, CurrentAxes,isRight,false);

                }else{

                    VertexGetRegionXAxes( Node.Right, ref regionIndex, CurrentAxes, isRight, false );
                }
            }
             */

        }


        #endregion


        #endregion
    }
}
