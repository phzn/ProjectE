using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.NotBurstCompatible;

namespace Unity.Entities
{
	// Token: 0x02000049 RID: 73
	internal sealed class EntityManagerDebugView
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0000A6A9 File Offset: 0x000088A9
		public EntityManagerDebugView(EntityManager target)
		{
			this.m_target = target;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000A6B8 File Offset: 0x000088B8
		public unsafe Entity_[] Entities
		{
			get
			{
				EntityQueryImpl* ptr = this.m_target.UniversalQueryWithSystems._Debugger_GetImpl();
				if (ptr == null)
				{
					return null;
				}
				List<Entity> list = new List<Entity>();
				if (!ptr->Debugger_GetData(list, null))
				{
					return null;
				}
				return Entity_.ResolveArray(new DebuggerDataAccess(this.m_target.World), list);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A708 File Offset: 0x00008908
		public static string CountAndCapacityViewStr(long count, long capacity, string scope)
		{
			double num = (double)count / (double)capacity;
			return string.Format("{0} ({1:P2} {2} capacity {3})", new object[] { count, num, scope, capacity });
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000A74C File Offset: 0x0000894C
		public EntityArchetype[] Archetypes
		{
			get
			{
				NativeList<EntityArchetype> nativeList = new NativeList<EntityArchetype>(Allocator.Temp);
				this.m_target.GetAllArchetypes(nativeList);
				EntityArchetype[] array = nativeList.ToArrayNBC<EntityArchetype>();
				nativeList.Dispose();
				return array;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A780 File Offset: 0x00008980
		public EntityArchetype[] ArchetypesUsed
		{
			get
			{
				EntityArchetype[] array;
				using (NativeList<EntityArchetype> nativeList = new NativeList<EntityArchetype>(Allocator.Temp))
				{
					this.m_target.GetAllArchetypes(nativeList);
					List<EntityArchetype> list = new List<EntityArchetype>();
					foreach (EntityArchetype entityArchetype in nativeList)
					{
						if (entityArchetype.ChunkCount != 0)
						{
							list.Add(entityArchetype);
						}
					}
					array = list.ToArray();
				}
				return array;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000A820 File Offset: 0x00008A20
		public string NumEntities
		{
			get
			{
				return EntityManagerDebugView.CountAndCapacityViewStr((long)this.Entities.Length, 134217728L, "World");
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A83B File Offset: 0x00008A3B
		public string NumEntityNames
		{
			get
			{
				return EntityManagerDebugView.CountAndCapacityViewStr((long)EntityNameStorage.s_State.Data.entries, 16384L, "Global");
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000A85D File Offset: 0x00008A5D
		public World World
		{
			get
			{
				return this.m_target.World;
			}
		}

		// Token: 0x0400008B RID: 139
		private EntityManager m_target;
	}
}
