using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC.SceneSystem;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using Pathfinding;
using PoolWaterTools;
using ProjectExhibitionist.Subway;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EE9 RID: 3817
	public sealed class NPCSeatedEncounter
	{
		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06007244 RID: 29252 RVA: 0x002E7C61 File Offset: 0x002E5E61
		public bool RestoresSeatedPose
		{
			get
			{
				return !this._resumeWandering;
			}
		}

		// Token: 0x06007245 RID: 29253 RVA: 0x002E7C6C File Offset: 0x002E5E6C
		public static bool IsTrainPassenger(NPCSenses npc)
		{
			return npc != null && npc.GetComponentInParent<SubwayPassengers>(true) != null;
		}

		// Token: 0x06007246 RID: 29254 RVA: 0x002E7C88 File Offset: 0x002E5E88
		public static void ConfigureStaticDetection(NPCSenses npc)
		{
			if (npc == null)
			{
				return;
			}
			bool flag = NPCSeatedEncounter.IsTrainPassenger(npc);
			if (!flag && !NPCSeatedEncounter.IsSeated(npc))
			{
				return;
			}
			npc.enabled = !flag;
			AttentionUIController component = npc.GetComponent<AttentionUIController>();
			if (component == null)
			{
				return;
			}
			component.TargetNPC = npc;
			component.enabled = !flag;
		}

		// Token: 0x06007247 RID: 29255 RVA: 0x002E7CE0 File Offset: 0x002E5EE0
		public static bool IsSeated(NPCSenses npc)
		{
			if (npc == null || NPCSeatedEncounter.Prepared.Contains(npc))
			{
				return false;
			}
			AStarNPCController component = npc.GetComponent<AStarNPCController>();
			if (component != null && component.enabled)
			{
				return component.CurrentPosture == NPCInterestPoint.PostureType.Sitting;
			}
			StaticPosePreview componentInChildren = npc.GetComponentInChildren<StaticPosePreview>(true);
			if (componentInChildren != null)
			{
				return componentInChildren.postureState == 1;
			}
			if (component != null && component.CurrentPosture == NPCInterestPoint.PostureType.Sitting)
			{
				return true;
			}
			if (npc.name.StartsWith("NPC_StaticSeated_", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			SceneNPCMarker componentInParent = npc.GetComponentInParent<SceneNPCMarker>(true);
			return componentInParent != null && componentInParent.EffectiveInitialPosture == NPCInterestPoint.PostureType.Sitting;
		}

		// Token: 0x06007248 RID: 29256 RVA: 0x002E7D88 File Offset: 0x002E5F88
		public static bool TryPrepare(NPCSenses npc, Transform player, out NPCSeatedEncounter prepared)
		{
			prepared = null;
			if (!NPCSeatedEncounter.IsSeated(npc))
			{
				return true;
			}
			Vector3 vector;
			if (NPCSeatedEncounter.IsTrainPassenger(npc) || !NPCSeatedEncounter.TryFindStandingPosition(npc.transform, player, out vector))
			{
				return false;
			}
			prepared = new NPCSeatedEncounter(npc);
			prepared.Stand(vector, player);
			return true;
		}

		// Token: 0x06007249 RID: 29257 RVA: 0x002E7DD0 File Offset: 0x002E5FD0
		private NPCSeatedEncounter(NPCSenses npc)
		{
			this._npc = npc;
			Transform transform = npc.transform;
			this._parent = transform.parent;
			this._localPosition = transform.localPosition;
			this._localRotation = transform.localRotation;
			this._worldPosition = transform.position;
			this._worldRotation = transform.rotation;
			this._controller = npc.GetComponent<AStarNPCController>();
			this._follower = npc.GetComponent<FollowerEntity>();
			this._wander = npc.GetComponent<NPCWanderController>();
			this._animator = npc.GetComponentInChildren<Animator>(true);
			this._equipment = npc.GetComponentInChildren<NPCEquipmentManager>(true);
			this._resumeWandering = this._controller != null && this._controller.enabled && this._wander != null && this._wander.enabled;
			NPCWanderController wander = this._wander;
			if (wander != null)
			{
				wander.PrepareForExternalAnimationSnapshot();
			}
			this._wasManualMode = this._controller != null && this._controller.IsManualMode;
			if (this._controller != null)
			{
				this._action = this._controller.CurrentAction;
				this._suppressRootMotion = this._controller.SuppressRootMotionPosition;
			}
			this.Capture(this._wander);
			this.Capture(this._controller);
			this.Capture(this._follower);
			this.Capture(npc.GetComponent<NPCStateMachine>());
			this.Capture(npc.GetComponent<NPCAlertHandler>());
			this.Capture(npc.GetComponent<NPCSimpleLookAt>());
			foreach (StaticPosePreview staticPosePreview in npc.GetComponentsInChildren<StaticPosePreview>(true))
			{
				this.Capture(staticPosePreview);
			}
			foreach (SeatedRandomReaction seatedRandomReaction in npc.GetComponentsInChildren<SeatedRandomReaction>(true))
			{
				this.Capture(seatedRandomReaction);
			}
			foreach (FieldPlayerLookAtCiel fieldPlayerLookAtCiel in npc.GetComponentsInChildren<FieldPlayerLookAtCiel>(true))
			{
				this.Capture(fieldPlayerLookAtCiel);
			}
			if (this._animator != null && this._animator.runtimeAnimatorController != null)
			{
				this._rootMotion = this._animator.applyRootMotion;
				this._culling = this._animator.cullingMode;
				this._animatorAction = NPCSeatedEncounter.ReadInt(this._animator, "UpperBodyAction");
				AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
				this._baseState = currentAnimatorStateInfo.fullPathHash;
				this._baseTime = currentAnimatorStateInfo.normalizedTime;
				this._layerWeights = new float[this._animator.layerCount];
				for (int j = 0; j < this._layerWeights.Length; j++)
				{
					this._layerWeights[j] = this._animator.GetLayerWeight(j);
				}
			}
			foreach (NPCSeatedEncounter.BehaviourState behaviourState in this._behaviours)
			{
				if (behaviourState.Component is SeatedRandomReaction)
				{
					behaviourState.Component.enabled = false;
				}
			}
			this._props = ((this._equipment != null) ? this._equipment.GetActivePropsNames() : Array.Empty<string>());
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
		}

		// Token: 0x0600724A RID: 29258 RVA: 0x002E8128 File Offset: 0x002E6328
		private void Capture(Behaviour component)
		{
			if (component != null)
			{
				this._behaviours.Add(new NPCSeatedEncounter.BehaviourState
				{
					Component = component,
					Enabled = component.enabled
				});
			}
		}

		// Token: 0x0600724B RID: 29259 RVA: 0x002E8168 File Offset: 0x002E6368
		private void Stand(Vector3 position, Transform player)
		{
			NPCSeatedEncounter.Prepared.Add(this._npc);
			foreach (NPCSeatedEncounter.BehaviourState behaviourState in this._behaviours)
			{
				behaviourState.Component.enabled = false;
			}
			if (this._controller != null)
			{
				this._controller.CancelManualModeForEncounter();
				this._controller.SuppressRootMotionPosition = true;
				this._controller.SetPosture(NPCInterestPoint.PostureType.Standing);
				this._controller.SetAction(NPCInterestPoint.ActionType.None);
			}
			this._npc.transform.position = position;
			if (player != null)
			{
				Vector3 vector = player.position - position;
				vector.y = 0f;
				if (vector.sqrMagnitude > 0.001f)
				{
					this._npc.transform.rotation = Quaternion.LookRotation(vector);
				}
			}
			if (this._equipment != null)
			{
				this._equipment.DeactivateAllProps();
			}
			if (this._animator != null && this._animator.runtimeAnimatorController != null)
			{
				this._animator.applyRootMotion = false;
				this._animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				NPCSeatedEncounter.SetInt(this._animator, "PostureState", 0);
				NPCSeatedEncounter.SetInt(this._animator, "UpperBodyAction", 0);
				NPCSeatedEncounter.SetInt(this._animator, "ReactionType", 0);
				foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
				{
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Trigger)
					{
						this._animator.ResetTrigger(animatorControllerParameter.nameHash);
					}
					else if (animatorControllerParameter.name == "Speed" && animatorControllerParameter.type == AnimatorControllerParameterType.Float)
					{
						this._animator.SetFloat(animatorControllerParameter.nameHash, 0f);
					}
				}
				for (int j = 1; j < this._animator.layerCount; j++)
				{
					if (this._animator.GetLayerName(j) != "Face")
					{
						this._animator.SetLayerWeight(j, 0f);
					}
				}
				NPCSeatedEncounter.PlayState(this._animator, "Base Layer.Standing.Normal Movement", "Base Layer.Normal Movement");
			}
			Physics.SyncTransforms();
		}

		// Token: 0x0600724C RID: 29260 RVA: 0x002E83C0 File Offset: 0x002E65C0
		public void Restore()
		{
			if (this._restored)
			{
				return;
			}
			this._restored = true;
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			NPCSeatedEncounter.Prepared.Remove(this._npc);
			if (this._npc == null)
			{
				return;
			}
			if (this._wander != null)
			{
				this._wander.enabled = false;
			}
			if (this._controller != null)
			{
				this._controller.CancelManualModeForEncounter();
				this._controller.SuppressRootMotionPosition = this._suppressRootMotion;
			}
			if (!this._resumeWandering)
			{
				if (this._parent != null && this._npc.transform.parent == this._parent)
				{
					this._npc.transform.localPosition = this._localPosition;
					this._npc.transform.localRotation = this._localRotation;
				}
				else
				{
					this._npc.transform.SetPositionAndRotation(this._worldPosition, this._worldRotation);
				}
			}
			if (this._controller != null)
			{
				this._controller.SetPosture(this._resumeWandering ? NPCInterestPoint.PostureType.Standing : NPCInterestPoint.PostureType.Sitting);
				this._controller.SetAction(this._resumeWandering ? NPCInterestPoint.ActionType.None : this._action);
			}
			foreach (NPCSeatedEncounter.BehaviourState behaviourState in this._behaviours)
			{
				if (!(behaviourState.Component == null))
				{
					behaviourState.Component.enabled = behaviourState.Enabled;
				}
			}
			if (this._follower != null)
			{
				if (this._resumeWandering && this._wasManualMode)
				{
					this._follower.enabled = true;
				}
				if (this._follower.enabled && this._follower.entityExists)
				{
					this._follower.Teleport(this._npc.transform.position, true);
					this._follower.isStopped = !this._resumeWandering;
				}
			}
			if (this._animator != null && this._animator.runtimeAnimatorController != null)
			{
				this._animator.applyRootMotion = this._rootMotion;
				NPCSeatedEncounter.SetInt(this._animator, "ReactionType", 0);
				NPCSeatedEncounter.SetInt(this._animator, "PostureState", this._resumeWandering ? 0 : 1);
				NPCSeatedEncounter.SetInt(this._animator, "UpperBodyAction", this._resumeWandering ? 0 : this._animatorAction);
				if (this._layerWeights != null)
				{
					for (int i = 0; i < Mathf.Min(this._layerWeights.Length, this._animator.layerCount); i++)
					{
						string layerName = this._animator.GetLayerName(i);
						if (!this._resumeWandering || (!(layerName == "Reactions") && !(layerName == "AlertBehavior")))
						{
							this._animator.SetLayerWeight(i, this._layerWeights[i]);
						}
					}
				}
				if (this._resumeWandering)
				{
					NPCSeatedEncounter.PlayState(this._animator, "Base Layer.Standing.Normal Movement", "Base Layer.Normal Movement");
				}
				else if (this._baseState != 0 && this._animator.HasState(0, this._baseState))
				{
					this._animator.Play(this._baseState, 0, this._baseTime);
					this._animator.Update(0f);
				}
				else
				{
					NPCSeatedEncounter.PlayState(this._animator, "Base Layer.Standing.SitIdle", "Base Layer.SitIdle");
				}
				this._animator.cullingMode = this._culling;
			}
			if (this._equipment != null)
			{
				this._equipment.DeactivateAllProps();
				this._equipment.SetAction(this._resumeWandering ? 0 : this._animatorAction);
				if (!this._resumeWandering)
				{
					foreach (string text in this._props)
					{
						this._equipment.ActivateAndLockProp(text);
					}
				}
			}
			Physics.SyncTransforms();
		}

		// Token: 0x0600724D RID: 29261 RVA: 0x002E87D4 File Offset: 0x002E69D4
		private void OnWorldOriginShifted(Vector3 delta)
		{
			this._worldPosition += delta;
		}

		// Token: 0x0600724E RID: 29262 RVA: 0x002E87E8 File Offset: 0x002E69E8
		private static int ReadInt(Animator animator, string name)
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.name == name && animatorControllerParameter.type == AnimatorControllerParameterType.Int)
				{
					return animator.GetInteger(animatorControllerParameter.nameHash);
				}
			}
			return 0;
		}

		// Token: 0x0600724F RID: 29263 RVA: 0x002E8834 File Offset: 0x002E6A34
		private static void SetInt(Animator animator, string name, int value)
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.name == name && animatorControllerParameter.type == AnimatorControllerParameterType.Int)
				{
					animator.SetInteger(animatorControllerParameter.nameHash, value);
					return;
				}
			}
		}

		// Token: 0x06007250 RID: 29264 RVA: 0x002E8880 File Offset: 0x002E6A80
		private static void PlayState(Animator animator, string path, string fallback)
		{
			int num = Animator.StringToHash(path);
			if (!animator.HasState(0, num))
			{
				num = Animator.StringToHash(fallback);
			}
			if (animator.HasState(0, num))
			{
				animator.Play(num, 0, 0f);
			}
			animator.Update(0f);
		}

		// Token: 0x06007251 RID: 29265 RVA: 0x002E88C8 File Offset: 0x002E6AC8
		public static bool TryFindStandingPosition(Transform npc, Transform player, out Vector3 position)
		{
			position = ((npc != null) ? npc.position : Vector3.zero);
			if (npc == null)
			{
				return false;
			}
			Physics.SyncTransforms();
			Vector3 vector = Vector3.ProjectOnPlane(npc.forward, Vector3.up).normalized;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			Vector3 position2 = npc.position;
			for (float num = 0.8f; num <= 3.21f; num += 0.4f)
			{
				for (int i = 0; i < 16; i++)
				{
					float num2 = (float)((i + 1) / 2) * 22.5f * ((i % 2 == 0) ? (-1f) : 1f);
					Vector3 vector2 = position2 + Quaternion.AngleAxis(num2, Vector3.up) * vector * num;
					Vector3 vector3;
					if (NPCSeatedEncounter.TryFloor(npc, vector2, out vector3) && vector3.y <= position2.y + 0.25f && vector3.y >= position2.y - 3f)
					{
						Vector3 vector4 = vector3 + Vector3.up * 0.04f;
						Collider collider;
						if (!PoolWaterRecovery.TryFindWater(vector4, out collider, 0.35f, true) && DetectionEncounterPlacement.HasStandingClearance(npc, vector4, 0.32f))
						{
							Vector3 vector5 = vector4 - position2;
							RaycastHit[] array = Physics.RaycastAll(position2 + Vector3.up * 1.05f, vector5.normalized, vector5.magnitude, -1, QueryTriggerInteraction.Ignore);
							bool flag = false;
							foreach (RaycastHit raycastHit in array)
							{
								if (!raycastHit.transform.IsChildOf(npc))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								if (position2.y - vector4.y > 0.8f)
								{
									Vector3 vector6 = new Vector3(position2.x, vector4.y + 1.5f, position2.z);
									Vector3 vector7 = vector4 + Vector3.up * 1.5f - vector6;
									foreach (RaycastHit raycastHit2 in Physics.RaycastAll(vector6, vector7.normalized, vector7.magnitude, -1, QueryTriggerInteraction.Ignore))
									{
										if (!raycastHit2.transform.IsChildOf(npc))
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										goto IL_02A2;
									}
								}
								if (!(player != null) || Mathf.Abs(player.position.y - vector4.y) >= 1.5f || Vector3.ProjectOnPlane(player.position - vector4, Vector3.up).sqrMagnitude >= 0.49f)
								{
									position = vector4;
									return true;
								}
							}
						}
					}
					IL_02A2:;
				}
			}
			return false;
		}

		// Token: 0x06007252 RID: 29266 RVA: 0x002E8B9C File Offset: 0x002E6D9C
		private static bool TryFloor(Transform npc, Vector3 candidate, out Vector3 ground)
		{
			ground = candidate;
			RaycastHit[] array = Physics.RaycastAll(candidate + Vector3.up * 0.25f, Vector3.down, 3.25f, -1, QueryTriggerInteraction.Ignore);
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			RaycastHit[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				RaycastHit raycastHit = array2[i];
				if (!raycastHit.transform.IsChildOf(npc))
				{
					if (raycastHit.normal.y < 0.65f)
					{
						return false;
					}
					ground = raycastHit.point;
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x0400755A RID: 30042
		private static readonly HashSet<NPCSenses> Prepared = new HashSet<NPCSenses>();

		// Token: 0x0400755B RID: 30043
		private const string StandingState = "Base Layer.Standing.Normal Movement";

		// Token: 0x0400755C RID: 30044
		private const string SeatedState = "Base Layer.Standing.SitIdle";

		// Token: 0x0400755D RID: 30045
		private const float Radius = 0.32f;

		// Token: 0x0400755E RID: 30046
		private readonly NPCSenses _npc;

		// Token: 0x0400755F RID: 30047
		private readonly Transform _parent;

		// Token: 0x04007560 RID: 30048
		private readonly Vector3 _localPosition;

		// Token: 0x04007561 RID: 30049
		private readonly Quaternion _localRotation;

		// Token: 0x04007562 RID: 30050
		private Vector3 _worldPosition;

		// Token: 0x04007563 RID: 30051
		private readonly Quaternion _worldRotation;

		// Token: 0x04007564 RID: 30052
		private readonly bool _resumeWandering;

		// Token: 0x04007565 RID: 30053
		private readonly AStarNPCController _controller;

		// Token: 0x04007566 RID: 30054
		private readonly FollowerEntity _follower;

		// Token: 0x04007567 RID: 30055
		private readonly NPCWanderController _wander;

		// Token: 0x04007568 RID: 30056
		private readonly Animator _animator;

		// Token: 0x04007569 RID: 30057
		private readonly NPCEquipmentManager _equipment;

		// Token: 0x0400756A RID: 30058
		private readonly bool _wasManualMode;

		// Token: 0x0400756B RID: 30059
		private readonly bool _rootMotion;

		// Token: 0x0400756C RID: 30060
		private readonly bool _suppressRootMotion;

		// Token: 0x0400756D RID: 30061
		private readonly AnimatorCullingMode _culling;

		// Token: 0x0400756E RID: 30062
		private readonly NPCInterestPoint.ActionType _action;

		// Token: 0x0400756F RID: 30063
		private readonly int _animatorAction;

		// Token: 0x04007570 RID: 30064
		private readonly int _baseState;

		// Token: 0x04007571 RID: 30065
		private readonly float _baseTime;

		// Token: 0x04007572 RID: 30066
		private readonly float[] _layerWeights;

		// Token: 0x04007573 RID: 30067
		private readonly string[] _props;

		// Token: 0x04007574 RID: 30068
		private readonly List<NPCSeatedEncounter.BehaviourState> _behaviours = new List<NPCSeatedEncounter.BehaviourState>();

		// Token: 0x04007575 RID: 30069
		private bool _restored;

		// Token: 0x02000EEA RID: 3818
		private struct BehaviourState
		{
			// Token: 0x04007576 RID: 30070
			public Behaviour Component;

			// Token: 0x04007577 RID: 30071
			public bool Enabled;
		}
	}
}
