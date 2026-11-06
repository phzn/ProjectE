using System;
using Unity.Entities;
using Unity.Properties;
using UnityEngine.Scripting;

namespace Pathfinding.ECS
{
	// Token: 0x020002FC RID: 764
	[GeneratePropertyBag]
	[TypeManager.TypeOverridesAttribute(true, true, true)]
	[Serializable]
	public class ManagedSettings : IComponentData, IQueryTypeParameter, ICloneable, IEquatable<ManagedSettings>
	{
		// Token: 0x060012F6 RID: 4854 RVA: 0x0007350D File Offset: 0x0007170D
		public object Clone()
		{
			return this.CloneAndSimplifyDefaults(false);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00073518 File Offset: 0x00071718
		public ManagedSettings CloneAndSimplifyDefaults(bool simplify)
		{
			float[] array = this.pathfindingSettings.tagCostMultipliers;
			if (simplify && array != null)
			{
				bool flag = true;
				for (int i = 0; i < this.pathfindingSettings.tagCostMultipliers.Length; i++)
				{
					flag &= this.pathfindingSettings.tagCostMultipliers[i] == 1f;
				}
				if (flag)
				{
					array = null;
				}
			}
			if (array != null)
			{
				array = (float[])array.Clone();
			}
			uint[] array2 = this.pathfindingSettings.tagEntryCosts;
			if (simplify && array2 != null)
			{
				bool flag2 = true;
				for (int j = 0; j < this.pathfindingSettings.tagEntryCosts.Length; j++)
				{
					flag2 &= this.pathfindingSettings.tagEntryCosts[j] == 0U;
				}
				if (flag2)
				{
					array2 = null;
				}
			}
			if (array2 != null)
			{
				array2 = (uint[])array2.Clone();
			}
			return new ManagedSettings
			{
				pathfindingSettings = new PathRequestSettings
				{
					graphMask = this.pathfindingSettings.graphMask,
					tagEntryCosts = array2,
					tagCostMultipliers = array,
					traversableTags = this.pathfindingSettings.traversableTags,
					traversalProvider = null
				},
				onTraverseOffMeshLink = null
			};
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00073630 File Offset: 0x00071830
		public bool Equals(ManagedSettings other)
		{
			return other != null && this.pathfindingSettings.Equals(other.pathfindingSettings) && this.onTraverseOffMeshLink == other.onTraverseOffMeshLink;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0000378A File Offset: 0x0000198A
		[Preserve]
		public ManagedSettings()
		{
		}

		// Token: 0x04000DC8 RID: 3528
		[NonSerialized]
		public IOffMeshLinkHandler onTraverseOffMeshLink;

		// Token: 0x04000DC9 RID: 3529
		public PathRequestSettings pathfindingSettings;
	}
}
