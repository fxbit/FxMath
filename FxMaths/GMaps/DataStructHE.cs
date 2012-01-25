using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace FxMaths.GMaps
{
    /// <summary>
    /// Function delegate for face execute.
    /// </summary>
    /// <param name="face"></param>
    public delegate void HalfEdgeFunction( Half_Edge face );

    struct BoundaryNodeMaxMin
    {
        /// <summary>
        /// The Max boundary node in the X axis
        /// </summary>
        public BoundaryNode X_Max;

        /// <summary>
        /// The Max boundary node in the Y axis
        /// </summary>
        public BoundaryNode Y_Max;

        /// <summary>
        /// The Min boundary node in the X axis
        /// </summary>
        public BoundaryNode X_Min;

        /// <summary>
        /// The Min boundary node in the Y axis
        /// </summary>
        public BoundaryNode Y_Min;
    }

    public class DataStructHE
    {

        #region Private Vars

        /// <summary>
        /// Tree with all the faces
        /// </summary>
        private IFaceTree faceTree;


        /// <summary>
        /// The root of the boundary
        /// </summary>
        private BoundaryNode RootBoundaryNode;

        /// <summary>
        /// Struct with the min max nodes
        /// </summary>
        private BoundaryNodeMaxMin BoundaryMinMax;
        #endregion


        #region Constructor

        /// <summary>
        /// Start empty data struct 
        /// This constructor is good only when you insert the triangles manually
        /// </summary>
        public DataStructHE( IFaceTree faceTree )
        {
            // init the face tree
            this.faceTree = faceTree;

            // ini the min max boundary
            BoundaryMinMax = new BoundaryNodeMaxMin();

        }

        /// <summary>
        /// Init the data struct with Predefine triangle
        /// </summary>
        /// <param name="ver1"></param>
        /// <param name="ver2"></param>
        /// <param name="ver3"></param>
        public DataStructHE(IVertex<float> ver1, IVertex<float> ver2, IVertex<float> ver3)
        {
            Init(ver1, ver2, ver3, new FaceDistMixTree(100));
        }

        /// <summary>
        /// Init the data Struct with Predefine triangle
        /// And give the prefer tree struct for the faces.
        /// </summary>
        /// <param name="ver1"></param>
        /// <param name="ver2"></param>
        /// <param name="ver3"></param>
        public DataStructHE(IVertex<float> ver1, IVertex<float> ver2, IVertex<float> ver3, IFaceTree faceTree)
        {
            Init(ver1, ver2, ver3, faceTree);
        }

        /// <summary>
        /// Init the internal state
        /// </summary>
        /// <param name="ver1"></param>
        /// <param name="ver2"></param>
        /// <param name="ver3"></param>
        /// <param name="faceTree"></param>
        private void Init(IVertex<float> ver1, IVertex<float> ver2, IVertex<float> ver3, IFaceTree faceTree)
        {
            // init the face tree
            this.faceTree = faceTree;

            // ini the min max boundary
            BoundaryMinMax = new BoundaryNodeMaxMin();

            // add the 3 half edge
            Half_Edge he1, he2, he3;

            // crate the triangle
            Half_Edge.CreateTriangle(ver1, ver2, ver3,
                                      out he1, out he2, out he3);


            // create and link the boundary list
            {
                // create the boundary list
                RootBoundaryNode = new BoundaryNode(he1, true);
                BoundaryNode nextBoundaryNode1 = new BoundaryNode(he1.NextEdge);
                BoundaryNode nextBoundaryNode2 = new BoundaryNode(he1.NextEdge.NextEdge);

                // link the root with the next node
                RootBoundaryNode.NextNode = nextBoundaryNode1;
                nextBoundaryNode1.PrevNode = RootBoundaryNode;

                // link the root with the next node
                nextBoundaryNode1.NextNode = nextBoundaryNode2;
                nextBoundaryNode2.PrevNode = nextBoundaryNode1;

                // link the first with the last one
                nextBoundaryNode2.NextNode = RootBoundaryNode;
                RootBoundaryNode.PrevNode = nextBoundaryNode2;

                // select the min/max nodes
                {
                    // init min/max
                    BoundaryMinMax.X_Max = RootBoundaryNode;
                    BoundaryMinMax.X_Min = RootBoundaryNode;
                    BoundaryMinMax.Y_Max = RootBoundaryNode;
                    BoundaryMinMax.Y_Min = RootBoundaryNode;

                    // set the start node and move to the next one 
                    // because we use the first for init
                    BoundaryNode tmpNode = RootBoundaryNode;
                    tmpNode = tmpNode.NextNode;

                    while (true)
                    {

                        // update the boundary max/min
                        UpdateBoundaryMaxMin(tmpNode);


                        // move to the next node
                        tmpNode = tmpNode.NextNode;

                        // break if we are in the start
                        if (tmpNode.IsTheRoot)
                            break;
                    }
                }
            }



            // create a face base on the  3 vertex
            Face newFace = new Face(he1);

            // add the face to the tree
            faceTree.Add(newFace);

        }

        #endregion


        #region Add/Remove



        #region Add/Remove Vertex



        /// <summary>
        /// Add vertex to data struct.
        /// </summary>
        /// <param name="vert"></param>
        public void AddVertex(IVertex<float> vert)
        {
            // find the face to attach
            Face selectFace = faceTree.FindFace(vert);

            // check if we find a triangle
            if (selectFace != null)
            {

                // disable the add base on select faces
#if true
                Half_Edge minEdge;
                double minDist = selectFace.minDistEdge(vert, out minEdge);
                // check how close we are in the edges

                if (minDist > 0.1)
                {

                    #region split the face to 3 faces

                    //  get the he of the prev face
                    Half_Edge he1 = selectFace.HalfEnd;
                    Half_Edge he2 = selectFace.HalfEnd.NextEdge;
                    Half_Edge he3 = selectFace.HalfEnd.NextEdge.NextEdge;


                    // remove the old face
                    faceTree.Remove(selectFace);

                    // ------------------------------------------------------------------------------ //
                    // create the first triangle
                    Half_Edge he_a1, he_a2;
                    Half_Edge.CreateTriangle(he1, he2.StartVertex, vert, out he_a1, out he_a2);

                    // ------------------------------------------------------------------------------ //
                    // create the sec triangle
                    Half_Edge he_b1, he_b2;
                    Half_Edge.CreateTriangle(he2, he3.StartVertex, vert, out he_b1, out he_b2);

                    // ------------------------------------------------------------------------------ //
                    // create the third triangle
                    Half_Edge he_c1, he_c2;
                    Half_Edge.CreateTriangle(he3, he1.StartVertex, vert, out he_c1, out he_c2);

                    // ------------------------------------------------------------------------------ //

                    // link he twin edge
                    he_a1.TwinEdge = he_b2;
                    he_b2.TwinEdge = he_a1;

                    he_a2.TwinEdge = he_c1;
                    he_c1.TwinEdge = he_a2;

                    he_b1.TwinEdge = he_c2;
                    he_c2.TwinEdge = he_b1;


                    // create 3 new faces
                    Face fa1 = new Face(he1);
                    Face fa2 = new Face(he2);
                    Face fa3 = new Face(he3);

                    // add the new faces to the tree
                    faceTree.Add(fa1);
                    faceTree.Add(fa2);
                    faceTree.Add(fa3);


                    // test for edge swap
                    he_a2.DelaunayCorrection(he1.TwinEdge, faceTree);
                    he_b2.DelaunayCorrection(he2.TwinEdge, faceTree);
                    he_c2.DelaunayCorrection(he3.TwinEdge, faceTree);

                    #endregion

                }
                else
                {
#if false
                    #region split the face to 4 faces

                    Console.WriteLine( "Find minDist:" + minDist.ToString() );

                    // calc the dist from the vert
                    double dx = minEdge.StartVertex.X - vert.X;
                    double dy = minEdge.StartVertex.Y - vert.Y;

                    double dist1 = Math.Sqrt( dx * dx + dy * dy );

                    dx = minEdge.NextEdge.StartVertex.X - vert.X;
                    dy = minEdge.NextEdge.StartVertex.Y - vert.Y;

                    double dist2 = Math.Sqrt( dx * dx + dy * dy );


                    Console.WriteLine( "vert1 dist :" + dist1.ToString() );
                    Console.WriteLine( "vert2 dist :" + dist2.ToString() );

                    #endregion
#endif
                }

#endif

            }
            else
            {
                //Console.WriteLine("Out of Boundary ");

                #region add new triangle to the Boundary

                // start the node from the start
                BoundaryNode tmpNode = RootBoundaryNode;
                int count = 0;

                BoundaryNode firstNode = null, lastNode = null;
                Boolean foundTheFirst = false, firstMatchIsRoot = false;
                Boolean TooClose = false;

                #region Forward Test
                // pass all the nodes and find the first and the last one that we can use
                while (true)
                {
                    Half_Edge he = tmpNode.currentEdge;


                    if ((vert.X - he.StartVertex.X) * (vert.X - he.StartVertex.X) + (vert.Y - he.StartVertex.Y) * (vert.Y - he.StartVertex.Y) < 0.1)
                    {

                        TooClose = true;
                        break;
                    }


                    // test the angle if is negative
                    // (neg mean that we are outside from the mesh )
                    if (Half_Edge.SideTest(he.StartVertex, he.NextEdge.StartVertex, vert))
                    {
                        count++;
                        if (foundTheFirst)
                        {
                            lastNode = tmpNode;
                        }
                        else
                        {
                            // set the first node 
                            firstNode = tmpNode;

                            // set the last node
                            lastNode = tmpNode;

                            // set the flag that we have find the first
                            foundTheFirst = true;

                            if (tmpNode.IsTheRoot)
                                firstMatchIsRoot = true;
                        }
                    }
                    else if (foundTheFirst)
                        break;

                    // go to the next node
                    tmpNode = tmpNode.NextNode;

                    // check that we are in the beginning
                    if (tmpNode.IsTheRoot)
                        break;

                }
                #endregion


                if (firstNode == null)
                    return;

                if (TooClose)
                    return;

                #region Backward Test
                // reset the variables
                tmpNode = RootBoundaryNode.PrevNode;

                // pass all the nodes and find the first and the last one that we can use
                if (firstMatchIsRoot)
                    while (true)
                    {
                        Half_Edge he = tmpNode.currentEdge;

                        if ((vert.X - he.StartVertex.X) * (vert.X - he.StartVertex.X) + (vert.Y - he.StartVertex.Y) * (vert.Y - he.StartVertex.Y) < 0.1)
                        {

                            TooClose = true;
                            break;
                        }


                        // test the angle if is negative
                        // (neg mean that we are outside from the mesh )
                        if (Half_Edge.SideTest(he.StartVertex, he.NextEdge.StartVertex, vert))
                        {
                            count++;
                            // set the first node 
                            firstNode = tmpNode;
                        }
                        else
                            break;

                        // go to the next node
                        tmpNode = tmpNode.PrevNode;

                        // check that we are in the beginning
                        if (tmpNode.IsTheRoot)
                            break;

                    }
                #endregion

                if (TooClose)
                    return;


                #region Link the new nodes 

                // pass the nodes from the first to last 
                tmpNode = firstNode;
                foundTheFirst = false;
                int count2 = 0;
                BoundaryNode endNode = lastNode.NextNode;
                while (tmpNode != endNode)
                {
                    Half_Edge he = tmpNode.currentEdge;
                    count2++;
                    Half_Edge he1, he2, he3;

                    // create the triangle
                    Half_Edge.CreateTriangle(he.StartVertex, vert, he.NextEdge.StartVertex,
                                              out he1, out he2, out he3);

                    // link the twin edge
                    he.TwinEdge = he3;
                    he3.TwinEdge = he;

                    // if we are in the mid of the search 
                    // link the prev edge to the triangle
                    if (foundTheFirst)
                    {

                        // link the twin edges
                        he1.TwinEdge = tmpNode.PrevNode.currentEdge;
                        tmpNode.PrevNode.currentEdge.TwinEdge = he1;

                        // check if we are going to remove the root node
                        if (tmpNode.PrevNode.IsTheRoot)
                        {
                            // change the root node
                            tmpNode.PrevNode.PrevNode.IsTheRoot = true;
                            RootBoundaryNode = tmpNode.PrevNode.PrevNode;
                        }

                        // remove the prev one
                        tmpNode.PrevNode.Remove();

                        // replace the current edge with this one
                        tmpNode.currentEdge = he2;

                        // update the boundary node
                        UpdateBoundaryMaxMin(tmpNode);
                    }
                    else
                    {

                        // replace the board node with the 2 news
                        tmpNode.currentEdge = he1;

                        // update the boundary size
                        UpdateBoundaryMaxMin(tmpNode);

                        // add a new one after the one that we are now
                        tmpNode.InsertNode(new BoundaryNode(he2), true);

                        // move to the next one
                        tmpNode = tmpNode.NextNode;

                        //update the new node
                        UpdateBoundaryMaxMin(tmpNode);

                        // set that we have find the first that is correct
                        foundTheFirst = true;
                    }


                    Face newFace = new Face(he1);

                    // add the triangle to the list 
                    faceTree.Add(newFace);

                    // fix the triangulation
                    newFace.HalfEnd.NextEdge.DelaunayCorrection(newFace.HalfEnd.NextEdge.NextEdge.TwinEdge, faceTree);

                    // move one to the list
                    tmpNode = tmpNode.NextNode;
                }

                #endregion
                

                #endregion

            }
        }


        /// <summary>
        /// Remove the vertex from the data struct.
        /// </summary>
        /// <param name="vert"></param>
        public void RemoveVertex(IVertex<float> vert)
        {
            Boolean isBoundaryPoint = false;

            // Find all the faces that include the vertex
            List<Face> faceList = faceTree.FindFaceRange(vert);

            // check if the vertex is in the boundary
            foreach (Face fa in faceList)
            {
                Half_Edge he = fa.HalfEnd;

                //// find the half edge that correspond to the vert 
                //if ( fa.HalfEnd.StartVertex.Equals(vert) )
                //    he = fa.HalfEnd;
                //else if ( fa.HalfEnd.NextEdge.StartVertex.Equals(vert) )
                //    he = fa.HalfEnd.NextEdge;
                //else
                //    he = fa.HalfEnd.NextEdge.NextEdge;

                // check if the face that start or end to the vert is in boundary
                if (he.TwinEdge == null || he.NextEdge.TwinEdge == null || he.NextEdge.NextEdge.TwinEdge == null)
                {
                    isBoundaryPoint = true;
                    break;
                }
            }

            // just for test remove all the faces from the find range
            foreach (Face fa in faceList)
            {

                // remove the face from the tree
                faceTree.Remove(fa);

                // check if the face was in the boarder
                if (isBoundaryPoint)
                {

                    #region remove the triangles that are in boundary

                    // get the outside half edge
                    Half_Edge he = fa.HalfEnd;

                    // set the twin edge to the boarder 
                    if (he.TwinEdge != null)
                    {
                        he.TwinEdge.TwinEdge = null;
                    }
                    // TODO: Remove the boundary edge from the boundary list and add the new one


                    // go to the next edge
                    he = he.NextEdge;

                    // set the twin edge to the boarder 
                    if (he.TwinEdge != null)
                    {
                        he.TwinEdge.TwinEdge = null;
                    }
                    // TODO: Remove the boundary edge from the boundary list and add the new one

                    // go to the next edge
                    he = he.NextEdge;

                    // set the twin edge to the boarder 
                    if (he.TwinEdge != null)
                    {
                        he.TwinEdge.TwinEdge = null;
                    }
                    // TODO: Remove the boundary edge from the boundary list and add the new one
                    #endregion

                }
            }
        }

        #endregion




        #region Add/Remove Faces

        /// <summary>
        /// Add external face
        /// </summary>
        /// <param name="face"></param>
        public void AddFace( Face face )
        {
            // add the external face
            faceTree.Add( face );
        }

        /// <summary>
        /// Remove external face
        /// </summary>
        /// <param name="face"></param>
        public void RemoveFace( Face face )
        {
            // add the external face
            faceTree.Remove( face );
        }

        #endregion


        #endregion


        #region Utils

        /// <summary>
        /// Execute the input function in each face
        /// </summary>
        /// <param name="func"></param>
        public void FaceFunctionExec(FaceFunction func)
        {
            // pass the function to the higher level
            faceTree.FunctionExec(func);
        }

        /// <summary>
        /// Get the Boundary points in whole mesh of the faces.
        /// </summary>
        /// <param name="level">The level of points that we want to retrieve.</param>
        /// <param name="faceSide">The side that we want to retrieve the points.</param>
        /// <returns></returns>
        public List<IVertex<float>> GetSidePoints(int level, FaceSide faceSide)
        {

            List<IVertex<float>> result = new List<IVertex<float>>();

            // check the side
            BoundaryNode StartNode = null;
            BoundaryNode tmpNode = null;
            Boolean Up = false;
            switch (faceSide)
            {
                case FaceSide.Right:
                    // select the start node
                    StartNode = BoundaryMinMax.X_Max;

                    Up = true;
                    break;

                case FaceSide.Left:
                    // select the start node
                    StartNode = BoundaryMinMax.X_Min;

                    Up = false;
                    break;
            }

            // add the min/max nodes
            result.Add(StartNode.currentEdge.StartVertex);


            #region Forward search
            // forward search
            tmpNode = StartNode.NextNode;
            float Y_limit = StartNode.currentEdge.StartVertex.Y;
            Boolean accept = false;
            while (true)
            {
                Half_Edge he = tmpNode.currentEdge;

                accept = (Up) ? (he.StartVertex.Y > Y_limit) : (he.StartVertex.Y < Y_limit);

                if (accept)
                {
                    Y_limit = he.StartVertex.Y;
                    // add the edge
                    result.Add(he.StartVertex);
                }
                else // if we fail the angle then break the search
                    break;

                // move to the next one 
                tmpNode = tmpNode.NextNode;

                // check of we are in the start
                if (tmpNode == StartNode)
                    break;
            }

            #endregion




            #region Backward search
            // forward search
            tmpNode = StartNode.PrevNode;
            Y_limit = StartNode.currentEdge.StartVertex.Y;
            while (true)
            {
                Half_Edge he = tmpNode.currentEdge;

                accept = (Up) ? (he.StartVertex.Y < Y_limit) : (he.StartVertex.Y > Y_limit);

                if (accept)
                {
                    Y_limit = he.StartVertex.Y;
                    // add the edge
                    result.Add(he.StartVertex);
                }
                else // if we fail the angle then break the search
                    break;

                // move to the next one 
                tmpNode = tmpNode.PrevNode;

                // check of we are in the start
                if (tmpNode == StartNode)
                    break;
            }

            #endregion



            return result;

        }

        /// <summary>
        /// Get the Boundary points in whole mesh of the faces.
        /// </summary>
        /// <param name="level">The level of points that we want to retrieve.</param>
        /// <param name="faceSide">The side that we want to retrieve the points.</param>
        public void GetSideBoundaryNode( int level, FaceSide faceSide, out BoundaryNode startNode, out BoundaryNode endNode )
        {
            // check the side
            BoundaryNode _StartNode = null;
            BoundaryNode tmpNode = null;
            Boolean Up = false;

            #region Set the startNode and the direction

            switch ( faceSide ) {
                case FaceSide.Right:
                    // select the start node
                    _StartNode = BoundaryMinMax.X_Max;

                    Up = true;
                    break;

                case FaceSide.Left:
                    // select the start node
                    _StartNode = BoundaryMinMax.X_Min;

                    Up = false;
                    break;
            }
            
            #endregion



            #region Forward search
            // forward search
            tmpNode = _StartNode.NextNode;
            float Y_limit = _StartNode.currentEdge.StartVertex.Y;
            Boolean accept = false;
            while ( true ) {
                Half_Edge he = tmpNode.currentEdge;

                accept = ( Up ) ? ( he.StartVertex.Y > Y_limit ) : ( he.StartVertex.Y < Y_limit );

                if ( accept ) {
                    Y_limit = he.StartVertex.Y;
                } else // if we fail the angle then break the search
                    break;

                // move to the next one 
                tmpNode = tmpNode.NextNode;

                // check of we are in the start
                if ( tmpNode == _StartNode )
                    break;
            }

            #endregion


            // set the end node ( go one backward to get the correct node )
            endNode = tmpNode.PrevNode;


            #region Backward search
            // Backward search
            tmpNode = _StartNode.PrevNode;
            Y_limit = _StartNode.currentEdge.StartVertex.Y;
            while ( true ) {
                Half_Edge he = tmpNode.currentEdge;

                accept = ( Up ) ? ( he.StartVertex.Y < Y_limit ) : ( he.StartVertex.Y > Y_limit );

                if ( accept ) {
                    Y_limit = he.StartVertex.Y;
                } else // if we fail the angle then break the search
                    break;

                // move to the next one 
                tmpNode = tmpNode.PrevNode;

                // check of we are in the start
                if ( tmpNode == _StartNode )
                    break;
            }

            #endregion


            // set the start node ( go one forward to get the correct node )
            startNode = tmpNode.NextNode;

        }


        /// <summary>
        /// Update the Boundary Max/Min
        /// </summary>
        /// <param name="tmpNode"></param>
        private void UpdateBoundaryMaxMin(BoundaryNode tmpNode)
        {
            // check for max/min
            if (tmpNode.currentEdge.StartVertex.X >= BoundaryMinMax.X_Max.currentEdge.StartVertex.X)
                BoundaryMinMax.X_Max = tmpNode;

            if (tmpNode.currentEdge.StartVertex.Y >= BoundaryMinMax.Y_Max.currentEdge.StartVertex.Y)
                BoundaryMinMax.Y_Max = tmpNode;

            if (tmpNode.currentEdge.StartVertex.X <= BoundaryMinMax.X_Min.currentEdge.StartVertex.X)
                BoundaryMinMax.X_Min = tmpNode;

            if (tmpNode.currentEdge.StartVertex.Y <= BoundaryMinMax.Y_Min.currentEdge.StartVertex.Y)
                BoundaryMinMax.Y_Min = tmpNode;
        }
        #endregion

    }
}
