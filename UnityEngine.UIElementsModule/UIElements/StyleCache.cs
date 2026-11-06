using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000414 RID: 1044
	internal static class StyleCache
	{
		// Token: 0x06002148 RID: 8520 RVA: 0x0007DF0C File Offset: 0x0007C10C
		public static bool TryGetValue(long hash, out ComputedStyle data)
		{
			return StyleCache.s_ComputedStyleCache.TryGetValue(hash, out data);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0007DF2A File Offset: 0x0007C12A
		public static void SetValue(long hash, ref ComputedStyle data)
		{
			StyleCache.s_ComputedStyleCache[hash] = data;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x0007DF40 File Offset: 0x0007C140
		public static bool TryGetValue(int hash, out StyleVariableContext data)
		{
			return StyleCache.s_StyleVariableContextCache.TryGetValue(hash, out data);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0007DF5E File Offset: 0x0007C15E
		public static void SetValue(int hash, StyleVariableContext data)
		{
			StyleCache.s_StyleVariableContextCache[hash] = data;
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x0007DF70 File Offset: 0x0007C170
		public static bool TryGetValue(int hash, out ComputedTransitionProperty[] data)
		{
			return StyleCache.s_ComputedTransitionsCache.TryGetValue(hash, out data);
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0007DF8E File Offset: 0x0007C18E
		public static void SetValue(int hash, ComputedTransitionProperty[] data)
		{
			StyleCache.s_ComputedTransitionsCache[hash] = data;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0007DFA0 File Offset: 0x0007C1A0
		public static void ClearStyleCache()
		{
			foreach (KeyValuePair<long, ComputedStyle> keyValuePair in StyleCache.s_ComputedStyleCache)
			{
				keyValuePair.Value.Release();
			}
			StyleCache.s_ComputedStyleCache.Clear();
			StyleCache.s_StyleVariableContextCache.Clear();
			StyleCache.s_ComputedTransitionsCache.Clear();
		}

		// Token: 0x04000E1E RID: 3614
		private static Dictionary<long, ComputedStyle> s_ComputedStyleCache = new Dictionary<long, ComputedStyle>();

		// Token: 0x04000E1F RID: 3615
		private static Dictionary<int, StyleVariableContext> s_StyleVariableContextCache = new Dictionary<int, StyleVariableContext>();

		// Token: 0x04000E20 RID: 3616
		private static Dictionary<int, ComputedTransitionProperty[]> s_ComputedTransitionsCache = new Dictionary<int, ComputedTransitionProperty[]>();
	}
}
