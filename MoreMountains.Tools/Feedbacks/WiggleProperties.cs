using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000257 RID: 599
	[Serializable]
	public class WiggleProperties
	{
		// Token: 0x06001093 RID: 4243 RVA: 0x00049FD8 File Offset: 0x000481D8
		public float GetDeltaTime()
		{
			return (this.UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * this.TimeMultiplier;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00049FF5 File Offset: 0x000481F5
		public float GetTime()
		{
			return (this.UseUnscaledTime ? Time.unscaledTime : Time.time) * this.TimeMultiplier;
		}

		// Token: 0x0400136C RID: 4972
		[Header("Status")]
		public bool WigglePermitted = true;

		// Token: 0x0400136D RID: 4973
		[Header("Type")]
		[Tooltip("the position mode : none, random or ping pong - none won't do anything, random will randomize min and max bounds, ping pong will oscillate between min and max bounds")]
		public WiggleTypes WiggleType = WiggleTypes.Random;

		// Token: 0x0400136E RID: 4974
		[Tooltip("if this is true, unscaled delta time, otherwise regular delta time")]
		public bool UseUnscaledTime;

		// Token: 0x0400136F RID: 4975
		[Tooltip("a multiplier to apply to all time related operations, allowing you to speed up or slow down the wiggle")]
		public float TimeMultiplier = 1f;

		// Token: 0x04001370 RID: 4976
		[Tooltip("whether or not this object should start wiggling automatically on Start()")]
		public bool StartWigglingAutomatically = true;

		// Token: 0x04001371 RID: 4977
		[Tooltip("if this is true, position will be ping ponged with an ease in/out curve")]
		public bool SmoothPingPong = true;

		// Token: 0x04001372 RID: 4978
		[Header("Speed")]
		[Tooltip("Whether or not the position's speed curve will be used")]
		public bool UseSpeedCurve;

		// Token: 0x04001373 RID: 4979
		[Tooltip("an animation curve to define the speed over time from one position to the other (x), and the actual position (y), allowing for overshoot")]
		public AnimationCurve SpeedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001374 RID: 4980
		[Header("Frequency")]
		[Tooltip("the minimum time (in seconds) between two position changes")]
		public float FrequencyMin;

		// Token: 0x04001375 RID: 4981
		[Tooltip("the maximum time (in seconds) between two position changes")]
		public float FrequencyMax = 1f;

		// Token: 0x04001376 RID: 4982
		[Header("Amplitude")]
		[Tooltip("the minimum position the object can have")]
		public Vector3 AmplitudeMin = Vector3.zero;

		// Token: 0x04001377 RID: 4983
		[Tooltip("the maximum position the object can have")]
		public Vector3 AmplitudeMax = Vector3.one;

		// Token: 0x04001378 RID: 4984
		[Tooltip("if this is true, amplitude will be relative, otherwise world space")]
		public bool RelativeAmplitude = true;

		// Token: 0x04001379 RID: 4985
		[Tooltip("if this is true, all amplitude values will match the x amplitude value")]
		public bool UniformValues;

		// Token: 0x0400137A RID: 4986
		[Tooltip("if this is true, when randomizing amplitude, the resulting vector's length will be forced to match ForcedVectorLength")]
		public bool ForceVectorLength;

		// Token: 0x0400137B RID: 4987
		[Tooltip("the length of the randomized amplitude if ForceVectorLength is true")]
		[MMCondition("ForceVectorLength", true)]
		public float ForcedVectorLength = 1f;

		// Token: 0x0400137C RID: 4988
		[Header("Curve")]
		[Tooltip("a curve to animate this property on")]
		public AnimationCurve Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400137D RID: 4989
		[Tooltip("the minimum value to randomize the curve's zero remap to")]
		public Vector3 RemapCurveZeroMin = Vector3.zero;

		// Token: 0x0400137E RID: 4990
		[Tooltip("the maximum value to randomize the curve's zero remap to")]
		public Vector3 RemapCurveZeroMax = Vector3.zero;

		// Token: 0x0400137F RID: 4991
		[Tooltip("the minimum value to randomize the curve's one remap to")]
		public Vector3 RemapCurveOneMin = Vector3.one;

		// Token: 0x04001380 RID: 4992
		[Tooltip("the maximum value to randomize the curve's one remap to")]
		public Vector3 RemapCurveOneMax = Vector3.one;

		// Token: 0x04001381 RID: 4993
		[Tooltip("whether or not to add the initial value of this property to the curve's outcome")]
		public bool RelativeCurveAmplitude = true;

		// Token: 0x04001382 RID: 4994
		[Tooltip("whether or not the curve should be read from left to right, then right to left")]
		public bool CurvePingPong;

		// Token: 0x04001383 RID: 4995
		[Header("Pause")]
		[Tooltip("the minimum time to spend between two random positions")]
		public float PauseMin;

		// Token: 0x04001384 RID: 4996
		[Tooltip("the maximum time to spend between two random positions")]
		public float PauseMax;

		// Token: 0x04001385 RID: 4997
		[Header("Limited Time")]
		[Tooltip("if this is true, this property will only animate for the specified time")]
		public bool LimitedTime;

		// Token: 0x04001386 RID: 4998
		[Tooltip("the maximum time left")]
		public float LimitedTimeTotal;

		// Token: 0x04001387 RID: 4999
		[Tooltip("the animation curve to use to decrease the effect of the wiggle as time goes")]
		public AnimationCurve LimitedTimeFalloff = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x04001388 RID: 5000
		[Tooltip("if this is true, original position will be restored when time left reaches zero")]
		public bool LimitedTimeResetValue = true;

		// Token: 0x04001389 RID: 5001
		[Tooltip("the actual time left")]
		[MMFReadOnly]
		public float LimitedTimeLeft;

		// Token: 0x0400138A RID: 5002
		[Header("Noise Frequency")]
		[Tooltip("the minimum time between two changes of noise frequency")]
		public Vector3 NoiseFrequencyMin = Vector3.zero;

		// Token: 0x0400138B RID: 5003
		[Tooltip("the maximum time between two changes of noise frequency")]
		public Vector3 NoiseFrequencyMax = Vector3.one;

		// Token: 0x0400138C RID: 5004
		[Header("Noise Shift")]
		[Tooltip("how much the noise should be shifted at minimum")]
		public Vector3 NoiseShiftMin = Vector3.zero;

		// Token: 0x0400138D RID: 5005
		[Tooltip("how much the noise should be shifted at maximum")]
		public Vector3 NoiseShiftMax = Vector3.zero;
	}
}
