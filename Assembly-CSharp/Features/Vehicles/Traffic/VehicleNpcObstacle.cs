using System;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BB0 RID: 2992
	[DisallowMultipleComponent]
	public class VehicleNpcObstacle : MonoBehaviour
	{
		// Token: 0x060055D7 RID: 21975 RVA: 0x00214D5B File Offset: 0x00212F5B
		private void Awake()
		{
			this._ai = base.GetComponent<VehicleAI>();
			this.ConfigureObstacleGeometry();
		}

		// Token: 0x060055D8 RID: 21976 RVA: 0x00214D70 File Offset: 0x00212F70
		private void OnEnable()
		{
			this.ClearObstacles();
			this._nextCheck = Time.time + Mathf.Max(0.05f, this.checkInterval);
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x00214D94 File Offset: 0x00212F94
		private void OnDisable()
		{
			this.ClearObstacles();
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x00214D9C File Offset: 0x00212F9C
		public bool ConfigureObstacleGeometry()
		{
			this._obstacle = base.GetComponent<RVOSquareObstacle>();
			this._cut = base.GetComponent<NavmeshCut>();
			this.ClearObstacles();
			Bounds bounds;
			if (!this.TryGetBodyBounds(out bounds))
			{
				return false;
			}
			if (this._obstacle == null)
			{
				this._obstacle = base.gameObject.AddComponent<RVOSquareObstacle>();
			}
			this._obstacle.enabled = false;
			this._obstacle.size = new Vector2(bounds.size.x, bounds.size.z);
			this._obstacle.center = new Vector2(bounds.center.x, bounds.center.z);
			this._obstacle.height = Mathf.Max(1f, bounds.size.y);
			if (this._cut == null)
			{
				this._cut = base.gameObject.AddComponent<NavmeshCut>();
			}
			this._cut.enabled = false;
			this._cut.type = NavmeshCut.MeshType.Box;
			this._cut.useRotationAndScale = true;
			this._cut.isDual = false;
			this._cut.radiusExpansionMode = NavmeshCut.RadiusExpansionMode.ExpandByAgentRadius;
			this._cut.rectangleSize = new Vector2(bounds.size.x, bounds.size.z);
			float num = Mathf.Min(0f, bounds.min.y) - 0.35f;
			float num2 = Mathf.Max(num + 1f, bounds.max.y + 0.1f);
			this._cut.center = new Vector3(bounds.center.x, (num + num2) * 0.5f, bounds.center.z);
			this._cut.height = num2 - num;
			this._cut.updateDistance = 0.4f;
			this._cut.updateRotationDistance = 10f;
			return true;
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x00214F90 File Offset: 0x00213190
		private bool TryGetBodyBounds(out Bounds bounds)
		{
			foreach (Collider collider in base.GetComponents<Collider>())
			{
				if (!collider.isTrigger)
				{
					MeshCollider meshCollider = collider as MeshCollider;
					if (meshCollider != null && meshCollider.sharedMesh != null)
					{
						bounds = meshCollider.sharedMesh.bounds;
						return bounds.size.x > 0f && bounds.size.z > 0f;
					}
					BoxCollider boxCollider = collider as BoxCollider;
					if (boxCollider != null)
					{
						bounds = new Bounds(boxCollider.center, boxCollider.size);
						return bounds.size.x > 0f && bounds.size.z > 0f;
					}
				}
			}
			bounds = default(Bounds);
			return false;
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x0021506C File Offset: 0x0021326C
		private void Update()
		{
			if (VehicleSimulationPause.IsPaused)
			{
				return;
			}
			if (this._ai == null)
			{
				return;
			}
			if (Time.time < this._nextCheck)
			{
				return;
			}
			this._nextCheck = Time.time + Mathf.Max(0.05f, this.checkInterval);
			this.RefreshObstacleState(Mathf.Abs(this._ai.CurrentSpeed), Time.time);
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x002150D8 File Offset: 0x002132D8
		private void RefreshObstacleState(float speed, float now)
		{
			bool flag = speed < this.activeBelowSpeed;
			if (this._obstacle != null && this._obstacle.enabled != flag)
			{
				this._obstacle.enabled = flag;
			}
			if (this._cut == null)
			{
				return;
			}
			if (this._cut.enabled)
			{
				if (speed >= Mathf.Max(this.cutBelowSpeed, this.releaseCutAboveSpeed))
				{
					this._cut.enabled = false;
					this._stoppedSince = -1f;
				}
				return;
			}
			if (speed > this.cutBelowSpeed)
			{
				this._stoppedSince = -1f;
				return;
			}
			if (this._stoppedSince < 0f)
			{
				this._stoppedSince = now;
			}
			if (now - this._stoppedSince >= this.stoppedDelay)
			{
				this._cut.enabled = true;
			}
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x002151A4 File Offset: 0x002133A4
		private void ClearObstacles()
		{
			this._stoppedSince = -1f;
			if (this._obstacle != null)
			{
				this._obstacle.enabled = false;
			}
			if (this._cut != null)
			{
				this._cut.enabled = false;
			}
		}

		// Token: 0x04005637 RID: 22071
		[Tooltip("The local avoidance obstacle is active below this speed (m/s).")]
		[SerializeField]
		private float activeBelowSpeed = 1f;

		// Token: 0x04005638 RID: 22072
		[SerializeField]
		private float checkInterval = 0.25f;

		// Token: 0x04005639 RID: 22073
		[Tooltip("Only cars below this speed (m/s) may start cutting the pedestrian navmesh.")]
		[SerializeField]
		[Min(0f)]
		private float cutBelowSpeed = 0.15f;

		// Token: 0x0400563A RID: 22074
		[Tooltip("Remove the cut once the car resumes moving (m/s).")]
		[SerializeField]
		[Min(0f)]
		private float releaseCutAboveSpeed = 0.3f;

		// Token: 0x0400563B RID: 22075
		[Tooltip("Seconds of a settled stop before changing the graph.")]
		[SerializeField]
		[Min(0f)]
		private float stoppedDelay = 0.75f;

		// Token: 0x0400563C RID: 22076
		private VehicleAI _ai;

		// Token: 0x0400563D RID: 22077
		private RVOSquareObstacle _obstacle;

		// Token: 0x0400563E RID: 22078
		private NavmeshCut _cut;

		// Token: 0x0400563F RID: 22079
		private float _nextCheck;

		// Token: 0x04005640 RID: 22080
		private float _stoppedSince = -1f;
	}
}
