using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200016D RID: 365
	[AddComponentMenu("More Mountains/Tools/Achievements/MMAchievementDisplayer")]
	public class MMAchievementDisplayer : MonoBehaviour, MMEventListener<MMAchievementUnlockedEvent>, MMEventListenerBase
	{
		// Token: 0x0600089B RID: 2203 RVA: 0x000289FF File Offset: 0x00026BFF
		public virtual IEnumerator DisplayAchievement(MMAchievement achievement)
		{
			if (base.transform == null || this.AchievementDisplayPrefab == null)
			{
				yield break;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.AchievementDisplayPrefab.gameObject);
			gameObject.transform.SetParent(base.transform, false);
			MMAchievementDisplayItem component = gameObject.GetComponent<MMAchievementDisplayItem>();
			if (component == null)
			{
				yield break;
			}
			component.Title.text = achievement.Title;
			component.Description.text = achievement.Description;
			component.Icon.sprite = achievement.UnlockedImage;
			if (achievement.AchievementType == AchievementTypes.Progress)
			{
				component.ProgressBarDisplay.gameObject.SetActive(true);
			}
			else
			{
				component.ProgressBarDisplay.gameObject.SetActive(false);
			}
			if (achievement.UnlockedSound != null)
			{
				MMSfxEvent.Trigger(achievement.UnlockedSound, null, 1f, 1f, 128);
			}
			CanvasGroup achievementCanvasGroup = gameObject.GetComponent<CanvasGroup>();
			if (achievementCanvasGroup != null)
			{
				achievementCanvasGroup.alpha = 0f;
				base.StartCoroutine(MMFade.FadeCanvasGroup(achievementCanvasGroup, this.AchievementFadeDuration, 1f, true));
				yield return this._achievementFadeOutWFS;
				base.StartCoroutine(MMFade.FadeCanvasGroup(achievementCanvasGroup, this.AchievementFadeDuration, 0f, true));
			}
			yield break;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00028A15 File Offset: 0x00026C15
		public virtual void OnMMEvent(MMAchievementUnlockedEvent achievementUnlockedEvent)
		{
			base.StartCoroutine(this.DisplayAchievement(achievementUnlockedEvent.Achievement));
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00028A2A File Offset: 0x00026C2A
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMAchievementUnlockedEvent>();
			this._achievementFadeOutWFS = new WaitForSeconds(this.AchievementFadeDuration + this.AchievementDisplayDuration);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00028A4A File Offset: 0x00026C4A
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMAchievementUnlockedEvent>();
		}

		// Token: 0x04000A45 RID: 2629
		[Header("Achievements")]
		public MMAchievementDisplayItem AchievementDisplayPrefab;

		// Token: 0x04000A46 RID: 2630
		public float AchievementDisplayDuration = 5f;

		// Token: 0x04000A47 RID: 2631
		public float AchievementFadeDuration = 0.2f;

		// Token: 0x04000A48 RID: 2632
		protected WaitForSeconds _achievementFadeOutWFS;
	}
}
