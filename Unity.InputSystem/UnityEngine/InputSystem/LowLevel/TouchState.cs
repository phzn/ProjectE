using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000DC RID: 220
	[StructLayout(LayoutKind.Explicit, Size = 56)]
	public struct TouchState : IInputStateTypeInfo
	{
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00044AD7 File Offset: 0x00042CD7
		public static FourCC Format
		{
			get
			{
				return new FourCC('T', 'O', 'U', 'C');
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x00044AE6 File Offset: 0x00042CE6
		// (set) Token: 0x06000D70 RID: 3440 RVA: 0x00044AEE File Offset: 0x00042CEE
		public TouchPhase phase
		{
			get
			{
				return (TouchPhase)this.phaseId;
			}
			set
			{
				this.phaseId = (byte)value;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x00044AF8 File Offset: 0x00042CF8
		public bool isNoneEndedOrCanceled
		{
			get
			{
				return this.phase == TouchPhase.None || this.phase == TouchPhase.Ended || this.phase == TouchPhase.Canceled;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00044B16 File Offset: 0x00042D16
		public bool isInProgress
		{
			get
			{
				return this.phase == TouchPhase.Began || this.phase == TouchPhase.Moved || this.phase == TouchPhase.Stationary;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x00044B35 File Offset: 0x00042D35
		// (set) Token: 0x06000D74 RID: 3444 RVA: 0x00044B42 File Offset: 0x00042D42
		public bool isPrimaryTouch
		{
			get
			{
				return (this.flags & 8) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 8;
					return;
				}
				this.flags &= 247;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x00044B6A File Offset: 0x00042D6A
		// (set) Token: 0x06000D76 RID: 3446 RVA: 0x00044B78 File Offset: 0x00042D78
		internal bool isOrphanedPrimaryTouch
		{
			get
			{
				return (this.flags & 64) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 64;
					return;
				}
				this.flags &= 191;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00044BA1 File Offset: 0x00042DA1
		// (set) Token: 0x06000D78 RID: 3448 RVA: 0x00044BAE File Offset: 0x00042DAE
		public bool isIndirectTouch
		{
			get
			{
				return (this.flags & 1) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 1;
					return;
				}
				this.flags &= 254;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x00044BD6 File Offset: 0x00042DD6
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x00044BDE File Offset: 0x00042DDE
		public bool isTap
		{
			get
			{
				return this.isTapPress;
			}
			set
			{
				this.isTapPress = value;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00044BE7 File Offset: 0x00042DE7
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x00044BF5 File Offset: 0x00042DF5
		internal bool isTapPress
		{
			get
			{
				return (this.flags & 16) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 16;
					return;
				}
				this.flags &= 239;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00044C1E File Offset: 0x00042E1E
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x00044C2C File Offset: 0x00042E2C
		internal bool isTapRelease
		{
			get
			{
				return (this.flags & 32) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 32;
					return;
				}
				this.flags &= 223;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00044C55 File Offset: 0x00042E55
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x00044C66 File Offset: 0x00042E66
		internal bool beganInSameFrame
		{
			get
			{
				return (this.flags & 128) > 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 128;
					return;
				}
				this.flags &= 127;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00044C8F File Offset: 0x00042E8F
		public FourCC format
		{
			get
			{
				return TouchState.Format;
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00044C98 File Offset: 0x00042E98
		public override string ToString()
		{
			return string.Format("{{ id={0} phase={1} pos={2} delta={3} pressure={4} radius={5} primary={6} }}", new object[] { this.touchId, this.phase, this.position, this.delta, this.pressure, this.radius, this.isPrimaryTouch });
		}

		// Token: 0x04000582 RID: 1410
		internal const int kSizeInBytes = 56;

		// Token: 0x04000583 RID: 1411
		[InputControl(displayName = "Touch ID", layout = "Integer", synthetic = true, dontReset = true)]
		[FieldOffset(0)]
		public int touchId;

		// Token: 0x04000584 RID: 1412
		[InputControl(displayName = "Position", dontReset = true)]
		[FieldOffset(4)]
		public Vector2 position;

		// Token: 0x04000585 RID: 1413
		[InputControl(displayName = "Delta", layout = "Delta")]
		[FieldOffset(12)]
		public Vector2 delta;

		// Token: 0x04000586 RID: 1414
		[InputControl(displayName = "Pressure", layout = "Axis")]
		[FieldOffset(20)]
		public float pressure;

		// Token: 0x04000587 RID: 1415
		[InputControl(displayName = "Radius")]
		[FieldOffset(24)]
		public Vector2 radius;

		// Token: 0x04000588 RID: 1416
		[InputControl(name = "phase", displayName = "Touch Phase", layout = "TouchPhase", synthetic = true)]
		[InputControl(name = "press", displayName = "Touch Contact?", layout = "TouchPress", useStateFrom = "phase")]
		[FieldOffset(32)]
		public byte phaseId;

		// Token: 0x04000589 RID: 1417
		[InputControl(name = "tapCount", displayName = "Tap Count", layout = "Integer")]
		[FieldOffset(33)]
		public byte tapCount;

		// Token: 0x0400058A RID: 1418
		[InputControl(name = "displayIndex", displayName = "Display Index", layout = "Integer")]
		[FieldOffset(34)]
		public byte displayIndex;

		// Token: 0x0400058B RID: 1419
		[InputControl(name = "indirectTouch", displayName = "Indirect Touch?", layout = "Button", bit = 0U, synthetic = true)]
		[InputControl(name = "tap", displayName = "Tap", layout = "Button", bit = 4U)]
		[FieldOffset(35)]
		public byte flags;

		// Token: 0x0400058C RID: 1420
		[FieldOffset(36)]
		internal uint updateStepCount;

		// Token: 0x0400058D RID: 1421
		[InputControl(displayName = "Start Time", layout = "Double", synthetic = true)]
		[FieldOffset(40)]
		public double startTime;

		// Token: 0x0400058E RID: 1422
		[InputControl(displayName = "Start Position", synthetic = true)]
		[FieldOffset(48)]
		public Vector2 startPosition;
	}
}
