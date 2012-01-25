using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{
    public class VertexHVStaticNodeV<T> where T : struct , IComparable<T>
    {

        // list with the nodes of this region
        public List<IVertex<T>> VertexList;

        // the boundary of the zone
        public T MinX;
        public T MaxX;

        public VertexHVStaticNodeV()
        {
            // init the list
            VertexList = new List<IVertex<T>>();
        }

    }

    public class VertexHVStaticNodeH<T> where T : struct , IComparable<T>
    {
        // list with vertical nodes
        public List<VertexHVStaticNodeV<T>> VRegionsList;

        // the boundary of the zone
        public T MinY;
        public T MaxY;
        
        public VertexHVStaticNodeH()
        {
            // init the list
            VRegionsList = new List<VertexHVStaticNodeV<T>>();
        }
    }

    public class VertexHVStaticTree<T> : IVertexTree<T> where T : struct , IComparable<T>
    {
        /// <summary>
        /// List with all the vertex of the tree
        /// </summary>
        List<IVertex<T>> VertexList;

        /// <summary>
        /// List with all the horizontal region zones
        /// </summary>
        public List<VertexHVStaticNodeH<T>> HRegionsList;
        
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
        public VertexHVStaticTree(int MaxVertexInHorizonLeaf, int MaxVertexInVerticalLeaf)
        {
            // set the vertex limit
            this.MaxVertexInHorizonLeaf = MaxVertexInHorizonLeaf;
            this.MaxVertexInVerticalLeaf = MaxVertexInVerticalLeaf;

            // init horizontal regions list
            HRegionsList = new List<VertexHVStaticNodeH<T>>();

            // init the vertex list
            VertexList = new List<IVertex<T>>();
        }

        #endregion


        #region IVertex Func

        public void Add(IVertex<T> vert)
        {
            // add the Vertex to the list
            //if (!VertexList.Contains(vert))
            {
                VertexList.Add(vert);
            }
        }

        public void AddRange( IVertex<T>[] vertArray )
        {
            // add the vertex to the internal list
            VertexList.AddRange(vertArray);
        }

        public void Remove( IVertex<T> vert )
        {
            // remove the vertex from the internal list
            VertexList.Remove(vert);
        }

        public int GetRegionNum()
        {
            return NumRegions;
        }

        public List<IVertex<T>> GetRegion( int index, ref IVertex<T> Min, ref IVertex<T> Max )
        {
            if (index < NumRegions)
            {
                int offset = 0;

                // find the horizontal region that the vertical region that we want exist
                for (int i = 0; i < HRegionsList.Count; i++)
                {
                    offset += HRegionsList[i].VRegionsList.Count;

                    // check if we have past the index
                    if (index < offset)
                    {
                        // go back one step
                        offset -= HRegionsList[i].VRegionsList.Count;

                        // find the index relative with the vregion.
                        index -= offset;
                        
                        Min.X = HRegionsList[i].VRegionsList[index].MinX;
                        Max.X = HRegionsList[i].VRegionsList[index].MaxX;

                        Min.Y = HRegionsList[i].MinY;
                        Max.Y = HRegionsList[i].MaxY;

                        return HRegionsList[i].VRegionsList[index].VertexList;
                    }

                }
            }

            return null;
        }


        public List<VertexQuadTreeRegionAxes> GetRegionAxes()
        {
            List<VertexQuadTreeRegionAxes> vertexQuadTreeRegionAxes = new List<VertexQuadTreeRegionAxes>();

            int offset = 0;

            // pass all horizontal regions
            for (int i = 0; i < HRegionsList.Count; i++)
            {
                for (int j = 1; j < HRegionsList[i].VRegionsList.Count; j++)
                {
                    VertexQuadTreeRegionAxes tmp = new VertexQuadTreeRegionAxes();
                    tmp.leftSide.Add(j - 1 + offset);
                    tmp.rightSide.Add(j + offset);
                    vertexQuadTreeRegionAxes.Add(tmp);
                }

                // inc the offset
                offset += HRegionsList[i].VRegionsList.Count;
            }


            return vertexQuadTreeRegionAxes;
        }

        public IVertex<T>[] GenerateArray()
        {
            IVertex<T>[] VertexArray = VertexList.ToArray();

            // the number of horizontal zones base on the max number of vertex.
            int HZonesNum = (int)Math.Ceiling((double)VertexArray.Length / (double)MaxVertexInHorizonLeaf);
            int NumVertPerHZone = (int)Math.Floor((double)VertexArray.Length / (double)HZonesNum);

            int countRegions = 0;

            // sort base on y
            Array.Sort(VertexArray, VertexYComparer<T>.vc);

            // set the lower y
            T y_split_old = VertexArray[0].Y;
            int h_index_old = 0;

            for (int i = 0; i < HZonesNum; i++)
            {
                int h_index = NumVertPerHZone * (i + 1);
                h_index = (h_index < VertexArray.Length) ? h_index : VertexArray.Length - 1;

                // calc the first split point
                T y_split = VertexArray[h_index].Y;

                // find the last index
                for (int g = h_index + 1; g < VertexArray.Length; g++)
                    if (VertexArray[g].Y.Equals(y_split))
                        h_index = g;
                    else
                        break;

                // new HZone 
                VertexHVStaticNodeH<T> tmpHZone = new VertexHVStaticNodeH<T>();
                tmpHZone.MinY = y_split_old;
                tmpHZone.MaxY = y_split;

                int HVertexNum = h_index - h_index_old;

                // sort base on x the sub-list of vertex list
                Array.Sort(VertexArray, h_index_old, HVertexNum, VertexXComparer<T>.vc);

                int VZonesNum = (int)Math.Ceiling((double)HVertexNum / (double)MaxVertexInVerticalLeaf);
                int NumVertPerVZone = (int)Math.Floor((double)HVertexNum / (double)VZonesNum);
                T x_split_old = VertexArray[h_index_old].X;
                int v_index_old = 0;

                for (int j = 0; j < VZonesNum; j++)
                {
                    int v_index = NumVertPerVZone * (j + 1);
                    v_index = (v_index < HVertexNum) ? v_index : HVertexNum - 1;

                    // find the x split point
                    T x_split = VertexArray[h_index_old + v_index].X;

                    // find the last index
                    for (int g = h_index_old + v_index + 1; g < VertexArray.Length; g++)
                        if (VertexArray[g].X.Equals(x_split))
                            v_index = g - h_index_old;
                        else
                            break;

                    // create a new V node
                    VertexHVStaticNodeV<T> tmpVZone = new VertexHVStaticNodeV<T>();
                    tmpVZone.MinX = x_split_old;
                    tmpVZone.MaxX = x_split;

                    if (x_split_old.CompareTo(x_split) > 0)
                    {
                        Console.WriteLine("asd");
                    }

                    // set the old v index
                    v_index_old = v_index + 1;
                    x_split_old = x_split;

                    // add the v zone to h zone
                    tmpHZone.VRegionsList.Add(tmpVZone);
                    countRegions++;
                }

                // set the old h index
                h_index_old = h_index + 1;
                y_split_old = y_split;

                // add the h zones to the list
                HRegionsList.Add(tmpHZone);

            }

            return VertexArray;
        }



        public void Generate()
        {

            // the number of horizontal zones base on the max number of vertex.
            int HZonesNum = (int)Math.Ceiling((double)VertexList.Count / (double)MaxVertexInHorizonLeaf);
            int NumVertPerHZone = (int)Math.Floor((double)VertexList.Count / (double)HZonesNum);

            int countRegions = 0;

            // sort base on y
            VertexList.Sort(VertexYComparer<T>.vc);

            // set the lower y
            T y_split_old = VertexList[0].Y;

            int h_index_old = 0;

            for (int i = 0; i < HZonesNum; i++)
            {
                int h_index = NumVertPerHZone * (i + 1);
                h_index = (h_index < VertexList.Count) ? h_index : VertexList.Count - 1;

                // calc the first split point
                T y_split = VertexList[h_index].Y;

                // find the last index
                for (int g = h_index + 1; g < VertexList.Count; g++)
                    if (VertexList[g].Y.Equals(y_split))
                        h_index = g;
                    else
                        break;

                // new HZone 
                VertexHVStaticNodeH<T> tmpHZone = new VertexHVStaticNodeH<T>();
                tmpHZone.MinY = y_split_old;
                tmpHZone.MaxY = y_split;

                int HVertexNum = h_index - h_index_old;

                // sort base on x the sub-list of vertex list
                VertexList.Sort(h_index_old, HVertexNum, VertexXComparer<T>.vc);

                int VZonesNum = (int)Math.Ceiling((double)HVertexNum / (double)MaxVertexInVerticalLeaf);
                int NumVertPerVZone = (int)Math.Floor((double)HVertexNum / (double)VZonesNum);
                T x_split_old = VertexList[h_index_old].X;
                int v_index_old = 0;

                for (int j = 0; j < VZonesNum; j++)
                {
                    int v_index = NumVertPerVZone * (j + 1);
                    v_index = ( v_index < HVertexNum) ? v_index : HVertexNum - 1 ;

                    // find the x split point
                    T x_split = VertexList[h_index_old + v_index].X;

                    // find the last index
                    for (int g = h_index_old + v_index + 1; g < VertexList.Count; g++)
                        if (VertexList[g].X.Equals(x_split))
                            v_index = g - h_index_old;
                        else
                            break;

                    // create a new V node
                    VertexHVStaticNodeV<T> tmpVZone = new VertexHVStaticNodeV<T>();
                    tmpVZone.MinX = x_split_old;
                    tmpVZone.MaxX = x_split;

                    if (x_split_old.CompareTo(x_split)>0)
                    {
                        Console.WriteLine("asd");
                    }

                    // add the points of this region
                    tmpVZone.VertexList.AddRange(VertexList.GetRange(h_index_old + v_index_old, v_index - v_index_old));

                    // set the old v index
                    v_index_old = v_index + 1;
                    x_split_old = x_split;

                    // add the v zone to h zone
                    tmpHZone.VRegionsList.Add(tmpVZone);
                    countRegions++;
                }

                // set the old h index
                h_index_old = h_index + 1;
                y_split_old = y_split;

                // add the h zones to the list
                HRegionsList.Add(tmpHZone);
            }

            // set the global number of regions
            NumRegions = countRegions;

        }

        #endregion


       

    }
}
