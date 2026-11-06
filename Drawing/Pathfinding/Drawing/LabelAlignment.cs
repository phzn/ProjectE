using System;
using Unity.Mathematics;

namespace Pathfinding.Drawing
{
	// Token: 0x02000008 RID: 8
	public struct LabelAlignment
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002138 File Offset: 0x00000338
		public LabelAlignment withPixelOffset(float x, float y)
		{
			return new LabelAlignment
			{
				relativePivot = this.relativePivot,
				pixelOffset = new float2(x, y)
			};
		}

		// Token: 0x04000007 RID: 7
		public float2 relativePivot;

		// Token: 0x04000008 RID: 8
		public float2 pixelOffset;

		// Token: 0x04000009 RID: 9
		public static readonly LabelAlignment TopLeft = new LabelAlignment
		{
			relativePivot = new float2(0f, 1f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x0400000A RID: 10
		public static readonly LabelAlignment MiddleLeft = new LabelAlignment
		{
			relativePivot = new float2(0f, 0.5f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x0400000B RID: 11
		public static readonly LabelAlignment BottomLeft = new LabelAlignment
		{
			relativePivot = new float2(0f, 0f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x0400000C RID: 12
		public static readonly LabelAlignment BottomCenter = new LabelAlignment
		{
			relativePivot = new float2(0.5f, 0f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x0400000D RID: 13
		public static readonly LabelAlignment BottomRight = new LabelAlignment
		{
			relativePivot = new float2(1f, 0f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x0400000E RID: 14
		public static readonly LabelAlignment MiddleRight = new LabelAlignment
		{
			relativePivot = new float2(1f, 0.5f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x0400000F RID: 15
		public static readonly LabelAlignment TopRight = new LabelAlignment
		{
			relativePivot = new float2(1f, 1f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x04000010 RID: 16
		public static readonly LabelAlignment TopCenter = new LabelAlignment
		{
			relativePivot = new float2(0.5f, 1f),
			pixelOffset = new float2(0f, 0f)
		};

		// Token: 0x04000011 RID: 17
		public static readonly LabelAlignment Center = new LabelAlignment
		{
			relativePivot = new float2(0.5f, 0.5f),
			pixelOffset = new float2(0f, 0f)
		};
	}
}
