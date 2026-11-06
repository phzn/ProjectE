using System;
using UnityEngine;

namespace Features.MallHack
{
	// Token: 0x02001022 RID: 4130
	public readonly struct HackMeObservationTarget
	{
		// Token: 0x06007D33 RID: 32051 RVA: 0x00334A2B File Offset: 0x00332C2B
		public HackMeObservationTarget(Renderer display, Vector3 approachPoint, Vector3 lookPoint, bool isLargeDisplay)
		{
			this.Display = display;
			this.ApproachPoint = approachPoint;
			this.LookPoint = lookPoint;
			this.IsLargeDisplay = isLargeDisplay;
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x06007D34 RID: 32052 RVA: 0x00334A4A File Offset: 0x00332C4A
		public bool IsValid
		{
			get
			{
				return this.Display != null && this.Display.enabled && this.Display.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x0400811E RID: 33054
		public readonly Renderer Display;

		// Token: 0x0400811F RID: 33055
		public readonly Vector3 ApproachPoint;

		// Token: 0x04008120 RID: 33056
		public readonly Vector3 LookPoint;

		// Token: 0x04008121 RID: 33057
		public readonly bool IsLargeDisplay;
	}
}
