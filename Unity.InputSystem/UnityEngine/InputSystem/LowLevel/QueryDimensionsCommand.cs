using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000BC RID: 188
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct QueryDimensionsCommand : IInputDeviceCommandInfo
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x000442A5 File Offset: 0x000424A5
		public static FourCC Type
		{
			get
			{
				return new FourCC('D', 'I', 'M', 'S');
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x000442B4 File Offset: 0x000424B4
		public FourCC typeStatic
		{
			get
			{
				return QueryDimensionsCommand.Type;
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x000442BC File Offset: 0x000424BC
		public static QueryDimensionsCommand Create()
		{
			return new QueryDimensionsCommand
			{
				baseCommand = new InputDeviceCommand(QueryDimensionsCommand.Type, 16)
			};
		}

		// Token: 0x04000508 RID: 1288
		internal const int kSize = 16;

		// Token: 0x04000509 RID: 1289
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x0400050A RID: 1290
		[FieldOffset(8)]
		public Vector2 outDimensions;
	}
}
