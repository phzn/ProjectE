using System;

namespace UnityEngine.UI
{
	// Token: 0x0200002E RID: 46
	internal static class Misc
	{
		// Token: 0x06000307 RID: 775 RVA: 0x000100FE File Offset: 0x0000E2FE
		public static void Destroy(Object obj)
		{
			if (obj != null)
			{
				if (Application.isPlaying)
				{
					if (obj is GameObject)
					{
						(obj as GameObject).transform.parent = null;
					}
					Object.Destroy(obj);
					return;
				}
				Object.DestroyImmediate(obj);
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010136 File Offset: 0x0000E336
		public static void DestroyImmediate(Object obj)
		{
			if (obj != null)
			{
				if (Application.isEditor)
				{
					Object.DestroyImmediate(obj);
					return;
				}
				Object.Destroy(obj);
			}
		}
	}
}
