using System;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CFF RID: 3327
	[DefaultExecutionOrder(32000)]
	internal sealed class LongWayHomeStadiumMixGuard : MonoBehaviour
	{
		// Token: 0x060060C4 RID: 24772 RVA: 0x00266A79 File Offset: 0x00264C79
		public void Bind(LongWayHomeStadiumSequence owner)
		{
			this._owner = owner;
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x00266A82 File Offset: 0x00264C82
		private void LateUpdate()
		{
			if (this._owner != null)
			{
				this._owner.LateMix();
			}
		}

		// Token: 0x040062C4 RID: 25284
		private LongWayHomeStadiumSequence _owner;
	}
}
