using System;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x02000187 RID: 391
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Spline))]
	public class SplineSmoother : MonoBehaviour
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00054531 File Offset: 0x00052731
		private Spline Spline
		{
			get
			{
				if (this.spline == null)
				{
					this.spline = base.GetComponent<Spline>();
				}
				return this.spline;
			}
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00054553 File Offset: 0x00052753
		private void OnValidate()
		{
			this.SmoothAll();
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0005455C File Offset: 0x0005275C
		private void OnEnable()
		{
			this.Spline.NodeListChanged += this.Spline_NodeListChanged;
			foreach (SplineNode splineNode in this.Spline.nodes)
			{
				splineNode.Changed += this.OnNodeChanged;
			}
			this.SmoothAll();
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000545DC File Offset: 0x000527DC
		private void OnDisable()
		{
			this.Spline.NodeListChanged -= this.Spline_NodeListChanged;
			foreach (SplineNode splineNode in this.Spline.nodes)
			{
				splineNode.Changed -= this.OnNodeChanged;
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00054654 File Offset: 0x00052854
		private void Spline_NodeListChanged(object sender, ListChangedEventArgs<SplineNode> args)
		{
			if (args.newItems != null)
			{
				foreach (SplineNode splineNode in args.newItems)
				{
					splineNode.Changed += this.OnNodeChanged;
				}
			}
			if (args.removedItems != null)
			{
				foreach (SplineNode splineNode2 in args.removedItems)
				{
					splineNode2.Changed -= this.OnNodeChanged;
				}
			}
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0005470C File Offset: 0x0005290C
		private void OnNodeChanged(object sender, EventArgs e)
		{
			SplineNode splineNode = (SplineNode)sender;
			this.SmoothNode(splineNode);
			int num = this.Spline.nodes.IndexOf(splineNode);
			if (num > 0)
			{
				this.SmoothNode(this.Spline.nodes[num - 1]);
			}
			if (num < this.Spline.nodes.Count - 1)
			{
				this.SmoothNode(this.Spline.nodes[num + 1]);
			}
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00054784 File Offset: 0x00052984
		private void SmoothNode(SplineNode node)
		{
			int num = this.Spline.nodes.IndexOf(node);
			Vector3 position = node.Position;
			Vector3 vector = Vector3.zero;
			float num2 = 0f;
			if (num != 0)
			{
				Vector3 position2 = this.Spline.nodes[num - 1].Position;
				Vector3 vector2 = position - position2;
				num2 += vector2.magnitude;
				vector += vector2.normalized;
			}
			if (num != this.Spline.nodes.Count - 1)
			{
				Vector3 position3 = this.Spline.nodes[num + 1].Position;
				Vector3 vector3 = position - position3;
				num2 += vector3.magnitude;
				vector -= vector3.normalized;
			}
			num2 *= 0.5f;
			vector = vector.normalized * num2 * this.curvature;
			Vector3 vector4 = vector + position;
			node.Direction = vector4;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00054878 File Offset: 0x00052A78
		private void SmoothAll()
		{
			foreach (SplineNode splineNode in this.Spline.nodes)
			{
				this.SmoothNode(splineNode);
			}
		}

		// Token: 0x04000C27 RID: 3111
		private Spline spline;

		// Token: 0x04000C28 RID: 3112
		[Range(0f, 1f)]
		public float curvature = 0.3f;
	}
}
