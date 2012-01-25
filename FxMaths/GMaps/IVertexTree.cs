using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{
    public interface IVertexTree<T> where T : struct , IComparable<T>
    {

        /// <summary>
        /// Add vertex to the tree
        /// </summary>
        /// <param name="vert"></param>
        void Add( IVertex<T> vert );

        /// <summary>
        /// Add vertex to the tree
        /// </summary>
        /// <param name="vertArray"></param>
        void AddRange( IVertex<T>[] vertArray );

        /// <summary>
        /// Remove vertex
        /// </summary>
        /// <param name="vert"></param>
        void Remove( IVertex<T> vert );
        
        /// <summary>
        /// Get the number of regions
        /// </summary>
        /// <returns></returns>
        int GetRegionNum();

        /// <summary>
        /// Get a list with the vertex that exist in 
        /// leaf of the tree
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        List<IVertex<T>> GetRegion( int index, ref IVertex<T> Min, ref IVertex<T> Max );

        /// <summary>
        /// Get X axes links
        /// </summary>
        /// <returns></returns>
        List<VertexQuadTreeRegionAxes> GetRegionAxes();


        /// <summary>
        /// Generate the tree. This is helpful if we have static tree
        /// that doesn't support dynamic add/remove.
        /// </summary>
        void Generate();
    }
}
