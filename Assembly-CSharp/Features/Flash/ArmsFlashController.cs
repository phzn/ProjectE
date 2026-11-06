using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Rank;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Flash
{
	// Token: 0x02001203 RID: 4611
	public class ArmsFlashController : MonoBehaviour, IFlashGate
	{
		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x06008E3D RID: 36413 RVA: 0x003A2849 File Offset: 0x003A0A49
		public string PlayerLayerName
		{
			get
			{
				if (this.config == null || string.IsNullOrWhiteSpace(this.config.sharedLayerName))
				{
					return "ClothFlashArms";
				}
				return this.config.sharedLayerName;
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x06008E3E RID: 36414 RVA: 0x003A2876 File Offset: 0x003A0A76
		public ArmsFlashConfig Config
		{
			get
			{
				return this.config;
			}
		}

		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x06008E3F RID: 36415 RVA: 0x003A287E File Offset: 0x003A0A7E
		public Animator PlayerAnimator
		{
			get
			{
				return this.animator;
			}
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x06008E40 RID: 36416 RVA: 0x003A2886 File Offset: 0x003A0A86
		public bool HasActiveArmsAnimation
		{
			get
			{
				return this.IsBusy || (this._isOpen && this.LayerIsInMyHoldState());
			}
		}

		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x06008E41 RID: 36417 RVA: 0x003A28A2 File Offset: 0x003A0AA2
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x06008E42 RID: 36418 RVA: 0x003A28AA File Offset: 0x003A0AAA
		public bool IsBusy
		{
			get
			{
				return this._drivingLayer || ClothActionGate.Holds(this);
			}
		}

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x06008E43 RID: 36419 RVA: 0x003A28BC File Offset: 0x003A0ABC
		public bool IsOnCooldown
		{
			get
			{
				return Time.time < this._cooldownUntil;
			}
		}

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x06008E44 RID: 36420 RVA: 0x003A28CB File Offset: 0x003A0ACB
		public bool IsEngaged
		{
			get
			{
				return this._isOpen || this.IsBusy;
			}
		}

		// Token: 0x06008E45 RID: 36421 RVA: 0x003A28DD File Offset: 0x003A0ADD
		public void SetRemoved()
		{
			this.SetGarmentRemoved();
		}

		// Token: 0x06008E46 RID: 36422 RVA: 0x003A28E5 File Offset: 0x003A0AE5
		public void SetRestored()
		{
			this.SetGarmentRestored();
		}

		// Token: 0x06008E47 RID: 36423 RVA: 0x003A28ED File Offset: 0x003A0AED
		public void SetGarmentSearchRoot(Transform root)
		{
			if (this._garmentSearchRoot == root)
			{
				return;
			}
			this._garmentSearchRoot = root;
			this.InvalidateWornStateCache();
			this.detailAnimator = null;
		}

		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06008E48 RID: 36424 RVA: 0x003A2914 File Offset: 0x003A0B14
		public bool IsWorn
		{
			get
			{
				if (this.animator == null)
				{
					return false;
				}
				Transform transform = ((this._garmentSearchRoot != null) ? this._garmentSearchRoot : this.animator.transform);
				Transform transform2;
				if (this.HasValidWornStateCache(transform))
				{
					transform2 = this._cachedWornContainer;
				}
				else
				{
					this.InvalidateWornStateCache();
					transform2 = this.FindWornContainerUnder(transform);
					if (transform2 != null && transform2.gameObject.activeInHierarchy)
					{
						this._cachedWornContainer = transform2;
						this._cachedWornRenderer = transform2.GetComponentInChildren<SkinnedMeshRenderer>(true);
						this._cachedWornLookupRoot = transform;
						this._cachedWornObjectName = this.config.wornObjectName;
					}
				}
				if (transform2 != null && transform2.gameObject.activeInHierarchy)
				{
					if (this._removed)
					{
						this._removed = false;
						this.detailAnimator = null;
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x06008E49 RID: 36425 RVA: 0x003A29E8 File Offset: 0x003A0BE8
		private bool HasValidWornStateCache(Transform lookupRoot)
		{
			return this._cachedWornContainer != null && this._cachedWornRenderer != null && this._cachedWornLookupRoot == lookupRoot && this.config != null && this._cachedWornObjectName == this.config.wornObjectName && this._cachedWornContainer.gameObject.activeInHierarchy && this._cachedWornContainer.IsChildOf(lookupRoot) && this._cachedWornRenderer.transform.IsChildOf(this._cachedWornContainer) && this.NameMatches(this._cachedWornContainer.name);
		}

		// Token: 0x06008E4A RID: 36426 RVA: 0x003A2A8D File Offset: 0x003A0C8D
		private void InvalidateWornStateCache()
		{
			this._cachedWornContainer = null;
			this._cachedWornRenderer = null;
			this._cachedWornLookupRoot = null;
			this._cachedWornObjectName = null;
		}

		// Token: 0x06008E4B RID: 36427 RVA: 0x003A2AAC File Offset: 0x003A0CAC
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			this._detailStartTriggerHash = Animator.StringToHash(this.config.startTrigger);
			this._detailEndTriggerHash = Animator.StringToHash(this.config.endTrigger);
			this._sharedStartTriggerHash = Animator.StringToHash("ClothFlashStart");
			this._sharedEndTriggerHash = Animator.StringToHash("ClothFlashEnd");
			this._sharedVariantHash = Animator.StringToHash("ClothFlashVariant");
			this._emptyStateHash = Animator.StringToHash(this.config.emptyStateName);
			this._holdStateHash = (string.IsNullOrEmpty(this.config.holdStateName) ? 0 : Animator.StringToHash(this.config.holdStateName));
			if (this.animator != null)
			{
				this._flashLayerIndex = this.animator.GetLayerIndex(this.PlayerLayerName);
				if (this._flashLayerIndex >= 0)
				{
					this.animator.SetLayerWeight(this._flashLayerIndex, 0f);
					return;
				}
				Debug.LogWarning("[ArmsFlashController] Shared layer '" + this.PlayerLayerName + "' not found on the player Animator.", this);
			}
		}

		// Token: 0x06008E4C RID: 36428 RVA: 0x003A2BD0 File Offset: 0x003A0DD0
		private void OnEnable()
		{
			FlashGateRegistry.Register(this);
		}

		// Token: 0x06008E4D RID: 36429 RVA: 0x003A2BD8 File Offset: 0x003A0DD8
		public Animator EnsureDetailAnimator()
		{
			this.ResolveDetailAnimator();
			return this.detailAnimator;
		}

		// Token: 0x06008E4E RID: 36430 RVA: 0x003A2BE8 File Offset: 0x003A0DE8
		private void ResolveDetailAnimator()
		{
			if (this.animator == null)
			{
				return;
			}
			if (this.detailAnimator != null && this.detailAnimator.runtimeAnimatorController != null)
			{
				if (this.detailAnimator.gameObject.activeInHierarchy)
				{
					BoobsSizeCorrectiveShapeDriver.EnsureOn(this.detailAnimator.gameObject);
					return;
				}
				this.detailAnimator = null;
			}
			if (this.detailAnimator == null)
			{
				foreach (Animator animator in ((this._garmentSearchRoot != null) ? this._garmentSearchRoot : this.animator.transform).GetComponentsInChildren<Animator>(true))
				{
					if (!(animator == this.animator) && this.NameMatches(animator.gameObject.name) && animator.gameObject.activeInHierarchy && !(animator.GetComponent<SkinnedMeshRenderer>() != null))
					{
						this.detailAnimator = animator;
						break;
					}
				}
			}
			if (this.detailAnimator == null)
			{
				Transform transform = this.FindWornContainer();
				if (transform != null)
				{
					Transform transform2 = ((transform.parent != null) ? transform.parent : transform);
					this.detailAnimator = transform2.GetComponent<Animator>();
					if (this.detailAnimator == null)
					{
						this.detailAnimator = transform2.gameObject.AddComponent<Animator>();
					}
				}
			}
			if (this.detailAnimator == null)
			{
				Debug.LogWarning("[ArmsFlashController] No worn garment '" + this.config.wornObjectName + "' found. It will not deform during the flash.", this);
				return;
			}
			if (this.detailAnimator.runtimeAnimatorController == null)
			{
				if (this.config.detailOnlyController == null)
				{
					Debug.LogWarning("[ArmsFlashController] Detail Animator on '" + this.detailAnimator.gameObject.name + "' has no controller and config.detailOnlyController is unassigned.", this);
					return;
				}
				this.detailAnimator.runtimeAnimatorController = this.config.detailOnlyController;
			}
			this.detailAnimator.applyRootMotion = false;
			this.detailAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			if (this.config.useDedicatedSizeClips)
			{
				this.PropagateBoobsTypeToDetail();
				return;
			}
			BoobsSizeCorrectiveShapeDriver.EnsureOn(this.detailAnimator.gameObject);
		}

		// Token: 0x06008E4F RID: 36431 RVA: 0x003A2E14 File Offset: 0x003A1014
		private void PropagateBoobsTypeToDetail()
		{
			if (this.animator == null || this.detailAnimator == null)
			{
				return;
			}
			if (this._boobsTypeHash == 0)
			{
				this._boobsTypeHash = Animator.StringToHash("BoobsType");
			}
			if (!ArmsFlashController.HasParam(this.detailAnimator, "BoobsType"))
			{
				return;
			}
			float @float = this.animator.GetFloat(this._boobsTypeHash);
			this.detailAnimator.SetFloat(this._boobsTypeHash, @float);
		}

		// Token: 0x06008E50 RID: 36432 RVA: 0x003A2E90 File Offset: 0x003A1090
		private static bool HasParam(Animator a, string name)
		{
			AnimatorControllerParameter[] parameters = a.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008E51 RID: 36433 RVA: 0x003A2EC5 File Offset: 0x003A10C5
		private Transform FindWornContainer()
		{
			if (this.animator == null)
			{
				return null;
			}
			if (this._garmentSearchRoot != null)
			{
				return this.FindWornContainerUnder(this._garmentSearchRoot);
			}
			return this.FindWornContainerUnder(this.animator.transform);
		}

		// Token: 0x06008E52 RID: 36434 RVA: 0x003A2F04 File Offset: 0x003A1104
		private Transform FindWornContainerUnder(Transform root)
		{
			if (root == null)
			{
				return null;
			}
			root.GetComponentsInChildren<Transform>(true, this._wornLookupCandidates);
			Transform transform = null;
			Transform transform2 = null;
			for (int i = 0; i < this._wornLookupCandidates.Count; i++)
			{
				Transform transform3 = this._wornLookupCandidates[i];
				if (this.NameMatches(transform3.name))
				{
					bool activeInHierarchy = transform3.gameObject.activeInHierarchy;
					SkinnedMeshRenderer skinnedMeshRenderer = transform3.GetComponent<SkinnedMeshRenderer>();
					if (activeInHierarchy && skinnedMeshRenderer != null)
					{
						return transform3;
					}
					if (skinnedMeshRenderer == null)
					{
						skinnedMeshRenderer = transform3.GetComponentInChildren<SkinnedMeshRenderer>(true);
					}
					if (!(skinnedMeshRenderer == null))
					{
						if (activeInHierarchy && transform == null)
						{
							transform = transform3;
						}
						if (transform2 == null)
						{
							transform2 = transform3;
						}
					}
				}
			}
			if (!(transform != null))
			{
				return transform2;
			}
			return transform;
		}

		// Token: 0x06008E53 RID: 36435 RVA: 0x003A2FC9 File Offset: 0x003A11C9
		private bool NameMatches(string n)
		{
			return !string.IsNullOrEmpty(n) && (n == this.config.wornObjectName || n.StartsWith(this.config.wornObjectName));
		}

		// Token: 0x06008E54 RID: 36436 RVA: 0x003A2FFC File Offset: 0x003A11FC
		private void Update()
		{
			if (this.config.listenForOwnInput && !FlyCamera.IsActive && this.config.toggleInput != null && this.config.toggleInput.GetButtonDown() && !this.IsOnCooldown)
			{
				this.Toggle();
			}
			this.UpdateLayerWeight();
		}

		// Token: 0x06008E55 RID: 36437 RVA: 0x003A3050 File Offset: 0x003A1250
		public void Toggle()
		{
			if (this._removed || this.animator == null || this._flashLayerIndex < 0)
			{
				return;
			}
			if (this.IsOnCooldown)
			{
				return;
			}
			if (!this._isOpen && this.config.gateAsFlash && PlayerActivityGate.IsBlocked(PlayerActivity.Flash))
			{
				BlockedActionFeedback.Show(null);
				return;
			}
			if (!this._isOpen && !this.IsWorn)
			{
				return;
			}
			if (!ClothActionGate.TryBegin(this))
			{
				return;
			}
			if (!this._isOpen && this.config.gateAsFlash && !FlashGateRegistry.TryAcquireActivity(this))
			{
				ClothActionGate.Release(this);
				return;
			}
			this._tokenDeadline = Time.time + 5f;
			this._isOpen = !this._isOpen;
			this._cooldownUntil = Time.time + Mathf.Max(0f, this.config.toggleCooldown);
			this.FireTriggers(this._isOpen);
			if (this.animator != null && this._flashLayerIndex >= 0)
			{
				if (!this.config.fadeInFromZero)
				{
					this.animator.SetLayerWeight(this._flashLayerIndex, 1f);
				}
				this._targetWeight = 1f;
				this._fadeSpeed = ((this.config.fadeInDuration > 0f) ? (1f / this.config.fadeInDuration) : float.PositiveInfinity);
				this._waitingForActivation = true;
				this._drivingLayer = true;
			}
			UnityEvent<bool> onFlashStateChanged = this.OnFlashStateChanged;
			if (onFlashStateChanged != null)
			{
				onFlashStateChanged.Invoke(this._isOpen);
			}
			if (this._isOpen)
			{
				UnityEvent onOpened = this.OnOpened;
				if (onOpened == null)
				{
					return;
				}
				onOpened.Invoke();
				return;
			}
			else
			{
				UnityEvent onClosed = this.OnClosed;
				if (onClosed == null)
				{
					return;
				}
				onClosed.Invoke();
				return;
			}
		}

		// Token: 0x06008E56 RID: 36438 RVA: 0x003A31F8 File Offset: 0x003A13F8
		private void OnDisable()
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.InterruptForEncounter();
			}
			FlashGateRegistry.Unregister(this);
			bool flag = ClothActionGate.Holds(this) || this.LayerIsInMyHoldState();
			this.ResetToNeutral(flag);
			ClothActionGate.Release(this);
		}

		// Token: 0x06008E57 RID: 36439 RVA: 0x003A323D File Offset: 0x003A143D
		private void OnDestroy()
		{
			FlashGateRegistry.Unregister(this);
		}

		// Token: 0x06008E58 RID: 36440 RVA: 0x003A3248 File Offset: 0x003A1448
		public void InterruptForEncounter()
		{
			if (!this.IsEngaged || (this.config != null && !this.config.gateAsFlash && !this.IsBusy))
			{
				return;
			}
			bool isOpen = this._isOpen;
			this.ResetToNeutral(this.IsBusy || this.LayerIsInMyHoldState());
			if (isOpen)
			{
				UnityEvent<bool> onFlashStateChanged = this.OnFlashStateChanged;
				if (onFlashStateChanged != null)
				{
					onFlashStateChanged.Invoke(false);
				}
				UnityEvent onClosed = this.OnClosed;
				if (onClosed != null)
				{
					onClosed.Invoke();
				}
			}
			if (this.config != null)
			{
				FlashDetailPose.Complete(this.detailAnimator, this.config.DetailClosedStateName, this.config.detailLayerIndex);
			}
		}

		// Token: 0x06008E59 RID: 36441 RVA: 0x003A32E8 File Offset: 0x003A14E8
		public void ResetToNeutral(bool clearSharedLayer)
		{
			this._isOpen = false;
			this._waitingForActivation = false;
			this._targetWeight = 0f;
			this._drivingLayer = false;
			this._cooldownUntil = 0f;
			if (clearSharedLayer && this.animator != null && this._flashLayerIndex >= 0)
			{
				this.animator.ResetTrigger(this._sharedStartTriggerHash);
				this.animator.ResetTrigger(this._sharedEndTriggerHash);
				this.animator.Play(this._emptyStateHash, this._flashLayerIndex, 0f);
				this.animator.SetLayerWeight(this._flashLayerIndex, 0f);
			}
			this.ResetDetailToNeutral();
			ClothActionGate.Release(this);
			FlashGateRegistry.ReleaseActivityIfIdle();
		}

		// Token: 0x06008E5A RID: 36442 RVA: 0x003A33A0 File Offset: 0x003A15A0
		private void ResetDetailToNeutral()
		{
			if (this.detailAnimator == null || this.detailAnimator.runtimeAnimatorController == null)
			{
				return;
			}
			this.detailAnimator.ResetTrigger(this._detailStartTriggerHash);
			this.detailAnimator.ResetTrigger(this._detailEndTriggerHash);
			if (!this.config.resetDetailToEmpty)
			{
				return;
			}
			int num = Mathf.Clamp(this.config.detailLayerIndex, 0, this.detailAnimator.layerCount - 1);
			if (this.detailAnimator.isActiveAndEnabled && this.detailAnimator.HasState(num, this._emptyStateHash))
			{
				this.detailAnimator.Play(this._emptyStateHash, num, 0f);
			}
		}

		// Token: 0x06008E5B RID: 36443 RVA: 0x003A3458 File Offset: 0x003A1658
		public void SetGarmentRemoved()
		{
			this.InvalidateWornStateCache();
			this._removed = true;
			bool isOpen = this._isOpen;
			this.ResetToNeutral(ClothActionGate.Holds(this) || this.LayerIsInMyHoldState());
			if (isOpen)
			{
				UnityEvent<bool> onFlashStateChanged = this.OnFlashStateChanged;
				if (onFlashStateChanged != null)
				{
					onFlashStateChanged.Invoke(false);
				}
				UnityEvent onClosed = this.OnClosed;
				if (onClosed != null)
				{
					onClosed.Invoke();
				}
			}
			ClothActionGate.Release(this);
		}

		// Token: 0x06008E5C RID: 36444 RVA: 0x003A34BC File Offset: 0x003A16BC
		private bool LayerIsInMyHoldState()
		{
			return this._holdStateHash != 0 && !(this.animator == null) && this._flashLayerIndex >= 0 && this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex).shortNameHash == this._holdStateHash;
		}

		// Token: 0x06008E5D RID: 36445 RVA: 0x003A350B File Offset: 0x003A170B
		public void SetOpenLatch(bool open)
		{
			this._isOpen = open;
		}

		// Token: 0x06008E5E RID: 36446 RVA: 0x003A3514 File Offset: 0x003A1714
		public void SetGarmentRestored()
		{
			this.InvalidateWornStateCache();
			this._removed = false;
			this.detailAnimator = null;
		}

		// Token: 0x06008E5F RID: 36447 RVA: 0x003A352C File Offset: 0x003A172C
		private void FireTriggers(bool open)
		{
			if (this.detailAnimator == null || this.detailAnimator.runtimeAnimatorController == null)
			{
				this.ResolveDetailAnimator();
			}
			this.animator.SetInteger(this._sharedVariantHash, this.config.flashVariant);
			this.animator.ResetTrigger(this._sharedStartTriggerHash);
			this.animator.ResetTrigger(this._sharedEndTriggerHash);
			this.animator.SetTrigger(open ? this._sharedStartTriggerHash : this._sharedEndTriggerHash);
			if (this.detailAnimator != null && this.detailAnimator.gameObject.activeInHierarchy && !this.detailAnimator.enabled)
			{
				this.detailAnimator.enabled = true;
			}
			if (this.detailAnimator != null && this.detailAnimator.isActiveAndEnabled)
			{
				if (this.config.useDedicatedSizeClips)
				{
					this.PropagateBoobsTypeToDetail();
				}
				this.detailAnimator.ResetTrigger(this._detailStartTriggerHash);
				this.detailAnimator.ResetTrigger(this._detailEndTriggerHash);
				this.detailAnimator.SetTrigger(open ? this._detailStartTriggerHash : this._detailEndTriggerHash);
			}
		}

		// Token: 0x06008E60 RID: 36448 RVA: 0x003A3660 File Offset: 0x003A1860
		private void UpdateLayerWeight()
		{
			if (this.animator == null || this._flashLayerIndex < 0)
			{
				return;
			}
			if (!this._drivingLayer)
			{
				return;
			}
			if (this._waitingForActivation)
			{
				if (this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex).shortNameHash != this._emptyStateHash || this.animator.IsInTransition(this._flashLayerIndex))
				{
					this._waitingForActivation = false;
				}
			}
			else if (this._targetWeight > 0f && (this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex).shortNameHash == this._emptyStateHash && !this.animator.IsInTransition(this._flashLayerIndex)))
			{
				this._targetWeight = 0f;
				this._fadeSpeed = ((this.config.fadeOutDuration > 0f) ? (1f / this.config.fadeOutDuration) : float.PositiveInfinity);
			}
			float layerWeight = this.animator.GetLayerWeight(this._flashLayerIndex);
			if (!Mathf.Approximately(layerWeight, this._targetWeight))
			{
				float num = Mathf.MoveTowards(layerWeight, this._targetWeight, this._fadeSpeed * Time.deltaTime);
				this.animator.SetLayerWeight(this._flashLayerIndex, num);
			}
			else if (this._targetWeight <= 0f && !ClothActionGate.Holds(this))
			{
				this._drivingLayer = false;
				FlashGateRegistry.ReleaseActivityIfIdle();
			}
			if (ClothActionGate.Holds(this))
			{
				bool flag = !this._waitingForActivation && this.animator.GetLayerWeight(this._flashLayerIndex) <= 0.001f && !this.IsDetailAnimating();
				bool flag2 = false;
				if (this._isOpen && this._holdStateHash != 0 && !this._waitingForActivation && !this.animator.IsInTransition(this._flashLayerIndex) && !this.IsDetailAnimating())
				{
					flag2 = this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex).shortNameHash == this._holdStateHash;
				}
				if (Time.time >= this._tokenDeadline && !flag && !flag2)
				{
					bool isOpen = this._isOpen;
					this.ResetToNeutral(true);
					if (isOpen)
					{
						UnityEvent<bool> onFlashStateChanged = this.OnFlashStateChanged;
						if (onFlashStateChanged != null)
						{
							onFlashStateChanged.Invoke(false);
						}
						UnityEvent onClosed = this.OnClosed;
						if (onClosed == null)
						{
							return;
						}
						onClosed.Invoke();
						return;
					}
				}
				else
				{
					if (flag)
					{
						this._drivingLayer = false;
						ClothActionGate.Release(this);
						FlashGateRegistry.ReleaseActivityIfIdle();
						return;
					}
					if (flag2)
					{
						ClothActionGate.Release(this);
					}
				}
			}
		}

		// Token: 0x06008E61 RID: 36449 RVA: 0x003A38D0 File Offset: 0x003A1AD0
		private bool IsDetailAnimating()
		{
			if (this.detailAnimator == null || !this.detailAnimator.isActiveAndEnabled)
			{
				return false;
			}
			if (this.detailAnimator.runtimeAnimatorController == null)
			{
				return false;
			}
			int num = Mathf.Clamp(this.config.detailLayerIndex, 0, this.detailAnimator.layerCount - 1);
			if (this.detailAnimator.IsInTransition(num))
			{
				return true;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.detailAnimator.GetCurrentAnimatorStateInfo(num);
			return currentAnimatorStateInfo.normalizedTime < 0.99f && currentAnimatorStateInfo.length > 0f;
		}

		// Token: 0x040091C3 RID: 37315
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x040091C4 RID: 37316
		[Tooltip("Animator on the worn garment playing the garment-only blendshape clips. Auto-found/created under the player Animator if not assigned.")]
		[SerializeField]
		private Animator detailAnimator;

		// Token: 0x040091C5 RID: 37317
		private Transform _garmentSearchRoot;

		// Token: 0x040091C6 RID: 37318
		private readonly List<Transform> _wornLookupCandidates = new List<Transform>();

		// Token: 0x040091C7 RID: 37319
		private Transform _cachedWornContainer;

		// Token: 0x040091C8 RID: 37320
		private SkinnedMeshRenderer _cachedWornRenderer;

		// Token: 0x040091C9 RID: 37321
		private Transform _cachedWornLookupRoot;

		// Token: 0x040091CA RID: 37322
		private string _cachedWornObjectName;

		// Token: 0x040091CB RID: 37323
		[Header("Config")]
		[SerializeField]
		private ArmsFlashConfig config = new ArmsFlashConfig();

		// Token: 0x040091CC RID: 37324
		[Header("Events")]
		[Tooltip("Raised on every toggle with the new open state (true = exposed, false = covered).")]
		public UnityEvent<bool> OnFlashStateChanged;

		// Token: 0x040091CD RID: 37325
		[Tooltip("Raised when the garment is flashed OPEN (exposed). Hook breast jiggle etc. here.")]
		public UnityEvent OnOpened;

		// Token: 0x040091CE RID: 37326
		[Tooltip("Raised when the garment is covered back up.")]
		public UnityEvent OnClosed;

		// Token: 0x040091CF RID: 37327
		public const string SharedLayerName = "ClothFlashArms";

		// Token: 0x040091D0 RID: 37328
		public const string SharedStartTrigger = "ClothFlashStart";

		// Token: 0x040091D1 RID: 37329
		public const string SharedEndTrigger = "ClothFlashEnd";

		// Token: 0x040091D2 RID: 37330
		public const string SharedVariantParam = "ClothFlashVariant";

		// Token: 0x040091D3 RID: 37331
		private int _detailStartTriggerHash;

		// Token: 0x040091D4 RID: 37332
		private int _detailEndTriggerHash;

		// Token: 0x040091D5 RID: 37333
		private int _sharedStartTriggerHash;

		// Token: 0x040091D6 RID: 37334
		private int _sharedEndTriggerHash;

		// Token: 0x040091D7 RID: 37335
		private int _sharedVariantHash;

		// Token: 0x040091D8 RID: 37336
		private int _emptyStateHash;

		// Token: 0x040091D9 RID: 37337
		private int _holdStateHash;

		// Token: 0x040091DA RID: 37338
		private int _flashLayerIndex = -1;

		// Token: 0x040091DB RID: 37339
		private float _targetWeight;

		// Token: 0x040091DC RID: 37340
		private float _fadeSpeed;

		// Token: 0x040091DD RID: 37341
		private bool _drivingLayer;

		// Token: 0x040091DE RID: 37342
		private bool _waitingForActivation;

		// Token: 0x040091DF RID: 37343
		private bool _isOpen;

		// Token: 0x040091E0 RID: 37344
		private bool _removed;

		// Token: 0x040091E1 RID: 37345
		private float _cooldownUntil;

		// Token: 0x040091E2 RID: 37346
		private float _tokenDeadline;

		// Token: 0x040091E3 RID: 37347
		private const string BoobsTypeParam = "BoobsType";

		// Token: 0x040091E4 RID: 37348
		private int _boobsTypeHash;
	}
}
