using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000245 RID: 581
	public class MMCameraShakerRotation : MMCameraShaker
	{
		// Token: 0x06001003 RID: 4099 RVA: 0x00047268 File Offset: 0x00045468
		public override void ShakeCamera(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool useUnscaledTime)
		{
			if (amplitudeX != 0f || amplitudeY != 0f || amplitudeZ != 0f)
			{
				this._wiggle.RotationWiggleProperties.AmplitudeMin.x = -amplitudeX;
				this._wiggle.RotationWiggleProperties.AmplitudeMin.y = -amplitudeY;
				this._wiggle.RotationWiggleProperties.AmplitudeMin.z = -amplitudeZ;
				this._wiggle.RotationWiggleProperties.AmplitudeMax.x = amplitudeX;
				this._wiggle.RotationWiggleProperties.AmplitudeMax.y = amplitudeY;
				this._wiggle.RotationWiggleProperties.AmplitudeMax.z = amplitudeZ;
			}
			else
			{
				this._wiggle.RotationWiggleProperties.AmplitudeMin = Vector3.one * -amplitude;
				this._wiggle.RotationWiggleProperties.AmplitudeMax = Vector3.one * amplitude;
			}
			this._wiggle.RotationWiggleProperties.UseUnscaledTime = useUnscaledTime;
			this._wiggle.RotationWiggleProperties.FrequencyMin = frequency;
			this._wiggle.RotationWiggleProperties.FrequencyMax = frequency;
			this._wiggle.RotationWiggleProperties.NoiseFrequencyMin = frequency * Vector3.one;
			this._wiggle.RotationWiggleProperties.NoiseFrequencyMax = frequency * Vector3.one;
			this._wiggle.WiggleRotation(duration);
		}
	}
}
