using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000337 RID: 823
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the border color of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Border Color")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitBorderColor : MMF_UIToolkitColorBase
	{
		// Token: 0x060016B2 RID: 5810 RVA: 0x0005FFE8 File Offset: 0x0005E1E8
		protected override void ApplyColor(Color newColor)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				if (this.BorderLeft)
				{
					visualElement.style.borderLeftColor = newColor;
				}
				if (this.BorderRight)
				{
					visualElement.style.borderRightColor = newColor;
				}
				if (this.BorderBottom)
				{
					visualElement.style.borderBottomColor = newColor;
				}
				if (this.BorderTop)
				{
					visualElement.style.borderTopColor = newColor;
				}
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000600A0 File Offset: 0x0005E2A0
		protected override Color GetInitialColor()
		{
			if (this.BorderLeft)
			{
				return this._visualElements[0].resolvedStyle.borderLeftColor;
			}
			if (this.BorderRight)
			{
				return this._visualElements[0].resolvedStyle.borderRightColor;
			}
			if (this.BorderBottom)
			{
				return this._visualElements[0].resolvedStyle.borderBottomColor;
			}
			if (this.BorderTop)
			{
				return this._visualElements[0].resolvedStyle.borderTopColor;
			}
			return Color.black;
		}

		// Token: 0x04001A1E RID: 6686
		[MMFInspectorGroup("Borders", true, 55, true, false)]
		[Tooltip("whether or not the feedback should modify the color of the left border")]
		public bool BorderLeft = true;

		// Token: 0x04001A1F RID: 6687
		[Tooltip("whether or not the feedback should modify the color of the right border")]
		public bool BorderRight = true;

		// Token: 0x04001A20 RID: 6688
		[Tooltip("whether or not the feedback should modify the color of the bottom border")]
		public bool BorderBottom = true;

		// Token: 0x04001A21 RID: 6689
		[Tooltip("whether or not the feedback should modify the color of the top border")]
		public bool BorderTop = true;
	}
}
