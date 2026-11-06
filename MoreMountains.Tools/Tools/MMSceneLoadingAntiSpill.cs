using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x0200014E RID: 334
	public class MMSceneLoadingAntiSpill
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x000239FC File Offset: 0x00021BFC
		public virtual void PrepareAntiFill(string sceneToLoadName, string antiSpillSceneName = "")
		{
			this._destinationScene = default(Scene);
			this._sceneToLoadName = sceneToLoadName;
			if (antiSpillSceneName == "")
			{
				this._antiSpillScene = SceneManager.CreateScene("AntiSpill_" + sceneToLoadName);
				this.PrepareAntiFillSetSceneActive();
				return;
			}
			MMSceneLoadingAntiSpill._scenesInBuild = MMScene.GetScenesInBuild();
			if (!MMSceneLoadingAntiSpill._scenesInBuild.Contains(antiSpillSceneName))
			{
				Debug.LogError("MMSceneLoadingAntiSpill : impossible to load the '" + antiSpillSceneName + "' scene, there is no such scene in the project's build settings.");
				return;
			}
			SceneManager.LoadScene(antiSpillSceneName, LoadSceneMode.Additive);
			this._antiSpillScene = SceneManager.GetSceneByName(antiSpillSceneName);
			this._antiSpillSceneName = this._antiSpillScene.name;
			SceneManager.sceneLoaded += this.PrepareAntiFillOnSceneLoaded;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00023AA9 File Offset: 0x00021CA9
		protected virtual void PrepareAntiFillOnSceneLoaded(Scene newScene, LoadSceneMode mode)
		{
			if (newScene.name != this._antiSpillSceneName)
			{
				return;
			}
			SceneManager.sceneLoaded -= this.PrepareAntiFillOnSceneLoaded;
			this.PrepareAntiFillSetSceneActive();
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00023AD8 File Offset: 0x00021CD8
		protected virtual void PrepareAntiFillSetSceneActive()
		{
			if (this._onActiveSceneChangedCallback != null)
			{
				SceneManager.activeSceneChanged -= this._onActiveSceneChangedCallback;
			}
			this._onActiveSceneChangedCallback = new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			SceneManager.activeSceneChanged += this._onActiveSceneChangedCallback;
			SceneManager.SetActiveScene(this._antiSpillScene);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00023B17 File Offset: 0x00021D17
		protected virtual void OnActiveSceneChanged(Scene from, Scene to)
		{
			if (from == this._antiSpillScene)
			{
				SceneManager.activeSceneChanged -= this._onActiveSceneChangedCallback;
				this._onActiveSceneChangedCallback = null;
				this.EmptyAntiSpillScene();
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00023B40 File Offset: 0x00021D40
		protected virtual void EmptyAntiSpillScene()
		{
			if (this._antiSpillScene.IsValid() && this._antiSpillScene.isLoaded)
			{
				this._spillSceneRoots.Clear();
				this._antiSpillScene.GetRootGameObjects(this._spillSceneRoots);
				this._destinationScene = SceneManager.GetSceneByName(this._sceneToLoadName);
				if (this._spillSceneRoots.Count > 0 && this._destinationScene.IsValid() && this._destinationScene.isLoaded)
				{
					foreach (GameObject gameObject in this._spillSceneRoots)
					{
						SceneManager.MoveGameObjectToScene(gameObject, this._destinationScene);
					}
				}
				SceneManager.UnloadSceneAsync(this._antiSpillScene);
			}
		}

		// Token: 0x0400090B RID: 2315
		protected Scene _antiSpillScene;

		// Token: 0x0400090C RID: 2316
		protected Scene _destinationScene;

		// Token: 0x0400090D RID: 2317
		protected UnityAction<Scene, Scene> _onActiveSceneChangedCallback;

		// Token: 0x0400090E RID: 2318
		protected string _sceneToLoadName;

		// Token: 0x0400090F RID: 2319
		protected string _antiSpillSceneName;

		// Token: 0x04000910 RID: 2320
		protected List<GameObject> _spillSceneRoots = new List<GameObject>(50);

		// Token: 0x04000911 RID: 2321
		protected static List<string> _scenesInBuild;
	}
}
