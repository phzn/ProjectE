using System;
using UnityEngine;

namespace Features.TimeFreeze
{
	// Token: 0x02000C0F RID: 3087
	[DefaultExecutionOrder(32000)]
	internal sealed class TimeFreezeAudioGuardRunner : MonoBehaviour
	{
		// Token: 0x06005950 RID: 22864 RVA: 0x0022EBB7 File Offset: 0x0022CDB7
		public void Bind(TimeFreezeAudioSnapshot owner)
		{
			this._owner = owner;
		}

		// Token: 0x06005951 RID: 22865 RVA: 0x0022EBC0 File Offset: 0x0022CDC0
		private void LateUpdate()
		{
			TimeFreezeAudioSnapshot owner = this._owner;
			if (owner == null)
			{
				return;
			}
			owner.GuardLateTick();
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x0022EBD2 File Offset: 0x0022CDD2
		private void OnDestroy()
		{
			TimeFreezeAudioSnapshot owner = this._owner;
			this._owner = null;
			if (owner == null)
			{
				return;
			}
			owner.OnGuardRunnerDestroyed(this);
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x0022EBEC File Offset: 0x0022CDEC
		public void Detach()
		{
			this._owner = null;
		}

		// Token: 0x040059F4 RID: 23028
		private TimeFreezeAudioSnapshot _owner;
	}
}
