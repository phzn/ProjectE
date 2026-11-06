using System;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D22 RID: 3362
	[DisallowMultipleComponent]
	public sealed class LongWayHomeUniversityBystanderEvents : MonoBehaviour
	{
		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x0600621F RID: 25119 RVA: 0x00271658 File Offset: 0x0026F858
		// (remove) Token: 0x06006220 RID: 25120 RVA: 0x00271690 File Offset: 0x0026F890
		public event Action Flash;

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06006221 RID: 25121 RVA: 0x002716C8 File Offset: 0x0026F8C8
		// (remove) Token: 0x06006222 RID: 25122 RVA: 0x00271700 File Offset: 0x0026F900
		public event Action VideoLightOn;

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06006223 RID: 25123 RVA: 0x00271738 File Offset: 0x0026F938
		// (remove) Token: 0x06006224 RID: 25124 RVA: 0x00271770 File Offset: 0x0026F970
		public event Action VideoLightOff;

		// Token: 0x06006225 RID: 25125 RVA: 0x002717A5 File Offset: 0x0026F9A5
		public void TriggerFlash()
		{
			Action flash = this.Flash;
			if (flash == null)
			{
				return;
			}
			flash();
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x002717A5 File Offset: 0x0026F9A5
		public void TriggerFlashWithMultiplier(float multiplier)
		{
			Action flash = this.Flash;
			if (flash == null)
			{
				return;
			}
			flash();
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x00004F31 File Offset: 0x00003131
		public void StopFlash()
		{
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x002717B7 File Offset: 0x0026F9B7
		public void StartVideoLight()
		{
			Action videoLightOn = this.VideoLightOn;
			if (videoLightOn == null)
			{
				return;
			}
			videoLightOn();
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x002717C9 File Offset: 0x0026F9C9
		public void StopVideoLight()
		{
			Action videoLightOff = this.VideoLightOff;
			if (videoLightOff == null)
			{
				return;
			}
			videoLightOff();
		}

		// Token: 0x0600622A RID: 25130 RVA: 0x00004F31 File Offset: 0x00003131
		public void PlayFootstep(AnimationEvent animationEvent)
		{
		}
	}
}
