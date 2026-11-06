using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000049 RID: 73
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineNoiseProfiles.html")]
	public sealed class NoiseSettings : SignalSourceAsset
	{
		// Token: 0x0600033F RID: 831 RVA: 0x000147C0 File Offset: 0x000129C0
		public static Vector3 GetCombinedFilterResults(NoiseSettings.TransformNoiseParams[] noiseParams, float time, Vector3 timeOffsets)
		{
			Vector3 vector = Vector3.zero;
			if (noiseParams != null)
			{
				for (int i = 0; i < noiseParams.Length; i++)
				{
					vector += noiseParams[i].GetValueAt(time, timeOffsets);
				}
			}
			return vector;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000340 RID: 832 RVA: 0x000147FA File Offset: 0x000129FA
		public override float SignalDuration
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00014801 File Offset: 0x00012A01
		public override void GetSignal(float timeSinceSignalStart, out Vector3 pos, out Quaternion rot)
		{
			pos = NoiseSettings.GetCombinedFilterResults(this.PositionNoise, timeSinceSignalStart, Vector3.zero);
			rot = Quaternion.Euler(NoiseSettings.GetCombinedFilterResults(this.OrientationNoise, timeSinceSignalStart, Vector3.zero));
		}

		// Token: 0x0400022A RID: 554
		[Tooltip("These are the noise channels for the virtual camera's position. Convincing noise setups typically mix low, medium and high frequencies together, so start with a size of 3")]
		[FormerlySerializedAs("m_Position")]
		public NoiseSettings.TransformNoiseParams[] PositionNoise = Array.Empty<NoiseSettings.TransformNoiseParams>();

		// Token: 0x0400022B RID: 555
		[Tooltip("These are the noise channels for the virtual camera's orientation. Convincing noise setups typically mix low, medium and high frequencies together, so start with a size of 3")]
		[FormerlySerializedAs("m_Orientation")]
		public NoiseSettings.TransformNoiseParams[] OrientationNoise = Array.Empty<NoiseSettings.TransformNoiseParams>();

		// Token: 0x020000BA RID: 186
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct NoiseParams
		{
			// Token: 0x06000470 RID: 1136 RVA: 0x00019AA0 File Offset: 0x00017CA0
			public float GetValueAt(float time, float timeOffset)
			{
				float num = this.Frequency * time + timeOffset;
				if (this.Constant)
				{
					return Mathf.Cos(num * 2f * 3.1415927f) * this.Amplitude * 0.5f;
				}
				return (Mathf.PerlinNoise(num, 0f) - 0.5f) * this.Amplitude;
			}

			// Token: 0x040003BB RID: 955
			[Tooltip("The frequency of noise for this channel.  Higher magnitudes vibrate faster.")]
			public float Frequency;

			// Token: 0x040003BC RID: 956
			[Tooltip("The amplitude of the noise for this channel.  Larger numbers vibrate higher.")]
			public float Amplitude;

			// Token: 0x040003BD RID: 957
			[Tooltip("If checked, then the amplitude and frequency will not be randomized.")]
			public bool Constant;
		}

		// Token: 0x020000BB RID: 187
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct TransformNoiseParams
		{
			// Token: 0x06000471 RID: 1137 RVA: 0x00019AF8 File Offset: 0x00017CF8
			public Vector3 GetValueAt(float time, Vector3 timeOffsets)
			{
				return new Vector3(this.X.GetValueAt(time, timeOffsets.x), this.Y.GetValueAt(time, timeOffsets.y), this.Z.GetValueAt(time, timeOffsets.z));
			}

			// Token: 0x040003BE RID: 958
			[Tooltip("Noise definition for X-axis")]
			public NoiseSettings.NoiseParams X;

			// Token: 0x040003BF RID: 959
			[Tooltip("Noise definition for Y-axis")]
			public NoiseSettings.NoiseParams Y;

			// Token: 0x040003C0 RID: 960
			[Tooltip("Noise definition for Z-axis")]
			public NoiseSettings.NoiseParams Z;
		}
	}
}
