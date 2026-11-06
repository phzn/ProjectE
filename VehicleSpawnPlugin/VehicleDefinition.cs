using System;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x0200000B RID: 11
	[CreateAssetMenu(menuName = "VehicleSpawnPlugin/Vehicle Definition", fileName = "NewVehicleDefinition")]
	public class VehicleDefinition : ScriptableObject
	{
		// Token: 0x04000098 RID: 152
		[Header("Identity")]
		public string vehicleId;

		// Token: 0x04000099 RID: 153
		public string displayName;

		// Token: 0x0400009A RID: 154
		public GameObject prefab;

		// Token: 0x0400009B RID: 155
		[Header("Movement")]
		[Range(1f, 40f)]
		public float cruiseSpeed = 8f;

		// Token: 0x0400009C RID: 156
		[Range(0f, 0.5f)]
		public float speedVariation = 0.15f;

		// Token: 0x0400009D RID: 157
		[Range(1f, 10f)]
		public float turnSpeed = 3f;

		// Token: 0x0400009E RID: 158
		[Range(1f, 10f)]
		public float vehicleLength = 4f;

		// Token: 0x0400009F RID: 159
		[Tooltip("Rotation applied on top of the spline direction. Set Y=90 if your model's forward axis is X+, Y=-90 if X-, etc.")]
		public Vector3 rotationOffset = Vector3.zero;

		// Token: 0x040000A0 RID: 160
		[Header("Pool & Spawn")]
		[Range(1f, 50f)]
		public int poolSize = 10;

		// Token: 0x040000A1 RID: 161
		[Range(0.1f, 10f)]
		public float spawnWeight = 1f;

		// Token: 0x040000A2 RID: 162
		[Header("Detection")]
		public DetectionProfile detectionProfile;
	}
}
