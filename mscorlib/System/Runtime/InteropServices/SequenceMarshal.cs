using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006CA RID: 1738
	public static class SequenceMarshal
	{
		// Token: 0x06003FFD RID: 16381 RVA: 0x000E049F File Offset: 0x000DE69F
		public static bool TryGetReadOnlySequenceSegment<T>(ReadOnlySequence<T> sequence, out ReadOnlySequenceSegment<T> startSegment, out int startIndex, out ReadOnlySequenceSegment<T> endSegment, out int endIndex)
		{
			return sequence.TryGetReadOnlySequenceSegment(out startSegment, out startIndex, out endSegment, out endIndex);
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x000E04AD File Offset: 0x000DE6AD
		public static bool TryGetArray<T>(ReadOnlySequence<T> sequence, out ArraySegment<T> segment)
		{
			return sequence.TryGetArray(out segment);
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x000E04B7 File Offset: 0x000DE6B7
		public static bool TryGetReadOnlyMemory<T>(ReadOnlySequence<T> sequence, out ReadOnlyMemory<T> memory)
		{
			if (!sequence.IsSingleSegment)
			{
				memory = default(ReadOnlyMemory<T>);
				return false;
			}
			memory = sequence.First;
			return true;
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x000E04D9 File Offset: 0x000DE6D9
		internal static bool TryGetString(ReadOnlySequence<char> sequence, out string text, out int start, out int length)
		{
			return sequence.TryGetString(out text, out start, out length);
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x000E04E5 File Offset: 0x000DE6E5
		public static bool TryRead<[IsUnmanaged] T>(ref SequenceReader<byte> reader, out T value) where T : struct, ValueType
		{
			return (ref reader).TryRead<T>(out value);
		}
	}
}
