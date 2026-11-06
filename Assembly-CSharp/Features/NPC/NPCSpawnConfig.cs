using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EF6 RID: 3830
	[CreateAssetMenu(fileName = "NPCSpawnConfig", menuName = "NPCs/Spawn Config")]
	public class NPCSpawnConfig : ScriptableObject
	{
		// Token: 0x060072B6 RID: 29366 RVA: 0x002EAB5C File Offset: 0x002E8D5C
		public float EvaluateTimeOfDayMultiplier(float hour)
		{
			if (!this.useTimeOfDayPopulation || this.populationByHour == null || this.populationByHour.Length == 0)
			{
				return 1f;
			}
			float num = Mathf.Repeat(hour, 24f);
			float num2 = float.NegativeInfinity;
			float num3 = 1f;
			float num4 = float.PositiveInfinity;
			float num5 = 1f;
			float num6 = float.PositiveInfinity;
			float num7 = 1f;
			float num8 = float.NegativeInfinity;
			float num9 = 1f;
			for (int i = 0; i < this.populationByHour.Length; i++)
			{
				float num10 = Mathf.Clamp(this.populationByHour[i].hour, 0f, 24f);
				float num11 = Mathf.Clamp01(this.populationByHour[i].multiplier);
				if (Mathf.Approximately(num10, 24f))
				{
					num10 = 0f;
				}
				if (num10 < num6)
				{
					num6 = num10;
					num7 = num11;
				}
				if (num10 > num8)
				{
					num8 = num10;
					num9 = num11;
				}
				if (num10 <= num && num10 > num2)
				{
					num2 = num10;
					num3 = num11;
				}
				if (num10 >= num && num10 < num4)
				{
					num4 = num10;
					num5 = num11;
				}
			}
			if (float.IsNegativeInfinity(num2))
			{
				num2 = num8 - 24f;
				num3 = num9;
			}
			if (float.IsPositiveInfinity(num4))
			{
				num4 = num6 + 24f;
				num5 = num7;
			}
			if (Mathf.Approximately(num2, num4))
			{
				return num3;
			}
			float num12 = Mathf.InverseLerp(num2, num4, num);
			return Mathf.Lerp(num3, num5, num12);
		}

		// Token: 0x040075CC RID: 30156
		[Header("Population")]
		[Tooltip("Hard cap of NPCs allowed in scene (vicini + ambient lontani).")]
		public int maxTotalNpc = 80;

		// Token: 0x040075CD RID: 30157
		[Tooltip("How many NPCs the system tries to keep ALWAYS in the NEAR radius around the player. These are the 'companions' that make the city feel alive. If fewer than this are inside nearRadius, the system spawns more there immediately.")]
		public int desiredActiveNpc = 15;

		// Token: 0x040075CE RID: 30158
		[Tooltip("How many NPCs the system keeps in the ambient ring, outside nearRadius and inside ambientRadius.")]
		[Min(0f)]
		public int desiredAmbientNpc = 10;

		// Token: 0x040075CF RID: 30159
		[Tooltip("Maintain desiredAmbientNpc between nearRadius and ambientRadius. Turn this off to keep only the guaranteed near-player population.")]
		public bool allowAmbientNpcs = true;

		// Token: 0x040075D0 RID: 30160
		[Header("Time of day population")]
		[Tooltip("Scale both near and ambient gameplay populations using Azure's current hour.")]
		public bool useTimeOfDayPopulation = true;

		// Token: 0x040075D1 RID: 30161
		[Tooltip("Population multiplier by hour. Keys may be edited without changing graphics presets.")]
		public NPCSpawnConfig.PopulationTimeKey[] populationByHour = new NPCSpawnConfig.PopulationTimeKey[]
		{
			new NPCSpawnConfig.PopulationTimeKey(0f, 0.2f),
			new NPCSpawnConfig.PopulationTimeKey(5f, 0.2f),
			new NPCSpawnConfig.PopulationTimeKey(6f, 0.35f),
			new NPCSpawnConfig.PopulationTimeKey(7f, 0.65f),
			new NPCSpawnConfig.PopulationTimeKey(8f, 1f),
			new NPCSpawnConfig.PopulationTimeKey(18f, 1f),
			new NPCSpawnConfig.PopulationTimeKey(19f, 0.8f),
			new NPCSpawnConfig.PopulationTimeKey(21f, 0.5f),
			new NPCSpawnConfig.PopulationTimeKey(23f, 0.25f),
			new NPCSpawnConfig.PopulationTimeKey(24f, 0.2f)
		};

		// Token: 0x040075D2 RID: 30162
		[Tooltip("Seconds between reads of the Azure clock.")]
		[Min(0.1f)]
		public float populationTimeUpdateInterval = 2f;

		// Token: 0x040075D3 RID: 30163
		[Tooltip("How many NPCs to pre-instantiate into the pool at startup. With baked prefabs this is fast; set to maxTotalNpc for steady-state.")]
		public int prePoolSize = 80;

		// Token: 0x040075D4 RID: 30164
		[Tooltip("Hard cap on baked NPC prefabs instantiated in one startup frame. The time budget can end the batch sooner.")]
		[Min(1f)]
		public int prePoolMaxInstancesPerFrame = 2;

		// Token: 0x040075D5 RID: 30165
		[Tooltip("Indicative main-thread CPU budget for one baked NPC pre-pool batch. At least one NPC is processed per frame.")]
		[Min(0.1f)]
		public float prePoolFrameBudgetMs = 4f;

		// Token: 0x040075D6 RID: 30166
		[Header("Zones (radii from player)")]
		[Tooltip("NEAR radius: where the system guarantees desiredActiveNpc are always present. Make it small (20-30m) for 'companions right around me'; larger (50-60m) for a wider 'crowd'.")]
		public float nearRadius = 30f;

		// Token: 0x040075D7 RID: 30167
		[Tooltip("Outer edge of the actively maintained ambient ring. Keep this <= activeRadius so managed NPCs stay simulated.")]
		public float ambientRadius = 55f;

		// Token: 0x040075D8 RID: 30168
		[Tooltip("Within this radius, NPC logic/animation is ON. Usually >= nearRadius.")]
		public float activeRadius = 60f;

		// Token: 0x040075D9 RID: 30169
		[Tooltip("Within this radius, ambient NPCs are allowed to exist (between nearRadius and spawnRadius).")]
		public float spawnRadius = 80f;

		// Token: 0x040075DA RID: 30170
		[Tooltip("Beyond this radius, NPCs are returned to the pool (proactive despawn).")]
		public float despawnRadius = 110f;

		// Token: 0x040075DB RID: 30171
		[Header("Spawn point selection")]
		[Tooltip("Preferred distance from player when scoring spawn points.")]
		public float preferredDistance = 25f;

		// Token: 0x040075DC RID: 30172
		[Tooltip("Spawn points closer than this are skipped (prevents popping right in front of the player).")]
		public float minDistanceFromPlayer = 8f;

		// Token: 0x040075DD RID: 30173
		[Tooltip("Weight of distance-from-preferred in the score (vs random).")]
		[Range(0f, 1f)]
		public float distanceWeight = 0.7f;

		// Token: 0x040075DE RID: 30174
		[Tooltip("Weight of jitter in the score (avoids deterministic clustering).")]
		[Range(0f, 1f)]
		public float randomWeight = 0.3f;

		// Token: 0x040075DF RID: 30175
		[Tooltip("Spawn points used in the last N seconds get a penalty in scoring to spread NPCs out.")]
		public float recentSpawnCooldown = 4f;

		// Token: 0x040075E0 RID: 30176
		[Tooltip("Minimum horizontal distance between a new NPC and every active or queued NPC. Must be larger than twice the navigation radius to prevent agents from spawning interpenetrated.")]
		[Min(0f)]
		public float minNpcSpawnSpacing = 1.5f;

		// Token: 0x040075E1 RID: 30177
		[Tooltip("Maximum horizontal correction allowed when projecting an authored spawn point onto the A* graph. Points farther away are rejected instead of snapping through a wall or onto another street.")]
		[Min(0f)]
		public float maxSpawnNavmeshHorizontalCorrection = 1.5f;

		// Token: 0x040075E2 RID: 30178
		[Tooltip("Maximum vertical correction allowed when projecting an authored spawn point onto the A* graph.")]
		[Min(0f)]
		public float maxSpawnNavmeshVerticalCorrection = 2f;

		// Token: 0x040075E3 RID: 30179
		[Header("Rate limiting")]
		[Tooltip("Max NPCs ENQUEUED per single rebalance cycle. Prevents 'bursts' of N NPCs appearing at once when entering a new area at high speed. Lower = more gradual.")]
		public int maxSpawnsPerRebalance = 3;

		// Token: 0x040075E4 RID: 30180
		[Tooltip("Max NPCs spawned per frame (avoids stutter when many slots need filling).")]
		public int spawnsPerFrame = 4;

		// Token: 0x040075E5 RID: 30181
		[Tooltip("Max NPCs despawned per frame.")]
		public int despawnsPerFrame = 8;

		// Token: 0x040075E6 RID: 30182
		[Tooltip("Maximum NPCs quietly returned to the pool per rebalance when the time-of-day target decreases.")]
		[Min(1f)]
		public int retirementsPerRebalance = 1;

		// Token: 0x040075E7 RID: 30183
		[Tooltip("Interval (seconds) between full rebalance passes.")]
		public float rebalanceInterval = 0.4f;

		// Token: 0x040075E8 RID: 30184
		[Header("Debug")]
		[Tooltip("Verbose log for the NPCZoneManager itself (rebalance, despawn, etc.)")]
		public bool enableDebugLogs;

		// Token: 0x040075E9 RID: 30185
		[Tooltip("Global verbose toggle for NPC component logs (EquipmentManager, ReactionAudio, AStarNPCController, ...). Off by default to keep production console quiet.")]
		public bool enableNpcVerboseLogs;

		// Token: 0x02000EF7 RID: 3831
		[Serializable]
		public struct PopulationTimeKey
		{
			// Token: 0x060072B8 RID: 29368 RVA: 0x002EAEBA File Offset: 0x002E90BA
			public PopulationTimeKey(float hour, float multiplier)
			{
				this.hour = hour;
				this.multiplier = multiplier;
			}

			// Token: 0x040075EA RID: 30186
			[Range(0f, 24f)]
			public float hour;

			// Token: 0x040075EB RID: 30187
			[Range(0f, 1f)]
			public float multiplier;
		}
	}
}
