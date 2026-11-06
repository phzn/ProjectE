using System;
using UnityEngine;

namespace Invector.vMelee
{
	// Token: 0x02000517 RID: 1303
	[Serializable]
	public class vBodyMember
	{
		// Token: 0x06001FA4 RID: 8100 RVA: 0x000BC27E File Offset: 0x000BA47E
		public void SetActiveDamage(bool active)
		{
			this.attackObject.SetActiveDamage(active);
		}

		// Token: 0x04002047 RID: 8263
		public Transform transform;

		// Token: 0x04002048 RID: 8264
		public string bodyPart;

		// Token: 0x04002049 RID: 8265
		public vMeleeAttackObject attackObject;

		// Token: 0x0400204A RID: 8266
		public bool isHuman;
	}
}
