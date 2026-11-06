using System;
using System.Collections.Generic;

namespace Unity.Entities
{
	// Token: 0x02000050 RID: 80
	internal sealed class EntityQueryDebugView
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000AFC0 File Offset: 0x000091C0
		public unsafe EntityQueryDesc[] Desc
		{
			get
			{
				EntityQueryImpl* ptr = this.m_EntityQuery._Debugger_GetImpl();
				if (ptr != null)
				{
					return ptr->GetEntityQueryDescs();
				}
				return null;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000AFE8 File Offset: 0x000091E8
		public unsafe Entity_[] MatchingEntities
		{
			get
			{
				EntityQueryImpl* ptr = this.m_EntityQuery._Debugger_GetImpl();
				if (ptr == null)
				{
					return null;
				}
				List<Entity> list = new List<Entity>();
				if (!ptr->Debugger_GetData(list, null))
				{
					return null;
				}
				return Entity_.ResolveArray(new DebuggerDataAccess(ptr->_Access->EntityComponentStore), list);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000B030 File Offset: 0x00009230
		public unsafe List<ArchetypeChunk> MatchingChunks
		{
			get
			{
				EntityQueryImpl* ptr = this.m_EntityQuery._Debugger_GetImpl();
				if (ptr == null)
				{
					return null;
				}
				List<ArchetypeChunk> list = new List<ArchetypeChunk>();
				if (!ptr->Debugger_GetData(null, list))
				{
					return null;
				}
				return list;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B064 File Offset: 0x00009264
		public unsafe EntityQueryFilter.ChangedFilter ChangedFilter
		{
			get
			{
				EntityQueryImpl* ptr = this.m_EntityQuery._GetImpl();
				if (ptr != null)
				{
					return ptr->_Filter.Changed;
				}
				return default(EntityQueryFilter.ChangedFilter);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000B098 File Offset: 0x00009298
		public unsafe object[] SharedFilter
		{
			get
			{
				EntityQueryImpl* ptr = this.m_EntityQuery._GetImpl();
				if (ptr == null)
				{
					return null;
				}
				int count = ptr->_Filter.Shared.Count;
				object[] array = new object[count];
				for (int i = 0; i < count; i++)
				{
					int num = *((ref ptr->_Filter.Shared.SharedComponentIndex.FixedElementField) + (IntPtr)i * 4);
					int num2 = *((ref ptr->_Filter.Shared.IndexInEntityQuery.FixedElementField) + (IntPtr)i * 4);
					TypeIndex typeIndex = ptr->_QueryData->RequiredComponents[num2].TypeIndex;
					array[i] = new DebuggerDataAccess(ptr->_Access->EntityComponentStore).GetSharedComponentDataBoxed(num, typeIndex);
				}
				return array;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000B154 File Offset: 0x00009354
		public EntityQueryDebugView(EntityQuery query)
		{
			this.m_EntityQuery = query;
		}

		// Token: 0x04000097 RID: 151
		private EntityQuery m_EntityQuery;
	}
}
