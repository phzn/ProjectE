using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Features.GoldBikini;
using Features.PlayerEmotions;
using Features.Rank;
using Features.Slip;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using PoolWaterTools;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.PoolDive
{
	// Token: 0x02000DD5 RID: 3541
	public class PoolDiveController : MonoBehaviour
	{
		// Token: 0x060068C4 RID: 26820 RVA: 0x0029E63C File Offset: 0x0029C83C
		private void BeginDiveView()
		{
			if (this._firstPerson == null)
			{
				this._firstPerson = base.GetComponentInParent<FirstPersonController>(true);
			}
			if (this._firstPerson == null && this.animator != null)
			{
				this._firstPerson = this.animator.GetComponent<FirstPersonController>();
			}
			FirstPersonController firstPerson = this._firstPerson;
			if (firstPerson == null)
			{
				return;
			}
			firstPerson.BeginInteractionView(this, true, 65f, true, false, null, null, null);
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x0029E6BD File Offset: 0x0029C8BD
		private void EndDiveView()
		{
			FirstPersonController firstPerson = this._firstPerson;
			if (firstPerson == null)
			{
				return;
			}
			firstPerson.EndInteractionView(this);
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060068C6 RID: 26822 RVA: 0x0029E6D0 File Offset: 0x0029C8D0
		public bool PlayerInZone
		{
			get
			{
				return this.playerInZone;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060068C7 RID: 26823 RVA: 0x0029E6D8 File Offset: 0x0029C8D8
		public bool IsDiving
		{
			get
			{
				return this._diving;
			}
		}

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060068C8 RID: 26824 RVA: 0x0029E6E0 File Offset: 0x0029C8E0
		public bool IsWaterHandoffActive
		{
			get
			{
				return this._diving || this._swimHandoffPending;
			}
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x0029E6F4 File Offset: 0x0029C8F4
		public bool InterruptForEncounter()
		{
			if (!this._diving && !this._swimHandoffPending && this._diveLease == null && !this._emotionSuppressed)
			{
				return false;
			}
			base.StopAllCoroutines();
			this.EndDiveView();
			if (this._emotionSuppressed)
			{
				PlayerEmotionController instance = PlayerEmotionController.Instance;
				if (instance != null)
				{
					instance.RestoreEmotionLayers();
				}
				this._emotionSuppressed = false;
			}
			if ((this._diving || this._swimHandoffPending) && this._cc != null && this._cc._rigidbody != null)
			{
				this._cc._rigidbody.isKinematic = this._rbWasKinematic;
			}
			this._diving = false;
			this._swimHandoffPending = false;
			this._cooldownUntil = Time.time + this.diveCooldown;
			this.ReleaseDiveContext();
			return true;
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060068CA RID: 26826 RVA: 0x0029E7BC File Offset: 0x0029C9BC
		public bool CanDive
		{
			get
			{
				if (!this.playerInZone || this._diving || this._swimHandoffPending || Time.time < this._cooldownUntil)
				{
					return false;
				}
				if (this._swimming == null)
				{
					return true;
				}
				if (this._swimming.isSwimming)
				{
					return false;
				}
				Transform transform = ((this.animator != null) ? this.animator.transform : base.transform);
				return !this._swimming.inTheWater || this._swimming.water == null || this.BodyCenterY(transform) > this._swimming.water.transform.position.y;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060068CB RID: 26827 RVA: 0x0029E876 File Offset: 0x0029CA76
		public bool CanStripOnDive
		{
			get
			{
				return (this.slipController != null && this.slipController.HasAnyWornWaterPiece) || (this.bikiniSlipController != null && this.bikiniSlipController.HasAnyWornWaterPiece);
			}
		}

		// Token: 0x060068CC RID: 26828 RVA: 0x0029E8B0 File Offset: 0x0029CAB0
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			this.ResolveSlipControllers();
			if (this.bikiniSlipDiveController == null)
			{
				this.bikiniSlipDiveController = base.GetComponentInParent<BikiniSlipDiveController>();
			}
			if (this.bikiniSlipDiveController == null)
			{
				this.bikiniSlipDiveController = base.GetComponentInChildren<BikiniSlipDiveController>();
			}
			this._swimming = base.GetComponentInParent<vSwimming>();
			if (this._swimming == null)
			{
				this._swimming = base.GetComponentInChildren<vSwimming>();
			}
			this._cc = base.GetComponentInParent<vThirdPersonController>();
			if (this._cc == null && this._swimming != null)
			{
				this._cc = this._swimming.GetComponentInParent<vThirdPersonController>();
			}
			this._stateGuard = base.GetComponentInParent<PoolEntryStateGuard>();
			if (this._stateGuard == null && this._swimming != null)
			{
				this._stateGuard = this._swimming.GetComponentInParent<PoolEntryStateGuard>();
			}
			if (this.audioSource == null && this.splashSound != null)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
				this.audioSource.playOnAwake = false;
				this.audioSource.spatialBlend = 1f;
			}
		}

		// Token: 0x060068CD RID: 26829 RVA: 0x0029E9F4 File Offset: 0x0029CBF4
		private void ResolveSlipControllers()
		{
			if (this.slipController != null && !this.slipController.HasWaterSlipPieces)
			{
				this.slipController = null;
			}
			if (this.bikiniSlipController == this.slipController || (this.bikiniSlipController != null && !this.bikiniSlipController.HasWaterSlipPieces))
			{
				this.bikiniSlipController = null;
			}
			foreach (ClothSlipController clothSlipController in ((this.animator != null) ? this.animator.transform : base.transform).GetComponentsInChildren<ClothSlipController>(true))
			{
				if (clothSlipController.HasWaterSlipPieces && !(clothSlipController == this.slipController) && !(clothSlipController == this.bikiniSlipController))
				{
					if (this.slipController == null)
					{
						this.slipController = clothSlipController;
					}
					else if (this.bikiniSlipController == null)
					{
						this.bikiniSlipController = clothSlipController;
					}
				}
			}
		}

		// Token: 0x060068CE RID: 26830 RVA: 0x0029EAE4 File Offset: 0x0029CCE4
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (!this.CanDive)
			{
				return;
			}
			bool flag = this.diveInput != null && this.diveInput.GetButtonDown();
			bool flag2 = this.CanStripOnDive && this.diveStripInput != null && this.diveStripInput.GetButtonDown();
			if (!flag && !flag2)
			{
				return;
			}
			string text;
			if (!this.EvaluateDiveAllowed(out text))
			{
				if (this.diveDiagLogs)
				{
					Debug.Log("[PoolDive][DIAG] Dive DENIED: " + text);
				}
				return;
			}
			if (this.diveDiagLogs)
			{
				Debug.Log("[PoolDive][DIAG] Dive ALLOWED (" + (flag2 ? "G/strip" : "F/clean") + "): " + text);
			}
			if (!this.TryAcquireDiveContext())
			{
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			base.StartCoroutine(this.DiveRoutine(flag2));
		}

		// Token: 0x060068CF RID: 26831 RVA: 0x0029EBB0 File Offset: 0x0029CDB0
		private bool TryAcquireDiveContext()
		{
			if (this._diveLease != null)
			{
				if (this._diveLease.IsActive)
				{
					return true;
				}
				this._diveLease = null;
			}
			Transform player = ((this._cc != null) ? this._cc.transform : ((this.animator != null) ? this.animator.transform : base.transform));
			if (!PlayerActivityGate.CanAcquireAfterReleasing(PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, (Object owner) => PoolWaterEntryPreparation.CanReleaseOwner(owner, player)))
			{
				return false;
			}
			PoolWaterEntryPreparation.Prepare(player);
			if (!PlayerActivityGate.TryAcquire("PoolDive", this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._diveLease, false, null))
			{
				return false;
			}
			InteractionCandidateRegistry.MarkInteract();
			return true;
		}

		// Token: 0x060068D0 RID: 26832 RVA: 0x0029EC6D File Offset: 0x0029CE6D
		private void ReleaseDiveContext()
		{
			PlayerActivityLease diveLease = this._diveLease;
			this._diveLease = null;
			if (diveLease == null)
			{
				return;
			}
			diveLease.Dispose();
		}

		// Token: 0x060068D1 RID: 26833 RVA: 0x0029EC88 File Offset: 0x0029CE88
		private bool EvaluateDiveAllowed(out string reason)
		{
			Transform transform = ((this.animator != null) ? this.animator.transform : base.transform);
			Collider collider = this.FindOverlappingWater(transform.position);
			if (collider == null)
			{
				reason = "nessun volume Water rilevato vicino al player";
				return false;
			}
			Vector3 forward = transform.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 vector = collider.ClosestPoint(transform.position + Vector3.up * 0.9f) - transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0004f)
			{
				vector = collider.bounds.center - transform.position;
				vector.y = 0f;
			}
			vector.Normalize();
			float num = Vector3.Dot(forward, vector);
			if (num < this.minFacingWaterDot)
			{
				reason = string.Format("non guarda verso l'acqua (dot={0:F2} < {1:F2})", num, this.minFacingWaterDot);
				return false;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(transform.position + Vector3.up * 0.9f, forward, out raycastHit, this.diveClearanceDistance, this.diveClearanceMask, QueryTriggerInteraction.Ignore))
			{
				reason = string.Format("ostacolo davanti a {0:F2}m ('{1}') < clearance {2:F2}m", raycastHit.distance, raycastHit.collider.name, this.diveClearanceDistance);
				return false;
			}
			reason = string.Format("ok (facing={0:F2}, clearance>={1:F2}m, water='{2}')", num, this.diveClearanceDistance, collider.name);
			return true;
		}

		// Token: 0x060068D2 RID: 26834 RVA: 0x0029EE24 File Offset: 0x0029D024
		public void SetPlayerInZone(DiveZone zone, bool inZone)
		{
			if (zone == null)
			{
				this.SetPlayerInZone(inZone);
				return;
			}
			if (inZone)
			{
				this._activeDiveZones.Add(zone);
			}
			else
			{
				this._activeDiveZones.Remove(zone);
			}
			this.playerInZone = this._activeDiveZones.Count > 0;
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x0029EE75 File Offset: 0x0029D075
		public void SetPlayerInZone(bool inZone)
		{
			this._activeDiveZones.Clear();
			this.playerInZone = inZone;
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x0029EE89 File Offset: 0x0029D089
		private IEnumerator DiveRoutine(bool strip)
		{
			if (this.animator == null || (strip && !this.CanStripOnDive))
			{
				this.ReleaseDiveContext();
				yield break;
			}
			this._diving = true;
			this.BeginDiveView();
			this._cooldownUntil = Time.time + this.diveCooldown + this.fallbackDiveDuration;
			if (this._stateGuard != null)
			{
				this._stateGuard.NotifyDiveStarting();
			}
			Transform playerRoot = this.animator.transform;
			Collider water = this.FindOverlappingWater(playerRoot.position);
			float waterlineY = ((water != null) ? water.transform.position.y : this.fallbackWaterlineY);
			PlayerEmotionController emo = PlayerEmotionController.Instance;
			if (emo != null)
			{
				emo.DisableEmotionLayersForAction();
				this._emotionSuppressed = true;
			}
			this.animator.CrossFadeInFixedTime(this.diveStateName, this.diveCrossfade);
			bool rbWasKinematic = false;
			if (this._cc != null && this._cc._rigidbody != null)
			{
				rbWasKinematic = this._cc._rigidbody.isKinematic;
				this._rbWasKinematic = rbWasKinematic;
				this._cc._rigidbody.velocity = Vector3.zero;
				this._cc._rigidbody.isKinematic = true;
			}
			float duration = this.ResolveDiveDuration();
			float elapsed = 0f;
			bool stripFired = false;
			bool embFired = false;
			bool splashed = false;
			while (elapsed < duration + 0.5f)
			{
				if (strip && !stripFired && elapsed >= this.stripDelaySeconds)
				{
					if (this.slipController != null)
					{
						this.slipController.SlipInWaterAll(this._diveLease);
					}
					if (this.bikiniSlipController != null && this.bikiniSlipController != this.slipController)
					{
						this.bikiniSlipController.SlipInWaterAll(this._diveLease);
					}
					stripFired = true;
				}
				if (strip && !embFired && this.DiveNormalizedTime() >= this.embSupremeNormalizedTime)
				{
					this.ApplyEmbSupreme();
					embFired = true;
				}
				if (!splashed && this.BodyCenterY(playerRoot) <= waterlineY)
				{
					this.PlaySplash(playerRoot, waterlineY);
					if (!strip && this.bikiniSlipDiveController != null)
					{
						this.bikiniSlipDiveController.TryFireRandomSlipsOnDive();
					}
					splashed = true;
				}
				if (this.DiveNormalizedTime() >= this.swimHandoffNormalizedTime)
				{
					break;
				}
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (this._emotionSuppressed && emo != null)
			{
				emo.RestoreEmotionLayers();
				this._emotionSuppressed = false;
			}
			this._rbWasKinematic = rbWasKinematic;
			bool flag = splashed || this.BodyCenterY(playerRoot) <= waterlineY + this.submergeDepth;
			if (this.diveDiagLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					string.Format("[PoolDive][DIAG] dive loop ended: elapsed={0:F2} duration={1:F2} ", elapsed, duration),
					string.Format("diveNorm={0:F2} splashed={1} ", this.DiveNormalizedTime(), splashed),
					string.Format("bodyCenterY={0:F2} waterlineY={1:F2} ", this.BodyCenterY(playerRoot), waterlineY),
					string.Format("(threshold {0:F2}) playerY={1:F2} ", waterlineY + this.submergeDepth, playerRoot.position.y),
					string.Format("=> willEngageSwimming={0} | waterAtStart={1}", flag, (water != null) ? water.name : "NULL")
				}));
			}
			this.CompleteDive(flag && this.EngageSwimming(water, playerRoot.position));
			yield break;
		}

		// Token: 0x060068D5 RID: 26837 RVA: 0x0029EEA0 File Offset: 0x0029D0A0
		private void CompleteDive(bool swimmingEngaged)
		{
			if (!swimmingEngaged)
			{
				if (this._cc != null && this._cc._rigidbody != null)
				{
					this._cc._rigidbody.isKinematic = this._rbWasKinematic;
				}
				PoolEntryStateGuard stateGuard = this._stateGuard;
				if (stateGuard != null)
				{
					stateGuard.RestoreSnapshot("dive handoff failed");
				}
				this.EndDiveView();
			}
			this._diving = false;
			this._cooldownUntil = Time.time + this.diveCooldown;
			this.ReleaseDiveContext();
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x0029EF24 File Offset: 0x0029D124
		private float DiveNormalizedTime()
		{
			if (this.animator == null)
			{
				return 0f;
			}
			int num = 0;
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(num);
			if (currentAnimatorStateInfo.IsName(this.diveStateName))
			{
				return currentAnimatorStateInfo.normalizedTime;
			}
			AnimatorStateInfo nextAnimatorStateInfo = this.animator.GetNextAnimatorStateInfo(num);
			if (this.animator.IsInTransition(num) && nextAnimatorStateInfo.IsName(this.diveStateName))
			{
				return nextAnimatorStateInfo.normalizedTime;
			}
			return 0f;
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x0029EFA4 File Offset: 0x0029D1A4
		private bool EngageSwimming(Collider knownWater, Vector3 playerPos)
		{
			if (this._swimming == null)
			{
				return false;
			}
			Collider collider = ((knownWater != null) ? knownWater : this.FindOverlappingWater(playerPos));
			if (collider == null)
			{
				Debug.LogWarning("[PoolDive] EngageSwimming: NO water collider (knownWater was null AND FindOverlappingWater found nothing at handoff) → swimming NOT engaged.");
				return false;
			}
			Transform transform = ((this.animator != null) ? this.animator.transform : base.transform);
			if (this.diveDiagLogs)
			{
				Debug.Log(string.Format("[PoolDive][DIAG] EngageSwimming ENTER: water='{0}' surfaceY={1:F2} ", collider.name, collider.transform.position.y) + string.Format("playerY(before)={0:F2} customAction(before)={1}", transform.position.y, (this._cc != null) ? this._cc.customAction.ToString() : "n/a"));
			}
			if (this.animator != null)
			{
				this.animator.CrossFadeInFixedTime("Swimming.Swimming", 0.1f, 0);
			}
			if (this._cc != null)
			{
				this._cc.customAction = false;
				this._cc.verticalVelocity = 0f;
			}
			if (this._swimming != null)
			{
				float num = collider.transform.position.y - this._swimming.heightOffset;
				if (this._cc != null)
				{
					num = collider.transform.position.y - (this._cc.colliderHeightDefault * 0.5f + this._swimming.heightOffset);
				}
				Vector3 position = transform.position;
				if (position.y > num)
				{
					position.y = num;
					transform.position = position;
				}
				if (this._cc != null && this._cc._rigidbody != null)
				{
					this._cc._rigidbody.position = transform.position;
					if (!this._cc._rigidbody.isKinematic)
					{
						this._cc._rigidbody.velocity = Vector3.zero;
					}
				}
			}
			this._swimming.OnActionEnter(collider);
			float num2 = 0.35f;
			float num3 = collider.transform.position.y - ((this._cc != null) ? (this._cc.colliderHeightDefault * 0.5f + this._swimming.heightOffset) : this._swimming.heightOffset) - num2;
			this._swimHandoffPending = true;
			base.StartCoroutine(this.LockSwimUntilStable(transform, collider, num3));
			return true;
		}

		// Token: 0x060068D8 RID: 26840 RVA: 0x0029F235 File Offset: 0x0029D435
		private IEnumerator LockSwimUntilStable(Transform playerRoot, Collider water, float surfaceTargetY)
		{
			int stable = 0;
			if (this.diveDiagLogs)
			{
				Debug.Log(string.Format("[PoolDive][DIAG] LockSwimUntilStable START: swimming={0} cc={1} ", this._swimming != null, this._cc != null) + string.Format("rb={0} surfaceTargetY={1:F2}", this._cc != null && this._cc._rigidbody != null, surfaceTargetY));
			}
			int num;
			for (int i = 0; i < 40; i = num + 1)
			{
				if (this._swimming == null || this._cc == null || this._cc._rigidbody == null)
				{
					if (this.diveDiagLogs)
					{
						Debug.Log("[PoolDive][DIAG] lock bail: null ref");
					}
					this._swimHandoffPending = false;
					this.EndDiveView();
					yield break;
				}
				if (this._swimming.IsClimbingOutOfWater)
				{
					this._swimHandoffPending = false;
					this.EndDiveView();
					yield break;
				}
				if (water == null || !water.enabled || !water.gameObject.activeInHierarchy)
				{
					this.InterruptForEncounter();
					this._swimming.ForceExitSwimming();
					if (this._stateGuard == null)
					{
						this._stateGuard = playerRoot.GetComponentInChildren<PoolEntryStateGuard>(true);
					}
					PoolEntryStateGuard stateGuard = this._stateGuard;
					if (stateGuard != null)
					{
						stateGuard.RestoreForEncounter();
					}
					yield break;
				}
				if (!this._cc._rigidbody.isKinematic)
				{
					this._cc._rigidbody.velocity = Vector3.zero;
				}
				this._cc._rigidbody.isKinematic = true;
				this._cc._rigidbody.useGravity = false;
				Vector3 position = playerRoot.position;
				position.y = surfaceTargetY;
				playerRoot.position = position;
				this._cc._rigidbody.position = position;
				this._cc.customAction = false;
				if (water != null)
				{
					this._swimming.water = water.gameObject;
				}
				this._swimming.inTheWater = true;
				bool flag = this.IsAnimatorInDive();
				stable = ((!flag && !this._cc.customAction && this._swimming.isSwimming && this._swimming.currentCharacterDepth >= 0f) ? (stable + 1) : 0);
				if (this.diveDiagLogs)
				{
					Debug.Log(string.Format("[PoolDive][DIAG] lock f{0}: playerY={1:F2} inDive={2} ", i, playerRoot.position.y, flag) + string.Format("customAction={0} isSwimming={1} inTheWater={2} ", this._cc.customAction, this._swimming.isSwimming, this._swimming.inTheWater) + string.Format("depth={0:F2} grounded={1} stable={2}", this._swimming.currentCharacterDepth, this._cc.isGrounded, stable));
				}
				if (stable >= 3)
				{
					break;
				}
				yield return new WaitForFixedUpdate();
				num = i;
			}
			if (this._cc != null && this._cc._rigidbody != null)
			{
				if (this.diveDiagLogs)
				{
					Debug.Log(string.Format("[PoolDive][DIAG] RELEASE: stable={0} playerY={1:F2} ", stable, playerRoot.position.y) + string.Format("isSwimming={0} depth={1:F2} → kinematic={2}", this._swimming.isSwimming, this._swimming.currentCharacterDepth, this._rbWasKinematic));
				}
				this._cc._rigidbody.isKinematic = this._rbWasKinematic;
				if (!this._cc._rigidbody.isKinematic)
				{
					this._cc._rigidbody.velocity = Vector3.zero;
				}
			}
			this._swimHandoffPending = false;
			this.EndDiveView();
			for (int i = 0; i < 8; i = num + 1)
			{
				yield return null;
				if (this._swimming == null || this._cc == null)
				{
					yield break;
				}
				if (this.diveDiagLogs)
				{
					Debug.Log(string.Format("[PoolDive][DIAG] post-release f{0}: playerY={1:F2} ", i, playerRoot.position.y) + string.Format("isSwimming={0} inTheWater={1} ", this._swimming.isSwimming, this._swimming.inTheWater) + string.Format("depth={0:F2} useGravity={1} ", this._swimming.currentCharacterDepth, this._cc._rigidbody.useGravity) + string.Format("kinematic={0}", this._cc._rigidbody.isKinematic));
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060068D9 RID: 26841 RVA: 0x0029F25C File Offset: 0x0029D45C
		private bool IsAnimatorInDive()
		{
			return !(this.animator == null) && (this.animator.GetCurrentAnimatorStateInfo(0).IsName(this.diveStateName) || (this.animator.IsInTransition(0) && this.animator.GetNextAnimatorStateInfo(0).IsName(this.diveStateName)));
		}

		// Token: 0x060068DA RID: 26842 RVA: 0x0029F2C4 File Offset: 0x0029D4C4
		private float BodyCenterY(Transform playerRoot)
		{
			return playerRoot.position.y + 0.9f;
		}

		// Token: 0x060068DB RID: 26843 RVA: 0x0029F2D8 File Offset: 0x0029D4D8
		private Collider FindOverlappingWater(Vector3 near)
		{
			foreach (Collider collider in Physics.OverlapSphere(near + Vector3.up, this.waterProbeRadius, -1, QueryTriggerInteraction.Collide))
			{
				if (collider != null && collider.CompareTag(this.waterTag))
				{
					return collider;
				}
			}
			return null;
		}

		// Token: 0x060068DC RID: 26844 RVA: 0x0029F32C File Offset: 0x0029D52C
		private void PlaySplash(Transform playerRoot, float waterlineY)
		{
			Vector3 vector = new Vector3(playerRoot.position.x, waterlineY, playerRoot.position.z);
			if (this._swimming != null && this._swimming.impactEffect != null)
			{
				Object.Instantiate<GameObject>(this._swimming.impactEffect, vector, playerRoot.rotation);
			}
		}

		// Token: 0x060068DD RID: 26845 RVA: 0x0029F390 File Offset: 0x0029D590
		private float ResolveDiveDuration()
		{
			if (this.animator.runtimeAnimatorController != null)
			{
				foreach (AnimationClip animationClip in this.animator.runtimeAnimatorController.animationClips)
				{
					if (animationClip != null && animationClip.name.Contains("dive"))
					{
						return animationClip.length;
					}
				}
			}
			return this.fallbackDiveDuration;
		}

		// Token: 0x060068DE RID: 26846 RVA: 0x0029F3FC File Offset: 0x0029D5FC
		private void ApplyEmbSupreme()
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.EnableDecay = false;
			instance.SetBalance(-100f, "PoolDiveStrip");
		}

		// Token: 0x060068DF RID: 26847 RVA: 0x0029F430 File Offset: 0x0029D630
		public void ResetForHomeReturn()
		{
			this.OnDisable();
			this._cooldownUntil = 0f;
		}

		// Token: 0x060068E0 RID: 26848 RVA: 0x0029F444 File Offset: 0x0029D644
		private void OnDisable()
		{
			base.StopAllCoroutines();
			this.EndDiveView();
			this._activeDiveZones.Clear();
			this.playerInZone = false;
			if (this._emotionSuppressed)
			{
				PlayerEmotionController instance = PlayerEmotionController.Instance;
				if (instance != null)
				{
					instance.RestoreEmotionLayers();
				}
				this._emotionSuppressed = false;
			}
			if ((this._diving || this._swimHandoffPending) && this._cc != null && this._cc._rigidbody != null)
			{
				this._cc._rigidbody.isKinematic = this._rbWasKinematic;
			}
			this._swimHandoffPending = false;
			this._diving = false;
			this.ReleaseDiveContext();
		}

		// Token: 0x04006B54 RID: 27476
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04006B55 RID: 27477
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04006B56 RID: 27478
		[Tooltip("Optional Gold Bikini slip controller. If present, the strip-dive (G) ALSO drops the two-piece bikini (each piece only if worn). Coexists with the school swimsuit slip.")]
		[SerializeField]
		private ClothSlipController bikiniSlipController;

		// Token: 0x04006B57 RID: 27479
		[Tooltip("Optional Gold Bikini blendShape slip-dive controller. If present, the strip-dive (G) rolls a PROBABILISTIC wardrobe slip (nipple/pussy escapes from a random side) on the worn bikini INSTEAD of dropping the whole piece — re-adjusted with P. Auto-found.")]
		[SerializeField]
		private BikiniSlipDiveController bikiniSlipDiveController;

		// Token: 0x04006B58 RID: 27480
		[Tooltip("Set by the DiveZone trigger; true while the player can dive.")]
		[SerializeField]
		private bool playerInZone;

		// Token: 0x04006B59 RID: 27481
		[Header("Animator")]
		[SerializeField]
		private string diveStateName = "PoolDive";

		// Token: 0x04006B5A RID: 27482
		[SerializeField]
		private float diveCrossfade = 0.1f;

		// Token: 0x04006B5B RID: 27483
		[Tooltip("Fallback dive length used for slip timing if the clip can't be read.")]
		[SerializeField]
		private float fallbackDiveDuration = 6.5f;

		// Token: 0x04006B5C RID: 27484
		[Tooltip("Seconds after the dive CrossFade begins at which the swimsuit slip (G variant) is fired. The slip happens at the START of the dive (when Ciel loses the suit), and the slip prefab is parented to the body so it follows the dive arc. Keep small (~0) so the slip and the dive are in sync; a tiny delay lets the dive pose blend in first.")]
		[SerializeField]
		private float stripDelaySeconds;

		// Token: 0x04006B5D RID: 27485
		[Tooltip("Normalized time into the dive clip at which we hand off to swimming. The PoolDive state's exit transition starts at 0.9 (and blends toward the walk/idle pose over 0.25), so we must hand off BEFORE 0.9 to never flash that pose. Default 0.85.")]
		[SerializeField]
		[Range(0.5f, 0.9f)]
		private float swimHandoffNormalizedTime = 0.85f;

		// Token: 0x04006B5E RID: 27486
		[Tooltip("Normalized time into the dive clip at which Ciel goes to EMB Supreme (EmotionBalance -100) — i.e. the moment she actually loses the swimsuit, roughly mid-dive. Only the G (strip) variant. The swimsuit-slip prefab itself still spawns at the start of the dive; this only delays the emotion/face.")]
		[SerializeField]
		[Range(0f, 0.9f)]
		private float embSupremeNormalizedTime = 0.5f;

		// Token: 0x04006B5F RID: 27487
		[Header("Water handoff")]
		[Tooltip("Tag on the swim trigger volume (matches vSwimming.waterTag).")]
		[SerializeField]
		private string waterTag = "Water";

		// Token: 0x04006B60 RID: 27488
		[Tooltip("Radius used to find the Water volume the player is overlapping.")]
		[SerializeField]
		private float waterProbeRadius = 1.2f;

		// Token: 0x04006B61 RID: 27489
		[Tooltip("Waterline Y used for the splash + submersion check if no Water volume is found. POI-Pool waterline is -5.66.")]
		[SerializeField]
		private float fallbackWaterlineY = -5.66f;

		// Token: 0x04006B62 RID: 27490
		[Tooltip("Safety-net tolerance: if the dive clip finished without the body crossing the waterline, still engage swimming if she's within this distance of it.")]
		[SerializeField]
		private float submergeDepth = 0.25f;

		// Token: 0x04006B63 RID: 27491
		[Header("Splash audio")]
		[Tooltip("Optional splash sound played the instant her body crosses the waterline.")]
		[SerializeField]
		private AudioClip splashSound;

		// Token: 0x04006B64 RID: 27492
		[SerializeField]
		[Range(0f, 1f)]
		private float splashVolume = 1f;

		// Token: 0x04006B65 RID: 27493
		[Tooltip("Optional AudioSource. If null one is added at runtime.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04006B66 RID: 27494
		[Header("Input")]
		[SerializeField]
		private GenericInput diveInput = new GenericInput("F", "A", "F");

		// Token: 0x04006B67 RID: 27495
		[SerializeField]
		private GenericInput diveStripInput = new GenericInput("G", "B", "G");

		// Token: 0x04006B68 RID: 27496
		[SerializeField]
		private float diveCooldown = 1f;

		// Token: 0x04006B69 RID: 27497
		[Header("Dive direction / clearance")]
		[Tooltip("Il tuffo è consentito solo se il player guarda VERSO l'acqua. Coseno minimo tra il forward orizzontale del player e la direzione player→acqua. 0.3 ≈ entro ~72° dall'acqua. Alza per richiedere di guardare più dritto verso la piscina.")]
		[SerializeField]
		[Range(-1f, 1f)]
		private float minFacingWaterDot = 0.3f;

		// Token: 0x04006B6A RID: 27498
		[Tooltip("Spazio orizzontale (m) che serve davanti per completare il tuffo senza trapassare una parete. Un raycast in avanti che colpisce un solido entro questa distanza blocca il tuffo.")]
		[SerializeField]
		private float diveClearanceDistance = 1.6f;

		// Token: 0x04006B6B RID: 27499
		[Tooltip("Layer considerati 'muro/ostacolo' per il check di spazio. Lascia Default; escludi water/player.")]
		[SerializeField]
		private LayerMask diveClearanceMask = -1;

		// Token: 0x04006B6C RID: 27500
		[Tooltip("Log diagnostici sul perché il tuffo è consentito o negato (attivare per il tuning di minFacingWaterDot / diveClearanceDistance).")]
		[SerializeField]
		private bool diveDiagLogs;

		// Token: 0x04006B6D RID: 27501
		private bool _diving;

		// Token: 0x04006B6E RID: 27502
		private float _cooldownUntil;

		// Token: 0x04006B6F RID: 27503
		private bool _emotionSuppressed;

		// Token: 0x04006B70 RID: 27504
		private PlayerActivityLease _diveLease;

		// Token: 0x04006B71 RID: 27505
		private readonly HashSet<DiveZone> _activeDiveZones = new HashSet<DiveZone>();

		// Token: 0x04006B72 RID: 27506
		private const PlayerContextResource DiveResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04006B73 RID: 27507
		private bool _rbWasKinematic;

		// Token: 0x04006B74 RID: 27508
		private bool _swimHandoffPending;

		// Token: 0x04006B75 RID: 27509
		private vSwimming _swimming;

		// Token: 0x04006B76 RID: 27510
		private vThirdPersonController _cc;

		// Token: 0x04006B77 RID: 27511
		private PoolEntryStateGuard _stateGuard;

		// Token: 0x04006B78 RID: 27512
		private FirstPersonController _firstPerson;
	}
}
