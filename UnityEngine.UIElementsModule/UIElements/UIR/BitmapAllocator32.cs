using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000477 RID: 1143
	internal struct BitmapAllocator32
	{
		// Token: 0x06002358 RID: 9048 RVA: 0x00089714 File Offset: 0x00087914
		public void Construct(int pageHeight, int entryWidth = 1, int entryHeight = 1)
		{
			this.m_PageHeight = pageHeight;
			this.m_Pages = new List<BitmapAllocator32.Page>(1);
			this.m_AllocMap = new List<uint>(this.m_PageHeight * this.m_Pages.Capacity);
			this.m_EntryWidth = entryWidth;
			this.m_EntryHeight = entryHeight;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x00089760 File Offset: 0x00087960
		public void ForceFirstAlloc(ushort firstPageX, ushort firstPageY)
		{
			this.m_AllocMap.Add(4294967294U);
			for (int i = 1; i < this.m_PageHeight; i++)
			{
				this.m_AllocMap.Add(uint.MaxValue);
			}
			this.m_Pages.Add(new BitmapAllocator32.Page
			{
				x = firstPageX,
				y = firstPageY,
				freeSlots = 32 * this.m_PageHeight - 1
			});
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000897D8 File Offset: 0x000879D8
		public BMPAlloc Allocate(BaseShaderInfoStorage storage)
		{
			int count = this.m_Pages.Count;
			for (int i = 0; i < count; i++)
			{
				BitmapAllocator32.Page page = this.m_Pages[i];
				bool flag = page.freeSlots == 0;
				if (!flag)
				{
					int j = i * this.m_PageHeight;
					int num = j + this.m_PageHeight;
					while (j < num)
					{
						uint num2 = this.m_AllocMap[j];
						bool flag2 = num2 == 0U;
						if (!flag2)
						{
							byte b = BitmapAllocator32.CountTrailingZeroes(num2);
							this.m_AllocMap[j] = num2 & ~(1U << (int)b);
							page.freeSlots--;
							this.m_Pages[i] = page;
							return new BMPAlloc
							{
								page = i,
								pageLine = (ushort)(j - i * this.m_PageHeight),
								bitIndex = b,
								ownedState = OwnedState.Owned
							};
						}
						j++;
					}
				}
			}
			RectInt rectInt;
			bool flag3 = storage == null || !storage.AllocateRect(32 * this.m_EntryWidth, this.m_PageHeight * this.m_EntryHeight, out rectInt);
			if (flag3)
			{
				return BMPAlloc.Invalid;
			}
			this.m_AllocMap.Capacity += this.m_PageHeight;
			this.m_AllocMap.Add(4294967294U);
			for (int k = 1; k < this.m_PageHeight; k++)
			{
				this.m_AllocMap.Add(uint.MaxValue);
			}
			this.m_Pages.Add(new BitmapAllocator32.Page
			{
				x = (ushort)rectInt.xMin,
				y = (ushort)rectInt.yMin,
				freeSlots = 32 * this.m_PageHeight - 1
			});
			return new BMPAlloc
			{
				page = this.m_Pages.Count - 1,
				ownedState = OwnedState.Owned
			};
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000899EC File Offset: 0x00087BEC
		public void Free(BMPAlloc alloc)
		{
			Debug.Assert(alloc.ownedState == OwnedState.Owned);
			int num = alloc.page * this.m_PageHeight + (int)alloc.pageLine;
			this.m_AllocMap[num] = this.m_AllocMap[num] | (1U << (int)alloc.bitIndex);
			BitmapAllocator32.Page page = this.m_Pages[alloc.page];
			page.freeSlots++;
			this.m_Pages[alloc.page] = page;
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x00089A74 File Offset: 0x00087C74
		public int entryWidth
		{
			get
			{
				return this.m_EntryWidth;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x00089A8C File Offset: 0x00087C8C
		public int entryHeight
		{
			get
			{
				return this.m_EntryHeight;
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x00089AA4 File Offset: 0x00087CA4
		internal void GetAllocPageAtlasLocation(int page, out ushort x, out ushort y)
		{
			BitmapAllocator32.Page page2 = this.m_Pages[page];
			x = page2.x;
			y = page2.y;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x00089AD0 File Offset: 0x00087CD0
		private static byte CountTrailingZeroes(uint val)
		{
			byte b = 0;
			bool flag = (val & 65535U) == 0U;
			if (flag)
			{
				val >>= 16;
				b = 16;
			}
			bool flag2 = (val & 255U) == 0U;
			if (flag2)
			{
				val >>= 8;
				b += 8;
			}
			bool flag3 = (val & 15U) == 0U;
			if (flag3)
			{
				val >>= 4;
				b += 4;
			}
			bool flag4 = (val & 3U) == 0U;
			if (flag4)
			{
				val >>= 2;
				b += 2;
			}
			bool flag5 = (val & 1U) == 0U;
			if (flag5)
			{
				b += 1;
			}
			return b;
		}

		// Token: 0x0400107E RID: 4222
		public const int kPageWidth = 32;

		// Token: 0x0400107F RID: 4223
		private int m_PageHeight;

		// Token: 0x04001080 RID: 4224
		private List<BitmapAllocator32.Page> m_Pages;

		// Token: 0x04001081 RID: 4225
		private List<uint> m_AllocMap;

		// Token: 0x04001082 RID: 4226
		private int m_EntryWidth;

		// Token: 0x04001083 RID: 4227
		private int m_EntryHeight;

		// Token: 0x02000478 RID: 1144
		private struct Page
		{
			// Token: 0x04001084 RID: 4228
			public ushort x;

			// Token: 0x04001085 RID: 4229
			public ushort y;

			// Token: 0x04001086 RID: 4230
			public int freeSlots;
		}
	}
}
