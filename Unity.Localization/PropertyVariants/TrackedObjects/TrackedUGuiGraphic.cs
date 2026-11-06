using System;
using UnityEngine.UI;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000108 RID: 264
	[DisplayName("UI Graphic", null)]
	[CustomTrackedObject(typeof(Graphic), true)]
	[Serializable]
	public class TrackedUGuiGraphic : JsonSerializerTrackedObject
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x00017214 File Offset: 0x00015414
		protected override void PostApplyTrackedProperties()
		{
			((Graphic)base.Target).SetAllDirty();
			base.PostApplyTrackedProperties();
		}
	}
}
