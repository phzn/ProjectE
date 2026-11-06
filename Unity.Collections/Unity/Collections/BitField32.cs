using System;
using System.Diagnostics;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x02000038 RID: 56
	[DebuggerTypeProxy(typeof(BitField32DebugView))]
	[GenerateTestsForBurstCompatibility]
	public struct BitField32
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00004B18 File Offset: 0x00002D18
		public BitField32(uint initialValue = 0U)
		{
			this.Value = initialValue;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004B21 File Offset: 0x00002D21
		public void Clear()
		{
			this.Value = 0U;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004B2A File Offset: 0x00002D2A
		public void SetBits(int pos, bool value)
		{
			this.Value = Bitwise.SetBits(this.Value, pos, 1U, value);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004B40 File Offset: 0x00002D40
		public void SetBits(int pos, bool value, int numBits)
		{
			uint num = uint.MaxValue >> 32 - numBits;
			this.Value = Bitwise.SetBits(this.Value, pos, num, value);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004B6C File Offset: 0x00002D6C
		public uint GetBits(int pos, int numBits = 1)
		{
			uint num = uint.MaxValue >> 32 - numBits;
			return Bitwise.ExtractBits(this.Value, pos, num);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004B90 File Offset: 0x00002D90
		public bool IsSet(int pos)
		{
			return this.GetBits(pos, 1) > 0U;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004B9D File Offset: 0x00002D9D
		public bool TestNone(int pos, int numBits = 1)
		{
			return this.GetBits(pos, numBits) == 0U;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004BAA File Offset: 0x00002DAA
		public bool TestAny(int pos, int numBits = 1)
		{
			return this.GetBits(pos, numBits) > 0U;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public bool TestAll(int pos, int numBits = 1)
		{
			uint num = uint.MaxValue >> 32 - numBits;
			return num == Bitwise.ExtractBits(this.Value, pos, num);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004BDF File Offset: 0x00002DDF
		public int CountBits()
		{
			return math.countbits(this.Value);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00004BEC File Offset: 0x00002DEC
		public int CountLeadingZeros()
		{
			return math.lzcnt(this.Value);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public int CountTrailingZeros()
		{
			return math.tzcnt(this.Value);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004C06 File Offset: 0x00002E06
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckArgs(int pos, int numBits)
		{
			if (pos > 31 || numBits == 0 || numBits > 32 || pos + numBits > 32)
			{
				throw new ArgumentException(string.Format("BitField32 invalid arguments: pos {0} (must be 0-31), numBits {1} (must be 1-32).", pos, numBits));
			}
		}

		// Token: 0x04000085 RID: 133
		public uint Value;
	}
}
