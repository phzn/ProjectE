using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020001CE RID: 462
	public struct ManagedComponentAccessor<T> where T : class
	{
		// Token: 0x06000AEA RID: 2794 RVA: 0x00034144 File Offset: 0x00032344
		internal unsafe ManagedComponentAccessor(NativeArray<int> indexArray, EntityManager entityManager)
		{
			EntityDataAccess* checkedEntityDataAccess = entityManager.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			this.m_IndexArray = indexArray;
			this.m_EntityComponentStore = entityComponentStore;
			this.m_ManagedComponentStore = managedComponentStore;
		}

		// Token: 0x170000DA RID: 218
		public unsafe T this[int index]
		{
			get
			{
				return (T)((object)this.m_ManagedComponentStore.m_ManagedComponentData[this.m_IndexArray[index]]);
			}
			set
			{
				int num = this.m_IndexArray[index];
				this.m_ManagedComponentStore.UpdateManagedComponentValue(&num, value, ref *this.m_EntityComponentStore);
				this.m_IndexArray[index] = num;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x000341DD File Offset: 0x000323DD
		public int Length
		{
			get
			{
				return this.m_IndexArray.Length;
			}
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000341EA File Offset: 0x000323EA
		public override string ToString()
		{
			return "ManagedComponentAccessor[" + typeof(T).FullName + "]";
		}

		// Token: 0x040004E9 RID: 1257
		private NativeArray<int> m_IndexArray;

		// Token: 0x040004EA RID: 1258
		private unsafe EntityComponentStore* m_EntityComponentStore;

		// Token: 0x040004EB RID: 1259
		private ManagedComponentStore m_ManagedComponentStore;
	}
}
