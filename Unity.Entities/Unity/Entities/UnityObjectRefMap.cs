using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x02000287 RID: 647
	internal struct UnityObjectRefMap : IDisposable
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00048BFD File Offset: 0x00046DFD
		public bool IsCreated
		{
			get
			{
				return this.InstanceIDs.IsCreated && this.InstanceIDMap.IsCreated;
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00048C19 File Offset: 0x00046E19
		public UnityObjectRefMap(Allocator allocator)
		{
			this.InstanceIDMap = new NativeHashMap<int, int>(0, allocator);
			this.InstanceIDs = new NativeList<int>(0, allocator);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00048C3F File Offset: 0x00046E3F
		public void Dispose()
		{
			this.InstanceIDMap.Dispose();
			this.InstanceIDs.Dispose();
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00048C58 File Offset: 0x00046E58
		public Object[] ToObjectArray()
		{
			List<Object> list = new List<Object>();
			if (this.IsCreated && this.InstanceIDs.Length > 0)
			{
				Resources.InstanceIDToObjectList(this.InstanceIDs.AsArray(), list);
			}
			return list.ToArray();
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00048C98 File Offset: 0x00046E98
		public int Add(int instanceId)
		{
			int num = -1;
			if (instanceId != 0 && this.IsCreated && !this.InstanceIDMap.TryGetValue(instanceId, out num))
			{
				num = this.InstanceIDs.Length;
				this.InstanceIDMap.Add(instanceId, num);
				this.InstanceIDs.Add(in instanceId);
			}
			return num;
		}

		// Token: 0x04000695 RID: 1685
		public NativeHashMap<int, int> InstanceIDMap;

		// Token: 0x04000696 RID: 1686
		public NativeList<int> InstanceIDs;
	}
}
