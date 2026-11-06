using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000D8 RID: 216
	public static class HaltonSequence
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x00024000 File Offset: 0x00022200
		public static float Get(int index, int radix)
		{
			float num = 0f;
			float num2 = 1f / (float)radix;
			while (index > 0)
			{
				num += (float)(index % radix) * num2;
				index /= radix;
				num2 /= (float)radix;
			}
			return num;
		}
	}
}
