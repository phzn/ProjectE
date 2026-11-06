using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000336 RID: 822
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the background color of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Background Color")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitBackgroundColor : MMF_UIToolkitColorBase
	{
		// Token: 0x060016AF RID: 5807 RVA: 0x0005FF64 File Offset: 0x0005E164
		protected override void ApplyColor(Color newColor)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.backgroundColor = newColor;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0005FFC8 File Offset: 0x0005E1C8
		protected override Color GetInitialColor()
		{
			return this._visualElements[0].resolvedStyle.backgroundColor;
		}
	}
}
