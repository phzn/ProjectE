using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Pegasus
{
	// Token: 0x020001A1 RID: 417
	[Serializable]
	public class PegasusBehaviour : PlayableBehaviour
	{
		// Token: 0x04000CC0 RID: 3264
		[HideInInspector]
		public PegasusManager pegasusManager;

		// Token: 0x04000CC1 RID: 3265
		[HideInInspector]
		public float pegasusProgress;
	}
}
