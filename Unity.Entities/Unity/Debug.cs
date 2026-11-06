using System;
using UnityEngine;

namespace Unity
{
	// Token: 0x02000008 RID: 8
	internal static class Debug
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002236 File Offset: 0x00000436
		public static void LogError(object message)
		{
			Debug.LogError(message);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000223E File Offset: 0x0000043E
		public static void LogWarning(object message)
		{
			Debug.LogWarning(message);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002246 File Offset: 0x00000446
		public static void Log(object message)
		{
			Debug.Log(message);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000224E File Offset: 0x0000044E
		public static void LogException(Exception exception)
		{
			Debug.LogException(exception);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002256 File Offset: 0x00000456
		public static void LogError(object message, Object context)
		{
			Debug.LogError(message, context);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000225F File Offset: 0x0000045F
		public static void LogWarning(object message, Object context)
		{
			Debug.LogWarning(message, context);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002268 File Offset: 0x00000468
		public static void Log(object message, Object context)
		{
			Debug.Log(message, context);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002271 File Offset: 0x00000471
		public static void LogException(Exception exception, Object context)
		{
			Debug.LogException(exception, context);
		}
	}
}
