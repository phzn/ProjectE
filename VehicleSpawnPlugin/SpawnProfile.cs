using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x0200000A RID: 10
	[CreateAssetMenu(menuName = "VehicleSpawnPlugin/Spawn Profile", fileName = "NewSpawnProfile")]
	public class SpawnProfile : ScriptableObject
	{
		// Token: 0x04000087 RID: 135
		[Header("Vehicle Types")]
		public List<VehicleDefinition> vehicleTypes = new List<VehicleDefinition>();

		// Token: 0x04000088 RID: 136
		[Header("Limits")]
		[Range(1f, 200f)]
		public int globalMaxVehicles = 20;

		// Token: 0x04000089 RID: 137
		[Header("Timing")]
		[Range(0.5f, 30f)]
		public float spawnInterval = 3f;

		// Token: 0x0400008A RID: 138
		[Range(0.5f, 30f)]
		public float despawnCheckInterval = 5f;

		// Token: 0x0400008B RID: 139
		[Header("Distance")]
		[Range(5f, 100f)]
		public float minSpawnDistanceFromPlayer = 20f;

		// Token: 0x0400008C RID: 140
		[Range(20f, 500f)]
		public float maxSpawnDistanceFromPlayer = 120f;

		// Token: 0x0400008D RID: 141
		[Header("Despawn (FIX COLLAUDO 1 — anti car-pop-in-view)")]
		[Tooltip("Hard distance cap (m). Beyond this a vehicle is ALWAYS eligible for distance-despawn, even if it is inside the camera frustum. Should be >= maxSpawnDistanceFromPlayer so a car that drifted far behind the camera is still cleaned up. Field name kept from a previous plugin version so the serialized value in NewSpawnProfile.asset revives.")]
		[Range(20f, 500f)]
		public float despawnDistance = 130f;

		// Token: 0x0400008E RID: 142
		[Tooltip("When true, a vehicle beyond maxSpawnDistanceFromPlayer is distance-despawned ONLY if it is OUTSIDE the camera frustum (with frustumMargin). If it is on-screen it is kept (or faded, for end-of-track / chunk-unload) so it never pops out in front of the player. It can still be despawned once past despawnDistance regardless of visibility. Serialized field revived from a previous plugin version.")]
		public bool despawnOnlyOutsideFrustum = true;

		// Token: 0x0400008F RID: 143
		[Tooltip("Hysteresis floor (m): a vehicle closer than this to the player is NEVER distance-despawned, preventing flip-flop spawn/despawn right at the player. Serialized field revived from a previous plugin version.")]
		[Range(0f, 200f)]
		public float safeDespawnRadius = 50f;

		// Token: 0x04000090 RID: 144
		[Tooltip("Fade-out duration (s) used when a VISIBLE vehicle must despawn at end-of-track or on chunk-unload. Off-screen vehicles despawn instantly (no fade cost).")]
		[Range(0.1f, 3f)]
		public float visibleDespawnFadeDuration = 0.7f;

		// Token: 0x04000091 RID: 145
		[Tooltip("Max number of vehicles fading out simultaneously. Extra requests queue; over the cap a still-visible car is kept a little longer rather than fading (perf ceiling).")]
		[Range(1f, 8f)]
		public int maxConcurrentFades = 3;

		// Token: 0x04000092 RID: 146
		[Header("Simulation LOD (DESIGN §5 — Simulation Bubble)")]
		[Tooltip("Full-simulation radius (m) around the player. Vehicles within this distance run the physical obstacle sensing (SenseAhead SphereCast + IDM car-following). Beyond it they keep advancing along the spline at the effective speed limit but skip the SphereCast — invisible/offscreen traffic costs almost nothing. Set to a value >= maxSpawnDistance to disable the LOD (all vehicles fully simulated).")]
		[Range(20f, 500f)]
		public float fullSimRadius = 60f;

		// Token: 0x04000093 RID: 147
		[Header("Frustum")]
		[Tooltip("When enabled, vehicles only spawn outside the camera frustum.")]
		public bool spawnOutsideFrustum = true;

		// Token: 0x04000094 RID: 148
		[Range(0f, 30f)]
		public float frustumMargin = 5f;

		// Token: 0x04000095 RID: 149
		[Header("Simplified — One Vehicle Per Lane (2026-07-16)")]
		[Tooltip("When true, cars ignore other CARS in SenseAhead (they still brake for the player). Meant for the 'one vehicle per lane' setup (TrackData.maxVehicles=1): two cars never share a lane so car-following is unnecessary, and this stops the false brake when the SphereCast catches the oncoming car on the parallel opposite lane.")]
		public bool oneVehiclePerLane = true;

		// Token: 0x04000096 RID: 150
		[Header("Cross-Chunk Connection")]
		[Tooltip("Max distance between end of one track and start of the next to be considered connected.")]
		[Range(0.1f, 5f)]
		public float connectionThreshold = 1.5f;

		// Token: 0x04000097 RID: 151
		[Tooltip("Min dot product between exit tangent and entry tangent. 1=same dir, 0=perpendicular, -1=opposite. Use -0.2 to allow corners while blocking U-turns.")]
		[Range(-1f, 1f)]
		public float connectionMinDot = -0.2f;
	}
}
