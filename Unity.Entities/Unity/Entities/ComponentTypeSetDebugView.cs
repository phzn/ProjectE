using System;

namespace Unity.Entities
{
	// Token: 0x0200004D RID: 77
	internal sealed class ComponentTypeSetDebugView
	{
		// Token: 0x06000222 RID: 546 RVA: 0x0000AC85 File Offset: 0x00008E85
		public ComponentTypeSetDebugView(in ComponentTypeSet componentTypeSet)
		{
			this.m_ComponentTypeSet = componentTypeSet;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000AC9C File Offset: 0x00008E9C
		public ComponentType[] ComponentTypes
		{
			get
			{
				ComponentType[] array = new ComponentType[this.m_ComponentTypeSet.Length];
				for (int i = 0; i < this.m_ComponentTypeSet.Length; i++)
				{
					array[i] = this.m_ComponentTypeSet.GetComponentType(i);
				}
				return array;
			}
		}

		// Token: 0x04000092 RID: 146
		private ComponentTypeSet m_ComponentTypeSet;
	}
}
