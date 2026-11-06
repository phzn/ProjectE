using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace MagicaCloth2
{
	// Token: 0x0200011D RID: 285
	internal static class NativeReferenceExtensions
	{
		// Token: 0x060005C8 RID: 1480 RVA: 0x0002FE3C File Offset: 0x0002E03C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int MC2InterlockedStartIndex(this NativeReference<int> counter, int dataCount)
		{
			return Interlocked.Add(ref *counter.GetUnsafePtr<int>(), dataCount) - dataCount;
		}
	}
}
