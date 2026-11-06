using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200037B RID: 891
	[AddComponentMenu("")]
	public class MMSequencerDemoManager : MonoBehaviour
	{
		// Token: 0x0600186A RID: 6250 RVA: 0x00067C73 File Offset: 0x00065E73
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00067C7B File Offset: 0x00065E7B
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.Toggle();
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00067C8A File Offset: 0x00065E8A
		protected virtual void Toggle()
		{
			if (this.TargetSequencer.Playing)
			{
				this.TargetSequencer.StopSequence();
				return;
			}
			this.TargetSequencer.PlaySequence();
		}

		// Token: 0x04001C6A RID: 7274
		[Header("Sequence")]
		public MMFeedbacksSequencer TargetSequencer;
	}
}
