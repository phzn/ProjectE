using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200033B RID: 827
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the font size of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Font Size")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitFontSize : MMF_UIToolkitFloatBase
	{
		// Token: 0x060016BD RID: 5821 RVA: 0x00060524 File Offset: 0x0005E724
		protected override void SetValue(float newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				int num = Mathf.FloorToInt(newValue);
				visualElement.style.fontSize = (float)num;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x00060590 File Offset: 0x0005E790
		protected override float GetInitialValue()
		{
			return this._visualElements[0].resolvedStyle.fontSize;
		}
	}
}
