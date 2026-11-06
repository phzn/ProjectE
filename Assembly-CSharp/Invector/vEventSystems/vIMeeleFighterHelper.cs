using System;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x02000635 RID: 1589
	public static class vIMeeleFighterHelper
	{
		// Token: 0x06002454 RID: 9300 RVA: 0x000CC130 File Offset: 0x000CA330
		public static bool IsMeleeFighter(this GameObject receiver)
		{
			return receiver.GetComponent<vIMeleeFighter>() != null;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000CC13C File Offset: 0x000CA33C
		public static void ApplyDamage(this GameObject receiver, vDamage damage, vIMeleeFighter attacker)
		{
			vIAttackReceiver component = receiver.GetComponent<vIAttackReceiver>();
			if (component != null)
			{
				component.OnReceiveAttack(damage, attacker);
				return;
			}
			receiver.ApplyDamage(damage);
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000CC163 File Offset: 0x000CA363
		public static vIMeleeFighter GetMeleeFighter(this GameObject receiver)
		{
			return receiver.GetComponent<vIMeleeFighter>();
		}
	}
}
