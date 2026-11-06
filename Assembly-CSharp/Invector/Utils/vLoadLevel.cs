using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000600 RID: 1536
	[vClassHeader("Load Level", true, "icon_v2", false, "", openClose = false)]
	public class vLoadLevel : vMonoBehaviour
	{
		// Token: 0x0600237A RID: 9082 RVA: 0x000C9AD0 File Offset: 0x000C7CD0
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				vThirdPersonInput component = other.transform.gameObject.GetComponent<vThirdPersonInput>();
				LoadLevelHelper.LoadScene(this.levelToLoad, this.spawnPointName, component);
			}
		}

		// Token: 0x0400238C RID: 9100
		[Tooltip("Write the name of the level you want to load")]
		public string levelToLoad;

		// Token: 0x0400238D RID: 9101
		[Tooltip("Assign here the spawnPoint name of the scene that you will load")]
		public string spawnPointName;
	}
}
