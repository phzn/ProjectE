using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200028C RID: 652
	[AddComponentMenu("More Mountains/Springs/MMSpringTMPDilate")]
	public class MMSpringTMPDilate : MMSpringFloatComponent<TMP_Text>
	{
		// Token: 0x0600126B RID: 4715 RVA: 0x0004FBB0 File Offset: 0x0004DDB0
		protected override void ApplyValue(float newValue)
		{
			base.ApplyValue(newValue);
			this.Target.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, newValue);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0004FBCF File Offset: 0x0004DDCF
		protected override void GrabCurrentValue()
		{
			this.FloatSpring.CurrentValue = this.Target.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);
		}
	}
}
