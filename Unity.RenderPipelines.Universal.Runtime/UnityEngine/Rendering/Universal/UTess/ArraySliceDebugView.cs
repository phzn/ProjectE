using System;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200012A RID: 298
	internal sealed class ArraySliceDebugView<T> where T : struct
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x000422BD File Offset: 0x000404BD
		public ArraySliceDebugView(ArraySlice<T> slice)
		{
			this.m_Slice = slice;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x000422CC File Offset: 0x000404CC
		public T[] Items
		{
			get
			{
				return this.m_Slice.ToArray();
			}
		}

		// Token: 0x040008D8 RID: 2264
		private ArraySlice<T> m_Slice;
	}
}
