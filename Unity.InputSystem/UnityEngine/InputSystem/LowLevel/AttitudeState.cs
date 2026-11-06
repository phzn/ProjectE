using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000D9 RID: 217
	internal struct AttitudeState : IInputStateTypeInfo
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00044AAB File Offset: 0x00042CAB
		public static FourCC kFormat
		{
			get
			{
				return new FourCC('A', 'T', 'T', 'D');
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x00044ABA File Offset: 0x00042CBA
		public FourCC format
		{
			get
			{
				return AttitudeState.kFormat;
			}
		}

		// Token: 0x04000579 RID: 1401
		[InputControl(displayName = "Attitude", processors = "CompensateRotation", noisy = true)]
		public Quaternion attitude;
	}
}
