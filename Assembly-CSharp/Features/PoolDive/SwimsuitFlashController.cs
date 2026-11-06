using System;
using Features.DebugTools;
using Features.Flash;
using Features.HideFlash;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Features.PoolDive
{
	// Token: 0x02000DDF RID: 3551
	public class SwimsuitFlashController : MonoBehaviour
	{
		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x0600692B RID: 26923 RVA: 0x002A164B File Offset: 0x0029F84B
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x0600692C RID: 26924 RVA: 0x002A1653 File Offset: 0x0029F853
		public bool IsBusy
		{
			get
			{
				return !(this.animator == null) && this._flashLayerIndex >= 0 && this.animator.GetLayerWeight(this._flashLayerIndex) > 0.001f;
			}
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x0600692D RID: 26925 RVA: 0x002A1686 File Offset: 0x0029F886
		public bool IsOnCooldown
		{
			get
			{
				return Time.time < this._cooldownUntil;
			}
		}

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x0600692E RID: 26926 RVA: 0x002A1698 File Offset: 0x0029F898
		public bool IsSwimsuitWorn
		{
			get
			{
				if (this.animator == null)
				{
					return false;
				}
				Transform transform = this.FindWornSwimsuitContainer();
				if (transform != null && transform.gameObject.activeInHierarchy)
				{
					if (this._swimsuitRemoved)
					{
						this._swimsuitRemoved = false;
						this.swimsuitAnimator = null;
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x0600692F RID: 26927 RVA: 0x002A16F0 File Offset: 0x0029F8F0
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			this._startTriggerHash = Animator.StringToHash(this.startTrigger);
			this._endTriggerHash = Animator.StringToHash(this.endTrigger);
			this._emptyStateHash = Animator.StringToHash(this.emptyStateName);
			if (this.animator != null)
			{
				this._flashLayerIndex = this.animator.GetLayerIndex(this.flashLayerName);
				if (this._flashLayerIndex >= 0)
				{
					this.animator.SetLayerWeight(this._flashLayerIndex, 0f);
					return;
				}
				Debug.LogWarning("[SwimsuitFlashController] Layer '" + this.flashLayerName + "' not found. Run Tools/SwimsuitDive/Setup.", this);
			}
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x002A17AC File Offset: 0x0029F9AC
		private void ResolveSwimsuitAnimator()
		{
			if (this.animator == null)
			{
				return;
			}
			if (this.swimsuitAnimator != null && this.swimsuitAnimator.runtimeAnimatorController != null)
			{
				return;
			}
			if (this.swimsuitAnimator == null)
			{
				foreach (Animator animator in this.animator.GetComponentsInChildren<Animator>(true))
				{
					if (!(animator == this.animator) && this.NameMatchesSwimsuit(animator.gameObject.name) && !(animator.GetComponent<SkinnedMeshRenderer>() != null))
					{
						this.swimsuitAnimator = animator;
						break;
					}
				}
			}
			if (this.swimsuitAnimator == null)
			{
				Transform transform = this.FindWornSwimsuitContainer();
				if (transform != null)
				{
					Transform transform2 = ((transform.parent != null) ? transform.parent : transform);
					this.swimsuitAnimator = transform2.GetComponent<Animator>();
					if (this.swimsuitAnimator == null)
					{
						this.swimsuitAnimator = transform2.gameObject.AddComponent<Animator>();
					}
				}
			}
			if (this.swimsuitAnimator == null)
			{
				Debug.LogWarning("[SwimsuitFlashController] No worn swimsuit '" + this.swimsuitObjectName + "' found. The swimsuit will not deform during the flash.", this);
				return;
			}
			if (this.swimsuitAnimator.runtimeAnimatorController == null)
			{
				if (this.swimsuitOnlyController == null)
				{
					Debug.LogWarning("[SwimsuitFlashController] Swimsuit Animator on '" + this.swimsuitAnimator.gameObject.name + "' has no controller and 'swimsuitOnlyController' is unassigned.", this);
					return;
				}
				this.swimsuitAnimator.runtimeAnimatorController = this.swimsuitOnlyController;
			}
			this.swimsuitAnimator.applyRootMotion = false;
			this.swimsuitAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x002A1950 File Offset: 0x0029FB50
		private Transform FindWornSwimsuitContainer()
		{
			if (this.animator == null)
			{
				return null;
			}
			foreach (Transform transform in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatchesSwimsuit(transform.name) && transform.gameObject.activeInHierarchy && transform.GetComponent<SkinnedMeshRenderer>() != null)
				{
					return transform;
				}
			}
			foreach (Transform transform2 in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatchesSwimsuit(transform2.name) && transform2.gameObject.activeInHierarchy && transform2.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform2;
				}
			}
			foreach (Transform transform3 in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatchesSwimsuit(transform3.name) && transform3.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform3;
				}
			}
			return null;
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x002A1A45 File Offset: 0x0029FC45
		private bool NameMatchesSwimsuit(string n)
		{
			return !string.IsNullOrEmpty(n) && (n == this.swimsuitObjectName || n.StartsWith(this.swimsuitObjectName));
		}

		// Token: 0x06006933 RID: 26931 RVA: 0x002A1A6D File Offset: 0x0029FC6D
		private void Update()
		{
			if (!FlyCamera.IsActive && this.toggleInput != null && this.toggleInput.GetButtonDown() && !this.IsOnCooldown)
			{
				this.Toggle();
			}
			this.UpdateLayerWeight();
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x002A1AA0 File Offset: 0x0029FCA0
		public void Toggle()
		{
			if (this._swimsuitRemoved || this.animator == null || this._flashLayerIndex < 0)
			{
				return;
			}
			if (HideFlashController.HasHeldCoverItem)
			{
				return;
			}
			if (this.IsOnCooldown)
			{
				return;
			}
			if (!this._isOpen && PlayerActivityGate.IsBlocked(PlayerActivity.Flash))
			{
				return;
			}
			if (!this._isOpen && !this.IsSwimsuitWorn)
			{
				return;
			}
			this._isOpen = !this._isOpen;
			this._cooldownUntil = Time.time + Mathf.Max(0f, this.toggleCooldown);
			this.FireTriggers(this._isOpen);
			if (this.animator != null && this._flashLayerIndex >= 0)
			{
				this.animator.SetLayerWeight(this._flashLayerIndex, 1f);
				this._targetWeight = 1f;
				this._fadeSpeed = ((this.fadeInDuration > 0f) ? (1f / this.fadeInDuration) : float.PositiveInfinity);
				this._waitingForActivation = true;
			}
			UnityEvent<bool> onFlashStateChanged = this.OnFlashStateChanged;
			if (onFlashStateChanged == null)
			{
				return;
			}
			onFlashStateChanged.Invoke(this._isOpen);
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x002A1BB2 File Offset: 0x0029FDB2
		public void SetSwimsuitRemoved()
		{
			this._swimsuitRemoved = true;
			if (this._isOpen)
			{
				this._isOpen = false;
				this.FireTriggers(false);
			}
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x002A1BD1 File Offset: 0x0029FDD1
		public void SetSwimsuitRestored()
		{
			this._swimsuitRemoved = false;
			this.swimsuitAnimator = null;
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x002A1BE4 File Offset: 0x0029FDE4
		public void InterruptForEncounter()
		{
			if (!this._isOpen && !this.IsBusy && !this._waitingForActivation)
			{
				return;
			}
			bool isOpen = this._isOpen;
			this._isOpen = false;
			this._waitingForActivation = false;
			this._targetWeight = 0f;
			this._cooldownUntil = 0f;
			if (this.animator != null && this._flashLayerIndex >= 0)
			{
				this.animator.ResetTrigger(this._startTriggerHash);
				this.animator.ResetTrigger(this._endTriggerHash);
				this.animator.Play(this._emptyStateHash, this._flashLayerIndex, 0f);
				this.animator.SetLayerWeight(this._flashLayerIndex, 0f);
			}
			if (this.swimsuitAnimator != null && this.swimsuitAnimator.runtimeAnimatorController != null)
			{
				this.swimsuitAnimator.ResetTrigger(this._startTriggerHash);
				this.swimsuitAnimator.ResetTrigger(this._endTriggerHash);
				FlashDetailPose.Complete(this.swimsuitAnimator, "PullUp", 0);
			}
			if (isOpen)
			{
				UnityEvent<bool> onFlashStateChanged = this.OnFlashStateChanged;
				if (onFlashStateChanged == null)
				{
					return;
				}
				onFlashStateChanged.Invoke(false);
			}
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x002A1D08 File Offset: 0x0029FF08
		private void FireTriggers(bool open)
		{
			int num = (open ? this._startTriggerHash : this._endTriggerHash);
			if (this.swimsuitAnimator == null || this.swimsuitAnimator.runtimeAnimatorController == null)
			{
				this.ResolveSwimsuitAnimator();
			}
			this.animator.ResetTrigger(this._startTriggerHash);
			this.animator.ResetTrigger(this._endTriggerHash);
			this.animator.SetTrigger(num);
			if (this.swimsuitAnimator != null && this.swimsuitAnimator.isActiveAndEnabled)
			{
				this.swimsuitAnimator.ResetTrigger(this._startTriggerHash);
				this.swimsuitAnimator.ResetTrigger(this._endTriggerHash);
				this.swimsuitAnimator.SetTrigger(num);
			}
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x002A1DC8 File Offset: 0x0029FFC8
		private void UpdateLayerWeight()
		{
			if (this.animator == null || this._flashLayerIndex < 0)
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
				this._fadeSpeed = ((this.fadeOutDuration > 0f) ? (1f / this.fadeOutDuration) : float.PositiveInfinity);
			}
			float layerWeight = this.animator.GetLayerWeight(this._flashLayerIndex);
			if (!Mathf.Approximately(layerWeight, this._targetWeight))
			{
				float num = Mathf.MoveTowards(layerWeight, this._targetWeight, this._fadeSpeed * Time.deltaTime);
				this.animator.SetLayerWeight(this._flashLayerIndex, num);
			}
		}

		// Token: 0x04006BD0 RID: 27600
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04006BD1 RID: 27601
		[Tooltip("Animator on the worn swimsuit playing the swimsuit-only clips. Auto-found under the player Animator if not assigned.")]
		[SerializeField]
		private Animator swimsuitAnimator;

		// Token: 0x04006BD2 RID: 27602
		[Tooltip("Mesh root name of the worn swimsuit (the SMR-owning object the blendshape clips bind to).")]
		[SerializeField]
		private string swimsuitObjectName = "School_Swimsuit";

		// Token: 0x04006BD3 RID: 27603
		[Tooltip("Swimsuit-only controller (SwimsuitFlash_SwimsuitOnly). The swimsuit is dressed at runtime, so this is added to its Animator on the fly if needed.")]
		[SerializeField]
		private RuntimeAnimatorController swimsuitOnlyController;

		// Token: 0x04006BD4 RID: 27604
		[Header("Animator")]
		[SerializeField]
		private string flashLayerName = "SwimsuitFlash";

		// Token: 0x04006BD5 RID: 27605
		[SerializeField]
		private string startTrigger = "SwimsuitFlashStart";

		// Token: 0x04006BD6 RID: 27606
		[SerializeField]
		private string endTrigger = "SwimsuitFlashEnd";

		// Token: 0x04006BD7 RID: 27607
		[SerializeField]
		private string emptyStateName = "Empty";

		// Token: 0x04006BD8 RID: 27608
		[Header("Layer Weight")]
		[SerializeField]
		private float fadeInDuration = 0.15f;

		// Token: 0x04006BD9 RID: 27609
		[SerializeField]
		private float fadeOutDuration = 0.25f;

		// Token: 0x04006BDA RID: 27610
		[Header("Input")]
		[SerializeField]
		private GenericInput toggleInput = new GenericInput("B", "Y", "B");

		// Token: 0x04006BDB RID: 27611
		[Tooltip("Seconds to lock further toggles after triggering an action.")]
		[SerializeField]
		private float toggleCooldown = 0.6f;

		// Token: 0x04006BDC RID: 27612
		[Header("Events")]
		public UnityEvent<bool> OnFlashStateChanged;

		// Token: 0x04006BDD RID: 27613
		private int _startTriggerHash;

		// Token: 0x04006BDE RID: 27614
		private int _endTriggerHash;

		// Token: 0x04006BDF RID: 27615
		private int _emptyStateHash;

		// Token: 0x04006BE0 RID: 27616
		private int _flashLayerIndex = -1;

		// Token: 0x04006BE1 RID: 27617
		private float _targetWeight;

		// Token: 0x04006BE2 RID: 27618
		private float _fadeSpeed;

		// Token: 0x04006BE3 RID: 27619
		private bool _waitingForActivation;

		// Token: 0x04006BE4 RID: 27620
		private bool _isOpen;

		// Token: 0x04006BE5 RID: 27621
		private bool _swimsuitRemoved;

		// Token: 0x04006BE6 RID: 27622
		private float _cooldownUntil;
	}
}
