using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200020A RID: 522
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to change the state of the target gameobject from active to inactive (or the opposite), on init, play, stop or reset. For each of these you can specify if you want to force a state (active or inactive), or toggle it (active becomes inactive, inactive becomes active).")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Set Active")]
	public class MMF_SetActive : MMF_Feedback
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x0003EEF8 File Offset: 0x0003D0F8
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0003EEFB File Offset: 0x0003D0FB
		protected override void AutomateTargetAcquisition()
		{
			this.TargetGameObject = this.FindAutomatedTargetGameObject();
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0003EF0C File Offset: 0x0003D10C
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._initialStates = new List<bool>(this.ExtraTargetGameObjects.Count);
			if (this.Active && this.TargetGameObject != null)
			{
				this._initialState = this.TargetGameObject.activeInHierarchy;
				for (int i = 0; i < this.ExtraTargetGameObjects.Count; i++)
				{
					this._initialStates.Add(this.ExtraTargetGameObjects[i].activeInHierarchy);
				}
				if (this.SetStateOnInit)
				{
					this.SetStatus(this.StateOnInit);
				}
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0003EFA3 File Offset: 0x0003D1A3
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SetActive.FeedbackTypeAuthorized || this.TargetGameObject == null)
			{
				return;
			}
			if (this.SetStateOnPlay)
			{
				this.SetStatus(this.StateOnPlay);
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0003EFD7 File Offset: 0x0003D1D7
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active && MMF_SetActive.FeedbackTypeAuthorized && this.TargetGameObject != null && this.SetStateOnStop)
			{
				this.SetStatus(this.StateOnStop);
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0003F014 File Offset: 0x0003D214
		protected override void CustomReset()
		{
			base.CustomReset();
			if (this.InCooldown)
			{
				return;
			}
			if (this.Active && MMF_SetActive.FeedbackTypeAuthorized && this.TargetGameObject != null && this.SetStateOnReset)
			{
				this.SetStatus(this.StateOnReset);
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0003F064 File Offset: 0x0003D264
		protected override void CustomPlayerComplete()
		{
			base.CustomPlayerComplete();
			if (this.InCooldown)
			{
				return;
			}
			if (this.Active && MMF_SetActive.FeedbackTypeAuthorized && this.TargetGameObject != null && this.SetStateOnPlayerComplete)
			{
				this.SetStatus(this.StateOnPlayerComplete);
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0003F0B4 File Offset: 0x0003D2B4
		protected override void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomSkipToTheEnd(position, feedbacksIntensity);
			if (this.InCooldown)
			{
				return;
			}
			if (this.Active && MMF_SetActive.FeedbackTypeAuthorized && this.TargetGameObject != null && this.SetStateOnSkip)
			{
				this.SetStatus(this.StateOnSkip);
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0003F104 File Offset: 0x0003D304
		protected virtual void SetStatus(MMF_SetActive.PossibleStates state)
		{
			bool flag = false;
			switch (state)
			{
			case MMF_SetActive.PossibleStates.Active:
				flag = this.NormalPlayDirection;
				break;
			case MMF_SetActive.PossibleStates.Inactive:
				flag = !this.NormalPlayDirection;
				break;
			case MMF_SetActive.PossibleStates.Toggle:
				flag = !this.TargetGameObject.activeInHierarchy;
				break;
			}
			this.ApplyStatus(this.TargetGameObject, flag);
			foreach (GameObject gameObject in this.ExtraTargetGameObjects)
			{
				this.ApplyStatus(gameObject, flag);
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0003F1A8 File Offset: 0x0003D3A8
		protected virtual void ApplyStatus(GameObject target, bool newState)
		{
			target.SetActive(newState);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0003F1B4 File Offset: 0x0003D3B4
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SetActive.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetGameObject.SetActive(this._initialState);
			for (int i = 0; i < this.ExtraTargetGameObjects.Count; i++)
			{
				this.ExtraTargetGameObjects[i].SetActive(this._initialStates[i]);
			}
		}

		// Token: 0x04001059 RID: 4185
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400105A RID: 4186
		[MMFInspectorGroup("Set Active Target", true, 12, true, false)]
		[Tooltip("the gameobject we want to change the active state of")]
		public GameObject TargetGameObject;

		// Token: 0x0400105B RID: 4187
		[Tooltip("a list of extra gameobjects we want to change the active state of")]
		public List<GameObject> ExtraTargetGameObjects;

		// Token: 0x0400105C RID: 4188
		[MMFInspectorGroup("States", true, 14, false, false)]
		[Tooltip("whether or not we should alter the state of the target object on init")]
		public bool SetStateOnInit;

		// Token: 0x0400105D RID: 4189
		[MMFCondition("SetStateOnInit", true)]
		[Tooltip("how to change the state on init")]
		public MMF_SetActive.PossibleStates StateOnInit = MMF_SetActive.PossibleStates.Inactive;

		// Token: 0x0400105E RID: 4190
		[Tooltip("whether or not we should alter the state of the target object on play")]
		public bool SetStateOnPlay;

		// Token: 0x0400105F RID: 4191
		[Tooltip("how to change the state on play")]
		[MMFCondition("SetStateOnPlay", true)]
		public MMF_SetActive.PossibleStates StateOnPlay = MMF_SetActive.PossibleStates.Inactive;

		// Token: 0x04001060 RID: 4192
		[Tooltip("whether or not we should alter the state of the target object on stop")]
		public bool SetStateOnStop;

		// Token: 0x04001061 RID: 4193
		[Tooltip("how to change the state on stop")]
		[MMFCondition("SetStateOnStop", true)]
		public MMF_SetActive.PossibleStates StateOnStop = MMF_SetActive.PossibleStates.Inactive;

		// Token: 0x04001062 RID: 4194
		[Tooltip("whether or not we should alter the state of the target object on reset")]
		public bool SetStateOnReset;

		// Token: 0x04001063 RID: 4195
		[Tooltip("how to change the state on reset")]
		[MMFCondition("SetStateOnReset", true)]
		public MMF_SetActive.PossibleStates StateOnReset = MMF_SetActive.PossibleStates.Inactive;

		// Token: 0x04001064 RID: 4196
		[Tooltip("whether or not we should alter the state of the target object on skip")]
		public bool SetStateOnSkip;

		// Token: 0x04001065 RID: 4197
		[Tooltip("how to change the state on skip")]
		[MMFCondition("SetStateOnSkip", true)]
		public MMF_SetActive.PossibleStates StateOnSkip = MMF_SetActive.PossibleStates.Inactive;

		// Token: 0x04001066 RID: 4198
		[Tooltip("whether or not we should alter the state of the target object when the player this feedback belongs to is done playing all its feedbacks")]
		public bool SetStateOnPlayerComplete;

		// Token: 0x04001067 RID: 4199
		[Tooltip("how to change the state on player complete")]
		[MMFCondition("SetStateOnPlayerComplete", true)]
		public MMF_SetActive.PossibleStates StateOnPlayerComplete = MMF_SetActive.PossibleStates.Inactive;

		// Token: 0x04001068 RID: 4200
		protected bool _initialState;

		// Token: 0x04001069 RID: 4201
		protected List<bool> _initialStates;

		// Token: 0x020004EC RID: 1260
		public enum PossibleStates
		{
			// Token: 0x040022FD RID: 8957
			Active,
			// Token: 0x040022FE RID: 8958
			Inactive,
			// Token: 0x040022FF RID: 8959
			Toggle
		}
	}
}
