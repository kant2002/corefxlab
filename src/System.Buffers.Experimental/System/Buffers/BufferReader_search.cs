﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    // TODO: the TryReadUntill methods are very inneficient. We need to fix that.
    public static partial class BufferReaderExtensions
    {
        public static bool TryReadUntill(ref BufferReader<ReadOnlyBytes> reader, out ReadOnlyBytes bytes, byte delimiter)
        {
            var copy = reader;
            var start = reader.Position;
            while (!reader.End) {
                Position end = reader.Position;
                if(reader.Take() == delimiter)
                {
                    bytes = new ReadOnlyBytes(start, end);
                    return true;
                }
            }
            reader = copy;
            bytes = default;
            return false;
        }

        public static bool TryReadUntill(ref BufferReader<ReadOnlyBytes> reader, out ReadOnlyBytes bytes, ReadOnlySpan<byte> delimiter)
        {
            if (delimiter.Length == 0)
            {
                bytes = ReadOnlyBytes.Empty;
                return true;
            }

            int matched = 0;
            var copy = reader;
            var start = reader.Position;
            var end = reader.Position;
            while (!reader.End)
            {
                if (reader.Take() == delimiter[matched]) {
                    matched++;
                }
                else
                {
                    end = reader.Position;
                    matched = 0;
                }
                if(matched >= delimiter.Length)
                {
                    bytes = new ReadOnlyBytes(start, end);
                    return true;
                }
            }
            reader = copy;
            bytes = default;
            return false;
        }
    }
}
