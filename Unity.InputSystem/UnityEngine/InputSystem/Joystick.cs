using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200003A RID: 58
	[InputControlLayout(stateType = typeof(JoystickState), isGenericTypeOfDevice = true)]
	public class Joystick : InputDevice
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00014B27 File Offset: 0x00012D27
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00014B2F File Offset: 0x00012D2F
		public ButtonControl trigger { get; protected set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00014B38 File Offset: 0x00012D38
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00014B40 File Offset: 0x00012D40
		public StickControl stick { get; protected set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00014B49 File Offset: 0x00012D49
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x00014B51 File Offset: 0x00012D51
		public AxisControl twist { get; protected set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00014B5A File Offset: 0x00012D5A
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x00014B62 File Offset: 0x00012D62
		public Vector2Control hatswitch { get; protected set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00014B6B File Offset: 0x00012D6B
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x00014B72 File Offset: 0x00012D72
		public static Joystick current { get; private set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00014B7A File Offset: 0x00012D7A
		public new static ReadOnlyArray<Joystick> all
		{
			get
			{
				return new ReadOnlyArray<Joystick>(Joystick.s_Joysticks, 0, Joystick.s_JoystickCount);
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00014B8C File Offset: 0x00012D8C
		protected override void FinishSetup()
		{
			this.trigger = base.GetChildControl<ButtonControl>("{PrimaryTrigger}");
			this.stick = base.GetChildControl<StickControl>("{Primary2DMotion}");
			this.twist = base.TryGetChildControl<AxisControl>("{Twist}");
			this.hatswitch = base.TryGetChildControl<Vector2Control>("{Hatswitch}");
			base.FinishSetup();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00014BE3 File Offset: 0x00012DE3
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Joystick.current = this;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00014BF1 File Offset: 0x00012DF1
		protected override void OnAdded()
		{
			ArrayHelpers.AppendWithCapacity<Joystick>(ref Joystick.s_Joysticks, ref Joystick.s_JoystickCount, this, 10);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00014C08 File Offset: 0x00012E08
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Joystick.current == this)
			{
				Joystick.current = null;
			}
			int num = Joystick.s_Joysticks.IndexOfReference(this, Joystick.s_JoystickCount);
			if (num != -1)
			{
				Joystick.s_Joysticks.EraseAtWithCapacity<Joystick>(ref Joystick.s_JoystickCount, num);
			}
		}

		// Token: 0x0400018A RID: 394
		private static int s_JoystickCount;

		// Token: 0x0400018B RID: 395
		private static Joystick[] s_Joysticks;
	}
}
