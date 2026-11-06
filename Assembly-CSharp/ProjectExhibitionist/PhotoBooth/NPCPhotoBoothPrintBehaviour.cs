using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.WantedPoster;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000966 RID: 2406
	[DisallowMultipleComponent]
	public sealed class NPCPhotoBoothPrintBehaviour : MonoBehaviour
	{
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060041D7 RID: 16855 RVA: 0x0018754D File Offset: 0x0018574D
		public bool IsBusy
		{
			get
			{
				return this._busy;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x00187555 File Offset: 0x00185755
		// (set) Token: 0x060041D9 RID: 16857 RVA: 0x0018755D File Offset: 0x0018575D
		public bool IsPhotographingPrint { get; private set; }

		// Token: 0x060041DA RID: 16858 RVA: 0x00187568 File Offset: 0x00185768
		public bool Begin(PhotoBoothPrint print, bool takePhoto, bool takePrint, PhotoBoothPrintNpcDirector director, NPCPhotoBoothPrintBehaviour.SequenceSettings settings)
		{
			if (this._busy || print == null || director == null)
			{
				return false;
			}
			if (!this.CacheAndValidateNpc())
			{
				return false;
			}
			if (!print.TryReserveForNpc(this))
			{
				return false;
			}
			this._print = print;
			this._director = director;
			this._settings = settings;
			this._takePhoto = takePhoto && this.FindPhotoReaction() != null;
			this._takePrint = takePrint;
			if (!this._takePhoto && !this._takePrint)
			{
				this._takePrint = true;
			}
			this._busy = true;
			this._finishing = false;
			this._wander.ExternalControl = true;
			this._wander.ForceStop();
			this._routine = base.StartCoroutine(this.RunSequence());
			return true;
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x0018762C File Offset: 0x0018582C
		private bool CacheAndValidateNpc()
		{
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._wander = base.GetComponent<NPCWanderController>();
			this._controller = base.GetComponent<AStarNPCController>();
			this._senses = base.GetComponent<NPCSenses>();
			this._equipment = base.GetComponent<NPCEquipmentManager>();
			this._lookAt = base.GetComponent<NPCSimpleLookAt>();
			this._animator = base.GetComponentInChildren<Animator>();
			NPCWantedPosterBehaviour component = base.GetComponent<NPCWantedPosterBehaviour>();
			return this._fsm != null && this._fsm.IsInitialized && (this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering) && this._wander != null && this._wander.isActiveAndEnabled && !this._wander.ExternalControl && this._controller != null && this._controller.isActiveAndEnabled && !this._controller.SuppressPOIBehavior && this._animator != null && this._animator.isHuman && (component == null || !component.IsBusy) && (this._senses == null || (!this._senses.IsAlert && !this._senses.IsDetected));
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x0018778A File Offset: 0x0018598A
		private IEnumerator RunSequence()
		{
			float navigationReadyDeadline = Time.time + 2f;
			while (!this._controller.IsNavigationReady && Time.time < navigationReadyDeadline)
			{
				if (!this.CanContinue(true))
				{
					this.Finish();
					yield break;
				}
				yield return null;
			}
			if (!this.CanContinue(true))
			{
				this.Finish();
				yield break;
			}
			this._controller.UnlockNavigation(false);
			this._controller.SetDestination(this.ResolveApproachPosition());
			float deadline = Time.time + this._settings.ApproachTimeout;
			Vector3 lastPrintPosition = this._print.NpcInterestPoint;
			while (Time.time < deadline)
			{
				if (!this.CanContinue(true))
				{
					this.Finish();
					yield break;
				}
				Vector3 vector = this._print.NpcInterestPoint - base.transform.position;
				vector.y = 0f;
				if (vector.magnitude <= this._settings.ApproachDistance || this._controller.HasReachedDestination())
				{
					break;
				}
				if ((this._print.NpcInterestPoint - lastPrintPosition).sqrMagnitude > 0.04f)
				{
					this._controller.SetDestination(this.ResolveApproachPosition());
					lastPrintPosition = this._print.NpcInterestPoint;
				}
				yield return null;
			}
			if (!this.CanContinue(true))
			{
				this.Finish();
				yield break;
			}
			Vector3 vector2 = this._print.NpcInterestPoint - base.transform.position;
			vector2.y = 0f;
			if (vector2.magnitude > this._settings.ApproachDistance * 1.75f)
			{
				this.Log("Approccio scaduto prima di raggiungere la foto.");
				this.Finish();
				yield break;
			}
			this._controller.StopAndLock();
			yield return this.RotateTowardPrint(0.45f);
			this.BeginLookingAt(this._print.NpcInterestPoint);
			if (!this.CanPlayerObserveTarget())
			{
				this.Log("Sequenza annullata: player non piu' vicino o senza visuale.");
				this.Finish();
				yield break;
			}
			if (this._takePhoto)
			{
				yield return this.PlayPhotoReaction();
				if (!this.CanContinue(true))
				{
					this.Finish();
					yield break;
				}
			}
			if (this._takePrint)
			{
				if (!this._takePhoto)
				{
					float inspectUntil = Time.time + 0.8f;
					while (Time.time < inspectUntil)
					{
						if (!this.CanContinue(true))
						{
							this.Finish();
							yield break;
						}
						yield return null;
					}
				}
				yield return this.PickUpAndLeave();
				if (!this._busy)
				{
					yield break;
				}
			}
			this.Finish();
			yield break;
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x00187799 File Offset: 0x00185999
		private IEnumerator PlayPhotoReaction()
		{
			NPCReactionEntry npcreactionEntry = this.FindPhotoReaction();
			int num = ((npcreactionEntry != null && npcreactionEntry.AnimatorReactionIndex > 0) ? npcreactionEntry.AnimatorReactionIndex : 1);
			string text = ((npcreactionEntry != null && !string.IsNullOrEmpty(npcreactionEntry.AnimationTrigger)) ? npcreactionEntry.AnimationTrigger : "ReactionTrigger");
			float num2 = ((npcreactionEntry != null && npcreactionEntry.ReactionDuration > 0f) ? npcreactionEntry.ReactionDuration : 2.2f);
			this._activatedPhotoProps.Clear();
			if (this._equipment != null && ((npcreactionEntry != null) ? npcreactionEntry.PropsToActivate : null) != null)
			{
				foreach (string text2 in npcreactionEntry.PropsToActivate)
				{
					if (!string.IsNullOrWhiteSpace(text2))
					{
						this._equipment.ActivateAndLockProp(text2);
						this._activatedPhotoProps.Add(text2);
					}
				}
			}
			this.IsPhotographingPrint = true;
			if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.ReactionTypeHash, AnimatorControllerParameterType.Int))
			{
				this._animator.SetInteger(NPCPhotoBoothPrintBehaviour.ReactionTypeHash, num);
			}
			int num3 = Animator.StringToHash(text);
			if (!this.HasAnimatorParameter(num3, AnimatorControllerParameterType.Trigger))
			{
				num3 = NPCPhotoBoothPrintBehaviour.ReactionTriggerHash;
			}
			if (this.HasAnimatorParameter(num3, AnimatorControllerParameterType.Trigger))
			{
				this._animator.ResetTrigger(num3);
				this._animator.SetTrigger(num3);
			}
			float endTime = Time.time + Mathf.Max(0.5f, num2);
			while (Time.time < endTime)
			{
				if (!this.CanContinue(true))
				{
					yield break;
				}
				this.FacePrintImmediate();
				yield return null;
			}
			this.IsPhotographingPrint = false;
			if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.ReactionTypeHash, AnimatorControllerParameterType.Int))
			{
				this._animator.SetInteger(NPCPhotoBoothPrintBehaviour.ReactionTypeHash, 0);
			}
			this.DeactivatePhotoProps();
			yield break;
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x001877A8 File Offset: 0x001859A8
		private IEnumerator PickUpAndLeave()
		{
			if (!this.CanPlayerObserveTarget())
			{
				yield break;
			}
			Vector3 printPosition = this._print.NpcInterestPoint;
			PhotoBoothPrint print = this._print;
			if (!print.ConsumeByNpc(this))
			{
				yield break;
			}
			this._heldPrint = print;
			this._print = null;
			if (this.ConfigureCarryPose())
			{
				float poseReadyTime = Time.time + 0.2f;
				while (Time.time < poseReadyTime)
				{
					if (!this.CanContinue(false))
					{
						this.Finish();
						yield break;
					}
					yield return null;
				}
			}
			Transform transform = this._animator.GetBoneTransform(HumanBodyBones.RightHand);
			if (transform == null)
			{
				transform = base.transform;
				this.Log("RightHand non disponibile: uso il root NPC come fallback.");
			}
			this._heldPrint.AttachToHand(transform);
			Transform heldTransform = this._heldPrint.PivotTransform;
			Vector3 startPosition = heldTransform.localPosition;
			Quaternion startRotation = heldTransform.localRotation;
			Vector3 startScale = heldTransform.localScale;
			Vector3 targetPosition = this._settings.CarryLocalPosition;
			Quaternion targetRotation = Quaternion.Euler(this._settings.CarryLocalEulerAngles);
			Vector3 targetScale = this._settings.CarryLocalScale;
			float duration = 0.45f;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				if (!this.CanContinue(false))
				{
					this.Finish();
					yield break;
				}
				elapsed += Time.deltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
				heldTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, num);
				heldTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, num);
				heldTransform.localScale = Vector3.Lerp(startScale, targetScale, num);
				yield return null;
			}
			heldTransform.localPosition = targetPosition;
			heldTransform.localRotation = targetRotation;
			heldTransform.localScale = targetScale;
			GameObject gameObject = new GameObject("PhotoBoothPrintGazeTarget");
			this._gazeTarget = gameObject.transform;
			this._gazeTarget.SetParent(heldTransform, false);
			this._gazeTarget.localPosition = Vector3.zero;
			if (this._lookAt != null)
			{
				this._lookAt.StartLookingAt(this._gazeTarget);
			}
			this._controller.UnlockNavigation(false);
			Vector3 away = base.transform.position - printPosition;
			away.y = 0f;
			if (away.sqrMagnitude < 0.01f)
			{
				away = -base.transform.forward;
			}
			away = Quaternion.Euler(0f, Random.Range(-30f, 30f), 0f) * away.normalized;
			float distance = Random.Range(this._settings.WalkAwayDistance.x, this._settings.WalkAwayDistance.y);
			this._controller.SetDestination(base.transform.position + away * distance);
			float carryUntil = Time.time + Random.Range(this._settings.CarryDuration.x, this._settings.CarryDuration.y);
			bool extendedDestination = false;
			while (Time.time < carryUntil)
			{
				if (!this.CanContinue(false))
				{
					this.Finish();
					yield break;
				}
				if (!extendedDestination && this._controller.HasReachedDestination() && Time.time < carryUntil - 2f)
				{
					extendedDestination = true;
					this._controller.SetDestination(base.transform.position + away * (distance * 0.6f));
				}
				yield return null;
			}
			if (this._heldPrint != null)
			{
				Vector3 fullScale = this._heldPrint.PivotTransform.localScale;
				float poseReadyTime = Time.time + 0.25f;
				while (Time.time < poseReadyTime && this.CanContinue(false))
				{
					float num2 = 1f - (poseReadyTime - Time.time) / 0.25f;
					this._heldPrint.PivotTransform.localScale = Vector3.Lerp(fullScale, Vector3.zero, num2);
					yield return null;
				}
				fullScale = default(Vector3);
			}
			yield break;
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x001877B8 File Offset: 0x001859B8
		private bool ConfigureCarryPose()
		{
			this._armsLayerIndex = this._animator.GetLayerIndex("Arms");
			if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.PrintCarryHash, AnimatorControllerParameterType.Bool) && this._armsLayerIndex >= 0)
			{
				this._previousArmsLayerWeight = this._animator.GetLayerWeight(this._armsLayerIndex);
				this._animator.SetLayerWeight(this._armsLayerIndex, 1f);
				if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.UpperBodyActionHash, AnimatorControllerParameterType.Int))
				{
					this._animator.SetInteger(NPCPhotoBoothPrintBehaviour.UpperBodyActionHash, 0);
				}
				if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.CarryRaiseHash, AnimatorControllerParameterType.Float))
				{
					this._animator.SetFloat(NPCPhotoBoothPrintBehaviour.CarryRaiseHash, Mathf.Clamp01(this._settings.CarryArmRaise));
				}
				this._animator.SetBool(NPCPhotoBoothPrintBehaviour.PrintCarryHash, true);
				return true;
			}
			if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.UpperBodyActionHash, AnimatorControllerParameterType.Int))
			{
				this._animator.SetInteger(NPCPhotoBoothPrintBehaviour.UpperBodyActionHash, 8);
			}
			return false;
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x001878A8 File Offset: 0x00185AA8
		private bool CanPlayerObserveTarget()
		{
			Camera camera;
			Transform transform;
			if (this._print == null || !NpcInterestPlayerVisibility.TryResolveContext(out camera, out transform))
			{
				return false;
			}
			Vector3 vector = base.transform.position + Vector3.up * 1.55f;
			if (this._animator != null && this._animator.isHuman)
			{
				Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Head);
				if (boneTransform != null)
				{
					vector = boneTransform.position;
				}
			}
			return NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, this._print.NpcInterestPoint, this._print.PivotTransform, this._settings.MaximumPlayerObservationDistance, this._settings.PlayerViewportPadding, this._settings.PlayerObstructionMask) && NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, vector, base.transform, this._settings.MaximumPlayerObservationDistance, this._settings.PlayerViewportPadding, this._settings.PlayerObstructionMask);
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x0018799C File Offset: 0x00185B9C
		private Vector3 ResolveApproachPosition()
		{
			Vector3 npcInterestPoint = this._print.NpcInterestPoint;
			Vector3 vector = Vector3.ProjectOnPlane(this._print.PivotTransform.up, Vector3.up);
			Vector3 vector2 = base.transform.position - npcInterestPoint;
			vector2.y = 0f;
			if (vector.sqrMagnitude < 0.01f)
			{
				vector = vector2;
			}
			if (vector.sqrMagnitude < 0.01f)
			{
				vector = -base.transform.forward;
			}
			vector.Normalize();
			if (vector2.sqrMagnitude > 0.01f && Vector3.Dot(vector, vector2) < 0f)
			{
				vector = -vector;
			}
			Vector3 vector3 = npcInterestPoint + vector * this._settings.ApproachDistance;
			vector3.y = base.transform.position.y;
			return vector3;
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x00187A77 File Offset: 0x00185C77
		private IEnumerator RotateTowardPrint(float maxDuration)
		{
			float endTime = Time.time + maxDuration;
			while (Time.time < endTime && this.CanContinue(true))
			{
				Vector3 vector = this._print.NpcInterestPoint - base.transform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude < 0.001f)
				{
					yield break;
				}
				Quaternion quaternion = Quaternion.LookRotation(vector.normalized, Vector3.up);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, 360f * Time.deltaTime);
				if (Quaternion.Angle(base.transform.rotation, quaternion) < 2f)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x00187A90 File Offset: 0x00185C90
		private void FacePrintImmediate()
		{
			if (this._print == null)
			{
				return;
			}
			Vector3 vector = this._print.NpcInterestPoint - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				return;
			}
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector.normalized, Vector3.up), 240f * Time.deltaTime);
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x00187B1C File Offset: 0x00185D1C
		private void BeginLookingAt(Vector3 point)
		{
			if (this._lookAt == null)
			{
				return;
			}
			if (!this._capturedLookAtSetting)
			{
				this._lookAtHorizontalOnly = this._lookAt.horizontalOnly;
				this._capturedLookAtSetting = true;
			}
			this._lookAt.horizontalOnly = false;
			this._lookAt.StartLookingAt(point);
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x00187B70 File Offset: 0x00185D70
		private NPCReactionEntry FindPhotoReaction()
		{
			NPCSenses senses = this._senses;
			bool flag;
			if (senses == null)
			{
				flag = null != null;
			}
			else
			{
				NPCReactionProfileSO reactionProfile = senses.ReactionProfile;
				flag = ((reactionProfile != null) ? reactionProfile.Reactions : null) != null;
			}
			if (!flag)
			{
				return null;
			}
			foreach (NPCReactionEntry npcreactionEntry in this._senses.ReactionProfile.Reactions)
			{
				if (npcreactionEntry != null && npcreactionEntry.Type == ReactionType.TakePhoto && npcreactionEntry.AnimatorReactionIndex == 1 && npcreactionEntry.PropsToActivate != null)
				{
					foreach (string text in npcreactionEntry.PropsToActivate)
					{
						if (!string.IsNullOrEmpty(text) && text.IndexOf("phone", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							return npcreactionEntry;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x00187C60 File Offset: 0x00185E60
		private bool CanContinue(bool requirePrint)
		{
			if (!this._busy || !base.isActiveAndEnabled || this._fsm == null || !this._fsm.IsInitialized || this._controller == null || !this._controller.isActiveAndEnabled || this._wander == null || !this._wander.isActiveAndEnabled)
			{
				return false;
			}
			NPCStateType currentStateType = this._fsm.CurrentStateType;
			return (currentStateType == NPCStateType.Idle || currentStateType == NPCStateType.Wandering || currentStateType == NPCStateType.LookingAt) && (!(this._senses != null) || (!this._senses.IsAlert && !this._senses.IsDetected)) && (!requirePrint || (this._print != null && this._print.IsNpcWallInterestEligible && this._print.IsReservedForNpc));
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x00187D48 File Offset: 0x00185F48
		private bool HasAnimatorParameter(int hash, AnimatorControllerParameterType type)
		{
			if (this._animator == null)
			{
				return false;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
			{
				if (animatorControllerParameter.nameHash == hash && animatorControllerParameter.type == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060041E8 RID: 16872 RVA: 0x00187D98 File Offset: 0x00185F98
		private void DeactivatePhotoProps()
		{
			if (this._equipment != null)
			{
				foreach (string text in this._activatedPhotoProps)
				{
					this._equipment.DeactivateProp(text);
				}
			}
			this._activatedPhotoProps.Clear();
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x00187E0C File Offset: 0x0018600C
		private void Finish()
		{
			if (!this._busy || this._finishing)
			{
				return;
			}
			this._finishing = true;
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
				this._routine = null;
			}
			this.IsPhotographingPrint = false;
			if (this._animator != null)
			{
				if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.ReactionTypeHash, AnimatorControllerParameterType.Int))
				{
					this._animator.SetInteger(NPCPhotoBoothPrintBehaviour.ReactionTypeHash, 0);
				}
				if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.UpperBodyActionHash, AnimatorControllerParameterType.Int))
				{
					this._animator.SetInteger(NPCPhotoBoothPrintBehaviour.UpperBodyActionHash, 0);
				}
				if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.PrintCarryHash, AnimatorControllerParameterType.Bool))
				{
					this._animator.SetBool(NPCPhotoBoothPrintBehaviour.PrintCarryHash, false);
				}
				if (this.HasAnimatorParameter(NPCPhotoBoothPrintBehaviour.CarryRaiseHash, AnimatorControllerParameterType.Float))
				{
					this._animator.SetFloat(NPCPhotoBoothPrintBehaviour.CarryRaiseHash, 0f);
				}
			}
			this.DeactivatePhotoProps();
			if (this._lookAt != null)
			{
				this._lookAt.StopLooking();
				if (this._capturedLookAtSetting)
				{
					this._lookAt.horizontalOnly = this._lookAtHorizontalOnly;
				}
			}
			this._capturedLookAtSetting = false;
			if (this._heldPrint != null)
			{
				Transform pivotTransform = this._heldPrint.PivotTransform;
				this._heldPrint = null;
				if (pivotTransform != null)
				{
					Object.Destroy(pivotTransform.gameObject);
				}
			}
			this._gazeTarget = null;
			if (this._animator != null && this._armsLayerIndex >= 0)
			{
				this._animator.SetLayerWeight(this._armsLayerIndex, this._previousArmsLayerWeight);
			}
			this._armsLayerIndex = -1;
			if (this._print != null)
			{
				this._print.ReleaseNpcReservation(this);
			}
			this._print = null;
			if (this._wander != null)
			{
				this._wander.ExternalControl = false;
			}
			bool flag = this._fsm == null || !this._fsm.IsInitialized || this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering || this._fsm.CurrentStateType == NPCStateType.LookingAt;
			if (flag && this._controller != null)
			{
				this._controller.UnlockNavigation(false);
				this._controller.ResetToDefault();
			}
			if (flag && this._wander != null)
			{
				this._wander.ResumeWandering();
			}
			PhotoBoothPrintNpcDirector director = this._director;
			this._director = null;
			this._busy = false;
			this._finishing = false;
			if (director == null)
			{
				return;
			}
			director.NotifySequenceFinished(this);
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x00188083 File Offset: 0x00186283
		private void OnDisable()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00188083 File Offset: 0x00186283
		private void OnDestroy()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x00188093 File Offset: 0x00186293
		private void Log(string message)
		{
			if (this._settings.DebugLogs)
			{
				Debug.Log("[PhotoBoothPrintNPC] " + base.name + ": " + message);
			}
		}

		// Token: 0x040041BF RID: 16831
		private static readonly int ReactionTypeHash = Animator.StringToHash("ReactionType");

		// Token: 0x040041C0 RID: 16832
		private static readonly int ReactionTriggerHash = Animator.StringToHash("ReactionTrigger");

		// Token: 0x040041C1 RID: 16833
		private static readonly int UpperBodyActionHash = Animator.StringToHash("UpperBodyAction");

		// Token: 0x040041C2 RID: 16834
		private static readonly int PrintCarryHash = Animator.StringToHash("PhotoBoothPrintCarry");

		// Token: 0x040041C3 RID: 16835
		private static readonly int CarryRaiseHash = Animator.StringToHash("WantedPosterCarryRaise");

		// Token: 0x040041C4 RID: 16836
		private const int CarryingBoxFrontAction = 8;

		// Token: 0x040041C5 RID: 16837
		private NPCStateMachine _fsm;

		// Token: 0x040041C6 RID: 16838
		private NPCWanderController _wander;

		// Token: 0x040041C7 RID: 16839
		private AStarNPCController _controller;

		// Token: 0x040041C8 RID: 16840
		private NPCSenses _senses;

		// Token: 0x040041C9 RID: 16841
		private NPCEquipmentManager _equipment;

		// Token: 0x040041CA RID: 16842
		private NPCSimpleLookAt _lookAt;

		// Token: 0x040041CB RID: 16843
		private Animator _animator;

		// Token: 0x040041CC RID: 16844
		private PhotoBoothPrint _print;

		// Token: 0x040041CD RID: 16845
		private PhotoBoothPrint _heldPrint;

		// Token: 0x040041CE RID: 16846
		private PhotoBoothPrintNpcDirector _director;

		// Token: 0x040041CF RID: 16847
		private NPCPhotoBoothPrintBehaviour.SequenceSettings _settings;

		// Token: 0x040041D0 RID: 16848
		private Coroutine _routine;

		// Token: 0x040041D1 RID: 16849
		private Transform _gazeTarget;

		// Token: 0x040041D2 RID: 16850
		private readonly List<string> _activatedPhotoProps = new List<string>();

		// Token: 0x040041D3 RID: 16851
		private bool _takePhoto;

		// Token: 0x040041D4 RID: 16852
		private bool _takePrint;

		// Token: 0x040041D5 RID: 16853
		private bool _busy;

		// Token: 0x040041D6 RID: 16854
		private bool _finishing;

		// Token: 0x040041D7 RID: 16855
		private bool _lookAtHorizontalOnly;

		// Token: 0x040041D8 RID: 16856
		private bool _capturedLookAtSetting;

		// Token: 0x040041D9 RID: 16857
		private int _armsLayerIndex = -1;

		// Token: 0x040041DA RID: 16858
		private float _previousArmsLayerWeight;

		// Token: 0x02000967 RID: 2407
		public struct SequenceSettings
		{
			// Token: 0x040041DC RID: 16860
			public float ApproachDistance;

			// Token: 0x040041DD RID: 16861
			public float ApproachTimeout;

			// Token: 0x040041DE RID: 16862
			public Vector2 WalkAwayDistance;

			// Token: 0x040041DF RID: 16863
			public Vector2 CarryDuration;

			// Token: 0x040041E0 RID: 16864
			public Vector3 CarryLocalPosition;

			// Token: 0x040041E1 RID: 16865
			public Vector3 CarryLocalEulerAngles;

			// Token: 0x040041E2 RID: 16866
			public Vector3 CarryLocalScale;

			// Token: 0x040041E3 RID: 16867
			public float CarryArmRaise;

			// Token: 0x040041E4 RID: 16868
			public float MaximumPlayerObservationDistance;

			// Token: 0x040041E5 RID: 16869
			public float PlayerViewportPadding;

			// Token: 0x040041E6 RID: 16870
			public LayerMask PlayerObstructionMask;

			// Token: 0x040041E7 RID: 16871
			public bool DebugLogs;
		}
	}
}
