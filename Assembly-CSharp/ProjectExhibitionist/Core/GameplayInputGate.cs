using System;
using Features.Loading;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A5C RID: 2652
	public static class GameplayInputGate
	{
		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06004B84 RID: 19332 RVA: 0x001D5A10 File Offset: 0x001D3C10
		public static bool IsMainMenu
		{
			get
			{
				Scene activeScene = SceneManager.GetActiveScene();
				if (GameplayInputGate._activeSceneHandle != activeScene.handle)
				{
					GameplayInputGate._activeSceneHandle = activeScene.handle;
					string name = activeScene.name;
					GameplayInputGate._isMainMenu = name == "MainMenu";
					GameplayInputGate._isWarmUp = name == "GameWarmUp";
				}
				return GameplayInputGate._isMainMenu;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06004B85 RID: 19333 RVA: 0x001D5A68 File Offset: 0x001D3C68
		public static bool IsFrontend
		{
			get
			{
				return GameplayInputGate.IsMainMenu || GameplayInputGate._isWarmUp;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06004B86 RID: 19334 RVA: 0x001B46A7 File Offset: 0x001B28A7
		public static bool IsLoading
		{
			get
			{
				return (LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive) || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning);
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06004B87 RID: 19335 RVA: 0x001D5A78 File Offset: 0x001D3C78
		public static bool BlocksGameplayInput
		{
			get
			{
				return Application.isPlaying && (GameplayInputGate.IsFrontend || GameplayInputGate.IsLoading);
			}
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x001D5A91 File Offset: 0x001D3C91
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			GameplayInputGate._activeSceneHandle = int.MinValue;
			GameplayInputGate._isMainMenu = false;
			GameplayInputGate._isWarmUp = false;
		}

		// Token: 0x04004D41 RID: 19777
		private static int _activeSceneHandle = int.MinValue;

		// Token: 0x04004D42 RID: 19778
		private static bool _isMainMenu;

		// Token: 0x04004D43 RID: 19779
		private static bool _isWarmUp;
	}
}
