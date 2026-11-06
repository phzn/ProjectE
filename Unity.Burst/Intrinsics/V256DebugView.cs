using System;
using System.Diagnostics;

namespace Unity.Burst.Intrinsics
{
	// Token: 0x02000038 RID: 56
	internal class V256DebugView
	{
		// Token: 0x06000A77 RID: 2679 RVA: 0x00006D8F File Offset: 0x00004F8F
		public V256DebugView(v256 value)
		{
			this.m_Value = value;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00006DA0 File Offset: 0x00004FA0
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
					this.m_Value.Byte7,
					this.m_Value.Byte8,
					this.m_Value.Byte9,
					this.m_Value.Byte10,
					this.m_Value.Byte11,
					this.m_Value.Byte12,
					this.m_Value.Byte13,
					this.m_Value.Byte14,
					this.m_Value.Byte15,
					this.m_Value.Byte16,
					this.m_Value.Byte17,
					this.m_Value.Byte18,
					this.m_Value.Byte19,
					this.m_Value.Byte20,
					this.m_Value.Byte21,
					this.m_Value.Byte22,
					this.m_Value.Byte23,
					this.m_Value.Byte24,
					this.m_Value.Byte25,
					this.m_Value.Byte26,
					this.m_Value.Byte27,
					this.m_Value.Byte28,
					this.m_Value.Byte29,
					this.m_Value.Byte30,
					this.m_Value.Byte31
				};
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00006F8C File Offset: 0x0000518C
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
					this.m_Value.SByte7,
					this.m_Value.SByte8,
					this.m_Value.SByte9,
					this.m_Value.SByte10,
					this.m_Value.SByte11,
					this.m_Value.SByte12,
					this.m_Value.SByte13,
					this.m_Value.SByte14,
					this.m_Value.SByte15,
					this.m_Value.SByte16,
					this.m_Value.SByte17,
					this.m_Value.SByte18,
					this.m_Value.SByte19,
					this.m_Value.SByte20,
					this.m_Value.SByte21,
					this.m_Value.SByte22,
					this.m_Value.SByte23,
					this.m_Value.SByte24,
					this.m_Value.SByte25,
					this.m_Value.SByte26,
					this.m_Value.SByte27,
					this.m_Value.SByte28,
					this.m_Value.SByte29,
					this.m_Value.SByte30,
					this.m_Value.SByte31
				};
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00007178 File Offset: 0x00005378
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
					this.m_Value.UShort3,
					this.m_Value.UShort4,
					this.m_Value.UShort5,
					this.m_Value.UShort6,
					this.m_Value.UShort7,
					this.m_Value.UShort8,
					this.m_Value.UShort9,
					this.m_Value.UShort10,
					this.m_Value.UShort11,
					this.m_Value.UShort12,
					this.m_Value.UShort13,
					this.m_Value.UShort14,
					this.m_Value.UShort15
				};
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00007274 File Offset: 0x00005474
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
					this.m_Value.SShort3,
					this.m_Value.SShort4,
					this.m_Value.SShort5,
					this.m_Value.SShort6,
					this.m_Value.SShort7,
					this.m_Value.SShort8,
					this.m_Value.SShort9,
					this.m_Value.SShort10,
					this.m_Value.SShort11,
					this.m_Value.SShort12,
					this.m_Value.SShort13,
					this.m_Value.SShort14,
					this.m_Value.SShort15
				};
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00007370 File Offset: 0x00005570
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public uint[] UInt
		{
			get
			{
				return new uint[]
				{
					this.m_Value.UInt0,
					this.m_Value.UInt1,
					this.m_Value.UInt2,
					this.m_Value.UInt3,
					this.m_Value.UInt4,
					this.m_Value.UInt5,
					this.m_Value.UInt6,
					this.m_Value.UInt7
				};
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x000073F4 File Offset: 0x000055F4
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public int[] SInt
		{
			get
			{
				return new int[]
				{
					this.m_Value.SInt0,
					this.m_Value.SInt1,
					this.m_Value.SInt2,
					this.m_Value.SInt3,
					this.m_Value.SInt4,
					this.m_Value.SInt5,
					this.m_Value.SInt6,
					this.m_Value.SInt7
				};
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00007478 File Offset: 0x00005678
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public float[] Float
		{
			get
			{
				return new float[]
				{
					this.m_Value.Float0,
					this.m_Value.Float1,
					this.m_Value.Float2,
					this.m_Value.Float3,
					this.m_Value.Float4,
					this.m_Value.Float5,
					this.m_Value.Float6,
					this.m_Value.Float7
				};
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x000074FB File Offset: 0x000056FB
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public long[] SLong
		{
			get
			{
				return new long[]
				{
					this.m_Value.SLong0,
					this.m_Value.SLong1,
					this.m_Value.SLong2,
					this.m_Value.SLong3
				};
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0000753B File Offset: 0x0000573B
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public ulong[] ULong
		{
			get
			{
				return new ulong[]
				{
					this.m_Value.ULong0,
					this.m_Value.ULong1,
					this.m_Value.ULong2,
					this.m_Value.ULong3
				};
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0000757B File Offset: 0x0000577B
		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public double[] Double
		{
			get
			{
				return new double[]
				{
					this.m_Value.Double0,
					this.m_Value.Double1,
					this.m_Value.Double2,
					this.m_Value.Double3
				};
			}
		}

		// Token: 0x04000183 RID: 387
		private v256 m_Value;
	}
}
