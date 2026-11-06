using System;

namespace Invector.vCharacterController
{
	// Token: 0x0200069E RID: 1694
	public interface vIAnimatorMoveReceiver
	{
		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002841 RID: 10305
		// (set) Token: 0x06002842 RID: 10306
		bool enabled { get; set; }

		// Token: 0x06002843 RID: 10307
		void OnAnimatorMoveEvent();
	}
}
