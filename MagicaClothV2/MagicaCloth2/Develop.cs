using System;
using System.Diagnostics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000108 RID: 264
	public static class Develop
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x0002D6F9 File Offset: 0x0002B8F9
		public static void Log(in object mes)
		{
			Debug.Log(string.Format("[MC2] {0}", mes));
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0002D70C File Offset: 0x0002B90C
		public static void LogWarning(in object mes)
		{
			Debug.LogWarning(string.Format("[MC2] {0}", mes));
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0002D71F File Offset: 0x0002B91F
		public static void LogError(in object mes)
		{
			Debug.LogError(string.Format("[MC2] {0}", mes));
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0002D732 File Offset: 0x0002B932
		[Conditional("MC2_LOG")]
		public static void DebugLog(in object mes)
		{
			Debug.Log(string.Format("[MC2 DEBUG] {0}", mes));
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0002D745 File Offset: 0x0002B945
		[Conditional("MC2_DEBUG")]
		public static void DebugLogWarning(in object mes)
		{
			Debug.LogWarning(string.Format("[MC2 DEBUG] {0}", mes));
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0002D758 File Offset: 0x0002B958
		[Conditional("MC2_DEBUG")]
		public static void DebugLogError(in object mes)
		{
			Debug.LogError(string.Format("[MC2 DEBUG] {0}", mes));
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000706D File Offset: 0x0000526D
		[Conditional("MC2_DEBUG")]
		public static void Assert(bool condition)
		{
		}
	}
}
