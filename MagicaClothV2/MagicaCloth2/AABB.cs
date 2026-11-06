using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x02000102 RID: 258
	[Serializable]
	public struct AABB : IEquatable<AABB>
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x0002B32B File Offset: 0x0002952B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AABB(in float3 min, in float3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0002B345 File Offset: 0x00029545
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AABB CreateFromCenterAndExtents(float3 center, float3 extents)
		{
			return AABB.CreateFromCenterAndHalfExtents(center, extents * 0.5f);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0002B358 File Offset: 0x00029558
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AABB CreateFromCenterAndHalfExtents(float3 center, float3 halfExtents)
		{
			float3 @float = center - halfExtents;
			float3 float2 = center + halfExtents;
			return new AABB(in @float, in float2);
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0002B37E File Offset: 0x0002957E
		public float3 Extents
		{
			get
			{
				return this.Max - this.Min;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0002B391 File Offset: 0x00029591
		public float3 HalfExtents
		{
			get
			{
				return (this.Max - this.Min) * 0.5f;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0002B3AE File Offset: 0x000295AE
		public float3 Center
		{
			get
			{
				return (this.Max + this.Min) * 0.5f;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0002B3CC File Offset: 0x000295CC
		public float MaxSideLength
		{
			get
			{
				float3 extents = this.Extents;
				return math.max(math.max(extents.x, extents.y), extents.z);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0002B3FC File Offset: 0x000295FC
		public bool IsValid
		{
			get
			{
				return math.all(this.Min <= this.Max);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0002B414 File Offset: 0x00029614
		public float SurfaceArea
		{
			get
			{
				float3 @float = this.Max - this.Min;
				return 2f * math.dot(@float, @float.yzx);
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0002B446 File Offset: 0x00029646
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(in float3 point)
		{
			return math.all((point >= this.Min) & (point <= this.Max));
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0002B474 File Offset: 0x00029674
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(in AABB aabb)
		{
			return math.all((this.Min <= aabb.Min) & (this.Max >= aabb.Max));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0002B4A2 File Offset: 0x000296A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(in AABB aabb)
		{
			return math.all((this.Max >= aabb.Min) & (this.Min <= aabb.Max));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0002B4D0 File Offset: 0x000296D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Expand(float signedDistance)
		{
			this.Min -= signedDistance;
			this.Max += signedDistance;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0002B4F6 File Offset: 0x000296F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(in AABB aabb)
		{
			this.Min = math.min(this.Min, aabb.Min);
			this.Max = math.max(this.Max, aabb.Max);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0002B526 File Offset: 0x00029726
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(in float3 point)
		{
			this.Min = math.min(this.Min, point);
			this.Max = math.max(this.Max, point);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0002B556 File Offset: 0x00029756
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(AABB other)
		{
			return this.Min.Equals(other.Min) && this.Max.Equals(other.Max);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0002B580 File Offset: 0x00029780
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Transform(in float4x4 toM)
		{
			float3 @float = math.transform(toM, this.Min);
			float3 float2 = math.transform(toM, this.Max);
			this.Min = math.min(@float, float2);
			this.Max = math.max(@float, float2);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0002B5CC File Offset: 0x000297CC
		public override string ToString()
		{
			return string.Format("AABB Center:{0}, HalfExtents:{1}, Min:{2}, Max:{3}", new object[] { this.Center, this.HalfExtents, this.Min, this.Max });
		}

		// Token: 0x04000893 RID: 2195
		public float3 Min;

		// Token: 0x04000894 RID: 2196
		public float3 Max;
	}
}
