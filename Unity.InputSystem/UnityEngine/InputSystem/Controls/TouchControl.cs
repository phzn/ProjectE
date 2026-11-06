using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x0200011F RID: 287
	[InputControlLayout(stateType = typeof(TouchState))]
	public class TouchControl : InputControl<TouchState>
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x0004F283 File Offset: 0x0004D483
		// (set) Token: 0x06001058 RID: 4184 RVA: 0x0004F28B File Offset: 0x0004D48B
		public TouchPressControl press { get; set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0004F294 File Offset: 0x0004D494
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x0004F29C File Offset: 0x0004D49C
		public IntegerControl displayIndex { get; set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0004F2A5 File Offset: 0x0004D4A5
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x0004F2AD File Offset: 0x0004D4AD
		public IntegerControl touchId { get; set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0004F2B6 File Offset: 0x0004D4B6
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x0004F2BE File Offset: 0x0004D4BE
		public Vector2Control position { get; set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0004F2C7 File Offset: 0x0004D4C7
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x0004F2CF File Offset: 0x0004D4CF
		public DeltaControl delta { get; set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x0004F2D8 File Offset: 0x0004D4D8
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x0004F2E0 File Offset: 0x0004D4E0
		public AxisControl pressure { get; set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0004F2E9 File Offset: 0x0004D4E9
		// (set) Token: 0x06001064 RID: 4196 RVA: 0x0004F2F1 File Offset: 0x0004D4F1
		public Vector2Control radius { get; set; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x0004F2FA File Offset: 0x0004D4FA
		// (set) Token: 0x06001066 RID: 4198 RVA: 0x0004F302 File Offset: 0x0004D502
		public TouchPhaseControl phase { get; set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x0004F30B File Offset: 0x0004D50B
		// (set) Token: 0x06001068 RID: 4200 RVA: 0x0004F313 File Offset: 0x0004D513
		public ButtonControl indirectTouch { get; set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x0004F31C File Offset: 0x0004D51C
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x0004F324 File Offset: 0x0004D524
		public ButtonControl tap { get; set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x0004F32D File Offset: 0x0004D52D
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x0004F335 File Offset: 0x0004D535
		public IntegerControl tapCount { get; set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0004F33E File Offset: 0x0004D53E
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x0004F346 File Offset: 0x0004D546
		public DoubleControl startTime { get; set; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0004F34F File Offset: 0x0004D54F
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x0004F357 File Offset: 0x0004D557
		public Vector2Control startPosition { get; set; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0004F360 File Offset: 0x0004D560
		public unsafe bool isInProgress
		{
			get
			{
				TouchPhase touchPhase = (TouchPhase)(*this.phase.value);
				return touchPhase - TouchPhase.Began <= 1 || touchPhase == TouchPhase.Stationary;
			}
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0004F387 File Offset: 0x0004D587
		public TouchControl()
		{
			this.m_StateBlock.format = new FourCC('T', 'O', 'U', 'C');
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0004F3A8 File Offset: 0x0004D5A8
		protected override void FinishSetup()
		{
			this.press = base.GetChildControl<TouchPressControl>("press");
			this.displayIndex = base.GetChildControl<IntegerControl>("displayIndex");
			this.touchId = base.GetChildControl<IntegerControl>("touchId");
			this.position = base.GetChildControl<Vector2Control>("position");
			this.delta = base.GetChildControl<DeltaControl>("delta");
			this.pressure = base.GetChildControl<AxisControl>("pressure");
			this.radius = base.GetChildControl<Vector2Control>("radius");
			this.phase = base.GetChildControl<TouchPhaseControl>("phase");
			this.indirectTouch = base.GetChildControl<ButtonControl>("indirectTouch");
			this.tap = base.GetChildControl<ButtonControl>("tap");
			this.tapCount = base.GetChildControl<IntegerControl>("tapCount");
			this.startTime = base.GetChildControl<DoubleControl>("startTime");
			this.startPosition = base.GetChildControl<Vector2Control>("startPosition");
			base.FinishSetup();
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0004F498 File Offset: 0x0004D698
		public unsafe override TouchState ReadUnprocessedValueFromState(void* statePtr)
		{
			TouchState* ptr = (TouchState*)((byte*)statePtr + this.m_StateBlock.byteOffset);
			return *ptr;
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0004F4BC File Offset: 0x0004D6BC
		public unsafe override void WriteValueIntoState(TouchState value, void* statePtr)
		{
			TouchState* ptr = (TouchState*)((byte*)statePtr + this.m_StateBlock.byteOffset);
			UnsafeUtility.MemCpy((void*)ptr, UnsafeUtility.AddressOf<TouchState>(ref value), (long)UnsafeUtility.SizeOf<TouchState>());
		}
	}
}
