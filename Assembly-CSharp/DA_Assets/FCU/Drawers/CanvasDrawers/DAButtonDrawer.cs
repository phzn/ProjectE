using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DA_Assets.DAB;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003A7 RID: 935
	[Serializable]
	public class DAButtonDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001840 RID: 6208 RVA: 0x0009A114 File Offset: 0x00098314
		public void SetupDAButton(SyncData btnSyncData)
		{
			DAButton component = btnSyncData.GameObject.GetComponent<DAButton>();
			SyncHelper[] array = (from x in btnSyncData.GameObject.GetComponentsInChildren<SyncHelper>(true)
				where x != null && x.ContainsCustomButtonTags()
				select x).ToArray<SyncHelper>();
			component.TargetGraphics.Clear();
			bool flag = this.monoBeh.CanvasDrawer.ButtonDrawer.UnityButtonDrawer.IsAllSprites(array);
			foreach (IGrouping<string, SyncHelper> grouping in from helper in array
				group helper by this.ExtractGroupKey(helper.gameObject.name))
			{
				SyncHelper mainObject = grouping.FirstOrDefault<SyncHelper>((SyncHelper helper) => helper.ContainsTag(FcuTag.BtnDefault));
				if (mainObject == null)
				{
					Debug.LogError("Object with tag 'BtnDefault' was not found for " + btnSyncData.GameObject.name + ".");
				}
				else
				{
					Graphic graphic;
					if (mainObject.TryGetComponentSafe<Graphic>(out graphic) && !component.TargetGraphics.Contains(graphic))
					{
						component.TargetGraphics.Add(graphic);
					}
					List<SyncHelper> list = grouping.Where<SyncHelper>((SyncHelper h) => h != mainObject).ToList<SyncHelper>();
					if (flag && mainObject.ContainsTag(FcuTag.Image))
					{
						SpriteAnimator spriteAnimator;
						mainObject.gameObject.TryAddComponent<SpriteAnimator>(out spriteAnimator, false);
						AnimatedProperty<Sprite> animatedProperty = default(AnimatedProperty<Sprite>);
						foreach (SyncHelper syncHelper in list)
						{
							Image image;
							if (syncHelper.TryGetComponentSafe<Image>(out image))
							{
								if (syncHelper.ContainsTag(FcuTag.BtnPressed))
								{
									animatedProperty.Pressed = image.sprite;
								}
								if (syncHelper.ContainsTag(FcuTag.BtnHover))
								{
									animatedProperty.Highlighted = image.sprite;
								}
								if (syncHelper.ContainsTag(FcuTag.BtnDisabled))
								{
									animatedProperty.Disabled = image.sprite;
								}
							}
							syncHelper.gameObject.Destroy();
						}
						spriteAnimator.Properties = animatedProperty;
						spriteAnimator.Animations = this.monoBeh.Settings.ButtonSettings.DAB_Settings.SpriteAnimations;
					}
					else
					{
						ColorAnimator colorAnimator;
						mainObject.gameObject.TryAddComponent<ColorAnimator>(out colorAnimator, false);
						AnimatedProperty<Color> animatedProperty2 = default(AnimatedProperty<Color>);
						animatedProperty2.Looped = SingletonScriptableObject<DabConfig>.Instance.DefaultColorProps.Looped;
						foreach (SyncHelper syncHelper2 in list)
						{
							Graphic graphic2;
							if (syncHelper2.TryGetComponentSafe<Graphic>(out graphic2))
							{
								if (syncHelper2.ContainsTag(FcuTag.BtnPressed))
								{
									animatedProperty2.Pressed = graphic2.color;
								}
								if (syncHelper2.ContainsTag(FcuTag.BtnHover))
								{
									animatedProperty2.Highlighted = graphic2.color;
								}
								if (syncHelper2.ContainsTag(FcuTag.BtnDisabled))
								{
									animatedProperty2.Disabled = graphic2.color;
								}
							}
							syncHelper2.gameObject.Destroy();
						}
						colorAnimator.Properties = animatedProperty2;
						colorAnimator.Animations = this.monoBeh.Settings.ButtonSettings.DAB_Settings.ColorAnimations;
					}
					if (this.monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Pressed.Enabled || this.monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Highlighted.Enabled || this.monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Disabled.Enabled || this.monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Looped.Enabled)
					{
						ScaleAnimator scaleAnimator;
						mainObject.gameObject.TryAddComponent<ScaleAnimator>(out scaleAnimator, false);
						scaleAnimator.Animations = this.monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations;
						scaleAnimator.Properties = this.monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleProperties;
					}
				}
			}
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0009A5D0 File Offset: 0x000987D0
		private string ExtractGroupKey(string fullName)
		{
			if (string.IsNullOrWhiteSpace(fullName))
			{
				return "no-digit";
			}
			Match match = Regex.Match(fullName.Split('-', StringSplitOptions.None)[0].Trim(), "\\d+");
			if (match.Success)
			{
				return match.Value;
			}
			return "no-digit";
		}
	}
}
