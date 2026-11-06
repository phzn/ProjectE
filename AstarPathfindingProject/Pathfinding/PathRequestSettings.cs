using System;
using Pathfinding.Util;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000093 RID: 147
	[Serializable]
	public struct PathRequestSettings : IEquatable<PathRequestSettings>
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000191D7 File Offset: 0x000173D7
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x000191DF File Offset: 0x000173DF
		[Obsolete("Use tagEntryCosts or tagCostMultipliers instead")]
		public uint[] tagPenalties
		{
			get
			{
				return this.tagEntryCosts;
			}
			set
			{
				this.tagEntryCosts = value;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x000191E8 File Offset: 0x000173E8
		public static PathRequestSettings Default
		{
			get
			{
				PathRequestSettings pathRequestSettings = new PathRequestSettings
				{
					graphMask = GraphMask.everything,
					tagCostMultipliers = new float[32],
					tagEntryCosts = new uint[32],
					traversableTags = -1,
					traversalProvider = null
				};
				for (int i = 0; i < pathRequestSettings.tagCostMultipliers.Length; i++)
				{
					pathRequestSettings.tagCostMultipliers[i] = 1f;
				}
				return pathRequestSettings;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00019258 File Offset: 0x00017458
		public NearestNodeConstraint ToNearestNodeConstraint()
		{
			return this.ToTraversalConstraint().ToNearestNodeConstraint();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00019274 File Offset: 0x00017474
		public TraversalConstraint ToTraversalConstraint()
		{
			return new TraversalConstraint
			{
				tags = this.traversableTags,
				traversalProvider = this.traversalProvider,
				graphMask = this.graphMask
			};
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000192B4 File Offset: 0x000174B4
		public TraversalCosts ToTraversalCosts()
		{
			return new TraversalCosts
			{
				tagEntryCosts = this.tagEntryCosts,
				tagCostMultipliers = this.tagCostMultipliers,
				traversalProvider = this.traversalProvider
			};
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000192F4 File Offset: 0x000174F4
		public bool Equals(PathRequestSettings other)
		{
			return this.graphMask == other.graphMask && Memory.SequenceEqual<float>(this.tagCostMultipliers, other.tagCostMultipliers) && Memory.SequenceEqual<uint>(this.tagEntryCosts, other.tagEntryCosts) && this.traversableTags == other.traversableTags && this.traversalProvider == other.traversalProvider;
		}

		// Token: 0x04000328 RID: 808
		public GraphMask graphMask;

		// Token: 0x04000329 RID: 809
		public int traversableTags;

		// Token: 0x0400032A RID: 810
		public float[] tagCostMultipliers;

		// Token: 0x0400032B RID: 811
		[FormerlySerializedAs("tagPenalties")]
		public uint[] tagEntryCosts;

		// Token: 0x0400032C RID: 812
		public ITraversalProvider traversalProvider;
	}
}
