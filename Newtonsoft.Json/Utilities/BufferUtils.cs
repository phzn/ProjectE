using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005C RID: 92
	[NullableContext(2)]
	[Nullable(0)]
	internal static class BufferUtils
	{
		// Token: 0x06000534 RID: 1332 RVA: 0x00015D7F File Offset: 0x00013F7F
		[NullableContext(1)]
		public static char[] RentBuffer([Nullable(2)] IArrayPool<char> bufferPool, int minSize)
		{
			if (bufferPool == null)
			{
				return new char[minSize];
			}
			return bufferPool.Rent(minSize);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00015D92 File Offset: 0x00013F92
		public static void ReturnBuffer(IArrayPool<char> bufferPool, char[] buffer)
		{
			if (bufferPool != null)
			{
				bufferPool.Return(buffer);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00015D9E File Offset: 0x00013F9E
		[return: Nullable(1)]
		public static char[] EnsureBufferSize(IArrayPool<char> bufferPool, int size, char[] buffer)
		{
			if (bufferPool == null)
			{
				return new char[size];
			}
			if (buffer != null)
			{
				bufferPool.Return(buffer);
			}
			return bufferPool.Rent(size);
		}
	}
}
