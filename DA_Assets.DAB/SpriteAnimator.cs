using System;
using DA_Assets.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.DAB
{
	// Token: 0x0200000D RID: 13
	[AddComponentMenu("UI/SpriteAnimator (DAB)", 32)]
	[RequireComponent(typeof(Image))]
	public class SpriteAnimator : BasePropertyAnimator<Sprite>
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002F10 File Offset: 0x00001110
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002F22 File Offset: 0x00001122
		private float _transitionProgress
		{
			get
			{
				return this._transitionMaterial.GetFloat("_TransitionProgress");
			}
			set
			{
				this._transitionMaterial.SetFloat("_TransitionProgress", value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002F35 File Offset: 0x00001135
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002F47 File Offset: 0x00001147
		private Texture _mainTex
		{
			get
			{
				return this._transitionMaterial.GetTexture("_MainTex");
			}
			set
			{
				this._transitionMaterial.SetTexture("_MainTex", value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002F5A File Offset: 0x0000115A
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002F6C File Offset: 0x0000116C
		private Texture _nextTex
		{
			get
			{
				return this._transitionMaterial.GetTexture("_NextTex");
			}
			set
			{
				this._transitionMaterial.SetTexture("_NextTex", value);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002F7F File Offset: 0x0000117F
		protected override void ApplyValue(float curveValue, Sprite from, Sprite to)
		{
			this._transitionProgress = curveValue;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002F88 File Offset: 0x00001188
		protected override void OnStartAnimation()
		{
			this.SetTexturesToMaterial();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002F90 File Offset: 0x00001190
		private void SetTexturesToMaterial()
		{
			if (this._transitionMaterial == null)
			{
				this._transitionMaterial = new Material(SingletonScriptableObject<DabConfig>.Instance.SpriteTransitionShader);
			}
			base._targetGraphic.material = this._transitionMaterial;
			Sprite idleValue = this._idleValue;
			this._mainTex = ((idleValue != null) ? idleValue.texture : null);
			Sprite targetValue = this._targetValue;
			this._nextTex = ((targetValue != null) ? targetValue.texture : null);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003001 File Offset: 0x00001201
		protected override void SetIdleValue()
		{
			this._idleValue = (base._targetGraphic as Image).sprite;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003019 File Offset: 0x00001219
		private void Reset()
		{
			this._animations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000302B File Offset: 0x0000122B
		private void OnDestroy()
		{
			Object.Destroy(this._transitionMaterial);
		}

		// Token: 0x04000026 RID: 38
		private Material _transitionMaterial;
	}
}
