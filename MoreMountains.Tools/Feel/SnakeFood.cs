using System;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200037F RID: 895
	[AddComponentMenu("")]
	public class SnakeFood : MonoBehaviour
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x00068281 File Offset: 0x00066481
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x00068289 File Offset: 0x00066489
		public SnakeFoodSpawner Spawner { get; set; }

		// Token: 0x06001889 RID: 6281 RVA: 0x00068294 File Offset: 0x00066494
		protected void OnTriggerEnter2D(Collider2D other)
		{
			this._snake = other.GetComponent<Snake>();
			if (this._snake != null)
			{
				this._snake.Eat();
				MMFeedbacks eatFeedback = this.EatFeedback;
				if (eatFeedback != null)
				{
					eatFeedback.PlayFeedbacks();
				}
				base.StartCoroutine(this.MoveFood());
			}
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x000682E4 File Offset: 0x000664E4
		protected virtual IEnumerator MoveFood()
		{
			this.Model.SetActive(false);
			yield return MMCoroutine.WaitFor(this.OffDelay);
			this.Model.SetActive(true);
			base.transform.position = this.Spawner.DetermineSpawnPosition();
			MMFeedbacks appearFeedback = this.AppearFeedback;
			if (appearFeedback != null)
			{
				appearFeedback.PlayFeedbacks();
			}
			yield break;
		}

		// Token: 0x04001C8E RID: 7310
		public float OffDelay = 1f;

		// Token: 0x04001C8F RID: 7311
		public GameObject Model;

		// Token: 0x04001C90 RID: 7312
		public MMFeedbacks EatFeedback;

		// Token: 0x04001C91 RID: 7313
		public MMFeedbacks AppearFeedback;

		// Token: 0x04001C93 RID: 7315
		protected Snake _snake;
	}
}
