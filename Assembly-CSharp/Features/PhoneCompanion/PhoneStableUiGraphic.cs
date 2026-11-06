using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E4F RID: 3663
	[DisallowMultipleComponent]
	public sealed class PhoneStableUiGraphic : UIBehaviour, IMaterialModifier
	{
		// Token: 0x06006D6E RID: 28014 RVA: 0x002BF294 File Offset: 0x002BD494
		internal void Bind(PhoneStableUiMaterials owner)
		{
			if (this._owner == owner)
			{
				return;
			}
			this.RestorePopMaterials();
			this._owner = owner;
			this.SetMaterialDirty();
		}

		// Token: 0x06006D6F RID: 28015 RVA: 0x002BF2B8 File Offset: 0x002BD4B8
		public Material GetModifiedMaterial(Material baseMaterial)
		{
			if (!base.isActiveAndEnabled || this._owner == null)
			{
				return baseMaterial;
			}
			if (this._graphic == null)
			{
				this._graphic = base.GetComponent<Graphic>();
			}
			CanvasRenderer canvasRenderer = ((this._graphic != null) ? this._graphic.canvasRenderer : null);
			if (canvasRenderer != null)
			{
				for (int i = 0; i < canvasRenderer.popMaterialCount; i++)
				{
					Material popMaterial = canvasRenderer.GetPopMaterial(i);
					Material stableMaterial = this._owner.GetStableMaterial(popMaterial);
					if (!(stableMaterial == popMaterial))
					{
						while (this._popSources.Count <= i)
						{
							this._popSources.Add(null);
							this._popReplacements.Add(null);
						}
						this._popSources[i] = popMaterial;
						this._popReplacements[i] = stableMaterial;
						canvasRenderer.SetPopMaterial(stableMaterial, i);
					}
				}
			}
			return this._owner.GetStableMaterial(baseMaterial);
		}

		// Token: 0x06006D70 RID: 28016 RVA: 0x002BF3A2 File Offset: 0x002BD5A2
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetMaterialDirty();
		}

		// Token: 0x06006D71 RID: 28017 RVA: 0x002BF3B0 File Offset: 0x002BD5B0
		protected override void OnDisable()
		{
			this.RestorePopMaterials();
			this.SetMaterialDirty();
			base.OnDisable();
		}

		// Token: 0x06006D72 RID: 28018 RVA: 0x002BF3C4 File Offset: 0x002BD5C4
		private void RestorePopMaterials()
		{
			CanvasRenderer canvasRenderer = ((this._graphic != null) ? this._graphic.canvasRenderer : null);
			if (canvasRenderer != null)
			{
				int num = Mathf.Min(canvasRenderer.popMaterialCount, this._popSources.Count);
				for (int i = 0; i < num; i++)
				{
					if (canvasRenderer.GetPopMaterial(i) == this._popReplacements[i])
					{
						canvasRenderer.SetPopMaterial(this._popSources[i], i);
					}
				}
			}
			this._popSources.Clear();
			this._popReplacements.Clear();
		}

		// Token: 0x06006D73 RID: 28019 RVA: 0x002BF45D File Offset: 0x002BD65D
		private void SetMaterialDirty()
		{
			if (this._graphic == null)
			{
				this._graphic = base.GetComponent<Graphic>();
			}
			if (this._graphic != null)
			{
				this._graphic.SetMaterialDirty();
			}
		}

		// Token: 0x0400700E RID: 28686
		private PhoneStableUiMaterials _owner;

		// Token: 0x0400700F RID: 28687
		private Graphic _graphic;

		// Token: 0x04007010 RID: 28688
		private readonly List<Material> _popSources = new List<Material>();

		// Token: 0x04007011 RID: 28689
		private readonly List<Material> _popReplacements = new List<Material>();
	}
}
