using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000052 RID: 82
	internal interface IUnsafeStreamBlockReader : IDisposable
	{
		// Token: 0x060001D7 RID: 471
		void Reset();

		// Token: 0x060001D8 RID: 472
		BlockInfo GetNextBlock();
	}
}
