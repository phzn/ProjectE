using System;

namespace Invector.vEventSystems
{
	// Token: 0x02000632 RID: 1586
	public interface vIAttackListener
	{
		// Token: 0x06002448 RID: 9288
		void OnEnableAttack();

		// Token: 0x06002449 RID: 9289
		void OnDisableAttack();

		// Token: 0x0600244A RID: 9290
		void ResetAttackTriggers();
	}
}
