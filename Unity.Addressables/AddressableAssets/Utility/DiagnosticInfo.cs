using System;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.Diagnostics;

namespace UnityEngine.AddressableAssets.Utility
{
	// Token: 0x0200001C RID: 28
	internal class DiagnosticInfo
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00006C94 File Offset: 0x00004E94
		public DiagnosticEvent CreateEvent(string category, ResourceManager.DiagnosticEventType eventType, int frame, int val)
		{
			return new DiagnosticEvent(category, this.DisplayName, this.ObjectId, (int)eventType, frame, val, this.Dependencies);
		}

		// Token: 0x0400004A RID: 74
		public string DisplayName;

		// Token: 0x0400004B RID: 75
		public int ObjectId;

		// Token: 0x0400004C RID: 76
		public int[] Dependencies;
	}
}
