using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x0200022B RID: 555
	[AddComponentMenu("Modern UI Pack/Notification/Notification Stacking")]
	public class NotificationStacking : MonoBehaviour
	{
		// Token: 0x06000F3E RID: 3902 RVA: 0x000659D8 File Offset: 0x00063BD8
		private void Update()
		{
			if (this.notifications.Count == 0)
			{
				return;
			}
			if (this.enableUpdating && this.notifications[this.currentNotification] != null)
			{
				this.notifications[this.currentNotification].Open();
				base.StopCoroutine("StartNotification");
				base.StartCoroutine("StartNotification");
				this.enableUpdating = false;
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00065A48 File Offset: 0x00063C48
		public void AddToStack(NotificationManager notif)
		{
			this.notifications.Add(notif);
			notif.gameObject.SetActive(false);
			this.enableUpdating = true;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00065A69 File Offset: 0x00063C69
		private IEnumerator StartNotification()
		{
			yield return new WaitForSecondsRealtime(this.notifications[this.currentNotification].timer + this.delay);
			Object.Destroy(this.notifications[this.currentNotification].gameObject);
			if (this.currentNotification == this.notifications.Count - 1)
			{
				this.notifications.Clear();
				this.currentNotification = 0;
			}
			else
			{
				this.currentNotification++;
				this.enableUpdating = true;
			}
			yield break;
		}

		// Token: 0x04001021 RID: 4129
		[Header("Settings")]
		public float delay = 1f;

		// Token: 0x04001022 RID: 4130
		private List<NotificationManager> notifications = new List<NotificationManager>();

		// Token: 0x04001023 RID: 4131
		private int currentNotification;

		// Token: 0x04001024 RID: 4132
		private bool enableUpdating;
	}
}
