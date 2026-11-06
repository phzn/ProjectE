using System;
using System.Runtime.CompilerServices;

namespace Pathfinding
{
	// Token: 0x0200009A RID: 154
	public struct Connection
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00019C2F File Offset: 0x00017E2F
		public int shapeEdge
		{
			get
			{
				return (int)(this.shapeEdgeInfo & 3);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00019C39 File Offset: 0x00017E39
		public int adjacentShapeEdge
		{
			get
			{
				return (this.shapeEdgeInfo >> 2) & 3;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00019C45 File Offset: 0x00017E45
		public bool edgesAreIdentical
		{
			get
			{
				return (this.shapeEdgeInfo & 64) > 0;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00019C53 File Offset: 0x00017E53
		public bool isEdgeShared
		{
			get
			{
				return (this.shapeEdgeInfo & 15) != 15;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00019C65 File Offset: 0x00017E65
		public bool isOutgoing
		{
			get
			{
				return (this.shapeEdgeInfo & 32) > 0;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00019C73 File Offset: 0x00017E73
		public bool isIncoming
		{
			get
			{
				return (this.shapeEdgeInfo & 16) > 0;
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00019C81 File Offset: 0x00017E81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Connection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			this.node = node;
			this.cost = cost;
			this.shapeEdgeInfo = Connection.PackShapeEdgeInfo(isOutgoing, isIncoming);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00019C9F File Offset: 0x00017E9F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte PackShapeEdgeInfo(bool isOutgoing, bool isIncoming)
		{
			return (byte)(15 | (isIncoming ? 16 : 0) | (isOutgoing ? 32 : 0));
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00019CB6 File Offset: 0x00017EB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte PackShapeEdgeInfo(byte shapeEdge, byte adjacentShapeEdge, bool areEdgesIdentical, bool isOutgoing, bool isIncoming)
		{
			return (byte)((int)shapeEdge | ((int)adjacentShapeEdge << 2) | (areEdgesIdentical ? 64 : 0) | (isOutgoing ? 32 : 0) | (isIncoming ? 16 : 0));
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019CDA File Offset: 0x00017EDA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Connection(GraphNode node, uint cost, byte shapeEdgeInfo)
		{
			this.node = node;
			this.cost = cost;
			this.shapeEdgeInfo = shapeEdgeInfo;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00019CF1 File Offset: 0x00017EF1
		public override int GetHashCode()
		{
			return this.node.GetHashCode() ^ (int)this.cost;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00019D08 File Offset: 0x00017F08
		public override bool Equals(object obj)
		{
			if (!(obj is Connection))
			{
				return false;
			}
			Connection connection = (Connection)obj;
			return connection.node == this.node && connection.cost == this.cost && connection.shapeEdgeInfo == this.shapeEdgeInfo;
		}

		// Token: 0x04000349 RID: 841
		public GraphNode node;

		// Token: 0x0400034A RID: 842
		public uint cost;

		// Token: 0x0400034B RID: 843
		public byte shapeEdgeInfo;

		// Token: 0x0400034C RID: 844
		public const byte NoSharedEdge = 15;

		// Token: 0x0400034D RID: 845
		public const byte IncomingConnection = 16;

		// Token: 0x0400034E RID: 846
		public const byte OutgoingConnection = 32;

		// Token: 0x0400034F RID: 847
		public const byte IdenticalEdge = 64;
	}
}
