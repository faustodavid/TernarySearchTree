﻿using System;

namespace TernarySearchTree
{
    internal static class Argument
    {
        internal static void IsNotNullAndNotEmpty(string argument, string argumentName)
        {
            IsNotNull(argument, argumentName);
            IsNotEmpty(argument, argumentName);
        }

        internal static void IsNotNullAndNotEmpty(in ReadOnlySpan<char> argument, string argumentName)
        {
            IsNotNull(argument, argumentName);
            IsNotEmpty(argument, argumentName);
        }

        internal static void IsNotNullAndNotEmpty(in ReadOnlyMemory<char> argument, string argumentName)
        {
            IsNotNull(argument, argumentName);
            IsNotEmpty(argument, argumentName);
        }

        internal static void IsNotNull(in ReadOnlySpan<char> argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        internal static void IsNotNull<T>(T argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        internal static void IsNotEmpty(string argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentException("Argument cannot be an empty string.", argumentName);
            }
        }

        internal static void IsNotEmpty(in ReadOnlySpan<char> argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentException("Argument cannot be an empty string.", argumentName);
            }
        }

        internal static void IsNotEmpty(in ReadOnlyMemory<char> argument, string argumentName)
        {
            if (argument.IsEmpty)
            {
                throw new ArgumentException("Argument cannot be an empty string.", argumentName);
            }
        }

        internal static void IsWithinRange(bool expression, string argumentName)
        {
            if (expression == false)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }            
        }

        internal static void ThrowIfNullOrEmpty(in ReadOnlySpan<char> argument, string argumentName)
        {
            ThrowIfNull(argument, argumentName);
            ThrowIfEmpty(argument, argumentName);
        }

        internal static void ThrowIfNull(in ReadOnlySpan<char> argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        internal static void ThrowIfEmpty(in ReadOnlySpan<char> argument, string argumentName)
        {
            if (argument.Length == 0)
            {
                throw new ArgumentException("Argument cannot be an empty ReadOnlySpan<char>.", argumentName);
            }
        }
    }
}
