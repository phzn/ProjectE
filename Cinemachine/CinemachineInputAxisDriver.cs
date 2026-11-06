using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public struct CinemachineInputAxisDriver
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x00012553 File Offset: 0x00010753
		public void Validate()
		{
			this.accelTime = Mathf.Max(0f, this.accelTime);
			this.decelTime = Mathf.Max(0f, this.decelTime);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00012584 File Offset: 0x00010784
		public bool Update(float deltaTime, ref AxisBase axis)
		{
			if (!string.IsNullOrEmpty(this.name))
			{
				try
				{
					this.inputValue = CinemachineCore.GetInputAxis(this.name);
				}
				catch (ArgumentException)
				{
				}
			}
			float num = this.inputValue * this.multiplier;
			if (deltaTime < 0.0001f)
			{
				this.mCurrentSpeed = 0f;
			}
			else
			{
				float num2 = num / deltaTime;
				float num3 = ((Mathf.Abs(num2) < Mathf.Abs(this.mCurrentSpeed)) ? this.decelTime : this.accelTime);
				num2 = this.mCurrentSpeed + Damper.Damp(num2 - this.mCurrentSpeed, num3, deltaTime);
				this.mCurrentSpeed = num2;
				float num4 = axis.m_MaxValue - axis.m_MinValue;
				if (!axis.m_Wrap && this.decelTime > 0.0001f && num4 > 0.0001f)
				{
					float num5 = this.ClampValue(ref axis, axis.m_Value);
					float num6 = this.ClampValue(ref axis, num5 + num2 * deltaTime);
					if (((num2 > 0f) ? (axis.m_MaxValue - num6) : (num6 - axis.m_MinValue)) < 0.1f * num4 && Mathf.Abs(num2) > 0.0001f)
					{
						num2 = Damper.Damp(num6 - num5, this.decelTime, deltaTime) / deltaTime;
					}
				}
				num = num2 * deltaTime;
			}
			axis.m_Value = this.ClampValue(ref axis, axis.m_Value + num);
			return Mathf.Abs(this.inputValue) > 0.0001f;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000126F0 File Offset: 0x000108F0
		public bool Update(float deltaTime, ref AxisState axis)
		{
			AxisBase axisBase = new AxisBase
			{
				m_Value = axis.Value,
				m_MinValue = axis.m_MinValue,
				m_MaxValue = axis.m_MaxValue,
				m_Wrap = axis.m_Wrap
			};
			bool flag = this.Update(deltaTime, ref axisBase);
			axis.Value = axisBase.m_Value;
			return flag;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00012750 File Offset: 0x00010950
		private float ClampValue(ref AxisBase axis, float v)
		{
			float num = axis.m_MaxValue - axis.m_MinValue;
			if (axis.m_Wrap && num > 0.0001f)
			{
				v = (v - axis.m_MinValue) % num;
				v += axis.m_MinValue + ((v < 0f) ? num : 0f);
			}
			return Mathf.Clamp(v, axis.m_MinValue, axis.m_MaxValue);
		}

		// Token: 0x040001E5 RID: 485
		[Tooltip("Multiply the input by this amount prior to processing.  Controls the input power.")]
		public float multiplier;

		// Token: 0x040001E6 RID: 486
		[Tooltip("The amount of time in seconds it takes to accelerate to a higher speed")]
		public float accelTime;

		// Token: 0x040001E7 RID: 487
		[Tooltip("The amount of time in seconds it takes to decelerate to a lower speed")]
		public float decelTime;

		// Token: 0x040001E8 RID: 488
		[Tooltip("The name of this axis as specified in Unity Input manager. Setting to an empty string will disable the automatic updating of this axis")]
		public string name;

		// Token: 0x040001E9 RID: 489
		[NoSaveDuringPlay]
		[Tooltip("The value of the input axis.  A value of 0 means no input.  You can drive this directly from a custom input system, or you can set the Axis Name and have the value driven by the internal Input Manager")]
		public float inputValue;

		// Token: 0x040001EA RID: 490
		private float mCurrentSpeed;

		// Token: 0x040001EB RID: 491
		private const float Epsilon = 0.0001f;
	}
}
