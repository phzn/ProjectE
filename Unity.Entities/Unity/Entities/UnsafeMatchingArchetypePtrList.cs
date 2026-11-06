using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200022A RID: 554
	[DebuggerTypeProxy(typeof(UnsafeMatchingArchetypePtrListDebugView))]
	internal struct UnsafeMatchingArchetypePtrList
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00041C67 File Offset: 0x0003FE67
		public unsafe MatchingArchetype** Ptr
		{
			get
			{
				return (MatchingArchetype**)this.ListData->Ptr;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00041C74 File Offset: 0x0003FE74
		public unsafe int Length
		{
			get
			{
				return this.ListData->Length;
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00041C81 File Offset: 0x0003FE81
		public void Dispose()
		{
			UnsafeList<IntPtr>.Destroy(this.ListData);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00041C90 File Offset: 0x0003FE90
		public unsafe void Add(void* t)
		{
			ref UnsafeList<IntPtr> listData = ref *this.ListData;
			IntPtr intPtr = (IntPtr)t;
			listData.Add(in intPtr);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00041CB1 File Offset: 0x0003FEB1
		public unsafe UnsafeMatchingArchetypePtrList(EntityComponentStore* entityComponentStore)
		{
			this.ListData = UnsafeList<IntPtr>.Create(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.entityComponentStore = entityComponentStore;
		}

		// Token: 0x040005BC RID: 1468
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeList<IntPtr>* ListData;

		// Token: 0x040005BD RID: 1469
		[NativeDisableUnsafePtrRestriction]
		public unsafe EntityComponentStore* entityComponentStore;
	}
}
