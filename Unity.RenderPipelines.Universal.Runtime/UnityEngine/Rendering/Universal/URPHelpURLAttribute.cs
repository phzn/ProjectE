using System;
using System.Diagnostics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000082 RID: 130
	[Conditional("UNITY_EDITOR")]
	internal class URPHelpURLAttribute : CoreRPHelpURLAttribute
	{
		// Token: 0x0600050F RID: 1295 RVA: 0x0001DD60 File Offset: 0x0001BF60
		public URPHelpURLAttribute(string pageName, string pageHash = "")
			: base(pageName, pageHash, "com.unity.render-pipelines.universal")
		{
		}
	}
}
