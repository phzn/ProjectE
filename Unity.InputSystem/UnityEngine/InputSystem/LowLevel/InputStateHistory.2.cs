using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000FD RID: 253
	public class InputStateHistory<TValue> : InputStateHistory, IReadOnlyList<InputStateHistory<TValue>.Record>, IEnumerable<InputStateHistory<TValue>.Record>, IEnumerable, IReadOnlyCollection<InputStateHistory<TValue>.Record> where TValue : struct
	{
		// Token: 0x06000EFE RID: 3838 RVA: 0x00049BD4 File Offset: 0x00047DD4
		public InputStateHistory(int? maxStateSizeInBytes = null)
			: base(maxStateSizeInBytes ?? UnsafeUtility.SizeOf<TValue>())
		{
			int? num = maxStateSizeInBytes;
			int num2 = UnsafeUtility.SizeOf<TValue>();
			if ((num.GetValueOrDefault() < num2) & (num != null))
			{
				throw new ArgumentException("Max state size cannot be smaller than sizeof(TValue)", "maxStateSizeInBytes");
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00049C2C File Offset: 0x00047E2C
		public InputStateHistory(InputControl<TValue> control)
			: base(control)
		{
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00049C38 File Offset: 0x00047E38
		public InputStateHistory(string path)
			: base(path)
		{
			foreach (InputControl inputControl in base.controls)
			{
				if (!typeof(TValue).IsAssignableFrom(inputControl.valueType))
				{
					throw new ArgumentException(string.Format("Control '{0}' matched by '{1}' has value type '{2}' which is incompatible with '{3}'", new object[]
					{
						inputControl,
						path,
						inputControl.valueType.GetNiceTypeName(),
						typeof(TValue).GetNiceTypeName()
					}));
				}
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00049CE4 File Offset: 0x00047EE4
		~InputStateHistory()
		{
			base.Destroy();
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00049D10 File Offset: 0x00047F10
		public unsafe InputStateHistory<TValue>.Record AddRecord(InputStateHistory<TValue>.Record record)
		{
			int num;
			InputStateHistory.RecordHeader* ptr = base.AllocateRecord(out num);
			InputStateHistory<TValue>.Record record2 = new InputStateHistory<TValue>.Record(this, num, ptr);
			record2.CopyFrom(record);
			return record2;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00049D3C File Offset: 0x00047F3C
		public unsafe InputStateHistory<TValue>.Record RecordStateChange(InputControl<TValue> control, TValue value, double time = -1.0)
		{
			InputEventPtr inputEventPtr;
			InputStateHistory<TValue>.Record record2;
			using (StateEvent.From(control.device, out inputEventPtr, Allocator.Temp))
			{
				byte* ptr = (byte*)StateEvent.From(inputEventPtr)->state - control.device.stateBlock.byteOffset;
				control.WriteValueIntoState(value, (void*)ptr);
				if (time >= 0.0)
				{
					inputEventPtr.time = time;
				}
				InputStateHistory.Record record = base.RecordStateChange(control, inputEventPtr);
				record2 = new InputStateHistory<TValue>.Record(this, record.recordIndex, record.header);
			}
			return record2;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00049DD8 File Offset: 0x00047FD8
		public new IEnumerator<InputStateHistory<TValue>.Record> GetEnumerator()
		{
			return new InputStateHistory<TValue>.Enumerator(this);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00049DE5 File Offset: 0x00047FE5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000450 RID: 1104
		public InputStateHistory<TValue>.Record this[int index]
		{
			get
			{
				if (index < 0 || index >= base.Count)
				{
					throw new ArgumentOutOfRangeException(string.Format("Index {0} is out of range for history with {1} entries", index, base.Count), "index");
				}
				int num = base.UserIndexToRecordIndex(index);
				return new InputStateHistory<TValue>.Record(this, num, base.GetRecord(num));
			}
			set
			{
				if (index < 0 || index >= base.Count)
				{
					throw new ArgumentOutOfRangeException(string.Format("Index {0} is out of range for history with {1} entries", index, base.Count), "index");
				}
				int num = base.UserIndexToRecordIndex(index);
				new InputStateHistory<TValue>.Record(this, num, base.GetRecord(num)).CopyFrom(value);
			}
		}

		// Token: 0x02000228 RID: 552
		private struct Enumerator : IEnumerator<InputStateHistory<TValue>.Record>, IEnumerator, IDisposable
		{
			// Token: 0x06001563 RID: 5475 RVA: 0x00063498 File Offset: 0x00061698
			public Enumerator(InputStateHistory<TValue> history)
			{
				this.m_History = history;
				this.m_Index = -1;
			}

			// Token: 0x06001564 RID: 5476 RVA: 0x000634A8 File Offset: 0x000616A8
			public bool MoveNext()
			{
				if (this.m_Index + 1 >= this.m_History.Count)
				{
					return false;
				}
				this.m_Index++;
				return true;
			}

			// Token: 0x06001565 RID: 5477 RVA: 0x000634D0 File Offset: 0x000616D0
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x170005C3 RID: 1475
			// (get) Token: 0x06001566 RID: 5478 RVA: 0x000634D9 File Offset: 0x000616D9
			public InputStateHistory<TValue>.Record Current
			{
				get
				{
					return this.m_History[this.m_Index];
				}
			}

			// Token: 0x170005C4 RID: 1476
			// (get) Token: 0x06001567 RID: 5479 RVA: 0x000634EC File Offset: 0x000616EC
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001568 RID: 5480 RVA: 0x000634F9 File Offset: 0x000616F9
			public void Dispose()
			{
			}

			// Token: 0x04000BD2 RID: 3026
			private readonly InputStateHistory<TValue> m_History;

			// Token: 0x04000BD3 RID: 3027
			private int m_Index;
		}

		// Token: 0x02000229 RID: 553
		public new struct Record : IEquatable<InputStateHistory<TValue>.Record>
		{
			// Token: 0x170005C5 RID: 1477
			// (get) Token: 0x06001569 RID: 5481 RVA: 0x000634FB File Offset: 0x000616FB
			internal unsafe InputStateHistory.RecordHeader* header
			{
				get
				{
					return this.m_Owner.GetRecord(this.recordIndex);
				}
			}

			// Token: 0x170005C6 RID: 1478
			// (get) Token: 0x0600156A RID: 5482 RVA: 0x0006350E File Offset: 0x0006170E
			internal int recordIndex
			{
				get
				{
					return this.m_IndexPlusOne - 1;
				}
			}

			// Token: 0x170005C7 RID: 1479
			// (get) Token: 0x0600156B RID: 5483 RVA: 0x00063518 File Offset: 0x00061718
			public unsafe bool valid
			{
				get
				{
					return this.m_Owner != null && this.m_IndexPlusOne != 0 && this.header->version == this.m_Version;
				}
			}

			// Token: 0x170005C8 RID: 1480
			// (get) Token: 0x0600156C RID: 5484 RVA: 0x0006353F File Offset: 0x0006173F
			public InputStateHistory<TValue> owner
			{
				get
				{
					return this.m_Owner;
				}
			}

			// Token: 0x170005C9 RID: 1481
			// (get) Token: 0x0600156D RID: 5485 RVA: 0x00063547 File Offset: 0x00061747
			public int index
			{
				get
				{
					this.CheckValid();
					return this.m_Owner.RecordIndexToUserIndex(this.recordIndex);
				}
			}

			// Token: 0x170005CA RID: 1482
			// (get) Token: 0x0600156E RID: 5486 RVA: 0x00063560 File Offset: 0x00061760
			public unsafe double time
			{
				get
				{
					this.CheckValid();
					return this.header->time;
				}
			}

			// Token: 0x170005CB RID: 1483
			// (get) Token: 0x0600156F RID: 5487 RVA: 0x00063574 File Offset: 0x00061774
			public unsafe InputControl<TValue> control
			{
				get
				{
					this.CheckValid();
					ReadOnlyArray<InputControl> controls = this.m_Owner.controls;
					if (controls.Count == 1 && !this.m_Owner.m_AddNewControls)
					{
						return (InputControl<TValue>)controls[0];
					}
					return (InputControl<TValue>)controls[this.header->controlIndex];
				}
			}

			// Token: 0x170005CC RID: 1484
			// (get) Token: 0x06001570 RID: 5488 RVA: 0x000635D0 File Offset: 0x000617D0
			public InputStateHistory<TValue>.Record next
			{
				get
				{
					this.CheckValid();
					int num = this.m_Owner.RecordIndexToUserIndex(this.recordIndex);
					if (num + 1 >= this.m_Owner.Count)
					{
						return default(InputStateHistory<TValue>.Record);
					}
					int num2 = this.m_Owner.UserIndexToRecordIndex(num + 1);
					return new InputStateHistory<TValue>.Record(this.m_Owner, num2, this.m_Owner.GetRecord(num2));
				}
			}

			// Token: 0x170005CD RID: 1485
			// (get) Token: 0x06001571 RID: 5489 RVA: 0x00063638 File Offset: 0x00061838
			public InputStateHistory<TValue>.Record previous
			{
				get
				{
					this.CheckValid();
					int num = this.m_Owner.RecordIndexToUserIndex(this.recordIndex);
					if (num - 1 < 0)
					{
						return default(InputStateHistory<TValue>.Record);
					}
					int num2 = this.m_Owner.UserIndexToRecordIndex(num - 1);
					return new InputStateHistory<TValue>.Record(this.m_Owner, num2, this.m_Owner.GetRecord(num2));
				}
			}

			// Token: 0x06001572 RID: 5490 RVA: 0x00063694 File Offset: 0x00061894
			internal unsafe Record(InputStateHistory<TValue> owner, int index, InputStateHistory.RecordHeader* header)
			{
				this.m_Owner = owner;
				this.m_IndexPlusOne = index + 1;
				this.m_Version = header->version;
			}

			// Token: 0x06001573 RID: 5491 RVA: 0x000636B2 File Offset: 0x000618B2
			internal Record(InputStateHistory<TValue> owner, int index)
			{
				this.m_Owner = owner;
				this.m_IndexPlusOne = index + 1;
				this.m_Version = 0U;
			}

			// Token: 0x06001574 RID: 5492 RVA: 0x000636CB File Offset: 0x000618CB
			public TValue ReadValue()
			{
				this.CheckValid();
				return this.m_Owner.ReadValue<TValue>(this.header);
			}

			// Token: 0x06001575 RID: 5493 RVA: 0x000636E4 File Offset: 0x000618E4
			public unsafe void* GetUnsafeMemoryPtr()
			{
				this.CheckValid();
				return this.GetUnsafeMemoryPtrUnchecked();
			}

			// Token: 0x06001576 RID: 5494 RVA: 0x000636F4 File Offset: 0x000618F4
			internal unsafe void* GetUnsafeMemoryPtrUnchecked()
			{
				if (this.m_Owner.controls.Count == 1 && !this.m_Owner.m_AddNewControls)
				{
					return (void*)this.header->statePtrWithoutControlIndex;
				}
				return (void*)this.header->statePtrWithControlIndex;
			}

			// Token: 0x06001577 RID: 5495 RVA: 0x0006373B File Offset: 0x0006193B
			public unsafe void* GetUnsafeExtraMemoryPtr()
			{
				this.CheckValid();
				return this.GetUnsafeExtraMemoryPtrUnchecked();
			}

			// Token: 0x06001578 RID: 5496 RVA: 0x00063749 File Offset: 0x00061949
			internal unsafe void* GetUnsafeExtraMemoryPtrUnchecked()
			{
				if (this.m_Owner.extraMemoryPerRecord == 0)
				{
					throw new InvalidOperationException("No extra memory has been set up for history records; set extraMemoryPerRecord");
				}
				return (void*)(this.header + this.m_Owner.bytesPerRecord / sizeof(InputStateHistory.RecordHeader) - this.m_Owner.extraMemoryPerRecord / sizeof(InputStateHistory.RecordHeader));
			}

			// Token: 0x06001579 RID: 5497 RVA: 0x00063784 File Offset: 0x00061984
			public void CopyFrom(InputStateHistory<TValue>.Record record)
			{
				this.CheckValid();
				if (!record.valid)
				{
					throw new ArgumentException("Given history record is not valid", "record");
				}
				InputStateHistory.Record record2 = new InputStateHistory.Record(this.m_Owner, this.recordIndex, this.header);
				record2.CopyFrom(new InputStateHistory.Record(record.m_Owner, record.recordIndex, record.header));
				this.m_Version = record2.version;
			}

			// Token: 0x0600157A RID: 5498 RVA: 0x000637F6 File Offset: 0x000619F6
			private unsafe void CheckValid()
			{
				if (this.m_Owner == null || this.m_IndexPlusOne == 0)
				{
					throw new InvalidOperationException("Value not initialized");
				}
				if (this.header->version != this.m_Version)
				{
					throw new InvalidOperationException("Record is no longer valid");
				}
			}

			// Token: 0x0600157B RID: 5499 RVA: 0x00063831 File Offset: 0x00061A31
			public bool Equals(InputStateHistory<TValue>.Record other)
			{
				return this.m_Owner == other.m_Owner && this.m_IndexPlusOne == other.m_IndexPlusOne && this.m_Version == other.m_Version;
			}

			// Token: 0x0600157C RID: 5500 RVA: 0x00063860 File Offset: 0x00061A60
			public override bool Equals(object obj)
			{
				if (obj is InputStateHistory<TValue>.Record)
				{
					InputStateHistory<TValue>.Record record = (InputStateHistory<TValue>.Record)obj;
					return this.Equals(record);
				}
				return false;
			}

			// Token: 0x0600157D RID: 5501 RVA: 0x00063885 File Offset: 0x00061A85
			public override int GetHashCode()
			{
				return (((((this.m_Owner != null) ? this.m_Owner.GetHashCode() : 0) * 397) ^ this.m_IndexPlusOne) * 397) ^ (int)this.m_Version;
			}

			// Token: 0x0600157E RID: 5502 RVA: 0x000638B7 File Offset: 0x00061AB7
			public override string ToString()
			{
				if (!this.valid)
				{
					return "<Invalid>";
				}
				return string.Format("{{ control={0} value={1} time={2} }}", this.control, this.ReadValue(), this.time);
			}

			// Token: 0x04000BD4 RID: 3028
			private readonly InputStateHistory<TValue> m_Owner;

			// Token: 0x04000BD5 RID: 3029
			private readonly int m_IndexPlusOne;

			// Token: 0x04000BD6 RID: 3030
			private uint m_Version;
		}
	}
}
