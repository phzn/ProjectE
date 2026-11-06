using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x0200005B RID: 91
	public interface ITextElement
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000430 RID: 1072
		Material sharedMaterial { get; }

		// Token: 0x06000431 RID: 1073
		void Rebuild(CanvasUpdate update);

		// Token: 0x06000432 RID: 1074
		int GetInstanceID();
	}
}
