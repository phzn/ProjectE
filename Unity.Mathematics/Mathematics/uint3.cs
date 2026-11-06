using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000045 RID: 69
	[DebuggerTypeProxy(typeof(uint3.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint3 : IEquatable<uint3>, IFormattable
	{
		// Token: 0x0600202D RID: 8237 RVA: 0x0005DA91 File Offset: 0x0005BC91
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(uint x, uint y, uint z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0005DAA8 File Offset: 0x0005BCA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(uint x, uint2 yz)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x0005DAC9 File Offset: 0x0005BCC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(uint2 xy, uint z)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x0005DAEA File Offset: 0x0005BCEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(uint3 xyz)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0005DB10 File Offset: 0x0005BD10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(uint v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0005DB27 File Offset: 0x0005BD27
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(bool v)
		{
			this.x = (v ? 1U : 0U);
			this.y = (v ? 1U : 0U);
			this.z = (v ? 1U : 0U);
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0005DB50 File Offset: 0x0005BD50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(bool3 v)
		{
			this.x = (v.x ? 1U : 0U);
			this.y = (v.y ? 1U : 0U);
			this.z = (v.z ? 1U : 0U);
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x0005DB88 File Offset: 0x0005BD88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(int v)
		{
			this.x = (uint)v;
			this.y = (uint)v;
			this.z = (uint)v;
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x0005DB9F File Offset: 0x0005BD9F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(int3 v)
		{
			this.x = (uint)v.x;
			this.y = (uint)v.y;
			this.z = (uint)v.z;
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0005DBC5 File Offset: 0x0005BDC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(float v)
		{
			this.x = (uint)v;
			this.y = (uint)v;
			this.z = (uint)v;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x0005DBDF File Offset: 0x0005BDDF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(float3 v)
		{
			this.x = (uint)v.x;
			this.y = (uint)v.y;
			this.z = (uint)v.z;
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0005DC08 File Offset: 0x0005BE08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(double v)
		{
			this.x = (uint)v;
			this.y = (uint)v;
			this.z = (uint)v;
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0005DC22 File Offset: 0x0005BE22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3(double3 v)
		{
			this.x = (uint)v.x;
			this.y = (uint)v.y;
			this.z = (uint)v.z;
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0005DC4B File Offset: 0x0005BE4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint3(uint v)
		{
			return new uint3(v);
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0005DC53 File Offset: 0x0005BE53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(bool v)
		{
			return new uint3(v);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x0005DC5B File Offset: 0x0005BE5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(bool3 v)
		{
			return new uint3(v);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x0005DC63 File Offset: 0x0005BE63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(int v)
		{
			return new uint3(v);
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x0005DC6B File Offset: 0x0005BE6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(int3 v)
		{
			return new uint3(v);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0005DC73 File Offset: 0x0005BE73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(float v)
		{
			return new uint3(v);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0005DC7B File Offset: 0x0005BE7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(float3 v)
		{
			return new uint3(v);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0005DC83 File Offset: 0x0005BE83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(double v)
		{
			return new uint3(v);
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x0005DC8B File Offset: 0x0005BE8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3(double3 v)
		{
			return new uint3(v);
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x0005DC93 File Offset: 0x0005BE93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator *(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x0005DCC1 File Offset: 0x0005BEC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator *(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0005DCE0 File Offset: 0x0005BEE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator *(uint lhs, uint3 rhs)
		{
			return new uint3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x0005DCFF File Offset: 0x0005BEFF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator +(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x0005DD2D File Offset: 0x0005BF2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator +(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0005DD4C File Offset: 0x0005BF4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator +(uint lhs, uint3 rhs)
		{
			return new uint3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x0005DD6B File Offset: 0x0005BF6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator -(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x0005DD99 File Offset: 0x0005BF99
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator -(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0005DDB8 File Offset: 0x0005BFB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator -(uint lhs, uint3 rhs)
		{
			return new uint3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x0005DDD7 File Offset: 0x0005BFD7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator /(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x0005DE05 File Offset: 0x0005C005
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator /(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x0005DE24 File Offset: 0x0005C024
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator /(uint lhs, uint3 rhs)
		{
			return new uint3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x0005DE43 File Offset: 0x0005C043
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator %(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x0005DE71 File Offset: 0x0005C071
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator %(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x0005DE90 File Offset: 0x0005C090
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator %(uint lhs, uint3 rhs)
		{
			return new uint3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x0005DEB0 File Offset: 0x0005C0B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator ++(uint3 val)
		{
			uint num = val.x + 1U;
			val.x = num;
			uint num2 = num;
			num = val.y + 1U;
			val.y = num;
			uint num3 = num;
			num = val.z + 1U;
			val.z = num;
			return new uint3(num2, num3, num);
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x0005DEF0 File Offset: 0x0005C0F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator --(uint3 val)
		{
			uint num = val.x - 1U;
			val.x = num;
			uint num2 = num;
			num = val.y - 1U;
			val.y = num;
			uint num3 = num;
			num = val.z - 1U;
			val.z = num;
			return new uint3(num2, num3, num);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x0005DF2F File Offset: 0x0005C12F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(uint3 lhs, uint3 rhs)
		{
			return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x0005DF60 File Offset: 0x0005C160
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(uint3 lhs, uint rhs)
		{
			return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x0005DF82 File Offset: 0x0005C182
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(uint lhs, uint3 rhs)
		{
			return new bool3(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z);
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x0005DFA4 File Offset: 0x0005C1A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(uint3 lhs, uint3 rhs)
		{
			return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x0005DFDE File Offset: 0x0005C1DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(uint3 lhs, uint rhs)
		{
			return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x0005E009 File Offset: 0x0005C209
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(uint lhs, uint3 rhs)
		{
			return new bool3(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x0005E034 File Offset: 0x0005C234
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(uint3 lhs, uint3 rhs)
		{
			return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x0005E065 File Offset: 0x0005C265
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(uint3 lhs, uint rhs)
		{
			return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0005E087 File Offset: 0x0005C287
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(uint lhs, uint3 rhs)
		{
			return new bool3(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x0005E0A9 File Offset: 0x0005C2A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(uint3 lhs, uint3 rhs)
		{
			return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x0005E0E3 File Offset: 0x0005C2E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(uint3 lhs, uint rhs)
		{
			return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x0005E10E File Offset: 0x0005C30E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(uint lhs, uint3 rhs)
		{
			return new bool3(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z);
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x0005E139 File Offset: 0x0005C339
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator -(uint3 val)
		{
			return new uint3((uint)(-(uint)((ulong)val.x)), (uint)(-(uint)((ulong)val.y)), (uint)(-(uint)((ulong)val.z)));
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x0005E15B File Offset: 0x0005C35B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator +(uint3 val)
		{
			return new uint3(val.x, val.y, val.z);
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0005E174 File Offset: 0x0005C374
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator <<(uint3 x, int n)
		{
			return new uint3(x.x << n, x.y << n, x.z << n);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0005E19C File Offset: 0x0005C39C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator >>(uint3 x, int n)
		{
			return new uint3(x.x >> n, x.y >> n, x.z >> n);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0005E1C4 File Offset: 0x0005C3C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(uint3 lhs, uint3 rhs)
		{
			return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x0005E1F5 File Offset: 0x0005C3F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(uint3 lhs, uint rhs)
		{
			return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x0005E217 File Offset: 0x0005C417
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(uint lhs, uint3 rhs)
		{
			return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x0005E239 File Offset: 0x0005C439
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(uint3 lhs, uint3 rhs)
		{
			return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x0005E273 File Offset: 0x0005C473
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(uint3 lhs, uint rhs)
		{
			return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x0005E29E File Offset: 0x0005C49E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(uint lhs, uint3 rhs)
		{
			return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x0005E2C9 File Offset: 0x0005C4C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator ~(uint3 val)
		{
			return new uint3(~val.x, ~val.y, ~val.z);
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x0005E2E5 File Offset: 0x0005C4E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator &(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x0005E313 File Offset: 0x0005C513
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator &(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0005E332 File Offset: 0x0005C532
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator &(uint lhs, uint3 rhs)
		{
			return new uint3(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z);
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x0005E351 File Offset: 0x0005C551
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator |(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z);
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x0005E37F File Offset: 0x0005C57F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator |(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs);
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x0005E39E File Offset: 0x0005C59E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator |(uint lhs, uint3 rhs)
		{
			return new uint3(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z);
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x0005E3BD File Offset: 0x0005C5BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator ^(uint3 lhs, uint3 rhs)
		{
			return new uint3(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z);
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x0005E3EB File Offset: 0x0005C5EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator ^(uint3 lhs, uint rhs)
		{
			return new uint3(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x0005E40A File Offset: 0x0005C60A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 operator ^(uint lhs, uint3 rhs)
		{
			return new uint3(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z);
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x0005E429 File Offset: 0x0005C629
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x0005E448 File Offset: 0x0005C648
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x0005E467 File Offset: 0x0005C667
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x0005E486 File Offset: 0x0005C686
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x0005E4A5 File Offset: 0x0005C6A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x0005E4C4 File Offset: 0x0005C6C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x0005E4E3 File Offset: 0x0005C6E3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600207B RID: 8315 RVA: 0x0005E502 File Offset: 0x0005C702
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x0005E521 File Offset: 0x0005C721
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x0005E540 File Offset: 0x0005C740
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x0005E55F File Offset: 0x0005C75F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x0005E57E File Offset: 0x0005C77E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x0005E59D File Offset: 0x0005C79D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06002081 RID: 8321 RVA: 0x0005E5BC File Offset: 0x0005C7BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x0005E5DB File Offset: 0x0005C7DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x0005E5FA File Offset: 0x0005C7FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x0005E619 File Offset: 0x0005C819
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x0005E638 File Offset: 0x0005C838
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x0005E657 File Offset: 0x0005C857
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x0005E676 File Offset: 0x0005C876
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x0005E695 File Offset: 0x0005C895
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06002089 RID: 8329 RVA: 0x0005E6B4 File Offset: 0x0005C8B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x0005E6D3 File Offset: 0x0005C8D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600208B RID: 8331 RVA: 0x0005E6F2 File Offset: 0x0005C8F2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x0005E711 File Offset: 0x0005C911
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x0600208D RID: 8333 RVA: 0x0005E730 File Offset: 0x0005C930
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x0005E74F File Offset: 0x0005C94F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600208F RID: 8335 RVA: 0x0005E76E File Offset: 0x0005C96E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x0005E78D File Offset: 0x0005C98D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x0005E7AC File Offset: 0x0005C9AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x0005E7CB File Offset: 0x0005C9CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x0005E7EA File Offset: 0x0005C9EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x0005E809 File Offset: 0x0005CA09
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06002095 RID: 8341 RVA: 0x0005E828 File Offset: 0x0005CA28
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x0005E847 File Offset: 0x0005CA47
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x0005E866 File Offset: 0x0005CA66
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x0005E885 File Offset: 0x0005CA85
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002099 RID: 8345 RVA: 0x0005E8A4 File Offset: 0x0005CAA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x0005E8C3 File Offset: 0x0005CAC3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600209B RID: 8347 RVA: 0x0005E8E2 File Offset: 0x0005CAE2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600209C RID: 8348 RVA: 0x0005E901 File Offset: 0x0005CB01
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600209D RID: 8349 RVA: 0x0005E920 File Offset: 0x0005CB20
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600209E RID: 8350 RVA: 0x0005E93F File Offset: 0x0005CB3F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600209F RID: 8351 RVA: 0x0005E95E File Offset: 0x0005CB5E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x0005E97D File Offset: 0x0005CB7D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060020A1 RID: 8353 RVA: 0x0005E99C File Offset: 0x0005CB9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x0005E9BB File Offset: 0x0005CBBB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x0005E9DA File Offset: 0x0005CBDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x060020A4 RID: 8356 RVA: 0x0005E9F9 File Offset: 0x0005CBF9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x0005EA18 File Offset: 0x0005CC18
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x0005EA37 File Offset: 0x0005CC37
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x060020A7 RID: 8359 RVA: 0x0005EA56 File Offset: 0x0005CC56
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060020A8 RID: 8360 RVA: 0x0005EA75 File Offset: 0x0005CC75
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060020A9 RID: 8361 RVA: 0x0005EA94 File Offset: 0x0005CC94
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x060020AA RID: 8362 RVA: 0x0005EAB3 File Offset: 0x0005CCB3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x0005EAD2 File Offset: 0x0005CCD2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x0005EAF1 File Offset: 0x0005CCF1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x060020AD RID: 8365 RVA: 0x0005EB10 File Offset: 0x0005CD10
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x060020AE RID: 8366 RVA: 0x0005EB2F File Offset: 0x0005CD2F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x060020AF RID: 8367 RVA: 0x0005EB4E File Offset: 0x0005CD4E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x0005EB6D File Offset: 0x0005CD6D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x060020B1 RID: 8369 RVA: 0x0005EB8C File Offset: 0x0005CD8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060020B2 RID: 8370 RVA: 0x0005EBAB File Offset: 0x0005CDAB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x0005EBCA File Offset: 0x0005CDCA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060020B4 RID: 8372 RVA: 0x0005EBE9 File Offset: 0x0005CDE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060020B5 RID: 8373 RVA: 0x0005EC08 File Offset: 0x0005CE08
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060020B6 RID: 8374 RVA: 0x0005EC27 File Offset: 0x0005CE27
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060020B7 RID: 8375 RVA: 0x0005EC46 File Offset: 0x0005CE46
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x0005EC65 File Offset: 0x0005CE65
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x0005EC84 File Offset: 0x0005CE84
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x0005ECA3 File Offset: 0x0005CEA3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x0005ECC2 File Offset: 0x0005CEC2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060020BC RID: 8380 RVA: 0x0005ECE1 File Offset: 0x0005CEE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x0005ED00 File Offset: 0x0005CF00
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x0005ED1F File Offset: 0x0005CF1F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x0005ED3E File Offset: 0x0005CF3E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060020C0 RID: 8384 RVA: 0x0005ED5D File Offset: 0x0005CF5D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060020C1 RID: 8385 RVA: 0x0005ED7C File Offset: 0x0005CF7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x0005ED9B File Offset: 0x0005CF9B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x0005EDBA File Offset: 0x0005CFBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060020C4 RID: 8388 RVA: 0x0005EDD9 File Offset: 0x0005CFD9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060020C5 RID: 8389 RVA: 0x0005EDF8 File Offset: 0x0005CFF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.x, this.x);
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060020C6 RID: 8390 RVA: 0x0005EE11 File Offset: 0x0005D011
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.x, this.y);
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x0005EE2A File Offset: 0x0005D02A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.x, this.z);
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x0005EE43 File Offset: 0x0005D043
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x0005EE5C File Offset: 0x0005D05C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x0005EE75 File Offset: 0x0005D075
		// (set) Token: 0x060020CB RID: 8395 RVA: 0x0005EE8E File Offset: 0x0005D08E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060020CC RID: 8396 RVA: 0x0005EEB4 File Offset: 0x0005D0B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x0005EECD File Offset: 0x0005D0CD
		// (set) Token: 0x060020CE RID: 8398 RVA: 0x0005EEE6 File Offset: 0x0005D0E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060020CF RID: 8399 RVA: 0x0005EF0C File Offset: 0x0005D10C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.x, this.z, this.z);
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060020D0 RID: 8400 RVA: 0x0005EF25 File Offset: 0x0005D125
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060020D1 RID: 8401 RVA: 0x0005EF3E File Offset: 0x0005D13E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x0005EF57 File Offset: 0x0005D157
		// (set) Token: 0x060020D3 RID: 8403 RVA: 0x0005EF70 File Offset: 0x0005D170
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060020D4 RID: 8404 RVA: 0x0005EF96 File Offset: 0x0005D196
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.y, this.x);
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060020D5 RID: 8405 RVA: 0x0005EFAF File Offset: 0x0005D1AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.y, this.y);
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060020D6 RID: 8406 RVA: 0x0005EFC8 File Offset: 0x0005D1C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.y, this.z);
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060020D7 RID: 8407 RVA: 0x0005EFE1 File Offset: 0x0005D1E1
		// (set) Token: 0x060020D8 RID: 8408 RVA: 0x0005EFFA File Offset: 0x0005D1FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060020D9 RID: 8409 RVA: 0x0005F020 File Offset: 0x0005D220
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.z, this.y);
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x0005F039 File Offset: 0x0005D239
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.y, this.z, this.z);
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060020DB RID: 8411 RVA: 0x0005F052 File Offset: 0x0005D252
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.x, this.x);
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x0005F06B File Offset: 0x0005D26B
		// (set) Token: 0x060020DD RID: 8413 RVA: 0x0005F084 File Offset: 0x0005D284
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x0005F0AA File Offset: 0x0005D2AA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.x, this.z);
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060020DF RID: 8415 RVA: 0x0005F0C3 File Offset: 0x0005D2C3
		// (set) Token: 0x060020E0 RID: 8416 RVA: 0x0005F0DC File Offset: 0x0005D2DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060020E1 RID: 8417 RVA: 0x0005F102 File Offset: 0x0005D302
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.y, this.y);
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060020E2 RID: 8418 RVA: 0x0005F11B File Offset: 0x0005D31B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.y, this.z);
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x0005F134 File Offset: 0x0005D334
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.z, this.x);
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x0005F14D File Offset: 0x0005D34D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.z, this.y);
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060020E5 RID: 8421 RVA: 0x0005F166 File Offset: 0x0005D366
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint3(this.z, this.z, this.z);
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060020E6 RID: 8422 RVA: 0x0005F17F File Offset: 0x0005D37F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.x, this.x);
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060020E7 RID: 8423 RVA: 0x0005F192 File Offset: 0x0005D392
		// (set) Token: 0x060020E8 RID: 8424 RVA: 0x0005F1A5 File Offset: 0x0005D3A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060020E9 RID: 8425 RVA: 0x0005F1BF File Offset: 0x0005D3BF
		// (set) Token: 0x060020EA RID: 8426 RVA: 0x0005F1D2 File Offset: 0x0005D3D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060020EB RID: 8427 RVA: 0x0005F1EC File Offset: 0x0005D3EC
		// (set) Token: 0x060020EC RID: 8428 RVA: 0x0005F1FF File Offset: 0x0005D3FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060020ED RID: 8429 RVA: 0x0005F219 File Offset: 0x0005D419
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.y, this.y);
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x0005F22C File Offset: 0x0005D42C
		// (set) Token: 0x060020EF RID: 8431 RVA: 0x0005F23F File Offset: 0x0005D43F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x0005F259 File Offset: 0x0005D459
		// (set) Token: 0x060020F1 RID: 8433 RVA: 0x0005F26C File Offset: 0x0005D46C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x0005F286 File Offset: 0x0005D486
		// (set) Token: 0x060020F3 RID: 8435 RVA: 0x0005F299 File Offset: 0x0005D499
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x060020F4 RID: 8436 RVA: 0x0005F2B3 File Offset: 0x0005D4B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public uint2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new uint2(this.z, this.z);
			}
		}

		// Token: 0x17000A32 RID: 2610
		public unsafe uint this[int index]
		{
			get
			{
				fixed (uint3* ptr = &this)
				{
					return ((uint*)ptr)[index];
				}
			}
			set
			{
				fixed (uint* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x0005F300 File Offset: 0x0005D500
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint3 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x0005F330 File Offset: 0x0005D530
		public override bool Equals(object o)
		{
			if (o is uint3)
			{
				uint3 @uint = (uint3)o;
				return this.Equals(@uint);
			}
			return false;
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x0005F355 File Offset: 0x0005D555
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x0005F362 File Offset: 0x0005D562
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint3({0}, {1}, {2})", this.x, this.y, this.z);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x0005F38F File Offset: 0x0005D58F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint3({0}, {1}, {2})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider), this.z.ToString(format, formatProvider));
		}

		// Token: 0x0400010A RID: 266
		public uint x;

		// Token: 0x0400010B RID: 267
		public uint y;

		// Token: 0x0400010C RID: 268
		public uint z;

		// Token: 0x0400010D RID: 269
		public static readonly uint3 zero;

		// Token: 0x02000065 RID: 101
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024D2 RID: 9426 RVA: 0x000693B0 File Offset: 0x000675B0
			public DebuggerProxy(uint3 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			// Token: 0x04000179 RID: 377
			public uint x;

			// Token: 0x0400017A RID: 378
			public uint y;

			// Token: 0x0400017B RID: 379
			public uint z;
		}
	}
}
