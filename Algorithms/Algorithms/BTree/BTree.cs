using System;

namespace Algorithms.BTree
{
    public class BTree<T>
    {
        private Node<T> Root { get; set; }

        private int Degree { get; set; }

        public BTree()
        { }

        public static BTree<T> BTreeCreate(int degree)
        {
            var allocatedNodeRoot = new Node<T> { IsLeaf = true, KeysNumber = 0};
            return new BTree<T> {Root = allocatedNodeRoot, Degree = degree};
        }

        private void BTreeSplitChild(Node<T> x, int i, Node<T> y)
        {
            var allocatedNode = new Node<T> {IsLeaf = y.IsLeaf, KeysNumber = Degree - 1};

            for (var j = 0; j < Degree - 1; j++)
            {
                allocatedNode.Children[j] = y.Children[j + Degree];
            }

            if (!y.IsLeaf)
            {
                for (var j = 0; j < Degree; j++)
                {
                    allocatedNode.Children[j] = y.Children[j + Degree];
                }
            }

            y.KeysNumber = Degree - 1;

            for (var j = x.KeysNumber; j >= i + 1; j--)
            {
                x.Children[j + 1] = x.Children[j];
            }

            x.Children[i + 1] = allocatedNode;

            for (var j = x.KeysNumber - 1; j >= i; j--)
            {
                x.Keys[j + 1] = x.Keys[j];
            }

            x.Keys[i] = y.Keys[Degree - 1];
            x.KeysNumber += 1;
        }

        private Node<T> BTreeSearch(Node<T> x, int key)
        {
            int i;
            if (x == null)
                return null;

            for (i = 0 ; i < x.KeysNumber; i++)
            {
                if(key < x.Keys[i])
                    break;

                if (key == x.Keys[i])
                    return x;
            }

            return x.IsLeaf ? null : BTreeSearch(x.Children[i], key);
        }

        private void BTreeSplit(Node<T> x, int position, Node<T> y)
        {
            var allocatedNode = new Node<T>();
            allocatedNode.IsLeaf = allocatedNode.IsLeaf;
            allocatedNode.KeysNumber = Degree - 1;

            for (var j = 0; j < Degree - 1; j++)
                allocatedNode.Keys[j] = y.Keys[j + Degree];
            
            if (!y.IsLeaf)
                for (var j = 0; j < Degree; j++)
                    x.Children[j + 1] = x.Children[j];

            x.Children[position + 1] = allocatedNode;

            for (var j = x.KeysNumber - 1; j >= position; j--)
                x.Children[j + 1] = x.Children[j];

            x.Children[position + 1] = allocatedNode;

            for (var j = x.KeysNumber - 1; j >= position; j--)
                x.Keys[j + 1] = x.Keys[j];

            x.Keys[position] = y.Keys[Degree - 1];
            x.KeysNumber += 1;
        }

        public void BTreeInsert(int key)
        {
            var r = Root;
            if (Root.KeysNumber == 2 * Degree - 1)
            {
                var allocatedNode = new Node<T>();
                r = allocatedNode;
                allocatedNode.IsLeaf = false;
                allocatedNode.KeysNumber = 0;
                allocatedNode.Children[0] = r;
                BTreeSplit(allocatedNode, 0, r);
                BTreeInsertValue(allocatedNode, key);
            }
            else
            {
                BTreeInsertValue(r, key);
            }
        }

        private void BTreeInsertValue(Node<T> x, int key)
        {
            if (x.IsLeaf)
            {
                var i = 0;
                for (i = x.KeysNumber - 1; i >= 0 && key < x.Keys[i]; i--)
                    x.Keys[i + 1] = x.Keys[i];

                x.Keys[i + 1] = key;
                x.KeysNumber += 1;
            }
            else
            {
                var i = 0;
                for(i = x.KeysNumber - 1; i >= 0 && key < x.Keys[i]; i--){}

                i++;
                var tmp = x.Children[i];
                if (tmp.KeysNumber == 2 * Degree - 1)
                {
                    BTreeSplit(x, i, tmp);
                    if (key > x.Keys[i])
                        i++;
                }
                BTreeInsertValue(x.Children[i], key);
            }
        }

        public void BTreeShow(Action<Node<T>> action)
        {
            action.Invoke(Root);
        }
    }
}