using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200037A RID: 890
	[AddComponentMenu("")]
	public class LettersDemoManager : MonoBehaviour
	{
		// Token: 0x06001863 RID: 6243 RVA: 0x00067B4F File Offset: 0x00065D4F
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00067B58 File Offset: 0x00065D58
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckAlphaInputPressedThisFrame(1))
			{
				this.PlayF();
			}
			if (FeelDemosInputHelper.CheckAlphaInputPressedThisFrame(2))
			{
				this.PlayE1();
			}
			if (FeelDemosInputHelper.CheckAlphaInputPressedThisFrame(3))
			{
				this.PlayE2();
			}
			if (FeelDemosInputHelper.CheckAlphaInputPressedThisFrame(4))
			{
				this.PlayL();
			}
			RaycastHit raycastHit;
			if (FeelDemosInputHelper.CheckMouseDown() && Physics.Raycast(Camera.main.ScreenPointToRay(FeelDemosInputHelper.MousePosition()), out raycastHit, 100f))
			{
				string name = raycastHit.transform.name;
				if (name == "ColliderF")
				{
					this.PlayF();
					return;
				}
				if (name == "ColliderE1")
				{
					this.PlayE1();
					return;
				}
				if (name == "ColliderE2")
				{
					this.PlayE2();
					return;
				}
				if (!(name == "ColliderL"))
				{
					return;
				}
				this.PlayL();
			}
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00067C23 File Offset: 0x00065E23
		protected virtual void PlayF()
		{
			MMFeedbacks feedbackF = this.FeedbackF;
			if (feedbackF == null)
			{
				return;
			}
			feedbackF.PlayFeedbacks();
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00067C35 File Offset: 0x00065E35
		protected virtual void PlayE1()
		{
			MMFeedbacks feedbackE = this.FeedbackE1;
			if (feedbackE == null)
			{
				return;
			}
			feedbackE.PlayFeedbacks();
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00067C47 File Offset: 0x00065E47
		protected virtual void PlayE2()
		{
			MMFeedbacks feedbackE = this.FeedbackE2;
			if (feedbackE == null)
			{
				return;
			}
			feedbackE.PlayFeedbacks();
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00067C59 File Offset: 0x00065E59
		protected virtual void PlayL()
		{
			MMFeedbacks feedbackL = this.FeedbackL;
			if (feedbackL == null)
			{
				return;
			}
			feedbackL.PlayFeedbacks();
		}

		// Token: 0x04001C65 RID: 7269
		[Header("Feedbacks")]
		public MMFeedbacks FeedbackF;

		// Token: 0x04001C66 RID: 7270
		public MMFeedbacks FeedbackE1;

		// Token: 0x04001C67 RID: 7271
		public MMFeedbacks FeedbackE2;

		// Token: 0x04001C68 RID: 7272
		public MMFeedbacks FeedbackL;

		// Token: 0x04001C69 RID: 7273
		protected Vector3 _mousePosition;
	}
}
