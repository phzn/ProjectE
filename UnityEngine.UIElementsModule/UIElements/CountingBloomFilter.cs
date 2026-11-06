using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.UIElements
{
	// Token: 0x02000150 RID: 336
	internal struct CountingBloomFilter
	{
		// Token: 0x06000AEB RID: 2795 RVA: 0x0002C120 File Offset: 0x0002A320
		private unsafe void AdjustSlot(uint index, bool increment)
		{
			if (increment)
			{
				bool flag = *((ref this.m_Counters.FixedElementField) + (UIntPtr)index) != byte.MaxValue;
				if (flag)
				{
					ref byte ptr = (ref this.m_Counters.FixedElementField) + (UIntPtr)index;
					ptr += 1;
				}
			}
			else
			{
				bool flag2 = *((ref this.m_Counters.FixedElementField) + (UIntPtr)index) > 0;
				if (flag2)
				{
					ref byte ptr2 = (ref this.m_Counters.FixedElementField) + (UIntPtr)index;
					ptr2 -= 1;
				}
			}
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0002C194 File Offset: 0x0002A394
		private uint Hash1(uint hash)
		{
			return hash & 16383U;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0002C1B0 File Offset: 0x0002A3B0
		private uint Hash2(uint hash)
		{
			return (hash >> 14) & 16383U;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0002C1CC File Offset: 0x0002A3CC
		private unsafe bool IsSlotEmpty(uint index)
		{
			return *((ref this.m_Counters.FixedElementField) + (UIntPtr)index) == 0;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0002C1F0 File Offset: 0x0002A3F0
		public void InsertHash(uint hash)
		{
			this.AdjustSlot(this.Hash1(hash), true);
			this.AdjustSlot(this.Hash2(hash), true);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002C211 File Offset: 0x0002A411
		public void RemoveHash(uint hash)
		{
			this.AdjustSlot(this.Hash1(hash), false);
			this.AdjustSlot(this.Hash2(hash), false);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0002C234 File Offset: 0x0002A434
		public bool ContainsHash(uint hash)
		{
			return !this.IsSlotEmpty(this.Hash1(hash)) && !this.IsSlotEmpty(this.Hash2(hash));
		}

		// Token: 0x04000536 RID: 1334
		private const int KEY_SIZE = 14;

		// Token: 0x04000537 RID: 1335
		private const uint ARRAY_SIZE = 16384U;

		// Token: 0x04000538 RID: 1336
		private const int KEY_MASK = 16383;

		// Token: 0x04000539 RID: 1337
		[FixedBuffer(typeof(byte), 16384)]
		private CountingBloomFilter.<m_Counters>e__FixedBuffer m_Counters;

		// Token: 0x02000151 RID: 337
		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 16384)]
		public struct <m_Counters>e__FixedBuffer
		{
			// Token: 0x0400053A RID: 1338
			public byte FixedElementField;
		}
	}
}
