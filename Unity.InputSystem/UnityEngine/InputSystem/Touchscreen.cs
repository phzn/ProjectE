using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000056 RID: 86
	[InputControlLayout(stateType = typeof(TouchscreenState), isGenericTypeOfDevice = true)]
	public class Touchscreen : Pointer, IInputStateCallbackReceiver, IEventMerger, ICustomDeviceReset
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0002E222 File Offset: 0x0002C422
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x0002E22A File Offset: 0x0002C42A
		public TouchControl primaryTouch { get; protected set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0002E233 File Offset: 0x0002C433
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x0002E23B File Offset: 0x0002C43B
		public ReadOnlyArray<TouchControl> touches { get; protected set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0002E244 File Offset: 0x0002C444
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0002E251 File Offset: 0x0002C451
		protected TouchControl[] touchControlArray
		{
			get
			{
				return this.touches.m_Array;
			}
			set
			{
				this.touches = new ReadOnlyArray<TouchControl>(value);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0002E25F File Offset: 0x0002C45F
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0002E266 File Offset: 0x0002C466
		public new static Touchscreen current { get; internal set; }

		// Token: 0x0600084D RID: 2125 RVA: 0x0002E26E File Offset: 0x0002C46E
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Touchscreen.current = this;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002E27C File Offset: 0x0002C47C
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Touchscreen.current == this)
			{
				Touchscreen.current = null;
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0002E294 File Offset: 0x0002C494
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.primaryTouch = base.GetChildControl<TouchControl>("primaryTouch");
			int num = 0;
			using (ReadOnlyArray<InputControl>.Enumerator enumerator = base.children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is TouchControl)
					{
						num++;
					}
				}
			}
			if (num >= 1)
			{
				num--;
			}
			TouchControl[] array = new TouchControl[num];
			int num2 = 0;
			foreach (InputControl inputControl in base.children)
			{
				if (inputControl != this.primaryTouch)
				{
					TouchControl touchControl = inputControl as TouchControl;
					if (touchControl != null)
					{
						array[num2++] = touchControl;
					}
				}
			}
			this.touches = new ReadOnlyArray<TouchControl>(array);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002E384 File Offset: 0x0002C584
		protected new unsafe void OnNextUpdate()
		{
			void* currentStatePtr = base.currentStatePtr;
			TouchState* ptr = (TouchState*)((byte*)((byte*)currentStatePtr + base.stateBlock.byteOffset) + 56);
			int i = 0;
			while (i < this.touches.Count)
			{
				if (ptr->delta != default(Vector2))
				{
					InputState.Change<Vector2>(this.touches[i].delta, Vector2.zero, InputUpdateType.None, default(InputEventPtr));
				}
				if (ptr->tapCount > 0 && InputState.currentTime >= ptr->startTime + (double)Touchscreen.s_TapTime + (double)Touchscreen.s_TapDelayTime)
				{
					InputState.Change<byte>(this.touches[i].tapCount, 0, InputUpdateType.None, default(InputEventPtr));
				}
				i++;
				ptr++;
			}
			TouchState* ptr2 = (TouchState*)((byte*)currentStatePtr + base.stateBlock.byteOffset);
			if (ptr2->delta != default(Vector2))
			{
				InputState.Change<Vector2>(this.primaryTouch.delta, Vector2.zero, InputUpdateType.None, default(InputEventPtr));
			}
			if (ptr2->tapCount > 0 && InputState.currentTime >= ptr2->startTime + (double)Touchscreen.s_TapTime + (double)Touchscreen.s_TapDelayTime)
			{
				InputState.Change<byte>(this.primaryTouch.tapCount, 0, InputUpdateType.None, default(InputEventPtr));
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002E4F0 File Offset: 0x0002C6F0
		protected new unsafe void OnStateEvent(InputEventPtr eventPtr)
		{
			if (eventPtr.type == 1145852993)
			{
				return;
			}
			StateEvent* ptr = StateEvent.FromUnchecked(eventPtr);
			if (ptr->stateFormat != TouchState.Format)
			{
				InputState.Change(this, eventPtr, InputUpdateType.None);
				return;
			}
			void* currentStatePtr = base.currentStatePtr;
			TouchState* ptr2 = (TouchState*)((byte*)currentStatePtr + this.touches[0].stateBlock.byteOffset);
			TouchState* ptr3 = (TouchState*)((byte*)currentStatePtr + this.primaryTouch.stateBlock.byteOffset);
			int count = this.touches.Count;
			TouchState touchState;
			if (ptr->stateSizeInBytes == 56U)
			{
				touchState = *(TouchState*)ptr->state;
			}
			else
			{
				touchState = default(TouchState);
				UnsafeUtility.MemCpy(UnsafeUtility.AddressOf<TouchState>(ref touchState), ptr->state, (long)((ulong)ptr->stateSizeInBytes));
			}
			touchState.tapCount = 0;
			touchState.isTapPress = false;
			touchState.isTapRelease = false;
			touchState.updateStepCount = InputUpdate.s_UpdateStepCount;
			if (touchState.phase != TouchPhase.Began)
			{
				int touchId = touchState.touchId;
				int i = 0;
				while (i < count)
				{
					if (ptr2[i].touchId == touchId)
					{
						bool isPrimaryTouch = ptr2[i].isPrimaryTouch;
						touchState.isPrimaryTouch = isPrimaryTouch;
						if (touchState.delta == default(Vector2))
						{
							touchState.delta = touchState.position - ptr2[i].position;
						}
						touchState.delta += ptr2[i].delta;
						touchState.startTime = ptr2[i].startTime;
						touchState.startPosition = ptr2[i].startPosition;
						bool flag = touchState.isNoneEndedOrCanceled && eventPtr.time - touchState.startTime <= (double)Touchscreen.s_TapTime && (touchState.position - touchState.startPosition).sqrMagnitude <= Touchscreen.s_TapRadiusSquared;
						if (flag)
						{
							touchState.tapCount = ptr2[i].tapCount + 1;
						}
						else
						{
							touchState.tapCount = ptr2[i].tapCount;
						}
						if (isPrimaryTouch)
						{
							if (touchState.isNoneEndedOrCanceled)
							{
								touchState.isPrimaryTouch = false;
								bool flag2 = false;
								for (int j = 0; j < count; j++)
								{
									if (j != i && ptr2[j].isInProgress)
									{
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									if (flag)
									{
										Touchscreen.TriggerTap(this.primaryTouch, ref touchState, eventPtr);
									}
									else
									{
										InputState.Change<TouchState>(this.primaryTouch, ref touchState, InputUpdateType.None, eventPtr);
									}
								}
								else
								{
									TouchState touchState2 = touchState;
									touchState2.phase = TouchPhase.Moved;
									touchState2.isOrphanedPrimaryTouch = true;
									InputState.Change<TouchState>(this.primaryTouch, ref touchState2, InputUpdateType.None, eventPtr);
								}
							}
							else
							{
								InputState.Change<TouchState>(this.primaryTouch, ref touchState, InputUpdateType.None, eventPtr);
							}
						}
						else if (touchState.isNoneEndedOrCanceled && ptr3->isOrphanedPrimaryTouch)
						{
							bool flag3 = false;
							for (int k = 0; k < count; k++)
							{
								if (k != i && ptr2[k].isInProgress)
								{
									flag3 = true;
									break;
								}
							}
							if (!flag3)
							{
								ptr3->isOrphanedPrimaryTouch = false;
								InputState.Change<byte>(this.primaryTouch.phase, 3, InputUpdateType.None, default(InputEventPtr));
							}
						}
						if (flag)
						{
							Touchscreen.TriggerTap(this.touches[i], ref touchState, eventPtr);
							return;
						}
						InputState.Change<TouchState>(this.touches[i], ref touchState, InputUpdateType.None, eventPtr);
						return;
					}
					else
					{
						i++;
					}
				}
				return;
			}
			int l = 0;
			while (l < count)
			{
				if (ptr2->isNoneEndedOrCanceled)
				{
					touchState.delta = Vector2.zero;
					touchState.startTime = eventPtr.time;
					touchState.startPosition = touchState.position;
					touchState.isPrimaryTouch = false;
					touchState.isOrphanedPrimaryTouch = false;
					touchState.isTap = false;
					touchState.tapCount = ptr2->tapCount;
					if (ptr3->isNoneEndedOrCanceled)
					{
						touchState.isPrimaryTouch = true;
						InputState.Change<TouchState>(this.primaryTouch, ref touchState, InputUpdateType.None, eventPtr);
					}
					InputState.Change<TouchState>(this.touches[l], ref touchState, InputUpdateType.None, eventPtr);
					return;
				}
				l++;
				ptr2++;
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0002E962 File Offset: 0x0002CB62
		void IInputStateCallbackReceiver.OnNextUpdate()
		{
			this.OnNextUpdate();
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0002E96A File Offset: 0x0002CB6A
		void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
		{
			this.OnStateEvent(eventPtr);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002E974 File Offset: 0x0002CB74
		unsafe bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
		{
			if (!eventPtr.IsA<StateEvent>())
			{
				return false;
			}
			StateEvent* ptr = StateEvent.FromUnchecked(eventPtr);
			if (ptr->stateFormat != TouchState.Format)
			{
				return false;
			}
			if (control == null)
			{
				TouchState* ptr2 = (TouchState*)((byte*)base.currentStatePtr + this.touches[0].stateBlock.byteOffset);
				TouchState* state = (TouchState*)ptr->state;
				int touchId = state->touchId;
				TouchPhase phase = state->phase;
				int count = this.touches.Count;
				for (int i = 0; i < count; i++)
				{
					TouchState* ptr3 = ptr2 + i;
					if (ptr3->touchId == touchId || (!ptr3->isInProgress && phase.IsActive()))
					{
						offset = this.primaryTouch.m_StateBlock.byteOffset + this.primaryTouch.m_StateBlock.alignedSizeInBytes - this.m_StateBlock.byteOffset + (uint)(i * UnsafeUtility.SizeOf<TouchState>());
						return true;
					}
				}
				return false;
			}
			TouchControl touchControl = control.FindInParentChain<TouchControl>();
			if (touchControl == null || touchControl.parent != this)
			{
				return false;
			}
			if (touchControl != this.primaryTouch)
			{
				return false;
			}
			offset = touchControl.stateBlock.byteOffset - this.m_StateBlock.byteOffset;
			return true;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0002EAB4 File Offset: 0x0002CCB4
		unsafe void ICustomDeviceReset.Reset()
		{
			void* currentStatePtr = base.currentStatePtr;
			using (NativeArray<byte> nativeArray = new NativeArray<byte>(StateEvent.GetEventSizeWithPayload<TouchState>(), Allocator.Temp, NativeArrayOptions.ClearMemory))
			{
				StateEvent* unsafePtr = (StateEvent*)nativeArray.GetUnsafePtr<byte>();
				unsafePtr->baseEvent = new InputEvent(1398030676, nativeArray.Length, base.deviceId, -1.0);
				TouchState* ptr = (TouchState*)((byte*)currentStatePtr + this.primaryTouch.stateBlock.byteOffset);
				if (ptr->phase.IsActive())
				{
					UnsafeUtility.MemCpy(unsafePtr->state, (void*)ptr, (long)UnsafeUtility.SizeOf<TouchState>());
					((TouchState*)unsafePtr->state)->phase = TouchPhase.Canceled;
					InputState.Change<TouchPhase>(this.primaryTouch.phase, TouchPhase.Canceled, InputUpdateType.None, new InputEventPtr((InputEvent*)unsafePtr));
				}
				TouchState* ptr2 = (TouchState*)((byte*)currentStatePtr + this.touches[0].stateBlock.byteOffset);
				int count = this.touches.Count;
				for (int i = 0; i < count; i++)
				{
					if (ptr2[i].phase.IsActive())
					{
						UnsafeUtility.MemCpy(unsafePtr->state, (void*)(ptr2 + i), (long)UnsafeUtility.SizeOf<TouchState>());
						((TouchState*)unsafePtr->state)->phase = TouchPhase.Canceled;
						InputState.Change<TouchPhase>(this.touches[i].phase, TouchPhase.Canceled, InputUpdateType.None, new InputEventPtr((InputEvent*)unsafePtr));
					}
				}
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0002EC3C File Offset: 0x0002CE3C
		internal unsafe static bool MergeForward(InputEventPtr currentEventPtr, InputEventPtr nextEventPtr)
		{
			if (currentEventPtr.type != 1398030676 || nextEventPtr.type != 1398030676)
			{
				return false;
			}
			StateEvent* ptr = StateEvent.FromUnchecked(currentEventPtr);
			StateEvent* ptr2 = StateEvent.FromUnchecked(nextEventPtr);
			if (ptr->stateFormat != TouchState.Format || ptr2->stateFormat != TouchState.Format)
			{
				return false;
			}
			TouchState* state = (TouchState*)ptr->state;
			TouchState* state2 = (TouchState*)ptr2->state;
			if (state->touchId != state2->touchId || state->phaseId != state2->phaseId || state->flags != state2->flags)
			{
				return false;
			}
			state2->delta += state->delta;
			return true;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0002ED06 File Offset: 0x0002CF06
		bool IEventMerger.MergeForward(InputEventPtr currentEventPtr, InputEventPtr nextEventPtr)
		{
			return Touchscreen.MergeForward(currentEventPtr, nextEventPtr);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0002ED0F File Offset: 0x0002CF0F
		private static void TriggerTap(TouchControl control, ref TouchState state, InputEventPtr eventPtr)
		{
			state.isTapPress = true;
			state.isTapRelease = false;
			InputState.Change<TouchState>(control, ref state, InputUpdateType.None, eventPtr);
			state.isTapPress = false;
			state.isTapRelease = true;
			InputState.Change<TouchState>(control, ref state, InputUpdateType.None, eventPtr);
			state.isTapRelease = false;
		}

		// Token: 0x04000278 RID: 632
		private static readonly ProfilerMarker k_TouchscreenUpdateMarker = new ProfilerMarker("Touchscreen.OnNextUpdate");

		// Token: 0x04000279 RID: 633
		private static readonly ProfilerMarker k_TouchAllocateMarker = new ProfilerMarker("TouchAllocate");

		// Token: 0x0400027B RID: 635
		internal static float s_TapTime;

		// Token: 0x0400027C RID: 636
		internal static float s_TapDelayTime;

		// Token: 0x0400027D RID: 637
		internal static float s_TapRadiusSquared;
	}
}
