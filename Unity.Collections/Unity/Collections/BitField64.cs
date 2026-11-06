using System;
using System.Diagnostics;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x0200003A RID: 58
	[DebuggerTypeProxy(typeof(BitField64DebugView))]
	[GenerateTestsForBurstCompatibility]
	public struct BitField64
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00004C7A File Offset: 0x00002E7A
		public BitField64(ulong initialValue = 0UL)
		{
			this.Value = initialValue;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00004C83 File Offset: 0x00002E83
		public void Clear()
		{
			this.Value = 0UL;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00004C8D File Offset: 0x00002E8D
		public void SetBits(int pos, bool value)
		{
			this.Value = Bitwise.SetBits(this.Value, pos, 1UL, value);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004CA4 File Offset: 0x00002EA4
		public void SetBits(int pos, bool value, int numBits = 1)
		{
			ulong num = ulong.MaxValue >> 64 - numBits;
			this.Value = Bitwise.SetBits(this.Value, pos, num, value);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public ulong GetBits(int pos, int numBits = 1)
		{
			ulong num = ulong.MaxValue >> 64 - numBits;
			return Bitwise.ExtractBits(this.Value, pos, num);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004CF5 File Offset: 0x00002EF5
		public bool IsSet(int pos)
		{
			return this.GetBits(pos, 1) > 0UL;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004D03 File Offset: 0x00002F03
		public bool TestNone(int pos, int numBits = 1)
		{
			return this.GetBits(pos, numBits) == 0UL;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004D11 File Offset: 0x00002F11
		public bool TestAny(int pos, int numBits = 1)
		{
			return this.GetBits(pos, numBits) > 0UL;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004D20 File Offset: 0x00002F20
		public bool TestAll(int pos, int numBits = 1)
		{
			ulong num = ulong.MaxValue >> 64 - numBits;
			return num == Bitwise.ExtractBits(this.Value, pos, num);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004D48 File Offset: 0x00002F48
		public int CountBits()
		{
			return math.countbits(this.Value);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004D55 File Offset: 0x00002F55
		public int CountLeadingZeros()
		{
			return math.lzcnt(this.Value);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00004D62 File Offset: 0x00002F62
		public int CountTrailingZeros()
		{
			return math.tzcnt(this.Value);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00004D6F File Offset: 0x00002F6F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckArgs(int pos, int numBits)
		{
			if (pos > 63 || numBits == 0 || numBits > 64 || pos + numBits > 64)
			{
				throw new ArgumentException(string.Format("BitField32 invalid arguments: pos {0} (must be 0-63), numBits {1} (must be 1-64).", pos, numBits));
			}
		}

		// Token: 0x04000087 RID: 135
		public ulong Value;
	}
}
