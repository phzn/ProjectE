using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200012A RID: 298
	public sealed class InputActionTrace : IEnumerable<InputActionTrace.ActionEventPtr>, IEnumerable, IDisposable
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0005008A File Offset: 0x0004E28A
		public InputEventBuffer buffer
		{
			get
			{
				return this.m_EventBuffer;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00050092 File Offset: 0x0004E292
		public int count
		{
			get
			{
				return this.m_EventBuffer.eventCount;
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0005009F File Offset: 0x0004E29F
		public InputActionTrace()
		{
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x000500A7 File Offset: 0x0004E2A7
		public InputActionTrace(InputAction action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			this.SubscribeTo(action);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x000500C4 File Offset: 0x0004E2C4
		public InputActionTrace(InputActionMap actionMap)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			this.SubscribeTo(actionMap);
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x000500E4 File Offset: 0x0004E2E4
		public void SubscribeToAll()
		{
			if (this.m_SubscribedToAll)
			{
				return;
			}
			this.HookOnActionChange();
			this.m_SubscribedToAll = true;
			while (this.m_SubscribedActions.length > 0)
			{
				this.UnsubscribeFrom(this.m_SubscribedActions[this.m_SubscribedActions.length - 1]);
			}
			while (this.m_SubscribedActionMaps.length > 0)
			{
				this.UnsubscribeFrom(this.m_SubscribedActionMaps[this.m_SubscribedActionMaps.length - 1]);
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00050164 File Offset: 0x0004E364
		public void UnsubscribeFromAll()
		{
			if (this.count == 0)
			{
				this.UnhookOnActionChange();
			}
			this.m_SubscribedToAll = false;
			while (this.m_SubscribedActions.length > 0)
			{
				this.UnsubscribeFrom(this.m_SubscribedActions[this.m_SubscribedActions.length - 1]);
			}
			while (this.m_SubscribedActionMaps.length > 0)
			{
				this.UnsubscribeFrom(this.m_SubscribedActionMaps[this.m_SubscribedActionMaps.length - 1]);
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x000501E4 File Offset: 0x0004E3E4
		public void SubscribeTo(InputAction action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (this.m_CallbackDelegate == null)
			{
				this.m_CallbackDelegate = new Action<InputAction.CallbackContext>(this.RecordAction);
			}
			action.performed += this.m_CallbackDelegate;
			action.started += this.m_CallbackDelegate;
			action.canceled += this.m_CallbackDelegate;
			this.m_SubscribedActions.AppendWithCapacity(action, 10);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0005024C File Offset: 0x0004E44C
		public void SubscribeTo(InputActionMap actionMap)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (this.m_CallbackDelegate == null)
			{
				this.m_CallbackDelegate = new Action<InputAction.CallbackContext>(this.RecordAction);
			}
			actionMap.actionTriggered += this.m_CallbackDelegate;
			this.m_SubscribedActionMaps.AppendWithCapacity(actionMap, 10);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0005029C File Offset: 0x0004E49C
		public void UnsubscribeFrom(InputAction action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (this.m_CallbackDelegate == null)
			{
				return;
			}
			action.performed -= this.m_CallbackDelegate;
			action.started -= this.m_CallbackDelegate;
			action.canceled -= this.m_CallbackDelegate;
			int num = this.m_SubscribedActions.IndexOfReference(action);
			if (num != -1)
			{
				this.m_SubscribedActions.RemoveAtWithCapacity(num);
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00050304 File Offset: 0x0004E504
		public void UnsubscribeFrom(InputActionMap actionMap)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (this.m_CallbackDelegate == null)
			{
				return;
			}
			actionMap.actionTriggered -= this.m_CallbackDelegate;
			int num = this.m_SubscribedActionMaps.IndexOfReference(actionMap);
			if (num != -1)
			{
				this.m_SubscribedActionMaps.RemoveAtWithCapacity(num);
			}
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00050354 File Offset: 0x0004E554
		public unsafe void RecordAction(InputAction.CallbackContext context)
		{
			int num = this.m_ActionMapStates.IndexOfReference(context.m_State);
			if (num == -1)
			{
				num = this.m_ActionMapStates.AppendWithCapacity(context.m_State, 10);
			}
			this.HookOnActionChange();
			int valueSizeInBytes = context.valueSizeInBytes;
			ActionEvent* ptr = (ActionEvent*)this.m_EventBuffer.AllocateEvent(ActionEvent.GetEventSizeWithValueSize(valueSizeInBytes), 2048, Allocator.Persistent);
			ref InputActionState.TriggerState ptr2 = ref context.m_State.actionStates[context.m_ActionIndex];
			ptr->baseEvent.type = ActionEvent.Type;
			ptr->baseEvent.time = ptr2.time;
			ptr->stateIndex = num;
			ptr->controlIndex = ptr2.controlIndex;
			ptr->bindingIndex = ptr2.bindingIndex;
			ptr->interactionIndex = ptr2.interactionIndex;
			ptr->startTime = ptr2.startTime;
			ptr->phase = ptr2.phase;
			byte* valueData = ptr->valueData;
			context.ReadValue((void*)valueData, valueSizeInBytes);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00050442 File Offset: 0x0004E642
		public void Clear()
		{
			this.m_EventBuffer.Reset();
			this.m_ActionMapStates.ClearWithCapacity();
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0005045C File Offset: 0x0004E65C
		~InputActionTrace()
		{
			this.DisposeInternal();
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00050488 File Offset: 0x0004E688
		public override string ToString()
		{
			if (this.count == 0)
			{
				return "[]";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			bool flag = true;
			foreach (InputActionTrace.ActionEventPtr actionEventPtr in this)
			{
				if (!flag)
				{
					stringBuilder.Append(",\n");
				}
				stringBuilder.Append(actionEventPtr.ToString());
				flag = false;
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0005051C File Offset: 0x0004E71C
		public void Dispose()
		{
			this.UnsubscribeFromAll();
			this.DisposeInternal();
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0005052C File Offset: 0x0004E72C
		private void DisposeInternal()
		{
			for (int i = 0; i < this.m_ActionMapStateClones.length; i++)
			{
				this.m_ActionMapStateClones[i].Dispose();
			}
			this.m_EventBuffer.Dispose();
			this.m_ActionMapStates.Clear();
			this.m_ActionMapStateClones.Clear();
			if (this.m_ActionChangeDelegate != null)
			{
				InputSystem.onActionChange -= this.m_ActionChangeDelegate;
				this.m_ActionChangeDelegate = null;
			}
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0005059B File Offset: 0x0004E79B
		public IEnumerator<InputActionTrace.ActionEventPtr> GetEnumerator()
		{
			return new InputActionTrace.Enumerator(this);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x000505A8 File Offset: 0x0004E7A8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x000505B0 File Offset: 0x0004E7B0
		private void HookOnActionChange()
		{
			if (this.m_OnActionChangeHooked)
			{
				return;
			}
			if (this.m_ActionChangeDelegate == null)
			{
				this.m_ActionChangeDelegate = new Action<object, InputActionChange>(this.OnActionChange);
			}
			InputSystem.onActionChange += this.m_ActionChangeDelegate;
			this.m_OnActionChangeHooked = true;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x000505E7 File Offset: 0x0004E7E7
		private void UnhookOnActionChange()
		{
			if (!this.m_OnActionChangeHooked)
			{
				return;
			}
			InputSystem.onActionChange -= this.m_ActionChangeDelegate;
			this.m_OnActionChangeHooked = false;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00050604 File Offset: 0x0004E804
		private void OnActionChange(object actionOrMapOrAsset, InputActionChange change)
		{
			if (this.m_SubscribedToAll && change - InputActionChange.ActionStarted <= 2)
			{
				InputAction inputAction = (InputAction)actionOrMapOrAsset;
				int actionIndexInState = inputAction.m_ActionIndexInState;
				InputActionState state = inputAction.m_ActionMap.m_State;
				InputAction.CallbackContext callbackContext = new InputAction.CallbackContext
				{
					m_State = state,
					m_ActionIndex = actionIndexInState
				};
				this.RecordAction(callbackContext);
				return;
			}
			if (change != InputActionChange.BoundControlsAboutToChange)
			{
				return;
			}
			InputAction inputAction2 = actionOrMapOrAsset as InputAction;
			if (inputAction2 != null)
			{
				this.CloneActionStateBeforeBindingsChange(inputAction2.m_ActionMap);
				return;
			}
			InputActionMap inputActionMap = actionOrMapOrAsset as InputActionMap;
			if (inputActionMap != null)
			{
				this.CloneActionStateBeforeBindingsChange(inputActionMap);
				return;
			}
			InputActionAsset inputActionAsset = actionOrMapOrAsset as InputActionAsset;
			if (inputActionAsset != null)
			{
				foreach (InputActionMap inputActionMap2 in inputActionAsset.actionMaps)
				{
					this.CloneActionStateBeforeBindingsChange(inputActionMap2);
				}
			}
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000506E8 File Offset: 0x0004E8E8
		private void CloneActionStateBeforeBindingsChange(InputActionMap actionMap)
		{
			InputActionState state = actionMap.m_State;
			if (state == null)
			{
				return;
			}
			int num = this.m_ActionMapStates.IndexOfReference(state);
			if (num == -1)
			{
				return;
			}
			InputActionState inputActionState = state.Clone();
			this.m_ActionMapStateClones.Append(inputActionState);
			this.m_ActionMapStates[num] = inputActionState;
		}

		// Token: 0x040006FC RID: 1788
		private bool m_SubscribedToAll;

		// Token: 0x040006FD RID: 1789
		private bool m_OnActionChangeHooked;

		// Token: 0x040006FE RID: 1790
		private InlinedArray<InputAction> m_SubscribedActions;

		// Token: 0x040006FF RID: 1791
		private InlinedArray<InputActionMap> m_SubscribedActionMaps;

		// Token: 0x04000700 RID: 1792
		private InputEventBuffer m_EventBuffer;

		// Token: 0x04000701 RID: 1793
		private InlinedArray<InputActionState> m_ActionMapStates;

		// Token: 0x04000702 RID: 1794
		private InlinedArray<InputActionState> m_ActionMapStateClones;

		// Token: 0x04000703 RID: 1795
		private Action<InputAction.CallbackContext> m_CallbackDelegate;

		// Token: 0x04000704 RID: 1796
		private Action<object, InputActionChange> m_ActionChangeDelegate;

		// Token: 0x02000240 RID: 576
		public struct ActionEventPtr
		{
			// Token: 0x170005F3 RID: 1523
			// (get) Token: 0x06001604 RID: 5636 RVA: 0x0006578C File Offset: 0x0006398C
			public unsafe InputAction action
			{
				get
				{
					return this.m_State.GetActionOrNull(this.m_Ptr->bindingIndex);
				}
			}

			// Token: 0x170005F4 RID: 1524
			// (get) Token: 0x06001605 RID: 5637 RVA: 0x000657A4 File Offset: 0x000639A4
			public unsafe InputActionPhase phase
			{
				get
				{
					return this.m_Ptr->phase;
				}
			}

			// Token: 0x170005F5 RID: 1525
			// (get) Token: 0x06001606 RID: 5638 RVA: 0x000657B1 File Offset: 0x000639B1
			public unsafe InputControl control
			{
				get
				{
					return this.m_State.controls[this.m_Ptr->controlIndex];
				}
			}

			// Token: 0x170005F6 RID: 1526
			// (get) Token: 0x06001607 RID: 5639 RVA: 0x000657CC File Offset: 0x000639CC
			public unsafe IInputInteraction interaction
			{
				get
				{
					int interactionIndex = this.m_Ptr->interactionIndex;
					if (interactionIndex == -1)
					{
						return null;
					}
					return this.m_State.interactions[interactionIndex];
				}
			}

			// Token: 0x170005F7 RID: 1527
			// (get) Token: 0x06001608 RID: 5640 RVA: 0x000657F8 File Offset: 0x000639F8
			public unsafe double time
			{
				get
				{
					return this.m_Ptr->baseEvent.time;
				}
			}

			// Token: 0x170005F8 RID: 1528
			// (get) Token: 0x06001609 RID: 5641 RVA: 0x0006580A File Offset: 0x00063A0A
			public unsafe double startTime
			{
				get
				{
					return this.m_Ptr->startTime;
				}
			}

			// Token: 0x170005F9 RID: 1529
			// (get) Token: 0x0600160A RID: 5642 RVA: 0x00065817 File Offset: 0x00063A17
			public double duration
			{
				get
				{
					return this.time - this.startTime;
				}
			}

			// Token: 0x170005FA RID: 1530
			// (get) Token: 0x0600160B RID: 5643 RVA: 0x00065826 File Offset: 0x00063A26
			public unsafe int valueSizeInBytes
			{
				get
				{
					return this.m_Ptr->valueSizeInBytes;
				}
			}

			// Token: 0x0600160C RID: 5644 RVA: 0x00065834 File Offset: 0x00063A34
			public unsafe object ReadValueAsObject()
			{
				if (this.m_Ptr == null)
				{
					throw new InvalidOperationException("ActionEventPtr is invalid");
				}
				byte* valueData = this.m_Ptr->valueData;
				int bindingIndex = this.m_Ptr->bindingIndex;
				if (!this.m_State.bindingStates[bindingIndex].isPartOfComposite)
				{
					int valueSizeInBytes = this.m_Ptr->valueSizeInBytes;
					return this.control.ReadValueFromBufferAsObject((void*)valueData, valueSizeInBytes);
				}
				int compositeOrCompositeBindingIndex = this.m_State.bindingStates[bindingIndex].compositeOrCompositeBindingIndex;
				int compositeOrCompositeBindingIndex2 = this.m_State.bindingStates[compositeOrCompositeBindingIndex].compositeOrCompositeBindingIndex;
				InputBindingComposite inputBindingComposite = this.m_State.composites[compositeOrCompositeBindingIndex2];
				Type valueType = inputBindingComposite.valueType;
				if (valueType == null)
				{
					throw new InvalidOperationException(string.Format("Cannot read value from Composite '{0}' which does not have a valueType set", inputBindingComposite));
				}
				return Marshal.PtrToStructure(new IntPtr((void*)valueData), valueType);
			}

			// Token: 0x0600160D RID: 5645 RVA: 0x00065920 File Offset: 0x00063B20
			public unsafe void ReadValue(void* buffer, int bufferSize)
			{
				int valueSizeInBytes = this.m_Ptr->valueSizeInBytes;
				if (bufferSize < valueSizeInBytes)
				{
					throw new ArgumentException(string.Format("Expected buffer of at least {0} bytes but got buffer of just {1} bytes instead", valueSizeInBytes, bufferSize), "bufferSize");
				}
				UnsafeUtility.MemCpy(buffer, (void*)this.m_Ptr->valueData, (long)valueSizeInBytes);
			}

			// Token: 0x0600160E RID: 5646 RVA: 0x00065974 File Offset: 0x00063B74
			public unsafe TValue ReadValue<TValue>() where TValue : struct
			{
				int valueSizeInBytes = this.m_Ptr->valueSizeInBytes;
				if (UnsafeUtility.SizeOf<TValue>() != valueSizeInBytes)
				{
					throw new InvalidOperationException(string.Format("Cannot read a value of type '{0}' with size {1} from event on action '{2}' with value size {3}", new object[]
					{
						typeof(TValue).Name,
						UnsafeUtility.SizeOf<TValue>(),
						this.action,
						valueSizeInBytes
					}));
				}
				TValue tvalue = new TValue();
				UnsafeUtility.MemCpy(UnsafeUtility.AddressOf<TValue>(ref tvalue), (void*)this.m_Ptr->valueData, (long)valueSizeInBytes);
				return tvalue;
			}

			// Token: 0x0600160F RID: 5647 RVA: 0x000659FC File Offset: 0x00063BFC
			public override string ToString()
			{
				if (this.m_Ptr == null)
				{
					return "<null>";
				}
				string text = ((this.action.actionMap != null) ? (this.action.actionMap.name + "/" + this.action.name) : this.action.name);
				return string.Format("{{ action={0} phase={1} time={2} control={3} value={4} interaction={5} duration={6} }}", new object[]
				{
					text,
					this.phase,
					this.time,
					this.control,
					this.ReadValueAsObject(),
					this.interaction,
					this.duration
				});
			}

			// Token: 0x04000C63 RID: 3171
			internal InputActionState m_State;

			// Token: 0x04000C64 RID: 3172
			internal unsafe ActionEvent* m_Ptr;
		}

		// Token: 0x02000241 RID: 577
		private struct Enumerator : IEnumerator<InputActionTrace.ActionEventPtr>, IEnumerator, IDisposable
		{
			// Token: 0x06001610 RID: 5648 RVA: 0x00065AB4 File Offset: 0x00063CB4
			public unsafe Enumerator(InputActionTrace trace)
			{
				this.m_Trace = trace;
				this.m_Buffer = (ActionEvent*)trace.m_EventBuffer.bufferPtr.data;
				this.m_EventCount = trace.m_EventBuffer.eventCount;
				this.m_CurrentEvent = null;
				this.m_CurrentIndex = 0;
			}

			// Token: 0x06001611 RID: 5649 RVA: 0x00065B04 File Offset: 0x00063D04
			public unsafe bool MoveNext()
			{
				if (this.m_CurrentIndex == this.m_EventCount)
				{
					return false;
				}
				if (this.m_CurrentEvent == null)
				{
					this.m_CurrentEvent = this.m_Buffer;
					return this.m_CurrentEvent != null;
				}
				this.m_CurrentIndex++;
				if (this.m_CurrentIndex == this.m_EventCount)
				{
					return false;
				}
				this.m_CurrentEvent = (ActionEvent*)InputEvent.GetNextInMemory((InputEvent*)this.m_CurrentEvent);
				return true;
			}

			// Token: 0x06001612 RID: 5650 RVA: 0x00065B75 File Offset: 0x00063D75
			public void Reset()
			{
				this.m_CurrentEvent = null;
				this.m_CurrentIndex = 0;
			}

			// Token: 0x06001613 RID: 5651 RVA: 0x00065B86 File Offset: 0x00063D86
			public void Dispose()
			{
			}

			// Token: 0x170005FB RID: 1531
			// (get) Token: 0x06001614 RID: 5652 RVA: 0x00065B88 File Offset: 0x00063D88
			public unsafe InputActionTrace.ActionEventPtr Current
			{
				get
				{
					InputActionState inputActionState = this.m_Trace.m_ActionMapStates[this.m_CurrentEvent->stateIndex];
					return new InputActionTrace.ActionEventPtr
					{
						m_State = inputActionState,
						m_Ptr = this.m_CurrentEvent
					};
				}
			}

			// Token: 0x170005FC RID: 1532
			// (get) Token: 0x06001615 RID: 5653 RVA: 0x00065BCF File Offset: 0x00063DCF
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000C65 RID: 3173
			private readonly InputActionTrace m_Trace;

			// Token: 0x04000C66 RID: 3174
			private unsafe readonly ActionEvent* m_Buffer;

			// Token: 0x04000C67 RID: 3175
			private readonly int m_EventCount;

			// Token: 0x04000C68 RID: 3176
			private unsafe ActionEvent* m_CurrentEvent;

			// Token: 0x04000C69 RID: 3177
			private int m_CurrentIndex;
		}
	}
}
