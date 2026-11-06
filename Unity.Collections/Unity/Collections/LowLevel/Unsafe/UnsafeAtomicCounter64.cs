using System;
using System.Threading;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000105 RID: 261
	[GenerateTestsForBurstCompatibility]
	public struct UnsafeAtomicCounter64
	{
		// Token: 0x06000B10 RID: 2832 RVA: 0x000219CC File Offset: 0x0001FBCC
		public unsafe UnsafeAtomicCounter64(void* ptr)
		{
			this.Counter = (long*)ptr;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000219D5 File Offset: 0x0001FBD5
		public unsafe void Reset(long value = 0L)
		{
			*this.Counter = value;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000219DF File Offset: 0x0001FBDF
		public unsafe long Add(long value)
		{
			return Interlocked.Add(UnsafeUtility.AsRef<long>((void*)this.Counter), value) - value;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x000219F4 File Offset: 0x0001FBF4
		public long Sub(long value)
		{
			return this.Add(-value);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00021A00 File Offset: 0x0001FC00
		public unsafe long AddSat(long value, long max = 9223372036854775807L)
		{
			long num = *this.Counter;
			long num2;
			do
			{
				num2 = num;
				num = ((num >= max) ? max : math.min(max, num + value));
				num = Interlocked.CompareExchange(UnsafeUtility.AsRef<long>((void*)this.Counter), num, num2);
			}
			while (num2 != num && num2 != max);
			return num2;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00021A44 File Offset: 0x0001FC44
		public unsafe long SubSat(long value, long min = -9223372036854775808L)
		{
			long num = *this.Counter;
			long num2;
			do
			{
				num2 = num;
				num = ((num <= min) ? min : math.max(min, num - value));
				num = Interlocked.CompareExchange(UnsafeUtility.AsRef<long>((void*)this.Counter), num, num2);
			}
			while (num2 != num && num2 != min);
			return num2;
		}

		// Token: 0x040004A5 RID: 1189
		public unsafe long* Counter;
	}
}
