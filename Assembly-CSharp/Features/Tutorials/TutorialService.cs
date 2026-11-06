using System;

namespace Features.Tutorials
{
	// Token: 0x02000BE0 RID: 3040
	public static class TutorialService
	{
		// Token: 0x06005775 RID: 22389 RVA: 0x00222408 File Offset: 0x00220608
		public static bool TryShowFirstUse(string topicId)
		{
			TutorialFirstUseController instance = TutorialFirstUseController.Instance;
			TutorialTopic tutorialTopic;
			return (instance != null && instance.IsPresenting(topicId)) || (TutorialService.AllowsFirstUseInterruption(topicId) && TutorialCatalog.TryGetTopic(topicId, out tutorialTopic) && !TutorialProgress.HasSeen(topicId) && instance != null && instance.TryPresent(topicId));
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x00222460 File Offset: 0x00220660
		public static void MarkSeenWithoutInterrupt(string topicId)
		{
			TutorialTopic tutorialTopic;
			if (TutorialCatalog.TryGetTopic(topicId, out tutorialTopic))
			{
				TutorialProgress.MarkSeen(topicId);
			}
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x0022247D File Offset: 0x0022067D
		public static bool AllowsFirstUseInterruption(string topicId)
		{
			return topicId == "cielhome_pc" || topicId == "smartphone" || topicId == "photo_mode";
		}
	}
}
