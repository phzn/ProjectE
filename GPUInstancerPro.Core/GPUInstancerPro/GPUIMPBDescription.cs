using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200000C RID: 12
	public abstract class GPUIMPBDescription : ScriptableObject
	{
		// Token: 0x0600006B RID: 107
		public abstract void SetMPBValues(GPUIRenderSourceGroup rsg, GPUIManager manager, int prototypeIndex);

		// Token: 0x04000019 RID: 25
		public Shader shader;
	}
}
