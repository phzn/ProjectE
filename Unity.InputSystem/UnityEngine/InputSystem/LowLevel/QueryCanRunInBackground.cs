using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000BB RID: 187
	[StructLayout(LayoutKind.Explicit, Size = 9)]
	public struct QueryCanRunInBackground : IInputDeviceCommandInfo
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0004425E File Offset: 0x0004245E
		public static FourCC Type
		{
			get
			{
				return new FourCC('Q', 'R', 'I', 'B');
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0004426D File Offset: 0x0004246D
		public FourCC typeStatic
		{
			get
			{
				return QueryCanRunInBackground.Type;
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00044274 File Offset: 0x00042474
		public static QueryCanRunInBackground Create()
		{
			return new QueryCanRunInBackground
			{
				baseCommand = new InputDeviceCommand(QueryCanRunInBackground.Type, 9),
				canRunInBackground = false
			};
		}

		// Token: 0x04000505 RID: 1285
		internal const int kSize = 9;

		// Token: 0x04000506 RID: 1286
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x04000507 RID: 1287
		[FieldOffset(8)]
		public bool canRunInBackground;
	}
}
