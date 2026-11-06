using System;
using Features.Detection;
using Pathfinding;
using UnityEngine;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F0B RID: 3851
	public class NPCFSMContext
	{
		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06007399 RID: 29593 RVA: 0x002F1E45 File Offset: 0x002F0045
		// (set) Token: 0x0600739A RID: 29594 RVA: 0x002F1E4D File Offset: 0x002F004D
		public GameObject GameObject { get; private set; }

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x0600739B RID: 29595 RVA: 0x002F1E56 File Offset: 0x002F0056
		// (set) Token: 0x0600739C RID: 29596 RVA: 0x002F1E5E File Offset: 0x002F005E
		public Transform Transform { get; private set; }

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x0600739D RID: 29597 RVA: 0x002F1E67 File Offset: 0x002F0067
		// (set) Token: 0x0600739E RID: 29598 RVA: 0x002F1E6F File Offset: 0x002F006F
		public Animator Animator { get; private set; }

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x0600739F RID: 29599 RVA: 0x002F1E78 File Offset: 0x002F0078
		// (set) Token: 0x060073A0 RID: 29600 RVA: 0x002F1E80 File Offset: 0x002F0080
		public NPCSenses Senses { get; private set; }

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x060073A1 RID: 29601 RVA: 0x002F1E89 File Offset: 0x002F0089
		// (set) Token: 0x060073A2 RID: 29602 RVA: 0x002F1E91 File Offset: 0x002F0091
		public AStarNPCController Controller { get; private set; }

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x060073A3 RID: 29603 RVA: 0x002F1E9A File Offset: 0x002F009A
		// (set) Token: 0x060073A4 RID: 29604 RVA: 0x002F1EA2 File Offset: 0x002F00A2
		public NPCEquipmentManager Equipment { get; private set; }

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x060073A5 RID: 29605 RVA: 0x002F1EAB File Offset: 0x002F00AB
		// (set) Token: 0x060073A6 RID: 29606 RVA: 0x002F1EB3 File Offset: 0x002F00B3
		public NPCSimpleLookAt LookAt { get; private set; }

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x060073A7 RID: 29607 RVA: 0x002F1EBC File Offset: 0x002F00BC
		// (set) Token: 0x060073A8 RID: 29608 RVA: 0x002F1EC4 File Offset: 0x002F00C4
		public NPCWanderController Wander { get; private set; }

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x060073A9 RID: 29609 RVA: 0x002F1ECD File Offset: 0x002F00CD
		// (set) Token: 0x060073AA RID: 29610 RVA: 0x002F1ED5 File Offset: 0x002F00D5
		public NPCDialogueHolder Dialogue { get; private set; }

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x060073AB RID: 29611 RVA: 0x002F1EDE File Offset: 0x002F00DE
		// (set) Token: 0x060073AC RID: 29612 RVA: 0x002F1EE6 File Offset: 0x002F00E6
		public NPCAlertHandler AlertHandler { get; private set; }

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x060073AD RID: 29613 RVA: 0x002F1EEF File Offset: 0x002F00EF
		public AlertAnimationProfileSO AlertAnimationProfile
		{
			get
			{
				NPCAlertHandler alertHandler = this.AlertHandler;
				if (alertHandler == null)
				{
					return null;
				}
				return alertHandler.AlertAnimationProfile;
			}
		}

		// Token: 0x060073AE RID: 29614 RVA: 0x002F1F02 File Offset: 0x002F0102
		public AlertAnimationEntry GetRandomStopAndStareAnimation()
		{
			AlertAnimationProfileSO alertAnimationProfile = this.AlertAnimationProfile;
			if (alertAnimationProfile == null)
			{
				return null;
			}
			return alertAnimationProfile.GetRandomStopAndStareAnimation();
		}

		// Token: 0x060073AF RID: 29615 RVA: 0x002F1F15 File Offset: 0x002F0115
		public AlertAnimationEntry GetRandomSearchingAnimation()
		{
			AlertAnimationProfileSO alertAnimationProfile = this.AlertAnimationProfile;
			if (alertAnimationProfile == null)
			{
				return null;
			}
			return alertAnimationProfile.GetRandomSearchingAnimation();
		}

		// Token: 0x060073B0 RID: 29616 RVA: 0x002F1F28 File Offset: 0x002F0128
		public void ActivateAlertProps(AlertAnimationEntry entry)
		{
			if (entry == null || entry.propsToActivate == null || entry.propsToActivate.Count == 0)
			{
				return;
			}
			foreach (string text in entry.propsToActivate)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (this.Equipment != null)
					{
						this.Equipment.ActivateAndLockProp(text);
					}
					else
					{
						Transform transform = this.FindDeepChild(this.Transform, text);
						if (transform != null)
						{
							transform.gameObject.SetActive(true);
						}
						else
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"[FSM] ",
								this.GameObject.name,
								": Prop '",
								text,
								"' not found"
							}));
						}
					}
				}
			}
		}

		// Token: 0x060073B1 RID: 29617 RVA: 0x002F2018 File Offset: 0x002F0218
		public void DeactivateAlertProps(AlertAnimationEntry entry)
		{
			if (entry == null || entry.propsToActivate == null || entry.propsToActivate.Count == 0)
			{
				return;
			}
			if (!entry.deactivatePropsOnExit)
			{
				return;
			}
			foreach (string text in entry.propsToActivate)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (this.Equipment != null)
					{
						this.Equipment.DeactivateProp(text);
					}
					else
					{
						Transform transform = this.FindDeepChild(this.Transform, text);
						if (transform != null)
						{
							transform.gameObject.SetActive(false);
						}
					}
				}
			}
		}

		// Token: 0x060073B2 RID: 29618 RVA: 0x002F20D0 File Offset: 0x002F02D0
		public void ActivateAlertProps(NPCReactionEntry entry)
		{
			if (entry == null || entry.PropsToActivate == null || entry.PropsToActivate.Count == 0)
			{
				return;
			}
			foreach (string text in entry.PropsToActivate)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (this.Equipment != null)
					{
						this.Equipment.ActivateAndLockProp(text);
					}
					else
					{
						Transform transform = this.FindDeepChild(this.Transform, text);
						if (transform != null)
						{
							transform.gameObject.SetActive(true);
						}
						else
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"[FSM] ",
								this.GameObject.name,
								": Prop '",
								text,
								"' not found"
							}));
						}
					}
				}
			}
		}

		// Token: 0x060073B3 RID: 29619 RVA: 0x002F21C0 File Offset: 0x002F03C0
		public void DeactivateAlertProps(NPCReactionEntry entry)
		{
			if (entry == null || entry.PropsToActivate == null || entry.PropsToActivate.Count == 0)
			{
				return;
			}
			foreach (string text in entry.PropsToActivate)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (this.Equipment != null)
					{
						this.Equipment.DeactivateProp(text);
					}
					else
					{
						Transform transform = this.FindDeepChild(this.Transform, text);
						if (transform != null)
						{
							transform.gameObject.SetActive(false);
						}
					}
				}
			}
		}

		// Token: 0x060073B4 RID: 29620 RVA: 0x002F226C File Offset: 0x002F046C
		private Transform FindDeepChild(Transform parent, string name)
		{
			if (parent.name == name)
			{
				return parent;
			}
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				Transform transform2 = this.FindDeepChild(transform, name);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x060073B5 RID: 29621 RVA: 0x002F22E8 File Offset: 0x002F04E8
		public float Attention
		{
			get
			{
				NPCSenses senses = this.Senses;
				if (senses == null)
				{
					return 0f;
				}
				return senses.CurrentAttention;
			}
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x060073B6 RID: 29622 RVA: 0x002F22FF File Offset: 0x002F04FF
		public DetectionSource DetectionSource
		{
			get
			{
				NPCSenses senses = this.Senses;
				if (senses == null)
				{
					return DetectionSource.None;
				}
				return senses.CurrentDetectionSource;
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x060073B7 RID: 29623 RVA: 0x002F2312 File Offset: 0x002F0512
		// (set) Token: 0x060073B8 RID: 29624 RVA: 0x002F231A File Offset: 0x002F051A
		public float StateEnteredTime { get; set; }

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x060073B9 RID: 29625 RVA: 0x002F2323 File Offset: 0x002F0523
		public float TimeInState
		{
			get
			{
				return Time.time - this.StateEnteredTime;
			}
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x060073BA RID: 29626 RVA: 0x002F2331 File Offset: 0x002F0531
		// (set) Token: 0x060073BB RID: 29627 RVA: 0x002F2339 File Offset: 0x002F0539
		public Vector3 LastSeenPlayerPosition { get; set; }

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x060073BC RID: 29628 RVA: 0x002F2342 File Offset: 0x002F0542
		// (set) Token: 0x060073BD RID: 29629 RVA: 0x002F234A File Offset: 0x002F054A
		public string CurrentOwner { get; set; }

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x060073BE RID: 29630 RVA: 0x002F2354 File Offset: 0x002F0554
		public Transform PlayerTransform
		{
			get
			{
				if (this._playerTransform == null)
				{
					SensoryManager instance = SensoryManager.Instance;
					StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
					this._playerTransform = ((stealthActor != null) ? stealthActor.transform : null);
				}
				return this._playerTransform;
			}
		}

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x060073BF RID: 29631 RVA: 0x002F239C File Offset: 0x002F059C
		public Vector3? PlayerPosition
		{
			get
			{
				Transform playerTransform = this.PlayerTransform;
				if (playerTransform == null)
				{
					return null;
				}
				return new Vector3?(playerTransform.position);
			}
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x060073C0 RID: 29632 RVA: 0x002F23C7 File Offset: 0x002F05C7
		public float DistanceToPlayer
		{
			get
			{
				if (this.PlayerTransform == null)
				{
					return float.MaxValue;
				}
				return Vector3.Distance(this.Transform.position, this.PlayerTransform.position);
			}
		}

		// Token: 0x060073C1 RID: 29633 RVA: 0x002F23F8 File Offset: 0x002F05F8
		public NPCFSMContext(GameObject npc)
		{
			this.GameObject = npc;
			this.Transform = npc.transform;
			this.Animator = npc.GetComponentInChildren<Animator>();
			this.Senses = npc.GetComponent<NPCSenses>();
			this.Controller = npc.GetComponent<AStarNPCController>();
			this.Equipment = npc.GetComponentInChildren<NPCEquipmentManager>();
			this.LookAt = npc.GetComponent<NPCSimpleLookAt>();
			this.Wander = npc.GetComponent<NPCWanderController>();
			this.Dialogue = npc.GetComponent<NPCDialogueHolder>();
			this.AlertHandler = npc.GetComponent<NPCAlertHandler>();
		}

		// Token: 0x060073C2 RID: 29634 RVA: 0x002F247E File Offset: 0x002F067E
		public void LockProps()
		{
			NPCEquipmentManager equipment = this.Equipment;
			if (equipment == null)
			{
				return;
			}
			equipment.LockPropsDeactivated();
		}

		// Token: 0x060073C3 RID: 29635 RVA: 0x002F2490 File Offset: 0x002F0690
		public void UnlockProps()
		{
			NPCEquipmentManager equipment = this.Equipment;
			if (equipment == null)
			{
				return;
			}
			equipment.UnlockProps();
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x002F24A2 File Offset: 0x002F06A2
		public void LockNavigation()
		{
			AStarNPCController controller = this.Controller;
			if (controller == null)
			{
				return;
			}
			controller.StopAndLock();
		}

		// Token: 0x060073C5 RID: 29637 RVA: 0x002F24B4 File Offset: 0x002F06B4
		public void UnlockNavigation()
		{
			AStarNPCController controller = this.Controller;
			if (controller == null)
			{
				return;
			}
			controller.UnlockNavigation(false);
		}

		// Token: 0x060073C6 RID: 29638 RVA: 0x002F24C7 File Offset: 0x002F06C7
		public void StartLookingAtPlayer()
		{
			if (this.LookAt != null && this.PlayerTransform != null)
			{
				this.LookAt.StartLookingAt(this.PlayerTransform);
			}
		}

		// Token: 0x060073C7 RID: 29639 RVA: 0x002F24F6 File Offset: 0x002F06F6
		public void StopLooking()
		{
			NPCSimpleLookAt lookAt = this.LookAt;
			if (lookAt == null)
			{
				return;
			}
			lookAt.StopLooking();
		}

		// Token: 0x060073C8 RID: 29640 RVA: 0x002F2508 File Offset: 0x002F0708
		public void RotateToward(Vector3 targetPosition, float speed = 5f)
		{
			this.RotateTowardWithDelta(targetPosition, speed, NPCSimulationPause.SimulationDeltaTime);
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x002F2518 File Offset: 0x002F0718
		internal void RotateTowardWithDelta(Vector3 targetPosition, float speed, float deltaTime)
		{
			if ((this.Senses != null && this.Senses.IsSeatedObserver) || (this.Wander != null && this.Wander.BenchOwnsBodyPose))
			{
				return;
			}
			Vector3 normalized = (targetPosition - this.Transform.position).normalized;
			normalized.y = 0f;
			if (normalized.sqrMagnitude > 0.001f)
			{
				Quaternion quaternion = Quaternion.LookRotation(normalized);
				this.Transform.rotation = Quaternion.Slerp(this.Transform.rotation, quaternion, deltaTime * speed);
			}
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x002F25B6 File Offset: 0x002F07B6
		public void SetTrigger(string triggerName)
		{
			Animator animator = this.Animator;
			if (animator == null)
			{
				return;
			}
			animator.SetTrigger(triggerName);
		}

		// Token: 0x060073CB RID: 29643 RVA: 0x002F25C9 File Offset: 0x002F07C9
		public void SetBool(string paramName, bool value)
		{
			Animator animator = this.Animator;
			if (animator == null)
			{
				return;
			}
			animator.SetBool(paramName, value);
		}

		// Token: 0x060073CC RID: 29644 RVA: 0x002F25DD File Offset: 0x002F07DD
		public void SetFloat(string paramName, float value)
		{
			Animator animator = this.Animator;
			if (animator == null)
			{
				return;
			}
			animator.SetFloat(paramName, value);
		}

		// Token: 0x060073CD RID: 29645 RVA: 0x002F25F1 File Offset: 0x002F07F1
		public void SetInt(string paramName, int value)
		{
			Animator animator = this.Animator;
			if (animator == null)
			{
				return;
			}
			animator.SetInteger(paramName, value);
		}

		// Token: 0x060073CE RID: 29646 RVA: 0x002F2605 File Offset: 0x002F0805
		public void SetDestination(Vector3 destination)
		{
			AStarNPCController controller = this.Controller;
			if (controller == null)
			{
				return;
			}
			controller.SetDestination(destination);
		}

		// Token: 0x060073CF RID: 29647 RVA: 0x002F2618 File Offset: 0x002F0818
		public bool HasReachedDestination()
		{
			AStarNPCController controller = this.Controller;
			return controller != null && controller.HasReachedDestination();
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x060073D0 RID: 29648 RVA: 0x002F262B File Offset: 0x002F082B
		public bool IsNavigationLocked
		{
			get
			{
				AStarNPCController controller = this.Controller;
				return controller != null && controller.IsNavigationLocked;
			}
		}

		// Token: 0x060073D1 RID: 29649 RVA: 0x002F2640 File Offset: 0x002F0840
		public bool HasDirectLineOfSight(Vector3 targetPosition)
		{
			if (this.Senses == null)
			{
				return false;
			}
			Vector3 eyePosition = this.Senses.EyePosition;
			Vector3 vector = targetPosition - eyePosition;
			float magnitude = vector.magnitude;
			int mask = LayerMask.GetMask(new string[] { "Default", "Environment", "Structure", "Wall" });
			return !Physics.Raycast(eyePosition, vector, magnitude, mask);
		}

		// Token: 0x060073D2 RID: 29650 RVA: 0x002F26B4 File Offset: 0x002F08B4
		public void SetFacialExpression(ReactionType reactionType)
		{
			if (this.Dialogue == null || this.Animator == null)
			{
				return;
			}
			int expressionForReaction = this.Dialogue.GetExpressionForReaction(reactionType);
			NPCPersonalityProfileSO personalityProfile = this.Dialogue.PersonalityProfile;
			string text = ((personalityProfile != null) ? personalityProfile.expressionParamName : null);
			if (!string.IsNullOrEmpty(text))
			{
				this.Animator.SetInteger(text, expressionForReaction);
			}
		}

		// Token: 0x060073D3 RID: 29651 RVA: 0x002F2718 File Offset: 0x002F0918
		public void ResetFacialExpression()
		{
			if (this.Dialogue == null || this.Animator == null)
			{
				return;
			}
			if (this.Dialogue.PersonalityProfile != null)
			{
				string expressionParamName = this.Dialogue.PersonalityProfile.expressionParamName;
				if (!string.IsNullOrEmpty(expressionParamName))
				{
					this.Animator.SetInteger(expressionParamName, this.Dialogue.PersonalityProfile.neutralExpressionIndex);
				}
			}
		}

		// Token: 0x060073D4 RID: 29652 RVA: 0x002F278C File Offset: 0x002F098C
		public void SetAlertExpression()
		{
			if (this.Dialogue == null || this.Animator == null)
			{
				return;
			}
			if (this.Dialogue.PersonalityProfile != null)
			{
				string expressionParamName = this.Dialogue.PersonalityProfile.expressionParamName;
				int alertExpressionIndex = this.Dialogue.PersonalityProfile.alertExpressionIndex;
				if (!string.IsNullOrEmpty(expressionParamName))
				{
					this.Animator.SetInteger(expressionParamName, alertExpressionIndex);
				}
			}
		}

		// Token: 0x060073D5 RID: 29653 RVA: 0x002F2800 File Offset: 0x002F0A00
		public void EnableAlertLayer()
		{
			if (this.Animator == null)
			{
				return;
			}
			if (4 < this.Animator.layerCount)
			{
				this.Animator.SetLayerWeight(4, 1f);
			}
		}

		// Token: 0x060073D6 RID: 29654 RVA: 0x002F2830 File Offset: 0x002F0A30
		public void DisableAlertLayer()
		{
			if (this.Animator == null)
			{
				return;
			}
			if (4 < this.Animator.layerCount)
			{
				this.Animator.SetLayerWeight(4, 0f);
			}
		}

		// Token: 0x060073D7 RID: 29655 RVA: 0x002F2860 File Offset: 0x002F0A60
		public void EnableReactionsLayer()
		{
			if (this.Animator == null)
			{
				return;
			}
			if (1 < this.Animator.layerCount)
			{
				this.Animator.SetLayerWeight(1, 1f);
			}
		}

		// Token: 0x060073D8 RID: 29656 RVA: 0x002F2890 File Offset: 0x002F0A90
		public void DisableReactionsLayer()
		{
			if (this.Animator == null)
			{
				return;
			}
			if (1 < this.Animator.layerCount)
			{
				this.Animator.SetLayerWeight(1, 0f);
			}
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x002F28C0 File Offset: 0x002F0AC0
		public void ZeroLocomotion()
		{
			if (this.Animator == null)
			{
				return;
			}
			this.TrySetFloat("Speed", 0f);
			this.TrySetFloat("VerticalSpeed", 0f);
			this.TrySetFloat("HorizontalSpeed", 0f);
		}

		// Token: 0x060073DA RID: 29658 RVA: 0x002F290C File Offset: 0x002F0B0C
		public void ResetToIdleAnimation()
		{
			if (this.Animator == null)
			{
				return;
			}
			this.TrySetInt("ActionState", 0);
			this.TrySetInt("PostureState", 0);
			this.TrySetFloat("ActionBlend", 0f);
		}

		// Token: 0x060073DB RID: 29659 RVA: 0x002F2948 File Offset: 0x002F0B48
		public void PrepareToStop(float stoppingDistance = 0.5f)
		{
			if (this.Controller != null)
			{
				Vector3 position = this.Controller.transform.position;
				Vector3 forward = this.Controller.transform.forward;
				Vector3 vector = position + forward * stoppingDistance;
				this.SetDestination(vector);
			}
		}

		// Token: 0x060073DC RID: 29660 RVA: 0x002F2998 File Offset: 0x002F0B98
		public bool HasStoppedMoving(float threshold = 0.1f)
		{
			if (this.Controller == null)
			{
				return true;
			}
			FollowerEntity component = this.Controller.GetComponent<FollowerEntity>();
			if (component != null)
			{
				return component.velocity.magnitude < threshold;
			}
			return !(this.Animator != null) || this.Animator.GetFloat("Speed") < threshold;
		}

		// Token: 0x060073DD RID: 29661 RVA: 0x002F2A00 File Offset: 0x002F0C00
		private void TrySetInt(string paramName, int value)
		{
			if (this.Animator == null || this.Animator.runtimeAnimatorController == null)
			{
				return;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in this.Animator.parameters)
			{
				if (animatorControllerParameter.name == paramName && animatorControllerParameter.type == AnimatorControllerParameterType.Int)
				{
					this.Animator.SetInteger(paramName, value);
					return;
				}
			}
		}

		// Token: 0x060073DE RID: 29662 RVA: 0x002F2A74 File Offset: 0x002F0C74
		private void TrySetFloat(string paramName, float value)
		{
			if (this.Animator == null || this.Animator.runtimeAnimatorController == null)
			{
				return;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in this.Animator.parameters)
			{
				if (animatorControllerParameter.name == paramName && animatorControllerParameter.type == AnimatorControllerParameterType.Float)
				{
					this.Animator.SetFloat(paramName, value);
					return;
				}
			}
		}

		// Token: 0x040076D9 RID: 30425
		private Transform _playerTransform;

		// Token: 0x040076DA RID: 30426
		private const int AlertBehaviorLayerIndex = 4;

		// Token: 0x040076DB RID: 30427
		private const int ReactionsLayerIndex = 1;
	}
}
