using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.EnhancedTouch
{
	// Token: 0x020000A1 RID: 161
	[AddComponentMenu("Input/Debug/Touch Simulation")]
	[ExecuteInEditMode]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/Touch.html#touch-simulation")]
	public class TouchSimulation : MonoBehaviour, IInputStateChangeMonitor
	{
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x000425EB File Offset: 0x000407EB
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x000425F3 File Offset: 0x000407F3
		public Touchscreen simulatedTouchscreen { get; private set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000425FC File Offset: 0x000407FC
		public static TouchSimulation instance
		{
			get
			{
				return TouchSimulation.s_Instance;
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00042604 File Offset: 0x00040804
		public static void Enable()
		{
			if (TouchSimulation.instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.SetActive(false);
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				TouchSimulation.s_Instance = gameObject.AddComponent<TouchSimulation>();
				TouchSimulation.instance.gameObject.SetActive(true);
			}
			TouchSimulation.instance.enabled = true;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00042657 File Offset: 0x00040857
		public static void Disable()
		{
			if (TouchSimulation.instance != null)
			{
				TouchSimulation.instance.enabled = false;
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00042671 File Offset: 0x00040871
		public static void Destroy()
		{
			TouchSimulation.Disable();
			if (TouchSimulation.s_Instance != null)
			{
				Object.Destroy(TouchSimulation.s_Instance.gameObject);
				TouchSimulation.s_Instance = null;
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0004269C File Offset: 0x0004089C
		protected void AddPointer(Pointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			if (this.m_Pointers.ContainsReference(this.m_NumPointers, pointer))
			{
				return;
			}
			ArrayHelpers.AppendWithCapacity<Pointer>(ref this.m_Pointers, ref this.m_NumPointers, pointer, 10);
			ArrayHelpers.Append<Vector2>(ref this.m_CurrentPositions, default(Vector2));
			ArrayHelpers.Append<int>(ref this.m_CurrentDisplayIndices, 0);
			InputSystem.DisableDevice(pointer, true);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0004270C File Offset: 0x0004090C
		protected void RemovePointer(Pointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			int num = this.m_Pointers.IndexOfReference(pointer, this.m_NumPointers);
			if (num == -1)
			{
				return;
			}
			for (int i = 0; i < this.m_Touches.Length; i++)
			{
				ButtonControl buttonControl = this.m_Touches[i];
				if (buttonControl == null || buttonControl.device == pointer)
				{
					this.UpdateTouch(i, num, TouchPhase.Canceled, default(InputEventPtr));
				}
			}
			this.m_Pointers.EraseAtWithCapacity<Pointer>(ref this.m_NumPointers, num);
			ArrayHelpers.EraseAt<Vector2>(ref this.m_CurrentPositions, num);
			ArrayHelpers.EraseAt<int>(ref this.m_CurrentDisplayIndices, num);
			if (pointer.added)
			{
				InputSystem.EnableDevice(pointer);
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x000427B4 File Offset: 0x000409B4
		private unsafe void OnEvent(InputEventPtr eventPtr, InputDevice device)
		{
			if (device == this.simulatedTouchscreen)
			{
				return;
			}
			int num = this.m_Pointers.IndexOfReference(device, this.m_NumPointers);
			if (num < 0)
			{
				return;
			}
			FourCC type = eventPtr.type;
			if (type != 1398030676 && type != 1145852993)
			{
				return;
			}
			Pointer pointer = this.m_Pointers[num];
			Vector2Control position = pointer.position;
			void* statePtrFromStateEventUnchecked = position.GetStatePtrFromStateEventUnchecked(eventPtr, type);
			if (statePtrFromStateEventUnchecked != null)
			{
				this.m_CurrentPositions[num] = position.ReadValueFromState(statePtrFromStateEventUnchecked);
			}
			IntegerControl displayIndex = pointer.displayIndex;
			void* statePtrFromStateEventUnchecked2 = displayIndex.GetStatePtrFromStateEventUnchecked(eventPtr, type);
			if (statePtrFromStateEventUnchecked2 != null)
			{
				this.m_CurrentDisplayIndices[num] = displayIndex.ReadValueFromState(statePtrFromStateEventUnchecked2);
			}
			for (int i = 0; i < this.m_Touches.Length; i++)
			{
				ButtonControl buttonControl = this.m_Touches[i];
				if (buttonControl != null && buttonControl.device == device)
				{
					void* statePtrFromStateEventUnchecked3 = buttonControl.GetStatePtrFromStateEventUnchecked(eventPtr, type);
					if (statePtrFromStateEventUnchecked3 == null)
					{
						if (statePtrFromStateEventUnchecked != null)
						{
							this.UpdateTouch(i, num, TouchPhase.Moved, eventPtr);
						}
					}
					else if (buttonControl.ReadValueFromState(statePtrFromStateEventUnchecked3) < ButtonControl.s_GlobalDefaultButtonPressPoint * ButtonControl.s_GlobalDefaultButtonReleaseThreshold)
					{
						this.UpdateTouch(i, num, TouchPhase.Ended, eventPtr);
					}
				}
			}
			foreach (InputControl inputControl in eventPtr.EnumerateControls(InputControlExtensions.Enumerate.IgnoreControlsInDefaultState, device, 0f))
			{
				if (inputControl.isButton)
				{
					void* statePtrFromStateEventUnchecked4 = inputControl.GetStatePtrFromStateEventUnchecked(eventPtr, type);
					float num2 = 0f;
					inputControl.ReadValueFromStateIntoBuffer(statePtrFromStateEventUnchecked4, UnsafeUtility.AddressOf<float>(ref num2), 4);
					if (num2 > ButtonControl.s_GlobalDefaultButtonPressPoint)
					{
						int num3 = this.m_Touches.IndexOfReference(inputControl, -1);
						if (num3 < 0)
						{
							num3 = this.m_Touches.IndexOfReference(null, -1);
							if (num3 >= 0)
							{
								this.m_Touches[num3] = (ButtonControl)inputControl;
								this.UpdateTouch(num3, num, TouchPhase.Began, eventPtr);
							}
						}
						else
						{
							this.UpdateTouch(num3, num, TouchPhase.Moved, eventPtr);
						}
					}
				}
			}
			eventPtr.handled = true;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000429C0 File Offset: 0x00040BC0
		private void OnDeviceChange(InputDevice device, InputDeviceChange change)
		{
			if (device == this.simulatedTouchscreen && change == InputDeviceChange.Removed)
			{
				TouchSimulation.Disable();
				return;
			}
			if (change != InputDeviceChange.Added)
			{
				if (change != InputDeviceChange.Removed)
				{
					return;
				}
				Pointer pointer = device as Pointer;
				if (pointer != null)
				{
					this.RemovePointer(pointer);
				}
			}
			else
			{
				Pointer pointer2 = device as Pointer;
				if (pointer2 != null)
				{
					if (device is Touchscreen)
					{
						return;
					}
					this.AddPointer(pointer2);
					return;
				}
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00042A14 File Offset: 0x00040C14
		protected void OnEnable()
		{
			if (this.simulatedTouchscreen != null)
			{
				if (!this.simulatedTouchscreen.added)
				{
					InputSystem.AddDevice(this.simulatedTouchscreen);
				}
			}
			else
			{
				this.simulatedTouchscreen = InputSystem.GetDevice("Simulated Touchscreen") as Touchscreen;
				if (this.simulatedTouchscreen == null)
				{
					this.simulatedTouchscreen = InputSystem.AddDevice<Touchscreen>("Simulated Touchscreen");
				}
			}
			if (this.m_Touches == null)
			{
				this.m_Touches = new ButtonControl[this.simulatedTouchscreen.touches.Count];
			}
			if (this.m_TouchIds == null)
			{
				this.m_TouchIds = new int[this.simulatedTouchscreen.touches.Count];
			}
			foreach (InputDevice inputDevice in InputSystem.devices)
			{
				this.OnDeviceChange(inputDevice, InputDeviceChange.Added);
			}
			if (this.m_OnDeviceChange == null)
			{
				this.m_OnDeviceChange = new Action<InputDevice, InputDeviceChange>(this.OnDeviceChange);
			}
			if (this.m_OnEvent == null)
			{
				this.m_OnEvent = new Action<InputEventPtr, InputDevice>(this.OnEvent);
			}
			InputSystem.onDeviceChange += this.m_OnDeviceChange;
			InputSystem.onEvent += this.m_OnEvent;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00042B5C File Offset: 0x00040D5C
		protected void OnDisable()
		{
			if (this.simulatedTouchscreen != null && this.simulatedTouchscreen.added)
			{
				InputSystem.RemoveDevice(this.simulatedTouchscreen);
			}
			for (int i = 0; i < this.m_NumPointers; i++)
			{
				InputSystem.EnableDevice(this.m_Pointers[i]);
			}
			this.m_Pointers.Clear<Pointer>(this.m_NumPointers);
			this.m_Touches.Clear<ButtonControl>();
			this.m_NumPointers = 0;
			this.m_LastTouchId = 0;
			InputSystem.onDeviceChange -= this.m_OnDeviceChange;
			InputSystem.onEvent -= this.m_OnEvent;
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00042BF4 File Offset: 0x00040DF4
		private void UpdateTouch(int touchIndex, int pointerIndex, TouchPhase phase, InputEventPtr eventPtr = default(InputEventPtr))
		{
			Vector2 vector = this.m_CurrentPositions[pointerIndex];
			byte b = (byte)this.m_CurrentDisplayIndices[pointerIndex];
			TouchState touchState = new TouchState
			{
				phase = phase,
				position = vector,
				displayIndex = b
			};
			if (phase == TouchPhase.Began)
			{
				touchState.startTime = (eventPtr.valid ? eventPtr.time : InputState.currentTime);
				touchState.startPosition = vector;
				int num = this.m_LastTouchId + 1;
				this.m_LastTouchId = num;
				touchState.touchId = num;
				this.m_TouchIds[touchIndex] = this.m_LastTouchId;
			}
			else
			{
				touchState.touchId = this.m_TouchIds[touchIndex];
			}
			InputSystem.QueueStateEvent<TouchState>(this.simulatedTouchscreen, touchState, -1.0);
			if (phase.IsEndedOrCanceled())
			{
				this.m_Touches[touchIndex] = null;
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00042CC4 File Offset: 0x00040EC4
		void IInputStateChangeMonitor.NotifyControlStateChanged(InputControl control, double time, InputEventPtr eventPtr, long monitorIndex)
		{
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00042CC6 File Offset: 0x00040EC6
		void IInputStateChangeMonitor.NotifyTimerExpired(InputControl control, double time, long monitorIndex, int timerIndex)
		{
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00042CC8 File Offset: 0x00040EC8
		protected void InstallStateChangeMonitors(int startIndex = 0)
		{
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00042CCA File Offset: 0x00040ECA
		protected void OnSourceControlChangedValue(InputControl control, double time, InputEventPtr eventPtr, long sourceDeviceAndButtonIndex)
		{
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00042CCC File Offset: 0x00040ECC
		protected void UninstallStateChangeMonitors(int startIndex = 0)
		{
		}

		// Token: 0x04000488 RID: 1160
		[NonSerialized]
		private int m_NumPointers;

		// Token: 0x04000489 RID: 1161
		[NonSerialized]
		private Pointer[] m_Pointers;

		// Token: 0x0400048A RID: 1162
		[NonSerialized]
		private Vector2[] m_CurrentPositions;

		// Token: 0x0400048B RID: 1163
		[NonSerialized]
		private int[] m_CurrentDisplayIndices;

		// Token: 0x0400048C RID: 1164
		[NonSerialized]
		private ButtonControl[] m_Touches;

		// Token: 0x0400048D RID: 1165
		[NonSerialized]
		private int[] m_TouchIds;

		// Token: 0x0400048E RID: 1166
		[NonSerialized]
		private int m_LastTouchId;

		// Token: 0x0400048F RID: 1167
		[NonSerialized]
		private Action<InputDevice, InputDeviceChange> m_OnDeviceChange;

		// Token: 0x04000490 RID: 1168
		[NonSerialized]
		private Action<InputEventPtr, InputDevice> m_OnEvent;

		// Token: 0x04000491 RID: 1169
		internal static TouchSimulation s_Instance;
	}
}
