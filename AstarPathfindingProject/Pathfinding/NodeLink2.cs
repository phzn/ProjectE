using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000086 RID: 134
	[AddComponentMenu("Pathfinding/Link2")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/nodelink2.html")]
	public class NodeLink2 : GraphModifier
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00016E81 File Offset: 0x00015081
		public Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00017055 File Offset: 0x00015255
		public Transform EndTransform
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00017060 File Offset: 0x00015260
		public static NodeLink2 GetNodeLink(GraphNode node)
		{
			LinkNode linkNode = node as LinkNode;
			if (linkNode == null)
			{
				return null;
			}
			return linkNode.linkSource.component as NodeLink2;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00017089 File Offset: 0x00015289
		internal bool isActive
		{
			get
			{
				return this.linkSource != null && (this.linkSource.status & OffMeshLinks.OffMeshLinkStatus.Active) > (OffMeshLinks.OffMeshLinkStatus)0;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x000170A5 File Offset: 0x000152A5
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x000170AD File Offset: 0x000152AD
		public IOffMeshLinkHandler onTraverseOffMeshLink
		{
			get
			{
				return this.onTraverseOffMeshLinkHandler;
			}
			set
			{
				this.onTraverseOffMeshLinkHandler = value;
				if (this.linkSource != null)
				{
					this.linkSource.handler = value;
				}
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000170CA File Offset: 0x000152CA
		public override void OnPostScan()
		{
			this.TryAddLink();
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000170D2 File Offset: 0x000152D2
		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && !BatchedEvents.Has<NodeLink2>(this))
			{
				BatchedEvents.Add<NodeLink2>(this, BatchedEvents.Event.Update, new Action<NodeLink2[], int>(NodeLink2.OnUpdate), 0);
			}
			this.TryAddLink();
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00017104 File Offset: 0x00015304
		private static void OnUpdate(NodeLink2[] components, int count)
		{
			if (Time.frameCount % 16 != 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				NodeLink2 nodeLink = components[i];
				Transform startTransform = nodeLink.StartTransform;
				Transform endTransform = nodeLink.EndTransform;
				bool flag = nodeLink.linkSource != null;
				if ((startTransform != null && endTransform != null) != flag || (flag && (startTransform.hasChanged || endTransform.hasChanged)))
				{
					if (startTransform != null)
					{
						startTransform.hasChanged = false;
					}
					if (endTransform != null)
					{
						endTransform.hasChanged = false;
					}
					nodeLink.RemoveLink();
					nodeLink.TryAddLink();
				}
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001719C File Offset: 0x0001539C
		private void TryAddLink()
		{
			if (this.linkSource != null && (this.linkSource.status == OffMeshLinks.OffMeshLinkStatus.Inactive || (this.linkSource.status & OffMeshLinks.OffMeshLinkStatus.PendingRemoval) != (OffMeshLinks.OffMeshLinkStatus)0))
			{
				this.linkSource = null;
			}
			if (this.linkSource == null && AstarPath.active != null && this.EndTransform != null)
			{
				this.StartTransform.hasChanged = false;
				this.EndTransform.hasChanged = false;
				this.linkSource = new OffMeshLinks.OffMeshLinkSource
				{
					start = new OffMeshLinks.Anchor
					{
						center = this.StartTransform.position,
						rotation = this.StartTransform.rotation,
						width = 0f
					},
					end = new OffMeshLinks.Anchor
					{
						center = this.EndTransform.position,
						rotation = this.EndTransform.rotation,
						width = 0f
					},
					directionality = (this.oneWay ? OffMeshLinks.Directionality.OneWay : OffMeshLinks.Directionality.TwoWay),
					tag = this.pathfindingTag,
					costFactor = this.costFactor,
					graphMask = this.graphMask,
					maxSnappingDistance = 1f,
					component = this,
					handler = this.onTraverseOffMeshLink
				};
				AstarPath.active.offMeshLinks.Add(this.linkSource);
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001730C File Offset: 0x0001550C
		private void RemoveLink()
		{
			if (AstarPath.active != null && this.linkSource != null)
			{
				AstarPath.active.offMeshLinks.Remove(this.linkSource);
			}
			this.linkSource = null;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001733F File Offset: 0x0001553F
		protected override void OnDisable()
		{
			base.OnDisable();
			BatchedEvents.Remove<NodeLink2>(this);
			this.RemoveLink();
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00017353 File Offset: 0x00015553
		[ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			this.Apply();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001735B File Offset: 0x0001555B
		public virtual void Apply()
		{
			this.RemoveLink();
			this.TryAddLink();
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001736C File Offset: 0x0001556C
		public override void DrawGizmos()
		{
			if (this.StartTransform == null || this.EndTransform == null)
			{
				return;
			}
			Vector3 position = this.StartTransform.position;
			Vector3 position2 = this.EndTransform.position;
			if (this.linkSource != null && Time.renderedFrameCount % 16 == 0 && Application.isEditor && (this.linkSource.start.center != position || this.linkSource.end.center != position2 || this.linkSource.directionality != (this.oneWay ? OffMeshLinks.Directionality.OneWay : OffMeshLinks.Directionality.TwoWay) || this.linkSource.costFactor != this.costFactor || this.linkSource.graphMask != this.graphMask || this.linkSource.tag != this.pathfindingTag))
			{
				this.Apply();
			}
			bool flag = GizmoContext.InActiveSelection(this);
			List<NavGraph> list = ((this.linkSource != null && AstarPath.active != null) ? AstarPath.active.offMeshLinks.ConnectedGraphs(this.linkSource) : null);
			Vector3 vector = Vector3.up;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					NavGraph navGraph = list[i];
					if (navGraph != null)
					{
						NavmeshBase navmeshBase = navGraph as NavmeshBase;
						if (navmeshBase != null)
						{
							vector = navmeshBase.transform.WorldUpAtGraphPosition(Vector3.zero);
							break;
						}
						GridGraph gridGraph = navGraph as GridGraph;
						if (gridGraph != null)
						{
							vector = gridGraph.transform.WorldUpAtGraphPosition(Vector3.zero);
							break;
						}
					}
				}
				ListPool<NavGraph>.Release(ref list);
			}
			bool flag2 = this.linkSource != null && this.linkSource.status == OffMeshLinks.OffMeshLinkStatus.Active;
			Color color = (flag ? NodeLink2.GizmosColorSelected : NodeLink2.GizmosColor);
			if (flag2)
			{
				color = Color.green;
			}
			Draw.Circle(position, vector, 0.4f, (this.linkSource != null && this.linkSource.status.HasFlag(OffMeshLinks.OffMeshLinkStatus.FailedToConnectStart)) ? Color.red : color);
			Draw.Circle(position2, vector, 0.4f, (this.linkSource != null && this.linkSource.status.HasFlag(OffMeshLinks.OffMeshLinkStatus.FailedToConnectEnd)) ? Color.red : color);
			NodeLink.DrawArch(position, position2, vector, color);
			if (flag)
			{
				Vector3 normalized = Vector3.Cross(vector, position2 - position).normalized;
				using (Draw.WithLineWidth(2f, true))
				{
					NodeLink.DrawArch(position + normalized * 0f, position2 + normalized * 0f, vector, color);
				}
			}
		}

		// Token: 0x040002DF RID: 735
		public Transform end;

		// Token: 0x040002E0 RID: 736
		public float costFactor = 1f;

		// Token: 0x040002E1 RID: 737
		public bool oneWay;

		// Token: 0x040002E2 RID: 738
		public PathfindingTag pathfindingTag = 0U;

		// Token: 0x040002E3 RID: 739
		public GraphMask graphMask = GraphMask.everything;

		// Token: 0x040002E4 RID: 740
		protected OffMeshLinks.OffMeshLinkSource linkSource;

		// Token: 0x040002E5 RID: 741
		private IOffMeshLinkHandler onTraverseOffMeshLinkHandler;

		// Token: 0x040002E6 RID: 742
		private static readonly Color GizmosColor = new Color(0.80784315f, 0.53333336f, 0.1882353f, 0.5f);

		// Token: 0x040002E7 RID: 743
		private static readonly Color GizmosColorSelected = new Color(0.92156863f, 0.48235294f, 0.1254902f, 1f);
	}
}
