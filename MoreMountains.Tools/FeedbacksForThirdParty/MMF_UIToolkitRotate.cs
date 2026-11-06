using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200033E RID: 830
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you rotate an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Rotate")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitRotate : MMF_UIToolkitFloatBase
	{
		// Token: 0x060016C6 RID: 5830 RVA: 0x000606B8 File Offset: 0x0005E8B8
		protected override void SetValue(float newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				this._styleRotate = new Rotate(newValue);
				visualElement.style.rotate = this._styleRotate;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00060734 File Offset: 0x0005E934
		protected override float GetInitialValue()
		{
			return this._visualElements[0].resolvedStyle.rotate.angle.value;
		}

		// Token: 0x04001A2D RID: 6701
		protected StyleRotate _styleRotate;
	}
}
