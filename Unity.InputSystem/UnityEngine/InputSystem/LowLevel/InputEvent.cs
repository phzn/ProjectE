using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;
using UnityEngineInternal.Input;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E6 RID: 230
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 20)]
	public struct InputEvent
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x000452CD File Offset: 0x000434CD
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x000452DF File Offset: 0x000434DF
		public FourCC type
		{
			get
			{
				return new FourCC((int)this.m_Event.type);
			}
			set
			{
				this.m_Event.type = (NativeInputEventType)value;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x000452F2 File Offset: 0x000434F2
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x00045300 File Offset: 0x00043500
		public uint sizeInBytes
		{
			get
			{
				return (uint)this.m_Event.sizeInBytes;
			}
			set
			{
				if (value > 65535U)
				{
					throw new ArgumentException("Maximum event size is " + ushort.MaxValue.ToString(), "value");
				}
				this.m_Event.sizeInBytes = (ushort)value;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00045344 File Offset: 0x00043544
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x0004535A File Offset: 0x0004355A
		public int eventId
		{
			get
			{
				return (int)((long)this.m_Event.eventId & 2147483647L);
			}
			set
			{
				this.m_Event.eventId = value | (int)((long)this.m_Event.eventId & (long)((ulong)int.MinValue));
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0004537D File Offset: 0x0004357D
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x0004538A File Offset: 0x0004358A
		public int deviceId
		{
			get
			{
				return (int)this.m_Event.deviceId;
			}
			set
			{
				this.m_Event.deviceId = (ushort)value;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00045399 File Offset: 0x00043599
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x000453AC File Offset: 0x000435AC
		public double time
		{
			get
			{
				return this.m_Event.time - InputRuntime.s_CurrentTimeOffsetToRealtimeSinceStartup;
			}
			set
			{
				this.m_Event.time = value + InputRuntime.s_CurrentTimeOffsetToRealtimeSinceStartup;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x000453C0 File Offset: 0x000435C0
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x000453CD File Offset: 0x000435CD
		internal double internalTime
		{
			get
			{
				return this.m_Event.time;
			}
			set
			{
				this.m_Event.time = value;
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000453DC File Offset: 0x000435DC
		public InputEvent(FourCC type, int sizeInBytes, int deviceId, double time = -1.0)
		{
			if (time < 0.0)
			{
				time = InputRuntime.s_Instance.currentTime;
			}
			this.m_Event.type = (NativeInputEventType)type;
			this.m_Event.sizeInBytes = (ushort)sizeInBytes;
			this.m_Event.deviceId = (ushort)deviceId;
			this.m_Event.time = time;
			this.m_Event.eventId = 0;
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00045446 File Offset: 0x00043646
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x00045464 File Offset: 0x00043664
		public bool handled
		{
			get
			{
				return ((long)this.m_Event.eventId & (long)((ulong)int.MinValue)) == (long)((ulong)int.MinValue);
			}
			set
			{
				if (value)
				{
					this.m_Event.eventId = (int)((long)this.m_Event.eventId | (long)((ulong)int.MinValue));
					return;
				}
				this.m_Event.eventId = (int)((long)this.m_Event.eventId & 2147483647L);
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x000454B4 File Offset: 0x000436B4
		public override string ToString()
		{
			return string.Format("id={0} type={1} device={2} size={3} time={4}", new object[] { this.eventId, this.type, this.deviceId, this.sizeInBytes, this.time });
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00045518 File Offset: 0x00043718
		internal unsafe static InputEvent* GetNextInMemory(InputEvent* currentPtr)
		{
			uint num = currentPtr->sizeInBytes.AlignToMultipleOf(4U);
			return currentPtr + num / (uint)sizeof(InputEvent);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00045538 File Offset: 0x00043738
		internal unsafe static InputEvent* GetNextInMemoryChecked(InputEvent* currentPtr, ref InputEventBuffer buffer)
		{
			uint num = currentPtr->sizeInBytes.AlignToMultipleOf(4U);
			InputEvent* ptr = currentPtr + num / (uint)sizeof(InputEvent);
			if (!buffer.Contains(ptr))
			{
				throw new InvalidOperationException(string.Format("Event '{0}' is last event in given buffer with size {1}", new InputEventPtr(currentPtr), buffer.sizeInBytes));
			}
			return ptr;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00045588 File Offset: 0x00043788
		public unsafe static bool Equals(InputEvent* first, InputEvent* second)
		{
			return first == second || (first != null && second != null && first->m_Event.sizeInBytes == second->m_Event.sizeInBytes && UnsafeUtility.MemCmp((void*)first, (void*)second, (long)((ulong)first->m_Event.sizeInBytes)) == 0);
		}

		// Token: 0x040005AD RID: 1453
		private const uint kHandledMask = 2147483648U;

		// Token: 0x040005AE RID: 1454
		private const uint kIdMask = 2147483647U;

		// Token: 0x040005AF RID: 1455
		internal const int kBaseEventSize = 20;

		// Token: 0x040005B0 RID: 1456
		public const int InvalidEventId = 0;

		// Token: 0x040005B1 RID: 1457
		internal const int kAlignment = 4;

		// Token: 0x040005B2 RID: 1458
		[FieldOffset(0)]
		private NativeInputEvent m_Event;
	}
}
