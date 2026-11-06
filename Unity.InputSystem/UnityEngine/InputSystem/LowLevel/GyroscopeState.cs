using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D7 RID: 215
	internal struct GyroscopeState : IInputStateTypeInfo
	{
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00044A7F File Offset: 0x00042C7F
		public static FourCC kFormat
		{
			get
			{
				return new FourCC('G', 'Y', 'R', 'O');
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x00044A8E File Offset: 0x00042C8E
		public FourCC format
		{
			get
			{
				return GyroscopeState.kFormat;
			}
		}

		// Token: 0x04000577 RID: 1399
		[InputControl(displayName = "Angular Velocity", processors = "CompensateDirection", noisy = true)]
		public Vector3 angularVelocity;
	}
}
