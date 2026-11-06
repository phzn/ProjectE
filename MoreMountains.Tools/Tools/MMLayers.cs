using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000102 RID: 258
	public class MMLayers
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x0001B418 File Offset: 0x00019618
		public static bool LayerInLayerMask(int layer, LayerMask layerMask)
		{
			return ((1 << layer) & layerMask) != 0;
		}
	}
}
