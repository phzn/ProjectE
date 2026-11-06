using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200002C RID: 44
	[DebuggerTypeProxy(typeof(half3.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct half3 : IEquatable<half3>, IFormattable
	{
		// Token: 0x06001628 RID: 5672 RVA: 0x0003FC7E File Offset: 0x0003DE7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(half x, half y, half z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0003FC95 File Offset: 0x0003DE95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(half x, half2 yz)
		{
			this.x = x;
			this.y = yz.x;
			this.z = yz.y;
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0003FCB6 File Offset: 0x0003DEB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(half2 xy, half z)
		{
			this.x = xy.x;
			this.y = xy.y;
			this.z = z;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0003FCD7 File Offset: 0x0003DED7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(half3 xyz)
		{
			this.x = xyz.x;
			this.y = xyz.y;
			this.z = xyz.z;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0003FCFD File Offset: 0x0003DEFD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(half v)
		{
			this.x = v;
			this.y = v;
			this.z = v;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0003FD14 File Offset: 0x0003DF14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(float v)
		{
			this.x = (half)v;
			this.y = (half)v;
			this.z = (half)v;
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0003FD3A File Offset: 0x0003DF3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(float3 v)
		{
			this.x = (half)v.x;
			this.y = (half)v.y;
			this.z = (half)v.z;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0003FD6F File Offset: 0x0003DF6F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(double v)
		{
			this.x = (half)v;
			this.y = (half)v;
			this.z = (half)v;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0003FD95 File Offset: 0x0003DF95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public half3(double3 v)
		{
			this.x = (half)v.x;
			this.y = (half)v.y;
			this.z = (half)v.z;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0003FDCA File Offset: 0x0003DFCA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator half3(half v)
		{
			return new half3(v);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0003FDD2 File Offset: 0x0003DFD2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half3(float v)
		{
			return new half3(v);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0003FDDA File Offset: 0x0003DFDA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half3(float3 v)
		{
			return new half3(v);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0003FDE2 File Offset: 0x0003DFE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half3(double v)
		{
			return new half3(v);
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0003FDEA File Offset: 0x0003DFEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator half3(double3 v)
		{
			return new half3(v);
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0003FDF2 File Offset: 0x0003DFF2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(half3 lhs, half3 rhs)
		{
			return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0003FE2C File Offset: 0x0003E02C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(half3 lhs, half rhs)
		{
			return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0003FE57 File Offset: 0x0003E057
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator ==(half lhs, half3 rhs)
		{
			return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0003FE82 File Offset: 0x0003E082
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(half3 lhs, half3 rhs)
		{
			return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0003FEBC File Offset: 0x0003E0BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(half3 lhs, half rhs)
		{
			return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0003FEE7 File Offset: 0x0003E0E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3 operator !=(half lhs, half3 rhs)
		{
			return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x0003FF12 File Offset: 0x0003E112
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x0003FF31 File Offset: 0x0003E131
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x0003FF50 File Offset: 0x0003E150
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.x, this.z);
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x0600163F RID: 5695 RVA: 0x0003FF6F File Offset: 0x0003E16F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x0003FF8E File Offset: 0x0003E18E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x0003FFAD File Offset: 0x0003E1AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.y, this.z);
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x0003FFCC File Offset: 0x0003E1CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.z, this.x);
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x0003FFEB File Offset: 0x0003E1EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.z, this.y);
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x0004000A File Offset: 0x0003E20A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.x, this.z, this.z);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x00040029 File Offset: 0x0003E229
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001646 RID: 5702 RVA: 0x00040048 File Offset: 0x0003E248
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x00040067 File Offset: 0x0003E267
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.x, this.z);
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001648 RID: 5704 RVA: 0x00040086 File Offset: 0x0003E286
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001649 RID: 5705 RVA: 0x000400A5 File Offset: 0x0003E2A5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x000400C4 File Offset: 0x0003E2C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.y, this.z);
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600164B RID: 5707 RVA: 0x000400E3 File Offset: 0x0003E2E3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.z, this.x);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x00040102 File Offset: 0x0003E302
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.z, this.y);
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x00040121 File Offset: 0x0003E321
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.y, this.z, this.z);
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x00040140 File Offset: 0x0003E340
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.x, this.x);
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x0600164F RID: 5711 RVA: 0x0004015F File Offset: 0x0003E35F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.x, this.y);
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x0004017E File Offset: 0x0003E37E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.x, this.z);
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001651 RID: 5713 RVA: 0x0004019D File Offset: 0x0003E39D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.y, this.x);
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x000401BC File Offset: 0x0003E3BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001653 RID: 5715 RVA: 0x000401DB File Offset: 0x0003E3DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x000401FA File Offset: 0x0003E3FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001655 RID: 5717 RVA: 0x00040219 File Offset: 0x0003E419
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x00040238 File Offset: 0x0003E438
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 xzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.x, this.z, this.z, this.z);
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x00040257 File Offset: 0x0003E457
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x00040276 File Offset: 0x0003E476
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001659 RID: 5721 RVA: 0x00040295 File Offset: 0x0003E495
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x000402B4 File Offset: 0x0003E4B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600165B RID: 5723 RVA: 0x000402D3 File Offset: 0x0003E4D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x000402F2 File Offset: 0x0003E4F2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.y, this.z);
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600165D RID: 5725 RVA: 0x00040311 File Offset: 0x0003E511
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.z, this.x);
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600165E RID: 5726 RVA: 0x00040330 File Offset: 0x0003E530
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.z, this.y);
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600165F RID: 5727 RVA: 0x0004034F File Offset: 0x0003E54F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.x, this.z, this.z);
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001660 RID: 5728 RVA: 0x0004036E File Offset: 0x0003E56E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001661 RID: 5729 RVA: 0x0004038D File Offset: 0x0003E58D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x000403AC File Offset: 0x0003E5AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.x, this.z);
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x000403CB File Offset: 0x0003E5CB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001664 RID: 5732 RVA: 0x000403EA File Offset: 0x0003E5EA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x00040409 File Offset: 0x0003E609
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.y, this.z);
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001666 RID: 5734 RVA: 0x00040428 File Offset: 0x0003E628
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.z, this.x);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001667 RID: 5735 RVA: 0x00040447 File Offset: 0x0003E647
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x00040466 File Offset: 0x0003E666
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001669 RID: 5737 RVA: 0x00040485 File Offset: 0x0003E685
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x000404A4 File Offset: 0x0003E6A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x000404C3 File Offset: 0x0003E6C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x000404E2 File Offset: 0x0003E6E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.y, this.x);
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x00040501 File Offset: 0x0003E701
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.y, this.y);
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x00040520 File Offset: 0x0003E720
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.y, this.z);
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x0004053F File Offset: 0x0003E73F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.z, this.x);
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x0004055E File Offset: 0x0003E75E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.z, this.y);
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x0004057D File Offset: 0x0003E77D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 yzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.y, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x0004059C File Offset: 0x0003E79C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.x, this.x);
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x000405BB File Offset: 0x0003E7BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.x, this.y);
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x000405DA File Offset: 0x0003E7DA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.x, this.z);
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x000405F9 File Offset: 0x0003E7F9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.y, this.x);
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001676 RID: 5750 RVA: 0x00040618 File Offset: 0x0003E818
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.y, this.y);
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x00040637 File Offset: 0x0003E837
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.y, this.z);
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00040656 File Offset: 0x0003E856
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.z, this.x);
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x00040675 File Offset: 0x0003E875
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.z, this.y);
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x00040694 File Offset: 0x0003E894
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zxzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.x, this.z, this.z);
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x000406B3 File Offset: 0x0003E8B3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.x, this.x);
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x000406D2 File Offset: 0x0003E8D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.x, this.y);
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600167D RID: 5757 RVA: 0x000406F1 File Offset: 0x0003E8F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.x, this.z);
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x00040710 File Offset: 0x0003E910
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.y, this.x);
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600167F RID: 5759 RVA: 0x0004072F File Offset: 0x0003E92F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.y, this.y);
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x0004074E File Offset: 0x0003E94E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.y, this.z);
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x0004076D File Offset: 0x0003E96D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.z, this.x);
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x0004078C File Offset: 0x0003E98C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.z, this.y);
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x000407AB File Offset: 0x0003E9AB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zyzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.y, this.z, this.z);
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001684 RID: 5764 RVA: 0x000407CA File Offset: 0x0003E9CA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.x, this.x);
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x000407E9 File Offset: 0x0003E9E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.x, this.y);
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x00040808 File Offset: 0x0003EA08
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.x, this.z);
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x00040827 File Offset: 0x0003EA27
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.y, this.x);
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x00040846 File Offset: 0x0003EA46
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.y, this.y);
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x00040865 File Offset: 0x0003EA65
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.y, this.z);
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x00040884 File Offset: 0x0003EA84
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.z, this.x);
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x0600168B RID: 5771 RVA: 0x000408A3 File Offset: 0x0003EAA3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.z, this.y);
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x000408C2 File Offset: 0x0003EAC2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half4 zzzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half4(this.z, this.z, this.z, this.z);
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600168D RID: 5773 RVA: 0x000408E1 File Offset: 0x0003EAE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.x);
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x000408FA File Offset: 0x0003EAFA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.y);
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600168F RID: 5775 RVA: 0x00040913 File Offset: 0x0003EB13
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.x, this.z);
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x0004092C File Offset: 0x0003EB2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.x);
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001691 RID: 5777 RVA: 0x00040945 File Offset: 0x0003EB45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.y);
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x0004095E File Offset: 0x0003EB5E
		// (set) Token: 0x06001693 RID: 5779 RVA: 0x00040977 File Offset: 0x0003EB77
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001694 RID: 5780 RVA: 0x0004099D File Offset: 0x0003EB9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.z, this.x);
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001695 RID: 5781 RVA: 0x000409B6 File Offset: 0x0003EBB6
		// (set) Token: 0x06001696 RID: 5782 RVA: 0x000409CF File Offset: 0x0003EBCF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x000409F5 File Offset: 0x0003EBF5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 xzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.x, this.z, this.z);
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001698 RID: 5784 RVA: 0x00040A0E File Offset: 0x0003EC0E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.x);
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001699 RID: 5785 RVA: 0x00040A27 File Offset: 0x0003EC27
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.y);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x0600169A RID: 5786 RVA: 0x00040A40 File Offset: 0x0003EC40
		// (set) Token: 0x0600169B RID: 5787 RVA: 0x00040A59 File Offset: 0x0003EC59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
				this.z = value.z;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x0600169C RID: 5788 RVA: 0x00040A7F File Offset: 0x0003EC7F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.x);
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x0600169D RID: 5789 RVA: 0x00040A98 File Offset: 0x0003EC98
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.y);
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x00040AB1 File Offset: 0x0003ECB1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.y, this.z);
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x00040ACA File Offset: 0x0003ECCA
		// (set) Token: 0x060016A0 RID: 5792 RVA: 0x00040AE3 File Offset: 0x0003ECE3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x00040B09 File Offset: 0x0003ED09
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.z, this.y);
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00040B22 File Offset: 0x0003ED22
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 yzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.y, this.z, this.z);
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060016A3 RID: 5795 RVA: 0x00040B3B File Offset: 0x0003ED3B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.x, this.x);
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00040B54 File Offset: 0x0003ED54
		// (set) Token: 0x060016A5 RID: 5797 RVA: 0x00040B6D File Offset: 0x0003ED6D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
				this.y = value.z;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x00040B93 File Offset: 0x0003ED93
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zxz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.x, this.z);
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060016A7 RID: 5799 RVA: 0x00040BAC File Offset: 0x0003EDAC
		// (set) Token: 0x060016A8 RID: 5800 RVA: 0x00040BC5 File Offset: 0x0003EDC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
				this.x = value.z;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060016A9 RID: 5801 RVA: 0x00040BEB File Offset: 0x0003EDEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.y, this.y);
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060016AA RID: 5802 RVA: 0x00040C04 File Offset: 0x0003EE04
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zyz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.y, this.z);
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060016AB RID: 5803 RVA: 0x00040C1D File Offset: 0x0003EE1D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zzx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.z, this.x);
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x00040C36 File Offset: 0x0003EE36
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zzy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.z, this.y);
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060016AD RID: 5805 RVA: 0x00040C4F File Offset: 0x0003EE4F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half3 zzz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half3(this.z, this.z, this.z);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060016AE RID: 5806 RVA: 0x00040C68 File Offset: 0x0003EE68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.x);
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x00040C7B File Offset: 0x0003EE7B
		// (set) Token: 0x060016B0 RID: 5808 RVA: 0x00040C8E File Offset: 0x0003EE8E
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

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x00040CA8 File Offset: 0x0003EEA8
		// (set) Token: 0x060016B2 RID: 5810 RVA: 0x00040CBB File Offset: 0x0003EEBB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 xz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.x, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00040CD5 File Offset: 0x0003EED5
		// (set) Token: 0x060016B4 RID: 5812 RVA: 0x00040CE8 File Offset: 0x0003EEE8
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

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x00040D02 File Offset: 0x0003EF02
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.y);
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x00040D15 File Offset: 0x0003EF15
		// (set) Token: 0x060016B7 RID: 5815 RVA: 0x00040D28 File Offset: 0x0003EF28
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 yz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.y, this.z);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.z = value.y;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x00040D42 File Offset: 0x0003EF42
		// (set) Token: 0x060016B9 RID: 5817 RVA: 0x00040D55 File Offset: 0x0003EF55
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 zx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.z, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060016BA RID: 5818 RVA: 0x00040D6F File Offset: 0x0003EF6F
		// (set) Token: 0x060016BB RID: 5819 RVA: 0x00040D82 File Offset: 0x0003EF82
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 zy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.z, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.z = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060016BC RID: 5820 RVA: 0x00040D9C File Offset: 0x0003EF9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public half2 zz
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new half2(this.z, this.z);
			}
		}

		// Token: 0x1700065E RID: 1630
		public unsafe half this[int index]
		{
			get
			{
				fixed (half3* ptr = &this)
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

		// Token: 0x060016BF RID: 5823 RVA: 0x00040DF9 File Offset: 0x0003EFF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(half3 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y && this.z == rhs.z;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00040E34 File Offset: 0x0003F034
		public override bool Equals(object o)
		{
			if (o is half3)
			{
				half3 half = (half3)o;
				return this.Equals(half);
			}
			return false;
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00040E59 File Offset: 0x0003F059
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00040E66 File Offset: 0x0003F066
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("half3({0}, {1}, {2})", this.x, this.y, this.z);
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00040E93 File Offset: 0x0003F093
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("half3({0}, {1}, {2})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider), this.z.ToString(format, formatProvider));
		}

		// Token: 0x040000B4 RID: 180
		public half x;

		// Token: 0x040000B5 RID: 181
		public half y;

		// Token: 0x040000B6 RID: 182
		public half z;

		// Token: 0x040000B7 RID: 183
		public static readonly half3 zero;

		// Token: 0x0200005F RID: 95
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024CC RID: 9420 RVA: 0x000692A8 File Offset: 0x000674A8
			public DebuggerProxy(half3 v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			// Token: 0x04000167 RID: 359
			public half x;

			// Token: 0x04000168 RID: 360
			public half y;

			// Token: 0x04000169 RID: 361
			public half z;
		}
	}
}
