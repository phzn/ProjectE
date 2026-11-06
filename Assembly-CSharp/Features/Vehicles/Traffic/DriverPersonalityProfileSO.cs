using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BA1 RID: 2977
	[CreateAssetMenu(fileName = "DriverPersonalityProfile", menuName = "Vehicles/Driver Personality Profile")]
	public class DriverPersonalityProfileSO : ScriptableObject
	{
		// Token: 0x0600557B RID: 21883 RVA: 0x00212760 File Offset: 0x00210960
		public DriverPersonality Pick()
		{
			if (this.Entries == null || this.Entries.Count == 0)
			{
				return DriverPersonality.Angry;
			}
			int num = 0;
			for (int i = 0; i < this.Entries.Count; i++)
			{
				num += Mathf.Max(0, this.Entries[i].Weight);
			}
			if (num <= 0)
			{
				return DriverPersonality.Angry;
			}
			int num2 = Random.Range(0, num);
			for (int j = 0; j < this.Entries.Count; j++)
			{
				int num3 = Mathf.Max(0, this.Entries[j].Weight);
				if (num2 < num3)
				{
					return this.Entries[j].Personality;
				}
				num2 -= num3;
			}
			return this.Entries[this.Entries.Count - 1].Personality;
		}

		// Token: 0x0400558A RID: 21898
		[Tooltip("Weighted table. A roll picks one entry proportionally to Weight; empty/zero-weight tables fall back to Angry.")]
		public List<DriverPersonalityProfileSO.Entry> Entries = new List<DriverPersonalityProfileSO.Entry>
		{
			new DriverPersonalityProfileSO.Entry
			{
				Personality = DriverPersonality.Angry,
				Weight = 30
			},
			new DriverPersonalityProfileSO.Entry
			{
				Personality = DriverPersonality.Scared,
				Weight = 20
			},
			new DriverPersonalityProfileSO.Entry
			{
				Personality = DriverPersonality.Coward,
				Weight = 20
			},
			new DriverPersonalityProfileSO.Entry
			{
				Personality = DriverPersonality.Gawker,
				Weight = 15
			},
			new DriverPersonalityProfileSO.Entry
			{
				Personality = DriverPersonality.CallPolice,
				Weight = 15
			}
		};

		// Token: 0x0400558B RID: 21899
		[Header("Scared (flees on foot)")]
		[Tooltip("How far (m) the flee destination is aimed, away from the player. The A* path is clamped to the farthest REACHABLE node toward it, so bigger = he runs as far as the sidewalk network allows (designer 2026-07-17: as far as possible).")]
		public float FleeDistance = 80f;

		// Token: 0x0400558C RID: 21900
		[Tooltip("Run speed while fleeing (>~4.5 = full Run blend).")]
		public float FleeSpeed = 5f;

		// Token: 0x0400558D RID: 21901
		[Tooltip("Seconds after which the fleeing driver fade-despawns (also despawns on arrival).")]
		public float FleeDespawnSeconds = 25f;

		// Token: 0x0400558E RID: 21902
		[Tooltip("Optional scream/panic clips, one picked at random (empty = silent).")]
		public AudioClip[] ScreamClips;

		// Token: 0x0400558F RID: 21903
		[Range(0f, 1f)]
		public float ScreamVolume = 0.9f;

		// Token: 0x04005590 RID: 21904
		[Header("Coward (drives off as soon as you're clear)")]
		[Tooltip("Player distance (m) from the car at which the coward floors it (much shorter than the Angry leaveDistance — he just wants OUT).")]
		public float CowardLeaveDistance = 2.5f;

		// Token: 0x04005591 RID: 21905
		[Tooltip("Temporary VehicleAI.maxAcceleration while peeling off (burnout feel).")]
		public float CowardBoostAcceleration = 8f;

		// Token: 0x04005592 RID: 21906
		[Tooltip("Seconds the acceleration boost lasts.")]
		public float CowardBoostSeconds = 3f;

		// Token: 0x04005593 RID: 21907
		[Header("Gawker (photo souvenir)")]
		[Tooltip("Seconds the photo reaction is held before the driver gets back in.")]
		public float PhotoSeconds = 4f;

		// Token: 0x04005594 RID: 21908
		[Tooltip("Prop name activated on the NPC while photographing (NPCEquipmentManager slot).")]
		public string PhonePropName = "Smartphone";

		// Token: 0x04005595 RID: 21909
		[Header("Call Police (suspicion seeding)")]
		[Tooltip("Suspicion added the moment the caller steps out (1 = Exposed threshold).")]
		public float SuspicionSeed = 1f;

		// Token: 0x04005596 RID: 21910
		[Tooltip("Extra suspicion added every RepeatInterval while the player LINGERS in range.")]
		public float SuspicionRepeatDelta = 0.5f;

		// Token: 0x04005597 RID: 21911
		public float SuspicionRepeatInterval = 5f;

		// Token: 0x04005598 RID: 21912
		[Tooltip("Player must be within this range (m) of the caller for repeat seeding to tick.")]
		public float SuspicionRepeatRange = 25f;

		// Token: 0x02000BA2 RID: 2978
		[Serializable]
		public class Entry
		{
			// Token: 0x04005599 RID: 21913
			public DriverPersonality Personality;

			// Token: 0x0400559A RID: 21914
			[Range(0f, 100f)]
			public int Weight = 10;
		}
	}
}
