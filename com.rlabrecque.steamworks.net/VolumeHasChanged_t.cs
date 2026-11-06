using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000087 RID: 135
	[CallbackIdentity(4002)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct VolumeHasChanged_t
	{
		// Token: 0x0400016F RID: 367
		public const int k_iCallback = 4002;

		// Token: 0x04000170 RID: 368
		public float m_flNewVolume;
	}
}
