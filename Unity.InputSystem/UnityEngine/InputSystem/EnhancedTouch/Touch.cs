using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.EnhancedTouch
{
	// Token: 0x0200009F RID: 159
	public struct Touch : IEquatable<Touch>
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00041FC6 File Offset: 0x000401C6
		public bool valid
		{
			get
			{
				return this.m_TouchRecord.valid;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00041FD3 File Offset: 0x000401D3
		public Finger finger
		{
			get
			{
				return this.m_Finger;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00041FDB File Offset: 0x000401DB
		public TouchPhase phase
		{
			get
			{
				return this.state.phase;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00041FE8 File Offset: 0x000401E8
		public bool began
		{
			get
			{
				return this.phase == TouchPhase.Began;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00041FF3 File Offset: 0x000401F3
		public bool inProgress
		{
			get
			{
				return this.phase == TouchPhase.Moved || this.phase == TouchPhase.Stationary || this.phase == TouchPhase.Began;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00042012 File Offset: 0x00040212
		public bool ended
		{
			get
			{
				return this.phase == TouchPhase.Ended || this.phase == TouchPhase.Canceled;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00042028 File Offset: 0x00040228
		public int touchId
		{
			get
			{
				return this.state.touchId;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x00042035 File Offset: 0x00040235
		public float pressure
		{
			get
			{
				return this.state.pressure;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00042042 File Offset: 0x00040242
		public Vector2 radius
		{
			get
			{
				return this.state.radius;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0004204F File Offset: 0x0004024F
		public double startTime
		{
			get
			{
				return this.state.startTime;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x0004205C File Offset: 0x0004025C
		public double time
		{
			get
			{
				return this.m_TouchRecord.time;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00042069 File Offset: 0x00040269
		public Touchscreen screen
		{
			get
			{
				return this.finger.screen;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00042076 File Offset: 0x00040276
		public Vector2 screenPosition
		{
			get
			{
				return this.state.position;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00042083 File Offset: 0x00040283
		public Vector2 startScreenPosition
		{
			get
			{
				return this.state.startPosition;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00042090 File Offset: 0x00040290
		public Vector2 delta
		{
			get
			{
				return this.state.delta;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0004209D File Offset: 0x0004029D
		public int tapCount
		{
			get
			{
				return (int)this.state.tapCount;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x000420AA File Offset: 0x000402AA
		public bool isTap
		{
			get
			{
				return this.state.isTap;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x000420B7 File Offset: 0x000402B7
		public int displayIndex
		{
			get
			{
				return (int)this.state.displayIndex;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x000420C4 File Offset: 0x000402C4
		public bool isInProgress
		{
			get
			{
				TouchPhase phase = this.phase;
				return phase - TouchPhase.Began <= 1 || phase == TouchPhase.Stationary;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x000420E5 File Offset: 0x000402E5
		internal uint updateStepCount
		{
			get
			{
				return this.state.updateStepCount;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x000420F2 File Offset: 0x000402F2
		internal uint uniqueId
		{
			get
			{
				return this.extraData.uniqueId;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x000420FF File Offset: 0x000402FF
		private unsafe ref TouchState state
		{
			get
			{
				return ref *(TouchState*)this.m_TouchRecord.GetUnsafeMemoryPtr();
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x0004210C File Offset: 0x0004030C
		private unsafe ref Touch.ExtraDataPerTouchState extraData
		{
			get
			{
				return ref *(Touch.ExtraDataPerTouchState*)this.m_TouchRecord.GetUnsafeExtraMemoryPtr();
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00042119 File Offset: 0x00040319
		public TouchHistory history
		{
			get
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Touch is invalid");
				}
				return this.finger.GetTouchHistory(this);
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0004213F File Offset: 0x0004033F
		public static ReadOnlyArray<Touch> activeTouches
		{
			get
			{
				Touch.s_GlobalState.playerState.UpdateActiveTouches();
				return new ReadOnlyArray<Touch>(Touch.s_GlobalState.playerState.activeTouches, 0, Touch.s_GlobalState.playerState.activeTouchCount);
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00042174 File Offset: 0x00040374
		public static ReadOnlyArray<Finger> fingers
		{
			get
			{
				return new ReadOnlyArray<Finger>(Touch.s_GlobalState.playerState.fingers, 0, Touch.s_GlobalState.playerState.totalFingerCount);
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0004219A File Offset: 0x0004039A
		public static ReadOnlyArray<Finger> activeFingers
		{
			get
			{
				Touch.s_GlobalState.playerState.UpdateActiveFingers();
				return new ReadOnlyArray<Finger>(Touch.s_GlobalState.playerState.activeFingers, 0, Touch.s_GlobalState.playerState.activeFingerCount);
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x000421CF File Offset: 0x000403CF
		public static IEnumerable<Touchscreen> screens
		{
			get
			{
				return Touch.s_GlobalState.touchscreens;
			}
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000C5D RID: 3165 RVA: 0x000421E0 File Offset: 0x000403E0
		// (remove) Token: 0x06000C5E RID: 3166 RVA: 0x00042200 File Offset: 0x00040400
		public static event Action<Finger> onFingerDown
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Touch.s_GlobalState.onFingerDown.AddCallback(value);
			}
			remove
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Touch.s_GlobalState.onFingerDown.RemoveCallback(value);
			}
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000C5F RID: 3167 RVA: 0x00042220 File Offset: 0x00040420
		// (remove) Token: 0x06000C60 RID: 3168 RVA: 0x00042240 File Offset: 0x00040440
		public static event Action<Finger> onFingerUp
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Touch.s_GlobalState.onFingerUp.AddCallback(value);
			}
			remove
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Touch.s_GlobalState.onFingerUp.RemoveCallback(value);
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000C61 RID: 3169 RVA: 0x00042260 File Offset: 0x00040460
		// (remove) Token: 0x06000C62 RID: 3170 RVA: 0x00042280 File Offset: 0x00040480
		public static event Action<Finger> onFingerMove
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Touch.s_GlobalState.onFingerMove.AddCallback(value);
			}
			remove
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Touch.s_GlobalState.onFingerMove.RemoveCallback(value);
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x000422A0 File Offset: 0x000404A0
		public static int maxHistoryLengthPerFinger
		{
			get
			{
				return Touch.s_GlobalState.historyLengthPerFinger;
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000422AC File Offset: 0x000404AC
		internal Touch(Finger finger, InputStateHistory<TouchState>.Record touchRecord)
		{
			this.m_Finger = finger;
			this.m_TouchRecord = touchRecord;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x000422BC File Offset: 0x000404BC
		public override string ToString()
		{
			if (!this.valid)
			{
				return "<None>";
			}
			return string.Format("{{id={0} finger={1} phase={2} position={3} delta={4} time={5}}}", new object[]
			{
				this.touchId,
				this.finger.index,
				this.phase,
				this.screenPosition,
				this.delta,
				this.time
			});
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00042340 File Offset: 0x00040540
		public bool Equals(Touch other)
		{
			return object.Equals(this.m_Finger, other.m_Finger) && this.m_TouchRecord.Equals(other.m_TouchRecord);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00042368 File Offset: 0x00040568
		public override bool Equals(object obj)
		{
			if (obj is Touch)
			{
				Touch touch = (Touch)obj;
				return this.Equals(touch);
			}
			return false;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0004238D File Offset: 0x0004058D
		public override int GetHashCode()
		{
			return (((this.m_Finger != null) ? this.m_Finger.GetHashCode() : 0) * 397) ^ this.m_TouchRecord.GetHashCode();
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x000423BD File Offset: 0x000405BD
		internal static void AddTouchscreen(Touchscreen screen)
		{
			Touch.s_GlobalState.touchscreens.AppendWithCapacity(screen, 5);
			Touch.s_GlobalState.playerState.AddFingers(screen);
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000423E4 File Offset: 0x000405E4
		internal static void RemoveTouchscreen(Touchscreen screen)
		{
			int num = Touch.s_GlobalState.touchscreens.IndexOfReference(screen);
			Touch.s_GlobalState.touchscreens.RemoveAtWithCapacity(num);
			Touch.s_GlobalState.playerState.RemoveFingers(screen);
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00042422 File Offset: 0x00040622
		internal static void BeginUpdate()
		{
			if (Touch.s_GlobalState.playerState.haveActiveTouchesNeedingRefreshNextUpdate)
			{
				Touch.s_GlobalState.playerState.haveBuiltActiveTouches = false;
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00042448 File Offset: 0x00040648
		private static Touch.GlobalState CreateGlobalState()
		{
			return new Touch.GlobalState
			{
				historyLengthPerFinger = 64
			};
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00042468 File Offset: 0x00040668
		internal static ISavedState SaveAndResetState()
		{
			ISavedState savedState = new SavedStructState<Touch.GlobalState>(ref Touch.s_GlobalState, delegate(ref Touch.GlobalState state)
			{
				Touch.s_GlobalState = state;
			}, delegate
			{
			});
			Touch.s_GlobalState = Touch.CreateGlobalState();
			return savedState;
		}

		// Token: 0x0400047F RID: 1151
		private readonly Finger m_Finger;

		// Token: 0x04000480 RID: 1152
		internal InputStateHistory<TouchState>.Record m_TouchRecord;

		// Token: 0x04000481 RID: 1153
		internal static Touch.GlobalState s_GlobalState = Touch.CreateGlobalState();

		// Token: 0x020001F8 RID: 504
		internal struct GlobalState
		{
			// Token: 0x04000B33 RID: 2867
			internal InlinedArray<Touchscreen> touchscreens;

			// Token: 0x04000B34 RID: 2868
			internal int historyLengthPerFinger;

			// Token: 0x04000B35 RID: 2869
			internal CallbackArray<Action<Finger>> onFingerDown;

			// Token: 0x04000B36 RID: 2870
			internal CallbackArray<Action<Finger>> onFingerMove;

			// Token: 0x04000B37 RID: 2871
			internal CallbackArray<Action<Finger>> onFingerUp;

			// Token: 0x04000B38 RID: 2872
			internal Touch.FingerAndTouchState playerState;
		}

		// Token: 0x020001F9 RID: 505
		internal struct FingerAndTouchState
		{
			// Token: 0x060014E4 RID: 5348 RVA: 0x000619E8 File Offset: 0x0005FBE8
			public void AddFingers(Touchscreen screen)
			{
				int count = screen.touches.Count;
				ArrayHelpers.EnsureCapacity<Finger>(ref this.fingers, this.totalFingerCount, count, 10);
				for (int i = 0; i < count; i++)
				{
					Finger finger = new Finger(screen, i, this.updateMask);
					ArrayHelpers.AppendWithCapacity<Finger>(ref this.fingers, ref this.totalFingerCount, finger, 10);
				}
			}

			// Token: 0x060014E5 RID: 5349 RVA: 0x00061A48 File Offset: 0x0005FC48
			public void RemoveFingers(Touchscreen screen)
			{
				int count = screen.touches.Count;
				for (int i = 0; i < this.fingers.Length; i++)
				{
					if (this.fingers[i].screen == screen)
					{
						for (int j = 0; j < count; j++)
						{
							this.fingers[i + j].m_StateHistory.Dispose();
						}
						ArrayHelpers.EraseSliceWithCapacity<Finger>(ref this.fingers, ref this.totalFingerCount, i, count);
						break;
					}
				}
				this.haveBuiltActiveTouches = false;
			}

			// Token: 0x060014E6 RID: 5350 RVA: 0x00061AC4 File Offset: 0x0005FCC4
			public void Destroy()
			{
				for (int i = 0; i < this.totalFingerCount; i++)
				{
					this.fingers[i].m_StateHistory.Dispose();
				}
				InputStateHistory<TouchState> inputStateHistory = this.activeTouchState;
				if (inputStateHistory != null)
				{
					inputStateHistory.Dispose();
				}
				this.activeTouchState = null;
			}

			// Token: 0x060014E7 RID: 5351 RVA: 0x00061B0C File Offset: 0x0005FD0C
			public void UpdateActiveFingers()
			{
				this.activeFingerCount = 0;
				for (int i = 0; i < this.totalFingerCount; i++)
				{
					Finger finger = this.fingers[i];
					if (finger.currentTouch.valid)
					{
						ArrayHelpers.AppendWithCapacity<Finger>(ref this.activeFingers, ref this.activeFingerCount, finger, 10);
					}
				}
			}

			// Token: 0x060014E8 RID: 5352 RVA: 0x00061B60 File Offset: 0x0005FD60
			public unsafe void UpdateActiveTouches()
			{
				if (this.haveBuiltActiveTouches)
				{
					return;
				}
				if (this.activeTouchState == null)
				{
					this.activeTouchState = new InputStateHistory<TouchState>(null)
					{
						extraMemoryPerRecord = UnsafeUtility.SizeOf<Touch.ExtraDataPerTouchState>()
					};
				}
				else
				{
					this.activeTouchState.Clear();
					this.activeTouchState.m_ControlCount = 0;
					this.activeTouchState.m_Controls.Clear<InputControl>();
				}
				this.activeTouchCount = 0;
				this.haveActiveTouchesNeedingRefreshNextUpdate = false;
				uint s_UpdateStepCount = InputUpdate.s_UpdateStepCount;
				for (int i = 0; i < this.totalFingerCount; i++)
				{
					ref Finger ptr = ref this.fingers[i];
					InputStateHistory<TouchState> stateHistory = ptr.m_StateHistory;
					int count = stateHistory.Count;
					if (count != 0)
					{
						int num = this.activeTouchCount;
						int num2 = 0;
						TouchState* ptr2 = default(TouchState*);
						int num3 = stateHistory.UserIndexToRecordIndex(count - 1);
						InputStateHistory.RecordHeader* ptr3 = stateHistory.GetRecordUnchecked(num3);
						int bytesPerRecord = stateHistory.bytesPerRecord;
						int num4 = bytesPerRecord - stateHistory.extraMemoryPerRecord;
						for (int j = 0; j < count; j++)
						{
							if (j != 0)
							{
								num3--;
								if (num3 < 0)
								{
									num3 = stateHistory.historyDepth - 1;
									ptr3 = stateHistory.GetRecordUnchecked(num3);
								}
								else
								{
									ptr3 -= bytesPerRecord / sizeof(InputStateHistory.RecordHeader);
								}
							}
							TouchState* statePtrWithoutControlIndex = (TouchState*)ptr3->statePtrWithoutControlIndex;
							bool flag = statePtrWithoutControlIndex->updateStepCount == s_UpdateStepCount;
							if (statePtrWithoutControlIndex->touchId == num2 && !statePtrWithoutControlIndex->phase.IsEndedOrCanceled())
							{
								if (flag && statePtrWithoutControlIndex->phase == TouchPhase.Began)
								{
									ptr2->phase = TouchPhase.Began;
									ptr2->position = statePtrWithoutControlIndex->position;
									ptr2->delta = default(Vector2);
									this.haveActiveTouchesNeedingRefreshNextUpdate = true;
								}
							}
							else
							{
								if (statePtrWithoutControlIndex->phase.IsEndedOrCanceled() && (!statePtrWithoutControlIndex->beganInSameFrame || statePtrWithoutControlIndex->updateStepCount != s_UpdateStepCount - 1U) && !flag)
								{
									break;
								}
								Touch.ExtraDataPerTouchState* ptr4 = (Touch.ExtraDataPerTouchState*)(ptr3 + num4 / sizeof(InputStateHistory.RecordHeader));
								int num5;
								InputStateHistory.RecordHeader* ptr5 = this.activeTouchState.AllocateRecord(out num5);
								TouchState* statePtrWithControlIndex = (TouchState*)ptr5->statePtrWithControlIndex;
								Touch.ExtraDataPerTouchState* ptr6 = (Touch.ExtraDataPerTouchState*)(ptr5 + this.activeTouchState.bytesPerRecord / sizeof(InputStateHistory.RecordHeader) - UnsafeUtility.SizeOf<Touch.ExtraDataPerTouchState>() / sizeof(InputStateHistory.RecordHeader));
								ptr5->time = ptr3->time;
								ptr5->controlIndex = ArrayHelpers.AppendWithCapacity<InputControl>(ref this.activeTouchState.m_Controls, ref this.activeTouchState.m_ControlCount, ptr.m_StateHistory.controls[0], 10);
								UnsafeUtility.MemCpy((void*)statePtrWithControlIndex, (void*)statePtrWithoutControlIndex, (long)UnsafeUtility.SizeOf<TouchState>());
								UnsafeUtility.MemCpy((void*)ptr6, (void*)ptr4, (long)UnsafeUtility.SizeOf<Touch.ExtraDataPerTouchState>());
								TouchPhase phase = statePtrWithoutControlIndex->phase;
								if ((phase == TouchPhase.Moved || phase == TouchPhase.Began) && !flag && (phase != TouchPhase.Moved || !statePtrWithoutControlIndex->beganInSameFrame || statePtrWithoutControlIndex->updateStepCount != s_UpdateStepCount - 1U))
								{
									statePtrWithControlIndex->phase = TouchPhase.Stationary;
									statePtrWithControlIndex->delta = default(Vector2);
								}
								else if (!flag && !statePtrWithoutControlIndex->beganInSameFrame)
								{
									statePtrWithControlIndex->delta = default(Vector2);
								}
								else
								{
									statePtrWithControlIndex->delta = ptr6->accumulatedDelta;
								}
								InputStateHistory<TouchState>.Record record = new InputStateHistory<TouchState>.Record(this.activeTouchState, num5, ptr5);
								Touch touch = new Touch(ptr, record);
								ArrayHelpers.InsertAtWithCapacity<Touch>(ref this.activeTouches, ref this.activeTouchCount, num, touch, 10);
								num2 = statePtrWithoutControlIndex->touchId;
								ptr2 = statePtrWithControlIndex;
								if (touch.phase != TouchPhase.Stationary)
								{
									this.haveActiveTouchesNeedingRefreshNextUpdate = true;
								}
							}
						}
					}
				}
				this.haveBuiltActiveTouches = true;
			}

			// Token: 0x04000B39 RID: 2873
			public InputUpdateType updateMask;

			// Token: 0x04000B3A RID: 2874
			public Finger[] fingers;

			// Token: 0x04000B3B RID: 2875
			public Finger[] activeFingers;

			// Token: 0x04000B3C RID: 2876
			public Touch[] activeTouches;

			// Token: 0x04000B3D RID: 2877
			public int activeFingerCount;

			// Token: 0x04000B3E RID: 2878
			public int activeTouchCount;

			// Token: 0x04000B3F RID: 2879
			public int totalFingerCount;

			// Token: 0x04000B40 RID: 2880
			public uint lastId;

			// Token: 0x04000B41 RID: 2881
			public bool haveBuiltActiveTouches;

			// Token: 0x04000B42 RID: 2882
			public bool haveActiveTouchesNeedingRefreshNextUpdate;

			// Token: 0x04000B43 RID: 2883
			public InputStateHistory<TouchState> activeTouchState;
		}

		// Token: 0x020001FA RID: 506
		internal struct ExtraDataPerTouchState
		{
			// Token: 0x04000B44 RID: 2884
			public Vector2 accumulatedDelta;

			// Token: 0x04000B45 RID: 2885
			public uint uniqueId;
		}
	}
}
