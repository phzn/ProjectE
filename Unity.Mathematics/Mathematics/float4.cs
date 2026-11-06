using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics
{
	// Token: 0x02000026 RID: 38
	[DebuggerTypeProxy(typeof(float4.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float4 : IEquatable<float4>, IFormattable
	{
		// Token: 0x06001327 RID: 4903 RVA: 0x00038195 File Offset: 0x00036395
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000381B4 File Offset: 0x000363B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float x, float y, float2 zw)
		{
			this.x = x;
			this.y = y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x000381DC File Offset: 0x000363DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float x, float2 yz, float w)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
			this.w = w;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00038204 File Offset: 0x00036404
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float x, float3 yzw)
		{
			this.x = x;
			this.y = yzw.x;
			this.z = yzw.y;
			this.w = yzw.z;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00038231 File Offset: 0x00036431
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float2 xy, float z, float w)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00038259 File Offset: 0x00036459
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float2 xy, float2 zw)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = zw.x;
			this.w = zw.y;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0003828B File Offset: 0x0003648B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float3 xyz, float w)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
			this.w = w;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000382B8 File Offset: 0x000364B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float4 xyzw)
		{
			this.x = xyzw.x;
			this.y = xyzw.y;
			this.z = xyzw.z;
			this.w = xyzw.w;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000382EA File Offset: 0x000364EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(float v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00038308 File Offset: 0x00036508
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(bool v)
		{
			this.x = (v ? 1f : 0f);
			this.y = (v ? 1f : 0f);
			this.z = (v ? 1f : 0f);
			this.w = (v ? 1f : 0f);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0003836C File Offset: 0x0003656C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(bool4 v)
		{
			this.x = (v.x ? 1f : 0f);
			this.y = (v.y ? 1f : 0f);
			this.z = (v.z ? 1f : 0f);
			this.w = (v.w ? 1f : 0f);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x000383E1 File Offset: 0x000365E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(int v)
		{
			this.x = (float)v;
			this.y = (float)v;
			this.z = (float)v;
			this.w = (float)v;
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00038403 File Offset: 0x00036603
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(int4 v)
		{
			this.x = (float)v.x;
			this.y = (float)v.y;
			this.z = (float)v.z;
			this.w = (float)v.w;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00038439 File Offset: 0x00036639
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(uint v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0003845F File Offset: 0x0003665F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(uint4 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
			this.w = v.w;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00038499 File Offset: 0x00036699
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(half v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
			this.w = v;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x000384CC File Offset: 0x000366CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(half4 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
			this.w = v.w;
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0003851D File Offset: 0x0003671D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(double v)
		{
			this.x = (float)v;
			this.y = (float)v;
			this.z = (float)v;
			this.w = (float)v;
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0003853F File Offset: 0x0003673F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float4(double4 v)
		{
			this.x = (float)v.x;
			this.y = (float)v.y;
			this.z = (float)v.z;
			this.w = (float)v.w;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00038575 File Offset: 0x00036775
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(float v)
		{
			return new float4(v);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0003857D File Offset: 0x0003677D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4(bool v)
		{
			return new float4(v);
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00038585 File Offset: 0x00036785
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4(bool4 v)
		{
			return new float4(v);
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0003858D File Offset: 0x0003678D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(int v)
		{
			return new float4(v);
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00038595 File Offset: 0x00036795
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(int4 v)
		{
			return new float4(v);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0003859D File Offset: 0x0003679D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(uint v)
		{
			return new float4(v);
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x000385A5 File Offset: 0x000367A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(uint4 v)
		{
			return new float4(v);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000385AD File Offset: 0x000367AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(half v)
		{
			return new float4(v);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x000385B5 File Offset: 0x000367B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4(half4 v)
		{
			return new float4(v);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x000385BD File Offset: 0x000367BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4(double v)
		{
			return new float4(v);
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000385C5 File Offset: 0x000367C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float4(double4 v)
		{
			return new float4(v);
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000385CD File Offset: 0x000367CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator *(float4 lhs, float4 rhs)
		{
			return new float4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00038608 File Offset: 0x00036808
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator *(float4 lhs, float rhs)
		{
			return new float4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0003862F File Offset: 0x0003682F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator *(float lhs, float4 rhs)
		{
			return new float4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00038656 File Offset: 0x00036856
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator +(float4 lhs, float4 rhs)
		{
			return new float4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00038691 File Offset: 0x00036891
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator +(float4 lhs, float rhs)
		{
			return new float4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000386B8 File Offset: 0x000368B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator +(float lhs, float4 rhs)
		{
			return new float4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x000386DF File Offset: 0x000368DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator -(float4 lhs, float4 rhs)
		{
			return new float4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0003871A File Offset: 0x0003691A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator -(float4 lhs, float rhs)
		{
			return new float4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00038741 File Offset: 0x00036941
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator -(float lhs, float4 rhs)
		{
			return new float4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00038768 File Offset: 0x00036968
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator /(float4 lhs, float4 rhs)
		{
			return new float4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x000387A3 File Offset: 0x000369A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator /(float4 lhs, float rhs)
		{
			return new float4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x000387CA File Offset: 0x000369CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator /(float lhs, float4 rhs)
		{
			return new float4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x000387F1 File Offset: 0x000369F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator %(float4 lhs, float4 rhs)
		{
			return new float4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0003882C File Offset: 0x00036A2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator %(float4 lhs, float rhs)
		{
			return new float4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00038853 File Offset: 0x00036A53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator %(float lhs, float4 rhs)
		{
			return new float4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0003887C File Offset: 0x00036A7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator ++(float4 val)
		{
			float num = val.x + 1f;
			val.x = num;
			float num2 = num;
			num = val.y + 1f;
			val.y = num;
			float num3 = num;
			num = val.z + 1f;
			val.z = num;
			float num4 = num;
			num = val.w + 1f;
			val.w = num;
			return new float4(num2, num3, num4, num);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x000388DC File Offset: 0x00036ADC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator --(float4 val)
		{
			float num = val.x - 1f;
			val.x = num;
			float num2 = num;
			num = val.y - 1f;
			val.y = num;
			float num3 = num;
			num = val.z - 1f;
			val.z = num;
			float num4 = num;
			num = val.w - 1f;
			val.w = num;
			return new float4(num2, num3, num4, num);
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0003893A File Offset: 0x00036B3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(float4 lhs, float4 rhs)
		{
			return new bool4(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z, lhs.w < rhs.w);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00038979 File Offset: 0x00036B79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(float4 lhs, float rhs)
		{
			return new bool4(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs, lhs.w < rhs);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000389A4 File Offset: 0x00036BA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <(float lhs, float4 rhs)
		{
			return new bool4(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z, lhs < rhs.w);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000389D0 File Offset: 0x00036BD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(float4 lhs, float4 rhs)
		{
			return new bool4(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z, lhs.w <= rhs.w);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00038A26 File Offset: 0x00036C26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(float4 lhs, float rhs)
		{
			return new bool4(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs, lhs.w <= rhs);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00038A5D File Offset: 0x00036C5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator <=(float lhs, float4 rhs)
		{
			return new bool4(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z, lhs <= rhs.w);
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00038A94 File Offset: 0x00036C94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(float4 lhs, float4 rhs)
		{
			return new bool4(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z, lhs.w > rhs.w);
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00038AD3 File Offset: 0x00036CD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(float4 lhs, float rhs)
		{
			return new bool4(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs, lhs.w > rhs);
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00038AFE File Offset: 0x00036CFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >(float lhs, float4 rhs)
		{
			return new bool4(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z, lhs > rhs.w);
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00038B2C File Offset: 0x00036D2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(float4 lhs, float4 rhs)
		{
			return new bool4(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z, lhs.w >= rhs.w);
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00038B82 File Offset: 0x00036D82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(float4 lhs, float rhs)
		{
			return new bool4(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs, lhs.w >= rhs);
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00038BB9 File Offset: 0x00036DB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator >=(float lhs, float4 rhs)
		{
			return new bool4(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z, lhs >= rhs.w);
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00038BF0 File Offset: 0x00036DF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator -(float4 val)
		{
			return new float4(-val.x, -val.y, -val.z, -val.w);
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00038C13 File Offset: 0x00036E13
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 operator +(float4 val)
		{
			return new float4(val.x, val.y, val.z, val.w);
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00038C32 File Offset: 0x00036E32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(float4 lhs, float4 rhs)
		{
			return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00038C71 File Offset: 0x00036E71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(float4 lhs, float rhs)
		{
			return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00038C9C File Offset: 0x00036E9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator ==(float lhs, float4 rhs)
		{
			return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00038CC8 File Offset: 0x00036EC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(float4 lhs, float4 rhs)
		{
			return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00038D1E File Offset: 0x00036F1E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(float4 lhs, float rhs)
		{
			return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00038D55 File Offset: 0x00036F55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 operator !=(float lhs, float4 rhs)
		{
			return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x00038D8C File Offset: 0x00036F8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x00038DAB File Offset: 0x00036FAB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x00038DCA File Offset: 0x00036FCA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00038DE9 File Offset: 0x00036FE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.w);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x00038E08 File Offset: 0x00037008
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x00038E27 File Offset: 0x00037027
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x00038E46 File Offset: 0x00037046
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00038E65 File Offset: 0x00037065
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.w);
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00038E84 File Offset: 0x00037084
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x00038EA3 File Offset: 0x000370A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x00038EC2 File Offset: 0x000370C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x00038EE1 File Offset: 0x000370E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x00038F00 File Offset: 0x00037100
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00038F1F File Offset: 0x0003711F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.w, this.y);
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x00038F3E File Offset: 0x0003713E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x00038F5D File Offset: 0x0003715D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x00038F7C File Offset: 0x0003717C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x00038F9B File Offset: 0x0003719B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x00038FBA File Offset: 0x000371BA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x00038FD9 File Offset: 0x000371D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.w);
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x00038FF8 File Offset: 0x000371F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x00039017 File Offset: 0x00037217
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x00039036 File Offset: 0x00037236
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x00039055 File Offset: 0x00037255
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.w);
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00039074 File Offset: 0x00037274
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x00039093 File Offset: 0x00037293
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x000390B2 File Offset: 0x000372B2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x000390D1 File Offset: 0x000372D1
		// (set) Token: 0x06001386 RID: 4998 RVA: 0x000390F0 File Offset: 0x000372F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x00039122 File Offset: 0x00037322
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.w, this.x);
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00039141 File Offset: 0x00037341
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.w, this.y);
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x00039160 File Offset: 0x00037360
		// (set) Token: 0x0600138A RID: 5002 RVA: 0x0003917F File Offset: 0x0003737F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.w = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x000391B1 File Offset: 0x000373B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.w, this.w);
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x000391D0 File Offset: 0x000373D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x000391EF File Offset: 0x000373EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x0003920E File Offset: 0x0003740E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x0003922D File Offset: 0x0003742D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.x, this.w);
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x0003924C File Offset: 0x0003744C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x0003926B File Offset: 0x0003746B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x0003928A File Offset: 0x0003748A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x000392A9 File Offset: 0x000374A9
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x000392C8 File Offset: 0x000374C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x000392FA File Offset: 0x000374FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x00039319 File Offset: 0x00037519
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x00039338 File Offset: 0x00037538
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x00039357 File Offset: 0x00037557
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.z, this.w);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x00039376 File Offset: 0x00037576
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.w, this.x);
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x00039395 File Offset: 0x00037595
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x000393B4 File Offset: 0x000375B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.w = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x000393E6 File Offset: 0x000375E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.w, this.z);
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x00039405 File Offset: 0x00037605
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.w, this.w);
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x00039424 File Offset: 0x00037624
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.x, this.x);
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x00039443 File Offset: 0x00037643
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.x, this.y);
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x00039462 File Offset: 0x00037662
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.x, this.z);
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00039481 File Offset: 0x00037681
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.x, this.w);
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x000394A0 File Offset: 0x000376A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.y, this.x);
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x000394BF File Offset: 0x000376BF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.y, this.y);
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x000394DE File Offset: 0x000376DE
		// (set) Token: 0x060013A5 RID: 5029 RVA: 0x000394FD File Offset: 0x000376FD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.y = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x0003952F File Offset: 0x0003772F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.y, this.w);
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x0003954E File Offset: 0x0003774E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.z, this.x);
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0003956D File Offset: 0x0003776D
		// (set) Token: 0x060013A9 RID: 5033 RVA: 0x0003958C File Offset: 0x0003778C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.z = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x000395BE File Offset: 0x000377BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.z, this.z);
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x000395DD File Offset: 0x000377DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.z, this.w);
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x000395FC File Offset: 0x000377FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.w, this.x);
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x0003961B File Offset: 0x0003781B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.w, this.y);
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x0003963A File Offset: 0x0003783A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.w, this.z);
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x00039659 File Offset: 0x00037859
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.w, this.w, this.w);
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00039678 File Offset: 0x00037878
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00039697 File Offset: 0x00037897
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x000396B6 File Offset: 0x000378B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060013B3 RID: 5043 RVA: 0x000396D5 File Offset: 0x000378D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.w);
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x000396F4 File Offset: 0x000378F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x00039713 File Offset: 0x00037913
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x00039732 File Offset: 0x00037932
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x00039751 File Offset: 0x00037951
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.w);
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00039770 File Offset: 0x00037970
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0003978F File Offset: 0x0003798F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x000397AE File Offset: 0x000379AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x000397CD File Offset: 0x000379CD
		// (set) Token: 0x060013BC RID: 5052 RVA: 0x000397EC File Offset: 0x000379EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0003981E File Offset: 0x00037A1E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.w, this.x);
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0003983D File Offset: 0x00037A3D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.w, this.y);
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0003985C File Offset: 0x00037A5C
		// (set) Token: 0x060013C0 RID: 5056 RVA: 0x0003987B File Offset: 0x00037A7B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.w = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x000398AD File Offset: 0x00037AAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.w, this.w);
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x000398CC File Offset: 0x00037ACC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x000398EB File Offset: 0x00037AEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0003990A File Offset: 0x00037B0A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00039929 File Offset: 0x00037B29
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.w);
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x00039948 File Offset: 0x00037B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x00039967 File Offset: 0x00037B67
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00039986 File Offset: 0x00037B86
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x000399A5 File Offset: 0x00037BA5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.w);
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x000399C4 File Offset: 0x00037BC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x000399E3 File Offset: 0x00037BE3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x00039A02 File Offset: 0x00037C02
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x00039A21 File Offset: 0x00037C21
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.z, this.w);
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x00039A40 File Offset: 0x00037C40
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.w, this.x);
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x00039A5F File Offset: 0x00037C5F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.w, this.y);
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x00039A7E File Offset: 0x00037C7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.w, this.z);
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x00039A9D File Offset: 0x00037C9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.w, this.w);
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x00039ABC File Offset: 0x00037CBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x00039ADB File Offset: 0x00037CDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x00039AFA File Offset: 0x00037CFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00039B19 File Offset: 0x00037D19
		// (set) Token: 0x060013D6 RID: 5078 RVA: 0x00039B38 File Offset: 0x00037D38
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x00039B6A File Offset: 0x00037D6A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x00039B89 File Offset: 0x00037D89
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x00039BA8 File Offset: 0x00037DA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00039BC7 File Offset: 0x00037DC7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.y, this.w);
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x00039BE6 File Offset: 0x00037DE6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00039C05 File Offset: 0x00037E05
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x00039C24 File Offset: 0x00037E24
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00039C43 File Offset: 0x00037E43
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.z, this.w);
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x00039C62 File Offset: 0x00037E62
		// (set) Token: 0x060013E0 RID: 5088 RVA: 0x00039C81 File Offset: 0x00037E81
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.w = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x00039CB3 File Offset: 0x00037EB3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.w, this.y);
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x00039CD2 File Offset: 0x00037ED2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.w, this.z);
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00039CF1 File Offset: 0x00037EF1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.w, this.w);
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00039D10 File Offset: 0x00037F10
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.x, this.x);
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00039D2F File Offset: 0x00037F2F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.x, this.y);
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x00039D4E File Offset: 0x00037F4E
		// (set) Token: 0x060013E7 RID: 5095 RVA: 0x00039D6D File Offset: 0x00037F6D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.x = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x00039D9F File Offset: 0x00037F9F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.x, this.w);
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00039DBE File Offset: 0x00037FBE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.y, this.x);
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00039DDD File Offset: 0x00037FDD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.y, this.y);
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x00039DFC File Offset: 0x00037FFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.y, this.z);
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x00039E1B File Offset: 0x0003801B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.y, this.w);
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x00039E3A File Offset: 0x0003803A
		// (set) Token: 0x060013EE RID: 5102 RVA: 0x00039E59 File Offset: 0x00038059
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.z = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x00039E8B File Offset: 0x0003808B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.z, this.y);
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x00039EAA File Offset: 0x000380AA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.z, this.z);
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x00039EC9 File Offset: 0x000380C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.z, this.w);
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x00039EE8 File Offset: 0x000380E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.w, this.x);
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x00039F07 File Offset: 0x00038107
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.w, this.y);
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x00039F26 File Offset: 0x00038126
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.w, this.z);
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x00039F45 File Offset: 0x00038145
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 ywww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.w, this.w, this.w);
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00039F64 File Offset: 0x00038164
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00039F83 File Offset: 0x00038183
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00039FA2 File Offset: 0x000381A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00039FC1 File Offset: 0x000381C1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.x, this.w);
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x00039FE0 File Offset: 0x000381E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x00039FFF File Offset: 0x000381FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x0003A01E File Offset: 0x0003821E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0003A03D File Offset: 0x0003823D
		// (set) Token: 0x060013FE RID: 5118 RVA: 0x0003A05C File Offset: 0x0003825C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x0003A08E File Offset: 0x0003828E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x0003A0AD File Offset: 0x000382AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x0003A0CC File Offset: 0x000382CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x0003A0EB File Offset: 0x000382EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0003A10A File Offset: 0x0003830A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x0003A129 File Offset: 0x00038329
		// (set) Token: 0x06001405 RID: 5125 RVA: 0x0003A148 File Offset: 0x00038348
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.w = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x0003A17A File Offset: 0x0003837A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x0003A199 File Offset: 0x00038399
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x0003A1B8 File Offset: 0x000383B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x0003A1D7 File Offset: 0x000383D7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x0003A1F6 File Offset: 0x000383F6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x0003A215 File Offset: 0x00038415
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x0003A234 File Offset: 0x00038434
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
				this.w = value.w;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x0003A266 File Offset: 0x00038466
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x0003A285 File Offset: 0x00038485
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x0003A2A4 File Offset: 0x000384A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0003A2C3 File Offset: 0x000384C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.y, this.w);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x0003A2E2 File Offset: 0x000384E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0003A301 File Offset: 0x00038501
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0003A320 File Offset: 0x00038520
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0003A33F File Offset: 0x0003853F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.z, this.w);
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0003A35E File Offset: 0x0003855E
		// (set) Token: 0x06001416 RID: 5142 RVA: 0x0003A37D File Offset: 0x0003857D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.w = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0003A3AF File Offset: 0x000385AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.w, this.y);
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0003A3CE File Offset: 0x000385CE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.w, this.z);
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x0003A3ED File Offset: 0x000385ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.w, this.w);
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0003A40C File Offset: 0x0003860C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0003A42B File Offset: 0x0003862B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0003A44A File Offset: 0x0003864A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0003A469 File Offset: 0x00038669
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.x, this.w);
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0003A488 File Offset: 0x00038688
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x0003A4A7 File Offset: 0x000386A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0003A4C6 File Offset: 0x000386C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0003A4E5 File Offset: 0x000386E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.y, this.w);
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0003A504 File Offset: 0x00038704
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0003A523 File Offset: 0x00038723
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x0003A542 File Offset: 0x00038742
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x0003A561 File Offset: 0x00038761
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.z, this.w);
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0003A580 File Offset: 0x00038780
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.w, this.x);
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0003A59F File Offset: 0x0003879F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.w, this.y);
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0003A5BE File Offset: 0x000387BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.w, this.z);
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x0003A5DD File Offset: 0x000387DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.w, this.w);
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0003A5FC File Offset: 0x000387FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.x, this.x);
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0003A61B File Offset: 0x0003881B
		// (set) Token: 0x0600142C RID: 5164 RVA: 0x0003A63A File Offset: 0x0003883A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.x = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0003A66C File Offset: 0x0003886C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.x, this.z);
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x0003A68B File Offset: 0x0003888B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.x, this.w);
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x0003A6AA File Offset: 0x000388AA
		// (set) Token: 0x06001430 RID: 5168 RVA: 0x0003A6C9 File Offset: 0x000388C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.y = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x0003A6FB File Offset: 0x000388FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.y, this.y);
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0003A71A File Offset: 0x0003891A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.y, this.z);
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0003A739 File Offset: 0x00038939
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.y, this.w);
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0003A758 File Offset: 0x00038958
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.z, this.x);
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0003A777 File Offset: 0x00038977
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.z, this.y);
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0003A796 File Offset: 0x00038996
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.z, this.z);
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x0003A7B5 File Offset: 0x000389B5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x0003A7D4 File Offset: 0x000389D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x0003A7F3 File Offset: 0x000389F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0003A812 File Offset: 0x00038A12
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x0003A831 File Offset: 0x00038A31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x0003A850 File Offset: 0x00038A50
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x0003A86F File Offset: 0x00038A6F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0003A88E File Offset: 0x00038A8E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0003A8AD File Offset: 0x00038AAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.x, this.w);
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0003A8CC File Offset: 0x00038ACC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x0003A8EB File Offset: 0x00038AEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0003A90A File Offset: 0x00038B0A
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x0003A929 File Offset: 0x00038B29
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.y = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x0003A95B File Offset: 0x00038B5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.y, this.w);
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0003A97A File Offset: 0x00038B7A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.z, this.x);
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x0003A999 File Offset: 0x00038B99
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x0003A9B8 File Offset: 0x00038BB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.z = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x0003A9EA File Offset: 0x00038BEA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x0003AA09 File Offset: 0x00038C09
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.z, this.w);
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x0003AA28 File Offset: 0x00038C28
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.w, this.x);
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0003AA47 File Offset: 0x00038C47
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.w, this.y);
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0003AA66 File Offset: 0x00038C66
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.w, this.z);
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0003AA85 File Offset: 0x00038C85
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wxww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.x, this.w, this.w);
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x0003AAA4 File Offset: 0x00038CA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x0003AAC3 File Offset: 0x00038CC3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x0003AAE2 File Offset: 0x00038CE2
		// (set) Token: 0x06001451 RID: 5201 RVA: 0x0003AB01 File Offset: 0x00038D01
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.x = value.z;
				this.z = value.w;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x0003AB33 File Offset: 0x00038D33
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.x, this.w);
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x0003AB52 File Offset: 0x00038D52
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x0003AB71 File Offset: 0x00038D71
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x0003AB90 File Offset: 0x00038D90
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0003ABAF File Offset: 0x00038DAF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.y, this.w);
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0003ABCE File Offset: 0x00038DCE
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x0003ABED File Offset: 0x00038DED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.z = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x0003AC1F File Offset: 0x00038E1F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.z, this.y);
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0003AC3E File Offset: 0x00038E3E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0003AC5D File Offset: 0x00038E5D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.z, this.w);
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0003AC7C File Offset: 0x00038E7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.w, this.x);
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0003AC9B File Offset: 0x00038E9B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.w, this.y);
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0003ACBA File Offset: 0x00038EBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.w, this.z);
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0003ACD9 File Offset: 0x00038ED9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wyww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.y, this.w, this.w);
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x0003ACF8 File Offset: 0x00038EF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x0003AD17 File Offset: 0x00038F17
		// (set) Token: 0x06001462 RID: 5218 RVA: 0x0003AD36 File Offset: 0x00038F36
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.x = value.z;
				this.y = value.w;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x0003AD68 File Offset: 0x00038F68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0003AD87 File Offset: 0x00038F87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.x, this.w);
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0003ADA6 File Offset: 0x00038FA6
		// (set) Token: 0x06001466 RID: 5222 RVA: 0x0003ADC5 File Offset: 0x00038FC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.y = value.z;
				this.x = value.w;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0003ADF7 File Offset: 0x00038FF7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0003AE16 File Offset: 0x00039016
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x0003AE35 File Offset: 0x00039035
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.y, this.w);
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x0003AE54 File Offset: 0x00039054
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x0003AE73 File Offset: 0x00039073
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.z, this.y);
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0003AE92 File Offset: 0x00039092
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x0003AEB1 File Offset: 0x000390B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.z, this.w);
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x0003AED0 File Offset: 0x000390D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.w, this.x);
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x0003AEEF File Offset: 0x000390EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.w, this.y);
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0003AF0E File Offset: 0x0003910E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.w, this.z);
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001471 RID: 5233 RVA: 0x0003AF2D File Offset: 0x0003912D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wzww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.z, this.w, this.w);
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0003AF4C File Offset: 0x0003914C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.x, this.x);
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x0003AF6B File Offset: 0x0003916B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.x, this.y);
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x0003AF8A File Offset: 0x0003918A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.x, this.z);
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0003AFA9 File Offset: 0x000391A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.x, this.w);
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x0003AFC8 File Offset: 0x000391C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.y, this.x);
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x0003AFE7 File Offset: 0x000391E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.y, this.y);
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0003B006 File Offset: 0x00039206
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.y, this.z);
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x0003B025 File Offset: 0x00039225
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.y, this.w);
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0003B044 File Offset: 0x00039244
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.z, this.x);
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0003B063 File Offset: 0x00039263
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.z, this.y);
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0003B082 File Offset: 0x00039282
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.z, this.z);
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x0600147D RID: 5245 RVA: 0x0003B0A1 File Offset: 0x000392A1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.z, this.w);
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x0003B0C0 File Offset: 0x000392C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.w, this.x);
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x0600147F RID: 5247 RVA: 0x0003B0DF File Offset: 0x000392DF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.w, this.y);
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x0003B0FE File Offset: 0x000392FE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.w, this.z);
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0003B11D File Offset: 0x0003931D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 wwww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.w, this.w, this.w, this.w);
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x0003B13C File Offset: 0x0003933C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0003B155 File Offset: 0x00039355
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x0003B16E File Offset: 0x0003936E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.z);
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0003B187 File Offset: 0x00039387
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.w);
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x0003B1A0 File Offset: 0x000393A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0003B1B9 File Offset: 0x000393B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.y);
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x0003B1D2 File Offset: 0x000393D2
		// (set) Token: 0x06001489 RID: 5257 RVA: 0x0003B1EB File Offset: 0x000393EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0003B211 File Offset: 0x00039411
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x0003B22A File Offset: 0x0003942A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x0003B250 File Offset: 0x00039450
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.z, this.x);
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0003B269 File Offset: 0x00039469
		// (set) Token: 0x0600148E RID: 5262 RVA: 0x0003B282 File Offset: 0x00039482
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0003B2A8 File Offset: 0x000394A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.z, this.z);
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x0003B2C1 File Offset: 0x000394C1
		// (set) Token: 0x06001491 RID: 5265 RVA: 0x0003B2DA File Offset: 0x000394DA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0003B300 File Offset: 0x00039500
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.w, this.x);
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0003B319 File Offset: 0x00039519
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x0003B332 File Offset: 0x00039532
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0003B358 File Offset: 0x00039558
		// (set) Token: 0x06001496 RID: 5270 RVA: 0x0003B371 File Offset: 0x00039571
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0003B397 File Offset: 0x00039597
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.w, this.w);
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0003B3B0 File Offset: 0x000395B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0003B3C9 File Offset: 0x000395C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x0003B3E2 File Offset: 0x000395E2
		// (set) Token: 0x0600149B RID: 5275 RVA: 0x0003B3FB File Offset: 0x000395FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0003B421 File Offset: 0x00039621
		// (set) Token: 0x0600149D RID: 5277 RVA: 0x0003B43A File Offset: 0x0003963A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0003B460 File Offset: 0x00039660
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.x);
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x0003B479 File Offset: 0x00039679
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.y);
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x0003B492 File Offset: 0x00039692
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.z);
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0003B4AB File Offset: 0x000396AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.w);
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x0003B4C4 File Offset: 0x000396C4
		// (set) Token: 0x060014A3 RID: 5283 RVA: 0x0003B4DD File Offset: 0x000396DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0003B503 File Offset: 0x00039703
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.z, this.y);
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0003B51C File Offset: 0x0003971C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.z, this.z);
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0003B535 File Offset: 0x00039735
		// (set) Token: 0x060014A7 RID: 5287 RVA: 0x0003B54E File Offset: 0x0003974E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0003B574 File Offset: 0x00039774
		// (set) Token: 0x060014A9 RID: 5289 RVA: 0x0003B58D File Offset: 0x0003978D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 ywx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x0003B5B3 File Offset: 0x000397B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 ywy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.w, this.y);
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x0003B5CC File Offset: 0x000397CC
		// (set) Token: 0x060014AC RID: 5292 RVA: 0x0003B5E5 File Offset: 0x000397E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 ywz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0003B60B File Offset: 0x0003980B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.w, this.w);
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x0003B624 File Offset: 0x00039824
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.x, this.x);
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0003B63D File Offset: 0x0003983D
		// (set) Token: 0x060014B0 RID: 5296 RVA: 0x0003B656 File Offset: 0x00039856
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0003B67C File Offset: 0x0003987C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.x, this.z);
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0003B695 File Offset: 0x00039895
		// (set) Token: 0x060014B3 RID: 5299 RVA: 0x0003B6AE File Offset: 0x000398AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x0003B6D4 File Offset: 0x000398D4
		// (set) Token: 0x060014B5 RID: 5301 RVA: 0x0003B6ED File Offset: 0x000398ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0003B713 File Offset: 0x00039913
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.y, this.y);
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0003B72C File Offset: 0x0003992C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.y, this.z);
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0003B745 File Offset: 0x00039945
		// (set) Token: 0x060014B9 RID: 5305 RVA: 0x0003B75E File Offset: 0x0003995E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.w = value.z;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x0003B784 File Offset: 0x00039984
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.z, this.x);
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x0003B79D File Offset: 0x0003999D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.z, this.y);
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060014BC RID: 5308 RVA: 0x0003B7B6 File Offset: 0x000399B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.z, this.z);
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x0003B7CF File Offset: 0x000399CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.z, this.w);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x0003B7E8 File Offset: 0x000399E8
		// (set) Token: 0x060014BF RID: 5311 RVA: 0x0003B801 File Offset: 0x00039A01
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0003B827 File Offset: 0x00039A27
		// (set) Token: 0x060014C1 RID: 5313 RVA: 0x0003B840 File Offset: 0x00039A40
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0003B866 File Offset: 0x00039A66
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.w, this.z);
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0003B87F File Offset: 0x00039A7F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.w, this.w);
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0003B898 File Offset: 0x00039A98
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.x, this.x);
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x0003B8B1 File Offset: 0x00039AB1
		// (set) Token: 0x060014C6 RID: 5318 RVA: 0x0003B8CA File Offset: 0x00039ACA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060014C7 RID: 5319 RVA: 0x0003B8F0 File Offset: 0x00039AF0
		// (set) Token: 0x060014C8 RID: 5320 RVA: 0x0003B909 File Offset: 0x00039B09
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x0003B92F File Offset: 0x00039B2F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wxw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.x, this.w);
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0003B948 File Offset: 0x00039B48
		// (set) Token: 0x060014CB RID: 5323 RVA: 0x0003B961 File Offset: 0x00039B61
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x0003B987 File Offset: 0x00039B87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.y, this.y);
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x0003B9A0 File Offset: 0x00039BA0
		// (set) Token: 0x060014CE RID: 5326 RVA: 0x0003B9B9 File Offset: 0x00039BB9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060014CF RID: 5327 RVA: 0x0003B9DF File Offset: 0x00039BDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wyw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.y, this.w);
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0003B9F8 File Offset: 0x00039BF8
		// (set) Token: 0x060014D1 RID: 5329 RVA: 0x0003BA11 File Offset: 0x00039C11
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060014D2 RID: 5330 RVA: 0x0003BA37 File Offset: 0x00039C37
		// (set) Token: 0x060014D3 RID: 5331 RVA: 0x0003BA50 File Offset: 0x00039C50
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0003BA76 File Offset: 0x00039C76
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.z, this.z);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x0003BA8F File Offset: 0x00039C8F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wzw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.z, this.w);
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x0003BAA8 File Offset: 0x00039CA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wwx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.w, this.x);
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x0003BAC1 File Offset: 0x00039CC1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wwy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.w, this.y);
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x0003BADA File Offset: 0x00039CDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 wwz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.w, this.z);
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x0003BAF3 File Offset: 0x00039CF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 www
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.w, this.w, this.w);
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0003BB0C File Offset: 0x00039D0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.x, this.x);
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x0003BB1F File Offset: 0x00039D1F
		// (set) Token: 0x060014DC RID: 5340 RVA: 0x0003BB32 File Offset: 0x00039D32
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

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x0003BB4C File Offset: 0x00039D4C
		// (set) Token: 0x060014DE RID: 5342 RVA: 0x0003BB5F File Offset: 0x00039D5F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x0003BB79 File Offset: 0x00039D79
		// (set) Token: 0x060014E0 RID: 5344 RVA: 0x0003BB8C File Offset: 0x00039D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 xw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.x, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x0003BBA6 File Offset: 0x00039DA6
		// (set) Token: 0x060014E2 RID: 5346 RVA: 0x0003BBB9 File Offset: 0x00039DB9
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

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x0003BBD3 File Offset: 0x00039DD3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.y, this.y);
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0003BBE6 File Offset: 0x00039DE6
		// (set) Token: 0x060014E5 RID: 5349 RVA: 0x0003BBF9 File Offset: 0x00039DF9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x0003BC13 File Offset: 0x00039E13
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x0003BC26 File Offset: 0x00039E26
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 yw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.y, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0003BC40 File Offset: 0x00039E40
		// (set) Token: 0x060014E9 RID: 5353 RVA: 0x0003BC53 File Offset: 0x00039E53
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0003BC6D File Offset: 0x00039E6D
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x0003BC80 File Offset: 0x00039E80
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0003BC9A File Offset: 0x00039E9A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.z, this.z);
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0003BCAD File Offset: 0x00039EAD
		// (set) Token: 0x060014EE RID: 5358 RVA: 0x0003BCC0 File Offset: 0x00039EC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 zw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.z, this.w);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.w = value.y;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0003BCDA File Offset: 0x00039EDA
		// (set) Token: 0x060014F0 RID: 5360 RVA: 0x0003BCED File Offset: 0x00039EED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 wx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.w, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0003BD07 File Offset: 0x00039F07
		// (set) Token: 0x060014F2 RID: 5362 RVA: 0x0003BD1A File Offset: 0x00039F1A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 wy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.w, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0003BD34 File Offset: 0x00039F34
		// (set) Token: 0x060014F4 RID: 5364 RVA: 0x0003BD47 File Offset: 0x00039F47
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 wz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.w, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.w = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x0003BD61 File Offset: 0x00039F61
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 ww
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.w, this.w);
			}
		}

		// Token: 0x170005C4 RID: 1476
		public unsafe float this[int index]
		{
			get
			{
				fixed (float4* ptr = &this)
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

		// Token: 0x060014F8 RID: 5368 RVA: 0x0003BDAC File Offset: 0x00039FAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float4 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z && this.w == rhs.w;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0003BDE8 File Offset: 0x00039FE8
		public override bool Equals(object o)
		{
			if (o is float4)
			{
				float4 @float = (float4)o;
				return this.Equals(@float);
			}
			return false;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0003BE0D File Offset: 0x0003A00D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0003BE1C File Offset: 0x0003A01C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float4({0}f, {1}f, {2}f, {3}f)", new object[] { this.x, this.y, this.z, this.w });
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0003BE74 File Offset: 0x0003A074
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float4({0}f, {1}f, {2}f, {3}f)", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.z.ToString(format, formatProvider),
				this.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0003BED1 File Offset: 0x0003A0D1
		public static implicit operator float4(Vector4 v)
		{
			return new float4(v.x, v.y, v.z, v.w);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0003BEF0 File Offset: 0x0003A0F0
		public static implicit operator Vector4(float4 v)
		{
			return new Vector4(v.x, v.y, v.z, v.w);
		}

		// Token: 0x0400009D RID: 157
		public float x;

		// Token: 0x0400009E RID: 158
		public float y;

		// Token: 0x0400009F RID: 159
		public float z;

		// Token: 0x040000A0 RID: 160
		public float w;

		// Token: 0x040000A1 RID: 161
		public static readonly float4 zero;

		// Token: 0x0200005D RID: 93
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024CA RID: 9418 RVA: 0x00069250 File Offset: 0x00067450
			public DebuggerProxy(float4 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
				this.w = v.w;
			}

			// Token: 0x04000161 RID: 353
			public float x;

			// Token: 0x04000162 RID: 354
			public float y;

			// Token: 0x04000163 RID: 355
			public float z;

			// Token: 0x04000164 RID: 356
			public float w;
		}
	}
}
