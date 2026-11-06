using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D8 RID: 216
	internal struct GravityState : IInputStateTypeInfo
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00044A95 File Offset: 0x00042C95
		public static FourCC kFormat
		{
			get
			{
				return new FourCC('G', 'R', 'V', ' ');
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x00044AA4 File Offset: 0x00042CA4
		public FourCC format
		{
			get
			{
				return GravityState.kFormat;
			}
		}

		// Token: 0x04000578 RID: 1400
		[InputControl(displayName = "Gravity", processors = "CompensateDirection", noisy = true)]
		public Vector3 gravity;
	}
}
