using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000005 RID: 5
	[Il2CppEagerStaticClassConstruction]
	public static class math
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000253C File Offset: 0x0000073C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform AffineTransform(float3 translation, quaternion rotation)
		{
			return new AffineTransform(translation, rotation);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002545 File Offset: 0x00000745
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform AffineTransform(float3 translation, quaternion rotation, float3 scale)
		{
			return new AffineTransform(translation, rotation, scale);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000254F File Offset: 0x0000074F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform AffineTransform(float3 translation, float3x3 rotationScale)
		{
			return new AffineTransform(translation, rotationScale);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002558 File Offset: 0x00000758
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform AffineTransform(float3x3 rotationScale)
		{
			return new AffineTransform(rotationScale);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002560 File Offset: 0x00000760
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform AffineTransform(float4x4 m)
		{
			return new AffineTransform(m);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002568 File Offset: 0x00000768
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform AffineTransform(float3x4 m)
		{
			return new AffineTransform(m);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002570 File Offset: 0x00000770
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform AffineTransform(RigidTransform rigid)
		{
			return new AffineTransform(rigid);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002578 File Offset: 0x00000778
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(AffineTransform transform)
		{
			return math.float4x4(math.float4(transform.rs.c0, 0f), math.float4(transform.rs.c1, 0f), math.float4(transform.rs.c2, 0f), math.float4(transform.t, 1f));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025D9 File Offset: 0x000007D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(AffineTransform transform)
		{
			return math.float3x4(transform.rs.c0, transform.rs.c1, transform.rs.c2, transform.t);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002607 File Offset: 0x00000807
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform mul(AffineTransform a, AffineTransform b)
		{
			return new AffineTransform(math.transform(a, b.t), math.mul(a.rs, b.rs));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000262B File Offset: 0x0000082B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform mul(float3x3 a, AffineTransform b)
		{
			return new AffineTransform(math.mul(a, b.t), math.mul(a, b.rs));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000264A File Offset: 0x0000084A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform mul(AffineTransform a, float3x3 b)
		{
			return new AffineTransform(a.t, math.mul(b, a.rs));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002663 File Offset: 0x00000863
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(AffineTransform a, float4 pos)
		{
			return math.float4(math.mul(a.rs, pos.xyz) + a.t * pos.w, pos.w);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002698 File Offset: 0x00000898
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 rotate(AffineTransform a, float3 dir)
		{
			return math.mul(a.rs, dir);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026A6 File Offset: 0x000008A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 transform(AffineTransform a, float3 pos)
		{
			return a.t + math.mul(a.rs, pos);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026C0 File Offset: 0x000008C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AffineTransform inverse(AffineTransform a)
		{
			AffineTransform affineTransform;
			affineTransform.rs = math.pseudoinverse(a.rs);
			affineTransform.t = math.mul(affineTransform.rs, -a.t);
			return affineTransform;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002700 File Offset: 0x00000900
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void decompose(AffineTransform a, out float3 translation, out quaternion rotation, out float3 scale)
		{
			translation = a.t;
			rotation = math.rotation(a.rs);
			float3x3 float3x = math.mul(math.float3x3(math.conjugate(rotation)), a.rs);
			scale = math.float3(float3x.c0.x, float3x.c1.y, float3x.c2.z);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002772 File Offset: 0x00000972
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(AffineTransform a)
		{
			return math.hash(a.rs) + 3318036811U * math.hash(a.t);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002794 File Offset: 0x00000994
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(AffineTransform a)
		{
			return math.hashwide(a.rs).xyzz + 3318036811U * math.hashwide(a.t).xyzz;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027D6 File Offset: 0x000009D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 bool2(bool x, bool y)
		{
			return new bool2(x, y);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000027DF File Offset: 0x000009DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 bool2(bool2 xy)
		{
			return new bool2(xy);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027E7 File Offset: 0x000009E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 bool2(bool v)
		{
			return new bool2(v);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000027EF File Offset: 0x000009EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool2 v)
		{
			return math.csum(math.select(math.uint2(2426570171U, 1561977301U), math.uint2(4205774813U, 1650214333U), v));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000281A File Offset: 0x00000A1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(bool2 v)
		{
			return math.select(math.uint2(3388112843U, 1831150513U), math.uint2(1848374953U, 3430200247U), v);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002840 File Offset: 0x00000A40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool shuffle(bool2 left, bool2 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000284A File Offset: 0x00000A4A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 shuffle(bool2 left, bool2 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.bool2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002861 File Offset: 0x00000A61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 shuffle(bool2 left, bool2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.bool3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002881 File Offset: 0x00000A81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 shuffle(bool2 left, bool2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.bool4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028AC File Offset: 0x00000AAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool select_shuffle_component(bool2 a, bool2 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002911 File Offset: 0x00000B11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 bool2x2(bool2 c0, bool2 c1)
		{
			return new bool2x2(c0, c1);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000291A File Offset: 0x00000B1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 bool2x2(bool m00, bool m01, bool m10, bool m11)
		{
			return new bool2x2(m00, m01, m10, m11);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002925 File Offset: 0x00000B25
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 bool2x2(bool v)
		{
			return new bool2x2(v);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000292D File Offset: 0x00000B2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 transpose(bool2x2 v)
		{
			return math.bool2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002960 File Offset: 0x00000B60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool2x2 v)
		{
			return math.csum(math.select(math.uint2(2062756937U, 2920485769U), math.uint2(1562056283U, 2265541847U), v.c0) + math.select(math.uint2(1283419601U, 1210229737U), math.uint2(2864955997U, 3525118277U), v.c1));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029CC File Offset: 0x00000BCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(bool2x2 v)
		{
			return math.select(math.uint2(2298260269U, 1632478733U), math.uint2(1537393931U, 2353355467U), v.c0) + math.select(math.uint2(3441847433U, 4052036147U), math.uint2(2011389559U, 2252224297U), v.c1);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A30 File Offset: 0x00000C30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 bool2x3(bool2 c0, bool2 c1, bool2 c2)
		{
			return new bool2x3(c0, c1, c2);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A3A File Offset: 0x00000C3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 bool2x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12)
		{
			return new bool2x3(m00, m01, m02, m10, m11, m12);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A49 File Offset: 0x00000C49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 bool2x3(bool v)
		{
			return new bool2x3(v);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A54 File Offset: 0x00000C54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 transpose(bool2x3 v)
		{
			return math.bool3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002AA8 File Offset: 0x00000CA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool2x3 v)
		{
			return math.csum(math.select(math.uint2(2078515003U, 4206465343U), math.uint2(3025146473U, 3763046909U), v.c0) + math.select(math.uint2(3678265601U, 2070747979U), math.uint2(1480171127U, 1588341193U), v.c1) + math.select(math.uint2(4234155257U, 1811310911U), math.uint2(2635799963U, 4165137857U), v.c2));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B40 File Offset: 0x00000D40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(bool2x3 v)
		{
			return math.select(math.uint2(2759770933U, 2759319383U), math.uint2(3299952959U, 3121178323U), v.c0) + math.select(math.uint2(2948522579U, 1531026433U), math.uint2(1365086453U, 3969870067U), v.c1) + math.select(math.uint2(4192899797U, 3271228601U), math.uint2(1634639009U, 3318036811U), v.c2);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002BD2 File Offset: 0x00000DD2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 bool2x4(bool2 c0, bool2 c1, bool2 c2, bool2 c3)
		{
			return new bool2x4(c0, c1, c2, c3);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002BDD File Offset: 0x00000DDD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 bool2x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13)
		{
			return new bool2x4(m00, m01, m02, m03, m10, m11, m12, m13);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002BF0 File Offset: 0x00000DF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 bool2x4(bool v)
		{
			return new bool2x4(v);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002BF8 File Offset: 0x00000DF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 transpose(bool2x4 v)
		{
			return math.bool4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002C64 File Offset: 0x00000E64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool2x4 v)
		{
			return math.csum(math.select(math.uint2(1168253063U, 4228926523U), math.uint2(1610574617U, 1584185147U), v.c0) + math.select(math.uint2(3041325733U, 3150930919U), math.uint2(3309258581U, 1770373673U), v.c1) + math.select(math.uint2(3778261171U, 3286279097U), math.uint2(4264629071U, 1898591447U), v.c2) + math.select(math.uint2(2641864091U, 1229113913U), math.uint2(3020867117U, 1449055807U), v.c3));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002D2C File Offset: 0x00000F2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(bool2x4 v)
		{
			return math.select(math.uint2(2479033387U, 3702457169U), math.uint2(1845824257U, 1963973621U), v.c0) + math.select(math.uint2(2134758553U, 1391111867U), math.uint2(1167706003U, 2209736489U), v.c1) + math.select(math.uint2(3261535807U, 1740411209U), math.uint2(2910609089U, 2183822701U), v.c2) + math.select(math.uint2(3029516053U, 3547472099U), math.uint2(2057487037U, 3781937309U), v.c3);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002DEC File Offset: 0x00000FEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 bool3(bool x, bool y, bool z)
		{
			return new bool3(x, y, z);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002DF6 File Offset: 0x00000FF6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 bool3(bool x, bool2 yz)
		{
			return new bool3(x, yz);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002DFF File Offset: 0x00000FFF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 bool3(bool2 xy, bool z)
		{
			return new bool3(xy, z);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002E08 File Offset: 0x00001008
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 bool3(bool3 xyz)
		{
			return new bool3(xyz);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002E10 File Offset: 0x00001010
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 bool3(bool v)
		{
			return new bool3(v);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002E18 File Offset: 0x00001018
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool3 v)
		{
			return math.csum(math.select(math.uint3(2716413241U, 1166264321U, 2503385333U), math.uint3(2944493077U, 2599999021U, 3814721321U), v));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E4D File Offset: 0x0000104D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(bool3 v)
		{
			return math.select(math.uint3(1595355149U, 1728931849U, 2062756937U), math.uint3(2920485769U, 1562056283U, 2265541847U), v);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002E7D File Offset: 0x0000107D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool shuffle(bool3 left, bool3 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002E87 File Offset: 0x00001087
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 shuffle(bool3 left, bool3 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.bool2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002E9E File Offset: 0x0000109E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 shuffle(bool3 left, bool3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.bool3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002EBE File Offset: 0x000010BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 shuffle(bool3 left, bool3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.bool4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002EE8 File Offset: 0x000010E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool select_shuffle_component(bool3 a, bool3 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002F5F File Offset: 0x0000115F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 bool3x2(bool3 c0, bool3 c1)
		{
			return new bool3x2(c0, c1);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002F68 File Offset: 0x00001168
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 bool3x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21)
		{
			return new bool3x2(m00, m01, m10, m11, m20, m21);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002F77 File Offset: 0x00001177
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x2 bool3x2(bool v)
		{
			return new bool3x2(v);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F80 File Offset: 0x00001180
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 transpose(bool3x2 v)
		{
			return math.bool2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002FD4 File Offset: 0x000011D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool3x2 v)
		{
			return math.csum(math.select(math.uint3(2627668003U, 1520214331U, 2949502447U), math.uint3(2827819133U, 3480140317U, 2642994593U), v.c0) + math.select(math.uint3(3940484981U, 1954192763U, 1091696537U), math.uint3(3052428017U, 4253034763U, 2338696631U), v.c1));
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003054 File Offset: 0x00001254
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(bool3x2 v)
		{
			return math.select(math.uint3(3757372771U, 1885959949U, 3508684087U), math.uint3(3919501043U, 1209161033U, 4007793211U), v.c0) + math.select(math.uint3(3819806693U, 3458005183U, 2078515003U), math.uint3(4206465343U, 3025146473U, 3763046909U), v.c1);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000030CC File Offset: 0x000012CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 bool3x3(bool3 c0, bool3 c1, bool3 c2)
		{
			return new bool3x3(c0, c1, c2);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000030D8 File Offset: 0x000012D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 bool3x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12, bool m20, bool m21, bool m22)
		{
			return new bool3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000030F8 File Offset: 0x000012F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 bool3x3(bool v)
		{
			return new bool3x3(v);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003100 File Offset: 0x00001300
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 transpose(bool3x3 v)
		{
			return math.bool3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003178 File Offset: 0x00001378
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool3x3 v)
		{
			return math.csum(math.select(math.uint3(3881277847U, 4017968839U, 1727237899U), math.uint3(1648514723U, 1385344481U, 3538260197U), v.c0) + math.select(math.uint3(4066109527U, 2613148903U, 3367528529U), math.uint3(1678332449U, 2918459647U, 2744611081U), v.c1) + math.select(math.uint3(1952372791U, 2631698677U, 4200781601U), math.uint3(2119021007U, 1760485621U, 3157985881U), v.c2));
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003230 File Offset: 0x00001430
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(bool3x3 v)
		{
			return math.select(math.uint3(2171534173U, 2723054263U, 1168253063U), math.uint3(4228926523U, 1610574617U, 1584185147U), v.c0) + math.select(math.uint3(3041325733U, 3150930919U, 3309258581U), math.uint3(1770373673U, 3778261171U, 3286279097U), v.c1) + math.select(math.uint3(4264629071U, 1898591447U, 2641864091U), math.uint3(1229113913U, 3020867117U, 1449055807U), v.c2);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000032E0 File Offset: 0x000014E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 bool3x4(bool3 c0, bool3 c1, bool3 c2, bool3 c3)
		{
			return new bool3x4(c0, c1, c2, c3);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000032EC File Offset: 0x000014EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 bool3x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23)
		{
			return new bool3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003312 File Offset: 0x00001512
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 bool3x4(bool v)
		{
			return new bool3x4(v);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000331C File Offset: 0x0000151C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 transpose(bool3x4 v)
		{
			return math.bool4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000033B4 File Offset: 0x000015B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool3x4 v)
		{
			return math.csum(math.select(math.uint3(2209710647U, 2201894441U, 2849577407U), math.uint3(3287031191U, 3098675399U, 1564399943U), v.c0) + math.select(math.uint3(1148435377U, 3416333663U, 1750611407U), math.uint3(3285396193U, 3110507567U, 4271396531U), v.c1) + math.select(math.uint3(4198118021U, 2908068253U, 3705492289U), math.uint3(2497566569U, 2716413241U, 1166264321U), v.c2) + math.select(math.uint3(2503385333U, 2944493077U, 2599999021U), math.uint3(3814721321U, 1595355149U, 1728931849U), v.c3));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000034A4 File Offset: 0x000016A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(bool3x4 v)
		{
			return math.select(math.uint3(2062756937U, 2920485769U, 1562056283U), math.uint3(2265541847U, 1283419601U, 1210229737U), v.c0) + math.select(math.uint3(2864955997U, 3525118277U, 2298260269U), math.uint3(1632478733U, 1537393931U, 2353355467U), v.c1) + math.select(math.uint3(3441847433U, 4052036147U, 2011389559U), math.uint3(2252224297U, 3784421429U, 1750626223U), v.c2) + math.select(math.uint3(3571447507U, 3412283213U, 2601761069U), math.uint3(1254033427U, 2248573027U, 3612677113U), v.c3);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000358C File Offset: 0x0000178C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool x, bool y, bool z, bool w)
		{
			return new bool4(x, y, z, w);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003597 File Offset: 0x00001797
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool x, bool y, bool2 zw)
		{
			return new bool4(x, y, zw);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000035A1 File Offset: 0x000017A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool x, bool2 yz, bool w)
		{
			return new bool4(x, yz, w);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000035AB File Offset: 0x000017AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool x, bool3 yzw)
		{
			return new bool4(x, yzw);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000035B4 File Offset: 0x000017B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool2 xy, bool z, bool w)
		{
			return new bool4(xy, z, w);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000035BE File Offset: 0x000017BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool2 xy, bool2 zw)
		{
			return new bool4(xy, zw);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000035C7 File Offset: 0x000017C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool3 xyz, bool w)
		{
			return new bool4(xyz, w);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000035D0 File Offset: 0x000017D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool4 xyzw)
		{
			return new bool4(xyzw);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000035D8 File Offset: 0x000017D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 bool4(bool v)
		{
			return new bool4(v);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000035E0 File Offset: 0x000017E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool4 v)
		{
			return math.csum(math.select(math.uint4(1610574617U, 1584185147U, 3041325733U, 3150930919U), math.uint4(3309258581U, 1770373673U, 3778261171U, 3286279097U), v));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000361F File Offset: 0x0000181F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(bool4 v)
		{
			return math.select(math.uint4(4264629071U, 1898591447U, 2641864091U, 1229113913U), math.uint4(3020867117U, 1449055807U, 2479033387U, 3702457169U), v);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003659 File Offset: 0x00001859
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool shuffle(bool4 left, bool4 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003663 File Offset: 0x00001863
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 shuffle(bool4 left, bool4 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.bool2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000367A File Offset: 0x0000187A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 shuffle(bool4 left, bool4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.bool3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000369A File Offset: 0x0000189A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 shuffle(bool4 left, bool4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.bool4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000036C4 File Offset: 0x000018C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool select_shuffle_component(bool4 a, bool4 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.LeftW:
				return a.w;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			case math.ShuffleComponent.RightW:
				return b.w;
			default:
				throw new ArgumentException("Invalid shuffle component: " + component.ToString());
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000374D File Offset: 0x0000194D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 bool4x2(bool4 c0, bool4 c1)
		{
			return new bool4x2(c0, c1);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003756 File Offset: 0x00001956
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 bool4x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21, bool m30, bool m31)
		{
			return new bool4x2(m00, m01, m10, m11, m20, m21, m30, m31);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003769 File Offset: 0x00001969
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 bool4x2(bool v)
		{
			return new bool4x2(v);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003774 File Offset: 0x00001974
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 transpose(bool4x2 v)
		{
			return math.bool2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000037E0 File Offset: 0x000019E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool4x2 v)
		{
			return math.csum(math.select(math.uint4(3516359879U, 3050356579U, 4178586719U, 2558655391U), math.uint4(1453413133U, 2152428077U, 1938706661U, 1338588197U), v.c0) + math.select(math.uint4(3439609253U, 3535343003U, 3546061613U, 2702024231U), math.uint4(1452124841U, 1966089551U, 2668168249U, 1587512777U), v.c1));
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003874 File Offset: 0x00001A74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(bool4x2 v)
		{
			return math.select(math.uint4(2353831999U, 3101256173U, 2891822459U, 2837054189U), math.uint4(3016004371U, 4097481403U, 2229788699U, 2382715877U), v.c0) + math.select(math.uint4(1851936439U, 1938025801U, 3712598587U, 3956330501U), math.uint4(2437373431U, 1441286183U, 2426570171U, 1561977301U), v.c1);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003900 File Offset: 0x00001B00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 bool4x3(bool4 c0, bool4 c1, bool4 c2)
		{
			return new bool4x3(c0, c1, c2);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000390C File Offset: 0x00001B0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 bool4x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12, bool m20, bool m21, bool m22, bool m30, bool m31, bool m32)
		{
			return new bool4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003932 File Offset: 0x00001B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 bool4x3(bool v)
		{
			return new bool4x3(v);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000393C File Offset: 0x00001B3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 transpose(bool4x3 v)
		{
			return math.bool3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000039D4 File Offset: 0x00001BD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool4x3 v)
		{
			return math.csum(math.select(math.uint4(3940484981U, 1954192763U, 1091696537U, 3052428017U), math.uint4(4253034763U, 2338696631U, 3757372771U, 1885959949U), v.c0) + math.select(math.uint4(3508684087U, 3919501043U, 1209161033U, 4007793211U), math.uint4(3819806693U, 3458005183U, 2078515003U, 4206465343U), v.c1) + math.select(math.uint4(3025146473U, 3763046909U, 3678265601U, 2070747979U), math.uint4(1480171127U, 1588341193U, 4234155257U, 1811310911U), v.c2));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003AA8 File Offset: 0x00001CA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(bool4x3 v)
		{
			return math.select(math.uint4(2635799963U, 4165137857U, 2759770933U, 2759319383U), math.uint4(3299952959U, 3121178323U, 2948522579U, 1531026433U), v.c0) + math.select(math.uint4(1365086453U, 3969870067U, 4192899797U, 3271228601U), math.uint4(1634639009U, 3318036811U, 3404170631U, 2048213449U), v.c1) + math.select(math.uint4(4164671783U, 1780759499U, 1352369353U, 2446407751U), math.uint4(1391928079U, 3475533443U, 3777095341U, 3385463369U), v.c2);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003B76 File Offset: 0x00001D76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 bool4x4(bool4 c0, bool4 c1, bool4 c2, bool4 c3)
		{
			return new bool4x4(c0, c1, c2, c3);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003B84 File Offset: 0x00001D84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 bool4x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23, bool m30, bool m31, bool m32, bool m33)
		{
			return new bool4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003BB2 File Offset: 0x00001DB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 bool4x4(bool v)
		{
			return new bool4x4(v);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003BBC File Offset: 0x00001DBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 transpose(bool4x4 v)
		{
			return math.bool4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003C80 File Offset: 0x00001E80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(bool4x4 v)
		{
			return math.csum(math.select(math.uint4(3516359879U, 3050356579U, 4178586719U, 2558655391U), math.uint4(1453413133U, 2152428077U, 1938706661U, 1338588197U), v.c0) + math.select(math.uint4(3439609253U, 3535343003U, 3546061613U, 2702024231U), math.uint4(1452124841U, 1966089551U, 2668168249U, 1587512777U), v.c1) + math.select(math.uint4(2353831999U, 3101256173U, 2891822459U, 2837054189U), math.uint4(3016004371U, 4097481403U, 2229788699U, 2382715877U), v.c2) + math.select(math.uint4(1851936439U, 1938025801U, 3712598587U, 3956330501U), math.uint4(2437373431U, 1441286183U, 2426570171U, 1561977301U), v.c3));
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003D98 File Offset: 0x00001F98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(bool4x4 v)
		{
			return math.select(math.uint4(4205774813U, 1650214333U, 3388112843U, 1831150513U), math.uint4(1848374953U, 3430200247U, 2209710647U, 2201894441U), v.c0) + math.select(math.uint4(2849577407U, 3287031191U, 3098675399U, 1564399943U), math.uint4(1148435377U, 3416333663U, 1750611407U, 3285396193U), v.c1) + math.select(math.uint4(3110507567U, 4271396531U, 4198118021U, 2908068253U), math.uint4(3705492289U, 2497566569U, 2716413241U, 1166264321U), v.c2) + math.select(math.uint4(2503385333U, 2944493077U, 2599999021U, 3814721321U), math.uint4(1595355149U, 1728931849U, 2062756937U, 2920485769U), v.c3);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003EA8 File Offset: 0x000020A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(double x, double y)
		{
			return new double2(x, y);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003EB1 File Offset: 0x000020B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(double2 xy)
		{
			return new double2(xy);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003EB9 File Offset: 0x000020B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(double v)
		{
			return new double2(v);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003EC1 File Offset: 0x000020C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(bool v)
		{
			return new double2(v);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003EC9 File Offset: 0x000020C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(bool2 v)
		{
			return new double2(v);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003ED1 File Offset: 0x000020D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(int v)
		{
			return new double2(v);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003ED9 File Offset: 0x000020D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(int2 v)
		{
			return new double2(v);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003EE1 File Offset: 0x000020E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(uint v)
		{
			return new double2(v);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003EE9 File Offset: 0x000020E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(uint2 v)
		{
			return new double2(v);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003EF1 File Offset: 0x000020F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(half v)
		{
			return new double2(v);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003EF9 File Offset: 0x000020F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(half2 v)
		{
			return new double2(v);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003F01 File Offset: 0x00002101
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(float v)
		{
			return new double2(v);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003F09 File Offset: 0x00002109
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 double2(float2 v)
		{
			return new double2(v);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003F11 File Offset: 0x00002111
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double2 v)
		{
			return math.csum(math.fold_to_uint(v) * math.uint2(2503385333U, 2944493077U)) + 2599999021U;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003F38 File Offset: 0x00002138
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(double2 v)
		{
			return math.fold_to_uint(v) * math.uint2(3814721321U, 1595355149U) + 1728931849U;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003F5E File Offset: 0x0000215E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double shuffle(double2 left, double2 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003F68 File Offset: 0x00002168
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 shuffle(double2 left, double2 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.double2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003F7F File Offset: 0x0000217F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 shuffle(double2 left, double2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.double3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003F9F File Offset: 0x0000219F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 shuffle(double2 left, double2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.double4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003FC8 File Offset: 0x000021C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double select_shuffle_component(double2 a, double2 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000402D File Offset: 0x0000222D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(double2 c0, double2 c1)
		{
			return new double2x2(c0, c1);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004036 File Offset: 0x00002236
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(double m00, double m01, double m10, double m11)
		{
			return new double2x2(m00, m01, m10, m11);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004041 File Offset: 0x00002241
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(double v)
		{
			return new double2x2(v);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004049 File Offset: 0x00002249
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(bool v)
		{
			return new double2x2(v);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004051 File Offset: 0x00002251
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(bool2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004059 File Offset: 0x00002259
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(int v)
		{
			return new double2x2(v);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004061 File Offset: 0x00002261
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(int2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004069 File Offset: 0x00002269
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(uint v)
		{
			return new double2x2(v);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004071 File Offset: 0x00002271
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(uint2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004079 File Offset: 0x00002279
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(float v)
		{
			return new double2x2(v);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004081 File Offset: 0x00002281
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 double2x2(float2x2 v)
		{
			return new double2x2(v);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004089 File Offset: 0x00002289
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 transpose(double2x2 v)
		{
			return math.double2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000040BC File Offset: 0x000022BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 inverse(double2x2 m)
		{
			double x = m.c0.x;
			double x2 = m.c1.x;
			double y = m.c0.y;
			double y2 = m.c1.y;
			double num = x * y2 - x2 * y;
			return math.double2x2(y2, -x2, -y, x) * (1.0 / num);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004120 File Offset: 0x00002320
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double determinant(double2x2 m)
		{
			double x = m.c0.x;
			double x2 = m.c1.x;
			double y = m.c0.y;
			double y2 = m.c1.y;
			return x * y2 - x2 * y;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004164 File Offset: 0x00002364
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double2x2 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint2(4253034763U, 2338696631U) + math.fold_to_uint(v.c1) * math.uint2(3757372771U, 1885959949U)) + 3508684087U;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000041C0 File Offset: 0x000023C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(double2x2 v)
		{
			return math.fold_to_uint(v.c0) * math.uint2(3919501043U, 1209161033U) + math.fold_to_uint(v.c1) * math.uint2(4007793211U, 3819806693U) + 3458005183U;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000421A File Offset: 0x0000241A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(double2 c0, double2 c1, double2 c2)
		{
			return new double2x3(c0, c1, c2);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004224 File Offset: 0x00002424
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(double m00, double m01, double m02, double m10, double m11, double m12)
		{
			return new double2x3(m00, m01, m02, m10, m11, m12);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004233 File Offset: 0x00002433
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(double v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000423B File Offset: 0x0000243B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(bool v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004243 File Offset: 0x00002443
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(bool2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000424B File Offset: 0x0000244B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(int v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004253 File Offset: 0x00002453
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(int2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000425B File Offset: 0x0000245B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(uint v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004263 File Offset: 0x00002463
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(uint2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000426B File Offset: 0x0000246B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(float v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004273 File Offset: 0x00002473
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 double2x3(float2x3 v)
		{
			return new double2x3(v);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000427C File Offset: 0x0000247C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 transpose(double2x3 v)
		{
			return math.double3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000042D0 File Offset: 0x000024D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double2x3 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint2(4066109527U, 2613148903U) + math.fold_to_uint(v.c1) * math.uint2(3367528529U, 1678332449U) + math.fold_to_uint(v.c2) * math.uint2(2918459647U, 2744611081U)) + 1952372791U;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004350 File Offset: 0x00002550
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(double2x3 v)
		{
			return math.fold_to_uint(v.c0) * math.uint2(2631698677U, 4200781601U) + math.fold_to_uint(v.c1) * math.uint2(2119021007U, 1760485621U) + math.fold_to_uint(v.c2) * math.uint2(3157985881U, 2171534173U) + 2723054263U;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000043CE File Offset: 0x000025CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(double2 c0, double2 c1, double2 c2, double2 c3)
		{
			return new double2x4(c0, c1, c2, c3);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000043D9 File Offset: 0x000025D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13)
		{
			return new double2x4(m00, m01, m02, m03, m10, m11, m12, m13);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000043EC File Offset: 0x000025EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(double v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000043F4 File Offset: 0x000025F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(bool v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000043FC File Offset: 0x000025FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(bool2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004404 File Offset: 0x00002604
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(int v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000440C File Offset: 0x0000260C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(int2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004414 File Offset: 0x00002614
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(uint v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000441C File Offset: 0x0000261C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(uint2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004424 File Offset: 0x00002624
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(float v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000442C File Offset: 0x0000262C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 double2x4(float2x4 v)
		{
			return new double2x4(v);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004434 File Offset: 0x00002634
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 transpose(double2x4 v)
		{
			return math.double4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000044A0 File Offset: 0x000026A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double2x4 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint2(2437373431U, 1441286183U) + math.fold_to_uint(v.c1) * math.uint2(2426570171U, 1561977301U) + math.fold_to_uint(v.c2) * math.uint2(4205774813U, 1650214333U) + math.fold_to_uint(v.c3) * math.uint2(3388112843U, 1831150513U)) + 1848374953U;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004544 File Offset: 0x00002744
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(double2x4 v)
		{
			return math.fold_to_uint(v.c0) * math.uint2(3430200247U, 2209710647U) + math.fold_to_uint(v.c1) * math.uint2(2201894441U, 2849577407U) + math.fold_to_uint(v.c2) * math.uint2(3287031191U, 3098675399U) + math.fold_to_uint(v.c3) * math.uint2(1564399943U, 1148435377U) + 3416333663U;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000045E6 File Offset: 0x000027E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(double x, double y, double z)
		{
			return new double3(x, y, z);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000045F0 File Offset: 0x000027F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(double x, double2 yz)
		{
			return new double3(x, yz);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000045F9 File Offset: 0x000027F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(double2 xy, double z)
		{
			return new double3(xy, z);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004602 File Offset: 0x00002802
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(double3 xyz)
		{
			return new double3(xyz);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000460A File Offset: 0x0000280A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(double v)
		{
			return new double3(v);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004612 File Offset: 0x00002812
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(bool v)
		{
			return new double3(v);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000461A File Offset: 0x0000281A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(bool3 v)
		{
			return new double3(v);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004622 File Offset: 0x00002822
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(int v)
		{
			return new double3(v);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000462A File Offset: 0x0000282A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(int3 v)
		{
			return new double3(v);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004632 File Offset: 0x00002832
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(uint v)
		{
			return new double3(v);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000463A File Offset: 0x0000283A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(uint3 v)
		{
			return new double3(v);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004642 File Offset: 0x00002842
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(half v)
		{
			return new double3(v);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000464A File Offset: 0x0000284A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(half3 v)
		{
			return new double3(v);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004652 File Offset: 0x00002852
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(float v)
		{
			return new double3(v);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000465A File Offset: 0x0000285A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 double3(float3 v)
		{
			return new double3(v);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004662 File Offset: 0x00002862
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double3 v)
		{
			return math.csum(math.fold_to_uint(v) * math.uint3(2937008387U, 3835713223U, 2216526373U)) + 3375971453U;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000468E File Offset: 0x0000288E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(double3 v)
		{
			return math.fold_to_uint(v) * math.uint3(3559829411U, 3652178029U, 2544260129U) + 2013864031U;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000046B9 File Offset: 0x000028B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double shuffle(double3 left, double3 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000046C3 File Offset: 0x000028C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 shuffle(double3 left, double3 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.double2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000046DA File Offset: 0x000028DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 shuffle(double3 left, double3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.double3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000046FA File Offset: 0x000028FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 shuffle(double3 left, double3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.double4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004724 File Offset: 0x00002924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double select_shuffle_component(double3 a, double3 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000479B File Offset: 0x0000299B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(double3 c0, double3 c1)
		{
			return new double3x2(c0, c1);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000047A4 File Offset: 0x000029A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(double m00, double m01, double m10, double m11, double m20, double m21)
		{
			return new double3x2(m00, m01, m10, m11, m20, m21);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000047B3 File Offset: 0x000029B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(double v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000047BB File Offset: 0x000029BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(bool v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000047C3 File Offset: 0x000029C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(bool3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000047CB File Offset: 0x000029CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(int v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000047D3 File Offset: 0x000029D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(int3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000047DB File Offset: 0x000029DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(uint v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000047E3 File Offset: 0x000029E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(uint3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000047EB File Offset: 0x000029EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(float v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000047F3 File Offset: 0x000029F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 double3x2(float3x2 v)
		{
			return new double3x2(v);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000047FC File Offset: 0x000029FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 transpose(double3x2 v)
		{
			return math.double2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004850 File Offset: 0x00002A50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double3x2 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint3(3996716183U, 2626301701U, 1306289417U) + math.fold_to_uint(v.c1) * math.uint3(2096137163U, 1548578029U, 4178800919U)) + 3898072289U;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000048B8 File Offset: 0x00002AB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(double3x2 v)
		{
			return math.fold_to_uint(v.c0) * math.uint3(4129428421U, 2631575897U, 2854656703U) + math.fold_to_uint(v.c1) * math.uint3(3578504047U, 4245178297U, 2173281923U) + 2973357649U;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000491C File Offset: 0x00002B1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(double3 c0, double3 c1, double3 c2)
		{
			return new double3x3(c0, c1, c2);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004928 File Offset: 0x00002B28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
		{
			return new double3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004948 File Offset: 0x00002B48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(double v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004950 File Offset: 0x00002B50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(bool v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004958 File Offset: 0x00002B58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(bool3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004960 File Offset: 0x00002B60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(int v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004968 File Offset: 0x00002B68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(int3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004970 File Offset: 0x00002B70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(uint v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004978 File Offset: 0x00002B78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(uint3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004980 File Offset: 0x00002B80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(float v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004988 File Offset: 0x00002B88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 double3x3(float3x3 v)
		{
			return new double3x3(v);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004990 File Offset: 0x00002B90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 transpose(double3x3 v)
		{
			return math.double3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004A08 File Offset: 0x00002C08
		public static double3x3 inverse(double3x3 m)
		{
			double3 c = m.c0;
			double3 c2 = m.c1;
			double3 c3 = m.c2;
			double3 @double = math.double3(c2.x, c3.x, c.x);
			double3 double2 = math.double3(c2.y, c3.y, c.y);
			double3 double3 = math.double3(c2.z, c3.z, c.z);
			double3 double4 = double2 * double3.yzx - double2.yzx * double3;
			double3 double5 = @double.yzx * double3 - @double * double3.yzx;
			double3 double6 = @double * double2.yzx - @double.yzx * double2;
			double num = 1.0 / math.csum(@double.zxy * double4);
			return math.double3x3(double4, double5, double6) * num;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004B08 File Offset: 0x00002D08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double determinant(double3x3 m)
		{
			double3 c = m.c0;
			double3 c2 = m.c1;
			double3 c3 = m.c2;
			double num = c2.y * c3.z - c2.z * c3.y;
			double num2 = c.y * c3.z - c.z * c3.y;
			double num3 = c.y * c2.z - c.z * c2.y;
			return c.x * num - c2.x * num2 + c3.x * num3;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004B9C File Offset: 0x00002D9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double3x3 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint3(2891822459U, 2837054189U, 3016004371U) + math.fold_to_uint(v.c1) * math.uint3(4097481403U, 2229788699U, 2382715877U) + math.fold_to_uint(v.c2) * math.uint3(1851936439U, 1938025801U, 3712598587U)) + 3956330501U;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004C2C File Offset: 0x00002E2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(double3x3 v)
		{
			return math.fold_to_uint(v.c0) * math.uint3(2437373431U, 1441286183U, 2426570171U) + math.fold_to_uint(v.c1) * math.uint3(1561977301U, 4205774813U, 1650214333U) + math.fold_to_uint(v.c2) * math.uint3(3388112843U, 1831150513U, 1848374953U) + 3430200247U;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004CB9 File Offset: 0x00002EB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(double3 c0, double3 c1, double3 c2, double3 c3)
		{
			return new double3x4(c0, c1, c2, c3);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004CC4 File Offset: 0x00002EC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23)
		{
			return new double3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004CEA File Offset: 0x00002EEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(double v)
		{
			return new double3x4(v);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004CF2 File Offset: 0x00002EF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(bool v)
		{
			return new double3x4(v);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004CFA File Offset: 0x00002EFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(bool3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004D02 File Offset: 0x00002F02
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(int v)
		{
			return new double3x4(v);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004D0A File Offset: 0x00002F0A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(int3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004D12 File Offset: 0x00002F12
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(uint v)
		{
			return new double3x4(v);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004D1A File Offset: 0x00002F1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(uint3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004D22 File Offset: 0x00002F22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(float v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004D2A File Offset: 0x00002F2A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 double3x4(float3x4 v)
		{
			return new double3x4(v);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004D34 File Offset: 0x00002F34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 transpose(double3x4 v)
		{
			return math.double4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004DCC File Offset: 0x00002FCC
		public static double3x4 fastinverse(double3x4 m)
		{
			double3 c = m.c0;
			double3 c2 = m.c1;
			double3 c3 = m.c2;
			double3 @double = m.c3;
			double3 double2 = math.double3(c.x, c2.x, c3.x);
			double3 double3 = math.double3(c.y, c2.y, c3.y);
			double3 double4 = math.double3(c.z, c2.z, c3.z);
			@double = -(double2 * @double.x + double3 * @double.y + double4 * @double.z);
			return math.double3x4(double2, double3, double4, @double);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004E80 File Offset: 0x00003080
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double3x4 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint3(3996716183U, 2626301701U, 1306289417U) + math.fold_to_uint(v.c1) * math.uint3(2096137163U, 1548578029U, 4178800919U) + math.fold_to_uint(v.c2) * math.uint3(3898072289U, 4129428421U, 2631575897U) + math.fold_to_uint(v.c3) * math.uint3(2854656703U, 3578504047U, 4245178297U)) + 2173281923U;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004F38 File Offset: 0x00003138
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(double3x4 v)
		{
			return math.fold_to_uint(v.c0) * math.uint3(2973357649U, 3881277847U, 4017968839U) + math.fold_to_uint(v.c1) * math.uint3(1727237899U, 1648514723U, 1385344481U) + math.fold_to_uint(v.c2) * math.uint3(3538260197U, 4066109527U, 2613148903U) + math.fold_to_uint(v.c3) * math.uint3(3367528529U, 1678332449U, 2918459647U) + 2744611081U;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004FEE File Offset: 0x000031EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double x, double y, double z, double w)
		{
			return new double4(x, y, z, w);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004FF9 File Offset: 0x000031F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double x, double y, double2 zw)
		{
			return new double4(x, y, zw);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005003 File Offset: 0x00003203
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double x, double2 yz, double w)
		{
			return new double4(x, yz, w);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000500D File Offset: 0x0000320D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double x, double3 yzw)
		{
			return new double4(x, yzw);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005016 File Offset: 0x00003216
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double2 xy, double z, double w)
		{
			return new double4(xy, z, w);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005020 File Offset: 0x00003220
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double2 xy, double2 zw)
		{
			return new double4(xy, zw);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005029 File Offset: 0x00003229
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double3 xyz, double w)
		{
			return new double4(xyz, w);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005032 File Offset: 0x00003232
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double4 xyzw)
		{
			return new double4(xyzw);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000503A File Offset: 0x0000323A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(double v)
		{
			return new double4(v);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005042 File Offset: 0x00003242
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(bool v)
		{
			return new double4(v);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000504A File Offset: 0x0000324A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(bool4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005052 File Offset: 0x00003252
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(int v)
		{
			return new double4(v);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000505A File Offset: 0x0000325A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(int4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005062 File Offset: 0x00003262
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(uint v)
		{
			return new double4(v);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000506A File Offset: 0x0000326A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(uint4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005072 File Offset: 0x00003272
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(half v)
		{
			return new double4(v);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000507A File Offset: 0x0000327A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(half4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005082 File Offset: 0x00003282
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(float v)
		{
			return new double4(v);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000508A File Offset: 0x0000328A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 double4(float4 v)
		{
			return new double4(v);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005092 File Offset: 0x00003292
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double4 v)
		{
			return math.csum(math.fold_to_uint(v) * math.uint4(2669441947U, 1260114311U, 2650080659U, 4052675461U)) + 2652487619U;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000050C3 File Offset: 0x000032C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(double4 v)
		{
			return math.fold_to_uint(v) * math.uint4(2174136431U, 3528391193U, 2105559227U, 1899745391U) + 1966790317U;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000050F3 File Offset: 0x000032F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double shuffle(double4 left, double4 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000050FD File Offset: 0x000032FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 shuffle(double4 left, double4 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.double2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005114 File Offset: 0x00003314
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 shuffle(double4 left, double4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.double3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005134 File Offset: 0x00003334
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 shuffle(double4 left, double4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.double4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005160 File Offset: 0x00003360
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double select_shuffle_component(double4 a, double4 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.LeftW:
				return a.w;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			case math.ShuffleComponent.RightW:
				return b.w;
			default:
				throw new ArgumentException("Invalid shuffle component: " + component.ToString());
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000051E9 File Offset: 0x000033E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(double4 c0, double4 c1)
		{
			return new double4x2(c0, c1);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000051F2 File Offset: 0x000033F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(double m00, double m01, double m10, double m11, double m20, double m21, double m30, double m31)
		{
			return new double4x2(m00, m01, m10, m11, m20, m21, m30, m31);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005205 File Offset: 0x00003405
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(double v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000520D File Offset: 0x0000340D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(bool v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005215 File Offset: 0x00003415
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(bool4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000521D File Offset: 0x0000341D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(int v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005225 File Offset: 0x00003425
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(int4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000522D File Offset: 0x0000342D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(uint v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005235 File Offset: 0x00003435
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(uint4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000523D File Offset: 0x0000343D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(float v)
		{
			return new double4x2(v);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005245 File Offset: 0x00003445
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 double4x2(float4x2 v)
		{
			return new double4x2(v);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005250 File Offset: 0x00003450
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 transpose(double4x2 v)
		{
			return math.double2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000052BC File Offset: 0x000034BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double4x2 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint4(1521739981U, 1735296007U, 3010324327U, 1875523709U) + math.fold_to_uint(v.c1) * math.uint4(2937008387U, 3835713223U, 2216526373U, 3375971453U)) + 3559829411U;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000532C File Offset: 0x0000352C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(double4x2 v)
		{
			return math.fold_to_uint(v.c0) * math.uint4(3652178029U, 2544260129U, 2013864031U, 2627668003U) + math.fold_to_uint(v.c1) * math.uint4(1520214331U, 2949502447U, 2827819133U, 3480140317U) + 2642994593U;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000539A File Offset: 0x0000359A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(double4 c0, double4 c1, double4 c2)
		{
			return new double4x3(c0, c1, c2);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000053A4 File Offset: 0x000035A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22, double m30, double m31, double m32)
		{
			return new double4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000053CA File Offset: 0x000035CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(double v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000053D2 File Offset: 0x000035D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(bool v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000053DA File Offset: 0x000035DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(bool4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000053E2 File Offset: 0x000035E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(int v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000053EA File Offset: 0x000035EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(int4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000053F2 File Offset: 0x000035F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(uint v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000053FA File Offset: 0x000035FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(uint4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005402 File Offset: 0x00003602
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(float v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000540A File Offset: 0x0000360A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 double4x3(float4x3 v)
		{
			return new double4x3(v);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005414 File Offset: 0x00003614
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 transpose(double4x3 v)
		{
			return math.double3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000054AC File Offset: 0x000036AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double4x3 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint4(2057338067U, 2942577577U, 2834440507U, 2671762487U) + math.fold_to_uint(v.c1) * math.uint4(2892026051U, 2455987759U, 3868600063U, 3170963179U) + math.fold_to_uint(v.c2) * math.uint4(2632835537U, 1136528209U, 2944626401U, 2972762423U)) + 1417889653U;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000554C File Offset: 0x0000374C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(double4x3 v)
		{
			return math.fold_to_uint(v.c0) * math.uint4(2080514593U, 2731544287U, 2828498809U, 2669441947U) + math.fold_to_uint(v.c1) * math.uint4(1260114311U, 2650080659U, 4052675461U, 2652487619U) + math.fold_to_uint(v.c2) * math.uint4(2174136431U, 3528391193U, 2105559227U, 1899745391U) + 1966790317U;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000055E8 File Offset: 0x000037E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(double4 c0, double4 c1, double4 c2, double4 c3)
		{
			return new double4x4(c0, c1, c2, c3);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000055F4 File Offset: 0x000037F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
		{
			return new double4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005622 File Offset: 0x00003822
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(double v)
		{
			return new double4x4(v);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000562A File Offset: 0x0000382A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(bool v)
		{
			return new double4x4(v);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005632 File Offset: 0x00003832
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(bool4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000563A File Offset: 0x0000383A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(int v)
		{
			return new double4x4(v);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005642 File Offset: 0x00003842
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(int4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000564A File Offset: 0x0000384A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(uint v)
		{
			return new double4x4(v);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005652 File Offset: 0x00003852
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(uint4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000565A File Offset: 0x0000385A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(float v)
		{
			return new double4x4(v);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005662 File Offset: 0x00003862
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 double4x4(float4x4 v)
		{
			return new double4x4(v);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000566C File Offset: 0x0000386C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 rotate(double4x4 a, double3 b)
		{
			return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z).xyz;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000056C0 File Offset: 0x000038C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 transform(double4x4 a, double3 b)
		{
			return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3).xyz;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005720 File Offset: 0x00003920
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 transpose(double4x4 v)
		{
			return math.double4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000057E4 File Offset: 0x000039E4
		public static double4x4 inverse(double4x4 m)
		{
			double4 c = m.c0;
			double4 c2 = m.c1;
			double4 c3 = m.c2;
			double4 c4 = m.c3;
			double4 @double = math.movelh(c2, c);
			double4 double2 = math.movelh(c3, c4);
			double4 double3 = math.movehl(c, c2);
			double4 double4 = math.movehl(c4, c3);
			double4 double5 = math.shuffle(c2, c, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightY, math.ShuffleComponent.RightZ);
			double4 double6 = math.shuffle(c3, c4, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightY, math.ShuffleComponent.RightZ);
			double4 double7 = math.shuffle(c2, c, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightW, math.ShuffleComponent.RightX);
			double4 double8 = math.shuffle(c3, c4, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightW, math.ShuffleComponent.RightX);
			double4 double9 = math.shuffle(double2, @double, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.RightZ);
			double4 double10 = math.shuffle(double2, @double, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightY, math.ShuffleComponent.RightW);
			double4 double11 = math.shuffle(double4, double3, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.RightZ);
			double4 double12 = math.shuffle(double4, double3, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightY, math.ShuffleComponent.RightW);
			double4 double13 = math.shuffle(@double, double2, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.RightZ);
			double4 double14 = double5 * double4 - double6 * double3;
			double4 double15 = @double * double4 - double2 * double3;
			double4 double16 = double8 * @double - double7 * double2;
			double4 double17 = math.shuffle(double14, double14, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightX);
			double4 double18 = math.shuffle(double14, double14, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW, math.ShuffleComponent.RightY);
			double4 double19 = math.shuffle(double15, double15, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightX);
			double4 double20 = math.shuffle(double15, double15, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW, math.ShuffleComponent.RightY);
			double4 double21 = double12 * double17 - double11 * double20 + double10 * double18;
			double4 double22 = double13 * double21;
			double22 += math.shuffle(double22, double22, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightW, math.ShuffleComponent.RightZ);
			double22 -= math.shuffle(double22, double22, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightX, math.ShuffleComponent.RightX);
			double4 double23 = math.double4(1.0) / double22;
			double4x4 double4x;
			double4x.c0 = double21 * double23;
			double4 double24 = math.shuffle(double16, double16, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightX);
			double4 double25 = math.shuffle(double16, double16, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW, math.ShuffleComponent.RightY);
			double4 double26 = double11 * double24 - double9 * double18 - double12 * double19;
			double4x.c1 = double26 * double23;
			double4 double27 = double9 * double20 - double10 * double24 - double12 * double25;
			double4x.c2 = double27 * double23;
			double4 double28 = double10 * double19 - double9 * double17 + double11 * double25;
			double4x.c3 = double28 * double23;
			return double4x;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005A78 File Offset: 0x00003C78
		public static double4x4 fastinverse(double4x4 m)
		{
			double4 c = m.c0;
			double4 c2 = m.c1;
			double4 c3 = m.c2;
			double4 @double = m.c3;
			double4 double2 = math.double4(0);
			double4 double3 = math.unpacklo(c, c3);
			double4 double4 = math.unpacklo(c2, double2);
			double4 double5 = math.unpackhi(c, c3);
			double4 double6 = math.unpackhi(c2, double2);
			double4 double7 = math.unpacklo(double3, double4);
			double4 double8 = math.unpackhi(double3, double4);
			double4 double9 = math.unpacklo(double5, double6);
			@double = -(double7 * @double.x + double8 * @double.y + double9 * @double.z);
			@double.w = 1.0;
			return math.double4x4(double7, double8, double9, @double);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005B3C File Offset: 0x00003D3C
		public static double determinant(double4x4 m)
		{
			double4 c = m.c0;
			double4 c2 = m.c1;
			double4 c3 = m.c2;
			double4 c4 = m.c3;
			double num = c2.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c2.z * c4.w - c2.w * c4.z) + c4.y * (c2.z * c3.w - c2.w * c3.z);
			double num2 = c.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c3.w - c.w * c3.z);
			double num3 = c.y * (c2.z * c4.w - c2.w * c4.z) - c2.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c2.w - c.w * c2.z);
			double num4 = c.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c.z * c3.w - c.w * c3.z) + c3.y * (c.z * c2.w - c.w * c2.z);
			return c.x * num - c2.x * num2 + c3.x * num3 - c4.x * num4;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005D34 File Offset: 0x00003F34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(double4x4 v)
		{
			return math.csum(math.fold_to_uint(v.c0) * math.uint4(1306289417U, 2096137163U, 1548578029U, 4178800919U) + math.fold_to_uint(v.c1) * math.uint4(3898072289U, 4129428421U, 2631575897U, 2854656703U) + math.fold_to_uint(v.c2) * math.uint4(3578504047U, 4245178297U, 2173281923U, 2973357649U) + math.fold_to_uint(v.c3) * math.uint4(3881277847U, 4017968839U, 1727237899U, 1648514723U)) + 1385344481U;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005E00 File Offset: 0x00004000
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(double4x4 v)
		{
			return math.fold_to_uint(v.c0) * math.uint4(3538260197U, 4066109527U, 2613148903U, 3367528529U) + math.fold_to_uint(v.c1) * math.uint4(1678332449U, 2918459647U, 2744611081U, 1952372791U) + math.fold_to_uint(v.c2) * math.uint4(2631698677U, 4200781601U, 2119021007U, 1760485621U) + math.fold_to_uint(v.c3) * math.uint4(3157985881U, 2171534173U, 2723054263U, 1168253063U) + 4228926523U;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005ECA File Offset: 0x000040CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(float x, float y)
		{
			return new float2(x, y);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005ED3 File Offset: 0x000040D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(float2 xy)
		{
			return new float2(xy);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005EDB File Offset: 0x000040DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(float v)
		{
			return new float2(v);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005EE3 File Offset: 0x000040E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(bool v)
		{
			return new float2(v);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005EEB File Offset: 0x000040EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(bool2 v)
		{
			return new float2(v);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005EF3 File Offset: 0x000040F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(int v)
		{
			return new float2(v);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005EFB File Offset: 0x000040FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(int2 v)
		{
			return new float2(v);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005F03 File Offset: 0x00004103
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(uint v)
		{
			return new float2(v);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005F0B File Offset: 0x0000410B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(uint2 v)
		{
			return new float2(v);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005F13 File Offset: 0x00004113
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(half v)
		{
			return new float2(v);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005F1B File Offset: 0x0000411B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(half2 v)
		{
			return new float2(v);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005F23 File Offset: 0x00004123
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(double v)
		{
			return new float2(v);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005F2B File Offset: 0x0000412B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 float2(double2 v)
		{
			return new float2(v);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005F33 File Offset: 0x00004133
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float2 v)
		{
			return math.csum(math.asuint(v) * math.uint2(4198118021U, 2908068253U)) + 3705492289U;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005F5A File Offset: 0x0000415A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(float2 v)
		{
			return math.asuint(v) * math.uint2(2497566569U, 2716413241U) + 1166264321U;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005F80 File Offset: 0x00004180
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float shuffle(float2 left, float2 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005F8A File Offset: 0x0000418A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 shuffle(float2 left, float2 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.float2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005FA1 File Offset: 0x000041A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 shuffle(float2 left, float2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.float3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005FC1 File Offset: 0x000041C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 shuffle(float2 left, float2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.float4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005FEC File Offset: 0x000041EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float select_shuffle_component(float2 a, float2 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006051 File Offset: 0x00004251
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(float2 c0, float2 c1)
		{
			return new float2x2(c0, c1);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000605A File Offset: 0x0000425A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(float m00, float m01, float m10, float m11)
		{
			return new float2x2(m00, m01, m10, m11);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006065 File Offset: 0x00004265
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(float v)
		{
			return new float2x2(v);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000606D File Offset: 0x0000426D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(bool v)
		{
			return new float2x2(v);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006075 File Offset: 0x00004275
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(bool2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000607D File Offset: 0x0000427D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(int v)
		{
			return new float2x2(v);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006085 File Offset: 0x00004285
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(int2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000608D File Offset: 0x0000428D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(uint v)
		{
			return new float2x2(v);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006095 File Offset: 0x00004295
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(uint2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000609D File Offset: 0x0000429D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(double v)
		{
			return new float2x2(v);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000060A5 File Offset: 0x000042A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 float2x2(double2x2 v)
		{
			return new float2x2(v);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000060AD File Offset: 0x000042AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 transpose(float2x2 v)
		{
			return math.float2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000060E0 File Offset: 0x000042E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 inverse(float2x2 m)
		{
			float x = m.c0.x;
			float x2 = m.c1.x;
			float y = m.c0.y;
			float y2 = m.c1.y;
			float num = x * y2 - x2 * y;
			return math.float2x2(y2, -x2, -y, x) * (1f / num);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006140 File Offset: 0x00004340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float determinant(float2x2 m)
		{
			float x = m.c0.x;
			float x2 = m.c1.x;
			float y = m.c0.y;
			float y2 = m.c1.y;
			return x * y2 - x2 * y;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006184 File Offset: 0x00004384
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float2x2 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint2(2627668003U, 1520214331U) + math.asuint(v.c1) * math.uint2(2949502447U, 2827819133U)) + 3480140317U;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000061E0 File Offset: 0x000043E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(float2x2 v)
		{
			return math.asuint(v.c0) * math.uint2(2642994593U, 3940484981U) + math.asuint(v.c1) * math.uint2(1954192763U, 1091696537U) + 3052428017U;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000623A File Offset: 0x0000443A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(float2 c0, float2 c1, float2 c2)
		{
			return new float2x3(c0, c1, c2);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006244 File Offset: 0x00004444
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(float m00, float m01, float m02, float m10, float m11, float m12)
		{
			return new float2x3(m00, m01, m02, m10, m11, m12);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006253 File Offset: 0x00004453
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(float v)
		{
			return new float2x3(v);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000625B File Offset: 0x0000445B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(bool v)
		{
			return new float2x3(v);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006263 File Offset: 0x00004463
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(bool2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000626B File Offset: 0x0000446B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(int v)
		{
			return new float2x3(v);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006273 File Offset: 0x00004473
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(int2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000627B File Offset: 0x0000447B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(uint v)
		{
			return new float2x3(v);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006283 File Offset: 0x00004483
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(uint2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000628B File Offset: 0x0000448B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(double v)
		{
			return new float2x3(v);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006293 File Offset: 0x00004493
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 float2x3(double2x3 v)
		{
			return new float2x3(v);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000629C File Offset: 0x0000449C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 transpose(float2x3 v)
		{
			return math.float3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000062F0 File Offset: 0x000044F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float2x3 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint2(3898072289U, 4129428421U) + math.asuint(v.c1) * math.uint2(2631575897U, 2854656703U) + math.asuint(v.c2) * math.uint2(3578504047U, 4245178297U)) + 2173281923U;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006370 File Offset: 0x00004570
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(float2x3 v)
		{
			return math.asuint(v.c0) * math.uint2(2973357649U, 3881277847U) + math.asuint(v.c1) * math.uint2(4017968839U, 1727237899U) + math.asuint(v.c2) * math.uint2(1648514723U, 1385344481U) + 3538260197U;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000063EE File Offset: 0x000045EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(float2 c0, float2 c1, float2 c2, float2 c3)
		{
			return new float2x4(c0, c1, c2, c3);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000063F9 File Offset: 0x000045F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13)
		{
			return new float2x4(m00, m01, m02, m03, m10, m11, m12, m13);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000640C File Offset: 0x0000460C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(float v)
		{
			return new float2x4(v);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006414 File Offset: 0x00004614
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(bool v)
		{
			return new float2x4(v);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000641C File Offset: 0x0000461C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(bool2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006424 File Offset: 0x00004624
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(int v)
		{
			return new float2x4(v);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000642C File Offset: 0x0000462C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(int2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006434 File Offset: 0x00004634
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(uint v)
		{
			return new float2x4(v);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000643C File Offset: 0x0000463C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(uint2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006444 File Offset: 0x00004644
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(double v)
		{
			return new float2x4(v);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000644C File Offset: 0x0000464C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 float2x4(double2x4 v)
		{
			return new float2x4(v);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006454 File Offset: 0x00004654
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 transpose(float2x4 v)
		{
			return math.float4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000064C0 File Offset: 0x000046C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float2x4 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint2(3546061613U, 2702024231U) + math.asuint(v.c1) * math.uint2(1452124841U, 1966089551U) + math.asuint(v.c2) * math.uint2(2668168249U, 1587512777U) + math.asuint(v.c3) * math.uint2(2353831999U, 3101256173U)) + 2891822459U;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006564 File Offset: 0x00004764
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(float2x4 v)
		{
			return math.asuint(v.c0) * math.uint2(2837054189U, 3016004371U) + math.asuint(v.c1) * math.uint2(4097481403U, 2229788699U) + math.asuint(v.c2) * math.uint2(2382715877U, 1851936439U) + math.asuint(v.c3) * math.uint2(1938025801U, 3712598587U) + 3956330501U;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006606 File Offset: 0x00004806
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(float x, float y, float z)
		{
			return new float3(x, y, z);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006610 File Offset: 0x00004810
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(float x, float2 yz)
		{
			return new float3(x, yz);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006619 File Offset: 0x00004819
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(float2 xy, float z)
		{
			return new float3(xy, z);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006622 File Offset: 0x00004822
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(float3 xyz)
		{
			return new float3(xyz);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000662A File Offset: 0x0000482A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(float v)
		{
			return new float3(v);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006632 File Offset: 0x00004832
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(bool v)
		{
			return new float3(v);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000663A File Offset: 0x0000483A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(bool3 v)
		{
			return new float3(v);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006642 File Offset: 0x00004842
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(int v)
		{
			return new float3(v);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000664A File Offset: 0x0000484A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(int3 v)
		{
			return new float3(v);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006652 File Offset: 0x00004852
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(uint v)
		{
			return new float3(v);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000665A File Offset: 0x0000485A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(uint3 v)
		{
			return new float3(v);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006662 File Offset: 0x00004862
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(half v)
		{
			return new float3(v);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000666A File Offset: 0x0000486A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(half3 v)
		{
			return new float3(v);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006672 File Offset: 0x00004872
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(double v)
		{
			return new float3(v);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000667A File Offset: 0x0000487A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 float3(double3 v)
		{
			return new float3(v);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006682 File Offset: 0x00004882
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float3 v)
		{
			return math.csum(math.asuint(v) * math.uint3(2601761069U, 1254033427U, 2248573027U)) + 3612677113U;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000066AE File Offset: 0x000048AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(float3 v)
		{
			return math.asuint(v) * math.uint3(1521739981U, 1735296007U, 3010324327U) + 1875523709U;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000066D9 File Offset: 0x000048D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float shuffle(float3 left, float3 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000066E3 File Offset: 0x000048E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 shuffle(float3 left, float3 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.float2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000066FA File Offset: 0x000048FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 shuffle(float3 left, float3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.float3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000671A File Offset: 0x0000491A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 shuffle(float3 left, float3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.float4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006744 File Offset: 0x00004944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float select_shuffle_component(float3 a, float3 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000067BB File Offset: 0x000049BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(float3 c0, float3 c1)
		{
			return new float3x2(c0, c1);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000067C4 File Offset: 0x000049C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(float m00, float m01, float m10, float m11, float m20, float m21)
		{
			return new float3x2(m00, m01, m10, m11, m20, m21);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000067D3 File Offset: 0x000049D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(float v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000067DB File Offset: 0x000049DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(bool v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000067E3 File Offset: 0x000049E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(bool3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000067EB File Offset: 0x000049EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(int v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000067F3 File Offset: 0x000049F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(int3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000067FB File Offset: 0x000049FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(uint v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006803 File Offset: 0x00004A03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(uint3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000680B File Offset: 0x00004A0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(double v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006813 File Offset: 0x00004A13
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 float3x2(double3x2 v)
		{
			return new float3x2(v);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000681C File Offset: 0x00004A1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 transpose(float3x2 v)
		{
			return math.float2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006870 File Offset: 0x00004A70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float3x2 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint3(3777095341U, 3385463369U, 1773538433U) + math.asuint(v.c1) * math.uint3(3773525029U, 4131962539U, 1809525511U)) + 4016293529U;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000068D8 File Offset: 0x00004AD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(float3x2 v)
		{
			return math.asuint(v.c0) * math.uint3(2416021567U, 2828384717U, 2636362241U) + math.asuint(v.c1) * math.uint3(1258410977U, 1952565773U, 2037535609U) + 3592785499U;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000693C File Offset: 0x00004B3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(float3 c0, float3 c1, float3 c2)
		{
			return new float3x3(c0, c1, c2);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006948 File Offset: 0x00004B48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
		{
			return new float3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006968 File Offset: 0x00004B68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(float v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006970 File Offset: 0x00004B70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(bool v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00006978 File Offset: 0x00004B78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(bool3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006980 File Offset: 0x00004B80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(int v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006988 File Offset: 0x00004B88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(int3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006990 File Offset: 0x00004B90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(uint v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006998 File Offset: 0x00004B98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(uint3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000069A0 File Offset: 0x00004BA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(double v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000069A8 File Offset: 0x00004BA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(double3x3 v)
		{
			return new float3x3(v);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000069B0 File Offset: 0x00004BB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 transpose(float3x3 v)
		{
			return math.float3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00006A28 File Offset: 0x00004C28
		public static float3x3 inverse(float3x3 m)
		{
			float3 c = m.c0;
			float3 c2 = m.c1;
			float3 c3 = m.c2;
			float3 @float = math.float3(c2.x, c3.x, c.x);
			float3 float2 = math.float3(c2.y, c3.y, c.y);
			float3 float3 = math.float3(c2.z, c3.z, c.z);
			float3 float4 = float2 * float3.yzx - float2.yzx * float3;
			float3 float5 = @float.yzx * float3 - @float * float3.yzx;
			float3 float6 = @float * float2.yzx - @float.yzx * float2;
			float num = 1f / math.csum(@float.zxy * float4);
			return math.float3x3(float4, float5, float6) * num;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00006B24 File Offset: 0x00004D24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float determinant(float3x3 m)
		{
			float3 c = m.c0;
			float3 c2 = m.c1;
			float3 c3 = m.c2;
			float num = c2.y * c3.z - c2.z * c3.y;
			float num2 = c.y * c3.z - c.z * c3.y;
			float num3 = c.y * c2.z - c.z * c2.y;
			return c.x * num - c2.x * num2 + c3.x * num3;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006BB8 File Offset: 0x00004DB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float3x3 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint3(1899745391U, 1966790317U, 3516359879U) + math.asuint(v.c1) * math.uint3(3050356579U, 4178586719U, 2558655391U) + math.asuint(v.c2) * math.uint3(1453413133U, 2152428077U, 1938706661U)) + 1338588197U;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00006C48 File Offset: 0x00004E48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(float3x3 v)
		{
			return math.asuint(v.c0) * math.uint3(3439609253U, 3535343003U, 3546061613U) + math.asuint(v.c1) * math.uint3(2702024231U, 1452124841U, 1966089551U) + math.asuint(v.c2) * math.uint3(2668168249U, 1587512777U, 2353831999U) + 3101256173U;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00006CD5 File Offset: 0x00004ED5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(float3 c0, float3 c1, float3 c2, float3 c3)
		{
			return new float3x4(c0, c1, c2, c3);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006CE0 File Offset: 0x00004EE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23)
		{
			return new float3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00006D06 File Offset: 0x00004F06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(float v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006D0E File Offset: 0x00004F0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(bool v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006D16 File Offset: 0x00004F16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(bool3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00006D1E File Offset: 0x00004F1E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(int v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00006D26 File Offset: 0x00004F26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(int3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00006D2E File Offset: 0x00004F2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(uint v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00006D36 File Offset: 0x00004F36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(uint3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006D3E File Offset: 0x00004F3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(double v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00006D46 File Offset: 0x00004F46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 float3x4(double3x4 v)
		{
			return new float3x4(v);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00006D50 File Offset: 0x00004F50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 transpose(float3x4 v)
		{
			return math.float4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00006DE8 File Offset: 0x00004FE8
		public static float3x4 fastinverse(float3x4 m)
		{
			float3 c = m.c0;
			float3 c2 = m.c1;
			float3 c3 = m.c2;
			float3 @float = m.c3;
			float3 float2 = math.float3(c.x, c2.x, c3.x);
			float3 float3 = math.float3(c.y, c2.y, c3.y);
			float3 float4 = math.float3(c.z, c2.z, c3.z);
			@float = -(float2 * @float.x + float3 * @float.y + float4 * @float.z);
			return math.float3x4(float2, float3, float4, @float);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00006E9C File Offset: 0x0000509C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float3x4 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint3(4192899797U, 3271228601U, 1634639009U) + math.asuint(v.c1) * math.uint3(3318036811U, 3404170631U, 2048213449U) + math.asuint(v.c2) * math.uint3(4164671783U, 1780759499U, 1352369353U) + math.asuint(v.c3) * math.uint3(2446407751U, 1391928079U, 3475533443U)) + 3777095341U;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00006F54 File Offset: 0x00005154
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(float3x4 v)
		{
			return math.asuint(v.c0) * math.uint3(3385463369U, 1773538433U, 3773525029U) + math.asuint(v.c1) * math.uint3(4131962539U, 1809525511U, 4016293529U) + math.asuint(v.c2) * math.uint3(2416021567U, 2828384717U, 2636362241U) + math.asuint(v.c3) * math.uint3(1258410977U, 1952565773U, 2037535609U) + 3592785499U;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000700A File Offset: 0x0000520A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float x, float y, float z, float w)
		{
			return new float4(x, y, z, w);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00007015 File Offset: 0x00005215
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float x, float y, float2 zw)
		{
			return new float4(x, y, zw);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000701F File Offset: 0x0000521F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float x, float2 yz, float w)
		{
			return new float4(x, yz, w);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007029 File Offset: 0x00005229
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float x, float3 yzw)
		{
			return new float4(x, yzw);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007032 File Offset: 0x00005232
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float2 xy, float z, float w)
		{
			return new float4(xy, z, w);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000703C File Offset: 0x0000523C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float2 xy, float2 zw)
		{
			return new float4(xy, zw);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00007045 File Offset: 0x00005245
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float3 xyz, float w)
		{
			return new float4(xyz, w);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000704E File Offset: 0x0000524E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float4 xyzw)
		{
			return new float4(xyzw);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007056 File Offset: 0x00005256
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(float v)
		{
			return new float4(v);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000705E File Offset: 0x0000525E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(bool v)
		{
			return new float4(v);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00007066 File Offset: 0x00005266
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(bool4 v)
		{
			return new float4(v);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000706E File Offset: 0x0000526E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(int v)
		{
			return new float4(v);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007076 File Offset: 0x00005276
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(int4 v)
		{
			return new float4(v);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000707E File Offset: 0x0000527E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(uint v)
		{
			return new float4(v);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007086 File Offset: 0x00005286
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(uint4 v)
		{
			return new float4(v);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000708E File Offset: 0x0000528E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(half v)
		{
			return new float4(v);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00007096 File Offset: 0x00005296
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(half4 v)
		{
			return new float4(v);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000709E File Offset: 0x0000529E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(double v)
		{
			return new float4(v);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000070A6 File Offset: 0x000052A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 float4(double4 v)
		{
			return new float4(v);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000070AE File Offset: 0x000052AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float4 v)
		{
			return math.csum(math.asuint(v) * math.uint4(3868600063U, 3170963179U, 2632835537U, 1136528209U)) + 2944626401U;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000070DF File Offset: 0x000052DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(float4 v)
		{
			return math.asuint(v) * math.uint4(2972762423U, 1417889653U, 2080514593U, 2731544287U) + 2828498809U;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000710F File Offset: 0x0000530F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float shuffle(float4 left, float4 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007119 File Offset: 0x00005319
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 shuffle(float4 left, float4 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.float2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007130 File Offset: 0x00005330
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 shuffle(float4 left, float4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.float3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007150 File Offset: 0x00005350
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 shuffle(float4 left, float4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.float4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000717C File Offset: 0x0000537C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float select_shuffle_component(float4 a, float4 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.LeftW:
				return a.w;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			case math.ShuffleComponent.RightW:
				return b.w;
			default:
				throw new ArgumentException("Invalid shuffle component: " + component.ToString());
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007205 File Offset: 0x00005405
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(float4 c0, float4 c1)
		{
			return new float4x2(c0, c1);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000720E File Offset: 0x0000540E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(float m00, float m01, float m10, float m11, float m20, float m21, float m30, float m31)
		{
			return new float4x2(m00, m01, m10, m11, m20, m21, m30, m31);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007221 File Offset: 0x00005421
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(float v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007229 File Offset: 0x00005429
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(bool v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007231 File Offset: 0x00005431
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(bool4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007239 File Offset: 0x00005439
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(int v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007241 File Offset: 0x00005441
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(int4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007249 File Offset: 0x00005449
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(uint v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007251 File Offset: 0x00005451
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(uint4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007259 File Offset: 0x00005459
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(double v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007261 File Offset: 0x00005461
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 float4x2(double4x2 v)
		{
			return new float4x2(v);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000726C File Offset: 0x0000546C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 transpose(float4x2 v)
		{
			return math.float2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000072D8 File Offset: 0x000054D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float4x2 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint4(2864955997U, 3525118277U, 2298260269U, 1632478733U) + math.asuint(v.c1) * math.uint4(1537393931U, 2353355467U, 3441847433U, 4052036147U)) + 2011389559U;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007348 File Offset: 0x00005548
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(float4x2 v)
		{
			return math.asuint(v.c0) * math.uint4(2252224297U, 3784421429U, 1750626223U, 3571447507U) + math.asuint(v.c1) * math.uint4(3412283213U, 2601761069U, 1254033427U, 2248573027U) + 3612677113U;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000073B6 File Offset: 0x000055B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(float4 c0, float4 c1, float4 c2)
		{
			return new float4x3(c0, c1, c2);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000073C0 File Offset: 0x000055C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22, float m30, float m31, float m32)
		{
			return new float4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000073E6 File Offset: 0x000055E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(float v)
		{
			return new float4x3(v);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000073EE File Offset: 0x000055EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(bool v)
		{
			return new float4x3(v);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000073F6 File Offset: 0x000055F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(bool4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000073FE File Offset: 0x000055FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(int v)
		{
			return new float4x3(v);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00007406 File Offset: 0x00005606
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(int4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000740E File Offset: 0x0000560E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(uint v)
		{
			return new float4x3(v);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007416 File Offset: 0x00005616
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(uint4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000741E File Offset: 0x0000561E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(double v)
		{
			return new float4x3(v);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00007426 File Offset: 0x00005626
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 float4x3(double4x3 v)
		{
			return new float4x3(v);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007430 File Offset: 0x00005630
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 transpose(float4x3 v)
		{
			return math.float3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000074C8 File Offset: 0x000056C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float4x3 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint4(3309258581U, 1770373673U, 3778261171U, 3286279097U) + math.asuint(v.c1) * math.uint4(4264629071U, 1898591447U, 2641864091U, 1229113913U) + math.asuint(v.c2) * math.uint4(3020867117U, 1449055807U, 2479033387U, 3702457169U)) + 1845824257U;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007568 File Offset: 0x00005768
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(float4x3 v)
		{
			return math.asuint(v.c0) * math.uint4(1963973621U, 2134758553U, 1391111867U, 1167706003U) + math.asuint(v.c1) * math.uint4(2209736489U, 3261535807U, 1740411209U, 2910609089U) + math.asuint(v.c2) * math.uint4(2183822701U, 3029516053U, 3547472099U, 2057487037U) + 3781937309U;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00007604 File Offset: 0x00005804
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(float4 c0, float4 c1, float4 c2, float4 c3)
		{
			return new float4x4(c0, c1, c2, c3);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007610 File Offset: 0x00005810
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
		{
			return new float4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000763E File Offset: 0x0000583E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(float v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00007646 File Offset: 0x00005846
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(bool v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000764E File Offset: 0x0000584E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(bool4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00007656 File Offset: 0x00005856
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(int v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000765E File Offset: 0x0000585E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(int4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00007666 File Offset: 0x00005866
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(uint v)
		{
			return new float4x4(v);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000766E File Offset: 0x0000586E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(uint4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007676 File Offset: 0x00005876
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(double v)
		{
			return new float4x4(v);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000767E File Offset: 0x0000587E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(double4x4 v)
		{
			return new float4x4(v);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00007688 File Offset: 0x00005888
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 rotate(float4x4 a, float3 b)
		{
			return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z).xyz;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000076DC File Offset: 0x000058DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 transform(float4x4 a, float3 b)
		{
			return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3).xyz;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000773C File Offset: 0x0000593C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 transpose(float4x4 v)
		{
			return math.float4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00007800 File Offset: 0x00005A00
		public static float4x4 inverse(float4x4 m)
		{
			float4 c = m.c0;
			float4 c2 = m.c1;
			float4 c3 = m.c2;
			float4 c4 = m.c3;
			float4 @float = math.movelh(c2, c);
			float4 float2 = math.movelh(c3, c4);
			float4 float3 = math.movehl(c, c2);
			float4 float4 = math.movehl(c4, c3);
			float4 float5 = math.shuffle(c2, c, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightY, math.ShuffleComponent.RightZ);
			float4 float6 = math.shuffle(c3, c4, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightY, math.ShuffleComponent.RightZ);
			float4 float7 = math.shuffle(c2, c, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightW, math.ShuffleComponent.RightX);
			float4 float8 = math.shuffle(c3, c4, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightW, math.ShuffleComponent.RightX);
			float4 float9 = math.shuffle(float2, @float, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.RightZ);
			float4 float10 = math.shuffle(float2, @float, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightY, math.ShuffleComponent.RightW);
			float4 float11 = math.shuffle(float4, float3, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.RightZ);
			float4 float12 = math.shuffle(float4, float3, math.ShuffleComponent.LeftW, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightY, math.ShuffleComponent.RightW);
			float4 float13 = math.shuffle(@float, float2, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.RightZ);
			float4 float14 = float5 * float4 - float6 * float3;
			float4 float15 = @float * float4 - float2 * float3;
			float4 float16 = float8 * @float - float7 * float2;
			float4 float17 = math.shuffle(float14, float14, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightX);
			float4 float18 = math.shuffle(float14, float14, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW, math.ShuffleComponent.RightY);
			float4 float19 = math.shuffle(float15, float15, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightX);
			float4 float20 = math.shuffle(float15, float15, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW, math.ShuffleComponent.RightY);
			float4 float21 = float12 * float17 - float11 * float20 + float10 * float18;
			float4 float22 = float13 * float21;
			float22 += math.shuffle(float22, float22, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightW, math.ShuffleComponent.RightZ);
			float22 -= math.shuffle(float22, float22, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightX, math.ShuffleComponent.RightX);
			float4 float23 = math.float4(1f) / float22;
			float4x4 float4x;
			float4x.c0 = float21 * float23;
			float4 float24 = math.shuffle(float16, float16, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightX);
			float4 float25 = math.shuffle(float16, float16, math.ShuffleComponent.LeftY, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW, math.ShuffleComponent.RightY);
			float4 float26 = float11 * float24 - float9 * float18 - float12 * float19;
			float4x.c1 = float26 * float23;
			float4 float27 = float9 * float20 - float10 * float24 - float12 * float25;
			float4x.c2 = float27 * float23;
			float4 float28 = float10 * float19 - float9 * float17 + float11 * float25;
			float4x.c3 = float28 * float23;
			return float4x;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00007A90 File Offset: 0x00005C90
		public static float4x4 fastinverse(float4x4 m)
		{
			float4 c = m.c0;
			float4 c2 = m.c1;
			float4 c3 = m.c2;
			float4 @float = m.c3;
			float4 float2 = math.float4(0);
			float4 float3 = math.unpacklo(c, c3);
			float4 float4 = math.unpacklo(c2, float2);
			float4 float5 = math.unpackhi(c, c3);
			float4 float6 = math.unpackhi(c2, float2);
			float4 float7 = math.unpacklo(float3, float4);
			float4 float8 = math.unpackhi(float3, float4);
			float4 float9 = math.unpacklo(float5, float6);
			@float = -(float7 * @float.x + float8 * @float.y + float9 * @float.z);
			@float.w = 1f;
			return math.float4x4(float7, float8, float9, @float);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00007B50 File Offset: 0x00005D50
		public static float determinant(float4x4 m)
		{
			float4 c = m.c0;
			float4 c2 = m.c1;
			float4 c3 = m.c2;
			float4 c4 = m.c3;
			float num = c2.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c2.z * c4.w - c2.w * c4.z) + c4.y * (c2.z * c3.w - c2.w * c3.z);
			float num2 = c.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c3.w - c.w * c3.z);
			float num3 = c.y * (c2.z * c4.w - c2.w * c4.z) - c2.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c2.w - c.w * c2.z);
			float num4 = c.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c.z * c3.w - c.w * c3.z) + c3.y * (c.z * c2.w - c.w * c2.z);
			return c.x * num - c2.x * num2 + c3.x * num3 - c4.x * num4;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00007D48 File Offset: 0x00005F48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(float4x4 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint4(3299952959U, 3121178323U, 2948522579U, 1531026433U) + math.asuint(v.c1) * math.uint4(1365086453U, 3969870067U, 4192899797U, 3271228601U) + math.asuint(v.c2) * math.uint4(1634639009U, 3318036811U, 3404170631U, 2048213449U) + math.asuint(v.c3) * math.uint4(4164671783U, 1780759499U, 1352369353U, 2446407751U)) + 1391928079U;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00007E14 File Offset: 0x00006014
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(float4x4 v)
		{
			return math.asuint(v.c0) * math.uint4(3475533443U, 3777095341U, 3385463369U, 1773538433U) + math.asuint(v.c1) * math.uint4(3773525029U, 4131962539U, 1809525511U, 4016293529U) + math.asuint(v.c2) * math.uint4(2416021567U, 2828384717U, 2636362241U, 1258410977U) + math.asuint(v.c3) * math.uint4(1952565773U, 2037535609U, 3592785499U, 3996716183U) + 2626301701U;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00007EDE File Offset: 0x000060DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half half(half x)
		{
			return new half(x);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00007EE6 File Offset: 0x000060E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half half(float v)
		{
			return new half(v);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00007EEE File Offset: 0x000060EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half half(double v)
		{
			return new half(v);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00007EF6 File Offset: 0x000060F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(half v)
		{
			return (uint)v.value * 1952372791U + 2171534173U;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00007F0A File Offset: 0x0000610A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half2 half2(half x, half y)
		{
			return new half2(x, y);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00007F13 File Offset: 0x00006113
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half2 half2(half2 xy)
		{
			return new half2(xy);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00007F1B File Offset: 0x0000611B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half2 half2(half v)
		{
			return new half2(v);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00007F23 File Offset: 0x00006123
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half2 half2(float v)
		{
			return new half2(v);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00007F2B File Offset: 0x0000612B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half2 half2(float2 v)
		{
			return new half2(v);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00007F33 File Offset: 0x00006133
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half2 half2(double v)
		{
			return new half2(v);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00007F3B File Offset: 0x0000613B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half2 half2(double2 v)
		{
			return new half2(v);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00007F43 File Offset: 0x00006143
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(half2 v)
		{
			return math.csum(math.uint2((uint)v.x.value, (uint)v.y.value) * math.uint2(1851936439U, 1938025801U)) + 3712598587U;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00007F7F File Offset: 0x0000617F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(half2 v)
		{
			return math.uint2((uint)v.x.value, (uint)v.y.value) * math.uint2(3956330501U, 2437373431U) + 1441286183U;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00007FBA File Offset: 0x000061BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(half x, half y, half z)
		{
			return new half3(x, y, z);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00007FC4 File Offset: 0x000061C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(half x, half2 yz)
		{
			return new half3(x, yz);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00007FCD File Offset: 0x000061CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(half2 xy, half z)
		{
			return new half3(xy, z);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00007FD6 File Offset: 0x000061D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(half3 xyz)
		{
			return new half3(xyz);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00007FDE File Offset: 0x000061DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(half v)
		{
			return new half3(v);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00007FE6 File Offset: 0x000061E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(float v)
		{
			return new half3(v);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00007FEE File Offset: 0x000061EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(float3 v)
		{
			return new half3(v);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00007FF6 File Offset: 0x000061F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(double v)
		{
			return new half3(v);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00007FFE File Offset: 0x000061FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half3 half3(double3 v)
		{
			return new half3(v);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008008 File Offset: 0x00006208
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(half3 v)
		{
			return math.csum(math.uint3((uint)v.x.value, (uint)v.y.value, (uint)v.z.value) * math.uint3(1750611407U, 3285396193U, 3110507567U)) + 4271396531U;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008060 File Offset: 0x00006260
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(half3 v)
		{
			return math.uint3((uint)v.x.value, (uint)v.y.value, (uint)v.z.value) * math.uint3(4198118021U, 2908068253U, 3705492289U) + 2497566569U;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000080B6 File Offset: 0x000062B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half x, half y, half z, half w)
		{
			return new half4(x, y, z, w);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000080C1 File Offset: 0x000062C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half x, half y, half2 zw)
		{
			return new half4(x, y, zw);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000080CB File Offset: 0x000062CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half x, half2 yz, half w)
		{
			return new half4(x, yz, w);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000080D5 File Offset: 0x000062D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half x, half3 yzw)
		{
			return new half4(x, yzw);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000080DE File Offset: 0x000062DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half2 xy, half z, half w)
		{
			return new half4(xy, z, w);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000080E8 File Offset: 0x000062E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half2 xy, half2 zw)
		{
			return new half4(xy, zw);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000080F1 File Offset: 0x000062F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half3 xyz, half w)
		{
			return new half4(xyz, w);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000080FA File Offset: 0x000062FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half4 xyzw)
		{
			return new half4(xyzw);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008102 File Offset: 0x00006302
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(half v)
		{
			return new half4(v);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000810A File Offset: 0x0000630A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(float v)
		{
			return new half4(v);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00008112 File Offset: 0x00006312
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(float4 v)
		{
			return new half4(v);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000811A File Offset: 0x0000631A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(double v)
		{
			return new half4(v);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008122 File Offset: 0x00006322
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static half4 half4(double4 v)
		{
			return new half4(v);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000812C File Offset: 0x0000632C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(half4 v)
		{
			return math.csum(math.uint4((uint)v.x.value, (uint)v.y.value, (uint)v.z.value, (uint)v.w.value) * math.uint4(1952372791U, 2631698677U, 4200781601U, 2119021007U)) + 1760485621U;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00008194 File Offset: 0x00006394
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(half4 v)
		{
			return math.uint4((uint)v.x.value, (uint)v.y.value, (uint)v.z.value, (uint)v.w.value) * math.uint4(3157985881U, 2171534173U, 2723054263U, 1168253063U) + 4228926523U;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000081FA File Offset: 0x000063FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(int x, int y)
		{
			return new int2(x, y);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00008203 File Offset: 0x00006403
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(int2 xy)
		{
			return new int2(xy);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000820B File Offset: 0x0000640B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(int v)
		{
			return new int2(v);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008213 File Offset: 0x00006413
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(bool v)
		{
			return new int2(v);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000821B File Offset: 0x0000641B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(bool2 v)
		{
			return new int2(v);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00008223 File Offset: 0x00006423
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(uint v)
		{
			return new int2(v);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000822B File Offset: 0x0000642B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(uint2 v)
		{
			return new int2(v);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00008233 File Offset: 0x00006433
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(float v)
		{
			return new int2(v);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000823B File Offset: 0x0000643B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(float2 v)
		{
			return new int2(v);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00008243 File Offset: 0x00006443
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(double v)
		{
			return new int2(v);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000824B File Offset: 0x0000644B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 int2(double2 v)
		{
			return new int2(v);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00008253 File Offset: 0x00006453
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int2 v)
		{
			return math.csum(math.asuint(v) * math.uint2(2209710647U, 2201894441U)) + 2849577407U;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000827A File Offset: 0x0000647A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(int2 v)
		{
			return math.asuint(v) * math.uint2(3287031191U, 3098675399U) + 1564399943U;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000082A0 File Offset: 0x000064A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int shuffle(int2 left, int2 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000082AA File Offset: 0x000064AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 shuffle(int2 left, int2 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.int2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000082C1 File Offset: 0x000064C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 shuffle(int2 left, int2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.int3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000082E1 File Offset: 0x000064E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 shuffle(int2 left, int2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.int4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000830C File Offset: 0x0000650C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int select_shuffle_component(int2 a, int2 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00008371 File Offset: 0x00006571
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(int2 c0, int2 c1)
		{
			return new int2x2(c0, c1);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000837A File Offset: 0x0000657A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(int m00, int m01, int m10, int m11)
		{
			return new int2x2(m00, m01, m10, m11);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00008385 File Offset: 0x00006585
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(int v)
		{
			return new int2x2(v);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000838D File Offset: 0x0000658D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(bool v)
		{
			return new int2x2(v);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00008395 File Offset: 0x00006595
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(bool2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000839D File Offset: 0x0000659D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(uint v)
		{
			return new int2x2(v);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000083A5 File Offset: 0x000065A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(uint2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000083AD File Offset: 0x000065AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(float v)
		{
			return new int2x2(v);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000083B5 File Offset: 0x000065B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(float2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000083BD File Offset: 0x000065BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(double v)
		{
			return new int2x2(v);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000083C5 File Offset: 0x000065C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 int2x2(double2x2 v)
		{
			return new int2x2(v);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x000083CD File Offset: 0x000065CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 transpose(int2x2 v)
		{
			return math.int2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00008400 File Offset: 0x00006600
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int determinant(int2x2 m)
		{
			int x = m.c0.x;
			int x2 = m.c1.x;
			int y = m.c0.y;
			int y2 = m.c1.y;
			return x * y2 - x2 * y;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00008444 File Offset: 0x00006644
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int2x2 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint2(3784421429U, 1750626223U) + math.asuint(v.c1) * math.uint2(3571447507U, 3412283213U)) + 2601761069U;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000084A0 File Offset: 0x000066A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(int2x2 v)
		{
			return math.asuint(v.c0) * math.uint2(1254033427U, 2248573027U) + math.asuint(v.c1) * math.uint2(3612677113U, 1521739981U) + 1735296007U;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000084FA File Offset: 0x000066FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(int2 c0, int2 c1, int2 c2)
		{
			return new int2x3(c0, c1, c2);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00008504 File Offset: 0x00006704
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(int m00, int m01, int m02, int m10, int m11, int m12)
		{
			return new int2x3(m00, m01, m02, m10, m11, m12);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00008513 File Offset: 0x00006713
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(int v)
		{
			return new int2x3(v);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000851B File Offset: 0x0000671B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(bool v)
		{
			return new int2x3(v);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00008523 File Offset: 0x00006723
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(bool2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000852B File Offset: 0x0000672B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(uint v)
		{
			return new int2x3(v);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00008533 File Offset: 0x00006733
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(uint2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000853B File Offset: 0x0000673B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(float v)
		{
			return new int2x3(v);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00008543 File Offset: 0x00006743
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(float2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000854B File Offset: 0x0000674B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(double v)
		{
			return new int2x3(v);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00008553 File Offset: 0x00006753
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 int2x3(double2x3 v)
		{
			return new int2x3(v);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000855C File Offset: 0x0000675C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 transpose(int2x3 v)
		{
			return math.int3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x000085B0 File Offset: 0x000067B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int2x3 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint2(3404170631U, 2048213449U) + math.asuint(v.c1) * math.uint2(4164671783U, 1780759499U) + math.asuint(v.c2) * math.uint2(1352369353U, 2446407751U)) + 1391928079U;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00008630 File Offset: 0x00006830
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(int2x3 v)
		{
			return math.asuint(v.c0) * math.uint2(3475533443U, 3777095341U) + math.asuint(v.c1) * math.uint2(3385463369U, 1773538433U) + math.asuint(v.c2) * math.uint2(3773525029U, 4131962539U) + 1809525511U;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x000086AE File Offset: 0x000068AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(int2 c0, int2 c1, int2 c2, int2 c3)
		{
			return new int2x4(c0, c1, c2, c3);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x000086B9 File Offset: 0x000068B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13)
		{
			return new int2x4(m00, m01, m02, m03, m10, m11, m12, m13);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000086CC File Offset: 0x000068CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(int v)
		{
			return new int2x4(v);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000086D4 File Offset: 0x000068D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(bool v)
		{
			return new int2x4(v);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000086DC File Offset: 0x000068DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(bool2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000086E4 File Offset: 0x000068E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(uint v)
		{
			return new int2x4(v);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000086EC File Offset: 0x000068EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(uint2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000086F4 File Offset: 0x000068F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(float v)
		{
			return new int2x4(v);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000086FC File Offset: 0x000068FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(float2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00008704 File Offset: 0x00006904
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(double v)
		{
			return new int2x4(v);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000870C File Offset: 0x0000690C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 int2x4(double2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00008714 File Offset: 0x00006914
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 transpose(int2x4 v)
		{
			return math.int4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00008780 File Offset: 0x00006980
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int2x4 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint2(2057338067U, 2942577577U) + math.asuint(v.c1) * math.uint2(2834440507U, 2671762487U) + math.asuint(v.c2) * math.uint2(2892026051U, 2455987759U) + math.asuint(v.c3) * math.uint2(3868600063U, 3170963179U)) + 2632835537U;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00008824 File Offset: 0x00006A24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(int2x4 v)
		{
			return math.asuint(v.c0) * math.uint2(1136528209U, 2944626401U) + math.asuint(v.c1) * math.uint2(2972762423U, 1417889653U) + math.asuint(v.c2) * math.uint2(2080514593U, 2731544287U) + math.asuint(v.c3) * math.uint2(2828498809U, 2669441947U) + 1260114311U;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000088C6 File Offset: 0x00006AC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(int x, int y, int z)
		{
			return new int3(x, y, z);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000088D0 File Offset: 0x00006AD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(int x, int2 yz)
		{
			return new int3(x, yz);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000088D9 File Offset: 0x00006AD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(int2 xy, int z)
		{
			return new int3(xy, z);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000088E2 File Offset: 0x00006AE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(int3 xyz)
		{
			return new int3(xyz);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000088EA File Offset: 0x00006AEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(int v)
		{
			return new int3(v);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000088F2 File Offset: 0x00006AF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(bool v)
		{
			return new int3(v);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000088FA File Offset: 0x00006AFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(bool3 v)
		{
			return new int3(v);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00008902 File Offset: 0x00006B02
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(uint v)
		{
			return new int3(v);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000890A File Offset: 0x00006B0A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(uint3 v)
		{
			return new int3(v);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00008912 File Offset: 0x00006B12
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(float v)
		{
			return new int3(v);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000891A File Offset: 0x00006B1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(float3 v)
		{
			return new int3(v);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00008922 File Offset: 0x00006B22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(double v)
		{
			return new int3(v);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000892A File Offset: 0x00006B2A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 int3(double3 v)
		{
			return new int3(v);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00008932 File Offset: 0x00006B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int3 v)
		{
			return math.csum(math.asuint(v) * math.uint3(1283419601U, 1210229737U, 2864955997U)) + 3525118277U;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000895E File Offset: 0x00006B5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(int3 v)
		{
			return math.asuint(v) * math.uint3(2298260269U, 1632478733U, 1537393931U) + 2353355467U;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00008989 File Offset: 0x00006B89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int shuffle(int3 left, int3 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00008993 File Offset: 0x00006B93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 shuffle(int3 left, int3 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.int2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000089AA File Offset: 0x00006BAA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 shuffle(int3 left, int3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.int3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000089CA File Offset: 0x00006BCA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 shuffle(int3 left, int3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.int4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000089F4 File Offset: 0x00006BF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int select_shuffle_component(int3 a, int3 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00008A6B File Offset: 0x00006C6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(int3 c0, int3 c1)
		{
			return new int3x2(c0, c1);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00008A74 File Offset: 0x00006C74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(int m00, int m01, int m10, int m11, int m20, int m21)
		{
			return new int3x2(m00, m01, m10, m11, m20, m21);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00008A83 File Offset: 0x00006C83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(int v)
		{
			return new int3x2(v);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00008A8B File Offset: 0x00006C8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(bool v)
		{
			return new int3x2(v);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00008A93 File Offset: 0x00006C93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(bool3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00008A9B File Offset: 0x00006C9B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(uint v)
		{
			return new int3x2(v);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00008AA3 File Offset: 0x00006CA3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(uint3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00008AAB File Offset: 0x00006CAB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(float v)
		{
			return new int3x2(v);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00008AB3 File Offset: 0x00006CB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(float3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00008ABB File Offset: 0x00006CBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(double v)
		{
			return new int3x2(v);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00008AC3 File Offset: 0x00006CC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 int3x2(double3x2 v)
		{
			return new int3x2(v);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008ACC File Offset: 0x00006CCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 transpose(int3x2 v)
		{
			return math.int2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00008B20 File Offset: 0x00006D20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int3x2 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint3(3678265601U, 2070747979U, 1480171127U) + math.asuint(v.c1) * math.uint3(1588341193U, 4234155257U, 1811310911U)) + 2635799963U;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00008B88 File Offset: 0x00006D88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(int3x2 v)
		{
			return math.asuint(v.c0) * math.uint3(4165137857U, 2759770933U, 2759319383U) + math.asuint(v.c1) * math.uint3(3299952959U, 3121178323U, 2948522579U) + 1531026433U;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00008BEC File Offset: 0x00006DEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(int3 c0, int3 c1, int3 c2)
		{
			return new int3x3(c0, c1, c2);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008BF8 File Offset: 0x00006DF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(int m00, int m01, int m02, int m10, int m11, int m12, int m20, int m21, int m22)
		{
			return new int3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008C18 File Offset: 0x00006E18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(int v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00008C20 File Offset: 0x00006E20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(bool v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00008C28 File Offset: 0x00006E28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(bool3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00008C30 File Offset: 0x00006E30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(uint v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00008C38 File Offset: 0x00006E38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(uint3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00008C40 File Offset: 0x00006E40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(float v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00008C48 File Offset: 0x00006E48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(float3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00008C50 File Offset: 0x00006E50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(double v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00008C58 File Offset: 0x00006E58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 int3x3(double3x3 v)
		{
			return new int3x3(v);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00008C60 File Offset: 0x00006E60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 transpose(int3x3 v)
		{
			return math.int3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00008CD8 File Offset: 0x00006ED8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int determinant(int3x3 m)
		{
			int3 c = m.c0;
			int3 c2 = m.c1;
			int3 c3 = m.c2;
			int num = c2.y * c3.z - c2.z * c3.y;
			int num2 = c.y * c3.z - c.z * c3.y;
			int num3 = c.y * c2.z - c.z * c2.y;
			return c.x * num - c2.x * num2 + c3.x * num3;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00008D6C File Offset: 0x00006F6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int3x3 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint3(2479033387U, 3702457169U, 1845824257U) + math.asuint(v.c1) * math.uint3(1963973621U, 2134758553U, 1391111867U) + math.asuint(v.c2) * math.uint3(1167706003U, 2209736489U, 3261535807U)) + 1740411209U;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00008DFC File Offset: 0x00006FFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(int3x3 v)
		{
			return math.asuint(v.c0) * math.uint3(2910609089U, 2183822701U, 3029516053U) + math.asuint(v.c1) * math.uint3(3547472099U, 2057487037U, 3781937309U) + math.asuint(v.c2) * math.uint3(2057338067U, 2942577577U, 2834440507U) + 2671762487U;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00008E89 File Offset: 0x00007089
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(int3 c0, int3 c1, int3 c2, int3 c3)
		{
			return new int3x4(c0, c1, c2, c3);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00008E94 File Offset: 0x00007094
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23)
		{
			return new int3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00008EBA File Offset: 0x000070BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(int v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00008EC2 File Offset: 0x000070C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(bool v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00008ECA File Offset: 0x000070CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(bool3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00008ED2 File Offset: 0x000070D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(uint v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00008EDA File Offset: 0x000070DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(uint3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00008EE2 File Offset: 0x000070E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(float v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00008EEA File Offset: 0x000070EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(float3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00008EF2 File Offset: 0x000070F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(double v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00008EFA File Offset: 0x000070FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 int3x4(double3x4 v)
		{
			return new int3x4(v);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00008F04 File Offset: 0x00007104
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 transpose(int3x4 v)
		{
			return math.int4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00008F9C File Offset: 0x0000719C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int3x4 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint3(1521739981U, 1735296007U, 3010324327U) + math.asuint(v.c1) * math.uint3(1875523709U, 2937008387U, 3835713223U) + math.asuint(v.c2) * math.uint3(2216526373U, 3375971453U, 3559829411U) + math.asuint(v.c3) * math.uint3(3652178029U, 2544260129U, 2013864031U)) + 2627668003U;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00009054 File Offset: 0x00007254
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(int3x4 v)
		{
			return math.asuint(v.c0) * math.uint3(1520214331U, 2949502447U, 2827819133U) + math.asuint(v.c1) * math.uint3(3480140317U, 2642994593U, 3940484981U) + math.asuint(v.c2) * math.uint3(1954192763U, 1091696537U, 3052428017U) + math.asuint(v.c3) * math.uint3(4253034763U, 2338696631U, 3757372771U) + 1885959949U;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000910A File Offset: 0x0000730A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int x, int y, int z, int w)
		{
			return new int4(x, y, z, w);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00009115 File Offset: 0x00007315
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int x, int y, int2 zw)
		{
			return new int4(x, y, zw);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000911F File Offset: 0x0000731F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int x, int2 yz, int w)
		{
			return new int4(x, yz, w);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00009129 File Offset: 0x00007329
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int x, int3 yzw)
		{
			return new int4(x, yzw);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00009132 File Offset: 0x00007332
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int2 xy, int z, int w)
		{
			return new int4(xy, z, w);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000913C File Offset: 0x0000733C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int2 xy, int2 zw)
		{
			return new int4(xy, zw);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00009145 File Offset: 0x00007345
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int3 xyz, int w)
		{
			return new int4(xyz, w);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000914E File Offset: 0x0000734E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int4 xyzw)
		{
			return new int4(xyzw);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00009156 File Offset: 0x00007356
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(int v)
		{
			return new int4(v);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000915E File Offset: 0x0000735E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(bool v)
		{
			return new int4(v);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00009166 File Offset: 0x00007366
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(bool4 v)
		{
			return new int4(v);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000916E File Offset: 0x0000736E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(uint v)
		{
			return new int4(v);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00009176 File Offset: 0x00007376
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(uint4 v)
		{
			return new int4(v);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000917E File Offset: 0x0000737E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(float v)
		{
			return new int4(v);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00009186 File Offset: 0x00007386
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(float4 v)
		{
			return new int4(v);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000918E File Offset: 0x0000738E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(double v)
		{
			return new int4(v);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00009196 File Offset: 0x00007396
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 int4(double4 v)
		{
			return new int4(v);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000919E File Offset: 0x0000739E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int4 v)
		{
			return math.csum(math.asuint(v) * math.uint4(1845824257U, 1963973621U, 2134758553U, 1391111867U)) + 1167706003U;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000091CF File Offset: 0x000073CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(int4 v)
		{
			return math.asuint(v) * math.uint4(2209736489U, 3261535807U, 1740411209U, 2910609089U) + 2183822701U;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000091FF File Offset: 0x000073FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int shuffle(int4 left, int4 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00009209 File Offset: 0x00007409
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 shuffle(int4 left, int4 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.int2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00009220 File Offset: 0x00007420
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 shuffle(int4 left, int4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.int3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00009240 File Offset: 0x00007440
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 shuffle(int4 left, int4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.int4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000926C File Offset: 0x0000746C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int select_shuffle_component(int4 a, int4 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.LeftW:
				return a.w;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			case math.ShuffleComponent.RightW:
				return b.w;
			default:
				throw new ArgumentException("Invalid shuffle component: " + component.ToString());
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000092F5 File Offset: 0x000074F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(int4 c0, int4 c1)
		{
			return new int4x2(c0, c1);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000092FE File Offset: 0x000074FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(int m00, int m01, int m10, int m11, int m20, int m21, int m30, int m31)
		{
			return new int4x2(m00, m01, m10, m11, m20, m21, m30, m31);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00009311 File Offset: 0x00007511
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(int v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00009319 File Offset: 0x00007519
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(bool v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00009321 File Offset: 0x00007521
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(bool4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00009329 File Offset: 0x00007529
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(uint v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00009331 File Offset: 0x00007531
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(uint4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00009339 File Offset: 0x00007539
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(float v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00009341 File Offset: 0x00007541
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(float4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00009349 File Offset: 0x00007549
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(double v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00009351 File Offset: 0x00007551
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 int4x2(double4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000935C File Offset: 0x0000755C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 transpose(int4x2 v)
		{
			return math.int2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000093C8 File Offset: 0x000075C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int4x2 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint4(4205774813U, 1650214333U, 3388112843U, 1831150513U) + math.asuint(v.c1) * math.uint4(1848374953U, 3430200247U, 2209710647U, 2201894441U)) + 2849577407U;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00009438 File Offset: 0x00007638
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(int4x2 v)
		{
			return math.asuint(v.c0) * math.uint4(3287031191U, 3098675399U, 1564399943U, 1148435377U) + math.asuint(v.c1) * math.uint4(3416333663U, 1750611407U, 3285396193U, 3110507567U) + 4271396531U;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000094A6 File Offset: 0x000076A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(int4 c0, int4 c1, int4 c2)
		{
			return new int4x3(c0, c1, c2);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000094B0 File Offset: 0x000076B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(int m00, int m01, int m02, int m10, int m11, int m12, int m20, int m21, int m22, int m30, int m31, int m32)
		{
			return new int4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000094D6 File Offset: 0x000076D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(int v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000094DE File Offset: 0x000076DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(bool v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000094E6 File Offset: 0x000076E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(bool4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000094EE File Offset: 0x000076EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(uint v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000094F6 File Offset: 0x000076F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(uint4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000094FE File Offset: 0x000076FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(float v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00009506 File Offset: 0x00007706
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(float4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000950E File Offset: 0x0000770E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(double v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00009516 File Offset: 0x00007716
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 int4x3(double4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00009520 File Offset: 0x00007720
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 transpose(int4x3 v)
		{
			return math.int3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000095B8 File Offset: 0x000077B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int4x3 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint4(1773538433U, 3773525029U, 4131962539U, 1809525511U) + math.asuint(v.c1) * math.uint4(4016293529U, 2416021567U, 2828384717U, 2636362241U) + math.asuint(v.c2) * math.uint4(1258410977U, 1952565773U, 2037535609U, 3592785499U)) + 3996716183U;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00009658 File Offset: 0x00007858
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(int4x3 v)
		{
			return math.asuint(v.c0) * math.uint4(2626301701U, 1306289417U, 2096137163U, 1548578029U) + math.asuint(v.c1) * math.uint4(4178800919U, 3898072289U, 4129428421U, 2631575897U) + math.asuint(v.c2) * math.uint4(2854656703U, 3578504047U, 4245178297U, 2173281923U) + 2973357649U;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000096F4 File Offset: 0x000078F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(int4 c0, int4 c1, int4 c2, int4 c3)
		{
			return new int4x4(c0, c1, c2, c3);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00009700 File Offset: 0x00007900
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23, int m30, int m31, int m32, int m33)
		{
			return new int4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000972E File Offset: 0x0000792E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(int v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00009736 File Offset: 0x00007936
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(bool v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000973E File Offset: 0x0000793E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(bool4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00009746 File Offset: 0x00007946
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(uint v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000974E File Offset: 0x0000794E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(uint4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00009756 File Offset: 0x00007956
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(float v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000975E File Offset: 0x0000795E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(float4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00009766 File Offset: 0x00007966
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(double v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000976E File Offset: 0x0000796E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 int4x4(double4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00009778 File Offset: 0x00007978
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 transpose(int4x4 v)
		{
			return math.int4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000983C File Offset: 0x00007A3C
		public static int determinant(int4x4 m)
		{
			int4 c = m.c0;
			int4 c2 = m.c1;
			int4 c3 = m.c2;
			int4 c4 = m.c3;
			int num = c2.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c2.z * c4.w - c2.w * c4.z) + c4.y * (c2.z * c3.w - c2.w * c3.z);
			int num2 = c.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c3.w - c.w * c3.z);
			int num3 = c.y * (c2.z * c4.w - c2.w * c4.z) - c2.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c2.w - c.w * c2.z);
			int num4 = c.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c.z * c3.w - c.w * c3.z) + c3.y * (c.z * c2.w - c.w * c2.z);
			return c.x * num - c2.x * num2 + c3.x * num3 - c4.x * num4;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00009A34 File Offset: 0x00007C34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(int4x4 v)
		{
			return math.csum(math.asuint(v.c0) * math.uint4(1562056283U, 2265541847U, 1283419601U, 1210229737U) + math.asuint(v.c1) * math.uint4(2864955997U, 3525118277U, 2298260269U, 1632478733U) + math.asuint(v.c2) * math.uint4(1537393931U, 2353355467U, 3441847433U, 4052036147U) + math.asuint(v.c3) * math.uint4(2011389559U, 2252224297U, 3784421429U, 1750626223U)) + 3571447507U;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00009B00 File Offset: 0x00007D00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(int4x4 v)
		{
			return math.asuint(v.c0) * math.uint4(3412283213U, 2601761069U, 1254033427U, 2248573027U) + math.asuint(v.c1) * math.uint4(3612677113U, 1521739981U, 1735296007U, 3010324327U) + math.asuint(v.c2) * math.uint4(1875523709U, 2937008387U, 3835713223U, 2216526373U) + math.asuint(v.c3) * math.uint4(3375971453U, 3559829411U, 3652178029U, 2544260129U) + 2013864031U;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00009BCA File Offset: 0x00007DCA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int asint(uint x)
		{
			return (int)(*(&x));
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00009BD0 File Offset: 0x00007DD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int2 asint(uint2 x)
		{
			return *(int2*)(&x);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00009BDA File Offset: 0x00007DDA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int3 asint(uint3 x)
		{
			return *(int3*)(&x);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00009BE4 File Offset: 0x00007DE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int4 asint(uint4 x)
		{
			return *(int4*)(&x);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00009BEE File Offset: 0x00007DEE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int asint(float x)
		{
			return *(int*)(&x);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00009BF4 File Offset: 0x00007DF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int2 asint(float2 x)
		{
			return *(int2*)(&x);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00009BFE File Offset: 0x00007DFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int3 asint(float3 x)
		{
			return *(int3*)(&x);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00009C08 File Offset: 0x00007E08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int4 asint(float4 x)
		{
			return *(int4*)(&x);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00009C12 File Offset: 0x00007E12
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint asuint(int x)
		{
			return (uint)x;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00009C15 File Offset: 0x00007E15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint2 asuint(int2 x)
		{
			return *(uint2*)(&x);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00009C1F File Offset: 0x00007E1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint3 asuint(int3 x)
		{
			return *(uint3*)(&x);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00009C29 File Offset: 0x00007E29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint4 asuint(int4 x)
		{
			return *(uint4*)(&x);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00009C33 File Offset: 0x00007E33
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint asuint(float x)
		{
			return *(uint*)(&x);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00009C39 File Offset: 0x00007E39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint2 asuint(float2 x)
		{
			return *(uint2*)(&x);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00009C43 File Offset: 0x00007E43
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint3 asuint(float3 x)
		{
			return *(uint3*)(&x);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00009C4D File Offset: 0x00007E4D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint4 asuint(float4 x)
		{
			return *(uint4*)(&x);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00009C57 File Offset: 0x00007E57
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long aslong(ulong x)
		{
			return (long)x;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00009C5A File Offset: 0x00007E5A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static long aslong(double x)
		{
			return *(long*)(&x);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00009C60 File Offset: 0x00007E60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong asulong(long x)
		{
			return (ulong)x;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00009C63 File Offset: 0x00007E63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ulong asulong(double x)
		{
			return (ulong)(*(long*)(&x));
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00009C69 File Offset: 0x00007E69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float asfloat(int x)
		{
			return *(float*)(&x);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00009C6F File Offset: 0x00007E6F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float2 asfloat(int2 x)
		{
			return *(float2*)(&x);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00009C79 File Offset: 0x00007E79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float3 asfloat(int3 x)
		{
			return *(float3*)(&x);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00009C83 File Offset: 0x00007E83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float4 asfloat(int4 x)
		{
			return *(float4*)(&x);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00009C8D File Offset: 0x00007E8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float asfloat(uint x)
		{
			return *(float*)(&x);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00009C93 File Offset: 0x00007E93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float2 asfloat(uint2 x)
		{
			return *(float2*)(&x);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00009C9D File Offset: 0x00007E9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float3 asfloat(uint3 x)
		{
			return *(float3*)(&x);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00009CA7 File Offset: 0x00007EA7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float4 asfloat(uint4 x)
		{
			return *(float4*)(&x);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00009CB4 File Offset: 0x00007EB4
		public static int bitmask(bool4 value)
		{
			int num = 0;
			if (value.x)
			{
				num |= 1;
			}
			if (value.y)
			{
				num |= 2;
			}
			if (value.z)
			{
				num |= 4;
			}
			if (value.w)
			{
				num |= 8;
			}
			return num;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00009CF4 File Offset: 0x00007EF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static double asdouble(long x)
		{
			return *(double*)(&x);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00009CFA File Offset: 0x00007EFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static double asdouble(ulong x)
		{
			return *(double*)(&x);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00009D00 File Offset: 0x00007F00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool isfinite(float x)
		{
			return math.abs(x) < float.PositiveInfinity;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00009D0F File Offset: 0x00007F0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 isfinite(float2 x)
		{
			return math.abs(x) < float.PositiveInfinity;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00009D21 File Offset: 0x00007F21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 isfinite(float3 x)
		{
			return math.abs(x) < float.PositiveInfinity;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00009D33 File Offset: 0x00007F33
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 isfinite(float4 x)
		{
			return math.abs(x) < float.PositiveInfinity;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00009D45 File Offset: 0x00007F45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool isfinite(double x)
		{
			return math.abs(x) < double.PositiveInfinity;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00009D58 File Offset: 0x00007F58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 isfinite(double2 x)
		{
			return math.abs(x) < double.PositiveInfinity;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00009D6E File Offset: 0x00007F6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 isfinite(double3 x)
		{
			return math.abs(x) < double.PositiveInfinity;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00009D84 File Offset: 0x00007F84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 isfinite(double4 x)
		{
			return math.abs(x) < double.PositiveInfinity;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00009D9A File Offset: 0x00007F9A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool isinf(float x)
		{
			return math.abs(x) == float.PositiveInfinity;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00009DA9 File Offset: 0x00007FA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 isinf(float2 x)
		{
			return math.abs(x) == float.PositiveInfinity;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00009DBB File Offset: 0x00007FBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 isinf(float3 x)
		{
			return math.abs(x) == float.PositiveInfinity;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00009DCD File Offset: 0x00007FCD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 isinf(float4 x)
		{
			return math.abs(x) == float.PositiveInfinity;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00009DDF File Offset: 0x00007FDF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool isinf(double x)
		{
			return math.abs(x) == double.PositiveInfinity;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00009DF2 File Offset: 0x00007FF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 isinf(double2 x)
		{
			return math.abs(x) == double.PositiveInfinity;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00009E08 File Offset: 0x00008008
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 isinf(double3 x)
		{
			return math.abs(x) == double.PositiveInfinity;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00009E1E File Offset: 0x0000801E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 isinf(double4 x)
		{
			return math.abs(x) == double.PositiveInfinity;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00009E34 File Offset: 0x00008034
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool isnan(float x)
		{
			return (math.asuint(x) & 2147483647U) > 2139095040U;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00009E49 File Offset: 0x00008049
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 isnan(float2 x)
		{
			return (math.asuint(x) & 2147483647U) > 2139095040U;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00009E65 File Offset: 0x00008065
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 isnan(float3 x)
		{
			return (math.asuint(x) & 2147483647U) > 2139095040U;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00009E81 File Offset: 0x00008081
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 isnan(float4 x)
		{
			return (math.asuint(x) & 2147483647U) > 2139095040U;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00009E9D File Offset: 0x0000809D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool isnan(double x)
		{
			return (math.asulong(x) & 9223372036854775807UL) > 9218868437227405312UL;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00009EBC File Offset: 0x000080BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 isnan(double2 x)
		{
			return math.bool2((math.asulong(x.x) & 9223372036854775807UL) > 9218868437227405312UL, (math.asulong(x.y) & 9223372036854775807UL) > 9218868437227405312UL);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00009F10 File Offset: 0x00008110
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 isnan(double3 x)
		{
			return math.bool3((math.asulong(x.x) & 9223372036854775807UL) > 9218868437227405312UL, (math.asulong(x.y) & 9223372036854775807UL) > 9218868437227405312UL, (math.asulong(x.z) & 9223372036854775807UL) > 9218868437227405312UL);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00009F84 File Offset: 0x00008184
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 isnan(double4 x)
		{
			return math.bool4((math.asulong(x.x) & 9223372036854775807UL) > 9218868437227405312UL, (math.asulong(x.y) & 9223372036854775807UL) > 9218868437227405312UL, (math.asulong(x.z) & 9223372036854775807UL) > 9218868437227405312UL, (math.asulong(x.w) & 9223372036854775807UL) > 9218868437227405312UL);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000A016 File Offset: 0x00008216
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ispow2(int x)
		{
			return x > 0 && (x & (x - 1)) == 0;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000A026 File Offset: 0x00008226
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 ispow2(int2 x)
		{
			return new bool2(math.ispow2(x.x), math.ispow2(x.y));
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000A043 File Offset: 0x00008243
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 ispow2(int3 x)
		{
			return new bool3(math.ispow2(x.x), math.ispow2(x.y), math.ispow2(x.z));
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000A06B File Offset: 0x0000826B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 ispow2(int4 x)
		{
			return new bool4(math.ispow2(x.x), math.ispow2(x.y), math.ispow2(x.z), math.ispow2(x.w));
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000A09E File Offset: 0x0000829E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ispow2(uint x)
		{
			return x > 0U && (x & (x - 1U)) == 0U;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000A0AE File Offset: 0x000082AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 ispow2(uint2 x)
		{
			return new bool2(math.ispow2(x.x), math.ispow2(x.y));
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000A0CB File Offset: 0x000082CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 ispow2(uint3 x)
		{
			return new bool3(math.ispow2(x.x), math.ispow2(x.y), math.ispow2(x.z));
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000A0F3 File Offset: 0x000082F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4 ispow2(uint4 x)
		{
			return new bool4(math.ispow2(x.x), math.ispow2(x.y), math.ispow2(x.z), math.ispow2(x.w));
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000A126 File Offset: 0x00008326
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int min(int x, int y)
		{
			if (x >= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000A12F File Offset: 0x0000832F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 min(int2 x, int2 y)
		{
			return new int2(math.min(x.x, y.x), math.min(x.y, y.y));
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000A158 File Offset: 0x00008358
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 min(int3 x, int3 y)
		{
			return new int3(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z));
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000A194 File Offset: 0x00008394
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 min(int4 x, int4 y)
		{
			return new int4(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z), math.min(x.w, y.w));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000A1EA File Offset: 0x000083EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint min(uint x, uint y)
		{
			if (x >= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000A1F3 File Offset: 0x000083F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 min(uint2 x, uint2 y)
		{
			return new uint2(math.min(x.x, y.x), math.min(x.y, y.y));
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000A21C File Offset: 0x0000841C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 min(uint3 x, uint3 y)
		{
			return new uint3(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z));
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000A258 File Offset: 0x00008458
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 min(uint4 x, uint4 y)
		{
			return new uint4(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z), math.min(x.w, y.w));
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000A2AE File Offset: 0x000084AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long min(long x, long y)
		{
			if (x >= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000A2B7 File Offset: 0x000084B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong min(ulong x, ulong y)
		{
			if (x >= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000A2C0 File Offset: 0x000084C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float min(float x, float y)
		{
			if (!float.IsNaN(y) && x >= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000A2D1 File Offset: 0x000084D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 min(float2 x, float2 y)
		{
			return new float2(math.min(x.x, y.x), math.min(x.y, y.y));
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000A2FA File Offset: 0x000084FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 min(float3 x, float3 y)
		{
			return new float3(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z));
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000A334 File Offset: 0x00008534
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 min(float4 x, float4 y)
		{
			return new float4(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z), math.min(x.w, y.w));
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000A38A File Offset: 0x0000858A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double min(double x, double y)
		{
			if (!double.IsNaN(y) && x >= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000A39B File Offset: 0x0000859B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 min(double2 x, double2 y)
		{
			return new double2(math.min(x.x, y.x), math.min(x.y, y.y));
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000A3C4 File Offset: 0x000085C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 min(double3 x, double3 y)
		{
			return new double3(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z));
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000A400 File Offset: 0x00008600
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 min(double4 x, double4 y)
		{
			return new double4(math.min(x.x, y.x), math.min(x.y, y.y), math.min(x.z, y.z), math.min(x.w, y.w));
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000A456 File Offset: 0x00008656
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int max(int x, int y)
		{
			if (x <= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000A45F File Offset: 0x0000865F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 max(int2 x, int2 y)
		{
			return new int2(math.max(x.x, y.x), math.max(x.y, y.y));
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000A488 File Offset: 0x00008688
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 max(int3 x, int3 y)
		{
			return new int3(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z));
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000A4C4 File Offset: 0x000086C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 max(int4 x, int4 y)
		{
			return new int4(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z), math.max(x.w, y.w));
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000A51A File Offset: 0x0000871A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint max(uint x, uint y)
		{
			if (x <= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000A523 File Offset: 0x00008723
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 max(uint2 x, uint2 y)
		{
			return new uint2(math.max(x.x, y.x), math.max(x.y, y.y));
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000A54C File Offset: 0x0000874C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 max(uint3 x, uint3 y)
		{
			return new uint3(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z));
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000A588 File Offset: 0x00008788
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 max(uint4 x, uint4 y)
		{
			return new uint4(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z), math.max(x.w, y.w));
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000A5DE File Offset: 0x000087DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long max(long x, long y)
		{
			if (x <= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000A5E7 File Offset: 0x000087E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong max(ulong x, ulong y)
		{
			if (x <= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000A5F0 File Offset: 0x000087F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float max(float x, float y)
		{
			if (!float.IsNaN(y) && x <= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000A601 File Offset: 0x00008801
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 max(float2 x, float2 y)
		{
			return new float2(math.max(x.x, y.x), math.max(x.y, y.y));
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000A62A File Offset: 0x0000882A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 max(float3 x, float3 y)
		{
			return new float3(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z));
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000A664 File Offset: 0x00008864
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 max(float4 x, float4 y)
		{
			return new float4(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z), math.max(x.w, y.w));
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000A6BA File Offset: 0x000088BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double max(double x, double y)
		{
			if (!double.IsNaN(y) && x <= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000A6CB File Offset: 0x000088CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 max(double2 x, double2 y)
		{
			return new double2(math.max(x.x, y.x), math.max(x.y, y.y));
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000A6F4 File Offset: 0x000088F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 max(double3 x, double3 y)
		{
			return new double3(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z));
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000A730 File Offset: 0x00008930
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 max(double4 x, double4 y)
		{
			return new double4(math.max(x.x, y.x), math.max(x.y, y.y), math.max(x.z, y.z), math.max(x.w, y.w));
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000A786 File Offset: 0x00008986
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float lerp(float start, float end, float t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000A78F File Offset: 0x0000898F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 lerp(float2 start, float2 end, float t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000A7A4 File Offset: 0x000089A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 lerp(float3 start, float3 end, float t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000A7B9 File Offset: 0x000089B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 lerp(float4 start, float4 end, float t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000A7CE File Offset: 0x000089CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 lerp(float2 start, float2 end, float2 t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000A7E3 File Offset: 0x000089E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 lerp(float3 start, float3 end, float3 t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000A7F8 File Offset: 0x000089F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 lerp(float4 start, float4 end, float4 t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000A80D File Offset: 0x00008A0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double lerp(double start, double end, double t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000A816 File Offset: 0x00008A16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 lerp(double2 start, double2 end, double t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000A82B File Offset: 0x00008A2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 lerp(double3 start, double3 end, double t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000A840 File Offset: 0x00008A40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 lerp(double4 start, double4 end, double t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000A855 File Offset: 0x00008A55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 lerp(double2 start, double2 end, double2 t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000A86A File Offset: 0x00008A6A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 lerp(double3 start, double3 end, double3 t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000A87F File Offset: 0x00008A7F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 lerp(double4 start, double4 end, double4 t)
		{
			return start + t * (end - start);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000A894 File Offset: 0x00008A94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float unlerp(float start, float end, float x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000A89D File Offset: 0x00008A9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 unlerp(float2 start, float2 end, float2 x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000A8B2 File Offset: 0x00008AB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 unlerp(float3 start, float3 end, float3 x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000A8C7 File Offset: 0x00008AC7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 unlerp(float4 start, float4 end, float4 x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000A8DC File Offset: 0x00008ADC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double unlerp(double start, double end, double x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000A8E5 File Offset: 0x00008AE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 unlerp(double2 start, double2 end, double2 x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000A8FA File Offset: 0x00008AFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 unlerp(double3 start, double3 end, double3 x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000A90F File Offset: 0x00008B0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 unlerp(double4 start, double4 end, double4 x)
		{
			return (x - start) / (end - start);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000A924 File Offset: 0x00008B24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float remap(float srcStart, float srcEnd, float dstStart, float dstEnd, float x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000A936 File Offset: 0x00008B36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 remap(float2 srcStart, float2 srcEnd, float2 dstStart, float2 dstEnd, float2 x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000A948 File Offset: 0x00008B48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 remap(float3 srcStart, float3 srcEnd, float3 dstStart, float3 dstEnd, float3 x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000A95A File Offset: 0x00008B5A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 remap(float4 srcStart, float4 srcEnd, float4 dstStart, float4 dstEnd, float4 x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000A96C File Offset: 0x00008B6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double remap(double srcStart, double srcEnd, double dstStart, double dstEnd, double x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000A97E File Offset: 0x00008B7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 remap(double2 srcStart, double2 srcEnd, double2 dstStart, double2 dstEnd, double2 x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000A990 File Offset: 0x00008B90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 remap(double3 srcStart, double3 srcEnd, double3 dstStart, double3 dstEnd, double3 x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000A9A2 File Offset: 0x00008BA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 remap(double4 srcStart, double4 srcEnd, double4 dstStart, double4 dstEnd, double4 x)
		{
			return math.lerp(dstStart, dstEnd, math.unlerp(srcStart, srcEnd, x));
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int mad(int mulA, int mulB, int addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000A9BB File Offset: 0x00008BBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 mad(int2 mulA, int2 mulB, int2 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000A9CA File Offset: 0x00008BCA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 mad(int3 mulA, int3 mulB, int3 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000A9D9 File Offset: 0x00008BD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 mad(int4 mulA, int4 mulB, int4 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000A9E8 File Offset: 0x00008BE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint mad(uint mulA, uint mulB, uint addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000A9EF File Offset: 0x00008BEF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 mad(uint2 mulA, uint2 mulB, uint2 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000A9FE File Offset: 0x00008BFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 mad(uint3 mulA, uint3 mulB, uint3 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000AA0D File Offset: 0x00008C0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 mad(uint4 mulA, uint4 mulB, uint4 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000AA1C File Offset: 0x00008C1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long mad(long mulA, long mulB, long addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000AA23 File Offset: 0x00008C23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong mad(ulong mulA, ulong mulB, ulong addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000AA2A File Offset: 0x00008C2A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float mad(float mulA, float mulB, float addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000AA31 File Offset: 0x00008C31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 mad(float2 mulA, float2 mulB, float2 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000AA40 File Offset: 0x00008C40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mad(float3 mulA, float3 mulB, float3 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000AA4F File Offset: 0x00008C4F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mad(float4 mulA, float4 mulB, float4 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000AA5E File Offset: 0x00008C5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double mad(double mulA, double mulB, double addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000AA65 File Offset: 0x00008C65
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 mad(double2 mulA, double2 mulB, double2 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000AA74 File Offset: 0x00008C74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mad(double3 mulA, double3 mulB, double3 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000AA83 File Offset: 0x00008C83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 mad(double4 mulA, double4 mulB, double4 addC)
		{
			return mulA * mulB + addC;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000AA92 File Offset: 0x00008C92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int clamp(int valueToClamp, int lowerBound, int upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000AAA1 File Offset: 0x00008CA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 clamp(int2 valueToClamp, int2 lowerBound, int2 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 clamp(int3 valueToClamp, int3 lowerBound, int3 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000AABF File Offset: 0x00008CBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 clamp(int4 valueToClamp, int4 lowerBound, int4 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000AACE File Offset: 0x00008CCE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint clamp(uint valueToClamp, uint lowerBound, uint upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000AADD File Offset: 0x00008CDD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 clamp(uint2 valueToClamp, uint2 lowerBound, uint2 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000AAEC File Offset: 0x00008CEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 clamp(uint3 valueToClamp, uint3 lowerBound, uint3 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000AAFB File Offset: 0x00008CFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 clamp(uint4 valueToClamp, uint4 lowerBound, uint4 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000AB0A File Offset: 0x00008D0A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long clamp(long valueToClamp, long lowerBound, long upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000AB19 File Offset: 0x00008D19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong clamp(ulong valueToClamp, ulong lowerBound, ulong upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000AB28 File Offset: 0x00008D28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float clamp(float valueToClamp, float lowerBound, float upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000AB37 File Offset: 0x00008D37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 clamp(float2 valueToClamp, float2 lowerBound, float2 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000AB46 File Offset: 0x00008D46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 clamp(float3 valueToClamp, float3 lowerBound, float3 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000AB55 File Offset: 0x00008D55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 clamp(float4 valueToClamp, float4 lowerBound, float4 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000AB64 File Offset: 0x00008D64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double clamp(double valueToClamp, double lowerBound, double upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000AB73 File Offset: 0x00008D73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 clamp(double2 valueToClamp, double2 lowerBound, double2 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000AB82 File Offset: 0x00008D82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 clamp(double3 valueToClamp, double3 lowerBound, double3 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000AB91 File Offset: 0x00008D91
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 clamp(double4 valueToClamp, double4 lowerBound, double4 upperBound)
		{
			return math.max(lowerBound, math.min(upperBound, valueToClamp));
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float saturate(float x)
		{
			return math.clamp(x, 0f, 1f);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000ABB2 File Offset: 0x00008DB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 saturate(float2 x)
		{
			return math.clamp(x, new float2(0f), new float2(1f));
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000ABCE File Offset: 0x00008DCE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 saturate(float3 x)
		{
			return math.clamp(x, new float3(0f), new float3(1f));
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000ABEA File Offset: 0x00008DEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 saturate(float4 x)
		{
			return math.clamp(x, new float4(0f), new float4(1f));
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000AC06 File Offset: 0x00008E06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double saturate(double x)
		{
			return math.clamp(x, 0.0, 1.0);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000AC20 File Offset: 0x00008E20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 saturate(double2 x)
		{
			return math.clamp(x, new double2(0.0), new double2(1.0));
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000AC44 File Offset: 0x00008E44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 saturate(double3 x)
		{
			return math.clamp(x, new double3(0.0), new double3(1.0));
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000AC68 File Offset: 0x00008E68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 saturate(double4 x)
		{
			return math.clamp(x, new double4(0.0), new double4(1.0));
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000AC8C File Offset: 0x00008E8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int abs(int x)
		{
			return math.max(-x, x);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000AC96 File Offset: 0x00008E96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 abs(int2 x)
		{
			return math.max(-x, x);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 abs(int3 x)
		{
			return math.max(-x, x);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000ACB2 File Offset: 0x00008EB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 abs(int4 x)
		{
			return math.max(-x, x);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000ACC0 File Offset: 0x00008EC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long abs(long x)
		{
			return math.max(-x, x);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000ACCA File Offset: 0x00008ECA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float abs(float x)
		{
			return math.asfloat(math.asuint(x) & 2147483647U);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000ACDD File Offset: 0x00008EDD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 abs(float2 x)
		{
			return math.asfloat(math.asuint(x) & 2147483647U);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 abs(float3 x)
		{
			return math.asfloat(math.asuint(x) & 2147483647U);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000AD0B File Offset: 0x00008F0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 abs(float4 x)
		{
			return math.asfloat(math.asuint(x) & 2147483647U);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000AD22 File Offset: 0x00008F22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double abs(double x)
		{
			return math.asdouble(math.asulong(x) & 9223372036854775807UL);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000AD39 File Offset: 0x00008F39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 abs(double2 x)
		{
			return math.double2(math.asdouble(math.asulong(x.x) & 9223372036854775807UL), math.asdouble(math.asulong(x.y) & 9223372036854775807UL));
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000AD74 File Offset: 0x00008F74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 abs(double3 x)
		{
			return math.double3(math.asdouble(math.asulong(x.x) & 9223372036854775807UL), math.asdouble(math.asulong(x.y) & 9223372036854775807UL), math.asdouble(math.asulong(x.z) & 9223372036854775807UL));
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000ADD4 File Offset: 0x00008FD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 abs(double4 x)
		{
			return math.double4(math.asdouble(math.asulong(x.x) & 9223372036854775807UL), math.asdouble(math.asulong(x.y) & 9223372036854775807UL), math.asdouble(math.asulong(x.z) & 9223372036854775807UL), math.asdouble(math.asulong(x.w) & 9223372036854775807UL));
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000AE4E File Offset: 0x0000904E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int dot(int x, int y)
		{
			return x * y;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000AE53 File Offset: 0x00009053
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int dot(int2 x, int2 y)
		{
			return x.x * y.x + x.y * y.y;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000AE70 File Offset: 0x00009070
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int dot(int3 x, int3 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000AE9B File Offset: 0x0000909B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int dot(int4 x, int4 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000AED4 File Offset: 0x000090D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint dot(uint x, uint y)
		{
			return x * y;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000AED9 File Offset: 0x000090D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint dot(uint2 x, uint2 y)
		{
			return x.x * y.x + x.y * y.y;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000AEF6 File Offset: 0x000090F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint dot(uint3 x, uint3 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000AF21 File Offset: 0x00009121
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint dot(uint4 x, uint4 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000AF5A File Offset: 0x0000915A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float dot(float x, float y)
		{
			return x * y;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000AF5F File Offset: 0x0000915F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float dot(float2 x, float2 y)
		{
			return x.x * y.x + x.y * y.y;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000AF7C File Offset: 0x0000917C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float dot(float3 x, float3 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000AFA7 File Offset: 0x000091A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float dot(float4 x, float4 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000AFE0 File Offset: 0x000091E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double dot(double x, double y)
		{
			return x * y;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000AFE5 File Offset: 0x000091E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double dot(double2 x, double2 y)
		{
			return x.x * y.x + x.y * y.y;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000B002 File Offset: 0x00009202
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double dot(double3 x, double3 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000B02D File Offset: 0x0000922D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double dot(double4 x, double4 y)
		{
			return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000B066 File Offset: 0x00009266
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float tan(float x)
		{
			return (float)Math.Tan((double)x);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000B070 File Offset: 0x00009270
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 tan(float2 x)
		{
			return new float2(math.tan(x.x), math.tan(x.y));
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000B08D File Offset: 0x0000928D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 tan(float3 x)
		{
			return new float3(math.tan(x.x), math.tan(x.y), math.tan(x.z));
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000B0B5 File Offset: 0x000092B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 tan(float4 x)
		{
			return new float4(math.tan(x.x), math.tan(x.y), math.tan(x.z), math.tan(x.w));
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000B0E8 File Offset: 0x000092E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double tan(double x)
		{
			return Math.Tan(x);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000B0F0 File Offset: 0x000092F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 tan(double2 x)
		{
			return new double2(math.tan(x.x), math.tan(x.y));
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000B10D File Offset: 0x0000930D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 tan(double3 x)
		{
			return new double3(math.tan(x.x), math.tan(x.y), math.tan(x.z));
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000B135 File Offset: 0x00009335
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 tan(double4 x)
		{
			return new double4(math.tan(x.x), math.tan(x.y), math.tan(x.z), math.tan(x.w));
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000B168 File Offset: 0x00009368
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float tanh(float x)
		{
			return (float)Math.Tanh((double)x);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000B172 File Offset: 0x00009372
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 tanh(float2 x)
		{
			return new float2(math.tanh(x.x), math.tanh(x.y));
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000B18F File Offset: 0x0000938F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 tanh(float3 x)
		{
			return new float3(math.tanh(x.x), math.tanh(x.y), math.tanh(x.z));
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000B1B7 File Offset: 0x000093B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 tanh(float4 x)
		{
			return new float4(math.tanh(x.x), math.tanh(x.y), math.tanh(x.z), math.tanh(x.w));
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000B1EA File Offset: 0x000093EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double tanh(double x)
		{
			return Math.Tanh(x);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000B1F2 File Offset: 0x000093F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 tanh(double2 x)
		{
			return new double2(math.tanh(x.x), math.tanh(x.y));
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000B20F File Offset: 0x0000940F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 tanh(double3 x)
		{
			return new double3(math.tanh(x.x), math.tanh(x.y), math.tanh(x.z));
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000B237 File Offset: 0x00009437
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 tanh(double4 x)
		{
			return new double4(math.tanh(x.x), math.tanh(x.y), math.tanh(x.z), math.tanh(x.w));
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000B26A File Offset: 0x0000946A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float atan(float x)
		{
			return (float)Math.Atan((double)x);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000B274 File Offset: 0x00009474
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 atan(float2 x)
		{
			return new float2(math.atan(x.x), math.atan(x.y));
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000B291 File Offset: 0x00009491
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 atan(float3 x)
		{
			return new float3(math.atan(x.x), math.atan(x.y), math.atan(x.z));
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000B2B9 File Offset: 0x000094B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 atan(float4 x)
		{
			return new float4(math.atan(x.x), math.atan(x.y), math.atan(x.z), math.atan(x.w));
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000B2EC File Offset: 0x000094EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double atan(double x)
		{
			return Math.Atan(x);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000B2F4 File Offset: 0x000094F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 atan(double2 x)
		{
			return new double2(math.atan(x.x), math.atan(x.y));
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000B311 File Offset: 0x00009511
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 atan(double3 x)
		{
			return new double3(math.atan(x.x), math.atan(x.y), math.atan(x.z));
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000B339 File Offset: 0x00009539
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 atan(double4 x)
		{
			return new double4(math.atan(x.x), math.atan(x.y), math.atan(x.z), math.atan(x.w));
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000B36C File Offset: 0x0000956C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float atan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000B378 File Offset: 0x00009578
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 atan2(float2 y, float2 x)
		{
			return new float2(math.atan2(y.x, x.x), math.atan2(y.y, x.y));
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000B3A1 File Offset: 0x000095A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 atan2(float3 y, float3 x)
		{
			return new float3(math.atan2(y.x, x.x), math.atan2(y.y, x.y), math.atan2(y.z, x.z));
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000B3DC File Offset: 0x000095DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 atan2(float4 y, float4 x)
		{
			return new float4(math.atan2(y.x, x.x), math.atan2(y.y, x.y), math.atan2(y.z, x.z), math.atan2(y.w, x.w));
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000B432 File Offset: 0x00009632
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double atan2(double y, double x)
		{
			return Math.Atan2(y, x);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000B43B File Offset: 0x0000963B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 atan2(double2 y, double2 x)
		{
			return new double2(math.atan2(y.x, x.x), math.atan2(y.y, x.y));
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000B464 File Offset: 0x00009664
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 atan2(double3 y, double3 x)
		{
			return new double3(math.atan2(y.x, x.x), math.atan2(y.y, x.y), math.atan2(y.z, x.z));
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000B4A0 File Offset: 0x000096A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 atan2(double4 y, double4 x)
		{
			return new double4(math.atan2(y.x, x.x), math.atan2(y.y, x.y), math.atan2(y.z, x.z), math.atan2(y.w, x.w));
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000B4F6 File Offset: 0x000096F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000B500 File Offset: 0x00009700
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 cos(float2 x)
		{
			return new float2(math.cos(x.x), math.cos(x.y));
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000B51D File Offset: 0x0000971D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 cos(float3 x)
		{
			return new float3(math.cos(x.x), math.cos(x.y), math.cos(x.z));
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000B545 File Offset: 0x00009745
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 cos(float4 x)
		{
			return new float4(math.cos(x.x), math.cos(x.y), math.cos(x.z), math.cos(x.w));
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000B578 File Offset: 0x00009778
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cos(double x)
		{
			return Math.Cos(x);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000B580 File Offset: 0x00009780
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 cos(double2 x)
		{
			return new double2(math.cos(x.x), math.cos(x.y));
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000B59D File Offset: 0x0000979D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 cos(double3 x)
		{
			return new double3(math.cos(x.x), math.cos(x.y), math.cos(x.z));
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000B5C5 File Offset: 0x000097C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 cos(double4 x)
		{
			return new double4(math.cos(x.x), math.cos(x.y), math.cos(x.z), math.cos(x.w));
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000B5F8 File Offset: 0x000097F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cosh(float x)
		{
			return (float)Math.Cosh((double)x);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000B602 File Offset: 0x00009802
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 cosh(float2 x)
		{
			return new float2(math.cosh(x.x), math.cosh(x.y));
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000B61F File Offset: 0x0000981F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 cosh(float3 x)
		{
			return new float3(math.cosh(x.x), math.cosh(x.y), math.cosh(x.z));
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000B647 File Offset: 0x00009847
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 cosh(float4 x)
		{
			return new float4(math.cosh(x.x), math.cosh(x.y), math.cosh(x.z), math.cosh(x.w));
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000B67A File Offset: 0x0000987A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cosh(double x)
		{
			return Math.Cosh(x);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000B682 File Offset: 0x00009882
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 cosh(double2 x)
		{
			return new double2(math.cosh(x.x), math.cosh(x.y));
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000B69F File Offset: 0x0000989F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 cosh(double3 x)
		{
			return new double3(math.cosh(x.x), math.cosh(x.y), math.cosh(x.z));
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000B6C7 File Offset: 0x000098C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 cosh(double4 x)
		{
			return new double4(math.cosh(x.x), math.cosh(x.y), math.cosh(x.z), math.cosh(x.w));
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000B6FA File Offset: 0x000098FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float acos(float x)
		{
			return (float)Math.Acos((double)x);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000B705 File Offset: 0x00009905
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 acos(float2 x)
		{
			return new float2(math.acos(x.x), math.acos(x.y));
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000B722 File Offset: 0x00009922
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 acos(float3 x)
		{
			return new float3(math.acos(x.x), math.acos(x.y), math.acos(x.z));
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000B74A File Offset: 0x0000994A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 acos(float4 x)
		{
			return new float4(math.acos(x.x), math.acos(x.y), math.acos(x.z), math.acos(x.w));
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000B77D File Offset: 0x0000997D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double acos(double x)
		{
			return Math.Acos(x);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000B785 File Offset: 0x00009985
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 acos(double2 x)
		{
			return new double2(math.acos(x.x), math.acos(x.y));
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000B7A2 File Offset: 0x000099A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 acos(double3 x)
		{
			return new double3(math.acos(x.x), math.acos(x.y), math.acos(x.z));
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000B7CA File Offset: 0x000099CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 acos(double4 x)
		{
			return new double4(math.acos(x.x), math.acos(x.y), math.acos(x.z), math.acos(x.w));
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000B7FD File Offset: 0x000099FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000B808 File Offset: 0x00009A08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 sin(float2 x)
		{
			return new float2(math.sin(x.x), math.sin(x.y));
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000B825 File Offset: 0x00009A25
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 sin(float3 x)
		{
			return new float3(math.sin(x.x), math.sin(x.y), math.sin(x.z));
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000B84D File Offset: 0x00009A4D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 sin(float4 x)
		{
			return new float4(math.sin(x.x), math.sin(x.y), math.sin(x.z), math.sin(x.w));
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000B880 File Offset: 0x00009A80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double sin(double x)
		{
			return Math.Sin(x);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000B888 File Offset: 0x00009A88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 sin(double2 x)
		{
			return new double2(math.sin(x.x), math.sin(x.y));
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000B8A5 File Offset: 0x00009AA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 sin(double3 x)
		{
			return new double3(math.sin(x.x), math.sin(x.y), math.sin(x.z));
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000B8CD File Offset: 0x00009ACD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 sin(double4 x)
		{
			return new double4(math.sin(x.x), math.sin(x.y), math.sin(x.z), math.sin(x.w));
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000B900 File Offset: 0x00009B00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float sinh(float x)
		{
			return (float)Math.Sinh((double)x);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000B90B File Offset: 0x00009B0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 sinh(float2 x)
		{
			return new float2(math.sinh(x.x), math.sinh(x.y));
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000B928 File Offset: 0x00009B28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 sinh(float3 x)
		{
			return new float3(math.sinh(x.x), math.sinh(x.y), math.sinh(x.z));
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000B950 File Offset: 0x00009B50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 sinh(float4 x)
		{
			return new float4(math.sinh(x.x), math.sinh(x.y), math.sinh(x.z), math.sinh(x.w));
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000B983 File Offset: 0x00009B83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double sinh(double x)
		{
			return Math.Sinh(x);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000B98B File Offset: 0x00009B8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 sinh(double2 x)
		{
			return new double2(math.sinh(x.x), math.sinh(x.y));
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 sinh(double3 x)
		{
			return new double3(math.sinh(x.x), math.sinh(x.y), math.sinh(x.z));
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000B9D0 File Offset: 0x00009BD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 sinh(double4 x)
		{
			return new double4(math.sinh(x.x), math.sinh(x.y), math.sinh(x.z), math.sinh(x.w));
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000BA03 File Offset: 0x00009C03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float asin(float x)
		{
			return (float)Math.Asin((double)x);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000BA0E File Offset: 0x00009C0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 asin(float2 x)
		{
			return new float2(math.asin(x.x), math.asin(x.y));
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000BA2B File Offset: 0x00009C2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 asin(float3 x)
		{
			return new float3(math.asin(x.x), math.asin(x.y), math.asin(x.z));
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000BA53 File Offset: 0x00009C53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 asin(float4 x)
		{
			return new float4(math.asin(x.x), math.asin(x.y), math.asin(x.z), math.asin(x.w));
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000BA86 File Offset: 0x00009C86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double asin(double x)
		{
			return Math.Asin(x);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000BA8E File Offset: 0x00009C8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 asin(double2 x)
		{
			return new double2(math.asin(x.x), math.asin(x.y));
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000BAAB File Offset: 0x00009CAB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 asin(double3 x)
		{
			return new double3(math.asin(x.x), math.asin(x.y), math.asin(x.z));
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000BAD3 File Offset: 0x00009CD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 asin(double4 x)
		{
			return new double4(math.asin(x.x), math.asin(x.y), math.asin(x.z), math.asin(x.w));
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000BB06 File Offset: 0x00009D06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float floor(float x)
		{
			return (float)Math.Floor((double)x);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000BB11 File Offset: 0x00009D11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 floor(float2 x)
		{
			return new float2(math.floor(x.x), math.floor(x.y));
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000BB2E File Offset: 0x00009D2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 floor(float3 x)
		{
			return new float3(math.floor(x.x), math.floor(x.y), math.floor(x.z));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000BB56 File Offset: 0x00009D56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 floor(float4 x)
		{
			return new float4(math.floor(x.x), math.floor(x.y), math.floor(x.z), math.floor(x.w));
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000BB89 File Offset: 0x00009D89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double floor(double x)
		{
			return Math.Floor(x);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000BB91 File Offset: 0x00009D91
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 floor(double2 x)
		{
			return new double2(math.floor(x.x), math.floor(x.y));
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000BBAE File Offset: 0x00009DAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 floor(double3 x)
		{
			return new double3(math.floor(x.x), math.floor(x.y), math.floor(x.z));
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000BBD6 File Offset: 0x00009DD6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 floor(double4 x)
		{
			return new double4(math.floor(x.x), math.floor(x.y), math.floor(x.z), math.floor(x.w));
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000BC09 File Offset: 0x00009E09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ceil(float x)
		{
			return (float)Math.Ceiling((double)x);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000BC14 File Offset: 0x00009E14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 ceil(float2 x)
		{
			return new float2(math.ceil(x.x), math.ceil(x.y));
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000BC31 File Offset: 0x00009E31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ceil(float3 x)
		{
			return new float3(math.ceil(x.x), math.ceil(x.y), math.ceil(x.z));
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000BC59 File Offset: 0x00009E59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 ceil(float4 x)
		{
			return new float4(math.ceil(x.x), math.ceil(x.y), math.ceil(x.z), math.ceil(x.w));
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000BC8C File Offset: 0x00009E8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double ceil(double x)
		{
			return Math.Ceiling(x);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000BC94 File Offset: 0x00009E94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 ceil(double2 x)
		{
			return new double2(math.ceil(x.x), math.ceil(x.y));
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000BCB1 File Offset: 0x00009EB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 ceil(double3 x)
		{
			return new double3(math.ceil(x.x), math.ceil(x.y), math.ceil(x.z));
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000BCD9 File Offset: 0x00009ED9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 ceil(double4 x)
		{
			return new double4(math.ceil(x.x), math.ceil(x.y), math.ceil(x.z), math.ceil(x.w));
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000BD0C File Offset: 0x00009F0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float round(float x)
		{
			return (float)Math.Round((double)x);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000BD17 File Offset: 0x00009F17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 round(float2 x)
		{
			return new float2(math.round(x.x), math.round(x.y));
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000BD34 File Offset: 0x00009F34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 round(float3 x)
		{
			return new float3(math.round(x.x), math.round(x.y), math.round(x.z));
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000BD5C File Offset: 0x00009F5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 round(float4 x)
		{
			return new float4(math.round(x.x), math.round(x.y), math.round(x.z), math.round(x.w));
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000BD8F File Offset: 0x00009F8F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double round(double x)
		{
			return Math.Round(x);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000BD97 File Offset: 0x00009F97
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 round(double2 x)
		{
			return new double2(math.round(x.x), math.round(x.y));
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000BDB4 File Offset: 0x00009FB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 round(double3 x)
		{
			return new double3(math.round(x.x), math.round(x.y), math.round(x.z));
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000BDDC File Offset: 0x00009FDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 round(double4 x)
		{
			return new double4(math.round(x.x), math.round(x.y), math.round(x.z), math.round(x.w));
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000BE0F File Offset: 0x0000A00F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float trunc(float x)
		{
			return (float)Math.Truncate((double)x);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000BE1A File Offset: 0x0000A01A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 trunc(float2 x)
		{
			return new float2(math.trunc(x.x), math.trunc(x.y));
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000BE37 File Offset: 0x0000A037
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 trunc(float3 x)
		{
			return new float3(math.trunc(x.x), math.trunc(x.y), math.trunc(x.z));
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000BE5F File Offset: 0x0000A05F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 trunc(float4 x)
		{
			return new float4(math.trunc(x.x), math.trunc(x.y), math.trunc(x.z), math.trunc(x.w));
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000BE92 File Offset: 0x0000A092
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double trunc(double x)
		{
			return Math.Truncate(x);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000BE9A File Offset: 0x0000A09A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 trunc(double2 x)
		{
			return new double2(math.trunc(x.x), math.trunc(x.y));
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000BEB7 File Offset: 0x0000A0B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 trunc(double3 x)
		{
			return new double3(math.trunc(x.x), math.trunc(x.y), math.trunc(x.z));
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000BEDF File Offset: 0x0000A0DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 trunc(double4 x)
		{
			return new double4(math.trunc(x.x), math.trunc(x.y), math.trunc(x.z), math.trunc(x.w));
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000BF12 File Offset: 0x0000A112
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float frac(float x)
		{
			return x - math.floor(x);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000BF1C File Offset: 0x0000A11C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 frac(float2 x)
		{
			return x - math.floor(x);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000BF2A File Offset: 0x0000A12A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 frac(float3 x)
		{
			return x - math.floor(x);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000BF38 File Offset: 0x0000A138
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 frac(float4 x)
		{
			return x - math.floor(x);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000BF46 File Offset: 0x0000A146
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double frac(double x)
		{
			return x - math.floor(x);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000BF50 File Offset: 0x0000A150
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 frac(double2 x)
		{
			return x - math.floor(x);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000BF5E File Offset: 0x0000A15E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 frac(double3 x)
		{
			return x - math.floor(x);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000BF6C File Offset: 0x0000A16C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 frac(double4 x)
		{
			return x - math.floor(x);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000BF7A File Offset: 0x0000A17A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float rcp(float x)
		{
			return 1f / x;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000BF83 File Offset: 0x0000A183
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 rcp(float2 x)
		{
			return 1f / x;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000BF90 File Offset: 0x0000A190
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 rcp(float3 x)
		{
			return 1f / x;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000BF9D File Offset: 0x0000A19D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 rcp(float4 x)
		{
			return 1f / x;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000BFAA File Offset: 0x0000A1AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double rcp(double x)
		{
			return 1.0 / x;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000BFB7 File Offset: 0x0000A1B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 rcp(double2 x)
		{
			return 1.0 / x;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000BFC8 File Offset: 0x0000A1C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 rcp(double3 x)
		{
			return 1.0 / x;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000BFD9 File Offset: 0x0000A1D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 rcp(double4 x)
		{
			return 1.0 / x;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000BFEA File Offset: 0x0000A1EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int sign(int x)
		{
			return ((x > 0) ? 1 : 0) - ((x < 0) ? 1 : 0);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000BFFD File Offset: 0x0000A1FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 sign(int2 x)
		{
			return new int2(math.sign(x.x), math.sign(x.y));
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000C01A File Offset: 0x0000A21A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 sign(int3 x)
		{
			return new int3(math.sign(x.x), math.sign(x.y), math.sign(x.z));
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000C042 File Offset: 0x0000A242
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 sign(int4 x)
		{
			return new int4(math.sign(x.x), math.sign(x.y), math.sign(x.z), math.sign(x.w));
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000C075 File Offset: 0x0000A275
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float sign(float x)
		{
			return ((x > 0f) ? 1f : 0f) - ((x < 0f) ? 1f : 0f);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000C0A0 File Offset: 0x0000A2A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 sign(float2 x)
		{
			return new float2(math.sign(x.x), math.sign(x.y));
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000C0BD File Offset: 0x0000A2BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 sign(float3 x)
		{
			return new float3(math.sign(x.x), math.sign(x.y), math.sign(x.z));
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000C0E5 File Offset: 0x0000A2E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 sign(float4 x)
		{
			return new float4(math.sign(x.x), math.sign(x.y), math.sign(x.z), math.sign(x.w));
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000C118 File Offset: 0x0000A318
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double sign(double x)
		{
			if (x != 0.0)
			{
				return ((x > 0.0) ? 1.0 : 0.0) - ((x < 0.0) ? 1.0 : 0.0);
			}
			return 0.0;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000C17C File Offset: 0x0000A37C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 sign(double2 x)
		{
			return new double2(math.sign(x.x), math.sign(x.y));
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000C199 File Offset: 0x0000A399
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 sign(double3 x)
		{
			return new double3(math.sign(x.x), math.sign(x.y), math.sign(x.z));
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000C1C1 File Offset: 0x0000A3C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 sign(double4 x)
		{
			return new double4(math.sign(x.x), math.sign(x.y), math.sign(x.z), math.sign(x.w));
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000C1F4 File Offset: 0x0000A3F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float pow(float x, float y)
		{
			return (float)Math.Pow((double)x, (double)y);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000C202 File Offset: 0x0000A402
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 pow(float2 x, float2 y)
		{
			return new float2(math.pow(x.x, y.x), math.pow(x.y, y.y));
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000C22B File Offset: 0x0000A42B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 pow(float3 x, float3 y)
		{
			return new float3(math.pow(x.x, y.x), math.pow(x.y, y.y), math.pow(x.z, y.z));
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000C268 File Offset: 0x0000A468
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 pow(float4 x, float4 y)
		{
			return new float4(math.pow(x.x, y.x), math.pow(x.y, y.y), math.pow(x.z, y.z), math.pow(x.w, y.w));
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000C2BE File Offset: 0x0000A4BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double pow(double x, double y)
		{
			return Math.Pow(x, y);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000C2C7 File Offset: 0x0000A4C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 pow(double2 x, double2 y)
		{
			return new double2(math.pow(x.x, y.x), math.pow(x.y, y.y));
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000C2F0 File Offset: 0x0000A4F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 pow(double3 x, double3 y)
		{
			return new double3(math.pow(x.x, y.x), math.pow(x.y, y.y), math.pow(x.z, y.z));
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000C32C File Offset: 0x0000A52C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 pow(double4 x, double4 y)
		{
			return new double4(math.pow(x.x, y.x), math.pow(x.y, y.y), math.pow(x.z, y.z), math.pow(x.w, y.w));
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000C382 File Offset: 0x0000A582
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float exp(float x)
		{
			return (float)Math.Exp((double)x);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000C38D File Offset: 0x0000A58D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 exp(float2 x)
		{
			return new float2(math.exp(x.x), math.exp(x.y));
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000C3AA File Offset: 0x0000A5AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 exp(float3 x)
		{
			return new float3(math.exp(x.x), math.exp(x.y), math.exp(x.z));
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000C3D2 File Offset: 0x0000A5D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 exp(float4 x)
		{
			return new float4(math.exp(x.x), math.exp(x.y), math.exp(x.z), math.exp(x.w));
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000C405 File Offset: 0x0000A605
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double exp(double x)
		{
			return Math.Exp(x);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000C40D File Offset: 0x0000A60D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 exp(double2 x)
		{
			return new double2(math.exp(x.x), math.exp(x.y));
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000C42A File Offset: 0x0000A62A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 exp(double3 x)
		{
			return new double3(math.exp(x.x), math.exp(x.y), math.exp(x.z));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000C452 File Offset: 0x0000A652
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 exp(double4 x)
		{
			return new double4(math.exp(x.x), math.exp(x.y), math.exp(x.z), math.exp(x.w));
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000C485 File Offset: 0x0000A685
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float exp2(float x)
		{
			return (float)Math.Exp((double)(x * 0.6931472f));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000C496 File Offset: 0x0000A696
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 exp2(float2 x)
		{
			return new float2(math.exp2(x.x), math.exp2(x.y));
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000C4B3 File Offset: 0x0000A6B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 exp2(float3 x)
		{
			return new float3(math.exp2(x.x), math.exp2(x.y), math.exp2(x.z));
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000C4DB File Offset: 0x0000A6DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 exp2(float4 x)
		{
			return new float4(math.exp2(x.x), math.exp2(x.y), math.exp2(x.z), math.exp2(x.w));
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000C50E File Offset: 0x0000A70E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double exp2(double x)
		{
			return Math.Exp(x * 0.6931471805599453);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000C520 File Offset: 0x0000A720
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 exp2(double2 x)
		{
			return new double2(math.exp2(x.x), math.exp2(x.y));
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000C53D File Offset: 0x0000A73D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 exp2(double3 x)
		{
			return new double3(math.exp2(x.x), math.exp2(x.y), math.exp2(x.z));
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000C565 File Offset: 0x0000A765
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 exp2(double4 x)
		{
			return new double4(math.exp2(x.x), math.exp2(x.y), math.exp2(x.z), math.exp2(x.w));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000C598 File Offset: 0x0000A798
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float exp10(float x)
		{
			return (float)Math.Exp((double)(x * 2.3025851f));
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000C5A9 File Offset: 0x0000A7A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 exp10(float2 x)
		{
			return new float2(math.exp10(x.x), math.exp10(x.y));
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000C5C6 File Offset: 0x0000A7C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 exp10(float3 x)
		{
			return new float3(math.exp10(x.x), math.exp10(x.y), math.exp10(x.z));
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000C5EE File Offset: 0x0000A7EE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 exp10(float4 x)
		{
			return new float4(math.exp10(x.x), math.exp10(x.y), math.exp10(x.z), math.exp10(x.w));
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000C621 File Offset: 0x0000A821
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double exp10(double x)
		{
			return Math.Exp(x * 2.302585092994046);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000C633 File Offset: 0x0000A833
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 exp10(double2 x)
		{
			return new double2(math.exp10(x.x), math.exp10(x.y));
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000C650 File Offset: 0x0000A850
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 exp10(double3 x)
		{
			return new double3(math.exp10(x.x), math.exp10(x.y), math.exp10(x.z));
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000C678 File Offset: 0x0000A878
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 exp10(double4 x)
		{
			return new double4(math.exp10(x.x), math.exp10(x.y), math.exp10(x.z), math.exp10(x.w));
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000C6AB File Offset: 0x0000A8AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float log(float x)
		{
			return (float)Math.Log((double)x);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000C6B6 File Offset: 0x0000A8B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 log(float2 x)
		{
			return new float2(math.log(x.x), math.log(x.y));
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000C6D3 File Offset: 0x0000A8D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 log(float3 x)
		{
			return new float3(math.log(x.x), math.log(x.y), math.log(x.z));
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000C6FB File Offset: 0x0000A8FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 log(float4 x)
		{
			return new float4(math.log(x.x), math.log(x.y), math.log(x.z), math.log(x.w));
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000C72E File Offset: 0x0000A92E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double log(double x)
		{
			return Math.Log(x);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000C736 File Offset: 0x0000A936
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 log(double2 x)
		{
			return new double2(math.log(x.x), math.log(x.y));
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000C753 File Offset: 0x0000A953
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 log(double3 x)
		{
			return new double3(math.log(x.x), math.log(x.y), math.log(x.z));
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000C77B File Offset: 0x0000A97B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 log(double4 x)
		{
			return new double4(math.log(x.x), math.log(x.y), math.log(x.z), math.log(x.w));
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000C7AE File Offset: 0x0000A9AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float log2(float x)
		{
			return (float)Math.Log((double)x, 2.0);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000C7C2 File Offset: 0x0000A9C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 log2(float2 x)
		{
			return new float2(math.log2(x.x), math.log2(x.y));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000C7DF File Offset: 0x0000A9DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 log2(float3 x)
		{
			return new float3(math.log2(x.x), math.log2(x.y), math.log2(x.z));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000C807 File Offset: 0x0000AA07
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 log2(float4 x)
		{
			return new float4(math.log2(x.x), math.log2(x.y), math.log2(x.z), math.log2(x.w));
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000C83A File Offset: 0x0000AA3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double log2(double x)
		{
			return Math.Log(x, 2.0);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000C84B File Offset: 0x0000AA4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 log2(double2 x)
		{
			return new double2(math.log2(x.x), math.log2(x.y));
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000C868 File Offset: 0x0000AA68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 log2(double3 x)
		{
			return new double3(math.log2(x.x), math.log2(x.y), math.log2(x.z));
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000C890 File Offset: 0x0000AA90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 log2(double4 x)
		{
			return new double4(math.log2(x.x), math.log2(x.y), math.log2(x.z), math.log2(x.w));
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000C8C3 File Offset: 0x0000AAC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float log10(float x)
		{
			return (float)Math.Log10((double)x);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000C8CE File Offset: 0x0000AACE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 log10(float2 x)
		{
			return new float2(math.log10(x.x), math.log10(x.y));
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000C8EB File Offset: 0x0000AAEB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 log10(float3 x)
		{
			return new float3(math.log10(x.x), math.log10(x.y), math.log10(x.z));
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000C913 File Offset: 0x0000AB13
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 log10(float4 x)
		{
			return new float4(math.log10(x.x), math.log10(x.y), math.log10(x.z), math.log10(x.w));
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000C946 File Offset: 0x0000AB46
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double log10(double x)
		{
			return Math.Log10(x);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000C94E File Offset: 0x0000AB4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 log10(double2 x)
		{
			return new double2(math.log10(x.x), math.log10(x.y));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000C96B File Offset: 0x0000AB6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 log10(double3 x)
		{
			return new double3(math.log10(x.x), math.log10(x.y), math.log10(x.z));
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000C993 File Offset: 0x0000AB93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 log10(double4 x)
		{
			return new double4(math.log10(x.x), math.log10(x.y), math.log10(x.z), math.log10(x.w));
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000C9C6 File Offset: 0x0000ABC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float fmod(float x, float y)
		{
			return x % y;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000C9CB File Offset: 0x0000ABCB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 fmod(float2 x, float2 y)
		{
			return new float2(x.x % y.x, x.y % y.y);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000C9EC File Offset: 0x0000ABEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 fmod(float3 x, float3 y)
		{
			return new float3(x.x % y.x, x.y % y.y, x.z % y.z);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000CA1A File Offset: 0x0000AC1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 fmod(float4 x, float4 y)
		{
			return new float4(x.x % y.x, x.y % y.y, x.z % y.z, x.w % y.w);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000CA55 File Offset: 0x0000AC55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double fmod(double x, double y)
		{
			return x % y;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000CA5A File Offset: 0x0000AC5A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 fmod(double2 x, double2 y)
		{
			return new double2(x.x % y.x, x.y % y.y);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000CA7B File Offset: 0x0000AC7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 fmod(double3 x, double3 y)
		{
			return new double3(x.x % y.x, x.y % y.y, x.z % y.z);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000CAA9 File Offset: 0x0000ACA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 fmod(double4 x, double4 y)
		{
			return new double4(x.x % y.x, x.y % y.y, x.z % y.z, x.w % y.w);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float modf(float x, out float i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000CAF2 File Offset: 0x0000ACF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 modf(float2 x, out float2 i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000CB0C File Offset: 0x0000AD0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 modf(float3 x, out float3 i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000CB26 File Offset: 0x0000AD26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 modf(float4 x, out float4 i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000CB40 File Offset: 0x0000AD40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double modf(double x, out double i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000CB4E File Offset: 0x0000AD4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 modf(double2 x, out double2 i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000CB68 File Offset: 0x0000AD68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 modf(double3 x, out double3 i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000CB82 File Offset: 0x0000AD82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 modf(double4 x, out double4 i)
		{
			i = math.trunc(x);
			return x - i;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float sqrt(float x)
		{
			return (float)Math.Sqrt((double)x);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000CBA7 File Offset: 0x0000ADA7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 sqrt(float2 x)
		{
			return new float2(math.sqrt(x.x), math.sqrt(x.y));
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000CBC4 File Offset: 0x0000ADC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 sqrt(float3 x)
		{
			return new float3(math.sqrt(x.x), math.sqrt(x.y), math.sqrt(x.z));
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000CBEC File Offset: 0x0000ADEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 sqrt(float4 x)
		{
			return new float4(math.sqrt(x.x), math.sqrt(x.y), math.sqrt(x.z), math.sqrt(x.w));
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000CC1F File Offset: 0x0000AE1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double sqrt(double x)
		{
			return Math.Sqrt(x);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000CC27 File Offset: 0x0000AE27
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 sqrt(double2 x)
		{
			return new double2(math.sqrt(x.x), math.sqrt(x.y));
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000CC44 File Offset: 0x0000AE44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 sqrt(double3 x)
		{
			return new double3(math.sqrt(x.x), math.sqrt(x.y), math.sqrt(x.z));
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000CC6C File Offset: 0x0000AE6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 sqrt(double4 x)
		{
			return new double4(math.sqrt(x.x), math.sqrt(x.y), math.sqrt(x.z), math.sqrt(x.w));
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000CC9F File Offset: 0x0000AE9F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float rsqrt(float x)
		{
			return 1f / math.sqrt(x);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000CCAD File Offset: 0x0000AEAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 rsqrt(float2 x)
		{
			return 1f / math.sqrt(x);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000CCBF File Offset: 0x0000AEBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 rsqrt(float3 x)
		{
			return 1f / math.sqrt(x);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000CCD1 File Offset: 0x0000AED1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 rsqrt(float4 x)
		{
			return 1f / math.sqrt(x);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000CCE3 File Offset: 0x0000AEE3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double rsqrt(double x)
		{
			return 1.0 / math.sqrt(x);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000CCF5 File Offset: 0x0000AEF5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 rsqrt(double2 x)
		{
			return 1.0 / math.sqrt(x);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000CD0B File Offset: 0x0000AF0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 rsqrt(double3 x)
		{
			return 1.0 / math.sqrt(x);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000CD21 File Offset: 0x0000AF21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 rsqrt(double4 x)
		{
			return 1.0 / math.sqrt(x);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000CD37 File Offset: 0x0000AF37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 normalize(float2 x)
		{
			return math.rsqrt(math.dot(x, x)) * x;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000CD4B File Offset: 0x0000AF4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 normalize(float3 x)
		{
			return math.rsqrt(math.dot(x, x)) * x;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000CD5F File Offset: 0x0000AF5F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 normalize(float4 x)
		{
			return math.rsqrt(math.dot(x, x)) * x;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000CD73 File Offset: 0x0000AF73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 normalize(double2 x)
		{
			return math.rsqrt(math.dot(x, x)) * x;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000CD87 File Offset: 0x0000AF87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 normalize(double3 x)
		{
			return math.rsqrt(math.dot(x, x)) * x;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000CD9B File Offset: 0x0000AF9B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 normalize(double4 x)
		{
			return math.rsqrt(math.dot(x, x)) * x;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000CDB0 File Offset: 0x0000AFB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 normalizesafe(float2 x, float2 defaultvalue = default(float2))
		{
			float num = math.dot(x, x);
			return math.select(defaultvalue, x * math.rsqrt(num), num > 1.1754944E-38f);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000CDE0 File Offset: 0x0000AFE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 normalizesafe(float3 x, float3 defaultvalue = default(float3))
		{
			float num = math.dot(x, x);
			return math.select(defaultvalue, x * math.rsqrt(num), num > 1.1754944E-38f);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000CE10 File Offset: 0x0000B010
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 normalizesafe(float4 x, float4 defaultvalue = default(float4))
		{
			float num = math.dot(x, x);
			return math.select(defaultvalue, x * math.rsqrt(num), num > 1.1754944E-38f);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000CE40 File Offset: 0x0000B040
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 normalizesafe(double2 x, double2 defaultvalue = default(double2))
		{
			double num = math.dot(x, x);
			return math.select(defaultvalue, x * math.rsqrt(num), num > 1.1754943508222875E-38);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000CE74 File Offset: 0x0000B074
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 normalizesafe(double3 x, double3 defaultvalue = default(double3))
		{
			double num = math.dot(x, x);
			return math.select(defaultvalue, x * math.rsqrt(num), num > 1.1754943508222875E-38);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 normalizesafe(double4 x, double4 defaultvalue = default(double4))
		{
			double num = math.dot(x, x);
			return math.select(defaultvalue, x * math.rsqrt(num), num > 1.1754943508222875E-38);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000CEDB File Offset: 0x0000B0DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float length(float x)
		{
			return math.abs(x);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000CEE3 File Offset: 0x0000B0E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float length(float2 x)
		{
			return math.sqrt(math.dot(x, x));
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000CEF1 File Offset: 0x0000B0F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float length(float3 x)
		{
			return math.sqrt(math.dot(x, x));
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000CEFF File Offset: 0x0000B0FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float length(float4 x)
		{
			return math.sqrt(math.dot(x, x));
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000CF0D File Offset: 0x0000B10D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double length(double x)
		{
			return math.abs(x);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000CF15 File Offset: 0x0000B115
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double length(double2 x)
		{
			return math.sqrt(math.dot(x, x));
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000CF23 File Offset: 0x0000B123
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double length(double3 x)
		{
			return math.sqrt(math.dot(x, x));
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000CF31 File Offset: 0x0000B131
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double length(double4 x)
		{
			return math.sqrt(math.dot(x, x));
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000CF3F File Offset: 0x0000B13F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float lengthsq(float x)
		{
			return x * x;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000CF44 File Offset: 0x0000B144
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float lengthsq(float2 x)
		{
			return math.dot(x, x);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000CF4D File Offset: 0x0000B14D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float lengthsq(float3 x)
		{
			return math.dot(x, x);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000CF56 File Offset: 0x0000B156
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float lengthsq(float4 x)
		{
			return math.dot(x, x);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000CF5F File Offset: 0x0000B15F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double lengthsq(double x)
		{
			return x * x;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000CF64 File Offset: 0x0000B164
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double lengthsq(double2 x)
		{
			return math.dot(x, x);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000CF6D File Offset: 0x0000B16D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double lengthsq(double3 x)
		{
			return math.dot(x, x);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000CF76 File Offset: 0x0000B176
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double lengthsq(double4 x)
		{
			return math.dot(x, x);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000CF7F File Offset: 0x0000B17F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distance(float x, float y)
		{
			return math.abs(y - x);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000CF89 File Offset: 0x0000B189
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distance(float2 x, float2 y)
		{
			return math.length(y - x);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000CF97 File Offset: 0x0000B197
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distance(float3 x, float3 y)
		{
			return math.length(y - x);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000CFA5 File Offset: 0x0000B1A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distance(float4 x, float4 y)
		{
			return math.length(y - x);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000CFB3 File Offset: 0x0000B1B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distance(double x, double y)
		{
			return math.abs(y - x);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000CFBD File Offset: 0x0000B1BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distance(double2 x, double2 y)
		{
			return math.length(y - x);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000CFCB File Offset: 0x0000B1CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distance(double3 x, double3 y)
		{
			return math.length(y - x);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000CFD9 File Offset: 0x0000B1D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distance(double4 x, double4 y)
		{
			return math.length(y - x);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000CFE7 File Offset: 0x0000B1E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distancesq(float x, float y)
		{
			return (y - x) * (y - x);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000CFF0 File Offset: 0x0000B1F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distancesq(float2 x, float2 y)
		{
			return math.lengthsq(y - x);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000CFFE File Offset: 0x0000B1FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distancesq(float3 x, float3 y)
		{
			return math.lengthsq(y - x);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0000D00C File Offset: 0x0000B20C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float distancesq(float4 x, float4 y)
		{
			return math.lengthsq(y - x);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000D01A File Offset: 0x0000B21A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distancesq(double x, double y)
		{
			return (y - x) * (y - x);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000D023 File Offset: 0x0000B223
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distancesq(double2 x, double2 y)
		{
			return math.lengthsq(y - x);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000D031 File Offset: 0x0000B231
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distancesq(double3 x, double3 y)
		{
			return math.lengthsq(y - x);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000D03F File Offset: 0x0000B23F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double distancesq(double4 x, double4 y)
		{
			return math.lengthsq(y - x);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000D050 File Offset: 0x0000B250
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 cross(float3 x, float3 y)
		{
			return (x * y.yzx - x.yzx * y).yzx;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000D084 File Offset: 0x0000B284
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 cross(double3 x, double3 y)
		{
			return (x * y.yzx - x.yzx * y).yzx;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float smoothstep(float xMin, float xMax, float x)
		{
			float num = math.saturate((x - xMin) / (xMax - xMin));
			return num * num * (3f - 2f * num);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 smoothstep(float2 xMin, float2 xMax, float2 x)
		{
			float2 @float = math.saturate((x - xMin) / (xMax - xMin));
			return @float * @float * (3f - 2f * @float);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000D12C File Offset: 0x0000B32C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 smoothstep(float3 xMin, float3 xMax, float3 x)
		{
			float3 @float = math.saturate((x - xMin) / (xMax - xMin));
			return @float * @float * (3f - 2f * @float);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000D174 File Offset: 0x0000B374
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 smoothstep(float4 xMin, float4 xMax, float4 x)
		{
			float4 @float = math.saturate((x - xMin) / (xMax - xMin));
			return @float * @float * (3f - 2f * @float);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000D1BC File Offset: 0x0000B3BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double smoothstep(double xMin, double xMax, double x)
		{
			double num = math.saturate((x - xMin) / (xMax - xMin));
			return num * num * (3.0 - 2.0 * num);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 smoothstep(double2 xMin, double2 xMax, double2 x)
		{
			double2 @double = math.saturate((x - xMin) / (xMax - xMin));
			return @double * @double * (3.0 - 2.0 * @double);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000D240 File Offset: 0x0000B440
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 smoothstep(double3 xMin, double3 xMax, double3 x)
		{
			double3 @double = math.saturate((x - xMin) / (xMax - xMin));
			return @double * @double * (3.0 - 2.0 * @double);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000D290 File Offset: 0x0000B490
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 smoothstep(double4 xMin, double4 xMax, double4 x)
		{
			double4 @double = math.saturate((x - xMin) / (xMax - xMin));
			return @double * @double * (3.0 - 2.0 * @double);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000D2DF File Offset: 0x0000B4DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(bool2 x)
		{
			return x.x || x.y;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000D2F1 File Offset: 0x0000B4F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(bool3 x)
		{
			return x.x || x.y || x.z;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000D30B File Offset: 0x0000B50B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(bool4 x)
		{
			return x.x || x.y || x.z || x.w;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000D32D File Offset: 0x0000B52D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(int2 x)
		{
			return x.x != 0 || x.y != 0;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000D342 File Offset: 0x0000B542
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(int3 x)
		{
			return x.x != 0 || x.y != 0 || x.z != 0;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000D35F File Offset: 0x0000B55F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(int4 x)
		{
			return x.x != 0 || x.y != 0 || x.z != 0 || x.w != 0;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000D384 File Offset: 0x0000B584
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(uint2 x)
		{
			return x.x != 0U || x.y > 0U;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000D399 File Offset: 0x0000B599
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(uint3 x)
		{
			return x.x != 0U || x.y != 0U || x.z > 0U;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000D3B6 File Offset: 0x0000B5B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(uint4 x)
		{
			return x.x != 0U || x.y != 0U || x.z != 0U || x.w > 0U;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000D3DB File Offset: 0x0000B5DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(float2 x)
		{
			return x.x != 0f || x.y != 0f;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000D3FC File Offset: 0x0000B5FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(float3 x)
		{
			return x.x != 0f || x.y != 0f || x.z != 0f;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000D42A File Offset: 0x0000B62A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(float4 x)
		{
			return x.x != 0f || x.y != 0f || x.z != 0f || x.w != 0f;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000D465 File Offset: 0x0000B665
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(double2 x)
		{
			return x.x != 0.0 || x.y != 0.0;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000D48E File Offset: 0x0000B68E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(double3 x)
		{
			return x.x != 0.0 || x.y != 0.0 || x.z != 0.0;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000D4C8 File Offset: 0x0000B6C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool any(double4 x)
		{
			return x.x != 0.0 || x.y != 0.0 || x.z != 0.0 || x.w != 0.0;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000D51E File Offset: 0x0000B71E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(bool2 x)
		{
			return x.x && x.y;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000D530 File Offset: 0x0000B730
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(bool3 x)
		{
			return x.x && x.y && x.z;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000D54A File Offset: 0x0000B74A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(bool4 x)
		{
			return x.x && x.y && x.z && x.w;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000D56C File Offset: 0x0000B76C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(int2 x)
		{
			return x.x != 0 && x.y != 0;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000D581 File Offset: 0x0000B781
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(int3 x)
		{
			return x.x != 0 && x.y != 0 && x.z != 0;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000D59E File Offset: 0x0000B79E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(int4 x)
		{
			return x.x != 0 && x.y != 0 && x.z != 0 && x.w != 0;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000D5C3 File Offset: 0x0000B7C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(uint2 x)
		{
			return x.x != 0U && x.y > 0U;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000D5D8 File Offset: 0x0000B7D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(uint3 x)
		{
			return x.x != 0U && x.y != 0U && x.z > 0U;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000D5F5 File Offset: 0x0000B7F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(uint4 x)
		{
			return x.x != 0U && x.y != 0U && x.z != 0U && x.w > 0U;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000D61A File Offset: 0x0000B81A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(float2 x)
		{
			return x.x != 0f && x.y != 0f;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000D63B File Offset: 0x0000B83B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(float3 x)
		{
			return x.x != 0f && x.y != 0f && x.z != 0f;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0000D669 File Offset: 0x0000B869
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(float4 x)
		{
			return x.x != 0f && x.y != 0f && x.z != 0f && x.w != 0f;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0000D6A4 File Offset: 0x0000B8A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(double2 x)
		{
			return x.x != 0.0 && x.y != 0.0;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0000D6CD File Offset: 0x0000B8CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(double3 x)
		{
			return x.x != 0.0 && x.y != 0.0 && x.z != 0.0;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000D708 File Offset: 0x0000B908
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool all(double4 x)
		{
			return x.x != 0.0 && x.y != 0.0 && x.z != 0.0 && x.w != 0.0;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0000D75E File Offset: 0x0000B95E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int select(int falseValue, int trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000D766 File Offset: 0x0000B966
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 select(int2 falseValue, int2 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000D76E File Offset: 0x0000B96E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 select(int3 falseValue, int3 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000D776 File Offset: 0x0000B976
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 select(int4 falseValue, int4 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000D77E File Offset: 0x0000B97E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 select(int2 falseValue, int2 trueValue, bool2 test)
		{
			return new int2(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 select(int3 falseValue, int3 trueValue, bool3 test)
		{
			return new int3(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000D808 File Offset: 0x0000BA08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 select(int4 falseValue, int4 trueValue, bool4 test)
		{
			return new int4(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z, test.w ? trueValue.w : falseValue.w);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000D872 File Offset: 0x0000BA72
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint select(uint falseValue, uint trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0000D87A File Offset: 0x0000BA7A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 select(uint2 falseValue, uint2 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0000D882 File Offset: 0x0000BA82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 select(uint3 falseValue, uint3 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0000D88A File Offset: 0x0000BA8A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 select(uint4 falseValue, uint4 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000D892 File Offset: 0x0000BA92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 select(uint2 falseValue, uint2 trueValue, bool2 test)
		{
			return new uint2(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 select(uint3 falseValue, uint3 trueValue, bool3 test)
		{
			return new uint3(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0000D91C File Offset: 0x0000BB1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 select(uint4 falseValue, uint4 trueValue, bool4 test)
		{
			return new uint4(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z, test.w ? trueValue.w : falseValue.w);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000D986 File Offset: 0x0000BB86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long select(long falseValue, long trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0000D98E File Offset: 0x0000BB8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong select(ulong falseValue, ulong trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000D996 File Offset: 0x0000BB96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float select(float falseValue, float trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000D99E File Offset: 0x0000BB9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 select(float2 falseValue, float2 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000D9A6 File Offset: 0x0000BBA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 select(float3 falseValue, float3 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000D9AE File Offset: 0x0000BBAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 select(float4 falseValue, float4 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000D9B6 File Offset: 0x0000BBB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 select(float2 falseValue, float2 trueValue, bool2 test)
		{
			return new float2(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000D9EC File Offset: 0x0000BBEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 select(float3 falseValue, float3 trueValue, bool3 test)
		{
			return new float3(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000DA40 File Offset: 0x0000BC40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 select(float4 falseValue, float4 trueValue, bool4 test)
		{
			return new float4(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z, test.w ? trueValue.w : falseValue.w);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000DAAA File Offset: 0x0000BCAA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double select(double falseValue, double trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000DAB2 File Offset: 0x0000BCB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 select(double2 falseValue, double2 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000DABA File Offset: 0x0000BCBA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 select(double3 falseValue, double3 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000DAC2 File Offset: 0x0000BCC2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 select(double4 falseValue, double4 trueValue, bool test)
		{
			if (!test)
			{
				return falseValue;
			}
			return trueValue;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000DACA File Offset: 0x0000BCCA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 select(double2 falseValue, double2 trueValue, bool2 test)
		{
			return new double2(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000DB00 File Offset: 0x0000BD00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 select(double3 falseValue, double3 trueValue, bool3 test)
		{
			return new double3(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000DB54 File Offset: 0x0000BD54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 select(double4 falseValue, double4 trueValue, bool4 test)
		{
			return new double4(test.x ? trueValue.x : falseValue.x, test.y ? trueValue.y : falseValue.y, test.z ? trueValue.z : falseValue.z, test.w ? trueValue.w : falseValue.w);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000DBBE File Offset: 0x0000BDBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float step(float threshold, float x)
		{
			return math.select(0f, 1f, x >= threshold);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000DBD6 File Offset: 0x0000BDD6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 step(float2 threshold, float2 x)
		{
			return math.select(math.float2(0f), math.float2(1f), x >= threshold);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000DBF8 File Offset: 0x0000BDF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 step(float3 threshold, float3 x)
		{
			return math.select(math.float3(0f), math.float3(1f), x >= threshold);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000DC1A File Offset: 0x0000BE1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 step(float4 threshold, float4 x)
		{
			return math.select(math.float4(0f), math.float4(1f), x >= threshold);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double step(double threshold, double x)
		{
			return math.select(0.0, 1.0, x >= threshold);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000DC5C File Offset: 0x0000BE5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 step(double2 threshold, double2 x)
		{
			return math.select(math.double2(0.0), math.double2(1.0), x >= threshold);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000DC86 File Offset: 0x0000BE86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 step(double3 threshold, double3 x)
		{
			return math.select(math.double3(0.0), math.double3(1.0), x >= threshold);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 step(double4 threshold, double4 x)
		{
			return math.select(math.double4(0.0), math.double4(1.0), x >= threshold);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000DCDA File Offset: 0x0000BEDA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 reflect(float2 i, float2 n)
		{
			return i - 2f * n * math.dot(i, n);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000DCF9 File Offset: 0x0000BEF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 reflect(float3 i, float3 n)
		{
			return i - 2f * n * math.dot(i, n);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0000DD18 File Offset: 0x0000BF18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 reflect(float4 i, float4 n)
		{
			return i - 2f * n * math.dot(i, n);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000DD37 File Offset: 0x0000BF37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 reflect(double2 i, double2 n)
		{
			return i - 2.0 * n * math.dot(i, n);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000DD5A File Offset: 0x0000BF5A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 reflect(double3 i, double3 n)
		{
			return i - 2.0 * n * math.dot(i, n);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000DD7D File Offset: 0x0000BF7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 reflect(double4 i, double4 n)
		{
			return i - 2.0 * n * math.dot(i, n);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 refract(float2 i, float2 n, float indexOfRefraction)
		{
			float num = math.dot(n, i);
			float num2 = 1f - indexOfRefraction * indexOfRefraction * (1f - num * num);
			return math.select(0f, indexOfRefraction * i - (indexOfRefraction * num + math.sqrt(num2)) * n, num2 >= 0f);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000DE00 File Offset: 0x0000C000
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 refract(float3 i, float3 n, float indexOfRefraction)
		{
			float num = math.dot(n, i);
			float num2 = 1f - indexOfRefraction * indexOfRefraction * (1f - num * num);
			return math.select(0f, indexOfRefraction * i - (indexOfRefraction * num + math.sqrt(num2)) * n, num2 >= 0f);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000DE60 File Offset: 0x0000C060
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 refract(float4 i, float4 n, float indexOfRefraction)
		{
			float num = math.dot(n, i);
			float num2 = 1f - indexOfRefraction * indexOfRefraction * (1f - num * num);
			return math.select(0f, indexOfRefraction * i - (indexOfRefraction * num + math.sqrt(num2)) * n, num2 >= 0f);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 refract(double2 i, double2 n, double indexOfRefraction)
		{
			double num = math.dot(n, i);
			double num2 = 1.0 - indexOfRefraction * indexOfRefraction * (1.0 - num * num);
			return math.select(0f, indexOfRefraction * i - (indexOfRefraction * num + math.sqrt(num2)) * n, num2 >= 0.0);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000DF2C File Offset: 0x0000C12C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 refract(double3 i, double3 n, double indexOfRefraction)
		{
			double num = math.dot(n, i);
			double num2 = 1.0 - indexOfRefraction * indexOfRefraction * (1.0 - num * num);
			return math.select(0f, indexOfRefraction * i - (indexOfRefraction * num + math.sqrt(num2)) * n, num2 >= 0.0);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000DF98 File Offset: 0x0000C198
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 refract(double4 i, double4 n, double indexOfRefraction)
		{
			double num = math.dot(n, i);
			double num2 = 1.0 - indexOfRefraction * indexOfRefraction * (1.0 - num * num);
			return math.select(0f, indexOfRefraction * i - (indexOfRefraction * num + math.sqrt(num2)) * n, num2 >= 0.0);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000E003 File Offset: 0x0000C203
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 project(float2 a, float2 ontoB)
		{
			return math.dot(a, ontoB) / math.dot(ontoB, ontoB) * ontoB;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000E01A File Offset: 0x0000C21A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 project(float3 a, float3 ontoB)
		{
			return math.dot(a, ontoB) / math.dot(ontoB, ontoB) * ontoB;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000E031 File Offset: 0x0000C231
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 project(float4 a, float4 ontoB)
		{
			return math.dot(a, ontoB) / math.dot(ontoB, ontoB) * ontoB;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000E048 File Offset: 0x0000C248
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 projectsafe(float2 a, float2 ontoB, float2 defaultValue = default(float2))
		{
			float2 @float = math.project(a, ontoB);
			return math.select(defaultValue, @float, math.all(math.isfinite(@float)));
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000E070 File Offset: 0x0000C270
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 projectsafe(float3 a, float3 ontoB, float3 defaultValue = default(float3))
		{
			float3 @float = math.project(a, ontoB);
			return math.select(defaultValue, @float, math.all(math.isfinite(@float)));
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000E098 File Offset: 0x0000C298
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 projectsafe(float4 a, float4 ontoB, float4 defaultValue = default(float4))
		{
			float4 @float = math.project(a, ontoB);
			return math.select(defaultValue, @float, math.all(math.isfinite(@float)));
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000E0BF File Offset: 0x0000C2BF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 project(double2 a, double2 ontoB)
		{
			return math.dot(a, ontoB) / math.dot(ontoB, ontoB) * ontoB;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000E0D6 File Offset: 0x0000C2D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 project(double3 a, double3 ontoB)
		{
			return math.dot(a, ontoB) / math.dot(ontoB, ontoB) * ontoB;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000E0ED File Offset: 0x0000C2ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 project(double4 a, double4 ontoB)
		{
			return math.dot(a, ontoB) / math.dot(ontoB, ontoB) * ontoB;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000E104 File Offset: 0x0000C304
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 projectsafe(double2 a, double2 ontoB, double2 defaultValue = default(double2))
		{
			double2 @double = math.project(a, ontoB);
			return math.select(defaultValue, @double, math.all(math.isfinite(@double)));
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000E12C File Offset: 0x0000C32C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 projectsafe(double3 a, double3 ontoB, double3 defaultValue = default(double3))
		{
			double3 @double = math.project(a, ontoB);
			return math.select(defaultValue, @double, math.all(math.isfinite(@double)));
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000E154 File Offset: 0x0000C354
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 projectsafe(double4 a, double4 ontoB, double4 defaultValue = default(double4))
		{
			double4 @double = math.project(a, ontoB);
			return math.select(defaultValue, @double, math.all(math.isfinite(@double)));
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000E17B File Offset: 0x0000C37B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 faceforward(float2 n, float2 i, float2 ng)
		{
			return math.select(n, -n, math.dot(ng, i) >= 0f);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000E19A File Offset: 0x0000C39A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 faceforward(float3 n, float3 i, float3 ng)
		{
			return math.select(n, -n, math.dot(ng, i) >= 0f);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000E1B9 File Offset: 0x0000C3B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 faceforward(float4 n, float4 i, float4 ng)
		{
			return math.select(n, -n, math.dot(ng, i) >= 0f);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 faceforward(double2 n, double2 i, double2 ng)
		{
			return math.select(n, -n, math.dot(ng, i) >= 0.0);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000E1FB File Offset: 0x0000C3FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 faceforward(double3 n, double3 i, double3 ng)
		{
			return math.select(n, -n, math.dot(ng, i) >= 0.0);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000E21E File Offset: 0x0000C41E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 faceforward(double4 n, double4 i, double4 ng)
		{
			return math.select(n, -n, math.dot(ng, i) >= 0.0);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000E241 File Offset: 0x0000C441
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(float x, out float s, out float c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000E253 File Offset: 0x0000C453
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(float2 x, out float2 s, out float2 c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000E26D File Offset: 0x0000C46D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(float3 x, out float3 s, out float3 c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000E287 File Offset: 0x0000C487
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(float4 x, out float4 s, out float4 c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000E2A1 File Offset: 0x0000C4A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(double x, out double s, out double c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0000E2B3 File Offset: 0x0000C4B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(double2 x, out double2 s, out double2 c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000E2CD File Offset: 0x0000C4CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(double3 x, out double3 s, out double3 c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000E2E7 File Offset: 0x0000C4E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void sincos(double4 x, out double4 s, out double4 c)
		{
			s = math.sin(x);
			c = math.cos(x);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000E301 File Offset: 0x0000C501
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int countbits(int x)
		{
			return math.countbits((uint)x);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000E309 File Offset: 0x0000C509
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 countbits(int2 x)
		{
			return math.countbits((uint2)x);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000E316 File Offset: 0x0000C516
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 countbits(int3 x)
		{
			return math.countbits((uint3)x);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0000E323 File Offset: 0x0000C523
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 countbits(int4 x)
		{
			return math.countbits((uint4)x);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0000E330 File Offset: 0x0000C530
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int countbits(uint x)
		{
			x -= (x >> 1) & 1431655765U;
			x = (x & 858993459U) + ((x >> 2) & 858993459U);
			return (int)(((x + (x >> 4)) & 252645135U) * 16843009U >> 24);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0000E368 File Offset: 0x0000C568
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 countbits(uint2 x)
		{
			x -= (x >> 1) & 1431655765U;
			x = (x & 858993459U) + ((x >> 2) & 858993459U);
			return math.int2(((x + (x >> 4)) & 252645135U) * 16843009U >> 24);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 countbits(uint3 x)
		{
			x -= (x >> 1) & 1431655765U;
			x = (x & 858993459U) + ((x >> 2) & 858993459U);
			return math.int3(((x + (x >> 4)) & 252645135U) * 16843009U >> 24);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000E458 File Offset: 0x0000C658
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 countbits(uint4 x)
		{
			x -= (x >> 1) & 1431655765U;
			x = (x & 858993459U) + ((x >> 2) & 858993459U);
			return math.int4(((x + (x >> 4)) & 252645135U) * 16843009U >> 24);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000E4D0 File Offset: 0x0000C6D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int countbits(ulong x)
		{
			x -= (x >> 1) & 6148914691236517205UL;
			x = (x & 3689348814741910323UL) + ((x >> 2) & 3689348814741910323UL);
			return (int)(((x + (x >> 4)) & 1085102592571150095UL) * 72340172838076673UL >> 56);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000E526 File Offset: 0x0000C726
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int countbits(long x)
		{
			return math.countbits((ulong)x);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0000E52E File Offset: 0x0000C72E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int lzcnt(int x)
		{
			return math.lzcnt((uint)x);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0000E536 File Offset: 0x0000C736
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 lzcnt(int2 x)
		{
			return math.int2(math.lzcnt(x.x), math.lzcnt(x.y));
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0000E553 File Offset: 0x0000C753
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 lzcnt(int3 x)
		{
			return math.int3(math.lzcnt(x.x), math.lzcnt(x.y), math.lzcnt(x.z));
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0000E57B File Offset: 0x0000C77B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 lzcnt(int4 x)
		{
			return math.int4(math.lzcnt(x.x), math.lzcnt(x.y), math.lzcnt(x.z), math.lzcnt(x.w));
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0000E5B0 File Offset: 0x0000C7B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int lzcnt(uint x)
		{
			if (x == 0U)
			{
				return 32;
			}
			math.LongDoubleUnion longDoubleUnion;
			longDoubleUnion.doubleValue = 0.0;
			longDoubleUnion.longValue = (long)(4841369599423283200UL + (ulong)x);
			longDoubleUnion.doubleValue -= 4503599627370496.0;
			return 1054 - (int)(longDoubleUnion.longValue >> 52);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0000E60A File Offset: 0x0000C80A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 lzcnt(uint2 x)
		{
			return math.int2(math.lzcnt(x.x), math.lzcnt(x.y));
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0000E627 File Offset: 0x0000C827
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 lzcnt(uint3 x)
		{
			return math.int3(math.lzcnt(x.x), math.lzcnt(x.y), math.lzcnt(x.z));
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0000E64F File Offset: 0x0000C84F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 lzcnt(uint4 x)
		{
			return math.int4(math.lzcnt(x.x), math.lzcnt(x.y), math.lzcnt(x.z), math.lzcnt(x.w));
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0000E682 File Offset: 0x0000C882
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int lzcnt(long x)
		{
			return math.lzcnt((ulong)x);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0000E68C File Offset: 0x0000C88C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int lzcnt(ulong x)
		{
			if (x == 0UL)
			{
				return 64;
			}
			uint num = (uint)(x >> 32);
			uint num2 = ((num != 0U) ? num : ((uint)x));
			int num3 = ((num != 0U) ? 1054 : 1086);
			math.LongDoubleUnion longDoubleUnion;
			longDoubleUnion.doubleValue = 0.0;
			longDoubleUnion.longValue = (long)(4841369599423283200UL + (ulong)num2);
			longDoubleUnion.doubleValue -= 4503599627370496.0;
			return num3 - (int)(longDoubleUnion.longValue >> 52);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0000E6FF File Offset: 0x0000C8FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int tzcnt(int x)
		{
			return math.tzcnt((uint)x);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0000E707 File Offset: 0x0000C907
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 tzcnt(int2 x)
		{
			return math.int2(math.tzcnt(x.x), math.tzcnt(x.y));
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000E724 File Offset: 0x0000C924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 tzcnt(int3 x)
		{
			return math.int3(math.tzcnt(x.x), math.tzcnt(x.y), math.tzcnt(x.z));
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0000E74C File Offset: 0x0000C94C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 tzcnt(int4 x)
		{
			return math.int4(math.tzcnt(x.x), math.tzcnt(x.y), math.tzcnt(x.z), math.tzcnt(x.w));
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0000E780 File Offset: 0x0000C980
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int tzcnt(uint x)
		{
			if (x == 0U)
			{
				return 32;
			}
			x &= (uint)(-(uint)((ulong)x));
			math.LongDoubleUnion longDoubleUnion;
			longDoubleUnion.doubleValue = 0.0;
			longDoubleUnion.longValue = (long)(4841369599423283200UL + (ulong)x);
			longDoubleUnion.doubleValue -= 4503599627370496.0;
			return (int)(longDoubleUnion.longValue >> 52) - 1023;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0000E7E2 File Offset: 0x0000C9E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 tzcnt(uint2 x)
		{
			return math.int2(math.tzcnt(x.x), math.tzcnt(x.y));
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0000E7FF File Offset: 0x0000C9FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 tzcnt(uint3 x)
		{
			return math.int3(math.tzcnt(x.x), math.tzcnt(x.y), math.tzcnt(x.z));
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0000E827 File Offset: 0x0000CA27
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 tzcnt(uint4 x)
		{
			return math.int4(math.tzcnt(x.x), math.tzcnt(x.y), math.tzcnt(x.z), math.tzcnt(x.w));
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0000E85A File Offset: 0x0000CA5A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int tzcnt(long x)
		{
			return math.tzcnt((ulong)x);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0000E864 File Offset: 0x0000CA64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int tzcnt(ulong x)
		{
			if (x == 0UL)
			{
				return 64;
			}
			x &= -x;
			uint num = (uint)x;
			uint num2 = ((num != 0U) ? num : ((uint)(x >> 32)));
			int num3 = ((num != 0U) ? 1023 : 991);
			math.LongDoubleUnion longDoubleUnion;
			longDoubleUnion.doubleValue = 0.0;
			longDoubleUnion.longValue = (long)(4841369599423283200UL + (ulong)num2);
			longDoubleUnion.doubleValue -= 4503599627370496.0;
			return (int)(longDoubleUnion.longValue >> 52) - num3;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0000E8DF File Offset: 0x0000CADF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int reversebits(int x)
		{
			return (int)math.reversebits((uint)x);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0000E8E7 File Offset: 0x0000CAE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 reversebits(int2 x)
		{
			return (int2)math.reversebits((uint2)x);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0000E8F9 File Offset: 0x0000CAF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 reversebits(int3 x)
		{
			return (int3)math.reversebits((uint3)x);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0000E90B File Offset: 0x0000CB0B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 reversebits(int4 x)
		{
			return (int4)math.reversebits((uint4)x);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0000E920 File Offset: 0x0000CB20
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint reversebits(uint x)
		{
			x = ((x >> 1) & 1431655765U) | ((x & 1431655765U) << 1);
			x = ((x >> 2) & 858993459U) | ((x & 858993459U) << 2);
			x = ((x >> 4) & 252645135U) | ((x & 252645135U) << 4);
			x = ((x >> 8) & 16711935U) | ((x & 16711935U) << 8);
			return (x >> 16) | (x << 16);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0000E98C File Offset: 0x0000CB8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 reversebits(uint2 x)
		{
			x = ((x >> 1) & 1431655765U) | ((x & 1431655765U) << 1);
			x = ((x >> 2) & 858993459U) | ((x & 858993459U) << 2);
			x = ((x >> 4) & 252645135U) | ((x & 252645135U) << 4);
			x = ((x >> 8) & 16711935U) | ((x & 16711935U) << 8);
			return (x >> 16) | (x << 16);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000EA54 File Offset: 0x0000CC54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 reversebits(uint3 x)
		{
			x = ((x >> 1) & 1431655765U) | ((x & 1431655765U) << 1);
			x = ((x >> 2) & 858993459U) | ((x & 858993459U) << 2);
			x = ((x >> 4) & 252645135U) | ((x & 252645135U) << 4);
			x = ((x >> 8) & 16711935U) | ((x & 16711935U) << 8);
			return (x >> 16) | (x << 16);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0000EB1C File Offset: 0x0000CD1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 reversebits(uint4 x)
		{
			x = ((x >> 1) & 1431655765U) | ((x & 1431655765U) << 1);
			x = ((x >> 2) & 858993459U) | ((x & 858993459U) << 2);
			x = ((x >> 4) & 252645135U) | ((x & 252645135U) << 4);
			x = ((x >> 8) & 16711935U) | ((x & 16711935U) << 8);
			return (x >> 16) | (x << 16);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0000EBE2 File Offset: 0x0000CDE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long reversebits(long x)
		{
			return (long)math.reversebits((ulong)x);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0000EBEC File Offset: 0x0000CDEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong reversebits(ulong x)
		{
			x = ((x >> 1) & 6148914691236517205UL) | ((x & 6148914691236517205UL) << 1);
			x = ((x >> 2) & 3689348814741910323UL) | ((x & 3689348814741910323UL) << 2);
			x = ((x >> 4) & 1085102592571150095UL) | ((x & 1085102592571150095UL) << 4);
			x = ((x >> 8) & 71777214294589695UL) | ((x & 71777214294589695UL) << 8);
			x = ((x >> 16) & 281470681808895UL) | ((x & 281470681808895UL) << 16);
			return (x >> 32) | (x << 32);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0000EC95 File Offset: 0x0000CE95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int rol(int x, int n)
		{
			return (int)math.rol((uint)x, n);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0000EC9E File Offset: 0x0000CE9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 rol(int2 x, int n)
		{
			return (int2)math.rol((uint2)x, n);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000ECB1 File Offset: 0x0000CEB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 rol(int3 x, int n)
		{
			return (int3)math.rol((uint3)x, n);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0000ECC4 File Offset: 0x0000CEC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 rol(int4 x, int n)
		{
			return (int4)math.rol((uint4)x, n);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000ECD7 File Offset: 0x0000CED7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint rol(uint x, int n)
		{
			return (x << n) | (x >> 32 - n);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000ECE9 File Offset: 0x0000CEE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 rol(uint2 x, int n)
		{
			return (x << n) | (x >> 32 - n);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0000ED01 File Offset: 0x0000CF01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 rol(uint3 x, int n)
		{
			return (x << n) | (x >> 32 - n);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0000ED19 File Offset: 0x0000CF19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 rol(uint4 x, int n)
		{
			return (x << n) | (x >> 32 - n);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0000ED31 File Offset: 0x0000CF31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long rol(long x, int n)
		{
			return (long)math.rol((ulong)x, n);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000ED3A File Offset: 0x0000CF3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong rol(ulong x, int n)
		{
			return (x << n) | (x >> 64 - n);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0000ED4C File Offset: 0x0000CF4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ror(int x, int n)
		{
			return (int)math.ror((uint)x, n);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000ED55 File Offset: 0x0000CF55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 ror(int2 x, int n)
		{
			return (int2)math.ror((uint2)x, n);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000ED68 File Offset: 0x0000CF68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 ror(int3 x, int n)
		{
			return (int3)math.ror((uint3)x, n);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000ED7B File Offset: 0x0000CF7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 ror(int4 x, int n)
		{
			return (int4)math.ror((uint4)x, n);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000ED8E File Offset: 0x0000CF8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ror(uint x, int n)
		{
			return (x >> n) | (x << 32 - n);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0000EDA0 File Offset: 0x0000CFA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 ror(uint2 x, int n)
		{
			return (x >> n) | (x << 32 - n);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 ror(uint3 x, int n)
		{
			return (x >> n) | (x << 32 - n);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000EDD0 File Offset: 0x0000CFD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 ror(uint4 x, int n)
		{
			return (x >> n) | (x << 32 - n);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ror(long x, int n)
		{
			return (long)math.ror((ulong)x, n);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000EDF1 File Offset: 0x0000CFF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ror(ulong x, int n)
		{
			return (x >> n) | (x << 64 - n);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000EE03 File Offset: 0x0000D003
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ceilpow2(int x)
		{
			x--;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000EE34 File Offset: 0x0000D034
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 ceilpow2(int2 x)
		{
			x -= 1;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 ceilpow2(int3 x)
		{
			x -= 1;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000EF0C File Offset: 0x0000D10C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 ceilpow2(int4 x)
		{
			x -= 1;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000EF75 File Offset: 0x0000D175
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ceilpow2(uint x)
		{
			x -= 1U;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1U;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 ceilpow2(uint2 x)
		{
			x -= 1U;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1U;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0000F010 File Offset: 0x0000D210
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 ceilpow2(uint3 x)
		{
			x -= 1U;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1U;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0000F07C File Offset: 0x0000D27C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 ceilpow2(uint4 x)
		{
			x -= 1U;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1U;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0000F0E5 File Offset: 0x0000D2E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ceilpow2(long x)
		{
			x -= 1L;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			x |= x >> 32;
			return x + 1L;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0000F11D File Offset: 0x0000D31D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ceilpow2(ulong x)
		{
			x -= 1UL;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			x |= x >> 32;
			return x + 1UL;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0000F155 File Offset: 0x0000D355
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ceillog2(int x)
		{
			return 32 - math.lzcnt((uint)(x - 1));
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0000F162 File Offset: 0x0000D362
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 ceillog2(int2 x)
		{
			return new int2(math.ceillog2(x.x), math.ceillog2(x.y));
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0000F17F File Offset: 0x0000D37F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 ceillog2(int3 x)
		{
			return new int3(math.ceillog2(x.x), math.ceillog2(x.y), math.ceillog2(x.z));
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0000F1A7 File Offset: 0x0000D3A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 ceillog2(int4 x)
		{
			return new int4(math.ceillog2(x.x), math.ceillog2(x.y), math.ceillog2(x.z), math.ceillog2(x.w));
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0000F1DA File Offset: 0x0000D3DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ceillog2(uint x)
		{
			return 32 - math.lzcnt(x - 1U);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0000F1E7 File Offset: 0x0000D3E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 ceillog2(uint2 x)
		{
			return new int2(math.ceillog2(x.x), math.ceillog2(x.y));
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0000F204 File Offset: 0x0000D404
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 ceillog2(uint3 x)
		{
			return new int3(math.ceillog2(x.x), math.ceillog2(x.y), math.ceillog2(x.z));
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0000F22C File Offset: 0x0000D42C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 ceillog2(uint4 x)
		{
			return new int4(math.ceillog2(x.x), math.ceillog2(x.y), math.ceillog2(x.z), math.ceillog2(x.w));
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0000F25F File Offset: 0x0000D45F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int floorlog2(int x)
		{
			return 31 - math.lzcnt((uint)x);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0000F26A File Offset: 0x0000D46A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 floorlog2(int2 x)
		{
			return new int2(math.floorlog2(x.x), math.floorlog2(x.y));
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0000F287 File Offset: 0x0000D487
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 floorlog2(int3 x)
		{
			return new int3(math.floorlog2(x.x), math.floorlog2(x.y), math.floorlog2(x.z));
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0000F2AF File Offset: 0x0000D4AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 floorlog2(int4 x)
		{
			return new int4(math.floorlog2(x.x), math.floorlog2(x.y), math.floorlog2(x.z), math.floorlog2(x.w));
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0000F2E2 File Offset: 0x0000D4E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int floorlog2(uint x)
		{
			return 31 - math.lzcnt(x);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0000F2ED File Offset: 0x0000D4ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 floorlog2(uint2 x)
		{
			return new int2(math.floorlog2(x.x), math.floorlog2(x.y));
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0000F30A File Offset: 0x0000D50A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 floorlog2(uint3 x)
		{
			return new int3(math.floorlog2(x.x), math.floorlog2(x.y), math.floorlog2(x.z));
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0000F332 File Offset: 0x0000D532
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 floorlog2(uint4 x)
		{
			return new int4(math.floorlog2(x.x), math.floorlog2(x.y), math.floorlog2(x.z), math.floorlog2(x.w));
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0000F365 File Offset: 0x0000D565
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float radians(float x)
		{
			return x * 0.017453292f;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0000F36E File Offset: 0x0000D56E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 radians(float2 x)
		{
			return x * 0.017453292f;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0000F37B File Offset: 0x0000D57B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 radians(float3 x)
		{
			return x * 0.017453292f;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0000F388 File Offset: 0x0000D588
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 radians(float4 x)
		{
			return x * 0.017453292f;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0000F395 File Offset: 0x0000D595
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double radians(double x)
		{
			return x * 0.017453292519943295;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000F3A2 File Offset: 0x0000D5A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 radians(double2 x)
		{
			return x * 0.017453292519943295;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0000F3B3 File Offset: 0x0000D5B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 radians(double3 x)
		{
			return x * 0.017453292519943295;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000F3C4 File Offset: 0x0000D5C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 radians(double4 x)
		{
			return x * 0.017453292519943295;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0000F3D5 File Offset: 0x0000D5D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float degrees(float x)
		{
			return x * 57.29578f;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0000F3DE File Offset: 0x0000D5DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 degrees(float2 x)
		{
			return x * 57.29578f;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000F3EB File Offset: 0x0000D5EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 degrees(float3 x)
		{
			return x * 57.29578f;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0000F3F8 File Offset: 0x0000D5F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 degrees(float4 x)
		{
			return x * 57.29578f;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000F405 File Offset: 0x0000D605
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double degrees(double x)
		{
			return x * 57.29577951308232;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000F412 File Offset: 0x0000D612
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 degrees(double2 x)
		{
			return x * 57.29577951308232;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0000F423 File Offset: 0x0000D623
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 degrees(double3 x)
		{
			return x * 57.29577951308232;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0000F434 File Offset: 0x0000D634
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 degrees(double4 x)
		{
			return x * 57.29577951308232;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0000F445 File Offset: 0x0000D645
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int cmin(int2 x)
		{
			return math.min(x.x, x.y);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0000F458 File Offset: 0x0000D658
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int cmin(int3 x)
		{
			return math.min(math.min(x.x, x.y), x.z);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000F476 File Offset: 0x0000D676
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int cmin(int4 x)
		{
			return math.min(math.min(x.x, x.y), math.min(x.z, x.w));
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0000F49F File Offset: 0x0000D69F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint cmin(uint2 x)
		{
			return math.min(x.x, x.y);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000F4B2 File Offset: 0x0000D6B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint cmin(uint3 x)
		{
			return math.min(math.min(x.x, x.y), x.z);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint cmin(uint4 x)
		{
			return math.min(math.min(x.x, x.y), math.min(x.z, x.w));
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000F4F9 File Offset: 0x0000D6F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cmin(float2 x)
		{
			return math.min(x.x, x.y);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0000F50C File Offset: 0x0000D70C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cmin(float3 x)
		{
			return math.min(math.min(x.x, x.y), x.z);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0000F52A File Offset: 0x0000D72A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cmin(float4 x)
		{
			return math.min(math.min(x.x, x.y), math.min(x.z, x.w));
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0000F553 File Offset: 0x0000D753
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cmin(double2 x)
		{
			return math.min(x.x, x.y);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000F566 File Offset: 0x0000D766
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cmin(double3 x)
		{
			return math.min(math.min(x.x, x.y), x.z);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0000F584 File Offset: 0x0000D784
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cmin(double4 x)
		{
			return math.min(math.min(x.x, x.y), math.min(x.z, x.w));
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0000F5AD File Offset: 0x0000D7AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int cmax(int2 x)
		{
			return math.max(x.x, x.y);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int cmax(int3 x)
		{
			return math.max(math.max(x.x, x.y), x.z);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0000F5DE File Offset: 0x0000D7DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int cmax(int4 x)
		{
			return math.max(math.max(x.x, x.y), math.max(x.z, x.w));
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000F607 File Offset: 0x0000D807
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint cmax(uint2 x)
		{
			return math.max(x.x, x.y);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0000F61A File Offset: 0x0000D81A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint cmax(uint3 x)
		{
			return math.max(math.max(x.x, x.y), x.z);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000F638 File Offset: 0x0000D838
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint cmax(uint4 x)
		{
			return math.max(math.max(x.x, x.y), math.max(x.z, x.w));
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000F661 File Offset: 0x0000D861
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cmax(float2 x)
		{
			return math.max(x.x, x.y);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000F674 File Offset: 0x0000D874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cmax(float3 x)
		{
			return math.max(math.max(x.x, x.y), x.z);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0000F692 File Offset: 0x0000D892
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float cmax(float4 x)
		{
			return math.max(math.max(x.x, x.y), math.max(x.z, x.w));
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0000F6BB File Offset: 0x0000D8BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cmax(double2 x)
		{
			return math.max(x.x, x.y);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0000F6CE File Offset: 0x0000D8CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cmax(double3 x)
		{
			return math.max(math.max(x.x, x.y), x.z);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0000F6EC File Offset: 0x0000D8EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double cmax(double4 x)
		{
			return math.max(math.max(x.x, x.y), math.max(x.z, x.w));
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000F715 File Offset: 0x0000D915
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int csum(int2 x)
		{
			return x.x + x.y;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000F724 File Offset: 0x0000D924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int csum(int3 x)
		{
			return x.x + x.y + x.z;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000F73A File Offset: 0x0000D93A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int csum(int4 x)
		{
			return x.x + x.y + x.z + x.w;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000F757 File Offset: 0x0000D957
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint csum(uint2 x)
		{
			return x.x + x.y;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000F766 File Offset: 0x0000D966
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint csum(uint3 x)
		{
			return x.x + x.y + x.z;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0000F77C File Offset: 0x0000D97C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint csum(uint4 x)
		{
			return x.x + x.y + x.z + x.w;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0000F799 File Offset: 0x0000D999
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float csum(float2 x)
		{
			return x.x + x.y;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float csum(float3 x)
		{
			return x.x + x.y + x.z;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0000F7BE File Offset: 0x0000D9BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float csum(float4 x)
		{
			return x.x + x.y + (x.z + x.w);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0000F7DB File Offset: 0x0000D9DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double csum(double2 x)
		{
			return x.x + x.y;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0000F7EA File Offset: 0x0000D9EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double csum(double3 x)
		{
			return x.x + x.y + x.z;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0000F800 File Offset: 0x0000DA00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double csum(double4 x)
		{
			return x.x + x.y + (x.z + x.w);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0000F81D File Offset: 0x0000DA1D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float square(float x)
		{
			return x * x;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0000F822 File Offset: 0x0000DA22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 square(float2 x)
		{
			return x * x;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0000F82B File Offset: 0x0000DA2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 square(float3 x)
		{
			return x * x;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0000F834 File Offset: 0x0000DA34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 square(float4 x)
		{
			return x * x;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0000F83D File Offset: 0x0000DA3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double square(double x)
		{
			return x * x;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0000F842 File Offset: 0x0000DA42
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 square(double2 x)
		{
			return x * x;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0000F84B File Offset: 0x0000DA4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 square(double3 x)
		{
			return x * x;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0000F854 File Offset: 0x0000DA54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 square(double4 x)
		{
			return x * x;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0000F85D File Offset: 0x0000DA5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int square(int x)
		{
			return x * x;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000F862 File Offset: 0x0000DA62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 square(int2 x)
		{
			return x * x;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0000F86B File Offset: 0x0000DA6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 square(int3 x)
		{
			return x * x;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0000F874 File Offset: 0x0000DA74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 square(int4 x)
		{
			return x * x;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0000F87D File Offset: 0x0000DA7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint square(uint x)
		{
			return x * x;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0000F882 File Offset: 0x0000DA82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 square(uint2 x)
		{
			return x * x;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0000F88B File Offset: 0x0000DA8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 square(uint3 x)
		{
			return x * x;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0000F894 File Offset: 0x0000DA94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 square(uint4 x)
		{
			return x * x;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int compress(int* output, int index, int4 val, bool4 mask)
		{
			if (mask.x)
			{
				output[index++] = val.x;
			}
			if (mask.y)
			{
				output[index++] = val.y;
			}
			if (mask.z)
			{
				output[index++] = val.z;
			}
			if (mask.w)
			{
				output[index++] = val.w;
			}
			return index;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000F916 File Offset: 0x0000DB16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int compress(uint* output, int index, uint4 val, bool4 mask)
		{
			return math.compress((int*)output, index, *(int4*)(&val), mask);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0000F928 File Offset: 0x0000DB28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int compress(float* output, int index, float4 val, bool4 mask)
		{
			return math.compress((int*)output, index, *(int4*)(&val), mask);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0000F93C File Offset: 0x0000DB3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float f16tof32(uint x)
		{
			uint num = (x & 32767U) << 13;
			uint num2 = num & 260046848U;
			uint num3 = num + 939524096U + math.select(0U, 939524096U, num2 == 260046848U);
			return math.asfloat(math.select(num3, math.asuint(math.asfloat(num3 + 8388608U) - 6.1035156E-05f), num2 == 0U) | ((x & 32768U) << 16));
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0000F9A8 File Offset: 0x0000DBA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 f16tof32(uint2 x)
		{
			uint2 @uint = (x & 32767U) << 13;
			uint2 uint2 = @uint & 260046848U;
			uint2 uint3 = @uint + 939524096U + math.select(0U, 939524096U, uint2 == 260046848U);
			return math.asfloat(math.select(uint3, math.asuint(math.asfloat(uint3 + 8388608U) - 6.1035156E-05f), uint2 == 0U) | ((x & 32768U) << 16));
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 f16tof32(uint3 x)
		{
			uint3 @uint = (x & 32767U) << 13;
			uint3 uint2 = @uint & 260046848U;
			uint3 uint3 = @uint + 939524096U + math.select(0U, 939524096U, uint2 == 260046848U);
			return math.asfloat(math.select(uint3, math.asuint(math.asfloat(uint3 + 8388608U) - 6.1035156E-05f), uint2 == 0U) | ((x & 32768U) << 16));
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 f16tof32(uint4 x)
		{
			uint4 @uint = (x & 32767U) << 13;
			uint4 uint2 = @uint & 260046848U;
			uint4 uint3 = @uint + 939524096U + math.select(0U, 939524096U, uint2 == 260046848U);
			return math.asfloat(math.select(uint3, math.asuint(math.asfloat(uint3 + 8388608U) - 6.1035156E-05f), uint2 == 0U) | ((x & 32768U) << 16));
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0000FB94 File Offset: 0x0000DD94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint f32tof16(float x)
		{
			uint num = math.asuint(x);
			uint num2 = num & 2147479552U;
			return math.select(math.asuint(math.min(math.asfloat(num2) * 1.92593E-34f, 260042750f)) + 4096U >> 13, math.select(31744U, 32256U, num2 > 2139095040U), num2 >= 2139095040U) | ((num & 2147487743U) >> 16);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0000FC08 File Offset: 0x0000DE08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 f32tof16(float2 x)
		{
			uint2 @uint = math.asuint(x);
			uint2 uint2 = @uint & 2147479552U;
			return math.select((uint2)(math.asint(math.min(math.asfloat(uint2) * 1.92593E-34f, 260042750f)) + 4096) >> 13, math.select(31744U, 32256U, (int2)uint2 > 2139095040), (int2)uint2 >= 2139095040) | ((@uint & 2147487743U) >> 16);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0000FCB8 File Offset: 0x0000DEB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 f32tof16(float3 x)
		{
			uint3 @uint = math.asuint(x);
			uint3 uint2 = @uint & 2147479552U;
			return math.select((uint3)(math.asint(math.min(math.asfloat(uint2) * 1.92593E-34f, 260042750f)) + 4096) >> 13, math.select(31744U, 32256U, (int3)uint2 > 2139095040), (int3)uint2 >= 2139095040) | ((@uint & 2147487743U) >> 16);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0000FD68 File Offset: 0x0000DF68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 f32tof16(float4 x)
		{
			uint4 @uint = math.asuint(x);
			uint4 uint2 = @uint & 2147479552U;
			return math.select((uint4)(math.asint(math.min(math.asfloat(uint2) * 1.92593E-34f, 260042750f)) + 4096) >> 13, math.select(31744U, 32256U, (int4)uint2 > 2139095040), (int4)uint2 >= 2139095040) | ((@uint & 2147487743U) >> 16);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0000FE18 File Offset: 0x0000E018
		public static void orthonormal_basis(float3 normal, out float3 basis1, out float3 basis2)
		{
			float num = ((normal.z >= 0f) ? 1f : (-1f));
			float num2 = -1f / (num + normal.z);
			float num3 = normal.x * normal.y * num2;
			basis1.x = 1f + num * normal.x * normal.x * num2;
			basis1.y = num * num3;
			basis1.z = -num * normal.x;
			basis2.x = num3;
			basis2.y = num + normal.y * normal.y * num2;
			basis2.z = -normal.y;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		public static void orthonormal_basis(double3 normal, out double3 basis1, out double3 basis2)
		{
			double num = ((normal.z >= 0.0) ? 1.0 : (-1.0));
			double num2 = -1.0 / (num + normal.z);
			double num3 = normal.x * normal.y * num2;
			basis1.x = 1.0 + num * normal.x * normal.x * num2;
			basis1.y = num * num3;
			basis1.z = -num * normal.x;
			basis2.x = num3;
			basis2.y = num + normal.y * normal.y * num2;
			basis2.z = -normal.y;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0000FF7A File Offset: 0x0000E17A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float chgsign(float x, float y)
		{
			return math.asfloat(math.asuint(x) ^ (math.asuint(y) & 2147483648U));
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0000FF94 File Offset: 0x0000E194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 chgsign(float2 x, float2 y)
		{
			return math.asfloat(math.asuint(x) ^ (math.asuint(y) & 2147483648U));
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0000FFB6 File Offset: 0x0000E1B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 chgsign(float3 x, float3 y)
		{
			return math.asfloat(math.asuint(x) ^ (math.asuint(y) & 2147483648U));
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0000FFD8 File Offset: 0x0000E1D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 chgsign(float4 x, float4 y)
		{
			return math.asfloat(math.asuint(x) ^ (math.asuint(y) & 2147483648U));
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static uint read32_little_endian(void* pBuffer)
		{
			return (uint)((int)(*(byte*)pBuffer) | ((int)((byte*)pBuffer)[1] << 8) | ((int)((byte*)pBuffer)[2] << 16) | ((int)((byte*)pBuffer)[3] << 24));
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00010024 File Offset: 0x0000E224
		private unsafe static uint hash_with_unaligned_loads(void* pBuffer, int numBytes, uint seed)
		{
			uint4* ptr = (uint4*)pBuffer;
			uint num = seed + 374761393U;
			if (numBytes >= 16)
			{
				uint4 @uint = new uint4(606290984U, 2246822519U, 0U, 1640531535U) + seed;
				int num2 = numBytes >> 4;
				for (int i = 0; i < num2; i++)
				{
					@uint += *(ptr++) * 2246822519U;
					@uint = (@uint << 13) | (@uint >> 19);
					@uint *= 2654435761U;
				}
				num = math.rol(@uint.x, 1) + math.rol(@uint.y, 7) + math.rol(@uint.z, 12) + math.rol(@uint.w, 18);
			}
			num += (uint)numBytes;
			uint* ptr2 = (uint*)ptr;
			for (int j = 0; j < ((numBytes >> 2) & 3); j++)
			{
				num += *(ptr2++) * 3266489917U;
				num = math.rol(num, 17) * 668265263U;
			}
			byte* ptr3 = (byte*)ptr2;
			for (int k = 0; k < (numBytes & 3); k++)
			{
				num += (uint)(*(ptr3++)) * 374761393U;
				num = math.rol(num, 11) * 2654435761U;
			}
			num ^= num >> 15;
			num *= 2246822519U;
			num ^= num >> 13;
			num *= 3266489917U;
			return num ^ (num >> 16);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001018C File Offset: 0x0000E38C
		private unsafe static uint hash_without_unaligned_loads(void* pBuffer, int numBytes, uint seed)
		{
			byte* ptr = (byte*)pBuffer;
			uint num = seed + 374761393U;
			if (numBytes >= 16)
			{
				uint4 @uint = new uint4(606290984U, 2246822519U, 0U, 1640531535U) + seed;
				int num2 = numBytes >> 4;
				for (int i = 0; i < num2; i++)
				{
					uint4 uint2 = new uint4(math.read32_little_endian((void*)ptr), math.read32_little_endian((void*)(ptr + 4)), math.read32_little_endian((void*)(ptr + 8)), math.read32_little_endian((void*)(ptr + 12)));
					@uint += uint2 * 2246822519U;
					@uint = math.rol(@uint, 13);
					@uint *= 2654435761U;
					ptr += 16;
				}
				num = math.rol(@uint.x, 1) + math.rol(@uint.y, 7) + math.rol(@uint.z, 12) + math.rol(@uint.w, 18);
			}
			num += (uint)numBytes;
			for (int j = 0; j < ((numBytes >> 2) & 3); j++)
			{
				num += math.read32_little_endian((void*)ptr) * 3266489917U;
				num = math.rol(num, 17) * 668265263U;
				ptr += 4;
			}
			for (int k = 0; k < (numBytes & 3); k++)
			{
				num += (uint)(*(ptr++)) * 374761393U;
				num = math.rol(num, 11) * 2654435761U;
			}
			num ^= num >> 15;
			num *= 2246822519U;
			num ^= num >> 13;
			num *= 3266489917U;
			return num ^ (num >> 16);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000102F4 File Offset: 0x0000E4F4
		public unsafe static uint hash(void* pBuffer, int numBytes, uint seed = 0U)
		{
			return math.hash_with_unaligned_loads(pBuffer, numBytes, seed);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000102FE File Offset: 0x0000E4FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 up()
		{
			return new float3(0f, 1f, 0f);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00010314 File Offset: 0x0000E514
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 down()
		{
			return new float3(0f, -1f, 0f);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001032A File Offset: 0x0000E52A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 forward()
		{
			return new float3(0f, 0f, 1f);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00010340 File Offset: 0x0000E540
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 back()
		{
			return new float3(0f, 0f, -1f);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00010356 File Offset: 0x0000E556
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 left()
		{
			return new float3(-1f, 0f, 0f);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001036C File Offset: 0x0000E56C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 right()
		{
			return new float3(1f, 0f, 0f);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00010384 File Offset: 0x0000E584
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 EulerXYZ(quaternion q)
		{
			float4 value = q.value;
			float4 @float = value * value.wwww * math.float4(2f);
			float4 float2 = value * value.yzxw * math.float4(2f);
			float4 float3 = value * value;
			float3 float4 = Unity.Mathematics.float3.zero;
			float num = float2.z - @float.y;
			if (num * num < 0.99999595f)
			{
				float num2 = float2.y + @float.x;
				float num3 = float3.z + float3.w - float3.y - float3.x;
				float num4 = float2.x + @float.z;
				float num5 = float3.x + float3.w - float3.y - float3.z;
				float4 = math.float3(math.atan2(num2, num3), -math.asin(num), math.atan2(num4, num5));
			}
			else
			{
				num = math.clamp(num, -1f, 1f);
				float4 float5 = math.float4(float2.z, @float.y, float2.x, @float.z);
				float num6 = 2f * (float5.x * float5.w + float5.y * float5.z);
				float num7 = math.csum(float5 * float5 * math.float4(-1f, 1f, -1f, 1f));
				float4 = math.float3(math.atan2(num6, num7), -math.asin(num), 0f);
			}
			return float4;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00010518 File Offset: 0x0000E718
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 EulerXZY(quaternion q)
		{
			float4 value = q.value;
			float4 @float = value * value.wwww * math.float4(2f);
			float4 float2 = value * value.yzxw * math.float4(2f);
			float4 float3 = value * value;
			float3 float4 = Unity.Mathematics.float3.zero;
			float num = float2.x + @float.z;
			if (num * num < 0.99999595f)
			{
				float num2 = -float2.y + @float.x;
				float num3 = float3.y + float3.w - float3.z - float3.x;
				float num4 = -float2.z + @float.y;
				float num5 = float3.x + float3.w - float3.y - float3.z;
				float4 = math.float3(math.atan2(num2, num3), math.asin(num), math.atan2(num4, num5));
			}
			else
			{
				num = math.clamp(num, -1f, 1f);
				float4 float5 = math.float4(float2.x, @float.z, float2.z, @float.y);
				float num6 = 2f * (float5.x * float5.w + float5.y * float5.z);
				float num7 = math.csum(float5 * float5 * math.float4(-1f, 1f, -1f, 1f));
				float4 = math.float3(math.atan2(num6, num7), math.asin(num), 0f);
			}
			return float4.xzy;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000106B4 File Offset: 0x0000E8B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 EulerYXZ(quaternion q)
		{
			float4 value = q.value;
			float4 @float = value * value.wwww * math.float4(2f);
			float4 float2 = value * value.yzxw * math.float4(2f);
			float4 float3 = value * value;
			float3 float4 = Unity.Mathematics.float3.zero;
			float num = float2.y + @float.x;
			if (num * num < 0.99999595f)
			{
				float num2 = -float2.z + @float.y;
				float num3 = float3.z + float3.w - float3.x - float3.y;
				float num4 = -float2.x + @float.z;
				float num5 = float3.y + float3.w - float3.z - float3.x;
				float4 = math.float3(math.atan2(num2, num3), math.asin(num), math.atan2(num4, num5));
			}
			else
			{
				num = math.clamp(num, -1f, 1f);
				float4 float5 = math.float4(float2.x, @float.z, float2.y, @float.x);
				float num6 = 2f * (float5.x * float5.w + float5.y * float5.z);
				float num7 = math.csum(float5 * float5 * math.float4(-1f, 1f, -1f, 1f));
				float4 = math.float3(math.atan2(num6, num7), math.asin(num), 0f);
			}
			return float4.yxz;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00010850 File Offset: 0x0000EA50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 EulerYZX(quaternion q)
		{
			float4 value = q.value;
			float4 @float = value * value.wwww * math.float4(2f);
			float4 float2 = value * value.yzxw * math.float4(2f);
			float4 float3 = value * value;
			float3 float4 = Unity.Mathematics.float3.zero;
			float num = float2.x - @float.z;
			if (num * num < 0.99999595f)
			{
				float num2 = float2.z + @float.y;
				float num3 = float3.x + float3.w - float3.z - float3.y;
				float num4 = float2.y + @float.x;
				float num5 = float3.y + float3.w - float3.x - float3.z;
				float4 = math.float3(math.atan2(num2, num3), -math.asin(num), math.atan2(num4, num5));
			}
			else
			{
				num = math.clamp(num, -1f, 1f);
				float4 float5 = math.float4(float2.x, @float.z, float2.y, @float.x);
				float num6 = 2f * (float5.x * float5.w + float5.y * float5.z);
				float num7 = math.csum(float5 * float5 * math.float4(-1f, 1f, -1f, 1f));
				float4 = math.float3(math.atan2(num6, num7), -math.asin(num), 0f);
			}
			return float4.zxy;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000109EC File Offset: 0x0000EBEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 EulerZXY(quaternion q)
		{
			float4 value = q.value;
			float4 @float = value * value.wwww * math.float4(2f);
			float4 float2 = value * value.yzxw * math.float4(2f);
			float4 float3 = value * value;
			float3 float4 = Unity.Mathematics.float3.zero;
			float num = float2.y - @float.x;
			if (num * num < 0.99999595f)
			{
				float num2 = float2.x + @float.z;
				float num3 = float3.y + float3.w - float3.x - float3.z;
				float num4 = float2.z + @float.y;
				float num5 = float3.z + float3.w - float3.x - float3.y;
				float4 = math.float3(math.atan2(num2, num3), -math.asin(num), math.atan2(num4, num5));
			}
			else
			{
				num = math.clamp(num, -1f, 1f);
				float4 float5 = math.float4(float2.z, @float.y, float2.y, @float.x);
				float num6 = 2f * (float5.x * float5.w + float5.y * float5.z);
				float num7 = math.csum(float5 * float5 * math.float4(-1f, 1f, -1f, 1f));
				float4 = math.float3(math.atan2(num6, num7), -math.asin(num), 0f);
			}
			return float4.yzx;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00010B88 File Offset: 0x0000ED88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 EulerZYX(quaternion q)
		{
			float4 value = q.value;
			float4 @float = value * value.wwww * math.float4(2f);
			float4 float2 = value * value.yzxw * math.float4(2f);
			float4 float3 = value * value;
			float3 float4 = Unity.Mathematics.float3.zero;
			float num = float2.z + @float.y;
			if (num * num < 0.99999595f)
			{
				float num2 = -float2.x + @float.z;
				float num3 = float3.x + float3.w - float3.y - float3.z;
				float num4 = -float2.y + @float.x;
				float num5 = float3.z + float3.w - float3.y - float3.x;
				float4 = math.float3(math.atan2(num2, num3), math.asin(num), math.atan2(num4, num5));
			}
			else
			{
				num = math.clamp(num, -1f, 1f);
				float4 float5 = math.float4(float2.z, @float.y, float2.y, @float.x);
				float num6 = 2f * (float5.x * float5.w + float5.y * float5.z);
				float num7 = math.csum(float5 * float5 * math.float4(-1f, 1f, -1f, 1f));
				float4 = math.float3(math.atan2(num6, num7), math.asin(num), 0f);
			}
			return float4.zyx;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00010D24 File Offset: 0x0000EF24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 Euler(quaternion q, math.RotationOrder order = math.RotationOrder.ZXY)
		{
			switch (order)
			{
			case math.RotationOrder.XYZ:
				return math.EulerXYZ(q);
			case math.RotationOrder.XZY:
				return math.EulerXZY(q);
			case math.RotationOrder.YXZ:
				return math.EulerYXZ(q);
			case math.RotationOrder.YZX:
				return math.EulerYZX(q);
			case math.RotationOrder.ZXY:
				return math.EulerZXY(q);
			case math.RotationOrder.ZYX:
				return math.EulerZYX(q);
			default:
				return Unity.Mathematics.float3.zero;
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00010D80 File Offset: 0x0000EF80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 mulScale(float3x3 m, float3 s)
		{
			return new float3x3(m.c0 * s.x, m.c1 * s.y, m.c2 * s.z);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00010DBA File Offset: 0x0000EFBA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 scaleMul(float3 s, float3x3 m)
		{
			return new float3x3(m.c0 * s, m.c1 * s, m.c2 * s);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00010DE5 File Offset: 0x0000EFE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float4 unpacklo(float4 a, float4 b)
		{
			return math.shuffle(a, b, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightY);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00010DF2 File Offset: 0x0000EFF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double4 unpacklo(double4 a, double4 b)
		{
			return math.shuffle(a, b, math.ShuffleComponent.LeftX, math.ShuffleComponent.RightX, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightY);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00010DFF File Offset: 0x0000EFFF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float4 unpackhi(float4 a, float4 b)
		{
			return math.shuffle(a, b, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00010E0C File Offset: 0x0000F00C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double4 unpackhi(double4 a, double4 b)
		{
			return math.shuffle(a, b, math.ShuffleComponent.LeftZ, math.ShuffleComponent.RightZ, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightW);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00010E19 File Offset: 0x0000F019
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float4 movelh(float4 a, float4 b)
		{
			return math.shuffle(a, b, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightX, math.ShuffleComponent.RightY);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00010E26 File Offset: 0x0000F026
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double4 movelh(double4 a, double4 b)
		{
			return math.shuffle(a, b, math.ShuffleComponent.LeftX, math.ShuffleComponent.LeftY, math.ShuffleComponent.RightX, math.ShuffleComponent.RightY);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00010E33 File Offset: 0x0000F033
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float4 movehl(float4 a, float4 b)
		{
			return math.shuffle(b, a, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightW);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00010E40 File Offset: 0x0000F040
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double4 movehl(double4 a, double4 b)
		{
			return math.shuffle(b, a, math.ShuffleComponent.LeftZ, math.ShuffleComponent.LeftW, math.ShuffleComponent.RightZ, math.ShuffleComponent.RightW);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00010E50 File Offset: 0x0000F050
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint fold_to_uint(double x)
		{
			math.LongDoubleUnion longDoubleUnion;
			longDoubleUnion.longValue = 0L;
			longDoubleUnion.doubleValue = x;
			return (uint)(longDoubleUnion.longValue >> 32) ^ (uint)longDoubleUnion.longValue;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00010E80 File Offset: 0x0000F080
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint2 fold_to_uint(double2 x)
		{
			return math.uint2(math.fold_to_uint(x.x), math.fold_to_uint(x.y));
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00010E9D File Offset: 0x0000F09D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint3 fold_to_uint(double3 x)
		{
			return math.uint3(math.fold_to_uint(x.x), math.fold_to_uint(x.y), math.fold_to_uint(x.z));
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00010EC5 File Offset: 0x0000F0C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint4 fold_to_uint(double4 x)
		{
			return math.uint4(math.fold_to_uint(x.x), math.fold_to_uint(x.y), math.fold_to_uint(x.z), math.fold_to_uint(x.w));
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00010EF8 File Offset: 0x0000F0F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(float4x4 f4x4)
		{
			return new float3x3(f4x4);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00010F00 File Offset: 0x0000F100
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 float3x3(quaternion rotation)
		{
			return new float3x3(rotation);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00010F08 File Offset: 0x0000F108
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(float3x3 rotation, float3 translation)
		{
			return new float4x4(rotation, translation);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00010F11 File Offset: 0x0000F111
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(quaternion rotation, float3 translation)
		{
			return new float4x4(rotation, translation);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00010F1A File Offset: 0x0000F11A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 float4x4(RigidTransform transform)
		{
			return new float4x4(transform);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00010F24 File Offset: 0x0000F124
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 orthonormalize(float3x3 i)
		{
			float3 c = i.c0;
			float3 @float = i.c1 - i.c0 * math.dot(i.c1, i.c0);
			float num = math.length(c);
			float num2 = math.length(@float);
			bool flag = num > 1E-30f && num2 > 1E-30f;
			float3x3 float3x;
			float3x.c0 = math.select(math.float3(1f, 0f, 0f), c / num, flag);
			float3x.c1 = math.select(math.float3(0f, 1f, 0f), @float / num2, flag);
			float3x.c2 = math.cross(float3x.c0, float3x.c1);
			return float3x;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 pseudoinverse(float3x3 m)
		{
			float num = 0.333333f * (math.lengthsq(m.c0) + math.lengthsq(m.c1) + math.lengthsq(m.c2));
			if (num < 1E-30f)
			{
				return Unity.Mathematics.float3x3.zero;
			}
			float3 @float = math.rsqrt(num);
			float3x3 float3x = math.mulScale(m, @float);
			float3x3 float3x2;
			if (!math.adjInverse(float3x, out float3x2, 1E-06f))
			{
				float3x2 = svd.svdInverse(float3x);
			}
			return math.mulScale(float3x2, @float);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001106A File Offset: 0x0000F26A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float mul(float a, float b)
		{
			return a * b;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001106F File Offset: 0x0000F26F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float mul(float2 a, float2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001108C File Offset: 0x0000F28C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 mul(float2 a, float2x2 b)
		{
			return math.float2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000110E8 File Offset: 0x0000F2E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mul(float2 a, float2x3 b)
		{
			return math.float3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001116C File Offset: 0x0000F36C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(float2 a, float2x4 b)
		{
			return math.float4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00011212 File Offset: 0x0000F412
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float mul(float3 a, float3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00011240 File Offset: 0x0000F440
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 mul(float3 a, float3x2 b)
		{
			return math.float2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000112C4 File Offset: 0x0000F4C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mul(float3 a, float3x3 b)
		{
			return math.float3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00011380 File Offset: 0x0000F580
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(float3 a, float3x4 b)
		{
			return math.float4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00011472 File Offset: 0x0000F672
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float mul(float4 a, float4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000114AC File Offset: 0x0000F6AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 mul(float4 a, float4x2 b)
		{
			return math.float2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00011554 File Offset: 0x0000F754
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mul(float4 a, float4x3 b)
		{
			return math.float3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00011648 File Offset: 0x0000F848
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(float4 a, float4x4 b)
		{
			return math.float4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00011786 File Offset: 0x0000F986
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 mul(float2x2 a, float2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000117B0 File Offset: 0x0000F9B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 mul(float2x2 a, float2x2 b)
		{
			return math.float2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00011824 File Offset: 0x0000FA24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 mul(float2x2 a, float2x3 b)
		{
			return math.float2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000118CC File Offset: 0x0000FACC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 mul(float2x2 a, float2x4 b)
		{
			return math.float2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000119A2 File Offset: 0x0000FBA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 mul(float2x3 a, float3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000119E4 File Offset: 0x0000FBE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 mul(float2x3 a, float3x2 b)
		{
			return math.float2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00011A90 File Offset: 0x0000FC90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 mul(float2x3 a, float3x3 b)
		{
			return math.float2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00011B88 File Offset: 0x0000FD88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 mul(float2x3 a, float3x4 b)
		{
			return math.float2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00011CCC File Offset: 0x0000FECC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 mul(float2x4 a, float4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00011D2C File Offset: 0x0000FF2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x2 mul(float2x4 a, float4x2 b)
		{
			return math.float2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00011E0C File Offset: 0x0001000C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x3 mul(float2x4 a, float4x3 b)
		{
			return math.float2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00011F54 File Offset: 0x00010154
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2x4 mul(float2x4 a, float4x4 b)
		{
			return math.float2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00012102 File Offset: 0x00010302
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mul(float3x2 a, float2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001212C File Offset: 0x0001032C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 mul(float3x2 a, float2x2 b)
		{
			return math.float3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x000121A0 File Offset: 0x000103A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 mul(float3x2 a, float2x3 b)
		{
			return math.float3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00012248 File Offset: 0x00010448
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 mul(float3x2 a, float2x4 b)
		{
			return math.float3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001231E File Offset: 0x0001051E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mul(float3x3 a, float3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00012360 File Offset: 0x00010560
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 mul(float3x3 a, float3x2 b)
		{
			return math.float3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001240C File Offset: 0x0001060C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 mul(float3x3 a, float3x3 b)
		{
			return math.float3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00012504 File Offset: 0x00010704
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 mul(float3x3 a, float3x4 b)
		{
			return math.float3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00012648 File Offset: 0x00010848
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mul(float3x4 a, float4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000126A8 File Offset: 0x000108A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x2 mul(float3x4 a, float4x2 b)
		{
			return math.float3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00012788 File Offset: 0x00010988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 mul(float3x4 a, float4x3 b)
		{
			return math.float3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000128D0 File Offset: 0x00010AD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x4 mul(float3x4 a, float4x4 b)
		{
			return math.float3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00012A7E File Offset: 0x00010C7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(float4x2 a, float2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00012AA8 File Offset: 0x00010CA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 mul(float4x2 a, float2x2 b)
		{
			return math.float4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00012B1C File Offset: 0x00010D1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 mul(float4x2 a, float2x3 b)
		{
			return math.float4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00012BC4 File Offset: 0x00010DC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 mul(float4x2 a, float2x4 b)
		{
			return math.float4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00012C9A File Offset: 0x00010E9A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(float4x3 a, float3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00012CDC File Offset: 0x00010EDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 mul(float4x3 a, float3x2 b)
		{
			return math.float4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00012D88 File Offset: 0x00010F88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 mul(float4x3 a, float3x3 b)
		{
			return math.float4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00012E80 File Offset: 0x00011080
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 mul(float4x3 a, float3x4 b)
		{
			return math.float4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00012FC4 File Offset: 0x000111C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(float4x4 a, float4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00013024 File Offset: 0x00011224
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x2 mul(float4x4 a, float4x2 b)
		{
			return math.float4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00013104 File Offset: 0x00011304
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x3 mul(float4x4 a, float4x3 b)
		{
			return math.float4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001324C File Offset: 0x0001144C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 mul(float4x4 a, float4x4 b)
		{
			return math.float4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000133FA File Offset: 0x000115FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double mul(double a, double b)
		{
			return a * b;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000133FF File Offset: 0x000115FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double mul(double2 a, double2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001341C File Offset: 0x0001161C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 mul(double2 a, double2x2 b)
		{
			return math.double2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00013478 File Offset: 0x00011678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mul(double2 a, double2x3 b)
		{
			return math.double3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000134FC File Offset: 0x000116FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 mul(double2 a, double2x4 b)
		{
			return math.double4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000135A2 File Offset: 0x000117A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double mul(double3 a, double3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000135D0 File Offset: 0x000117D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 mul(double3 a, double3x2 b)
		{
			return math.double2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00013654 File Offset: 0x00011854
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mul(double3 a, double3x3 b)
		{
			return math.double3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00013710 File Offset: 0x00011910
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 mul(double3 a, double3x4 b)
		{
			return math.double4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00013802 File Offset: 0x00011A02
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double mul(double4 a, double4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001383C File Offset: 0x00011A3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 mul(double4 a, double4x2 b)
		{
			return math.double2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x000138E4 File Offset: 0x00011AE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mul(double4 a, double4x3 b)
		{
			return math.double3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000139D8 File Offset: 0x00011BD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 mul(double4 a, double4x4 b)
		{
			return math.double4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00013B16 File Offset: 0x00011D16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 mul(double2x2 a, double2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00013B40 File Offset: 0x00011D40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 mul(double2x2 a, double2x2 b)
		{
			return math.double2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00013BB4 File Offset: 0x00011DB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 mul(double2x2 a, double2x3 b)
		{
			return math.double2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00013C5C File Offset: 0x00011E5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 mul(double2x2 a, double2x4 b)
		{
			return math.double2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00013D32 File Offset: 0x00011F32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 mul(double2x3 a, double3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00013D74 File Offset: 0x00011F74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 mul(double2x3 a, double3x2 b)
		{
			return math.double2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00013E20 File Offset: 0x00012020
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 mul(double2x3 a, double3x3 b)
		{
			return math.double2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00013F18 File Offset: 0x00012118
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 mul(double2x3 a, double3x4 b)
		{
			return math.double2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001405C File Offset: 0x0001225C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2 mul(double2x4 a, double4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000140BC File Offset: 0x000122BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x2 mul(double2x4 a, double4x2 b)
		{
			return math.double2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001419C File Offset: 0x0001239C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x3 mul(double2x4 a, double4x3 b)
		{
			return math.double2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x000142E4 File Offset: 0x000124E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double2x4 mul(double2x4 a, double4x4 b)
		{
			return math.double2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00014492 File Offset: 0x00012692
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mul(double3x2 a, double2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000144BC File Offset: 0x000126BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 mul(double3x2 a, double2x2 b)
		{
			return math.double3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00014530 File Offset: 0x00012730
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 mul(double3x2 a, double2x3 b)
		{
			return math.double3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000145D8 File Offset: 0x000127D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 mul(double3x2 a, double2x4 b)
		{
			return math.double3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000146AE File Offset: 0x000128AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mul(double3x3 a, double3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000146F0 File Offset: 0x000128F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 mul(double3x3 a, double3x2 b)
		{
			return math.double3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001479C File Offset: 0x0001299C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 mul(double3x3 a, double3x3 b)
		{
			return math.double3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00014894 File Offset: 0x00012A94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 mul(double3x3 a, double3x4 b)
		{
			return math.double3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000149D8 File Offset: 0x00012BD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3 mul(double3x4 a, double4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00014A38 File Offset: 0x00012C38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x2 mul(double3x4 a, double4x2 b)
		{
			return math.double3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00014B18 File Offset: 0x00012D18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x3 mul(double3x4 a, double4x3 b)
		{
			return math.double3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00014C60 File Offset: 0x00012E60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double3x4 mul(double3x4 a, double4x4 b)
		{
			return math.double3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00014E0E File Offset: 0x0001300E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 mul(double4x2 a, double2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00014E38 File Offset: 0x00013038
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 mul(double4x2 a, double2x2 b)
		{
			return math.double4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00014EAC File Offset: 0x000130AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 mul(double4x2 a, double2x3 b)
		{
			return math.double4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00014F54 File Offset: 0x00013154
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 mul(double4x2 a, double2x4 b)
		{
			return math.double4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001502A File Offset: 0x0001322A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 mul(double4x3 a, double3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001506C File Offset: 0x0001326C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 mul(double4x3 a, double3x2 b)
		{
			return math.double4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00015118 File Offset: 0x00013318
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 mul(double4x3 a, double3x3 b)
		{
			return math.double4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00015210 File Offset: 0x00013410
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 mul(double4x3 a, double3x4 b)
		{
			return math.double4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00015354 File Offset: 0x00013554
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4 mul(double4x4 a, double4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000153B4 File Offset: 0x000135B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x2 mul(double4x4 a, double4x2 b)
		{
			return math.double4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00015494 File Offset: 0x00013694
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x3 mul(double4x4 a, double4x3 b)
		{
			return math.double4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000155DC File Offset: 0x000137DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double4x4 mul(double4x4 a, double4x4 b)
		{
			return math.double4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001578A File Offset: 0x0001398A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int mul(int a, int b)
		{
			return a * b;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001578F File Offset: 0x0001398F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int mul(int2 a, int2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000157AC File Offset: 0x000139AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 mul(int2 a, int2x2 b)
		{
			return math.int2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00015808 File Offset: 0x00013A08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 mul(int2 a, int2x3 b)
		{
			return math.int3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001588C File Offset: 0x00013A8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 mul(int2 a, int2x4 b)
		{
			return math.int4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00015932 File Offset: 0x00013B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int mul(int3 a, int3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00015960 File Offset: 0x00013B60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 mul(int3 a, int3x2 b)
		{
			return math.int2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000159E4 File Offset: 0x00013BE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 mul(int3 a, int3x3 b)
		{
			return math.int3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00015AA0 File Offset: 0x00013CA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 mul(int3 a, int3x4 b)
		{
			return math.int4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00015B92 File Offset: 0x00013D92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int mul(int4 a, int4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00015BCC File Offset: 0x00013DCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 mul(int4 a, int4x2 b)
		{
			return math.int2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00015C74 File Offset: 0x00013E74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 mul(int4 a, int4x3 b)
		{
			return math.int3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00015D68 File Offset: 0x00013F68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 mul(int4 a, int4x4 b)
		{
			return math.int4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00015EA6 File Offset: 0x000140A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 mul(int2x2 a, int2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00015ED0 File Offset: 0x000140D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 mul(int2x2 a, int2x2 b)
		{
			return math.int2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00015F44 File Offset: 0x00014144
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 mul(int2x2 a, int2x3 b)
		{
			return math.int2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00015FEC File Offset: 0x000141EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 mul(int2x2 a, int2x4 b)
		{
			return math.int2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000160C2 File Offset: 0x000142C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 mul(int2x3 a, int3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00016104 File Offset: 0x00014304
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 mul(int2x3 a, int3x2 b)
		{
			return math.int2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x000161B0 File Offset: 0x000143B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 mul(int2x3 a, int3x3 b)
		{
			return math.int2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000162A8 File Offset: 0x000144A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 mul(int2x3 a, int3x4 b)
		{
			return math.int2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000163EC File Offset: 0x000145EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 mul(int2x4 a, int4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001644C File Offset: 0x0001464C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x2 mul(int2x4 a, int4x2 b)
		{
			return math.int2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001652C File Offset: 0x0001472C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x3 mul(int2x4 a, int4x3 b)
		{
			return math.int2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00016674 File Offset: 0x00014874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 mul(int2x4 a, int4x4 b)
		{
			return math.int2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00016822 File Offset: 0x00014A22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 mul(int3x2 a, int2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001684C File Offset: 0x00014A4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 mul(int3x2 a, int2x2 b)
		{
			return math.int3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000168C0 File Offset: 0x00014AC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 mul(int3x2 a, int2x3 b)
		{
			return math.int3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00016968 File Offset: 0x00014B68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 mul(int3x2 a, int2x4 b)
		{
			return math.int3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00016A3E File Offset: 0x00014C3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 mul(int3x3 a, int3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00016A80 File Offset: 0x00014C80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 mul(int3x3 a, int3x2 b)
		{
			return math.int3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00016B2C File Offset: 0x00014D2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 mul(int3x3 a, int3x3 b)
		{
			return math.int3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00016C24 File Offset: 0x00014E24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 mul(int3x3 a, int3x4 b)
		{
			return math.int3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00016D68 File Offset: 0x00014F68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 mul(int3x4 a, int4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00016DC8 File Offset: 0x00014FC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x2 mul(int3x4 a, int4x2 b)
		{
			return math.int3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00016EA8 File Offset: 0x000150A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x3 mul(int3x4 a, int4x3 b)
		{
			return math.int3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00016FF0 File Offset: 0x000151F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3x4 mul(int3x4 a, int4x4 b)
		{
			return math.int3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001719E File Offset: 0x0001539E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 mul(int4x2 a, int2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000171C8 File Offset: 0x000153C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 mul(int4x2 a, int2x2 b)
		{
			return math.int4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001723C File Offset: 0x0001543C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 mul(int4x2 a, int2x3 b)
		{
			return math.int4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000172E4 File Offset: 0x000154E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 mul(int4x2 a, int2x4 b)
		{
			return math.int4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000173BA File Offset: 0x000155BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 mul(int4x3 a, int3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000173FC File Offset: 0x000155FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 mul(int4x3 a, int3x2 b)
		{
			return math.int4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000174A8 File Offset: 0x000156A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 mul(int4x3 a, int3x3 b)
		{
			return math.int4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000175A0 File Offset: 0x000157A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 mul(int4x3 a, int3x4 b)
		{
			return math.int4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000176E4 File Offset: 0x000158E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 mul(int4x4 a, int4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00017744 File Offset: 0x00015944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 mul(int4x4 a, int4x2 b)
		{
			return math.int4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00017824 File Offset: 0x00015A24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 mul(int4x4 a, int4x3 b)
		{
			return math.int4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001796C File Offset: 0x00015B6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 mul(int4x4 a, int4x4 b)
		{
			return math.int4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00017B1A File Offset: 0x00015D1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint mul(uint a, uint b)
		{
			return a * b;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00017B1F File Offset: 0x00015D1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint mul(uint2 a, uint2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00017B3C File Offset: 0x00015D3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 mul(uint2 a, uint2x2 b)
		{
			return math.uint2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00017B98 File Offset: 0x00015D98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 mul(uint2 a, uint2x3 b)
		{
			return math.uint3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00017C1C File Offset: 0x00015E1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 mul(uint2 a, uint2x4 b)
		{
			return math.uint4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00017CC2 File Offset: 0x00015EC2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint mul(uint3 a, uint3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00017CF0 File Offset: 0x00015EF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 mul(uint3 a, uint3x2 b)
		{
			return math.uint2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00017D74 File Offset: 0x00015F74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 mul(uint3 a, uint3x3 b)
		{
			return math.uint3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00017E30 File Offset: 0x00016030
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 mul(uint3 a, uint3x4 b)
		{
			return math.uint4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00017F22 File Offset: 0x00016122
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint mul(uint4 a, uint4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00017F5C File Offset: 0x0001615C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 mul(uint4 a, uint4x2 b)
		{
			return math.uint2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00018004 File Offset: 0x00016204
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 mul(uint4 a, uint4x3 b)
		{
			return math.uint3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000180F8 File Offset: 0x000162F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 mul(uint4 a, uint4x4 b)
		{
			return math.uint4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00018236 File Offset: 0x00016436
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 mul(uint2x2 a, uint2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00018260 File Offset: 0x00016460
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 mul(uint2x2 a, uint2x2 b)
		{
			return math.uint2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000182D4 File Offset: 0x000164D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 mul(uint2x2 a, uint2x3 b)
		{
			return math.uint2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001837C File Offset: 0x0001657C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 mul(uint2x2 a, uint2x4 b)
		{
			return math.uint2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00018452 File Offset: 0x00016652
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 mul(uint2x3 a, uint3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00018494 File Offset: 0x00016694
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 mul(uint2x3 a, uint3x2 b)
		{
			return math.uint2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00018540 File Offset: 0x00016740
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 mul(uint2x3 a, uint3x3 b)
		{
			return math.uint2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00018638 File Offset: 0x00016838
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 mul(uint2x3 a, uint3x4 b)
		{
			return math.uint2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001877C File Offset: 0x0001697C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 mul(uint2x4 a, uint4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000187DC File Offset: 0x000169DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 mul(uint2x4 a, uint4x2 b)
		{
			return math.uint2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000188BC File Offset: 0x00016ABC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 mul(uint2x4 a, uint4x3 b)
		{
			return math.uint2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00018A04 File Offset: 0x00016C04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 mul(uint2x4 a, uint4x4 b)
		{
			return math.uint2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00018BB2 File Offset: 0x00016DB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 mul(uint3x2 a, uint2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00018BDC File Offset: 0x00016DDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 mul(uint3x2 a, uint2x2 b)
		{
			return math.uint3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00018C50 File Offset: 0x00016E50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 mul(uint3x2 a, uint2x3 b)
		{
			return math.uint3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00018CF8 File Offset: 0x00016EF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 mul(uint3x2 a, uint2x4 b)
		{
			return math.uint3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00018DCE File Offset: 0x00016FCE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 mul(uint3x3 a, uint3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00018E10 File Offset: 0x00017010
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 mul(uint3x3 a, uint3x2 b)
		{
			return math.uint3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00018EBC File Offset: 0x000170BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 mul(uint3x3 a, uint3x3 b)
		{
			return math.uint3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00018FB4 File Offset: 0x000171B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 mul(uint3x3 a, uint3x4 b)
		{
			return math.uint3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000190F8 File Offset: 0x000172F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 mul(uint3x4 a, uint4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00019158 File Offset: 0x00017358
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 mul(uint3x4 a, uint4x2 b)
		{
			return math.uint3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00019238 File Offset: 0x00017438
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 mul(uint3x4 a, uint4x3 b)
		{
			return math.uint3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00019380 File Offset: 0x00017580
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 mul(uint3x4 a, uint4x4 b)
		{
			return math.uint3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001952E File Offset: 0x0001772E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 mul(uint4x2 a, uint2 b)
		{
			return a.c0 * b.x + a.c1 * b.y;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00019558 File Offset: 0x00017758
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 mul(uint4x2 a, uint2x2 b)
		{
			return math.uint4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000195CC File Offset: 0x000177CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 mul(uint4x2 a, uint2x3 b)
		{
			return math.uint4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00019674 File Offset: 0x00017874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 mul(uint4x2 a, uint2x4 b)
		{
			return math.uint4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001974A File Offset: 0x0001794A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 mul(uint4x3 a, uint3 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001978C File Offset: 0x0001798C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 mul(uint4x3 a, uint3x2 b)
		{
			return math.uint4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00019838 File Offset: 0x00017A38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 mul(uint4x3 a, uint3x3 b)
		{
			return math.uint4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00019930 File Offset: 0x00017B30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 mul(uint4x3 a, uint3x4 b)
		{
			return math.uint4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00019A74 File Offset: 0x00017C74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 mul(uint4x4 a, uint4 b)
		{
			return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00019AD4 File Offset: 0x00017CD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 mul(uint4x4 a, uint4x2 b)
		{
			return math.uint4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00019BB4 File Offset: 0x00017DB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 mul(uint4x4 a, uint4x3 b)
		{
			return math.uint4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00019CFC File Offset: 0x00017EFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 mul(uint4x4 a, uint4x4 b)
		{
			return math.uint4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00019EAA File Offset: 0x000180AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion quaternion(float x, float y, float z, float w)
		{
			return new quaternion(x, y, z, w);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00019EB5 File Offset: 0x000180B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion quaternion(float4 value)
		{
			return new quaternion(value);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00019EBD File Offset: 0x000180BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion quaternion(float3x3 m)
		{
			return new quaternion(m);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00019EC5 File Offset: 0x000180C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion quaternion(float4x4 m)
		{
			return new quaternion(m);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00019ECD File Offset: 0x000180CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion conjugate(quaternion q)
		{
			return math.quaternion(q.value * math.float4(-1f, -1f, -1f, 1f));
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00019EF8 File Offset: 0x000180F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion inverse(quaternion q)
		{
			float4 value = q.value;
			return math.quaternion(math.rcp(math.dot(value, value)) * value * math.float4(-1f, -1f, -1f, 1f));
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00019F41 File Offset: 0x00018141
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float dot(quaternion a, quaternion b)
		{
			return math.dot(a.value, b.value);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00019F54 File Offset: 0x00018154
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float length(quaternion q)
		{
			return math.sqrt(math.dot(q.value, q.value));
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00019F6C File Offset: 0x0001816C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float lengthsq(quaternion q)
		{
			return math.dot(q.value, q.value);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00019F80 File Offset: 0x00018180
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion normalize(quaternion q)
		{
			float4 value = q.value;
			return math.quaternion(math.rsqrt(math.dot(value, value)) * value);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00019FAC File Offset: 0x000181AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion normalizesafe(quaternion q)
		{
			float4 value = q.value;
			float num = math.dot(value, value);
			return math.quaternion(math.select(Unity.Mathematics.quaternion.identity.value, value * math.rsqrt(num), num > 1.1754944E-38f));
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00019FF0 File Offset: 0x000181F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion normalizesafe(quaternion q, quaternion defaultvalue)
		{
			float4 value = q.value;
			float num = math.dot(value, value);
			return math.quaternion(math.select(defaultvalue.value, value * math.rsqrt(num), num > 1.1754944E-38f));
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001A030 File Offset: 0x00018230
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion unitexp(quaternion q)
		{
			float num = math.rsqrt(math.dot(q.value.xyz, q.value.xyz));
			float num2;
			float num3;
			math.sincos(math.rcp(num), out num2, out num3);
			return math.quaternion(math.float4(q.value.xyz * num * num2, num3));
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001A094 File Offset: 0x00018294
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion exp(quaternion q)
		{
			float num = math.rsqrt(math.dot(q.value.xyz, q.value.xyz));
			float num2;
			float num3;
			math.sincos(math.rcp(num), out num2, out num3);
			return math.quaternion(math.float4(q.value.xyz * num * num2, num3) * math.exp(q.value.w));
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001A10C File Offset: 0x0001830C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion unitlog(quaternion q)
		{
			float num = math.clamp(q.value.w, -1f, 1f);
			float num2 = math.acos(num) * math.rsqrt(1f - num * num);
			return math.quaternion(math.float4(q.value.xyz * num2, 0f));
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001A16C File Offset: 0x0001836C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion log(quaternion q)
		{
			float num = math.dot(q.value.xyz, q.value.xyz);
			float num2 = num + q.value.w * q.value.w;
			float num3 = math.acos(math.clamp(q.value.w * math.rsqrt(num2), -1f, 1f)) * math.rsqrt(num);
			return math.quaternion(math.float4(q.value.xyz * num3, 0.5f * math.log(num2)));
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001A208 File Offset: 0x00018408
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion mul(quaternion a, quaternion b)
		{
			return math.quaternion(a.value.wwww * b.value + (a.value.xyzx * b.value.wwwx + a.value.yzxy * b.value.zxyy) * math.float4(1f, 1f, 1f, -1f) - a.value.zxyz * b.value.yzxz);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001A2B8 File Offset: 0x000184B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 mul(quaternion q, float3 v)
		{
			float3 @float = 2f * math.cross(q.value.xyz, v);
			return v + q.value.w * @float + math.cross(q.value.xyz, @float);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001A310 File Offset: 0x00018510
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 rotate(quaternion q, float3 v)
		{
			float3 @float = 2f * math.cross(q.value.xyz, v);
			return v + q.value.w * @float + math.cross(q.value.xyz, @float);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001A368 File Offset: 0x00018568
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion nlerp(quaternion q1, quaternion q2, float t)
		{
			return math.normalize(q1.value + t * (math.chgsign(q2.value, math.dot(q1, q2)) - q1.value));
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001A3A8 File Offset: 0x000185A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion slerp(quaternion q1, quaternion q2, float t)
		{
			float num = math.dot(q1, q2);
			if (num < 0f)
			{
				num = -num;
				q2.value = -q2.value;
			}
			if (num < 0.9995f)
			{
				float num2 = math.acos(num);
				float num3 = math.rsqrt(1f - num * num);
				float num4 = math.sin(num2 * (1f - t)) * num3;
				float num5 = math.sin(num2 * t) * num3;
				return math.quaternion(q1.value * num4 + q2.value * num5);
			}
			return math.nlerp(q1, q2, t);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001A440 File Offset: 0x00018640
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float angle(quaternion q1, quaternion q2)
		{
			float num = math.asin(math.length(math.normalize(math.mul(math.conjugate(q1), q2)).value.xyz));
			return num + num;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001A478 File Offset: 0x00018678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion rotation(float3x3 m)
		{
			float num = math.determinant(m);
			if (math.abs(1f - num) < 1E-06f)
			{
				return math.quaternion(m);
			}
			if (math.abs(num) > 1E-06f)
			{
				float3x3 float3x = math.mulScale(m, math.rsqrt(math.float3(math.lengthsq(m.c0), math.lengthsq(m.c1), math.lengthsq(m.c2))));
				if (math.abs(1f - math.determinant(float3x)) < 1E-06f)
				{
					return math.quaternion(float3x);
				}
			}
			return svd.svdRotation(m);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001A50C File Offset: 0x0001870C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float3x3 adj(float3x3 m, out float det)
		{
			float3x3 float3x;
			float3x.c0 = math.cross(m.c1, m.c2);
			float3x.c1 = math.cross(m.c2, m.c0);
			float3x.c2 = math.cross(m.c0, m.c1);
			det = math.dot(m.c0, float3x.c0);
			return math.transpose(float3x);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001A57C File Offset: 0x0001877C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool adjInverse(float3x3 m, out float3x3 i, float epsilon = 1E-30f)
		{
			float num;
			i = math.adj(m, out num);
			bool flag = math.abs(num) > epsilon;
			float3 @float = math.select(math.float3(1f), math.rcp(num), flag);
			i = math.scaleMul(@float, i);
			return flag;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001A5D0 File Offset: 0x000187D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(quaternion q)
		{
			return math.hash(q.value);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001A5DD File Offset: 0x000187DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(quaternion q)
		{
			return math.hashwide(q.value);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001A5EA File Offset: 0x000187EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 forward(quaternion q)
		{
			return math.mul(q, math.float3(0f, 0f, 1f));
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001A606 File Offset: 0x00018806
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform RigidTransform(quaternion rot, float3 pos)
		{
			return new RigidTransform(rot, pos);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001A60F File Offset: 0x0001880F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform RigidTransform(float3x3 rotation, float3 translation)
		{
			return new RigidTransform(rotation, translation);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001A618 File Offset: 0x00018818
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform RigidTransform(float4x4 transform)
		{
			return new RigidTransform(transform);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001A620 File Offset: 0x00018820
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform inverse(RigidTransform t)
		{
			quaternion quaternion = math.inverse(t.rot);
			float3 @float = math.mul(quaternion, -t.pos);
			return new RigidTransform(quaternion, @float);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001A650 File Offset: 0x00018850
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RigidTransform mul(RigidTransform a, RigidTransform b)
		{
			return new RigidTransform(math.mul(a.rot, b.rot), math.mul(a.rot, b.pos) + a.pos);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001A684 File Offset: 0x00018884
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 mul(RigidTransform a, float4 pos)
		{
			return math.float4(math.mul(a.rot, pos.xyz) + a.pos * pos.w, pos.w);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001A6B9 File Offset: 0x000188B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 rotate(RigidTransform a, float3 dir)
		{
			return math.mul(a.rot, dir);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001A6C7 File Offset: 0x000188C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 transform(RigidTransform a, float3 pos)
		{
			return math.mul(a.rot, pos) + a.pos;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001A6E0 File Offset: 0x000188E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(RigidTransform t)
		{
			return math.hash(t.rot) + 3318036811U * math.hash(t.pos);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001A700 File Offset: 0x00018900
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(RigidTransform t)
		{
			return math.hashwide(t.rot) + 3318036811U * math.hashwide(t.pos).xyzz;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001A73A File Offset: 0x0001893A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(uint x, uint y)
		{
			return new uint2(x, y);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001A743 File Offset: 0x00018943
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(uint2 xy)
		{
			return new uint2(xy);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001A74B File Offset: 0x0001894B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(uint v)
		{
			return new uint2(v);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001A753 File Offset: 0x00018953
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(bool v)
		{
			return new uint2(v);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001A75B File Offset: 0x0001895B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(bool2 v)
		{
			return new uint2(v);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001A763 File Offset: 0x00018963
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(int v)
		{
			return new uint2(v);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001A76B File Offset: 0x0001896B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(int2 v)
		{
			return new uint2(v);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001A773 File Offset: 0x00018973
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(float v)
		{
			return new uint2(v);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001A77B File Offset: 0x0001897B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(float2 v)
		{
			return new uint2(v);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001A783 File Offset: 0x00018983
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(double v)
		{
			return new uint2(v);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001A78B File Offset: 0x0001898B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 uint2(double2 v)
		{
			return new uint2(v);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001A793 File Offset: 0x00018993
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint2 v)
		{
			return math.csum(v * math.uint2(1148435377U, 3416333663U)) + 1750611407U;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001A7B5 File Offset: 0x000189B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(uint2 v)
		{
			return v * math.uint2(3285396193U, 3110507567U) + 4271396531U;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001A7D6 File Offset: 0x000189D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint shuffle(uint2 left, uint2 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001A7E0 File Offset: 0x000189E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 shuffle(uint2 left, uint2 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.uint2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001A7F7 File Offset: 0x000189F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 shuffle(uint2 left, uint2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.uint3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001A817 File Offset: 0x00018A17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 shuffle(uint2 left, uint2 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.uint4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001A840 File Offset: 0x00018A40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint select_shuffle_component(uint2 a, uint2 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001A8A5 File Offset: 0x00018AA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(uint2 c0, uint2 c1)
		{
			return new uint2x2(c0, c1);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001A8AE File Offset: 0x00018AAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(uint m00, uint m01, uint m10, uint m11)
		{
			return new uint2x2(m00, m01, m10, m11);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001A8B9 File Offset: 0x00018AB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(uint v)
		{
			return new uint2x2(v);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001A8C1 File Offset: 0x00018AC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(bool v)
		{
			return new uint2x2(v);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001A8C9 File Offset: 0x00018AC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(bool2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001A8D1 File Offset: 0x00018AD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(int v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001A8D9 File Offset: 0x00018AD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(int2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001A8E1 File Offset: 0x00018AE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(float v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001A8E9 File Offset: 0x00018AE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(float2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001A8F1 File Offset: 0x00018AF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(double v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001A8F9 File Offset: 0x00018AF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 uint2x2(double2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001A901 File Offset: 0x00018B01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 transpose(uint2x2 v)
		{
			return math.uint2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001A934 File Offset: 0x00018B34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint2x2 v)
		{
			return math.csum(v.c0 * math.uint2(3010324327U, 1875523709U) + v.c1 * math.uint2(2937008387U, 3835713223U)) + 2216526373U;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001A988 File Offset: 0x00018B88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(uint2x2 v)
		{
			return v.c0 * math.uint2(3375971453U, 3559829411U) + v.c1 * math.uint2(3652178029U, 2544260129U) + 2013864031U;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001A9D8 File Offset: 0x00018BD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(uint2 c0, uint2 c1, uint2 c2)
		{
			return new uint2x3(c0, c1, c2);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001A9E2 File Offset: 0x00018BE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12)
		{
			return new uint2x3(m00, m01, m02, m10, m11, m12);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001A9F1 File Offset: 0x00018BF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(uint v)
		{
			return new uint2x3(v);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001A9F9 File Offset: 0x00018BF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(bool v)
		{
			return new uint2x3(v);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001AA01 File Offset: 0x00018C01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(bool2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001AA09 File Offset: 0x00018C09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(int v)
		{
			return new uint2x3(v);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001AA11 File Offset: 0x00018C11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(int2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001AA19 File Offset: 0x00018C19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(float v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001AA21 File Offset: 0x00018C21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(float2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001AA29 File Offset: 0x00018C29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(double v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001AA31 File Offset: 0x00018C31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 uint2x3(double2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001AA3C File Offset: 0x00018C3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 transpose(uint2x3 v)
		{
			return math.uint3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001AA90 File Offset: 0x00018C90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint2x3 v)
		{
			return math.csum(v.c0 * math.uint2(4016293529U, 2416021567U) + v.c1 * math.uint2(2828384717U, 2636362241U) + v.c2 * math.uint2(1258410977U, 1952565773U)) + 2037535609U;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001AB00 File Offset: 0x00018D00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(uint2x3 v)
		{
			return v.c0 * math.uint2(3592785499U, 3996716183U) + v.c1 * math.uint2(2626301701U, 1306289417U) + v.c2 * math.uint2(2096137163U, 1548578029U) + 4178800919U;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001AB6F File Offset: 0x00018D6F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(uint2 c0, uint2 c1, uint2 c2, uint2 c3)
		{
			return new uint2x4(c0, c1, c2, c3);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001AB7A File Offset: 0x00018D7A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13)
		{
			return new uint2x4(m00, m01, m02, m03, m10, m11, m12, m13);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001AB8D File Offset: 0x00018D8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(uint v)
		{
			return new uint2x4(v);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001AB95 File Offset: 0x00018D95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(bool v)
		{
			return new uint2x4(v);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001AB9D File Offset: 0x00018D9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(bool2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001ABA5 File Offset: 0x00018DA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(int v)
		{
			return new uint2x4(v);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001ABAD File Offset: 0x00018DAD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(int2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001ABB5 File Offset: 0x00018DB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(float v)
		{
			return new uint2x4(v);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001ABBD File Offset: 0x00018DBD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(float2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001ABC5 File Offset: 0x00018DC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(double v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001ABCD File Offset: 0x00018DCD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 uint2x4(double2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001ABD8 File Offset: 0x00018DD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 transpose(uint2x4 v)
		{
			return math.uint4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001AC44 File Offset: 0x00018E44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint2x4 v)
		{
			return math.csum(v.c0 * math.uint2(2650080659U, 4052675461U) + v.c1 * math.uint2(2652487619U, 2174136431U) + v.c2 * math.uint2(3528391193U, 2105559227U) + v.c3 * math.uint2(1899745391U, 1966790317U)) + 3516359879U;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001ACD4 File Offset: 0x00018ED4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 hashwide(uint2x4 v)
		{
			return v.c0 * math.uint2(3050356579U, 4178586719U) + v.c1 * math.uint2(2558655391U, 1453413133U) + v.c2 * math.uint2(2152428077U, 1938706661U) + v.c3 * math.uint2(1338588197U, 3439609253U) + 3535343003U;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001AD62 File Offset: 0x00018F62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(uint x, uint y, uint z)
		{
			return new uint3(x, y, z);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001AD6C File Offset: 0x00018F6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(uint x, uint2 yz)
		{
			return new uint3(x, yz);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001AD75 File Offset: 0x00018F75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(uint2 xy, uint z)
		{
			return new uint3(xy, z);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001AD7E File Offset: 0x00018F7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(uint3 xyz)
		{
			return new uint3(xyz);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001AD86 File Offset: 0x00018F86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(uint v)
		{
			return new uint3(v);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001AD8E File Offset: 0x00018F8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(bool v)
		{
			return new uint3(v);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001AD96 File Offset: 0x00018F96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(bool3 v)
		{
			return new uint3(v);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001AD9E File Offset: 0x00018F9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(int v)
		{
			return new uint3(v);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001ADA6 File Offset: 0x00018FA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(int3 v)
		{
			return new uint3(v);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001ADAE File Offset: 0x00018FAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(float v)
		{
			return new uint3(v);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001ADB6 File Offset: 0x00018FB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(float3 v)
		{
			return new uint3(v);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001ADBE File Offset: 0x00018FBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(double v)
		{
			return new uint3(v);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001ADC6 File Offset: 0x00018FC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 uint3(double3 v)
		{
			return new uint3(v);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001ADCE File Offset: 0x00018FCE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint3 v)
		{
			return math.csum(v * math.uint3(3441847433U, 4052036147U, 2011389559U)) + 2252224297U;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001ADF5 File Offset: 0x00018FF5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(uint3 v)
		{
			return v * math.uint3(3784421429U, 1750626223U, 3571447507U) + 3412283213U;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001AE1B File Offset: 0x0001901B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint shuffle(uint3 left, uint3 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001AE25 File Offset: 0x00019025
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 shuffle(uint3 left, uint3 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.uint2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001AE3C File Offset: 0x0001903C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 shuffle(uint3 left, uint3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.uint3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001AE5C File Offset: 0x0001905C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 shuffle(uint3 left, uint3 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.uint4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001AE88 File Offset: 0x00019088
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint select_shuffle_component(uint3 a, uint3 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			}
			throw new ArgumentException("Invalid shuffle component: " + component.ToString());
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001AEFF File Offset: 0x000190FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(uint3 c0, uint3 c1)
		{
			return new uint3x2(c0, c1);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001AF08 File Offset: 0x00019108
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21)
		{
			return new uint3x2(m00, m01, m10, m11, m20, m21);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001AF17 File Offset: 0x00019117
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(uint v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001AF1F File Offset: 0x0001911F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(bool v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001AF27 File Offset: 0x00019127
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(bool3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001AF2F File Offset: 0x0001912F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(int v)
		{
			return new uint3x2(v);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001AF37 File Offset: 0x00019137
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(int3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001AF3F File Offset: 0x0001913F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(float v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001AF47 File Offset: 0x00019147
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(float3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001AF4F File Offset: 0x0001914F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(double v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001AF57 File Offset: 0x00019157
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x2 uint3x2(double3x2 v)
		{
			return new uint3x2(v);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001AF60 File Offset: 0x00019160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 transpose(uint3x2 v)
		{
			return math.uint2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001AFB4 File Offset: 0x000191B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint3x2 v)
		{
			return math.csum(v.c0 * math.uint3(1365086453U, 3969870067U, 4192899797U) + v.c1 * math.uint3(3271228601U, 1634639009U, 3318036811U)) + 3404170631U;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001B010 File Offset: 0x00019210
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(uint3x2 v)
		{
			return v.c0 * math.uint3(2048213449U, 4164671783U, 1780759499U) + v.c1 * math.uint3(1352369353U, 2446407751U, 1391928079U) + 3475533443U;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001B06A File Offset: 0x0001926A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(uint3 c0, uint3 c1, uint3 c2)
		{
			return new uint3x3(c0, c1, c2);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001B074 File Offset: 0x00019274
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12, uint m20, uint m21, uint m22)
		{
			return new uint3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001B094 File Offset: 0x00019294
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(uint v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001B09C File Offset: 0x0001929C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(bool v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001B0A4 File Offset: 0x000192A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(bool3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001B0AC File Offset: 0x000192AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(int v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001B0B4 File Offset: 0x000192B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(int3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001B0BC File Offset: 0x000192BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(float v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001B0C4 File Offset: 0x000192C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(float3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001B0CC File Offset: 0x000192CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(double v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001B0D4 File Offset: 0x000192D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 uint3x3(double3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0001B0DC File Offset: 0x000192DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 transpose(uint3x3 v)
		{
			return math.uint3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001B154 File Offset: 0x00019354
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint3x3 v)
		{
			return math.csum(v.c0 * math.uint3(2892026051U, 2455987759U, 3868600063U) + v.c1 * math.uint3(3170963179U, 2632835537U, 1136528209U) + v.c2 * math.uint3(2944626401U, 2972762423U, 1417889653U)) + 2080514593U;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001B1D4 File Offset: 0x000193D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(uint3x3 v)
		{
			return v.c0 * math.uint3(2731544287U, 2828498809U, 2669441947U) + v.c1 * math.uint3(1260114311U, 2650080659U, 4052675461U) + v.c2 * math.uint3(2652487619U, 2174136431U, 3528391193U) + 2105559227U;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001B252 File Offset: 0x00019452
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(uint3 c0, uint3 c1, uint3 c2, uint3 c3)
		{
			return new uint3x4(c0, c1, c2, c3);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001B260 File Offset: 0x00019460
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23)
		{
			return new uint3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001B286 File Offset: 0x00019486
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(uint v)
		{
			return new uint3x4(v);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001B28E File Offset: 0x0001948E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(bool v)
		{
			return new uint3x4(v);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001B296 File Offset: 0x00019496
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(bool3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001B29E File Offset: 0x0001949E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(int v)
		{
			return new uint3x4(v);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001B2A6 File Offset: 0x000194A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(int3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001B2AE File Offset: 0x000194AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(float v)
		{
			return new uint3x4(v);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001B2B6 File Offset: 0x000194B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(float3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001B2BE File Offset: 0x000194BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(double v)
		{
			return new uint3x4(v);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001B2C6 File Offset: 0x000194C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 uint3x4(double3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001B2D0 File Offset: 0x000194D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 transpose(uint3x4 v)
		{
			return math.uint4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001B368 File Offset: 0x00019568
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint3x4 v)
		{
			return math.csum(v.c0 * math.uint3(3508684087U, 3919501043U, 1209161033U) + v.c1 * math.uint3(4007793211U, 3819806693U, 3458005183U) + v.c2 * math.uint3(2078515003U, 4206465343U, 3025146473U) + v.c3 * math.uint3(3763046909U, 3678265601U, 2070747979U)) + 1480171127U;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001B40C File Offset: 0x0001960C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 hashwide(uint3x4 v)
		{
			return v.c0 * math.uint3(1588341193U, 4234155257U, 1811310911U) + v.c1 * math.uint3(2635799963U, 4165137857U, 2759770933U) + v.c2 * math.uint3(2759319383U, 3299952959U, 3121178323U) + v.c3 * math.uint3(2948522579U, 1531026433U, 1365086453U) + 3969870067U;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001B4AE File Offset: 0x000196AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint x, uint y, uint z, uint w)
		{
			return new uint4(x, y, z, w);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001B4B9 File Offset: 0x000196B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint x, uint y, uint2 zw)
		{
			return new uint4(x, y, zw);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001B4C3 File Offset: 0x000196C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint x, uint2 yz, uint w)
		{
			return new uint4(x, yz, w);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001B4CD File Offset: 0x000196CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint x, uint3 yzw)
		{
			return new uint4(x, yzw);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001B4D6 File Offset: 0x000196D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint2 xy, uint z, uint w)
		{
			return new uint4(xy, z, w);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001B4E0 File Offset: 0x000196E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint2 xy, uint2 zw)
		{
			return new uint4(xy, zw);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001B4E9 File Offset: 0x000196E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint3 xyz, uint w)
		{
			return new uint4(xyz, w);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001B4F2 File Offset: 0x000196F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint4 xyzw)
		{
			return new uint4(xyzw);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001B4FA File Offset: 0x000196FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(uint v)
		{
			return new uint4(v);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001B502 File Offset: 0x00019702
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(bool v)
		{
			return new uint4(v);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001B50A File Offset: 0x0001970A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(bool4 v)
		{
			return new uint4(v);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001B512 File Offset: 0x00019712
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(int v)
		{
			return new uint4(v);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001B51A File Offset: 0x0001971A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(int4 v)
		{
			return new uint4(v);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001B522 File Offset: 0x00019722
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(float v)
		{
			return new uint4(v);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001B52A File Offset: 0x0001972A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(float4 v)
		{
			return new uint4(v);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001B532 File Offset: 0x00019732
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(double v)
		{
			return new uint4(v);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001B53A File Offset: 0x0001973A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 uint4(double4 v)
		{
			return new uint4(v);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001B542 File Offset: 0x00019742
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint4 v)
		{
			return math.csum(v * math.uint4(3029516053U, 3547472099U, 2057487037U, 3781937309U)) + 2057338067U;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001B56E File Offset: 0x0001976E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(uint4 v)
		{
			return v * math.uint4(2942577577U, 2834440507U, 2671762487U, 2892026051U) + 2455987759U;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001B599 File Offset: 0x00019799
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint shuffle(uint4 left, uint4 right, math.ShuffleComponent x)
		{
			return math.select_shuffle_component(left, right, x);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001B5A3 File Offset: 0x000197A3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2 shuffle(uint4 left, uint4 right, math.ShuffleComponent x, math.ShuffleComponent y)
		{
			return math.uint2(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y));
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001B5BA File Offset: 0x000197BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 shuffle(uint4 left, uint4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z)
		{
			return math.uint3(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z));
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001B5DA File Offset: 0x000197DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 shuffle(uint4 left, uint4 right, math.ShuffleComponent x, math.ShuffleComponent y, math.ShuffleComponent z, math.ShuffleComponent w)
		{
			return math.uint4(math.select_shuffle_component(left, right, x), math.select_shuffle_component(left, right, y), math.select_shuffle_component(left, right, z), math.select_shuffle_component(left, right, w));
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001B604 File Offset: 0x00019804
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint select_shuffle_component(uint4 a, uint4 b, math.ShuffleComponent component)
		{
			switch (component)
			{
			case math.ShuffleComponent.LeftX:
				return a.x;
			case math.ShuffleComponent.LeftY:
				return a.y;
			case math.ShuffleComponent.LeftZ:
				return a.z;
			case math.ShuffleComponent.LeftW:
				return a.w;
			case math.ShuffleComponent.RightX:
				return b.x;
			case math.ShuffleComponent.RightY:
				return b.y;
			case math.ShuffleComponent.RightZ:
				return b.z;
			case math.ShuffleComponent.RightW:
				return b.w;
			default:
				throw new ArgumentException("Invalid shuffle component: " + component.ToString());
			}
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001B68D File Offset: 0x0001988D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(uint4 c0, uint4 c1)
		{
			return new uint4x2(c0, c1);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001B696 File Offset: 0x00019896
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21, uint m30, uint m31)
		{
			return new uint4x2(m00, m01, m10, m11, m20, m21, m30, m31);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001B6A9 File Offset: 0x000198A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(uint v)
		{
			return new uint4x2(v);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001B6B1 File Offset: 0x000198B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(bool v)
		{
			return new uint4x2(v);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001B6B9 File Offset: 0x000198B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(bool4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001B6C1 File Offset: 0x000198C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(int v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001B6C9 File Offset: 0x000198C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(int4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001B6D1 File Offset: 0x000198D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(float v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001B6D9 File Offset: 0x000198D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(float4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001B6E1 File Offset: 0x000198E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(double v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001B6E9 File Offset: 0x000198E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 uint4x2(double4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0001B6F4 File Offset: 0x000198F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 transpose(uint4x2 v)
		{
			return math.uint2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001B760 File Offset: 0x00019960
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint4x2 v)
		{
			return math.csum(v.c0 * math.uint4(4198118021U, 2908068253U, 3705492289U, 2497566569U) + v.c1 * math.uint4(2716413241U, 1166264321U, 2503385333U, 2944493077U)) + 2599999021U;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0001B7C8 File Offset: 0x000199C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(uint4x2 v)
		{
			return v.c0 * math.uint4(3814721321U, 1595355149U, 1728931849U, 2062756937U) + v.c1 * math.uint4(2920485769U, 1562056283U, 2265541847U, 1283419601U) + 1210229737U;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0001B82C File Offset: 0x00019A2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(uint4 c0, uint4 c1, uint4 c2)
		{
			return new uint4x3(c0, c1, c2);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001B838 File Offset: 0x00019A38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12, uint m20, uint m21, uint m22, uint m30, uint m31, uint m32)
		{
			return new uint4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0001B85E File Offset: 0x00019A5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(uint v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001B866 File Offset: 0x00019A66
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(bool v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001B86E File Offset: 0x00019A6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(bool4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001B876 File Offset: 0x00019A76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(int v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001B87E File Offset: 0x00019A7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(int4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001B886 File Offset: 0x00019A86
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(float v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001B88E File Offset: 0x00019A8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(float4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001B896 File Offset: 0x00019A96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(double v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001B89E File Offset: 0x00019A9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 uint4x3(double4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001B8A8 File Offset: 0x00019AA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 transpose(uint4x3 v)
		{
			return math.uint3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001B940 File Offset: 0x00019B40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint4x3 v)
		{
			return math.csum(v.c0 * math.uint4(3881277847U, 4017968839U, 1727237899U, 1648514723U) + v.c1 * math.uint4(1385344481U, 3538260197U, 4066109527U, 2613148903U) + v.c2 * math.uint4(3367528529U, 1678332449U, 2918459647U, 2744611081U)) + 1952372791U;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001B9D0 File Offset: 0x00019BD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(uint4x3 v)
		{
			return v.c0 * math.uint4(2631698677U, 4200781601U, 2119021007U, 1760485621U) + v.c1 * math.uint4(3157985881U, 2171534173U, 2723054263U, 1168253063U) + v.c2 * math.uint4(4228926523U, 1610574617U, 1584185147U, 3041325733U) + 3150930919U;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001BA5D File Offset: 0x00019C5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(uint4 c0, uint4 c1, uint4 c2, uint4 c3)
		{
			return new uint4x4(c0, c1, c2, c3);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001BA68 File Offset: 0x00019C68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23, uint m30, uint m31, uint m32, uint m33)
		{
			return new uint4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001BA96 File Offset: 0x00019C96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(uint v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001BA9E File Offset: 0x00019C9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(bool v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001BAA6 File Offset: 0x00019CA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(bool4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001BAAE File Offset: 0x00019CAE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(int v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001BAB6 File Offset: 0x00019CB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(int4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001BABE File Offset: 0x00019CBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(float v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001BAC6 File Offset: 0x00019CC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(float4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001BACE File Offset: 0x00019CCE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(double v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001BAD6 File Offset: 0x00019CD6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 uint4x4(double4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001BAE0 File Offset: 0x00019CE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 transpose(uint4x4 v)
		{
			return math.uint4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001BBA4 File Offset: 0x00019DA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint hash(uint4x4 v)
		{
			return math.csum(v.c0 * math.uint4(2627668003U, 1520214331U, 2949502447U, 2827819133U) + v.c1 * math.uint4(3480140317U, 2642994593U, 3940484981U, 1954192763U) + v.c2 * math.uint4(1091696537U, 3052428017U, 4253034763U, 2338696631U) + v.c3 * math.uint4(3757372771U, 1885959949U, 3508684087U, 3919501043U)) + 1209161033U;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001BC5C File Offset: 0x00019E5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4 hashwide(uint4x4 v)
		{
			return v.c0 * math.uint4(4007793211U, 3819806693U, 3458005183U, 2078515003U) + v.c1 * math.uint4(4206465343U, 3025146473U, 3763046909U, 3678265601U) + v.c2 * math.uint4(2070747979U, 1480171127U, 1588341193U, 4234155257U) + v.c3 * math.uint4(1811310911U, 2635799963U, 4165137857U, 2759770933U) + 2759319383U;
		}

		// Token: 0x04000005 RID: 5
		public const double E_DBL = 2.718281828459045;

		// Token: 0x04000006 RID: 6
		public const double LOG2E_DBL = 1.4426950408889634;

		// Token: 0x04000007 RID: 7
		public const double LOG10E_DBL = 0.4342944819032518;

		// Token: 0x04000008 RID: 8
		public const double LN2_DBL = 0.6931471805599453;

		// Token: 0x04000009 RID: 9
		public const double LN10_DBL = 2.302585092994046;

		// Token: 0x0400000A RID: 10
		public const double PI_DBL = 3.141592653589793;

		// Token: 0x0400000B RID: 11
		public const double PI2_DBL = 6.283185307179586;

		// Token: 0x0400000C RID: 12
		public const double PIHALF_DBL = 1.5707963267948966;

		// Token: 0x0400000D RID: 13
		public const double TAU_DBL = 6.283185307179586;

		// Token: 0x0400000E RID: 14
		public const double TODEGREES_DBL = 57.29577951308232;

		// Token: 0x0400000F RID: 15
		public const double TORADIANS_DBL = 0.017453292519943295;

		// Token: 0x04000010 RID: 16
		public const double SQRT2_DBL = 1.4142135623730951;

		// Token: 0x04000011 RID: 17
		public const double EPSILON_DBL = 2.220446049250313E-16;

		// Token: 0x04000012 RID: 18
		public const double INFINITY_DBL = double.PositiveInfinity;

		// Token: 0x04000013 RID: 19
		public const double NAN_DBL = double.NaN;

		// Token: 0x04000014 RID: 20
		public const float FLT_MIN_NORMAL = 1.1754944E-38f;

		// Token: 0x04000015 RID: 21
		public const double DBL_MIN_NORMAL = 2.2250738585072014E-308;

		// Token: 0x04000016 RID: 22
		public const float E = 2.7182817f;

		// Token: 0x04000017 RID: 23
		public const float LOG2E = 1.442695f;

		// Token: 0x04000018 RID: 24
		public const float LOG10E = 0.4342945f;

		// Token: 0x04000019 RID: 25
		public const float LN2 = 0.6931472f;

		// Token: 0x0400001A RID: 26
		public const float LN10 = 2.3025851f;

		// Token: 0x0400001B RID: 27
		public const float PI = 3.1415927f;

		// Token: 0x0400001C RID: 28
		public const float PI2 = 6.2831855f;

		// Token: 0x0400001D RID: 29
		public const float PIHALF = 1.5707964f;

		// Token: 0x0400001E RID: 30
		public const float TAU = 6.2831855f;

		// Token: 0x0400001F RID: 31
		public const float TODEGREES = 57.29578f;

		// Token: 0x04000020 RID: 32
		public const float TORADIANS = 0.017453292f;

		// Token: 0x04000021 RID: 33
		public const float SQRT2 = 1.4142135f;

		// Token: 0x04000022 RID: 34
		public const float EPSILON = 1.1920929E-07f;

		// Token: 0x04000023 RID: 35
		public const float INFINITY = float.PositiveInfinity;

		// Token: 0x04000024 RID: 36
		public const float NAN = float.NaN;

		// Token: 0x02000052 RID: 82
		public enum RotationOrder : byte
		{
			// Token: 0x04000138 RID: 312
			XYZ,
			// Token: 0x04000139 RID: 313
			XZY,
			// Token: 0x0400013A RID: 314
			YXZ,
			// Token: 0x0400013B RID: 315
			YZX,
			// Token: 0x0400013C RID: 316
			ZXY,
			// Token: 0x0400013D RID: 317
			ZYX,
			// Token: 0x0400013E RID: 318
			Default = 4
		}

		// Token: 0x02000053 RID: 83
		public enum ShuffleComponent : byte
		{
			// Token: 0x04000140 RID: 320
			LeftX,
			// Token: 0x04000141 RID: 321
			LeftY,
			// Token: 0x04000142 RID: 322
			LeftZ,
			// Token: 0x04000143 RID: 323
			LeftW,
			// Token: 0x04000144 RID: 324
			RightX,
			// Token: 0x04000145 RID: 325
			RightY,
			// Token: 0x04000146 RID: 326
			RightZ,
			// Token: 0x04000147 RID: 327
			RightW
		}

		// Token: 0x02000054 RID: 84
		[StructLayout(LayoutKind.Explicit)]
		internal struct LongDoubleUnion
		{
			// Token: 0x04000148 RID: 328
			[FieldOffset(0)]
			public long longValue;

			// Token: 0x04000149 RID: 329
			[FieldOffset(0)]
			public double doubleValue;
		}
	}
}
