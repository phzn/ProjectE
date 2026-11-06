using System;
using System.Collections.Generic;
using System.Reflection;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using Pathfinding;
using Pathfinding.ECS;
using UnityEngine;
using UnityEngine.AI;

namespace Features.NPC
{
	// Token: 0x02000EEF RID: 3823
	[DefaultExecutionOrder(32000)]
	internal sealed class NPCSimulationPauseDriver : MonoBehaviour
	{
		// Token: 0x06007282 RID: 29314 RVA: 0x002E99CE File Offset: 0x002E7BCE
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
		}

		// Token: 0x06007283 RID: 29315 RVA: 0x002E99E1 File Offset: 0x002E7BE1
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
		}

		// Token: 0x06007284 RID: 29316 RVA: 0x002E99F4 File Offset: 0x002E7BF4
		private void OnWorldOriginShifted(Vector3 delta)
		{
			foreach (NPCSimulationPauseDriver.ParticipantState participantState in this._participants.Values)
			{
				participantState.ApplyWorldOriginShift(delta);
			}
			foreach (NPCSimulationPauseDriver.ParticipantState participantState2 in this._ragdollDeferred.Values)
			{
				participantState2.ApplyWorldOriginShift(delta);
			}
		}

		// Token: 0x06007285 RID: 29317 RVA: 0x002E9A90 File Offset: 0x002E7C90
		internal static NPCSimulationPauseDriver GetOrCreate()
		{
			if (NPCSimulationPauseDriver._instance != null)
			{
				return NPCSimulationPauseDriver._instance;
			}
			GameObject gameObject = new GameObject("[NPC Simulation Pause]");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			Object.DontDestroyOnLoad(gameObject);
			NPCSimulationPauseDriver._instance = gameObject.AddComponent<NPCSimulationPauseDriver>();
			return NPCSimulationPauseDriver._instance;
		}

		// Token: 0x06007286 RID: 29318 RVA: 0x002E9ACC File Offset: 0x002E7CCC
		internal static void EndCurrentPause()
		{
			if (NPCSimulationPauseDriver._instance != null)
			{
				NPCSimulationPauseDriver._instance.EndPause();
			}
		}

		// Token: 0x06007287 RID: 29319 RVA: 0x002E9AE5 File Offset: 0x002E7CE5
		internal static void ParticipantDisabledCurrent(Transform root)
		{
			if (NPCSimulationPauseDriver._instance != null)
			{
				NPCSimulationPauseDriver._instance.ParticipantDisabled(root);
			}
		}

		// Token: 0x06007288 RID: 29320 RVA: 0x002E9AFF File Offset: 0x002E7CFF
		internal static void PrepareForRagdollCurrent(Transform root)
		{
			if (NPCSimulationPauseDriver._instance != null)
			{
				NPCSimulationPauseDriver._instance.PrepareForRagdoll(root);
			}
		}

		// Token: 0x06007289 RID: 29321 RVA: 0x002E9B19 File Offset: 0x002E7D19
		internal static void RestoreAfterRagdollCurrent(Transform root, NPCRagdollRecovery recovery)
		{
			if (NPCSimulationPauseDriver._instance != null)
			{
				NPCSimulationPauseDriver._instance.RestoreAfterRagdoll(root, recovery);
			}
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x002E9B34 File Offset: 0x002E7D34
		internal static void ResetStaticState()
		{
			if (NPCSimulationPauseDriver._instance == null)
			{
				return;
			}
			NPCSimulationPauseDriver._instance.RestoreEverything();
			NPCSimulationPauseDriver._instance = null;
		}

		// Token: 0x0600728B RID: 29323 RVA: 0x002E9B54 File Offset: 0x002E7D54
		internal void BeginPause()
		{
			if (this._paused)
			{
				return;
			}
			this._paused = true;
			this.CaptureActiveParticipants();
			this._nextComponentHoldRefresh = Time.unscaledTime + 0.5f;
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x002E9B80 File Offset: 0x002E7D80
		internal void EndPause()
		{
			if (!this._paused)
			{
				return;
			}
			foreach (KeyValuePair<int, NPCSimulationPauseDriver.ParticipantState> keyValuePair in this._participants)
			{
				NPCSimulationPauseDriver.ParticipantState value = keyValuePair.Value;
				if (value.IsRagdollRecovering)
				{
					this._ragdollDeferred[keyValuePair.Key] = value;
				}
				else
				{
					value.Restore();
				}
			}
			this._participants.Clear();
			this._removeBuffer.Clear();
			this._paused = false;
			this.RestoreCompletedDeferredRagdolls();
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x002E9C24 File Offset: 0x002E7E24
		internal void Register(Component component)
		{
			if (!this._paused || component == null)
			{
				return;
			}
			Transform transform = NPCSimulationPauseDriver.ResolveNpcRoot(component);
			if (transform == null || !transform.gameObject.scene.IsValid() || !transform.gameObject.activeInHierarchy)
			{
				return;
			}
			this.RegisterRoot(transform);
		}

		// Token: 0x0600728E RID: 29326 RVA: 0x002E9C80 File Offset: 0x002E7E80
		private void LateUpdate()
		{
			if (!this._paused)
			{
				return;
			}
			bool flag = Time.unscaledTime >= this._nextComponentHoldRefresh;
			if (flag)
			{
				this._nextComponentHoldRefresh = Time.unscaledTime + 0.5f;
			}
			this.MaintainAllParticipants(flag);
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x002E9CC4 File Offset: 0x002E7EC4
		private void CaptureActiveParticipants()
		{
			this.RegisterAll<AStarNPCController>(Object.FindObjectsByType<AStarNPCController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<NPCStateMachine>(Object.FindObjectsByType<NPCStateMachine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<NPCSimpleLookAt>(Object.FindObjectsByType<NPCSimpleLookAt>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<FieldPlayerLookAtCiel>(Object.FindObjectsByType<FieldPlayerLookAtCiel>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<SeatedRandomReaction>(Object.FindObjectsByType<SeatedRandomReaction>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<NPCRagdollRecovery>(Object.FindObjectsByType<NPCRagdollRecovery>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<NPCStumbleHandler>(Object.FindObjectsByType<NPCStumbleHandler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<GuardPatrol>(Object.FindObjectsByType<GuardPatrol>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<SimpleAStarWander>(Object.FindObjectsByType<SimpleAStarWander>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			this.RegisterAll<NPCDiveController>(Object.FindObjectsByType<NPCDiveController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x002E9D54 File Offset: 0x002E7F54
		private void RegisterAll<T>(T[] components) where T : Component
		{
			for (int i = 0; i < components.Length; i++)
			{
				this.Register(components[i]);
			}
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x002E9D84 File Offset: 0x002E7F84
		private void RegisterRoot(Transform root)
		{
			int instanceID = root.GetInstanceID();
			if (this._participants.ContainsKey(instanceID) || this._ragdollDeferred.ContainsKey(instanceID))
			{
				return;
			}
			NPCRagdollRecovery component = root.GetComponent<NPCRagdollRecovery>();
			if (component != null && component.IsRecovering)
			{
				return;
			}
			NPCSimulationPauseDriver.ParticipantState participantState = new NPCSimulationPauseDriver.ParticipantState(root);
			this._participants.Add(instanceID, participantState);
			participantState.Hold();
			if (root.GetComponent<NPCSimulationPauseParticipantHook>() == null)
			{
				root.gameObject.AddComponent<NPCSimulationPauseParticipantHook>();
			}
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x002E9E04 File Offset: 0x002E8004
		private void MaintainAllParticipants(bool refreshComponentHold)
		{
			this._removeBuffer.Clear();
			foreach (KeyValuePair<int, NPCSimulationPauseDriver.ParticipantState> keyValuePair in this._participants)
			{
				NPCSimulationPauseDriver.ParticipantState value = keyValuePair.Value;
				if (!value.IsActive)
				{
					value.Restore();
					this._removeBuffer.Add(keyValuePair.Key);
				}
				else
				{
					value.PinRootIfDisplaced();
					if (refreshComponentHold)
					{
						value.HoldComponents();
					}
				}
			}
			for (int i = 0; i < this._removeBuffer.Count; i++)
			{
				this._participants.Remove(this._removeBuffer[i]);
			}
		}

		// Token: 0x06007293 RID: 29331 RVA: 0x002E9EC4 File Offset: 0x002E80C4
		internal void ParticipantDisabled(Transform root)
		{
			if (root == null)
			{
				return;
			}
			int instanceID = root.GetInstanceID();
			NPCSimulationPauseDriver.ParticipantState participantState;
			if (this._participants.TryGetValue(instanceID, out participantState))
			{
				participantState.Restore();
				this._participants.Remove(instanceID);
			}
			NPCSimulationPauseDriver.ParticipantState participantState2;
			if (this._ragdollDeferred.TryGetValue(instanceID, out participantState2))
			{
				participantState2.Restore();
				this._ragdollDeferred.Remove(instanceID);
			}
		}

		// Token: 0x06007294 RID: 29332 RVA: 0x002E9F28 File Offset: 0x002E8128
		private void PrepareForRagdoll(Transform root)
		{
			if (!this._paused || root == null)
			{
				return;
			}
			int instanceID = root.GetInstanceID();
			NPCSimulationPauseDriver.ParticipantState participantState;
			if (!this._participants.TryGetValue(instanceID, out participantState))
			{
				return;
			}
			participantState.RestoreFollowerForRagdollSnapshot();
			this._participants.Remove(instanceID);
			this._ragdollDeferred[instanceID] = participantState;
		}

		// Token: 0x06007295 RID: 29333 RVA: 0x002E9F80 File Offset: 0x002E8180
		private void RestoreAfterRagdoll(Transform root, NPCRagdollRecovery recovery)
		{
			if (root == null)
			{
				return;
			}
			int instanceID = root.GetInstanceID();
			NPCSimulationPauseDriver.ParticipantState participantState;
			if (this._ragdollDeferred.TryGetValue(instanceID, out participantState))
			{
				participantState.Restore();
				this._ragdollDeferred.Remove(instanceID);
			}
			if (this._paused && root.gameObject.activeInHierarchy && (recovery == null || !recovery.IsRecovering))
			{
				this.RegisterRoot(root);
			}
		}

		// Token: 0x06007296 RID: 29334 RVA: 0x002E9FF0 File Offset: 0x002E81F0
		private void RestoreCompletedDeferredRagdolls()
		{
			this._removeBuffer.Clear();
			foreach (KeyValuePair<int, NPCSimulationPauseDriver.ParticipantState> keyValuePair in this._ragdollDeferred)
			{
				NPCSimulationPauseDriver.ParticipantState value = keyValuePair.Value;
				if (!value.IsRagdollRecovering)
				{
					value.Restore();
					this._removeBuffer.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this._removeBuffer.Count; i++)
			{
				this._ragdollDeferred.Remove(this._removeBuffer[i]);
			}
			this._removeBuffer.Clear();
		}

		// Token: 0x06007297 RID: 29335 RVA: 0x002EA0A8 File Offset: 0x002E82A8
		private void RestoreEverything()
		{
			foreach (NPCSimulationPauseDriver.ParticipantState participantState in this._participants.Values)
			{
				participantState.Restore();
			}
			foreach (NPCSimulationPauseDriver.ParticipantState participantState2 in this._ragdollDeferred.Values)
			{
				participantState2.Restore();
			}
			this._participants.Clear();
			this._ragdollDeferred.Clear();
			this._removeBuffer.Clear();
			this._paused = false;
		}

		// Token: 0x06007298 RID: 29336 RVA: 0x002EA16C File Offset: 0x002E836C
		private static Transform ResolveNpcRoot(Component component)
		{
			AStarNPCController componentInParent = component.GetComponentInParent<AStarNPCController>(true);
			if (componentInParent != null)
			{
				return componentInParent.transform;
			}
			NPCStateMachine componentInParent2 = component.GetComponentInParent<NPCStateMachine>(true);
			if (componentInParent2 != null)
			{
				return componentInParent2.transform;
			}
			NPCRagdollRecovery componentInParent3 = component.GetComponentInParent<NPCRagdollRecovery>(true);
			if (componentInParent3 != null)
			{
				return componentInParent3.transform;
			}
			NPCStumbleHandler componentInParent4 = component.GetComponentInParent<NPCStumbleHandler>(true);
			if (componentInParent4 != null)
			{
				return componentInParent4.transform;
			}
			return component.transform;
		}

		// Token: 0x040075A4 RID: 30116
		private const float ComponentHoldRefreshInterval = 0.5f;

		// Token: 0x040075A5 RID: 30117
		private static NPCSimulationPauseDriver _instance;

		// Token: 0x040075A6 RID: 30118
		private readonly Dictionary<int, NPCSimulationPauseDriver.ParticipantState> _participants = new Dictionary<int, NPCSimulationPauseDriver.ParticipantState>(128);

		// Token: 0x040075A7 RID: 30119
		private readonly Dictionary<int, NPCSimulationPauseDriver.ParticipantState> _ragdollDeferred = new Dictionary<int, NPCSimulationPauseDriver.ParticipantState>(16);

		// Token: 0x040075A8 RID: 30120
		private readonly List<int> _removeBuffer = new List<int>(16);

		// Token: 0x040075A9 RID: 30121
		private bool _paused;

		// Token: 0x040075AA RID: 30122
		private float _nextComponentHoldRefresh;

		// Token: 0x02000EF0 RID: 3824
		private sealed class ParticipantState
		{
			// Token: 0x170012AE RID: 4782
			// (get) Token: 0x0600729A RID: 29338 RVA: 0x002EA211 File Offset: 0x002E8411
			internal bool IsActive
			{
				get
				{
					return this._root != null && this._root.gameObject.activeInHierarchy;
				}
			}

			// Token: 0x170012AF RID: 4783
			// (get) Token: 0x0600729B RID: 29339 RVA: 0x002EA233 File Offset: 0x002E8433
			internal bool IsRagdollRecovering
			{
				get
				{
					return this._ragdollRecovery != null && this._ragdollRecovery.IsRecovering;
				}
			}

			// Token: 0x0600729C RID: 29340 RVA: 0x002EA250 File Offset: 0x002E8450
			internal ParticipantState(Transform root)
			{
				this._root = root;
				this._position = root.position;
				this._rotation = root.rotation;
				this._ragdollRecovery = root.GetComponent<NPCRagdollRecovery>();
				FollowerEntity component = root.GetComponent<FollowerEntity>();
				this._follower = ((component != null) ? new NPCSimulationPauseDriver.FollowerState(component) : null);
				NavMeshAgent[] componentsInChildren = root.GetComponentsInChildren<NavMeshAgent>(true);
				this._agents = new NPCSimulationPauseDriver.AgentState[componentsInChildren.Length];
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this._agents[i] = new NPCSimulationPauseDriver.AgentState(componentsInChildren[i]);
				}
				Animator[] componentsInChildren2 = root.GetComponentsInChildren<Animator>(true);
				this._animators = new NPCSimulationPauseDriver.AnimatorState[componentsInChildren2.Length];
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					this._animators[j] = new NPCSimulationPauseDriver.AnimatorState(componentsInChildren2[j]);
				}
				Rigidbody[] componentsInChildren3 = root.GetComponentsInChildren<Rigidbody>(true);
				List<NPCSimulationPauseDriver.BodyState> list = new List<NPCSimulationPauseDriver.BodyState>(componentsInChildren3.Length);
				if (!(this._ragdollRecovery != null) || !this._ragdollRecovery.IsPhysicallyRagdolled)
				{
					foreach (Rigidbody rigidbody in componentsInChildren3)
					{
						if (rigidbody != null && !rigidbody.isKinematic && rigidbody.gameObject.activeInHierarchy)
						{
							list.Add(new NPCSimulationPauseDriver.BodyState(rigidbody));
						}
					}
				}
				this._dynamicBodies = list.ToArray();
			}

			// Token: 0x0600729D RID: 29341 RVA: 0x002EA3A5 File Offset: 0x002E85A5
			internal void Hold()
			{
				this.PinRootIfDisplaced();
				this.HoldComponents();
			}

			// Token: 0x0600729E RID: 29342 RVA: 0x002EA3B3 File Offset: 0x002E85B3
			internal void ApplyWorldOriginShift(Vector3 delta)
			{
				this._position += delta;
			}

			// Token: 0x0600729F RID: 29343 RVA: 0x002EA3C8 File Offset: 0x002E85C8
			internal void PinRootIfDisplaced()
			{
				if (this._root == null)
				{
					return;
				}
				if ((!(this._ragdollRecovery != null) || !this._ragdollRecovery.IsPhysicallyRagdolled) && (this._root.position != this._position || this._root.rotation != this._rotation))
				{
					this._root.SetPositionAndRotation(this._position, this._rotation);
				}
			}

			// Token: 0x060072A0 RID: 29344 RVA: 0x002EA44C File Offset: 0x002E864C
			internal void HoldComponents()
			{
				if (this._root == null)
				{
					return;
				}
				NPCSimulationPauseDriver.FollowerState follower = this._follower;
				if (follower != null)
				{
					follower.Hold();
				}
				for (int i = 0; i < this._agents.Length; i++)
				{
					this._agents[i].Hold();
				}
				for (int j = 0; j < this._animators.Length; j++)
				{
					this._animators[j].Hold();
				}
				for (int k = 0; k < this._dynamicBodies.Length; k++)
				{
					this._dynamicBodies[k].Hold();
				}
			}

			// Token: 0x060072A1 RID: 29345 RVA: 0x002EA4DC File Offset: 0x002E86DC
			internal void Restore()
			{
				NPCSimulationPauseDriver.FollowerState follower = this._follower;
				if (follower != null)
				{
					follower.Restore();
				}
				for (int i = 0; i < this._agents.Length; i++)
				{
					this._agents[i].Restore();
				}
				for (int j = 0; j < this._animators.Length; j++)
				{
					this._animators[j].Restore();
				}
				for (int k = 0; k < this._dynamicBodies.Length; k++)
				{
					this._dynamicBodies[k].Restore();
				}
			}

			// Token: 0x060072A2 RID: 29346 RVA: 0x002EA55A File Offset: 0x002E875A
			internal void RestoreFollowerForRagdollSnapshot()
			{
				NPCSimulationPauseDriver.FollowerState follower = this._follower;
				if (follower == null)
				{
					return;
				}
				follower.Restore();
			}

			// Token: 0x040075AB RID: 30123
			private readonly Transform _root;

			// Token: 0x040075AC RID: 30124
			private Vector3 _position;

			// Token: 0x040075AD RID: 30125
			private readonly Quaternion _rotation;

			// Token: 0x040075AE RID: 30126
			private readonly NPCSimulationPauseDriver.FollowerState _follower;

			// Token: 0x040075AF RID: 30127
			private readonly NPCSimulationPauseDriver.AgentState[] _agents;

			// Token: 0x040075B0 RID: 30128
			private readonly NPCSimulationPauseDriver.AnimatorState[] _animators;

			// Token: 0x040075B1 RID: 30129
			private readonly NPCSimulationPauseDriver.BodyState[] _dynamicBodies;

			// Token: 0x040075B2 RID: 30130
			private readonly NPCRagdollRecovery _ragdollRecovery;
		}

		// Token: 0x02000EF1 RID: 3825
		private sealed class FollowerState
		{
			// Token: 0x060072A3 RID: 29347 RVA: 0x002EA56C File Offset: 0x002E876C
			internal FollowerState(FollowerEntity value)
			{
				this._value = value;
				this.TryCapture();
			}

			// Token: 0x060072A4 RID: 29348 RVA: 0x002EA584 File Offset: 0x002E8784
			private bool TryCapture()
			{
				if (this._value == null || !this._value.entityExists)
				{
					return false;
				}
				if (this._captured)
				{
					return true;
				}
				this._captured = true;
				this._stopped = this._value.isStopped;
				this._maxSpeed = this._value.maxSpeed;
				this._simulateMovement = this._value.simulateMovement;
				this._updatePosition = this._value.updatePosition;
				this._updateRotation = this._value.updateRotation;
				return true;
			}

			// Token: 0x060072A5 RID: 29349 RVA: 0x002EA618 File Offset: 0x002E8818
			internal void Hold()
			{
				if (!this.TryCapture())
				{
					return;
				}
				if (!Mathf.Approximately(this._value.maxSpeed, 0f))
				{
					this._value.maxSpeed = 0f;
				}
				if (!this._value.isStopped)
				{
					this._value.isStopped = true;
				}
				if (this._value.updatePosition)
				{
					this._value.updatePosition = false;
				}
				if (this._value.updateRotation)
				{
					this._value.updateRotation = false;
				}
				if (this._value.simulateMovement)
				{
					this._value.simulateMovement = false;
				}
			}

			// Token: 0x060072A6 RID: 29350 RVA: 0x002EA6BC File Offset: 0x002E88BC
			internal void Restore()
			{
				if (!this._captured || this._value == null)
				{
					return;
				}
				if (!this._value.entityExists)
				{
					MovementSettings movementSettings = this._value.movementSettings;
					movementSettings.follower.speed = this._maxSpeed;
					movementSettings.isStopped = this._stopped;
					NPCSimulationPauseDriver.FollowerState.MovementField.SetValue(this._value, movementSettings);
					NPCSimulationPauseDriver.FollowerState.SyncPositionField.SetValue(this._value, this._updatePosition);
					NPCSimulationPauseDriver.FollowerState.SyncRotationField.SetValue(this._value, this._updateRotation);
					return;
				}
				this._value.maxSpeed = this._maxSpeed;
				this._value.isStopped = this._stopped;
				this._value.updatePosition = this._updatePosition;
				this._value.updateRotation = this._updateRotation;
				this._value.simulateMovement = this._simulateMovement;
			}

			// Token: 0x040075B3 RID: 30131
			private const BindingFlags PrivateInstance = BindingFlags.Instance | BindingFlags.NonPublic;

			// Token: 0x040075B4 RID: 30132
			private static readonly FieldInfo SyncPositionField = typeof(FollowerEntity).GetField("syncPosition", BindingFlags.Instance | BindingFlags.NonPublic);

			// Token: 0x040075B5 RID: 30133
			private static readonly FieldInfo SyncRotationField = typeof(FollowerEntity).GetField("syncRotation", BindingFlags.Instance | BindingFlags.NonPublic);

			// Token: 0x040075B6 RID: 30134
			private static readonly FieldInfo MovementField = typeof(FollowerEntity).GetField("movement", BindingFlags.Instance | BindingFlags.NonPublic);

			// Token: 0x040075B7 RID: 30135
			private readonly FollowerEntity _value;

			// Token: 0x040075B8 RID: 30136
			private bool _stopped;

			// Token: 0x040075B9 RID: 30137
			private float _maxSpeed;

			// Token: 0x040075BA RID: 30138
			private bool _captured;

			// Token: 0x040075BB RID: 30139
			private bool _simulateMovement;

			// Token: 0x040075BC RID: 30140
			private bool _updatePosition;

			// Token: 0x040075BD RID: 30141
			private bool _updateRotation;
		}

		// Token: 0x02000EF2 RID: 3826
		private sealed class AgentState
		{
			// Token: 0x060072A8 RID: 29352 RVA: 0x002EA81A File Offset: 0x002E8A1A
			internal AgentState(NavMeshAgent value)
			{
				this._value = value;
				this.TryCapture();
			}

			// Token: 0x060072A9 RID: 29353 RVA: 0x002EA830 File Offset: 0x002E8A30
			private bool TryCapture()
			{
				if (this._captured)
				{
					return true;
				}
				if (this._value == null || !this._value.enabled || !this._value.isOnNavMesh)
				{
					return false;
				}
				this._captured = true;
				this._stopped = this._value.isStopped;
				this._updatePosition = this._value.updatePosition;
				this._updateRotation = this._value.updateRotation;
				this._velocity = this._value.velocity;
				return true;
			}

			// Token: 0x060072AA RID: 29354 RVA: 0x002EA8C0 File Offset: 0x002E8AC0
			internal void Hold()
			{
				if (!this.TryCapture())
				{
					return;
				}
				if (this._value == null || !this._value.enabled || !this._value.isOnNavMesh)
				{
					return;
				}
				this._value.isStopped = true;
				this._value.updatePosition = false;
				this._value.updateRotation = false;
				this._value.velocity = Vector3.zero;
			}

			// Token: 0x060072AB RID: 29355 RVA: 0x002EA934 File Offset: 0x002E8B34
			internal void Restore()
			{
				if (!this._captured || this._value == null || !this._value.enabled || !this._value.isOnNavMesh)
				{
					return;
				}
				this._value.isStopped = this._stopped;
				this._value.updatePosition = this._updatePosition;
				this._value.updateRotation = this._updateRotation;
				this._value.velocity = this._velocity;
			}

			// Token: 0x040075BE RID: 30142
			private readonly NavMeshAgent _value;

			// Token: 0x040075BF RID: 30143
			private bool _stopped;

			// Token: 0x040075C0 RID: 30144
			private bool _captured;

			// Token: 0x040075C1 RID: 30145
			private bool _updatePosition;

			// Token: 0x040075C2 RID: 30146
			private bool _updateRotation;

			// Token: 0x040075C3 RID: 30147
			private Vector3 _velocity;
		}

		// Token: 0x02000EF3 RID: 3827
		private sealed class AnimatorState
		{
			// Token: 0x060072AC RID: 29356 RVA: 0x002EA9B6 File Offset: 0x002E8BB6
			internal AnimatorState(Animator value)
			{
				this._value = value;
				this._speed = ((value != null) ? value.speed : 1f);
			}

			// Token: 0x060072AD RID: 29357 RVA: 0x002EA9E1 File Offset: 0x002E8BE1
			internal void Hold()
			{
				if (this._value != null)
				{
					this._value.speed = 0f;
				}
			}

			// Token: 0x060072AE RID: 29358 RVA: 0x002EAA01 File Offset: 0x002E8C01
			internal void Restore()
			{
				if (this._value != null)
				{
					this._value.speed = this._speed;
				}
			}

			// Token: 0x040075C4 RID: 30148
			private readonly Animator _value;

			// Token: 0x040075C5 RID: 30149
			private readonly float _speed;
		}

		// Token: 0x02000EF4 RID: 3828
		private sealed class BodyState
		{
			// Token: 0x060072AF RID: 29359 RVA: 0x002EAA24 File Offset: 0x002E8C24
			internal BodyState(Rigidbody value)
			{
				this._value = value;
				this._kinematic = value.isKinematic;
				this._useGravity = value.useGravity;
				this._sleeping = value.IsSleeping();
				this._velocity = value.velocity;
				this._angularVelocity = value.angularVelocity;
			}

			// Token: 0x060072B0 RID: 29360 RVA: 0x002EAA7C File Offset: 0x002E8C7C
			internal void Hold()
			{
				if (this._value == null)
				{
					return;
				}
				this._value.velocity = Vector3.zero;
				this._value.angularVelocity = Vector3.zero;
				this._value.useGravity = false;
				this._value.isKinematic = true;
			}

			// Token: 0x060072B1 RID: 29361 RVA: 0x002EAAD0 File Offset: 0x002E8CD0
			internal void Restore()
			{
				if (this._value == null)
				{
					return;
				}
				this._value.isKinematic = this._kinematic;
				this._value.useGravity = this._useGravity;
				this._value.velocity = this._velocity;
				this._value.angularVelocity = this._angularVelocity;
				if (this._sleeping)
				{
					this._value.Sleep();
				}
			}

			// Token: 0x040075C6 RID: 30150
			private readonly Rigidbody _value;

			// Token: 0x040075C7 RID: 30151
			private readonly bool _kinematic;

			// Token: 0x040075C8 RID: 30152
			private readonly bool _useGravity;

			// Token: 0x040075C9 RID: 30153
			private readonly bool _sleeping;

			// Token: 0x040075CA RID: 30154
			private readonly Vector3 _velocity;

			// Token: 0x040075CB RID: 30155
			private readonly Vector3 _angularVelocity;
		}
	}
}
