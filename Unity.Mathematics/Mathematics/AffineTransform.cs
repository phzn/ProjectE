using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000004 RID: 4
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct AffineTransform : IEquatable<AffineTransform>, IFormattable
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020CB File Offset: 0x000002CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AffineTransform(float3 translation, quaternion rotation)
		{
			this.rs = math.float3x3(rotation);
			this.t = translation;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020E0 File Offset: 0x000002E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AffineTransform(float3 translation, quaternion rotation, float3 scale)
		{
			this.rs = math.mulScale(math.float3x3(rotation), scale);
			this.t = translation;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020FB File Offset: 0x000002FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AffineTransform(float3 translation, float3x3 rotationScale)
		{
			this.rs = rotationScale;
			this.t = translation;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000210B File Offset: 0x0000030B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AffineTransform(float3x3 rotationScale)
		{
			this.rs = rotationScale;
			this.t = float3.zero;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000211F File Offset: 0x0000031F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AffineTransform(RigidTransform rigid)
		{
			this.rs = math.float3x3(rigid.rot);
			this.t = rigid.pos;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000213E File Offset: 0x0000033E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AffineTransform(float3x4 m)
		{
			this.rs = math.float3x3(m.c0, m.c1, m.c2);
			this.t = m.c3;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000216C File Offset: 0x0000036C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AffineTransform(float4x4 m)
		{
			this.rs = math.float3x3(m.c0.xyz, m.c1.xyz, m.c2.xyz);
			this.t = m.c3.xyz;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021BA File Offset: 0x000003BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3x4(AffineTransform m)
		{
			return math.float3x4(m.rs.c0, m.rs.c1, m.rs.c2, m.t);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021E8 File Offset: 0x000003E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float4x4(AffineTransform m)
		{
			return math.float4x4(math.float4(m.rs.c0, 0f), math.float4(m.rs.c1, 0f), math.float4(m.rs.c2, 0f), math.float4(m.t, 1f));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002249 File Offset: 0x00000449
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(AffineTransform rhs)
		{
			return this.rs.Equals(rhs.rs) && this.t.Equals(rhs.t);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002274 File Offset: 0x00000474
		public override bool Equals(object o)
		{
			if (o is AffineTransform)
			{
				AffineTransform affineTransform = (AffineTransform)o;
				return this.Equals(affineTransform);
			}
			return false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002299 File Offset: 0x00000499
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022A8 File Offset: 0x000004A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("AffineTransform(({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f,  {6}f, {7}f, {8}f), ({9}f, {10}f, {11}f))", new object[]
			{
				this.rs.c0.x,
				this.rs.c1.x,
				this.rs.c2.x,
				this.rs.c0.y,
				this.rs.c1.y,
				this.rs.c2.y,
				this.rs.c0.z,
				this.rs.c1.z,
				this.rs.c2.z,
				this.t.x,
				this.t.y,
				this.t.z
			});
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023DC File Offset: 0x000005DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("AffineTransform(({0}f, {1}f, {2}f,  {3}f, {4}f, {5}f,  {6}f, {7}f, {8}f), ({9}f, {10}f, {11}f))", new object[]
			{
				this.rs.c0.x.ToString(format, formatProvider),
				this.rs.c1.x.ToString(format, formatProvider),
				this.rs.c2.x.ToString(format, formatProvider),
				this.rs.c0.y.ToString(format, formatProvider),
				this.rs.c1.y.ToString(format, formatProvider),
				this.rs.c2.y.ToString(format, formatProvider),
				this.rs.c0.z.ToString(format, formatProvider),
				this.rs.c1.z.ToString(format, formatProvider),
				this.rs.c2.z.ToString(format, formatProvider),
				this.t.x.ToString(format, formatProvider),
				this.t.y.ToString(format, formatProvider),
				this.t.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000001 RID: 1
		public float3x3 rs;

		// Token: 0x04000002 RID: 2
		public float3 t;

		// Token: 0x04000003 RID: 3
		public static readonly AffineTransform identity = new AffineTransform(float3.zero, float3x3.identity);

		// Token: 0x04000004 RID: 4
		public static readonly AffineTransform zero;
	}
}
