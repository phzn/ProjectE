using System;

namespace Unity.Collections
{
	// Token: 0x0200003B RID: 59
	internal sealed class BitField64DebugView
	{
		// Token: 0x06000143 RID: 323 RVA: 0x00004DA1 File Offset: 0x00002FA1
		public BitField64DebugView(BitField64 data)
		{
			this.Data = data;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public bool[] Bits
		{
			get
			{
				bool[] array = new bool[64];
				for (int i = 0; i < 64; i++)
				{
					array[i] = this.Data.IsSet(i);
				}
				return array;
			}
		}

		// Token: 0x04000088 RID: 136
		private BitField64 Data;
	}
}
