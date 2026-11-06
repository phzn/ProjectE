using System;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000020 RID: 32
	public interface IAllocationStrategy
	{
		// Token: 0x060000E6 RID: 230
		object New(Type type, int typeHash);

		// Token: 0x060000E7 RID: 231
		void Release(int typeHash, object obj);
	}
}
