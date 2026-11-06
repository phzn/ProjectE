using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x02000666 RID: 1638
	[CreateAssetMenu(menuName = "Invector/Shooter/New BulletLifeSettings")]
	public class vBulletLifeSettings : ScriptableObject
	{
		// Token: 0x060025D7 RID: 9687 RVA: 0x000D2C88 File Offset: 0x000D0E88
		public vBulletLifeSettings.vBulletLifeInfo GetReduceLife(string tag, int layer)
		{
			vBulletLifeSettings.vBulletLostLife vBulletLostLife = this.bulletLostLifeList.Find((vBulletLifeSettings.vBulletLostLife blf) => this.isValid(blf, tag, layer));
			vBulletLifeSettings.vBulletLifeInfo vBulletLifeInfo = default(vBulletLifeSettings.vBulletLifeInfo);
			if (vBulletLostLife != null)
			{
				vBulletLifeInfo.lostLife = vBulletLostLife.reduceLife;
				vBulletLifeInfo.lostDamage = vBulletLostLife.damageReducePercentage;
				vBulletLifeInfo.minChangeTrajectory = vBulletLostLife.minChangeTrajectory;
				vBulletLifeInfo.maxChangeTrajectory = vBulletLostLife.maxChangeTrajectory;
				vBulletLifeInfo.maxThicknessToCross = vBulletLostLife.maxThicknessToCross;
				vBulletLifeInfo.ricochet = vBulletLostLife.ricochet;
			}
			return vBulletLifeInfo;
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000D2D22 File Offset: 0x000D0F22
		private bool isValid(vBulletLifeSettings.vBulletLostLife blf, string tag, int layer)
		{
			return blf.layers == (blf.layers | (1 << layer)) && blf.tags.Contains(tag);
		}

		// Token: 0x0400258C RID: 9612
		public List<vBulletLifeSettings.vBulletLostLife> bulletLostLifeList;

		// Token: 0x0400258D RID: 9613
		private bool seedGenerated;

		// Token: 0x02000667 RID: 1639
		[Serializable]
		public class vBulletLostLife
		{
			// Token: 0x060025DA RID: 9690 RVA: 0x000D2D54 File Offset: 0x000D0F54
			public vBulletLostLife()
			{
				this.layers = 1;
				this.tags = new List<string> { "Untagged" };
				this.reduceLife = 100;
			}

			// Token: 0x0400258E RID: 9614
			public LayerMask layers = 1;

			// Token: 0x0400258F RID: 9615
			public List<string> tags = new List<string> { "Untagged" };

			// Token: 0x04002590 RID: 9616
			public int reduceLife = 100;

			// Token: 0x04002591 RID: 9617
			public bool ricochet;

			// Token: 0x04002592 RID: 9618
			[vHideInInspector("ricochet", true)]
			public float maxThicknessToCross = 0.2f;

			// Token: 0x04002593 RID: 9619
			[Range(0f, 100f)]
			public int damageReducePercentage = 50;

			// Token: 0x04002594 RID: 9620
			[Range(0f, 90f)]
			public float minChangeTrajectory = 2f;

			// Token: 0x04002595 RID: 9621
			[Range(0f, 90f)]
			public float maxChangeTrajectory = 2f;
		}

		// Token: 0x02000668 RID: 1640
		public struct vBulletLifeInfo
		{
			// Token: 0x04002596 RID: 9622
			public int lostLife;

			// Token: 0x04002597 RID: 9623
			public int lostDamage;

			// Token: 0x04002598 RID: 9624
			public float minChangeTrajectory;

			// Token: 0x04002599 RID: 9625
			public float maxChangeTrajectory;

			// Token: 0x0400259A RID: 9626
			public float maxThicknessToCross;

			// Token: 0x0400259B RID: 9627
			public bool ricochet;
		}
	}
}
