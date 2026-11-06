using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public struct LimbPoseData
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00005274 File Offset: 0x00003474
		public static LimbPoseData Default
		{
			get
			{
				return new LimbPoseData
				{
					positionOffset = Vector3.zero,
					rotationOffset = Vector3.zero,
					ikWeight = 1f,
					hintWeight = 1f
				};
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000052BC File Offset: 0x000034BC
		public static LimbPoseData DefaultPelvis
		{
			get
			{
				return new LimbPoseData
				{
					positionOffset = Vector3.zero,
					rotationOffset = Vector3.zero,
					ikWeight = 0.3f,
					hintWeight = 0.3f
				};
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00005304 File Offset: 0x00003504
		public static LimbPoseData DefaultSpine
		{
			get
			{
				return new LimbPoseData
				{
					positionOffset = Vector3.zero,
					rotationOffset = Vector3.zero,
					ikWeight = 0.5f,
					hintWeight = 0.5f
				};
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000534C File Offset: 0x0000354C
		public static LimbPoseData Lerp(LimbPoseData a, LimbPoseData b, float t)
		{
			return new LimbPoseData
			{
				positionOffset = Vector3.Lerp(a.positionOffset, b.positionOffset, t),
				rotationOffset = LimbPoseData.LerpEuler(a.rotationOffset, b.rotationOffset, t),
				ikWeight = Mathf.Lerp(a.ikWeight, b.ikWeight, t),
				hintWeight = Mathf.Lerp(a.hintWeight, b.hintWeight, t)
			};
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000053C6 File Offset: 0x000035C6
		private static Vector3 LerpEuler(Vector3 a, Vector3 b, float t)
		{
			return new Vector3(Mathf.LerpAngle(a.x, b.x, t), Mathf.LerpAngle(a.y, b.y, t), Mathf.LerpAngle(a.z, b.z, t));
		}

		// Token: 0x04000085 RID: 133
		[Tooltip("Position offset relative to the vehicle anchor (local space)")]
		public Vector3 positionOffset;

		// Token: 0x04000086 RID: 134
		[Tooltip("Rotation offset relative to the vehicle anchor (euler angles)")]
		public Vector3 rotationOffset;

		// Token: 0x04000087 RID: 135
		[Range(0f, 1f)]
		[Tooltip("IK constraint weight (0 = animation, 1 = full IK)")]
		public float ikWeight;

		// Token: 0x04000088 RID: 136
		[Range(0f, 1f)]
		[Tooltip("Hint weight for pole vector control")]
		public float hintWeight;
	}
}
