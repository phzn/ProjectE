using System;
using System.Collections.Generic;
using Features.PlayerEmotions;
using Invector.vCover;
using ProjectExhibitionist;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Detection
{
	// Token: 0x020012D1 RID: 4817
	public class PlayerEmotionController : MonoBehaviour
	{
		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06009527 RID: 38183 RVA: 0x003D399B File Offset: 0x003D1B9B
		// (set) Token: 0x06009528 RID: 38184 RVA: 0x003D39A2 File Offset: 0x003D1BA2
		public static PlayerEmotionController Instance { get; private set; }

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x06009529 RID: 38185 RVA: 0x003D39AA File Offset: 0x003D1BAA
		public float ShameLevel
		{
			get
			{
				if (!(EmotionManager.Instance != null))
				{
					return 0f;
				}
				return EmotionManager.Instance.ShameLevel;
			}
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x0600952A RID: 38186 RVA: 0x003D39C9 File Offset: 0x003D1BC9
		public float ExhibitionLevel
		{
			get
			{
				if (!(EmotionManager.Instance != null))
				{
					return 0f;
				}
				return EmotionManager.Instance.ExhibitionLevel;
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x0600952B RID: 38187 RVA: 0x003D39E8 File Offset: 0x003D1BE8
		public float EmotionBalance
		{
			get
			{
				if (!(EmotionManager.Instance != null))
				{
					return 0f;
				}
				return EmotionManager.Instance.EmotionBalance;
			}
		}

		// Token: 0x14000109 RID: 265
		// (add) Token: 0x0600952C RID: 38188 RVA: 0x003D3A08 File Offset: 0x003D1C08
		// (remove) Token: 0x0600952D RID: 38189 RVA: 0x003D3A40 File Offset: 0x003D1C40
		public event Action<float, float> OnHighestDetectionAttentionChanged;

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x0600952E RID: 38190 RVA: 0x003D3A75 File Offset: 0x003D1C75
		public float HighestCurrentAttention
		{
			get
			{
				return this._highestCurrentAttention;
			}
		}

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x0600952F RID: 38191 RVA: 0x003D3A7D File Offset: 0x003D1C7D
		private bool _homeBasePresentationOverride
		{
			get
			{
				return this._homeBasePresentationOwners.Count > 0;
			}
		}

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x06009530 RID: 38192 RVA: 0x003D3A8D File Offset: 0x003D1C8D
		public bool EmotionLayersDisabledForAction
		{
			get
			{
				return this._emotionLayerDisableCount > 0;
			}
		}

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x06009531 RID: 38193 RVA: 0x003D3A98 File Offset: 0x003D1C98
		public bool HomeBasePresentationOverrideActive
		{
			get
			{
				return this._homeBasePresentationOverride;
			}
		}

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x06009532 RID: 38194 RVA: 0x003D3AA0 File Offset: 0x003D1CA0
		public bool IsFacialExpressionOverriddenForAction
		{
			get
			{
				return this._facialExpressionDisabledForAction;
			}
		}

		// Token: 0x06009533 RID: 38195 RVA: 0x003D3AA8 File Offset: 0x003D1CA8
		private void Awake()
		{
			if (PlayerEmotionController.Instance == null)
			{
				PlayerEmotionController.Instance = this;
				return;
			}
			Object.Destroy(this);
		}

		// Token: 0x06009534 RID: 38196 RVA: 0x003D3AC4 File Offset: 0x003D1CC4
		private void Start()
		{
			this.InitializeAnimator();
			if (this.ClothingController != null)
			{
				this.ClothingController.OnClothingStateChanged.AddListener(new UnityAction<bool>(this.OnClothingStateChanged));
			}
			this.TrySubscribeToEmotionManager();
			this.UpdateTargetLayerWeights();
		}

		// Token: 0x06009535 RID: 38197 RVA: 0x003D3B04 File Offset: 0x003D1D04
		private void OnDestroy()
		{
			if (PlayerEmotionController.Instance == this)
			{
				PlayerEmotionController.Instance = null;
			}
			if (this.ClothingController != null)
			{
				this.ClothingController.OnClothingStateChanged.RemoveListener(new UnityAction<bool>(this.OnClothingStateChanged));
			}
			if (this._subscribedToEmotionManager && EmotionManager.Instance != null)
			{
				EmotionManager.Instance.OnEmotionChanged -= this.OnEmotionManagerChanged;
				this._subscribedToEmotionManager = false;
			}
		}

		// Token: 0x06009536 RID: 38198 RVA: 0x003D3B80 File Offset: 0x003D1D80
		private void OnEmotionManagerChanged(EmotionChangedEventArgs args)
		{
			UnityEvent<float, float> onEmotionLevelsChanged = this.OnEmotionLevelsChanged;
			if (onEmotionLevelsChanged != null)
			{
				onEmotionLevelsChanged.Invoke(args.ShameLevel, args.ExhibitionLevel);
			}
			this.UpdateTargetLayerWeights();
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] EmotionManager update: Balance={0:F1} (Shame={1:F1}, Exhibition={2:F1}) Source: {3}", new object[] { args.NewBalance, args.ShameLevel, args.ExhibitionLevel, args.SourceId }));
			}
		}

		// Token: 0x06009537 RID: 38199 RVA: 0x003D3C00 File Offset: 0x003D1E00
		private void OnClothingStateChanged(bool isFullyClothed)
		{
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] Clothing state changed: isFullyClothed = {0}", isFullyClothed));
			}
			this.UpdateTargetLayerWeights();
		}

		// Token: 0x06009538 RID: 38200 RVA: 0x003D3C25 File Offset: 0x003D1E25
		private void Update()
		{
			if (!this._subscribedToEmotionManager)
			{
				this.TrySubscribeToEmotionManager();
			}
			if (Time.time - this._lastTargetRefreshTime > 1f)
			{
				this._lastTargetRefreshTime = Time.time;
				this.UpdateTargetLayerWeights();
			}
			this.UpdateAnimatorLayers();
		}

		// Token: 0x06009539 RID: 38201 RVA: 0x003D3C60 File Offset: 0x003D1E60
		private void TrySubscribeToEmotionManager()
		{
			if (this._subscribedToEmotionManager)
			{
				return;
			}
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.OnEmotionChanged += this.OnEmotionManagerChanged;
				this._subscribedToEmotionManager = true;
				this.UpdateTargetLayerWeights();
				if (this.EnableDebugLog)
				{
					Debug.Log("[PlayerEmotionController] Successfully subscribed to EmotionManager");
				}
			}
		}

		// Token: 0x0600953A RID: 38202 RVA: 0x003D3CB8 File Offset: 0x003D1EB8
		private void InitializeAnimator()
		{
			if (this.PlayerAnimator == null)
			{
				this.PlayerAnimator = base.GetComponentInChildren<Animator>();
			}
			if (this.PlayerAnimator != null)
			{
				if (!string.IsNullOrEmpty(this.FaceLayerName))
				{
					this._faceLayerIndex = this.PlayerAnimator.GetLayerIndex(this.FaceLayerName);
					if (this._faceLayerIndex < 0)
					{
						Debug.LogWarning("[PlayerEmotionController] Face layer '" + this.FaceLayerName + "' not found in animator.");
					}
				}
				if (!string.IsNullOrEmpty(this.ConfidentLayerName))
				{
					this._confidentLayerIndex = this.PlayerAnimator.GetLayerIndex(this.ConfidentLayerName);
					if (this._confidentLayerIndex < 0)
					{
						Debug.LogWarning("[PlayerEmotionController] Confident layer '" + this.ConfidentLayerName + "' not found in animator.");
					}
				}
				if (!string.IsNullOrEmpty(this.EmbarrassedLayerName))
				{
					this._embarrassedLayerIndex = this.PlayerAnimator.GetLayerIndex(this.EmbarrassedLayerName);
					if (this._embarrassedLayerIndex < 0)
					{
						Debug.LogWarning("[PlayerEmotionController] Embarrassed layer '" + this.EmbarrassedLayerName + "' not found in animator.");
					}
				}
				this.UpdateTargetLayerWeights();
			}
		}

		// Token: 0x0600953B RID: 38203 RVA: 0x003D3DCC File Offset: 0x003D1FCC
		private void UpdateTargetLayerWeights()
		{
			bool flag = this.ClothingController != null && this.ClothingController.IsFullyClothed;
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] UpdateTargetLayerWeights - ClothingController: {0}, isClothed: {1}", (this.ClothingController != null) ? "assigned" : "NULL", flag));
			}
			if (this._homeBasePresentationOverride)
			{
				this._targetConfidentWeight = 0f;
				this._targetEmbarrassedWeight = 0f;
				this.ApplyNeutralExpressionIfAvailable();
				return;
			}
			if (flag)
			{
				this._targetConfidentWeight = 0f;
				this._targetEmbarrassedWeight = 0f;
				if (!this._facialExpressionDisabledForAction && this.PlayerAnimator != null && !string.IsNullOrEmpty(this.ExpressionParamName))
				{
					this.PlayerAnimator.SetInteger(this.ExpressionParamName, this.NeutralExpressionIndex);
					if (this.EnableDebugLog)
					{
						Debug.Log(string.Format("[PlayerEmotionController] Clothed - Set {0} = {1} (Base Face)", this.ExpressionParamName, this.NeutralExpressionIndex));
					}
				}
				return;
			}
			float num = this.ExhibitionLevel - this.ShameLevel;
			if (num > this.LayerSwitchThreshold)
			{
				this._targetConfidentWeight = 1f;
				this._targetEmbarrassedWeight = 0f;
			}
			else if (num < -this.LayerSwitchThreshold)
			{
				this._targetConfidentWeight = 0f;
				this._targetEmbarrassedWeight = 1f;
			}
			else
			{
				float num2 = this.ExhibitionLevel + this.ShameLevel;
				if (num2 > 0.01f)
				{
					float num3 = this.ExhibitionLevel / num2;
					this._targetConfidentWeight = num3;
					this._targetEmbarrassedWeight = 1f - num3;
				}
				else
				{
					this._targetConfidentWeight = 1f;
					this._targetEmbarrassedWeight = 0f;
				}
			}
			if (this.PlayerAnimator != null && !string.IsNullOrEmpty(this.EmotionBlendParamName))
			{
				float num4 = this.EmotionBalance / 100f;
				this.PlayerAnimator.SetFloat(this.EmotionBlendParamName, num4);
			}
			this.UpdateFacialExpressionForEmotionLevels();
		}

		// Token: 0x0600953C RID: 38204 RVA: 0x003D3FAC File Offset: 0x003D21AC
		private void UpdateFacialExpressionForEmotionLevels()
		{
			if (this.PlayerAnimator == null || string.IsNullOrEmpty(this.ExpressionParamName))
			{
				return;
			}
			if (this._homeBasePresentationOverride)
			{
				this.ApplyNeutralExpressionIfAvailable();
				return;
			}
			if (this._facialExpressionDisabledForAction)
			{
				return;
			}
			float num = this.ExhibitionLevel - this.ShameLevel;
			int num2;
			if (num > this.LayerSwitchThreshold)
			{
				if (this.ExhibitionLevel > 70f)
				{
					num2 = 3;
				}
				else
				{
					num2 = 2;
				}
			}
			else if (num < -this.LayerSwitchThreshold)
			{
				if (this.ShameLevel > 70f)
				{
					num2 = 4;
				}
				else
				{
					num2 = 1;
				}
			}
			else if (this.ExhibitionLevel >= this.ShameLevel)
			{
				num2 = 2;
			}
			else
			{
				num2 = 1;
			}
			int integer = this.PlayerAnimator.GetInteger(this.ExpressionParamName);
			if (integer != num2)
			{
				this.PlayerAnimator.SetInteger(this.ExpressionParamName, num2);
				if (this.EnableDebugLog)
				{
					Debug.Log(string.Format("[PlayerEmotionController] Expression changed: {0} -> {1} (Shame: {2:F1}, Exhib: {3:F1})", new object[] { integer, num2, this.ShameLevel, this.ExhibitionLevel }));
				}
			}
		}

		// Token: 0x0600953D RID: 38205 RVA: 0x003D40C0 File Offset: 0x003D22C0
		private void UpdateAnimatorLayers()
		{
			if (this.PlayerAnimator == null)
			{
				return;
			}
			if (this._emotionLayerDisableCount > 0)
			{
				return;
			}
			if (this.UseInstantLayerBlending)
			{
				if (this._confidentLayerIndex >= 0)
				{
					this.ApplyEmotionLayerWeight(this._confidentLayerIndex, this._targetConfidentWeight);
				}
				if (this._embarrassedLayerIndex >= 0)
				{
					this.ApplyEmotionLayerWeight(this._embarrassedLayerIndex, this._targetEmbarrassedWeight);
					return;
				}
			}
			else
			{
				float num = this.LayerBlendSpeed * Time.deltaTime;
				if (this._confidentLayerIndex >= 0)
				{
					float num2 = Mathf.MoveTowards(this.PlayerAnimator.GetLayerWeight(this._confidentLayerIndex), this._targetConfidentWeight, num);
					this.ApplyEmotionLayerWeight(this._confidentLayerIndex, num2);
				}
				if (this._embarrassedLayerIndex >= 0)
				{
					float num3 = Mathf.MoveTowards(this.PlayerAnimator.GetLayerWeight(this._embarrassedLayerIndex), this._targetEmbarrassedWeight, num);
					this.ApplyEmotionLayerWeight(this._embarrassedLayerIndex, num3);
				}
			}
		}

		// Token: 0x0600953E RID: 38206 RVA: 0x003D419D File Offset: 0x003D239D
		private void ApplyEmotionLayerWeight(int layer, float weight)
		{
			if (weight > 0f && this.PlayerAnimator.GetLayerWeight(layer) <= 0f)
			{
				this.SynchronizeIncomingWallCoverLayer(layer);
			}
			this.PlayerAnimator.SetLayerWeight(layer, weight);
		}

		// Token: 0x0600953F RID: 38207 RVA: 0x003D41D0 File Offset: 0x003D23D0
		private void SynchronizeIncomingWallCoverLayer(int layer)
		{
			if (this._wallCoverController == null)
			{
				this._wallCoverController = base.GetComponent<vBasicCoverController>();
			}
			if (this._wallCoverController == null)
			{
				this._wallCoverController = this.PlayerAnimator.GetComponentInParent<vBasicCoverController>();
			}
			if (this._wallCoverController == null || !this._wallCoverController.enabled || (!this._wallCoverController.inCover && !this._wallCoverController.goingToCoverPoint))
			{
				return;
			}
			AnimatorStateInfo animatorStateInfo = this.PlayerAnimator.GetCurrentAnimatorStateInfo(0);
			if (this.PlayerAnimator.IsInTransition(0))
			{
				AnimatorStateInfo nextAnimatorStateInfo = this.PlayerAnimator.GetNextAnimatorStateInfo(0);
				if (this._wallCoverController.IsCoverAnimationState(nextAnimatorStateInfo))
				{
					animatorStateInfo = nextAnimatorStateInfo;
				}
			}
			if (!this._wallCoverController.IsCoverAnimationState(animatorStateInfo) || !this.PlayerAnimator.HasState(layer, animatorStateInfo.shortNameHash))
			{
				return;
			}
			this.PlayerAnimator.Play(animatorStateInfo.shortNameHash, layer, animatorStateInfo.normalizedTime);
		}

		// Token: 0x06009540 RID: 38208 RVA: 0x003D42C4 File Offset: 0x003D24C4
		public void OnNPCDetectionProgress(NPCSenses npc, float attention)
		{
			if (npc == null)
			{
				return;
			}
			if (attention > 0.01f)
			{
				this._detectingNPCs.Add(npc);
			}
			else
			{
				this._detectingNPCs.Remove(npc);
			}
			float highestCurrentAttention = this._highestCurrentAttention;
			this._highestCurrentAttention = 0f;
			foreach (NPCSenses npcsenses in this._detectingNPCs)
			{
				if (npcsenses != null && npcsenses.CurrentAttention > this._highestCurrentAttention)
				{
					this._highestCurrentAttention = npcsenses.CurrentAttention;
				}
			}
			if (!Mathf.Approximately(highestCurrentAttention, this._highestCurrentAttention))
			{
				Action<float, float> onHighestDetectionAttentionChanged = this.OnHighestDetectionAttentionChanged;
				if (onHighestDetectionAttentionChanged != null)
				{
					onHighestDetectionAttentionChanged(highestCurrentAttention, this._highestCurrentAttention);
				}
			}
			if (!this._hasTriggeredForCurrentDetection && this._highestCurrentAttention >= this.FacialTriggerThreshold && highestCurrentAttention < this.FacialTriggerThreshold)
			{
				this.TriggerDetectionExpression();
				this._hasTriggeredForCurrentDetection = true;
			}
			if (this._highestCurrentAttention < this.FacialTriggerThreshold * 0.5f)
			{
				this._hasTriggeredForCurrentDetection = false;
			}
		}

		// Token: 0x06009541 RID: 38209 RVA: 0x003D43E0 File Offset: 0x003D25E0
		public void OnDetectionStarted()
		{
			this._isInDetectionSequence = true;
		}

		// Token: 0x06009542 RID: 38210 RVA: 0x003D43E9 File Offset: 0x003D25E9
		public void OnDetectionEnded()
		{
			this._detectingNPCs.Clear();
			this._highestCurrentAttention = 0f;
			this._hasTriggeredForCurrentDetection = false;
			this._isInDetectionSequence = false;
			this._lastDetectionEndTime = Time.time;
			this._isExpressionActive = false;
		}

		// Token: 0x06009543 RID: 38211 RVA: 0x003D4424 File Offset: 0x003D2624
		[Obsolete("Use DetectionEmotionSource.OnPlayerChoice() instead. This method is kept for legacy compatibility.")]
		public void OnPlayerActionSelected(ChoiceCategory actionCategory, float levelIncrease, float oppositeDecrease)
		{
			if (EmotionManager.Instance == null)
			{
				return;
			}
			float num = ((actionCategory == ChoiceCategory.Exhibit) ? levelIncrease : (-levelIncrease));
			EmotionManager.Instance.ModifyBalance(num, "PlayerAction");
			this.TriggerExpressionUpdate();
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] Action selected: {0} -> Balance delta: {1:F1}", actionCategory, num));
			}
		}

		// Token: 0x06009544 RID: 38212 RVA: 0x003D4484 File Offset: 0x003D2684
		private void TriggerDetectionExpression()
		{
			if (this._facialExpressionDisabledForAction)
			{
				return;
			}
			if (Time.time - this._lastExpressionTime < this.ExpressionCooldown)
			{
				return;
			}
			int expressionForCurrentState = this.GetExpressionForCurrentState();
			this.SetExpression(expressionForCurrentState);
			this._lastExpressionTime = Time.time;
			this._isExpressionActive = true;
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] Triggered expression {0} (Shame: {1}, Exhibition: {2})", expressionForCurrentState, this.ShameLevel, this.ExhibitionLevel));
			}
		}

		// Token: 0x06009545 RID: 38213 RVA: 0x003D4504 File Offset: 0x003D2704
		private int GetExpressionForCurrentState()
		{
			bool flag = this.ExhibitionLevel > this.ShameLevel;
			float num = (flag ? this.ExhibitionLevel : this.ShameLevel);
			List<PlayerEmotionController.FacialExpressionEntry> list = (flag ? this.ExhibitionExpressions : this.ShameExpressions);
			if (list == null || list.Count == 0)
			{
				return this.NeutralExpressionIndex;
			}
			foreach (PlayerEmotionController.FacialExpressionEntry facialExpressionEntry in list)
			{
				if (num >= facialExpressionEntry.MinLevel && num <= facialExpressionEntry.MaxLevel)
				{
					return facialExpressionEntry.ExpressionIndex;
				}
			}
			if (list.Count <= 0)
			{
				return this.NeutralExpressionIndex;
			}
			return list[0].ExpressionIndex;
		}

		// Token: 0x06009546 RID: 38214 RVA: 0x003D45CC File Offset: 0x003D27CC
		public void SetExpression(int expressionIndex)
		{
			if (this.PlayerAnimator == null)
			{
				return;
			}
			this.PlayerAnimator.SetInteger(this.ExpressionParamName, expressionIndex);
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] Set {0} = {1}", this.ExpressionParamName, expressionIndex));
			}
			UnityEvent<int> onExpressionTriggered = this.OnExpressionTriggered;
			if (onExpressionTriggered == null)
			{
				return;
			}
			onExpressionTriggered.Invoke(expressionIndex);
		}

		// Token: 0x06009547 RID: 38215 RVA: 0x003D462E File Offset: 0x003D282E
		public void ResetToNeutral()
		{
			this.SetExpression(this.NeutralExpressionIndex);
			this._isExpressionActive = false;
		}

		// Token: 0x06009548 RID: 38216 RVA: 0x003D4644 File Offset: 0x003D2844
		public void TriggerExpressionUpdate()
		{
			if (this._homeBasePresentationOverride)
			{
				this.ApplyNeutralExpressionIfAvailable();
				return;
			}
			if (this._facialExpressionDisabledForAction)
			{
				return;
			}
			int expressionForCurrentState = this.GetExpressionForCurrentState();
			this.SetExpression(expressionForCurrentState);
			this._isExpressionActive = true;
			this._lastExpressionTime = Time.time;
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] TriggerExpressionUpdate: Expression={0} (Shame={1:F1}, Exhib={2:F1})", expressionForCurrentState, this.ShameLevel, this.ExhibitionLevel));
			}
		}

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x06009549 RID: 38217 RVA: 0x003D46BC File Offset: 0x003D28BC
		public bool IsExhibitionDominant
		{
			get
			{
				return this.ExhibitionLevel > this.ShameLevel;
			}
		}

		// Token: 0x0600954A RID: 38218 RVA: 0x003D46CC File Offset: 0x003D28CC
		public ChoiceCategory GetDominantEmotion()
		{
			if (this.ExhibitionLevel <= this.ShameLevel)
			{
				return ChoiceCategory.Shame;
			}
			return ChoiceCategory.Exhibit;
		}

		// Token: 0x0600954B RID: 38219 RVA: 0x003D46E0 File Offset: 0x003D28E0
		public void ForceLayerWeightUpdate()
		{
			this.UpdateTargetLayerWeights();
			if (this.PlayerAnimator == null || this._emotionLayerDisableCount > 0)
			{
				return;
			}
			if (this._confidentLayerIndex >= 0)
			{
				this.ApplyEmotionLayerWeight(this._confidentLayerIndex, this._targetConfidentWeight);
			}
			if (this._embarrassedLayerIndex >= 0)
			{
				this.ApplyEmotionLayerWeight(this._embarrassedLayerIndex, this._targetEmbarrassedWeight);
			}
		}

		// Token: 0x0600954C RID: 38220 RVA: 0x003D4744 File Offset: 0x003D2944
		public void SetHomeBasePresentationOverride(Object owner, bool enabled)
		{
			if (owner == null)
			{
				return;
			}
			bool homeBasePresentationOverride = this._homeBasePresentationOverride;
			if (!(enabled ? this._homeBasePresentationOwners.Add(owner.GetInstanceID()) : this._homeBasePresentationOwners.Remove(owner.GetInstanceID())) || homeBasePresentationOverride == this._homeBasePresentationOverride)
			{
				return;
			}
			this.UpdateTargetLayerWeights();
			if (this.PlayerAnimator == null)
			{
				return;
			}
			if (this._homeBasePresentationOverride)
			{
				if (this._confidentLayerIndex >= 0)
				{
					this.PlayerAnimator.SetLayerWeight(this._confidentLayerIndex, 0f);
				}
				if (this._embarrassedLayerIndex >= 0)
				{
					this.PlayerAnimator.SetLayerWeight(this._embarrassedLayerIndex, 0f);
				}
				this.ApplyNeutralExpressionIfAvailable();
				return;
			}
			if (this._emotionLayerDisableCount <= 0)
			{
				this.ForceLayerWeightUpdate();
			}
		}

		// Token: 0x0600954D RID: 38221 RVA: 0x003D4806 File Offset: 0x003D2A06
		private void ApplyNeutralExpressionIfAvailable()
		{
			if (this._facialExpressionDisabledForAction || this.PlayerAnimator == null || string.IsNullOrEmpty(this.ExpressionParamName))
			{
				return;
			}
			this.PlayerAnimator.SetInteger(this.ExpressionParamName, this.NeutralExpressionIndex);
		}

		// Token: 0x0600954E RID: 38222 RVA: 0x003D4844 File Offset: 0x003D2A44
		public void DisableEmotionLayersForAction()
		{
			if (this.PlayerAnimator == null)
			{
				Debug.LogWarning("[PlayerEmotionController] DisableEmotionLayersForAction: PlayerAnimator NULL — soppressione layer saltata!", this);
				return;
			}
			this._emotionLayerDisableCount++;
			this._emotionLayersDisabledTime = Time.time;
			if (this._emotionLayerDisableCount == 1)
			{
				if (this._confidentLayerIndex >= 0)
				{
					this._savedConfidentWeight = this.PlayerAnimator.GetLayerWeight(this._confidentLayerIndex);
				}
				if (this._embarrassedLayerIndex >= 0)
				{
					this._savedEmbarrassedWeight = this.PlayerAnimator.GetLayerWeight(this._embarrassedLayerIndex);
				}
			}
			if (this._confidentLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._confidentLayerIndex, 0f);
			}
			if (this._embarrassedLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._embarrassedLayerIndex, 0f);
			}
			if (this.EnableDebugLog)
			{
				Debug.Log(string.Format("[PlayerEmotionController] Disabled emotion layers for action (Confident: {0:F2}, Embarrassed: {1:F2}, idxConf={2}, idxEmb={3})", new object[] { this._savedConfidentWeight, this._savedEmbarrassedWeight, this._confidentLayerIndex, this._embarrassedLayerIndex }));
			}
		}

		// Token: 0x0600954F RID: 38223 RVA: 0x003D4960 File Offset: 0x003D2B60
		public void RestoreEmotionLayers()
		{
			if (this.PlayerAnimator == null || this._emotionLayerDisableCount <= 0)
			{
				return;
			}
			this._emotionLayerDisableCount--;
			if (this._emotionLayerDisableCount > 0)
			{
				if (this.EnableDebugLog)
				{
					Debug.Log(string.Format("[PlayerEmotionController] Restore emotion layers (still held, refcount={0})", this._emotionLayerDisableCount));
				}
				return;
			}
			this.UpdateTargetLayerWeights();
			if (this.EnableDebugLog)
			{
				Debug.Log("[PlayerEmotionController] Restored emotion layers after action");
			}
		}

		// Token: 0x06009550 RID: 38224 RVA: 0x003D49D7 File Offset: 0x003D2BD7
		public void DisableFacialExpressionForAction()
		{
			this._facialExpressionDisabledForAction = true;
			if (this.EnableDebugLog)
			{
				Debug.Log("[PlayerEmotionController] Disabled facial expression updates for action");
			}
		}

		// Token: 0x06009551 RID: 38225 RVA: 0x003D49F2 File Offset: 0x003D2BF2
		public void RestoreFacialExpression()
		{
			if (!this._facialExpressionDisabledForAction)
			{
				return;
			}
			this._facialExpressionDisabledForAction = false;
			this.UpdateFacialExpressionForEmotionLevels();
			if (this.EnableDebugLog)
			{
				Debug.Log("[PlayerEmotionController] Restored facial expression updates after action");
			}
		}

		// Token: 0x04009A40 RID: 39488
		[Header("Detection Threshold")]
		[Tooltip("Detection progress (0-1) at which facial expression triggers. E.g., 0.5 = halfway to full detection")]
		[Range(0f, 1f)]
		public float FacialTriggerThreshold = 0.5f;

		// Token: 0x04009A41 RID: 39489
		[Tooltip("Minimum time between facial expression changes (prevents rapid flickering)")]
		public float ExpressionCooldown = 0.1f;

		// Token: 0x04009A42 RID: 39490
		[Header("Debug")]
		[Tooltip("Enable debug logging for emotion changes")]
		public bool EnableDebugLog;

		// Token: 0x04009A43 RID: 39491
		[Header("Animator Configuration")]
		[Tooltip("Reference to the player's Animator")]
		public Animator PlayerAnimator;

		// Token: 0x04009A44 RID: 39492
		[Tooltip("Name of the animator layer for facial expressions")]
		public string FaceLayerName = "Face";

		// Token: 0x04009A45 RID: 39493
		[Tooltip("Name of the Confident/Exhibitionist body animation layer (ex Naked)")]
		public string ConfidentLayerName = "Confident";

		// Token: 0x04009A46 RID: 39494
		[Tooltip("Name of the Embarrassed/Shame body animation layer")]
		public string EmbarrassedLayerName = "Embarassed";

		// Token: 0x04009A47 RID: 39495
		[Tooltip("Reference to ClothingToggleController to check if player is clothed")]
		public ClothingToggleController ClothingController;

		// Token: 0x04009A48 RID: 39496
		[Tooltip("Animator parameter for expression index (Int). Transitions use this value to switch states.")]
		public string ExpressionParamName = "FaceExpressionIndex";

		// Token: 0x04009A49 RID: 39497
		[Tooltip("Animator float parameter for emotion blend (-1 = Shame, +1 = Exhib)")]
		public string EmotionBlendParamName = "EmotionBlend";

		// Token: 0x04009A4A RID: 39498
		[Header("Layer Blend Settings")]
		[Tooltip("If true, layer weight changes are instant. If false, uses smooth blending.")]
		public bool UseInstantLayerBlending = true;

		// Token: 0x04009A4B RID: 39499
		[Tooltip("Speed of layer weight transitions (higher = faster blend). Only used if UseInstantLayerBlending is false.")]
		[Range(0.1f, 20f)]
		public float LayerBlendSpeed = 8f;

		// Token: 0x04009A4C RID: 39500
		[Tooltip("Minimum difference between levels to trigger layer switch")]
		public float LayerSwitchThreshold = 2f;

		// Token: 0x04009A4D RID: 39501
		[Header("Expression Mapping")]
		[Tooltip("Expressions for high shame (scared, embarrassed)")]
		public List<PlayerEmotionController.FacialExpressionEntry> ShameExpressions = new List<PlayerEmotionController.FacialExpressionEntry>();

		// Token: 0x04009A4E RID: 39502
		[Tooltip("Expressions for high exhibition (confident, bold)")]
		public List<PlayerEmotionController.FacialExpressionEntry> ExhibitionExpressions = new List<PlayerEmotionController.FacialExpressionEntry>();

		// Token: 0x04009A4F RID: 39503
		[Tooltip("Neutral/default expression index")]
		public int NeutralExpressionIndex;

		// Token: 0x04009A50 RID: 39504
		[Header("Events")]
		public UnityEvent<int> OnExpressionTriggered;

		// Token: 0x04009A51 RID: 39505
		public UnityEvent<float, float> OnEmotionLevelsChanged;

		// Token: 0x04009A53 RID: 39507
		private int _faceLayerIndex = -1;

		// Token: 0x04009A54 RID: 39508
		private int _confidentLayerIndex = -1;

		// Token: 0x04009A55 RID: 39509
		private int _embarrassedLayerIndex = -1;

		// Token: 0x04009A56 RID: 39510
		private vBasicCoverController _wallCoverController;

		// Token: 0x04009A57 RID: 39511
		private float _targetConfidentWeight;

		// Token: 0x04009A58 RID: 39512
		private float _targetEmbarrassedWeight;

		// Token: 0x04009A59 RID: 39513
		private float _lastExpressionTime = -999f;

		// Token: 0x04009A5A RID: 39514
		private bool _isExpressionActive;

		// Token: 0x04009A5B RID: 39515
		private bool _subscribedToEmotionManager;

		// Token: 0x04009A5C RID: 39516
		private HashSet<NPCSenses> _detectingNPCs = new HashSet<NPCSenses>();

		// Token: 0x04009A5D RID: 39517
		private float _highestCurrentAttention;

		// Token: 0x04009A5E RID: 39518
		private bool _hasTriggeredForCurrentDetection;

		// Token: 0x04009A5F RID: 39519
		private float _lastDetectionEndTime = -999f;

		// Token: 0x04009A60 RID: 39520
		private bool _isInDetectionSequence;

		// Token: 0x04009A61 RID: 39521
		private float _savedConfidentWeight;

		// Token: 0x04009A62 RID: 39522
		private float _savedEmbarrassedWeight;

		// Token: 0x04009A63 RID: 39523
		private int _emotionLayerDisableCount;

		// Token: 0x04009A64 RID: 39524
		private bool _facialExpressionDisabledForAction;

		// Token: 0x04009A65 RID: 39525
		private readonly HashSet<int> _homeBasePresentationOwners = new HashSet<int>();

		// Token: 0x04009A66 RID: 39526
		private float _emotionLayersDisabledTime;

		// Token: 0x04009A67 RID: 39527
		private float _lastTargetRefreshTime;

		// Token: 0x04009A68 RID: 39528
		private const float TARGET_REFRESH_INTERVAL = 1f;

		// Token: 0x020012D2 RID: 4818
		[Serializable]
		public class FacialExpressionEntry
		{
			// Token: 0x04009A69 RID: 39529
			[Tooltip("Display name for debugging")]
			public string Name;

			// Token: 0x04009A6A RID: 39530
			[Tooltip("Animator expression index")]
			public int ExpressionIndex;

			// Token: 0x04009A6B RID: 39531
			[Tooltip("Minimum emotion level (0-100) to use this expression")]
			[Range(0f, 100f)]
			public float MinLevel;

			// Token: 0x04009A6C RID: 39532
			[Tooltip("Maximum emotion level (0-100) to use this expression")]
			[Range(0f, 100f)]
			public float MaxLevel = 100f;
		}
	}
}
