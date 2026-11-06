using System;
using Pathfinding.Serialization;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x02000214 RID: 532
	[JsonDynamicType]
	[JsonDynamicTypeAlias("Pathfinding.RuleTexture", typeof(RuleTexture))]
	[JsonDynamicTypeAlias("Pathfinding.RuleAnglePenalty", typeof(RuleAnglePenalty))]
	[JsonDynamicTypeAlias("Pathfinding.RuleElevationPenalty", typeof(RuleElevationPenalty))]
	[JsonDynamicTypeAlias("Pathfinding.RulePerLayerModifications", typeof(RulePerLayerModifications))]
	public abstract class GridGraphRule
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x00055C9E File Offset: 0x00053E9E
		public virtual int Hash
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00055CA6 File Offset: 0x00053EA6
		public virtual void SetDirty()
		{
			this.dirty++;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void DisposeUnmanagedData()
		{
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void Register(GridGraphRules rules)
		{
		}

		// Token: 0x040009FE RID: 2558
		[JsonMember]
		public bool enabled = true;

		// Token: 0x040009FF RID: 2559
		private int dirty = 1;

		// Token: 0x02000215 RID: 533
		public enum Pass
		{
			// Token: 0x04000A01 RID: 2561
			BeforeCollision,
			// Token: 0x04000A02 RID: 2562
			BeforeConnections,
			// Token: 0x04000A03 RID: 2563
			AfterConnections,
			// Token: 0x04000A04 RID: 2564
			AfterErosion,
			// Token: 0x04000A05 RID: 2565
			PostProcess,
			// Token: 0x04000A06 RID: 2566
			AfterApplied
		}
	}
}
