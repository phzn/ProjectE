using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000346 RID: 838
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you set the visibility of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Visible")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitVisible : MMF_UIToolkitBoolBase
	{
		// Token: 0x060016E0 RID: 5856 RVA: 0x00060C10 File Offset: 0x0005EE10
		protected override void SetValue()
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				MMF_UIToolkitVisible.Modes mode = this.Mode;
				if (mode != MMF_UIToolkitVisible.Modes.Set)
				{
					if (mode == MMF_UIToolkitVisible.Modes.Toggle)
					{
						visualElement.visible = !visualElement.visible;
					}
				}
				else
				{
					visualElement.visible = this.Visible;
				}
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00060C90 File Offset: 0x0005EE90
		protected override void SetValue(bool newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				visualElement.visible = newValue;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00060CEC File Offset: 0x0005EEEC
		protected override bool GetInitialValue()
		{
			return this._visualElements[0].visible;
		}

		// Token: 0x04001A35 RID: 6709
		[Header("Visible")]
		[Tooltip("the selected mode (set : sets the object visible or not, toggle : toggles the object's visibility)")]
		public MMF_UIToolkitVisible.Modes Mode;

		// Token: 0x04001A36 RID: 6710
		[Tooltip("whether to set the object visible (true) or not")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public bool Visible;

		// Token: 0x02000581 RID: 1409
		public enum Modes
		{
			// Token: 0x040024D1 RID: 9425
			Set,
			// Token: 0x040024D2 RID: 9426
			Toggle
		}
	}
}
