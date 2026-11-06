using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000AB RID: 171
	public abstract class RenderPipelineResources : ScriptableObject
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001BC88 File Offset: 0x00019E88
		protected virtual string packagePath
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001BC8B File Offset: 0x00019E8B
		internal string packagePath_Internal
		{
			get
			{
				return this.packagePath;
			}
		}
	}
}
