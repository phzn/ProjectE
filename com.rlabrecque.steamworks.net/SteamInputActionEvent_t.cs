using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000199 RID: 409
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct SteamInputActionEvent_t
	{
		// Token: 0x04000B8D RID: 2957
		[FieldOffset(0)]
		public InputHandle_t controllerHandle;

		// Token: 0x04000B8E RID: 2958
		[FieldOffset(8)]
		public ESteamInputActionEventType eEventType;

		// Token: 0x04000B8F RID: 2959
		[FieldOffset(12)]
		public SteamInputActionEvent_t.OptionValue m_val;

		// Token: 0x020001EA RID: 490
		[Serializable]
		public struct AnalogAction_t
		{
			// Token: 0x04000C2C RID: 3116
			public InputAnalogActionHandle_t actionHandle;

			// Token: 0x04000C2D RID: 3117
			public InputAnalogActionData_t analogActionData;
		}

		// Token: 0x020001EB RID: 491
		[Serializable]
		public struct DigitalAction_t
		{
			// Token: 0x04000C2E RID: 3118
			public InputDigitalActionHandle_t actionHandle;

			// Token: 0x04000C2F RID: 3119
			public InputDigitalActionData_t digitalActionData;
		}

		// Token: 0x020001EC RID: 492
		[Serializable]
		[StructLayout(LayoutKind.Explicit)]
		public struct OptionValue
		{
			// Token: 0x04000C30 RID: 3120
			[FieldOffset(0)]
			public SteamInputActionEvent_t.AnalogAction_t analogAction;

			// Token: 0x04000C31 RID: 3121
			[FieldOffset(0)]
			public SteamInputActionEvent_t.DigitalAction_t digitalAction;
		}
	}
}
