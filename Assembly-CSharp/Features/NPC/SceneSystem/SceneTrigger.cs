using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F49 RID: 3913
	[RequireComponent(typeof(Collider))]
	public class SceneTrigger : MonoBehaviour
	{
		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x06007548 RID: 30024 RVA: 0x002F8829 File Offset: 0x002F6A29
		public bool IsSceneActive
		{
			get
			{
				return this._isSceneActive;
			}
		}

		// Token: 0x06007549 RID: 30025 RVA: 0x002F8834 File Offset: 0x002F6A34
		private void OnTriggerEnter(Collider other)
		{
			if (!this.isAvailable || this._isSceneActive)
			{
				return;
			}
			if (this.oneTimeOnly && this._hasBeenUsed)
			{
				return;
			}
			if (!other.CompareTag("Player"))
			{
				return;
			}
			this._playerInRange = true;
			if (!this.requiresInteraction)
			{
				this.StartScene();
				return;
			}
			this.ShowPrompt(true);
		}

		// Token: 0x0600754A RID: 30026 RVA: 0x002F888E File Offset: 0x002F6A8E
		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Player"))
			{
				return;
			}
			this._playerInRange = false;
			this.ShowPrompt(false);
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x002F88AC File Offset: 0x002F6AAC
		private void Update()
		{
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this._playerInRange && !this.requiresInteraction && !this._isSceneActive)
			{
				this.StartScene();
				return;
			}
			if (this._playerInRange && this.requiresInteraction && !this._isSceneActive && GameplayInput.GetKeyDown(this.interactionKey))
			{
				this.StartScene();
			}
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x002F8918 File Offset: 0x002F6B18
		public void StartScene()
		{
			if (this._isSceneActive || this.sceneDefinition == null || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			this.ShowPrompt(false);
			this._isSceneActive = true;
			if (SceneDirector.Instance != null)
			{
				SceneDirector.Instance.StartScene(this.sceneDefinition, this);
				return;
			}
			Debug.LogError("[SceneTrigger] SceneDirector.Instance is null! Cannot start scene.");
			this._isSceneActive = false;
		}

		// Token: 0x0600754D RID: 30029 RVA: 0x002F898D File Offset: 0x002F6B8D
		public void OnSceneEnded()
		{
			this._isSceneActive = false;
			if (this.oneTimeOnly)
			{
				this._hasBeenUsed = true;
			}
		}

		// Token: 0x0600754E RID: 30030 RVA: 0x002F89A8 File Offset: 0x002F6BA8
		public Transform GetSpawnPoint(string pointName)
		{
			NamedTransform namedTransform = this.spawnPoints.Find((NamedTransform sp) => sp.name == pointName);
			if (namedTransform == null)
			{
				return null;
			}
			return namedTransform.transform;
		}

		// Token: 0x0600754F RID: 30031 RVA: 0x002F89E4 File Offset: 0x002F6BE4
		public Transform GetSpawnPointByIndex(int index)
		{
			if (index >= 0 && index < this.spawnPoints.Count)
			{
				return this.spawnPoints[index].transform;
			}
			return null;
		}

		// Token: 0x06007550 RID: 30032 RVA: 0x002F8A0C File Offset: 0x002F6C0C
		public Transform GetWaypoint(string waypointName)
		{
			NamedTransform namedTransform = this.waypoints.Find((NamedTransform wp) => wp.name == waypointName);
			if (namedTransform == null)
			{
				return null;
			}
			return namedTransform.transform;
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x002F8A48 File Offset: 0x002F6C48
		private void ShowPrompt(bool show)
		{
			if (show)
			{
				Debug.Log("[SceneTrigger] " + this.interactionPrompt);
			}
		}

		// Token: 0x06007552 RID: 30034 RVA: 0x002F8A64 File Offset: 0x002F6C64
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			foreach (NamedTransform namedTransform in this.spawnPoints)
			{
				if (namedTransform.transform != null)
				{
					Gizmos.DrawWireSphere(namedTransform.transform.position, 0.3f);
					Gizmos.DrawLine(namedTransform.transform.position, namedTransform.transform.position + namedTransform.transform.forward * 0.5f);
				}
			}
			Gizmos.color = Color.cyan;
			foreach (NamedTransform namedTransform2 in this.waypoints)
			{
				if (namedTransform2.transform != null)
				{
					Gizmos.DrawWireCube(namedTransform2.transform.position, Vector3.one * 0.3f);
				}
			}
		}

		// Token: 0x04007829 RID: 30761
		[Header("Scene")]
		[Tooltip("The scene definition to execute when triggered")]
		public SceneDefinitionSO sceneDefinition;

		// Token: 0x0400782A RID: 30762
		[Header("Interaction")]
		[Tooltip("If true, player must press interact key. If false, scene starts on trigger enter.")]
		public bool requiresInteraction = true;

		// Token: 0x0400782B RID: 30763
		[Tooltip("Prompt shown to player when in range")]
		public string interactionPrompt = "Press E to enter";

		// Token: 0x0400782C RID: 30764
		[Tooltip("Key to press for interaction")]
		public KeyCode interactionKey = KeyCode.E;

		// Token: 0x0400782D RID: 30765
		[Header("Availability")]
		[Tooltip("If true, this trigger can only be used once")]
		public bool oneTimeOnly;

		// Token: 0x0400782E RID: 30766
		[Tooltip("If false, the trigger is disabled and cannot be activated")]
		public bool isAvailable = true;

		// Token: 0x0400782F RID: 30767
		[Header("Spawn Points")]
		[Tooltip("Named spawn points for NPCs. Index matches SceneNPCSlot.spawnPointIndex")]
		public List<NamedTransform> spawnPoints = new List<NamedTransform>();

		// Token: 0x04007830 RID: 30768
		[Header("Waypoints")]
		[Tooltip("Named waypoints used by scene actions (e.g. 'desk', 'door', 'center')")]
		public List<NamedTransform> waypoints = new List<NamedTransform>();

		// Token: 0x04007831 RID: 30769
		private bool _playerInRange;

		// Token: 0x04007832 RID: 30770
		private bool _hasBeenUsed;

		// Token: 0x04007833 RID: 30771
		private bool _isSceneActive;
	}
}
