using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001AB RID: 427
	[CreateAssetMenu(menuName = "MoreMountains/MMChannel", fileName = "MMChannel")]
	public class MMChannel : ScriptableObject
	{
		// Token: 0x06000A4F RID: 2639 RVA: 0x0002F2F9 File Offset: 0x0002D4F9
		public static bool Match(MMChannelData dataA, MMChannelData dataB)
		{
			if (dataA.MMChannelMode != dataB.MMChannelMode)
			{
				return false;
			}
			if (dataA.MMChannelMode == MMChannelModes.Int)
			{
				return dataA.Channel == dataB.Channel;
			}
			return dataA.MMChannelDefinition == dataB.MMChannelDefinition;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0002F333 File Offset: 0x0002D533
		public static bool Match(MMChannelData dataA, MMChannelModes modeB, int channelB, MMChannel channelDefinitionB)
		{
			if (dataA == null)
			{
				return true;
			}
			if (dataA.MMChannelMode != modeB)
			{
				return false;
			}
			if (dataA.MMChannelMode == MMChannelModes.Int)
			{
				return dataA.Channel == channelB;
			}
			return dataA.MMChannelDefinition == channelDefinitionB;
		}
	}
}
