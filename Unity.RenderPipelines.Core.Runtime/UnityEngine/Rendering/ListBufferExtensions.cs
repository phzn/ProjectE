using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering
{
	// Token: 0x0200004B RID: 75
	public static class ListBufferExtensions
	{
		// Token: 0x0600028D RID: 653 RVA: 0x0000C302 File Offset: 0x0000A502
		public unsafe static void QuickSort<[IsUnmanaged] T>(this ListBuffer<T> self) where T : struct, ValueType, IComparable<T>
		{
			CoreUnsafeUtils.QuickSort<int>(self.Count, (void*)self.BufferPtr);
		}
	}
}
