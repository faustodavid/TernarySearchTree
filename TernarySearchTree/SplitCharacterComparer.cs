﻿using System.Collections.Generic;

namespace TernarySearchTree
{
    internal struct SplitCharacterComparer<TValue> : IComparer<Node<TValue>>
    {
        public int Compare(Node<TValue> x, Node<TValue> y)
        {
            return x.SplitCharacter.CompareTo(y.SplitCharacter);
        }
    }
}
