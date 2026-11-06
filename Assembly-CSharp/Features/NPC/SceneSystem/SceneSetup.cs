using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bozo.AnimeCharacters;
using Features.Detection;
using Features.NPC.StateMachine;
using Pathfinding;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F3D RID: 3901
	public class SceneSetup : MonoBehaviour
	{
		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x060074F4 RID: 29940 RVA: 0x002F6B36 File Offset: 0x002F4D36
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x060074F5 RID: 29941 RVA: 0x002F6B3E File Offset: 0x002F4D3E
		public int SpawnedCount
		{
			get
			{
				return this._spawnedCount;
			}
		}

		// Token: 0x060074F6 RID: 29942 RVA: 0x002F6B48 File Offset: 0x002F4D48
		private void Start()
		{
			if (this.runtimeSpawner == null)
			{
				this.runtimeSpawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
			}
			if (this.autoStart)
			{
				if (this.startDelay > 0f)
				{
					base.StartCoroutine(this.DelayedStart());
					return;
				}
				this.SetupScene();
			}
		}

		// Token: 0x060074F7 RID: 29943 RVA: 0x002F6B98 File Offset: 0x002F4D98
		private IEnumerator DelayedStart()
		{
			yield return SceneSetup.WaitForSimulationSeconds(this.startDelay);
			this.SetupScene();
			yield break;
		}

		// Token: 0x060074F8 RID: 29944 RVA: 0x002F6BA8 File Offset: 0x002F4DA8
		public async Task SetupScene()
		{
			if (this._isActive)
			{
				this.Log("Scene already active, call TeardownScene() first.");
			}
			else
			{
				this._markers.Clear();
				base.GetComponentsInChildren<SceneNPCMarker>(true, this._markers);
				if (this._markers.Count == 0)
				{
					this.Log("No SceneNPCMarker found under this GameObject.");
				}
				else
				{
					this.Log(string.Format("Found {0} NPC markers. Spawning...", this._markers.Count));
					this._isActive = true;
					this._spawnedCount = 0;
					if (this._npcContainer == null)
					{
						GameObject gameObject = new GameObject("[SceneNPCs] " + base.gameObject.name);
						this._npcContainer = gameObject.transform;
						this.Log("Created NPC container: " + gameObject.name);
					}
					foreach (SceneNPCMarker sceneNPCMarker in this._markers)
					{
						if (sceneNPCMarker.characterData == null)
						{
							Debug.LogWarning("[SceneSetup] Marker '" + sceneNPCMarker.npcId + "' has no CharacterObject, skipping.");
						}
						else
						{
							await this.SpawnNPCFromMarker(sceneNPCMarker);
						}
					}
					List<SceneNPCMarker>.Enumerator enumerator = default(List<SceneNPCMarker>.Enumerator);
					this.Log(string.Format("Scene setup complete. {0}/{1} NPCs spawned.", this._spawnedCount, this._markers.Count));
					foreach (SceneNPCMarker sceneNPCMarker2 in this._markers)
					{
						if (sceneNPCMarker2.spawnedNPC != null)
						{
							Coroutine coroutine = base.StartCoroutine(this.ExecuteBehaviorPattern(sceneNPCMarker2));
							this._behaviorCoroutines[sceneNPCMarker2.npcId] = coroutine;
						}
					}
				}
			}
		}

		// Token: 0x060074F9 RID: 29945 RVA: 0x002F6BEC File Offset: 0x002F4DEC
		public void TeardownScene()
		{
			if (!this._isActive)
			{
				return;
			}
			this.Log("Tearing down scene...");
			foreach (KeyValuePair<string, Coroutine> keyValuePair in this._behaviorCoroutines)
			{
				if (keyValuePair.Value != null)
				{
					base.StopCoroutine(keyValuePair.Value);
				}
			}
			this._behaviorCoroutines.Clear();
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this._spawnedNPCs)
			{
				if (keyValuePair2.Value != null)
				{
					if (this.runtimeSpawner != null)
					{
						this.runtimeSpawner.DespawnNPC(keyValuePair2.Value, false);
					}
					else
					{
						Object.Destroy(keyValuePair2.Value);
					}
				}
			}
			this._spawnedNPCs.Clear();
			if (this._npcContainer != null)
			{
				Object.Destroy(this._npcContainer.gameObject);
				this._npcContainer = null;
			}
			foreach (SceneNPCMarker sceneNPCMarker in this._markers)
			{
				sceneNPCMarker.spawnedNPC = null;
			}
			this._isActive = false;
			this._spawnedCount = 0;
			this.Log("Scene teardown complete.");
		}

		// Token: 0x060074FA RID: 29946 RVA: 0x002F6D70 File Offset: 0x002F4F70
		private void OnDestroy()
		{
			if (this._isActive)
			{
				this.TeardownScene();
			}
		}

		// Token: 0x060074FB RID: 29947 RVA: 0x002F6D80 File Offset: 0x002F4F80
		private async Task SpawnNPCFromMarker(SceneNPCMarker marker)
		{
			if (this.runtimeSpawner == null)
			{
				Debug.LogError("[SceneSetup] No NPCRuntimeSpawner available!");
			}
			else
			{
				Vector3 vector;
				Quaternion quaternion;
				Transform transform;
				if (marker.spawnMode == SpawnMode.UsePOI && marker.targetPOI != null)
				{
					vector = marker.targetPOI.NavigationPosition;
					quaternion = Quaternion.identity;
					transform = this._npcContainer;
					this.Log("Spawning '" + marker.npcId + "' at POI approach point for " + marker.targetPOI.name);
				}
				else
				{
					vector = marker.transform.position;
					quaternion = (marker.useFacingDirection ? marker.transform.rotation : Quaternion.identity);
					transform = marker.transform;
				}
				GameObject gameObject = await this.runtimeSpawner.SpawnNPC(marker.characterData, vector, quaternion, transform, null, true);
				if (gameObject == null)
				{
					Debug.LogError("[SceneSetup] Failed to spawn NPC for marker '" + marker.npcId + "'");
				}
				else
				{
					gameObject.SetActive(false);
					if (marker.spawnMode == SpawnMode.Direct)
					{
						gameObject.transform.SetParent(marker.transform);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localRotation = Quaternion.identity;
					}
					this.DisableScriptsForScene(gameObject, marker);
					gameObject.SetActive(true);
					NPCSimulationPause.RegisterParticipant(gameObject.transform);
					base.StartCoroutine(this.ApplyScenePoseDeferred(gameObject, marker));
					marker.spawnedNPC = gameObject;
					this._spawnedNPCs[marker.npcId] = gameObject;
					this._spawnedCount++;
					this.Log(string.Format("Spawned '{0}' ({1}) - {2}/{3}", new object[]
					{
						marker.npcId,
						marker.characterData.name,
						marker.spawnMode,
						marker.behaviorPattern
					}));
				}
			}
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x002F6DCC File Offset: 0x002F4FCC
		private void DisableScriptsForScene(GameObject npc, SceneNPCMarker marker)
		{
			int num = 0;
			foreach (Behaviour behaviour in npc.GetComponentsInChildren<Behaviour>(true))
			{
				if (!(behaviour == null) && !(behaviour is Animator) && !(behaviour is OutfitSystem) && !(behaviour is NPCSimulationPauseParticipantHook))
				{
					NPCSenses npcsenses = behaviour as NPCSenses;
					if (npcsenses != null)
					{
						npcsenses.enabled = marker.enableDetection;
						if (!marker.enableDetection)
						{
							num++;
						}
					}
					else
					{
						behaviour.enabled = false;
						num++;
					}
				}
			}
			this.Log(string.Format("'{0}' Disabled {1} components (whitelist: Animator, CapsuleCollider, OutfitSystem, SimulationPauseHook, optional NPCSenses)", marker.npcId, num));
		}

		// Token: 0x060074FD RID: 29949 RVA: 0x002F6E64 File Offset: 0x002F5064
		private IEnumerator ApplyScenePoseDeferred(GameObject npc, SceneNPCMarker marker)
		{
			yield return null;
			Animator componentInChildren = npc.GetComponentInChildren<Animator>();
			if (marker.spawnMode == SpawnMode.Direct)
			{
				NPCInterestPoint.PostureType effectivePosture = this.GetEffectivePosture(marker);
				if (componentInChildren != null)
				{
					componentInChildren.SetInteger(SceneSetup.PostureStateHash, (int)effectivePosture);
					componentInChildren.SetInteger(SceneSetup.UpperBodyActionHash, (int)marker.action);
					componentInChildren.SetFloat(SceneSetup.SpeedHash, 0f);
					if (effectivePosture == NPCInterestPoint.PostureType.Sitting)
					{
						componentInChildren.applyRootMotion = true;
						componentInChildren.Play("SitIdle", 0, 0f);
						componentInChildren.Update(0f);
						this.Log("'" + marker.npcId + "' Direct sit: jumped to SitIdle state");
					}
				}
				this.Log(string.Format("'{0}' Direct pose: posture={1}, action={2}", marker.npcId, effectivePosture, marker.action));
			}
			else if (marker.spawnMode == SpawnMode.UsePOI && componentInChildren != null)
			{
				componentInChildren.SetInteger(SceneSetup.PostureStateHash, 0);
				componentInChildren.SetInteger(SceneSetup.UpperBodyActionHash, 0);
				componentInChildren.SetFloat(SceneSetup.SpeedHash, 0f);
			}
			if (marker.personalityProfile != null)
			{
				NPCDialogueHolder component = npc.GetComponent<NPCDialogueHolder>();
				if (component != null)
				{
					component.PersonalityProfile = marker.personalityProfile;
				}
			}
			yield break;
		}

		// Token: 0x060074FE RID: 29950 RVA: 0x002F6E81 File Offset: 0x002F5081
		private NPCInterestPoint.PostureType GetEffectivePosture(SceneNPCMarker marker)
		{
			if (!(marker != null))
			{
				return NPCInterestPoint.PostureType.Standing;
			}
			return marker.EffectiveInitialPosture;
		}

		// Token: 0x060074FF RID: 29951 RVA: 0x002F6E94 File Offset: 0x002F5094
		private static IEnumerator WaitForSimulationSeconds(float seconds)
		{
			float elapsed = 0f;
			while (elapsed < seconds)
			{
				elapsed += NPCSimulationPause.SimulationDeltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06007500 RID: 29952 RVA: 0x002F6EA3 File Offset: 0x002F50A3
		private IEnumerator ExecuteBehaviorPattern(SceneNPCMarker marker)
		{
			if (marker.startDelay > 0f)
			{
				yield return SceneSetup.WaitForSimulationSeconds(marker.startDelay);
			}
			while (NPCSimulationPause.IsPaused)
			{
				yield return null;
			}
			GameObject spawnedNPC = marker.spawnedNPC;
			if (spawnedNPC == null)
			{
				yield break;
			}
			if (marker.spawnMode == SpawnMode.UsePOI && marker.targetPOI != null)
			{
				this.EnableNavigationComponents(spawnedNPC);
				yield return this.BehaviorUsePOI(marker);
				yield break;
			}
			switch (marker.behaviorPattern)
			{
			case NPCBehaviorPattern.Idle:
				yield break;
			case NPCBehaviorPattern.SitIdle:
				yield break;
			case NPCBehaviorPattern.SitAction:
				yield break;
			case NPCBehaviorPattern.StandAction:
				yield break;
			case NPCBehaviorPattern.Patrol:
				this.EnableNavigationComponents(spawnedNPC);
				yield return this.BehaviorPatrol(marker);
				break;
			case NPCBehaviorPattern.LookAtPlayer:
				this.EnableLookAtComponent(spawnedNPC);
				yield return this.BehaviorLookAtPlayer(marker);
				break;
			case NPCBehaviorPattern.LookAtTarget:
				this.EnableLookAtComponent(spawnedNPC);
				yield return this.BehaviorLookAtTarget(marker);
				break;
			case NPCBehaviorPattern.WalkToAndSit:
				this.EnableNavigationComponents(spawnedNPC);
				yield return this.BehaviorWalkToAndSit(marker);
				break;
			case NPCBehaviorPattern.Custom:
				yield break;
			}
			yield break;
		}

		// Token: 0x06007501 RID: 29953 RVA: 0x002F6EBC File Offset: 0x002F50BC
		private void EnableNavigationComponents(GameObject npc)
		{
			AStarNPCController component = npc.GetComponent<AStarNPCController>();
			if (component != null)
			{
				component.enabled = true;
			}
			FollowerEntity component2 = npc.GetComponent<FollowerEntity>();
			if (component2 != null)
			{
				component2.enabled = true;
			}
			NPCStateMachine component3 = npc.GetComponent<NPCStateMachine>();
			if (component3 != null)
			{
				component3.enabled = true;
			}
			this.Log("'" + npc.name + "' Re-enabled navigation components for behavior pattern");
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x002F6F2C File Offset: 0x002F512C
		private void EnableLookAtComponent(GameObject npc)
		{
			NPCSimpleLookAt component = npc.GetComponent<NPCSimpleLookAt>();
			if (component != null)
			{
				component.enabled = true;
			}
		}

		// Token: 0x06007503 RID: 29955 RVA: 0x002F6F50 File Offset: 0x002F5150
		private IEnumerator BehaviorLookAtPlayer(SceneNPCMarker marker)
		{
			NPCSimpleLookAt component = marker.spawnedNPC.GetComponent<NPCSimpleLookAt>();
			if (component == null)
			{
				yield break;
			}
			Transform transform = null;
			if (SensoryManager.Instance != null)
			{
				StealthActor player = SensoryManager.Instance.GetPlayer();
				if (player != null)
				{
					transform = player.transform;
				}
			}
			if (transform == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					transform = gameObject.transform;
				}
			}
			if (transform != null)
			{
				component.StartLookingAt(transform);
			}
			yield break;
		}

		// Token: 0x06007504 RID: 29956 RVA: 0x002F6F5F File Offset: 0x002F515F
		private IEnumerator BehaviorLookAtTarget(SceneNPCMarker marker)
		{
			NPCSimpleLookAt component = marker.spawnedNPC.GetComponent<NPCSimpleLookAt>();
			if (component == null)
			{
				yield break;
			}
			if (marker.lookAtTarget != null && marker.lookAtTarget.spawnedNPC != null)
			{
				component.StartLookingAt(marker.lookAtTarget.spawnedNPC.transform);
			}
			else if (marker.interactionTarget != null)
			{
				component.StartLookingAt(marker.interactionTarget);
			}
			yield return null;
			yield break;
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x002F6F6E File Offset: 0x002F516E
		private IEnumerator BehaviorPatrol(SceneNPCMarker marker)
		{
			AStarNPCController controller = marker.spawnedNPC.GetComponent<AStarNPCController>();
			if (controller == null || marker.patrolWaypoints == null || marker.patrolWaypoints.Length == 0)
			{
				yield break;
			}
			if (controller.IsNavigationLocked)
			{
				controller.UnlockNavigation(false);
			}
			if (controller.IsManualMode)
			{
				controller.ExitManualMode(false);
			}
			while (NPCSimulationPause.IsPaused || !controller.IsNavigationReady)
			{
				yield return null;
			}
			int waypointIndex = 0;
			for (;;)
			{
				if (NPCSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					Transform wp = marker.patrolWaypoints[waypointIndex];
					if (wp == null)
					{
						waypointIndex = (waypointIndex + 1) % marker.patrolWaypoints.Length;
					}
					else
					{
						controller.SetPosture(NPCInterestPoint.PostureType.Standing);
						controller.SetAction(NPCInterestPoint.ActionType.None);
						controller.SetDestination(wp.position);
						float timeout = 30f;
						float elapsed = 0f;
						while (elapsed < timeout)
						{
							if (NPCSimulationPause.IsPaused)
							{
								yield return null;
							}
							else
							{
								if (Vector3.Distance(controller.transform.position, wp.position) < 0.5f || controller.HasReachedDestination())
								{
									break;
								}
								elapsed += NPCSimulationPause.SimulationDeltaTime;
								yield return null;
							}
						}
						controller.Stop();
						float num = ((marker.actionDuration > 0f) ? marker.actionDuration : Random.Range(2f, 5f));
						yield return SceneSetup.WaitForSimulationSeconds(num);
						waypointIndex = (waypointIndex + 1) % marker.patrolWaypoints.Length;
						wp = null;
					}
				}
			}
			yield break;
		}

		// Token: 0x06007506 RID: 29958 RVA: 0x002F6F7D File Offset: 0x002F517D
		private IEnumerator BehaviorWalkToAndSit(SceneNPCMarker marker)
		{
			AStarNPCController controller = marker.spawnedNPC.GetComponent<AStarNPCController>();
			if (controller == null)
			{
				yield break;
			}
			Transform destination = null;
			if (marker.patrolWaypoints != null && marker.patrolWaypoints.Length != 0)
			{
				destination = marker.patrolWaypoints[0];
			}
			else if (marker.interactionTarget != null)
			{
				destination = marker.interactionTarget;
			}
			if (destination == null)
			{
				yield break;
			}
			if (controller.IsNavigationLocked)
			{
				controller.UnlockNavigation(false);
			}
			if (controller.IsManualMode)
			{
				controller.ExitManualMode(false);
			}
			while (NPCSimulationPause.IsPaused || !controller.IsNavigationReady)
			{
				yield return null;
			}
			controller.SetPosture(NPCInterestPoint.PostureType.Standing);
			controller.SetAction(NPCInterestPoint.ActionType.None);
			controller.SetDestination(destination.position);
			float timeout = 30f;
			float elapsed = 0f;
			while (elapsed < timeout)
			{
				if (NPCSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					if (Vector3.Distance(controller.transform.position, destination.position) < 0.5f || controller.HasReachedDestination())
					{
						break;
					}
					elapsed += NPCSimulationPause.SimulationDeltaTime;
					yield return null;
				}
			}
			controller.Stop();
			marker.spawnedNPC.transform.rotation = destination.rotation;
			yield return null;
			controller.SetPosture(NPCInterestPoint.PostureType.Sitting);
			controller.SetAction(marker.action);
			yield break;
		}

		// Token: 0x06007507 RID: 29959 RVA: 0x002F6F8C File Offset: 0x002F518C
		private IEnumerator BehaviorUsePOI(SceneNPCMarker marker)
		{
			GameObject npc = marker.spawnedNPC;
			AStarNPCController controller = npc.GetComponent<AStarNPCController>();
			NPCInterestPoint poi = marker.targetPOI;
			if (controller == null || poi == null)
			{
				yield break;
			}
			this.Log("'" + marker.npcId + "' UsePOI: starting approach to " + poi.name);
			NPCStateMachine fsm = npc.GetComponent<NPCStateMachine>();
			if (fsm != null)
			{
				fsm.ForceState(NPCStateType.Wandering, "SceneSetup");
			}
			if (controller.IsNavigationLocked)
			{
				controller.UnlockNavigation(false);
			}
			if (controller.IsManualMode)
			{
				controller.ExitManualMode(false);
			}
			while (NPCSimulationPause.IsPaused || !controller.IsNavigationReady)
			{
				yield return null;
			}
			controller.SetPosture(NPCInterestPoint.PostureType.Standing);
			controller.SetAction(NPCInterestPoint.ActionType.None);
			controller.SetDestination(poi.NavigationPosition);
			this.Log(string.Format("'{0}' UsePOI: navigating to {1}", marker.npcId, poi.NavigationPosition));
			float timeout = 30f;
			float elapsed = 0f;
			while (elapsed < timeout)
			{
				if (NPCSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					if (controller.HasReachedDestination() || Vector3.Distance(npc.transform.position, poi.NavigationPosition) < 0.5f)
					{
						break;
					}
					elapsed += NPCSimulationPause.SimulationDeltaTime;
					yield return null;
				}
			}
			controller.Stop();
			this.Log("'" + marker.npcId + "' UsePOI: arrived at navigation point");
			if (poi.HasSeparateApproachPoint)
			{
				controller.EnterManualMode();
				while (NPCSimulationPause.IsPaused || !controller.IsManualModeReady)
				{
					yield return null;
				}
				Vector3 walkTarget = poi.TargetPosition;
				float walkSpeed = 1.5f;
				controller.SetSpeedOverride(walkSpeed);
				this.Log(string.Format("'{0}' UsePOI: walking in to {1}", marker.npcId, walkTarget));
				while (Vector3.Distance(npc.transform.position, walkTarget) > 0.1f)
				{
					if (NPCSimulationPause.IsPaused)
					{
						yield return null;
					}
					else
					{
						float simulationDeltaTime = NPCSimulationPause.SimulationDeltaTime;
						float num = walkSpeed * simulationDeltaTime;
						npc.transform.position = Vector3.MoveTowards(npc.transform.position, walkTarget, num);
						Vector3 vector = walkTarget - npc.transform.position;
						vector.y = 0f;
						if (vector.sqrMagnitude > 0.001f)
						{
							Quaternion quaternion = Quaternion.LookRotation(vector);
							npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, quaternion, 180f * simulationDeltaTime);
						}
						yield return null;
					}
				}
				npc.transform.position = walkTarget;
				controller.ClearSpeedOverride();
				this.Log("'" + marker.npcId + "' UsePOI: walk-in complete");
				walkTarget = default(Vector3);
			}
			if (poi.RequiresAlignment)
			{
				Quaternion targetRotation = poi.TargetRotation;
				float walkSpeed = 2f;
				float alignElapsed = 0f;
				while (alignElapsed < walkSpeed)
				{
					if (NPCSimulationPause.IsPaused)
					{
						yield return null;
					}
					else
					{
						if (Quaternion.Angle(npc.transform.rotation, targetRotation) < 2f)
						{
							npc.transform.rotation = targetRotation;
							break;
						}
						npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, targetRotation, 180f * NPCSimulationPause.SimulationDeltaTime);
						alignElapsed += NPCSimulationPause.SimulationDeltaTime;
						yield return null;
					}
				}
				npc.transform.rotation = targetRotation;
				this.Log("'" + marker.npcId + "' UsePOI: aligned to POI rotation");
				targetRotation = default(Quaternion);
			}
			controller.EnableRootMotion();
			controller.ConfigureForPOI(poi, false);
			if (fsm != null)
			{
				fsm.ForceState(NPCStateType.Frozen, "SceneSetup");
			}
			this.Log(string.Format("'{0}' UsePOI: configured for POI ({1}, action set)", marker.npcId, poi.posture));
			if (marker.action != NPCInterestPoint.ActionType.None)
			{
				controller.SetAction(marker.action);
			}
			yield break;
		}

		// Token: 0x06007508 RID: 29960 RVA: 0x002F6FA4 File Offset: 0x002F51A4
		public GameObject GetNPC(string npcId)
		{
			GameObject gameObject;
			this._spawnedNPCs.TryGetValue(npcId, out gameObject);
			return gameObject;
		}

		// Token: 0x06007509 RID: 29961 RVA: 0x002F6FC1 File Offset: 0x002F51C1
		public Dictionary<string, GameObject> GetAllNPCs()
		{
			return new Dictionary<string, GameObject>(this._spawnedNPCs);
		}

		// Token: 0x0600750A RID: 29962 RVA: 0x002F6FCE File Offset: 0x002F51CE
		public List<SceneNPCMarker> GetMarkers()
		{
			return new List<SceneNPCMarker>(this._markers);
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x002F6FDB File Offset: 0x002F51DB
		private void Log(string message)
		{
			if (this.debugLog)
			{
				Debug.Log("[SceneSetup] " + message);
			}
		}

		// Token: 0x040077DF RID: 30687
		[Header("References")]
		[Tooltip("Reference to the NPCRuntimeSpawner. If null, tries to find one in scene.")]
		[SerializeField]
		private NPCRuntimeSpawner runtimeSpawner;

		// Token: 0x040077E0 RID: 30688
		[Header("Scene Settings")]
		[Tooltip("If true, spawns NPCs automatically on Start")]
		[SerializeField]
		private bool autoStart = true;

		// Token: 0x040077E1 RID: 30689
		[Tooltip("Delay before spawning NPCs (seconds)")]
		[SerializeField]
		private float startDelay;

		// Token: 0x040077E2 RID: 30690
		[Tooltip("If true, logs detailed info")]
		[SerializeField]
		private bool debugLog = true;

		// Token: 0x040077E3 RID: 30691
		[Header("Runtime State")]
		[SerializeField]
		private bool _isActive;

		// Token: 0x040077E4 RID: 30692
		[SerializeField]
		private int _spawnedCount;

		// Token: 0x040077E5 RID: 30693
		private static readonly int PostureStateHash = Animator.StringToHash("PostureState");

		// Token: 0x040077E6 RID: 30694
		private static readonly int UpperBodyActionHash = Animator.StringToHash("UpperBodyAction");

		// Token: 0x040077E7 RID: 30695
		private static readonly int SpeedHash = Animator.StringToHash("Speed");

		// Token: 0x040077E8 RID: 30696
		private List<SceneNPCMarker> _markers = new List<SceneNPCMarker>();

		// Token: 0x040077E9 RID: 30697
		private Dictionary<string, GameObject> _spawnedNPCs = new Dictionary<string, GameObject>();

		// Token: 0x040077EA RID: 30698
		private Dictionary<string, Coroutine> _behaviorCoroutines = new Dictionary<string, Coroutine>();

		// Token: 0x040077EB RID: 30699
		private Transform _npcContainer;
	}
}
