using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200002B RID: 43
	[DebuggerTypeProxy(typeof(half2.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct half2 : IEquatable<half2>, IFormattable
	{
		// Token: 0x060015F1 RID: 5617 RVA: 0x0003F6AC File Offset: 0x0003D8AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half2(half x, half y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0003F6BC File Offset: 0x0003D8BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half2(half2 xy)
		{
			this.x = xy.x;
			this.y = xy.y;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0003F6D6 File Offset: 0x0003D8D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half2(half v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0003F6E6 File Offset: 0x0003D8E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half2(float v)
		{
			this.x = (half)v;
			this.y = (half)v;
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0003F700 File Offset: 0x0003D900
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half2(float2 v)
		{
			this.x = (half)v.x;
			this.y = (half)v.y;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0003F724 File Offset: 0x0003D924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half2(double v)
		{
			this.x = (half)v;
			this.y = (half)v;
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0003F73E File Offset: 0x0003D93E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half2(double2 v)
		{
			this.x = (half)v.x;
			this.y = (half)v.y;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0003F762 File Offset: 0x0003D962
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator half2(half v)
		{
			return new half2(v);
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0003F76A File Offset: 0x0003D96A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half2(float v)
		{
			return new half2(v);
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0003F772 File Offset: 0x0003D972
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half2(float2 v)
		{
			return new half2(v);
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0003F77A File Offset: 0x0003D97A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half2(double v)
		{
			return new half2(v);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0003F782 File Offset: 0x0003D982
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half2(double2 v)
		{
			return new half2(v);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0003F78A File Offset: 0x0003D98A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(half2 lhs, half2 rhs)
		{
			return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0003F7B3 File Offset: 0x0003D9B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(half2 lhs, half rhs)
		{
			return new bool2(lhs.x == rhs, lhs.y == rhs);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0003F7D2 File Offset: 0x0003D9D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(half lhs, half2 rhs)
		{
			return new bool2(lhs == rhs.x, lhs == rhs.y);
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0003F7F1 File Offset: 0x0003D9F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(half2 lhs, half2 rhs)
		{
			return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0003F81A File Offset: 0x0003DA1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(half2 lhs, half rhs)
		{
			return new bool2(lhs.x != rhs, lhs.y != rhs);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0003F839 File Offset: 0x0003DA39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(half lhs, half2 rhs)
		{
			return new bool2(lhs != rhs.x, lhs != rhs.y);
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001603 RID: 5635 RVA: 0x0003F858 File Offset: 0x0003DA58
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x0003F877 File Offset: 0x0003DA77
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001605 RID: 5637 RVA: 0x0003F896 File Offset: 0x0003DA96
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x0003F8B5 File Offset: 0x0003DAB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x0003F8D4 File Offset: 0x0003DAD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x0003F8F3 File Offset: 0x0003DAF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x0003F912 File Offset: 0x0003DB12
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x0003F931 File Offset: 0x0003DB31
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600160B RID: 5643 RVA: 0x0003F950 File Offset: 0x0003DB50
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x0003F96F File Offset: 0x0003DB6F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600160D RID: 5645 RVA: 0x0003F98E File Offset: 0x0003DB8E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x0003F9AD File Offset: 0x0003DBAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x0003F9CC File Offset: 0x0003DBCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x0003F9EB File Offset: 0x0003DBEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x0003FA0A File Offset: 0x0003DC0A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x0003FA29 File Offset: 0x0003DC29
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001613 RID: 5651 RVA: 0x0003FA48 File Offset: 0x0003DC48
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.x);
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x0003FA61 File Offset: 0x0003DC61
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.y);
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001615 RID: 5653 RVA: 0x0003FA7A File Offset: 0x0003DC7A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.x);
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x0003FA93 File Offset: 0x0003DC93
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.y);
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001617 RID: 5655 RVA: 0x0003FAAC File Offset: 0x0003DCAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.x);
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x0003FAC5 File Offset: 0x0003DCC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.y);
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001619 RID: 5657 RVA: 0x0003FADE File Offset: 0x0003DCDE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.x);
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600161A RID: 5658 RVA: 0x0003FAF7 File Offset: 0x0003DCF7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.y);
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600161B RID: 5659 RVA: 0x0003FB10 File Offset: 0x0003DD10
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.x);
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x0003FB23 File Offset: 0x0003DD23
		// (set) Token: 0x0600161D RID: 5661 RVA: 0x0003FB36 File Offset: 0x0003DD36
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x0003FB50 File Offset: 0x0003DD50
		// (set) Token: 0x0600161F RID: 5663 RVA: 0x0003FB63 File Offset: 0x0003DD63
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x0003FB7D File Offset: 0x0003DD7D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.y);
			}
		}

		// Token: 0x170005E8 RID: 1512
		public unsafe half this[int index]
		{
			get
			{
				fixed (half2* ptr = &this)
				{
					return ((half*)ptr)[index];
				}
			}
			set
			{
				fixed (half* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0003FBD9 File Offset: 0x0003DDD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(half2 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0003FC04 File Offset: 0x0003DE04
		public override bool Equals(object o)
		{
			if (o is half2)
			{
				half2 half = (half2)o;
				return this.Equals(half);
			}
			return false;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0003FC29 File Offset: 0x0003DE29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0003FC36 File Offset: 0x0003DE36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("half2({0}, {1})", this.x, this.y);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0003FC58 File Offset: 0x0003DE58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("half2({0}, {1})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider));
		}

		// Token: 0x040000B1 RID: 177
		public half x;

		// Token: 0x040000B2 RID: 178
		public half y;

		// Token: 0x040000B3 RID: 179
		public static readonly half2 zero;

		// Token: 0x0200005E RID: 94
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024CB RID: 9419 RVA: 0x00069288 File Offset: 0x00067488
			public DebuggerProxy(half2 v)
			{
				this.x = v.x;
				this.y = v.y;
			}

			// Token: 0x04000165 RID: 357
			public half x;

			// Token: 0x04000166 RID: 358
			public half y;
		}
	}
}
