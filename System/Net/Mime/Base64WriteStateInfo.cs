using System;

namespace System.Net.Mime
{
	// Token: 0x020007CB RID: 1995
	internal class Base64WriteStateInfo : WriteStateInfoBase
	{
		// Token: 0x06003FEA RID: 16362 RVA: 0x000DA605 File Offset: 0x000D8805
		internal Base64WriteStateInfo()
		{
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x000DA60D File Offset: 0x000D880D
		internal Base64WriteStateInfo(int bufferSize, byte[] header, byte[] footer, int maxLineLength, int mimeHeaderLength)
			: base(bufferSize, header, footer, maxLineLength, mimeHeaderLength)
		{
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06003FEC RID: 16364 RVA: 0x000DA61C File Offset: 0x000D881C
		// (set) Token: 0x06003FED RID: 16365 RVA: 0x000DA624 File Offset: 0x000D8824
		internal int Padding { get; set; }

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06003FEE RID: 16366 RVA: 0x000DA62D File Offset: 0x000D882D
		// (set) Token: 0x06003FEF RID: 16367 RVA: 0x000DA635 File Offset: 0x000D8835
		internal byte LastBits { get; set; }
	}
}
