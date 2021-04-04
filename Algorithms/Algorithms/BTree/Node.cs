using System;
using System.Collections.Generic;

namespace Algorithms.BTree
{
    public class Node<T>
    {
        public T Data { get; set; }
        public int KeysNumber { get; set; }

        public readonly List<int> Keys = new List<int>();
        public bool IsLeaf { get; set; }

        public readonly List<Node<T>> Children = new List<Node<T>>();

        public int Find(int k)
        {
            for (var i = 0; i < this.KeysNumber; i++)
            {
                if (this.Keys[i] == k)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}