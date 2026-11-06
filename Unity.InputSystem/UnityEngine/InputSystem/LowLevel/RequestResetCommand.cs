using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C3 RID: 195
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct RequestResetCommand : IInputDeviceCommandInfo
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00044664 File Offset: 0x00042864
		public static FourCC Type
		{
			get
			{
				return new FourCC('R', 'S', 'E', 'T');
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x00044673 File Offset: 0x00042873
		public FourCC typeStatic
		{
			get
			{
				return RequestResetCommand.Type;
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0004467C File Offset: 0x0004287C
		public static RequestResetCommand Create()
		{
			return new RequestResetCommand
			{
				baseCommand = new InputDeviceCommand(RequestResetCommand.Type, 8)
			};
		}

		// Token: 0x04000524 RID: 1316
		internal const int kSize = 8;

		// Token: 0x04000525 RID: 1317
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;
	}
}
