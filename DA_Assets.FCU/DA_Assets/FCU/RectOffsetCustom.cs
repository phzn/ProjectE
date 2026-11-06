using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000041 RID: 65
	public struct RectOffsetCustom
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00003DD6 File Offset: 0x00001FD6
		public RectOffsetCustom(int left, int right, int top, int bottom)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00003DF5 File Offset: 0x00001FF5
		public RectOffset ToRectOffset()
		{
			return new RectOffset(this.left, this.right, this.top, this.bottom);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00003E14 File Offset: 0x00002014
		public override string ToString()
		{
			return string.Format("RectOffsetCustom(Left: {0}, Right: {1}, Top: {2}, Bottom: {3})", new object[] { this.left, this.right, this.top, this.bottom });
		}

		// Token: 0x04000355 RID: 853
		public int left;

		// Token: 0x04000356 RID: 854
		public int right;

		// Token: 0x04000357 RID: 855
		public int top;

		// Token: 0x04000358 RID: 856
		public int bottom;
	}
}
