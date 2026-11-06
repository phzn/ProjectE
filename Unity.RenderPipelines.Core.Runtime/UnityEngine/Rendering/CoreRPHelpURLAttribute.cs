using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x0200007F RID: 127
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false)]
	public class CoreRPHelpURLAttribute : HelpURLAttribute
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x000117F7 File Offset: 0x0000F9F7
		public CoreRPHelpURLAttribute(string pageName, string packageName = "com.unity.render-pipelines.core")
			: base(DocumentationInfo.GetPageLink(packageName, pageName, ""))
		{
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001180B File Offset: 0x0000FA0B
		public CoreRPHelpURLAttribute(string pageName, string pageHash, string packageName = "com.unity.render-pipelines.core")
			: base(DocumentationInfo.GetPageLink(packageName, pageName, pageHash))
		{
		}
	}
}
