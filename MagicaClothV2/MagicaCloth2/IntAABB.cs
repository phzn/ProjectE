using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x02000103 RID: 259
	[Serializable]
	public struct IntAABB : IEquatable<IntAABB>
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x0002B621 File Offset: 0x00029821
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntAABB(int3 min, int3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0002B631 File Offset: 0x00029831
		public int3 Extents
		{
			get
			{
				return this.Max - this.Min;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0002B644 File Offset: 0x00029844
		public int3 Center
		{
			get
			{
				return (this.Max + this.Min) / 2;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0002B65D File Offset: 0x0002985D
		public bool IsValid
		{
			get
			{
				return math.all(this.Min <= this.Max);
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0002B675 File Offset: 0x00029875
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(int3 point)
		{
			return math.all((point >= this.Min) & (point <= this.Max));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0002B699 File Offset: 0x00029899
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(IntAABB aabb)
		{
			return math.all((this.Min <= aabb.Min) & (this.Max >= aabb.Max));
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0002B6C7 File Offset: 0x000298C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Overlaps(IntAABB aabb)
		{
			return math.all((this.Max >= aabb.Min) & (this.Min <= aabb.Max));
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0002B6F5 File Offset: 0x000298F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Expand(int signedDistance)
		{
			this.Min -= signedDistance;
			this.Max += signedDistance;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0002B71B File Offset: 0x0002991B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(IntAABB aabb)
		{
			this.Min = math.min(this.Min, aabb.Min);
			this.Max = math.max(this.Max, aabb.Max);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0002B74B File Offset: 0x0002994B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(int3 point)
		{
			this.Min = math.min(this.Min, point);
			this.Max = math.max(this.Max, point);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0002B771 File Offset: 0x00029971
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(IntAABB other)
		{
			return this.Min.Equals(other.Min) && this.Max.Equals(other.Max);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0002B799 File Offset: 0x00029999
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("AABB({0}, {1})", this.Min, this.Max);
		}

		// Token: 0x04000895 RID: 2197
		public int3 Min;

		// Token: 0x04000896 RID: 2198
		public int3 Max;
	}
}
