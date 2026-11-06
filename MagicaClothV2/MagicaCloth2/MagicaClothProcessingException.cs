using System;

namespace MagicaCloth2
{
	// Token: 0x0200011E RID: 286
	[Serializable]
	public class MagicaClothProcessingException : Exception
	{
		// Token: 0x060005C9 RID: 1481 RVA: 0x0002FE51 File Offset: 0x0002E051
		public MagicaClothProcessingException()
		{
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0002FE59 File Offset: 0x0002E059
		public MagicaClothProcessingException(string message)
			: base(message)
		{
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0002FE62 File Offset: 0x0002E062
		public MagicaClothProcessingException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
