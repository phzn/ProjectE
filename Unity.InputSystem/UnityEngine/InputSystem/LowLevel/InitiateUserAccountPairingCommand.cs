using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000B7 RID: 183
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct InitiateUserAccountPairingCommand : IInputDeviceCommandInfo
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x000441A3 File Offset: 0x000423A3
		public static FourCC Type
		{
			get
			{
				return new FourCC('P', 'A', 'I', 'R');
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x000441B2 File Offset: 0x000423B2
		public FourCC typeStatic
		{
			get
			{
				return InitiateUserAccountPairingCommand.Type;
			}
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000441BC File Offset: 0x000423BC
		public static InitiateUserAccountPairingCommand Create()
		{
			return new InitiateUserAccountPairingCommand
			{
				baseCommand = new InputDeviceCommand(InitiateUserAccountPairingCommand.Type, 8)
			};
		}

		// Token: 0x040004FD RID: 1277
		internal const int kSize = 8;

		// Token: 0x040004FE RID: 1278
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x02000207 RID: 519
		public enum Result
		{
			// Token: 0x04000B7F RID: 2943
			SuccessfullyInitiated = 1,
			// Token: 0x04000B80 RID: 2944
			ErrorNotSupported = -1,
			// Token: 0x04000B81 RID: 2945
			ErrorAlreadyInProgress = -2
		}
	}
}
