using System;

namespace Invector.vShooter
{
	// Token: 0x0200066F RID: 1647
	public static class IKLocomotionOptionsHelper
	{
		// Token: 0x060025F4 RID: 9716 RVA: 0x000D3E70 File Offset: 0x000D2070
		public static vShooterWeapon.IKLocomotionOptions Copy(this vShooterWeapon.IKLocomotionOptions options)
		{
			return new vShooterWeapon.IKLocomotionOptions
			{
				use = options.use,
				useOnIdle = options.useOnIdle,
				useOnWalk = options.useOnWalk,
				useOnRun = options.useOnRun,
				useOnSprint = options.useOnSprint
			};
		}
	}
}
