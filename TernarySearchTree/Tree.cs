﻿using System;
using System.Collections.Generic;

namespace TernarySearchTree
{
    internal static class Tree
    {
        internal static void GetSiblings<TValue>(Node<TValue> node, IList<Node<TValue>> siblings)
        {
            if (node == null)
            {
                return;
            }

            siblings.Add(node);

            GetSiblings(node.LowerNode, siblings);
            GetSiblings(node.HigherNode, siblings);
        }

        internal static IEnumerable<Node<TValue>> GetEqualNodes<TValue>(Node<TValue> node)
        {
            foreach (var n in GetAllNodes(node))
            {
                if (n.EqualNode != null)
                {
                    yield return node;
                }
            }
        }

        internal static Node<TValue> OptimizeEqualNode<TValue>(Node<TValue> equalNode)
        {
            var siblings = new List<Node<TValue>>();
            GetSiblings(equalNode, siblings);
            siblings.Sort(new SplitCharacterComparer<TValue>());

            return OptimizeSiblings(siblings, 0, siblings.Count);
        }

        internal static Node<TValue> OptimizeSiblings<TValue>(IList<Node<TValue>> nodes, int index, int count)
        {
            if (count == 0)
            {
                return null;
            }

            if (count == 1)
            {
                var leafNode = nodes[index];
                leafNode.LowerNode = null;
                leafNode.HigherNode = null;
                return leafNode;
            }

            var middleNode = nodes[index + count / 2];
            middleNode.LowerNode = OptimizeSiblings<TValue>(nodes, index, count / 2);
            middleNode.HigherNode = OptimizeSiblings<TValue>(nodes, index + count / 2 + 1, (count - 1) / 2);
            return middleNode;
        }

        internal static Node<TValue> GetNodeWithValue<TValue>(Node<TValue> node, ReadOnlySpan<char> key)
        {
            var currentKeyCharacterIndex = 0;
            var currentKeyCharacter = key[currentKeyCharacterIndex];

            while (node != null)
            {
                if (currentKeyCharacter < node.SplitCharacter)
                {
                    node = node.LowerNode;
                }
                else if (currentKeyCharacter > node.SplitCharacter)
                {
                    node = node.HigherNode;
                }
                else
                {
                    if (++currentKeyCharacterIndex == key.Length)
                    {
                        return node.HasValue ? node : null;
                    }

                    currentKeyCharacter = key[currentKeyCharacterIndex];

                    node = node.EqualNode;
                }
            }

            return null;
        }

        internal static Node<TValue> GetNode<TValue>(Node<TValue> node, string key)
        {
            // Setup current key index and character.
            var currentKeyCharacterIndex = 0;
            var currentKeyCharacter = key[currentKeyCharacterIndex];

            // Loop while we have a node reference.
            while (node != null)
            {
                if (currentKeyCharacter < node.SplitCharacter)
                {
                    // Set current node to lower node.
                    node = node.LowerNode;
                }
                else if (currentKeyCharacter > node.SplitCharacter)
                {
                    // Set current node to higher node.
                    node = node.HigherNode;
                }
                else
                {
                    // If we have processed the whole key, return node information if it has a value.
                    if (++currentKeyCharacterIndex == key.Length)
                    {
                        return node;
                    }

                    // Advance to next split character.
                    currentKeyCharacter = key[currentKeyCharacterIndex];

                    // Set current node to equal node.
                    node = node.EqualNode;
                }
            }

            // If we get here there was no nodes to traverse, the key could not be found.
            return null;
        }

        internal static Node<TValue> CreateNodes<TValue>(ref Node<TValue> root, ReadOnlySpan<char> key)
        {
            var currentKeyCharacterIndex = 0;
            var currentKeyCharacter = key[currentKeyCharacterIndex];

            if (root == null)
            {
                root = new Node<TValue>(currentKeyCharacter);
            }

            var node = root;

            for (;;)
            {
                if (currentKeyCharacter < node.SplitCharacter)
                {
                    if (node.LowerNode == null)
                    {
                        node.LowerNode = new Node<TValue>(currentKeyCharacter);
                    }

                    node = node.LowerNode;
                }
                else if (currentKeyCharacter > node.SplitCharacter)
                {
                    if (node.HigherNode == null)
                    {
                        node.HigherNode = new Node<TValue>(currentKeyCharacter);
                    }

                    node = node.HigherNode;
                }
                else if (++currentKeyCharacterIndex < key.Length)
                {
                    currentKeyCharacter = key[currentKeyCharacterIndex];

                    if (node.EqualNode == null)
                    {
                        node.EqualNode = new Node<TValue>(currentKeyCharacter);
                    }

                    node = node.EqualNode;
                }
                else
                {
                    return node;
                }
            }
        }

        internal static IEnumerable<string> GetAllKeys<TValue>(Node<TValue> node, string key)
        {
            if (node == null)
            {
                yield break;
            }

            if (node.HasValue)
            {
                yield return key + node.SplitCharacter;
            }

            foreach (var lowerValueKey in GetAllKeys(node.LowerNode, key))
            {
                yield return lowerValueKey;
            }

            foreach (var equalValueKey in GetAllKeys(node.EqualNode, key + node.SplitCharacter))
            {
                yield return equalValueKey;
            }

            foreach (var higherValueKey in GetAllKeys(node.HigherNode, key))
            {
                yield return higherValueKey;
            }
        }

        internal static IEnumerable<TValue> GetAllValues<TValue>(Node<TValue> node)
        {
            if (node == null)
            {
                yield break;
            }

            if (node.HasValue)
            {
                yield return node.Value;
            }

            foreach (var lowerValue in GetAllValues(node.LowerNode))
            {
                yield return lowerValue;
            }

            foreach (var equalValue in GetAllValues(node.EqualNode))
            {
                yield return equalValue;
            }

            foreach (var higherValue in GetAllValues(node.HigherNode))
            {
                yield return higherValue;
            }
        }

        internal static IEnumerable<Node<TValue>> GetAllNodes<TValue>(Node<TValue> node)
        {
            if (node == null)
            {
                yield break;
            }

            yield return node;

            foreach (var lowerNode in GetAllNodes(node.LowerNode))
            {
                yield return lowerNode;
            }

            foreach (var equalNode in GetAllNodes(node.EqualNode))
            {
                yield return equalNode;
            }

            foreach (var higherNode in GetAllNodes(node.HigherNode))
            {
                yield return higherNode;
            }
        }

        internal static IEnumerable<KeyValuePair<string, TValue>> GetAllKeyValuePairs<TValue>(Node<TValue> node, string key)
        {
            if (node == null)
            {
                yield break;
            }

            if (node.HasValue)
            {
                yield return new KeyValuePair<string, TValue>(key + node.SplitCharacter, node.Value);
            }

            foreach (var lowerKeyValuePair in GetAllKeyValuePairs(node.LowerNode, key))
            {
                yield return lowerKeyValuePair;
            }

            foreach (var equalKeyValuePair in GetAllKeyValuePairs(node.EqualNode, key + node.SplitCharacter))
            {
                yield return equalKeyValuePair;
            }

            foreach (var higherKeyValuePair in GetAllKeyValuePairs(node.HigherNode, key))
            {
                yield return higherKeyValuePair;
            }
        }

        internal static bool RemoveNode<TValue>(Node<TValue> node, string key, int keyIndex)
        {
            var currentKeyCharacter = key[keyIndex];

            if (currentKeyCharacter < node.SplitCharacter)
            {
                if (RemoveNode(node.LowerNode, key, keyIndex))
                {
                    node.LowerNode = null;
                    return node.CanBeRemoved;
                }

                if (node.LowerNode.CanBeSimplified)
                {
                    node.LowerNode = node.LowerNode.LowerNode ?? node.LowerNode.HigherNode;
                }
            }

            if (currentKeyCharacter > node.SplitCharacter)
            {
                if (RemoveNode(node.HigherNode, key, keyIndex))
                {
                    node.HigherNode = null;
                    return node.CanBeRemoved;
                }

                if (node.HigherNode.CanBeSimplified)
                {
                    node.HigherNode = node.HigherNode.HigherNode ?? node.HigherNode.LowerNode;
                }
            }

            if (keyIndex < key.Length - 1 && currentKeyCharacter == node.SplitCharacter && RemoveNode(node.EqualNode, key, keyIndex + 1))
            {
                node.EqualNode = null;
                return node.CanBeRemoved;
            }

            if (keyIndex == key.Length - 1 && currentKeyCharacter == node.SplitCharacter)
            {
                node.ClearValue();
            }

            return node.CanBeRemoved;
        }
    }
}
