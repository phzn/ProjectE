using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000460 RID: 1120
	public class vAnimatorSetBool : vAnimatorSetValue<bool>
	{
		// Token: 0x06001BDA RID: 7130 RVA: 0x000AF297 File Offset: 0x000AD497
		protected override bool GetEnterValue()
		{
			if (!this.randomEnter)
			{
				return base.GetEnterValue();
			}
			return Random.Range(0, 100) > 50;
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000AF2B4 File Offset: 0x000AD4B4
		protected override bool GetExitValue()
		{
			if (!this.randomExit)
			{
				return base.GetExitValue();
			}
			return Random.Range(0, 100) > 50;
		}

		// Token: 0x04001D20 RID: 7456
		[vHelpBox("Random Value between True and False", vHelpBoxAttribute.MessageType.None)]
		public bool randomEnter;

		// Token: 0x04001D21 RID: 7457
		public bool randomExit;
	}
}
