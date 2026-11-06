using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003BE RID: 958
	[Serializable]
	public class MaskDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060018A0 RID: 6304 RVA: 0x0009CF18 File Offset: 0x0009B118
		public void Draw(FObject fobject)
		{
			GameObject gameObject;
			if (fobject.IsObjectMask() && !fobject.ContainsTag(FcuTag.Frame))
			{
				FObject fobject2;
				this.monoBeh.CurrentProject.TryGetByIndex(fobject.Data.ParentIndex, out fobject2);
				gameObject = fobject2.Data.GameObject;
			}
			else
			{
				gameObject = fobject.Data.GameObject;
			}
			int num = 0;
			if (fobject.IsObjectMask())
			{
				if (!this.monoBeh.IsNova())
				{
					if (!this.monoBeh.UsingSpriteRenderer())
					{
						num = 2;
						this.monoBeh.CanvasDrawer.ImageDrawer.Draw(fobject, gameObject);
						Mask mask;
						gameObject.TryAddComponent<Mask>(out mask, false);
						mask.showMaskGraphic = false;
					}
					else
					{
						num = 3;
					}
				}
				fobject.Data.GameObject.Destroy();
			}
			else if ((fobject.IsFrameMask() || fobject.IsClipMask()) && !this.monoBeh.IsNova())
			{
				if (this.monoBeh.UseImageLinearMaterial() || fobject.Data.FRect.absoluteAngle != 0f)
				{
					num = 5;
					Mask mask2;
					gameObject.TryAddComponent<Mask>(out mask2, false);
				}
				else if (!this.monoBeh.UsingSpriteRenderer())
				{
					num = 6;
					RectMask2D rectMask2D;
					gameObject.TryAddComponent<RectMask2D>(out rectMask2D, false);
				}
				else
				{
					num = 7;
				}
			}
			FcuLogger.Debug(string.Format("Draw | {0} | {1}", fobject.Data.NameHierarchy, num), FcuDebugSettingsFlags.LogMask);
		}
	}
}
