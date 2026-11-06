using System;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000021 RID: 33
	public class DefaultAllocationStrategy : IAllocationStrategy
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00004B90 File Offset: 0x00002D90
		public object New(Type type, int typeHash)
		{
			return Activator.CreateInstance(type);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004B98 File Offset: 0x00002D98
		public void Release(int typeHash, object obj)
		{
		}
	}
}
