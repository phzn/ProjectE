using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000343 RID: 835
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the text color an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Text Color")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitTextColor : MMF_UIToolkitColorBase
	{
		// Token: 0x060016D7 RID: 5847 RVA: 0x00060A24 File Offset: 0x0005EC24
		protected override void ApplyColor(Color newColor)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.color = newColor;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00060A88 File Offset: 0x0005EC88
		protected override Color GetInitialColor()
		{
			return this._visualElements[0].resolvedStyle.color;
		}
	}
}
