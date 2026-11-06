using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000089 RID: 137
	[RequireComponent(typeof(LineRenderer))]
	public class MMLineRendererCircle : MonoBehaviour
	{
		// Token: 0x06000307 RID: 775 RVA: 0x0000F925 File Offset: 0x0000DB25
		protected virtual void Awake()
		{
			this.Initialization();
			this.DrawCircle();
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000F933 File Offset: 0x0000DB33
		protected virtual void Initialization()
		{
			this._line = base.gameObject.GetComponent<LineRenderer>();
			this._line.positionCount = this.PositionsCount + 1;
			this._line.useWorldSpace = false;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000F968 File Offset: 0x0000DB68
		public virtual void DrawCircle()
		{
			this._angle = 0f;
			this._z = this.NormalOffset;
			switch (this.Axis)
			{
			case MMLineRendererCircle.DrawAxis.X:
				this.DrawCircleX();
				return;
			case MMLineRendererCircle.DrawAxis.Y:
				this.DrawCircleY();
				return;
			case MMLineRendererCircle.DrawAxis.Z:
				this.DrawCircleZ();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000F9BA File Offset: 0x0000DBBA
		protected virtual float ComputeX()
		{
			return Mathf.Cos(0.017453292f * this._angle) * this.HorizontalRadius;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		protected virtual float ComputeY()
		{
			return Mathf.Sin(0.017453292f * this._angle) * this.VerticalRadius;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
		protected virtual void DrawCircleX()
		{
			for (int i = 0; i < this.PositionsCount + 1; i++)
			{
				this._x = this.ComputeX();
				this._y = this.ComputeY();
				this._newPosition.x = this._z;
				this._newPosition.y = this._y;
				this._newPosition.z = this._x;
				this._line.SetPosition(i, this._newPosition);
				this._angle += 360f / (float)this.PositionsCount;
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000FA8C File Offset: 0x0000DC8C
		protected virtual void DrawCircleY()
		{
			for (int i = 0; i < this.PositionsCount + 1; i++)
			{
				this._x = this.ComputeX();
				this._y = this.ComputeY();
				this._newPosition.x = this._y;
				this._newPosition.y = this._z;
				this._newPosition.z = this._x;
				this._line.SetPosition(i, this._newPosition);
				this._angle += 360f / (float)this.PositionsCount;
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000FB28 File Offset: 0x0000DD28
		protected virtual void DrawCircleZ()
		{
			for (int i = 0; i < this.PositionsCount + 1; i++)
			{
				this._x = this.ComputeX();
				this._y = this.ComputeY();
				this._newPosition.x = this._x;
				this._newPosition.y = this._y;
				this._newPosition.z = this._z;
				this._line.SetPosition(i, this._newPosition);
				this._angle += 360f / (float)this.PositionsCount;
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000FBC2 File Offset: 0x0000DDC2
		protected virtual void OnValidate()
		{
			if (this.AutoRedrawOnValuesChange)
			{
				this.DrawCircle();
			}
		}

		// Token: 0x0400043F RID: 1087
		[Header("Draw Axis")]
		[Tooltip("the axis on which to draw the circle")]
		public MMLineRendererCircle.DrawAxis Axis = MMLineRendererCircle.DrawAxis.Z;

		// Token: 0x04000440 RID: 1088
		[Tooltip("the distance by which to push the circle on the draw axis")]
		public float NormalOffset;

		// Token: 0x04000441 RID: 1089
		[Header("Geometry")]
		[Tooltip("the amount of segments on the line renderer. More segments, more smoothness, more performance cost")]
		[Range(0f, 2000f)]
		public int PositionsCount = 60;

		// Token: 0x04000442 RID: 1090
		[Header("Shape")]
		[Tooltip("the length of the circle's horizontal radius")]
		public float HorizontalRadius = 10f;

		// Token: 0x04000443 RID: 1091
		[Tooltip("the length of the circle's vertical radius")]
		public float VerticalRadius = 10f;

		// Token: 0x04000444 RID: 1092
		[Header("Debug")]
		[Tooltip("if this is true, the circle will be redrawn every time you change a value in the inspector, otherwise you'll have to call the DrawCircle method (or press the debug button below)")]
		public bool AutoRedrawOnValuesChange;

		// Token: 0x04000445 RID: 1093
		[MMInspectorButton("DrawCircle")]
		public bool DrawCircleButton;

		// Token: 0x04000446 RID: 1094
		protected LineRenderer _line;

		// Token: 0x04000447 RID: 1095
		protected Vector3 _newPosition;

		// Token: 0x04000448 RID: 1096
		protected float _angle;

		// Token: 0x04000449 RID: 1097
		protected float _x;

		// Token: 0x0400044A RID: 1098
		protected float _y;

		// Token: 0x0400044B RID: 1099
		protected float _z;

		// Token: 0x020003EE RID: 1006
		public enum DrawAxis
		{
			// Token: 0x04001E7E RID: 7806
			X,
			// Token: 0x04001E7F RID: 7807
			Y,
			// Token: 0x04001E80 RID: 7808
			Z
		}
	}
}
