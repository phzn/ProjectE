using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C1 RID: 193
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	internal struct QuerySamplingFrequencyCommand : IInputDeviceCommandInfo
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x000445B4 File Offset: 0x000427B4
		public static FourCC Type
		{
			get
			{
				return new FourCC('S', 'M', 'P', 'L');
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x000445C3 File Offset: 0x000427C3
		public FourCC typeStatic
		{
			get
			{
				return QuerySamplingFrequencyCommand.Type;
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000445CC File Offset: 0x000427CC
		public static QuerySamplingFrequencyCommand Create()
		{
			return new QuerySamplingFrequencyCommand
			{
				baseCommand = new InputDeviceCommand(QuerySamplingFrequencyCommand.Type, 12)
			};
		}

		// Token: 0x0400051D RID: 1309
		internal const int kSize = 12;

		// Token: 0x0400051E RID: 1310
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x0400051F RID: 1311
		[FieldOffset(8)]
		public float frequency;
	}
}
