using System;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000029 RID: 41
	internal struct ResourceHandle
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000914D File Offset: 0x0000734D
		public int index
		{
			get
			{
				return (int)(this.m_Value & 65535U);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000915B File Offset: 0x0000735B
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00009163 File Offset: 0x00007363
		public RenderGraphResourceType type { readonly get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000916C File Offset: 0x0000736C
		public int iType
		{
			get
			{
				return (int)this.type;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009174 File Offset: 0x00007374
		internal ResourceHandle(int value, RenderGraphResourceType type, bool shared)
		{
			this.m_Value = (uint)((value & 65535) | (int)(shared ? ResourceHandle.s_SharedResourceValidBit : ResourceHandle.s_CurrentValidBit));
			this.type = type;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000919A File Offset: 0x0000739A
		public static implicit operator int(ResourceHandle handle)
		{
			return handle.index;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000091A4 File Offset: 0x000073A4
		public bool IsValid()
		{
			uint num = this.m_Value & 4294901760U;
			return num != 0U && (num == ResourceHandle.s_CurrentValidBit || num == ResourceHandle.s_SharedResourceValidBit);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000091D8 File Offset: 0x000073D8
		public static void NewFrame(int executionIndex)
		{
			uint num = ResourceHandle.s_CurrentValidBit;
			ResourceHandle.s_CurrentValidBit = (uint)((uint)((executionIndex >> 16) ^ ((executionIndex & 65535) * 58546883)) << 16);
			if (ResourceHandle.s_CurrentValidBit == 0U || ResourceHandle.s_CurrentValidBit == ResourceHandle.s_SharedResourceValidBit)
			{
				uint num2 = 1U;
				while (num == num2 << 16)
				{
					num2 += 1U;
				}
				ResourceHandle.s_CurrentValidBit = num2 << 16;
			}
		}

		// Token: 0x040000DF RID: 223
		private const uint kValidityMask = 4294901760U;

		// Token: 0x040000E0 RID: 224
		private const uint kIndexMask = 65535U;

		// Token: 0x040000E1 RID: 225
		private uint m_Value;

		// Token: 0x040000E2 RID: 226
		private static uint s_CurrentValidBit = 65536U;

		// Token: 0x040000E3 RID: 227
		private static uint s_SharedResourceValidBit = 2147418112U;
	}
}
