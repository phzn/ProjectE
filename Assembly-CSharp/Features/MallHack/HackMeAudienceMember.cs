using System;
using UnityEngine;

namespace Features.MallHack
{
	// Token: 0x02001002 RID: 4098
	[DisallowMultipleComponent]
	public sealed class HackMeAudienceMember : MonoBehaviour
	{
		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06007C67 RID: 31847 RVA: 0x0032D146 File Offset: 0x0032B346
		// (set) Token: 0x06007C68 RID: 31848 RVA: 0x0032D14E File Offset: 0x0032B34E
		public bool IsBusy { get; internal set; }

		// Token: 0x06007C69 RID: 31849 RVA: 0x0032D158 File Offset: 0x0032B358
		public static bool IsWatchingHackedScreen(Component phone)
		{
			if (phone == null)
			{
				return false;
			}
			HackMeAudienceMember componentInParent = phone.GetComponentInParent<HackMeAudienceMember>();
			return componentInParent != null && componentInParent.IsBusy;
		}
	}
}
