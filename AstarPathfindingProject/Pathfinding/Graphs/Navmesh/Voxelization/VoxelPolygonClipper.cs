using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization
{
	// Token: 0x020001DA RID: 474
	internal struct VoxelPolygonClipper
	{
		// Token: 0x1700020E RID: 526
		public unsafe Vector3 this[int i]
		{
			set
			{
				*((ref this.x.FixedElementField) + (IntPtr)i * 4) = value.x;
				*((ref this.y.FixedElementField) + (IntPtr)i * 4) = value.y;
				*((ref this.z.FixedElementField) + (IntPtr)i * 4) = value.z;
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0004C930 File Offset: 0x0004AB30
		public unsafe void ClipPolygonAlongX([NoAlias] ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			float num2 = multi * *((ref this.x.FixedElementField) + (IntPtr)(this.n - 1) * 4) + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * *((ref this.x.FixedElementField) + (IntPtr)i * 4) + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					*((ref result.x.FixedElementField) + (IntPtr)num * 4) = *((ref this.x.FixedElementField) + (IntPtr)num3 * 4) + (*((ref this.x.FixedElementField) + (IntPtr)i * 4) - *((ref this.x.FixedElementField) + (IntPtr)num3 * 4)) * num5;
					*((ref result.y.FixedElementField) + (IntPtr)num * 4) = *((ref this.y.FixedElementField) + (IntPtr)num3 * 4) + (*((ref this.y.FixedElementField) + (IntPtr)i * 4) - *((ref this.y.FixedElementField) + (IntPtr)num3 * 4)) * num5;
					*((ref result.z.FixedElementField) + (IntPtr)num * 4) = *((ref this.z.FixedElementField) + (IntPtr)num3 * 4) + (*((ref this.z.FixedElementField) + (IntPtr)i * 4) - *((ref this.z.FixedElementField) + (IntPtr)num3 * 4)) * num5;
					num++;
				}
				if (flag2)
				{
					*((ref result.x.FixedElementField) + (IntPtr)num * 4) = *((ref this.x.FixedElementField) + (IntPtr)i * 4);
					*((ref result.y.FixedElementField) + (IntPtr)num * 4) = *((ref this.y.FixedElementField) + (IntPtr)i * 4);
					*((ref result.z.FixedElementField) + (IntPtr)num * 4) = *((ref this.z.FixedElementField) + (IntPtr)i * 4);
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0004CB14 File Offset: 0x0004AD14
		public unsafe void ClipPolygonAlongZWithYZ([NoAlias] ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			Hint.Assume(this.n >= 0);
			Hint.Assume(this.n <= 8);
			float num2 = multi * *((ref this.z.FixedElementField) + (IntPtr)(this.n - 1) * 4) + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * *((ref this.z.FixedElementField) + (IntPtr)i * 4) + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					*((ref result.y.FixedElementField) + (IntPtr)num * 4) = *((ref this.y.FixedElementField) + (IntPtr)num3 * 4) + (*((ref this.y.FixedElementField) + (IntPtr)i * 4) - *((ref this.y.FixedElementField) + (IntPtr)num3 * 4)) * num5;
					*((ref result.z.FixedElementField) + (IntPtr)num * 4) = *((ref this.z.FixedElementField) + (IntPtr)num3 * 4) + (*((ref this.z.FixedElementField) + (IntPtr)i * 4) - *((ref this.z.FixedElementField) + (IntPtr)num3 * 4)) * num5;
					num++;
				}
				if (flag2)
				{
					*((ref result.y.FixedElementField) + (IntPtr)num * 4) = *((ref this.y.FixedElementField) + (IntPtr)i * 4);
					*((ref result.z.FixedElementField) + (IntPtr)num * 4) = *((ref this.z.FixedElementField) + (IntPtr)i * 4);
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0004CCB0 File Offset: 0x0004AEB0
		public unsafe void ClipPolygonAlongZWithY([NoAlias] ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			Hint.Assume(this.n >= 3);
			Hint.Assume(this.n <= 8);
			float num2 = multi * *((ref this.z.FixedElementField) + (IntPtr)(this.n - 1) * 4) + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * *((ref this.z.FixedElementField) + (IntPtr)i * 4) + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					*((ref result.y.FixedElementField) + (IntPtr)num * 4) = *((ref this.y.FixedElementField) + (IntPtr)num3 * 4) + (*((ref this.y.FixedElementField) + (IntPtr)i * 4) - *((ref this.y.FixedElementField) + (IntPtr)num3 * 4)) * num5;
					num++;
				}
				if (flag2)
				{
					*((ref result.y.FixedElementField) + (IntPtr)num * 4) = *((ref this.y.FixedElementField) + (IntPtr)i * 4);
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x040008EA RID: 2282
		[FixedBuffer(typeof(float), 8)]
		public VoxelPolygonClipper.<x>e__FixedBuffer x;

		// Token: 0x040008EB RID: 2283
		[FixedBuffer(typeof(float), 8)]
		public VoxelPolygonClipper.<y>e__FixedBuffer y;

		// Token: 0x040008EC RID: 2284
		[FixedBuffer(typeof(float), 8)]
		public VoxelPolygonClipper.<z>e__FixedBuffer z;

		// Token: 0x040008ED RID: 2285
		public int n;

		// Token: 0x020001DB RID: 475
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct <x>e__FixedBuffer
		{
			// Token: 0x040008EE RID: 2286
			public float FixedElementField;
		}

		// Token: 0x020001DC RID: 476
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct <y>e__FixedBuffer
		{
			// Token: 0x040008EF RID: 2287
			public float FixedElementField;
		}

		// Token: 0x020001DD RID: 477
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct <z>e__FixedBuffer
		{
			// Token: 0x040008F0 RID: 2288
			public float FixedElementField;
		}
	}
}
