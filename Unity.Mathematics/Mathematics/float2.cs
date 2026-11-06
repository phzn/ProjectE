using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics
{
	// Token: 0x0200001E RID: 30
	[DebuggerTypeProxy(typeof(float2.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float2 : IEquatable<float2>, IFormattable
	{
		// Token: 0x06001060 RID: 4192 RVA: 0x000309FB File Offset: 0x0002EBFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00030A0B File Offset: 0x0002EC0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(float2 xy)
		{
			this.x = xy.x;
			this.y = xy.y;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00030A25 File Offset: 0x0002EC25
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(float v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00030A35 File Offset: 0x0002EC35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(bool v)
		{
			this.x = (v ? 1f : 0f);
			this.y = (v ? 1f : 0f);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00030A61 File Offset: 0x0002EC61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(bool2 v)
		{
			this.x = (v.x ? 1f : 0f);
			this.y = (v.y ? 1f : 0f);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00030A97 File Offset: 0x0002EC97
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(int v)
		{
			this.x = (float)v;
			this.y = (float)v;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00030AA9 File Offset: 0x0002ECA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(int2 v)
		{
			this.x = (float)v.x;
			this.y = (float)v.y;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00030AC5 File Offset: 0x0002ECC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(uint v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00030AD9 File Offset: 0x0002ECD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(uint2 v)
		{
			this.x = v.x;
			this.y = v.y;
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00030AF7 File Offset: 0x0002ECF7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(half v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00030B11 File Offset: 0x0002ED11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(half2 v)
		{
			this.x = v.x;
			this.y = v.y;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00030B35 File Offset: 0x0002ED35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(double v)
		{
			this.x = (float)v;
			this.y = (float)v;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x00030B47 File Offset: 0x0002ED47
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float2(double2 v)
		{
			this.x = (float)v.x;
			this.y = (float)v.y;
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00030B63 File Offset: 0x0002ED63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2(float v)
		{
			return new float2(v);
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00030B6B File Offset: 0x0002ED6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2(bool v)
		{
			return new float2(v);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00030B73 File Offset: 0x0002ED73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2(bool2 v)
		{
			return new float2(v);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00030B7B File Offset: 0x0002ED7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2(int v)
		{
			return new float2(v);
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00030B83 File Offset: 0x0002ED83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2(int2 v)
		{
			return new float2(v);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00030B8B File Offset: 0x0002ED8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2(uint v)
		{
			return new float2(v);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00030B93 File Offset: 0x0002ED93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2(uint2 v)
		{
			return new float2(v);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00030B9B File Offset: 0x0002ED9B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2(half v)
		{
			return new float2(v);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00030BA3 File Offset: 0x0002EDA3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float2(half2 v)
		{
			return new float2(v);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00030BAB File Offset: 0x0002EDAB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2(double v)
		{
			return new float2(v);
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00030BB3 File Offset: 0x0002EDB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float2(double2 v)
		{
			return new float2(v);
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00030BBB File Offset: 0x0002EDBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator *(float2 lhs, float2 rhs)
		{
			return new float2(lhs.x * rhs.x, lhs.y * rhs.y);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00030BDC File Offset: 0x0002EDDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator *(float2 lhs, float rhs)
		{
			return new float2(lhs.x * rhs, lhs.y * rhs);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00030BF3 File Offset: 0x0002EDF3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator *(float lhs, float2 rhs)
		{
			return new float2(lhs * rhs.x, lhs * rhs.y);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00030C0A File Offset: 0x0002EE0A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator +(float2 lhs, float2 rhs)
		{
			return new float2(lhs.x + rhs.x, lhs.y + rhs.y);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00030C2B File Offset: 0x0002EE2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator +(float2 lhs, float rhs)
		{
			return new float2(lhs.x + rhs, lhs.y + rhs);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00030C42 File Offset: 0x0002EE42
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator +(float lhs, float2 rhs)
		{
			return new float2(lhs + rhs.x, lhs + rhs.y);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00030C59 File Offset: 0x0002EE59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator -(float2 lhs, float2 rhs)
		{
			return new float2(lhs.x - rhs.x, lhs.y - rhs.y);
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00030C7A File Offset: 0x0002EE7A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator -(float2 lhs, float rhs)
		{
			return new float2(lhs.x - rhs, lhs.y - rhs);
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00030C91 File Offset: 0x0002EE91
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator -(float lhs, float2 rhs)
		{
			return new float2(lhs - rhs.x, lhs - rhs.y);
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00030CA8 File Offset: 0x0002EEA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator /(float2 lhs, float2 rhs)
		{
			return new float2(lhs.x / rhs.x, lhs.y / rhs.y);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00030CC9 File Offset: 0x0002EEC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator /(float2 lhs, float rhs)
		{
			return new float2(lhs.x / rhs, lhs.y / rhs);
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00030CE0 File Offset: 0x0002EEE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator /(float lhs, float2 rhs)
		{
			return new float2(lhs / rhs.x, lhs / rhs.y);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00030CF7 File Offset: 0x0002EEF7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator %(float2 lhs, float2 rhs)
		{
			return new float2(lhs.x % rhs.x, lhs.y % rhs.y);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00030D18 File Offset: 0x0002EF18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator %(float2 lhs, float rhs)
		{
			return new float2(lhs.x % rhs, lhs.y % rhs);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00030D2F File Offset: 0x0002EF2F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator %(float lhs, float2 rhs)
		{
			return new float2(lhs % rhs.x, lhs % rhs.y);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00030D48 File Offset: 0x0002EF48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator ++(float2 val)
		{
			float num = val.x + 1f;
			val.x = num;
			float num2 = num;
			num = val.y + 1f;
			val.y = num;
			return new float2(num2, num);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00030D80 File Offset: 0x0002EF80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator --(float2 val)
		{
			float num = val.x - 1f;
			val.x = num;
			float num2 = num;
			num = val.y - 1f;
			val.y = num;
			return new float2(num2, num);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00030DB8 File Offset: 0x0002EFB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(float2 lhs, float2 rhs)
		{
			return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00030DDB File Offset: 0x0002EFDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(float2 lhs, float rhs)
		{
			return new bool2(lhs.x < rhs, lhs.y < rhs);
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00030DF4 File Offset: 0x0002EFF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(float lhs, float2 rhs)
		{
			return new bool2(lhs < rhs.x, lhs < rhs.y);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00030E0D File Offset: 0x0002F00D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(float2 lhs, float2 rhs)
		{
			return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00030E36 File Offset: 0x0002F036
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(float2 lhs, float rhs)
		{
			return new bool2(lhs.x <= rhs, lhs.y <= rhs);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00030E55 File Offset: 0x0002F055
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(float lhs, float2 rhs)
		{
			return new bool2(lhs <= rhs.x, lhs <= rhs.y);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00030E74 File Offset: 0x0002F074
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(float2 lhs, float2 rhs)
		{
			return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00030E97 File Offset: 0x0002F097
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(float2 lhs, float rhs)
		{
			return new bool2(lhs.x > rhs, lhs.y > rhs);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00030EB0 File Offset: 0x0002F0B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(float lhs, float2 rhs)
		{
			return new bool2(lhs > rhs.x, lhs > rhs.y);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00030EC9 File Offset: 0x0002F0C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(float2 lhs, float2 rhs)
		{
			return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00030EF2 File Offset: 0x0002F0F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(float2 lhs, float rhs)
		{
			return new bool2(lhs.x >= rhs, lhs.y >= rhs);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00030F11 File Offset: 0x0002F111
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(float lhs, float2 rhs)
		{
			return new bool2(lhs >= rhs.x, lhs >= rhs.y);
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00030F30 File Offset: 0x0002F130
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator -(float2 val)
		{
			return new float2(-val.x, -val.y);
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00030F45 File Offset: 0x0002F145
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 operator +(float2 val)
		{
			return new float2(val.x, val.y);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00030F58 File Offset: 0x0002F158
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(float2 lhs, float2 rhs)
		{
			return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00030F7B File Offset: 0x0002F17B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(float2 lhs, float rhs)
		{
			return new bool2(lhs.x == rhs, lhs.y == rhs);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00030F94 File Offset: 0x0002F194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(float lhs, float2 rhs)
		{
			return new bool2(lhs == rhs.x, lhs == rhs.y);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00030FAD File Offset: 0x0002F1AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(float2 lhs, float2 rhs)
		{
			return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00030FD6 File Offset: 0x0002F1D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(float2 lhs, float rhs)
		{
			return new bool2(lhs.x != rhs, lhs.y != rhs);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00030FF5 File Offset: 0x0002F1F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(float lhs, float2 rhs)
		{
			return new bool2(lhs != rhs.x, lhs != rhs.y);
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x00031014 File Offset: 0x0002F214
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x00031033 File Offset: 0x0002F233
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x00031052 File Offset: 0x0002F252
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00031071 File Offset: 0x0002F271
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x00031090 File Offset: 0x0002F290
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x000310AF File Offset: 0x0002F2AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x000310CE File Offset: 0x0002F2CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x000310ED File Offset: 0x0002F2ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x0003110C File Offset: 0x0002F30C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0003112B File Offset: 0x0002F32B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x0003114A File Offset: 0x0002F34A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00031169 File Offset: 0x0002F369
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00031188 File Offset: 0x0002F388
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x000311A7 File Offset: 0x0002F3A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x000311C6 File Offset: 0x0002F3C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x000311E5 File Offset: 0x0002F3E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00031204 File Offset: 0x0002F404
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.x);
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x0003121D File Offset: 0x0002F41D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.y);
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00031236 File Offset: 0x0002F436
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.x);
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x0003124F File Offset: 0x0002F44F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.y);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00031268 File Offset: 0x0002F468
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.x);
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x00031281 File Offset: 0x0002F481
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.y);
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x0003129A File Offset: 0x0002F49A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.x);
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000312B3 File Offset: 0x0002F4B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.y);
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x000312CC File Offset: 0x0002F4CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.x, this.x);
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x000312DF File Offset: 0x0002F4DF
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x000312F2 File Offset: 0x0002F4F2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x0003130C File Offset: 0x0002F50C
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x0003131F File Offset: 0x0002F51F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x00031339 File Offset: 0x0002F539
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.y, this.y);
			}
		}

		// Token: 0x170003F7 RID: 1015
		public unsafe float this[int index]
		{
			get
			{
				fixed (float2* ptr = &this)
				{
					return ((float*)ptr)[index];
				}
			}
			set
			{
				fixed (float* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00031384 File Offset: 0x0002F584
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float2 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y;
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x000313A4 File Offset: 0x0002F5A4
		public override bool Equals(object o)
		{
			if (o is float2)
			{
				float2 @float = (float2)o;
				return this.Equals(@float);
			}
			return false;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x000313C9 File Offset: 0x0002F5C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x000313D6 File Offset: 0x0002F5D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float2({0}f, {1}f)", this.x, this.y);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x000313F8 File Offset: 0x0002F5F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float2({0}f, {1}f)", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider));
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0003141E File Offset: 0x0002F61E
		public static implicit operator Vector2(float2 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00031431 File Offset: 0x0002F631
		public static implicit operator float2(Vector2 v)
		{
			return new float2(v.x, v.y);
		}

		// Token: 0x0400007C RID: 124
		public float x;

		// Token: 0x0400007D RID: 125
		public float y;

		// Token: 0x0400007E RID: 126
		public static readonly float2 zero;

		// Token: 0x0200005B RID: 91
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024C8 RID: 9416 RVA: 0x00069204 File Offset: 0x00067404
			public DebuggerProxy(float2 v)
			{
				this.x = v.x;
				this.y = v.y;
			}

			// Token: 0x0400015C RID: 348
			public float x;

			// Token: 0x0400015D RID: 349
			public float y;
		}
	}
}
