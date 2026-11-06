using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Unity.Burst.Intrinsics
{
	// Token: 0x0200003B RID: 59
	[DebuggerTypeProxy(typeof(V64DebugView))]
	[StructLayout(LayoutKind.Explicit)]
	public struct v64
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x000085A8 File Offset: 0x000067A8
		public v64(byte b)
		{
			this = default(v64);
			this.Byte7 = b;
			this.Byte6 = b;
			this.Byte5 = b;
			this.Byte4 = b;
			this.Byte3 = b;
			this.Byte2 = b;
			this.Byte1 = b;
			this.Byte0 = b;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00008604 File Offset: 0x00006804
		public v64(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
		{
			this = default(v64);
			this.Byte0 = a;
			this.Byte1 = b;
			this.Byte2 = c;
			this.Byte3 = d;
			this.Byte4 = e;
			this.Byte5 = f;
			this.Byte6 = g;
			this.Byte7 = h;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00008658 File Offset: 0x00006858
		public v64(sbyte b)
		{
			this = default(v64);
			this.SByte7 = b;
			this.SByte6 = b;
			this.SByte5 = b;
			this.SByte4 = b;
			this.SByte3 = b;
			this.SByte2 = b;
			this.SByte1 = b;
			this.SByte0 = b;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000086B4 File Offset: 0x000068B4
		public v64(sbyte a, sbyte b, sbyte c, sbyte d, sbyte e, sbyte f, sbyte g, sbyte h)
		{
			this = default(v64);
			this.SByte0 = a;
			this.SByte1 = b;
			this.SByte2 = c;
			this.SByte3 = d;
			this.SByte4 = e;
			this.SByte5 = f;
			this.SByte6 = g;
			this.SByte7 = h;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00008708 File Offset: 0x00006908
		public v64(short v)
		{
			this = default(v64);
			this.SShort3 = v;
			this.SShort2 = v;
			this.SShort1 = v;
			this.SShort0 = v;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0000873E File Offset: 0x0000693E
		public v64(short a, short b, short c, short d)
		{
			this = default(v64);
			this.SShort0 = a;
			this.SShort1 = b;
			this.SShort2 = c;
			this.SShort3 = d;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00008764 File Offset: 0x00006964
		public v64(ushort v)
		{
			this = default(v64);
			this.UShort3 = v;
			this.UShort2 = v;
			this.UShort1 = v;
			this.UShort0 = v;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0000879A File Offset: 0x0000699A
		public v64(ushort a, ushort b, ushort c, ushort d)
		{
			this = default(v64);
			this.UShort0 = a;
			this.UShort1 = b;
			this.UShort2 = c;
			this.UShort3 = d;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000087C0 File Offset: 0x000069C0
		public v64(int v)
		{
			this = default(v64);
			this.SInt1 = v;
			this.SInt0 = v;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000087E4 File Offset: 0x000069E4
		public v64(int a, int b)
		{
			this = default(v64);
			this.SInt0 = a;
			this.SInt1 = b;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000087FC File Offset: 0x000069FC
		public v64(uint v)
		{
			this = default(v64);
			this.UInt1 = v;
			this.UInt0 = v;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00008820 File Offset: 0x00006A20
		public v64(uint a, uint b)
		{
			this = default(v64);
			this.UInt0 = a;
			this.UInt1 = b;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00008838 File Offset: 0x00006A38
		public v64(float f)
		{
			this = default(v64);
			this.Float1 = f;
			this.Float0 = f;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0000885C File Offset: 0x00006A5C
		public v64(float a, float b)
		{
			this = default(v64);
			this.Float0 = a;
			this.Float1 = b;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00008873 File Offset: 0x00006A73
		public v64(double a)
		{
			this = default(v64);
			this.Double0 = a;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00008883 File Offset: 0x00006A83
		public v64(long a)
		{
			this = default(v64);
			this.SLong0 = a;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00008893 File Offset: 0x00006A93
		public v64(ulong a)
		{
			this = default(v64);
			this.ULong0 = a;
		}

		// Token: 0x0400024E RID: 590
		[FieldOffset(0)]
		public byte Byte0;

		// Token: 0x0400024F RID: 591
		[FieldOffset(1)]
		public byte Byte1;

		// Token: 0x04000250 RID: 592
		[FieldOffset(2)]
		public byte Byte2;

		// Token: 0x04000251 RID: 593
		[FieldOffset(3)]
		public byte Byte3;

		// Token: 0x04000252 RID: 594
		[FieldOffset(4)]
		public byte Byte4;

		// Token: 0x04000253 RID: 595
		[FieldOffset(5)]
		public byte Byte5;

		// Token: 0x04000254 RID: 596
		[FieldOffset(6)]
		public byte Byte6;

		// Token: 0x04000255 RID: 597
		[FieldOffset(7)]
		public byte Byte7;

		// Token: 0x04000256 RID: 598
		[FieldOffset(0)]
		public sbyte SByte0;

		// Token: 0x04000257 RID: 599
		[FieldOffset(1)]
		public sbyte SByte1;

		// Token: 0x04000258 RID: 600
		[FieldOffset(2)]
		public sbyte SByte2;

		// Token: 0x04000259 RID: 601
		[FieldOffset(3)]
		public sbyte SByte3;

		// Token: 0x0400025A RID: 602
		[FieldOffset(4)]
		public sbyte SByte4;

		// Token: 0x0400025B RID: 603
		[FieldOffset(5)]
		public sbyte SByte5;

		// Token: 0x0400025C RID: 604
		[FieldOffset(6)]
		public sbyte SByte6;

		// Token: 0x0400025D RID: 605
		[FieldOffset(7)]
		public sbyte SByte7;

		// Token: 0x0400025E RID: 606
		[FieldOffset(0)]
		public ushort UShort0;

		// Token: 0x0400025F RID: 607
		[FieldOffset(2)]
		public ushort UShort1;

		// Token: 0x04000260 RID: 608
		[FieldOffset(4)]
		public ushort UShort2;

		// Token: 0x04000261 RID: 609
		[FieldOffset(6)]
		public ushort UShort3;

		// Token: 0x04000262 RID: 610
		[FieldOffset(0)]
		public short SShort0;

		// Token: 0x04000263 RID: 611
		[FieldOffset(2)]
		public short SShort1;

		// Token: 0x04000264 RID: 612
		[FieldOffset(4)]
		public short SShort2;

		// Token: 0x04000265 RID: 613
		[FieldOffset(6)]
		public short SShort3;

		// Token: 0x04000266 RID: 614
		[FieldOffset(0)]
		public uint UInt0;

		// Token: 0x04000267 RID: 615
		[FieldOffset(4)]
		public uint UInt1;

		// Token: 0x04000268 RID: 616
		[FieldOffset(0)]
		public int SInt0;

		// Token: 0x04000269 RID: 617
		[FieldOffset(4)]
		public int SInt1;

		// Token: 0x0400026A RID: 618
		[FieldOffset(0)]
		public ulong ULong0;

		// Token: 0x0400026B RID: 619
		[FieldOffset(0)]
		public long SLong0;

		// Token: 0x0400026C RID: 620
		[FieldOffset(0)]
		public float Float0;

		// Token: 0x0400026D RID: 621
		[FieldOffset(4)]
		public float Float1;

		// Token: 0x0400026E RID: 622
		[FieldOffset(0)]
		public double Double0;
	}
}
