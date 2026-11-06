using System;

namespace Unity
{
	// Token: 0x02000BCF RID: 3023
	internal sealed class ThrowStub : ObjectDisposedException
	{
		// Token: 0x06006B64 RID: 27492 RVA: 0x0001B98F File Offset: 0x00019B8F
		public static void ThrowNotSupportedException()
		{
			throw new PlatformNotSupportedException();
		}
	}
}
