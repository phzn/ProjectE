using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E4 RID: 740
	[AddComponentMenu("")]
	[FeedbackPath("Camera/Cinemachine Transition")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.Cinemachine", null)]
	[FeedbackHelp("This feedback will let you change the priorities of your cameras. It requires a bit of setup : adding a MMCinemachinePriorityListener to your different cameras, with unique Channel values on them. Optionally, you can add a MMCinemachinePriorityBrainListener on your Cinemachine Brain to handle different transition types and durations. Then all you have to do is pick a channel and a new priority on your feedback, and play it. Magic transition!")]
	public class MMF_CinemachineTransition : MMF_Feedback
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0005898C File Offset: 0x00056B8C
		// (set) Token: 0x060014B7 RID: 5303 RVA: 0x0005899F File Offset: 0x00056B9F
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.BlendDefintion.m_Time);
			}
			set
			{
				this.BlendDefintion.m_Time = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x000589AD File Offset: 0x00056BAD
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x000589B0 File Offset: 0x00056BB0
		protected override void AutomateTargetAcquisition()
		{
			this.TargetVirtualCamera = this.FindAutomatedTarget<CinemachineVirtualCamera>();
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x000589BE File Offset: 0x00056BBE
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000589C4 File Offset: 0x00056BC4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CinemachineTransition.FeedbackTypeAuthorized)
			{
				return;
			}
			this._tempBlend = this.BlendDefintion;
			this._tempBlend.m_Time = this.FeedbackDuration;
			if (this.Mode == MMF_CinemachineTransition.Modes.Event)
			{
				MMCinemachinePriorityEvent.Trigger(this.ChannelData, this.ForceMaxPriority, this.NewPriority, this.ForceTransition, this._tempBlend, this.ResetValuesAfterTransition, this.ComputedTimescaleMode, false);
				return;
			}
			MMCinemachinePriorityEvent.Trigger(this.ChannelData, this.ForceMaxPriority, 0, this.ForceTransition, this._tempBlend, this.ResetValuesAfterTransition, this.ComputedTimescaleMode, false);
			this.SetPriority(this.NewPriority);
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00058A6E File Offset: 0x00056C6E
		protected virtual void SetPriority(int newPriority)
		{
			this.TargetVirtualCamera.Priority = newPriority;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00058A7C File Offset: 0x00056C7C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_CinemachineTransition.FeedbackTypeAuthorized)
			{
				return;
			}
			MMCinemachinePriorityEvent.Trigger(this.ChannelData, this.ForceMaxPriority, 0, this.ForceTransition, this._tempBlend, this.ResetValuesAfterTransition, this.ComputedTimescaleMode, true);
		}

		// Token: 0x040017AC RID: 6060
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040017AD RID: 6061
		[MMFInspectorGroup("Cinemachine Transition", true, 52, false, false)]
		[Tooltip("the selected mode (either via event, or via direct binding of a specific camera)")]
		public MMF_CinemachineTransition.Modes Mode;

		// Token: 0x040017AE RID: 6062
		[Tooltip("the virtual camera to target")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public CinemachineVirtualCamera TargetVirtualCamera;

		// Token: 0x040017AF RID: 6063
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetValuesAfterTransition = true;

		// Token: 0x040017B0 RID: 6064
		[Header("Priority")]
		[Tooltip("the new priority to apply to all virtual cameras on the specified channel")]
		public int NewPriority = 10;

		// Token: 0x040017B1 RID: 6065
		[Tooltip("whether or not to force all virtual cameras on other channels to reset their priority to zero")]
		public bool ForceMaxPriority = true;

		// Token: 0x040017B2 RID: 6066
		[Tooltip("whether or not to apply a new blend")]
		public bool ForceTransition;

		// Token: 0x040017B3 RID: 6067
		[Tooltip("the new blend definition to apply")]
		[MMFCondition("ForceTransition", true)]
		public CinemachineBlendDefinition BlendDefintion;

		// Token: 0x040017B4 RID: 6068
		protected CinemachineBlendDefinition _tempBlend;

		// Token: 0x0200055C RID: 1372
		public enum Modes
		{
			// Token: 0x04002486 RID: 9350
			Event,
			// Token: 0x04002487 RID: 9351
			Binding
		}
	}
}
