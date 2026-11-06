using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C7 RID: 199
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	internal struct WarpMousePositionCommand : IInputDeviceCommandInfo
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x0004477D File Offset: 0x0004297D
		public static FourCC Type
		{
			get
			{
				return new FourCC('W', 'P', 'M', 'S');
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x0004478C File Offset: 0x0004298C
		public FourCC typeStatic
		{
			get
			{
				return WarpMousePositionCommand.Type;
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00044794 File Offset: 0x00042994
		public static WarpMousePositionCommand Create(Vector2 position)
		{
			return new WarpMousePositionCommand
			{
				baseCommand = new InputDeviceCommand(WarpMousePositionCommand.Type, 16),
				warpPositionInPlayerDisplaySpace = position
			};
		}

		// Token: 0x0400052E RID: 1326
		internal const int kSize = 16;

		// Token: 0x0400052F RID: 1327
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x04000530 RID: 1328
		[FieldOffset(8)]
		public Vector2 warpPositionInPlayerDisplaySpace;
	}
}
