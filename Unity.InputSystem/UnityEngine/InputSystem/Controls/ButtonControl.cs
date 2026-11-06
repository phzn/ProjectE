using System;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000116 RID: 278
	public class ButtonControl : AxisControl
	{
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x0004E636 File Offset: 0x0004C836
		// (set) Token: 0x0600100F RID: 4111 RVA: 0x0004E63E File Offset: 0x0004C83E
		internal bool needsToCheckFramePress { get; private set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x0004E647 File Offset: 0x0004C847
		public float pressPointOrDefault
		{
			get
			{
				if (this.pressPoint <= 0f)
				{
					return ButtonControl.s_GlobalDefaultButtonPressPoint;
				}
				return this.pressPoint;
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0004E664 File Offset: 0x0004C864
		public ButtonControl()
		{
			this.m_StateBlock.format = InputStateBlock.FormatBit;
			this.m_MinValue = 0f;
			this.m_MaxValue = 1f;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0004E6C0 File Offset: 0x0004C8C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new bool IsValueConsideredPressed(float value)
		{
			return value >= this.pressPointOrDefault;
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x0004E6CE File Offset: 0x0004C8CE
		public unsafe bool isPressed
		{
			get
			{
				if (!this.needsToCheckFramePress)
				{
					return this.IsValueConsideredPressed(*base.value);
				}
				return this.m_LastUpdateWasPress;
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0004E6EC File Offset: 0x0004C8EC
		private void BeginTestingForFramePresses(bool currentlyPressed, bool pressedLastFrame)
		{
			this.needsToCheckFramePress = true;
			base.device.m_ButtonControlsCheckingPressState.Add(this);
			this.m_LastUpdateWasPress = currentlyPressed;
			if (currentlyPressed && !pressedLastFrame)
			{
				this.m_UpdateCountLastPressed = base.device.m_CurrentUpdateStepCount;
				return;
			}
			if (pressedLastFrame && !currentlyPressed)
			{
				this.m_UpdateCountLastReleased = base.device.m_CurrentUpdateStepCount;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x0004E748 File Offset: 0x0004C948
		public unsafe bool wasPressedThisFrame
		{
			get
			{
				if (!this.needsToCheckFramePress)
				{
					bool flag = this.IsValueConsideredPressed(*base.value);
					bool flag2 = this.IsValueConsideredPressed(base.ReadValueFromPreviousFrame());
					this.BeginTestingForFramePresses(flag, flag2);
					return base.device.wasUpdatedThisFrame && flag && !flag2;
				}
				return InputUpdate.s_UpdateStepCount == this.m_UpdateCountLastPressed;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x0004E7A4 File Offset: 0x0004C9A4
		public unsafe bool wasReleasedThisFrame
		{
			get
			{
				if (!this.needsToCheckFramePress)
				{
					bool flag = this.IsValueConsideredPressed(*base.value);
					bool flag2 = this.IsValueConsideredPressed(base.ReadValueFromPreviousFrame());
					this.BeginTestingForFramePresses(flag, flag2);
					return base.device.wasUpdatedThisFrame && !flag && flag2;
				}
				return InputUpdate.s_UpdateStepCount == this.m_UpdateCountLastReleased;
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0004E800 File Offset: 0x0004CA00
		internal unsafe void UpdateWasPressed()
		{
			bool flag = this.IsValueConsideredPressed(*base.value);
			if (this.m_LastUpdateWasPress != flag)
			{
				if (flag)
				{
					this.m_UpdateCountLastPressed = base.device.m_CurrentUpdateStepCount;
				}
				else
				{
					this.m_UpdateCountLastReleased = base.device.m_CurrentUpdateStepCount;
				}
				this.m_LastUpdateWasPress = flag;
			}
		}

		// Token: 0x040006B1 RID: 1713
		private bool m_NeedsToCheckFramePress;

		// Token: 0x040006B2 RID: 1714
		private uint m_UpdateCountLastPressed = uint.MaxValue;

		// Token: 0x040006B3 RID: 1715
		private uint m_UpdateCountLastReleased = uint.MaxValue;

		// Token: 0x040006B4 RID: 1716
		private bool m_LastUpdateWasPress;

		// Token: 0x040006B6 RID: 1718
		public float pressPoint = -1f;

		// Token: 0x040006B7 RID: 1719
		internal static float s_GlobalDefaultButtonPressPoint;

		// Token: 0x040006B8 RID: 1720
		internal static float s_GlobalDefaultButtonReleaseThreshold;

		// Token: 0x040006B9 RID: 1721
		internal const float kMinButtonPressPoint = 0.0001f;
	}
}
