using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200005B RID: 91
	[SaveDuringPlay]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/api/Cinemachine.CinemachineIndependentImpulseListener.html")]
	public class CinemachineIndependentImpulseListener : MonoBehaviour
	{
		// Token: 0x060003AA RID: 938 RVA: 0x000168C4 File Offset: 0x00014AC4
		private void Reset()
		{
			this.m_ChannelMask = 1;
			this.m_Gain = 1f;
			this.m_Use2DDistance = false;
			this.m_UseLocalSpace = true;
			this.m_ReactionSettings = new CinemachineImpulseListener.ImpulseReaction
			{
				m_AmplitudeGain = 1f,
				m_FrequencyGain = 1f,
				m_Duration = 1f
			};
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00016924 File Offset: 0x00014B24
		private void OnEnable()
		{
			this.impulsePosLastFrame = Vector3.zero;
			this.impulseRotLastFrame = Quaternion.identity;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001693C File Offset: 0x00014B3C
		private void Update()
		{
			base.transform.position -= this.impulsePosLastFrame;
			base.transform.rotation = base.transform.rotation * Quaternion.Inverse(this.impulseRotLastFrame);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001698C File Offset: 0x00014B8C
		private void LateUpdate()
		{
			bool impulseAt = CinemachineImpulseManager.Instance.GetImpulseAt(base.transform.position, this.m_Use2DDistance, this.m_ChannelMask, out this.impulsePosLastFrame, out this.impulseRotLastFrame);
			Vector3 vector;
			Quaternion quaternion;
			bool reaction = this.m_ReactionSettings.GetReaction(Time.deltaTime, this.impulsePosLastFrame, out vector, out quaternion);
			if (impulseAt)
			{
				this.impulseRotLastFrame = Quaternion.SlerpUnclamped(Quaternion.identity, this.impulseRotLastFrame, this.m_Gain);
				this.impulsePosLastFrame *= this.m_Gain;
			}
			if (reaction)
			{
				this.impulsePosLastFrame += vector;
				this.impulseRotLastFrame *= quaternion;
			}
			if (impulseAt || reaction)
			{
				if (this.m_UseLocalSpace)
				{
					this.impulsePosLastFrame = base.transform.rotation * this.impulsePosLastFrame;
				}
				base.transform.position += this.impulsePosLastFrame;
				base.transform.rotation = base.transform.rotation * this.impulseRotLastFrame;
			}
		}

		// Token: 0x0400027C RID: 636
		private Vector3 impulsePosLastFrame;

		// Token: 0x0400027D RID: 637
		private Quaternion impulseRotLastFrame;

		// Token: 0x0400027E RID: 638
		[Tooltip("Impulse events on channels not included in the mask will be ignored.")]
		[CinemachineImpulseChannelProperty]
		public int m_ChannelMask;

		// Token: 0x0400027F RID: 639
		[Tooltip("Gain to apply to the Impulse signal.  1 is normal strength.  Setting this to 0 completely mutes the signal.")]
		public float m_Gain;

		// Token: 0x04000280 RID: 640
		[Tooltip("Enable this to perform distance calculation in 2D (ignore Z)")]
		public bool m_Use2DDistance;

		// Token: 0x04000281 RID: 641
		[Tooltip("Enable this to process all impulse signals in camera space")]
		public bool m_UseLocalSpace;

		// Token: 0x04000282 RID: 642
		[Tooltip("This controls the secondary reaction of the listener to the incoming impulse.  The impulse might be for example a sharp shock, and the secondary reaction could be a vibration whose amplitude and duration is controlled by the size of the original impulse.  This allows different listeners to respond in different ways to the same impulse signal.")]
		public CinemachineImpulseListener.ImpulseReaction m_ReactionSettings;
	}
}
