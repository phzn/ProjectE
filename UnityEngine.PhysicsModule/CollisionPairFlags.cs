using System;

namespace UnityEngine
{
	// Token: 0x0200003F RID: 63
	internal enum CollisionPairFlags : ushort
	{
		// Token: 0x040000E9 RID: 233
		RemovedShape = 1,
		// Token: 0x040000EA RID: 234
		RemovedOtherShape,
		// Token: 0x040000EB RID: 235
		ActorPairHasFirstTouch = 4,
		// Token: 0x040000EC RID: 236
		ActorPairLostTouch = 8,
		// Token: 0x040000ED RID: 237
		InternalHasImpulses = 16,
		// Token: 0x040000EE RID: 238
		InternalContactsAreFlipped = 32
	}
}
