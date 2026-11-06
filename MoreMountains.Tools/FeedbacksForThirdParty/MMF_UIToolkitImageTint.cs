using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200033C RID: 828
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the image tint of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Image Tint")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitImageTint : MMF_UIToolkitColorBase
	{
		// Token: 0x060016C0 RID: 5824 RVA: 0x000605B0 File Offset: 0x0005E7B0
		protected override void ApplyColor(Color newColor)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.unityBackgroundImageTintColor = newColor;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00060614 File Offset: 0x0005E814
		protected override Color GetInitialColor()
		{
			return this._visualElements[0].resolvedStyle.unityBackgroundImageTintColor;
		}
	}
}
