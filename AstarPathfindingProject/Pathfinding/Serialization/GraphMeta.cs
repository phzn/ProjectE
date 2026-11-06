using System;
using System.Collections.Generic;

namespace Pathfinding.Serialization
{
	// Token: 0x0200024B RID: 587
	public class GraphMeta
	{
		// Token: 0x06000EBA RID: 3770 RVA: 0x0005AA00 File Offset: 0x00058C00
		public Type GetGraphType(int index, Type[] availableGraphTypes)
		{
			if (string.IsNullOrEmpty(this.typeNames[index]))
			{
				return null;
			}
			for (int i = 0; i < availableGraphTypes.Length; i++)
			{
				if (availableGraphTypes[i].FullName == this.typeNames[index])
				{
					return availableGraphTypes[i];
				}
			}
			throw new Exception("No graph of type '" + this.typeNames[index] + "' could be created, type does not exist");
		}

		// Token: 0x04000B10 RID: 2832
		public Version version;

		// Token: 0x04000B11 RID: 2833
		public int graphs;

		// Token: 0x04000B12 RID: 2834
		public List<string> guids;

		// Token: 0x04000B13 RID: 2835
		public List<string> typeNames;
	}
}
