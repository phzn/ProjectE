using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Features.Detection;
using Features.NPC.StateMachine;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F31 RID: 3889
	public class SceneDirector : MonoBehaviour
	{
		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x060074BE RID: 29886 RVA: 0x002F533B File Offset: 0x002F353B
		// (set) Token: 0x060074BF RID: 29887 RVA: 0x002F5342 File Offset: 0x002F3542
		public static SceneDirector Instance { get; private set; }

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x060074C0 RID: 29888 RVA: 0x002F534A File Offset: 0x002F354A
		public bool IsSceneActive
		{
			get
			{
				return this._isSceneActive;
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x060074C1 RID: 29889 RVA: 0x002F5352 File Offset: 0x002F3552
		public string CurrentSceneName
		{
			get
			{
				return this._currentSceneName;
			}
		}

		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x060074C2 RID: 29890 RVA: 0x002F535A File Offset: 0x002F355A
		public SceneContext CurrentContext
		{
			get
			{
				return this._currentContext;
			}
		}

		// Token: 0x060074C3 RID: 29891 RVA: 0x002F5362 File Offset: 0x002F3562
		private void Awake()
		{
			if (SceneDirector.Instance != null && SceneDirector.Instance != this)
			{
				Debug.LogWarning("[SceneDirector] Duplicate instance destroyed.");
				Object.Destroy(base.gameObject);
				return;
			}
			SceneDirector.Instance = this;
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x002F539A File Offset: 0x002F359A
		private void OnDestroy()
		{
			if (SceneDirector.Instance == this)
			{
				SceneDirector.Instance = null;
			}
		}

		// Token: 0x060074C5 RID: 29893 RVA: 0x002F53B0 File Offset: 0x002F35B0
		public async void StartScene(SceneDefinitionSO definition, SceneTrigger trigger)
		{
			string text;
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				this.Log("Scene start deferred because NPC simulation is hard-paused.");
			}
			else if (this._isSceneActive)
			{
				Debug.LogWarning("[SceneDirector] A scene is already active. End it first.");
			}
			else if (!definition.Validate(out text))
			{
				Debug.LogError("[SceneDirector] Scene validation failed: " + text);
			}
			else
			{
				this.Log("Starting scene: " + definition.sceneName);
				this._isSceneActive = true;
				this._currentSceneName = definition.sceneName;
				this._currentContext = new SceneContext(definition, trigger);
				this._currentContext.PlayerTransform = this.FindPlayerTransform();
				this.RegisterTriggerPoints(trigger);
				await this.SpawnSceneNPCs(definition, trigger);
				TaskAwaiter<bool> taskAwaiter = this.WaitForSimulationResume().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult() && this._currentContext != null && !this._currentContext.IsInterrupted)
				{
					this._sequenceCoroutine = base.StartCoroutine(this.ExecuteSequence());
				}
			}
		}

		// Token: 0x060074C6 RID: 29894 RVA: 0x002F53F8 File Offset: 0x002F35F8
		public void EndScene()
		{
			if (!this._isSceneActive)
			{
				return;
			}
			this.Log("Ending scene: " + this._currentSceneName);
			if (this._currentContext != null)
			{
				this._currentContext.IsInterrupted = true;
			}
			if (this._sequenceCoroutine != null)
			{
				base.StopCoroutine(this._sequenceCoroutine);
				this._sequenceCoroutine = null;
			}
			foreach (Coroutine coroutine in this._parallelCoroutines)
			{
				if (coroutine != null)
				{
					base.StopCoroutine(coroutine);
				}
			}
			this._parallelCoroutines.Clear();
			this.CleanupSceneNPCs();
			SceneContext currentContext = this._currentContext;
			if (currentContext != null)
			{
				SceneTrigger trigger = currentContext.Trigger;
				if (trigger != null)
				{
					trigger.OnSceneEnded();
				}
			}
			this._currentContext = null;
			this._isSceneActive = false;
			this._currentSceneName = "";
			this.Log("Scene ended.");
		}

		// Token: 0x060074C7 RID: 29895 RVA: 0x002F54F0 File Offset: 0x002F36F0
		private async Task SpawnSceneNPCs(SceneDefinitionSO definition, SceneTrigger trigger)
		{
			foreach (SceneNPCSlot slot in definition.npcSlots)
			{
				TaskAwaiter<bool> taskAwaiter = this.WaitForSimulationResume().GetAwaiter();
				TaskAwaiter<bool> taskAwaiter2;
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					return;
				}
				Transform spawnPoint = null;
				if (!string.IsNullOrEmpty(slot.spawnPointName))
				{
					spawnPoint = trigger.GetSpawnPoint(slot.spawnPointName);
				}
				if (spawnPoint == null)
				{
					spawnPoint = trigger.GetSpawnPointByIndex(slot.spawnPointIndex);
				}
				if (spawnPoint == null)
				{
					Debug.LogError(string.Format("[SceneDirector] No spawn point for slot '{0}' (name: '{1}', index: {2})", slot.slotId, slot.spawnPointName, slot.spawnPointIndex));
				}
				else
				{
					GameObject npc = null;
					if (this.runtimeSpawner != null)
					{
						npc = await this.runtimeSpawner.SpawnNPC(slot.characterData, spawnPoint.position, spawnPoint.rotation, null, null, true);
					}
					if (npc == null)
					{
						Debug.LogError("[SceneDirector] Failed to spawn NPC for slot '" + slot.slotId + "'");
					}
					else
					{
						taskAwaiter = this.WaitForSimulationResume().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							this.DespawnNPC(npc);
							return;
						}
						this.Log(string.Concat(new string[] { "Spawned NPC '", slot.slotId, "': ", npc.name, " at ", spawnPoint.name }));
						this.ConfigureSceneNPC(npc, slot);
						this._currentContext.RegisterNPC(slot.slotId, npc);
						spawnPoint = null;
						npc = null;
						slot = null;
					}
				}
			}
			List<SceneNPCSlot>.Enumerator enumerator = default(List<SceneNPCSlot>.Enumerator);
		}

		// Token: 0x060074C8 RID: 29896 RVA: 0x002F5544 File Offset: 0x002F3744
		private void ConfigureSceneNPC(GameObject npc, SceneNPCSlot slot)
		{
			NPCWanderController component = npc.GetComponent<NPCWanderController>();
			if (component != null)
			{
				component.enabled = false;
			}
			NPCSenses component2 = npc.GetComponent<NPCSenses>();
			if (component2 != null)
			{
				component2.enabled = slot.enableDetection;
			}
			NPCStateMachine component3 = npc.GetComponent<NPCStateMachine>();
			if (component3 != null)
			{
				component3.ForceState(NPCStateType.Frozen, "SceneDirector");
			}
			AStarNPCController component4 = npc.GetComponent<AStarNPCController>();
			if (component4 != null)
			{
				component4.SetPosture(slot.initialPosture);
				component4.SetAction(slot.initialAction);
				component4.Stop();
			}
			if (slot.personalityProfile != null)
			{
				NPCDialogueHolder component5 = npc.GetComponent<NPCDialogueHolder>();
				if (component5 != null)
				{
					component5.PersonalityProfile = slot.personalityProfile;
				}
			}
			this.Log(string.Format("Configured NPC '{0}': detection={1}, posture={2}, action={3}", new object[] { slot.slotId, slot.enableDetection, slot.initialPosture, slot.initialAction }));
		}

		// Token: 0x060074C9 RID: 29897 RVA: 0x002F5645 File Offset: 0x002F3845
		private IEnumerator ExecuteSequence()
		{
			SceneDefinitionSO definition = this._currentContext.Definition;
			List<SceneActionEntry> sequence = definition.actionSequence;
			do
			{
				int[] idx = new int[1];
				while (idx[0] < sequence.Count && !this._currentContext.IsInterrupted)
				{
					while ((NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f) && !this._currentContext.IsInterrupted)
					{
						yield return null;
					}
					if (this._currentContext.IsInterrupted)
					{
						break;
					}
					SceneActionEntry sceneActionEntry = sequence[idx[0]];
					if (sceneActionEntry.action == null)
					{
						this.Log(string.Format("Step {0} '{1}': action is null, skipping", idx[0], sceneActionEntry.label));
						idx[0]++;
					}
					else if (sceneActionEntry.parallelWithNext)
					{
						List<SceneActionEntry> list = this.CollectParallelGroup(sequence, idx);
						yield return this.ExecuteParallelGroup(list);
					}
					else
					{
						this.Log(string.Format("Step {0} '{1}': executing {2} on '{3}'", new object[]
						{
							idx[0],
							sceneActionEntry.label,
							sceneActionEntry.action.displayName,
							sceneActionEntry.targetSlotId
						}));
						yield return sceneActionEntry.action.Execute(this._currentContext, sceneActionEntry.targetSlotId);
						idx[0]++;
					}
				}
				if (definition.loopSequence && !this._currentContext.IsInterrupted)
				{
					this.Log("Scene sequence looping...");
				}
				idx = null;
			}
			while (definition.loopSequence && !this._currentContext.IsInterrupted);
			if (!this._currentContext.IsInterrupted)
			{
				this.Log("Scene sequence completed naturally.");
				this.EndScene();
			}
			yield break;
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x002F5654 File Offset: 0x002F3854
		private List<SceneActionEntry> CollectParallelGroup(List<SceneActionEntry> sequence, int[] index)
		{
			List<SceneActionEntry> list = new List<SceneActionEntry>();
			while (index[0] < sequence.Count && sequence[index[0]].parallelWithNext)
			{
				list.Add(sequence[index[0]]);
				index[0]++;
			}
			if (index[0] < sequence.Count)
			{
				list.Add(sequence[index[0]]);
				index[0]++;
			}
			return list;
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x002F56C6 File Offset: 0x002F38C6
		private IEnumerator ExecuteParallelGroup(List<SceneActionEntry> parallelEntries)
		{
			this.Log(string.Format("Executing parallel group of {0} actions", parallelEntries.Count));
			this._parallelCoroutines.Clear();
			bool[] completionFlags = new bool[parallelEntries.Count];
			for (int i = 0; i < parallelEntries.Count; i++)
			{
				SceneActionEntry sceneActionEntry = parallelEntries[i];
				int num = i;
				if (sceneActionEntry.action != null)
				{
					Coroutine coroutine = base.StartCoroutine(this.RunParallelAction(sceneActionEntry, num, completionFlags));
					this._parallelCoroutines.Add(coroutine);
				}
				else
				{
					completionFlags[i] = true;
				}
			}
			while (!this._currentContext.IsInterrupted)
			{
				bool flag = true;
				for (int j = 0; j < completionFlags.Length; j++)
				{
					if (!completionFlags[j])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				yield return null;
			}
			this._parallelCoroutines.Clear();
			yield break;
		}

		// Token: 0x060074CC RID: 29900 RVA: 0x002F56DC File Offset: 0x002F38DC
		private IEnumerator RunParallelAction(SceneActionEntry entry, int flagIndex, bool[] completionFlags)
		{
			while ((NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f) && !this._currentContext.IsInterrupted)
			{
				yield return null;
			}
			if (this._currentContext.IsInterrupted)
			{
				completionFlags[flagIndex] = true;
				yield break;
			}
			this.Log(string.Concat(new string[]
			{
				"  Parallel: ",
				entry.action.displayName,
				" on '",
				entry.targetSlotId,
				"'"
			}));
			yield return entry.action.Execute(this._currentContext, entry.targetSlotId);
			completionFlags[flagIndex] = true;
			yield break;
		}

		// Token: 0x060074CD RID: 29901 RVA: 0x002F5700 File Offset: 0x002F3900
		private void CleanupSceneNPCs()
		{
			if (this._currentContext == null)
			{
				return;
			}
			foreach (SceneNPCSlot sceneNPCSlot in this._currentContext.Definition.npcSlots)
			{
				GameObject npc = this._currentContext.GetNPC(sceneNPCSlot.slotId);
				if (!(npc == null))
				{
					switch (sceneNPCSlot.endBehavior)
					{
					case SceneEndNPCBehavior.Despawn:
						this.DespawnNPC(npc);
						break;
					case SceneEndNPCBehavior.Stay:
					{
						NPCStateMachine component = npc.GetComponent<NPCStateMachine>();
						if (component != null)
						{
							component.ForceState(NPCStateType.Idle, "SceneDirector_EndStay");
						}
						break;
					}
					case SceneEndNPCBehavior.Wander:
					{
						NPCWanderController component2 = npc.GetComponent<NPCWanderController>();
						if (component2 != null)
						{
							component2.enabled = true;
						}
						NPCSenses component3 = npc.GetComponent<NPCSenses>();
						if (component3 != null)
						{
							component3.enabled = true;
						}
						NPCStateMachine component4 = npc.GetComponent<NPCStateMachine>();
						if (component4 != null)
						{
							component4.ForceState(NPCStateType.Idle, "SceneDirector_EndWander");
						}
						AStarNPCController component5 = npc.GetComponent<AStarNPCController>();
						if (component5 != null)
						{
							component5.ResetToDefault();
						}
						break;
					}
					}
					this.Log(string.Format("Cleanup NPC '{0}': {1}", sceneNPCSlot.slotId, sceneNPCSlot.endBehavior));
				}
			}
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x002F5840 File Offset: 0x002F3A40
		private void DespawnNPC(GameObject npc)
		{
			if (this.runtimeSpawner != null)
			{
				this.runtimeSpawner.DespawnNPC(npc, false);
				return;
			}
			Object.Destroy(npc);
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x002F5864 File Offset: 0x002F3A64
		private async Task<bool> WaitForSimulationResume()
		{
			while (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				if (!this._isSceneActive || this._currentContext == null || this._currentContext.IsInterrupted)
				{
					return false;
				}
				await Task.Yield();
			}
			return this._isSceneActive && this._currentContext != null && !this._currentContext.IsInterrupted;
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x002F58A8 File Offset: 0x002F3AA8
		private Transform FindPlayerTransform()
		{
			if (SensoryManager.Instance != null)
			{
				StealthActor player = SensoryManager.Instance.GetPlayer();
				if (player != null)
				{
					return player.transform;
				}
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject == null)
			{
				return null;
			}
			return gameObject.transform;
		}

		// Token: 0x060074D1 RID: 29905 RVA: 0x002F58F4 File Offset: 0x002F3AF4
		private void RegisterTriggerPoints(SceneTrigger trigger)
		{
			foreach (NamedTransform namedTransform in trigger.spawnPoints)
			{
				if (namedTransform.transform != null && !string.IsNullOrEmpty(namedTransform.name))
				{
					this._currentContext.SpawnPoints[namedTransform.name] = namedTransform.transform;
				}
			}
			foreach (NamedTransform namedTransform2 in trigger.waypoints)
			{
				if (namedTransform2.transform != null && !string.IsNullOrEmpty(namedTransform2.name))
				{
					this._currentContext.Waypoints[namedTransform2.name] = namedTransform2.transform;
				}
			}
		}

		// Token: 0x060074D2 RID: 29906 RVA: 0x002F59EC File Offset: 0x002F3BEC
		private void Log(string message)
		{
			if (this.debugLog)
			{
				Debug.Log("[SceneDirector] " + message);
			}
		}

		// Token: 0x04007784 RID: 30596
		[Header("References")]
		[Tooltip("Reference to the NPCRuntimeSpawner for creating scene NPCs")]
		[SerializeField]
		private NPCRuntimeSpawner runtimeSpawner;

		// Token: 0x04007785 RID: 30597
		[Header("Settings")]
		[Tooltip("If true, logs detailed scene execution info")]
		[SerializeField]
		private bool debugLog = true;

		// Token: 0x04007786 RID: 30598
		[Header("Runtime State")]
		[SerializeField]
		private bool _isSceneActive;

		// Token: 0x04007787 RID: 30599
		[SerializeField]
		private string _currentSceneName;

		// Token: 0x04007788 RID: 30600
		private SceneContext _currentContext;

		// Token: 0x04007789 RID: 30601
		private Coroutine _sequenceCoroutine;

		// Token: 0x0400778A RID: 30602
		private List<Coroutine> _parallelCoroutines = new List<Coroutine>();
	}
}
