using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200016C RID: 364
	[Serializable]
	public class MMAchievement
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x00028948 File Offset: 0x00026B48
		public virtual void UnlockAchievement()
		{
			if (this.UnlockedStatus)
			{
				return;
			}
			this.UnlockedStatus = true;
			MMGameEvent.Trigger("Save", 0, default(Vector2), default(Vector3), false, "");
			MMAchievementUnlockedEvent.Trigger(this);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002898E File Offset: 0x00026B8E
		public virtual void LockAchievement()
		{
			this.UnlockedStatus = false;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00028997 File Offset: 0x00026B97
		public virtual void AddProgress(int newProgress)
		{
			this.ProgressCurrent += newProgress;
			this.EvaluateProgress();
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x000289AD File Offset: 0x00026BAD
		public virtual void SetProgress(int newProgress)
		{
			this.ProgressCurrent = newProgress;
			this.EvaluateProgress();
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000289BC File Offset: 0x00026BBC
		protected virtual void EvaluateProgress()
		{
			MMAchievementChangedEvent.Trigger(this);
			if (this.ProgressCurrent >= this.ProgressTarget)
			{
				this.ProgressCurrent = this.ProgressTarget;
				this.UnlockAchievement();
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x000289E4 File Offset: 0x00026BE4
		public virtual MMAchievement Copy()
		{
			new MMAchievement();
			return JsonUtility.FromJson<MMAchievement>(JsonUtility.ToJson(this));
		}

		// Token: 0x04000A39 RID: 2617
		[Header("Identification")]
		public string AchievementID;

		// Token: 0x04000A3A RID: 2618
		public AchievementTypes AchievementType;

		// Token: 0x04000A3B RID: 2619
		public bool HiddenAchievement;

		// Token: 0x04000A3C RID: 2620
		public bool UnlockedStatus;

		// Token: 0x04000A3D RID: 2621
		[Header("Description")]
		public string Title;

		// Token: 0x04000A3E RID: 2622
		public string Description;

		// Token: 0x04000A3F RID: 2623
		public int Points;

		// Token: 0x04000A40 RID: 2624
		[Header("Image and Sounds")]
		public Sprite LockedImage;

		// Token: 0x04000A41 RID: 2625
		public Sprite UnlockedImage;

		// Token: 0x04000A42 RID: 2626
		public AudioClip UnlockedSound;

		// Token: 0x04000A43 RID: 2627
		[Header("Progress")]
		public int ProgressTarget;

		// Token: 0x04000A44 RID: 2628
		public int ProgressCurrent;
	}
}
