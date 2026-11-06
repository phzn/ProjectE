using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000EF RID: 239
	internal interface IInputRuntime
	{
		// Token: 0x06000E43 RID: 3651
		int AllocateDeviceId();

		// Token: 0x06000E44 RID: 3652
		void Update(InputUpdateType type);

		// Token: 0x06000E45 RID: 3653
		unsafe void QueueEvent(InputEvent* ptr);

		// Token: 0x06000E46 RID: 3654
		unsafe long DeviceCommand(int deviceId, InputDeviceCommand* commandPtr);

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000E47 RID: 3655
		// (set) Token: 0x06000E48 RID: 3656
		InputUpdateDelegate onUpdate { get; set; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000E49 RID: 3657
		// (set) Token: 0x06000E4A RID: 3658
		Action<InputUpdateType> onBeforeUpdate { get; set; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000E4B RID: 3659
		// (set) Token: 0x06000E4C RID: 3660
		Func<InputUpdateType, bool> onShouldRunUpdate { get; set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000E4D RID: 3661
		// (set) Token: 0x06000E4E RID: 3662
		Action<int, string> onDeviceDiscovered { get; set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000E4F RID: 3663
		// (set) Token: 0x06000E50 RID: 3664
		Action<bool> onPlayerFocusChanged { get; set; }

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000E51 RID: 3665
		bool isPlayerFocused { get; }

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000E52 RID: 3666
		// (set) Token: 0x06000E53 RID: 3667
		Action onShutdown { get; set; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000E54 RID: 3668
		// (set) Token: 0x06000E55 RID: 3669
		float pollingFrequency { get; set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000E56 RID: 3670
		double currentTime { get; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000E57 RID: 3671
		double currentTimeForFixedUpdate { get; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000E58 RID: 3672
		float unscaledGameTime { get; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000E59 RID: 3673
		double currentTimeOffsetToRealtimeSinceStartup { get; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000E5A RID: 3674
		// (set) Token: 0x06000E5B RID: 3675
		bool runInBackground { get; set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000E5C RID: 3676
		Vector2 screenSize { get; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000E5D RID: 3677
		ScreenOrientation screenOrientation { get; }
	}
}
