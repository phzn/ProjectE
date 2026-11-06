using System;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E9 RID: 233
	public struct InputEventPtr : IEquatable<InputEventPtr>
	{
		// Token: 0x06000DD9 RID: 3545 RVA: 0x00045AFA File Offset: 0x00043CFA
		public unsafe InputEventPtr(InputEvent* eventPtr)
		{
			this.m_EventPtr = eventPtr;
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00045B03 File Offset: 0x00043D03
		public bool valid
		{
			get
			{
				return this.m_EventPtr != null;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00045B12 File Offset: 0x00043D12
		// (set) Token: 0x06000DDC RID: 3548 RVA: 0x00045B29 File Offset: 0x00043D29
		public unsafe bool handled
		{
			get
			{
				return this.valid && this.m_EventPtr->handled;
			}
			set
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("The InputEventPtr is not valid.");
				}
				this.m_EventPtr->handled = value;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00045B4A File Offset: 0x00043D4A
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00045B61 File Offset: 0x00043D61
		public unsafe int id
		{
			get
			{
				if (!this.valid)
				{
					return 0;
				}
				return this.m_EventPtr->eventId;
			}
			set
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("The InputEventPtr is not valid.");
				}
				this.m_EventPtr->eventId = value;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00045B84 File Offset: 0x00043D84
		public unsafe FourCC type
		{
			get
			{
				if (!this.valid)
				{
					return default(FourCC);
				}
				return this.m_EventPtr->type;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x00045BAE File Offset: 0x00043DAE
		public unsafe uint sizeInBytes
		{
			get
			{
				if (!this.valid)
				{
					return 0U;
				}
				return this.m_EventPtr->sizeInBytes;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00045BC5 File Offset: 0x00043DC5
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00045BDC File Offset: 0x00043DDC
		public unsafe int deviceId
		{
			get
			{
				if (!this.valid)
				{
					return 0;
				}
				return this.m_EventPtr->deviceId;
			}
			set
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("The InputEventPtr is not valid.");
				}
				this.m_EventPtr->deviceId = value;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x00045BFD File Offset: 0x00043DFD
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x00045C1C File Offset: 0x00043E1C
		public unsafe double time
		{
			get
			{
				if (!this.valid)
				{
					return 0.0;
				}
				return this.m_EventPtr->time;
			}
			set
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("The InputEventPtr is not valid.");
				}
				this.m_EventPtr->time = value;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x00045C3D File Offset: 0x00043E3D
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x00045C5C File Offset: 0x00043E5C
		internal unsafe double internalTime
		{
			get
			{
				if (!this.valid)
				{
					return 0.0;
				}
				return this.m_EventPtr->internalTime;
			}
			set
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("The InputEventPtr is not valid.");
				}
				this.m_EventPtr->internalTime = value;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00045C7D File Offset: 0x00043E7D
		public unsafe InputEvent* data
		{
			get
			{
				return this.m_EventPtr;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00045C88 File Offset: 0x00043E88
		internal unsafe FourCC stateFormat
		{
			get
			{
				FourCC type = this.type;
				if (type == 1398030676)
				{
					return StateEvent.FromUnchecked(this)->stateFormat;
				}
				if (type == 1145852993)
				{
					return DeltaStateEvent.FromUnchecked(this)->stateFormat;
				}
				string text = "Event must be a StateEvent or DeltaStateEvent but is ";
				InputEventPtr inputEventPtr = this;
				throw new InvalidOperationException(text + inputEventPtr.ToString());
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00045D08 File Offset: 0x00043F08
		internal unsafe uint stateSizeInBytes
		{
			get
			{
				if (this.IsA<StateEvent>())
				{
					return StateEvent.From(this)->stateSizeInBytes;
				}
				if (this.IsA<DeltaStateEvent>())
				{
					return DeltaStateEvent.From(this)->deltaStateSizeInBytes;
				}
				string text = "Event must be a StateEvent or DeltaStateEvent but is ";
				InputEventPtr inputEventPtr = this;
				throw new InvalidOperationException(text + inputEventPtr.ToString());
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00045D6C File Offset: 0x00043F6C
		internal unsafe uint stateOffset
		{
			get
			{
				if (this.IsA<DeltaStateEvent>())
				{
					return DeltaStateEvent.From(this)->stateOffset;
				}
				string text = "Event must be a DeltaStateEvent but is ";
				InputEventPtr inputEventPtr = this;
				throw new InvalidOperationException(text + inputEventPtr.ToString());
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00045DB8 File Offset: 0x00043FB8
		public unsafe bool IsA<TOtherEvent>() where TOtherEvent : struct, IInputEventTypeInfo
		{
			if (this.m_EventPtr == null)
			{
				return false;
			}
			TOtherEvent totherEvent = default(TOtherEvent);
			return this.m_EventPtr->type == totherEvent.typeStatic;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00045DF8 File Offset: 0x00043FF8
		public InputEventPtr Next()
		{
			if (!this.valid)
			{
				return default(InputEventPtr);
			}
			return new InputEventPtr(InputEvent.GetNextInMemory(this.m_EventPtr));
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00045E28 File Offset: 0x00044028
		public unsafe override string ToString()
		{
			if (!this.valid)
			{
				return "null";
			}
			InputEvent inputEvent = *this.m_EventPtr;
			return inputEvent.ToString();
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00045E5C File Offset: 0x0004405C
		public unsafe InputEvent* ToPointer()
		{
			return this;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00045E69 File Offset: 0x00044069
		public bool Equals(InputEventPtr other)
		{
			return this.m_EventPtr == other.m_EventPtr || InputEvent.Equals(this.m_EventPtr, other.m_EventPtr);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00045E8C File Offset: 0x0004408C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is InputEventPtr)
			{
				InputEventPtr inputEventPtr = (InputEventPtr)obj;
				return this.Equals(inputEventPtr);
			}
			return false;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00045EB6 File Offset: 0x000440B6
		public override int GetHashCode()
		{
			return this.m_EventPtr;
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00045EC0 File Offset: 0x000440C0
		public static bool operator ==(InputEventPtr left, InputEventPtr right)
		{
			return left.m_EventPtr == right.m_EventPtr;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00045ED0 File Offset: 0x000440D0
		public static bool operator !=(InputEventPtr left, InputEventPtr right)
		{
			return left.m_EventPtr != right.m_EventPtr;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00045EE3 File Offset: 0x000440E3
		public unsafe static implicit operator InputEventPtr(InputEvent* eventPtr)
		{
			return new InputEventPtr(eventPtr);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00045EEB File Offset: 0x000440EB
		public unsafe static InputEventPtr From(InputEvent* eventPtr)
		{
			return new InputEventPtr(eventPtr);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00045EF3 File Offset: 0x000440F3
		public unsafe static implicit operator InputEvent*(InputEventPtr eventPtr)
		{
			return eventPtr.data;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00045EFC File Offset: 0x000440FC
		public unsafe static InputEvent* FromInputEventPtr(InputEventPtr eventPtr)
		{
			return eventPtr.data;
		}

		// Token: 0x040005B9 RID: 1465
		private unsafe readonly InputEvent* m_EventPtr;
	}
}
