using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	// Token: 0x02000011 RID: 17
	public static class FontUpdateTracker
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00005270 File Offset: 0x00003470
		public static void TrackText(Text t)
		{
			if (t.font == null)
			{
				return;
			}
			HashSet<Text> hashSet;
			FontUpdateTracker.m_Tracked.TryGetValue(t.font, out hashSet);
			if (hashSet == null)
			{
				if (FontUpdateTracker.m_Tracked.Count == 0)
				{
					Font.textureRebuilt += FontUpdateTracker.RebuildForFont;
				}
				hashSet = new HashSet<Text>();
				FontUpdateTracker.m_Tracked.Add(t.font, hashSet);
			}
			hashSet.Add(t);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000052E0 File Offset: 0x000034E0
		private static void RebuildForFont(Font f)
		{
			HashSet<Text> hashSet;
			FontUpdateTracker.m_Tracked.TryGetValue(f, out hashSet);
			if (hashSet == null)
			{
				return;
			}
			foreach (Text text in hashSet)
			{
				text.FontTextureChanged();
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005340 File Offset: 0x00003540
		public static void UntrackText(Text t)
		{
			if (t.font == null)
			{
				return;
			}
			HashSet<Text> hashSet;
			FontUpdateTracker.m_Tracked.TryGetValue(t.font, out hashSet);
			if (hashSet == null)
			{
				return;
			}
			hashSet.Remove(t);
			if (hashSet.Count == 0)
			{
				FontUpdateTracker.m_Tracked.Remove(t.font);
				if (FontUpdateTracker.m_Tracked.Count == 0)
				{
					Font.textureRebuilt -= FontUpdateTracker.RebuildForFont;
				}
			}
		}

		// Token: 0x0400004D RID: 77
		private static Dictionary<Font, HashSet<Text>> m_Tracked = new Dictionary<Font, HashSet<Text>>();
	}
}
