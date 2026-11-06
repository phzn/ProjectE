using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000344 RID: 836
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the transform origin an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Transform Origin")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitTransformOrigin : MMF_UIToolkitVector2Base
	{
		// Token: 0x060016DA RID: 5850 RVA: 0x00060AA8 File Offset: 0x0005ECA8
		protected override void SetValue(Vector2 newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(new Length(newValue.x, this.LengthUnitX), new Length(newValue.y, this.LengthUnitY)));
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00060B34 File Offset: 0x0005ED34
		protected override Vector2 GetInitialValue()
		{
			return this._visualElements[0].resolvedStyle.transformOrigin;
		}

		// Token: 0x04001A31 RID: 6705
		[Header("Units")]
		[Tooltip("how to interpret the x value")]
		public LengthUnit LengthUnitX;

		// Token: 0x04001A32 RID: 6706
		[Tooltip("how to interpret the y value")]
		public LengthUnit LengthUnitY;
	}
}
