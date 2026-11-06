using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngineInternal.Input;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000F5 RID: 245
	internal class NativeInputRuntime : IInputRuntime
	{
		// Token: 0x06000E7E RID: 3710 RVA: 0x00047668 File Offset: 0x00045868
		public int AllocateDeviceId()
		{
			return NativeInputSystem.AllocateDeviceId();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0004766F File Offset: 0x0004586F
		public void Update(InputUpdateType updateType)
		{
			NativeInputSystem.Update((NativeInputUpdateType)updateType);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00047677 File Offset: 0x00045877
		public unsafe void QueueEvent(InputEvent* ptr)
		{
			NativeInputSystem.QueueInputEvent((IntPtr)((void*)ptr));
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00047684 File Offset: 0x00045884
		public unsafe long DeviceCommand(int deviceId, InputDeviceCommand* commandPtr)
		{
			if (commandPtr == null)
			{
				throw new ArgumentNullException("commandPtr");
			}
			return NativeInputSystem.IOCTL(deviceId, commandPtr->type, new IntPtr(commandPtr->payloadPtr), commandPtr->payloadSizeInBytes);
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x000476B8 File Offset: 0x000458B8
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x000476C0 File Offset: 0x000458C0
		public unsafe InputUpdateDelegate onUpdate
		{
			get
			{
				return this.m_OnUpdate;
			}
			set
			{
				if (value != null)
				{
					NativeInputSystem.onUpdate = delegate(NativeInputUpdateType updateType, NativeInputEventBuffer* eventBufferPtr)
					{
						InputEventBuffer inputEventBuffer = new InputEventBuffer((InputEvent*)eventBufferPtr->eventBuffer, eventBufferPtr->eventCount, eventBufferPtr->sizeInBytes, eventBufferPtr->capacityInBytes);
						try
						{
							value((InputUpdateType)updateType, ref inputEventBuffer);
						}
						catch (Exception ex)
						{
							Debug.LogException(ex);
							Debug.LogError(string.Format("{0} during event processing of {1} update; resetting event buffer", ex.GetType().Name, updateType));
							inputEventBuffer.Reset();
						}
						if (inputEventBuffer.eventCount > 0)
						{
							eventBufferPtr->eventCount = inputEventBuffer.eventCount;
							eventBufferPtr->sizeInBytes = (int)inputEventBuffer.sizeInBytes;
							eventBufferPtr->capacityInBytes = (int)inputEventBuffer.capacityInBytes;
							eventBufferPtr->eventBuffer = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(inputEventBuffer.data);
							return;
						}
						eventBufferPtr->eventCount = 0;
						eventBufferPtr->sizeInBytes = 0;
					};
				}
				else
				{
					NativeInputSystem.onUpdate = null;
				}
				this.m_OnUpdate = value;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00047707 File Offset: 0x00045907
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x00047710 File Offset: 0x00045910
		public Action<InputUpdateType> onBeforeUpdate
		{
			get
			{
				return this.m_OnBeforeUpdate;
			}
			set
			{
				if (value != null)
				{
					NativeInputSystem.onBeforeUpdate = delegate(NativeInputUpdateType updateType)
					{
						value((InputUpdateType)updateType);
					};
				}
				else
				{
					NativeInputSystem.onBeforeUpdate = null;
				}
				this.m_OnBeforeUpdate = value;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00047757 File Offset: 0x00045957
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x00047760 File Offset: 0x00045960
		public Func<InputUpdateType, bool> onShouldRunUpdate
		{
			get
			{
				return this.m_OnShouldRunUpdate;
			}
			set
			{
				if (value != null)
				{
					NativeInputSystem.onShouldRunUpdate = (NativeInputUpdateType updateType) => value((InputUpdateType)updateType);
				}
				else
				{
					NativeInputSystem.onShouldRunUpdate = null;
				}
				this.m_OnShouldRunUpdate = value;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x000477A7 File Offset: 0x000459A7
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x000477AE File Offset: 0x000459AE
		public Action<int, string> onDeviceDiscovered
		{
			get
			{
				return NativeInputSystem.onDeviceDiscovered;
			}
			set
			{
				NativeInputSystem.onDeviceDiscovered = value;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x000477B6 File Offset: 0x000459B6
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x000477BE File Offset: 0x000459BE
		public Action onShutdown
		{
			get
			{
				return this.m_ShutdownMethod;
			}
			set
			{
				if (value == null)
				{
					Application.quitting -= this.OnShutdown;
				}
				else if (this.m_ShutdownMethod == null)
				{
					Application.quitting += this.OnShutdown;
				}
				this.m_ShutdownMethod = value;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x000477F6 File Offset: 0x000459F6
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x000477FE File Offset: 0x000459FE
		public Action<bool> onPlayerFocusChanged
		{
			get
			{
				return this.m_FocusChangedMethod;
			}
			set
			{
				if (value == null)
				{
					Application.focusChanged -= this.OnFocusChanged;
				}
				else if (this.m_FocusChangedMethod == null)
				{
					Application.focusChanged += this.OnFocusChanged;
				}
				this.m_FocusChangedMethod = value;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x00047836 File Offset: 0x00045A36
		public bool isPlayerFocused
		{
			get
			{
				return Application.isFocused;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0004783D File Offset: 0x00045A3D
		// (set) Token: 0x06000E90 RID: 3728 RVA: 0x00047845 File Offset: 0x00045A45
		public float pollingFrequency
		{
			get
			{
				return this.m_PollingFrequency;
			}
			set
			{
				this.m_PollingFrequency = value;
				NativeInputSystem.SetPollingFrequency(value);
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00047854 File Offset: 0x00045A54
		public double currentTime
		{
			get
			{
				return NativeInputSystem.currentTime;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0004785B File Offset: 0x00045A5B
		public double currentTimeForFixedUpdate
		{
			get
			{
				return (double)Time.fixedUnscaledTime + this.currentTimeOffsetToRealtimeSinceStartup;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0004786A File Offset: 0x00045A6A
		public double currentTimeOffsetToRealtimeSinceStartup
		{
			get
			{
				return NativeInputSystem.currentTimeOffsetToRealtimeSinceStartup;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x00047871 File Offset: 0x00045A71
		public float unscaledGameTime
		{
			get
			{
				return Time.unscaledTime;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00047878 File Offset: 0x00045A78
		// (set) Token: 0x06000E96 RID: 3734 RVA: 0x00047889 File Offset: 0x00045A89
		public bool runInBackground
		{
			get
			{
				return Application.runInBackground || this.m_RunInBackground;
			}
			set
			{
				this.m_RunInBackground = value;
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00047892 File Offset: 0x00045A92
		private void OnShutdown()
		{
			this.m_ShutdownMethod();
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0004789F File Offset: 0x00045A9F
		private bool OnWantsToShutdown()
		{
			if (!this.m_DidCallOnShutdown)
			{
				this.OnShutdown();
				this.m_DidCallOnShutdown = true;
			}
			return true;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x000478B7 File Offset: 0x00045AB7
		private void OnFocusChanged(bool focus)
		{
			this.m_FocusChangedMethod(focus);
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x000478C5 File Offset: 0x00045AC5
		public Vector2 screenSize
		{
			get
			{
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000478D8 File Offset: 0x00045AD8
		public ScreenOrientation screenOrientation
		{
			get
			{
				return Screen.orientation;
			}
		}

		// Token: 0x040005F8 RID: 1528
		public static readonly NativeInputRuntime instance = new NativeInputRuntime();

		// Token: 0x040005F9 RID: 1529
		private bool m_RunInBackground;

		// Token: 0x040005FA RID: 1530
		private Action m_ShutdownMethod;

		// Token: 0x040005FB RID: 1531
		private InputUpdateDelegate m_OnUpdate;

		// Token: 0x040005FC RID: 1532
		private Action<InputUpdateType> m_OnBeforeUpdate;

		// Token: 0x040005FD RID: 1533
		private Func<InputUpdateType, bool> m_OnShouldRunUpdate;

		// Token: 0x040005FE RID: 1534
		private float m_PollingFrequency = 60f;

		// Token: 0x040005FF RID: 1535
		private bool m_DidCallOnShutdown;

		// Token: 0x04000600 RID: 1536
		private Action<bool> m_FocusChangedMethod;
	}
}
