using System;
using Features.MainMenu;
using Features.Rank;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Achievements
{
	// Token: 0x0200153A RID: 5434
	public sealed class AchievementRuntimeHost : MonoBehaviour
	{
		// Token: 0x0600A835 RID: 43061 RVA: 0x00463B99 File Offset: 0x00461D99
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			if (AchievementRuntimeHost._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Achievement Runtime");
			Object.DontDestroyOnLoad(gameObject);
			AchievementRuntimeHost._instance = gameObject.AddComponent<AchievementRuntimeHost>();
		}

		// Token: 0x0600A836 RID: 43062 RVA: 0x00463BC3 File Offset: 0x00461DC3
		private void Awake()
		{
			CollectionUnlockRegistry.Changed += this.CollectionChanged;
			RankProgressionService.Changed += this.CollectionChanged;
			SceneManager.sceneLoaded += this.SceneLoaded;
		}

		// Token: 0x0600A837 RID: 43063 RVA: 0x00463BF8 File Offset: 0x00461DF8
		private void CollectionChanged()
		{
			this._collectionDirty = true;
		}

		// Token: 0x0600A838 RID: 43064 RVA: 0x00463C01 File Offset: 0x00461E01
		private void SceneLoaded(Scene scene, LoadSceneMode mode)
		{
			AchievementTrackingService.SceneArrived(scene.name);
		}

		// Token: 0x0600A839 RID: 43065 RVA: 0x00463C0F File Offset: 0x00461E0F
		private void Update()
		{
			if (this._collectionDirty)
			{
				this._collectionDirty = false;
				AchievementTrackingService.CheckCollection();
			}
			if (Time.unscaledTime < this._nextFlush)
			{
				return;
			}
			this._nextFlush = Time.unscaledTime + 5f;
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A83A RID: 43066 RVA: 0x00463C49 File Offset: 0x00461E49
		private void OnApplicationPause(bool paused)
		{
			if (paused)
			{
				AchievementTrackingService.SaveNow();
			}
		}

		// Token: 0x0600A83B RID: 43067 RVA: 0x00463C53 File Offset: 0x00461E53
		private void OnApplicationQuit()
		{
			AchievementTrackingService.SaveNow();
		}

		// Token: 0x0600A83C RID: 43068 RVA: 0x00463C5C File Offset: 0x00461E5C
		private void OnDestroy()
		{
			CollectionUnlockRegistry.Changed -= this.CollectionChanged;
			RankProgressionService.Changed -= this.CollectionChanged;
			SceneManager.sceneLoaded -= this.SceneLoaded;
			AchievementTrackingService.SaveNow();
			if (AchievementRuntimeHost._instance == this)
			{
				AchievementRuntimeHost._instance = null;
			}
		}

		// Token: 0x0400AD0D RID: 44301
		private static AchievementRuntimeHost _instance;

		// Token: 0x0400AD0E RID: 44302
		private float _nextFlush;

		// Token: 0x0400AD0F RID: 44303
		private bool _collectionDirty;
	}
}
