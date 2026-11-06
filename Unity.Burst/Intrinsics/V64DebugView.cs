using System;
using System.Diagnostics;

namespace Unity.Burst.Intrinsics
{
	// Token: 0x02000036 RID: 54
	internal class V64DebugView
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x00006710 File Offset: 0x00004910
		public V64DebugView(v64 value)
		{
			this.m_Value = value;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00006720 File Offset: 0x00004920
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public byte[] Byte
		{
			get
			{
				return new byte[]
				{
					this.m_Value.Byte0,
					this.m_Value.Byte1,
					this.m_Value.Byte2,
					this.m_Value.Byte3,
					this.m_Value.Byte4,
					this.m_Value.Byte5,
					this.m_Value.Byte6,
					this.m_Value.Byte7
				};
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000067A4 File Offset: 0x000049A4
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public sbyte[] SByte
		{
			get
			{
				return new sbyte[]
				{
					this.m_Value.SByte0,
					this.m_Value.SByte1,
					this.m_Value.SByte2,
					this.m_Value.SByte3,
					this.m_Value.SByte4,
					this.m_Value.SByte5,
					this.m_Value.SByte6,
					this.m_Value.SByte7
				};
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x00006827 File Offset: 0x00004A27
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public ushort[] UShort
		{
			get
			{
				return new ushort[]
				{
					this.m_Value.UShort0,
					this.m_Value.UShort1,
					this.m_Value.UShort2,
					this.m_Value.UShort3
				};
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x00006867 File Offset: 0x00004A67
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public short[] SShort
		{
			get
			{
				return new short[]
				{
					this.m_Value.SShort0,
					this.m_Value.SShort1,
					this.m_Value.SShort2,
					this.m_Value.SShort3
				};
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x000068A7 File Offset: 0x00004AA7
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public uint[] UInt
		{
			get
			{
				return new uint[]
				{
					this.m_Value.UInt0,
					this.m_Value.UInt1
				};
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x000068CB File Offset: 0x00004ACB
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public int[] SInt
		{
			get
			{
				return new int[]
				{
					this.m_Value.SInt0,
					this.m_Value.SInt1
				};
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x000068EF File Offset: 0x00004AEF
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public float[] Float
		{
			get
			{
				return new float[]
				{
					this.m_Value.Float0,
					this.m_Value.Float1
				};
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00006913 File Offset: 0x00004B13
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public long[] SLong
		{
			get
			{
				return new long[] { this.m_Value.SLong0 };
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00006929 File Offset: 0x00004B29
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public ulong[] ULong
		{
			get
			{
				return new ulong[] { this.m_Value.ULong0 };
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0000693F File Offset: 0x00004B3F
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public double[] Double
		{
			get
			{
				return new double[] { this.m_Value.Double0 };
			}
		}

		// Token: 0x04000181 RID: 385
		private v64 m_Value;
	}
}
