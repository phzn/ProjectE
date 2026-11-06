using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B64 RID: 2916
	[DisallowMultipleComponent]
	public sealed class NPCWantedPosterBehaviour : MonoBehaviour
	{
		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x060052CC RID: 21196 RVA: 0x00201FEC File Offset: 0x002001EC
		public bool IsBusy
		{
			get
			{
				return this._busy;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x060052CD RID: 21197 RVA: 0x00201FF4 File Offset: 0x002001F4
		// (set) Token: 0x060052CE RID: 21198 RVA: 0x00201FFC File Offset: 0x002001FC
		public bool IsPhotographingPoster { get; private set; }

		// Token: 0x060052CF RID: 21199 RVA: 0x00202008 File Offset: 0x00200208
		public bool Begin(WantedPoster poster, bool takePhoto, bool takePoster, WantedPosterNpcDirector director, NPCWantedPosterBehaviour.SequenceSettings settings)
		{
			if (this._busy || poster == null || director == null)
			{
				return false;
			}
			if (!this.CacheAndValidateNpc())
			{
				return false;
			}
			if (!poster.TryReserveForNpc(this))
			{
				return false;
			}
			this._poster = poster;
			this._director = director;
			this._settings = settings;
			this._takePhoto = takePhoto && this.FindPhotoReaction() != null;
			this._takePoster = takePoster;
			if (!this._takePhoto && !this._takePoster)
			{
				this._takePoster = true;
			}
			this._busy = true;
			this._finishing = false;
			this._wander.ExternalControl = true;
			this._wander.ForceStop();
			this._routine = base.StartCoroutine(this.RunSequence());
			return true;
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x002020CC File Offset: 0x002002CC
		private bool CacheAndValidateNpc()
		{
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._wander = base.GetComponent<NPCWanderController>();
			this._controller = base.GetComponent<AStarNPCController>();
			this._senses = base.GetComponent<NPCSenses>();
			this._equipment = base.GetComponent<NPCEquipmentManager>();
			this._lookAt = base.GetComponent<NPCSimpleLookAt>();
			this._animator = base.GetComponentInChildren<Animator>();
			NPCPhotoBoothPrintBehaviour component = base.GetComponent<NPCPhotoBoothPrintBehaviour>();
			return this._fsm != null && this._fsm.IsInitialized && (this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering) && this._wander != null && this._wander.isActiveAndEnabled && !this._wander.ExternalControl && this._controller != null && this._controller.isActiveAndEnabled && !this._controller.SuppressPOIBehavior && this._animator != null && this._animator.isHuman && (component == null || !component.IsBusy) && (this._senses == null || (!this._senses.IsAlert && !this._senses.IsDetected));
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x0020222A File Offset: 0x0020042A
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
			Vector3 vector = this.ResolveApproachPosition();
			this._controller.SetDestination(vector);
			float deadline = Time.time + this._settings.ApproachTimeout;
			Vector3 lastPosterPosition = this._poster.NpcInterestPoint;
			while (Time.time < deadline)
			{
				if (!this.CanContinue(true))
				{
					this.Finish();
					yield break;
				}
				Vector3 vector2 = this._poster.NpcInterestPoint - base.transform.position;
				vector2.y = 0f;
				if (vector2.magnitude <= this._settings.ApproachDistance || this._controller.HasReachedDestination())
				{
					break;
				}
				if ((this._poster.NpcInterestPoint - lastPosterPosition).sqrMagnitude > 0.04f)
				{
					vector = this.ResolveApproachPosition();
					this._controller.SetDestination(vector);
					lastPosterPosition = this._poster.NpcInterestPoint;
				}
				yield return null;
			}
			if (!this.CanContinue(true))
			{
				this.Finish();
				yield break;
			}
			Vector3 vector3 = this._poster.NpcInterestPoint - base.transform.position;
			vector3.y = 0f;
			if (vector3.magnitude > this._settings.ApproachDistance * 1.75f)
			{
				this.Log("Approccio scaduto prima di raggiungere il poster.");
				this.Finish();
				yield break;
			}
			this._controller.StopAndLock();
			yield return this.RotateTowardPoster(0.45f);
			this.BeginLookingAt(this._poster.NpcInterestPoint);
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
			if (this._takePoster)
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

		// Token: 0x060052D2 RID: 21202 RVA: 0x00202239 File Offset: 0x00200439
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
			this.IsPhotographingPoster = true;
			if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.ReactionTypeHash, AnimatorControllerParameterType.Int))
			{
				this._animator.SetInteger(NPCWantedPosterBehaviour.ReactionTypeHash, num);
			}
			int num3 = Animator.StringToHash(text);
			if (!this.HasAnimatorParameter(num3, AnimatorControllerParameterType.Trigger))
			{
				num3 = NPCWantedPosterBehaviour.ReactionTriggerHash;
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
				this.FacePosterImmediate();
				yield return null;
			}
			this.IsPhotographingPoster = false;
			if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.ReactionTypeHash, AnimatorControllerParameterType.Int))
			{
				this._animator.SetInteger(NPCWantedPosterBehaviour.ReactionTypeHash, 0);
			}
			this.DeactivatePhotoProps();
			yield break;
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x00202248 File Offset: 0x00200448
		private IEnumerator PickUpAndLeave()
		{
			if (!this.CanPlayerObserveTarget())
			{
				yield break;
			}
			Vector3 posterPosition = this._poster.NpcInterestPoint;
			this._heldPoster = this.CreateCarriedPosterVisual(this._poster);
			if (this._heldPoster == null || !this._poster.ConsumeByNpc(this))
			{
				if (this._heldPoster != null)
				{
					Object.Destroy(this._heldPoster);
				}
				this._heldPoster = null;
				yield break;
			}
			this._poster = null;
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
			Transform heldTransform = this._heldPoster.transform;
			Transform transform = this._animator.GetBoneTransform(HumanBodyBones.RightHand);
			if (transform == null)
			{
				transform = base.transform;
				this.Log("RightHand non disponibile: uso il root NPC come fallback.");
			}
			Vector3 vector = base.transform.TransformPoint(this._settings.CarryLocalPosition);
			Quaternion quaternion = base.transform.rotation * Quaternion.Euler(this._settings.CarryLocalEulerAngles);
			Vector3 vector2 = Vector3.Scale(base.transform.lossyScale, this._settings.CarryLocalScale);
			heldTransform.SetParent(transform, true);
			Vector3 startPosition = heldTransform.localPosition;
			Quaternion startRotation = heldTransform.localRotation;
			Vector3 startScale = heldTransform.localScale;
			Vector3 targetPosition = transform.InverseTransformPoint(vector);
			Quaternion targetRotation = Quaternion.Inverse(transform.rotation) * quaternion;
			Vector3 targetScale = NPCWantedPosterBehaviour.DivideScale(vector2, transform.lossyScale);
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
			MeshFilter component = this._heldPoster.GetComponent<MeshFilter>();
			GameObject gameObject = new GameObject("WantedPosterGazeTarget");
			this._gazeTarget = gameObject.transform;
			this._gazeTarget.SetParent(heldTransform, false);
			this._gazeTarget.localPosition = ((component != null && component.sharedMesh != null) ? component.sharedMesh.bounds.center : new Vector3(0f, 0.45f, 0f));
			if (this._lookAt != null)
			{
				this._lookAt.StartLookingAt(this._gazeTarget);
			}
			this._controller.UnlockNavigation(false);
			Vector3 away = base.transform.position - posterPosition;
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
			if (this._heldPoster != null)
			{
				Vector3 fullScale = this._heldPoster.transform.localScale;
				float poseReadyTime = Time.time + 0.25f;
				while (Time.time < poseReadyTime && this.CanContinue(false))
				{
					float num2 = 1f - (poseReadyTime - Time.time) / 0.25f;
					this._heldPoster.transform.localScale = Vector3.Lerp(fullScale, Vector3.zero, num2);
					yield return null;
				}
				fullScale = default(Vector3);
			}
			yield break;
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x00202258 File Offset: 0x00200458
		private bool ConfigureCarryPose()
		{
			this._armsLayerIndex = this._animator.GetLayerIndex("Arms");
			if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.WantedPosterCarryHash, AnimatorControllerParameterType.Bool) && this._armsLayerIndex >= 0)
			{
				this._previousArmsLayerWeight = this._animator.GetLayerWeight(this._armsLayerIndex);
				this._animator.SetLayerWeight(this._armsLayerIndex, 1f);
				if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.UpperBodyActionHash, AnimatorControllerParameterType.Int))
				{
					this._animator.SetInteger(NPCWantedPosterBehaviour.UpperBodyActionHash, 0);
				}
				if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.WantedPosterCarryRaiseHash, AnimatorControllerParameterType.Float))
				{
					this._animator.SetFloat(NPCWantedPosterBehaviour.WantedPosterCarryRaiseHash, Mathf.Clamp01(this._settings.CarryArmRaise));
				}
				this._animator.SetBool(NPCWantedPosterBehaviour.WantedPosterCarryHash, true);
				return true;
			}
			if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.UpperBodyActionHash, AnimatorControllerParameterType.Int))
			{
				this._animator.SetInteger(NPCWantedPosterBehaviour.UpperBodyActionHash, 8);
			}
			return false;
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x00202348 File Offset: 0x00200548
		private bool CanPlayerObserveTarget()
		{
			Camera camera;
			Transform transform;
			if (this._poster == null || !NpcInterestPlayerVisibility.TryResolveContext(out camera, out transform))
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
			return NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, this._poster.NpcInterestPoint, this._poster.transform, this._settings.MaximumPlayerObservationDistance, this._settings.PlayerViewportPadding, this._settings.PlayerObstructionMask) && NpcInterestPlayerVisibility.IsVisibleToPlayer(camera, transform, vector, base.transform, this._settings.MaximumPlayerObservationDistance, this._settings.PlayerViewportPadding, this._settings.PlayerObstructionMask);
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x0020243C File Offset: 0x0020063C
		private static Vector3 DivideScale(Vector3 desiredWorldScale, Vector3 parentWorldScale)
		{
			return new Vector3((Mathf.Abs(parentWorldScale.x) > 0.0001f) ? (desiredWorldScale.x / parentWorldScale.x) : desiredWorldScale.x, (Mathf.Abs(parentWorldScale.y) > 0.0001f) ? (desiredWorldScale.y / parentWorldScale.y) : desiredWorldScale.y, (Mathf.Abs(parentWorldScale.z) > 0.0001f) ? (desiredWorldScale.z / parentWorldScale.z) : desiredWorldScale.z);
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x002024C4 File Offset: 0x002006C4
		private GameObject CreateCarriedPosterVisual(WantedPoster source)
		{
			MeshFilter meshFilter = ((source != null) ? source.GetComponent<MeshFilter>() : null);
			MeshRenderer meshRenderer = ((source != null) ? source.Renderer : null);
			if (meshFilter == null || meshFilter.sharedMesh == null || meshRenderer == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject("NPC_CarriedWantedPoster");
			gameObject.transform.SetPositionAndRotation(source.transform.position, source.transform.rotation);
			gameObject.transform.localScale = source.transform.lossyScale;
			gameObject.AddComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
			MeshRenderer meshRenderer2 = gameObject.AddComponent<MeshRenderer>();
			meshRenderer2.sharedMaterials = meshRenderer.sharedMaterials;
			meshRenderer2.shadowCastingMode = meshRenderer.shadowCastingMode;
			meshRenderer2.receiveShadows = meshRenderer.receiveShadows;
			WantedPosterPhotoSystem instance = WantedPosterPhotoSystem.Instance;
			if (instance != null)
			{
				instance.ApplyCurrentMaterialSnapshotTo(gameObject);
			}
			return gameObject;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x002025A8 File Offset: 0x002007A8
		private Vector3 ResolveApproachPosition()
		{
			Vector3 npcInterestPoint = this._poster.NpcInterestPoint;
			Vector3 normalized = this._poster.transform.forward.normalized;
			Vector3 vector = Vector3.ProjectOnPlane(normalized, Vector3.up);
			if (Mathf.Abs(Vector3.Dot(normalized, Vector3.up)) > 0.65f || vector.sqrMagnitude < 0.01f)
			{
				vector = base.transform.position - npcInterestPoint;
				vector.y = 0f;
			}
			if (vector.sqrMagnitude < 0.01f)
			{
				vector = -base.transform.forward;
			}
			vector.Normalize();
			Vector3 vector2 = npcInterestPoint + vector * this._settings.ApproachDistance;
			vector2.y = base.transform.position.y;
			return vector2;
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x0020267D File Offset: 0x0020087D
		private IEnumerator RotateTowardPoster(float maxDuration)
		{
			float endTime = Time.time + maxDuration;
			while (Time.time < endTime && this.CanContinue(true))
			{
				Vector3 vector = this._poster.NpcInterestPoint - base.transform.position;
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

		// Token: 0x060052DA RID: 21210 RVA: 0x00202694 File Offset: 0x00200894
		private void FacePosterImmediate()
		{
			if (this._poster == null)
			{
				return;
			}
			Vector3 vector = this._poster.NpcInterestPoint - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				return;
			}
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector.normalized, Vector3.up), 240f * Time.deltaTime);
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x00202720 File Offset: 0x00200920
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

		// Token: 0x060052DC RID: 21212 RVA: 0x00202774 File Offset: 0x00200974
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

		// Token: 0x060052DD RID: 21213 RVA: 0x00202864 File Offset: 0x00200A64
		private bool CanContinue(bool requirePoster)
		{
			if (!this._busy || !base.isActiveAndEnabled || this._fsm == null || !this._fsm.IsInitialized || this._controller == null || !this._controller.isActiveAndEnabled || this._wander == null || !this._wander.isActiveAndEnabled)
			{
				return false;
			}
			if (!GameSystemsBootstrapper.IsRunReady(true) || this._director == null || !this._director.isActiveAndEnabled)
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance != null && instance.IsTransitioning)
			{
				return false;
			}
			NPCStateType currentStateType = this._fsm.CurrentStateType;
			return (currentStateType == NPCStateType.Idle || currentStateType == NPCStateType.Wandering || currentStateType == NPCStateType.LookingAt) && (!(this._senses != null) || (!this._senses.IsAlert && !this._senses.IsDetected)) && (!requirePoster || (this._poster != null && this._poster.IsNpcInterestEligible && this._poster.IsReservedForNpc));
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x0020298C File Offset: 0x00200B8C
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

		// Token: 0x060052DF RID: 21215 RVA: 0x002029DC File Offset: 0x00200BDC
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

		// Token: 0x060052E0 RID: 21216 RVA: 0x00202A50 File Offset: 0x00200C50
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
			this.IsPhotographingPoster = false;
			if (this._animator != null)
			{
				if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.ReactionTypeHash, AnimatorControllerParameterType.Int))
				{
					this._animator.SetInteger(NPCWantedPosterBehaviour.ReactionTypeHash, 0);
				}
				if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.UpperBodyActionHash, AnimatorControllerParameterType.Int))
				{
					this._animator.SetInteger(NPCWantedPosterBehaviour.UpperBodyActionHash, 0);
				}
				if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.WantedPosterCarryHash, AnimatorControllerParameterType.Bool))
				{
					this._animator.SetBool(NPCWantedPosterBehaviour.WantedPosterCarryHash, false);
				}
				if (this.HasAnimatorParameter(NPCWantedPosterBehaviour.WantedPosterCarryRaiseHash, AnimatorControllerParameterType.Float))
				{
					this._animator.SetFloat(NPCWantedPosterBehaviour.WantedPosterCarryRaiseHash, 0f);
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
			if (this._heldPoster != null)
			{
				Object.Destroy(this._heldPoster);
			}
			this._heldPoster = null;
			this._gazeTarget = null;
			if (this._animator != null && this._armsLayerIndex >= 0)
			{
				this._animator.SetLayerWeight(this._armsLayerIndex, this._previousArmsLayerWeight);
			}
			this._armsLayerIndex = -1;
			if (this._poster != null)
			{
				this._poster.ReleaseNpcReservation(this);
			}
			this._poster = null;
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
			WantedPosterNpcDirector director = this._director;
			this._director = null;
			this._busy = false;
			this._finishing = false;
			if (director == null)
			{
				return;
			}
			director.NotifySequenceFinished(this);
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x00202CB2 File Offset: 0x00200EB2
		private void OnDisable()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x00202CB2 File Offset: 0x00200EB2
		private void OnDestroy()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x00202CC2 File Offset: 0x00200EC2
		private void Log(string message)
		{
			if (this._settings.DebugLogs)
			{
				Debug.Log("[WantedPosterNPC] " + base.name + ": " + message);
			}
		}

		// Token: 0x0400534D RID: 21325
		private static readonly int ReactionTypeHash = Animator.StringToHash("ReactionType");

		// Token: 0x0400534E RID: 21326
		private static readonly int ReactionTriggerHash = Animator.StringToHash("ReactionTrigger");

		// Token: 0x0400534F RID: 21327
		private static readonly int UpperBodyActionHash = Animator.StringToHash("UpperBodyAction");

		// Token: 0x04005350 RID: 21328
		private static readonly int WantedPosterCarryHash = Animator.StringToHash("WantedPosterCarry");

		// Token: 0x04005351 RID: 21329
		private static readonly int WantedPosterCarryRaiseHash = Animator.StringToHash("WantedPosterCarryRaise");

		// Token: 0x04005352 RID: 21330
		private const int CarryingBoxFrontAction = 8;

		// Token: 0x04005353 RID: 21331
		private NPCStateMachine _fsm;

		// Token: 0x04005354 RID: 21332
		private NPCWanderController _wander;

		// Token: 0x04005355 RID: 21333
		private AStarNPCController _controller;

		// Token: 0x04005356 RID: 21334
		private NPCSenses _senses;

		// Token: 0x04005357 RID: 21335
		private NPCEquipmentManager _equipment;

		// Token: 0x04005358 RID: 21336
		private NPCSimpleLookAt _lookAt;

		// Token: 0x04005359 RID: 21337
		private Animator _animator;

		// Token: 0x0400535A RID: 21338
		private WantedPoster _poster;

		// Token: 0x0400535B RID: 21339
		private WantedPosterNpcDirector _director;

		// Token: 0x0400535C RID: 21340
		private NPCWantedPosterBehaviour.SequenceSettings _settings;

		// Token: 0x0400535D RID: 21341
		private Coroutine _routine;

		// Token: 0x0400535E RID: 21342
		private GameObject _heldPoster;

		// Token: 0x0400535F RID: 21343
		private Transform _gazeTarget;

		// Token: 0x04005360 RID: 21344
		private readonly List<string> _activatedPhotoProps = new List<string>();

		// Token: 0x04005361 RID: 21345
		private bool _takePhoto;

		// Token: 0x04005362 RID: 21346
		private bool _takePoster;

		// Token: 0x04005363 RID: 21347
		private bool _busy;

		// Token: 0x04005364 RID: 21348
		private bool _finishing;

		// Token: 0x04005365 RID: 21349
		private bool _lookAtHorizontalOnly;

		// Token: 0x04005366 RID: 21350
		private bool _capturedLookAtSetting;

		// Token: 0x04005367 RID: 21351
		private int _armsLayerIndex = -1;

		// Token: 0x04005368 RID: 21352
		private float _previousArmsLayerWeight;

		// Token: 0x02000B65 RID: 2917
		public struct SequenceSettings
		{
			// Token: 0x0400536A RID: 21354
			public float ApproachDistance;

			// Token: 0x0400536B RID: 21355
			public float ApproachTimeout;

			// Token: 0x0400536C RID: 21356
			public Vector2 WalkAwayDistance;

			// Token: 0x0400536D RID: 21357
			public Vector2 CarryDuration;

			// Token: 0x0400536E RID: 21358
			public Vector3 CarryLocalPosition;

			// Token: 0x0400536F RID: 21359
			public Vector3 CarryLocalEulerAngles;

			// Token: 0x04005370 RID: 21360
			public Vector3 CarryLocalScale;

			// Token: 0x04005371 RID: 21361
			public float CarryArmRaise;

			// Token: 0x04005372 RID: 21362
			public float MaximumPlayerObservationDistance;

			// Token: 0x04005373 RID: 21363
			public float PlayerViewportPadding;

			// Token: 0x04005374 RID: 21364
			public LayerMask PlayerObstructionMask;

			// Token: 0x04005375 RID: 21365
			public bool DebugLogs;
		}
	}
}
