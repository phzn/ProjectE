using System;
using System.Diagnostics;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED7 RID: 3799
	public static class NPCDebug
	{
		// Token: 0x060071BF RID: 29119 RVA: 0x002E379F File Offset: 0x002E199F
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public static void Log(string msg)
		{
			if (NPCDebug.Verbose)
			{
				Debug.Log(msg);
			}
		}

		// Token: 0x060071C0 RID: 29120 RVA: 0x002E37AE File Offset: 0x002E19AE
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public static void Log(string msg, Object ctx)
		{
			if (NPCDebug.Verbose)
			{
				Debug.Log(msg, ctx);
			}
		}

		// Token: 0x040074A8 RID: 29864
		public static bool Verbose;
	}
}
