using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics
{
	// Token: 0x02000022 RID: 34
	[DebuggerTypeProxy(typeof(float3.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct float3 : IEquatable<float3>, IFormattable
	{
		// Token: 0x06001186 RID: 4486 RVA: 0x00033871 File Offset: 0x00031A71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00033888 File Offset: 0x00031A88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(float x, float2 yz)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x000338A9 File Offset: 0x00031AA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(float2 xy, float z)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000338CA File Offset: 0x00031ACA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(float3 xyz)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x000338F0 File Offset: 0x00031AF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(float v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00033908 File Offset: 0x00031B08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(bool v)
		{
			this.x = (v ? 1f : 0f);
			this.y = (v ? 1f : 0f);
			this.z = (v ? 1f : 0f);
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00033954 File Offset: 0x00031B54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(bool3 v)
		{
			this.x = (v.x ? 1f : 0f);
			this.y = (v.y ? 1f : 0f);
			this.z = (v.z ? 1f : 0f);
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x000339AF File Offset: 0x00031BAF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(int v)
		{
			this.x = (float)v;
			this.y = (float)v;
			this.z = (float)v;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x000339C9 File Offset: 0x00031BC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(int3 v)
		{
			this.x = (float)v.x;
			this.y = (float)v.y;
			this.z = (float)v.z;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x000339F2 File Offset: 0x00031BF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(uint v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00033A0F File Offset: 0x00031C0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(uint3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00033A3B File Offset: 0x00031C3B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(half v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00033A61 File Offset: 0x00031C61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(half3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00033A96 File Offset: 0x00031C96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(double v)
		{
			this.x = (float)v;
			this.y = (float)v;
			this.z = (float)v;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00033AB0 File Offset: 0x00031CB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float3(double3 v)
		{
			this.x = (float)v.x;
			this.y = (float)v.y;
			this.z = (float)v.z;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00033AD9 File Offset: 0x00031CD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3(float v)
		{
			return new float3(v);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00033AE1 File Offset: 0x00031CE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3(bool v)
		{
			return new float3(v);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00033AE9 File Offset: 0x00031CE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3(bool3 v)
		{
			return new float3(v);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00033AF1 File Offset: 0x00031CF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3(int v)
		{
			return new float3(v);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00033AF9 File Offset: 0x00031CF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3(int3 v)
		{
			return new float3(v);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00033B01 File Offset: 0x00031D01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3(uint v)
		{
			return new float3(v);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00033B09 File Offset: 0x00031D09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3(uint3 v)
		{
			return new float3(v);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00033B11 File Offset: 0x00031D11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3(half v)
		{
			return new float3(v);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00033B19 File Offset: 0x00031D19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float3(half3 v)
		{
			return new float3(v);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00033B21 File Offset: 0x00031D21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3(double v)
		{
			return new float3(v);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00033B29 File Offset: 0x00031D29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3(double3 v)
		{
			return new float3(v);
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00033B31 File Offset: 0x00031D31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator *(float3 lhs, float3 rhs)
		{
			return new float3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00033B5F File Offset: 0x00031D5F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator *(float3 lhs, float rhs)
		{
			return new float3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00033B7E File Offset: 0x00031D7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator *(float lhs, float3 rhs)
		{
			return new float3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00033B9D File Offset: 0x00031D9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator +(float3 lhs, float3 rhs)
		{
			return new float3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00033BCB File Offset: 0x00031DCB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator +(float3 lhs, float rhs)
		{
			return new float3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00033BEA File Offset: 0x00031DEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator +(float lhs, float3 rhs)
		{
			return new float3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00033C09 File Offset: 0x00031E09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator -(float3 lhs, float3 rhs)
		{
			return new float3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00033C37 File Offset: 0x00031E37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator -(float3 lhs, float rhs)
		{
			return new float3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00033C56 File Offset: 0x00031E56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator -(float lhs, float3 rhs)
		{
			return new float3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00033C75 File Offset: 0x00031E75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator /(float3 lhs, float3 rhs)
		{
			return new float3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00033CA3 File Offset: 0x00031EA3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator /(float3 lhs, float rhs)
		{
			return new float3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00033CC2 File Offset: 0x00031EC2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator /(float lhs, float3 rhs)
		{
			return new float3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00033CE1 File Offset: 0x00031EE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator %(float3 lhs, float3 rhs)
		{
			return new float3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00033D0F File Offset: 0x00031F0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator %(float3 lhs, float rhs)
		{
			return new float3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00033D2E File Offset: 0x00031F2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator %(float lhs, float3 rhs)
		{
			return new float3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00033D50 File Offset: 0x00031F50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator ++(float3 val)
		{
			float num = val.x + 1f;
			val.x = num;
			float num2 = num;
			num = val.y + 1f;
			val.y = num;
			float num3 = num;
			num = val.z + 1f;
			val.z = num;
			return new float3(num2, num3, num);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00033D9C File Offset: 0x00031F9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator --(float3 val)
		{
			float num = val.x - 1f;
			val.x = num;
			float num2 = num;
			num = val.y - 1f;
			val.y = num;
			float num3 = num;
			num = val.z - 1f;
			val.z = num;
			return new float3(num2, num3, num);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00033DE7 File Offset: 0x00031FE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(float3 lhs, float3 rhs)
		{
			return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00033E18 File Offset: 0x00032018
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(float3 lhs, float rhs)
		{
			return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00033E3A File Offset: 0x0003203A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <(float lhs, float3 rhs)
		{
			return new bool3(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00033E5C File Offset: 0x0003205C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(float3 lhs, float3 rhs)
		{
			return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00033E96 File Offset: 0x00032096
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(float3 lhs, float rhs)
		{
			return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00033EC1 File Offset: 0x000320C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator <=(float lhs, float3 rhs)
		{
			return new bool3(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00033EEC File Offset: 0x000320EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(float3 lhs, float3 rhs)
		{
			return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00033F1D File Offset: 0x0003211D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(float3 lhs, float rhs)
		{
			return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00033F3F File Offset: 0x0003213F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >(float lhs, float3 rhs)
		{
			return new bool3(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00033F61 File Offset: 0x00032161
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(float3 lhs, float3 rhs)
		{
			return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00033F9B File Offset: 0x0003219B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(float3 lhs, float rhs)
		{
			return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00033FC6 File Offset: 0x000321C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator >=(float lhs, float3 rhs)
		{
			return new bool3(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00033FF1 File Offset: 0x000321F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator -(float3 val)
		{
			return new float3(-val.x, -val.y, -val.z);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0003400D File Offset: 0x0003220D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 operator +(float3 val)
		{
			return new float3(val.x, val.y, val.z);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00034026 File Offset: 0x00032226
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(float3 lhs, float3 rhs)
		{
			return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00034057 File Offset: 0x00032257
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(float3 lhs, float rhs)
		{
			return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00034079 File Offset: 0x00032279
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(float lhs, float3 rhs)
		{
			return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0003409B File Offset: 0x0003229B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(float3 lhs, float3 rhs)
		{
			return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x000340D5 File Offset: 0x000322D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(float3 lhs, float rhs)
		{
			return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00034100 File Offset: 0x00032300
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(float lhs, float3 rhs)
		{
			return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x0003412B File Offset: 0x0003232B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0003414A File Offset: 0x0003234A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00034169 File Offset: 0x00032369
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x00034188 File Offset: 0x00032388
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x000341A7 File Offset: 0x000323A7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x000341C6 File Offset: 0x000323C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x000341E5 File Offset: 0x000323E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00034204 File Offset: 0x00032404
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x00034223 File Offset: 0x00032423
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00034242 File Offset: 0x00032442
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x00034261 File Offset: 0x00032461
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00034280 File Offset: 0x00032480
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x0003429F File Offset: 0x0003249F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x000342BE File Offset: 0x000324BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x000342DD File Offset: 0x000324DD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x000342FC File Offset: 0x000324FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x0003431B File Offset: 0x0003251B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x0003433A File Offset: 0x0003253A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00034359 File Offset: 0x00032559
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x00034378 File Offset: 0x00032578
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x00034397 File Offset: 0x00032597
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x000343B6 File Offset: 0x000325B6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x000343D5 File Offset: 0x000325D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x000343F4 File Offset: 0x000325F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x00034413 File Offset: 0x00032613
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x00034432 File Offset: 0x00032632
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x00034451 File Offset: 0x00032651
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x00034470 File Offset: 0x00032670
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0003448F File Offset: 0x0003268F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x000344AE File Offset: 0x000326AE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x000344CD File Offset: 0x000326CD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000344EC File Offset: 0x000326EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x0003450B File Offset: 0x0003270B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0003452A File Offset: 0x0003272A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00034549 File Offset: 0x00032749
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00034568 File Offset: 0x00032768
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00034587 File Offset: 0x00032787
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x000345A6 File Offset: 0x000327A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x000345C5 File Offset: 0x000327C5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x000345E4 File Offset: 0x000327E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x00034603 File Offset: 0x00032803
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x00034622 File Offset: 0x00032822
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x00034641 File Offset: 0x00032841
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x00034660 File Offset: 0x00032860
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0003467F File Offset: 0x0003287F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0003469E File Offset: 0x0003289E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x000346BD File Offset: 0x000328BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x000346DC File Offset: 0x000328DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x000346FB File Offset: 0x000328FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x0003471A File Offset: 0x0003291A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x00034739 File Offset: 0x00032939
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00034758 File Offset: 0x00032958
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060011F9 RID: 4601 RVA: 0x00034777 File Offset: 0x00032977
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x00034796 File Offset: 0x00032996
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x000347B5 File Offset: 0x000329B5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x000347D4 File Offset: 0x000329D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x000347F3 File Offset: 0x000329F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x00034812 File Offset: 0x00032A12
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x00034831 File Offset: 0x00032A31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x00034850 File Offset: 0x00032A50
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x0003486F File Offset: 0x00032A6F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x0003488E File Offset: 0x00032A8E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x000348AD File Offset: 0x00032AAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x000348CC File Offset: 0x00032ACC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x000348EB File Offset: 0x00032AEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0003490A File Offset: 0x00032B0A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x00034929 File Offset: 0x00032B29
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x00034948 File Offset: 0x00032B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x00034967 File Offset: 0x00032B67
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x00034986 File Offset: 0x00032B86
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x000349A5 File Offset: 0x00032BA5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x000349C4 File Offset: 0x00032BC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x000349E3 File Offset: 0x00032BE3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x00034A02 File Offset: 0x00032C02
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x00034A21 File Offset: 0x00032C21
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x00034A40 File Offset: 0x00032C40
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x00034A5F File Offset: 0x00032C5F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x00034A7E File Offset: 0x00032C7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x00034A9D File Offset: 0x00032C9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x00034ABC File Offset: 0x00032CBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x00034ADB File Offset: 0x00032CDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x00034AFA File Offset: 0x00032CFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.x);
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00034B13 File Offset: 0x00032D13
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.y);
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x00034B2C File Offset: 0x00032D2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.x, this.z);
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00034B45 File Offset: 0x00032D45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.x);
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x00034B5E File Offset: 0x00032D5E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.y, this.y);
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x00034B77 File Offset: 0x00032D77
		// (set) Token: 0x0600121C RID: 4636 RVA: 0x00034B90 File Offset: 0x00032D90
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

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x00034BB6 File Offset: 0x00032DB6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x00034BCF File Offset: 0x00032DCF
		// (set) Token: 0x0600121F RID: 4639 RVA: 0x00034BE8 File Offset: 0x00032DE8
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

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001220 RID: 4640 RVA: 0x00034C0E File Offset: 0x00032E0E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.x, this.z, this.z);
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00034C27 File Offset: 0x00032E27
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001222 RID: 4642 RVA: 0x00034C40 File Offset: 0x00032E40
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x00034C59 File Offset: 0x00032E59
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x00034C72 File Offset: 0x00032E72
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

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x00034C98 File Offset: 0x00032E98
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.x);
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x00034CB1 File Offset: 0x00032EB1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.y);
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x00034CCA File Offset: 0x00032ECA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.y, this.z);
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x00034CE3 File Offset: 0x00032EE3
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x00034CFC File Offset: 0x00032EFC
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

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x00034D22 File Offset: 0x00032F22
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.z, this.y);
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x00034D3B File Offset: 0x00032F3B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.y, this.z, this.z);
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x00034D54 File Offset: 0x00032F54
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.x, this.x);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x00034D6D File Offset: 0x00032F6D
		// (set) Token: 0x0600122E RID: 4654 RVA: 0x00034D86 File Offset: 0x00032F86
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

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00034DAC File Offset: 0x00032FAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.x, this.z);
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00034DC5 File Offset: 0x00032FC5
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x00034DDE File Offset: 0x00032FDE
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

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x00034E04 File Offset: 0x00033004
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.y, this.y);
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x00034E1D File Offset: 0x0003301D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.y, this.z);
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00034E36 File Offset: 0x00033036
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.z, this.x);
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x00034E4F File Offset: 0x0003304F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.z, this.y);
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x00034E68 File Offset: 0x00033068
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float3(this.z, this.z, this.z);
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x00034E81 File Offset: 0x00033081
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.x, this.x);
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x00034E94 File Offset: 0x00033094
		// (set) Token: 0x06001239 RID: 4665 RVA: 0x00034EA7 File Offset: 0x000330A7
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

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x00034EC1 File Offset: 0x000330C1
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x00034ED4 File Offset: 0x000330D4
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

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x00034EEE File Offset: 0x000330EE
		// (set) Token: 0x0600123D RID: 4669 RVA: 0x00034F01 File Offset: 0x00033101
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

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x00034F1B File Offset: 0x0003311B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.y, this.y);
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00034F2E File Offset: 0x0003312E
		// (set) Token: 0x06001240 RID: 4672 RVA: 0x00034F41 File Offset: 0x00033141
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

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00034F5B File Offset: 0x0003315B
		// (set) Token: 0x06001242 RID: 4674 RVA: 0x00034F6E File Offset: 0x0003316E
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

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x00034F88 File Offset: 0x00033188
		// (set) Token: 0x06001244 RID: 4676 RVA: 0x00034F9B File Offset: 0x0003319B
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

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001245 RID: 4677 RVA: 0x00034FB5 File Offset: 0x000331B5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new float2(this.z, this.z);
			}
		}

		// Token: 0x17000470 RID: 1136
		public unsafe float this[int index]
		{
			get
			{
				fixed (float3* ptr = &this)
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

		// Token: 0x06001248 RID: 4680 RVA: 0x00035000 File Offset: 0x00033200
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(float3 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00035030 File Offset: 0x00033230
		public override bool Equals(object o)
		{
			if (o is float3)
			{
				float3 @float = (float3)o;
				return this.Equals(@float);
			}
			return false;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00035055 File Offset: 0x00033255
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00035062 File Offset: 0x00033262
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("float3({0}f, {1}f, {2}f)", this.x, this.y, this.z);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0003508F File Offset: 0x0003328F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("float3({0}f, {1}f, {2}f)", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider), this.z.ToString(format, formatProvider));
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000350C2 File Offset: 0x000332C2
		public static implicit operator Vector3(float3 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000350DB File Offset: 0x000332DB
		public static implicit operator float3(Vector3 v)
		{
			return new float3(v.x, v.y, v.z);
		}

		// Token: 0x0400008C RID: 140
		public float x;

		// Token: 0x0400008D RID: 141
		public float y;

		// Token: 0x0400008E RID: 142
		public float z;

		// Token: 0x0400008F RID: 143
		public static readonly float3 zero;

		// Token: 0x0200005C RID: 92
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024C9 RID: 9417 RVA: 0x00069224 File Offset: 0x00067424
			public DebuggerProxy(float3 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			// Token: 0x0400015E RID: 350
			public float x;

			// Token: 0x0400015F RID: 351
			public float y;

			// Token: 0x04000160 RID: 352
			public float z;
		}
	}
}
