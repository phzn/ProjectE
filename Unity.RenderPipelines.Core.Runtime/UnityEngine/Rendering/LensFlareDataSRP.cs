using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000A6 RID: 166
	[Serializable]
	public sealed class LensFlareDataSRP : ScriptableObject
	{
		// Token: 0x0600054A RID: 1354 RVA: 0x0001BC79 File Offset: 0x00019E79
		public LensFlareDataSRP()
		{
			this.elements = null;
		}

		// Token: 0x040003C0 RID: 960
		public LensFlareDataElementSRP[] elements;
	}
}
