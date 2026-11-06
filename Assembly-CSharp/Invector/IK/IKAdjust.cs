using System;
using UnityEngine;

namespace Invector.IK
{
	// Token: 0x02000521 RID: 1313
	[Serializable]
	public class IKAdjust
	{
		// Token: 0x06001FE0 RID: 8160 RVA: 0x000BD6C8 File Offset: 0x000BB8C8
		public IKAdjust()
		{
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000BD708 File Offset: 0x000BB908
		public IKAdjust(string name)
		{
			this.name = name;
		}

		// Token: 0x0400209A RID: 8346
		public string name;

		// Token: 0x0400209B RID: 8347
		[ContextMenuItem("Copy", "ResetBiography")]
		public IKOffsetTransform weaponHandOffset = new IKOffsetTransform();

		// Token: 0x0400209C RID: 8348
		public IKOffsetTransform weaponHintOffset = new IKOffsetTransform();

		// Token: 0x0400209D RID: 8349
		public IKOffsetTransform supportHandOffset = new IKOffsetTransform();

		// Token: 0x0400209E RID: 8350
		public IKOffsetTransform supportHintOffset = new IKOffsetTransform();

		// Token: 0x0400209F RID: 8351
		public IKOffsetSpine spineOffset = new IKOffsetSpine();
	}
}
