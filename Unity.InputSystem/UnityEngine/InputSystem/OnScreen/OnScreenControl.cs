using System;
using Unity.Collections;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.OnScreen
{
	// Token: 0x02000097 RID: 151
	public abstract class OnScreenControl : MonoBehaviour
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0003FBCA File Offset: 0x0003DDCA
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x0003FBD2 File Offset: 0x0003DDD2
		public string controlPath
		{
			get
			{
				return this.controlPathInternal;
			}
			set
			{
				this.controlPathInternal = value;
				if (base.isActiveAndEnabled)
				{
					this.SetupInputControl();
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x0003FBE9 File Offset: 0x0003DDE9
		public InputControl control
		{
			get
			{
				return this.m_Control;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000BF6 RID: 3062
		// (set) Token: 0x06000BF7 RID: 3063
		protected abstract string controlPathInternal { get; set; }

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0003FBF4 File Offset: 0x0003DDF4
		private void SetupInputControl()
		{
			string controlPathInternal = this.controlPathInternal;
			if (string.IsNullOrEmpty(controlPathInternal))
			{
				return;
			}
			string text = InputControlPath.TryGetDeviceLayout(controlPathInternal);
			if (text == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Cannot determine device layout to use based on control path '",
					controlPathInternal,
					"' used in ",
					base.GetType().Name,
					" component"
				}), this);
				return;
			}
			InternedString internedString = new InternedString(text);
			int num = -1;
			for (int i = 0; i < OnScreenControl.s_OnScreenDevices.length; i++)
			{
				if (OnScreenControl.s_OnScreenDevices[i].device.m_Layout == internedString)
				{
					num = i;
					break;
				}
			}
			InputDevice inputDevice;
			if (num == -1)
			{
				try
				{
					inputDevice = InputSystem.AddDevice(text, null, null);
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Could not create device with layout '",
						text,
						"' used in '",
						base.GetType().Name,
						"' component"
					}));
					Debug.LogException(ex);
					return;
				}
				InputSystem.AddDeviceUsage(inputDevice, "OnScreen");
				InputEventPtr inputEventPtr;
				NativeArray<byte> nativeArray = StateEvent.From(inputDevice, out inputEventPtr, Allocator.Persistent);
				num = OnScreenControl.s_OnScreenDevices.Append(new OnScreenControl.OnScreenDeviceInfo
				{
					eventPtr = inputEventPtr,
					buffer = nativeArray,
					device = inputDevice
				});
			}
			else
			{
				inputDevice = OnScreenControl.s_OnScreenDevices[num].device;
			}
			this.m_Control = InputControlPath.TryFindControl(inputDevice, controlPathInternal, 0);
			if (this.m_Control == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Cannot find control with path '",
					controlPathInternal,
					"' on device of type '",
					text,
					"' referenced by component '",
					base.GetType().Name,
					"'"
				}), this);
				if (OnScreenControl.s_OnScreenDevices[num].firstControl == null)
				{
					OnScreenControl.s_OnScreenDevices[num].Destroy();
					OnScreenControl.s_OnScreenDevices.RemoveAt(num);
				}
				return;
			}
			this.m_InputEventPtr = OnScreenControl.s_OnScreenDevices[num].eventPtr;
			OnScreenControl.s_OnScreenDevices[num] = OnScreenControl.s_OnScreenDevices[num].AddControl(this);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003FE30 File Offset: 0x0003E030
		protected void SendValueToControl<TValue>(TValue value) where TValue : struct
		{
			if (this.m_Control == null)
			{
				return;
			}
			InputControl<TValue> inputControl = this.m_Control as InputControl<TValue>;
			if (inputControl == null)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"The control path ",
					this.controlPath,
					" yields a control of type ",
					this.m_Control.GetType().Name,
					" which is not an InputControl with value type ",
					typeof(TValue).Name
				}), "value");
			}
			this.m_InputEventPtr.internalTime = InputRuntime.s_Instance.currentTime;
			inputControl.WriteValueIntoEvent(value, this.m_InputEventPtr);
			InputSystem.QueueEvent(this.m_InputEventPtr);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0003FEDE File Offset: 0x0003E0DE
		protected void SentDefaultValueToControl()
		{
			if (this.m_Control == null)
			{
				return;
			}
			this.m_InputEventPtr.internalTime = InputRuntime.s_Instance.currentTime;
			this.m_Control.ResetToDefaultStateInEvent(this.m_InputEventPtr);
			InputSystem.QueueEvent(this.m_InputEventPtr);
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0003FF1B File Offset: 0x0003E11B
		internal static bool HasAnyActive
		{
			get
			{
				return OnScreenControl.s_nbActiveInstances != 0;
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0003FF28 File Offset: 0x0003E128
		protected virtual void OnEnable()
		{
			OnScreenControl.s_nbActiveInstances++;
			this.SetupInputControl();
			if (this.m_Control == null)
			{
				return;
			}
			if (OnScreenControl.s_nbActiveInstances == 1 && PlayerInput.isSinglePlayer)
			{
				PlayerInput playerByIndex = PlayerInput.GetPlayerByIndex(0);
				if (playerByIndex != null && !playerByIndex.neverAutoSwitchControlSchemes)
				{
					ReadOnlyArray<InputDevice> devices = playerByIndex.devices;
					bool flag = false;
					foreach (InputDevice inputDevice in devices)
					{
						if (this.m_Control.device.deviceId == inputDevice.deviceId)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						playerByIndex.SwitchCurrentControlScheme(new InputDevice[] { this.m_Control.device });
					}
				}
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0003FFF8 File Offset: 0x0003E1F8
		protected virtual void OnDisable()
		{
			OnScreenControl.s_nbActiveInstances--;
			if (this.m_Control == null)
			{
				return;
			}
			InputDevice device = this.m_Control.device;
			for (int i = 0; i < OnScreenControl.s_OnScreenDevices.length; i++)
			{
				if (OnScreenControl.s_OnScreenDevices[i].device == device)
				{
					OnScreenControl.OnScreenDeviceInfo onScreenDeviceInfo = OnScreenControl.s_OnScreenDevices[i].RemoveControl(this);
					if (onScreenDeviceInfo.firstControl == null)
					{
						OnScreenControl.s_OnScreenDevices[i].Destroy();
						OnScreenControl.s_OnScreenDevices.RemoveAt(i);
					}
					else
					{
						OnScreenControl.s_OnScreenDevices[i] = onScreenDeviceInfo;
						if (!this.m_Control.CheckStateIsAtDefault())
						{
							this.SentDefaultValueToControl();
						}
					}
					this.m_Control = null;
					this.m_InputEventPtr = default(InputEventPtr);
					return;
				}
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000400CA File Offset: 0x0003E2CA
		internal string GetWarningMessage()
		{
			return string.Format("{0} needs to be attached as a child to a UI Canvas and have a RectTransform component to function properly.", base.GetType());
		}

		// Token: 0x04000461 RID: 1121
		private InputControl m_Control;

		// Token: 0x04000462 RID: 1122
		private OnScreenControl m_NextControlOnDevice;

		// Token: 0x04000463 RID: 1123
		private InputEventPtr m_InputEventPtr;

		// Token: 0x04000464 RID: 1124
		private static int s_nbActiveInstances;

		// Token: 0x04000465 RID: 1125
		private static InlinedArray<OnScreenControl.OnScreenDeviceInfo> s_OnScreenDevices;

		// Token: 0x020001E4 RID: 484
		private struct OnScreenDeviceInfo
		{
			// Token: 0x060014B4 RID: 5300 RVA: 0x000600A6 File Offset: 0x0005E2A6
			public OnScreenControl.OnScreenDeviceInfo AddControl(OnScreenControl control)
			{
				control.m_NextControlOnDevice = this.firstControl;
				this.firstControl = control;
				return this;
			}

			// Token: 0x060014B5 RID: 5301 RVA: 0x000600C4 File Offset: 0x0005E2C4
			public OnScreenControl.OnScreenDeviceInfo RemoveControl(OnScreenControl control)
			{
				if (this.firstControl == control)
				{
					this.firstControl = control.m_NextControlOnDevice;
				}
				else
				{
					OnScreenControl onScreenControl = this.firstControl.m_NextControlOnDevice;
					OnScreenControl onScreenControl2 = this.firstControl;
					while (onScreenControl != null)
					{
						if (!(onScreenControl != control))
						{
							onScreenControl2.m_NextControlOnDevice = onScreenControl.m_NextControlOnDevice;
							break;
						}
						onScreenControl2 = onScreenControl;
						onScreenControl = onScreenControl.m_NextControlOnDevice;
					}
				}
				control.m_NextControlOnDevice = null;
				return this;
			}

			// Token: 0x060014B6 RID: 5302 RVA: 0x00060138 File Offset: 0x0005E338
			public void Destroy()
			{
				if (this.buffer.IsCreated)
				{
					this.buffer.Dispose();
				}
				if (this.device != null)
				{
					InputSystem.RemoveDevice(this.device);
				}
				this.device = null;
				this.buffer = default(NativeArray<byte>);
			}

			// Token: 0x04000A15 RID: 2581
			public InputEventPtr eventPtr;

			// Token: 0x04000A16 RID: 2582
			public NativeArray<byte> buffer;

			// Token: 0x04000A17 RID: 2583
			public InputDevice device;

			// Token: 0x04000A18 RID: 2584
			public OnScreenControl firstControl;
		}
	}
}
