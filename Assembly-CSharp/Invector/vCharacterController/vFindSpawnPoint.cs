using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Invector.vCharacterController
{
	// Token: 0x020006AB RID: 1707
	public class vFindSpawnPoint : MonoBehaviour
	{
		// Token: 0x06002988 RID: 10632 RVA: 0x000E9C19 File Offset: 0x000E7E19
		public void AlighObjetToSpawnPoint(GameObject target, string spawnPointName)
		{
			this.target = target;
			this.spawnPointName = spawnPointName;
			SceneManager.sceneLoaded += this.OnLevelFinishedLoading;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000E9C48 File Offset: 0x000E7E48
		private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			GameObject gameObject = GameObject.Find(this.spawnPointName);
			if (gameObject && this.target)
			{
				this.target.transform.position = gameObject.transform.position;
				this.target.transform.rotation = gameObject.transform.rotation;
				return;
			}
			try
			{
				Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}

		// Token: 0x040029AC RID: 10668
		public Transform spawnPoint;

		// Token: 0x040029AD RID: 10669
		public string spawnPointName;

		// Token: 0x040029AE RID: 10670
		public GameObject target;
	}
}
