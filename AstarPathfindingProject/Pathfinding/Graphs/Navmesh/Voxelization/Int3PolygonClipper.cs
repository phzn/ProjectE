using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Collections;

namespace Pathfinding.Graphs.Navmesh.Voxelization
{
	// Token: 0x020001D8 RID: 472
	internal struct Int3PolygonClipper
	{
		// Token: 0x06000D53 RID: 3411 RVA: 0x0004C79C File Offset: 0x0004A99C
		public unsafe int ClipPolygon(UnsafeSpan<Int3> vIn, int n, UnsafeSpan<Int3> vOut, int multi, int offset, int axis)
		{
			for (int i = 0; i < n; i++)
			{
				*((ref this.clipPolygonCache.FixedElementField) + (IntPtr)i * 4) = (float)(multi * vIn[i][axis] + offset);
			}
			int num = 0;
			int j = 0;
			int num2 = n - 1;
			while (j < n)
			{
				bool flag = *((ref this.clipPolygonCache.FixedElementField) + (IntPtr)num2 * 4) >= 0f;
				bool flag2 = *((ref this.clipPolygonCache.FixedElementField) + (IntPtr)j * 4) >= 0f;
				if (flag != flag2)
				{
					double num3 = (double)(*((ref this.clipPolygonCache.FixedElementField) + (IntPtr)num2 * 4)) / (double)(*((ref this.clipPolygonCache.FixedElementField) + (IntPtr)num2 * 4) - *((ref this.clipPolygonCache.FixedElementField) + (IntPtr)j * 4));
					*vOut[num] = *vIn[num2] + (*vIn[j] - *vIn[num2]) * num3;
					num++;
				}
				if (flag2)
				{
					*vOut[num] = *vIn[j];
					num++;
				}
				num2 = j;
				j++;
			}
			return num;
		}

		// Token: 0x040008E8 RID: 2280
		[FixedBuffer(typeof(float), 21)]
		private Int3PolygonClipper.<clipPolygonCache>e__FixedBuffer clipPolygonCache;

		// Token: 0x020001D9 RID: 473
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 84)]
		public struct <clipPolygonCache>e__FixedBuffer
		{
			// Token: 0x040008E9 RID: 2281
			public float FixedElementField;
		}
	}
}
