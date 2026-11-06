using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200049E RID: 1182
	[Serializable]
	public class vDamageEffect
	{
		// Token: 0x04001E17 RID: 7703
		public string damageType = "";

		// Token: 0x04001E18 RID: 7704
		public List<GameObject> customDamageEffect;

		// Token: 0x04001E19 RID: 7705
		public bool rotateToHitDirection = true;

		// Token: 0x04001E1A RID: 7706
		[Tooltip("Attach prefab in Damage Receiver transform")]
		public bool attachInReceiver;

		// Token: 0x04001E1B RID: 7707
		public UnityEvent onTriggerEffect;
	}
}
