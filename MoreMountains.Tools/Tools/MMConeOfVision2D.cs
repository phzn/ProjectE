using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200009A RID: 154
	[AddComponentMenu("More Mountains/Tools/Vision/MMConeOfVision2D")]
	[Serializable]
	public class MMConeOfVision2D : MonoBehaviour
	{
		// Token: 0x06000355 RID: 853 RVA: 0x0001100D File Offset: 0x0000F20D
		protected virtual void Awake()
		{
			this._visionMesh = new Mesh();
			this.Direction = Vector3.right;
			if (this.ShouldDrawMesh)
			{
				this.VisionMeshFilter.mesh = this._visionMesh;
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0001103E File Offset: 0x0000F23E
		protected virtual void LateUpdate()
		{
			if (Time.time - this._lastScanTimestamp > this.ScanFrequencyInSeconds && this.ShouldScanForTargets)
			{
				this.ScanForTargets();
			}
			this.DrawMesh();
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011068 File Offset: 0x0000F268
		public virtual void SetDirectionAndAngles(Vector3 direction, Vector3 eulerAngles)
		{
			this.Direction = direction;
			this.EulerAngles = eulerAngles;
			this.EulerAngles.y = this.EulerAngles.y + this.AngleOffset;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00011090 File Offset: 0x0000F290
		protected virtual void ScanForTargets()
		{
			this._lastScanTimestamp = Time.time;
			this.VisibleTargets.Clear();
			this._targetsWithinDistance = Physics2D.OverlapCircleAll(base.transform.position, this.VisionRadius, this.TargetMask);
			foreach (Collider2D collider2D in this._targetsWithinDistance)
			{
				this._target = collider2D.transform;
				this._directionToTarget = (this._target.position - base.transform.position).normalized;
				if (Vector3.Angle(this.Direction, this._directionToTarget) < this.VisionAngle / 2f)
				{
					this._distanceToTarget = Vector3.Distance(base.transform.position, this._target.position);
					this._scanForTargetsHit2D = Physics2D.Raycast(base.transform.position, this._directionToTarget, this._distanceToTarget, this.ObstacleMask);
					if (!this._scanForTargetsHit2D)
					{
						this.VisibleTargets.Add(this._target);
					}
				}
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000111C8 File Offset: 0x0000F3C8
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
						MMConeOfVision2D.MeshEdgePosition meshEdgePosition = this.FindMeshEdgePosition(this._oldViewCast, this._viewCast);
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
			this._vertices[0].x = 0f;
			this._vertices[0].y = 0f;
			this._vertices[0].z = 0f;
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

		// Token: 0x0600035A RID: 858 RVA: 0x0001144C File Offset: 0x0000F64C
		private MMConeOfVision2D.MeshEdgePosition FindMeshEdgePosition(MMConeOfVision2D.RaycastData minimumViewCast, MMConeOfVision2D.RaycastData maximumViewCast)
		{
			float num = minimumViewCast.Angle;
			float num2 = maximumViewCast.Angle;
			this._minPoint = minimumViewCast.Point;
			this._maxPoint = maximumViewCast.Point;
			for (int i = 0; i < this.EdgePrecision; i++)
			{
				float num3 = (num + num2) / 2f;
				MMConeOfVision2D.RaycastData raycastData = this.RaycastAtAngle(num3);
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
			return new MMConeOfVision2D.MeshEdgePosition(this._minPoint, this._maxPoint);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00011504 File Offset: 0x0000F704
		private MMConeOfVision2D.RaycastData RaycastAtAngle(float angle)
		{
			this._direction = MMMaths.DirectionFromAngle2D(angle, 0f);
			this._raycastAtAngleHit2D = Physics2D.Raycast(base.transform.position, this._direction, this.VisionRadius, this.ObstacleMask);
			if (this._raycastAtAngleHit2D)
			{
				this._returnRaycastData.Hit = true;
				this._returnRaycastData.Point = this._raycastAtAngleHit2D.point;
				this._returnRaycastData.Distance = this._raycastAtAngleHit2D.distance;
				this._returnRaycastData.Angle = angle;
			}
			else
			{
				this._returnRaycastData.Hit = false;
				this._returnRaycastData.Point = base.transform.position + this._direction * this.VisionRadius;
				this._returnRaycastData.Distance = this.VisionRadius;
				this._returnRaycastData.Angle = angle;
			}
			return this._returnRaycastData;
		}

		// Token: 0x040004A3 RID: 1187
		[Header("Vision")]
		public LayerMask ObstacleMask;

		// Token: 0x040004A4 RID: 1188
		public float VisionRadius = 5f;

		// Token: 0x040004A5 RID: 1189
		[Range(0f, 360f)]
		public float VisionAngle = 20f;

		// Token: 0x040004A6 RID: 1190
		[Range(0f, 360f)]
		public float AngleOffset;

		// Token: 0x040004A7 RID: 1191
		[MMReadOnly]
		public Vector3 Direction;

		// Token: 0x040004A8 RID: 1192
		[MMReadOnly]
		public Vector3 EulerAngles;

		// Token: 0x040004A9 RID: 1193
		[Header("Target scanning")]
		public bool ShouldScanForTargets = true;

		// Token: 0x040004AA RID: 1194
		public LayerMask TargetMask;

		// Token: 0x040004AB RID: 1195
		public float ScanFrequencyInSeconds = 1f;

		// Token: 0x040004AC RID: 1196
		[MMReadOnly]
		public List<Transform> VisibleTargets = new List<Transform>();

		// Token: 0x040004AD RID: 1197
		[Header("Mesh")]
		public bool ShouldDrawMesh = true;

		// Token: 0x040004AE RID: 1198
		public float MeshDensity = 0.2f;

		// Token: 0x040004AF RID: 1199
		public int EdgePrecision = 3;

		// Token: 0x040004B0 RID: 1200
		public float EdgeThreshold = 0.5f;

		// Token: 0x040004B1 RID: 1201
		public MeshFilter VisionMeshFilter;

		// Token: 0x040004B2 RID: 1202
		protected Mesh _visionMesh;

		// Token: 0x040004B3 RID: 1203
		protected Collider2D[] _targetsWithinDistance;

		// Token: 0x040004B4 RID: 1204
		protected Transform _target;

		// Token: 0x040004B5 RID: 1205
		protected Vector3 _directionToTarget;

		// Token: 0x040004B6 RID: 1206
		protected float _distanceToTarget;

		// Token: 0x040004B7 RID: 1207
		protected float _lastScanTimestamp;

		// Token: 0x040004B8 RID: 1208
		protected RaycastHit2D _scanForTargetsHit2D;

		// Token: 0x040004B9 RID: 1209
		protected List<Vector3> _viewPoints = new List<Vector3>();

		// Token: 0x040004BA RID: 1210
		protected MMConeOfVision2D.RaycastData _oldViewCast;

		// Token: 0x040004BB RID: 1211
		protected MMConeOfVision2D.RaycastData _viewCast;

		// Token: 0x040004BC RID: 1212
		protected Vector3[] _vertices;

		// Token: 0x040004BD RID: 1213
		protected int[] _triangles;

		// Token: 0x040004BE RID: 1214
		protected Vector3 _minPoint;

		// Token: 0x040004BF RID: 1215
		protected Vector3 _maxPoint;

		// Token: 0x040004C0 RID: 1216
		protected Vector3 _direction;

		// Token: 0x040004C1 RID: 1217
		protected MMConeOfVision2D.RaycastData _returnRaycastData;

		// Token: 0x040004C2 RID: 1218
		protected RaycastHit2D _raycastAtAngleHit2D;

		// Token: 0x040004C3 RID: 1219
		protected int _numberOfVerticesLastTime;

		// Token: 0x020003F5 RID: 1013
		public struct RaycastData
		{
			// Token: 0x06001948 RID: 6472 RVA: 0x0006AD83 File Offset: 0x00068F83
			public RaycastData(bool hit, Vector3 point, float distance, float angle)
			{
				this.Hit = hit;
				this.Point = point;
				this.Distance = distance;
				this.Angle = angle;
			}

			// Token: 0x04001EA0 RID: 7840
			public bool Hit;

			// Token: 0x04001EA1 RID: 7841
			public Vector3 Point;

			// Token: 0x04001EA2 RID: 7842
			public float Distance;

			// Token: 0x04001EA3 RID: 7843
			public float Angle;
		}

		// Token: 0x020003F6 RID: 1014
		public struct MeshEdgePosition
		{
			// Token: 0x06001949 RID: 6473 RVA: 0x0006ADA2 File Offset: 0x00068FA2
			public MeshEdgePosition(Vector3 pointA, Vector3 pointB)
			{
				this.PointA = pointA;
				this.PointB = pointB;
			}

			// Token: 0x04001EA4 RID: 7844
			public Vector3 PointA;

			// Token: 0x04001EA5 RID: 7845
			public Vector3 PointB;
		}
	}
}
