using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000070 RID: 112
	[ExecuteAlways]
	[AddComponentMenu("More Mountains/Tools/Particles/MMDelayParticles")]
	public class MMDelayParticles : MonoBehaviour
	{
		// Token: 0x0600029C RID: 668 RVA: 0x0000CA90 File Offset: 0x0000AC90
		protected virtual void Start()
		{
			if (this.ApplyDelayOnStart)
			{
				this.ApplyDelay();
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000CAA0 File Offset: 0x0000ACA0
		protected virtual void ApplyDelay()
		{
			if (base.gameObject.GetComponent<ParticleSystem>() != null)
			{
				ParticleSystem.MainModule main = base.gameObject.GetComponent<ParticleSystem>().main;
				main.startDelay = main.startDelay.constant + this.Delay;
			}
			Component[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
			this.particleSystems = componentsInChildren;
			componentsInChildren = this.particleSystems;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ParticleSystem.MainModule main2 = ((ParticleSystem)componentsInChildren[i]).main;
				main2.startDelay = main2.startDelay.constant + this.Delay;
			}
		}

		// Token: 0x040003C8 RID: 968
		[Header("Delay")]
		public float Delay;

		// Token: 0x040003C9 RID: 969
		public bool DelayChildren = true;

		// Token: 0x040003CA RID: 970
		public bool ApplyDelayOnStart;

		// Token: 0x040003CB RID: 971
		[MMInspectorButton("ApplyDelay")]
		public bool ApplyDelayButton;

		// Token: 0x040003CC RID: 972
		protected Component[] particleSystems;
	}
}
