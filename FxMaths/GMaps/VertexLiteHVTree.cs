// -----------------------------------------------------------------------
// <copyright file="VertexLiteHVTree.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace FxMaths.GMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;



    public class VertexLiteHVTree_Horizontal<T>
    {
        /// <summary>
        /// The start of the region in the main Array
        /// </summary>
        public int StartID;

        /// <summary>
        /// The start of the region in the main Array
        /// </summary>
        public int EndID;

        // the boundary of the zone
        public T MinY;
        public T MaxY;

        /// <summary>
        /// List with all vertical regions
        /// </summary>
        public List<VertexLiteHVTree_Vertical<T>> VerticalRegionList = new List<VertexLiteHVTree_Vertical<T>>();

    }

    public class VertexLiteHVTree_Vertical<T>
    {
        /// <summary>
        /// The start of the region in the main Array
        /// </summary>
        public int StartID;

        /// <summary>
        /// The start of the region in the main Array
        /// </summary>
        public int EndID;

        // the boundary of the zone
        public T MinX;
        public T MaxX;
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VertexLiteHVTree<T> : IVertexTree<T> where T : struct, IComparable<T>
    {
        #region Public Variables

        /// <summary>
        /// Root of the region structure
        /// </summary>
        public List<VertexLiteHVTree_Horizontal<T>> Root;


        #endregion



        #region Private Variables

        /// <summary>
        /// The number of points per region
        /// </summary>
        private int PointsPerRegion;

        /// <summary>
        /// The Array with the internal vertex
        /// </summary>
        public IVertex<T>[] VertexArray;

        /// <summary>
        /// For performance get region we are going to have copy list
        /// </summary>
        private List<IVertex<T>> VertexList;

        /// <summary>
        /// The number of regions
        /// </summary>
        private int RegionsNum;

        #endregion


        public VertexLiteHVTree(int NumOfPointsPerRegion)
        {
            // init root list
            Root = new List<VertexLiteHVTree_Horizontal<T>>();
            PointsPerRegion = NumOfPointsPerRegion;
        }


        #region Add/Remove

        void IVertexTree<T>.Add(IVertex<T> vert)
        {
            throw new NotImplementedException();
        }

        void IVertexTree<T>.AddRange(IVertex<T>[] vertArray)
        {
            VertexArray = vertArray;
        }

        void IVertexTree<T>.Remove(IVertex<T> vert)
        {
            throw new NotImplementedException();
        }
        
        #endregion



        int IVertexTree<T>.GetRegionNum()
        {
            return RegionsNum;
        }

        List<IVertex<T>> IVertexTree<T>.GetRegion(int index, ref IVertex<T> Min, ref IVertex<T> Max)
        {
            if (index < RegionsNum)
            {
                int offset = 0;

                // find the horizontal region that the vertical region that we want exist
                for (int i = 0; i < Root.Count; i++)
                {
                    offset += Root[i].VerticalRegionList.Count;

                    // check if we have past the index
                    if (index < offset)
                    {
                        // go back one step
                        offset -= Root[i].VerticalRegionList.Count;

                        // find the index relative with the vregion.
                        index -= offset;

                        VertexLiteHVTree_Vertical<T> tmpVert = Root[i].VerticalRegionList[index];

                        Min.X = tmpVert.MinX;
                        Max.X = tmpVert.MaxX;

                        Min.Y = Root[i].MinY;
                        Max.Y = Root[i].MaxY;

                        return VertexList.GetRange(tmpVert.StartID, tmpVert.EndID - tmpVert.StartID + 1);
                    }

                }
            }

            return null;
        }

        List<VertexQuadTreeRegionAxes> IVertexTree<T>.GetRegionAxes()
        {
            List<VertexQuadTreeRegionAxes> vertexQuadTreeRegionAxes = new List<VertexQuadTreeRegionAxes>();

            int offset = 0;

            // pass all horizontal regions
            for (int i = 0; i < Root.Count; i++)
            {
                for (int j = 1; j < Root[i].VerticalRegionList.Count; j++)
                {
                    VertexQuadTreeRegionAxes tmp = new VertexQuadTreeRegionAxes();
                    tmp.leftSide.Add(j - 1 + offset);
                    tmp.rightSide.Add(j + offset);
                    vertexQuadTreeRegionAxes.Add(tmp);
                }

                // inc the offset
                offset += Root[i].VerticalRegionList.Count;
            }


            return vertexQuadTreeRegionAxes;
        }




        void IVertexTree<T>.Generate()
        {
            int VertexNum = VertexArray.Length;
#if true
            RegionsNum = (int)Math.Ceiling((float)VertexNum / (float)PointsPerRegion);


            int HorizontalRegions = (int)Math.Ceiling(Math.Sqrt(RegionsNum));
            int HorizontalRegionsOffset = (int)Math.Ceiling((float)VertexNum / (float)HorizontalRegions);
            int VerticalRegions = (int)Math.Ceiling((float)RegionsNum / (float)HorizontalRegions);   
#else
            int HorizontalRegions = (int)Math.Floor(Math.Sqrt(VertexNum / Math.Log(VertexNum, 2)));
            int HorizontalRegionsOffset = (int)Math.Ceiling((float)VertexNum / (float)HorizontalRegions);
            int VerticalRegions = HorizontalRegions;
            
#endif

            // recalc the region number
            RegionsNum = HorizontalRegions * VerticalRegions;

            // sort base on y
            Array.Sort(VertexArray, VertexYComparer<T>.vc);

            int startID = 0;
            T y_max = VertexArray[0].Y;

            // split the array horizontally
            for (int i = 0; i < HorizontalRegions; i++)
            {
                // create tmp Horizontal region
                VertexLiteHVTree_Horizontal<T> tmpHorizontal = new VertexLiteHVTree_Horizontal<T>();

                tmpHorizontal.EndID = (i + 1) * HorizontalRegionsOffset;

                if (tmpHorizontal.EndID >= VertexArray.Length)
                    tmpHorizontal.EndID = VertexArray.Length - 1;
                else
                {
                    // calc the first split point
                    T y_split = VertexArray[tmpHorizontal.EndID].Y;

                    // find the last index
                    for (int g = tmpHorizontal.EndID + 1; g < VertexArray.Length; g++)
                        if (VertexArray[g].Y.Equals(y_split))
                            tmpHorizontal.EndID = g;
                        else
                            break;
                }

                // set the startID
                tmpHorizontal.StartID = startID;

                // renew the startID for the next one
                startID = tmpHorizontal.EndID + 1;

                // calc the min/max
                tmpHorizontal.MinY = VertexArray[tmpHorizontal.EndID].Y;
                tmpHorizontal.MaxY = y_max;

                y_max = tmpHorizontal.MinY;

                // add to the list
                Root.Add(tmpHorizontal);
            }


            // split the array horizontally
            Parallel.For(0, HorizontalRegions, (g) =>
            {
                
                VertexLiteHVTree_Horizontal<T> HRegion = Root[g];

                // sort the horizontal region base on x
                Array.Sort(VertexArray, HRegion.StartID, HRegion.EndID - HRegion.StartID + 1, VertexXComparer<T>.vc);


                int VerticalRegionsOffset = (int)Math.Ceiling((float)(HRegion.EndID - HRegion.StartID + 1) / (float)VerticalRegions);

                int startId = HRegion.StartID;
                T x_max = VertexArray[startId].X;

                for (int i = 1; i <= VerticalRegions; i++)
                {

                    // create tmp vertical region
                    VertexLiteHVTree_Vertical<T> tmpVertical = new VertexLiteHVTree_Vertical<T>();

                    tmpVertical.EndID = i * VerticalRegionsOffset + HRegion.StartID;

                    // check if we have exist the array
                    if (tmpVertical.EndID >= HRegion.EndID)
                    {
                        tmpVertical.EndID = HRegion.EndID;
                    }
                    else
                    {
                        // calc the first split point
                        T x_split = VertexArray[tmpVertical.EndID].X;

                        // find the last index
                        for (int h = tmpVertical.EndID + 1; h < VertexArray.Length; h++)
                            if (VertexArray[h].X.Equals(x_split))
                                tmpVertical.EndID = h;
                            else
                                break;
                    }

                    // set the startID
                    tmpVertical.StartID = startId;

                    // set the next startID one point after the one that we are now
                    startId = tmpVertical.EndID + 1;

                    // set the max/min
                    tmpVertical.MinX = VertexArray[tmpVertical.EndID].X;
                    tmpVertical.MaxX = x_max;

                    x_max = tmpVertical.MinX;


                    // add the vertical region to the horizontal
                    HRegion.VerticalRegionList.Add(tmpVertical);
                }

                    
            });


            // create the vertex list
            VertexList = VertexArray.ToList<IVertex<T>>();
        }
    }
}
