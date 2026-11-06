using System;
using Features.Detection;
using Features.NPC.StateMachine;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EFA RID: 3834
	public class NPCStumbleHandler : MonoBehaviour
	{
		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x060072C7 RID: 29383 RVA: 0x002EB22A File Offset: 0x002E942A
		public bool IsStumbling
		{
			get
			{
				return this._isStumbling;
			}
		}

		// Token: 0x060072C8 RID: 29384 RVA: 0x002EB232 File Offset: 0x002E9432
		private void Awake()
		{
			this._senses = base.GetComponent<NPCSenses>();
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._alertHandler = base.GetComponent<NPCAlertHandler>();
			this._npcController = base.GetComponent<AStarNPCController>();
			this._animator = base.GetComponentInChildren<Animator>();
		}

		// Token: 0x060072C9 RID: 29385 RVA: 0x002EB270 File Offset: 0x002E9470
		private void OnEnable()
		{
			this._ragdollRecovery = base.GetComponent<NPCRagdollRecovery>();
			if (this._ragdollRecovery == null)
			{
				this._ragdollRecovery = base.gameObject.AddComponent<NPCRagdollRecovery>();
			}
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x060072CA RID: 29386 RVA: 0x002EB2CA File Offset: 0x002E94CA
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this._isStumbling = false;
			this._displacementRemaining = 0f;
		}

		// Token: 0x060072CB RID: 29387 RVA: 0x002EB2F0 File Offset: 0x002E94F0
		private void Update()
		{
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
			if (frameTimeLoss > 0f)
			{
				this._stumbleEndTime += frameTimeLoss;
				this._lastStumbleTime += frameTimeLoss;
			}
			if (this._isStumbling)
			{
				this.UpdateStumble();
			}
		}

		// Token: 0x060072CC RID: 29388 RVA: 0x002EB34C File Offset: 0x002E954C
		private void OnCollisionEnter(Collision collision)
		{
			if (!collision.gameObject.CompareTag(this.playerTag))
			{
				return;
			}
			CielOnFootNpcCrashRagdoll componentInParent = collision.gameObject.GetComponentInParent<CielOnFootNpcCrashRagdoll>();
			if (componentInParent != null && componentInParent.TryTriggerNpcCollision(this, collision))
			{
				return;
			}
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			if (this._ragdollRecovery != null && this._ragdollRecovery.IsRecovering)
			{
				return;
			}
			if (collision.relativeVelocity.magnitude < this.minCollisionVelocity)
			{
				return;
			}
			if (Time.time - this._lastStumbleTime < this.stumbleCooldown)
			{
				return;
			}
			Vector3 point = collision.contacts[0].point;
			this.TriggerStumble(point, collision.relativeVelocity);
		}

		// Token: 0x060072CD RID: 29389 RVA: 0x002EB3FC File Offset: 0x002E95FC
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (!hit.gameObject.CompareTag(this.playerTag))
			{
				return;
			}
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			if (Time.time - this._lastStumbleTime < this.stumbleCooldown)
			{
				return;
			}
			Vector3 moveDirection = hit.moveDirection;
			float num = hit.moveLength / Time.deltaTime;
			if (num < this.minCollisionVelocity)
			{
				return;
			}
			this.TriggerStumble(hit.point, moveDirection * num);
		}

		// Token: 0x060072CE RID: 29390 RVA: 0x002EB46C File Offset: 0x002E966C
		public void TriggerStumble(Vector3 collisionPoint, Vector3 impactVelocity)
		{
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			if (this._ragdollRecovery != null && this._ragdollRecovery.IsRecovering)
			{
				return;
			}
			if (this._isStumbling)
			{
				return;
			}
			if (Time.time - this._lastStumbleTime < this.stumbleCooldown)
			{
				return;
			}
			this._lastStumbleTime = Time.time;
			this._isStumbling = true;
			this._stumbleEndTime = Time.time + this.stumbleDuration;
			NPCStumbleHandler.StumbleDirection stumbleDirection = this.CalculateStumbleDirection(collisionPoint);
			this._displacementDirection = this.CalculateDisplacementDirection(stumbleDirection);
			this._displacementRemaining = this.displacementDistance;
			this.InterruptCurrentBehavior();
			if (this._senses != null)
			{
				float currentAttention = this._senses.CurrentAttention;
				if (currentAttention < this.stumbleAttentionLevel)
				{
					this._senses.SetAttentionState(this.stumbleAttentionLevel);
					if (this.debugLog)
					{
						Debug.Log(string.Format("[Stumble] {0}: Attention SET from {1:F2} to {2:F2}", base.gameObject.name, currentAttention, this.stumbleAttentionLevel));
					}
				}
				else if (this.debugLog)
				{
					Debug.Log(string.Format("[Stumble] {0}: Attention already at {1:F2}, stumble is visual only", base.gameObject.name, currentAttention));
				}
			}
			if (this._animator != null)
			{
				if (this.reactionsLayerIndex >= 0 && this.reactionsLayerIndex < this._animator.layerCount)
				{
					this._animator.SetLayerWeight(this.reactionsLayerIndex, 1f);
				}
				this._animator.SetFloat(this.stumbleDirectionParam, (float)stumbleDirection);
				this._animator.SetTrigger(this.stumbleTrigger);
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[Stumble] {0}: Triggered stumble direction={1}, displacement={2}", base.gameObject.name, stumbleDirection, this._displacementDirection));
			}
		}

		// Token: 0x060072CF RID: 29391 RVA: 0x002EB634 File Offset: 0x002E9834
		private NPCStumbleHandler.StumbleDirection CalculateStumbleDirection(Vector3 collisionPoint)
		{
			Vector3 vector = collisionPoint - base.transform.position;
			Vector3 vector2 = base.transform.InverseTransformDirection(vector.normalized);
			float num = Mathf.Abs(vector2.x);
			if (Mathf.Abs(vector2.z) > num)
			{
				if (vector2.z <= 0f)
				{
					return NPCStumbleHandler.StumbleDirection.Back;
				}
				return NPCStumbleHandler.StumbleDirection.Front;
			}
			else
			{
				if (vector2.x <= 0f)
				{
					return NPCStumbleHandler.StumbleDirection.Left;
				}
				return NPCStumbleHandler.StumbleDirection.Right;
			}
		}

		// Token: 0x060072D0 RID: 29392 RVA: 0x002EB6A4 File Offset: 0x002E98A4
		private Vector3 CalculateDisplacementDirection(NPCStumbleHandler.StumbleDirection direction)
		{
			switch (direction)
			{
			case NPCStumbleHandler.StumbleDirection.Front:
				return -base.transform.forward;
			case NPCStumbleHandler.StumbleDirection.Back:
				return base.transform.forward;
			case NPCStumbleHandler.StumbleDirection.Left:
				return base.transform.right;
			case NPCStumbleHandler.StumbleDirection.Right:
				return -base.transform.right;
			default:
				return -base.transform.forward;
			}
		}

		// Token: 0x060072D1 RID: 29393 RVA: 0x002EB713 File Offset: 0x002E9913
		private void InterruptCurrentBehavior()
		{
			if (this._fsm != null)
			{
				this._fsm.ForceState(NPCStateType.Stumbling, "Stumble");
				return;
			}
			if (this._npcController != null)
			{
				this._npcController.StopAndLock();
			}
		}

		// Token: 0x060072D2 RID: 29394 RVA: 0x002EB74F File Offset: 0x002E994F
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x060072D3 RID: 29395 RVA: 0x002EB761 File Offset: 0x002E9961
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x060072D4 RID: 29396 RVA: 0x002EB780 File Offset: 0x002E9980
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			float num = Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			this._stumbleEndTime += num;
			this._lastStumbleTime += num;
			this._simulationPauseActive = false;
		}

		// Token: 0x060072D5 RID: 29397 RVA: 0x002EB7D0 File Offset: 0x002E99D0
		private void UpdateStumble()
		{
			if (this._displacementRemaining > 0f)
			{
				float num = this.displacementSpeed * NPCSimulationPause.SimulationDeltaTime;
				num = Mathf.Min(num, this._displacementRemaining);
				Vector3 vector = this._displacementDirection * num;
				base.transform.position += vector;
				this._displacementRemaining -= num;
			}
			if (Time.time >= this._stumbleEndTime)
			{
				this.EndStumble();
			}
		}

		// Token: 0x060072D6 RID: 29398 RVA: 0x002EB849 File Offset: 0x002E9A49
		private void EndStumble()
		{
			this._isStumbling = false;
			this.RotateTowardPlayer();
			if (this.debugLog)
			{
				Debug.Log("[Stumble] " + base.gameObject.name + ": Stumble ended, FSM will transition to Searching");
			}
		}

		// Token: 0x060072D7 RID: 29399 RVA: 0x002EB880 File Offset: 0x002E9A80
		private void RotateTowardPlayer()
		{
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (stealthActor == null)
			{
				return;
			}
			Vector3 vector = stealthActor.Position - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.01f)
			{
				base.transform.rotation = Quaternion.LookRotation(vector.normalized);
				if (this.debugLog)
				{
					Debug.Log(string.Format("[Stumble] {0}: Rotated to face player at {1}", base.gameObject.name, stealthActor.Position));
				}
			}
		}

		// Token: 0x040075FB RID: 30203
		[Header("Collision Detection")]
		[Tooltip("Tag of the player GameObject")]
		public string playerTag = "Player";

		// Token: 0x040075FC RID: 30204
		[Tooltip("Minimum velocity for collision to trigger stumble")]
		public float minCollisionVelocity = 1.5f;

		// Token: 0x040075FD RID: 30205
		[Tooltip("Cooldown between stumbles (seconds)")]
		public float stumbleCooldown = 2f;

		// Token: 0x040075FE RID: 30206
		[Header("Attention On Stumble")]
		[Tooltip("Attention level to SET when stumbled (not added, but set to this value if current is lower)")]
		[Range(0f, 1f)]
		public float stumbleAttentionLevel = 0.65f;

		// Token: 0x040075FF RID: 30207
		[Header("Displacement")]
		[Tooltip("How far the NPC is pushed during stumble (meters)")]
		public float displacementDistance = 0.8f;

		// Token: 0x04007600 RID: 30208
		[Tooltip("How fast the displacement happens (meters/second)")]
		public float displacementSpeed = 3f;

		// Token: 0x04007601 RID: 30209
		[Header("Animation")]
		[Tooltip("Animator trigger for stumble")]
		public string stumbleTrigger = "StumbleTrigger";

		// Token: 0x04007602 RID: 30210
		[Tooltip("Animator int parameter for stumble direction (0=Front, 1=Back, 2=Left, 3=Right)")]
		public string stumbleDirectionParam = "StumbleDirection";

		// Token: 0x04007603 RID: 30211
		[Tooltip("Duration of stumble animation (for state timing)")]
		public float stumbleDuration = 1.2f;

		// Token: 0x04007604 RID: 30212
		[Tooltip("Index of the Reactions layer in animator")]
		public int reactionsLayerIndex = 1;

		// Token: 0x04007605 RID: 30213
		[Header("Debug")]
		public bool debugLog;

		// Token: 0x04007606 RID: 30214
		private NPCSenses _senses;

		// Token: 0x04007607 RID: 30215
		private NPCStateMachine _fsm;

		// Token: 0x04007608 RID: 30216
		private NPCAlertHandler _alertHandler;

		// Token: 0x04007609 RID: 30217
		private AStarNPCController _npcController;

		// Token: 0x0400760A RID: 30218
		private Animator _animator;

		// Token: 0x0400760B RID: 30219
		private NPCRagdollRecovery _ragdollRecovery;

		// Token: 0x0400760C RID: 30220
		private float _lastStumbleTime = -999f;

		// Token: 0x0400760D RID: 30221
		private bool _isStumbling;

		// Token: 0x0400760E RID: 30222
		private Vector3 _displacementDirection;

		// Token: 0x0400760F RID: 30223
		private float _displacementRemaining;

		// Token: 0x04007610 RID: 30224
		private float _stumbleEndTime;

		// Token: 0x04007611 RID: 30225
		private bool _simulationPauseActive;

		// Token: 0x04007612 RID: 30226
		private float _simulationPauseStartedAt;

		// Token: 0x02000EFB RID: 3835
		public enum StumbleDirection
		{
			// Token: 0x04007614 RID: 30228
			Front,
			// Token: 0x04007615 RID: 30229
			Back,
			// Token: 0x04007616 RID: 30230
			Left,
			// Token: 0x04007617 RID: 30231
			Right
		}
	}
}
