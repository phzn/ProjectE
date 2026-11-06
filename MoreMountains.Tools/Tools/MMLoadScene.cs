using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x02000147 RID: 327
	public class MMLoadScene : MonoBehaviour
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x00023240 File Offset: 0x00021440
		public virtual void LoadScene()
		{
			switch (this.LoadingSceneMode)
			{
			case MMLoadScene.LoadingSceneModes.UnityNative:
				SceneManager.LoadScene(this.SceneName);
				return;
			case MMLoadScene.LoadingSceneModes.MMSceneLoadingManager:
				MMSceneLoadingManager.LoadScene(this.SceneName);
				return;
			case MMLoadScene.LoadingSceneModes.MMAdditiveSceneLoadingManager:
				MMAdditiveSceneLoadingManager.LoadScene(this.SceneName, "MMAdditiveLoadingScreen", ThreadPriority.High, true, true, 0f, 0.25f, 0.1f, 0.25f, 0.2f, null, null, 5f, MMAdditiveSceneLoadingManager.FadeModes.FadeInThenOut, MMAdditiveSceneLoadingManagerSettings.UnloadMethods.AllScenes, "", null);
				return;
			default:
				return;
			}
		}

		// Token: 0x040008C7 RID: 2247
		[Tooltip("the name of the scene that needs to be loaded when LoadScene gets called")]
		public string SceneName;

		// Token: 0x040008C8 RID: 2248
		[Tooltip("defines whether the scene will be loaded using Unity's native API or MoreMountains' way")]
		public MMLoadScene.LoadingSceneModes LoadingSceneMode;

		// Token: 0x02000448 RID: 1096
		public enum LoadingSceneModes
		{
			// Token: 0x04001FE1 RID: 8161
			UnityNative,
			// Token: 0x04001FE2 RID: 8162
			MMSceneLoadingManager,
			// Token: 0x04001FE3 RID: 8163
			MMAdditiveSceneLoadingManager
		}
	}
}
