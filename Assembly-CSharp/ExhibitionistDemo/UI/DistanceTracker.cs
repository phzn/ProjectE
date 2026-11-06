using System;
using Features.RenderingPrecision;
using Features.RunModes;
using UnityEngine;

namespace ExhibitionistDemo.UI
{
	// Token: 0x02000794 RID: 1940
	public class DistanceTracker : MonoBehaviour
	{
		// Token: 0x06003306 RID: 13062 RVA: 0x00121CDC File Offset: 0x0011FEDC
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x00121CEF File Offset: 0x0011FEEF
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x00121D02 File Offset: 0x0011FF02
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			this.startPosition += delta;
			this.homeDestination += delta;
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x00121D28 File Offset: 0x0011FF28
		private void Start()
		{
			this.InitializeStartingPoint();
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x00121D30 File Offset: 0x0011FF30
		private void Update()
		{
			if (this.autoUpdate && !this.sceneTrackingPaused)
			{
				this.UpdateDistance();
			}
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00121D48 File Offset: 0x0011FF48
		private void InitializeStartingPoint()
		{
			if (this.startingPoint != null)
			{
				this.startPosition = this.startingPoint.position;
				Debug.Log(string.Format("DistanceTracker: Using starting point at {0}", this.startPosition));
			}
			else
			{
				this.startPosition = base.transform.position;
				Debug.Log(string.Format("DistanceTracker: No starting point assigned, using initial player position at {0}", this.startPosition));
			}
			this.currentDistance = 0f;
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x00121DC6 File Offset: 0x0011FFC6
		private void UpdateDistance()
		{
			this.currentDistance = Vector3.Distance(this.startPosition, base.transform.position);
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x00121DE4 File Offset: 0x0011FFE4
		public float GetDistance()
		{
			if (this.sceneTrackingPaused)
			{
				return this.pausedDistance;
			}
			if (!this.autoUpdate)
			{
				this.UpdateDistance();
			}
			return this.currentDistance + this.bonusDistance;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00121E10 File Offset: 0x00120010
		public float GetDistance2D()
		{
			if (this.sceneTrackingPaused)
			{
				return this.pausedDistance2D;
			}
			return this.CalculateDistance2D() + this.bonusDistance;
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00121E30 File Offset: 0x00120030
		private float CalculateDistance2D()
		{
			Vector3 vector = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			return Vector3.Distance(new Vector3(this.startPosition.x, 0f, this.startPosition.z), vector);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00121E90 File Offset: 0x00120090
		public float GetDisplayDistance(bool use2D = true)
		{
			if (!RunModeService.IsLongWayHome)
			{
				float num = (this.sceneTrackingPaused ? this.pausedBonusDistance : this.bonusDistance);
				return Mathf.Max(0f, (use2D ? this.GetDistance2D() : this.GetDistance()) - num);
			}
			if (this.hasHomeDestination)
			{
				if (!this.sceneTrackingPaused)
				{
					Vector3 vector = base.transform.position - this.homeDestination;
					vector.y = 0f;
					this.homeDisplayDistance = vector.magnitude;
				}
				return this.homeDisplayDistance;
			}
			if (!this.hasHeldHomeDistance)
			{
				return 0f;
			}
			return this.homeDisplayDistance;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x00121F34 File Offset: 0x00120134
		public void SetHomeDestination(Vector3 destination)
		{
			if (!RunModeService.IsLongWayHome)
			{
				return;
			}
			this.homeDestination = destination;
			this.hasHomeDestination = true;
			this.hasHeldHomeDistance = false;
			this.GetDisplayDistance(true);
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x00121F5B File Offset: 0x0012015B
		public void SetHeldHomeDistance(float meters)
		{
			if (!RunModeService.IsLongWayHome)
			{
				return;
			}
			this.hasHomeDestination = false;
			this.hasHeldHomeDistance = true;
			this.homeDisplayDistance = Mathf.Max(0f, meters);
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x00121F84 File Offset: 0x00120184
		public void SetSceneTrackingPaused(bool paused)
		{
			if (this.sceneTrackingPaused == paused)
			{
				return;
			}
			if (paused)
			{
				this.GetDisplayDistance(true);
				this.UpdateDistance();
				this.pausedDistance = this.currentDistance + this.bonusDistance;
				this.pausedDistance2D = this.CalculateDistance2D() + this.bonusDistance;
				this.pausedBonusDistance = this.bonusDistance;
				this.sceneTrackingPaused = true;
				return;
			}
			this.sceneTrackingPaused = false;
			this.UpdateDistance();
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x00121FF3 File Offset: 0x001201F3
		public bool IsSceneTrackingPaused
		{
			get
			{
				return this.sceneTrackingPaused;
			}
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x00121FFB File Offset: 0x001201FB
		public void ExcludeRecoveryDisplacement(Vector3 displacement)
		{
			if (this.sceneTrackingPaused)
			{
				return;
			}
			displacement.y = 0f;
			this.startPosition += displacement;
			this.UpdateDistance();
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x0012202A File Offset: 0x0012022A
		public void AddDistance(float meters)
		{
			if (meters <= 0f)
			{
				return;
			}
			this.bonusDistance += meters;
			Debug.Log(string.Format("[DistanceTracker] +{0:F0} m (bonus totale {1:F0} m).", meters, this.bonusDistance));
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x00122063 File Offset: 0x00120263
		public float BonusDistance
		{
			get
			{
				return this.bonusDistance;
			}
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x0012206B File Offset: 0x0012026B
		public void ResetStartingPoint()
		{
			this.startPosition = base.transform.position;
			this.currentDistance = 0f;
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x0012208C File Offset: 0x0012028C
		public void ResetForNewRun()
		{
			this.startingPoint = null;
			this.bonusDistance = 0f;
			this.startPosition = base.transform.position;
			this.currentDistance = 0f;
			this.sceneTrackingPaused = false;
			this.pausedDistance = 0f;
			this.pausedDistance2D = 0f;
			this.pausedBonusDistance = 0f;
			this.hasHomeDestination = false;
			this.hasHeldHomeDistance = false;
			this.homeDestination = Vector3.zero;
			this.homeDisplayDistance = 0f;
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x00122113 File Offset: 0x00120313
		public void SetStartingPoint(Vector3 newStartPosition)
		{
			this.startPosition = newStartPosition;
			this.UpdateDistance();
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00122122 File Offset: 0x00120322
		public void SetStartingPoint(Transform newStartPoint)
		{
			if (newStartPoint != null)
			{
				this.startingPoint = newStartPoint;
				this.startPosition = newStartPoint.position;
				this.UpdateDistance();
			}
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x00122148 File Offset: 0x00120348
		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(this.startPosition, 1f);
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(this.startPosition, base.transform.position);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(base.transform.position, 0.5f);
			}
		}

		// Token: 0x040033B3 RID: 13235
		[Header("Starting Point")]
		[Tooltip("The starting point reference (empty GameObject). If null, uses initial player position")]
		public Transform startingPoint;

		// Token: 0x040033B4 RID: 13236
		[Header("Settings")]
		[Tooltip("Update distance every frame or only when requested")]
		public bool autoUpdate = true;

		// Token: 0x040033B5 RID: 13237
		private Vector3 startPosition;

		// Token: 0x040033B6 RID: 13238
		private float currentDistance;

		// Token: 0x040033B7 RID: 13239
		private bool sceneTrackingPaused;

		// Token: 0x040033B8 RID: 13240
		private float pausedDistance;

		// Token: 0x040033B9 RID: 13241
		private float pausedDistance2D;

		// Token: 0x040033BA RID: 13242
		private float pausedBonusDistance;

		// Token: 0x040033BB RID: 13243
		private bool hasHomeDestination;

		// Token: 0x040033BC RID: 13244
		private Vector3 homeDestination;

		// Token: 0x040033BD RID: 13245
		private float homeDisplayDistance;

		// Token: 0x040033BE RID: 13246
		private bool hasHeldHomeDistance;

		// Token: 0x040033BF RID: 13247
		private float bonusDistance;
	}
}
