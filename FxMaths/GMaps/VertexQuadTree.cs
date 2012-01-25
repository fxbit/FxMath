using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{

    public class VertexQuadTreeRegionAxes
    {
        /// <summary>
        /// List with the index of the left side
        /// </summary>
        public List<int> leftSide;

        /// <summary>
        /// List with the index of the Right Side
        /// </summary>
        public List<int> rightSide;

        /// <summary>
        /// List with the index of the left side
        /// </summary>
        public List<int> upSide;

        /// <summary>
        /// List with the index of the Right Side
        /// </summary>
        public List<int> downSide;

        public VertexQuadTreeRegionAxes()
        {
            leftSide = new List<int>();
            rightSide = new List<int>();
            upSide = new List<int>();
            downSide = new List<int>();
        }
    }

    public class VertexQuadTreeNode<T> where T : struct , IComparable<T>
    {
        // Childs
        public VertexQuadTreeNode<T> UpLeft;
        public VertexQuadTreeNode<T> UpRight;
        public VertexQuadTreeNode<T> DownLeft;
        public VertexQuadTreeNode<T> DownRight;

        // split planes
        public T X_mid;
        public T Y_mid;

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

        public VertexQuadTreeNode()
        {
            // allocate list
            VertexList = new List<IVertex<T>>();

            // we start as leaf
            isLeaf = true;

            NumSubRegions = 1;

            NumVertex = 0;
        }

    }

    public class VertexQuadTree<T> : IVertexTree<T> where T : struct , IComparable<T>
    {

        public VertexQuadTreeNode<T> TreeRoot;

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
        public VertexQuadTree(int MaxVertexPerLeaf)
        {
            // set the vertex limit
            this.MaxVertexPerLeaf = MaxVertexPerLeaf;

            // init the root
            TreeRoot = new VertexQuadTreeNode<T>();

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

        private void VertexAdd( VertexQuadTreeNode<T> Node, IVertex<T> vert, out int LeafInc )
        {
            // set to zero the leaf inc
            LeafInc = 0;

            // check if we are in leaf
            if (Node.isLeaf)
            {
                // check if we must split the node
                if (Node.VertexList.Count > MaxVertexPerLeaf)
                {

                    #region split the node to 4 sub-nodes

                    /// split the node to 4 sub-nodes

                    // init the sub-nodes
                    Node.UpLeft = new VertexQuadTreeNode<T>();
                    Node.DownLeft = new VertexQuadTreeNode<T>();
                    Node.UpRight = new VertexQuadTreeNode<T>();
                    Node.DownRight = new VertexQuadTreeNode<T>();

                    /// calc the split points

                    // sort base on x
                    Node.VertexList.Sort( VertexXComparer<T>.vc );

                    /// calc the split points
                    int split_index = (int)Math.Truncate( Node.VertexList.Count / 2.0f );

                    // get the mid point
                    Node.X_mid = Node.VertexList[split_index].X;

                    // sort base on y
                    Node.VertexList.Sort(VertexYComparer<T>.vc );

                    // get the mid point
                    Node.Y_mid = Node.VertexList[split_index].Y;

#if false
                    /*
                                                          // init the max/min
                                       dynamic  max_X = MiscUtils.GetMinValue<T>(), min_X = MiscUtils.GetMaxValue<T>();
                                       dynamic  max_Y = MiscUtils.GetMinValue<T>(), min_Y = MiscUtils.GetMaxValue<T>();
                     * 
                                       // pass all the vertex of the node and find the min/max
                                       foreach ( IVertex<T> ver in Node.VertexList ) {
                                           // get the max x
                                           if ( max_X.CompareTo( ver.X ) < 0 )
                                               max_X = ver.X;

                                           // get the min x
                                           if ( min_X.CompareTo( ver.X ) > 0 )
                                               min_X = ver.X;

                                           // get the max Y
                                           if ( max_Y.CompareTo( ver.Y ) < 0 )
                                               max_Y = ver.Y;

                                           // get the min Y
                                           if ( min_Y.CompareTo( ver.Y ) > 0 )
                                               min_Y = ver.Y;
                                       }

                                       // get the mid value of min/max
                                       Node.X_mid = (min_X + max_X) / 2;
                                       Node.Y_mid = (min_Y + max_Y) / 2;
                   
                   */
#endif

                    /// split the vertex to the sub-nodes
                    foreach ( IVertex<T> ver in Node.VertexList ) {
                        // check if we are Right/Left
                        if ( ver.X.CompareTo( Node.X_mid ) > 0 ) {
                            // check if we are Up/Down
                            if ( ver.Y.CompareTo( Node.Y_mid ) > 0 ) {
                                Node.UpRight.VertexList.Add( ver );
                                Node.UpRight.NumVertex++;
                            } else {
                                Node.DownRight.VertexList.Add( ver );
                                Node.DownRight.NumVertex++;
                            }

                        } else {

                            // check if we are Up/Down
                            if ( ver.Y.CompareTo( Node.Y_mid ) > 0 ) {
                                Node.UpLeft.VertexList.Add( ver );
                                Node.UpLeft.NumVertex++;
                            } else {
                                Node.DownLeft.VertexList.Add( ver );
                                Node.DownLeft.NumVertex++;
                            }

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

                    // inc the leafs (4 news - 1 old)
                    NumRegions += 3;

                    // the num of new regions
                    LeafInc = 3;

                    // increase the num of sub regions
                    Node.NumSubRegions += LeafInc;
                }
                else
                {
                    // add the Vertex to the list
                    if (!Node.VertexList.Contains(vert))
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
                    // check if we are Up/Down
                    if (vert.Y.CompareTo(Node.Y_mid)>0)
                    {
                        VertexAdd(Node.UpRight, vert, out LeafInc);
                    }
                    else
                    {
                        VertexAdd(Node.DownRight, vert, out LeafInc);
                    }

                }
                else
                {
                    // check if we are Up/Down
                    if (vert.Y.CompareTo(Node.Y_mid)>0)
                    {
                        VertexAdd(Node.UpLeft, vert, out LeafInc);
                    }
                    else
                    {
                        VertexAdd(Node.DownLeft, vert, out LeafInc);
                    }
                }

                #endregion

                // increase the node regions
                Node.NumSubRegions += LeafInc;

                // increase the node vertex num
                Node.NumVertex++;
            }

        }

        private void VertexRemove( VertexQuadTreeNode<T> Node, IVertex<T> vert )
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
                    // check if we are Up/Down
                    if (vert.Y.CompareTo(Node.Y_mid)>0)
                    {
                        VertexRemove(Node.UpRight, vert);
                    }
                    else
                    {
                        VertexRemove(Node.DownRight, vert);
                    }

                }
                else
                {
                    // check if we are Up/Down
                    if (vert.Y.CompareTo(Node.Y_mid)>0)
                    {
                        VertexRemove(Node.UpLeft, vert);
                    }
                    else
                    {
                        VertexRemove(Node.DownLeft, vert);
                    }
                }

                #endregion

            }

        }

        private List<IVertex<T>> VertexGetRegion( VertexQuadTreeNode<T> Node, int regionIndex, ref IVertex<T> Min, ref IVertex<T> Max )
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
                if (regionIndex < Node.UpRight.NumSubRegions)
                {
                    Min.Y = Node.Y_mid;
                    Min.X = Node.X_mid;
                    return VertexGetRegion(Node.UpRight, regionIndex, ref Min, ref Max);
                }

                // find if we move to the up left
                regionIndex -= Node.UpRight.NumSubRegions;

                if (regionIndex < Node.UpLeft.NumSubRegions)
                {
                    Min.Y = Node.Y_mid;
                    Max.X = Node.X_mid;
                    return VertexGetRegion(Node.UpLeft, regionIndex, ref Min, ref Max);
                }

                // find if we move to the down left
                regionIndex -=  Node.UpLeft.NumSubRegions;

                if (regionIndex < Node.DownLeft.NumSubRegions)
                {
                    Max.Y = Node.Y_mid;
                    Max.X = Node.X_mid;
                    return VertexGetRegion(Node.DownLeft, regionIndex, ref Min, ref Max);
                }

                // find if we move to the down Right
                regionIndex -= Node.DownLeft.NumSubRegions;

                if (regionIndex < Node.DownRight.NumSubRegions)
                {
                    Max.Y = Node.Y_mid;
                    Min.X = Node.X_mid;
                    return VertexGetRegion(Node.DownRight, regionIndex, ref Min, ref Max);
                }

                #endregion

                return null;
            }

        }


        #region Axes region 

        
        private void VertexGetRegionAxes(VertexQuadTreeNode<T> Node, ref int regionIndex , List<VertexQuadTreeRegionAxes> AxesList)
        {
            if (!Node.isLeaf)
            {
                // create a new axes for the level that i am now
                VertexQuadTreeRegionAxes newVQTRA = new VertexQuadTreeRegionAxes();
                AxesList.Add(newVQTRA);
                int dummyRegionIndex = regionIndex;

                // fill the X axis
                VertexGetRegionXAxes(Node.UpRight, ref dummyRegionIndex, newVQTRA,true,true);

                VertexGetRegionXAxes(Node.UpLeft, ref dummyRegionIndex, newVQTRA, false, true);

                VertexGetRegionXAxes(Node.DownLeft, ref dummyRegionIndex, newVQTRA, false, true);

                VertexGetRegionXAxes(Node.DownRight, ref dummyRegionIndex, newVQTRA, true, true);

                // reset the region index
                dummyRegionIndex = regionIndex;

                // fill the Y axis
                VertexGetRegionYAxes( Node.UpRight, ref dummyRegionIndex, newVQTRA, true, true );

                VertexGetRegionYAxes( Node.UpLeft, ref dummyRegionIndex, newVQTRA, true, true );

                VertexGetRegionYAxes( Node.DownLeft, ref dummyRegionIndex, newVQTRA, false, true );

                VertexGetRegionYAxes( Node.DownRight, ref dummyRegionIndex, newVQTRA, false, true );
                
                // go to create all the others axis in the other paths
                dummyRegionIndex = regionIndex;
                VertexGetRegionAxes( Node.UpRight, ref dummyRegionIndex, AxesList );

                dummyRegionIndex = Node.UpRight.NumSubRegions + regionIndex;
                VertexGetRegionAxes( Node.UpLeft, ref dummyRegionIndex, AxesList );

                dummyRegionIndex = Node.UpRight.NumSubRegions + Node.UpLeft.NumSubRegions + regionIndex;
                VertexGetRegionAxes( Node.DownLeft, ref dummyRegionIndex, AxesList );

                dummyRegionIndex = Node.UpRight.NumSubRegions + Node.UpLeft.NumSubRegions + Node.DownLeft.NumSubRegions + regionIndex;
                VertexGetRegionAxes( Node.DownRight, ref dummyRegionIndex, AxesList );
            }
        }

        private void VertexGetRegionXAxes(VertexQuadTreeNode<T> Node, ref int regionIndex, VertexQuadTreeRegionAxes CurrentAxes, Boolean isRight,Boolean isRoot)
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
                    regionIndex += Node.UpRight.NumSubRegions;

                    VertexGetRegionXAxes(Node.UpLeft, ref regionIndex, CurrentAxes,isRight,false);

                    VertexGetRegionXAxes(Node.DownLeft, ref regionIndex, CurrentAxes, isRight, false);

                    // inc region because we bye bass the region
                    regionIndex += Node.DownRight.NumSubRegions;

                }else{

                    VertexGetRegionXAxes( Node.UpRight, ref regionIndex, CurrentAxes, isRight, false );
                    
                    // inc region because we bye bass the region
                    regionIndex += Node.UpLeft.NumSubRegions;

                    // inc region because we bye bass the region
                    regionIndex += Node.DownLeft.NumSubRegions;

                    VertexGetRegionXAxes( Node.DownRight, ref regionIndex, CurrentAxes, isRight, false );

                }
            }

        }


        private void VertexGetRegionYAxes( VertexQuadTreeNode<T> Node, ref int regionIndex, VertexQuadTreeRegionAxes CurrentAxes, Boolean isUp, Boolean isRoot )
        {
            if ( Node.isLeaf ) {
                // xor the root and right to be able to have only one if :P
                if ( isUp ) {
                    // add the region to the specific axes
                    CurrentAxes.upSide.Add( regionIndex );
                } else {
                    // add the region to the specific axes
                    CurrentAxes.downSide.Add( regionIndex );
                }

                // inc the region index
                regionIndex++;
            } else {
                //  goto to lower level 
                // but filter the sides base on the left and right
                if ( isUp ) {

                    // inc region because we bye bass the region
                    regionIndex += Node.UpRight.NumSubRegions + Node.UpLeft.NumSubRegions;

                    VertexGetRegionYAxes( Node.DownLeft, ref regionIndex, CurrentAxes, isUp, false );

                    VertexGetRegionYAxes( Node.DownRight, ref regionIndex, CurrentAxes, isUp, false );


                } else {

                    VertexGetRegionYAxes( Node.UpRight, ref regionIndex, CurrentAxes, isUp, false );

                    VertexGetRegionYAxes( Node.UpLeft, ref regionIndex, CurrentAxes, isUp, false );

                    // inc region because we bye bass the region
                    regionIndex += Node.DownLeft.NumSubRegions + Node.DownRight.NumSubRegions;

                }
            }

        }

        #endregion



        #region Axes region Pair


        private void VertexGetRegionAxesPair( VertexQuadTreeNode<T> Node, ref int regionIndex, List<VertexQuadTreeRegionAxes> AxesList )
        {
            if ( !Node.isLeaf ) {
                // create a new axes for the level that i am now
                VertexQuadTreeRegionAxes newVQTRA = new VertexQuadTreeRegionAxes();
                AxesList.Add( newVQTRA );
                int dummyRegionIndex = regionIndex;

                // fill the X axis
                VertexGetRegionXAxesPair( Node.UpRight, ref dummyRegionIndex, newVQTRA, true, true );

                VertexGetRegionXAxesPair( Node.UpLeft, ref dummyRegionIndex, newVQTRA, false, true );

                VertexGetRegionXAxesPair( Node.DownLeft, ref dummyRegionIndex, newVQTRA, false, true );

                VertexGetRegionXAxesPair( Node.DownRight, ref dummyRegionIndex, newVQTRA, true, true );

                // reset the region index
                dummyRegionIndex = regionIndex;

                // fill the Y axis
                VertexGetRegionYAxesPair( Node.UpRight, ref dummyRegionIndex, newVQTRA, true, true );

                VertexGetRegionYAxesPair( Node.UpLeft, ref dummyRegionIndex, newVQTRA, true, true );

                VertexGetRegionYAxesPair( Node.DownLeft, ref dummyRegionIndex, newVQTRA, false, true );

                VertexGetRegionYAxesPair( Node.DownRight, ref dummyRegionIndex, newVQTRA, false, true );

                // go to create all the others axis in the other paths
                dummyRegionIndex = regionIndex;
                VertexGetRegionAxesPair( Node.UpRight, ref dummyRegionIndex, AxesList );

                dummyRegionIndex = Node.UpRight.NumSubRegions + regionIndex;
                VertexGetRegionAxesPair( Node.UpLeft, ref dummyRegionIndex, AxesList );

                dummyRegionIndex = Node.UpRight.NumSubRegions + Node.UpLeft.NumSubRegions + regionIndex;
                VertexGetRegionAxesPair( Node.DownLeft, ref dummyRegionIndex, AxesList );

                dummyRegionIndex = Node.UpRight.NumSubRegions + Node.UpLeft.NumSubRegions + Node.DownLeft.NumSubRegions + regionIndex;
                VertexGetRegionAxesPair( Node.DownRight, ref dummyRegionIndex, AxesList );
            }
        }

        private void VertexGetRegionXAxesPair( VertexQuadTreeNode<T> Node, ref int regionIndex, VertexQuadTreeRegionAxes CurrentAxes, Boolean isRight, Boolean isRoot )
        {
            if ( Node.isLeaf ) {
                // xor the root and right to be able to have only one if :P
                if ( isRight ) {
                    // add the region to the specific axes
                    CurrentAxes.rightSide.Add( regionIndex );
                } else {
                    // add the region to the specific axes
                    CurrentAxes.leftSide.Add( regionIndex );
                }

                // inc the region index
                regionIndex++;
            } else {
                //  goto to lower level 
                // but filter the sides base on the left and right
                if ( isRight ) {
                    // inc region because we bye bass the region
                    regionIndex += Node.UpRight.NumSubRegions;

                    VertexGetRegionXAxesPair( Node.UpLeft, ref regionIndex, CurrentAxes, isRight, false );

                    VertexGetRegionXAxesPair( Node.DownLeft, ref regionIndex, CurrentAxes, isRight, false );

                    // inc region because we bye bass the region
                    regionIndex += Node.DownRight.NumSubRegions;

                } else {

                    VertexGetRegionXAxesPair( Node.UpRight, ref regionIndex, CurrentAxes, isRight, false );

                    // inc region because we bye bass the region
                    regionIndex += Node.UpLeft.NumSubRegions;

                    // inc region because we bye bass the region
                    regionIndex += Node.DownLeft.NumSubRegions;

                    VertexGetRegionXAxesPair( Node.DownRight, ref regionIndex, CurrentAxes, isRight, false );

                }
            }

        }


        private void VertexGetRegionYAxesPair( VertexQuadTreeNode<T> Node, ref int regionIndex, VertexQuadTreeRegionAxes CurrentAxes, Boolean isUp, Boolean isRoot )
        {
            if ( Node.isLeaf ) {
                // xor the root and right to be able to have only one if :P
                if ( isUp ) {
                    // add the region to the specific axes
                    CurrentAxes.upSide.Add( regionIndex );
                } else {
                    // add the region to the specific axes
                    CurrentAxes.downSide.Add( regionIndex );
                }

                // inc the region index
                regionIndex++;
            } else {
                //  goto to lower level 
                // but filter the sides base on the left and right
                if ( isUp ) {

                    // inc region because we bye bass the region
                    regionIndex += Node.UpRight.NumSubRegions + Node.UpLeft.NumSubRegions;

                    VertexGetRegionYAxesPair( Node.DownLeft, ref regionIndex, CurrentAxes, isUp, false );

                    VertexGetRegionYAxesPair( Node.DownRight, ref regionIndex, CurrentAxes, isUp, false );


                } else {

                    VertexGetRegionYAxesPair( Node.UpRight, ref regionIndex, CurrentAxes, isUp, false );

                    VertexGetRegionYAxesPair( Node.UpLeft, ref regionIndex, CurrentAxes, isUp, false );

                    // inc region because we bye bass the region
                    regionIndex += Node.DownLeft.NumSubRegions + Node.DownRight.NumSubRegions;

                }
            }

        }

        #endregion


        #endregion
    }
}
