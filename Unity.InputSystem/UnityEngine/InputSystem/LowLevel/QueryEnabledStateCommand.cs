using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000BD RID: 189
	[StructLayout(LayoutKind.Explicit, Size = 9)]
	public struct QueryEnabledStateCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x000442E5 File Offset: 0x000424E5
		public static FourCC Type
		{
			get
			{
				return new FourCC('Q', 'E', 'N', 'B');
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x000442F4 File Offset: 0x000424F4
		public FourCC typeStatic
		{
			get
			{
				return QueryEnabledStateCommand.Type;
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x000442FC File Offset: 0x000424FC
		public static QueryEnabledStateCommand Create()
		{
			return new QueryEnabledStateCommand
			{
				baseCommand = new InputDeviceCommand(QueryEnabledStateCommand.Type, 9)
			};
		}

		// Token: 0x0400050B RID: 1291
		internal const int kSize = 9;

		// Token: 0x0400050C RID: 1292
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x0400050D RID: 1293
		[FieldOffset(8)]
		public bool isEnabled;
	}
}
