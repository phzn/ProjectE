using System;
using System.Collections;
using Features.DebugTools;
using Features.Rank;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Flash
{
	// Token: 0x02001207 RID: 4615
	public class ClothFullFlashController : MonoBehaviour, IFlashGate
	{
		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06008E78 RID: 36472 RVA: 0x003A3FD8 File Offset: 0x003A21D8
		public int FlashVariant
		{
			get
			{
				return this.flashVariant;
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x06008E79 RID: 36473 RVA: 0x003A3FE0 File Offset: 0x003A21E0
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x06008E7A RID: 36474 RVA: 0x003A3FE8 File Offset: 0x003A21E8
		public bool IsEngaged
		{
			get
			{
				return this._isOpen || this._closeFadeRoutine != null || ClothActionGate.Holds(this);
			}
		}

		// Token: 0x06008E7B RID: 36475 RVA: 0x003A4002 File Offset: 0x003A2202
		public void SetRemoved()
		{
			this._removed = true;
			bool isOpen = this._isOpen;
			this.ResetToNeutral();
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
			}
		}

		// Token: 0x06008E7C RID: 36476 RVA: 0x003A403B File Offset: 0x003A223B
		public void SetRestored()
		{
			this._removed = false;
			this.detailAnimator = null;
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x06008E7D RID: 36477 RVA: 0x003A404C File Offset: 0x003A224C
		public bool IsWorn
		{
			get
			{
				if (this.animator == null)
				{
					return false;
				}
				Transform transform = this.FindWornContainer();
				bool flag = transform != null && transform.gameObject.activeInHierarchy;
				if (flag && this._removed)
				{
					this._removed = false;
					this.detailAnimator = null;
				}
				return flag;
			}
		}

		// Token: 0x06008E7E RID: 36478 RVA: 0x003A40A0 File Offset: 0x003A22A0
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			this._openTriggerHash = Animator.StringToHash("ClothFullOpen");
			this._closeTriggerHash = Animator.StringToHash("ClothFullClose");
			this._variantHash = Animator.StringToHash("ClothFullVariant");
			this._detailStartHash = Animator.StringToHash(this.detailStartTrigger);
			this._detailEndHash = Animator.StringToHash(this.detailEndTrigger);
			if (this.animator != null)
			{
				this._flashLayerIndex = this.animator.GetLayerIndex("ClothFlashFull");
				this._locomotionLayerIndex = ((this.flashVariant == 1) ? this.animator.GetLayerIndex("TowelFlashLocomotion") : (-1));
				this._gestureLayerIndex = ((this.flashVariant == 1) ? this.animator.GetLayerIndex("TowelFlashGesture") : (-1));
				this._baseArmsLayerIndex = ((this.flashVariant == 1) ? this.animator.GetLayerIndex("TowelFlashBaseArms") : (-1));
				if (this._locomotionLayerIndex >= 0)
				{
					this.animator.SetLayerWeight(this._locomotionLayerIndex, 0f);
				}
				if (this._gestureLayerIndex >= 0)
				{
					this.animator.SetLayerWeight(this._gestureLayerIndex, 0f);
				}
				if (this._baseArmsLayerIndex >= 0)
				{
					this.animator.SetLayerWeight(this._baseArmsLayerIndex, 0f);
				}
				if (this._flashLayerIndex >= 0)
				{
					this.animator.SetLayerWeight(this._flashLayerIndex, 0f);
					return;
				}
				Debug.LogWarning("[ClothFullFlashController] Shared layer 'ClothFlashFull' not found on the player Animator.", this);
			}
		}

		// Token: 0x06008E7F RID: 36479 RVA: 0x003A422E File Offset: 0x003A242E
		private void OnEnable()
		{
			FlashGateRegistry.Register(this);
			this.detailAnimator = null;
			this._removed = false;
		}

		// Token: 0x06008E80 RID: 36480 RVA: 0x003A4244 File Offset: 0x003A2444
		private void OnDisable()
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.InterruptForEncounter();
			}
			FlashGateRegistry.Unregister(this);
			this.ResetToNeutral();
			ClothActionGate.Release(this);
		}

		// Token: 0x06008E81 RID: 36481 RVA: 0x003A426B File Offset: 0x003A246B
		private void OnDestroy()
		{
			FlashGateRegistry.Unregister(this);
		}

		// Token: 0x06008E82 RID: 36482 RVA: 0x003A4274 File Offset: 0x003A2474
		private void Update()
		{
			this.UpdateLocomotionLayerWeight();
			this.SynchronizeBaseArmsPhase();
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (Time.time < this._cooldownUntil)
			{
				return;
			}
			if (this.toggleInput != null && this.toggleInput.GetButtonDown())
			{
				this.Toggle();
				this._cooldownUntil = Time.time + Mathf.Max(0f, this.toggleCooldown);
			}
		}

		// Token: 0x06008E83 RID: 36483 RVA: 0x003A42DA File Offset: 0x003A24DA
		private void LateUpdate()
		{
			this.UpdateLocomotionLayerWeight();
		}

		// Token: 0x06008E84 RID: 36484 RVA: 0x003A42E4 File Offset: 0x003A24E4
		private void UpdateLocomotionLayerWeight()
		{
			if (this.animator == null || this._flashLayerIndex < 0 || this._locomotionLayerIndex < 0 || this._gestureLayerIndex < 0 || this._baseArmsLayerIndex < 0)
			{
				return;
			}
			float num = Mathf.Clamp01(this.animator.GetFloat(ClothFullFlashController.InputMagnitudeHash) / 0.1f);
			float layerWeight = this.animator.GetLayerWeight(this._flashLayerIndex);
			float num2 = layerWeight * num;
			this.animator.SetLayerWeight(this._locomotionLayerIndex, num2);
			this.animator.SetLayerWeight(this._baseArmsLayerIndex, (num2 > 0f) ? 1f : 0f);
			this.animator.SetLayerWeight(this._gestureLayerIndex, (num2 > 0f) ? layerWeight : 0f);
		}

		// Token: 0x06008E85 RID: 36485 RVA: 0x003A43B0 File Offset: 0x003A25B0
		private void SynchronizeBaseArmsPhase()
		{
			if (this.animator == null || this._baseArmsLayerIndex < 0 || this.animator.GetLayerWeight(this._baseArmsLayerIndex) <= 0f)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.shortNameHash == ClothFullFlashController.BaseLocomotionHash)
			{
				AnimatorStateInfo currentAnimatorStateInfo2 = this.animator.GetCurrentAnimatorStateInfo(this._baseArmsLayerIndex);
				this.animator.SetFloat(ClothFullFlashController.BaseArmsCycleHash, Mathf.Repeat(currentAnimatorStateInfo.normalizedTime - currentAnimatorStateInfo2.normalizedTime, 1f));
			}
		}

		// Token: 0x06008E86 RID: 36486 RVA: 0x003A4443 File Offset: 0x003A2643
		public void Toggle()
		{
			if (!this._isOpen)
			{
				this.Open();
				return;
			}
			this.Close();
		}

		// Token: 0x06008E87 RID: 36487 RVA: 0x003A445C File Offset: 0x003A265C
		private void Open()
		{
			if (this._removed || this.animator == null || this._flashLayerIndex < 0)
			{
				return;
			}
			if (!this.IsWorn)
			{
				return;
			}
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Flash))
			{
				BlockedActionFeedback.Show(null);
				return;
			}
			if (!ClothActionGate.TryBegin(this))
			{
				return;
			}
			if (!FlashGateRegistry.TryAcquireActivity(this))
			{
				ClothActionGate.Release(this);
				return;
			}
			if (this._closeFadeRoutine != null)
			{
				base.StopCoroutine(this._closeFadeRoutine);
				this._closeFadeRoutine = null;
			}
			if (this._openReleaseRoutine != null)
			{
				base.StopCoroutine(this._openReleaseRoutine);
				this._openReleaseRoutine = null;
			}
			this._isOpen = true;
			this.ResolveDetailAnimator();
			this.animator.SetInteger(this._variantHash, this.flashVariant);
			this.animator.SetLayerWeight(this._flashLayerIndex, 1f);
			this.UpdateLocomotionLayerWeight();
			this.SynchronizeBaseArmsPhase();
			this.animator.ResetTrigger(this._closeTriggerHash);
			this.animator.SetTrigger(this._openTriggerHash);
			this.FireDetail(true);
			this._openReleaseRoutine = base.StartCoroutine(this.ReleaseAfterOpenGesture());
			UnityEvent<bool> onFlashStateChanged = this.OnFlashStateChanged;
			if (onFlashStateChanged != null)
			{
				onFlashStateChanged.Invoke(true);
			}
			UnityEvent onOpened = this.OnOpened;
			if (onOpened == null)
			{
				return;
			}
			onOpened.Invoke();
		}

		// Token: 0x06008E88 RID: 36488 RVA: 0x003A4592 File Offset: 0x003A2792
		private IEnumerator ReleaseAfterOpenGesture()
		{
			int openHash = Animator.StringToHash(this.openStateName);
			float waited = 0f;
			while (waited < this.closeMaxWait)
			{
				if (!this._isOpen || (this.animator != null && this._flashLayerIndex >= 0 && this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex).shortNameHash == openHash && !this.animator.IsInTransition(this._flashLayerIndex)))
				{
					IL_0159:
					while (waited < this.closeMaxWait && this._isOpen)
					{
						if (this.animator != null && this._flashLayerIndex >= 0)
						{
							AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex);
							if (currentAnimatorStateInfo.shortNameHash != openHash || currentAnimatorStateInfo.normalizedTime >= 0.98f)
							{
								break;
							}
						}
						waited += Time.deltaTime;
						yield return null;
					}
					this._openReleaseRoutine = null;
					if (this._isOpen)
					{
						ClothActionGate.Release(this);
					}
					yield break;
				}
				waited += Time.deltaTime;
				yield return null;
			}
			goto IL_0159;
		}

		// Token: 0x06008E89 RID: 36489 RVA: 0x003A45A4 File Offset: 0x003A27A4
		private void Close()
		{
			if (this.animator == null || this._flashLayerIndex < 0)
			{
				return;
			}
			if (!ClothActionGate.TryBegin(this))
			{
				return;
			}
			if (this._openReleaseRoutine != null)
			{
				base.StopCoroutine(this._openReleaseRoutine);
				this._openReleaseRoutine = null;
			}
			this._isOpen = false;
			this.animator.ResetTrigger(this._openTriggerHash);
			this.animator.SetTrigger(this._closeTriggerHash);
			this.FireDetail(false);
			if (this._closeFadeRoutine != null)
			{
				base.StopCoroutine(this._closeFadeRoutine);
			}
			this._closeFadeRoutine = base.StartCoroutine(this.FadeLayerAfterClose());
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
		}

		// Token: 0x06008E8A RID: 36490 RVA: 0x003A4665 File Offset: 0x003A2865
		private void ForceCloseImmediate()
		{
			this.ResetToNeutral();
		}

		// Token: 0x06008E8B RID: 36491 RVA: 0x003A4670 File Offset: 0x003A2870
		public void InterruptForEncounter()
		{
			if (!this.IsEngaged)
			{
				return;
			}
			bool isOpen = this._isOpen;
			this.ResetToNeutral();
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
			FlashDetailPose.Complete(this.detailAnimator, this.detailClosedStateName, 0);
		}

		// Token: 0x06008E8C RID: 36492 RVA: 0x003A46CC File Offset: 0x003A28CC
		public void ResetToNeutral()
		{
			this._isOpen = false;
			this._cooldownUntil = 0f;
			if (this._closeFadeRoutine != null)
			{
				base.StopCoroutine(this._closeFadeRoutine);
				this._closeFadeRoutine = null;
			}
			if (this._openReleaseRoutine != null)
			{
				base.StopCoroutine(this._openReleaseRoutine);
				this._openReleaseRoutine = null;
			}
			if (this.animator != null && this._flashLayerIndex >= 0)
			{
				this.animator.ResetTrigger(this._openTriggerHash);
				this.animator.ResetTrigger(this._closeTriggerHash);
				int num = Animator.StringToHash("Empty");
				if (this.animator.HasState(this._flashLayerIndex, num))
				{
					this.animator.Play(num, this._flashLayerIndex, 0f);
				}
				this.animator.SetLayerWeight(this._flashLayerIndex, 0f);
			}
			if (this.animator != null && this._locomotionLayerIndex >= 0)
			{
				this.animator.SetLayerWeight(this._locomotionLayerIndex, 0f);
			}
			if (this.animator != null && this._gestureLayerIndex >= 0)
			{
				this.animator.SetLayerWeight(this._gestureLayerIndex, 0f);
			}
			if (this.animator != null && this._baseArmsLayerIndex >= 0)
			{
				this.animator.SetLayerWeight(this._baseArmsLayerIndex, 0f);
			}
			this.ResetDetailToNeutral();
			ClothActionGate.Release(this);
			FlashGateRegistry.ReleaseActivityIfIdle();
		}

		// Token: 0x06008E8D RID: 36493 RVA: 0x003A4840 File Offset: 0x003A2A40
		private void ResetDetailToNeutral()
		{
			if (this.detailAnimator == null || this.detailAnimator.runtimeAnimatorController == null)
			{
				return;
			}
			this.detailAnimator.ResetTrigger(this._detailStartHash);
			this.detailAnimator.ResetTrigger(this._detailEndHash);
			int num = Animator.StringToHash("Empty");
			if (this.detailAnimator.isActiveAndEnabled && this.detailAnimator.HasState(0, num))
			{
				this.detailAnimator.Play(num, 0, 0f);
			}
		}

		// Token: 0x06008E8E RID: 36494 RVA: 0x003A48CA File Offset: 0x003A2ACA
		private IEnumerator FadeLayerAfterClose()
		{
			int closeHash = Animator.StringToHash(this.closeStateName);
			float waited = 0f;
			while (waited < this.closeMaxWait)
			{
				if (this._isOpen)
				{
					this._closeFadeRoutine = null;
					yield break;
				}
				if (!(this.animator != null) || this._flashLayerIndex < 0 || this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex).shortNameHash != closeHash || this.animator.IsInTransition(this._flashLayerIndex))
				{
					waited += Time.deltaTime;
					yield return null;
				}
				else
				{
					IL_016D:
					while (waited < this.closeMaxWait)
					{
						if (this._isOpen)
						{
							this._closeFadeRoutine = null;
							yield break;
						}
						if (this.animator != null && this._flashLayerIndex >= 0)
						{
							AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(this._flashLayerIndex);
							if (currentAnimatorStateInfo.shortNameHash != closeHash || currentAnimatorStateInfo.normalizedTime >= this.closeCompleteAt)
							{
								break;
							}
						}
						waited += Time.deltaTime;
						yield return null;
					}
					if (this._isOpen)
					{
						this._closeFadeRoutine = null;
						yield break;
					}
					float t = 0f;
					float start = ((this.animator != null && this._flashLayerIndex >= 0) ? this.animator.GetLayerWeight(this._flashLayerIndex) : 0f);
					float dur = Mathf.Max(0.01f, this.fadeOutDuration);
					while (t < dur)
					{
						if (this._isOpen)
						{
							this._closeFadeRoutine = null;
							yield break;
						}
						t += Time.deltaTime;
						if (this.animator != null && this._flashLayerIndex >= 0)
						{
							this.animator.SetLayerWeight(this._flashLayerIndex, Mathf.Lerp(start, 0f, t / dur));
						}
						this.UpdateLocomotionLayerWeight();
						yield return null;
					}
					if (this.animator != null && this._flashLayerIndex >= 0)
					{
						this.animator.SetLayerWeight(this._flashLayerIndex, 0f);
					}
					this.UpdateLocomotionLayerWeight();
					this._closeFadeRoutine = null;
					ClothActionGate.Release(this);
					FlashGateRegistry.ReleaseActivityIfIdle();
					yield break;
				}
			}
			goto IL_016D;
		}

		// Token: 0x06008E8F RID: 36495 RVA: 0x003A48D9 File Offset: 0x003A2AD9
		public Animator EnsureDetailAnimator()
		{
			this.ResolveDetailAnimator();
			return this.detailAnimator;
		}

		// Token: 0x06008E90 RID: 36496 RVA: 0x003A48E8 File Offset: 0x003A2AE8
		private void FireDetail(bool open)
		{
			if (this.detailAnimator == null || this.detailAnimator.runtimeAnimatorController == null)
			{
				this.ResolveDetailAnimator();
			}
			if (this.detailAnimator == null)
			{
				return;
			}
			if (this.detailAnimator.gameObject.activeInHierarchy && !this.detailAnimator.enabled)
			{
				this.detailAnimator.enabled = true;
			}
			if (!this.detailAnimator.isActiveAndEnabled)
			{
				return;
			}
			this.PropagateBoobsTypeToDetail();
			this.detailAnimator.ResetTrigger(this._detailStartHash);
			this.detailAnimator.ResetTrigger(this._detailEndHash);
			this.detailAnimator.SetTrigger(open ? this._detailStartHash : this._detailEndHash);
		}

		// Token: 0x06008E91 RID: 36497 RVA: 0x003A49A8 File Offset: 0x003A2BA8
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
			if (!ClothFullFlashController.HasParam(this.detailAnimator, "BoobsType"))
			{
				return;
			}
			this.detailAnimator.SetFloat(this._boobsTypeHash, this.animator.GetFloat(this._boobsTypeHash));
		}

		// Token: 0x06008E92 RID: 36498 RVA: 0x003A4A20 File Offset: 0x003A2C20
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

		// Token: 0x06008E93 RID: 36499 RVA: 0x003A4A58 File Offset: 0x003A2C58
		private void ResolveDetailAnimator()
		{
			if (this.animator == null)
			{
				return;
			}
			if (this.detailAnimator != null && this.detailAnimator.runtimeAnimatorController != null)
			{
				return;
			}
			if (this.detailAnimator == null)
			{
				foreach (Animator animator in this.animator.GetComponentsInChildren<Animator>(true))
				{
					if (!(animator == this.animator) && this.NameMatches(animator.gameObject.name) && !(animator.GetComponent<SkinnedMeshRenderer>() != null))
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
				Debug.LogWarning("[ClothFullFlashController] No worn garment '" + this.wornObjectName + "' found. It will not deform during the flash.", this);
				return;
			}
			if (this.detailAnimator.runtimeAnimatorController == null)
			{
				if (this.detailOnlyController == null)
				{
					Debug.LogWarning("[ClothFullFlashController] Detail Animator on '" + this.detailAnimator.gameObject.name + "' has no controller and detailOnlyController is unassigned.", this);
					return;
				}
				this.detailAnimator.runtimeAnimatorController = this.detailOnlyController;
			}
			this.detailAnimator.applyRootMotion = false;
			this.detailAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this.PropagateBoobsTypeToDetail();
		}

		// Token: 0x06008E94 RID: 36500 RVA: 0x003A4C00 File Offset: 0x003A2E00
		private Transform FindWornContainer()
		{
			if (this.animator == null)
			{
				return null;
			}
			foreach (Transform transform in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatches(transform.name) && transform.gameObject.activeInHierarchy && transform.GetComponent<SkinnedMeshRenderer>() != null)
				{
					return transform;
				}
			}
			foreach (Transform transform2 in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatches(transform2.name) && transform2.gameObject.activeInHierarchy && transform2.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform2;
				}
			}
			foreach (Transform transform3 in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatches(transform3.name) && transform3.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform3;
				}
			}
			return null;
		}

		// Token: 0x06008E95 RID: 36501 RVA: 0x003A4CF5 File Offset: 0x003A2EF5
		private bool NameMatches(string n)
		{
			return !string.IsNullOrEmpty(n) && (n == this.wornObjectName || n.StartsWith(this.wornObjectName));
		}

		// Token: 0x040091F1 RID: 37361
		public const string SharedLayerName = "ClothFlashFull";

		// Token: 0x040091F2 RID: 37362
		public const string TowelLocomotionLayerName = "TowelFlashLocomotion";

		// Token: 0x040091F3 RID: 37363
		public const string TowelGestureLayerName = "TowelFlashGesture";

		// Token: 0x040091F4 RID: 37364
		public const string TowelBaseArmsLayerName = "TowelFlashBaseArms";

		// Token: 0x040091F5 RID: 37365
		public const string BaseArmsStateName = "Base_Movement";

		// Token: 0x040091F6 RID: 37366
		public const string BaseLocomotionStateName = "Free Movement";

		// Token: 0x040091F7 RID: 37367
		public const string BaseArmsCycleParameter = "TowelBaseArmsCycleOffset";

		// Token: 0x040091F8 RID: 37368
		public const string SharedOpenTrigger = "ClothFullOpen";

		// Token: 0x040091F9 RID: 37369
		public const string SharedCloseTrigger = "ClothFullClose";

		// Token: 0x040091FA RID: 37370
		public const string SharedVariantParam = "ClothFullVariant";

		// Token: 0x040091FB RID: 37371
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x040091FC RID: 37372
		[Tooltip("Animator on the worn garment playing the garment-only blendshape clips. Auto-found/created.")]
		[SerializeField]
		private Animator detailAnimator;

		// Token: 0x040091FD RID: 37373
		[Header("Variant on the shared ClothFlashFull layer")]
		[Tooltip("Which variant sub-states this garment drives (ClothFullVariant int). Towel=1.")]
		[SerializeField]
		private int flashVariant = 1;

		// Token: 0x040091FE RID: 37374
		[Header("Worn garment")]
		[Tooltip("Mesh root name of the worn garment (matched by exact name or StartsWith). E.g. 'Bath_Towel'.")]
		[SerializeField]
		private string wornObjectName = "Bath_Towel";

		// Token: 0x040091FF RID: 37375
		[Tooltip("Garment-only blendshape controller (e.g. TowelFlash_TowelOnly), added at runtime if needed.")]
		[SerializeField]
		private RuntimeAnimatorController detailOnlyController;

		// Token: 0x04009200 RID: 37376
		[Tooltip("Trigger on the DETAIL animator when opening (blendshape open).")]
		[SerializeField]
		private string detailStartTrigger = "TowelFlashStart";

		// Token: 0x04009201 RID: 37377
		[Tooltip("Trigger on the DETAIL animator when closing (blendshape close).")]
		[SerializeField]
		private string detailEndTrigger = "TowelFlashEnd";

		// Token: 0x04009202 RID: 37378
		[Tooltip("Authored detail closing state sampled at its endpoint when a flash is interrupted.")]
		[SerializeField]
		private string detailClosedStateName = "Flash_End";

		// Token: 0x04009203 RID: 37379
		[Header("Layer weight")]
		[SerializeField]
		private float fadeInDuration = 0.1f;

		// Token: 0x04009204 RID: 37380
		[SerializeField]
		private float fadeOutDuration = 0.2f;

		// Token: 0x04009205 RID: 37381
		[Tooltip("Nome dello stato di CHIUSURA sul layer ClothFlashFull (es. 'Towel_Close'). La coroutine aspetta che QUESTO stato finisca di giocare (normalizedTime >= soglia) prima di sfumare il layer a 0, così il gesto di chiusura si completa SEMPRE prima del blend off (niente più interruzione a metà). Niente timer fissi.")]
		[SerializeField]
		private string closeStateName = "Towel_Close";

		// Token: 0x04009206 RID: 37382
		[Tooltip("Nome dello stato di APERTURA sul layer ClothFlashFull (es. 'Towel_Open'). Serve a rilasciare il semaforo quando la gesture di apertura è conclusa (telo aperto+fermo).")]
		[SerializeField]
		private string openStateName = "Towel_Open";

		// Token: 0x04009207 RID: 37383
		[Tooltip("normalizedTime a cui considerare il gesto di chiusura concluso (0.98 = quasi fine clip).")]
		[Range(0.8f, 1f)]
		[SerializeField]
		private float closeCompleteAt = 0.98f;

		// Token: 0x04009208 RID: 37384
		[Tooltip("Safety: secondi massimi di attesa del completamento prima di forzare il fade (anti-deadlock).")]
		[SerializeField]
		private float closeMaxWait = 3f;

		// Token: 0x04009209 RID: 37385
		[Header("Input")]
		[Tooltip("Tasto di toggle apri/chiudi. Letto solo mentre il componente è enabled (il bridge lo abilita su worn+selected).")]
		[SerializeField]
		private GenericInput toggleInput = new GenericInput("B", "Y", "B");

		// Token: 0x0400920A RID: 37386
		[Tooltip("Secondi di lock dopo un toggle, per evitare doppi trigger.")]
		[SerializeField]
		private float toggleCooldown = 0.6f;

		// Token: 0x0400920B RID: 37387
		private float _cooldownUntil;

		// Token: 0x0400920C RID: 37388
		[Header("Events")]
		public UnityEvent<bool> OnFlashStateChanged;

		// Token: 0x0400920D RID: 37389
		public UnityEvent OnOpened;

		// Token: 0x0400920E RID: 37390
		public UnityEvent OnClosed;

		// Token: 0x0400920F RID: 37391
		private int _openTriggerHash;

		// Token: 0x04009210 RID: 37392
		private int _closeTriggerHash;

		// Token: 0x04009211 RID: 37393
		private int _variantHash;

		// Token: 0x04009212 RID: 37394
		private int _detailStartHash;

		// Token: 0x04009213 RID: 37395
		private int _detailEndHash;

		// Token: 0x04009214 RID: 37396
		private int _flashLayerIndex = -1;

		// Token: 0x04009215 RID: 37397
		private int _locomotionLayerIndex = -1;

		// Token: 0x04009216 RID: 37398
		private int _gestureLayerIndex = -1;

		// Token: 0x04009217 RID: 37399
		private int _baseArmsLayerIndex = -1;

		// Token: 0x04009218 RID: 37400
		private static readonly int InputMagnitudeHash = Animator.StringToHash("InputMagnitude");

		// Token: 0x04009219 RID: 37401
		private static readonly int BaseLocomotionHash = Animator.StringToHash("Free Movement");

		// Token: 0x0400921A RID: 37402
		private static readonly int BaseArmsCycleHash = Animator.StringToHash("TowelBaseArmsCycleOffset");

		// Token: 0x0400921B RID: 37403
		private bool _isOpen;

		// Token: 0x0400921C RID: 37404
		private bool _removed;

		// Token: 0x0400921D RID: 37405
		private Coroutine _closeFadeRoutine;

		// Token: 0x0400921E RID: 37406
		private Coroutine _openReleaseRoutine;

		// Token: 0x0400921F RID: 37407
		private const string BoobsTypeParam = "BoobsType";

		// Token: 0x04009220 RID: 37408
		private int _boobsTypeHash;
	}
}
