using System;
using UnityEngine.UI;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000109 RID: 265
	[DisplayName("UI Dropdown", null)]
	[CustomTrackedObject(typeof(Dropdown), true)]
	[Serializable]
	public class TrackedUGuiDropdown : JsonSerializerTrackedObject
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x00017234 File Offset: 0x00015434
		protected override void PostApplyTrackedProperties()
		{
			((Dropdown)base.Target).RefreshShownValue();
			base.PostApplyTrackedProperties();
		}
	}
}
