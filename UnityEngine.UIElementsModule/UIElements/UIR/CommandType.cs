using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000440 RID: 1088
	internal enum CommandType
	{
		// Token: 0x04000F10 RID: 3856
		Draw,
		// Token: 0x04000F11 RID: 3857
		ImmediateCull,
		// Token: 0x04000F12 RID: 3858
		Immediate,
		// Token: 0x04000F13 RID: 3859
		PushView,
		// Token: 0x04000F14 RID: 3860
		PopView,
		// Token: 0x04000F15 RID: 3861
		PushScissor,
		// Token: 0x04000F16 RID: 3862
		PopScissor,
		// Token: 0x04000F17 RID: 3863
		PushRenderTexture,
		// Token: 0x04000F18 RID: 3864
		PopRenderTexture,
		// Token: 0x04000F19 RID: 3865
		BlitToPreviousRT,
		// Token: 0x04000F1A RID: 3866
		PushDefaultMaterial,
		// Token: 0x04000F1B RID: 3867
		PopDefaultMaterial
	}
}
