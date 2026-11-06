using System;
using Pathfinding.Drawing;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001D7 RID: 471
	[AddComponentMenu("Pathfinding/Navmesh/RelevantGraphSurface")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/relevantgraphsurface.html")]
	public class RelevantGraphSurface : VersionedMonoBehaviour
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x0004C5AF File Offset: 0x0004A7AF
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x0004C5B7 File Offset: 0x0004A7B7
		public RelevantGraphSurface Next
		{
			get
			{
				return this.next;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0004C5BF File Offset: 0x0004A7BF
		public RelevantGraphSurface Prev
		{
			get
			{
				return this.prev;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0004C5C7 File Offset: 0x0004A7C7
		public static RelevantGraphSurface Root
		{
			get
			{
				return RelevantGraphSurface.root;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0004C5CE File Offset: 0x0004A7CE
		public void UpdatePosition()
		{
			this.position = base.transform.position;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0004C5E1 File Offset: 0x0004A7E1
		private void OnEnable()
		{
			this.UpdatePosition();
			if (RelevantGraphSurface.root == null)
			{
				RelevantGraphSurface.root = this;
				return;
			}
			this.next = RelevantGraphSurface.root;
			RelevantGraphSurface.root.prev = this;
			RelevantGraphSurface.root = this;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0004C61C File Offset: 0x0004A81C
		private void OnDisable()
		{
			if (RelevantGraphSurface.root == this)
			{
				RelevantGraphSurface.root = this.next;
				if (RelevantGraphSurface.root != null)
				{
					RelevantGraphSurface.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0004C6A8 File Offset: 0x0004A8A8
		public static void UpdateAllPositions()
		{
			RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.root;
			while (relevantGraphSurface != null)
			{
				relevantGraphSurface.UpdatePosition();
				relevantGraphSurface = relevantGraphSurface.Next;
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0004C6D4 File Offset: 0x0004A8D4
		public static void FindAllGraphSurfaces()
		{
			RelevantGraphSurface[] array = UnityCompatibility.FindObjectsByTypeUnsorted<RelevantGraphSurface>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnDisable();
				array[i].OnEnable();
			}
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0004C708 File Offset: 0x0004A908
		public override void DrawGizmos()
		{
			Color color = new Color(0.22352941f, 0.827451f, 0.18039216f);
			if (!GizmoContext.InActiveSelection(this))
			{
				color.a *= 0.4f;
			}
			Draw.Line(base.transform.position - Vector3.up * this.maxRange, base.transform.position + Vector3.up * this.maxRange, color);
		}

		// Token: 0x040008E3 RID: 2275
		private static RelevantGraphSurface root;

		// Token: 0x040008E4 RID: 2276
		public float maxRange = 1f;

		// Token: 0x040008E5 RID: 2277
		private RelevantGraphSurface prev;

		// Token: 0x040008E6 RID: 2278
		private RelevantGraphSurface next;

		// Token: 0x040008E7 RID: 2279
		private Vector3 position;
	}
}
