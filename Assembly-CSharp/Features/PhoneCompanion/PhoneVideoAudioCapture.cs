using System;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E53 RID: 3667
	[DisallowMultipleComponent]
	public sealed class PhoneVideoAudioCapture : MonoBehaviour
	{
		// Token: 0x06006D86 RID: 28038 RVA: 0x002BFE74 File Offset: 0x002BE074
		public void Begin(Action<float[], int> callback)
		{
			object callbackLock = this._callbackLock;
			lock (callbackLock)
			{
				this._callback = callback;
			}
		}

		// Token: 0x06006D87 RID: 28039 RVA: 0x002BFEB8 File Offset: 0x002BE0B8
		public void End()
		{
			object callbackLock = this._callbackLock;
			lock (callbackLock)
			{
				this._callback = null;
			}
		}

		// Token: 0x06006D88 RID: 28040 RVA: 0x002BFEFC File Offset: 0x002BE0FC
		private void OnAudioFilterRead(float[] data, int channels)
		{
			object callbackLock = this._callbackLock;
			Action<float[], int> callback;
			lock (callbackLock)
			{
				callback = this._callback;
			}
			if (callback != null)
			{
				callback(data, channels);
			}
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x002BFF48 File Offset: 0x002BE148
		private void OnDisable()
		{
			this.End();
		}

		// Token: 0x04007027 RID: 28711
		private readonly object _callbackLock = new object();

		// Token: 0x04007028 RID: 28712
		private Action<float[], int> _callback;
	}
}
