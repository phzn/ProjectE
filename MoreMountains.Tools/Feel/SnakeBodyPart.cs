using System;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200037E RID: 894
	[AddComponentMenu("")]
	public class SnakeBodyPart : MonoBehaviour
	{
		// Token: 0x06001880 RID: 6272 RVA: 0x000681AB File Offset: 0x000663AB
		protected virtual void Awake()
		{
			this._collider2D = base.gameObject.MMGetComponentNoAlloc<BoxCollider2D>();
			base.StartCoroutine(this.ActivateCollider());
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x000681CB File Offset: 0x000663CB
		protected virtual IEnumerator ActivateCollider()
		{
			yield return MMCoroutine.WaitFor(1f);
			this._collider2D.enabled = true;
			yield break;
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x000681DA File Offset: 0x000663DA
		protected void Update()
		{
			base.transform.position = this.TargetRecorder.Positions[this.Offset];
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x000681FD File Offset: 0x000663FD
		public virtual void Eat(float intensity)
		{
			MMFeedbacks eatFeedback = this.EatFeedback;
			if (eatFeedback == null)
			{
				return;
			}
			eatFeedback.PlayFeedbacks(base.transform.position, intensity, false);
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0006821C File Offset: 0x0006641C
		public virtual void New()
		{
			MMFeedbacks newFeedback = this.NewFeedback;
			if (newFeedback != null)
			{
				newFeedback.Initialization(false);
			}
			MMFeedbacks newFeedback2 = this.NewFeedback;
			if (newFeedback2 == null)
			{
				return;
			}
			newFeedback2.PlayFeedbacks();
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00068240 File Offset: 0x00066440
		protected void OnTriggerEnter2D(Collider2D other)
		{
			if (this.Index == 0)
			{
				return;
			}
			this._snake = other.GetComponent<Snake>();
			if (this._snake != null)
			{
				this._snake.Lose(this);
			}
		}

		// Token: 0x04001C87 RID: 7303
		public MMPositionRecorder TargetRecorder;

		// Token: 0x04001C88 RID: 7304
		public MMFeedbacks EatFeedback;

		// Token: 0x04001C89 RID: 7305
		public MMFeedbacks NewFeedback;

		// Token: 0x04001C8A RID: 7306
		public int Offset = 20;

		// Token: 0x04001C8B RID: 7307
		public int Index;

		// Token: 0x04001C8C RID: 7308
		protected Snake _snake;

		// Token: 0x04001C8D RID: 7309
		protected BoxCollider2D _collider2D;
	}
}
