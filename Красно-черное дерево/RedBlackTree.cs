using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Красно_черное_дерево
{
    public class RedBlackTree
    {
        private RBTreeNode root;
        private readonly RBTreeNode nullNode;

        public RedBlackTree()
        {
            nullNode = new RBTreeNode(null) { Color = Color.Black };
        }

        public void Insert(string value)
        {
            var node = new RBTreeNode(value);
            node.Left = nullNode;
            node.Right = nullNode;

            if (root == null)
            {
                root = node;
                root.Color = Color.Black;
                return;
            }

            RBTreeNode current = root;
            RBTreeNode parent = null;

            while (current != nullNode)
            {
                parent = current;
                if (string.Compare(node.Value, current.Value) < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            node.Parent = parent;
            if (string.Compare(node.Value, parent.Value) < 0)
                parent.Left = node;
            else
                parent.Right = node;

            FixInsert(node);
        }

        private void FixInsert(RBTreeNode node)
        {
            while (node != root && node.Parent.Color == Color.Red)
            {
                if (node.Parent == node.Parent.Parent.Left)
                {
                    var uncle = node.Parent.Parent.Right;
                    if (uncle.Color == Color.Red)
                    {
                        node.Parent.Color = Color.Black;
                        uncle.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Right)
                        {
                            node = node.Parent;
                            RotateLeft(node);
                        }
                        node.Parent.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        RotateRight(node.Parent.Parent);
                    }
                }
                else
                {
                    var uncle = node.Parent.Parent.Left;
                    if (uncle.Color == Color.Red)
                    {
                        node.Parent.Color = Color.Black;
                        uncle.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RotateRight(node);
                        }
                        node.Parent.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }
            root.Color = Color.Black;
        }

        public bool Search(string value)
        {
            var current = root;
            while (current != nullNode)
            {
                int cmp = string.Compare(value, current.Value);
                if (cmp == 0)
                    return true;
                else if (cmp < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }
            return false;
        }

        public void Delete(string value)
        {
            RBTreeNode node = root;
            while (node != nullNode && string.Compare(node.Value, value) != 0)
            {
                if (string.Compare(value, node.Value) < 0)
                    node = node.Left;
                else
                    node = node.Right;
            }

            if (node == nullNode)
                return;

            RBTreeNode x;
            Color originalColor = node.Color;
            RBTreeNode y = node;

            if (node.Left == nullNode)
            {
                x = node.Right;
                Transplant(node, node.Right);
            }
            else if (node.Right == nullNode)
            {
                x = node.Left;
                Transplant(node, node.Left);
            }
            else
            {
                y = Minimum(node.Right);
                originalColor = y.Color;
                x = y.Right;

                if (y.Parent == node)
                    x.Parent = y;
                else
                {
                    Transplant(y, y.Right);
                    y.Right = node.Right;
                    y.Right.Parent = y;
                }

                Transplant(node, y);
                y.Left = node.Left;
                y.Left.Parent = y;
                y.Color = node.Color;
            }

            if (originalColor == Color.Black)
                FixDelete(x);
        }

        private void FixDelete(RBTreeNode x)
        {
            while (x != root && x.Color == Color.Black)
            {
                if (x == x.Parent.Left)
                {
                    var w = x.Parent.Right;
                    if (w.Color == Color.Red)
                    {
                        w.Color = Color.Black;
                        x.Parent.Color = Color.Red;
                        RotateLeft(x.Parent);
                        w = x.Parent.Right;
                    }

                    if (w.Left.Color == Color.Black && w.Right.Color == Color.Black)
                    {
                        w.Color = Color.Red;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Right.Color == Color.Black)
                        {
                            w.Left.Color = Color.Black;
                            w.Color = Color.Red;
                            RotateRight(w);
                            w = x.Parent.Right;
                        }

                        w.Color = x.Parent.Color;
                        x.Parent.Color = Color.Black;
                        w.Right.Color = Color.Black;
                        RotateLeft(x.Parent);
                        x = root;
                    }
                }
                else
                {
                    var w = x.Parent.Left;
                    if (w.Color == Color.Red)
                    {
                        w.Color = Color.Black;
                        x.Parent.Color = Color.Red;
                        RotateRight(x.Parent);
                        w = x.Parent.Left;
                    }

                    if (w.Right.Color == Color.Black && w.Left.Color == Color.Black)
                    {
                        w.Color = Color.Red;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Left.Color == Color.Black)
                        {
                            w.Right.Color = Color.Black;
                            w.Color = Color.Red;
                            RotateLeft(w);
                            w = x.Parent.Left;
                        }

                        w.Color = x.Parent.Color;
                        x.Parent.Color = Color.Black;
                        w.Left.Color = Color.Black;
                        RotateRight(x.Parent);
                        x = root;
                    }
                }
            }
            x.Color = Color.Black;
        }

        private void Transplant(RBTreeNode u, RBTreeNode v)
        {
            if (u.Parent == null)
                root = v;
            else if (u == u.Parent.Left)
                u.Parent.Left = v;
            else
                u.Parent.Right = v;
            v.Parent = u.Parent;
        }

        private RBTreeNode Minimum(RBTreeNode node)
        {
            while (node.Left != nullNode)
                node = node.Left;
            return node;
        }

        private void RotateLeft(RBTreeNode x)
        {
            var y = x.Right;
            x.Right = y.Left;
            if (y.Left != nullNode)
                y.Left.Parent = x;
            y.Parent = x.Parent;

            if (x.Parent == null)
                root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }

        private void RotateRight(RBTreeNode x)
        {
            var y = x.Left;
            x.Left = y.Right;
            if (y.Right != nullNode)
                y.Right.Parent = x;
            y.Parent = x.Parent;

            if (x.Parent == null)
                root = y;
            else if (x == x.Parent.Right)
                x.Parent.Right = y;
            else
                x.Parent.Left = y;

            y.Right = x;
            x.Parent = y;
        }
    }
}
