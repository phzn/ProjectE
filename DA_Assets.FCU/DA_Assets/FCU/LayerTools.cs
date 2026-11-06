using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200002B RID: 43
	public class LayerTools
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00003688 File Offset: 0x00001888
		public static int AddLayer(string layerName)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num != -1)
			{
				return num;
			}
			return LayerMask.NameToLayer(layerName);
		}
	}
}
