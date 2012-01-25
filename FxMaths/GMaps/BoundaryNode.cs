using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.GMaps
{
    public class BoundaryNode
    {
        /// <summary>
        /// The half edge in this node
        /// </summary>
        public Half_Edge currentEdge;

        /// <summary>
        /// The next node in the boundary
        /// </summary>
        public BoundaryNode NextNode = null;

        /// <summary>
        /// The prev node of the boundary
        /// </summary>
        public BoundaryNode PrevNode = null;

        /// <summary>
        /// Var that set if we are the root
        /// </summary>
        public Boolean IsTheRoot;

        /// <summary>
        /// Create a new node
        /// </summary>
        /// <param name="item"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        public BoundaryNode( Half_Edge item, Boolean IsTheRoot = false )
        {
            // set the current item
            this.currentEdge = item;

            // set if this node is the root
            this.IsTheRoot = IsTheRoot;
        }

        /// <summary>
        /// insert node After ore before of this node
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="IsAfter"></param>
        public void InsertNode( BoundaryNode newNode, Boolean IsAfter )
        {
            if ( IsAfter ) {

                // set the next node to link to the new node
                NextNode.PrevNode = newNode;

                // set the new node to link to the next node
                newNode.NextNode = NextNode;

                // set this node to link to the new node
                this.NextNode = newNode;

                // link the new node to link to this node
                newNode.PrevNode = this;

            } else {

                // set the prev node to link to the new node
                PrevNode.NextNode = newNode;

                // set the new node to link to the old node
                newNode.PrevNode = PrevNode;

                // set this node to link to the new node
                this.PrevNode = newNode;

                // link the new node to this node
                newNode.NextNode = this;
            }
        }

        /// <summary>
        /// Remove the current node.
        /// if this node is the root then set the prev one as a root
        /// </summary>
        public void Remove()
        {
            // link the next one with the prev one
            this.PrevNode.NextNode = this.NextNode;
            this.NextNode.PrevNode = this.PrevNode;

            // clean the pointers
            this.NextNode = null;
            this.PrevNode = null;

            // if this node is the root then set the prev one as a root
         //   if ( this.IsTheRoot )
         //       this.NextNode.IsTheRoot = true;
        }
    }
}
