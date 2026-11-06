using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000338 RID: 824
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the border radius of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Border Radius")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitBorderRadius : MMF_UIToolkitFloatBase
	{
		// Token: 0x060016B5 RID: 5813 RVA: 0x00060154 File Offset: 0x0005E354
		protected override void SetValue(float newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				if (this.BottomLeft)
				{
					visualElement.style.borderBottomLeftRadius = newValue;
				}
				if (this.BottomRight)
				{
					visualElement.style.borderBottomRightRadius = newValue;
				}
				if (this.TopLeft)
				{
					visualElement.style.borderTopLeftRadius = newValue;
				}
				if (this.TopRight)
				{
					visualElement.style.borderTopRightRadius = newValue;
				}
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0006020C File Offset: 0x0005E40C
		protected override float GetInitialValue()
		{
			if (this.BottomLeft)
			{
				return this._visualElements[0].resolvedStyle.borderBottomLeftRadius;
			}
			if (this.BottomRight)
			{
				return this._visualElements[0].resolvedStyle.borderBottomRightRadius;
			}
			if (this.TopLeft)
			{
				return this._visualElements[0].resolvedStyle.borderTopLeftRadius;
			}
			if (this.TopRight)
			{
				return this._visualElements[0].resolvedStyle.borderTopRightRadius;
			}
			return this._visualElements[0].resolvedStyle.borderBottomLeftRadius;
		}

		// Token: 0x04001A22 RID: 6690
		[Tooltip("whether to modify the bottom left border radius or not")]
		public bool BottomLeft = true;

		// Token: 0x04001A23 RID: 6691
		[Tooltip("whether to modify the bottom right border radius or not")]
		public bool BottomRight = true;

		// Token: 0x04001A24 RID: 6692
		[Tooltip("whether to modify the top left border radius or not")]
		public bool TopLeft = true;

		// Token: 0x04001A25 RID: 6693
		[Tooltip("whether to modify the top right border radius or not")]
		public bool TopRight = true;
	}
}
