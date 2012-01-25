using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{

    #region Tree Node

    public class VertexVerticalTreeNode<T> where T : struct , IComparable<T>
    {
        // Childs
        public VertexVerticalTreeNode<T> Left;
        public VertexVerticalTreeNode<T> Right;

        // split planes
        public T X_mid;

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

        public VertexVerticalTreeNode()
        {
            // allocate list
            VertexList = new List<IVertex<T>>();

            // we start as leaf
            isLeaf = true;

            NumSubRegions = 1;

            NumVertex = 0;
        }

    }

    #endregion


    public class VertexVerticalTree<T> : IVertexTree<T> where T : struct , IComparable<T>
    {
         
        public VertexVerticalTreeNode<T> TreeRoot;

        /// <summary>
        /// Leaf capacity.
        /// </summary>
        private int MaxVertexPerLeaf = 100;


        private int NumRegions = 1;

        #region Constructor

        /// <summary>
        /// Init the vertex tree.
        /// Specify the leaf capacity.
        /// </summary>
        /// <param name="MaxVertexPerLeaf"></param>
        public VertexVerticalTree(int MaxVertexPerLeaf)
        {
            // set the vertex limit
            this.MaxVertexPerLeaf = MaxVertexPerLeaf;

            // init the root
            TreeRoot = new VertexVerticalTreeNode<T>();

        }

        #endregion


        #region IVertex Func

        public void Add(IVertex<T> vert)
        {
            int incLeafs;
            // start recursive adding
            VertexAdd(TreeRoot, vert, out incLeafs);
        }

        public void AddRange( IVertex<T>[] vertArray )
        {
            
        }

        public void Remove( IVertex<T> vert )
        {
            VertexRemove(TreeRoot, vert);
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

        private void VertexAdd(VertexVerticalTreeNode<T> Node, IVertex<T> vert, out int LeafInc)
        {
            // set to zero the leaf inc
            LeafInc = 0;

            // check if we are in leaf
            if (Node.isLeaf)
            {
                // check if we must split the node
                if (Node.VertexList.Count > MaxVertexPerLeaf)
                {

                    #region split the node to 2 sub-nodes

                    /// split the node to 2 sub-nodes

                    // init the sub-nodes
                    Node.Left = new VertexVerticalTreeNode<T>();
                    Node.Right = new VertexVerticalTreeNode<T>();

                    // sort base on x
                    Node.VertexList.Sort(VertexXComparer<T>.vc);

                    /// calc the split points
                    int x_split_index = (int)Math.Truncate(Node.VertexList.Count / 2.0f);

                    // get the x_mid
                    Node.X_mid = Node.VertexList[x_split_index].X;

                    /// split the vertex to the sub-nodes
                    foreach ( IVertex<T> ver in Node.VertexList ) {
                        // check if we are Right/Left
                        if ( ver.X.CompareTo( Node.X_mid ) > 0 ) {
                            Node.Right.VertexList.Add( ver );
                        } else {
                            Node.Left.VertexList.Add( ver );
                        }
                    }

                    // set that we are not leaf any more
                    Node.isLeaf = false;

                    // clean the list of vertex 
                    Node.VertexList.Clear();

                    #endregion

                    // dummy leaf inc
                    int dummy;

                    // call again the adding to add the new vertex
                    VertexAdd(Node, vert, out dummy);

                    // inc the leafs (2 news - 1 old)
                    NumRegions += 1;

                    // the num of new regions
                    LeafInc = 1;

                    // increase the num of sub regions
                    Node.NumSubRegions += LeafInc;
                }
                else
                {
                    // add the Vertex to the list
                    if (!Node.VertexList.Contains(vert, VertexXYComparer<T>.vc))
                    {
                        Node.VertexList.Add(vert);

                        // inc the num Vertex
                        Node.NumVertex++;
                    }
                }
            }
            else
            {
                #region Call the child for adding

                // check if we are Right/Left
                if (vert.X.CompareTo(Node.X_mid)>0)
                {
                    VertexAdd(Node.Right, vert, out LeafInc);
                }
                else
                {
                    VertexAdd(Node.Left, vert, out LeafInc);
                }

                #endregion

                // increase the node regions
                Node.NumSubRegions += LeafInc;

                // increase the node vertex num
                Node.NumVertex++;
            }

        }

        private void VertexRemove(VertexVerticalTreeNode<T> Node, IVertex<T> vert)
        {
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

        }

        private List<IVertex<T>> VertexGetRegion( VertexVerticalTreeNode<T> Node, int regionIndex, ref IVertex<T> Min, ref IVertex<T> Max )
        {
            if (Node.isLeaf)
            {
                // return the specific region
                return Node.VertexList;
            }
            else
            {
                #region Call the child for searching 

                // find if we move to the up Right
                if (regionIndex < Node.Right.NumSubRegions)
                {
                    Min.X = Node.X_mid;
                    return VertexGetRegion(Node.Right, regionIndex, ref Min, ref Max);
                }

                // find if we move to the down left
                regionIndex -=  Node.Right.NumSubRegions;

                if (regionIndex < Node.Left.NumSubRegions)
                {
                    Max.X = Node.X_mid;
                    return VertexGetRegion(Node.Left, regionIndex, ref Min, ref Max);
                }

                #endregion

                return null;
            }

        }


        #region Axes region 

        
        private void VertexGetRegionAxes(VertexVerticalTreeNode<T> Node, ref int regionIndex , List<VertexQuadTreeRegionAxes> AxesList)
        {
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
        }

        private void VertexGetRegionXAxes(VertexVerticalTreeNode<T> Node, ref int regionIndex, VertexQuadTreeRegionAxes CurrentAxes, Boolean isRight,Boolean isRoot)
        {
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

        }


        #endregion


        #endregion
    }
}
