using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000021 RID: 33
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineBasicMultiChannelPerlin : CinemachineComponentBase
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000C1DE File Offset: 0x0000A3DE
		public override bool IsValid
		{
			get
			{
				return base.enabled && this.m_NoiseProfile != null;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000C1F6 File Offset: 0x0000A3F6
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Noise;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (!this.IsValid || deltaTime < 0f)
			{
				this.mInitialized = false;
				return;
			}
			if (!this.mInitialized)
			{
				this.Initialize();
			}
			if (TargetPositionCache.CacheMode == TargetPositionCache.Mode.Playback && TargetPositionCache.HasCurrentTime)
			{
				this.mNoiseTime = TargetPositionCache.CurrentTime * this.m_FrequencyGain;
			}
			else
			{
				this.mNoiseTime += deltaTime * this.m_FrequencyGain;
			}
			curState.PositionCorrection += curState.CorrectedOrientation * NoiseSettings.GetCombinedFilterResults(this.m_NoiseProfile.PositionNoise, this.mNoiseTime, this.mNoiseOffsets) * this.m_AmplitudeGain;
			Quaternion quaternion = Quaternion.Euler(NoiseSettings.GetCombinedFilterResults(this.m_NoiseProfile.OrientationNoise, this.mNoiseTime, this.mNoiseOffsets) * this.m_AmplitudeGain);
			if (this.m_PivotOffset != Vector3.zero)
			{
				Matrix4x4 matrix4x = Matrix4x4.Translate(-this.m_PivotOffset);
				matrix4x = Matrix4x4.Rotate(quaternion) * matrix4x;
				matrix4x = Matrix4x4.Translate(this.m_PivotOffset) * matrix4x;
				curState.PositionCorrection += curState.CorrectedOrientation * matrix4x.MultiplyPoint(Vector3.zero);
			}
			curState.OrientationCorrection *= quaternion;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000C35D File Offset: 0x0000A55D
		public void ReSeed()
		{
			this.mNoiseOffsets = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000C397 File Offset: 0x0000A597
		private void Initialize()
		{
			this.mInitialized = true;
			this.mNoiseTime = CinemachineCore.CurrentTime * this.m_FrequencyGain;
			if (this.mNoiseOffsets == Vector3.zero)
			{
				this.ReSeed();
			}
		}

		// Token: 0x040000FF RID: 255
		[Tooltip("The asset containing the Noise Profile.  Define the frequencies and amplitudes there to make a characteristic noise profile.  Make your own or just use one of the many presets.")]
		[FormerlySerializedAs("m_Definition")]
		[NoiseSettingsProperty]
		public NoiseSettings m_NoiseProfile;

		// Token: 0x04000100 RID: 256
		[Tooltip("When rotating the camera, offset the camera's pivot position by this much (camera space)")]
		public Vector3 m_PivotOffset = Vector3.zero;

		// Token: 0x04000101 RID: 257
		[Tooltip("Gain to apply to the amplitudes defined in the NoiseSettings asset.  1 is normal.  Setting this to 0 completely mutes the noise.")]
		public float m_AmplitudeGain = 1f;

		// Token: 0x04000102 RID: 258
		[Tooltip("Scale factor to apply to the frequencies defined in the NoiseSettings asset.  1 is normal.  Larger magnitudes will make the noise shake more rapidly.")]
		public float m_FrequencyGain = 1f;

		// Token: 0x04000103 RID: 259
		private bool mInitialized;

		// Token: 0x04000104 RID: 260
		private float mNoiseTime;

		// Token: 0x04000105 RID: 261
		[SerializeField]
		[HideInInspector]
		private Vector3 mNoiseOffsets = Vector3.zero;
	}
}
