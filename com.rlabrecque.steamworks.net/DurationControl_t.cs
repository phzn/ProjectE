using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D2 RID: 210
	[CallbackIdentity(167)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DurationControl_t
	{
		// Token: 0x04000295 RID: 661
		public const int k_iCallback = 167;

		// Token: 0x04000296 RID: 662
		public EResult m_eResult;

		// Token: 0x04000297 RID: 663
		public AppId_t m_appid;

		// Token: 0x04000298 RID: 664
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bApplicable;

		// Token: 0x04000299 RID: 665
		public int m_csecsLast5h;

		// Token: 0x0400029A RID: 666
		public EDurationControlProgress m_progress;

		// Token: 0x0400029B RID: 667
		public EDurationControlNotification m_notification;

		// Token: 0x0400029C RID: 668
		public int m_csecsToday;

		// Token: 0x0400029D RID: 669
		public int m_csecsRemaining;
	}
}
