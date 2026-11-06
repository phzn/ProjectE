using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x0200002C RID: 44
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[Serializable]
	public struct AxisState
	{
		// Token: 0x0600020A RID: 522 RVA: 0x00010054 File Offset: 0x0000E254
		public AxisState(float minValue, float maxValue, bool wrap, bool rangeLocked, float maxSpeed, float accelTime, float decelTime, string name, bool invert)
		{
			this.m_MinValue = minValue;
			this.m_MaxValue = maxValue;
			this.m_Wrap = wrap;
			this.ValueRangeLocked = rangeLocked;
			this.HasRecentering = false;
			this.m_Recentering = new AxisState.Recentering(false, 1f, 2f);
			this.m_SpeedMode = AxisState.SpeedMode.MaxSpeed;
			this.m_MaxSpeed = maxSpeed;
			this.m_AccelTime = accelTime;
			this.m_DecelTime = decelTime;
			this.Value = (minValue + maxValue) / 2f;
			this.m_InputAxisName = name;
			this.m_InputAxisValue = 0f;
			this.m_InvertInput = invert;
			this.m_CurrentSpeed = 0f;
			this.m_InputAxisProvider = null;
			this.m_InputAxisIndex = 0;
			this.m_LastUpdateTime = 0f;
			this.m_LastUpdateFrame = 0;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00010110 File Offset: 0x0000E310
		public void Validate()
		{
			if (this.m_SpeedMode == AxisState.SpeedMode.MaxSpeed)
			{
				this.m_MaxSpeed = Mathf.Max(0f, this.m_MaxSpeed);
			}
			this.m_AccelTime = Mathf.Max(0f, this.m_AccelTime);
			this.m_DecelTime = Mathf.Max(0f, this.m_DecelTime);
			this.m_MaxValue = Mathf.Clamp(this.m_MaxValue, this.m_MinValue, this.m_MaxValue);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00010184 File Offset: 0x0000E384
		public void Reset()
		{
			this.m_InputAxisValue = 0f;
			this.m_CurrentSpeed = 0f;
			this.m_LastUpdateTime = 0f;
			this.m_LastUpdateFrame = 0;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000101AE File Offset: 0x0000E3AE
		public void SetInputAxisProvider(int axis, AxisState.IInputAxisProvider provider)
		{
			this.m_InputAxisIndex = axis;
			this.m_InputAxisProvider = provider;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600020E RID: 526 RVA: 0x000101BE File Offset: 0x0000E3BE
		public bool HasInputProvider
		{
			get
			{
				return this.m_InputAxisProvider != null;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000101CC File Offset: 0x0000E3CC
		public bool Update(float deltaTime)
		{
			if (Time.frameCount == this.m_LastUpdateFrame)
			{
				return false;
			}
			this.m_LastUpdateFrame = Time.frameCount;
			if (deltaTime > 0f && this.m_LastUpdateTime != 0f)
			{
				deltaTime = Time.realtimeSinceStartup - this.m_LastUpdateTime;
			}
			this.m_LastUpdateTime = Time.realtimeSinceStartup;
			if (this.m_InputAxisProvider != null)
			{
				this.m_InputAxisValue = this.m_InputAxisProvider.GetAxisValue(this.m_InputAxisIndex);
			}
			else if (!string.IsNullOrEmpty(this.m_InputAxisName))
			{
				try
				{
					this.m_InputAxisValue = CinemachineCore.GetInputAxis(this.m_InputAxisName);
				}
				catch (ArgumentException ex)
				{
					Debug.LogError(ex.ToString());
				}
			}
			float num = this.m_InputAxisValue;
			if (this.m_InvertInput)
			{
				num *= -1f;
			}
			if (this.m_SpeedMode == AxisState.SpeedMode.MaxSpeed)
			{
				return this.MaxSpeedUpdate(num, deltaTime);
			}
			num *= this.m_MaxSpeed;
			if (deltaTime < 0f)
			{
				this.m_CurrentSpeed = 0f;
			}
			else if (deltaTime > 0.0001f)
			{
				float num2 = ((Mathf.Abs(num) < Mathf.Abs(this.m_CurrentSpeed)) ? this.m_DecelTime : this.m_AccelTime);
				this.m_CurrentSpeed += Damper.Damp(num - this.m_CurrentSpeed, num2, deltaTime);
				float num3 = this.m_MaxValue - this.m_MinValue;
				if (!this.m_Wrap && this.m_DecelTime > 0.0001f && num3 > 0.0001f)
				{
					float num4 = this.ClampValue(this.Value);
					float num5 = this.ClampValue(num4 + this.m_CurrentSpeed * deltaTime);
					if (((this.m_CurrentSpeed > 0f) ? (this.m_MaxValue - num5) : (num5 - this.m_MinValue)) < 0.1f * num3 && Mathf.Abs(this.m_CurrentSpeed) > 0.0001f)
					{
						this.m_CurrentSpeed = Damper.Damp(num5 - num4, this.m_DecelTime, deltaTime) / deltaTime;
					}
				}
				num = this.m_CurrentSpeed * deltaTime;
			}
			this.Value = this.ClampValue(this.Value + this.m_CurrentSpeed);
			return Mathf.Abs(num) > 0.0001f;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000103E8 File Offset: 0x0000E5E8
		private float ClampValue(float v)
		{
			float num = this.m_MaxValue - this.m_MinValue;
			if (this.m_Wrap && num > 0.0001f)
			{
				v = (v - this.m_MinValue) % num;
				v += this.m_MinValue + ((v < 0f) ? num : 0f);
			}
			return Mathf.Clamp(v, this.m_MinValue, this.m_MaxValue);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0001044C File Offset: 0x0000E64C
		private bool MaxSpeedUpdate(float input, float deltaTime)
		{
			if (this.m_MaxSpeed > 0.0001f)
			{
				float num = input * this.m_MaxSpeed;
				if (Mathf.Abs(num) < 0.0001f || (Mathf.Sign(this.m_CurrentSpeed) == Mathf.Sign(num) && Mathf.Abs(num) < Mathf.Abs(this.m_CurrentSpeed)))
				{
					float num2 = Mathf.Min(Mathf.Abs(num - this.m_CurrentSpeed) / Mathf.Max(0.0001f, this.m_DecelTime) * deltaTime, Mathf.Abs(this.m_CurrentSpeed));
					this.m_CurrentSpeed -= Mathf.Sign(this.m_CurrentSpeed) * num2;
				}
				else
				{
					float num3 = Mathf.Abs(num - this.m_CurrentSpeed) / Mathf.Max(0.0001f, this.m_AccelTime);
					this.m_CurrentSpeed += Mathf.Sign(num) * num3 * deltaTime;
					if (Mathf.Sign(this.m_CurrentSpeed) == Mathf.Sign(num) && Mathf.Abs(this.m_CurrentSpeed) > Mathf.Abs(num))
					{
						this.m_CurrentSpeed = num;
					}
				}
			}
			float maxSpeed = this.GetMaxSpeed();
			this.m_CurrentSpeed = Mathf.Clamp(this.m_CurrentSpeed, -maxSpeed, maxSpeed);
			if (Mathf.Abs(this.m_CurrentSpeed) < 0.0001f)
			{
				this.m_CurrentSpeed = 0f;
			}
			this.Value += this.m_CurrentSpeed * deltaTime;
			if (this.Value > this.m_MaxValue || this.Value < this.m_MinValue)
			{
				if (this.m_Wrap)
				{
					if (this.Value > this.m_MaxValue)
					{
						this.Value = this.m_MinValue + (this.Value - this.m_MaxValue);
					}
					else
					{
						this.Value = this.m_MaxValue + (this.Value - this.m_MinValue);
					}
				}
				else
				{
					this.Value = Mathf.Clamp(this.Value, this.m_MinValue, this.m_MaxValue);
					this.m_CurrentSpeed = 0f;
				}
			}
			return Mathf.Abs(input) > 0.0001f;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00010648 File Offset: 0x0000E848
		private float GetMaxSpeed()
		{
			float num = this.m_MaxValue - this.m_MinValue;
			if (!this.m_Wrap && num > 0f)
			{
				float num2 = num / 10f;
				if (this.m_CurrentSpeed > 0f && this.m_MaxValue - this.Value < num2)
				{
					float num3 = (this.m_MaxValue - this.Value) / num2;
					return Mathf.Lerp(0f, this.m_MaxSpeed, num3);
				}
				if (this.m_CurrentSpeed < 0f && this.Value - this.m_MinValue < num2)
				{
					float num4 = (this.Value - this.m_MinValue) / num2;
					return Mathf.Lerp(0f, this.m_MaxSpeed, num4);
				}
			}
			return this.m_MaxSpeed;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00010705 File Offset: 0x0000E905
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0001070D File Offset: 0x0000E90D
		public bool ValueRangeLocked { readonly get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00010716 File Offset: 0x0000E916
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0001071E File Offset: 0x0000E91E
		public bool HasRecentering { readonly get; set; }

		// Token: 0x0400018D RID: 397
		[NoSaveDuringPlay]
		[Tooltip("The current value of the axis.")]
		public float Value;

		// Token: 0x0400018E RID: 398
		[Tooltip("How to interpret the Max Speed setting: in units/second, or as a direct input value multiplier")]
		public AxisState.SpeedMode m_SpeedMode;

		// Token: 0x0400018F RID: 399
		[Tooltip("The maximum speed of this axis in units/second, or the input value multiplier, depending on the Speed Mode")]
		public float m_MaxSpeed;

		// Token: 0x04000190 RID: 400
		[Tooltip("The amount of time in seconds it takes to accelerate to MaxSpeed with the supplied Axis at its maximum value")]
		public float m_AccelTime;

		// Token: 0x04000191 RID: 401
		[Tooltip("The amount of time in seconds it takes to decelerate the axis to zero if the supplied axis is in a neutral position")]
		public float m_DecelTime;

		// Token: 0x04000192 RID: 402
		[FormerlySerializedAs("m_AxisName")]
		[Tooltip("The name of this axis as specified in Unity Input manager. Setting to an empty string will disable the automatic updating of this axis")]
		public string m_InputAxisName;

		// Token: 0x04000193 RID: 403
		[NoSaveDuringPlay]
		[Tooltip("The value of the input axis.  A value of 0 means no input.  You can drive this directly from a custom input system, or you can set the Axis Name and have the value driven by the internal Input Manager")]
		public float m_InputAxisValue;

		// Token: 0x04000194 RID: 404
		[FormerlySerializedAs("m_InvertAxis")]
		[Tooltip("If checked, then the raw value of the input axis will be inverted before it is used")]
		public bool m_InvertInput;

		// Token: 0x04000195 RID: 405
		[Tooltip("The minimum value for the axis")]
		public float m_MinValue;

		// Token: 0x04000196 RID: 406
		[Tooltip("The maximum value for the axis")]
		public float m_MaxValue;

		// Token: 0x04000197 RID: 407
		[Tooltip("If checked, then the axis will wrap around at the min/max values, forming a loop")]
		public bool m_Wrap;

		// Token: 0x04000198 RID: 408
		[Tooltip("Automatic recentering to at-rest position")]
		public AxisState.Recentering m_Recentering;

		// Token: 0x04000199 RID: 409
		private float m_CurrentSpeed;

		// Token: 0x0400019A RID: 410
		private float m_LastUpdateTime;

		// Token: 0x0400019B RID: 411
		private int m_LastUpdateFrame;

		// Token: 0x0400019C RID: 412
		private const float Epsilon = 0.0001f;

		// Token: 0x0400019D RID: 413
		private AxisState.IInputAxisProvider m_InputAxisProvider;

		// Token: 0x0400019E RID: 414
		private int m_InputAxisIndex;

		// Token: 0x020000A0 RID: 160
		public enum SpeedMode
		{
			// Token: 0x0400034F RID: 847
			MaxSpeed,
			// Token: 0x04000350 RID: 848
			InputValueGain
		}

		// Token: 0x020000A1 RID: 161
		public interface IInputAxisProvider
		{
			// Token: 0x06000448 RID: 1096
			float GetAxisValue(int axis);
		}

		// Token: 0x020000A2 RID: 162
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct Recentering
		{
			// Token: 0x06000449 RID: 1097 RVA: 0x0001914C File Offset: 0x0001734C
			public Recentering(bool enabled, float waitTime, float recenteringTime)
			{
				this.m_enabled = enabled;
				this.m_WaitTime = waitTime;
				this.m_RecenteringTime = recenteringTime;
				this.mLastAxisInputTime = 0f;
				this.mRecenteringVelocity = 0f;
				this.m_LegacyHeadingDefinition = (this.m_LegacyVelocityFilterStrength = -1);
				this.m_LastUpdateTime = 0f;
			}

			// Token: 0x0600044A RID: 1098 RVA: 0x0001919F File Offset: 0x0001739F
			public void Validate()
			{
				this.m_WaitTime = Mathf.Max(0f, this.m_WaitTime);
				this.m_RecenteringTime = Mathf.Max(0f, this.m_RecenteringTime);
			}

			// Token: 0x0600044B RID: 1099 RVA: 0x000191CD File Offset: 0x000173CD
			public void CopyStateFrom(ref AxisState.Recentering other)
			{
				if (this.mLastAxisInputTime != other.mLastAxisInputTime)
				{
					other.mRecenteringVelocity = 0f;
				}
				this.mLastAxisInputTime = other.mLastAxisInputTime;
			}

			// Token: 0x0600044C RID: 1100 RVA: 0x000191F4 File Offset: 0x000173F4
			public void CancelRecentering()
			{
				this.mLastAxisInputTime = Time.realtimeSinceStartup;
				this.mRecenteringVelocity = 0f;
			}

			// Token: 0x0600044D RID: 1101 RVA: 0x0001920C File Offset: 0x0001740C
			public void RecenterNow()
			{
				this.mLastAxisInputTime = -1f;
			}

			// Token: 0x0600044E RID: 1102 RVA: 0x0001921C File Offset: 0x0001741C
			public void DoRecentering(ref AxisState axis, float deltaTime, float recenterTarget)
			{
				if (deltaTime > 0f)
				{
					deltaTime = Time.realtimeSinceStartup - this.m_LastUpdateTime;
				}
				this.m_LastUpdateTime = Time.realtimeSinceStartup;
				if (!this.m_enabled && deltaTime >= 0f)
				{
					return;
				}
				recenterTarget = axis.ClampValue(recenterTarget);
				if (deltaTime < 0f)
				{
					this.CancelRecentering();
					if (this.m_enabled)
					{
						axis.Value = recenterTarget;
					}
					return;
				}
				float num = axis.ClampValue(axis.Value);
				float num2 = recenterTarget - num;
				if (num2 == 0f)
				{
					return;
				}
				if (this.mLastAxisInputTime >= 0f && Time.realtimeSinceStartup < this.mLastAxisInputTime + this.m_WaitTime)
				{
					return;
				}
				float num3 = axis.m_MaxValue - axis.m_MinValue;
				if (axis.m_Wrap && Mathf.Abs(num2) > num3 * 0.5f)
				{
					num += Mathf.Sign(recenterTarget - num) * num3;
				}
				if (this.m_RecenteringTime < 0.001f || Mathf.Abs(num - recenterTarget) < 0.001f)
				{
					num = recenterTarget;
				}
				else
				{
					num = Mathf.SmoothDamp(num, recenterTarget, ref this.mRecenteringVelocity, this.m_RecenteringTime, 9999f, deltaTime);
				}
				axis.Value = axis.ClampValue(num);
			}

			// Token: 0x0600044F RID: 1103 RVA: 0x00019338 File Offset: 0x00017538
			internal bool LegacyUpgrade(ref int heading, ref int velocityFilter)
			{
				if (this.m_LegacyHeadingDefinition != -1 && this.m_LegacyVelocityFilterStrength != -1)
				{
					heading = this.m_LegacyHeadingDefinition;
					velocityFilter = this.m_LegacyVelocityFilterStrength;
					this.m_LegacyHeadingDefinition = (this.m_LegacyVelocityFilterStrength = -1);
					return true;
				}
				return false;
			}

			// Token: 0x04000351 RID: 849
			[Tooltip("If checked, will enable automatic recentering of the axis. If unchecked, recenting is disabled.")]
			public bool m_enabled;

			// Token: 0x04000352 RID: 850
			[Tooltip("If no user input has been detected on the axis, the axis will wait this long in seconds before recentering.")]
			public float m_WaitTime;

			// Token: 0x04000353 RID: 851
			[Tooltip("How long it takes to reach destination once recentering has started.")]
			public float m_RecenteringTime;

			// Token: 0x04000354 RID: 852
			private float m_LastUpdateTime;

			// Token: 0x04000355 RID: 853
			private float mLastAxisInputTime;

			// Token: 0x04000356 RID: 854
			private float mRecenteringVelocity;

			// Token: 0x04000357 RID: 855
			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("m_HeadingDefinition")]
			private int m_LegacyHeadingDefinition;

			// Token: 0x04000358 RID: 856
			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("m_VelocityFilterStrength")]
			private int m_LegacyVelocityFilterStrength;
		}
	}
}
