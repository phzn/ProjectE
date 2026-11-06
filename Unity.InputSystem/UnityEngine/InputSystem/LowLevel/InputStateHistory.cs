using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000FC RID: 252
	public class InputStateHistory : IDisposable, IEnumerable<InputStateHistory.Record>, IEnumerable, IInputStateChangeMonitor
	{
		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x000492BB File Offset: 0x000474BB
		public int Count
		{
			get
			{
				return this.m_RecordCount;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x000492C3 File Offset: 0x000474C3
		public uint version
		{
			get
			{
				return this.m_CurrentVersion;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x000492CB File Offset: 0x000474CB
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x000492D3 File Offset: 0x000474D3
		public int historyDepth
		{
			get
			{
				return this.m_HistoryDepth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("History depth cannot be negative", "value");
				}
				if (this.m_RecordBuffer.IsCreated)
				{
					throw new NotImplementedException();
				}
				this.m_HistoryDepth = value;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00049303 File Offset: 0x00047503
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0004930B File Offset: 0x0004750B
		public int extraMemoryPerRecord
		{
			get
			{
				return this.m_ExtraMemoryPerRecord;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Memory size cannot be negative", "value");
				}
				if (this.m_RecordBuffer.IsCreated)
				{
					throw new NotImplementedException();
				}
				this.m_ExtraMemoryPerRecord = value;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0004933C File Offset: 0x0004753C
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x0004936E File Offset: 0x0004756E
		public InputUpdateType updateMask
		{
			get
			{
				InputUpdateType? updateMask = this.m_UpdateMask;
				if (updateMask == null)
				{
					return InputSystem.s_Manager.updateMask & ~InputUpdateType.Editor;
				}
				return updateMask.GetValueOrDefault();
			}
			set
			{
				if (value == InputUpdateType.None)
				{
					throw new ArgumentException("'InputUpdateType.None' is not a valid update mask", "value");
				}
				this.m_UpdateMask = new InputUpdateType?(value);
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x0004938F File Offset: 0x0004758F
		public ReadOnlyArray<InputControl> controls
		{
			get
			{
				return new ReadOnlyArray<InputControl>(this.m_Controls, 0, this.m_ControlCount);
			}
		}

		// Token: 0x1700044C RID: 1100
		public InputStateHistory.Record this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_RecordCount)
				{
					throw new ArgumentOutOfRangeException(string.Format("Index {0} is out of range for history with {1} entries", index, this.m_RecordCount), "index");
				}
				int num = this.UserIndexToRecordIndex(index);
				return new InputStateHistory.Record(this, num, this.GetRecord(num));
			}
			set
			{
				if (index < 0 || index >= this.m_RecordCount)
				{
					throw new ArgumentOutOfRangeException(string.Format("Index {0} is out of range for history with {1} entries", index, this.m_RecordCount), "index");
				}
				int num = this.UserIndexToRecordIndex(index);
				new InputStateHistory.Record(this, num, this.GetRecord(num)).CopyFrom(value);
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0004945B File Offset: 0x0004765B
		// (set) Token: 0x06000EE1 RID: 3809 RVA: 0x00049463 File Offset: 0x00047663
		public Action<InputStateHistory.Record> onRecordAdded { get; set; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0004946C File Offset: 0x0004766C
		// (set) Token: 0x06000EE3 RID: 3811 RVA: 0x00049474 File Offset: 0x00047674
		public Func<InputControl, double, InputEventPtr, bool> onShouldRecordStateChange { get; set; }

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0004947D File Offset: 0x0004767D
		public InputStateHistory(int maxStateSizeInBytes)
		{
			if (maxStateSizeInBytes <= 0)
			{
				throw new ArgumentException("State size must be >= 0", "maxStateSizeInBytes");
			}
			this.m_AddNewControls = true;
			this.m_StateSizeInBytes = maxStateSizeInBytes.AlignToMultipleOf(4);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x000494B8 File Offset: 0x000476B8
		public InputStateHistory(string path)
		{
			using (InputControlList<InputControl> inputControlList = InputSystem.FindControls(path))
			{
				this.m_Controls = inputControlList.ToArray(false);
				this.m_ControlCount = this.m_Controls.Length;
			}
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0004951C File Offset: 0x0004771C
		public InputStateHistory(InputControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			this.m_Controls = new InputControl[] { control };
			this.m_ControlCount = 1;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00049554 File Offset: 0x00047754
		public InputStateHistory(IEnumerable<InputControl> controls)
		{
			if (controls != null)
			{
				this.m_Controls = controls.ToArray<InputControl>();
				this.m_ControlCount = this.m_Controls.Length;
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00049584 File Offset: 0x00047784
		~InputStateHistory()
		{
			this.Dispose();
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x000495B0 File Offset: 0x000477B0
		public void Clear()
		{
			this.m_HeadIndex = 0;
			this.m_RecordCount = 0;
			this.m_CurrentVersion += 1U;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x000495D0 File Offset: 0x000477D0
		public unsafe InputStateHistory.Record AddRecord(InputStateHistory.Record record)
		{
			int num;
			InputStateHistory.RecordHeader* ptr = this.AllocateRecord(out num);
			InputStateHistory.Record record2 = new InputStateHistory.Record(this, num, ptr);
			record2.CopyFrom(record);
			return record2;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x000495FC File Offset: 0x000477FC
		public void StartRecording()
		{
			foreach (InputControl inputControl in this.controls)
			{
				InputState.AddChangeMonitor(inputControl, this, -1L, 0U);
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00049654 File Offset: 0x00047854
		public void StopRecording()
		{
			foreach (InputControl inputControl in this.controls)
			{
				InputState.RemoveChangeMonitor(inputControl, this, -1L);
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000496AC File Offset: 0x000478AC
		public unsafe InputStateHistory.Record RecordStateChange(InputControl control, InputEventPtr eventPtr)
		{
			if (eventPtr.IsA<DeltaStateEvent>())
			{
				throw new NotImplementedException();
			}
			if (!eventPtr.IsA<StateEvent>())
			{
				throw new ArgumentException(string.Format("Event must be a state event but is '{0}' instead", eventPtr), "eventPtr");
			}
			byte* ptr = (byte*)StateEvent.From(eventPtr)->state - control.device.stateBlock.byteOffset;
			return this.RecordStateChange(control, (void*)ptr, eventPtr.time);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0004971C File Offset: 0x0004791C
		public unsafe InputStateHistory.Record RecordStateChange(InputControl control, void* statePtr, double time)
		{
			int num = this.m_Controls.IndexOfReference(control, this.m_ControlCount);
			if (num == -1)
			{
				if (!this.m_AddNewControls)
				{
					throw new ArgumentException(string.Format("Control '{0}' is not part of InputStateHistory", control), "control");
				}
				if ((ulong)control.stateBlock.alignedSizeInBytes > (ulong)((long)this.m_StateSizeInBytes))
				{
					throw new InvalidOperationException(string.Format("Cannot add control '{0}' with state larger than {1} bytes", control, this.m_StateSizeInBytes));
				}
				num = ArrayHelpers.AppendWithCapacity<InputControl>(ref this.m_Controls, ref this.m_ControlCount, control, 10);
			}
			int num2;
			InputStateHistory.RecordHeader* ptr = this.AllocateRecord(out num2);
			ptr->time = time;
			ref InputStateHistory.RecordHeader ptr2 = ref *ptr;
			uint num3 = this.m_CurrentVersion + 1U;
			this.m_CurrentVersion = num3;
			ptr2.version = num3;
			byte* ptr3 = ptr->statePtrWithoutControlIndex;
			if (this.m_ControlCount > 1 || this.m_AddNewControls)
			{
				ptr->controlIndex = num;
				ptr3 = ptr->statePtrWithControlIndex;
			}
			uint alignedSizeInBytes = control.stateBlock.alignedSizeInBytes;
			uint byteOffset = control.stateBlock.byteOffset;
			UnsafeUtility.MemCpy((void*)ptr3, (void*)((byte*)statePtr + byteOffset), (long)((ulong)alignedSizeInBytes));
			InputStateHistory.Record record = new InputStateHistory.Record(this, num2, ptr);
			Action<InputStateHistory.Record> onRecordAdded = this.onRecordAdded;
			if (onRecordAdded != null)
			{
				onRecordAdded(record);
			}
			return record;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00049849 File Offset: 0x00047A49
		public IEnumerator<InputStateHistory.Record> GetEnumerator()
		{
			return new InputStateHistory.Enumerator(this);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00049856 File Offset: 0x00047A56
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0004985E File Offset: 0x00047A5E
		public void Dispose()
		{
			this.StopRecording();
			this.Destroy();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00049872 File Offset: 0x00047A72
		protected void Destroy()
		{
			if (this.m_RecordBuffer.IsCreated)
			{
				this.m_RecordBuffer.Dispose();
				this.m_RecordBuffer = default(NativeArray<byte>);
			}
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00049898 File Offset: 0x00047A98
		private void Allocate()
		{
			if (!this.m_AddNewControls)
			{
				this.m_StateSizeInBytes = 0;
				foreach (InputControl inputControl in this.controls)
				{
					this.m_StateSizeInBytes = (int)Math.Max((uint)this.m_StateSizeInBytes, inputControl.stateBlock.alignedSizeInBytes);
				}
			}
			int num = this.bytesPerRecord * this.m_HistoryDepth;
			this.m_RecordBuffer = new NativeArray<byte>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00049934 File Offset: 0x00047B34
		protected internal int RecordIndexToUserIndex(int index)
		{
			if (index < this.m_HeadIndex)
			{
				return this.m_HistoryDepth - this.m_HeadIndex + index;
			}
			return index - this.m_HeadIndex;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00049957 File Offset: 0x00047B57
		protected internal int UserIndexToRecordIndex(int index)
		{
			return (this.m_HeadIndex + index) % this.m_HistoryDepth;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00049968 File Offset: 0x00047B68
		protected internal unsafe InputStateHistory.RecordHeader* GetRecord(int index)
		{
			if (!this.m_RecordBuffer.IsCreated)
			{
				throw new InvalidOperationException("History buffer has been disposed");
			}
			if (index < 0 || index >= this.m_HistoryDepth)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.GetRecordUnchecked(index);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x000499A1 File Offset: 0x00047BA1
		internal unsafe InputStateHistory.RecordHeader* GetRecordUnchecked(int index)
		{
			return (InputStateHistory.RecordHeader*)((byte*)this.m_RecordBuffer.GetUnsafePtr<byte>() + index * this.bytesPerRecord);
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x000499B8 File Offset: 0x00047BB8
		protected internal unsafe InputStateHistory.RecordHeader* AllocateRecord(out int index)
		{
			if (!this.m_RecordBuffer.IsCreated)
			{
				this.Allocate();
			}
			index = (this.m_HeadIndex + this.m_RecordCount) % this.m_HistoryDepth;
			if (this.m_RecordCount == this.m_HistoryDepth)
			{
				this.m_HeadIndex = (this.m_HeadIndex + 1) % this.m_HistoryDepth;
			}
			else
			{
				this.m_RecordCount++;
			}
			return (InputStateHistory.RecordHeader*)((byte*)this.m_RecordBuffer.GetUnsafePtr<byte>() + this.bytesPerRecord * index);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00049A38 File Offset: 0x00047C38
		protected unsafe TValue ReadValue<TValue>(InputStateHistory.RecordHeader* data) where TValue : struct
		{
			bool flag = this.m_ControlCount == 1 && !this.m_AddNewControls;
			InputControl inputControl = (flag ? this.controls[0] : this.controls[data->controlIndex]);
			InputControl<TValue> inputControl2 = inputControl as InputControl<TValue>;
			if (inputControl2 == null)
			{
				throw new InvalidOperationException(string.Format("Cannot read value of type '{0}' from control '{1}' with value type '{2}'", typeof(TValue).GetNiceTypeName(), inputControl, inputControl.valueType.GetNiceTypeName()));
			}
			byte* ptr = (flag ? data->statePtrWithoutControlIndex : data->statePtrWithControlIndex);
			ptr -= inputControl.stateBlock.byteOffset;
			return inputControl2.ReadValueFromState((void*)ptr);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00049AE4 File Offset: 0x00047CE4
		protected unsafe object ReadValueAsObject(InputStateHistory.RecordHeader* data)
		{
			bool flag = this.m_ControlCount == 1 && !this.m_AddNewControls;
			InputControl inputControl = (flag ? this.controls[0] : this.controls[data->controlIndex]);
			byte* ptr = (flag ? data->statePtrWithoutControlIndex : data->statePtrWithControlIndex);
			ptr -= inputControl.stateBlock.byteOffset;
			return inputControl.ReadValueFromStateAsObject((void*)ptr);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00049B5C File Offset: 0x00047D5C
		void IInputStateChangeMonitor.NotifyControlStateChanged(InputControl control, double time, InputEventPtr eventPtr, long monitorIndex)
		{
			bool currentUpdateType = InputState.currentUpdateType != InputUpdateType.None;
			InputUpdateType updateMask = this.updateMask;
			if (((currentUpdateType ? InputUpdateType.Dynamic : InputUpdateType.None) & updateMask) == InputUpdateType.None)
			{
				return;
			}
			if (this.onShouldRecordStateChange != null && !this.onShouldRecordStateChange(control, time, eventPtr))
			{
				return;
			}
			this.RecordStateChange(control, control.currentStatePtr, time);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00049BA2 File Offset: 0x00047DA2
		void IInputStateChangeMonitor.NotifyTimerExpired(InputControl control, double time, long monitorIndex, int timerIndex)
		{
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x00049BA4 File Offset: 0x00047DA4
		internal int bytesPerRecord
		{
			get
			{
				return (this.m_StateSizeInBytes + this.m_ExtraMemoryPerRecord + ((this.m_ControlCount == 1 && !this.m_AddNewControls) ? 12 : 16)).AlignToMultipleOf(4);
			}
		}

		// Token: 0x04000638 RID: 1592
		private const int kDefaultHistorySize = 128;

		// Token: 0x0400063B RID: 1595
		internal InputControl[] m_Controls;

		// Token: 0x0400063C RID: 1596
		internal int m_ControlCount;

		// Token: 0x0400063D RID: 1597
		private NativeArray<byte> m_RecordBuffer;

		// Token: 0x0400063E RID: 1598
		private int m_StateSizeInBytes;

		// Token: 0x0400063F RID: 1599
		private int m_RecordCount;

		// Token: 0x04000640 RID: 1600
		private int m_HistoryDepth = 128;

		// Token: 0x04000641 RID: 1601
		private int m_ExtraMemoryPerRecord;

		// Token: 0x04000642 RID: 1602
		internal int m_HeadIndex;

		// Token: 0x04000643 RID: 1603
		internal uint m_CurrentVersion;

		// Token: 0x04000644 RID: 1604
		private InputUpdateType? m_UpdateMask;

		// Token: 0x04000645 RID: 1605
		internal readonly bool m_AddNewControls;

		// Token: 0x02000225 RID: 549
		private struct Enumerator : IEnumerator<InputStateHistory.Record>, IEnumerator, IDisposable
		{
			// Token: 0x06001544 RID: 5444 RVA: 0x00062EAA File Offset: 0x000610AA
			public Enumerator(InputStateHistory history)
			{
				this.m_History = history;
				this.m_Index = -1;
			}

			// Token: 0x06001545 RID: 5445 RVA: 0x00062EBA File Offset: 0x000610BA
			public bool MoveNext()
			{
				if (this.m_Index + 1 >= this.m_History.Count)
				{
					return false;
				}
				this.m_Index++;
				return true;
			}

			// Token: 0x06001546 RID: 5446 RVA: 0x00062EE2 File Offset: 0x000610E2
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x170005B5 RID: 1461
			// (get) Token: 0x06001547 RID: 5447 RVA: 0x00062EEB File Offset: 0x000610EB
			public InputStateHistory.Record Current
			{
				get
				{
					return this.m_History[this.m_Index];
				}
			}

			// Token: 0x170005B6 RID: 1462
			// (get) Token: 0x06001548 RID: 5448 RVA: 0x00062EFE File Offset: 0x000610FE
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001549 RID: 5449 RVA: 0x00062F0B File Offset: 0x0006110B
			public void Dispose()
			{
			}

			// Token: 0x04000BC6 RID: 3014
			private readonly InputStateHistory m_History;

			// Token: 0x04000BC7 RID: 3015
			private int m_Index;
		}

		// Token: 0x02000226 RID: 550
		[StructLayout(LayoutKind.Explicit)]
		protected internal struct RecordHeader
		{
			// Token: 0x170005B7 RID: 1463
			// (get) Token: 0x0600154A RID: 5450 RVA: 0x00062F10 File Offset: 0x00061110
			public unsafe byte* statePtrWithControlIndex
			{
				get
				{
					fixed (byte* ptr = &this.m_StateWithControlIndex.FixedElementField)
					{
						return ptr;
					}
				}
			}

			// Token: 0x170005B8 RID: 1464
			// (get) Token: 0x0600154B RID: 5451 RVA: 0x00062F2C File Offset: 0x0006112C
			public unsafe byte* statePtrWithoutControlIndex
			{
				get
				{
					fixed (byte* ptr = &this.m_StateWithoutControlIndex.FixedElementField)
					{
						return ptr;
					}
				}
			}

			// Token: 0x04000BC8 RID: 3016
			[FieldOffset(0)]
			public double time;

			// Token: 0x04000BC9 RID: 3017
			[FieldOffset(8)]
			public uint version;

			// Token: 0x04000BCA RID: 3018
			[FieldOffset(12)]
			public int controlIndex;

			// Token: 0x04000BCB RID: 3019
			[FixedBuffer(typeof(byte), 1)]
			[FieldOffset(12)]
			private InputStateHistory.RecordHeader.<m_StateWithoutControlIndex>e__FixedBuffer m_StateWithoutControlIndex;

			// Token: 0x04000BCC RID: 3020
			[FixedBuffer(typeof(byte), 1)]
			[FieldOffset(16)]
			private InputStateHistory.RecordHeader.<m_StateWithControlIndex>e__FixedBuffer m_StateWithControlIndex;

			// Token: 0x04000BCD RID: 3021
			public const int kSizeWithControlIndex = 16;

			// Token: 0x04000BCE RID: 3022
			public const int kSizeWithoutControlIndex = 12;

			// Token: 0x02000282 RID: 642
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1)]
			public struct <m_StateWithControlIndex>e__FixedBuffer
			{
				// Token: 0x04000D1D RID: 3357
				public byte FixedElementField;
			}

			// Token: 0x02000283 RID: 643
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1)]
			public struct <m_StateWithoutControlIndex>e__FixedBuffer
			{
				// Token: 0x04000D1E RID: 3358
				public byte FixedElementField;
			}
		}

		// Token: 0x02000227 RID: 551
		public struct Record : IEquatable<InputStateHistory.Record>
		{
			// Token: 0x170005B9 RID: 1465
			// (get) Token: 0x0600154C RID: 5452 RVA: 0x00062F47 File Offset: 0x00061147
			internal unsafe InputStateHistory.RecordHeader* header
			{
				get
				{
					return this.m_Owner.GetRecord(this.recordIndex);
				}
			}

			// Token: 0x170005BA RID: 1466
			// (get) Token: 0x0600154D RID: 5453 RVA: 0x00062F5A File Offset: 0x0006115A
			internal int recordIndex
			{
				get
				{
					return this.m_IndexPlusOne - 1;
				}
			}

			// Token: 0x170005BB RID: 1467
			// (get) Token: 0x0600154E RID: 5454 RVA: 0x00062F64 File Offset: 0x00061164
			internal uint version
			{
				get
				{
					return this.m_Version;
				}
			}

			// Token: 0x170005BC RID: 1468
			// (get) Token: 0x0600154F RID: 5455 RVA: 0x00062F6C File Offset: 0x0006116C
			public unsafe bool valid
			{
				get
				{
					return this.m_Owner != null && this.m_IndexPlusOne != 0 && this.header->version == this.m_Version;
				}
			}

			// Token: 0x170005BD RID: 1469
			// (get) Token: 0x06001550 RID: 5456 RVA: 0x00062F93 File Offset: 0x00061193
			public InputStateHistory owner
			{
				get
				{
					return this.m_Owner;
				}
			}

			// Token: 0x170005BE RID: 1470
			// (get) Token: 0x06001551 RID: 5457 RVA: 0x00062F9B File Offset: 0x0006119B
			public int index
			{
				get
				{
					this.CheckValid();
					return this.m_Owner.RecordIndexToUserIndex(this.recordIndex);
				}
			}

			// Token: 0x170005BF RID: 1471
			// (get) Token: 0x06001552 RID: 5458 RVA: 0x00062FB4 File Offset: 0x000611B4
			public unsafe double time
			{
				get
				{
					this.CheckValid();
					return this.header->time;
				}
			}

			// Token: 0x170005C0 RID: 1472
			// (get) Token: 0x06001553 RID: 5459 RVA: 0x00062FC8 File Offset: 0x000611C8
			public unsafe InputControl control
			{
				get
				{
					this.CheckValid();
					ReadOnlyArray<InputControl> controls = this.m_Owner.controls;
					if (controls.Count == 1 && !this.m_Owner.m_AddNewControls)
					{
						return controls[0];
					}
					return controls[this.header->controlIndex];
				}
			}

			// Token: 0x170005C1 RID: 1473
			// (get) Token: 0x06001554 RID: 5460 RVA: 0x0006301C File Offset: 0x0006121C
			public InputStateHistory.Record next
			{
				get
				{
					this.CheckValid();
					int num = this.m_Owner.RecordIndexToUserIndex(this.recordIndex);
					if (num + 1 >= this.m_Owner.Count)
					{
						return default(InputStateHistory.Record);
					}
					int num2 = this.m_Owner.UserIndexToRecordIndex(num + 1);
					return new InputStateHistory.Record(this.m_Owner, num2, this.m_Owner.GetRecord(num2));
				}
			}

			// Token: 0x170005C2 RID: 1474
			// (get) Token: 0x06001555 RID: 5461 RVA: 0x00063084 File Offset: 0x00061284
			public InputStateHistory.Record previous
			{
				get
				{
					this.CheckValid();
					int num = this.m_Owner.RecordIndexToUserIndex(this.recordIndex);
					if (num - 1 < 0)
					{
						return default(InputStateHistory.Record);
					}
					int num2 = this.m_Owner.UserIndexToRecordIndex(num - 1);
					return new InputStateHistory.Record(this.m_Owner, num2, this.m_Owner.GetRecord(num2));
				}
			}

			// Token: 0x06001556 RID: 5462 RVA: 0x000630E0 File Offset: 0x000612E0
			internal unsafe Record(InputStateHistory owner, int index, InputStateHistory.RecordHeader* header)
			{
				this.m_Owner = owner;
				this.m_IndexPlusOne = index + 1;
				this.m_Version = header->version;
			}

			// Token: 0x06001557 RID: 5463 RVA: 0x000630FE File Offset: 0x000612FE
			public TValue ReadValue<TValue>() where TValue : struct
			{
				this.CheckValid();
				return this.m_Owner.ReadValue<TValue>(this.header);
			}

			// Token: 0x06001558 RID: 5464 RVA: 0x00063117 File Offset: 0x00061317
			public object ReadValueAsObject()
			{
				this.CheckValid();
				return this.m_Owner.ReadValueAsObject(this.header);
			}

			// Token: 0x06001559 RID: 5465 RVA: 0x00063130 File Offset: 0x00061330
			public unsafe void* GetUnsafeMemoryPtr()
			{
				this.CheckValid();
				return this.GetUnsafeMemoryPtrUnchecked();
			}

			// Token: 0x0600155A RID: 5466 RVA: 0x00063140 File Offset: 0x00061340
			internal unsafe void* GetUnsafeMemoryPtrUnchecked()
			{
				if (this.m_Owner.controls.Count == 1 && !this.m_Owner.m_AddNewControls)
				{
					return (void*)this.header->statePtrWithoutControlIndex;
				}
				return (void*)this.header->statePtrWithControlIndex;
			}

			// Token: 0x0600155B RID: 5467 RVA: 0x00063187 File Offset: 0x00061387
			public unsafe void* GetUnsafeExtraMemoryPtr()
			{
				this.CheckValid();
				return this.GetUnsafeExtraMemoryPtrUnchecked();
			}

			// Token: 0x0600155C RID: 5468 RVA: 0x00063195 File Offset: 0x00061395
			internal unsafe void* GetUnsafeExtraMemoryPtrUnchecked()
			{
				if (this.m_Owner.extraMemoryPerRecord == 0)
				{
					throw new InvalidOperationException("No extra memory has been set up for history records; set extraMemoryPerRecord");
				}
				return (void*)(this.header + this.m_Owner.bytesPerRecord / sizeof(InputStateHistory.RecordHeader) - this.m_Owner.extraMemoryPerRecord / sizeof(InputStateHistory.RecordHeader));
			}

			// Token: 0x0600155D RID: 5469 RVA: 0x000631D0 File Offset: 0x000613D0
			public unsafe void CopyFrom(InputStateHistory.Record record)
			{
				if (!record.valid)
				{
					throw new ArgumentException("Given history record is not valid", "record");
				}
				this.CheckValid();
				InputControl control = record.control;
				int num = this.m_Owner.controls.IndexOfReference(control);
				if (num == -1)
				{
					if (!this.m_Owner.m_AddNewControls)
					{
						throw new InvalidOperationException(string.Format("Control '{0}' is not tracked by target history", record.control));
					}
					num = ArrayHelpers.AppendWithCapacity<InputControl>(ref this.m_Owner.m_Controls, ref this.m_Owner.m_ControlCount, control, 10);
				}
				int stateSizeInBytes = this.m_Owner.m_StateSizeInBytes;
				if (stateSizeInBytes != record.m_Owner.m_StateSizeInBytes)
				{
					throw new InvalidOperationException(string.Format("Cannot copy record from owner with state size '{0}' to owner with state size '{1}'", record.m_Owner.m_StateSizeInBytes, stateSizeInBytes));
				}
				InputStateHistory.RecordHeader* header = this.header;
				InputStateHistory.RecordHeader* header2 = record.header;
				UnsafeUtility.MemCpy((void*)header, (void*)header2, 12L);
				ref InputStateHistory.RecordHeader ptr = ref *header;
				InputStateHistory owner = this.m_Owner;
				uint num2 = owner.m_CurrentVersion + 1U;
				owner.m_CurrentVersion = num2;
				ptr.version = num2;
				this.m_Version = header->version;
				byte* ptr2 = header->statePtrWithoutControlIndex;
				if (this.m_Owner.controls.Count > 1 || this.m_Owner.m_AddNewControls)
				{
					header->controlIndex = num;
					ptr2 = header->statePtrWithControlIndex;
				}
				byte* ptr3 = ((record.m_Owner.m_ControlCount > 1 || record.m_Owner.m_AddNewControls) ? header2->statePtrWithControlIndex : header2->statePtrWithoutControlIndex);
				UnsafeUtility.MemCpy((void*)ptr2, (void*)ptr3, (long)stateSizeInBytes);
				int extraMemoryPerRecord = this.m_Owner.m_ExtraMemoryPerRecord;
				if (extraMemoryPerRecord > 0 && extraMemoryPerRecord == record.m_Owner.m_ExtraMemoryPerRecord)
				{
					UnsafeUtility.MemCpy(this.GetUnsafeExtraMemoryPtr(), record.GetUnsafeExtraMemoryPtr(), (long)extraMemoryPerRecord);
				}
				Action<InputStateHistory.Record> onRecordAdded = this.m_Owner.onRecordAdded;
				if (onRecordAdded == null)
				{
					return;
				}
				onRecordAdded(this);
			}

			// Token: 0x0600155E RID: 5470 RVA: 0x000633A7 File Offset: 0x000615A7
			internal unsafe void CheckValid()
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

			// Token: 0x0600155F RID: 5471 RVA: 0x000633E2 File Offset: 0x000615E2
			public bool Equals(InputStateHistory.Record other)
			{
				return this.m_Owner == other.m_Owner && this.m_IndexPlusOne == other.m_IndexPlusOne && this.m_Version == other.m_Version;
			}

			// Token: 0x06001560 RID: 5472 RVA: 0x00063410 File Offset: 0x00061610
			public override bool Equals(object obj)
			{
				if (obj is InputStateHistory.Record)
				{
					InputStateHistory.Record record = (InputStateHistory.Record)obj;
					return this.Equals(record);
				}
				return false;
			}

			// Token: 0x06001561 RID: 5473 RVA: 0x00063435 File Offset: 0x00061635
			public override int GetHashCode()
			{
				return (((((this.m_Owner != null) ? this.m_Owner.GetHashCode() : 0) * 397) ^ this.m_IndexPlusOne) * 397) ^ (int)this.m_Version;
			}

			// Token: 0x06001562 RID: 5474 RVA: 0x00063467 File Offset: 0x00061667
			public override string ToString()
			{
				if (!this.valid)
				{
					return "<Invalid>";
				}
				return string.Format("{{ control={0} value={1} time={2} }}", this.control, this.ReadValueAsObject(), this.time);
			}

			// Token: 0x04000BCF RID: 3023
			private readonly InputStateHistory m_Owner;

			// Token: 0x04000BD0 RID: 3024
			private readonly int m_IndexPlusOne;

			// Token: 0x04000BD1 RID: 3025
			private uint m_Version;
		}
	}
}
