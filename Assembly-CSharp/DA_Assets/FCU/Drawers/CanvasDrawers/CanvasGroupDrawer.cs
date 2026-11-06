using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003AC RID: 940
	[Serializable]
	public class CanvasGroupDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001857 RID: 6231 RVA: 0x0009AB04 File Offset: 0x00098D04
		public void Draw(FObject fobject)
		{
			if (fobject.Data.FcuImageType == FcuImageType.Downloadable)
			{
				return;
			}
			CanvasGroup canvasGroup;
			fobject.Data.GameObject.TryAddComponent<CanvasGroup>(out canvasGroup, false);
			canvasGroup.alpha = fobject.Opacity.Value;
		}
	}
}
