using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C6 RID: 454
	public class MMF_PlayerDebugInput : MonoBehaviour
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x0003264D File Offset: 0x0003084D
		protected virtual void Awake()
		{
			this._player = base.gameObject.GetComponent<MMF_Player>();
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00032660 File Offset: 0x00030860
		protected virtual void Update()
		{
			if (Input.GetKeyDown(this.PlayButton))
			{
				this._player.PlayFeedbacks();
			}
		}

		// Token: 0x04000D0F RID: 3343
		public KeyCode PlayButton = KeyCode.P;

		// Token: 0x04000D10 RID: 3344
		protected MMF_Player _player;
	}
}
