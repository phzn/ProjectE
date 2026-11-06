using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D6 RID: 214
	internal struct AccelerometerState : IInputStateTypeInfo
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00044A69 File Offset: 0x00042C69
		public static FourCC kFormat
		{
			get
			{
				return new FourCC('A', 'C', 'C', 'L');
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x00044A78 File Offset: 0x00042C78
		public FourCC format
		{
			get
			{
				return AccelerometerState.kFormat;
			}
		}

		// Token: 0x04000576 RID: 1398
		[InputControl(displayName = "Acceleration", processors = "CompensateDirection", noisy = true)]
		public Vector3 acceleration;
	}
}
