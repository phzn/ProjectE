using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E9 RID: 233
	public static class LayermaskExtensions
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x00016E4B File Offset: 0x0001504B
		public static bool MMContains(this LayerMask mask, int layer)
		{
			return (mask.value & (1 << layer)) > 0;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00016E5E File Offset: 0x0001505E
		public static bool MMContains(this LayerMask mask, GameObject gameobject)
		{
			return (mask.value & (1 << gameobject.layer)) > 0;
		}
	}
}
