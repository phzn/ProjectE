using System;

namespace Unity.Collections
{
	// Token: 0x0200007B RID: 123
	public interface IUTF8Bytes
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060006CE RID: 1742
		bool IsEmpty { get; }

		// Token: 0x060006CF RID: 1743
		unsafe byte* GetUnsafePtr();

		// Token: 0x060006D0 RID: 1744
		bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory);
	}
}
