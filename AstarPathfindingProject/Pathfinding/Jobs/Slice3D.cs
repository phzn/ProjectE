using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Jobs
{
	// Token: 0x02000176 RID: 374
	public readonly struct Slice3D
	{
		// Token: 0x06000BA9 RID: 2985 RVA: 0x00040DD8 File Offset: 0x0003EFD8
		public Slice3D(IntBounds outer, IntBounds slice)
		{
			this = new Slice3D(outer.size, slice.Offset(-outer.min));
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00040DF9 File Offset: 0x0003EFF9
		public Slice3D(int3 outerSize, IntBounds slice)
		{
			this.outerSize = outerSize;
			this.slice = slice;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00003788 File Offset: 0x00001988
		public void AssertMatchesOuter<[IsUnmanaged] T>(UnsafeSpan<T> values) where T : struct, ValueType
		{
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00003788 File Offset: 0x00001988
		public void AssertMatchesOuter<T>(NativeArray<T> values) where T : struct
		{
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00003788 File Offset: 0x00001988
		public void AssertMatchesInner<T>(NativeArray<T> values) where T : struct
		{
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00003788 File Offset: 0x00001988
		public void AssertSameSize(Slice3D other)
		{
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00040E0C File Offset: 0x0003F00C
		public int InnerCoordinateToOuterIndex(int x, int y, int z)
		{
			ValueTuple<int, int, int> outerStrides = this.outerStrides;
			int item = outerStrides.Item1;
			int item2 = outerStrides.Item2;
			int item3 = outerStrides.Item3;
			return (x + this.slice.min.x) * item + (y + this.slice.min.y) * item2 + (z + this.slice.min.z) * item3;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00040E74 File Offset: 0x0003F074
		public int length
		{
			get
			{
				return this.slice.size.x * this.slice.size.y * this.slice.size.z;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00040EBC File Offset: 0x0003F0BC
		public ValueTuple<int, int, int> outerStrides
		{
			get
			{
				return new ValueTuple<int, int, int>(1, this.outerSize.x * this.outerSize.z, this.outerSize.x);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00040EE8 File Offset: 0x0003F0E8
		public ValueTuple<int, int, int> innerStrides
		{
			get
			{
				return new ValueTuple<int, int, int>(1, this.slice.size.x * this.slice.size.z, this.slice.size.x);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00040F38 File Offset: 0x0003F138
		public int outerStartIndex
		{
			get
			{
				ValueTuple<int, int, int> outerStrides = this.outerStrides;
				int item = outerStrides.Item1;
				int item2 = outerStrides.Item2;
				int item3 = outerStrides.Item3;
				return this.slice.min.x * item + this.slice.min.y * item2 + this.slice.min.z * item3;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00040F98 File Offset: 0x0003F198
		public bool coversEverything
		{
			get
			{
				return math.all(this.slice.size == this.outerSize);
			}
		}

		// Token: 0x040007A0 RID: 1952
		public readonly int3 outerSize;

		// Token: 0x040007A1 RID: 1953
		public readonly IntBounds slice;
	}
}
