using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000259 RID: 601
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Various/MMWiggle")]
	public class MMWiggle : MonoBehaviour
	{
		// Token: 0x06001096 RID: 4246 RVA: 0x0004A144 File Offset: 0x00048344
		public virtual void WigglePosition(float duration)
		{
			this.WiggleValue(ref this.PositionWiggleProperties, ref this._positionInternalProperties, duration);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0004A159 File Offset: 0x00048359
		public virtual void WiggleRotation(float duration)
		{
			this.WiggleValue(ref this.RotationWiggleProperties, ref this._rotationInternalProperties, duration);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0004A16E File Offset: 0x0004836E
		public virtual void WiggleScale(float duration)
		{
			this.WiggleValue(ref this.ScaleWiggleProperties, ref this._scaleInternalProperties, duration);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0004A183 File Offset: 0x00048383
		protected virtual void WiggleValue(ref WiggleProperties property, ref InternalWiggleProperties internalProperties, float duration)
		{
			this.InitializeRandomValues(ref property, ref internalProperties);
			internalProperties.limitedTimeValueSave = internalProperties.initialValue;
			property.LimitedTime = true;
			property.LimitedTimeLeft = duration;
			property.LimitedTimeTotal = duration;
			property.WigglePermitted = true;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0004A1B9 File Offset: 0x000483B9
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0004A1C4 File Offset: 0x000483C4
		public virtual void Initialization()
		{
			this._positionInternalProperties.initialValue = base.transform.localPosition;
			this._positionInternalProperties.startValue = base.transform.localPosition;
			this._rotationInternalProperties.initialValue = base.transform.localEulerAngles;
			this._rotationInternalProperties.startValue = base.transform.localEulerAngles;
			this._scaleInternalProperties.initialValue = base.transform.localScale;
			this._scaleInternalProperties.startValue = base.transform.localScale;
			this.InitializeRandomValues(ref this.PositionWiggleProperties, ref this._positionInternalProperties);
			this.InitializeRandomValues(ref this.RotationWiggleProperties, ref this._rotationInternalProperties);
			this.InitializeRandomValues(ref this.ScaleWiggleProperties, ref this._scaleInternalProperties);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0004A28C File Offset: 0x0004848C
		protected virtual void InitializeRandomValues(ref WiggleProperties properties, ref InternalWiggleProperties internalProperties)
		{
			internalProperties.newValue = internalProperties.initialValue;
			internalProperties.timeSinceLastChange = 0f;
			internalProperties.returnVector = Vector3.zero;
			internalProperties.randomFrequency = Random.Range(properties.FrequencyMin, properties.FrequencyMax);
			internalProperties.randomNoiseFrequency = Vector3.zero;
			internalProperties.randomAmplitude = Vector3.zero;
			internalProperties.timeSinceLastPause = 0f;
			internalProperties.pauseDuration = 0f;
			internalProperties.noiseElapsedTime = 0f;
			internalProperties.curveDirection = 1f;
			properties.LimitedTimeLeft = properties.LimitedTimeTotal;
			this.RandomizeVector3(ref internalProperties.randomAmplitude, properties.AmplitudeMin, properties.AmplitudeMax);
			this.RandomizeVector3(ref internalProperties.randomNoiseFrequency, properties.NoiseFrequencyMin, properties.NoiseFrequencyMax);
			this.RandomizeVector3(ref internalProperties.randomNoiseShift, properties.NoiseShiftMin, properties.NoiseShiftMax);
			this.RandomizeVector3(ref internalProperties.remapZero, properties.RemapCurveZeroMin, properties.RemapCurveZeroMax);
			this.RandomizeVector3(ref internalProperties.remapOne, properties.RemapCurveOneMin, properties.RemapCurveOneMax);
			if (properties.ForceVectorLength)
			{
				internalProperties.randomAmplitude = internalProperties.randomAmplitude.normalized * properties.ForcedVectorLength;
			}
			internalProperties.newValue = this.DetermineNewValue(properties, internalProperties.newValue, internalProperties.initialValue, ref internalProperties.startValue, ref internalProperties.randomAmplitude, ref internalProperties.randomFrequency, ref internalProperties.pauseDuration, true);
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0004A404 File Offset: 0x00048604
		protected virtual void Update()
		{
			if (this.UpdateMode == MMWiggle.UpdateModes.Update)
			{
				this.ProcessUpdate();
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0004A414 File Offset: 0x00048614
		protected virtual void LateUpdate()
		{
			if (this.UpdateMode == MMWiggle.UpdateModes.LateUpdate)
			{
				this.ProcessUpdate();
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0004A425 File Offset: 0x00048625
		protected virtual void FixedUpdate()
		{
			if (this.UpdateMode == MMWiggle.UpdateModes.FixedUpdate)
			{
				this.ProcessUpdate();
			}
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0004A438 File Offset: 0x00048638
		protected virtual void ProcessUpdate()
		{
			this._positionInternalProperties.returnVector = base.transform.localPosition;
			if (this.UpdateValue(this.PositionActive, this.PositionWiggleProperties, ref this._positionInternalProperties))
			{
				base.transform.localPosition = this._positionInternalProperties.returnVector;
			}
			this._rotationInternalProperties.returnVector = base.transform.localEulerAngles;
			if (this.UpdateValue(this.RotationActive, this.RotationWiggleProperties, ref this._rotationInternalProperties))
			{
				base.transform.localEulerAngles = this._rotationInternalProperties.returnVector;
			}
			this._scaleInternalProperties.returnVector = base.transform.localScale;
			if (this.UpdateValue(this.ScaleActive, this.ScaleWiggleProperties, ref this._scaleInternalProperties))
			{
				base.transform.localScale = this._scaleInternalProperties.returnVector;
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0004A518 File Offset: 0x00048718
		protected virtual bool UpdateValue(bool valueActive, WiggleProperties properties, ref InternalWiggleProperties internalProperties)
		{
			if (!valueActive)
			{
				return false;
			}
			if (!properties.WigglePermitted)
			{
				return false;
			}
			if (properties.LimitedTime && properties.LimitedTimeTotal > 0f)
			{
				float limitedTimeLeft = properties.LimitedTimeLeft;
				properties.LimitedTimeLeft -= properties.GetDeltaTime();
				if (properties.LimitedTimeLeft <= 0f)
				{
					if (limitedTimeLeft > 0f && properties.LimitedTimeResetValue)
					{
						internalProperties.returnVector = internalProperties.limitedTimeValueSave;
						properties.LimitedTimeLeft = 0f;
						properties.WigglePermitted = false;
						return true;
					}
					return false;
				}
			}
			switch (properties.WiggleType)
			{
			case WiggleTypes.Random:
				return this.MoveVector3TowardsTarget(ref internalProperties.returnVector, properties, ref internalProperties.startValue, internalProperties.initialValue, ref internalProperties.newValue, ref internalProperties.timeSinceLastPause, ref internalProperties.timeSinceLastChange, ref internalProperties.randomAmplitude, ref internalProperties.randomFrequency, ref internalProperties.pauseDuration, internalProperties.randomFrequency);
			case WiggleTypes.PingPong:
				return this.MoveVector3TowardsTarget(ref internalProperties.returnVector, properties, ref internalProperties.startValue, internalProperties.initialValue, ref internalProperties.newValue, ref internalProperties.timeSinceLastPause, ref internalProperties.timeSinceLastChange, ref internalProperties.randomAmplitude, ref internalProperties.randomFrequency, ref internalProperties.pauseDuration, internalProperties.randomFrequency);
			case WiggleTypes.Noise:
				internalProperties.returnVector = this.AnimateNoiseValue(ref internalProperties, properties);
				return true;
			case WiggleTypes.Curve:
				internalProperties.returnVector = this.AnimateCurveValue(ref internalProperties, properties);
				return true;
			default:
				return false;
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0004A670 File Offset: 0x00048870
		protected float ApplyFalloff(WiggleProperties properties)
		{
			float num = 1f;
			if (properties.LimitedTime && properties.LimitedTimeTotal > 0f)
			{
				float num2 = (properties.LimitedTimeTotal - properties.LimitedTimeLeft) / properties.LimitedTimeTotal;
				num = properties.LimitedTimeFalloff.Evaluate(num2);
			}
			return num;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0004A6BC File Offset: 0x000488BC
		protected virtual Vector3 AnimateNoiseValue(ref InternalWiggleProperties internalProperties, WiggleProperties properties)
		{
			internalProperties.noiseElapsedTime += properties.GetDeltaTime();
			internalProperties.newValue.x = (Mathf.PerlinNoise(internalProperties.randomNoiseFrequency.x * internalProperties.noiseElapsedTime, internalProperties.randomNoiseShift.x) * 2f - 1f) * internalProperties.randomAmplitude.x;
			internalProperties.newValue.y = (Mathf.PerlinNoise(internalProperties.randomNoiseFrequency.y * internalProperties.noiseElapsedTime, internalProperties.randomNoiseShift.y) * 2f - 1f) * internalProperties.randomAmplitude.y;
			internalProperties.newValue.z = (Mathf.PerlinNoise(internalProperties.randomNoiseFrequency.z * internalProperties.noiseElapsedTime, internalProperties.randomNoiseShift.z) * 2f - 1f) * internalProperties.randomAmplitude.z;
			internalProperties.newValue *= this.ApplyFalloff(properties);
			if (properties.RelativeAmplitude)
			{
				internalProperties.newValue += internalProperties.initialValue;
			}
			if (properties.UniformValues)
			{
				internalProperties.newValue.y = internalProperties.newValue.x;
				internalProperties.newValue.z = internalProperties.newValue.x;
			}
			return internalProperties.newValue;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0004A824 File Offset: 0x00048A24
		protected virtual Vector3 AnimateCurveValue(ref InternalWiggleProperties internalProperties, WiggleProperties properties)
		{
			internalProperties.timeSinceLastPause += properties.GetDeltaTime();
			internalProperties.timeSinceLastChange += properties.GetDeltaTime();
			if (internalProperties.timeSinceLastPause < internalProperties.pauseDuration)
			{
				float num = ((internalProperties.curveDirection == 1f) ? 1f : 0f);
				this.EvaluateCurve(properties.Curve, num, internalProperties.remapZero, internalProperties.remapOne, ref internalProperties.newValue, properties);
				if (properties.RelativeCurveAmplitude)
				{
					internalProperties.newValue += internalProperties.initialValue;
				}
			}
			if (internalProperties.timeSinceLastPause == internalProperties.timeSinceLastChange)
			{
				internalProperties.timeSinceLastChange = 0f;
			}
			if (internalProperties.randomFrequency > 0f)
			{
				float num2 = internalProperties.timeSinceLastChange / internalProperties.randomFrequency;
				if (internalProperties.curveDirection < 0f)
				{
					num2 = 1f - num2;
				}
				this.EvaluateCurve(properties.Curve, num2, internalProperties.remapZero, internalProperties.remapOne, ref internalProperties.newValue, properties);
				if (internalProperties.timeSinceLastChange > internalProperties.randomFrequency)
				{
					internalProperties.timeSinceLastChange = 0f;
					internalProperties.timeSinceLastPause = 0f;
					if (properties.CurvePingPong)
					{
						internalProperties.curveDirection = -internalProperties.curveDirection;
					}
					this.RandomizeFloat(ref internalProperties.randomFrequency, properties.FrequencyMin, properties.FrequencyMax);
				}
			}
			if (properties.RelativeCurveAmplitude)
			{
				internalProperties.newValue = internalProperties.initialValue + internalProperties.newValue;
			}
			return internalProperties.newValue;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0004A9A0 File Offset: 0x00048BA0
		protected virtual void EvaluateCurve(AnimationCurve curve, float percent, Vector3 remapMin, Vector3 remapMax, ref Vector3 returnValue, WiggleProperties properties)
		{
			returnValue.x = MMFeedbacksHelpers.Remap(curve.Evaluate(percent), 0f, 1f, remapMin.x, remapMax.x);
			returnValue.y = MMFeedbacksHelpers.Remap(curve.Evaluate(percent), 0f, 1f, remapMin.y, remapMax.y);
			returnValue.z = MMFeedbacksHelpers.Remap(curve.Evaluate(percent), 0f, 1f, remapMin.z, remapMax.z);
			returnValue *= this.ApplyFalloff(properties);
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0004AA48 File Offset: 0x00048C48
		protected virtual bool MoveVector3TowardsTarget(ref Vector3 movedValue, WiggleProperties properties, ref Vector3 startValue, Vector3 initialValue, ref Vector3 destinationValue, ref float timeSinceLastPause, ref float timeSinceLastValueChange, ref Vector3 randomAmplitude, ref float randomFrequency, ref float pauseDuration, float frequency)
		{
			timeSinceLastPause += properties.GetDeltaTime();
			timeSinceLastValueChange += properties.GetDeltaTime();
			if (timeSinceLastPause < pauseDuration)
			{
				return false;
			}
			if (timeSinceLastPause == timeSinceLastValueChange)
			{
				timeSinceLastValueChange = 0f;
			}
			if (frequency > 0f)
			{
				float num = timeSinceLastValueChange / frequency;
				if (!properties.UseSpeedCurve)
				{
					movedValue = Vector3.Lerp(startValue, destinationValue, num);
				}
				else
				{
					float num2 = properties.SpeedCurve.Evaluate(num);
					movedValue = Vector3.LerpUnclamped(startValue, destinationValue, num2);
				}
				if (timeSinceLastValueChange > frequency)
				{
					timeSinceLastValueChange = 0f;
					timeSinceLastPause = 0f;
					movedValue = destinationValue;
					destinationValue = this.DetermineNewValue(properties, movedValue, initialValue, ref startValue, ref randomAmplitude, ref randomFrequency, ref pauseDuration, false);
				}
			}
			return true;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0004AB28 File Offset: 0x00048D28
		protected virtual Vector3 DetermineNewValue(WiggleProperties properties, Vector3 newValue, Vector3 initialValue, ref Vector3 startValue, ref Vector3 randomAmplitude, ref float randomFrequency, ref float pauseDuration, bool firstPlay = false)
		{
			WiggleTypes wiggleType = properties.WiggleType;
			if (wiggleType == WiggleTypes.Random)
			{
				startValue = newValue;
				this.RandomizeFloat(ref randomFrequency, properties.FrequencyMin, properties.FrequencyMax);
				this.RandomizeVector3(ref randomAmplitude, properties.AmplitudeMin, properties.AmplitudeMax);
				this.RandomizeFloat(ref pauseDuration, properties.PauseMin, properties.PauseMax);
				newValue = randomAmplitude;
				if (properties.UniformValues)
				{
					newValue.y = newValue.x;
					newValue.z = newValue.x;
				}
				newValue *= this.ApplyFalloff(properties);
				if (properties.RelativeAmplitude)
				{
					newValue += initialValue;
				}
				return newValue;
			}
			if (wiggleType == WiggleTypes.PingPong)
			{
				if (properties.RelativeAmplitude)
				{
					if (firstPlay)
					{
						startValue = properties.AmplitudeMin * this.ApplyFalloff(properties) + initialValue;
						newValue = properties.AmplitudeMax * this.ApplyFalloff(properties) + initialValue;
					}
					else if (newValue == properties.AmplitudeMin + initialValue)
					{
						startValue = newValue;
						newValue = properties.AmplitudeMax * this.ApplyFalloff(properties) + initialValue;
					}
					else
					{
						startValue = newValue;
						newValue = properties.AmplitudeMin * this.ApplyFalloff(properties) + initialValue;
					}
				}
				else if (firstPlay)
				{
					startValue = properties.AmplitudeMin * this.ApplyFalloff(properties);
					newValue = properties.AmplitudeMax * this.ApplyFalloff(properties);
				}
				else
				{
					startValue = newValue;
					newValue = ((newValue == properties.AmplitudeMin) ? (properties.AmplitudeMax * this.ApplyFalloff(properties)) : properties.AmplitudeMin);
				}
				this.RandomizeFloat(ref randomFrequency, properties.FrequencyMin, properties.FrequencyMax);
				this.RandomizeFloat(ref pauseDuration, properties.PauseMin, properties.PauseMax);
				if (properties.UniformValues)
				{
					newValue.y = newValue.x;
					newValue.z = newValue.x;
				}
				return newValue;
			}
			return Vector3.zero;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0004AD42 File Offset: 0x00048F42
		protected virtual float RandomizeFloat(ref float randomizedFloat, float floatMin, float floatMax)
		{
			randomizedFloat = Random.Range(floatMin, floatMax);
			return randomizedFloat;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0004AD50 File Offset: 0x00048F50
		protected virtual Vector3 RandomizeVector3(ref Vector3 randomizedVector, Vector3 vectorMin, Vector3 vectorMax)
		{
			randomizedVector.x = Random.Range(vectorMin.x, vectorMax.x);
			randomizedVector.y = Random.Range(vectorMin.y, vectorMax.y);
			randomizedVector.z = Random.Range(vectorMin.z, vectorMax.z);
			return randomizedVector;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0004ADA8 File Offset: 0x00048FA8
		public virtual void RestoreInitialValues()
		{
			base.transform.localPosition = this._positionInternalProperties.initialValue;
			base.transform.localEulerAngles = this._rotationInternalProperties.initialValue;
			base.transform.localScale = this._scaleInternalProperties.initialValue;
		}

		// Token: 0x0400139F RID: 5023
		[Tooltip("the selected update mode")]
		public MMWiggle.UpdateModes UpdateMode;

		// Token: 0x040013A0 RID: 5024
		[Tooltip("whether or not position wiggle is active")]
		public bool PositionActive;

		// Token: 0x040013A1 RID: 5025
		[Tooltip("whether or not rotation wiggle is active")]
		public bool RotationActive;

		// Token: 0x040013A2 RID: 5026
		[Tooltip("whether or not scale wiggle is active")]
		public bool ScaleActive;

		// Token: 0x040013A3 RID: 5027
		[Tooltip("all public info related to position wiggling")]
		public WiggleProperties PositionWiggleProperties;

		// Token: 0x040013A4 RID: 5028
		[Tooltip("all public info related to rotation wiggling")]
		public WiggleProperties RotationWiggleProperties;

		// Token: 0x040013A5 RID: 5029
		[Tooltip("all public info related to scale wiggling")]
		public WiggleProperties ScaleWiggleProperties;

		// Token: 0x040013A6 RID: 5030
		[Tooltip("a debug duration used in conjunction with the debug buttons")]
		public float DebugWiggleDuration = 2f;

		// Token: 0x040013A7 RID: 5031
		protected InternalWiggleProperties _positionInternalProperties;

		// Token: 0x040013A8 RID: 5032
		protected InternalWiggleProperties _rotationInternalProperties;

		// Token: 0x040013A9 RID: 5033
		protected InternalWiggleProperties _scaleInternalProperties;

		// Token: 0x02000526 RID: 1318
		public enum UpdateModes
		{
			// Token: 0x040023A5 RID: 9125
			Update,
			// Token: 0x040023A6 RID: 9126
			FixedUpdate,
			// Token: 0x040023A7 RID: 9127
			LateUpdate
		}
	}
}
