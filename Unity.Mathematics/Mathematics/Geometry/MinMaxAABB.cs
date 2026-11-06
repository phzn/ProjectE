using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics.Geometry
{
	// Token: 0x0200004D RID: 77
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct MinMaxAABB : IEquatable<MinMaxAABB>
	{
		// Token: 0x0600249E RID: 9374 RVA: 0x00068A77 File Offset: 0x00066C77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MinMaxAABB(float3 min, float3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00068A87 File Offset: 0x00066C87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MinMaxAABB CreateFromCenterAndExtents(float3 center, float3 extents)
		{
			return MinMaxAABB.CreateFromCenterAndHalfExtents(center, extents * 0.5f);
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x00068A9A File Offset: 0x00066C9A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MinMaxAABB CreateFromCenterAndHalfExtents(float3 center, float3 halfExtents)
		{
			return new MinMaxAABB(center - halfExtents, center + halfExtents);
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x00068AAF File Offset: 0x00066CAF
		public float3 Extents
		{
			get
			{
				return this.Max - this.Min;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x00068AC2 File Offset: 0x00066CC2
		public float3 HalfExtents
		{
			get
			{
				return (this.Max - this.Min) * 0.5f;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x00068ADF File Offset: 0x00066CDF
		public float3 Center
		{
			get
			{
				return (this.Max + this.Min) * 0.5f;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x00068AFC File Offset: 0x00066CFC
		public bool IsValid
		{
			get
			{
				return math.all(this.Min <= this.Max);
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x00068B14 File Offset: 0x00066D14
		public float SurfaceArea
		{
			get
			{
				float3 @float = this.Max - this.Min;
				return 2f * math.dot(@float, @float.yzx);
			}
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x00068B46 File Offset: 0x00066D46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(float3 point)
		{
			return math.all((point >= this.Min) & (point <= this.Max));
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x00068B6A File Offset: 0x00066D6A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(MinMaxAABB aabb)
		{
			return math.all((this.Min <= aabb.Min) & (this.Max >= aabb.Max));
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x00068B98 File Offset: 0x00066D98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(MinMaxAABB aabb)
		{
			return math.all((this.Max >= aabb.Min) & (this.Min <= aabb.Max));
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x00068BC6 File Offset: 0x00066DC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Expand(float signedDistance)
		{
			this.Min -= signedDistance;
			this.Max += signedDistance;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x00068BEC File Offset: 0x00066DEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(MinMaxAABB aabb)
		{
			this.Min = math.min(this.Min, aabb.Min);
			this.Max = math.max(this.Max, aabb.Max);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x00068C1C File Offset: 0x00066E1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(float3 point)
		{
			this.Min = math.min(this.Min, point);
			this.Max = math.max(this.Max, point);
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x00068C42 File Offset: 0x00066E42
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(MinMaxAABB other)
		{
			return this.Min.Equals(other.Min) && this.Max.Equals(other.Max);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x00068C6A File Offset: 0x00066E6A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("MinMaxAABB({0}, {1})", this.Min, this.Max);
		}

		// Token: 0x0400012D RID: 301
		public float3 Min;

		// Token: 0x0400012E RID: 302
		public float3 Max;
	}
}
