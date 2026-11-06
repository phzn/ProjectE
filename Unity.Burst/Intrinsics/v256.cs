using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Unity.Burst.Intrinsics
{
	// Token: 0x0200003A RID: 58
	[DebuggerTypeProxy(typeof(V256DebugView))]
	[StructLayout(LayoutKind.Explicit)]
	public struct v256
	{
		// Token: 0x06000A97 RID: 2711 RVA: 0x00007B6C File Offset: 0x00005D6C
		public v256(byte b)
		{
			this = default(v256);
			this.Byte31 = b;
			this.Byte30 = b;
			this.Byte29 = b;
			this.Byte28 = b;
			this.Byte27 = b;
			this.Byte26 = b;
			this.Byte25 = b;
			this.Byte24 = b;
			this.Byte23 = b;
			this.Byte22 = b;
			this.Byte21 = b;
			this.Byte20 = b;
			this.Byte19 = b;
			this.Byte18 = b;
			this.Byte17 = b;
			this.Byte16 = b;
			this.Byte15 = b;
			this.Byte14 = b;
			this.Byte13 = b;
			this.Byte12 = b;
			this.Byte11 = b;
			this.Byte10 = b;
			this.Byte9 = b;
			this.Byte8 = b;
			this.Byte7 = b;
			this.Byte6 = b;
			this.Byte5 = b;
			this.Byte4 = b;
			this.Byte3 = b;
			this.Byte2 = b;
			this.Byte1 = b;
			this.Byte0 = b;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00007CA0 File Offset: 0x00005EA0
		public v256(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k, byte l, byte m, byte n, byte o, byte p, byte q, byte r, byte s, byte t, byte u, byte v, byte w, byte x, byte y, byte z, byte A, byte B, byte C, byte D, byte E, byte F)
		{
			this = default(v256);
			this.Byte0 = a;
			this.Byte1 = b;
			this.Byte2 = c;
			this.Byte3 = d;
			this.Byte4 = e;
			this.Byte5 = f;
			this.Byte6 = g;
			this.Byte7 = h;
			this.Byte8 = i;
			this.Byte9 = j;
			this.Byte10 = k;
			this.Byte11 = l;
			this.Byte12 = m;
			this.Byte13 = n;
			this.Byte14 = o;
			this.Byte15 = p;
			this.Byte16 = q;
			this.Byte17 = r;
			this.Byte18 = s;
			this.Byte19 = t;
			this.Byte20 = u;
			this.Byte21 = v;
			this.Byte22 = w;
			this.Byte23 = x;
			this.Byte24 = y;
			this.Byte25 = z;
			this.Byte26 = A;
			this.Byte27 = B;
			this.Byte28 = C;
			this.Byte29 = D;
			this.Byte30 = E;
			this.Byte31 = F;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00007DB4 File Offset: 0x00005FB4
		public v256(sbyte b)
		{
			this = default(v256);
			this.SByte31 = b;
			this.SByte30 = b;
			this.SByte29 = b;
			this.SByte28 = b;
			this.SByte27 = b;
			this.SByte26 = b;
			this.SByte25 = b;
			this.SByte24 = b;
			this.SByte23 = b;
			this.SByte22 = b;
			this.SByte21 = b;
			this.SByte20 = b;
			this.SByte19 = b;
			this.SByte18 = b;
			this.SByte17 = b;
			this.SByte16 = b;
			this.SByte15 = b;
			this.SByte14 = b;
			this.SByte13 = b;
			this.SByte12 = b;
			this.SByte11 = b;
			this.SByte10 = b;
			this.SByte9 = b;
			this.SByte8 = b;
			this.SByte7 = b;
			this.SByte6 = b;
			this.SByte5 = b;
			this.SByte4 = b;
			this.SByte3 = b;
			this.SByte2 = b;
			this.SByte1 = b;
			this.SByte0 = b;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00007EE8 File Offset: 0x000060E8
		public v256(sbyte a, sbyte b, sbyte c, sbyte d, sbyte e, sbyte f, sbyte g, sbyte h, sbyte i, sbyte j, sbyte k, sbyte l, sbyte m, sbyte n, sbyte o, sbyte p, sbyte q, sbyte r, sbyte s, sbyte t, sbyte u, sbyte v, sbyte w, sbyte x, sbyte y, sbyte z, sbyte A, sbyte B, sbyte C, sbyte D, sbyte E, sbyte F)
		{
			this = default(v256);
			this.SByte0 = a;
			this.SByte1 = b;
			this.SByte2 = c;
			this.SByte3 = d;
			this.SByte4 = e;
			this.SByte5 = f;
			this.SByte6 = g;
			this.SByte7 = h;
			this.SByte8 = i;
			this.SByte9 = j;
			this.SByte10 = k;
			this.SByte11 = l;
			this.SByte12 = m;
			this.SByte13 = n;
			this.SByte14 = o;
			this.SByte15 = p;
			this.SByte16 = q;
			this.SByte17 = r;
			this.SByte18 = s;
			this.SByte19 = t;
			this.SByte20 = u;
			this.SByte21 = v;
			this.SByte22 = w;
			this.SByte23 = x;
			this.SByte24 = y;
			this.SByte25 = z;
			this.SByte26 = A;
			this.SByte27 = B;
			this.SByte28 = C;
			this.SByte29 = D;
			this.SByte30 = E;
			this.SByte31 = F;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00007FFC File Offset: 0x000061FC
		public v256(short v)
		{
			this = default(v256);
			this.SShort15 = v;
			this.SShort14 = v;
			this.SShort13 = v;
			this.SShort12 = v;
			this.SShort11 = v;
			this.SShort10 = v;
			this.SShort9 = v;
			this.SShort8 = v;
			this.SShort7 = v;
			this.SShort6 = v;
			this.SShort5 = v;
			this.SShort4 = v;
			this.SShort3 = v;
			this.SShort2 = v;
			this.SShort1 = v;
			this.SShort0 = v;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000080A0 File Offset: 0x000062A0
		public v256(short a, short b, short c, short d, short e, short f, short g, short h, short i, short j, short k, short l, short m, short n, short o, short p)
		{
			this = default(v256);
			this.SShort0 = a;
			this.SShort1 = b;
			this.SShort2 = c;
			this.SShort3 = d;
			this.SShort4 = e;
			this.SShort5 = f;
			this.SShort6 = g;
			this.SShort7 = h;
			this.SShort8 = i;
			this.SShort9 = j;
			this.SShort10 = k;
			this.SShort11 = l;
			this.SShort12 = m;
			this.SShort13 = n;
			this.SShort14 = o;
			this.SShort15 = p;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00008134 File Offset: 0x00006334
		public v256(ushort v)
		{
			this = default(v256);
			this.UShort15 = v;
			this.UShort14 = v;
			this.UShort13 = v;
			this.UShort12 = v;
			this.UShort11 = v;
			this.UShort10 = v;
			this.UShort9 = v;
			this.UShort8 = v;
			this.UShort7 = v;
			this.UShort6 = v;
			this.UShort5 = v;
			this.UShort4 = v;
			this.UShort3 = v;
			this.UShort2 = v;
			this.UShort1 = v;
			this.UShort0 = v;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000081D8 File Offset: 0x000063D8
		public v256(ushort a, ushort b, ushort c, ushort d, ushort e, ushort f, ushort g, ushort h, ushort i, ushort j, ushort k, ushort l, ushort m, ushort n, ushort o, ushort p)
		{
			this = default(v256);
			this.UShort0 = a;
			this.UShort1 = b;
			this.UShort2 = c;
			this.UShort3 = d;
			this.UShort4 = e;
			this.UShort5 = f;
			this.UShort6 = g;
			this.UShort7 = h;
			this.UShort8 = i;
			this.UShort9 = j;
			this.UShort10 = k;
			this.UShort11 = l;
			this.UShort12 = m;
			this.UShort13 = n;
			this.UShort14 = o;
			this.UShort15 = p;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0000826C File Offset: 0x0000646C
		public v256(int v)
		{
			this = default(v256);
			this.SInt7 = v;
			this.SInt6 = v;
			this.SInt5 = v;
			this.SInt4 = v;
			this.SInt3 = v;
			this.SInt2 = v;
			this.SInt1 = v;
			this.SInt0 = v;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000082C8 File Offset: 0x000064C8
		public v256(int a, int b, int c, int d, int e, int f, int g, int h)
		{
			this = default(v256);
			this.SInt0 = a;
			this.SInt1 = b;
			this.SInt2 = c;
			this.SInt3 = d;
			this.SInt4 = e;
			this.SInt5 = f;
			this.SInt6 = g;
			this.SInt7 = h;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0000831C File Offset: 0x0000651C
		public v256(uint v)
		{
			this = default(v256);
			this.UInt7 = v;
			this.UInt6 = v;
			this.UInt5 = v;
			this.UInt4 = v;
			this.UInt3 = v;
			this.UInt2 = v;
			this.UInt1 = v;
			this.UInt0 = v;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00008378 File Offset: 0x00006578
		public v256(uint a, uint b, uint c, uint d, uint e, uint f, uint g, uint h)
		{
			this = default(v256);
			this.UInt0 = a;
			this.UInt1 = b;
			this.UInt2 = c;
			this.UInt3 = d;
			this.UInt4 = e;
			this.UInt5 = f;
			this.UInt6 = g;
			this.UInt7 = h;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000083CC File Offset: 0x000065CC
		public v256(float f)
		{
			this = default(v256);
			this.Float7 = f;
			this.Float6 = f;
			this.Float5 = f;
			this.Float4 = f;
			this.Float3 = f;
			this.Float2 = f;
			this.Float1 = f;
			this.Float0 = f;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00008428 File Offset: 0x00006628
		public v256(float a, float b, float c, float d, float e, float f, float g, float h)
		{
			this = default(v256);
			this.Float0 = a;
			this.Float1 = b;
			this.Float2 = c;
			this.Float3 = d;
			this.Float4 = e;
			this.Float5 = f;
			this.Float6 = g;
			this.Float7 = h;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0000847C File Offset: 0x0000667C
		public v256(double f)
		{
			this = default(v256);
			this.Double3 = f;
			this.Double2 = f;
			this.Double1 = f;
			this.Double0 = f;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000084B2 File Offset: 0x000066B2
		public v256(double a, double b, double c, double d)
		{
			this = default(v256);
			this.Double0 = a;
			this.Double1 = b;
			this.Double2 = c;
			this.Double3 = d;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000084D8 File Offset: 0x000066D8
		public v256(long f)
		{
			this = default(v256);
			this.SLong3 = f;
			this.SLong2 = f;
			this.SLong1 = f;
			this.SLong0 = f;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0000850E File Offset: 0x0000670E
		public v256(long a, long b, long c, long d)
		{
			this = default(v256);
			this.SLong0 = a;
			this.SLong1 = b;
			this.SLong2 = c;
			this.SLong3 = d;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00008534 File Offset: 0x00006734
		public v256(ulong f)
		{
			this = default(v256);
			this.ULong3 = f;
			this.ULong2 = f;
			this.ULong1 = f;
			this.ULong0 = f;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0000856A File Offset: 0x0000676A
		public v256(ulong a, ulong b, ulong c, ulong d)
		{
			this = default(v256);
			this.ULong0 = a;
			this.ULong1 = b;
			this.ULong2 = c;
			this.ULong3 = d;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00008590 File Offset: 0x00006790
		public v256(v128 lo, v128 hi)
		{
			this = default(v256);
			this.Lo128 = lo;
			this.Hi128 = hi;
		}

		// Token: 0x040001C8 RID: 456
		[FieldOffset(0)]
		public byte Byte0;

		// Token: 0x040001C9 RID: 457
		[FieldOffset(1)]
		public byte Byte1;

		// Token: 0x040001CA RID: 458
		[FieldOffset(2)]
		public byte Byte2;

		// Token: 0x040001CB RID: 459
		[FieldOffset(3)]
		public byte Byte3;

		// Token: 0x040001CC RID: 460
		[FieldOffset(4)]
		public byte Byte4;

		// Token: 0x040001CD RID: 461
		[FieldOffset(5)]
		public byte Byte5;

		// Token: 0x040001CE RID: 462
		[FieldOffset(6)]
		public byte Byte6;

		// Token: 0x040001CF RID: 463
		[FieldOffset(7)]
		public byte Byte7;

		// Token: 0x040001D0 RID: 464
		[FieldOffset(8)]
		public byte Byte8;

		// Token: 0x040001D1 RID: 465
		[FieldOffset(9)]
		public byte Byte9;

		// Token: 0x040001D2 RID: 466
		[FieldOffset(10)]
		public byte Byte10;

		// Token: 0x040001D3 RID: 467
		[FieldOffset(11)]
		public byte Byte11;

		// Token: 0x040001D4 RID: 468
		[FieldOffset(12)]
		public byte Byte12;

		// Token: 0x040001D5 RID: 469
		[FieldOffset(13)]
		public byte Byte13;

		// Token: 0x040001D6 RID: 470
		[FieldOffset(14)]
		public byte Byte14;

		// Token: 0x040001D7 RID: 471
		[FieldOffset(15)]
		public byte Byte15;

		// Token: 0x040001D8 RID: 472
		[FieldOffset(16)]
		public byte Byte16;

		// Token: 0x040001D9 RID: 473
		[FieldOffset(17)]
		public byte Byte17;

		// Token: 0x040001DA RID: 474
		[FieldOffset(18)]
		public byte Byte18;

		// Token: 0x040001DB RID: 475
		[FieldOffset(19)]
		public byte Byte19;

		// Token: 0x040001DC RID: 476
		[FieldOffset(20)]
		public byte Byte20;

		// Token: 0x040001DD RID: 477
		[FieldOffset(21)]
		public byte Byte21;

		// Token: 0x040001DE RID: 478
		[FieldOffset(22)]
		public byte Byte22;

		// Token: 0x040001DF RID: 479
		[FieldOffset(23)]
		public byte Byte23;

		// Token: 0x040001E0 RID: 480
		[FieldOffset(24)]
		public byte Byte24;

		// Token: 0x040001E1 RID: 481
		[FieldOffset(25)]
		public byte Byte25;

		// Token: 0x040001E2 RID: 482
		[FieldOffset(26)]
		public byte Byte26;

		// Token: 0x040001E3 RID: 483
		[FieldOffset(27)]
		public byte Byte27;

		// Token: 0x040001E4 RID: 484
		[FieldOffset(28)]
		public byte Byte28;

		// Token: 0x040001E5 RID: 485
		[FieldOffset(29)]
		public byte Byte29;

		// Token: 0x040001E6 RID: 486
		[FieldOffset(30)]
		public byte Byte30;

		// Token: 0x040001E7 RID: 487
		[FieldOffset(31)]
		public byte Byte31;

		// Token: 0x040001E8 RID: 488
		[FieldOffset(0)]
		public sbyte SByte0;

		// Token: 0x040001E9 RID: 489
		[FieldOffset(1)]
		public sbyte SByte1;

		// Token: 0x040001EA RID: 490
		[FieldOffset(2)]
		public sbyte SByte2;

		// Token: 0x040001EB RID: 491
		[FieldOffset(3)]
		public sbyte SByte3;

		// Token: 0x040001EC RID: 492
		[FieldOffset(4)]
		public sbyte SByte4;

		// Token: 0x040001ED RID: 493
		[FieldOffset(5)]
		public sbyte SByte5;

		// Token: 0x040001EE RID: 494
		[FieldOffset(6)]
		public sbyte SByte6;

		// Token: 0x040001EF RID: 495
		[FieldOffset(7)]
		public sbyte SByte7;

		// Token: 0x040001F0 RID: 496
		[FieldOffset(8)]
		public sbyte SByte8;

		// Token: 0x040001F1 RID: 497
		[FieldOffset(9)]
		public sbyte SByte9;

		// Token: 0x040001F2 RID: 498
		[FieldOffset(10)]
		public sbyte SByte10;

		// Token: 0x040001F3 RID: 499
		[FieldOffset(11)]
		public sbyte SByte11;

		// Token: 0x040001F4 RID: 500
		[FieldOffset(12)]
		public sbyte SByte12;

		// Token: 0x040001F5 RID: 501
		[FieldOffset(13)]
		public sbyte SByte13;

		// Token: 0x040001F6 RID: 502
		[FieldOffset(14)]
		public sbyte SByte14;

		// Token: 0x040001F7 RID: 503
		[FieldOffset(15)]
		public sbyte SByte15;

		// Token: 0x040001F8 RID: 504
		[FieldOffset(16)]
		public sbyte SByte16;

		// Token: 0x040001F9 RID: 505
		[FieldOffset(17)]
		public sbyte SByte17;

		// Token: 0x040001FA RID: 506
		[FieldOffset(18)]
		public sbyte SByte18;

		// Token: 0x040001FB RID: 507
		[FieldOffset(19)]
		public sbyte SByte19;

		// Token: 0x040001FC RID: 508
		[FieldOffset(20)]
		public sbyte SByte20;

		// Token: 0x040001FD RID: 509
		[FieldOffset(21)]
		public sbyte SByte21;

		// Token: 0x040001FE RID: 510
		[FieldOffset(22)]
		public sbyte SByte22;

		// Token: 0x040001FF RID: 511
		[FieldOffset(23)]
		public sbyte SByte23;

		// Token: 0x04000200 RID: 512
		[FieldOffset(24)]
		public sbyte SByte24;

		// Token: 0x04000201 RID: 513
		[FieldOffset(25)]
		public sbyte SByte25;

		// Token: 0x04000202 RID: 514
		[FieldOffset(26)]
		public sbyte SByte26;

		// Token: 0x04000203 RID: 515
		[FieldOffset(27)]
		public sbyte SByte27;

		// Token: 0x04000204 RID: 516
		[FieldOffset(28)]
		public sbyte SByte28;

		// Token: 0x04000205 RID: 517
		[FieldOffset(29)]
		public sbyte SByte29;

		// Token: 0x04000206 RID: 518
		[FieldOffset(30)]
		public sbyte SByte30;

		// Token: 0x04000207 RID: 519
		[FieldOffset(31)]
		public sbyte SByte31;

		// Token: 0x04000208 RID: 520
		[FieldOffset(0)]
		public ushort UShort0;

		// Token: 0x04000209 RID: 521
		[FieldOffset(2)]
		public ushort UShort1;

		// Token: 0x0400020A RID: 522
		[FieldOffset(4)]
		public ushort UShort2;

		// Token: 0x0400020B RID: 523
		[FieldOffset(6)]
		public ushort UShort3;

		// Token: 0x0400020C RID: 524
		[FieldOffset(8)]
		public ushort UShort4;

		// Token: 0x0400020D RID: 525
		[FieldOffset(10)]
		public ushort UShort5;

		// Token: 0x0400020E RID: 526
		[FieldOffset(12)]
		public ushort UShort6;

		// Token: 0x0400020F RID: 527
		[FieldOffset(14)]
		public ushort UShort7;

		// Token: 0x04000210 RID: 528
		[FieldOffset(16)]
		public ushort UShort8;

		// Token: 0x04000211 RID: 529
		[FieldOffset(18)]
		public ushort UShort9;

		// Token: 0x04000212 RID: 530
		[FieldOffset(20)]
		public ushort UShort10;

		// Token: 0x04000213 RID: 531
		[FieldOffset(22)]
		public ushort UShort11;

		// Token: 0x04000214 RID: 532
		[FieldOffset(24)]
		public ushort UShort12;

		// Token: 0x04000215 RID: 533
		[FieldOffset(26)]
		public ushort UShort13;

		// Token: 0x04000216 RID: 534
		[FieldOffset(28)]
		public ushort UShort14;

		// Token: 0x04000217 RID: 535
		[FieldOffset(30)]
		public ushort UShort15;

		// Token: 0x04000218 RID: 536
		[FieldOffset(0)]
		public short SShort0;

		// Token: 0x04000219 RID: 537
		[FieldOffset(2)]
		public short SShort1;

		// Token: 0x0400021A RID: 538
		[FieldOffset(4)]
		public short SShort2;

		// Token: 0x0400021B RID: 539
		[FieldOffset(6)]
		public short SShort3;

		// Token: 0x0400021C RID: 540
		[FieldOffset(8)]
		public short SShort4;

		// Token: 0x0400021D RID: 541
		[FieldOffset(10)]
		public short SShort5;

		// Token: 0x0400021E RID: 542
		[FieldOffset(12)]
		public short SShort6;

		// Token: 0x0400021F RID: 543
		[FieldOffset(14)]
		public short SShort7;

		// Token: 0x04000220 RID: 544
		[FieldOffset(16)]
		public short SShort8;

		// Token: 0x04000221 RID: 545
		[FieldOffset(18)]
		public short SShort9;

		// Token: 0x04000222 RID: 546
		[FieldOffset(20)]
		public short SShort10;

		// Token: 0x04000223 RID: 547
		[FieldOffset(22)]
		public short SShort11;

		// Token: 0x04000224 RID: 548
		[FieldOffset(24)]
		public short SShort12;

		// Token: 0x04000225 RID: 549
		[FieldOffset(26)]
		public short SShort13;

		// Token: 0x04000226 RID: 550
		[FieldOffset(28)]
		public short SShort14;

		// Token: 0x04000227 RID: 551
		[FieldOffset(30)]
		public short SShort15;

		// Token: 0x04000228 RID: 552
		[FieldOffset(0)]
		public uint UInt0;

		// Token: 0x04000229 RID: 553
		[FieldOffset(4)]
		public uint UInt1;

		// Token: 0x0400022A RID: 554
		[FieldOffset(8)]
		public uint UInt2;

		// Token: 0x0400022B RID: 555
		[FieldOffset(12)]
		public uint UInt3;

		// Token: 0x0400022C RID: 556
		[FieldOffset(16)]
		public uint UInt4;

		// Token: 0x0400022D RID: 557
		[FieldOffset(20)]
		public uint UInt5;

		// Token: 0x0400022E RID: 558
		[FieldOffset(24)]
		public uint UInt6;

		// Token: 0x0400022F RID: 559
		[FieldOffset(28)]
		public uint UInt7;

		// Token: 0x04000230 RID: 560
		[FieldOffset(0)]
		public int SInt0;

		// Token: 0x04000231 RID: 561
		[FieldOffset(4)]
		public int SInt1;

		// Token: 0x04000232 RID: 562
		[FieldOffset(8)]
		public int SInt2;

		// Token: 0x04000233 RID: 563
		[FieldOffset(12)]
		public int SInt3;

		// Token: 0x04000234 RID: 564
		[FieldOffset(16)]
		public int SInt4;

		// Token: 0x04000235 RID: 565
		[FieldOffset(20)]
		public int SInt5;

		// Token: 0x04000236 RID: 566
		[FieldOffset(24)]
		public int SInt6;

		// Token: 0x04000237 RID: 567
		[FieldOffset(28)]
		public int SInt7;

		// Token: 0x04000238 RID: 568
		[FieldOffset(0)]
		public ulong ULong0;

		// Token: 0x04000239 RID: 569
		[FieldOffset(8)]
		public ulong ULong1;

		// Token: 0x0400023A RID: 570
		[FieldOffset(16)]
		public ulong ULong2;

		// Token: 0x0400023B RID: 571
		[FieldOffset(24)]
		public ulong ULong3;

		// Token: 0x0400023C RID: 572
		[FieldOffset(0)]
		public long SLong0;

		// Token: 0x0400023D RID: 573
		[FieldOffset(8)]
		public long SLong1;

		// Token: 0x0400023E RID: 574
		[FieldOffset(16)]
		public long SLong2;

		// Token: 0x0400023F RID: 575
		[FieldOffset(24)]
		public long SLong3;

		// Token: 0x04000240 RID: 576
		[FieldOffset(0)]
		public float Float0;

		// Token: 0x04000241 RID: 577
		[FieldOffset(4)]
		public float Float1;

		// Token: 0x04000242 RID: 578
		[FieldOffset(8)]
		public float Float2;

		// Token: 0x04000243 RID: 579
		[FieldOffset(12)]
		public float Float3;

		// Token: 0x04000244 RID: 580
		[FieldOffset(16)]
		public float Float4;

		// Token: 0x04000245 RID: 581
		[FieldOffset(20)]
		public float Float5;

		// Token: 0x04000246 RID: 582
		[FieldOffset(24)]
		public float Float6;

		// Token: 0x04000247 RID: 583
		[FieldOffset(28)]
		public float Float7;

		// Token: 0x04000248 RID: 584
		[FieldOffset(0)]
		public double Double0;

		// Token: 0x04000249 RID: 585
		[FieldOffset(8)]
		public double Double1;

		// Token: 0x0400024A RID: 586
		[FieldOffset(16)]
		public double Double2;

		// Token: 0x0400024B RID: 587
		[FieldOffset(24)]
		public double Double3;

		// Token: 0x0400024C RID: 588
		[FieldOffset(0)]
		public v128 Lo128;

		// Token: 0x0400024D RID: 589
		[FieldOffset(16)]
		public v128 Hi128;
	}
}
