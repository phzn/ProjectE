using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000345 RID: 837
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you translate an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Translate")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitTranslate : MMF_UIToolkitVector2Base
	{
		// Token: 0x060016DD RID: 5853 RVA: 0x00060B5C File Offset: 0x0005ED5C
		protected override void SetValue(Vector2 newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.style.translate = new StyleTranslate(new Translate(new Length(newValue.x, this.LengthUnitX), new Length(newValue.y, this.LengthUnitY)));
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00060BE8 File Offset: 0x0005EDE8
		protected override Vector2 GetInitialValue()
		{
			return this._visualElements[0].resolvedStyle.translate;
		}

		// Token: 0x04001A33 RID: 6707
		[Header("Units")]
		[Tooltip("how to interpret the x value")]
		public LengthUnit LengthUnitX;

		// Token: 0x04001A34 RID: 6708
		[Tooltip("how to interpret the y value")]
		public LengthUnit LengthUnitY;
	}
}
