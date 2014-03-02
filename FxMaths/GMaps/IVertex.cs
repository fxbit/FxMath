using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;

namespace FxMaths.GMaps
{
    public class VertexXComparer<T> : IComparer<IVertex<T>> where T : struct , IComparable<T>
    {
        public static VertexXComparer<T> vc = new VertexXComparer<T>();

        public int Compare(IVertex<T> ver1, IVertex<T> ver2)
        {
            return ver1.X.CompareTo(ver2.X);
        }
    }

    public class VertexYComparer<T> : IComparer<IVertex<T>> where T : struct , IComparable<T>
    {
        public static VertexYComparer<T> vc = new VertexYComparer<T>();

        public int Compare(IVertex<T> ver1, IVertex<T> ver2)
        {
            return ver1.Y.CompareTo(ver2.Y);
        }
    }

    public class VertexXYComparer<T> : IEqualityComparer<IVertex<T>> where T : struct , IComparable<T>
    {
        /// <summary>
        /// The vertex comparer
        /// </summary>
        public static VertexXYComparer<T> vc = new VertexXYComparer<T>(0.1);
        
        /// <summary>
        /// The eps of how close can be the vertex 
        /// </summary>
        public double Eps = 0.1;


        /// <summary>
        /// Compare the vertex base on the eps between them.
        /// </summary>
        /// <param name="eps"></param>
        public VertexXYComparer(double eps)
        {
            // set the internal eps
            this.Eps = eps;
        }

        public bool Equals(IVertex<T> ver1, IVertex<T> ver2)
        {
            dynamic dx = ver1.X;
            dx -=ver2.X;

            dynamic dy = ver1.Y;
            dy -= ver2.Y;

            return (Math.Abs(dx) <= Eps) && (Math.Abs(dy) <= Eps);
        }

        public int GetHashCode(IVertex<T> obj)
        {
            return obj.GetHashCode();
        }
    }


    public interface IVertex<T> : IEquatable<IVertex<T>>, IEqualityComparer<IVertex<T>> where T : struct , IComparable<T>
    {
        T X { get; set; }
        T Y { get; set; }
        T Z { get; set; }

        #region I/O

        /// <summary>
        /// Write the vertex to the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        void WriteToDataStream(DataStream dataStream);

        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        void ReadFromDataStream(DataStream dataStream);

        #endregion

        #region General Utils

        /// <summary>
        /// Return a copy of the vertex.
        /// </summary>
        /// <returns></returns>
        IVertex<T> Copy();

        #endregion

        #region Math functions

        float Dot(ref IVertex<T> vec);

        void Add(ref IVertex<T> vec);

        void Subtract(ref IVertex<T> vec);

        float Distance(ref IVertex<T> vec);

        float Length();

        void Normalize();

        #endregion

        #region String Uitls

        string ToString(String Format);

        #endregion
    }
}
