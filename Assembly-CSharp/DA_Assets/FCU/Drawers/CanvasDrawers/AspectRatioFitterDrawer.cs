using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x0200039E RID: 926
	[Serializable]
	public class AspectRatioFitterDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001824 RID: 6180 RVA: 0x000998E0 File Offset: 0x00097AE0
		public void Draw(FObject fobject)
		{
			PreserveRatioMode preserveRatioMode = this.monoBeh.Settings.ImageSpritesSettings.PreserveRatioMode;
			AspectRatioFitter aspectRatioFitter;
			if (preserveRatioMode == PreserveRatioMode.WidthControlsHeight)
			{
				fobject.Data.GameObject.TryAddComponent<AspectRatioFitter>(out aspectRatioFitter, false);
				aspectRatioFitter.aspectMode = this.ConvertMode(this.monoBeh.Settings.ImageSpritesSettings.PreserveRatioMode);
				return;
			}
			if (preserveRatioMode != PreserveRatioMode.HeightControlsWidth)
			{
				return;
			}
			fobject.Data.GameObject.TryAddComponent<AspectRatioFitter>(out aspectRatioFitter, false);
			aspectRatioFitter.aspectMode = this.ConvertMode(this.monoBeh.Settings.ImageSpritesSettings.PreserveRatioMode);
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0009997C File Offset: 0x00097B7C
		private AspectRatioFitter.AspectMode ConvertMode(PreserveRatioMode mode)
		{
			AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.None;
			if (mode != PreserveRatioMode.WidthControlsHeight)
			{
				if (mode == PreserveRatioMode.HeightControlsWidth)
				{
					aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
				}
			}
			else
			{
				aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
			}
			return aspectMode;
		}
	}
}
