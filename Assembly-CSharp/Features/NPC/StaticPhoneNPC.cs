using System;
using Features.Detection;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using ProjectExhibitionist.NPCs;
using UnityEngine;
using UnityEngine.AI;

namespace Features.NPC
{
	// Token: 0x02000F04 RID: 3844
	[DisallowMultipleComponent]
	public sealed class StaticPhoneNPC : MonoBehaviour
	{
		// Token: 0x0600734E RID: 29518 RVA: 0x002F07A4 File Offset: 0x002EE9A4
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.StaticPhoneNPC_encounterController)
			{
				return this.encounterController;
			}
			throw new InvalidOperationException("Unsupported StaticPhoneNPC POI binding: " + field.ToString());
		}

		// Token: 0x0600734F RID: 29519 RVA: 0x002F07CE File Offset: 0x002EE9CE
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.StaticPhoneNPC_encounterController)
			{
				this.encounterController = (RuntimeAnimatorController)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported StaticPhoneNPC POI binding: " + field.ToString());
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06007350 RID: 29520 RVA: 0x002F07FE File Offset: 0x002EE9FE
		public float AttentionMultiplier
		{
			get
			{
				if (!base.isActiveAndEnabled)
				{
					return 1f;
				}
				return this.attentionMultiplier;
			}
		}

		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06007351 RID: 29521 RVA: 0x002F0814 File Offset: 0x002EEA14
		public bool IsEncounterPrepared
		{
			get
			{
				return this._encounterPrepared;
			}
		}

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06007352 RID: 29522 RVA: 0x002F081C File Offset: 0x002EEA1C
		public bool IsPhotoReactionActive
		{
			get
			{
				return this._photoActive;
			}
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06007353 RID: 29523 RVA: 0x002F0824 File Offset: 0x002EEA24
		public RuntimeAnimatorController EncounterController
		{
			get
			{
				return this.encounterController;
			}
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06007354 RID: 29524 RVA: 0x002F082C File Offset: 0x002EEA2C
		public Animator PoseAnimator
		{
			get
			{
				return this.animator;
			}
		}

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06007355 RID: 29525 RVA: 0x002F0834 File Offset: 0x002EEA34
		public bool OwnsPresentation
		{
			get
			{
				return this._ownsPresentation;
			}
		}

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06007356 RID: 29526 RVA: 0x002F083C File Offset: 0x002EEA3C
		// (remove) Token: 0x06007357 RID: 29527 RVA: 0x002F0874 File Offset: 0x002EEA74
		public event Action<StaticPhoneNPC, bool> PresentationOwnershipChanged;

		// Token: 0x06007358 RID: 29528 RVA: 0x002F08AC File Offset: 0x002EEAAC
		public void Configure(Animator poseAnimator, GameObject phone, RuntimeAnimatorController normalController, Transform reactionHand, Vector3 gripPosition, Quaternion gripRotation, Vector3 gripScale)
		{
			this.animator = poseAnimator;
			this.smartphone = phone;
			this.encounterController = normalController;
			this.photoHand = reactionHand;
			this.photoPhonePosition = gripPosition;
			this.photoPhoneRotation = gripRotation;
			this.photoPhoneScale = gripScale;
			this.phoneStartsActive = phone != null && phone.activeSelf;
		}

		// Token: 0x06007359 RID: 29529 RVA: 0x002F0908 File Offset: 0x002EEB08
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.Initialize();
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
			this.DisableMovement();
			if (this.smartphone != null)
			{
				this.smartphone.SetActive(this.phoneStartsActive);
			}
			if (this._senses != null)
			{
				this._senses.enabled = true;
			}
			AttentionUIController component = base.GetComponent<AttentionUIController>();
			if (component != null)
			{
				component.TargetNPC = this._senses;
				component.enabled = true;
				if (component.CanvasRoot != null)
				{
					component.CanvasRoot.enabled = true;
				}
			}
			NPCSimulationPause.RegisterParticipant(this);
		}

		// Token: 0x0600735A RID: 29530 RVA: 0x002F09B8 File Offset: 0x002EEBB8
		private void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this._senses = base.GetComponent<NPCSenses>();
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>(true);
			}
			if (this.animator == null)
			{
				return;
			}
			this._phoneController = this.animator.runtimeAnimatorController;
			this._originalRootMotion = this.animator.applyRootMotion;
			this._authoredPosition = base.transform.position;
			this._authoredRotation = base.transform.rotation;
			this._authoredParent = base.transform.parent;
			this._authoredLocalPosition = base.transform.localPosition;
			this._authoredLocalRotation = base.transform.localRotation;
			if (this.smartphone != null)
			{
				Transform transform = this.smartphone.transform;
				this._phoneParent = transform.parent;
				this._phonePosition = transform.localPosition;
				this._phoneRotation = transform.localRotation;
				this._phoneScale = transform.localScale;
				this._originalPhoneActive = this.phoneStartsActive;
			}
			this._initialized = true;
		}

		// Token: 0x0600735B RID: 29531 RVA: 0x002F0AD8 File Offset: 0x002EECD8
		private void Update()
		{
			if (NPCSimulationPause.IsPaused || this._senses == null || this._encounterPrepared)
			{
				return;
			}
			if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				return;
			}
			this.UpdateAttentionReaction(this._senses.CurrentAttention);
		}

		// Token: 0x0600735C RID: 29532 RVA: 0x002F0B2E File Offset: 0x002EED2E
		private void Start()
		{
			if (!this._ownsPresentation && this.smartphone != null)
			{
				this.smartphone.SetActive(this.phoneStartsActive);
			}
		}

		// Token: 0x0600735D RID: 29533 RVA: 0x002F0B58 File Offset: 0x002EED58
		public void UpdateAttentionReaction(float attention)
		{
			if (this._encounterPrepared)
			{
				return;
			}
			this.Initialize();
			bool flag = attention > 0.5f && (this._senses == null || (!this._senses.IsInCooldown && !this._senses.IsGhostFleeing));
			if (flag == this._photoActive)
			{
				return;
			}
			if (!flag)
			{
				this.RestorePhonePose();
				return;
			}
			if (!this.UseEncounterController())
			{
				return;
			}
			this._photoActive = true;
			this.animator.SetInteger("ReactionType", 1);
			this.animator.SetTrigger("ReactionTrigger");
		}

		// Token: 0x0600735E RID: 29534 RVA: 0x002F0BF4 File Offset: 0x002EEDF4
		public void PrepareForEncounter()
		{
			this.Initialize();
			if (this._encounterPrepared)
			{
				return;
			}
			this.UseEncounterController();
			this._photoActive = false;
			this._encounterPrepared = true;
		}

		// Token: 0x0600735F RID: 29535 RVA: 0x002F0C1A File Offset: 0x002EEE1A
		public void RestoreAfterEncounter()
		{
			this._encounterPrepared = false;
			this.DisableMovement();
			this.RestorePhonePose();
		}

		// Token: 0x06007360 RID: 29536 RVA: 0x002F0C30 File Offset: 0x002EEE30
		private bool UseEncounterController()
		{
			if (!this._initialized || this.animator == null || this.encounterController == null)
			{
				return false;
			}
			this.DisableMovement();
			bool flag = !this._ownsPresentation;
			this.SetPresentationOwnership(true);
			if (this.animator.runtimeAnimatorController != this.encounterController)
			{
				if (this.animator.isInitialized && this.animator.layerCount > 0)
				{
					AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
					this._phoneState = currentAnimatorStateInfo.fullPathHash;
					this._phoneTime = currentAnimatorStateInfo.normalizedTime;
				}
				this.animator.runtimeAnimatorController = this.encounterController;
			}
			else if (flag)
			{
				this.animator.Rebind();
			}
			this.animator.applyRootMotion = false;
			int layerIndex = this.animator.GetLayerIndex("AlertBehavior");
			if (layerIndex >= 0)
			{
				this.animator.SetLayerWeight(layerIndex, 0f);
			}
			int layerIndex2 = this.animator.GetLayerIndex("Reactions");
			if (layerIndex2 >= 0)
			{
				this.animator.SetLayerWeight(layerIndex2, 1f);
			}
			if (this.smartphone != null)
			{
				Transform transform = this.smartphone.transform;
				if (this.photoHand != null)
				{
					transform.SetParent(this.photoHand, false);
				}
				transform.localPosition = this.photoPhonePosition;
				transform.localRotation = this.photoPhoneRotation;
				transform.localScale = this.photoPhoneScale;
				this.smartphone.SetActive(true);
			}
			return true;
		}

		// Token: 0x06007361 RID: 29537 RVA: 0x002F0DB8 File Offset: 0x002EEFB8
		private void RestorePhonePose()
		{
			bool flag = this._photoActive || this._encounterPrepared || (this.animator != null && this.animator.runtimeAnimatorController == this.encounterController);
			this._photoActive = false;
			if (!this._initialized)
			{
				return;
			}
			if (this.animator != null && flag)
			{
				this.animator.runtimeAnimatorController = this._phoneController;
				this.animator.applyRootMotion = this._originalRootMotion;
				if (this.animator.isActiveAndEnabled && this._phoneController != null)
				{
					if (this._phoneState != 0)
					{
						this.animator.Play(this._phoneState, 0, this._phoneTime);
					}
					this.animator.Update(0f);
				}
			}
			if (this._authoredParent != null && base.transform.parent == this._authoredParent)
			{
				base.transform.localPosition = this._authoredLocalPosition;
				base.transform.localRotation = this._authoredLocalRotation;
			}
			else
			{
				base.transform.position = this._authoredPosition;
				base.transform.rotation = this._authoredRotation;
			}
			if (this.smartphone != null)
			{
				foreach (SmartphoneFlash smartphoneFlash in this.smartphone.GetComponentsInChildren<SmartphoneFlash>(true))
				{
					smartphoneFlash.StopFlash();
					smartphoneFlash.StopVideoLight();
				}
				Transform transform = this.smartphone.transform;
				transform.SetParent(this._phoneParent, false);
				transform.localPosition = this._phonePosition;
				transform.localRotation = this._phoneRotation;
				transform.localScale = this._phoneScale;
				this.smartphone.SetActive(this._originalPhoneActive);
			}
			this.SetPresentationOwnership(false);
		}

		// Token: 0x06007362 RID: 29538 RVA: 0x002F0F84 File Offset: 0x002EF184
		private void DisableMovement()
		{
			foreach (Behaviour behaviour in base.GetComponents<Behaviour>())
			{
				if (behaviour is AStarNPCController || behaviour is NPCWanderController || behaviour is NPCStateMachine || behaviour is NavMeshAgent || behaviour.GetType().Name == "FollowerEntity")
				{
					behaviour.enabled = false;
				}
			}
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x002F0FE8 File Offset: 0x002EF1E8
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			if (!this._initialized)
			{
				return;
			}
			this.RestoreAfterEncounter();
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x002F100A File Offset: 0x002EF20A
		private void OnWorldOriginShifted(Vector3 delta)
		{
			this._authoredPosition += delta;
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x002F101E File Offset: 0x002EF21E
		private void SetPresentationOwnership(bool owned)
		{
			if (this._ownsPresentation == owned)
			{
				return;
			}
			this._ownsPresentation = owned;
			Action<StaticPhoneNPC, bool> presentationOwnershipChanged = this.PresentationOwnershipChanged;
			if (presentationOwnershipChanged == null)
			{
				return;
			}
			presentationOwnershipChanged(this, owned);
		}

		// Token: 0x0400768A RID: 30346
		public const float PhotoAttentionThreshold = 0.5f;

		// Token: 0x0400768B RID: 30347
		[SerializeField]
		private Animator animator;

		// Token: 0x0400768C RID: 30348
		[SerializeField]
		private GameObject smartphone;

		// Token: 0x0400768D RID: 30349
		[SerializeField]
		private RuntimeAnimatorController encounterController;

		// Token: 0x0400768E RID: 30350
		[SerializeField]
		[Range(0.1f, 1f)]
		private float attentionMultiplier = 0.8f;

		// Token: 0x0400768F RID: 30351
		[SerializeField]
		private Transform photoHand;

		// Token: 0x04007690 RID: 30352
		[SerializeField]
		private Vector3 photoPhonePosition;

		// Token: 0x04007691 RID: 30353
		[SerializeField]
		private Quaternion photoPhoneRotation = Quaternion.identity;

		// Token: 0x04007692 RID: 30354
		[SerializeField]
		private Vector3 photoPhoneScale = Vector3.one;

		// Token: 0x04007693 RID: 30355
		[SerializeField]
		private bool phoneStartsActive = true;

		// Token: 0x04007694 RID: 30356
		private NPCSenses _senses;

		// Token: 0x04007695 RID: 30357
		private RuntimeAnimatorController _phoneController;

		// Token: 0x04007696 RID: 30358
		private Transform _phoneParent;

		// Token: 0x04007697 RID: 30359
		private Vector3 _phonePosition;

		// Token: 0x04007698 RID: 30360
		private Quaternion _phoneRotation;

		// Token: 0x04007699 RID: 30361
		private Vector3 _phoneScale;

		// Token: 0x0400769A RID: 30362
		private Vector3 _authoredPosition;

		// Token: 0x0400769B RID: 30363
		private Quaternion _authoredRotation;

		// Token: 0x0400769C RID: 30364
		private Transform _authoredParent;

		// Token: 0x0400769D RID: 30365
		private Vector3 _authoredLocalPosition;

		// Token: 0x0400769E RID: 30366
		private Quaternion _authoredLocalRotation;

		// Token: 0x0400769F RID: 30367
		private bool _originalPhoneActive;

		// Token: 0x040076A0 RID: 30368
		private bool _originalRootMotion;

		// Token: 0x040076A1 RID: 30369
		private int _phoneState;

		// Token: 0x040076A2 RID: 30370
		private float _phoneTime;

		// Token: 0x040076A3 RID: 30371
		private bool _initialized;

		// Token: 0x040076A4 RID: 30372
		private bool _photoActive;

		// Token: 0x040076A5 RID: 30373
		private bool _encounterPrepared;

		// Token: 0x040076A6 RID: 30374
		private bool _ownsPresentation;
	}
}
