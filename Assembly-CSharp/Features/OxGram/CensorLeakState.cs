using System;
using Features.PhoneCompanion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.OxGram
{
	// Token: 0x02000E89 RID: 3721
	public static class CensorLeakState
	{
		// Token: 0x06006FC3 RID: 28611 RVA: 0x002D22A9 File Offset: 0x002D04A9
		static CensorLeakState()
		{
			SceneManager.activeSceneChanged += CensorLeakState.HandleActiveSceneChanged;
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06006FC4 RID: 28612 RVA: 0x002D22BC File Offset: 0x002D04BC
		// (set) Token: 0x06006FC5 RID: 28613 RVA: 0x002D22D6 File Offset: 0x002D04D6
		public static bool GameOverPending
		{
			get
			{
				if (CensorLeakState._gameOverPending && !OxGramPhotoPrivacy.CanLeakInCurrentScene)
				{
					CensorLeakState.Reset();
				}
				return CensorLeakState._gameOverPending;
			}
			set
			{
				CensorLeakState._gameOverPending = value && OxGramPhotoPrivacy.CanLeakInCurrentScene;
				SmartphoneController.SetCensorLeakMovementLock(CensorLeakState._gameOverPending);
			}
		}

		// Token: 0x06006FC6 RID: 28614 RVA: 0x002D22F2 File Offset: 0x002D04F2
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
			CensorLeakState.Reset();
			SceneManager.activeSceneChanged -= CensorLeakState.HandleActiveSceneChanged;
			SceneManager.activeSceneChanged += CensorLeakState.HandleActiveSceneChanged;
		}

		// Token: 0x06006FC7 RID: 28615 RVA: 0x002D231B File Offset: 0x002D051B
		private static void HandleActiveSceneChanged(Scene previous, Scene current)
		{
			if (!OxGramPhotoPrivacy.CanLeakInCurrentScene)
			{
				CensorLeakState.Reset();
			}
		}

		// Token: 0x06006FC8 RID: 28616 RVA: 0x002D2329 File Offset: 0x002D0529
		public static void Reset()
		{
			CensorLeakState.GameOverPending = false;
		}

		// Token: 0x0400729A RID: 29338
		private static bool _gameOverPending;
	}
}
