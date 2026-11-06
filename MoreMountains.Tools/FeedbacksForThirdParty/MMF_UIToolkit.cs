using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000331 RID: 817
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback is a base for UI Toolkit feedbacks")]
	public class MMF_UIToolkit : MMF_Feedback
	{
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x0005F3E4 File Offset: 0x0005D5E4
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x0005F3E7 File Offset: 0x0005D5E7
		protected override void AutomateTargetAcquisition()
		{
			this.TargetDocument = this.FindAutomatedTarget<UIDocument>();
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x0005F3F5 File Offset: 0x0005D5F5
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.PerformQuery();
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x0005F404 File Offset: 0x0005D604
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0005F408 File Offset: 0x0005D608
		protected virtual void PerformQuery()
		{
			MMF_UIToolkit.QueryModes queryMode = this.QueryMode;
			if (queryMode == MMF_UIToolkit.QueryModes.Name)
			{
				this._visualElements = this.TargetDocument.rootVisualElement.Query(this.Query, null).ToList();
				return;
			}
			if (queryMode != MMF_UIToolkit.QueryModes.Class)
			{
				return;
			}
			this._visualElements = this.TargetDocument.rootVisualElement.Query(null, this.Query).ToList();
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0005F46F File Offset: 0x0005D66F
		protected virtual void HandleMarkDirty(VisualElement element)
		{
			if (this.MarkDirty)
			{
				element.MarkDirtyRepaint();
			}
		}

		// Token: 0x040019EF RID: 6639
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040019F0 RID: 6640
		[MMFInspectorGroup("Target", true, 54, true, false)]
		[Tooltip("the UI document on which to make modifications")]
		public UIDocument TargetDocument;

		// Token: 0x040019F1 RID: 6641
		[Tooltip("the way to perform the query, either via element name or via class")]
		public MMF_UIToolkit.QueryModes QueryMode;

		// Token: 0x040019F2 RID: 6642
		[Tooltip("the query to perform (replace this with your own element name or class)")]
		public string Query = "ButtonA";

		// Token: 0x040019F3 RID: 6643
		[Tooltip("whether to mark the UI document dirty after the operation. Set this to true when making a change that requires a repaint such as when using generateVisualContent to render a mesh and the mesh data has now changed.")]
		public bool MarkDirty;

		// Token: 0x040019F4 RID: 6644
		protected List<VisualElement> _visualElements = new List<VisualElement>();

		// Token: 0x02000579 RID: 1401
		public enum QueryModes
		{
			// Token: 0x040024B1 RID: 9393
			Name,
			// Token: 0x040024B2 RID: 9394
			Class
		}
	}
}
