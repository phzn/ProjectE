using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000DE RID: 222
	[StructLayout(LayoutKind.Explicit, Size = 37)]
	internal struct ActionEvent : IInputEventTypeInfo
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00044D66 File Offset: 0x00042F66
		public static FourCC Type
		{
			get
			{
				return new FourCC('A', 'C', 'T', 'N');
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00044D75 File Offset: 0x00042F75
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x00044D7D File Offset: 0x00042F7D
		public double startTime
		{
			get
			{
				return this.m_StartTime;
			}
			set
			{
				this.m_StartTime = value;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00044D86 File Offset: 0x00042F86
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x00044D8E File Offset: 0x00042F8E
		public InputActionPhase phase
		{
			get
			{
				return (InputActionPhase)this.m_Phase;
			}
			set
			{
				this.m_Phase = (byte)value;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00044D98 File Offset: 0x00042F98
		public unsafe byte* valueData
		{
			get
			{
				fixed (byte* ptr = &this.m_ValueData.FixedElementField)
				{
					return ptr;
				}
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00044DB3 File Offset: 0x00042FB3
		public int valueSizeInBytes
		{
			get
			{
				return (int)(this.baseEvent.sizeInBytes - 20U - 16U);
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00044DC6 File Offset: 0x00042FC6
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x00044DCE File Offset: 0x00042FCE
		public int stateIndex
		{
			get
			{
				return (int)this.m_StateIndex;
			}
			set
			{
				if (value < 0 || value > 255)
				{
					throw new NotSupportedException("State count cannot exceed byte.MaxValue");
				}
				this.m_StateIndex = (byte)value;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00044DEF File Offset: 0x00042FEF
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x00044DF7 File Offset: 0x00042FF7
		public int controlIndex
		{
			get
			{
				return (int)this.m_ControlIndex;
			}
			set
			{
				if (value < 0 || value > 65535)
				{
					throw new NotSupportedException("Control count cannot exceed ushort.MaxValue");
				}
				this.m_ControlIndex = (ushort)value;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00044E18 File Offset: 0x00043018
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x00044E20 File Offset: 0x00043020
		public int bindingIndex
		{
			get
			{
				return (int)this.m_BindingIndex;
			}
			set
			{
				if (value < 0 || value > 65535)
				{
					throw new NotSupportedException("Binding count cannot exceed ushort.MaxValue");
				}
				this.m_BindingIndex = (ushort)value;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00044E41 File Offset: 0x00043041
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x00044E58 File Offset: 0x00043058
		public int interactionIndex
		{
			get
			{
				if (this.m_InteractionIndex == 65535)
				{
					return -1;
				}
				return (int)this.m_InteractionIndex;
			}
			set
			{
				if (value == -1)
				{
					this.m_InteractionIndex = ushort.MaxValue;
					return;
				}
				if (value < 0 || value >= 65535)
				{
					throw new NotSupportedException("Interaction count cannot exceed ushort.MaxValue-1");
				}
				this.m_InteractionIndex = (ushort)value;
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00044E8C File Offset: 0x0004308C
		public unsafe InputEventPtr ToEventPtr()
		{
			fixed (ActionEvent* ptr = &this)
			{
				return new InputEventPtr((InputEvent*)ptr);
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00044EA2 File Offset: 0x000430A2
		public FourCC typeStatic
		{
			get
			{
				return ActionEvent.Type;
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00044EA9 File Offset: 0x000430A9
		public static int GetEventSizeWithValueSize(int valueSizeInBytes)
		{
			return 36 + valueSizeInBytes;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00044EB0 File Offset: 0x000430B0
		public unsafe static ActionEvent* From(InputEventPtr ptr)
		{
			if (!ptr.valid)
			{
				throw new ArgumentNullException("ptr");
			}
			if (!ptr.IsA<ActionEvent>())
			{
				throw new InvalidCastException(string.Format("Cannot cast event with type '{0}' into ActionEvent", ptr.type));
			}
			return (ActionEvent*)ptr.data;
		}

		// Token: 0x04000593 RID: 1427
		[FieldOffset(0)]
		public InputEvent baseEvent;

		// Token: 0x04000594 RID: 1428
		[FieldOffset(20)]
		private ushort m_ControlIndex;

		// Token: 0x04000595 RID: 1429
		[FieldOffset(22)]
		private ushort m_BindingIndex;

		// Token: 0x04000596 RID: 1430
		[FieldOffset(24)]
		private ushort m_InteractionIndex;

		// Token: 0x04000597 RID: 1431
		[FieldOffset(26)]
		private byte m_StateIndex;

		// Token: 0x04000598 RID: 1432
		[FieldOffset(27)]
		private byte m_Phase;

		// Token: 0x04000599 RID: 1433
		[FieldOffset(28)]
		private double m_StartTime;

		// Token: 0x0400059A RID: 1434
		[FixedBuffer(typeof(byte), 1)]
		[FieldOffset(36)]
		public ActionEvent.<m_ValueData>e__FixedBuffer m_ValueData;

		// Token: 0x02000212 RID: 530
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct <m_ValueData>e__FixedBuffer
		{
			// Token: 0x04000B95 RID: 2965
			public byte FixedElementField;
		}
	}
}
