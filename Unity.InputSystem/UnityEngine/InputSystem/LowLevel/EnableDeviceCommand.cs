using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000B4 RID: 180
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct EnableDeviceCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00044104 File Offset: 0x00042304
		public static FourCC Type
		{
			get
			{
				return new FourCC('E', 'N', 'B', 'L');
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00044113 File Offset: 0x00042313
		public FourCC typeStatic
		{
			get
			{
				return EnableDeviceCommand.Type;
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0004411C File Offset: 0x0004231C
		public static EnableDeviceCommand Create()
		{
			return new EnableDeviceCommand
			{
				baseCommand = new InputDeviceCommand(EnableDeviceCommand.Type, 8)
			};
		}

		// Token: 0x040004F8 RID: 1272
		internal const int kSize = 8;

		// Token: 0x040004F9 RID: 1273
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;
	}
}
