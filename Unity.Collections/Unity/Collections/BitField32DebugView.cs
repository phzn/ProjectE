using System;

namespace Unity.Collections
{
	// Token: 0x02000039 RID: 57
	internal sealed class BitField32DebugView
	{
		// Token: 0x06000134 RID: 308 RVA: 0x00004C38 File Offset: 0x00002E38
		public BitField32DebugView(BitField32 bitfield)
		{
			this.BitField = bitfield;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00004C48 File Offset: 0x00002E48
		public bool[] Bits
		{
			get
			{
				bool[] array = new bool[32];
				for (int i = 0; i < 32; i++)
				{
					array[i] = this.BitField.IsSet(i);
				}
				return array;
			}
		}

		// Token: 0x04000086 RID: 134
		private BitField32 BitField;
	}
}
