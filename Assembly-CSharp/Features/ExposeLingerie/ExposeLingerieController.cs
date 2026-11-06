using System;
using System.Collections;
using BoZo.CharacterCreator.MC;
using Features.Detection;
using Features.Flash;
using Features.InsertableToys;
using Features.Rank;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Features.ExposeLingerie
{
	// Token: 0x02001214 RID: 4628
	public class ExposeLingerieController : MonoBehaviour, IFlashGate
	{
		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x06008F07 RID: 36615 RVA: 0x003A81BC File Offset: 0x003A63BC
		public bool IsPlaying
		{
			get
			{
				return this._isPlaying;
			}
		}

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x06008F08 RID: 36616 RVA: 0x003A81C4 File Offset: 0x003A63C4
		public Animator PlayerAnimator
		{
			get
			{
				return this.animator;
			}
		}

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x06008F09 RID: 36617 RVA: 0x003A81CC File Offset: 0x003A63CC
		public bool IsOnCooldown
		{
			get
			{
				return Time.time < this._cooldownUntil;
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x06008F0A RID: 36618 RVA: 0x003A81DB File Offset: 0x003A63DB
		public bool IsBraOpen
		{
			get
			{
				return this._braOpen;
			}
		}

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x06008F0B RID: 36619 RVA: 0x003A81E3 File Offset: 0x003A63E3
		public bool IsPantiesOpen
		{
			get
			{
				return this._pantiesOpen;
			}
		}

		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x06008F0C RID: 36620 RVA: 0x003A81EB File Offset: 0x003A63EB
		public bool AnyOpen
		{
			get
			{
				return this._braOpen || this._pantiesOpen;
			}
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x06008F0D RID: 36621 RVA: 0x003A81FD File Offset: 0x003A63FD
		public bool IsEngaged
		{
			get
			{
				return this._isPlaying || this.AnyOpen;
			}
		}

		// Token: 0x06008F0E RID: 36622 RVA: 0x003A820F File Offset: 0x003A640F
		void IFlashGate.SetRemoved()
		{
			this.NotifyGarmentRemoved(true);
			this.NotifyGarmentRemoved(false);
		}

		// Token: 0x06008F0F RID: 36623 RVA: 0x00004F31 File Offset: 0x00003131
		void IFlashGate.SetRestored()
		{
		}

		// Token: 0x06008F10 RID: 36624 RVA: 0x003A8220 File Offset: 0x003A6420
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			this._openTriggerHash = Animator.StringToHash("ClothFlashStart");
			this._closeTriggerHash = Animator.StringToHash("ClothFlashEnd");
			this._variantHash = Animator.StringToHash("ClothFlashVariant");
			this._boobsTypeHash = Animator.StringToHash("BoobsType");
			if (this.animator != null)
			{
				this._layerIndex = this.animator.GetLayerIndex("ClothFlashArms");
				if (this._layerIndex < 0)
				{
					Debug.LogWarning("[ExposeLingerieController] Shared layer 'ClothFlashArms' not found on the player Animator.", this);
				}
				this._playerInput = this.animator.GetComponentInParent<vThirdPersonInput>();
				if (this._playerInput == null)
				{
					this._playerInput = this.animator.GetComponentInChildren<vThirdPersonInput>(true);
				}
			}
		}

		// Token: 0x06008F11 RID: 36625 RVA: 0x003A82F0 File Offset: 0x003A64F0
		private void OnDisable()
		{
			if (this._isPlaying || this._routine != null)
			{
				this.ForceEndImmediate();
			}
			this.ReleaseGestureInteraction();
		}

		// Token: 0x06008F12 RID: 36626 RVA: 0x003A830E File Offset: 0x003A650E
		public bool ToggleBra()
		{
			return this.Toggle(true);
		}

		// Token: 0x06008F13 RID: 36627 RVA: 0x003A8317 File Offset: 0x003A6517
		public bool TogglePanties()
		{
			return this.Toggle(false);
		}

		// Token: 0x06008F14 RID: 36628 RVA: 0x003A8320 File Offset: 0x003A6520
		private bool Toggle(bool isBra)
		{
			bool flag = (isBra ? this._braOpen : this._pantiesOpen);
			return this.Play(isBra, !flag);
		}

		// Token: 0x06008F15 RID: 36629 RVA: 0x003A834A File Offset: 0x003A654A
		public void NotifyGarmentRemoved(bool isBra)
		{
			if (isBra)
			{
				this._braOpen = false;
			}
			else
			{
				this._pantiesOpen = false;
			}
			if (this._isPlaying || this._routine != null)
			{
				this.ForceEndImmediate();
			}
		}

		// Token: 0x06008F16 RID: 36630 RVA: 0x003A8375 File Offset: 0x003A6575
		public void ClearOpenLatch(bool isBra)
		{
			if (isBra)
			{
				this._braOpen = false;
				return;
			}
			this._pantiesOpen = false;
		}

		// Token: 0x06008F17 RID: 36631 RVA: 0x003A838C File Offset: 0x003A658C
		private bool Play(bool isBra, bool opening)
		{
			if (!isBra && !opening)
			{
				InsertableToyController insertableToyController = ((this.animator != null) ? this.animator.GetComponentInChildren<InsertableToyController>(true) : base.GetComponentInParent<InsertableToyController>());
				if (insertableToyController != null && insertableToyController.IsInserted)
				{
					BlockedActionFeedback.Show("Remove the inserted item first");
					return false;
				}
			}
			if (this._isPlaying || this._routine != null || this.IsOnCooldown)
			{
				return false;
			}
			if (this.animator == null || this._layerIndex < 0)
			{
				return false;
			}
			if (opening && PlayerActivityGate.IsBlocked(PlayerActivity.LingerieOpen))
			{
				BlockedActionFeedback.Show(null);
				return false;
			}
			if (!ClothActionGate.TryBegin(this))
			{
				return false;
			}
			if (!this.TryAcquireGestureInteraction())
			{
				ClothActionGate.Release(this);
				return false;
			}
			Animator animator = this.ResolveDetailAnimator(isBra ? this.braRootName : this.pantiesRootName);
			this._detailAnimator = animator;
			this._cooldownUntil = Time.time + Mathf.Max(0f, this.toggleCooldown);
			this._isPlaying = true;
			if (isBra)
			{
				this._braOpen = opening;
			}
			else
			{
				this._pantiesOpen = opening;
			}
			int num = (isBra ? this.braVariant : this.pantiesVariant);
			int num2 = Mathf.Clamp(Mathf.RoundToInt(this.animator.GetFloat(this._boobsTypeHash)), 0, 5);
			this.animator.SetInteger(this._variantHash, num);
			this.animator.ResetTrigger(opening ? this._closeTriggerHash : this._openTriggerHash);
			this.animator.SetTrigger(opening ? this._openTriggerHash : this._closeTriggerHash);
			if (animator != null)
			{
				animator.enabled = true;
				if (isBra)
				{
					animator.SetFloat(this._boobsTypeHash, (float)num2);
				}
				string text = (isBra ? (opening ? "Bra_Open" : "Bra_Close") : (opening ? "Panties" : "Panties_Close"));
				this._detailStateName = text;
				animator.Play(text, 0, 0f);
			}
			if (this.lockInputDuringGesture && this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(true);
				this._lockedInput = true;
			}
			if (this.suppressEmotionLayers && PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.DisableEmotionLayersForAction();
				this._suppressedLayers = true;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[ExposeLingerie] Play ",
					isBra ? "BRA" : "PANTIES",
					" ",
					opening ? "OPEN" : "CLOSE",
					" ",
					string.Format("variant={0} boobs={1} layer={2} detail={3}", new object[]
					{
						num,
						num2,
						this._layerIndex,
						(animator != null) ? animator.gameObject.name : "NULL"
					})
				}), this);
			}
			ClothingBlendShapeSyncController.BeginExternalGarmentDrive();
			UnityEvent onGestureStarted = this.OnGestureStarted;
			if (onGestureStarted != null)
			{
				onGestureStarted.Invoke();
			}
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
			}
			this._routine = base.StartCoroutine(this.WatchGesture());
			return true;
		}

		// Token: 0x06008F18 RID: 36632 RVA: 0x003A86A3 File Offset: 0x003A68A3
		private IEnumerator WatchGesture()
		{
			PlayerActivityLease gestureLease = this._gestureLease;
			float waited = 0f;
			while (waited < this.maxGestureSeconds)
			{
				float num = ((this.fadeInDuration > 0f) ? Mathf.MoveTowards(this.animator.GetLayerWeight(this._layerIndex), 1f, Time.deltaTime / this.fadeInDuration) : 1f);
				this.animator.SetLayerWeight(this._layerIndex, num);
				if (this.animator.GetCurrentAnimatorStateInfo(this._layerIndex).IsTag("ExposeLingerie"))
				{
					break;
				}
				waited += Time.deltaTime;
				yield return null;
			}
			if (this.debugLogs)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(this._layerIndex);
				Debug.Log(string.Format("[ExposeLingerie] Gesture state entered={0} after {1:F2}s ", currentAnimatorStateInfo.IsTag("ExposeLingerie"), waited) + string.Format("(weight={0:F2}, normTime={1:F2})", this.animator.GetLayerWeight(this._layerIndex), currentAnimatorStateInfo.normalizedTime), this);
			}
			while (waited < this.maxGestureSeconds)
			{
				this.animator.SetLayerWeight(this._layerIndex, 1f);
				if (!this.animator.GetCurrentAnimatorStateInfo(this._layerIndex).IsTag("ExposeLingerie") && !this.animator.IsInTransition(this._layerIndex))
				{
					break;
				}
				waited += Time.deltaTime;
				yield return null;
			}
			this.EndGesture();
			float t = 0f;
			float start = this.animator.GetLayerWeight(this._layerIndex);
			float dur = Mathf.Max(0.01f, this.fadeOutDuration);
			while (t < dur)
			{
				if (this._isPlaying)
				{
					if (this._gestureLease == gestureLease)
					{
						this.ReleaseGestureInteraction();
					}
					yield break;
				}
				t += Time.deltaTime;
				this.animator.SetLayerWeight(this._layerIndex, Mathf.Lerp(start, 0f, t / dur));
				yield return null;
			}
			this.animator.SetLayerWeight(this._layerIndex, 0f);
			if (this._gestureLease == gestureLease)
			{
				this.ReleaseGestureInteraction();
			}
			this._routine = null;
			yield break;
		}

		// Token: 0x06008F19 RID: 36633 RVA: 0x003A86B4 File Offset: 0x003A68B4
		private void EndGesture()
		{
			if (!this._isPlaying)
			{
				return;
			}
			this._isPlaying = false;
			ClothActionGate.Release(this);
			ClothingBlendShapeSyncController.EndExternalGarmentDrive();
			if (this._lockedInput && this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(false);
			}
			this._lockedInput = false;
			if (this._suppressedLayers && PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.RestoreEmotionLayers();
			}
			this._suppressedLayers = false;
			if (this._detailAnimator != null)
			{
				this._detailAnimator.enabled = false;
			}
			UnityEvent onGestureEnded = this.OnGestureEnded;
			if (onGestureEnded == null)
			{
				return;
			}
			onGestureEnded.Invoke();
		}

		// Token: 0x06008F1A RID: 36634 RVA: 0x003A8755 File Offset: 0x003A6955
		private void ReleaseGestureInteraction()
		{
			PlayerActivityLease gestureLease = this._gestureLease;
			if (gestureLease != null)
			{
				gestureLease.Dispose();
			}
			this._gestureLease = null;
		}

		// Token: 0x06008F1B RID: 36635 RVA: 0x003A8770 File Offset: 0x003A6970
		private bool TryAcquireGestureInteraction()
		{
			PlayerContextResource playerContextResource = (FlashGateRegistry.HasSettledSeat(this.animator) ? PlayerContextResource.HeldItem : PlayerContextResource.WorldInteraction);
			return PlayerActivityGate.TryAcquire("ExposeLingerieGesture", this, PlayerActivity.None, playerContextResource, PlayerActivity.Interact, out this._gestureLease, false, null);
		}

		// Token: 0x06008F1C RID: 36636 RVA: 0x003A87A8 File Offset: 0x003A69A8
		public void InterruptForEncounter()
		{
			if (!this._isPlaying && this._routine == null && !ClothActionGate.Holds(this) && (this._gestureLease == null || !this._gestureLease.IsActive))
			{
				return;
			}
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
				this._routine = null;
			}
			if (this._isPlaying)
			{
				FlashDetailPose.Complete(this._detailAnimator, this._detailStateName, 0);
			}
			this.EndGesture();
			ClothActionGate.Release(this);
			this.ReleaseGestureInteraction();
			this.ClearArmGesture();
			this._cooldownUntil = 0f;
		}

		// Token: 0x06008F1D RID: 36637 RVA: 0x003A883C File Offset: 0x003A6A3C
		private void ClearArmGesture()
		{
			if (this.animator == null || this._layerIndex < 0)
			{
				return;
			}
			this.animator.ResetTrigger(this._openTriggerHash);
			this.animator.ResetTrigger(this._closeTriggerHash);
			this.animator.Play("Empty", this._layerIndex);
			this.animator.SetLayerWeight(this._layerIndex, 0f);
		}

		// Token: 0x06008F1E RID: 36638 RVA: 0x003A88B0 File Offset: 0x003A6AB0
		private void ForceEndImmediate()
		{
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
				this._routine = null;
			}
			this.EndGesture();
			this.ReleaseGestureInteraction();
			this.ClearArmGesture();
			this._braOpen = false;
			this._pantiesOpen = false;
			this._cooldownUntil = 0f;
		}

		// Token: 0x06008F1F RID: 36639 RVA: 0x003A8904 File Offset: 0x003A6B04
		private Animator ResolveDetailAnimator(string rootName)
		{
			if (this.animator == null)
			{
				return null;
			}
			Transform transform = this.FindGarmentRoot(rootName);
			if (transform == null)
			{
				Debug.LogWarning("[ExposeLingerieController] No worn garment '" + rootName + "' found. It will not deform during the gesture.", this);
				return null;
			}
			Animator animator = transform.GetComponent<Animator>();
			if (animator == null)
			{
				animator = transform.gameObject.AddComponent<Animator>();
			}
			if (animator.runtimeAnimatorController == null)
			{
				if (this.detailController == null)
				{
					Debug.LogWarning("[ExposeLingerieController] detailController is unassigned.", this);
					return null;
				}
				animator.runtimeAnimatorController = this.detailController;
			}
			animator.applyRootMotion = false;
			animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			return animator;
		}

		// Token: 0x06008F20 RID: 36640 RVA: 0x003A89AC File Offset: 0x003A6BAC
		private Transform FindGarmentRoot(string rootName)
		{
			string text = ((rootName == this.braRootName) ? "Bra" : "Panty");
			foreach (Transform transform in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (ExposeLingerieController.NameMatches(transform.name, rootName) && transform.gameObject.activeInHierarchy && transform.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform;
				}
			}
			SkinnedMeshRenderer[] componentsInChildren = this.animator.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			int i = 0;
			while (i < componentsInChildren.Length)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
				if (skinnedMeshRenderer.gameObject.activeInHierarchy && ExposeLingerieController.NameMatches(skinnedMeshRenderer.name, text))
				{
					if (!(skinnedMeshRenderer.transform.parent != null))
					{
						return skinnedMeshRenderer.transform;
					}
					return skinnedMeshRenderer.transform.parent;
				}
				else
				{
					i++;
				}
			}
			foreach (Transform transform2 in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (ExposeLingerieController.NameMatches(transform2.name, rootName) && transform2.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x06008F21 RID: 36641 RVA: 0x003A8ACB File Offset: 0x003A6CCB
		private static bool NameMatches(string n, string expected)
		{
			return !string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(expected) && (n == expected || n.StartsWith(expected));
		}

		// Token: 0x0400928F RID: 37519
		public const string SharedLayerName = "ClothFlashArms";

		// Token: 0x04009290 RID: 37520
		public const string SharedOpenTrigger = "ClothFlashStart";

		// Token: 0x04009291 RID: 37521
		public const string SharedCloseTrigger = "ClothFlashEnd";

		// Token: 0x04009292 RID: 37522
		public const string SharedVariantParam = "ClothFlashVariant";

		// Token: 0x04009293 RID: 37523
		public const string BoobsTypeParam = "BoobsType";

		// Token: 0x04009294 RID: 37524
		public const string GestureTag = "ExposeLingerie";

		// Token: 0x04009295 RID: 37525
		private const string EmptyStateName = "Empty";

		// Token: 0x04009296 RID: 37526
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04009297 RID: 37527
		[Header("Variants on the shared ClothFlashArms layer (Swimsuit=1, BikiniBra=2, BikiniPanties=3)")]
		[SerializeField]
		private int braVariant = 4;

		// Token: 0x04009298 RID: 37528
		[SerializeField]
		private int pantiesVariant = 5;

		// Token: 0x04009299 RID: 37529
		[Header("Worn garments (detail animator hosts)")]
		[Tooltip("Root name of the dressed bra prefab instance (matched by exact name or StartsWith). Its child SMR is named 'Bra', which is the path the clips bind blendshapes to.")]
		[SerializeField]
		private string braRootName = "ExposeBra";

		// Token: 0x0400929A RID: 37530
		[Tooltip("Root name of the dressed panties prefab instance (child SMR 'Panty').")]
		[SerializeField]
		private string pantiesRootName = "ExposePanties";

		// Token: 0x0400929B RID: 37531
		[Tooltip("Garment-only detail controller (ExposeLingerieDetail): Empty + Bra_B0..5 / Panties open states and their _Close (speed -1) twins. Added to the garment root on the fly.")]
		[SerializeField]
		private RuntimeAnimatorController detailController;

		// Token: 0x0400929C RID: 37532
		[Header("Gesture")]
		[Tooltip("Layer weight fade-in: smooth blend of the arms into the gesture (bikini-style).")]
		[SerializeField]
		private float fadeInDuration = 0.15f;

		// Token: 0x0400929D RID: 37533
		[SerializeField]
		private float fadeOutDuration = 0.25f;

		// Token: 0x0400929E RID: 37534
		[Tooltip("Safety: max seconds before a gesture is force-ended (anti-deadlock).")]
		[SerializeField]
		private float maxGestureSeconds = 8f;

		// Token: 0x0400929F RID: 37535
		[Tooltip("Lock player input while the gesture plays. Arms-only gesture: leave OFF so she can walk/run.")]
		[SerializeField]
		private bool lockInputDuringGesture;

		// Token: 0x040092A0 RID: 37536
		[Tooltip("Suppress the Confident/Embarassed emotion layers while the gesture plays. Arms-only: leave OFF.")]
		[SerializeField]
		private bool suppressEmotionLayers;

		// Token: 0x040092A1 RID: 37537
		[SerializeField]
		private float toggleCooldown = 0.6f;

		// Token: 0x040092A2 RID: 37538
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040092A3 RID: 37539
		[Header("Events")]
		[Tooltip("Raised when an expose/cover gesture starts (face switches to HornySupreme here).")]
		public UnityEvent OnGestureStarted;

		// Token: 0x040092A4 RID: 37540
		[Tooltip("Raised when the gesture has finished playing.")]
		public UnityEvent OnGestureEnded;

		// Token: 0x040092A5 RID: 37541
		private int _openTriggerHash;

		// Token: 0x040092A6 RID: 37542
		private int _closeTriggerHash;

		// Token: 0x040092A7 RID: 37543
		private int _variantHash;

		// Token: 0x040092A8 RID: 37544
		private int _boobsTypeHash;

		// Token: 0x040092A9 RID: 37545
		private int _layerIndex = -1;

		// Token: 0x040092AA RID: 37546
		private bool _isPlaying;

		// Token: 0x040092AB RID: 37547
		private bool _braOpen;

		// Token: 0x040092AC RID: 37548
		private bool _pantiesOpen;

		// Token: 0x040092AD RID: 37549
		private float _cooldownUntil;

		// Token: 0x040092AE RID: 37550
		private Coroutine _routine;

		// Token: 0x040092AF RID: 37551
		private Animator _detailAnimator;

		// Token: 0x040092B0 RID: 37552
		private string _detailStateName;

		// Token: 0x040092B1 RID: 37553
		private vThirdPersonInput _playerInput;

		// Token: 0x040092B2 RID: 37554
		private bool _lockedInput;

		// Token: 0x040092B3 RID: 37555
		private bool _suppressedLayers;

		// Token: 0x040092B4 RID: 37556
		private PlayerActivityLease _gestureLease;
	}
}
