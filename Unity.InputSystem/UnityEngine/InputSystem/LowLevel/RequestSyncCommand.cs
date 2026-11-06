using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C4 RID: 196
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct RequestSyncCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000D38 RID: 3384 RVA: 0x000446A4 File Offset: 0x000428A4
		public static FourCC Type
		{
			get
			{
				return new FourCC('S', 'Y', 'N', 'C');
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x000446B3 File Offset: 0x000428B3
		public FourCC typeStatic
		{
			get
			{
				return RequestSyncCommand.Type;
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x000446BC File Offset: 0x000428BC
		public static RequestSyncCommand Create()
		{
			return new RequestSyncCommand
			{
				baseCommand = new InputDeviceCommand(RequestSyncCommand.Type, 8)
			};
		}

		// Token: 0x04000526 RID: 1318
		internal const int kSize = 8;

		// Token: 0x04000527 RID: 1319
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;
	}
}
