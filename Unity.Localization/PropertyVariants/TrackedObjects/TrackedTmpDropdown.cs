using System;
using TMPro;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000106 RID: 262
	[DisplayName("TMP Dropdown", null)]
	[CustomTrackedObject(typeof(TMP_Dropdown), true)]
	[Serializable]
	public class TrackedTmpDropdown : JsonSerializerTrackedObject
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x00016F2D File Offset: 0x0001512D
		protected override void PostApplyTrackedProperties()
		{
			((TMP_Dropdown)base.Target).RefreshShownValue();
			base.PostApplyTrackedProperties();
		}
	}
}
