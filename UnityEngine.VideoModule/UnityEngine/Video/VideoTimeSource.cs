using System;
using UnityEngine.Scripting;

namespace UnityEngine.Video
{
	// Token: 0x02000008 RID: 8
	[Obsolete("VideoTimeSource is deprecated. Use TimeUpdateMode instead. (UnityUpgradable) -> VideoTimeUpdateMode")]
	[RequiredByNativeCode]
	public enum VideoTimeSource
	{
		// Token: 0x04000014 RID: 20
		[Obsolete("AudioDSPTimeSource is deprecated. Use DSPTime instead. (UnityUpgradable) -> DSPTime")]
		AudioDSPTimeSource,
		// Token: 0x04000015 RID: 21
		[Obsolete("GameTimeSource is deprecated. Use GameTime instead. (UnityUpgradable) -> GameTime")]
		GameTimeSource
	}
}
