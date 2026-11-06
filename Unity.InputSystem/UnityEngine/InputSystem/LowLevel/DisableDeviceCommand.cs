using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000B3 RID: 179
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct DisableDeviceCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x000440C6 File Offset: 0x000422C6
		public static FourCC Type
		{
			get
			{
				return new FourCC('D', 'S', 'B', 'L');
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x000440D5 File Offset: 0x000422D5
		public FourCC typeStatic
		{
			get
			{
				return DisableDeviceCommand.Type;
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000440DC File Offset: 0x000422DC
		public static DisableDeviceCommand Create()
		{
			return new DisableDeviceCommand
			{
				baseCommand = new InputDeviceCommand(DisableDeviceCommand.Type, 8)
			};
		}

		// Token: 0x040004F6 RID: 1270
		internal const int kSize = 8;

		// Token: 0x040004F7 RID: 1271
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;
	}
}
