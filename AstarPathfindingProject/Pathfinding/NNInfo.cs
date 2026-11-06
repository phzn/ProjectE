using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200002A RID: 42
	public readonly struct NNInfo
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000B123 File Offset: 0x00009323
		[Obsolete("This field has been renamed to 'position'", true)]
		public Vector3 clampedPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000B12B File Offset: 0x0000932B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NNInfo(GraphNode node, Vector3 position, float distanceCostSqr)
		{
			this.node = node;
			if (node == null)
			{
				this.position = Vector3.positiveInfinity;
				this.distanceCostSqr = float.PositiveInfinity;
				return;
			}
			this.position = position;
			this.distanceCostSqr = distanceCostSqr;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000B123 File Offset: 0x00009323
		public static explicit operator Vector3(NNInfo ob)
		{
			return ob.position;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000B15C File Offset: 0x0000935C
		public static explicit operator GraphNode(NNInfo ob)
		{
			return ob.node;
		}

		// Token: 0x04000153 RID: 339
		public readonly GraphNode node;

		// Token: 0x04000154 RID: 340
		public readonly Vector3 position;

		// Token: 0x04000155 RID: 341
		public readonly float distanceCostSqr;

		// Token: 0x04000156 RID: 342
		public static readonly NNInfo Empty = new NNInfo(null, Vector3.positiveInfinity, float.PositiveInfinity);
	}
}
