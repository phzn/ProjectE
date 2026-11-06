using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C1 RID: 193
	[CallbackIdentity(3416)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetAppDependenciesResult_t
	{
		// Token: 0x0400025D RID: 605
		public const int k_iCallback = 3416;

		// Token: 0x0400025E RID: 606
		public EResult m_eResult;

		// Token: 0x0400025F RID: 607
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000260 RID: 608
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public AppId_t[] m_rgAppIDs;

		// Token: 0x04000261 RID: 609
		public uint m_nNumAppDependencies;

		// Token: 0x04000262 RID: 610
		public uint m_nTotalNumAppDependencies;
	}
}
