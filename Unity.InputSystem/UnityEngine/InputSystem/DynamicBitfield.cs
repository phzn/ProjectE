using System;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000062 RID: 98
	internal struct DynamicBitfield
	{
		// Token: 0x060009C5 RID: 2501 RVA: 0x00036AF0 File Offset: 0x00034CF0
		public void SetLength(int newLength)
		{
			int num = DynamicBitfield.BitCountToULongCount(newLength);
			if (this.array.length < num)
			{
				this.array.SetLength(num);
			}
			this.length = newLength;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00036B28 File Offset: 0x00034D28
		public void SetBit(int bitIndex)
		{
			ref InlinedArray<ulong> ptr = ref this.array;
			int num = bitIndex / 64;
			ptr[num] |= 1UL << bitIndex % 64;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00036B5A File Offset: 0x00034D5A
		public bool TestBit(int bitIndex)
		{
			return (this.array[bitIndex / 64] & (1UL << bitIndex % 64)) > 0UL;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00036B7C File Offset: 0x00034D7C
		public void ClearBit(int bitIndex)
		{
			ref InlinedArray<ulong> ptr = ref this.array;
			int num = bitIndex / 64;
			ptr[num] &= ~(1UL << bitIndex % 64);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00036BB0 File Offset: 0x00034DB0
		public bool AnyBitIsSet()
		{
			for (int i = 0; i < this.array.length; i++)
			{
				if (this.array[i] != 0UL)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00036BE4 File Offset: 0x00034DE4
		private static int BitCountToULongCount(int bitCount)
		{
			return (bitCount + 63) / 64;
		}

		// Token: 0x04000337 RID: 823
		public InlinedArray<ulong> array;

		// Token: 0x04000338 RID: 824
		public int length;
	}
}
