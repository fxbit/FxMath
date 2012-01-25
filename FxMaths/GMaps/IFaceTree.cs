using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{
    public enum FaceSide{Left,Right,Up,Down};

    public interface IFaceTree
    {
        /// <summary>
        /// Add a specific face.
        /// </summary>
        /// <param name="newFace"></param>
        void Add(Face newFace);

        /// <summary>
        /// Remove a specific face.
        /// </summary>
        /// <param name="oldFace"></param>
        void Remove(Face oldFace);

        /// <summary>
        /// Find the face that the vertex hit.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        Face FindFace( IVertex<float> vertex );
        
        /// <summary>
        /// Find all the faces that have the specific vertex.
        /// The vertex must be exactly the same ( pointer compare );
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        List<Face> FindFaceRange( IVertex<float> vertex );

        /// <summary>
        /// Update the tree base on the updated face.
        /// </summary>
        /// <param name="face"></param>
        void Update(Face face);

        /// <summary>
        /// execute to all faces a given function; 
        /// </summary>
        /// <param name="func"></param>
        void FunctionExec(FaceFunction func);

        /// <summary>
        /// Get the statistics of the tree
        /// </summary>
        /// <param name="MaxDepth"></param>
        /// <param name="MinDepth"></param>
        /// <param name="NumNodes"></param>
        /// <param name="NumLeaf"></param>
        /// <param name="NumFaces"></param>
        /// <param name="NumFacesInNodes"></param>
        void GetStat(out int MaxDepth, out int MinDepth, out int NumNodes, out int NumLeaf, out int NumFaces, out int NumFacesInNodes);

        /// <summary>
        /// Get the Boundary points in whole mesh of the faces.
        /// </summary>
        /// <param name="level">The level of points that we want to retrieve.</param>
        /// <param name="faceSide">The side that we want to retrieve the points.</param>
        /// <returns></returns>
        List<IVertex<float>> GetSidePoints( int level, FaceSide faceSide );

    }
}
