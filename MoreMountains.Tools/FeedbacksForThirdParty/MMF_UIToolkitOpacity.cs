using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200033D RID: 829
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the opacity of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Opacity")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitOpacity : MMF_UIToolkitFloatBase
	{
		// Token: 0x060016C3 RID: 5827 RVA: 0x00060634 File Offset: 0x0005E834
		protected override void SetValue(float newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.opacity = newValue;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00060698 File Offset: 0x0005E898
		protected override float GetInitialValue()
		{
			return this._visualElements[0].resolvedStyle.opacity;
		}
	}
}
