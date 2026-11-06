using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000026 RID: 38
	public class MMPlotter : MonoBehaviour
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00004B1C File Offset: 0x00002D1C
		public virtual string[] GetMethodsList()
		{
			this.FillMethodList();
			List<string> list = new List<string>();
			foreach (MethodInfo methodInfo in this._methodList)
			{
				list.Add(methodInfo.Name);
			}
			return list.ToArray();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004B88 File Offset: 0x00002D88
		public virtual float InvokeTween(int index, object[] parameters)
		{
			return (float)this._methodList[index].Invoke(this, parameters);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004BA2 File Offset: 0x00002DA2
		public virtual string TweenName(int index)
		{
			if (this._methodList == null)
			{
				this.FillMethodList();
			}
			return this._methodList[index].Name;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004BC4 File Offset: 0x00002DC4
		protected virtual void FillMethodList()
		{
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
			MethodInfo[] methods = typeof(MMTweenDefinitions).GetMethods(bindingFlags);
			this._methodList = methods.OrderBy<MethodInfo, string>((MethodInfo item) => item.Name).ToList<MethodInfo>();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004C15 File Offset: 0x00002E15
		protected virtual void OnEnable()
		{
			this._parameter = new object[1];
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004C23 File Offset: 0x00002E23
		protected virtual void Start()
		{
			this.FillMethodList();
			this.DrawGraph();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004C34 File Offset: 0x00002E34
		protected virtual void Initialization()
		{
			this._points = new Transform[this.Resolution];
			this.DistanceBetweenPoints = this.GraphSize / (float)this.Resolution;
			this._pointScale = this.DistanceBetweenPoints * this.PointScaleFactor;
			this._scale = this._pointScale * Vector3.one;
			this._position = Vector3.zero;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004C9A File Offset: 0x00002E9A
		public virtual void DrawGraph()
		{
			this.Cleanup();
			this.Initialization();
			this.DrawAxis();
			this.DrawPoints();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004CB4 File Offset: 0x00002EB4
		protected virtual void DrawAxis()
		{
			this._axis = Object.Instantiate<MMPlotterAxis>(this.Axis);
			this._axis.SetLabel(this.TweenName(this.TweenMethodIndex).Replace("_", " "));
			this._axis.transform.SetParent(base.transform);
			this._axis.transform.localPosition = Vector3.zero;
			this._positionPointInitialPosition = this._axis.PositionPoint.transform.localPosition;
			this._positionPointVerticalInitialPosition = this._axis.PositionPointVertical.transform.localPosition;
			this._rotationPointInitialRotation = this._axis.RotationPoint.transform.localEulerAngles;
			this._scalePointInitialScale = this._axis.ScalePoint.transform.localScale;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004D90 File Offset: 0x00002F90
		protected virtual void DrawPoints()
		{
			for (int i = 0; i < this._points.Length; i++)
			{
				this._point = Object.Instantiate<Transform>(this.PlotPointPrefab);
				this._point.name = base.name + "Point" + i.ToString();
				this._pointValues.x = (float)i * (1f / (float)this.Resolution);
				this._parameter[0] = this._pointValues.x;
				this._pointValues.y = this.InvokeTween(this.TweenMethodIndex, this._parameter);
				this._position.x = (float)i * this.DistanceBetweenPoints;
				this._position.y = this._pointValues.y * this.GraphSize;
				this._point.localPosition = this._position;
				this._point.localScale = this._scale;
				this._point.gameObject.MMGetComponentNoAlloc<MeshRenderer>().material = this.PlotPointMaterial;
				this._point.SetParent(base.transform, false);
				this._points[i] = this._point;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004EC7 File Offset: 0x000030C7
		public virtual void SetMaterial(Material newMaterial)
		{
			this.PlotPointMaterial = newMaterial;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004ED0 File Offset: 0x000030D0
		protected virtual void Cleanup()
		{
			base.transform.MMDestroyAllChildren();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004EE0 File Offset: 0x000030E0
		protected virtual void Update()
		{
			this._curvePointNewMovement = this.Vector3Zero;
			this._curvePointNewMovement.x = this._currentMovement;
			this._parameter[0] = this._currentMovement;
			this._newValue = this.InvokeTween(this.TweenMethodIndex, this._parameter);
			this._curvePointNewMovement.y = this._newValue;
			this._curvePointNewMovement *= this.GraphSize;
			this._axis.PlotterCurvePoint.transform.localPosition = this._curvePointNewMovement;
			this._curvePointNewMovement = this._positionPointInitialPosition;
			this._curvePointNewMovement.x = this._newValue;
			this._axis.PositionPoint.transform.localPosition = this._curvePointNewMovement;
			this._curvePointNewMovement = this._positionPointVerticalInitialPosition;
			this._curvePointNewMovement.y = this._newValue;
			this._axis.PositionPointVertical.transform.localPosition = this._curvePointNewMovement;
			this._curvePointNewMovement = this._rotationPointInitialRotation;
			this._curvePointNewMovement.z = this._newValue * 360f;
			this._axis.RotationPoint.transform.localEulerAngles = this._curvePointNewMovement;
			this._curvePointNewMovement = this._scalePointInitialScale;
			this._curvePointNewMovement *= this._newValue;
			this._axis.ScalePoint.transform.localScale = this._curvePointNewMovement;
			if (Time.unscaledTime - this._lastMovementEndedAt < this.MovementPauseDuration)
			{
				if (Time.unscaledTime - this._lastMovementEndedAt < this.MovementPauseDuration / 2f)
				{
					this._currentMovement = 1f;
					this._newScaleUnit = MMTween.Tween(Time.unscaledTime - this._lastMovementEndedAt, 0f, this.MovementPauseDuration / 2f, 1f, 0f, MMTween.MMTweenCurve.EaseInCubic);
					this._newScale = Vector3.one * this._newScaleUnit;
					this._axis.PlotterCurvePoint.localScale = this._newScale * 0.1f;
					this._axis.PositionPoint.localScale = this._newScale;
					this._axis.PositionPointVertical.localScale = this._newScale;
					this._axis.RotationPoint.localScale = this._newScale;
					this._axis.ScalePoint.localScale = this._newScale;
				}
				else
				{
					this._currentMovement = 0f;
					this._newScaleUnit = MMTween.Tween(Time.unscaledTime - this._lastMovementEndedAt, this.MovementPauseDuration / 2f, this.MovementPauseDuration, 0f, 1f, MMTween.MMTweenCurve.EaseOutCubic);
					this._newScale = Vector3.one * this._newScaleUnit;
					this._axis.PlotterCurvePoint.localScale = this._newScale * 0.1f;
					this._axis.PositionPointVertical.localScale = this._newScale;
					this._axis.PositionPoint.localScale = this._newScale;
					this._axis.RotationPoint.localScale = this._newScale;
					this._axis.ScalePoint.localScale = Vector3.zero;
				}
			}
			else
			{
				this._axis.PlotterCurvePoint.localScale = Vector3.one * 0.1f;
				this._currentMovement += Time.unscaledDeltaTime;
			}
			if (this._currentMovement > 1f)
			{
				this._lastMovementEndedAt = Time.unscaledTime;
				this._currentMovement = 1f;
			}
			this._axis.TimeLabel.text = this._timeString;
		}

		// Token: 0x040000F3 RID: 243
		public MethodInfo TweenMethod;

		// Token: 0x040000F4 RID: 244
		public int TweenMethodIndex;

		// Token: 0x040000F5 RID: 245
		[Header("Graph")]
		public float GraphSize = 1f;

		// Token: 0x040000F6 RID: 246
		[Range(0f, 1000f)]
		public int Resolution = 100;

		// Token: 0x040000F7 RID: 247
		[Header("Points")]
		public Transform PlotPointPrefab;

		// Token: 0x040000F8 RID: 248
		public float PointScaleFactor = 1f;

		// Token: 0x040000F9 RID: 249
		public Material PlotPointMaterial;

		// Token: 0x040000FA RID: 250
		[MMReadOnly]
		public float DistanceBetweenPoints = 1f;

		// Token: 0x040000FB RID: 251
		[Header("Axis")]
		public MMPlotterAxis Axis;

		// Token: 0x040000FC RID: 252
		protected Transform[] _points;

		// Token: 0x040000FD RID: 253
		protected float _pointScale;

		// Token: 0x040000FE RID: 254
		protected Vector3 _scale;

		// Token: 0x040000FF RID: 255
		protected Vector3 _position;

		// Token: 0x04000100 RID: 256
		protected Transform _point;

		// Token: 0x04000101 RID: 257
		protected Vector3 _horizontalAxisStart;

		// Token: 0x04000102 RID: 258
		protected Vector3 _horizontalAxisEnd;

		// Token: 0x04000103 RID: 259
		protected Vector3 _verticalAxisStart;

		// Token: 0x04000104 RID: 260
		protected Vector3 _verticalAxisEnd;

		// Token: 0x04000105 RID: 261
		protected float _axisWidth;

		// Token: 0x04000106 RID: 262
		protected List<MethodInfo> _methodList;

		// Token: 0x04000107 RID: 263
		protected Vector2 _pointValues = Vector2.zero;

		// Token: 0x04000108 RID: 264
		protected object[] _parameter;

		// Token: 0x04000109 RID: 265
		protected MMPlotterAxis _axis;

		// Token: 0x0400010A RID: 266
		protected Vector3 _positionPointInitialPosition;

		// Token: 0x0400010B RID: 267
		protected Vector3 _positionPointVerticalInitialPosition;

		// Token: 0x0400010C RID: 268
		protected Vector3 _rotationPointInitialRotation;

		// Token: 0x0400010D RID: 269
		protected Vector3 _scalePointInitialScale;

		// Token: 0x0400010E RID: 270
		[Header("Movement")]
		public float MovementPauseDuration = 0.5f;

		// Token: 0x0400010F RID: 271
		protected float _currentMovement;

		// Token: 0x04000110 RID: 272
		protected float _lastMovementEndedAt;

		// Token: 0x04000111 RID: 273
		protected Vector3 _curvePointNewMovement = Vector3.zero;

		// Token: 0x04000112 RID: 274
		protected string _timeString;

		// Token: 0x04000113 RID: 275
		protected const float _plotterCurvePointScale = 0.1f;

		// Token: 0x04000114 RID: 276
		protected Vector3 _newScale;

		// Token: 0x04000115 RID: 277
		protected float _newValue;

		// Token: 0x04000116 RID: 278
		protected float _newScaleUnit;

		// Token: 0x04000117 RID: 279
		protected Vector3 Vector3Zero = Vector3.zero;
	}
}
