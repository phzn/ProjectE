using System;
using Features.DebugTools;
using Features.Flash;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist
{
	// Token: 0x020008E6 RID: 2278
	public class MaskToggleController : MonoBehaviour
	{
		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06003D79 RID: 15737 RVA: 0x0016D934 File Offset: 0x0016BB34
		public bool IsLowered
		{
			get
			{
				return this.isLowered;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06003D7A RID: 15738 RVA: 0x0016D93C File Offset: 0x0016BB3C
		// (set) Token: 0x06003D7B RID: 15739 RVA: 0x0016D944 File Offset: 0x0016BB44
		public bool InputEnabled { get; set; } = true;

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x0016D94D File Offset: 0x0016BB4D
		public bool IsOnCooldown
		{
			get
			{
				return Time.time < this._cooldownUntil;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x0016D95C File Offset: 0x0016BB5C
		public float CooldownRemaining
		{
			get
			{
				return Mathf.Max(0f, this._cooldownUntil - Time.time);
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06003D7E RID: 15742 RVA: 0x0016D974 File Offset: 0x0016BB74
		public bool IsMaskAvailable
		{
			get
			{
				return this._maskAvailable;
			}
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x0016D97C File Offset: 0x0016BB7C
		public void SetMaskAvailable(bool available)
		{
			if (this._maskAvailable == available)
			{
				return;
			}
			this._maskAvailable = available;
			if (!available)
			{
				this.isLowered = false;
				this._activeAction = null;
				this._currentBlendShapeValue = 0f;
				this.ReleaseBlendShapeOverride();
				ClothActionGate.Release(this);
			}
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x0016D9B7 File Offset: 0x0016BBB7
		private void ReleaseBlendShapeOverride()
		{
			if (this.maskRenderer != null && this._blendShapeIndex >= 0)
			{
				this.maskRenderer.SetBlendShapeWeight(this._blendShapeIndex, 0f);
			}
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x0016D9E6 File Offset: 0x0016BBE6
		public void RebindMaskRenderer(SkinnedMeshRenderer newMaskRenderer)
		{
			this.maskRenderer = newMaskRenderer;
			this.CacheBlendShapeIndex();
			this.SetMaskAvailable(newMaskRenderer != null && this._blendShapeIndex >= 0);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0016DA14 File Offset: 0x0016BC14
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			this._lowerTriggerHash = Animator.StringToHash(this.lowerTrigger);
			this._raiseTriggerHash = Animator.StringToHash(this.raiseTrigger);
			this._emptyStateHash = Animator.StringToHash(this.emptyStateName);
			if (this.animator != null)
			{
				this._maskLayerIndex = this.animator.GetLayerIndex(this.maskLayerName);
				if (this._maskLayerIndex >= 0)
				{
					this.animator.SetLayerWeight(this._maskLayerIndex, 0f);
				}
			}
			this.CacheBlendShapeIndex();
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x0016DAB8 File Offset: 0x0016BCB8
		private void OnDisable()
		{
			this.ResetToNeutral();
			ClothActionGate.Release(this);
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x0016DAC8 File Offset: 0x0016BCC8
		public void ResetToNeutral()
		{
			this.isLowered = false;
			this._activeAction = null;
			this._waitingForActivation = false;
			this._targetWeight = 0f;
			this._cooldownUntil = 0f;
			this._currentBlendShapeValue = 0f;
			this.ReleaseBlendShapeOverride();
			if (this.animator != null && this._maskLayerIndex >= 0)
			{
				this.animator.ResetTrigger(this._lowerTriggerHash);
				this.animator.ResetTrigger(this._raiseTriggerHash);
				if (this.animator.HasState(this._maskLayerIndex, this._emptyStateHash))
				{
					this.animator.Play(this._emptyStateHash, this._maskLayerIndex, 0f);
				}
				this.animator.SetLayerWeight(this._maskLayerIndex, 0f);
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0016DB98 File Offset: 0x0016BD98
		private void Update()
		{
			this.RefreshMaskAvailability();
			if (this._maskAvailable && this.InputEnabled && !FlyCamera.IsActive && this.toggleInput != null && this.toggleInput.GetButtonDown() && !this.IsOnCooldown)
			{
				this.Toggle();
			}
			this.UpdateLayerWeight();
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x0016DBEC File Offset: 0x0016BDEC
		private void RefreshMaskAvailability()
		{
			bool flag = this.maskRenderer != null && this.maskRenderer.enabled && this.maskRenderer.gameObject.activeInHierarchy && this.maskRenderer.sharedMesh != null && this.maskRenderer.sharedMesh.GetBlendShapeIndex(this.downBlendshapeName) >= 0;
			if (flag && this._blendShapeIndex < 0)
			{
				this.CacheBlendShapeIndex();
			}
			this.SetMaskAvailable(flag);
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x0016DC74 File Offset: 0x0016BE74
		private void LateUpdate()
		{
			if (!this._hasEverToggled)
			{
				return;
			}
			if (this._activeAction != null)
			{
				float num = Time.time - this._actionStartTime - this._activeAction.delay;
				if (num <= 0f)
				{
					this._currentBlendShapeValue = this._activeAction.startValue;
				}
				else if (this._activeAction.duration <= 0f || num >= this._activeAction.duration)
				{
					this._currentBlendShapeValue = this._activeAction.targetValue;
					this._activeAction = null;
				}
				else
				{
					this._currentBlendShapeValue = Mathf.Lerp(this._activeAction.startValue, this._activeAction.targetValue, num / this._activeAction.duration);
				}
			}
			this.ApplyBlendShape(this._currentBlendShapeValue);
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x0016DD3D File Offset: 0x0016BF3D
		public void Toggle()
		{
			this.SetLowered(!this.isLowered);
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x0016DD50 File Offset: 0x0016BF50
		public void SetLowered(bool lowered)
		{
			if (this.IsOnCooldown)
			{
				return;
			}
			if (this._hasEverToggled && this.isLowered == lowered)
			{
				return;
			}
			if (!ClothActionGate.TryBegin(this))
			{
				return;
			}
			this._tokenDeadline = Time.time + 5f;
			this.isLowered = lowered;
			this._hasEverToggled = true;
			this._activeAction = (this.isLowered ? this.lowerAction : this.raiseAction);
			this._actionStartTime = Time.time;
			this._currentBlendShapeValue = this._activeAction.startValue;
			this._cooldownUntil = Time.time + Mathf.Max(0f, this.toggleCooldown);
			if (this.animator != null && this._maskLayerIndex >= 0)
			{
				this.animator.SetLayerWeight(this._maskLayerIndex, 1f);
				this._targetWeight = 1f;
				this._fadeSpeed = ((this.fadeInDuration > 0f) ? (1f / this.fadeInDuration) : float.PositiveInfinity);
				this.animator.ResetTrigger(this._lowerTriggerHash);
				this.animator.ResetTrigger(this._raiseTriggerHash);
				this.animator.SetTrigger(this.isLowered ? this._lowerTriggerHash : this._raiseTriggerHash);
				this._waitingForActivation = true;
			}
			UnityEvent<bool> onMaskStateChanged = this.OnMaskStateChanged;
			if (onMaskStateChanged == null)
			{
				return;
			}
			onMaskStateChanged.Invoke(this.isLowered);
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x0016DEB8 File Offset: 0x0016C0B8
		private void UpdateLayerWeight()
		{
			if (this.animator == null || this._maskLayerIndex < 0)
			{
				return;
			}
			if (this._waitingForActivation)
			{
				if (this.animator.GetCurrentAnimatorStateInfo(this._maskLayerIndex).shortNameHash != this._emptyStateHash || this.animator.IsInTransition(this._maskLayerIndex))
				{
					this._waitingForActivation = false;
				}
			}
			else if (this._targetWeight > 0f && (this.animator.GetCurrentAnimatorStateInfo(this._maskLayerIndex).shortNameHash == this._emptyStateHash && !this.animator.IsInTransition(this._maskLayerIndex)))
			{
				this._targetWeight = 0f;
				this._fadeSpeed = ((this.fadeOutDuration > 0f) ? (1f / this.fadeOutDuration) : float.PositiveInfinity);
			}
			float layerWeight = this.animator.GetLayerWeight(this._maskLayerIndex);
			if (!Mathf.Approximately(layerWeight, this._targetWeight))
			{
				float num = Mathf.MoveTowards(layerWeight, this._targetWeight, this._fadeSpeed * Time.deltaTime);
				this.animator.SetLayerWeight(this._maskLayerIndex, num);
			}
			if (ClothActionGate.Holds(this) && ((!this._waitingForActivation && this.animator.GetLayerWeight(this._maskLayerIndex) <= 0.001f) || Time.time >= this._tokenDeadline))
			{
				ClothActionGate.Release(this);
			}
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x0016E02C File Offset: 0x0016C22C
		private void CacheBlendShapeIndex()
		{
			this._blendShapeIndex = -1;
			if (this.maskRenderer == null || this.maskRenderer.sharedMesh == null)
			{
				return;
			}
			this._blendShapeIndex = this.maskRenderer.sharedMesh.GetBlendShapeIndex(this.downBlendshapeName);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x0016E07E File Offset: 0x0016C27E
		private void ApplyBlendShape(float value)
		{
			if (this.maskRenderer == null || this._blendShapeIndex < 0)
			{
				return;
			}
			this.maskRenderer.SetBlendShapeWeight(this._blendShapeIndex, value);
		}

		// Token: 0x04003D80 RID: 15744
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04003D81 RID: 15745
		[SerializeField]
		private SkinnedMeshRenderer maskRenderer;

		// Token: 0x04003D82 RID: 15746
		[Header("Animator")]
		[SerializeField]
		private string maskLayerName = "Mask";

		// Token: 0x04003D83 RID: 15747
		[SerializeField]
		private string lowerTrigger = "LowerMask";

		// Token: 0x04003D84 RID: 15748
		[SerializeField]
		private string raiseTrigger = "UpperMask";

		// Token: 0x04003D85 RID: 15749
		[SerializeField]
		private string emptyStateName = "Empty";

		// Token: 0x04003D86 RID: 15750
		[Header("Layer Weight")]
		[SerializeField]
		private float fadeInDuration = 0.05f;

		// Token: 0x04003D87 RID: 15751
		[SerializeField]
		private float fadeOutDuration = 0.2f;

		// Token: 0x04003D88 RID: 15752
		[Header("Blendshape")]
		[SerializeField]
		private string downBlendshapeName = "DOWN";

		// Token: 0x04003D89 RID: 15753
		[SerializeField]
		private MaskToggleController.BlendShapeAction lowerAction = new MaskToggleController.BlendShapeAction
		{
			startValue = 0f,
			targetValue = 100f,
			delay = 0f,
			duration = 0.4f
		};

		// Token: 0x04003D8A RID: 15754
		[SerializeField]
		private MaskToggleController.BlendShapeAction raiseAction = new MaskToggleController.BlendShapeAction
		{
			startValue = 100f,
			targetValue = 0f,
			delay = 0f,
			duration = 0.4f
		};

		// Token: 0x04003D8B RID: 15755
		[Header("Input")]
		[SerializeField]
		private GenericInput toggleInput = new GenericInput("B", "Y", "B");

		// Token: 0x04003D8C RID: 15756
		[Tooltip("Seconds to lock further toggles after triggering an action.")]
		[SerializeField]
		private float toggleCooldown = 2f;

		// Token: 0x04003D8D RID: 15757
		[Header("State")]
		[SerializeField]
		private bool isLowered;

		// Token: 0x04003D8E RID: 15758
		[Header("Events")]
		public UnityEvent<bool> OnMaskStateChanged;

		// Token: 0x04003D8F RID: 15759
		private const string SourceId = "MaskToggleController";

		// Token: 0x04003D90 RID: 15760
		private int _lowerTriggerHash;

		// Token: 0x04003D91 RID: 15761
		private int _raiseTriggerHash;

		// Token: 0x04003D92 RID: 15762
		private int _emptyStateHash;

		// Token: 0x04003D93 RID: 15763
		private int _maskLayerIndex = -1;

		// Token: 0x04003D94 RID: 15764
		private int _blendShapeIndex = -1;

		// Token: 0x04003D95 RID: 15765
		private float _targetWeight;

		// Token: 0x04003D96 RID: 15766
		private float _fadeSpeed;

		// Token: 0x04003D97 RID: 15767
		private bool _waitingForActivation;

		// Token: 0x04003D98 RID: 15768
		private bool _hasEverToggled;

		// Token: 0x04003D99 RID: 15769
		private MaskToggleController.BlendShapeAction _activeAction;

		// Token: 0x04003D9A RID: 15770
		private float _actionStartTime;

		// Token: 0x04003D9B RID: 15771
		private float _currentBlendShapeValue;

		// Token: 0x04003D9C RID: 15772
		private float _cooldownUntil;

		// Token: 0x04003D9D RID: 15773
		private float _tokenDeadline;

		// Token: 0x04003D9E RID: 15774
		private bool _maskAvailable = true;

		// Token: 0x020008E7 RID: 2279
		[Serializable]
		public class BlendShapeAction
		{
			// Token: 0x04003DA0 RID: 15776
			[Range(0f, 100f)]
			public float startValue;

			// Token: 0x04003DA1 RID: 15777
			[Range(0f, 100f)]
			public float targetValue;

			// Token: 0x04003DA2 RID: 15778
			[Tooltip("Seconds to wait after the trigger before starting the blend.")]
			public float delay;

			// Token: 0x04003DA3 RID: 15779
			[Tooltip("Duration of the blend from start to target.")]
			public float duration = 0.4f;
		}
	}
}
