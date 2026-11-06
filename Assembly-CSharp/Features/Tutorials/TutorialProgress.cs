using System;
using UnityEngine;

namespace Features.Tutorials
{
	// Token: 0x02000BDF RID: 3039
	public static class TutorialProgress
	{
		// Token: 0x14000093 RID: 147
		// (add) Token: 0x0600576F RID: 22383 RVA: 0x002222E8 File Offset: 0x002204E8
		// (remove) Token: 0x06005770 RID: 22384 RVA: 0x0022231C File Offset: 0x0022051C
		public static event Action Changed;

		// Token: 0x06005771 RID: 22385 RVA: 0x0022234F File Offset: 0x0022054F
		public static bool HasSeen(string topicId)
		{
			return !string.IsNullOrWhiteSpace(topicId) && PlayerPrefs.GetInt("Tutorial.Seen.v1." + topicId, 0) != 0;
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x0022236F File Offset: 0x0022056F
		public static void MarkSeen(string topicId)
		{
			if (string.IsNullOrWhiteSpace(topicId) || TutorialProgress.HasSeen(topicId))
			{
				return;
			}
			PlayerPrefs.SetInt("Tutorial.Seen.v1." + topicId, 1);
			PlayerPrefs.Save();
			Action changed = TutorialProgress.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x002223A8 File Offset: 0x002205A8
		public static void ResetAll()
		{
			for (int i = 0; i < TutorialCatalog.Topics.Count; i++)
			{
				PlayerPrefs.DeleteKey("Tutorial.Seen.v1." + TutorialCatalog.Topics[i].Id);
			}
			PlayerPrefs.Save();
			Action changed = TutorialProgress.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x002223FD File Offset: 0x002205FD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			TutorialProgress.Changed = null;
		}

		// Token: 0x040057D9 RID: 22489
		private const string SeenPrefix = "Tutorial.Seen.v1.";
	}
}
