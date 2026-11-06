using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x0200004A RID: 74
	internal sealed class WorldDebugView
	{
		// Token: 0x06000214 RID: 532 RVA: 0x0000A86A File Offset: 0x00008A6A
		public WorldDebugView(World world)
		{
			this.m_world = world;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000A87C File Offset: 0x00008A7C
		public List<SystemDebugView> AllSystems
		{
			get
			{
				List<SystemDebugView> list = new List<SystemDebugView>();
				List<SystemDebugView> list2;
				using (NativeArray<SystemHandle> allSystems = this.m_world.Unmanaged.GetAllSystems(Allocator.TempJob))
				{
					for (int i = 0; i < allSystems.Length; i++)
					{
						list.Add(new SystemDebugView(allSystems[i]));
					}
					list2 = list;
				}
				return list2;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		public ComponentSystemBase InitializationSystemGroup
		{
			get
			{
				return this.m_world.GetExistingSystemManaged<InitializationSystemGroup>();
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000A901 File Offset: 0x00008B01
		public ComponentSystemBase SimulationSystemGroup
		{
			get
			{
				return this.m_world.GetExistingSystemManaged<SimulationSystemGroup>();
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000A90E File Offset: 0x00008B0E
		public ComponentSystemBase PresentationSystemGroup
		{
			get
			{
				return this.m_world.GetExistingSystemManaged<PresentationSystemGroup>();
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000A91B File Offset: 0x00008B1B
		public EntityManager EntityManager
		{
			get
			{
				return this.m_world.EntityManager;
			}
		}

		// Token: 0x0400008C RID: 140
		private World m_world;
	}
}
