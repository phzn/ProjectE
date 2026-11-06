using System;
using Features.Detection;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000F02 RID: 3842
	[DisallowMultipleComponent]
	public class SeatedRandomReaction : MonoBehaviour
	{
		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x0600732C RID: 29484 RVA: 0x002EFA5E File Offset: 0x002EDC5E
		public float EffectiveTriggerRange
		{
			get
			{
				return NpcVisionDifficultyPolicy.ApplyRadius(this.triggerRange);
			}
		}

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x0600732D RID: 29485 RVA: 0x002EFA6C File Offset: 0x002EDC6C
		// (remove) Token: 0x0600732E RID: 29486 RVA: 0x002EFAA4 File Offset: 0x002EDCA4
		public event Action<bool> ReactionActiveChanged;

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x0600732F RID: 29487 RVA: 0x002EFAD9 File Offset: 0x002EDCD9
		public bool IsReacting
		{
			get
			{
				return this._reacting;
			}
		}

		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x06007330 RID: 29488 RVA: 0x002EFAE1 File Offset: 0x002EDCE1
		// (set) Token: 0x06007331 RID: 29489 RVA: 0x002EFAE9 File Offset: 0x002EDCE9
		internal NPCAmbientSuspension ProximitySuspension { get; set; }

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x06007332 RID: 29490 RVA: 0x002EFAF2 File Offset: 0x002EDCF2
		public bool PresentationAllowsCapture
		{
			get
			{
				return this._capturePresentationOwner == null || this._presentationAllowsCapture;
			}
		}

		// Token: 0x06007333 RID: 29491 RVA: 0x002EFB0A File Offset: 0x002EDD0A
		public void SetPresentationCaptureAllowed(Object owner, bool allowed)
		{
			if (owner == null)
			{
				return;
			}
			this._capturePresentationOwner = owner;
			this._presentationAllowsCapture = allowed;
			if (!allowed && this._reacting)
			{
				this.EndReaction();
			}
		}

		// Token: 0x06007334 RID: 29492 RVA: 0x002EFB35 File Offset: 0x002EDD35
		private void Awake()
		{
			if (this.animator == null || !this.animator.transform.IsChildOf(base.transform))
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			this._equipment = base.GetComponentInChildren<NPCEquipmentManager>();
		}

		// Token: 0x06007335 RID: 29493 RVA: 0x002EFB78 File Offset: 0x002EDD78
		private void OnEnable()
		{
			this.EnableSeatedEncounterDetection();
			this._simulationPauseActive = false;
			this._nextDecisionTime = Time.time + this.decisionInterval;
			this._reacting = false;
			this._isPhotographer = Random.value < this.photographerChance;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x06007336 RID: 29494 RVA: 0x002EFBE0 File Offset: 0x002EDDE0
		private void EnableSeatedEncounterDetection()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			NPCSeatedEncounter.ConfigureStaticDetection(base.GetComponentInParent<NPCSenses>(true));
		}

		// Token: 0x06007337 RID: 29495 RVA: 0x002EFBF6 File Offset: 0x002EDDF6
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			if (this._reacting)
			{
				this.EndReaction();
			}
		}

		// Token: 0x06007338 RID: 29496 RVA: 0x002EFC18 File Offset: 0x002EDE18
		private void Update()
		{
			if (this.ProximitySuspension != null && this.ProximitySuspension.IsSuspended)
			{
				return;
			}
			if (!this.PresentationAllowsCapture || !PlayerExposureState.IsExposed)
			{
				if (this._reacting)
				{
					this.EndReaction();
				}
				return;
			}
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
			if (frameTimeLoss > 0f)
			{
				this._nextDecisionTime += frameTimeLoss;
				this._reactionEndTime += frameTimeLoss;
			}
			if (this._reacting)
			{
				if (Time.time >= this._reactionEndTime || !this.CanSeePlayer(this.ResolveTarget()))
				{
					this.EndReaction();
				}
				return;
			}
			if (!this._isPhotographer)
			{
				return;
			}
			if (Time.time < this._nextDecisionTime)
			{
				return;
			}
			this._nextDecisionTime = Time.time + this.decisionInterval;
			if (Time.time < this._reactionEndTime + this.cooldown)
			{
				return;
			}
			Transform transform = this.ResolveTarget();
			if (transform == null)
			{
				if (this.debugLog)
				{
					Debug.Log(string.Concat(new string[] { "[SeatedReaction] ", base.name, ": no player target (tag '", this.playerTag, "')" }));
				}
				return;
			}
			if (!this.CanSeePlayer(transform))
			{
				if (this.debugLog)
				{
					float num = Vector3.Distance(base.transform.position, transform.position);
					Debug.Log(string.Format("[SeatedReaction] {0}: player outside visible front/range (dist={1:F1}, range={2})", base.name, num, this.triggerRange));
				}
				return;
			}
			if (Random.value > this.reactionChance)
			{
				return;
			}
			this.StartReaction();
		}

		// Token: 0x06007339 RID: 29497 RVA: 0x002EFDBB File Offset: 0x002EDFBB
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x0600733A RID: 29498 RVA: 0x002EFDCD File Offset: 0x002EDFCD
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x0600733B RID: 29499 RVA: 0x002EFDEC File Offset: 0x002EDFEC
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			float num = Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			this._nextDecisionTime += num;
			this._reactionEndTime += num;
			this._simulationPauseActive = false;
		}

		// Token: 0x0600733C RID: 29500 RVA: 0x002EFE3C File Offset: 0x002EE03C
		public bool CanCapturePlayer()
		{
			return this.PresentationAllowsCapture && PlayerExposureState.IsExposed && this.CanSeePlayer(this.ResolveTarget());
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x002EFE5C File Offset: 0x002EE05C
		private bool CanSeePlayer(Transform target)
		{
			if (target == null)
			{
				return false;
			}
			Vector3 vector = target.position - base.transform.position;
			float effectiveTriggerRange = this.EffectiveTriggerRange;
			if (vector.sqrMagnitude > effectiveTriggerRange * effectiveTriggerRange)
			{
				return false;
			}
			vector.y = 0f;
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude < 0.0001f || vector.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			if (Vector3.Angle(forward, vector) > this.frontConeHalfAngle)
			{
				return false;
			}
			if (this._sight == null)
			{
				this._sight = new NPCPassiveLineOfSight(this, 1.1f);
			}
			return this._sight.CanSee(target);
		}

		// Token: 0x0600733E RID: 29502 RVA: 0x002EFF18 File Offset: 0x002EE118
		private void StartReaction()
		{
			if (this.animator == null || !this.CanCapturePlayer())
			{
				return;
			}
			int num = (this.randomPhotoOrVideo ? ((Random.value < this.takePhotoChance) ? 1 : 2) : this.fixedReactionType);
			if (this.reactionsLayerIndex >= 0 && this.reactionsLayerIndex < this.animator.layerCount)
			{
				this.animator.SetLayerWeight(this.reactionsLayerIndex, 1f);
			}
			this.animator.SetInteger(SeatedRandomReaction.ReactionTypeHash, num);
			this.animator.SetTrigger(SeatedRandomReaction.ReactionTriggerHash);
			if (!string.IsNullOrEmpty(this.phonePropName) && this._equipment != null)
			{
				this._equipment.ActivateAndLockProp(this.phonePropName);
			}
			this._reacting = true;
			this._reactionEndTime = Time.time + this.reactionDuration;
			Action<bool> reactionActiveChanged = this.ReactionActiveChanged;
			if (reactionActiveChanged != null)
			{
				reactionActiveChanged(true);
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[SeatedReaction] {0}: reaction {1} (type={2})", base.name, (num == 2) ? "TakeVideo" : "TakePhoto", num));
			}
		}

		// Token: 0x0600733F RID: 29503 RVA: 0x002F003C File Offset: 0x002EE23C
		private void EndReaction()
		{
			this._reacting = false;
			this._reactionEndTime = Time.time;
			Action<bool> reactionActiveChanged = this.ReactionActiveChanged;
			if (reactionActiveChanged != null)
			{
				reactionActiveChanged(false);
			}
			if (this.animator != null)
			{
				this.animator.ResetTrigger(SeatedRandomReaction.ReactionTriggerHash);
				this.animator.SetInteger(SeatedRandomReaction.ReactionTypeHash, 0);
			}
			if (!string.IsNullOrEmpty(this.phonePropName) && this._equipment != null)
			{
				this._equipment.DeactivateProp(this.phonePropName);
			}
			if (this.debugLog)
			{
				Debug.Log("[SeatedReaction] " + base.name + ": reaction ended");
			}
		}

		// Token: 0x06007340 RID: 29504 RVA: 0x002F00EC File Offset: 0x002EE2EC
		private Transform ResolveTarget()
		{
			if (this.manualTarget != null)
			{
				return this.manualTarget;
			}
			if (this._cachedPlayer != null && this._cachedPlayer.gameObject.activeInHierarchy)
			{
				return this._cachedPlayer;
			}
			if (string.IsNullOrEmpty(this.playerTag))
			{
				return null;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.playerTag);
			this._cachedPlayer = ((gameObject != null) ? gameObject.transform : null);
			return this._cachedPlayer;
		}

		// Token: 0x06007341 RID: 29505 RVA: 0x002F0170 File Offset: 0x002EE370
		private void OnDrawGizmosSelected()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			Gizmos.color = new Color(1f, 0.3f, 0.8f, 0.5f);
			Gizmos.DrawWireSphere(base.transform.position, this.triggerRange);
			Vector3 forward = base.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			Quaternion quaternion = Quaternion.AngleAxis(-this.frontConeHalfAngle, Vector3.up);
			Quaternion quaternion2 = Quaternion.AngleAxis(this.frontConeHalfAngle, Vector3.up);
			Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.7f);
			Gizmos.DrawRay(base.transform.position, quaternion * forward * this.triggerRange);
			Gizmos.DrawRay(base.transform.position, quaternion2 * forward * this.triggerRange);
		}

		// Token: 0x04007669 RID: 30313
		[Header("Target")]
		[Tooltip("Tag del player. Default 'Player'.")]
		public string playerTag = "Player";

		// Token: 0x0400766A RID: 30314
		[Tooltip("Se assegnato, bypassa la ricerca per tag.")]
		public Transform manualTarget;

		// Token: 0x0400766B RID: 30315
		[Header("Condizioni di attivazione")]
		[Tooltip("Distanza massima player-NPC entro cui può partire la reaction (metri).")]
		[Min(0.5f)]
		public float triggerRange = 8f;

		// Token: 0x0400766C RID: 30316
		[Tooltip("Semiangolo del cono frontale (gradi): il player deve stare entro questo angolo rispetto al forward dell'NPC. 90 = mezzo spazio davanti.")]
		[Range(5f, 180f)]
		public float frontConeHalfAngle = 60f;

		// Token: 0x0400766D RID: 30317
		[Header("Casualità")]
		[Tooltip("Probabilità (0-1) che QUESTO NPC sia il tipo che tira fuori il telefono. Il dado si tira UNA volta per attivazione: chi lo fallisce non reagisce affatto finché resta acceso, si limita a fissare. Serve perché il dado per-decisione qui sotto, ripetuto ogni pochi secondi, prima o poi fa reagire chiunque. 1 = tutti fotografano (comportamento storico).")]
		[Range(0f, 1f)]
		public float photographerChance = 1f;

		// Token: 0x0400766E RID: 30318
		[Tooltip("Ogni quanto (secondi) l'NPC 'tira un dado' per decidere se reagire, quando le condizioni (vicino + davanti) sono soddisfatte.")]
		[Min(0.1f)]
		public float decisionInterval = 1.5f;

		// Token: 0x0400766F RID: 30319
		[Tooltip("Probabilità (0-1) di far partire la reaction a ogni decisione valida.")]
		[Range(0f, 1f)]
		public float reactionChance = 0.35f;

		// Token: 0x04007670 RID: 30320
		[Tooltip("Pausa minima tra due reaction (secondi), così non spamma il cellulare.")]
		[Min(0f)]
		public float cooldown = 6f;

		// Token: 0x04007671 RID: 30321
		[Header("Reaction")]
		[Tooltip("Se vero, sceglie a caso tra foto (1) e video (2). Se falso, usa solo 'fixedReactionType'.")]
		public bool randomPhotoOrVideo = true;

		// Token: 0x04007672 RID: 30322
		[Tooltip("Chance of choosing a photo instead of a video for random reactions.")]
		[Range(0f, 1f)]
		public float takePhotoChance = 0.8f;

		// Token: 0x04007673 RID: 30323
		[Tooltip("ReactionType da usare se randomPhotoOrVideo è falso. 1=TakePhoto, 2=TakeVideo.")]
		[Range(1f, 2f)]
		public int fixedReactionType = 2;

		// Token: 0x04007674 RID: 30324
		[Tooltip("Durata (secondi) della reaction prima di tornare seduto idle.")]
		[Min(0.5f)]
		public float reactionDuration = 3.5f;

		// Token: 0x04007675 RID: 30325
		[Tooltip("Nome del prop telefono (figlio dell'NPC) da attivare durante la reaction. Vuoto = non gestire prop (li gestisce già la clip/equipment).")]
		public string phonePropName = "Smartphone";

		// Token: 0x04007676 RID: 30326
		[Header("Animator")]
		[Tooltip("Animator dell'NPC. Auto-trovato se vuoto.")]
		public Animator animator;

		// Token: 0x04007677 RID: 30327
		[Tooltip("Indice del layer Reactions nel controller. Nel NPC_Master_Seated è 1.")]
		public int reactionsLayerIndex = 1;

		// Token: 0x04007678 RID: 30328
		[Header("Debug")]
		public bool debugLog;

		// Token: 0x04007679 RID: 30329
		public bool drawGizmos;

		// Token: 0x0400767B RID: 30331
		private static readonly int ReactionTypeHash = Animator.StringToHash("ReactionType");

		// Token: 0x0400767C RID: 30332
		private static readonly int ReactionTriggerHash = Animator.StringToHash("ReactionTrigger");

		// Token: 0x0400767D RID: 30333
		private Transform _cachedPlayer;

		// Token: 0x0400767E RID: 30334
		private NPCPassiveLineOfSight _sight;

		// Token: 0x0400767F RID: 30335
		private NPCEquipmentManager _equipment;

		// Token: 0x04007680 RID: 30336
		private float _nextDecisionTime;

		// Token: 0x04007681 RID: 30337
		private float _reactionEndTime;

		// Token: 0x04007682 RID: 30338
		private bool _reacting;

		// Token: 0x04007684 RID: 30340
		private bool _isPhotographer = true;

		// Token: 0x04007685 RID: 30341
		private bool _simulationPauseActive;

		// Token: 0x04007686 RID: 30342
		private float _simulationPauseStartedAt;

		// Token: 0x04007687 RID: 30343
		private Object _capturePresentationOwner;

		// Token: 0x04007688 RID: 30344
		private bool _presentationAllowsCapture = true;
	}
}
