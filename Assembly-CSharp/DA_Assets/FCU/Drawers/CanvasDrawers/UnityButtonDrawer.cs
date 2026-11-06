using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003AA RID: 938
	[Serializable]
	public class UnityButtonDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600184A RID: 6218 RVA: 0x0009A668 File Offset: 0x00098868
		public void SetupUnityButton(SyncData btnSyncData)
		{
			SyncHelper[] array;
			bool flag;
			this.SetupSelectable(btnSyncData, out array, out flag);
			Button component = btnSyncData.GameObject.GetComponent<Button>();
			if (flag)
			{
				this.SetCustomTargetGraphics(array, component);
				return;
			}
			this.SetDefaultTargetGraphic(array, component);
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0009A6A0 File Offset: 0x000988A0
		public bool IsAllSprites(SyncHelper[] btnChilds)
		{
			return btnChilds.Where<SyncHelper>((SyncHelper x) => x.ContainsTag(FcuTag.Image)).Count<SyncHelper>((SyncHelper x) => x.Data.IsSprite()) > 1;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0009A6FC File Offset: 0x000988FC
		public void SetupSelectable(SyncData btnSyncData, out SyncHelper[] btnChilds, out bool hasCustomButtonBackgrounds)
		{
			btnChilds = btnSyncData.GameObject.GetComponentsInChildren<SyncHelper>(true).Skip<SyncHelper>(1).ToArray<SyncHelper>();
			hasCustomButtonBackgrounds = false;
			List<FObject> list = new List<FObject>();
			foreach (int num in btnSyncData.ChildIndexes)
			{
				FObject fobject;
				if (this.monoBeh.CurrentProject.TryGetByIndex(num, out fobject))
				{
					if (fobject.ContainsTag(FcuTag.Image))
					{
						list.Add(fobject);
					}
					if (fobject.ContainsAnyTag(new FcuTag[]
					{
						FcuTag.BtnDefault,
						FcuTag.BtnDisabled,
						FcuTag.BtnHover,
						FcuTag.BtnPressed,
						FcuTag.BtnSelected
					}))
					{
						hasCustomButtonBackgrounds = true;
						break;
					}
				}
			}
			bool flag = this.IsAllSprites(btnChilds);
			if (this.monoBeh.IsUGUI())
			{
				Selectable component = btnSyncData.GameObject.GetComponent<Selectable>();
				if (flag)
				{
					component.transition = Selectable.Transition.SpriteSwap;
					return;
				}
				component.transition = Selectable.Transition.ColorTint;
			}
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0009A7EC File Offset: 0x000989EC
		private void SetCustomTargetGraphics(SyncHelper[] syncHelpers, Button btn)
		{
			foreach (SyncHelper syncHelper in syncHelpers)
			{
				if (syncHelper.ContainsTag(FcuTag.Image))
				{
					if (btn.transition == Selectable.Transition.SpriteSwap)
					{
						this.SetSprite(btn, syncHelper);
					}
					else
					{
						this.SetImageColor(btn, syncHelper);
					}
				}
				else if (syncHelper.ContainsTag(FcuTag.Text))
				{
					this.SetText(syncHelper);
				}
			}
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0009A84B File Offset: 0x00098A4B
		private void SetText(SyncHelper syncHelper)
		{
			if (syncHelper.ContainsAnyTag(new FcuTag[]
			{
				FcuTag.BtnDisabled,
				FcuTag.BtnHover,
				FcuTag.BtnPressed,
				FcuTag.BtnSelected
			}))
			{
				syncHelper.gameObject.Destroy();
			}
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0009A874 File Offset: 0x00098A74
		public void SetSprite(Selectable selectable, SyncHelper syncHelper)
		{
			selectable.transition = Selectable.Transition.SpriteSwap;
			SpriteState spriteState = selectable.spriteState;
			Image image;
			if (syncHelper.ContainsTag(FcuTag.BtnDefault))
			{
				Graphic graphic;
				if (syncHelper.TryGetComponentSafe<Graphic>(out graphic))
				{
					selectable.targetGraphic = graphic;
				}
			}
			else if (syncHelper.TryGetComponentSafe<Image>(out image))
			{
				if (syncHelper.ContainsTag(FcuTag.BtnPressed))
				{
					spriteState.pressedSprite = image.sprite;
					syncHelper.gameObject.Destroy();
				}
				else if (syncHelper.ContainsTag(FcuTag.BtnHover))
				{
					spriteState.highlightedSprite = image.sprite;
					syncHelper.gameObject.Destroy();
				}
				else if (syncHelper.ContainsTag(FcuTag.BtnSelected))
				{
					spriteState.selectedSprite = image.sprite;
					syncHelper.gameObject.Destroy();
				}
				else if (syncHelper.ContainsTag(FcuTag.BtnDisabled))
				{
					spriteState.disabledSprite = image.sprite;
					syncHelper.gameObject.Destroy();
				}
			}
			selectable.spriteState = spriteState;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0009A968 File Offset: 0x00098B68
		public void SetImageColor(Selectable selectable, SyncHelper syncHelper)
		{
			selectable.transition = Selectable.Transition.ColorTint;
			ColorBlock colors = selectable.colors;
			Graphic graphic;
			if (syncHelper.TryGetComponentSafe<Graphic>(out graphic))
			{
				if (syncHelper.ContainsTag(FcuTag.BtnDefault))
				{
					selectable.targetGraphic = graphic;
				}
				else if (syncHelper.ContainsTag(FcuTag.BtnPressed))
				{
					colors.pressedColor = graphic.color;
					syncHelper.gameObject.Destroy();
				}
				else if (syncHelper.ContainsTag(FcuTag.BtnHover))
				{
					colors.highlightedColor = graphic.color;
					syncHelper.gameObject.Destroy();
				}
				else if (syncHelper.ContainsTag(FcuTag.BtnSelected))
				{
					colors.selectedColor = graphic.color;
					syncHelper.gameObject.Destroy();
				}
				else if (syncHelper.ContainsTag(FcuTag.BtnDisabled))
				{
					colors.disabledColor = graphic.color;
					syncHelper.gameObject.Destroy();
				}
			}
			selectable.colors = colors;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0009AA50 File Offset: 0x00098C50
		public void SetDefaultTargetGraphic(SyncHelper[] syncHelpers, Selectable btn)
		{
			Graphic graphic = null;
			if (!syncHelpers.IsEmpty<SyncHelper>() && syncHelpers.First<SyncHelper>().TryGetComponentSafe<Graphic>(out graphic))
			{
				btn.targetGraphic = graphic;
				return;
			}
			for (int i = 0; i < syncHelpers.Length; i++)
			{
				Image image;
				if (syncHelpers[i].TryGetComponentSafe<Image>(out image))
				{
					btn.targetGraphic = image;
					return;
				}
			}
			for (int i = 0; i < syncHelpers.Length; i++)
			{
				Graphic graphic2;
				if (syncHelpers[i].TryGetComponentSafe<Graphic>(out graphic2))
				{
					btn.targetGraphic = graphic2;
					return;
				}
			}
			Graphic graphic3;
			if (btn.TryGetComponentSafe<Graphic>(out graphic3))
			{
				btn.targetGraphic = graphic3;
			}
		}
	}
}
