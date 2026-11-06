using System;

namespace Unity.Entities
{
	// Token: 0x02000052 RID: 82
	internal sealed class ComponentSystemGroupDebugView
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000B386 File Offset: 0x00009586
		public ComponentSystemGroupDebugView(ComponentSystemGroup mComponentSystemGroup)
		{
			this.m_componentSystemGroup = mComponentSystemGroup;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000B398 File Offset: 0x00009598
		public SystemDebugView[] Systems
		{
			get
			{
				int length = this.m_componentSystemGroup.m_MasterUpdateList.Length;
				SystemDebugView[] array = new SystemDebugView[length];
				for (int i = 0; i < length; i++)
				{
					UpdateIndex updateIndex = this.m_componentSystemGroup.m_MasterUpdateList[i];
					if (updateIndex.IsManaged)
					{
						array[i] = new SystemDebugView(this.m_componentSystemGroup.m_managedSystemsToUpdate[updateIndex.Index]);
					}
					else
					{
						array[i] = new SystemDebugView(this.m_componentSystemGroup.m_UnmanagedSystemsToUpdate[updateIndex.Index]);
					}
				}
				return array;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000B425 File Offset: 0x00009625
		public bool Enabled
		{
			get
			{
				return this.m_componentSystemGroup.Enabled;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000B432 File Offset: 0x00009632
		public bool EnableSystemSorting
		{
			get
			{
				return this.m_componentSystemGroup.EnableSystemSorting;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000B43F File Offset: 0x0000963F
		public World World
		{
			get
			{
				return this.m_componentSystemGroup.World;
			}
		}

		// Token: 0x0400009A RID: 154
		private ComponentSystemGroup m_componentSystemGroup;
	}
}
