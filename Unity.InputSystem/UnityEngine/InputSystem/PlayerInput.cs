using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200005E RID: 94
	[AddComponentMenu("Input/Player Input")]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/PlayerInput.html")]
	public class PlayerInput : MonoBehaviour
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00034426 File Offset: 0x00032626
		public bool inputIsActive
		{
			get
			{
				return this.m_InputActive;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0003442E File Offset: 0x0003262E
		[Obsolete("Use inputIsActive instead.")]
		public bool active
		{
			get
			{
				return this.inputIsActive;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00034436 File Offset: 0x00032636
		public int playerIndex
		{
			get
			{
				return this.m_PlayerIndex;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0003443E File Offset: 0x0003263E
		public int splitScreenIndex
		{
			get
			{
				return this.m_SplitScreenIndex;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00034446 File Offset: 0x00032646
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x0003446C File Offset: 0x0003266C
		public InputActionAsset actions
		{
			get
			{
				if (!this.m_ActionsInitialized && base.gameObject.activeInHierarchy)
				{
					this.InitializeActions();
				}
				return this.m_Actions;
			}
			set
			{
				if (this.m_Actions == value)
				{
					return;
				}
				if (this.m_Actions != null)
				{
					this.m_Actions.Disable();
					if (this.m_ActionsInitialized)
					{
						this.UninitializeActions();
					}
				}
				this.m_Actions = value;
				if (this.m_Enabled)
				{
					this.ClearCaches();
					this.AssignUserAndDevices();
					this.InitializeActions();
					if (this.m_InputActive)
					{
						this.ActivateInput();
					}
				}
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x000344E0 File Offset: 0x000326E0
		public string currentControlScheme
		{
			get
			{
				if (!this.m_InputUser.valid)
				{
					return null;
				}
				InputControlScheme? controlScheme = this.m_InputUser.controlScheme;
				if (controlScheme == null)
				{
					return null;
				}
				return controlScheme.GetValueOrDefault().name;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00034522 File Offset: 0x00032722
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x0003452A File Offset: 0x0003272A
		public string defaultControlScheme
		{
			get
			{
				return this.m_DefaultControlScheme;
			}
			set
			{
				this.m_DefaultControlScheme = value;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00034533 File Offset: 0x00032733
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0003453B File Offset: 0x0003273B
		public bool neverAutoSwitchControlSchemes
		{
			get
			{
				return this.m_NeverAutoSwitchControlSchemes;
			}
			set
			{
				if (this.m_NeverAutoSwitchControlSchemes == value)
				{
					return;
				}
				this.m_NeverAutoSwitchControlSchemes = value;
				if (this.m_Enabled)
				{
					if (!value && !this.m_OnUnpairedDeviceUsedHooked)
					{
						this.StartListeningForUnpairedDeviceActivity();
						return;
					}
					if (value && this.m_OnUnpairedDeviceUsedHooked)
					{
						this.StopListeningForUnpairedDeviceActivity();
					}
				}
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00034579 File Offset: 0x00032779
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x00034581 File Offset: 0x00032781
		public InputActionMap currentActionMap
		{
			get
			{
				return this.m_CurrentActionMap;
			}
			set
			{
				InputActionMap currentActionMap = this.m_CurrentActionMap;
				this.m_CurrentActionMap = null;
				if (currentActionMap != null)
				{
					currentActionMap.Disable();
				}
				this.m_CurrentActionMap = value;
				InputActionMap currentActionMap2 = this.m_CurrentActionMap;
				if (currentActionMap2 == null)
				{
					return;
				}
				currentActionMap2.Enable();
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x000345B2 File Offset: 0x000327B2
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x000345BA File Offset: 0x000327BA
		public string defaultActionMap
		{
			get
			{
				return this.m_DefaultActionMap;
			}
			set
			{
				this.m_DefaultActionMap = value;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x000345C3 File Offset: 0x000327C3
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x000345CB File Offset: 0x000327CB
		public PlayerNotifications notificationBehavior
		{
			get
			{
				return this.m_NotificationBehavior;
			}
			set
			{
				if (this.m_NotificationBehavior == value)
				{
					return;
				}
				if (this.m_Enabled)
				{
					this.UninitializeActions();
				}
				this.m_NotificationBehavior = value;
				if (this.m_Enabled)
				{
					this.InitializeActions();
				}
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x000345FA File Offset: 0x000327FA
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x00034607 File Offset: 0x00032807
		public ReadOnlyArray<PlayerInput.ActionEvent> actionEvents
		{
			get
			{
				return this.m_ActionEvents;
			}
			set
			{
				if (this.m_Enabled)
				{
					this.UninitializeActions();
				}
				this.m_ActionEvents = value.ToArray();
				if (this.m_Enabled)
				{
					this.InitializeActions();
				}
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00034632 File Offset: 0x00032832
		public PlayerInput.DeviceLostEvent deviceLostEvent
		{
			get
			{
				if (this.m_DeviceLostEvent == null)
				{
					this.m_DeviceLostEvent = new PlayerInput.DeviceLostEvent();
				}
				return this.m_DeviceLostEvent;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0003464D File Offset: 0x0003284D
		public PlayerInput.DeviceRegainedEvent deviceRegainedEvent
		{
			get
			{
				if (this.m_DeviceRegainedEvent == null)
				{
					this.m_DeviceRegainedEvent = new PlayerInput.DeviceRegainedEvent();
				}
				return this.m_DeviceRegainedEvent;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x00034668 File Offset: 0x00032868
		public PlayerInput.ControlsChangedEvent controlsChangedEvent
		{
			get
			{
				if (this.m_ControlsChangedEvent == null)
				{
					this.m_ControlsChangedEvent = new PlayerInput.ControlsChangedEvent();
				}
				return this.m_ControlsChangedEvent;
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600095F RID: 2399 RVA: 0x00034683 File Offset: 0x00032883
		// (remove) Token: 0x06000960 RID: 2400 RVA: 0x0003469F File Offset: 0x0003289F
		public event Action<InputAction.CallbackContext> onActionTriggered
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_ActionTriggeredCallbacks.AddCallback(value);
			}
			remove
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_ActionTriggeredCallbacks.RemoveCallback(value);
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000961 RID: 2401 RVA: 0x000346BB File Offset: 0x000328BB
		// (remove) Token: 0x06000962 RID: 2402 RVA: 0x000346D7 File Offset: 0x000328D7
		public event Action<PlayerInput> onDeviceLost
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_DeviceLostCallbacks.AddCallback(value);
			}
			remove
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_DeviceLostCallbacks.RemoveCallback(value);
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000963 RID: 2403 RVA: 0x000346F3 File Offset: 0x000328F3
		// (remove) Token: 0x06000964 RID: 2404 RVA: 0x0003470F File Offset: 0x0003290F
		public event Action<PlayerInput> onDeviceRegained
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_DeviceRegainedCallbacks.AddCallback(value);
			}
			remove
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_DeviceRegainedCallbacks.RemoveCallback(value);
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000965 RID: 2405 RVA: 0x0003472B File Offset: 0x0003292B
		// (remove) Token: 0x06000966 RID: 2406 RVA: 0x00034747 File Offset: 0x00032947
		public event Action<PlayerInput> onControlsChanged
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_ControlsChangedCallbacks.AddCallback(value);
			}
			remove
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_ControlsChangedCallbacks.RemoveCallback(value);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00034763 File Offset: 0x00032963
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0003476B File Offset: 0x0003296B
		public Camera camera
		{
			get
			{
				return this.m_Camera;
			}
			set
			{
				this.m_Camera = value;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x00034774 File Offset: 0x00032974
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0003477C File Offset: 0x0003297C
		public InputSystemUIInputModule uiInputModule
		{
			get
			{
				return this.m_UIInputModule;
			}
			set
			{
				if (this.m_UIInputModule == value)
				{
					return;
				}
				if (this.m_UIInputModule != null && this.m_UIInputModule.actionsAsset == this.m_Actions)
				{
					this.m_UIInputModule.actionsAsset = null;
				}
				this.m_UIInputModule = value;
				if (this.m_UIInputModule != null && this.m_Actions != null)
				{
					this.m_UIInputModule.actionsAsset = this.m_Actions;
				}
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x000347FE File Offset: 0x000329FE
		public InputUser user
		{
			get
			{
				return this.m_InputUser;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00034808 File Offset: 0x00032A08
		public ReadOnlyArray<InputDevice> devices
		{
			get
			{
				if (!this.m_InputUser.valid)
				{
					return default(ReadOnlyArray<InputDevice>);
				}
				return this.m_InputUser.pairedDevices;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00034838 File Offset: 0x00032A38
		public bool hasMissingRequiredDevices
		{
			get
			{
				return this.user.valid && this.user.hasMissingRequiredDevices;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00034865 File Offset: 0x00032A65
		public static ReadOnlyArray<PlayerInput> all
		{
			get
			{
				return new ReadOnlyArray<PlayerInput>(PlayerInput.s_AllActivePlayers, 0, PlayerInput.s_AllActivePlayersCount);
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00034877 File Offset: 0x00032A77
		public static bool isSinglePlayer
		{
			get
			{
				return PlayerInput.s_AllActivePlayersCount <= 1 && (PlayerInputManager.instance == null || !PlayerInputManager.instance.joiningEnabled);
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000348A0 File Offset: 0x00032AA0
		public TDevice GetDevice<TDevice>() where TDevice : InputDevice
		{
			foreach (InputDevice inputDevice in this.devices)
			{
				TDevice tdevice = inputDevice as TDevice;
				if (tdevice != null)
				{
					return tdevice;
				}
			}
			return default(TDevice);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00034914 File Offset: 0x00032B14
		public void ActivateInput()
		{
			this.UpdateDelegates();
			this.m_InputActive = true;
			if (this.m_CurrentActionMap == null && this.m_Actions != null && !string.IsNullOrEmpty(this.m_DefaultActionMap))
			{
				this.SwitchCurrentActionMap(this.m_DefaultActionMap);
				return;
			}
			InputActionMap currentActionMap = this.m_CurrentActionMap;
			if (currentActionMap == null)
			{
				return;
			}
			currentActionMap.Enable();
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00034970 File Offset: 0x00032B70
		private void UpdateDelegates()
		{
			if (this.m_Actions == null)
			{
				this.m_AllMapsHashCode = 0;
				return;
			}
			int num = 0;
			foreach (InputActionMap inputActionMap in this.m_Actions.actionMaps)
			{
				num ^= inputActionMap.GetHashCode();
			}
			if (this.m_AllMapsHashCode != num)
			{
				if (this.m_NotificationBehavior != PlayerNotifications.InvokeUnityEvents)
				{
					this.InstallOnActionTriggeredHook();
				}
				this.CacheMessageNames();
				this.m_AllMapsHashCode = num;
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00034A0C File Offset: 0x00032C0C
		public void DeactivateInput()
		{
			InputActionMap currentActionMap = this.m_CurrentActionMap;
			if (currentActionMap != null)
			{
				currentActionMap.Disable();
			}
			this.m_InputActive = false;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00034A26 File Offset: 0x00032C26
		[Obsolete("Use DeactivateInput instead.")]
		public void PassivateInput()
		{
			this.DeactivateInput();
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00034A30 File Offset: 0x00032C30
		public bool SwitchCurrentControlScheme(params InputDevice[] devices)
		{
			if (devices == null)
			{
				throw new ArgumentNullException("devices");
			}
			if (this.actions == null)
			{
				throw new InvalidOperationException("Must set actions on PlayerInput in order to be able to switch control schemes");
			}
			InputControlScheme? inputControlScheme = InputControlScheme.FindControlSchemeForDevices<InputDevice[], ReadOnlyArray<InputControlScheme>>(devices, this.actions.controlSchemes, null, false);
			if (inputControlScheme == null)
			{
				return false;
			}
			InputControlScheme value = inputControlScheme.Value;
			this.SwitchControlSchemeInternal(ref value, devices);
			return true;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00034A98 File Offset: 0x00032C98
		public void SwitchCurrentControlScheme(string controlScheme, params InputDevice[] devices)
		{
			if (string.IsNullOrEmpty(controlScheme))
			{
				throw new ArgumentNullException("controlScheme");
			}
			if (devices == null)
			{
				throw new ArgumentNullException("devices");
			}
			InputControlScheme inputControlScheme;
			this.user.FindControlScheme(controlScheme, out inputControlScheme);
			this.SwitchControlSchemeInternal(ref inputControlScheme, devices);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00034AE0 File Offset: 0x00032CE0
		public void SwitchCurrentActionMap(string mapNameOrId)
		{
			if (!this.m_Enabled)
			{
				Debug.LogError("Cannot switch to actions '" + mapNameOrId + "'; input is not enabled", this);
				return;
			}
			if (this.m_Actions == null)
			{
				Debug.LogError("Cannot switch to actions '" + mapNameOrId + "'; no actions set on PlayerInput", this);
				return;
			}
			InputActionMap inputActionMap = this.m_Actions.FindActionMap(mapNameOrId, false);
			if (inputActionMap == null)
			{
				Debug.LogError(string.Format("Cannot find action map '{0}' in actions '{1}'", mapNameOrId, this.m_Actions), this);
				return;
			}
			this.currentActionMap = inputActionMap;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00034B64 File Offset: 0x00032D64
		public static PlayerInput GetPlayerByIndex(int playerIndex)
		{
			for (int i = 0; i < PlayerInput.s_AllActivePlayersCount; i++)
			{
				if (PlayerInput.s_AllActivePlayers[i].playerIndex == playerIndex)
				{
					return PlayerInput.s_AllActivePlayers[i];
				}
			}
			return null;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00034B9C File Offset: 0x00032D9C
		public static PlayerInput FindFirstPairedToDevice(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			for (int i = 0; i < PlayerInput.s_AllActivePlayersCount; i++)
			{
				if (PlayerInput.s_AllActivePlayers[i].devices.ContainsReference(device))
				{
					return PlayerInput.s_AllActivePlayers[i];
				}
			}
			return null;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00034BE4 File Offset: 0x00032DE4
		public static PlayerInput Instantiate(GameObject prefab, int playerIndex = -1, string controlScheme = null, int splitScreenIndex = -1, InputDevice pairWithDevice = null)
		{
			if (prefab == null)
			{
				throw new ArgumentNullException("prefab");
			}
			PlayerInput.s_InitPlayerIndex = playerIndex;
			PlayerInput.s_InitSplitScreenIndex = splitScreenIndex;
			PlayerInput.s_InitControlScheme = controlScheme;
			if (pairWithDevice != null)
			{
				ArrayHelpers.AppendWithCapacity<InputDevice>(ref PlayerInput.s_InitPairWithDevices, ref PlayerInput.s_InitPairWithDevicesCount, pairWithDevice, 10);
			}
			return PlayerInput.DoInstantiate(prefab);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00034C38 File Offset: 0x00032E38
		public static PlayerInput Instantiate(GameObject prefab, int playerIndex = -1, string controlScheme = null, int splitScreenIndex = -1, params InputDevice[] pairWithDevices)
		{
			if (prefab == null)
			{
				throw new ArgumentNullException("prefab");
			}
			PlayerInput.s_InitPlayerIndex = playerIndex;
			PlayerInput.s_InitSplitScreenIndex = splitScreenIndex;
			PlayerInput.s_InitControlScheme = controlScheme;
			if (pairWithDevices != null)
			{
				for (int i = 0; i < pairWithDevices.Length; i++)
				{
					ArrayHelpers.AppendWithCapacity<InputDevice>(ref PlayerInput.s_InitPairWithDevices, ref PlayerInput.s_InitPairWithDevicesCount, pairWithDevices[i], 10);
				}
			}
			return PlayerInput.DoInstantiate(prefab);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00034C9C File Offset: 0x00032E9C
		private static PlayerInput DoInstantiate(GameObject prefab)
		{
			bool flag = PlayerInput.s_DestroyIfDeviceSetupUnsuccessful;
			GameObject gameObject;
			try
			{
				gameObject = Object.Instantiate<GameObject>(prefab);
				gameObject.SetActive(true);
			}
			finally
			{
				PlayerInput.s_InitPairWithDevicesCount = 0;
				if (PlayerInput.s_InitPairWithDevices != null)
				{
					Array.Clear(PlayerInput.s_InitPairWithDevices, 0, PlayerInput.s_InitPairWithDevicesCount);
				}
				PlayerInput.s_InitControlScheme = null;
				PlayerInput.s_InitPlayerIndex = -1;
				PlayerInput.s_InitSplitScreenIndex = -1;
				PlayerInput.s_DestroyIfDeviceSetupUnsuccessful = false;
			}
			PlayerInput componentInChildren = gameObject.GetComponentInChildren<PlayerInput>();
			if (componentInChildren == null)
			{
				Object.DestroyImmediate(gameObject);
				Debug.LogError("The GameObject does not have a PlayerInput component", prefab);
				return null;
			}
			if (flag && (!componentInChildren.user.valid || componentInChildren.hasMissingRequiredDevices))
			{
				Object.DestroyImmediate(gameObject);
				return null;
			}
			return componentInChildren;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00034D4C File Offset: 0x00032F4C
		private void InitializeActions()
		{
			if (this.m_ActionsInitialized)
			{
				return;
			}
			if (this.m_Actions == null)
			{
				return;
			}
			for (int i = 0; i < PlayerInput.s_AllActivePlayersCount; i++)
			{
				if (PlayerInput.s_AllActivePlayers[i].m_Actions == this.m_Actions && PlayerInput.s_AllActivePlayers[i] != this)
				{
					this.CopyActionAssetAndApplyBindingOverrides();
					break;
				}
			}
			if (this.uiInputModule != null)
			{
				this.uiInputModule.actionsAsset = this.m_Actions;
			}
			switch (this.m_NotificationBehavior)
			{
			case PlayerNotifications.SendMessages:
			case PlayerNotifications.BroadcastMessages:
				this.InstallOnActionTriggeredHook();
				if (this.m_ActionMessageNames == null)
				{
					this.CacheMessageNames();
				}
				break;
			case PlayerNotifications.InvokeUnityEvents:
				if (this.m_ActionEvents != null)
				{
					foreach (PlayerInput.ActionEvent actionEvent in this.m_ActionEvents)
					{
						string actionId = actionEvent.actionId;
						if (!string.IsNullOrEmpty(actionId))
						{
							InputAction inputAction = this.m_Actions.FindAction(actionId, false);
							if (inputAction != null)
							{
								inputAction.performed += actionEvent.Invoke;
								inputAction.canceled += actionEvent.Invoke;
								inputAction.started += actionEvent.Invoke;
							}
						}
					}
				}
				break;
			case PlayerNotifications.InvokeCSharpEvents:
				this.InstallOnActionTriggeredHook();
				break;
			}
			this.m_ActionsInitialized = true;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00034EA4 File Offset: 0x000330A4
		private void CopyActionAssetAndApplyBindingOverrides()
		{
			InputActionAsset actions = this.m_Actions;
			this.m_Actions = Object.Instantiate<InputActionAsset>(this.m_Actions);
			for (int i = 0; i < actions.actionMaps.Count; i++)
			{
				for (int j = 0; j < actions.actionMaps[i].bindings.Count; j++)
				{
					this.m_Actions.actionMaps[i].ApplyBindingOverride(j, actions.actionMaps[i].bindings[j]);
				}
			}
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00034F44 File Offset: 0x00033144
		private void UninitializeActions()
		{
			if (!this.m_ActionsInitialized)
			{
				return;
			}
			if (this.m_Actions == null)
			{
				return;
			}
			this.UninstallOnActionTriggeredHook();
			if (this.m_NotificationBehavior == PlayerNotifications.InvokeUnityEvents && this.m_ActionEvents != null)
			{
				foreach (PlayerInput.ActionEvent actionEvent in this.m_ActionEvents)
				{
					string actionId = actionEvent.actionId;
					if (!string.IsNullOrEmpty(actionId))
					{
						InputAction inputAction = this.m_Actions.FindAction(actionId, false);
						if (inputAction != null)
						{
							inputAction.performed -= actionEvent.Invoke;
							inputAction.canceled -= actionEvent.Invoke;
							inputAction.started -= actionEvent.Invoke;
						}
					}
				}
			}
			this.m_CurrentActionMap = null;
			this.m_ActionsInitialized = false;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00035004 File Offset: 0x00033204
		private void InstallOnActionTriggeredHook()
		{
			if (this.m_ActionTriggeredDelegate == null)
			{
				this.m_ActionTriggeredDelegate = new Action<InputAction.CallbackContext>(this.OnActionTriggered);
			}
			foreach (InputActionMap inputActionMap in this.m_Actions.actionMaps)
			{
				inputActionMap.actionTriggered += this.m_ActionTriggeredDelegate;
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0003507C File Offset: 0x0003327C
		private void UninstallOnActionTriggeredHook()
		{
			if (this.m_ActionTriggeredDelegate != null)
			{
				foreach (InputActionMap inputActionMap in this.m_Actions.actionMaps)
				{
					inputActionMap.actionTriggered -= this.m_ActionTriggeredDelegate;
				}
			}
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000350E4 File Offset: 0x000332E4
		private void OnActionTriggered(InputAction.CallbackContext context)
		{
			if (!this.m_InputActive)
			{
				return;
			}
			PlayerNotifications notificationBehavior = this.m_NotificationBehavior;
			if (notificationBehavior > PlayerNotifications.BroadcastMessages)
			{
				if (notificationBehavior == PlayerNotifications.InvokeCSharpEvents)
				{
					DelegateHelpers.InvokeCallbacksSafe<InputAction.CallbackContext>(ref this.m_ActionTriggeredCallbacks, context, "PlayerInput.onActionTriggered", null);
					return;
				}
			}
			else
			{
				InputAction action = context.action;
				if (!context.performed && (!context.canceled || action.type != InputActionType.Value))
				{
					return;
				}
				if (this.m_ActionMessageNames == null)
				{
					this.CacheMessageNames();
				}
				string text = this.m_ActionMessageNames[action.m_Id];
				if (this.m_InputValueObject == null)
				{
					this.m_InputValueObject = new InputValue();
				}
				this.m_InputValueObject.m_Context = new InputAction.CallbackContext?(context);
				if (this.m_NotificationBehavior == PlayerNotifications.BroadcastMessages)
				{
					base.BroadcastMessage(text, this.m_InputValueObject, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					base.SendMessage(text, this.m_InputValueObject, SendMessageOptions.DontRequireReceiver);
				}
				this.m_InputValueObject.m_Context = null;
			}
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x000351C0 File Offset: 0x000333C0
		private void CacheMessageNames()
		{
			if (this.m_Actions == null)
			{
				return;
			}
			if (this.m_ActionMessageNames != null)
			{
				this.m_ActionMessageNames.Clear();
			}
			else
			{
				this.m_ActionMessageNames = new Dictionary<string, string>();
			}
			foreach (InputAction inputAction in this.m_Actions)
			{
				inputAction.MakeSureIdIsInPlace();
				string text = CSharpCodeHelpers.MakeTypeName(inputAction.name, "");
				this.m_ActionMessageNames[inputAction.m_Id] = "On" + text;
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0003526C File Offset: 0x0003346C
		private void ClearCaches()
		{
			if (this.m_ActionMessageNames != null)
			{
				this.m_ActionMessageNames.Clear();
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00035284 File Offset: 0x00033484
		private void AssignUserAndDevices()
		{
			if (this.m_InputUser.valid)
			{
				this.m_InputUser.UnpairDevices();
			}
			if (!(this.m_Actions == null))
			{
				if (this.m_Actions.controlSchemes.Count > 0)
				{
					if (!string.IsNullOrEmpty(PlayerInput.s_InitControlScheme))
					{
						InputControlScheme? inputControlScheme = this.m_Actions.FindControlScheme(PlayerInput.s_InitControlScheme);
						if (inputControlScheme == null)
						{
							Debug.LogError(string.Format("No control scheme '{0}' in '{1}'", PlayerInput.s_InitControlScheme, this.m_Actions), this);
						}
						else
						{
							this.TryToActivateControlScheme(inputControlScheme.Value);
						}
					}
					else if (!string.IsNullOrEmpty(this.m_DefaultControlScheme))
					{
						InputControlScheme? inputControlScheme2 = this.m_Actions.FindControlScheme(this.m_DefaultControlScheme);
						if (inputControlScheme2 == null)
						{
							Debug.LogError(string.Format("Cannot find default control scheme '{0}' in '{1}'", this.m_DefaultControlScheme, this.m_Actions), this);
						}
						else
						{
							this.TryToActivateControlScheme(inputControlScheme2.Value);
						}
					}
					if (PlayerInput.s_InitPairWithDevicesCount > 0 && (!this.m_InputUser.valid || this.m_InputUser.controlScheme == null))
					{
						InputControlScheme? inputControlScheme3 = InputControlScheme.FindControlSchemeForDevices<ReadOnlyArray<InputDevice>, ReadOnlyArray<InputControlScheme>>(new ReadOnlyArray<InputDevice>(PlayerInput.s_InitPairWithDevices, 0, PlayerInput.s_InitPairWithDevicesCount), this.m_Actions.controlSchemes, null, true);
						if (inputControlScheme3 != null)
						{
							this.TryToActivateControlScheme(inputControlScheme3.Value);
							goto IL_02D7;
						}
						goto IL_02D7;
					}
					else
					{
						if ((this.m_InputUser.valid && this.m_InputUser.controlScheme != null) || !string.IsNullOrEmpty(PlayerInput.s_InitControlScheme))
						{
							goto IL_02D7;
						}
						using (InputControlList<InputDevice> unpairedInputDevices = InputUser.GetUnpairedInputDevices())
						{
							InputControlScheme? inputControlScheme4 = InputControlScheme.FindControlSchemeForDevices<InputControlList<InputDevice>, ReadOnlyArray<InputControlScheme>>(unpairedInputDevices, this.m_Actions.controlSchemes, null, false);
							if (inputControlScheme4 != null)
							{
								this.TryToActivateControlScheme(inputControlScheme4.Value);
								goto IL_02D7;
							}
							if (InputSystem.devices.Count > 0 && unpairedInputDevices.Count == 0)
							{
								Debug.LogWarning("Cannot find matching control scheme for " + base.name + " (all control schemes are already paired to matching devices)", this);
							}
							goto IL_02D7;
						}
					}
				}
				if (PlayerInput.s_InitPairWithDevicesCount > 0)
				{
					for (int i = 0; i < PlayerInput.s_InitPairWithDevicesCount; i++)
					{
						this.m_InputUser = InputUser.PerformPairingWithDevice(PlayerInput.s_InitPairWithDevices[i], this.m_InputUser, InputUserPairingOptions.None);
					}
				}
				else
				{
					using (InputControlList<InputDevice> unpairedInputDevices2 = InputUser.GetUnpairedInputDevices())
					{
						for (int j = 0; j < unpairedInputDevices2.Count; j++)
						{
							InputDevice inputDevice = unpairedInputDevices2[j];
							if (this.HaveBindingForDevice(inputDevice))
							{
								this.m_InputUser = InputUser.PerformPairingWithDevice(inputDevice, this.m_InputUser, InputUserPairingOptions.None);
							}
						}
					}
				}
				IL_02D7:
				if (this.m_InputUser.valid)
				{
					this.m_InputUser.AssociateActionsWithUser(this.m_Actions);
				}
				return;
			}
			if (PlayerInput.s_InitPairWithDevicesCount > 0)
			{
				for (int k = 0; k < PlayerInput.s_InitPairWithDevicesCount; k++)
				{
					this.m_InputUser = InputUser.PerformPairingWithDevice(PlayerInput.s_InitPairWithDevices[k], this.m_InputUser, InputUserPairingOptions.None);
				}
				return;
			}
			this.m_InputUser = default(InputUser);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x000355A4 File Offset: 0x000337A4
		private bool HaveBindingForDevice(InputDevice device)
		{
			if (this.m_Actions == null)
			{
				return false;
			}
			ReadOnlyArray<InputActionMap> actionMaps = this.m_Actions.actionMaps;
			for (int i = 0; i < actionMaps.Count; i++)
			{
				if (actionMaps[i].IsUsableWithDevice(device))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x000355F4 File Offset: 0x000337F4
		private void UnassignUserAndDevices()
		{
			if (this.m_InputUser.valid)
			{
				this.m_InputUser.UnpairDevicesAndRemoveUser();
			}
			if (this.m_Actions != null)
			{
				this.m_Actions.devices = null;
			}
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0003563C File Offset: 0x0003383C
		private bool TryToActivateControlScheme(InputControlScheme controlScheme)
		{
			if (PlayerInput.s_InitPairWithDevicesCount > 0)
			{
				for (int i = 0; i < PlayerInput.s_InitPairWithDevicesCount; i++)
				{
					InputDevice inputDevice = PlayerInput.s_InitPairWithDevices[i];
					if (!controlScheme.SupportsDevice(inputDevice))
					{
						return false;
					}
				}
				for (int j = 0; j < PlayerInput.s_InitPairWithDevicesCount; j++)
				{
					InputDevice inputDevice2 = PlayerInput.s_InitPairWithDevices[j];
					this.m_InputUser = InputUser.PerformPairingWithDevice(inputDevice2, this.m_InputUser, InputUserPairingOptions.None);
				}
			}
			if (!this.m_InputUser.valid)
			{
				this.m_InputUser = InputUser.CreateUserWithoutPairedDevices();
			}
			this.m_InputUser.ActivateControlScheme(controlScheme).AndPairRemainingDevices();
			if (this.user.hasMissingRequiredDevices)
			{
				this.m_InputUser.ActivateControlScheme(null);
				this.m_InputUser.UnpairDevices();
				return false;
			}
			return true;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x000356FC File Offset: 0x000338FC
		private void AssignPlayerIndex()
		{
			if (PlayerInput.s_InitPlayerIndex != -1)
			{
				this.m_PlayerIndex = PlayerInput.s_InitPlayerIndex;
				return;
			}
			int num = int.MaxValue;
			int num2 = int.MinValue;
			for (int i = 0; i < PlayerInput.s_AllActivePlayersCount; i++)
			{
				int playerIndex = PlayerInput.s_AllActivePlayers[i].playerIndex;
				num = Math.Min(num, playerIndex);
				num2 = Math.Max(num2, playerIndex);
			}
			if (num != 2147483647 && num > 0)
			{
				this.m_PlayerIndex = num - 1;
				return;
			}
			if (num2 != -2147483648)
			{
				for (int j = num; j < num2; j++)
				{
					if (PlayerInput.GetPlayerByIndex(j) == null)
					{
						this.m_PlayerIndex = j;
						return;
					}
				}
				this.m_PlayerIndex = num2 + 1;
				return;
			}
			this.m_PlayerIndex = 0;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x000357B0 File Offset: 0x000339B0
		private void OnEnable()
		{
			this.m_Enabled = true;
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				this.AssignPlayerIndex();
				this.InitializeActions();
				this.AssignUserAndDevices();
				this.ActivateInput();
			}
			if (PlayerInput.s_InitSplitScreenIndex >= 0)
			{
				this.m_SplitScreenIndex = PlayerInput.s_InitSplitScreenIndex;
			}
			else
			{
				this.m_SplitScreenIndex = this.playerIndex;
			}
			ArrayHelpers.AppendWithCapacity<PlayerInput>(ref PlayerInput.s_AllActivePlayers, ref PlayerInput.s_AllActivePlayersCount, this, 10);
			for (int i = 1; i < PlayerInput.s_AllActivePlayersCount; i++)
			{
				int num = i;
				while (num > 0 && PlayerInput.s_AllActivePlayers[num - 1].playerIndex > PlayerInput.s_AllActivePlayers[num].playerIndex)
				{
					PlayerInput.s_AllActivePlayers.SwapElements<PlayerInput>(num, num - 1);
					num--;
				}
			}
			if (PlayerInput.s_AllActivePlayersCount == 1)
			{
				if (PlayerInput.s_UserChangeDelegate == null)
				{
					PlayerInput.s_UserChangeDelegate = new Action<InputUser, InputUserChange, InputDevice>(PlayerInput.OnUserChange);
				}
				InputUser.onChange += PlayerInput.s_UserChangeDelegate;
			}
			if (PlayerInput.isSinglePlayer)
			{
				if (this.m_Actions != null && this.m_Actions.controlSchemes.Count == 0)
				{
					this.StartListeningForDeviceChanges();
				}
				else if (!this.neverAutoSwitchControlSchemes)
				{
					this.StartListeningForUnpairedDeviceActivity();
				}
			}
			this.HandleControlsChanged();
			PlayerInputManager instance = PlayerInputManager.instance;
			if (instance == null)
			{
				return;
			}
			instance.NotifyPlayerJoined(this);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000358F8 File Offset: 0x00033AF8
		private void StartListeningForUnpairedDeviceActivity()
		{
			if (this.m_OnUnpairedDeviceUsedHooked)
			{
				return;
			}
			if (this.m_UnpairedDeviceUsedDelegate == null)
			{
				this.m_UnpairedDeviceUsedDelegate = new Action<InputControl, InputEventPtr>(this.OnUnpairedDeviceUsed);
			}
			if (this.m_PreFilterUnpairedDeviceUsedDelegate == null)
			{
				this.m_PreFilterUnpairedDeviceUsedDelegate = new Func<InputDevice, InputEventPtr, bool>(PlayerInput.OnPreFilterUnpairedDeviceUsed);
			}
			InputUser.onUnpairedDeviceUsed += this.m_UnpairedDeviceUsedDelegate;
			InputUser.onPrefilterUnpairedDeviceActivity += this.m_PreFilterUnpairedDeviceUsedDelegate;
			InputUser.listenForUnpairedDeviceActivity++;
			this.m_OnUnpairedDeviceUsedHooked = true;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0003596B File Offset: 0x00033B6B
		private void StopListeningForUnpairedDeviceActivity()
		{
			if (!this.m_OnUnpairedDeviceUsedHooked)
			{
				return;
			}
			InputUser.onUnpairedDeviceUsed -= this.m_UnpairedDeviceUsedDelegate;
			InputUser.onPrefilterUnpairedDeviceActivity -= this.m_PreFilterUnpairedDeviceUsedDelegate;
			InputUser.listenForUnpairedDeviceActivity--;
			this.m_OnUnpairedDeviceUsedHooked = false;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0003599F File Offset: 0x00033B9F
		private void StartListeningForDeviceChanges()
		{
			if (this.m_OnDeviceChangeHooked)
			{
				return;
			}
			if (this.m_DeviceChangeDelegate == null)
			{
				this.m_DeviceChangeDelegate = new Action<InputDevice, InputDeviceChange>(this.OnDeviceChange);
			}
			InputSystem.onDeviceChange += this.m_DeviceChangeDelegate;
			this.m_OnDeviceChangeHooked = true;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000359D6 File Offset: 0x00033BD6
		private void StopListeningForDeviceChanges()
		{
			if (!this.m_OnDeviceChangeHooked)
			{
				return;
			}
			InputSystem.onDeviceChange -= this.m_DeviceChangeDelegate;
			this.m_OnDeviceChangeHooked = false;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x000359F4 File Offset: 0x00033BF4
		private void OnDisable()
		{
			this.m_Enabled = false;
			int num = PlayerInput.s_AllActivePlayers.IndexOfReference(this, PlayerInput.s_AllActivePlayersCount);
			if (num != -1)
			{
				PlayerInput.s_AllActivePlayers.EraseAtWithCapacity<PlayerInput>(ref PlayerInput.s_AllActivePlayersCount, num);
			}
			if (PlayerInput.s_AllActivePlayersCount == 0 && PlayerInput.s_UserChangeDelegate != null)
			{
				InputUser.onChange -= PlayerInput.s_UserChangeDelegate;
			}
			this.StopListeningForUnpairedDeviceActivity();
			this.StopListeningForDeviceChanges();
			PlayerInputManager instance = PlayerInputManager.instance;
			if (instance != null)
			{
				instance.NotifyPlayerLeft(this);
			}
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				this.DeactivateInput();
				this.UnassignUserAndDevices();
				this.UninitializeActions();
			}
			this.m_PlayerIndex = -1;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00035AA0 File Offset: 0x00033CA0
		public void DebugLogAction(InputAction.CallbackContext context)
		{
			Debug.Log(context.ToString());
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00035AB4 File Offset: 0x00033CB4
		private void HandleDeviceLost()
		{
			switch (this.m_NotificationBehavior)
			{
			case PlayerNotifications.SendMessages:
				base.SendMessage("OnDeviceLost", this, SendMessageOptions.DontRequireReceiver);
				return;
			case PlayerNotifications.BroadcastMessages:
				base.BroadcastMessage("OnDeviceLost", this, SendMessageOptions.DontRequireReceiver);
				return;
			case PlayerNotifications.InvokeUnityEvents:
			{
				PlayerInput.DeviceLostEvent deviceLostEvent = this.m_DeviceLostEvent;
				if (deviceLostEvent == null)
				{
					return;
				}
				deviceLostEvent.Invoke(this);
				return;
			}
			case PlayerNotifications.InvokeCSharpEvents:
				DelegateHelpers.InvokeCallbacksSafe<PlayerInput>(ref this.m_DeviceLostCallbacks, this, "onDeviceLost", null);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00035B20 File Offset: 0x00033D20
		private void HandleDeviceRegained()
		{
			switch (this.m_NotificationBehavior)
			{
			case PlayerNotifications.SendMessages:
				base.SendMessage("OnDeviceRegained", this, SendMessageOptions.DontRequireReceiver);
				return;
			case PlayerNotifications.BroadcastMessages:
				base.BroadcastMessage("OnDeviceRegained", this, SendMessageOptions.DontRequireReceiver);
				return;
			case PlayerNotifications.InvokeUnityEvents:
			{
				PlayerInput.DeviceRegainedEvent deviceRegainedEvent = this.m_DeviceRegainedEvent;
				if (deviceRegainedEvent == null)
				{
					return;
				}
				deviceRegainedEvent.Invoke(this);
				return;
			}
			case PlayerNotifications.InvokeCSharpEvents:
				DelegateHelpers.InvokeCallbacksSafe<PlayerInput>(ref this.m_DeviceRegainedCallbacks, this, "onDeviceRegained", null);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00035B8C File Offset: 0x00033D8C
		private void HandleControlsChanged()
		{
			switch (this.m_NotificationBehavior)
			{
			case PlayerNotifications.SendMessages:
				base.SendMessage("OnControlsChanged", this, SendMessageOptions.DontRequireReceiver);
				return;
			case PlayerNotifications.BroadcastMessages:
				base.BroadcastMessage("OnControlsChanged", this, SendMessageOptions.DontRequireReceiver);
				return;
			case PlayerNotifications.InvokeUnityEvents:
			{
				PlayerInput.ControlsChangedEvent controlsChangedEvent = this.m_ControlsChangedEvent;
				if (controlsChangedEvent == null)
				{
					return;
				}
				controlsChangedEvent.Invoke(this);
				return;
			}
			case PlayerNotifications.InvokeCSharpEvents:
				DelegateHelpers.InvokeCallbacksSafe<PlayerInput>(ref this.m_ControlsChangedCallbacks, this, "onControlsChanged", null);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00035BF8 File Offset: 0x00033DF8
		private static void OnUserChange(InputUser user, InputUserChange change, InputDevice device)
		{
			if (change - InputUserChange.DeviceLost <= 1)
			{
				for (int i = 0; i < PlayerInput.s_AllActivePlayersCount; i++)
				{
					PlayerInput playerInput = PlayerInput.s_AllActivePlayers[i];
					if (playerInput.m_InputUser == user)
					{
						if (change == InputUserChange.DeviceLost)
						{
							playerInput.HandleDeviceLost();
						}
						else if (change == InputUserChange.DeviceRegained)
						{
							playerInput.HandleDeviceRegained();
						}
					}
				}
				return;
			}
			if (change != InputUserChange.ControlsChanged)
			{
				return;
			}
			for (int j = 0; j < PlayerInput.s_AllActivePlayersCount; j++)
			{
				PlayerInput playerInput2 = PlayerInput.s_AllActivePlayers[j];
				if (playerInput2.m_InputUser == user)
				{
					playerInput2.HandleControlsChanged();
				}
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00035C7C File Offset: 0x00033E7C
		private static bool OnPreFilterUnpairedDeviceUsed(InputDevice device, InputEventPtr eventPtr)
		{
			InputActionAsset actions = PlayerInput.all[0].actions;
			return actions != null && (!OnScreenControl.HasAnyActive || !(device is Pointer)) && actions.IsUsableWithDevice(device);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00035CC0 File Offset: 0x00033EC0
		private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr)
		{
			if (!PlayerInput.isSinglePlayer || this.neverAutoSwitchControlSchemes)
			{
				return;
			}
			PlayerInput playerInput = PlayerInput.all[0];
			if (playerInput.m_Actions == null)
			{
				return;
			}
			InputDevice device = control.device;
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				using (InputControlList<InputDevice> unpairedInputDevices = InputUser.GetUnpairedInputDevices())
				{
					if (unpairedInputDevices.Count > 1)
					{
						int num = unpairedInputDevices.IndexOf(device);
						unpairedInputDevices.SwapElements(0, num);
					}
					ReadOnlyArray<InputDevice> devices = playerInput.devices;
					for (int i = 0; i < devices.Count; i++)
					{
						unpairedInputDevices.Add(devices[i]);
					}
					InputControlScheme inputControlScheme;
					InputControlScheme.MatchResult matchResult;
					if (InputControlScheme.FindControlSchemeForDevices<InputControlList<InputDevice>, ReadOnlyArray<InputControlScheme>>(unpairedInputDevices, playerInput.m_Actions.controlSchemes, out inputControlScheme, out matchResult, device, false))
					{
						try
						{
							bool valid = playerInput.user.valid;
							if (valid)
							{
								playerInput.user.UnpairDevices();
							}
							InputControlList<InputDevice> devices2 = matchResult.devices;
							for (int j = 0; j < devices2.Count; j++)
							{
								playerInput.m_InputUser = InputUser.PerformPairingWithDevice(devices2[j], playerInput.m_InputUser, InputUserPairingOptions.None);
								if (!valid && playerInput.actions != null)
								{
									playerInput.m_InputUser.AssociateActionsWithUser(playerInput.actions);
								}
							}
							playerInput.user.ActivateControlScheme(inputControlScheme);
						}
						finally
						{
							matchResult.Dispose();
						}
					}
				}
			}
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00035E7C File Offset: 0x0003407C
		private void OnDeviceChange(InputDevice device, InputDeviceChange change)
		{
			if (change == InputDeviceChange.Added && PlayerInput.isSinglePlayer && this.m_Actions != null && this.m_Actions.controlSchemes.Count == 0 && this.HaveBindingForDevice(device) && this.m_InputUser.valid)
			{
				InputUser.PerformPairingWithDevice(device, this.m_InputUser, InputUserPairingOptions.None);
			}
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00035EDC File Offset: 0x000340DC
		private void SwitchControlSchemeInternal(ref InputControlScheme controlScheme, params InputDevice[] devices)
		{
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				for (int i = this.user.pairedDevices.Count - 1; i >= 0; i--)
				{
					if (!devices.ContainsReference(this.user.pairedDevices[i]))
					{
						this.user.UnpairDevice(this.user.pairedDevices[i]);
					}
				}
				foreach (InputDevice inputDevice in devices)
				{
					if (!this.user.pairedDevices.ContainsReference(inputDevice))
					{
						InputUser.PerformPairingWithDevice(inputDevice, this.user, InputUserPairingOptions.None);
					}
				}
				if (this.user.controlScheme == null || !this.user.controlScheme.Value.Equals(controlScheme))
				{
					this.user.ActivateControlScheme(controlScheme);
				}
			}
		}

		// Token: 0x040002EE RID: 750
		public const string DeviceLostMessage = "OnDeviceLost";

		// Token: 0x040002EF RID: 751
		public const string DeviceRegainedMessage = "OnDeviceRegained";

		// Token: 0x040002F0 RID: 752
		public const string ControlsChangedMessage = "OnControlsChanged";

		// Token: 0x040002F1 RID: 753
		private int m_AllMapsHashCode;

		// Token: 0x040002F2 RID: 754
		[Tooltip("Input actions associated with the player.")]
		[SerializeField]
		internal InputActionAsset m_Actions;

		// Token: 0x040002F3 RID: 755
		[Tooltip("Determine how notifications should be sent when an input-related event associated with the player happens.")]
		[SerializeField]
		internal PlayerNotifications m_NotificationBehavior;

		// Token: 0x040002F4 RID: 756
		[Tooltip("UI InputModule that should have it's input actions synchronized to this PlayerInput's actions.")]
		[SerializeField]
		internal InputSystemUIInputModule m_UIInputModule;

		// Token: 0x040002F5 RID: 757
		[Tooltip("Event that is triggered when the PlayerInput loses a paired device (e.g. its battery runs out).")]
		[SerializeField]
		internal PlayerInput.DeviceLostEvent m_DeviceLostEvent;

		// Token: 0x040002F6 RID: 758
		[SerializeField]
		internal PlayerInput.DeviceRegainedEvent m_DeviceRegainedEvent;

		// Token: 0x040002F7 RID: 759
		[SerializeField]
		internal PlayerInput.ControlsChangedEvent m_ControlsChangedEvent;

		// Token: 0x040002F8 RID: 760
		[SerializeField]
		internal PlayerInput.ActionEvent[] m_ActionEvents;

		// Token: 0x040002F9 RID: 761
		[SerializeField]
		internal bool m_NeverAutoSwitchControlSchemes;

		// Token: 0x040002FA RID: 762
		[SerializeField]
		internal string m_DefaultControlScheme;

		// Token: 0x040002FB RID: 763
		[SerializeField]
		internal string m_DefaultActionMap;

		// Token: 0x040002FC RID: 764
		[SerializeField]
		internal int m_SplitScreenIndex = -1;

		// Token: 0x040002FD RID: 765
		[Tooltip("Reference to the player's view camera. Note that this is only required when using split-screen and/or per-player UIs. Otherwise it is safe to leave this property uninitialized.")]
		[SerializeField]
		internal Camera m_Camera;

		// Token: 0x040002FE RID: 766
		[NonSerialized]
		private InputValue m_InputValueObject;

		// Token: 0x040002FF RID: 767
		[NonSerialized]
		internal InputActionMap m_CurrentActionMap;

		// Token: 0x04000300 RID: 768
		[NonSerialized]
		private int m_PlayerIndex = -1;

		// Token: 0x04000301 RID: 769
		[NonSerialized]
		private bool m_InputActive;

		// Token: 0x04000302 RID: 770
		[NonSerialized]
		private bool m_Enabled;

		// Token: 0x04000303 RID: 771
		[NonSerialized]
		internal bool m_ActionsInitialized;

		// Token: 0x04000304 RID: 772
		[NonSerialized]
		private Dictionary<string, string> m_ActionMessageNames;

		// Token: 0x04000305 RID: 773
		[NonSerialized]
		private InputUser m_InputUser;

		// Token: 0x04000306 RID: 774
		[NonSerialized]
		private Action<InputAction.CallbackContext> m_ActionTriggeredDelegate;

		// Token: 0x04000307 RID: 775
		[NonSerialized]
		private CallbackArray<Action<PlayerInput>> m_DeviceLostCallbacks;

		// Token: 0x04000308 RID: 776
		[NonSerialized]
		private CallbackArray<Action<PlayerInput>> m_DeviceRegainedCallbacks;

		// Token: 0x04000309 RID: 777
		[NonSerialized]
		private CallbackArray<Action<PlayerInput>> m_ControlsChangedCallbacks;

		// Token: 0x0400030A RID: 778
		[NonSerialized]
		private CallbackArray<Action<InputAction.CallbackContext>> m_ActionTriggeredCallbacks;

		// Token: 0x0400030B RID: 779
		[NonSerialized]
		private Action<InputControl, InputEventPtr> m_UnpairedDeviceUsedDelegate;

		// Token: 0x0400030C RID: 780
		[NonSerialized]
		private Func<InputDevice, InputEventPtr, bool> m_PreFilterUnpairedDeviceUsedDelegate;

		// Token: 0x0400030D RID: 781
		[NonSerialized]
		private bool m_OnUnpairedDeviceUsedHooked;

		// Token: 0x0400030E RID: 782
		[NonSerialized]
		private Action<InputDevice, InputDeviceChange> m_DeviceChangeDelegate;

		// Token: 0x0400030F RID: 783
		[NonSerialized]
		private bool m_OnDeviceChangeHooked;

		// Token: 0x04000310 RID: 784
		internal static int s_AllActivePlayersCount;

		// Token: 0x04000311 RID: 785
		internal static PlayerInput[] s_AllActivePlayers;

		// Token: 0x04000312 RID: 786
		private static Action<InputUser, InputUserChange, InputDevice> s_UserChangeDelegate;

		// Token: 0x04000313 RID: 787
		private static int s_InitPairWithDevicesCount;

		// Token: 0x04000314 RID: 788
		private static InputDevice[] s_InitPairWithDevices;

		// Token: 0x04000315 RID: 789
		private static int s_InitPlayerIndex = -1;

		// Token: 0x04000316 RID: 790
		private static int s_InitSplitScreenIndex = -1;

		// Token: 0x04000317 RID: 791
		private static string s_InitControlScheme;

		// Token: 0x04000318 RID: 792
		internal static bool s_DestroyIfDeviceSetupUnsuccessful;

		// Token: 0x020001BF RID: 447
		[Serializable]
		public class ActionEvent : UnityEvent<InputAction.CallbackContext>
		{
			// Token: 0x17000585 RID: 1413
			// (get) Token: 0x06001482 RID: 5250 RVA: 0x0005F4D7 File Offset: 0x0005D6D7
			public string actionId
			{
				get
				{
					return this.m_ActionId;
				}
			}

			// Token: 0x17000586 RID: 1414
			// (get) Token: 0x06001483 RID: 5251 RVA: 0x0005F4DF File Offset: 0x0005D6DF
			public string actionName
			{
				get
				{
					return this.m_ActionName;
				}
			}

			// Token: 0x06001484 RID: 5252 RVA: 0x0005F4E7 File Offset: 0x0005D6E7
			public ActionEvent()
			{
			}

			// Token: 0x06001485 RID: 5253 RVA: 0x0005F4F0 File Offset: 0x0005D6F0
			public ActionEvent(InputAction action)
			{
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				if (action.isSingletonAction)
				{
					throw new ArgumentException(string.Format("Action must be part of an asset (given action '{0}' is a singleton)", action));
				}
				if (action.actionMap.asset == null)
				{
					throw new ArgumentException(string.Format("Action must be part of an asset (given action '{0}' is not)", action));
				}
				this.m_ActionId = action.id.ToString();
				this.m_ActionName = action.actionMap.name + "/" + action.name;
			}

			// Token: 0x06001486 RID: 5254 RVA: 0x0005F589 File Offset: 0x0005D789
			public ActionEvent(Guid actionGUID, string name = null)
			{
				this.m_ActionId = actionGUID.ToString();
				this.m_ActionName = name;
			}

			// Token: 0x0400095A RID: 2394
			[SerializeField]
			private string m_ActionId;

			// Token: 0x0400095B RID: 2395
			[SerializeField]
			private string m_ActionName;
		}

		// Token: 0x020001C0 RID: 448
		[Serializable]
		public class DeviceLostEvent : UnityEvent<PlayerInput>
		{
		}

		// Token: 0x020001C1 RID: 449
		[Serializable]
		public class DeviceRegainedEvent : UnityEvent<PlayerInput>
		{
		}

		// Token: 0x020001C2 RID: 450
		[Serializable]
		public class ControlsChangedEvent : UnityEvent<PlayerInput>
		{
		}
	}
}
