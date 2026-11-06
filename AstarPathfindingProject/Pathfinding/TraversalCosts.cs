using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000C7 RID: 199
	[Serializable]
	public struct TraversalCosts
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001EFA3 File Offset: 0x0001D1A3
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x0001EFAB File Offset: 0x0001D1AB
		public uint[] tagEntryCosts
		{
			get
			{
				return this.tagEntryCostsInternal;
			}
			set
			{
				if (value != null && value.Length != 32)
				{
					throw new ArgumentException("Entry costs array must be of length 32");
				}
				this.tagEntryCostsInternal = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001EFC9 File Offset: 0x0001D1C9
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x0001EFD1 File Offset: 0x0001D1D1
		public float[] tagCostMultipliers
		{
			get
			{
				return this.tagCostMultipliersInternal;
			}
			set
			{
				if (value != null && value.Length != 32)
				{
					throw new ArgumentException("Tag cost multiplier array must be of length 32");
				}
				this.tagCostMultipliersInternal = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001EFEF File Offset: 0x0001D1EF
		public bool hasCosts
		{
			get
			{
				return this.tagCostMultipliersInternal != null || this.tagEntryCostsInternal != null || this.traversalProvider != null;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001AA07 File Offset: 0x00018C07
		[Obsolete("Use GetTraversalCostMultiplier instead")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetTraversalCost(GraphNode node)
		{
			return 0U;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001F00C File Offset: 0x0001D20C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetTraversalCostMultiplier(GraphNode node)
		{
			ITraversalProvider traversalProvider = this.traversalProvider;
			if (traversalProvider != null)
			{
				return traversalProvider.GetTraversalCostMultiplier(ref this, node);
			}
			float[] array = this.tagCostMultipliersInternal;
			if (array == null)
			{
				return 1f;
			}
			return array[(int)node.Tag];
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001F037 File Offset: 0x0001D237
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetConnectionCost(GraphNode from, GraphNode to)
		{
			ITraversalProvider traversalProvider = this.traversalProvider;
			if (traversalProvider == null)
			{
				uint[] array = this.tagEntryCostsInternal;
				return ((array != null) ? array[(int)to.Tag] : 0U) + to.Penalty;
			}
			return traversalProvider.GetConnectionCost(ref this, from, to);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001F067 File Offset: 0x0001D267
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetDefaultTraversalCostMultiplier(GraphNode node)
		{
			float[] array = this.tagCostMultipliersInternal;
			if (array == null)
			{
				return 1f;
			}
			return array[(int)node.Tag];
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001F080 File Offset: 0x0001D280
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetDefaultConnectionCost(GraphNode from, GraphNode to)
		{
			return to.Penalty + this.GetTagEntryCost(to.Tag);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001F095 File Offset: 0x0001D295
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetTagEntryCost(uint tag)
		{
			uint[] array = this.tagEntryCostsInternal;
			if (array == null)
			{
				return 0U;
			}
			return array[(int)tag];
		}

		// Token: 0x04000442 RID: 1090
		public ITraversalProvider traversalProvider;

		// Token: 0x04000443 RID: 1091
		[SerializeField]
		private uint[] tagEntryCostsInternal;

		// Token: 0x04000444 RID: 1092
		[SerializeField]
		private float[] tagCostMultipliersInternal;
	}
}
