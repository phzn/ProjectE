using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000016 RID: 22
	[DebuggerTypeProxy(typeof(double3.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct double3 : IEquatable<double3>, IFormattable
	{
		// Token: 0x06000C47 RID: 3143 RVA: 0x00026589 File Offset: 0x00024789
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000265A0 File Offset: 0x000247A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(double x, double2 yz)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000265C1 File Offset: 0x000247C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(double2 xy, double z)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000265E2 File Offset: 0x000247E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(double3 xyz)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00026608 File Offset: 0x00024808
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(double v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00026620 File Offset: 0x00024820
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(bool v)
		{
			this.x = (v ? 1.0 : 0.0);
			this.y = (v ? 1.0 : 0.0);
			this.z = (v ? 1.0 : 0.0);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00026684 File Offset: 0x00024884
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(bool3 v)
		{
			this.x = (v.x ? 1.0 : 0.0);
			this.y = (v.y ? 1.0 : 0.0);
			this.z = (v.z ? 1.0 : 0.0);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000266F7 File Offset: 0x000248F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(int v)
		{
			this.x = (double)v;
			this.y = (double)v;
			this.z = (double)v;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00026711 File Offset: 0x00024911
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(int3 v)
		{
			this.x = (double)v.x;
			this.y = (double)v.y;
			this.z = (double)v.z;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0002673A File Offset: 0x0002493A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(uint v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00026757 File Offset: 0x00024957
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(uint3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00026783 File Offset: 0x00024983
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(half v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000267A9 File Offset: 0x000249A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(half3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000267DE File Offset: 0x000249DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(float v)
		{
			this.x = (double)v;
			this.y = (double)v;
			this.z = (double)v;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000267F8 File Offset: 0x000249F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double3(float3 v)
		{
			this.x = (double)v.x;
			this.y = (double)v.y;
			this.z = (double)v.z;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00026821 File Offset: 0x00024A21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(double v)
		{
			return new double3(v);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00026829 File Offset: 0x00024A29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3(bool v)
		{
			return new double3(v);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00026831 File Offset: 0x00024A31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator double3(bool3 v)
		{
			return new double3(v);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00026839 File Offset: 0x00024A39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(int v)
		{
			return new double3(v);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00026841 File Offset: 0x00024A41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(int3 v)
		{
			return new double3(v);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00026849 File Offset: 0x00024A49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(uint v)
		{
			return new double3(v);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00026851 File Offset: 0x00024A51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(uint3 v)
		{
			return new double3(v);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00026859 File Offset: 0x00024A59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(half v)
		{
			return new double3(v);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00026861 File Offset: 0x00024A61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(half3 v)
		{
			return new double3(v);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00026869 File Offset: 0x00024A69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(float v)
		{
			return new double3(v);
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00026871 File Offset: 0x00024A71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double3(float3 v)
		{
			return new double3(v);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00026879 File Offset: 0x00024A79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator *(double3 lhs, double3 rhs)
		{
			return new double3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x000268A7 File Offset: 0x00024AA7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator *(double3 lhs, double rhs)
		{
			return new double3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000268C6 File Offset: 0x00024AC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator *(double lhs, double3 rhs)
		{
			return new double3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000268E5 File Offset: 0x00024AE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator +(double3 lhs, double3 rhs)
		{
			return new double3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00026913 File Offset: 0x00024B13
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator +(double3 lhs, double rhs)
		{
			return new double3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00026932 File Offset: 0x00024B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator +(double lhs, double3 rhs)
		{
			return new double3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00026951 File Offset: 0x00024B51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator -(double3 lhs, double3 rhs)
		{
			return new double3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0002697F File Offset: 0x00024B7F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator -(double3 lhs, double rhs)
		{
			return new double3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0002699E File Offset: 0x00024B9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator -(double lhs, double3 rhs)
		{
			return new double3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000269BD File Offset: 0x00024BBD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator /(double3 lhs, double3 rhs)
		{
			return new double3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x000269EB File Offset: 0x00024BEB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator /(double3 lhs, double rhs)
		{
			return new double3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00026A0A File Offset: 0x00024C0A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator /(double lhs, double3 rhs)
		{
			return new double3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00026A29 File Offset: 0x00024C29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator %(double3 lhs, double3 rhs)
		{
			return new double3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00026A57 File Offset: 0x00024C57
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator %(double3 lhs, double rhs)
		{
			return new double3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00026A76 File Offset: 0x00024C76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator %(double lhs, double3 rhs)
		{
			return new double3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00026A98 File Offset: 0x00024C98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator ++(double3 val)
		{
			double num = val.x + 1.0;
			val.x = num;
			double num2 = num;
			num = val.y + 1.0;
			val.y = num;
			double num3 = num;
			num = val.z + 1.0;
			val.z = num;
			return new double3(num2, num3, num);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00026AF0 File Offset: 0x00024CF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator --(double3 val)
		{
			double num = val.x - 1.0;
			val.x = num;
			double num2 = num;
			num = val.y - 1.0;
			val.y = num;
			double num3 = num;
			num = val.z - 1.0;
			val.z = num;
			return new double3(num2, num3, num);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00026B47 File Offset: 0x00024D47
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(double3 lhs, double3 rhs)
		{
			return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00026B78 File Offset: 0x00024D78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(double3 lhs, double rhs)
		{
			return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00026B9A File Offset: 0x00024D9A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(double lhs, double3 rhs)
		{
			return new bool3(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00026BBC File Offset: 0x00024DBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(double3 lhs, double3 rhs)
		{
			return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00026BF6 File Offset: 0x00024DF6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(double3 lhs, double rhs)
		{
			return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00026C21 File Offset: 0x00024E21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(double lhs, double3 rhs)
		{
			return new bool3(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00026C4C File Offset: 0x00024E4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(double3 lhs, double3 rhs)
		{
			return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00026C7D File Offset: 0x00024E7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(double3 lhs, double rhs)
		{
			return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00026C9F File Offset: 0x00024E9F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(double lhs, double3 rhs)
		{
			return new bool3(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00026CC1 File Offset: 0x00024EC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(double3 lhs, double3 rhs)
		{
			return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00026CFB File Offset: 0x00024EFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(double3 lhs, double rhs)
		{
			return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00026D26 File Offset: 0x00024F26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(double lhs, double3 rhs)
		{
			return new bool3(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00026D51 File Offset: 0x00024F51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator -(double3 val)
		{
			return new double3(-val.x, -val.y, -val.z);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00026D6D File Offset: 0x00024F6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 operator +(double3 val)
		{
			return new double3(val.x, val.y, val.z);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00026D86 File Offset: 0x00024F86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(double3 lhs, double3 rhs)
		{
			return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00026DB7 File Offset: 0x00024FB7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(double3 lhs, double rhs)
		{
			return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00026DD9 File Offset: 0x00024FD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(double lhs, double3 rhs)
		{
			return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00026DFB File Offset: 0x00024FFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(double3 lhs, double3 rhs)
		{
			return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00026E35 File Offset: 0x00025035
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(double3 lhs, double rhs)
		{
			return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00026E60 File Offset: 0x00025060
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(double lhs, double3 rhs)
		{
			return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00026E8B File Offset: 0x0002508B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00026EAA File Offset: 0x000250AA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00026EC9 File Offset: 0x000250C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00026EE8 File Offset: 0x000250E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00026F07 File Offset: 0x00025107
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00026F26 File Offset: 0x00025126
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00026F45 File Offset: 0x00025145
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00026F64 File Offset: 0x00025164
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00026F83 File Offset: 0x00025183
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x00026FA2 File Offset: 0x000251A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00026FC1 File Offset: 0x000251C1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00026FE0 File Offset: 0x000251E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00026FFF File Offset: 0x000251FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0002701E File Offset: 0x0002521E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0002703D File Offset: 0x0002523D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0002705C File Offset: 0x0002525C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0002707B File Offset: 0x0002527B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0002709A File Offset: 0x0002529A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000270B9 File Offset: 0x000252B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x000270D8 File Offset: 0x000252D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x000270F7 File Offset: 0x000252F7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00027116 File Offset: 0x00025316
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00027135 File Offset: 0x00025335
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x00027154 File Offset: 0x00025354
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00027173 File Offset: 0x00025373
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00027192 File Offset: 0x00025392
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x000271B1 File Offset: 0x000253B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x000271D0 File Offset: 0x000253D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x000271EF File Offset: 0x000253EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0002720E File Offset: 0x0002540E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0002722D File Offset: 0x0002542D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0002724C File Offset: 0x0002544C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0002726B File Offset: 0x0002546B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0002728A File Offset: 0x0002548A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x000272A9 File Offset: 0x000254A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x000272C8 File Offset: 0x000254C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x000272E7 File Offset: 0x000254E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00027306 File Offset: 0x00025506
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00027325 File Offset: 0x00025525
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00027344 File Offset: 0x00025544
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00027363 File Offset: 0x00025563
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00027382 File Offset: 0x00025582
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x000273A1 File Offset: 0x000255A1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x000273C0 File Offset: 0x000255C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x000273DF File Offset: 0x000255DF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x000273FE File Offset: 0x000255FE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0002741D File Offset: 0x0002561D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0002743C File Offset: 0x0002563C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0002745B File Offset: 0x0002565B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0002747A File Offset: 0x0002567A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00027499 File Offset: 0x00025699
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x000274B8 File Offset: 0x000256B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x000274D7 File Offset: 0x000256D7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x000274F6 File Offset: 0x000256F6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00027515 File Offset: 0x00025715
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00027534 File Offset: 0x00025734
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00027553 File Offset: 0x00025753
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00027572 File Offset: 0x00025772
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00027591 File Offset: 0x00025791
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x000275B0 File Offset: 0x000257B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x000275CF File Offset: 0x000257CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x000275EE File Offset: 0x000257EE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0002760D File Offset: 0x0002580D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0002762C File Offset: 0x0002582C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0002764B File Offset: 0x0002584B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0002766A File Offset: 0x0002586A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00027689 File Offset: 0x00025889
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x000276A8 File Offset: 0x000258A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x000276C7 File Offset: 0x000258C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x000276E6 File Offset: 0x000258E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x00027705 File Offset: 0x00025905
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00027724 File Offset: 0x00025924
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00027743 File Offset: 0x00025943
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00027762 File Offset: 0x00025962
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00027781 File Offset: 0x00025981
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x000277A0 File Offset: 0x000259A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x000277BF File Offset: 0x000259BF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x000277DE File Offset: 0x000259DE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x000277FD File Offset: 0x000259FD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0002781C File Offset: 0x00025A1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0002783B File Offset: 0x00025A3B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0002785A File Offset: 0x00025A5A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00027873 File Offset: 0x00025A73
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0002788C File Offset: 0x00025A8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.x, this.z);
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000278A5 File Offset: 0x00025AA5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000278BE File Offset: 0x00025ABE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000278D7 File Offset: 0x00025AD7
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x000278F0 File Offset: 0x00025AF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00027916 File Offset: 0x00025B16
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0002792F File Offset: 0x00025B2F
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x00027948 File Offset: 0x00025B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0002796E File Offset: 0x00025B6E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.x, this.z, this.z);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x00027987 File Offset: 0x00025B87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x000279A0 File Offset: 0x00025BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.y);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x000279B9 File Offset: 0x00025BB9
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x000279D2 File Offset: 0x00025BD2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000279F8 File Offset: 0x00025BF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.x);
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00027A11 File Offset: 0x00025C11
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.y);
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00027A2A File Offset: 0x00025C2A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.y, this.z);
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00027A43 File Offset: 0x00025C43
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x00027A5C File Offset: 0x00025C5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x00027A82 File Offset: 0x00025C82
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.z, this.y);
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00027A9B File Offset: 0x00025C9B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.y, this.z, this.z);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00027AB4 File Offset: 0x00025CB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.x, this.x);
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00027ACD File Offset: 0x00025CCD
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x00027AE6 File Offset: 0x00025CE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00027B0C File Offset: 0x00025D0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.x, this.z);
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00027B25 File Offset: 0x00025D25
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x00027B3E File Offset: 0x00025D3E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x00027B64 File Offset: 0x00025D64
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.y, this.y);
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00027B7D File Offset: 0x00025D7D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.y, this.z);
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00027B96 File Offset: 0x00025D96
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.z, this.x);
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00027BAF File Offset: 0x00025DAF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.z, this.y);
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00027BC8 File Offset: 0x00025DC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double3(this.z, this.z, this.z);
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00027BE1 File Offset: 0x00025DE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.x);
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00027BF4 File Offset: 0x00025DF4
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x00027C07 File Offset: 0x00025E07
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00027C21 File Offset: 0x00025E21
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00027C34 File Offset: 0x00025E34
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00027C4E File Offset: 0x00025E4E
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x00027C61 File Offset: 0x00025E61
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00027C7B File Offset: 0x00025E7B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.y);
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00027C8E File Offset: 0x00025E8E
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00027CA1 File Offset: 0x00025EA1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00027CBB File Offset: 0x00025EBB
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x00027CCE File Offset: 0x00025ECE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00027CE8 File Offset: 0x00025EE8
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x00027CFB File Offset: 0x00025EFB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00027D15 File Offset: 0x00025F15
		[EditorBrowsable(EditorBrowsableState.Never)]
		public double2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new double2(this.z, this.z);
			}
		}

		// Token: 0x17000283 RID: 643
		public unsafe double this[int index]
		{
			get
			{
				fixed (double3* ptr = &this)
				{
					return ((double*)ptr)[index];
				}
			}
			set
			{
				fixed (double* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00027D60 File Offset: 0x00025F60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(double3 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00027D90 File Offset: 0x00025F90
		public override bool Equals(object o)
		{
			if (o is double3)
			{
				double3 @double = (double3)o;
				return this.Equals(@double);
			}
			return false;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00027DB5 File Offset: 0x00025FB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00027DC2 File Offset: 0x00025FC2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("double3({0}, {1}, {2})", this.x, this.y, this.z);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00027DEF File Offset: 0x00025FEF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("double3({0}, {1}, {2})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider), this.z.ToString(format, formatProvider));
		}

		// Token: 0x04000059 RID: 89
		public double x;

		// Token: 0x0400005A RID: 90
		public double y;

		// Token: 0x0400005B RID: 91
		public double z;

		// Token: 0x0400005C RID: 92
		public static readonly double3 zero;

		// Token: 0x02000059 RID: 89
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024C6 RID: 9414 RVA: 0x000691A0 File Offset: 0x000673A0
			public DebuggerProxy(double3 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			// Token: 0x04000155 RID: 341
			public double x;

			// Token: 0x04000156 RID: 342
			public double y;

			// Token: 0x04000157 RID: 343
			public double z;
		}
	}
}
