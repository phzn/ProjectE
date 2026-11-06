using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x02000634 RID: 1588
	public interface vIMeleeFighter : vIAttackReceiver, vIAttackListener
	{
		// Token: 0x0600244C RID: 9292
		void BreakAttack(int breakAtkID);

		// Token: 0x0600244D RID: 9293
		void OnRecoil(int recoilID);

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x0600244E RID: 9294
		bool isAttacking { get; }

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600244F RID: 9295
		bool isArmed { get; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002450 RID: 9296
		bool isBlocking { get; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002451 RID: 9297
		Transform transform { get; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002452 RID: 9298
		GameObject gameObject { get; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002453 RID: 9299
		vICharacter character { get; }
	}
}
