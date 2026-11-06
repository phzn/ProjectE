using System;

namespace Features.PlayerEmotions
{
	// Token: 0x02000E11 RID: 3601
	public class EmotionChangedEventArgs : EventArgs
	{
		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x06006B28 RID: 27432 RVA: 0x002AF1E1 File Offset: 0x002AD3E1
		public float OldBalance { get; }

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x06006B29 RID: 27433 RVA: 0x002AF1E9 File Offset: 0x002AD3E9
		public float NewBalance { get; }

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x06006B2A RID: 27434 RVA: 0x002AF1F1 File Offset: 0x002AD3F1
		public string SourceId { get; }

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x06006B2B RID: 27435 RVA: 0x002AF1F9 File Offset: 0x002AD3F9
		public EmotionSourceCategory Category { get; }

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x06006B2C RID: 27436 RVA: 0x002AF201 File Offset: 0x002AD401
		public float Delta
		{
			get
			{
				return this.NewBalance - this.OldBalance;
			}
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x06006B2D RID: 27437 RVA: 0x002AF210 File Offset: 0x002AD410
		public bool IsShame
		{
			get
			{
				return this.NewBalance < 0f;
			}
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x06006B2E RID: 27438 RVA: 0x002AF21F File Offset: 0x002AD41F
		public bool IsExhibition
		{
			get
			{
				return this.NewBalance > 0f;
			}
		}

		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x06006B2F RID: 27439 RVA: 0x002AF22E File Offset: 0x002AD42E
		public float ShameLevel
		{
			get
			{
				if (this.NewBalance >= 0f)
				{
					return 0f;
				}
				return -this.NewBalance;
			}
		}

		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x06006B30 RID: 27440 RVA: 0x002AF24A File Offset: 0x002AD44A
		public float ExhibitionLevel
		{
			get
			{
				if (this.NewBalance <= 0f)
				{
					return 0f;
				}
				return this.NewBalance;
			}
		}

		// Token: 0x06006B31 RID: 27441 RVA: 0x002AF265 File Offset: 0x002AD465
		public EmotionChangedEventArgs(float oldBalance, float newBalance, string sourceId, EmotionSourceCategory category)
		{
			this.OldBalance = oldBalance;
			this.NewBalance = newBalance;
			this.SourceId = sourceId;
			this.Category = category;
		}
	}
}
