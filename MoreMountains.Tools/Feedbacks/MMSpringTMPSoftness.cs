using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200028F RID: 655
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPSoftness")]
	public class MMSpringTMPSoftness : MMSpringFloatComponent<TMP_Text>
	{
		// Token: 0x06001274 RID: 4724 RVA: 0x0004FC41 File Offset: 0x0004DE41
		protected override void ApplyValue(float newValue)
		{
			base.ApplyValue(newValue);
			this.Target.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, newValue);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0004FC60 File Offset: 0x0004DE60
		protected override void GrabCurrentValue()
		{
			this.FloatSpring.CurrentValue = this.Target.fontMaterial.GetFloat(ShaderUtilities.ID_OutlineSoftness);
		}
	}
}
