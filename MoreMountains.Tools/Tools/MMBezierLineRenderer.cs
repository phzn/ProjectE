using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000087 RID: 135
	[ExecuteAlways]
	[RequireComponent(typeof(LineRenderer))]
	[AddComponentMenu("More Mountains/Tools/Sprites/MMBezierLineRenderer")]
	public class MMBezierLineRenderer : MonoBehaviour
	{
		// Token: 0x060002FD RID: 765 RVA: 0x0000F5E7 File Offset: 0x0000D7E7
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000F5F0 File Offset: 0x0000D7F0
		protected virtual void Initialization()
		{
			if (this._initialized)
			{
				return;
			}
			this._sortingLayerID = SortingLayer.NameToID(this.SortingLayerName);
			this.NumberOfCurves = this.AdjustmentHandles.Length / 3;
			this._lineRenderer = base.GetComponent<LineRenderer>();
			if (this._lineRenderer != null)
			{
				this._lineRenderer.sortingLayerID = this._sortingLayerID;
			}
			this._initialized = true;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000F659 File Offset: 0x0000D859
		protected virtual void LateUpdate()
		{
			this.DrawCurve();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000F664 File Offset: 0x0000D864
		protected virtual void DrawCurve()
		{
			for (int i = 0; i < this.NumberOfCurves; i++)
			{
				for (int j = 1; j <= this.NumberOfSegments; j++)
				{
					float num = (float)(j - 1) / (float)(this.NumberOfSegments - 1);
					int num2 = i * 3;
					this._point = this.BezierPoint(num, this.AdjustmentHandles[num2].position, this.AdjustmentHandles[num2 + 1].position, this.AdjustmentHandles[num2 + 2].position, this.AdjustmentHandles[num2 + 3].position);
					this._lineRenderer.positionCount = i * this.NumberOfSegments + j;
					this._lineRenderer.SetPosition(i * this.NumberOfSegments + (j - 1), this._point);
				}
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000F72C File Offset: 0x0000D92C
		protected virtual Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float num = 1f - t;
			float num2 = t * t;
			float num3 = num * num;
			float num4 = num3 * num;
			float num5 = num2 * t;
			this._p = num4 * p0;
			this._p += 3f * num3 * t * p1;
			this._p += 3f * num * num2 * p2;
			this._p += num5 * p3;
			return this._p;
		}

		// Token: 0x0400042F RID: 1071
		public Transform[] AdjustmentHandles;

		// Token: 0x04000430 RID: 1072
		public int NumberOfSegments = 50;

		// Token: 0x04000431 RID: 1073
		public string SortingLayerName = "Default";

		// Token: 0x04000432 RID: 1074
		[MMReadOnly]
		public int NumberOfCurves;

		// Token: 0x04000433 RID: 1075
		protected int _sortingLayerID;

		// Token: 0x04000434 RID: 1076
		protected LineRenderer _lineRenderer;

		// Token: 0x04000435 RID: 1077
		protected Vector3 _point;

		// Token: 0x04000436 RID: 1078
		protected Vector3 _p;

		// Token: 0x04000437 RID: 1079
		protected bool _initialized;
	}
}
