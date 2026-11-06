using System;
using System.Security;

namespace Microsoft.Win32
{
	// Token: 0x020000AA RID: 170
	internal static class ThrowHelper
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x000173B4 File Offset: 0x000155B4
		internal static void ThrowArgumentException(string msg)
		{
			throw new ArgumentException(msg);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000173BC File Offset: 0x000155BC
		internal static void ThrowArgumentException(string msg, string argument)
		{
			throw new ArgumentException(msg, argument);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000173C5 File Offset: 0x000155C5
		internal static void ThrowArgumentNullException(string argument)
		{
			throw new ArgumentNullException(argument);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000173CD File Offset: 0x000155CD
		internal static void ThrowInvalidOperationException(string msg)
		{
			throw new InvalidOperationException(msg);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000173D5 File Offset: 0x000155D5
		internal static void ThrowSecurityException(string msg)
		{
			throw new SecurityException(msg);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000173DD File Offset: 0x000155DD
		internal static void ThrowUnauthorizedAccessException(string msg)
		{
			throw new UnauthorizedAccessException(msg);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000173E5 File Offset: 0x000155E5
		internal static void ThrowObjectDisposedException(string objectName, string msg)
		{
			throw new ObjectDisposedException(objectName, msg);
		}
	}
}
