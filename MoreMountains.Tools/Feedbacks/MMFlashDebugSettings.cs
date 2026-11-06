using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000249 RID: 585
	[Serializable]
	public class MMFlashDebugSettings
	{
		// Token: 0x04001281 RID: 4737
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001282 RID: 4738
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMFEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04001283 RID: 4739
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMFEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04001284 RID: 4740
		public Color FlashColor = Color.white;

		// Token: 0x04001285 RID: 4741
		public float FlashDuration = 0.2f;

		// Token: 0x04001286 RID: 4742
		public float FlashAlpha = 1f;

		// Token: 0x04001287 RID: 4743
		public int FlashID;
	}
}
