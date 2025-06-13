using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Красно_черное_дерево
{
    public enum Color
    {
        Red,
        Black
    }
    public class RBTreeNode
    {
        public string Value;
        public Color Color;
        public RBTreeNode Left, Right, Parent;

        public RBTreeNode(string value)
        {
            Value = value;
            Color = Color.Red;
        }
    }
}
