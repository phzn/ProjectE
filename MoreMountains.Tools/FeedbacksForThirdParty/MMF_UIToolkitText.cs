using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000342 RID: 834
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the text an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Text")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitText : MMF_UIToolkit
	{
		// Token: 0x060016D1 RID: 5841 RVA: 0x00060940 File Offset: 0x0005EB40
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			this.SetValue(this.NewText);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00060950 File Offset: 0x0005EB50
		protected virtual void SetValue(string newValue)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				(visualElement as TextElement).text = newValue;
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x000609B0 File Offset: 0x0005EBB0
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this._visualElements == null || this._visualElements.Count == 0)
			{
				return;
			}
			this._initialText = this.GetInitialValue();
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x000609DB File Offset: 0x0005EBDB
		protected virtual string GetInitialValue()
		{
			return (this._visualElements[0] as TextElement).text;
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000609F3 File Offset: 0x0005EBF3
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_UIToolkit.FeedbackTypeAuthorized)
			{
				return;
			}
			this.SetValue(this._initialText);
		}

		// Token: 0x04001A2F RID: 6703
		[Header("Text")]
		[Tooltip("the new text to set on the target object(s)")]
		public string NewText = "";

		// Token: 0x04001A30 RID: 6704
		protected string _initialText;
	}
}
