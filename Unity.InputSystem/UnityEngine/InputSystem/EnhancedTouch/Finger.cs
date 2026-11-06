using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.EnhancedTouch
{
	// Token: 0x0200009E RID: 158
	public class Finger
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00041AE7 File Offset: 0x0003FCE7
		public Touchscreen screen { get; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00041AEF File Offset: 0x0003FCEF
		public int index { get; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00041AF8 File Offset: 0x0003FCF8
		public bool isActive
		{
			get
			{
				return this.currentTouch.valid;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00041B14 File Offset: 0x0003FD14
		public Vector2 screenPosition
		{
			get
			{
				Touch lastTouch = this.lastTouch;
				if (!lastTouch.valid)
				{
					return default(Vector2);
				}
				return lastTouch.screenPosition;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00041B44 File Offset: 0x0003FD44
		public Touch lastTouch
		{
			get
			{
				int count = this.m_StateHistory.Count;
				if (count == 0)
				{
					return default(Touch);
				}
				return new Touch(this, this.m_StateHistory[count - 1]);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00041B80 File Offset: 0x0003FD80
		public Touch currentTouch
		{
			get
			{
				Touch lastTouch = this.lastTouch;
				if (!lastTouch.valid)
				{
					return default(Touch);
				}
				if (lastTouch.isInProgress)
				{
					return lastTouch;
				}
				if (lastTouch.updateStepCount == InputUpdate.s_UpdateStepCount)
				{
					return lastTouch;
				}
				return default(Touch);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00041BCB File Offset: 0x0003FDCB
		public TouchHistory touchHistory
		{
			get
			{
				return new TouchHistory(this, this.m_StateHistory, -1, -1);
			}
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00041BDC File Offset: 0x0003FDDC
		internal unsafe Finger(Touchscreen screen, int index, InputUpdateType updateMask)
		{
			this.screen = screen;
			this.index = index;
			this.m_StateHistory = new InputStateHistory<TouchState>(screen.touches[index])
			{
				historyDepth = Touch.maxHistoryLengthPerFinger,
				extraMemoryPerRecord = UnsafeUtility.SizeOf<Touch.ExtraDataPerTouchState>(),
				onRecordAdded = new Action<InputStateHistory.Record>(this.OnTouchRecorded),
				onShouldRecordStateChange = new Func<InputControl, double, InputEventPtr, bool>(Finger.ShouldRecordTouch),
				updateMask = updateMask
			};
			this.m_StateHistory.StartRecording();
			if (screen.touches[index].isInProgress)
			{
				this.m_StateHistory.RecordStateChange(screen.touches[index], *screen.touches[index].value, -1.0);
			}
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00041CB8 File Offset: 0x0003FEB8
		private unsafe static bool ShouldRecordTouch(InputControl control, double time, InputEventPtr eventPtr)
		{
			if (!eventPtr.valid)
			{
				return false;
			}
			FourCC type = eventPtr.type;
			if (type != 1398030676 && type != 1145852993)
			{
				return false;
			}
			TouchState* ptr = (TouchState*)((byte*)control.currentStatePtr + control.stateBlock.byteOffset);
			return !ptr->isTapRelease;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00041D20 File Offset: 0x0003FF20
		private unsafe void OnTouchRecorded(InputStateHistory.Record record)
		{
			int recordIndex = record.recordIndex;
			InputStateHistory.RecordHeader* recordUnchecked = this.m_StateHistory.GetRecordUnchecked(recordIndex);
			TouchState* statePtrWithoutControlIndex = (TouchState*)recordUnchecked->statePtrWithoutControlIndex;
			statePtrWithoutControlIndex->updateStepCount = InputUpdate.s_UpdateStepCount;
			Touch.s_GlobalState.playerState.haveBuiltActiveTouches = false;
			Touch.ExtraDataPerTouchState* ptr = (Touch.ExtraDataPerTouchState*)(recordUnchecked + this.m_StateHistory.bytesPerRecord / sizeof(InputStateHistory.RecordHeader) - UnsafeUtility.SizeOf<Touch.ExtraDataPerTouchState>() / sizeof(InputStateHistory.RecordHeader));
			ref Touch.ExtraDataPerTouchState ptr2 = ref *ptr;
			uint num = Touch.s_GlobalState.playerState.lastId + 1U;
			Touch.s_GlobalState.playerState.lastId = num;
			ptr2.uniqueId = num;
			ptr->accumulatedDelta = statePtrWithoutControlIndex->delta;
			if (statePtrWithoutControlIndex->phase != TouchPhase.Began)
			{
				if (recordIndex != this.m_StateHistory.m_HeadIndex)
				{
					int num2 = ((recordIndex == 0) ? (this.m_StateHistory.historyDepth - 1) : (recordIndex - 1));
					TouchState* statePtrWithoutControlIndex2 = (TouchState*)this.m_StateHistory.GetRecordUnchecked(num2)->statePtrWithoutControlIndex;
					statePtrWithoutControlIndex->delta -= statePtrWithoutControlIndex2->delta;
					statePtrWithoutControlIndex->beganInSameFrame = statePtrWithoutControlIndex2->beganInSameFrame && statePtrWithoutControlIndex2->updateStepCount == statePtrWithoutControlIndex->updateStepCount;
				}
			}
			else
			{
				statePtrWithoutControlIndex->beganInSameFrame = true;
			}
			switch (statePtrWithoutControlIndex->phase)
			{
			case TouchPhase.Began:
				DelegateHelpers.InvokeCallbacksSafe<Finger>(ref Touch.s_GlobalState.onFingerDown, this, "Touch.onFingerDown", null);
				return;
			case TouchPhase.Moved:
				DelegateHelpers.InvokeCallbacksSafe<Finger>(ref Touch.s_GlobalState.onFingerMove, this, "Touch.onFingerMove", null);
				return;
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				DelegateHelpers.InvokeCallbacksSafe<Finger>(ref Touch.s_GlobalState.onFingerUp, this, "Touch.onFingerUp", null);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00041E90 File Offset: 0x00040090
		private unsafe Touch FindTouch(uint uniqueId)
		{
			foreach (InputStateHistory<TouchState>.Record record in this.m_StateHistory)
			{
				if (((Touch.ExtraDataPerTouchState*)record.GetUnsafeExtraMemoryPtrUnchecked())->uniqueId == uniqueId)
				{
					return new Touch(this, record);
				}
			}
			return default(Touch);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00041EFC File Offset: 0x000400FC
		internal unsafe TouchHistory GetTouchHistory(Touch touch)
		{
			InputStateHistory<TouchState>.Record touchRecord = touch.m_TouchRecord;
			if (touchRecord.owner != this.m_StateHistory)
			{
				touch = this.FindTouch(touch.uniqueId);
				if (!touch.valid)
				{
					return default(TouchHistory);
				}
			}
			int touchId = touch.touchId;
			int num = touch.m_TouchRecord.index;
			int num2 = 0;
			if (touch.phase != TouchPhase.Began)
			{
				InputStateHistory<TouchState>.Record record = touch.m_TouchRecord.previous;
				while (record.valid)
				{
					TouchState* unsafeMemoryPtr = (TouchState*)record.GetUnsafeMemoryPtr();
					if (unsafeMemoryPtr->touchId != touchId)
					{
						break;
					}
					num2++;
					if (unsafeMemoryPtr->phase == TouchPhase.Began)
					{
						break;
					}
					record = record.previous;
				}
			}
			if (num2 == 0)
			{
				return default(TouchHistory);
			}
			num--;
			return new TouchHistory(this, this.m_StateHistory, num, num2);
		}

		// Token: 0x0400047E RID: 1150
		internal readonly InputStateHistory<TouchState> m_StateHistory;
	}
}
