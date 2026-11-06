using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Serialization;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200009B RID: 155
	public abstract class GraphNode
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00019D52 File Offset: 0x00017F52
		public NavGraph Graph
		{
			get
			{
				return AstarData.GetGraph(this);
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00019D5A File Offset: 0x00017F5A
		public void Destroy()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ClearConnections(true);
			if (AstarPath.active != null)
			{
				AstarPath.active.DestroyNode(this);
			}
			this.NodeIndex = 268435454U;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00019D8F File Offset: 0x00017F8F
		public bool Destroyed
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.NodeIndex == 268435454U;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00019D9E File Offset: 0x00017F9E
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00019DAC File Offset: 0x00017FAC
		public uint NodeIndex
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (uint)(this.nodeIndex & 268435455);
			}
			[IgnoredByDeepProfiler]
			internal set
			{
				this.nodeIndex = (this.nodeIndex & -268435456) | (int)value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00019DC2 File Offset: 0x00017FC2
		internal virtual int PathNodeVariants
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00019DC5 File Offset: 0x00017FC5
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x00019DD6 File Offset: 0x00017FD6
		internal bool TemporaryFlag1
		{
			get
			{
				return (this.nodeIndex & 268435456) != 0;
			}
			set
			{
				this.nodeIndex = (this.nodeIndex & -268435457) | (value ? 268435456 : 0);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00019DF6 File Offset: 0x00017FF6
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x00019E07 File Offset: 0x00018007
		internal bool TemporaryFlag2
		{
			get
			{
				return (this.nodeIndex & 536870912) != 0;
			}
			set
			{
				this.nodeIndex = (this.nodeIndex & -536870913) | (value ? 536870912 : 0);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00019E27 File Offset: 0x00018027
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x00019E2F File Offset: 0x0001802F
		public uint Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00019E38 File Offset: 0x00018038
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x00019E40 File Offset: 0x00018040
		public uint Penalty
		{
			get
			{
				return this.penalty;
			}
			set
			{
				if (value > 16777215U)
				{
					Debug.LogWarning("Very high penalty applied. Are you sure negative values haven't underflowed?\nPenalty values this high could with long paths cause overflows and in some cases infinite loops because of that.\nPenalty value applied: " + value.ToString());
				}
				this.penalty = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00019E67 File Offset: 0x00018067
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00019E74 File Offset: 0x00018074
		public bool Walkable
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 1U) > 0U;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (this.flags & 4294967294U) | (value ? 1U : 0U);
				AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00019E9D File Offset: 0x0001809D
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00019EAD File Offset: 0x000180AD
		internal int HierarchicalNodeIndex
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (int)((this.flags & 262142U) >> 1);
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.flags = (this.flags & 4294705153U) | (uint)((uint)value << 1);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00019EC5 File Offset: 0x000180C5
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x00019ED6 File Offset: 0x000180D6
		internal bool IsHierarchicalNodeDirty
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.flags & 262144U) > 0U;
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.flags = (this.flags & 4294705151U) | ((value ? 1U : 0U) << 18);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00019EF5 File Offset: 0x000180F5
		public uint Area
		{
			get
			{
				return AstarPath.active.hierarchicalGraph.GetConnectedComponent(this.HierarchicalNodeIndex);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00019F0C File Offset: 0x0001810C
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x00019F1D File Offset: 0x0001811D
		public uint GraphIndex
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.flags & 4278190080U) >> 24;
			}
			set
			{
				this.flags = (this.flags & 16777215U) | (value << 24);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00019F36 File Offset: 0x00018136
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00019F47 File Offset: 0x00018147
		public uint Tag
		{
			get
			{
				return (this.flags & 16252928U) >> 19;
			}
			set
			{
				this.flags = (this.flags & 4278714367U) | ((value << 19) & 16252928U);
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00019F66 File Offset: 0x00018166
		public void SetConnectivityDirty()
		{
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00019F78 File Offset: 0x00018178
		public virtual void GetConnections(Action<GraphNode> action, int connectionFilter = 32)
		{
			this.GetConnections<Action<GraphNode>>(delegate(GraphNode node, ref Action<GraphNode> action)
			{
				action(node);
			}, ref action, connectionFilter);
		}

		// Token: 0x0600053C RID: 1340
		public abstract void GetConnections<T>(GraphNode.NodeActionWithData<T> action, ref T data, int connectionFilter = 32);

		// Token: 0x0600053D RID: 1341 RVA: 0x00019FA2 File Offset: 0x000181A2
		public static void Connect(GraphNode lhs, GraphNode rhs, uint cost, OffMeshLinks.Directionality directionality = OffMeshLinks.Directionality.TwoWay)
		{
			if (lhs.Destroyed || rhs.Destroyed)
			{
				throw new ArgumentException("Cannot connect destroyed nodes");
			}
			lhs.AddPartialConnection(rhs, cost, true, directionality == OffMeshLinks.Directionality.TwoWay);
			rhs.AddPartialConnection(lhs, cost, directionality == OffMeshLinks.Directionality.TwoWay, true);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00019FD9 File Offset: 0x000181D9
		public static void Disconnect(GraphNode lhs, GraphNode rhs)
		{
			lhs.RemovePartialConnection(rhs);
			rhs.RemovePartialConnection(lhs);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00019FE9 File Offset: 0x000181E9
		[Obsolete("Use the static Connect method instead, or AddPartialConnection if you really need to")]
		public void AddConnection(GraphNode node, uint cost)
		{
			this.AddPartialConnection(node, cost, true, true);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00019FF5 File Offset: 0x000181F5
		[Obsolete("Use the static Disconnect method instead, or RemovePartialConnection if you really need to")]
		public void RemoveConnection(GraphNode node)
		{
			this.RemovePartialConnection(node);
		}

		// Token: 0x06000541 RID: 1345
		public abstract void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming);

		// Token: 0x06000542 RID: 1346
		public abstract void RemovePartialConnection(GraphNode node);

		// Token: 0x06000543 RID: 1347
		public abstract void ClearConnections(bool alsoReverse = true);

		// Token: 0x06000544 RID: 1348 RVA: 0x00019FFE File Offset: 0x000181FE
		[Obsolete("Use ContainsOutgoingConnection instead")]
		public bool ContainsConnection(GraphNode node)
		{
			return this.ContainsOutgoingConnection(node);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001A008 File Offset: 0x00018208
		public virtual bool ContainsOutgoingConnection(GraphNode node)
		{
			bool flag = false;
			this.GetConnections<bool>(delegate(GraphNode neighbour, ref bool contains)
			{
				contains |= neighbour == node;
			}, ref flag, 32);
			return flag;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001A03C File Offset: 0x0001823C
		[Obsolete("Use GetPortal(GraphNode, out Vector3, out Vector3) instead")]
		public bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			Vector3 vector;
			Vector3 vector2;
			if (!backwards && this.GetPortal(other, out vector, out vector2))
			{
				if (left != null)
				{
					left.Add(vector);
					right.Add(vector2);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001A06E File Offset: 0x0001826E
		public virtual bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			left = Vector3.zero;
			right = Vector3.zero;
			return false;
		}

		// Token: 0x06000548 RID: 1352
		public abstract void Open(ref Path.SearchContext ctx, uint pathNodeIndex, uint gScore);

		// Token: 0x06000549 RID: 1353
		public abstract void OpenAtPoint(ref Path.SearchContext ctx, uint pathNodeIndex, Int3 position, uint gScore);

		// Token: 0x0600054A RID: 1354 RVA: 0x0001A087 File Offset: 0x00018287
		public virtual Int3 DecodeVariantPosition(uint pathNodeIndex, uint fractionAlongEdge)
		{
			return this.position;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00005C0D File Offset: 0x00003E0D
		public virtual float SurfaceArea()
		{
			return 0f;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001A08F File Offset: 0x0001828F
		public virtual Vector3 RandomPointOnSurface()
		{
			return (Vector3)this.position;
		}

		// Token: 0x0600054D RID: 1357
		public abstract Vector3 ClosestPointOnNode(Vector3 p);

		// Token: 0x0600054E RID: 1358 RVA: 0x0001A09C File Offset: 0x0001829C
		public virtual bool ContainsPoint(Int3 point)
		{
			return this.ContainsPoint((Vector3)point);
		}

		// Token: 0x0600054F RID: 1359
		public abstract bool ContainsPoint(Vector3 point);

		// Token: 0x06000550 RID: 1360
		public abstract bool ContainsPointInGraphSpace(Int3 point);

		// Token: 0x06000551 RID: 1361 RVA: 0x0001A0AA File Offset: 0x000182AA
		public virtual int GetGizmoHashCode()
		{
			return this.position.GetHashCode() ^ (int)(19U * this.Penalty) ^ (int)(41U * (this.flags & 4294443009U));
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001A0D7 File Offset: 0x000182D7
		public virtual void SerializeNode(GraphSerializationContext ctx)
		{
			ctx.writer.Write(this.Penalty);
			ctx.writer.Write(this.Flags & 4294443009U);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001A104 File Offset: 0x00018304
		public virtual void DeserializeNode(GraphSerializationContext ctx)
		{
			this.Penalty = ctx.reader.ReadUInt32();
			this.Flags = (ctx.reader.ReadUInt32() & 4294443009U) | (this.Flags & 524286U);
			this.GraphIndex = ctx.graphIndex;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void SerializeReferences(GraphSerializationContext ctx)
		{
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void DeserializeReferences(GraphSerializationContext ctx)
		{
		}

		// Token: 0x04000350 RID: 848
		private int nodeIndex;

		// Token: 0x04000351 RID: 849
		protected uint flags;

		// Token: 0x04000352 RID: 850
		private uint penalty;

		// Token: 0x04000353 RID: 851
		private const uint NodeIndexMask = 268435455U;

		// Token: 0x04000354 RID: 852
		public const uint DestroyedNodeIndex = 268435454U;

		// Token: 0x04000355 RID: 853
		public const int InvalidNodeIndex = 0;

		// Token: 0x04000356 RID: 854
		private const int TemporaryFlag1Mask = 268435456;

		// Token: 0x04000357 RID: 855
		private const int TemporaryFlag2Mask = 536870912;

		// Token: 0x04000358 RID: 856
		public Int3 position;

		// Token: 0x04000359 RID: 857
		private const int FlagsWalkableOffset = 0;

		// Token: 0x0400035A RID: 858
		private const uint FlagsWalkableMask = 1U;

		// Token: 0x0400035B RID: 859
		private const int FlagsHierarchicalIndexOffset = 1;

		// Token: 0x0400035C RID: 860
		private const uint HierarchicalIndexMask = 262142U;

		// Token: 0x0400035D RID: 861
		private const int HierarchicalDirtyOffset = 18;

		// Token: 0x0400035E RID: 862
		private const uint HierarchicalDirtyMask = 262144U;

		// Token: 0x0400035F RID: 863
		private const int FlagsGraphOffset = 24;

		// Token: 0x04000360 RID: 864
		private const uint FlagsGraphMask = 4278190080U;

		// Token: 0x04000361 RID: 865
		public const uint MaxHierarchicalNodeIndex = 131071U;

		// Token: 0x04000362 RID: 866
		public const uint MaxGraphIndex = 254U;

		// Token: 0x04000363 RID: 867
		public const uint InvalidGraphIndex = 255U;

		// Token: 0x04000364 RID: 868
		private const int FlagsTagOffset = 19;

		// Token: 0x04000365 RID: 869
		public const int MaxTagIndex = 31;

		// Token: 0x04000366 RID: 870
		private const uint FlagsTagMask = 16252928U;

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x06000558 RID: 1368
		public delegate void NodeActionWithData<T>(GraphNode node, ref T data);
	}
}
