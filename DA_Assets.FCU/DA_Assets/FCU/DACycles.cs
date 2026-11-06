using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DA_Assets.Extensions;

namespace DA_Assets.FCU
{
	// Token: 0x02000028 RID: 40
	public class DACycles
	{
		// Token: 0x0600008E RID: 142 RVA: 0x0000340C File Offset: 0x0000160C
		public static async Task ForEach<T>(IList<T> source, Action<T> body, float iterationTimeout = 0f, int beforeWaitItersCount = 0)
		{
			if (!source.IsEmpty<T>())
			{
				for (int i = 0; i < source.Count; i++)
				{
					if (i != 0 && iterationTimeout != 0f && beforeWaitItersCount != 0 && i % beforeWaitItersCount == 0)
					{
						await Task.Delay((int)(iterationTimeout * 1000f));
					}
					body(source[i]);
				}
			}
		}
	}
}
