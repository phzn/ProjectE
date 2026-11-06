using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200075B RID: 1883
	public class PlayVFXOnStart : MonoBehaviour
	{
		// Token: 0x060030AC RID: 12460 RVA: 0x00112C58 File Offset: 0x00110E58
		public void OnEnable()
		{
			VisualEffect[] effects = this.Effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].Play();
			}
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x00112C84 File Offset: 0x00110E84
		public void OnDisable()
		{
			VisualEffect[] effects = this.Effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].Stop();
			}
		}

		// Token: 0x04003123 RID: 12579
		public VisualEffect[] Effects;
	}
}
