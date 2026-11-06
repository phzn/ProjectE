using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000056 RID: 86
	[SaveDuringPlay]
	[AddComponentMenu("")]
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[ExecuteAlways]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineImpulseListener.html")]
	public class CinemachineImpulseListener : CinemachineExtension
	{
		// Token: 0x06000392 RID: 914 RVA: 0x000163D0 File Offset: 0x000145D0
		private void Reset()
		{
			this.m_ApplyAfter = CinemachineCore.Stage.Noise;
			this.m_ChannelMask = 1;
			this.m_Gain = 1f;
			this.m_Use2DDistance = false;
			this.m_UseCameraSpace = true;
			this.m_ReactionSettings = new CinemachineImpulseListener.ImpulseReaction
			{
				m_AmplitudeGain = 1f,
				m_FrequencyGain = 1f,
				m_Duration = 1f
			};
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00016438 File Offset: 0x00014638
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == this.m_ApplyAfter && deltaTime >= 0f)
			{
				Vector3 vector;
				Quaternion quaternion;
				bool impulseAt = CinemachineImpulseManager.Instance.GetImpulseAt(state.FinalPosition, this.m_Use2DDistance, this.m_ChannelMask, out vector, out quaternion);
				Vector3 vector2;
				Quaternion quaternion2;
				bool reaction = this.m_ReactionSettings.GetReaction(deltaTime, vector, out vector2, out quaternion2);
				if (impulseAt)
				{
					quaternion = Quaternion.SlerpUnclamped(Quaternion.identity, quaternion, this.m_Gain);
					vector *= this.m_Gain;
				}
				if (reaction)
				{
					vector += vector2;
					quaternion *= quaternion2;
				}
				if (impulseAt || reaction)
				{
					if (this.m_UseCameraSpace)
					{
						vector = state.RawOrientation * vector;
					}
					state.PositionCorrection += vector;
					state.OrientationCorrection *= quaternion;
				}
			}
		}

		// Token: 0x0400026F RID: 623
		[Tooltip("When to apply the impulse reaction.  Default is after the Noise stage.  Modify this if necessary to influence the ordering of extension effects")]
		public CinemachineCore.Stage m_ApplyAfter = CinemachineCore.Stage.Aim;

		// Token: 0x04000270 RID: 624
		[Tooltip("Impulse events on channels not included in the mask will be ignored.")]
		[CinemachineImpulseChannelProperty]
		public int m_ChannelMask;

		// Token: 0x04000271 RID: 625
		[Tooltip("Gain to apply to the Impulse signal.  1 is normal strength.  Setting this to 0 completely mutes the signal.")]
		public float m_Gain;

		// Token: 0x04000272 RID: 626
		[Tooltip("Enable this to perform distance calculation in 2D (ignore Z)")]
		public bool m_Use2DDistance;

		// Token: 0x04000273 RID: 627
		[Tooltip("Enable this to process all impulse signals in camera space")]
		public bool m_UseCameraSpace;

		// Token: 0x04000274 RID: 628
		[Tooltip("This controls the secondary reaction of the listener to the incoming impulse.  The impulse might be for example a sharp shock, and the secondary reaction could be a vibration whose amplitude and duration is controlled by the size of the original impulse.  This allows different listeners to respond in different ways to the same impulse signal.")]
		public CinemachineImpulseListener.ImpulseReaction m_ReactionSettings;

		// Token: 0x020000C9 RID: 201
		[Serializable]
		public struct ImpulseReaction
		{
			// Token: 0x0600048C RID: 1164 RVA: 0x0001A450 File Offset: 0x00018650
			public void ReSeed()
			{
				this.m_NoiseOffsets = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
			}

			// Token: 0x0600048D RID: 1165 RVA: 0x0001A48C File Offset: 0x0001868C
			public bool GetReaction(float deltaTime, Vector3 impulsePos, out Vector3 pos, out Quaternion rot)
			{
				if (!this.m_Initialized)
				{
					this.m_Initialized = true;
					this.m_CurrentAmount = 0f;
					this.m_CurrentDamping = 0f;
					this.m_CurrentTime = CinemachineCore.CurrentTime * this.m_FrequencyGain;
					if (this.m_NoiseOffsets == Vector3.zero)
					{
						this.ReSeed();
					}
				}
				pos = Vector3.zero;
				rot = Quaternion.identity;
				float sqrMagnitude = impulsePos.sqrMagnitude;
				if (this.m_SecondaryNoise == null || (sqrMagnitude < 0.001f && this.m_CurrentAmount < 0.0001f))
				{
					return false;
				}
				if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Playback && TargetPositionCache.HasCurrentTime)
				{
					this.m_CurrentTime = TargetPositionCache.CurrentTime * this.m_FrequencyGain;
				}
				else
				{
					this.m_CurrentTime += deltaTime * this.m_FrequencyGain;
				}
				this.m_CurrentAmount = Mathf.Max(this.m_CurrentAmount, Mathf.Sqrt(sqrMagnitude));
				this.m_CurrentDamping = Mathf.Max(this.m_CurrentDamping, Mathf.Max(1f, Mathf.Sqrt(this.m_CurrentAmount)) * this.m_Duration);
				float num = this.m_CurrentAmount * this.m_AmplitudeGain;
				pos = NoiseSettings.GetCombinedFilterResults(this.m_SecondaryNoise.PositionNoise, this.m_CurrentTime, this.m_NoiseOffsets) * num;
				rot = Quaternion.Euler(NoiseSettings.GetCombinedFilterResults(this.m_SecondaryNoise.OrientationNoise, this.m_CurrentTime, this.m_NoiseOffsets) * num);
				this.m_CurrentAmount -= Damper.Damp(this.m_CurrentAmount, this.m_CurrentDamping, deltaTime);
				this.m_CurrentDamping -= Damper.Damp(this.m_CurrentDamping, this.m_CurrentDamping, deltaTime);
				return true;
			}

			// Token: 0x040003F1 RID: 1009
			[Tooltip("Secondary shake that will be triggered by the primary impulse.")]
			[NoiseSettingsProperty]
			public NoiseSettings m_SecondaryNoise;

			// Token: 0x040003F2 RID: 1010
			[Tooltip("Gain to apply to the amplitudes defined in the signal source.  1 is normal.  Setting this to 0 completely mutes the signal.")]
			public float m_AmplitudeGain;

			// Token: 0x040003F3 RID: 1011
			[Tooltip("Scale factor to apply to the time axis.  1 is normal.  Larger magnitudes will make the signal progress more rapidly.")]
			public float m_FrequencyGain;

			// Token: 0x040003F4 RID: 1012
			[Tooltip("How long the secondary reaction lasts.")]
			public float m_Duration;

			// Token: 0x040003F5 RID: 1013
			private float m_CurrentAmount;

			// Token: 0x040003F6 RID: 1014
			private float m_CurrentTime;

			// Token: 0x040003F7 RID: 1015
			private float m_CurrentDamping;

			// Token: 0x040003F8 RID: 1016
			private bool m_Initialized;

			// Token: 0x040003F9 RID: 1017
			[SerializeField]
			[HideInInspector]
			private Vector3 m_NoiseOffsets;
		}
	}
}
