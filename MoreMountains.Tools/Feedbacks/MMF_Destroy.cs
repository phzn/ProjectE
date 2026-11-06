using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E2 RID: 482
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to destroy a target gameobject, either via Destroy, DestroyImmediate, or SetActive:False")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Destroy")]
	public class MMF_Destroy : MMF_Feedback
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00035B68 File Offset: 0x00033D68
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00035B6B File Offset: 0x00033D6B
		protected override void AutomateTargetAcquisition()
		{
			this.TargetGameObject = this.FindAutomatedTargetGameObject();
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00035B7C File Offset: 0x00033D7C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Destroy.FeedbackTypeAuthorized || this.TargetGameObject == null)
			{
				return;
			}
			this.ProceedWithDestruction(this.TargetGameObject);
			foreach (GameObject gameObject in this.ExtraTargetGameObjects)
			{
				this.ProceedWithDestruction(gameObject);
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00035BFC File Offset: 0x00033DFC
		protected virtual void ProceedWithDestruction(GameObject go)
		{
			switch (this.Mode)
			{
			case MMF_Destroy.Modes.Destroy:
				this.Owner.ProxyDestroy(go);
				return;
			case MMF_Destroy.Modes.DestroyImmediate:
				this.Owner.ProxyDestroyImmediate(go);
				return;
			case MMF_Destroy.Modes.Disable:
				this._initialActiveState = go.activeInHierarchy;
				go.SetActive(false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00035C50 File Offset: 0x00033E50
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Destroy.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.Mode == MMF_Destroy.Modes.Disable)
			{
				this.TargetGameObject.SetActive(this._initialActiveState);
			}
		}

		// Token: 0x04000E16 RID: 3606
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E17 RID: 3607
		[MMFInspectorGroup("Destruction", true, 18, true, false)]
		[Tooltip("the game object we want to destroy")]
		public GameObject TargetGameObject;

		// Token: 0x04000E18 RID: 3608
		[Tooltip("the optional list of extra gameobjects we want to change the active state of")]
		public List<GameObject> ExtraTargetGameObjects;

		// Token: 0x04000E19 RID: 3609
		[Tooltip("the selected destruction mode")]
		public MMF_Destroy.Modes Mode;

		// Token: 0x04000E1A RID: 3610
		protected bool _initialActiveState;

		// Token: 0x020004B1 RID: 1201
		public enum Modes
		{
			// Token: 0x040021E1 RID: 8673
			Destroy,
			// Token: 0x040021E2 RID: 8674
			DestroyImmediate,
			// Token: 0x040021E3 RID: 8675
			Disable
		}
	}
}
