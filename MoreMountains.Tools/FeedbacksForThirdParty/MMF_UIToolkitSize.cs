using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000340 RID: 832
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the size an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Size")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitSize : MMF_UIToolkitVector2Base
	{
		// Token: 0x060016CC RID: 5836 RVA: 0x00060814 File Offset: 0x0005EA14
		protected override void SetValue(Vector2 newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.width = newValue.x;
				visualElement.style.height = newValue.y;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00060894 File Offset: 0x0005EA94
		protected override Vector2 GetInitialValue()
		{
			return new Vector2(this._visualElements[0].resolvedStyle.width, this._visualElements[0].resolvedStyle.height);
		}
	}
}
