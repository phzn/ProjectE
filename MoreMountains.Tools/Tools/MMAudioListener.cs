using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000B7 RID: 183
	[RequireComponent(typeof(AudioListener))]
	public class MMAudioListener : MonoBehaviour
	{
		// Token: 0x0600039B RID: 923 RVA: 0x000121D0 File Offset: 0x000103D0
		protected virtual void OnEnable()
		{
			this._audioListener = base.gameObject.GetComponent<AudioListener>();
			this._otherListeners = Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
			foreach (AudioListener audioListener in this._otherListeners)
			{
				if (audioListener != null && audioListener != this._audioListener)
				{
					audioListener.enabled = false;
				}
			}
		}

		// Token: 0x04000512 RID: 1298
		protected AudioListener _audioListener;

		// Token: 0x04000513 RID: 1299
		protected AudioListener[] _otherListeners;
	}
}
