using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000061 RID: 97
	public struct Int3 : IEquatable<Int3>
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000114C4 File Offset: 0x0000F6C4
		public static Int3 zero
		{
			get
			{
				return default(Int3);
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000114DC File Offset: 0x0000F6DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int3(Vector3 position)
		{
			this.x = (int)Math.Round((double)(position.x * 1000f));
			this.y = (int)Math.Round((double)(position.y * 1000f));
			this.z = (int)Math.Round((double)(position.z * 1000f));
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00011534 File Offset: 0x0000F734
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int3(int _x, int _y, int _z)
		{
			this.x = _x;
			this.y = _y;
			this.z = _z;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001154B File Offset: 0x0000F74B
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Int3 lhs, Int3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00011579 File Offset: 0x0000F779
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Int3 lhs, Int3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000115AA File Offset: 0x0000F7AA
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Int3(Vector3 ob)
		{
			return new Int3((int)Math.Round((double)(ob.x * 1000f)), (int)Math.Round((double)(ob.y * 1000f)), (int)Math.Round((double)(ob.z * 1000f)));
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000115EA File Offset: 0x0000F7EA
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3(Int3 ob)
		{
			return new Vector3((float)ob.x * 0.001f, (float)ob.y * 0.001f, (float)ob.z * 0.001f);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00011618 File Offset: 0x0000F818
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3(Int3 ob)
		{
			return (int3)ob * 0.001f;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001162F File Offset: 0x0000F82F
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(Int3 ob)
		{
			return new int3(ob.x, ob.y, ob.z);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00011648 File Offset: 0x0000F848
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator -(Int3 lhs, Int3 rhs)
		{
			lhs.x -= rhs.x;
			lhs.y -= rhs.y;
			lhs.z -= rhs.z;
			return lhs;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001167E File Offset: 0x0000F87E
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator -(Int3 lhs)
		{
			lhs.x = -lhs.x;
			lhs.y = -lhs.y;
			lhs.z = -lhs.z;
			return lhs;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000116AB File Offset: 0x0000F8AB
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator +(Int3 lhs, Int3 rhs)
		{
			lhs.x += rhs.x;
			lhs.y += rhs.y;
			lhs.z += rhs.z;
			return lhs;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000116E1 File Offset: 0x0000F8E1
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator *(Int3 lhs, int rhs)
		{
			lhs.x *= rhs;
			lhs.y *= rhs;
			lhs.z *= rhs;
			return lhs;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00011708 File Offset: 0x0000F908
		public static Int3 operator *(Int3 lhs, float rhs)
		{
			lhs.x = (int)Math.Round((double)((float)lhs.x * rhs));
			lhs.y = (int)Math.Round((double)((float)lhs.y * rhs));
			lhs.z = (int)Math.Round((double)((float)lhs.z * rhs));
			return lhs;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001175C File Offset: 0x0000F95C
		public static Int3 operator *(Int3 lhs, double rhs)
		{
			lhs.x = (int)Math.Round((double)lhs.x * rhs);
			lhs.y = (int)Math.Round((double)lhs.y * rhs);
			lhs.z = (int)Math.Round((double)lhs.z * rhs);
			return lhs;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000117AC File Offset: 0x0000F9AC
		public static Int3 operator /(Int3 lhs, float rhs)
		{
			lhs.x = (int)Math.Round((double)((float)lhs.x / rhs));
			lhs.y = (int)Math.Round((double)((float)lhs.y / rhs));
			lhs.z = (int)Math.Round((double)((float)lhs.z / rhs));
			return lhs;
		}

		// Token: 0x170000D5 RID: 213
		public int this[int i]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (i == 0)
				{
					return this.x;
				}
				if (i != 1)
				{
					return this.z;
				}
				return this.y;
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (i == 0)
				{
					this.x = value;
					return;
				}
				if (i == 1)
				{
					this.y = value;
					return;
				}
				this.z = value;
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001183C File Offset: 0x0000FA3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 Max(Int3 lhs, Int3 rhs)
		{
			return new Int3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00011876 File Offset: 0x0000FA76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 Min(Int3 lhs, Int3 rhs)
		{
			return new Int3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000118B0 File Offset: 0x0000FAB0
		public static float Angle(Int3 lhs, Int3 rhs)
		{
			double num = (double)Int3.Dot(lhs, rhs) / ((double)lhs.magnitude * (double)rhs.magnitude);
			num = ((num < -1.0) ? (-1.0) : ((num > 1.0) ? 1.0 : num));
			return (float)Math.Acos(num);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001190F File Offset: 0x0000FB0F
		public static int Dot(Int3 lhs, Int3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001193A File Offset: 0x0000FB3A
		public static long DotLong(Int3 lhs, Int3 rhs)
		{
			return (long)lhs.x * (long)rhs.x + (long)lhs.y * (long)rhs.y + (long)lhs.z * (long)rhs.z;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001196B File Offset: 0x0000FB6B
		public Int3 Normal2D()
		{
			return new Int3(this.z, this.y, -this.x);
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00011988 File Offset: 0x0000FB88
		public float magnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000119BC File Offset: 0x0000FBBC
		public int costMagnitude
		{
			get
			{
				return (int)Math.Round((double)this.magnitude);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000119CC File Offset: 0x0000FBCC
		public float sqrMagnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)(num * num + num2 * num2 + num3 * num3);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000119FC File Offset: 0x0000FBFC
		public long sqrMagnitudeLong
		{
			get
			{
				long num = (long)this.x;
				long num2 = (long)this.y;
				long num3 = (long)this.z;
				return num * num + num2 * num2 + num3 * num3;
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00011A2A File Offset: 0x0000FC2A
		public static implicit operator string(Int3 obj)
		{
			return obj.ToString();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00011A3C File Offset: 0x0000FC3C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"( ",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00011AA0 File Offset: 0x0000FCA0
		public override bool Equals(object obj)
		{
			if (!(obj is Int3))
			{
				return false;
			}
			Int3 @int = (Int3)obj;
			return this.x == @int.x && this.y == @int.y && this.z == @int.z;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001154B File Offset: 0x0000F74B
		public bool Equals(Int3 other)
		{
			return this.x == other.x && this.y == other.y && this.z == other.z;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00011AEA File Offset: 0x0000FCEA
		public override int GetHashCode()
		{
			return this.x * 73856093 + this.y * 19349669 + this.z * 83492791;
		}

		// Token: 0x0400021C RID: 540
		public int x;

		// Token: 0x0400021D RID: 541
		public int y;

		// Token: 0x0400021E RID: 542
		public int z;

		// Token: 0x0400021F RID: 543
		public const int Precision = 1000;

		// Token: 0x04000220 RID: 544
		public const float FloatPrecision = 1000f;

		// Token: 0x04000221 RID: 545
		public const float PrecisionFactor = 0.001f;
	}
}
