using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics
{
	// Token: 0x0200003A RID: 58
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct quaternion : IEquatable<quaternion>, IFormattable
	{
		// Token: 0x06001E2C RID: 7724 RVA: 0x000528DF File Offset: 0x00050ADF
		public static implicit operator Quaternion(quaternion q)
		{
			return new Quaternion(q.value.x, q.value.y, q.value.z, q.value.w);
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00052912 File Offset: 0x00050B12
		public static implicit operator quaternion(Quaternion q)
		{
			return new quaternion(q.x, q.y, q.z, q.w);
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00052931 File Offset: 0x00050B31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public quaternion(float x, float y, float z, float w)
		{
			this.value.x = x;
			this.value.y = y;
			this.value.z = z;
			this.value.w = w;
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x00052964 File Offset: 0x00050B64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public quaternion(float4 value)
		{
			this.value = value;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0005296D File Offset: 0x00050B6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator quaternion(float4 v)
		{
			return new quaternion(v);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x00052978 File Offset: 0x00050B78
		public quaternion(float3x3 m)
		{
			float3 c = m.c0;
			float3 c2 = m.c1;
			float3 c3 = m.c2;
			uint num = math.asuint(c.x) & 2147483648U;
			float num2 = c2.y + math.asfloat(math.asuint(c3.z) ^ num);
			uint4 @uint = math.uint4((int)num >> 31);
			uint4 uint2 = math.uint4(math.asint(num2) >> 31);
			float num3 = 1f + math.abs(c.x);
			uint4 uint3 = math.uint4(0U, 2147483648U, 2147483648U, 2147483648U) ^ (@uint & math.uint4(0U, 2147483648U, 0U, 2147483648U)) ^ (uint2 & math.uint4(2147483648U, 2147483648U, 2147483648U, 0U));
			this.value = math.float4(num3, c.y, c3.x, c2.z) + math.asfloat(math.asuint(math.float4(num2, c2.x, c.z, c3.y)) ^ uint3);
			this.value = math.asfloat((math.asuint(this.value) & ~@uint) | (math.asuint(this.value.zwxy) & @uint));
			this.value = math.asfloat((math.asuint(this.value.wzyx) & ~uint2) | (math.asuint(this.value) & uint2));
			this.value = math.normalize(this.value);
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x00052B28 File Offset: 0x00050D28
		public quaternion(float4x4 m)
		{
			float4 c = m.c0;
			float4 c2 = m.c1;
			float4 c3 = m.c2;
			uint num = math.asuint(c.x) & 2147483648U;
			float num2 = c2.y + math.asfloat(math.asuint(c3.z) ^ num);
			uint4 @uint = math.uint4((int)num >> 31);
			uint4 uint2 = math.uint4(math.asint(num2) >> 31);
			float num3 = 1f + math.abs(c.x);
			uint4 uint3 = math.uint4(0U, 2147483648U, 2147483648U, 2147483648U) ^ (@uint & math.uint4(0U, 2147483648U, 0U, 2147483648U)) ^ (uint2 & math.uint4(2147483648U, 2147483648U, 2147483648U, 0U));
			this.value = math.float4(num3, c.y, c3.x, c2.z) + math.asfloat(math.asuint(math.float4(num2, c2.x, c.z, c3.y)) ^ uint3);
			this.value = math.asfloat((math.asuint(this.value) & ~@uint) | (math.asuint(this.value.zwxy) & @uint));
			this.value = math.asfloat((math.asuint(this.value.wzyx) & ~uint2) | (math.asuint(this.value) & uint2));
			this.value = math.normalize(this.value);
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x00052CD8 File Offset: 0x00050ED8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion AxisAngle(float3 axis, float angle)
		{
			float num;
			float num2;
			math.sincos(0.5f * angle, out num, out num2);
			return math.quaternion(math.float4(axis * num, num2));
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x00052D08 File Offset: 0x00050F08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerXYZ(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(0.5f * xyz, out @float, out float2);
			return math.quaternion(math.float4(@float.xyz, float2.x) * float2.yxxy * float2.zzyz + @float.yxxy * @float.zzyz * math.float4(float2.xyz, @float.x) * math.float4(-1f, 1f, -1f, 1f));
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x00052DA8 File Offset: 0x00050FA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerXZY(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(0.5f * xyz, out @float, out float2);
			return math.quaternion(math.float4(@float.xyz, float2.x) * float2.yxxy * float2.zzyz + @float.yxxy * @float.zzyz * math.float4(float2.xyz, @float.x) * math.float4(1f, 1f, -1f, -1f));
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x00052E48 File Offset: 0x00051048
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerYXZ(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(0.5f * xyz, out @float, out float2);
			return math.quaternion(math.float4(@float.xyz, float2.x) * float2.yxxy * float2.zzyz + @float.yxxy * @float.zzyz * math.float4(float2.xyz, @float.x) * math.float4(-1f, 1f, 1f, -1f));
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x00052EE8 File Offset: 0x000510E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerYZX(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(0.5f * xyz, out @float, out float2);
			return math.quaternion(math.float4(@float.xyz, float2.x) * float2.yxxy * float2.zzyz + @float.yxxy * @float.zzyz * math.float4(float2.xyz, @float.x) * math.float4(-1f, -1f, 1f, 1f));
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x00052F88 File Offset: 0x00051188
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerZXY(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(0.5f * xyz, out @float, out float2);
			return math.quaternion(math.float4(@float.xyz, float2.x) * float2.yxxy * float2.zzyz + @float.yxxy * @float.zzyz * math.float4(float2.xyz, @float.x) * math.float4(1f, -1f, -1f, 1f));
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x00053028 File Offset: 0x00051228
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerZYX(float3 xyz)
		{
			float3 @float;
			float3 float2;
			math.sincos(0.5f * xyz, out @float, out float2);
			return math.quaternion(math.float4(@float.xyz, float2.x) * float2.yxxy * float2.zzyz + @float.yxxy * @float.zzyz * math.float4(float2.xyz, @float.x) * math.float4(1f, -1f, 1f, -1f));
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000530C5 File Offset: 0x000512C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerXYZ(float x, float y, float z)
		{
			return quaternion.EulerXYZ(math.float3(x, y, z));
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000530D4 File Offset: 0x000512D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerXZY(float x, float y, float z)
		{
			return quaternion.EulerXZY(math.float3(x, y, z));
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000530E3 File Offset: 0x000512E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerYXZ(float x, float y, float z)
		{
			return quaternion.EulerYXZ(math.float3(x, y, z));
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000530F2 File Offset: 0x000512F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerYZX(float x, float y, float z)
		{
			return quaternion.EulerYZX(math.float3(x, y, z));
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x00053101 File Offset: 0x00051301
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerZXY(float x, float y, float z)
		{
			return quaternion.EulerZXY(math.float3(x, y, z));
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x00053110 File Offset: 0x00051310
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion EulerZYX(float x, float y, float z)
		{
			return quaternion.EulerZYX(math.float3(x, y, z));
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00053120 File Offset: 0x00051320
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			switch (order)
			{
			case math.RotationOrder.XYZ:
				return quaternion.EulerXYZ(xyz);
			case math.RotationOrder.XZY:
				return quaternion.EulerXZY(xyz);
			case math.RotationOrder.YXZ:
				return quaternion.EulerYXZ(xyz);
			case math.RotationOrder.YZX:
				return quaternion.EulerYZX(xyz);
			case math.RotationOrder.ZXY:
				return quaternion.EulerZXY(xyz);
			case math.RotationOrder.ZYX:
				return quaternion.EulerZYX(xyz);
			default:
				return quaternion.identity;
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0005317C File Offset: 0x0005137C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion Euler(float x, float y, float z, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			return quaternion.Euler(math.float3(x, y, z), order);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0005318C File Offset: 0x0005138C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion RotateX(float angle)
		{
			float num;
			float num2;
			math.sincos(0.5f * angle, out num, out num2);
			return math.quaternion(num, 0f, 0f, num2);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000531BC File Offset: 0x000513BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion RotateY(float angle)
		{
			float num;
			float num2;
			math.sincos(0.5f * angle, out num, out num2);
			return math.quaternion(0f, num, 0f, num2);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000531EC File Offset: 0x000513EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion RotateZ(float angle)
		{
			float num;
			float num2;
			math.sincos(0.5f * angle, out num, out num2);
			return math.quaternion(0f, 0f, num, num2);
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0005321C File Offset: 0x0005141C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion LookRotation(float3 forward, float3 up)
		{
			float3 @float = math.normalize(math.cross(up, forward));
			return math.quaternion(math.float3x3(@float, math.cross(forward, @float), forward));
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0005324C File Offset: 0x0005144C
		public static quaternion LookRotationSafe(float3 forward, float3 up)
		{
			float num = math.dot(forward, forward);
			float num2 = math.dot(up, up);
			forward *= math.rsqrt(num);
			up *= math.rsqrt(num2);
			float3 @float = math.cross(up, forward);
			float num3 = math.dot(@float, @float);
			@float *= math.rsqrt(num3);
			float num4 = math.min(math.min(num, num2), num3);
			float num5 = math.max(math.max(num, num2), num3);
			bool flag = num4 > 1E-35f && num5 < 1E+35f && math.isfinite(num) && math.isfinite(num2) && math.isfinite(num3);
			return math.quaternion(math.select(math.float4(0f, 0f, 0f, 1f), math.quaternion(math.float3x3(@float, math.cross(forward, @float), forward)).value, flag));
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00053328 File Offset: 0x00051528
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(quaternion x)
		{
			return this.value.x == x.value.x && this.value.y == x.value.y && this.value.z == x.value.z && this.value.w == x.value.w;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x00053398 File Offset: 0x00051598
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object x)
		{
			if (x is quaternion)
			{
				quaternion quaternion = (quaternion)x;
				return this.Equals(quaternion);
			}
			return false;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000533BD File Offset: 0x000515BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000533CC File Offset: 0x000515CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("quaternion({0}f, {1}f, {2}f, {3}f)", new object[]
			{
				this.value.x,
				this.value.y,
				this.value.z,
				this.value.w
			});
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x00053438 File Offset: 0x00051638
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("quaternion({0}f, {1}f, {2}f, {3}f)", new object[]
			{
				this.value.x.ToString(format, formatProvider),
				this.value.y.ToString(format, formatProvider),
				this.value.z.ToString(format, formatProvider),
				this.value.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000F0 RID: 240
		public float4 value;

		// Token: 0x040000F1 RID: 241
		public static readonly quaternion identity = new quaternion(0f, 0f, 0f, 1f);
	}
}
