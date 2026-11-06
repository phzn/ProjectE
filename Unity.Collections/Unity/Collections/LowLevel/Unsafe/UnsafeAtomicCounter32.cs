using System;
using System.Threading;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000104 RID: 260
	[GenerateTestsForBurstCompatibility]
	public struct UnsafeAtomicCounter32
	{
		// Token: 0x06000B0A RID: 2826 RVA: 0x00021912 File Offset: 0x0001FB12
		public unsafe UnsafeAtomicCounter32(void* ptr)
		{
			this.Counter = (int*)ptr;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0002191B File Offset: 0x0001FB1B
		public unsafe void Reset(int value = 0)
		{
			*this.Counter = value;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00021925 File Offset: 0x0001FB25
		public unsafe int Add(int value)
		{
			return Interlocked.Add(UnsafeUtility.AsRef<int>((void*)this.Counter), value) - value;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002193A File Offset: 0x0001FB3A
		public int Sub(int value)
		{
			return this.Add(-value);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00021944 File Offset: 0x0001FB44
		public unsafe int AddSat(int value, int max = 2147483647)
		{
			int num = *this.Counter;
			int num2;
			do
			{
				num2 = num;
				num = ((num >= max) ? max : math.min(max, num + value));
				num = Interlocked.CompareExchange(UnsafeUtility.AsRef<int>((void*)this.Counter), num, num2);
			}
			while (num2 != num && num2 != max);
			return num2;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00021988 File Offset: 0x0001FB88
		public unsafe int SubSat(int value, int min = -2147483648)
		{
			int num = *this.Counter;
			int num2;
			do
			{
				num2 = num;
				num = ((num <= min) ? min : math.max(min, num - value));
				num = Interlocked.CompareExchange(UnsafeUtility.AsRef<int>((void*)this.Counter), num, num2);
			}
			while (num2 != num && num2 != min);
			return num2;
		}

		// Token: 0x040004A4 RID: 1188
		public unsafe int* Counter;
	}
}
