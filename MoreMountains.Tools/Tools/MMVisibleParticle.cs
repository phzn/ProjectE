using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000074 RID: 116
	[AddComponentMenu("More Mountains/Tools/Particles/MMVisibleParticle")]
	public class MMVisibleParticle : MonoBehaviour
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x0000CC46 File Offset: 0x0000AE46
		protected virtual void Start()
		{
			base.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "VisibleParticles";
		}
	}
}
