using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E1 RID: 737
	[AddComponentMenu("")]
	[FeedbackPath("Camera/Cinemachine Impulse")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.Cinemachine", null)]
	[FeedbackHelp("This feedback lets you trigger a Cinemachine Impulse event. You'll need a Cinemachine Impulse Listener on your camera for this to work.")]
	public class MMF_CinemachineImpulse : MMF_Feedback
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x000586E9 File Offset: 0x000568E9
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x000586EC File Offset: 0x000568EC
		public override float FeedbackDuration
		{
			get
			{
				if (this.m_ImpulseDefinition == null)
				{
					return 0f;
				}
				return this.m_ImpulseDefinition.m_TimeEnvelope.Duration;
			}
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0005870C File Offset: 0x0005690C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CinemachineImpulse.FeedbackTypeAuthorized)
			{
				return;
			}
			CinemachineImpulseManager.Instance.IgnoreTimeScale = !this.InScaledTimescaleMode;
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			this.m_ImpulseDefinition.CreateEvent(position, this.Velocity * num);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0005875D File Offset: 0x0005695D
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CinemachineImpulse.FeedbackTypeAuthorized || !this.ClearImpulseOnStop)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			CinemachineImpulseManager.Instance.Clear();
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0005878C File Offset: 0x0005698C
		public override void OnAddFeedback()
		{
			if (this.m_ImpulseDefinition == null)
			{
				this.m_ImpulseDefinition = new CinemachineImpulseDefinition();
			}
			this.m_ImpulseDefinition.m_RawSignal = Resources.Load<NoiseSettings>("MM_6D_Shake");
			this.Velocity = new Vector3(5f, 5f, 5f);
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x000587DC File Offset: 0x000569DC
		public override void OnDrawGizmosSelectedHandler()
		{
			if (!this.DrawGizmos)
			{
				return;
			}
			if (this.m_ImpulseDefinition.m_ImpulseType == CinemachineImpulseDefinition.ImpulseTypes.Dissipating || this.m_ImpulseDefinition.m_ImpulseType == CinemachineImpulseDefinition.ImpulseTypes.Propagating || this.m_ImpulseDefinition.m_ImpulseType == CinemachineImpulseDefinition.ImpulseTypes.Legacy)
			{
				Gizmos.color = MMColors.Aqua;
				Gizmos.DrawWireSphere(this.Owner.transform.position, this.m_ImpulseDefinition.m_DissipationDistance);
			}
			if (this.m_ImpulseDefinition.m_ImpulseType == CinemachineImpulseDefinition.ImpulseTypes.Legacy)
			{
				Gizmos.color = MMColors.ReunoYellow;
				Gizmos.DrawWireSphere(this.Owner.transform.position, this.m_ImpulseDefinition.m_ImpactRadius);
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0005887E File Offset: 0x00056A7E
		public override void AutomaticShakerSetup()
		{
			MMCinemachineHelpers.AutomaticCinemachineShakersSetup(this.Owner, "CinemachineImpulse");
		}

		// Token: 0x040017A2 RID: 6050
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040017A3 RID: 6051
		[MMFInspectorGroup("Cinemachine Impulse", true, 28, false, false)]
		[Tooltip("the impulse definition to broadcast")]
		public CinemachineImpulseDefinition m_ImpulseDefinition = new CinemachineImpulseDefinition();

		// Token: 0x040017A4 RID: 6052
		[Tooltip("the velocity to apply to the impulse shake")]
		public Vector3 Velocity;

		// Token: 0x040017A5 RID: 6053
		[Tooltip("whether or not to clear impulses (stopping camera shakes) when the Stop method is called on that feedback")]
		public bool ClearImpulseOnStop;

		// Token: 0x040017A6 RID: 6054
		[Header("Gizmos")]
		[Tooltip("whether or not to draw gizmos to showcase the various distance properties of this feedback, when applicable. Dissipation distance in blue, impact radius in yellow.")]
		public bool DrawGizmos;
	}
}
