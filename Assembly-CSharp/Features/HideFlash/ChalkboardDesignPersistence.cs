using System;
using System.Collections;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory;
using Features.NeckSigns;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace Features.HideFlash
{
	// Token: 0x020011A3 RID: 4515
	[DisallowMultipleComponent]
	[RequireComponent(typeof(NeckSignSurface))]
	public sealed class ChalkboardDesignPersistence : MonoBehaviour
	{
		// Token: 0x06008A38 RID: 35384 RVA: 0x00387F6C File Offset: 0x0038616C
		private void OnEnable()
		{
			this._surface = base.GetComponent<NeckSignSurface>();
			this._surface.DesignChanged += this.StoreDesign;
			if (!this.TryBindPlayer() && Application.isPlaying)
			{
				this._resolveRoutine = base.StartCoroutine(this.ResolvePlayerAfterStartup());
			}
		}

		// Token: 0x06008A39 RID: 35385 RVA: 0x00387FBD File Offset: 0x003861BD
		private IEnumerator ResolvePlayerAfterStartup()
		{
			int num;
			for (int attempt = 0; attempt < 20; attempt = num + 1)
			{
				yield return new WaitForSecondsRealtime(0.5f);
				if (this.TryBindPlayer())
				{
					break;
				}
				num = attempt;
			}
			this._resolveRoutine = null;
			yield break;
		}

		// Token: 0x06008A3A RID: 35386 RVA: 0x00387FCC File Offset: 0x003861CC
		private bool TryBindPlayer()
		{
			if (this._style != null)
			{
				return true;
			}
			MainCharacterAppearanceApplier mainCharacterAppearanceApplier = base.GetComponentInParent<MainCharacterAppearanceApplier>();
			if (mainCharacterAppearanceApplier == null && PlayerInventoryManager.Instance != null)
			{
				mainCharacterAppearanceApplier = PlayerInventoryManager.Instance.GetComponentInParent<MainCharacterAppearanceApplier>();
			}
			if (mainCharacterAppearanceApplier == null || !mainCharacterAppearanceApplier.UsesSavedMainCharacterPreset)
			{
				return false;
			}
			MCStyleCustomizationService componentInChildren = mainCharacterAppearanceApplier.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (componentInChildren == null)
			{
				return false;
			}
			this.Configure(componentInChildren);
			return true;
		}

		// Token: 0x06008A3B RID: 35387 RVA: 0x00388040 File Offset: 0x00386240
		public void Configure(MCStyleCustomizationService style)
		{
			if (this._style != null)
			{
				this._style.OnNeckSignDesignsChanged -= this.ReloadDesign;
			}
			this._style = style;
			this._appearance = ((style != null) ? style.GetComponentInParent<MainCharacterAppearanceApplier>() : null);
			if (this._surface == null)
			{
				this._surface = base.GetComponent<NeckSignSurface>();
			}
			if (this._style != null)
			{
				this._style.OnNeckSignDesignsChanged += this.ReloadDesign;
			}
			this.ReloadDesign(null);
		}

		// Token: 0x06008A3C RID: 35388 RVA: 0x003880D8 File Offset: 0x003862D8
		private void ReloadDesign(string changedPartId = null)
		{
			if (this._committing || this._surface == null || this._style == null || this._surface.PartId != "Item.Chalkboard" || (!string.IsNullOrEmpty(changedPartId) && changedPartId != "Item.Chalkboard"))
			{
				return;
			}
			this._surface.ApplyDesign(this._style.GetNeckSignDesign("Item.Chalkboard"));
		}

		// Token: 0x06008A3D RID: 35389 RVA: 0x00388154 File Offset: 0x00386354
		private void StoreDesign()
		{
			if (this._committing || this._surface == null || this._surface.PartId != "Item.Chalkboard")
			{
				return;
			}
			NeckSignDesignData neckSignDesignData = this._surface.CaptureDesign();
			this._committing = true;
			try
			{
				if (!(this._style == null) || this.TryBindPlayer())
				{
					this._style.SetNeckSignDesign(neckSignDesignData);
					if (Application.isPlaying && this._appearance != null && this._appearance.UsesSavedMainCharacterPreset)
					{
						MainCharacterAppearanceService.TrySaveNeckSignDesign(neckSignDesignData);
					}
				}
			}
			finally
			{
				this._committing = false;
			}
		}

		// Token: 0x06008A3E RID: 35390 RVA: 0x0038820C File Offset: 0x0038640C
		private void OnDisable()
		{
			if (this._resolveRoutine != null)
			{
				base.StopCoroutine(this._resolveRoutine);
				this._resolveRoutine = null;
			}
			if (this._surface != null)
			{
				this._surface.CommitDesign();
				this._surface.DesignChanged -= this.StoreDesign;
			}
			if (this._style != null)
			{
				this._style.OnNeckSignDesignsChanged -= this.ReloadDesign;
			}
			this._style = null;
			this._appearance = null;
		}

		// Token: 0x04008D0B RID: 36107
		public const string DesignId = "Item.Chalkboard";

		// Token: 0x04008D0C RID: 36108
		private NeckSignSurface _surface;

		// Token: 0x04008D0D RID: 36109
		private MCStyleCustomizationService _style;

		// Token: 0x04008D0E RID: 36110
		private MainCharacterAppearanceApplier _appearance;

		// Token: 0x04008D0F RID: 36111
		private Coroutine _resolveRoutine;

		// Token: 0x04008D10 RID: 36112
		private bool _committing;
	}
}
