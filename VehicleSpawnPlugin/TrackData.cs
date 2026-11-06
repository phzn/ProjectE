using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000011 RID: 17
	[RequireComponent(typeof(SplineContainer))]
	public class TrackData : MonoBehaviour
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000070D6 File Offset: 0x000052D6
		public SplineContainer SplineContainer
		{
			get
			{
				if (this._container == null)
				{
					this._container = base.GetComponent<SplineContainer>();
				}
				return this._container;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000070F8 File Offset: 0x000052F8
		public bool HasValidPath
		{
			get
			{
				return this.SplineContainer != null && this.SplineContainer.Spline != null && this.SplineContainer.Spline.Count >= 2;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000712D File Offset: 0x0000532D
		public float GetLength()
		{
			return this.SplineContainer.Spline.CalculateLength(this.SplineContainer.transform.localToWorldMatrix);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00007154 File Offset: 0x00005354
		public Vector3 GetWorldPosition(float t)
		{
			float3 @float = this.SplineContainer.Spline.EvaluatePosition(t);
			return this.SplineContainer.transform.TransformPoint(@float);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000718C File Offset: 0x0000538C
		public Vector3 GetWorldTangent(float t)
		{
			float3 @float = this.SplineContainer.Spline.EvaluateTangent(t);
			return this.SplineContainer.transform.TransformDirection(math.normalize(@float));
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000071C8 File Offset: 0x000053C8
		public float GetClosestT(Vector3 worldPos)
		{
			float3 @float = this.SplineContainer.transform.InverseTransformPoint(worldPos);
			float3 float2;
			float num;
			SplineUtility.GetNearestPoint<Spline>(this.SplineContainer.Spline, @float, out float2, out num, 4, 2);
			return num;
		}

		// Token: 0x040000B0 RID: 176
		[Tooltip("Maximum speed allowed on this track (m/s). Overrides vehicle cruise speed if lower.")]
		[Range(1f, 60f)]
		public float speedLimit = 14f;

		// Token: 0x040000B1 RID: 177
		[Tooltip("If true the spline is treated as a closed loop — vehicle circles indefinitely.")]
		public bool isLoop;

		// Token: 0x040000B2 RID: 178
		[Tooltip("Spawn points on this track. Place them at the start of the lane, outside the player view range.")]
		public List<Transform> spawnPoints = new List<Transform>();

		// Token: 0x040000B3 RID: 179
		[Tooltip("Maximum simultaneous vehicles on this track.")]
		[Range(1f, 30f)]
		public int maxVehicles = 5;

		// Token: 0x040000B4 RID: 180
		[Tooltip("Relative likelihood this track is chosen as the next lane at an intersection. Higher = more traffic flows onto it (e.g. straight-through 4, right turn 2, left turn 1). 0 disables selection of this track as a connection.")]
		[Range(0f, 10f)]
		public float connectionWeight = 1f;

		// Token: 0x040000B5 RID: 181
		[Tooltip("Intersection mutex group. Tracks whose paths CROSS at an intersection centre (X cross, T branch) share the same non-empty id → the spawner treats the whole group as ONE slot (at most one vehicle across all of them at a time). Prevents two cars on DIFFERENT turn lanes colliding where the lanes intersect. Empty = no group (normal per-lane capacity).")]
		public string intersectionGroupId = "";

		// Token: 0x040000B6 RID: 182
		private SplineContainer _container;
	}
}
