using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000DA RID: 218
	internal struct LinearAccelerationState : IInputStateTypeInfo
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00044AC1 File Offset: 0x00042CC1
		public static FourCC kFormat
		{
			get
			{
				return new FourCC('L', 'A', 'A', 'C');
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x00044AD0 File Offset: 0x00042CD0
		public FourCC format
		{
			get
			{
				return LinearAccelerationState.kFormat;
			}
		}

		// Token: 0x0400057A RID: 1402
		[InputControl(displayName = "Acceleration", processors = "CompensateDirection", noisy = true)]
		public Vector3 acceleration;
	}
}
