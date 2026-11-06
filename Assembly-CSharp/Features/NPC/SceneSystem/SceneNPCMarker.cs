using System;
using Bozo.AnimeCharacters;
using Features.Detection;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F39 RID: 3897
	public class SceneNPCMarker : MonoBehaviour
	{
		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x060074EC RID: 29932 RVA: 0x002F6664 File Offset: 0x002F4864
		public NPCInterestPoint.PostureType EffectiveInitialPosture
		{
			get
			{
				switch (this.behaviorPattern)
				{
				case NPCBehaviorPattern.Idle:
				case NPCBehaviorPattern.StandAction:
				case NPCBehaviorPattern.Patrol:
					return NPCInterestPoint.PostureType.Standing;
				case NPCBehaviorPattern.SitIdle:
				case NPCBehaviorPattern.SitAction:
				case NPCBehaviorPattern.WalkToAndSit:
					return NPCInterestPoint.PostureType.Sitting;
				}
				return this.posture;
			}
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x002F66AC File Offset: 0x002F48AC
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.npcId) || !this.npcId.EndsWith(string.Format("_{0}", base.GetInstanceID())))
			{
				this.npcId = string.Format("npc_{0}_{1}", base.gameObject.name, base.GetInstanceID());
			}
		}

		// Token: 0x060074EE RID: 29934 RVA: 0x002F6710 File Offset: 0x002F4910
		private void OnDrawGizmos()
		{
			Gizmos.color = this.GetGizmoColor();
			Vector3 position = base.transform.position;
			float num = ((this.posture == NPCInterestPoint.PostureType.Sitting) ? 0.7f : 1f);
			Gizmos.DrawWireSphere(position + Vector3.up * num, 0.2f);
			Gizmos.DrawWireCube(position + Vector3.up * (num * 0.5f), new Vector3(0.3f, num, 0.2f));
			if (this.useFacingDirection)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(position + Vector3.up * 0.5f, base.transform.forward * 0.8f);
			}
			if (this.spawnMode == SpawnMode.UsePOI && this.targetPOI != null)
			{
				Gizmos.color = new Color(1f, 0.8f, 0f, 0.8f);
				Gizmos.DrawLine(position + Vector3.up * 0.3f, this.targetPOI.transform.position + Vector3.up * 0.3f);
				Gizmos.DrawWireSphere(this.targetPOI.transform.position + Vector3.up * 0.3f, 0.15f);
			}
			if (this.lookAtTarget != null)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(position + Vector3.up * num, this.lookAtTarget.transform.position + Vector3.up * num);
			}
			if (this.interactionTarget != null)
			{
				Gizmos.color = Color.magenta;
				SceneNPCMarker.DrawDashedLineGizmo(position + Vector3.up * 0.5f, this.interactionTarget.position + Vector3.up * 0.5f, 0.3f);
			}
			if (this.patrolWaypoints != null && this.patrolWaypoints.Length != 0)
			{
				Gizmos.color = new Color(0f, 1f, 0.5f, 0.6f);
				Vector3 vector = position;
				foreach (Transform transform in this.patrolWaypoints)
				{
					if (transform != null)
					{
						Gizmos.DrawLine(vector + Vector3.up * 0.1f, transform.position + Vector3.up * 0.1f);
						Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.1f, 0.15f);
						vector = transform.position;
					}
				}
			}
		}

		// Token: 0x060074EF RID: 29935 RVA: 0x002F69DF File Offset: 0x002F4BDF
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(base.transform.position, 0.5f);
		}

		// Token: 0x060074F0 RID: 29936 RVA: 0x002F6A00 File Offset: 0x002F4C00
		private static void DrawDashedLineGizmo(Vector3 from, Vector3 to, float dashLength)
		{
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			vector /= magnitude;
			float num = 0f;
			bool flag = true;
			while (num < magnitude)
			{
				float num2 = Mathf.Min(dashLength, magnitude - num);
				if (flag)
				{
					Gizmos.DrawLine(from + vector * num, from + vector * (num + num2));
				}
				num += num2;
				flag = !flag;
			}
		}

		// Token: 0x060074F1 RID: 29937 RVA: 0x002F6A70 File Offset: 0x002F4C70
		private Color GetGizmoColor()
		{
			switch (this.behaviorPattern)
			{
			case NPCBehaviorPattern.Idle:
				return Color.gray;
			case NPCBehaviorPattern.SitIdle:
				return new Color(0.5f, 0.5f, 1f);
			case NPCBehaviorPattern.SitAction:
				return new Color(0.3f, 0.3f, 1f);
			case NPCBehaviorPattern.StandAction:
				return new Color(0.3f, 1f, 0.3f);
			case NPCBehaviorPattern.Patrol:
				return Color.green;
			case NPCBehaviorPattern.LookAtPlayer:
				return Color.red;
			case NPCBehaviorPattern.LookAtTarget:
				return Color.yellow;
			case NPCBehaviorPattern.WalkToAndSit:
				return new Color(1f, 0.5f, 0f);
			case NPCBehaviorPattern.Custom:
				return Color.magenta;
			default:
				return Color.white;
			}
		}

		// Token: 0x040077B5 RID: 30645
		[Header("Identity")]
		[Tooltip("Unique ID for this NPC in the scene (auto-generated if empty)")]
		public string npcId;

		// Token: 0x040077B6 RID: 30646
		[Tooltip("Character data defining appearance and outfit")]
		public CharacterObject characterData;

		// Token: 0x040077B7 RID: 30647
		[Header("Spawn Mode")]
		[Tooltip("Direct: NPC appears instantly at marker position (skip walk/sit animations).\nUsePOI: NPC walks to a POI and performs the full approach sequence.")]
		public SpawnMode spawnMode;

		// Token: 0x040077B8 RID: 30648
		[Tooltip("Target POI for UsePOI mode. The NPC will navigate to this POI and perform the approach/sit sequence.")]
		public NPCInterestPoint targetPOI;

		// Token: 0x040077B9 RID: 30649
		[Header("Initial State")]
		[Tooltip("Posture when spawned (Direct mode only)")]
		public NPCInterestPoint.PostureType posture;

		// Token: 0x040077BA RID: 30650
		[Tooltip("Upper body action when spawned")]
		public NPCInterestPoint.ActionType action;

		// Token: 0x040077BB RID: 30651
		[Tooltip("Direction the NPC faces (uses this Transform's forward)")]
		public bool useFacingDirection = true;

		// Token: 0x040077BC RID: 30652
		[Header("Behavior Pattern")]
		[Tooltip("What this NPC does during the scene")]
		public NPCBehaviorPattern behaviorPattern;

		// Token: 0x040077BD RID: 30653
		[Tooltip("Optional: Waypoints for patrol/walk patterns (child Transforms or references)")]
		public Transform[] patrolWaypoints;

		// Token: 0x040077BE RID: 30654
		[Tooltip("Optional: Another marker this NPC should look at")]
		public SceneNPCMarker lookAtTarget;

		// Token: 0x040077BF RID: 30655
		[Tooltip("Optional: Another marker or Transform this NPC interacts with")]
		public Transform interactionTarget;

		// Token: 0x040077C0 RID: 30656
		[Header("Timing")]
		[Tooltip("Delay before this NPC starts their behavior (seconds)")]
		public float startDelay;

		// Token: 0x040077C1 RID: 30657
		[Tooltip("How long the NPC performs their action before switching (0 = indefinite)")]
		public float actionDuration;

		// Token: 0x040077C2 RID: 30658
		[Header("Detection")]
		[Tooltip("If false, NPCSenses is disabled - NPC won't detect the player")]
		public bool enableDetection;

		// Token: 0x040077C3 RID: 30659
		[Header("Personality")]
		[Tooltip("Personality profile for expressions and reactions (optional)")]
		public NPCPersonalityProfileSO personalityProfile;

		// Token: 0x040077C4 RID: 30660
		[Header("End Behavior")]
		[Tooltip("What happens to this NPC when the scene ends")]
		public SceneEndNPCBehavior endBehavior;

		// Token: 0x040077C5 RID: 30661
		[Header("Editor Preview")]
		[Tooltip("Prefab to instantiate for visual preview in the editor (e.g. BMAC_MergedCharacterBase)")]
		public GameObject previewPrefab;

		// Token: 0x040077C6 RID: 30662
		[NonSerialized]
		public GameObject spawnedNPC;

		// Token: 0x040077C7 RID: 30663
		[NonSerialized]
		public GameObject previewInstance;
	}
}
