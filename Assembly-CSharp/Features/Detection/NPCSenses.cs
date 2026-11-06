using System;
using System.Collections;
using Features.NPC;
using Features.NPC.StateMachine;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012C9 RID: 4809
	public class NPCSenses : MonoBehaviour
	{
		// Token: 0x060094DE RID: 38110 RVA: 0x003D2495 File Offset: 0x003D0695
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCSenses_ReactionProfile)
			{
				return this.ReactionProfile;
			}
			throw new InvalidOperationException("Unsupported NPCSenses POI binding: " + field.ToString());
		}

		// Token: 0x060094DF RID: 38111 RVA: 0x003D24BF File Offset: 0x003D06BF
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCSenses_ReactionProfile)
			{
				this.ReactionProfile = (NPCReactionProfileSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported NPCSenses POI binding: " + field.ToString());
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x060094E0 RID: 38112 RVA: 0x003D24EF File Offset: 0x003D06EF
		// (set) Token: 0x060094E1 RID: 38113 RVA: 0x003D24F7 File Offset: 0x003D06F7
		public bool HasLineOfSight
		{
			get
			{
				return this._hasLineOfSight;
			}
			set
			{
				if (value && !this._hasLineOfSight)
				{
					Action<NPCSenses> onPlayerSpotted = NPCSenses.OnPlayerSpotted;
					if (onPlayerSpotted != null)
					{
						onPlayerSpotted(this);
					}
				}
				this._hasLineOfSight = value;
			}
		}

		// Token: 0x14000107 RID: 263
		// (add) Token: 0x060094E2 RID: 38114 RVA: 0x003D251C File Offset: 0x003D071C
		// (remove) Token: 0x060094E3 RID: 38115 RVA: 0x003D2550 File Offset: 0x003D0750
		public static event Action<NPCSenses> OnPlayerSpotted;

		// Token: 0x14000108 RID: 264
		// (add) Token: 0x060094E4 RID: 38116 RVA: 0x003D2584 File Offset: 0x003D0784
		// (remove) Token: 0x060094E5 RID: 38117 RVA: 0x003D25B8 File Offset: 0x003D07B8
		public static event Action<NPCSenses, float, float> OnAttentionChanged;

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x060094E6 RID: 38118 RVA: 0x003D25EB File Offset: 0x003D07EB
		// (set) Token: 0x060094E7 RID: 38119 RVA: 0x003D25F3 File Offset: 0x003D07F3
		public bool IsGhostFleeing { get; internal set; }

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x060094E8 RID: 38120 RVA: 0x003D25FC File Offset: 0x003D07FC
		public bool IsSearching
		{
			get
			{
				return this._fsm != null && this._fsm.IsInitialized && this._fsm.CurrentStateType == NPCStateType.Searching;
			}
		}

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x060094E9 RID: 38121 RVA: 0x003D262A File Offset: 0x003D082A
		public float EffectiveViewRadius
		{
			get
			{
				return this.BaseViewRadius * this._actionVisionMult;
			}
		}

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x060094EA RID: 38122 RVA: 0x003D2639 File Offset: 0x003D0839
		public float EffectiveViewAngle
		{
			get
			{
				return this.BaseViewAngle * this._actionAngleMult;
			}
		}

		// Token: 0x060094EB RID: 38123 RVA: 0x003D2648 File Offset: 0x003D0848
		public float GetViewAngleForTarget(Vector3 targetPosition)
		{
			if ((targetPosition - this.EyePosition).sqrMagnitude > 4f || !this.IsSeatedObserver)
			{
				return this.EffectiveViewAngle;
			}
			return Mathf.Max(this.BaseViewAngle, 200f) * this._actionAngleMult;
		}

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x060094EC RID: 38124 RVA: 0x003D2696 File Offset: 0x003D0896
		public StaticPhoneNPC StaticPhone
		{
			get
			{
				if (this._staticPhone == null)
				{
					base.TryGetComponent<StaticPhoneNPC>(out this._staticPhone);
				}
				return this._staticPhone;
			}
		}

		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x060094ED RID: 38125 RVA: 0x003D26B9 File Offset: 0x003D08B9
		public bool UsesStaticPhoneReaction
		{
			get
			{
				return this.StaticPhone != null && this.StaticPhone.isActiveAndEnabled;
			}
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x060094EE RID: 38126 RVA: 0x003D26D6 File Offset: 0x003D08D6
		public float StaticPhoneAttentionMultiplier
		{
			get
			{
				if (!this.UsesStaticPhoneReaction)
				{
					return 1f;
				}
				return this.StaticPhone.AttentionMultiplier;
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x060094EF RID: 38127 RVA: 0x003D26F1 File Offset: 0x003D08F1
		public float AttentionMultiplier
		{
			get
			{
				return this._actionAttentionLevel * this.StaticPhoneAttentionMultiplier;
			}
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x060094F0 RID: 38128 RVA: 0x003D2700 File Offset: 0x003D0900
		public bool IsSeatedObserver
		{
			get
			{
				return NPCSeatedEncounter.IsSeated(this);
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x060094F1 RID: 38129 RVA: 0x003D2708 File Offset: 0x003D0908
		public bool BlocksFinalDetectionWhileSeated
		{
			get
			{
				return NPCSeatedEncounter.IsTrainPassenger(this) && this.IsSeatedObserver;
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x060094F2 RID: 38130 RVA: 0x003D271C File Offset: 0x003D091C
		public Vector3 EyePosition
		{
			get
			{
				if (this.EyeTransform != null && !this.IsRigRootReference(this.EyeTransform))
				{
					return this.EyeTransform.position;
				}
				Transform transform = this.ResolveHumanoidHead();
				if (transform != null)
				{
					return transform.position;
				}
				return base.transform.position + Vector3.up * (this.IsSeatedObserver ? 1.1f : 1.6f);
			}
		}

		// Token: 0x060094F3 RID: 38131 RVA: 0x003D2798 File Offset: 0x003D0998
		private bool IsRigRootReference(Transform reference)
		{
			if (reference == base.transform)
			{
				return true;
			}
			if (!reference.IsChildOf(base.transform))
			{
				return false;
			}
			string name = reference.name;
			return string.Equals(name, "root", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "armature", StringComparison.OrdinalIgnoreCase) || string.Equals(name, "BodyRig", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060094F4 RID: 38132 RVA: 0x003D27F8 File Offset: 0x003D09F8
		private Transform ResolveHumanoidHead()
		{
			if (this._humanoidHead != null && this._eyeAnimator != null && this._eyeAnimator.avatar == this._eyeAvatar && this._humanoidHead.IsChildOf(base.transform) && (this._humanoidHead.gameObject.activeInHierarchy || !base.gameObject.activeInHierarchy))
			{
				return this._humanoidHead;
			}
			if (Time.unscaledTime < this._nextHeadLookupTime)
			{
				return null;
			}
			this._nextHeadLookupTime = Time.unscaledTime + 0.5f;
			this._humanoidHead = null;
			this._eyeAnimator = base.GetComponent<Animator>();
			if (!this.IsUsableHumanoid(this._eyeAnimator))
			{
				this._eyeAnimator = null;
				foreach (Animator animator in base.GetComponentsInChildren<Animator>(true))
				{
					if (this.IsUsableHumanoid(animator))
					{
						this._eyeAnimator = animator;
						break;
					}
				}
			}
			if (this._eyeAnimator == null)
			{
				return null;
			}
			this._eyeAvatar = this._eyeAnimator.avatar;
			this._humanoidHead = this._eyeAnimator.GetBoneTransform(HumanBodyBones.Head);
			return this._humanoidHead;
		}

		// Token: 0x060094F5 RID: 38133 RVA: 0x003D2920 File Offset: 0x003D0B20
		private bool IsUsableHumanoid(Animator animator)
		{
			return animator != null && animator.avatar != null && animator.isHuman && (animator.gameObject.activeInHierarchy || !base.gameObject.activeInHierarchy);
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x060094F6 RID: 38134 RVA: 0x003D2960 File Offset: 0x003D0B60
		public AttentionUIController AttentionUI
		{
			get
			{
				if (!(this._attentionUI != null))
				{
					return this._attentionUI = base.GetComponent<AttentionUIController>();
				}
				return this._attentionUI;
			}
		}

		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x060094F7 RID: 38135 RVA: 0x003D2991 File Offset: 0x003D0B91
		public Vector3 ViewDirection
		{
			get
			{
				return base.transform.forward;
			}
		}

		// Token: 0x060094F8 RID: 38136 RVA: 0x003D29A0 File Offset: 0x003D0BA0
		private void OnEnable()
		{
			this._eyeAnimator = null;
			this._eyeAvatar = null;
			this._humanoidHead = null;
			this._nextHeadLookupTime = 0f;
			if (SensoryManager.Instance)
			{
				SensoryManager.Instance.RegisterNPC(this);
			}
			this._wanderController = base.GetComponent<NPCWanderController>();
			this._fsm = base.GetComponent<NPCStateMachine>();
		}

		// Token: 0x060094F9 RID: 38137 RVA: 0x003D29FC File Offset: 0x003D0BFC
		private void Start()
		{
			if (SensoryManager.Instance)
			{
				SensoryManager.Instance.RegisterNPC(this);
				return;
			}
			Debug.LogWarning("[NPCSenses] " + base.name + " could not register with SensoryManager! Is it present in the scene?");
		}

		// Token: 0x060094FA RID: 38138 RVA: 0x003D2A30 File Offset: 0x003D0C30
		private void OnDisable()
		{
			this._ghostVisualContactLatched = false;
			this.IsGhostFleeing = false;
			if (SensoryManager.Instance)
			{
				SensoryManager.Instance.UnregisterNPC(this);
			}
		}

		// Token: 0x060094FB RID: 38139 RVA: 0x003D2A57 File Offset: 0x003D0C57
		public void UpdateGhostVisualContact(bool hasContact)
		{
			if (hasContact && !this._ghostVisualContactLatched)
			{
				Action<NPCSenses> onPlayerSpotted = NPCSenses.OnPlayerSpotted;
				if (onPlayerSpotted != null)
				{
					onPlayerSpotted(this);
				}
			}
			this._ghostVisualContactLatched = hasContact;
		}

		// Token: 0x060094FC RID: 38140 RVA: 0x003D2A7C File Offset: 0x003D0C7C
		public void SuppressDetectionForGhost()
		{
			this.HasLineOfSight = false;
			this.HasAudioContact = false;
			this.CurrentDetectionSource = DetectionSource.None;
			this.SetAttentionState(0f);
			this.IsAlert = false;
			this.IsDetected = false;
			this.IsPassiveReacting = false;
			this.PassiveReactionType = 0;
			this.PassiveReactionEntry = null;
			Animator componentInChildren = base.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.isActiveAndEnabled)
			{
				componentInChildren.SetInteger("ReactionType", 0);
			}
		}

		// Token: 0x060094FD RID: 38141 RVA: 0x003D2AF0 File Offset: 0x003D0CF0
		public void StartCooldown()
		{
			if (this.IsInCooldown)
			{
				return;
			}
			base.StartCoroutine(this.CooldownRoutine());
		}

		// Token: 0x060094FE RID: 38142 RVA: 0x003D2B08 File Offset: 0x003D0D08
		private IEnumerator CooldownRoutine()
		{
			this.IsInCooldown = true;
			float duration = ((SensoryManager.Instance != null && SensoryManager.Instance.GlobalConfig != null) ? SensoryManager.Instance.GlobalConfig.CooldownTime : 5f);
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += NPCSimulationPause.SimulationDeltaTime;
				yield return null;
			}
			this.IsInCooldown = false;
			this.CurrentAttention = 0f;
			this.IsDetected = false;
			yield break;
		}

		// Token: 0x060094FF RID: 38143 RVA: 0x003D2B18 File Offset: 0x003D0D18
		public void UpdateActionState(NPCActionDefinition action)
		{
			if (action == null)
			{
				this._actionVisionMult = 1f;
				this._actionAngleMult = 1f;
				this._actionAttentionLevel = 1f;
				return;
			}
			this._actionVisionMult = action.visionRadiusMultiplier;
			this._actionAngleMult = action.visionAngleMultiplier;
			this._actionAttentionLevel = action.attentionLevel;
		}

		// Token: 0x06009500 RID: 38144 RVA: 0x003D2B74 File Offset: 0x003D0D74
		public void SetAttentionState(float newAttention)
		{
			float currentAttention = this.CurrentAttention;
			this.CurrentAttention = newAttention;
			if (!Mathf.Approximately(currentAttention, newAttention))
			{
				Action<NPCSenses, float, float> onAttentionChanged = NPCSenses.OnAttentionChanged;
				if (onAttentionChanged == null)
				{
					return;
				}
				onAttentionChanged(this, currentAttention, newAttention);
			}
		}

		// Token: 0x06009501 RID: 38145 RVA: 0x003D2BAC File Offset: 0x003D0DAC
		private void OnDrawGizmosSelected()
		{
			Vector3 eyePosition = this.EyePosition;
			Vector3 viewDirection = this.ViewDirection;
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(eyePosition, viewDirection * this.EffectiveViewRadius);
			Vector3 vector = Quaternion.Euler(0f, this.EffectiveViewAngle * 0.5f, 0f) * viewDirection;
			Vector3 vector2 = Quaternion.Euler(0f, -this.EffectiveViewAngle * 0.5f, 0f) * viewDirection;
			Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
			Gizmos.DrawLine(eyePosition, eyePosition + vector * this.EffectiveViewRadius);
			Gizmos.DrawLine(eyePosition, eyePosition + vector2 * this.EffectiveViewRadius);
			if (Application.isPlaying && SensoryManager.Instance)
			{
				Gizmos.color = (this.IsDetected ? Color.red : ((this.CurrentAttention > 0f) ? Color.yellow : Color.green));
				Gizmos.DrawWireSphere(eyePosition, 0.2f);
			}
		}

		// Token: 0x04009A03 RID: 39427
		[Header("Configuration")]
		[Tooltip("Explicit eye-position anchor. Character/rig roots are resolved to the humanoid head automatically. View direction always uses the character root.")]
		public Transform EyeTransform;

		// Token: 0x04009A04 RID: 39428
		public NPCReactionProfileSO ReactionProfile;

		// Token: 0x04009A05 RID: 39429
		[Header("Base Stats")]
		public float BaseViewRadius = 15f;

		// Token: 0x04009A06 RID: 39430
		public float BaseViewAngle = 110f;

		// Token: 0x04009A07 RID: 39431
		[Header("Runtime State")]
		[Range(0f, 1f)]
		public float CurrentAttention;

		// Token: 0x04009A08 RID: 39432
		public bool IsAlert;

		// Token: 0x04009A09 RID: 39433
		public bool IsDetected;

		// Token: 0x04009A0A RID: 39434
		public bool IsInCooldown;

		// Token: 0x04009A0B RID: 39435
		[Header("Debug")]
		[Tooltip("Enable detailed logging for this NPC's detection state")]
		public bool DebugLog;

		// Token: 0x04009A0C RID: 39436
		[Header("Sensory Contact State")]
		[Tooltip("True if the NPC currently sees the player (Raycast + FOV successful)")]
		[SerializeField]
		private bool _hasLineOfSight;

		// Token: 0x04009A0D RID: 39437
		[Tooltip("True if the NPC currently hears the player (Distance < Noise Radius)")]
		public bool HasAudioContact;

		// Token: 0x04009A10 RID: 39440
		[Tooltip("Current source of detection (Visual has priority over Audio)")]
		public DetectionSource CurrentDetectionSource;

		// Token: 0x04009A11 RID: 39441
		[HideInInspector]
		public Vector3 LastKnownPlayerPosition;

		// Token: 0x04009A12 RID: 39442
		[HideInInspector]
		public bool IsPassiveReacting;

		// Token: 0x04009A13 RID: 39443
		[HideInInspector]
		public int PassiveReactionType = 1;

		// Token: 0x04009A14 RID: 39444
		[HideInInspector]
		public NPCReactionEntry PassiveReactionEntry;

		// Token: 0x04009A16 RID: 39446
		private bool _ghostVisualContactLatched;

		// Token: 0x04009A17 RID: 39447
		private AttentionUIController _attentionUI;

		// Token: 0x04009A18 RID: 39448
		private float _actionVisionMult = 1f;

		// Token: 0x04009A19 RID: 39449
		private float _actionAngleMult = 1f;

		// Token: 0x04009A1A RID: 39450
		private float _actionAttentionLevel = 1f;

		// Token: 0x04009A1B RID: 39451
		private NPCWanderController _wanderController;

		// Token: 0x04009A1C RID: 39452
		private NPCStateMachine _fsm;

		// Token: 0x04009A1D RID: 39453
		private StaticPhoneNPC _staticPhone;

		// Token: 0x04009A1E RID: 39454
		private Animator _eyeAnimator;

		// Token: 0x04009A1F RID: 39455
		private Avatar _eyeAvatar;

		// Token: 0x04009A20 RID: 39456
		private Transform _humanoidHead;

		// Token: 0x04009A21 RID: 39457
		private float _nextHeadLookupTime;
	}
}
