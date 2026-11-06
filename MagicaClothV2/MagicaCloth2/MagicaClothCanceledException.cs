using System;

namespace MagicaCloth2
{
	// Token: 0x0200011F RID: 287
	[Serializable]
	public class MagicaClothCanceledException : Exception
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x0002FE51 File Offset: 0x0002E051
		public MagicaClothCanceledException()
		{
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0002FE59 File Offset: 0x0002E059
		public MagicaClothCanceledException(string message)
			: base(message)
		{
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0002FE62 File Offset: 0x0002E062
		public MagicaClothCanceledException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
