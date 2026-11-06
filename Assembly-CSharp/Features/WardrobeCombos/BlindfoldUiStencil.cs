using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.WardrobeCombos
{
	// Token: 0x02000B58 RID: 2904
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Graphic))]
	public sealed class BlindfoldUiStencil : MonoBehaviour, IMaterialModifier
	{
		// Token: 0x0600526C RID: 21100 RVA: 0x001FF1D0 File Offset: 0x001FD3D0
		private void OnEnable()
		{
			this._graphic = base.GetComponent<Graphic>();
			this._graphic.SetMaterialDirty();
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x001FF1E9 File Offset: 0x001FD3E9
		private void OnDisable()
		{
			if (this._graphic != null)
			{
				this._graphic.SetMaterialDirty();
			}
			this.ReleaseMaterial();
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x001FF20A File Offset: 0x001FD40A
		private void OnDestroy()
		{
			this.ReleaseMaterial();
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x001FF214 File Offset: 0x001FD414
		public Material GetModifiedMaterial(Material baseMaterial)
		{
			Mask mask;
			if (base.TryGetComponent<Mask>(out mask) && !mask.showMaskGraphic)
			{
				return baseMaterial;
			}
			if (!base.isActiveAndEnabled || baseMaterial == null || !baseMaterial.HasProperty("_Stencil") || !baseMaterial.HasProperty("_StencilComp") || !baseMaterial.HasProperty("_StencilOp") || !baseMaterial.HasProperty("_StencilWriteMask") || !baseMaterial.HasProperty("_StencilReadMask"))
			{
				return baseMaterial;
			}
			if (this._source == baseMaterial && this._marked != null)
			{
				return this._marked;
			}
			this.ReleaseMaterial();
			this._source = baseMaterial;
			this._marked = new Material(baseMaterial)
			{
				name = baseMaterial.name + " (Blindfold UI stencil)",
				hideFlags = HideFlags.HideAndDontSave
			};
			int @int = baseMaterial.GetInt("_Stencil");
			int int2 = baseMaterial.GetInt("_StencilWriteMask");
			bool flag = mask != null;
			this._marked.SetInt("_Stencil", @int | 64);
			this._marked.SetInt("_StencilReadMask", baseMaterial.GetInt("_StencilReadMask") & -65);
			this._marked.SetInt("_StencilWriteMask", 64 | (flag ? int2 : 0));
			this._marked.SetInt("_StencilOp", 2);
			if (this._marked.HasProperty("_UseUIAlphaClip"))
			{
				this._marked.SetFloat("_UseUIAlphaClip", 1f);
				this._marked.EnableKeyword("UNITY_UI_ALPHACLIP");
			}
			return this._marked;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x001FF39D File Offset: 0x001FD59D
		private void ReleaseMaterial()
		{
			if (this._marked != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._marked);
				}
				else
				{
					Object.DestroyImmediate(this._marked);
				}
			}
			this._source = null;
			this._marked = null;
		}

		// Token: 0x04005305 RID: 21253
		public const int UiBit = 64;

		// Token: 0x04005306 RID: 21254
		private Graphic _graphic;

		// Token: 0x04005307 RID: 21255
		private Material _source;

		// Token: 0x04005308 RID: 21256
		private Material _marked;
	}
}
