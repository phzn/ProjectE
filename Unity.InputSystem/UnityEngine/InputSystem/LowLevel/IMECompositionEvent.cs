using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E4 RID: 228
	[StructLayout(LayoutKind.Explicit, Size = 152)]
	public struct IMECompositionEvent : IInputEventTypeInfo
	{
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x000451A6 File Offset: 0x000433A6
		public FourCC typeStatic
		{
			get
			{
				return 1229800787;
			}
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x000451B4 File Offset: 0x000433B4
		public static IMECompositionEvent Create(int deviceId, string compositionString, double time)
		{
			return new IMECompositionEvent
			{
				baseEvent = new InputEvent(1229800787, 152, deviceId, time),
				compositionString = new IMECompositionString(compositionString)
			};
		}

		// Token: 0x040005A7 RID: 1447
		internal const int kIMECharBufferSize = 64;

		// Token: 0x040005A8 RID: 1448
		public const int Type = 1229800787;

		// Token: 0x040005A9 RID: 1449
		[FieldOffset(0)]
		public InputEvent baseEvent;

		// Token: 0x040005AA RID: 1450
		[FieldOffset(20)]
		public IMECompositionString compositionString;
	}
}
