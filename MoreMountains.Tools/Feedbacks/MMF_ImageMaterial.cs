using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001ED RID: 493
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the material on a target UI Image")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Image Material")]
	public class MMF_ImageMaterial : MMF_Feedback
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0003743A File Offset: 0x0003563A
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0003743D File Offset: 0x0003563D
		protected override void AutomateTargetAcquisition()
		{
			this.TargetImage = this.FindAutomatedTarget<Image>();
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0003744C File Offset: 0x0003564C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageMaterial.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetImage == null)
			{
				return;
			}
			this._initialMaterial = this.TargetImage.material;
			this.TargetImage.material = this.NewMaterial;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0003749A File Offset: 0x0003569A
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ImageMaterial.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetImage.material = this._initialMaterial;
		}

		// Token: 0x04000E78 RID: 3704
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E79 RID: 3705
		[MMFInspectorGroup("Image", true, 12, true, false)]
		[Tooltip("the target Image we want to change the material on")]
		public Image TargetImage;

		// Token: 0x04000E7A RID: 3706
		[Tooltip("the new material to apply to the target image")]
		public Material NewMaterial;

		// Token: 0x04000E7B RID: 3707
		protected Material _initialMaterial;
	}
}
