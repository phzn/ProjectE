using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002D8 RID: 728
	internal static class PrefabCreatorExtensions
	{
		// Token: 0x0600144D RID: 5197 RVA: 0x000835DA File Offset: 0x000817DA
		internal static void SetParentEx(this Transform transform, Transform parent)
		{
			if (transform == null)
			{
				Debug.LogError("transform is null");
				return;
			}
			transform.SetParent(parent, false);
		}
	}
}
