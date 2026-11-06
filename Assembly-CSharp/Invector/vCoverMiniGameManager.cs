using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Invector
{
	// Token: 0x0200043E RID: 1086
	public class vCoverMiniGameManager : MonoBehaviour
	{
		// Token: 0x06001B4D RID: 6989 RVA: 0x000AC324 File Offset: 0x000AA524
		private void Start()
		{
			if (this.getChildrenDetectionController)
			{
				this.securityCameras = base.GetComponentsInChildren<vDetectionController>();
			}
			for (int i = 0; i < this.securityCameras.Length; i++)
			{
				vDetectionController vDetectionController = this.securityCameras[i];
				vDetectionController.onFindTransformTarget.AddListener(delegate(Transform t)
				{
					this.OnPlayerIsFound();
				});
				vDetectionController.onLostTransformTarget.AddListener(delegate(Transform t)
				{
					this.OnPlayerIsLost();
				});
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000AC38D File Offset: 0x000AA58D
		public virtual void OnPlayerIsFound()
		{
			this.onPlayerIsFound.Invoke();
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x000AC39A File Offset: 0x000AA59A
		public virtual void OnPlayerIsLost()
		{
			this.onPlayerIsLost.Invoke();
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x000AC3A8 File Offset: 0x000AA5A8
		public virtual void ResetScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		// Token: 0x04001C76 RID: 7286
		public bool getChildrenDetectionController;

		// Token: 0x04001C77 RID: 7287
		public vDetectionController[] securityCameras;

		// Token: 0x04001C78 RID: 7288
		public UnityEvent onPlayerIsFound;

		// Token: 0x04001C79 RID: 7289
		public UnityEvent onPlayerIsLost;
	}
}
