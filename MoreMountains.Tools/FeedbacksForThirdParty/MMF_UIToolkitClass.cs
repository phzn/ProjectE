using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200033A RID: 826
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the class of an element on a target UI Document")]
	[FeedbackPath("UI Toolkit/UITK Class")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.UIToolkit", null)]
	public class MMF_UIToolkitClass : MMF_UIToolkit
	{
		// Token: 0x060016BB RID: 5819 RVA: 0x0006044C File Offset: 0x0005E64C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			foreach (VisualElement visualElement in this._visualElements)
			{
				switch (this.Mode)
				{
				case MMF_UIToolkitClass.Modes.AddToClassList:
					visualElement.AddToClassList(this.ClassName);
					break;
				case MMF_UIToolkitClass.Modes.EnableInClassList:
					visualElement.EnableInClassList(this.ClassName, this.Enable);
					break;
				case MMF_UIToolkitClass.Modes.ToggleInClassList:
					visualElement.ToggleInClassList(this.ClassName);
					break;
				case MMF_UIToolkitClass.Modes.RemoveFromClassList:
					visualElement.RemoveFromClassList(this.ClassName);
					break;
				case MMF_UIToolkitClass.Modes.ClearClassList:
					visualElement.ClearClassList();
					break;
				}
				this.HandleMarkDirty(visualElement);
			}
		}

		// Token: 0x04001A2A RID: 6698
		[Header("Class Manipulation")]
		[Tooltip("whether to add, enable, toggle, remove or clear the class list")]
		public MMF_UIToolkitClass.Modes Mode;

		// Token: 0x04001A2B RID: 6699
		[Tooltip("the name of the class to add, enable, toggle or remove")]
		[MMFEnumCondition("Mode", new int[] { 0, 1, 2, 3 })]
		public string ClassName = "";

		// Token: 0x04001A2C RID: 6700
		[Tooltip("in EnableInClassList mode, whether to enable or disable the class")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool Enable = true;

		// Token: 0x02000580 RID: 1408
		public enum Modes
		{
			// Token: 0x040024CB RID: 9419
			AddToClassList,
			// Token: 0x040024CC RID: 9420
			EnableInClassList,
			// Token: 0x040024CD RID: 9421
			ToggleInClassList,
			// Token: 0x040024CE RID: 9422
			RemoveFromClassList,
			// Token: 0x040024CF RID: 9423
			ClearClassList
		}
	}
}
