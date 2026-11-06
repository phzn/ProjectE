using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003A0 RID: 928
	[Serializable]
	public class ContentSizeFitterDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600182D RID: 6189 RVA: 0x00099B10 File Offset: 0x00097D10
		public void Draw(FObject fobject)
		{
			TextAutoResize textAutoResize = fobject.Style.TextAutoResize;
			if (textAutoResize != TextAutoResize.HEIGHT)
			{
				if (textAutoResize == TextAutoResize.WIDTH_AND_HEIGHT)
				{
					ContentSizeFitter contentSizeFitter;
					fobject.Data.GameObject.TryAddComponent<ContentSizeFitter>(out contentSizeFitter, false);
					contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
					contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
					return;
				}
			}
			else
			{
				ContentSizeFitter contentSizeFitter2;
				fobject.Data.GameObject.TryAddComponent<ContentSizeFitter>(out contentSizeFitter2, false);
				contentSizeFitter2.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
				contentSizeFitter2.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			}
		}
	}
}
