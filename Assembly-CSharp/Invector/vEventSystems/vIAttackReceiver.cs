using System;

namespace Invector.vEventSystems
{
	// Token: 0x02000633 RID: 1587
	public interface vIAttackReceiver
	{
		// Token: 0x0600244B RID: 9291
		void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker);
	}
}
