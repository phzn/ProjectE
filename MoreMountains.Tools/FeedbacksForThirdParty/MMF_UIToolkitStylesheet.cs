using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000341 RID: 833
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the stylesheet on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Stylesheet")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitStylesheet : MMF_UIToolkit
	{
		// Token: 0x060016CF RID: 5839 RVA: 0x000608D0 File Offset: 0x0005EAD0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.styleSheets.Add(this.NewStylesheet);
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x04001A2E RID: 6702
		[Header("Stylesheet")]
		[Tooltip("the new stylesheet to apply to the document")]
		public StyleSheet NewStylesheet;
	}
}
