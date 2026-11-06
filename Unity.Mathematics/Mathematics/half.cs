using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200002A RID: 42
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct half : IEquatable<half>, IFormattable
	{
		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x0003F587 File Offset: 0x0003D787
		public static float MaxValue
		{
			get
			{
				return 65504f;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x0003F58E File Offset: 0x0003D78E
		public static float MinValue
		{
			get
			{
				return -65504f;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060015E0 RID: 5600 RVA: 0x0003F595 File Offset: 0x0003D795
		public static half MaxValueAsHalf
		{
			get
			{
				return new half(half.MaxValue);
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x0003F5A1 File Offset: 0x0003D7A1
		public static half MinValueAsHalf
		{
			get
			{
				return new half(half.MinValue);
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0003F5AD File Offset: 0x0003D7AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half(half x)
		{
			this.value = x.value;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0003F5BB File Offset: 0x0003D7BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half(float v)
		{
			this.value = (ushort)math.f32tof16(v);
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0003F5CA File Offset: 0x0003D7CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half(double v)
		{
			this.value = (ushort)math.f32tof16((float)v);
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0003F5DA File Offset: 0x0003D7DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half(float v)
		{
			return new half(v);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0003F5E2 File Offset: 0x0003D7E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half(double v)
		{
			return new half(v);
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0003F5EA File Offset: 0x0003D7EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float(half d)
		{
			return math.f16tof32((uint)d.value);
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0003F5F7 File Offset: 0x0003D7F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double(half d)
		{
			return (double)math.f16tof32((uint)d.value);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x0003F605 File Offset: 0x0003D805
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(half lhs, half rhs)
		{
			return lhs.value == rhs.value;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0003F615 File Offset: 0x0003D815
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(half lhs, half rhs)
		{
			return lhs.value != rhs.value;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0003F628 File Offset: 0x0003D828
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(half rhs)
		{
			return this.value == rhs.value;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x0003F638 File Offset: 0x0003D838
		public override bool Equals(object o)
		{
			if (o is half)
			{
				half half = (half)o;
				return this.Equals(half);
			}
			return false;
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0003F65D File Offset: 0x0003D85D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)this.value;
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0003F668 File Offset: 0x0003D868
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return math.f16tof32((uint)this.value).ToString();
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0003F688 File Offset: 0x0003D888
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return math.f16tof32((uint)this.value).ToString(format, formatProvider);
		}

		// Token: 0x040000AF RID: 175
		public ushort value;

		// Token: 0x040000B0 RID: 176
		public static readonly half zero;
	}
}
