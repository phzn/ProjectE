using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Unity.Burst.Intrinsics
{
	// Token: 0x02000039 RID: 57
	[DebuggerTypeProxy(typeof(V128DebugView))]
	[StructLayout(LayoutKind.Explicit)]
	public struct v128
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x000075BC File Offset: 0x000057BC
		public v128(byte b)
		{
			this = default(v128);
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

		// Token: 0x06000A83 RID: 2691 RVA: 0x00007660 File Offset: 0x00005860
		public v128(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k, byte l, byte m, byte n, byte o, byte p)
		{
			this = default(v128);
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
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000076F4 File Offset: 0x000058F4
		public v128(sbyte b)
		{
			this = default(v128);
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

		// Token: 0x06000A85 RID: 2693 RVA: 0x00007798 File Offset: 0x00005998
		public v128(sbyte a, sbyte b, sbyte c, sbyte d, sbyte e, sbyte f, sbyte g, sbyte h, sbyte i, sbyte j, sbyte k, sbyte l, sbyte m, sbyte n, sbyte o, sbyte p)
		{
			this = default(v128);
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
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0000782C File Offset: 0x00005A2C
		public v128(short v)
		{
			this = default(v128);
			this.SShort7 = v;
			this.SShort6 = v;
			this.SShort5 = v;
			this.SShort4 = v;
			this.SShort3 = v;
			this.SShort2 = v;
			this.SShort1 = v;
			this.SShort0 = v;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00007888 File Offset: 0x00005A88
		public v128(short a, short b, short c, short d, short e, short f, short g, short h)
		{
			this = default(v128);
			this.SShort0 = a;
			this.SShort1 = b;
			this.SShort2 = c;
			this.SShort3 = d;
			this.SShort4 = e;
			this.SShort5 = f;
			this.SShort6 = g;
			this.SShort7 = h;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000078DC File Offset: 0x00005ADC
		public v128(ushort v)
		{
			this = default(v128);
			this.UShort7 = v;
			this.UShort6 = v;
			this.UShort5 = v;
			this.UShort4 = v;
			this.UShort3 = v;
			this.UShort2 = v;
			this.UShort1 = v;
			this.UShort0 = v;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00007938 File Offset: 0x00005B38
		public v128(ushort a, ushort b, ushort c, ushort d, ushort e, ushort f, ushort g, ushort h)
		{
			this = default(v128);
			this.UShort0 = a;
			this.UShort1 = b;
			this.UShort2 = c;
			this.UShort3 = d;
			this.UShort4 = e;
			this.UShort5 = f;
			this.UShort6 = g;
			this.UShort7 = h;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0000798C File Offset: 0x00005B8C
		public v128(int v)
		{
			this = default(v128);
			this.SInt3 = v;
			this.SInt2 = v;
			this.SInt1 = v;
			this.SInt0 = v;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000079C2 File Offset: 0x00005BC2
		public v128(int a, int b, int c, int d)
		{
			this = default(v128);
			this.SInt0 = a;
			this.SInt1 = b;
			this.SInt2 = c;
			this.SInt3 = d;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000079E8 File Offset: 0x00005BE8
		public v128(uint v)
		{
			this = default(v128);
			this.UInt3 = v;
			this.UInt2 = v;
			this.UInt1 = v;
			this.UInt0 = v;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00007A1E File Offset: 0x00005C1E
		public v128(uint a, uint b, uint c, uint d)
		{
			this = default(v128);
			this.UInt0 = a;
			this.UInt1 = b;
			this.UInt2 = c;
			this.UInt3 = d;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00007A44 File Offset: 0x00005C44
		public v128(float f)
		{
			this = default(v128);
			this.Float3 = f;
			this.Float2 = f;
			this.Float1 = f;
			this.Float0 = f;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00007A7A File Offset: 0x00005C7A
		public v128(float a, float b, float c, float d)
		{
			this = default(v128);
			this.Float0 = a;
			this.Float1 = b;
			this.Float2 = c;
			this.Float3 = d;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00007AA0 File Offset: 0x00005CA0
		public v128(double f)
		{
			this = default(v128);
			this.Double1 = f;
			this.Double0 = f;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00007AC4 File Offset: 0x00005CC4
		public v128(double a, double b)
		{
			this = default(v128);
			this.Double0 = a;
			this.Double1 = b;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00007ADC File Offset: 0x00005CDC
		public v128(long f)
		{
			this = default(v128);
			this.SLong1 = f;
			this.SLong0 = f;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00007B00 File Offset: 0x00005D00
		public v128(long a, long b)
		{
			this = default(v128);
			this.SLong0 = a;
			this.SLong1 = b;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00007B18 File Offset: 0x00005D18
		public v128(ulong f)
		{
			this = default(v128);
			this.ULong1 = f;
			this.ULong0 = f;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00007B3C File Offset: 0x00005D3C
		public v128(ulong a, ulong b)
		{
			this = default(v128);
			this.ULong0 = a;
			this.ULong1 = b;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00007B53 File Offset: 0x00005D53
		public v128(v64 lo, v64 hi)
		{
			this = default(v128);
			this.Lo64 = lo;
			this.Hi64 = hi;
		}

		// Token: 0x04000184 RID: 388
		[FieldOffset(0)]
		public byte Byte0;

		// Token: 0x04000185 RID: 389
		[FieldOffset(1)]
		public byte Byte1;

		// Token: 0x04000186 RID: 390
		[FieldOffset(2)]
		public byte Byte2;

		// Token: 0x04000187 RID: 391
		[FieldOffset(3)]
		public byte Byte3;

		// Token: 0x04000188 RID: 392
		[FieldOffset(4)]
		public byte Byte4;

		// Token: 0x04000189 RID: 393
		[FieldOffset(5)]
		public byte Byte5;

		// Token: 0x0400018A RID: 394
		[FieldOffset(6)]
		public byte Byte6;

		// Token: 0x0400018B RID: 395
		[FieldOffset(7)]
		public byte Byte7;

		// Token: 0x0400018C RID: 396
		[FieldOffset(8)]
		public byte Byte8;

		// Token: 0x0400018D RID: 397
		[FieldOffset(9)]
		public byte Byte9;

		// Token: 0x0400018E RID: 398
		[FieldOffset(10)]
		public byte Byte10;

		// Token: 0x0400018F RID: 399
		[FieldOffset(11)]
		public byte Byte11;

		// Token: 0x04000190 RID: 400
		[FieldOffset(12)]
		public byte Byte12;

		// Token: 0x04000191 RID: 401
		[FieldOffset(13)]
		public byte Byte13;

		// Token: 0x04000192 RID: 402
		[FieldOffset(14)]
		public byte Byte14;

		// Token: 0x04000193 RID: 403
		[FieldOffset(15)]
		public byte Byte15;

		// Token: 0x04000194 RID: 404
		[FieldOffset(0)]
		public sbyte SByte0;

		// Token: 0x04000195 RID: 405
		[FieldOffset(1)]
		public sbyte SByte1;

		// Token: 0x04000196 RID: 406
		[FieldOffset(2)]
		public sbyte SByte2;

		// Token: 0x04000197 RID: 407
		[FieldOffset(3)]
		public sbyte SByte3;

		// Token: 0x04000198 RID: 408
		[FieldOffset(4)]
		public sbyte SByte4;

		// Token: 0x04000199 RID: 409
		[FieldOffset(5)]
		public sbyte SByte5;

		// Token: 0x0400019A RID: 410
		[FieldOffset(6)]
		public sbyte SByte6;

		// Token: 0x0400019B RID: 411
		[FieldOffset(7)]
		public sbyte SByte7;

		// Token: 0x0400019C RID: 412
		[FieldOffset(8)]
		public sbyte SByte8;

		// Token: 0x0400019D RID: 413
		[FieldOffset(9)]
		public sbyte SByte9;

		// Token: 0x0400019E RID: 414
		[FieldOffset(10)]
		public sbyte SByte10;

		// Token: 0x0400019F RID: 415
		[FieldOffset(11)]
		public sbyte SByte11;

		// Token: 0x040001A0 RID: 416
		[FieldOffset(12)]
		public sbyte SByte12;

		// Token: 0x040001A1 RID: 417
		[FieldOffset(13)]
		public sbyte SByte13;

		// Token: 0x040001A2 RID: 418
		[FieldOffset(14)]
		public sbyte SByte14;

		// Token: 0x040001A3 RID: 419
		[FieldOffset(15)]
		public sbyte SByte15;

		// Token: 0x040001A4 RID: 420
		[FieldOffset(0)]
		public ushort UShort0;

		// Token: 0x040001A5 RID: 421
		[FieldOffset(2)]
		public ushort UShort1;

		// Token: 0x040001A6 RID: 422
		[FieldOffset(4)]
		public ushort UShort2;

		// Token: 0x040001A7 RID: 423
		[FieldOffset(6)]
		public ushort UShort3;

		// Token: 0x040001A8 RID: 424
		[FieldOffset(8)]
		public ushort UShort4;

		// Token: 0x040001A9 RID: 425
		[FieldOffset(10)]
		public ushort UShort5;

		// Token: 0x040001AA RID: 426
		[FieldOffset(12)]
		public ushort UShort6;

		// Token: 0x040001AB RID: 427
		[FieldOffset(14)]
		public ushort UShort7;

		// Token: 0x040001AC RID: 428
		[FieldOffset(0)]
		public short SShort0;

		// Token: 0x040001AD RID: 429
		[FieldOffset(2)]
		public short SShort1;

		// Token: 0x040001AE RID: 430
		[FieldOffset(4)]
		public short SShort2;

		// Token: 0x040001AF RID: 431
		[FieldOffset(6)]
		public short SShort3;

		// Token: 0x040001B0 RID: 432
		[FieldOffset(8)]
		public short SShort4;

		// Token: 0x040001B1 RID: 433
		[FieldOffset(10)]
		public short SShort5;

		// Token: 0x040001B2 RID: 434
		[FieldOffset(12)]
		public short SShort6;

		// Token: 0x040001B3 RID: 435
		[FieldOffset(14)]
		public short SShort7;

		// Token: 0x040001B4 RID: 436
		[FieldOffset(0)]
		public uint UInt0;

		// Token: 0x040001B5 RID: 437
		[FieldOffset(4)]
		public uint UInt1;

		// Token: 0x040001B6 RID: 438
		[FieldOffset(8)]
		public uint UInt2;

		// Token: 0x040001B7 RID: 439
		[FieldOffset(12)]
		public uint UInt3;

		// Token: 0x040001B8 RID: 440
		[FieldOffset(0)]
		public int SInt0;

		// Token: 0x040001B9 RID: 441
		[FieldOffset(4)]
		public int SInt1;

		// Token: 0x040001BA RID: 442
		[FieldOffset(8)]
		public int SInt2;

		// Token: 0x040001BB RID: 443
		[FieldOffset(12)]
		public int SInt3;

		// Token: 0x040001BC RID: 444
		[FieldOffset(0)]
		public ulong ULong0;

		// Token: 0x040001BD RID: 445
		[FieldOffset(8)]
		public ulong ULong1;

		// Token: 0x040001BE RID: 446
		[FieldOffset(0)]
		public long SLong0;

		// Token: 0x040001BF RID: 447
		[FieldOffset(8)]
		public long SLong1;

		// Token: 0x040001C0 RID: 448
		[FieldOffset(0)]
		public float Float0;

		// Token: 0x040001C1 RID: 449
		[FieldOffset(4)]
		public float Float1;

		// Token: 0x040001C2 RID: 450
		[FieldOffset(8)]
		public float Float2;

		// Token: 0x040001C3 RID: 451
		[FieldOffset(12)]
		public float Float3;

		// Token: 0x040001C4 RID: 452
		[FieldOffset(0)]
		public double Double0;

		// Token: 0x040001C5 RID: 453
		[FieldOffset(8)]
		public double Double1;

		// Token: 0x040001C6 RID: 454
		[FieldOffset(0)]
		public v64 Lo64;

		// Token: 0x040001C7 RID: 455
		[FieldOffset(8)]
		public v64 Hi64;
	}
}
