using System;

namespace Invector
{
	// Token: 0x02000462 RID: 1122
	public class vAnimatorSetInt : vAnimatorSetValue<int>
	{
		// Token: 0x06001BE0 RID: 7136 RVA: 0x000AF3C1 File Offset: 0x000AD5C1
		protected override int GetEnterValue()
		{
			if (!this.randomEnter)
			{
				return base.GetEnterValue();
			}
			return this.random.Range(base.GetEnterValue(), this.maxEnterValue);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000AF3E9 File Offset: 0x000AD5E9
		protected override int GetExitValue()
		{
			if (!this.randomExit)
			{
				return base.GetExitValue();
			}
			return this.random.Range(base.GetExitValue(), this.maxExitValue);
		}

		// Token: 0x04001D29 RID: 7465
		private vFisherYatesRandom random = new vFisherYatesRandom();

		// Token: 0x04001D2A RID: 7466
		[vHelpBox("Random Value between Default Value and Max Value", vHelpBoxAttribute.MessageType.None)]
		public bool randomEnter;

		// Token: 0x04001D2B RID: 7467
		[vHideInInspector("randomEnter", false)]
		public int maxEnterValue;

		// Token: 0x04001D2C RID: 7468
		public bool randomExit;

		// Token: 0x04001D2D RID: 7469
		[vHideInInspector("randomExit", false)]
		public int maxExitValue;
	}
}
