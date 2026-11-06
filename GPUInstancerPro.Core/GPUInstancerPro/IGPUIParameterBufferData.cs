using System;

namespace GPUInstancerPro
{
	// Token: 0x02000012 RID: 18
	public interface IGPUIParameterBufferData
	{
		// Token: 0x06000088 RID: 136
		void SetParameterBufferData();

		// Token: 0x06000089 RID: 137
		bool TryGetParameterBufferIndex(out int index);
	}
}
