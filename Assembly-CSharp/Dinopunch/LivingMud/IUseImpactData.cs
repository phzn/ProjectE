using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000752 RID: 1874
	public interface IUseImpactData
	{
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x0600307C RID: 12412
		// (set) Token: 0x0600307D RID: 12413
		float mass { get; set; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x0600307E RID: 12414
		// (set) Token: 0x0600307F RID: 12415
		float speed { get; set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003080 RID: 12416
		// (set) Token: 0x06003081 RID: 12417
		Vector3 impactDirection { get; set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06003082 RID: 12418
		// (set) Token: 0x06003083 RID: 12419
		Vector3 impactPoint { get; set; }

		// Token: 0x06003084 RID: 12420 RVA: 0x00112337 File Offset: 0x00110537
		void SetImpactData(float mass, float speed, Vector3 direction, Vector3 position)
		{
			this.mass = mass;
			this.speed = speed;
			this.impactDirection = direction;
			this.impactPoint = position;
		}
	}
}
