using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x0200011A RID: 282
	public class DpadControl : Vector2Control
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x0004EA9B File Offset: 0x0004CC9B
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x0004EAA3 File Offset: 0x0004CCA3
		[InputControl(name = "x", layout = "DpadAxis", useStateFrom = "right", synthetic = true)]
		[InputControl(name = "y", layout = "DpadAxis", useStateFrom = "up", synthetic = true)]
		[InputControl(bit = 0U, displayName = "Up")]
		public ButtonControl up { get; set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0004EAAC File Offset: 0x0004CCAC
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x0004EAB4 File Offset: 0x0004CCB4
		[InputControl(bit = 1U, displayName = "Down")]
		public ButtonControl down { get; set; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x0004EABD File Offset: 0x0004CCBD
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x0004EAC5 File Offset: 0x0004CCC5
		[InputControl(bit = 2U, displayName = "Left")]
		public ButtonControl left { get; set; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0004EACE File Offset: 0x0004CCCE
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x0004EAD6 File Offset: 0x0004CCD6
		[InputControl(bit = 3U, displayName = "Right")]
		public ButtonControl right { get; set; }

		// Token: 0x06001031 RID: 4145 RVA: 0x0004EADF File Offset: 0x0004CCDF
		public DpadControl()
		{
			this.m_StateBlock.sizeInBits = 4U;
			this.m_StateBlock.format = InputStateBlock.FormatBit;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0004EB04 File Offset: 0x0004CD04
		protected override void FinishSetup()
		{
			this.up = base.GetChildControl<ButtonControl>("up");
			this.down = base.GetChildControl<ButtonControl>("down");
			this.left = base.GetChildControl<ButtonControl>("left");
			this.right = base.GetChildControl<ButtonControl>("right");
			base.FinishSetup();
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0004EB5C File Offset: 0x0004CD5C
		public unsafe override Vector2 ReadUnprocessedValueFromState(void* statePtr)
		{
			bool flag = this.up.ReadValueFromStateWithCaching(statePtr) >= this.up.pressPointOrDefault;
			bool flag2 = this.down.ReadValueFromStateWithCaching(statePtr) >= this.down.pressPointOrDefault;
			bool flag3 = this.left.ReadValueFromStateWithCaching(statePtr) >= this.left.pressPointOrDefault;
			bool flag4 = this.right.ReadValueFromStateWithCaching(statePtr) >= this.right.pressPointOrDefault;
			return DpadControl.MakeDpadVector(flag, flag2, flag3, flag4, true);
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0004EBE8 File Offset: 0x0004CDE8
		public unsafe override void WriteValueIntoState(Vector2 value, void* statePtr)
		{
			bool flag = this.up.IsValueConsideredPressed(value.y);
			bool flag2 = this.down.IsValueConsideredPressed(value.y * -1f);
			bool flag3 = this.left.IsValueConsideredPressed(value.x * -1f);
			bool flag4 = this.right.IsValueConsideredPressed(value.x);
			this.up.WriteValueIntoState((flag && !flag2) ? value.y : 0f, statePtr);
			this.down.WriteValueIntoState((flag2 && !flag) ? (value.y * -1f) : 0f, statePtr);
			this.left.WriteValueIntoState((flag3 && !flag4) ? (value.x * -1f) : 0f, statePtr);
			this.right.WriteValueIntoState((flag4 && !flag3) ? value.x : 0f, statePtr);
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0004ECD4 File Offset: 0x0004CED4
		public static Vector2 MakeDpadVector(bool up, bool down, bool left, bool right, bool normalize = true)
		{
			float num = (up ? 1f : 0f);
			float num2 = (down ? (-1f) : 0f);
			float num3 = (left ? (-1f) : 0f);
			float num4 = (right ? 1f : 0f);
			Vector2 vector = new Vector2(num3 + num4, num + num2);
			if (normalize && vector.x != 0f && vector.y != 0f)
			{
				vector = new Vector2(vector.x * 0.707107f, vector.y * 0.707107f);
			}
			return vector;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0004ED71 File Offset: 0x0004CF71
		public static Vector2 MakeDpadVector(float up, float down, float left, float right)
		{
			return new Vector2(-left + right, up - down);
		}

		// Token: 0x0200023D RID: 573
		[InputControlLayout(hideInUI = true)]
		public class DpadAxisControl : AxisControl
		{
			// Token: 0x170005F2 RID: 1522
			// (get) Token: 0x060015FF RID: 5631 RVA: 0x0006570F File Offset: 0x0006390F
			// (set) Token: 0x06001600 RID: 5632 RVA: 0x00065717 File Offset: 0x00063917
			public int component { get; set; }

			// Token: 0x06001601 RID: 5633 RVA: 0x00065720 File Offset: 0x00063920
			protected override void FinishSetup()
			{
				base.FinishSetup();
				this.component = ((base.name == "x") ? 0 : 1);
				this.m_StateBlock = this.m_Parent.m_StateBlock;
			}

			// Token: 0x06001602 RID: 5634 RVA: 0x00065758 File Offset: 0x00063958
			public unsafe override float ReadUnprocessedValueFromState(void* statePtr)
			{
				return ((DpadControl)this.m_Parent).ReadUnprocessedValueFromState(statePtr)[this.component];
			}
		}

		// Token: 0x0200023E RID: 574
		internal enum ButtonBits
		{
			// Token: 0x04000C5B RID: 3163
			Up,
			// Token: 0x04000C5C RID: 3164
			Down,
			// Token: 0x04000C5D RID: 3165
			Left,
			// Token: 0x04000C5E RID: 3166
			Right
		}
	}
}
