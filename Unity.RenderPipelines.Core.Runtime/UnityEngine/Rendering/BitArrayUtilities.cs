using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000CE RID: 206
	public static class BitArrayUtilities
	{
		// Token: 0x060006A3 RID: 1699 RVA: 0x000206DE File Offset: 0x0001E8DE
		public static bool Get8(uint index, byte data)
		{
			return ((int)data & (1 << (int)index)) != 0;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000206EB File Offset: 0x0001E8EB
		public static bool Get16(uint index, ushort data)
		{
			return ((int)data & (1 << (int)index)) != 0;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x000206F8 File Offset: 0x0001E8F8
		public static bool Get32(uint index, uint data)
		{
			return (data & (1U << (int)index)) > 0U;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00020705 File Offset: 0x0001E905
		public static bool Get64(uint index, ulong data)
		{
			return (data & (1UL << (int)index)) > 0UL;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00020714 File Offset: 0x0001E914
		public static bool Get128(uint index, ulong data1, ulong data2)
		{
			if (index >= 64U)
			{
				return (data2 & (1UL << (int)(index - 64U))) > 0UL;
			}
			return (data1 & (1UL << (int)index)) > 0UL;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002073C File Offset: 0x0001E93C
		public static bool Get256(uint index, ulong data1, ulong data2, ulong data3, ulong data4)
		{
			if (index >= 128U)
			{
				if (index >= 192U)
				{
					return (data4 & (1UL << (int)(index - 192U))) > 0UL;
				}
				return (data3 & (1UL << (int)(index - 128U))) > 0UL;
			}
			else
			{
				if (index >= 64U)
				{
					return (data2 & (1UL << (int)(index - 64U))) > 0UL;
				}
				return (data1 & (1UL << (int)index)) > 0UL;
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000207A5 File Offset: 0x0001E9A5
		public static void Set8(uint index, ref byte data, bool value)
		{
			data = (byte)(value ? ((int)data | (1 << (int)index)) : ((int)data & ~(1 << (int)index)));
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000207C2 File Offset: 0x0001E9C2
		public static void Set16(uint index, ref ushort data, bool value)
		{
			data = (ushort)(value ? ((int)data | (1 << (int)index)) : ((int)data & ~(1 << (int)index)));
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000207DF File Offset: 0x0001E9DF
		public static void Set32(uint index, ref uint data, bool value)
		{
			data = (value ? (data | (1U << (int)index)) : (data & ~(1U << (int)index)));
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000207FB File Offset: 0x0001E9FB
		public static void Set64(uint index, ref ulong data, bool value)
		{
			data = (value ? (data | (1UL << (int)index)) : (data & ~(1UL << (int)index)));
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002081C File Offset: 0x0001EA1C
		public static void Set128(uint index, ref ulong data1, ref ulong data2, bool value)
		{
			if (index < 64U)
			{
				data1 = (value ? (data1 | (1UL << (int)index)) : (data1 & ~(1UL << (int)index)));
				return;
			}
			data2 = (value ? (data2 | (1UL << (int)(index - 64U))) : (data2 & ~(1UL << (int)(index - 64U))));
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00020870 File Offset: 0x0001EA70
		public static void Set256(uint index, ref ulong data1, ref ulong data2, ref ulong data3, ref ulong data4, bool value)
		{
			if (index < 64U)
			{
				data1 = (value ? (data1 | (1UL << (int)index)) : (data1 & ~(1UL << (int)index)));
				return;
			}
			if (index < 128U)
			{
				data2 = (value ? (data2 | (1UL << (int)(index - 64U))) : (data2 & ~(1UL << (int)(index - 64U))));
				return;
			}
			if (index < 192U)
			{
				data3 = (value ? (data3 | (1UL << (int)(index - 64U))) : (data3 & ~(1UL << (int)(index - 128U))));
				return;
			}
			data4 = (value ? (data4 | (1UL << (int)(index - 64U))) : (data4 & ~(1UL << (int)(index - 192U))));
		}
	}
}
