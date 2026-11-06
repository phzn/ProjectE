using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200033F RID: 831
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you scale an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Scale")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitScale : MMF_UIToolkitVector2Base
	{
		// Token: 0x060016C9 RID: 5833 RVA: 0x00060770 File Offset: 0x0005E970
		protected override void SetValue(Vector2 newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.scale = new StyleScale(new Scale(newValue));
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000607DC File Offset: 0x0005E9DC
		protected override Vector2 GetInitialValue()
		{
			return this._visualElements[0].resolvedStyle.scale.value;
		}
	}
}
