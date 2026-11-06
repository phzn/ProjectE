using System;
using BoZo.CharacterCreator.MC;
using UnityEngine;

namespace Features.Flash
{
	// Token: 0x0200120D RID: 4621
	internal static class FlashDetailPose
	{
		// Token: 0x06008EBB RID: 36539 RVA: 0x003A5900 File Offset: 0x003A3B00
		internal static void Complete(Animator detail, string stateName, int layer = 0)
		{
			if (detail == null || detail.runtimeAnimatorController == null || !detail.gameObject.activeInHierarchy || string.IsNullOrEmpty(stateName))
			{
				return;
			}
			layer = Mathf.Clamp(layer, 0, detail.layerCount - 1);
			int num = Animator.StringToHash(stateName);
			if (!detail.HasState(layer, num))
			{
				return;
			}
			bool enabled = detail.enabled;
			detail.enabled = true;
			detail.Play(num, layer, 1f);
			detail.Update(0f);
			detail.enabled = enabled;
			ClothingBlendShapeSyncController componentInParent = detail.GetComponentInParent<ClothingBlendShapeSyncController>();
			if (componentInParent == null)
			{
				return;
			}
			componentInParent.RequestResync();
		}
	}
}
