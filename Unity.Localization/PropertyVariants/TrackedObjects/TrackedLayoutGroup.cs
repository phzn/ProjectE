using System;
using UnityEngine.UI;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x0200010A RID: 266
	[DisplayName("Layout Group", null)]
	[CustomTrackedObject(typeof(LayoutGroup), true)]
	[Serializable]
	public class TrackedLayoutGroup : JsonSerializerTrackedObject
	{
		// Token: 0x060006CA RID: 1738 RVA: 0x00017254 File Offset: 0x00015454
		protected override void PostApplyTrackedProperties()
		{
			LayoutGroup layoutGroup = base.Target as LayoutGroup;
			if (layoutGroup != null)
			{
				RectTransform rectTransform = layoutGroup.transform as RectTransform;
				if (rectTransform != null)
				{
					LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
				}
			}
			base.PostApplyTrackedProperties();
		}
	}
}
