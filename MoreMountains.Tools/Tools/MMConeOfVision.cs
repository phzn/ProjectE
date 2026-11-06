using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000099 RID: 153
	[AddComponentMenu("More Mountains/Tools/Vision/MMConeOfVision")]
	[Serializable]
	public class MMConeOfVision : MonoBehaviour
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600034C RID: 844 RVA: 0x000109D0 File Offset: 0x0000EBD0
		public virtual Vector3 Center
		{
			get
			{
				return base.transform.position + this.Offset;
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000109E8 File Offset: 0x0000EBE8
		protected virtual void Awake()
		{
			this._visionMesh = new Mesh();
			if (this.ShouldDrawMesh)
			{
				this.VisionMeshFilter.mesh = this._visionMesh;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00010A0E File Offset: 0x0000EC0E
		protected virtual void LateUpdate()
		{
			if (Time.time - this._lastScanTimestamp > this.ScanFrequencyInSeconds && this.ShouldScanForTargets)
			{
				this.ScanForTargets();
			}
			this.DrawMesh();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00010A38 File Offset: 0x0000EC38
		public virtual void SetDirectionAndAngles(Vector3 direction, Vector3 eulerAngles)
		{
			this.Direction = direction;
			this.EulerAngles = eulerAngles;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00010A48 File Offset: 0x0000EC48
		protected virtual void ScanForTargets()
		{
			this._lastScanTimestamp = Time.time;
			this.VisibleTargets.Clear();
			this._targetsWithinDistance = Physics.OverlapSphere(this.Center, this.VisionRadius, this.TargetMask);
			foreach (Collider collider in this._targetsWithinDistance)
			{
				this._target = collider.transform;
				this._directionToTarget = (this._target.position - this.Center).normalized;
				if (Vector3.Angle(this.Direction, this._directionToTarget) < this.VisionAngle / 2f)
				{
					this._distanceToTarget = Vector3.Distance(this.Center, this._target.position);
					bool flag = false;
					using (List<Transform>.Enumerator enumerator = this.VisibleTargets.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == this._target)
							{
								flag = true;
							}
						}
					}
					if (!Physics.Raycast(this.Center, this._directionToTarget, this._distanceToTarget, this.ObstacleMask) && !flag)
					{
						this.VisibleTargets.Add(this._target);
					}
				}
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		protected virtual void DrawMesh()
		{
			if (!this.ShouldDrawMesh)
			{
				return;
			}
			int num = Mathf.RoundToInt(this.MeshDensity * this.VisionAngle);
			float num2 = this.VisionAngle / (float)num;
			this._viewPoints.Clear();
			for (int i = 0; i <= num; i++)
			{
				float num3 = num2 * (float)i + this.EulerAngles.y - this.VisionAngle / 2f;
				this._viewCast = this.RaycastAtAngle(num3);
				if (i > 0)
				{
					bool flag = Mathf.Abs(this._oldViewCast.Distance - this._viewCast.Distance) > this.EdgeThreshold;
					if (this._oldViewCast.Hit != this._viewCast.Hit || (this._oldViewCast.Hit && this._viewCast.Hit && flag))
					{
						MMConeOfVision.MeshEdgePosition meshEdgePosition = this.FindMeshEdgePosition(this._oldViewCast, this._viewCast);
						if (meshEdgePosition.PointA != Vector3.zero)
						{
							this._viewPoints.Add(meshEdgePosition.PointA);
						}
						if (meshEdgePosition.PointB != Vector3.zero)
						{
							this._viewPoints.Add(meshEdgePosition.PointB);
						}
					}
				}
				this._viewPoints.Add(this._viewCast.Point);
				this._oldViewCast = this._viewCast;
			}
			int num4 = this._viewPoints.Count + 1;
			if (num4 != this._numberOfVerticesLastTime)
			{
				Array.Resize<Vector3>(ref this._vertices, num4);
				Array.Resize<int>(ref this._triangles, (num4 - 2) * 3);
			}
			this._vertices[0] = this.Offset;
			for (int j = 0; j < num4 - 1; j++)
			{
				this._vertices[j + 1] = base.transform.InverseTransformPoint(this._viewPoints[j]);
				if (j < num4 - 2)
				{
					this._triangles[j * 3] = 0;
					this._triangles[j * 3 + 1] = j + 1;
					this._triangles[j * 3 + 2] = j + 2;
				}
			}
			this._visionMesh.Clear();
			this._visionMesh.vertices = this._vertices;
			this._visionMesh.triangles = this._triangles;
			this._visionMesh.RecalculateNormals();
			this._numberOfVerticesLastTime = num4;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00010DFC File Offset: 0x0000EFFC
		private MMConeOfVision.MeshEdgePosition FindMeshEdgePosition(MMConeOfVision.RaycastData minimumViewCast, MMConeOfVision.RaycastData maximumViewCast)
		{
			float num = minimumViewCast.Angle;
			float num2 = maximumViewCast.Angle;
			this._minPoint = minimumViewCast.Point;
			this._maxPoint = maximumViewCast.Point;
			for (int i = 0; i < this.EdgePrecision; i++)
			{
				float num3 = (num + num2) / 2f;
				MMConeOfVision.RaycastData raycastData = this.RaycastAtAngle(num3);
				bool flag = Mathf.Abs(minimumViewCast.Distance - raycastData.Distance) > this.EdgeThreshold;
				if (raycastData.Hit == minimumViewCast.Hit && !flag)
				{
					num = num3;
					this._minPoint = raycastData.Point;
				}
				else
				{
					num2 = num3;
					this._maxPoint = raycastData.Point;
				}
			}
			return new MMConeOfVision.MeshEdgePosition(this._minPoint, this._maxPoint);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00010EB4 File Offset: 0x0000F0B4
		private MMConeOfVision.RaycastData RaycastAtAngle(float angle)
		{
			this._direction = MMMaths.DirectionFromAngle(angle, 0f);
			if (Physics.Raycast(this.Center, this._direction, out this._raycastAtAngleHit, this.VisionRadius, this.ObstacleMask))
			{
				this._returnRaycastData.Hit = true;
				this._returnRaycastData.Point = this._raycastAtAngleHit.point;
				this._returnRaycastData.Distance = this._raycastAtAngleHit.distance;
				this._returnRaycastData.Angle = angle;
			}
			else
			{
				this._returnRaycastData.Hit = false;
				this._returnRaycastData.Point = this.Center + this._direction * this.VisionRadius;
				this._returnRaycastData.Distance = this.VisionRadius;
				this._returnRaycastData.Angle = angle;
			}
			return this._returnRaycastData;
		}

		// Token: 0x04000483 RID: 1155
		[Header("Vision")]
		public LayerMask ObstacleMask;

		// Token: 0x04000484 RID: 1156
		public float VisionRadius = 5f;

		// Token: 0x04000485 RID: 1157
		[Range(0f, 360f)]
		public float VisionAngle = 20f;

		// Token: 0x04000486 RID: 1158
		[MMReadOnly]
		public Vector3 Direction;

		// Token: 0x04000487 RID: 1159
		[MMReadOnly]
		public Vector3 EulerAngles;

		// Token: 0x04000488 RID: 1160
		public Vector3 Offset;

		// Token: 0x04000489 RID: 1161
		[Header("Target scanning")]
		public bool ShouldScanForTargets = true;

		// Token: 0x0400048A RID: 1162
		public LayerMask TargetMask;

		// Token: 0x0400048B RID: 1163
		public float ScanFrequencyInSeconds = 1f;

		// Token: 0x0400048C RID: 1164
		[MMReadOnly]
		public List<Transform> VisibleTargets = new List<Transform>();

		// Token: 0x0400048D RID: 1165
		[Header("Mesh")]
		public bool ShouldDrawMesh = true;

		// Token: 0x0400048E RID: 1166
		public float MeshDensity = 0.2f;

		// Token: 0x0400048F RID: 1167
		public int EdgePrecision = 3;

		// Token: 0x04000490 RID: 1168
		public float EdgeThreshold = 0.5f;

		// Token: 0x04000491 RID: 1169
		public MeshFilter VisionMeshFilter;

		// Token: 0x04000492 RID: 1170
		protected Mesh _visionMesh;

		// Token: 0x04000493 RID: 1171
		protected Collider[] _targetsWithinDistance;

		// Token: 0x04000494 RID: 1172
		protected Transform _target;

		// Token: 0x04000495 RID: 1173
		protected Vector3 _directionToTarget;

		// Token: 0x04000496 RID: 1174
		protected float _distanceToTarget;

		// Token: 0x04000497 RID: 1175
		protected float _lastScanTimestamp;

		// Token: 0x04000498 RID: 1176
		protected List<Vector3> _viewPoints = new List<Vector3>();

		// Token: 0x04000499 RID: 1177
		protected MMConeOfVision.RaycastData _oldViewCast;

		// Token: 0x0400049A RID: 1178
		protected MMConeOfVision.RaycastData _viewCast;

		// Token: 0x0400049B RID: 1179
		protected Vector3[] _vertices;

		// Token: 0x0400049C RID: 1180
		protected int[] _triangles;

		// Token: 0x0400049D RID: 1181
		protected Vector3 _minPoint;

		// Token: 0x0400049E RID: 1182
		protected Vector3 _maxPoint;

		// Token: 0x0400049F RID: 1183
		protected Vector3 _direction;

		// Token: 0x040004A0 RID: 1184
		protected MMConeOfVision.RaycastData _returnRaycastData;

		// Token: 0x040004A1 RID: 1185
		protected RaycastHit _raycastAtAngleHit;

		// Token: 0x040004A2 RID: 1186
		protected int _numberOfVerticesLastTime;

		// Token: 0x020003F3 RID: 1011
		public struct RaycastData
		{
			// Token: 0x06001946 RID: 6470 RVA: 0x0006AD54 File Offset: 0x00068F54
			public RaycastData(bool hit, Vector3 point, float distance, float angle)
			{
				this.Hit = hit;
				this.Point = point;
				this.Distance = distance;
				this.Angle = angle;
			}

			// Token: 0x04001E9A RID: 7834
			public bool Hit;

			// Token: 0x04001E9B RID: 7835
			public Vector3 Point;

			// Token: 0x04001E9C RID: 7836
			public float Distance;

			// Token: 0x04001E9D RID: 7837
			public float Angle;
		}

		// Token: 0x020003F4 RID: 1012
		public struct MeshEdgePosition
		{
			// Token: 0x06001947 RID: 6471 RVA: 0x0006AD73 File Offset: 0x00068F73
			public MeshEdgePosition(Vector3 pointA, Vector3 pointB)
			{
				this.PointA = pointA;
				this.PointB = pointB;
			}

			// Token: 0x04001E9E RID: 7838
			public Vector3 PointA;

			// Token: 0x04001E9F RID: 7839
			public Vector3 PointB;
		}
	}
}
