using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace SplineMesh
{
	// Token: 0x02000182 RID: 386
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class Spline : MonoBehaviour
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00053984 File Offset: 0x00051B84
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x0005398C File Offset: 0x00051B8C
		public bool IsLoop
		{
			get
			{
				return this.isLoop;
			}
			set
			{
				this.isLoop = value;
				this.updateLoopBinding();
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000C0E RID: 3086 RVA: 0x0005399C File Offset: 0x00051B9C
		// (remove) Token: 0x06000C0F RID: 3087 RVA: 0x000539D4 File Offset: 0x00051BD4
		public event ListChangeHandler<SplineNode> NodeListChanged;

		// Token: 0x06000C10 RID: 3088 RVA: 0x00053A0C File Offset: 0x00051C0C
		private void Reset()
		{
			this.nodes.Clear();
			this.curves.Clear();
			this.AddNode(new SplineNode(new Vector3(5f, 0f, 0f), new Vector3(5f, 0f, -3f)));
			this.AddNode(new SplineNode(new Vector3(10f, 0f, 0f), new Vector3(10f, 0f, 3f)));
			this.RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>
			{
				type = ListChangeType.clear
			});
			this.UpdateAfterCurveChanged();
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00053AAD File Offset: 0x00051CAD
		private void OnEnable()
		{
			this.RefreshCurves();
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00053AB5 File Offset: 0x00051CB5
		public ReadOnlyCollection<CubicBezierCurve> GetCurves()
		{
			return this.curves.AsReadOnly();
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00053AC2 File Offset: 0x00051CC2
		private void RaiseNodeListChanged(ListChangedEventArgs<SplineNode> args)
		{
			if (this.NodeListChanged != null)
			{
				this.NodeListChanged(this, args);
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00053ADC File Offset: 0x00051CDC
		private void UpdateAfterCurveChanged()
		{
			this.Length = 0f;
			foreach (CubicBezierCurve cubicBezierCurve in this.curves)
			{
				this.Length += cubicBezierCurve.Length;
			}
			this.CurveChanged.Invoke();
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00053B54 File Offset: 0x00051D54
		public CurveSample GetSample(float t)
		{
			int nodeIndexForTime = this.GetNodeIndexForTime(t);
			return this.curves[nodeIndexForTime].GetSample(t - (float)nodeIndexForTime);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00053B7E File Offset: 0x00051D7E
		public CubicBezierCurve GetCurve(float t)
		{
			return this.curves[this.GetNodeIndexForTime(t)];
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00053B94 File Offset: 0x00051D94
		private int GetNodeIndexForTime(float t)
		{
			if (t < 0f || t > (float)(this.nodes.Count - 1))
			{
				throw new ArgumentException(string.Format("Time must be between 0 and last node index ({0}). Given time was {1}.", this.nodes.Count - 1, t));
			}
			int num = Mathf.FloorToInt(t);
			if (num == this.nodes.Count - 1)
			{
				num--;
			}
			return num;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00053C00 File Offset: 0x00051E00
		public void RefreshCurves()
		{
			this.curves.Clear();
			for (int i = 0; i < this.nodes.Count - 1; i++)
			{
				SplineNode splineNode = this.nodes[i];
				SplineNode splineNode2 = this.nodes[i + 1];
				CubicBezierCurve cubicBezierCurve = new CubicBezierCurve(splineNode, splineNode2);
				cubicBezierCurve.Changed.AddListener(new UnityAction(this.UpdateAfterCurveChanged));
				this.curves.Add(cubicBezierCurve);
			}
			this.RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>
			{
				type = ListChangeType.clear
			});
			this.UpdateAfterCurveChanged();
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00053C90 File Offset: 0x00051E90
		public CurveSample GetSampleAtDistance(float d)
		{
			if (d < 0f || d > this.Length)
			{
				throw new ArgumentException(string.Format("Distance must be between 0 and spline length ({0}). Given distance was {1}.", this.Length, d));
			}
			foreach (CubicBezierCurve cubicBezierCurve in this.curves)
			{
				if (d > cubicBezierCurve.Length && d < cubicBezierCurve.Length + 0.0001f)
				{
					d = cubicBezierCurve.Length;
				}
				if (d <= cubicBezierCurve.Length)
				{
					return cubicBezierCurve.GetSampleAtDistance(d);
				}
				d -= cubicBezierCurve.Length;
			}
			throw new Exception("Something went wrong with GetSampleAtDistance.");
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00053D5C File Offset: 0x00051F5C
		public void AddNode(SplineNode node)
		{
			this.nodes.Add(node);
			if (this.nodes.Count != 1)
			{
				CubicBezierCurve cubicBezierCurve = new CubicBezierCurve(this.nodes[this.nodes.IndexOf(node) - 1], node);
				cubicBezierCurve.Changed.AddListener(new UnityAction(this.UpdateAfterCurveChanged));
				this.curves.Add(cubicBezierCurve);
			}
			this.RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>
			{
				type = ListChangeType.Add,
				newItems = new List<SplineNode> { node }
			});
			this.UpdateAfterCurveChanged();
			this.updateLoopBinding();
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00053DF8 File Offset: 0x00051FF8
		public void InsertNode(int index, SplineNode node)
		{
			if (index == 0)
			{
				throw new Exception("Can't insert a node at index 0");
			}
			SplineNode splineNode = this.nodes[index - 1];
			SplineNode splineNode2 = this.nodes[index];
			this.nodes.Insert(index, node);
			this.curves[index - 1].ConnectEnd(node);
			CubicBezierCurve cubicBezierCurve = new CubicBezierCurve(node, splineNode2);
			cubicBezierCurve.Changed.AddListener(new UnityAction(this.UpdateAfterCurveChanged));
			this.curves.Insert(index, cubicBezierCurve);
			this.RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>
			{
				type = ListChangeType.Insert,
				newItems = new List<SplineNode> { node },
				insertIndex = index
			});
			this.UpdateAfterCurveChanged();
			this.updateLoopBinding();
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00053EB4 File Offset: 0x000520B4
		public void RemoveNode(SplineNode node)
		{
			int num = this.nodes.IndexOf(node);
			if (this.nodes.Count <= 2)
			{
				throw new Exception("Can't remove the node because a spline needs at least 2 nodes.");
			}
			CubicBezierCurve cubicBezierCurve = ((num == this.nodes.Count - 1) ? this.curves[num - 1] : this.curves[num]);
			if (num != 0 && num != this.nodes.Count - 1)
			{
				SplineNode splineNode = this.nodes[num + 1];
				this.curves[num - 1].ConnectEnd(splineNode);
			}
			this.nodes.RemoveAt(num);
			cubicBezierCurve.Changed.RemoveListener(new UnityAction(this.UpdateAfterCurveChanged));
			this.curves.Remove(cubicBezierCurve);
			this.RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>
			{
				type = ListChangeType.Remove,
				removedItems = new List<SplineNode> { node },
				removeIndex = num
			});
			this.UpdateAfterCurveChanged();
			this.updateLoopBinding();
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00053FB4 File Offset: 0x000521B4
		private void updateLoopBinding()
		{
			if (this.start != null)
			{
				this.start.Changed -= this.StartNodeChanged;
			}
			if (this.end != null)
			{
				this.end.Changed -= this.EndNodeChanged;
			}
			if (this.isLoop)
			{
				this.start = this.nodes[0];
				this.end = this.nodes[this.nodes.Count - 1];
				this.start.Changed += this.StartNodeChanged;
				this.end.Changed += this.EndNodeChanged;
				this.StartNodeChanged(null, null);
				return;
			}
			this.start = null;
			this.end = null;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0005407C File Offset: 0x0005227C
		private void StartNodeChanged(object sender, EventArgs e)
		{
			this.end.Changed -= this.EndNodeChanged;
			this.end.Position = this.start.Position;
			this.end.Direction = this.start.Direction;
			this.end.Roll = this.start.Roll;
			this.end.Scale = this.start.Scale;
			this.end.Up = this.start.Up;
			this.end.Changed += this.EndNodeChanged;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00054128 File Offset: 0x00052328
		private void EndNodeChanged(object sender, EventArgs e)
		{
			this.start.Changed -= this.StartNodeChanged;
			this.start.Position = this.end.Position;
			this.start.Direction = this.end.Direction;
			this.start.Roll = this.end.Roll;
			this.start.Scale = this.end.Scale;
			this.start.Up = this.end.Up;
			this.start.Changed += this.StartNodeChanged;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x000541D4 File Offset: 0x000523D4
		public CurveSample GetProjectionSample(Vector3 pointToProject)
		{
			CurveSample curveSample = default(CurveSample);
			float num = float.MaxValue;
			foreach (CubicBezierCurve cubicBezierCurve in this.curves)
			{
				CurveSample projectionSample = cubicBezierCurve.GetProjectionSample(pointToProject);
				if (cubicBezierCurve == this.curves[0])
				{
					curveSample = projectionSample;
					num = (projectionSample.location - pointToProject).sqrMagnitude;
				}
				else
				{
					float sqrMagnitude = (projectionSample.location - pointToProject).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						curveSample = projectionSample;
					}
				}
			}
			return curveSample;
		}

		// Token: 0x04000C0F RID: 3087
		public List<SplineNode> nodes = new List<SplineNode>();

		// Token: 0x04000C10 RID: 3088
		[HideInInspector]
		public List<CubicBezierCurve> curves = new List<CubicBezierCurve>();

		// Token: 0x04000C11 RID: 3089
		public float Length;

		// Token: 0x04000C12 RID: 3090
		[SerializeField]
		private bool isLoop;

		// Token: 0x04000C14 RID: 3092
		[HideInInspector]
		public UnityEvent CurveChanged = new UnityEvent();

		// Token: 0x04000C15 RID: 3093
		private SplineNode start;

		// Token: 0x04000C16 RID: 3094
		private SplineNode end;
	}
}
