using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000339 RID: 825
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the border width of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Border Width")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitBorderWidth : MMF_UIToolkitFloatBase
	{
		// Token: 0x060016B8 RID: 5816 RVA: 0x000602D0 File Offset: 0x0005E4D0
		protected override void SetValue(float newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				if (this.Left)
				{
					visualElement.style.borderLeftWidth = newValue;
				}
				if (this.Right)
				{
					visualElement.style.borderRightWidth = newValue;
				}
				if (this.Bottom)
				{
					visualElement.style.borderBottomWidth = newValue;
				}
				if (this.Top)
				{
					visualElement.style.borderTopWidth = newValue;
				}
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00060388 File Offset: 0x0005E588
		protected override float GetInitialValue()
		{
			if (this.Left)
			{
				return this._visualElements[0].resolvedStyle.borderLeftWidth;
			}
			if (this.Right)
			{
				return this._visualElements[0].resolvedStyle.borderRightWidth;
			}
			if (this.Bottom)
			{
				return this._visualElements[0].resolvedStyle.borderBottomWidth;
			}
			if (this.Top)
			{
				return this._visualElements[0].resolvedStyle.borderTopWidth;
			}
			return this._visualElements[0].resolvedStyle.borderLeftWidth;
		}

		// Token: 0x04001A26 RID: 6694
		[Tooltip("whether to modify the left border width or not")]
		public bool Left = true;

		// Token: 0x04001A27 RID: 6695
		[Tooltip("whether to modify the right border width or not")]
		public bool Right = true;

		// Token: 0x04001A28 RID: 6696
		[Tooltip("whether to modify the top border width or not")]
		public bool Top = true;

		// Token: 0x04001A29 RID: 6697
		[Tooltip("whether to modify the bottom border width or not")]
		public bool Bottom = true;
	}
}
