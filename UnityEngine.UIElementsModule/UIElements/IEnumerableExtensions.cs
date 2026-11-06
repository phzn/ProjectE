using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200025F RID: 607
	internal static class IEnumerableExtensions
	{
		// Token: 0x0600115C RID: 4444 RVA: 0x0003F064 File Offset: 0x0003D264
		internal static bool HasValues(this IEnumerable<string> collection)
		{
			bool flag = collection == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				using (IEnumerator<string> enumerator = collection.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}
	}
}
