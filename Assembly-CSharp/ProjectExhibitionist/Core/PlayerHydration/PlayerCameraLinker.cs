using System;
using System.Collections;
using Invector.vCamera;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.Core.PlayerHydration
{
	// Token: 0x02000AC1 RID: 2753
	[DisallowMultipleComponent]
	public class PlayerCameraLinker : MonoBehaviour
	{
		// Token: 0x06004E28 RID: 20008 RVA: 0x001E4C18 File Offset: 0x001E2E18
		private IEnumerator Start()
		{
			float elapsed = 0f;
			vThirdPersonCamera cam = null;
			while (elapsed < this.waitTimeout)
			{
				if (cam == null)
				{
					cam = vThirdPersonCamera.instance;
				}
				if (PlayerRegistry.Player != null && cam != null)
				{
					break;
				}
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			if (PlayerRegistry.Player == null)
			{
				Debug.LogWarning("[PlayerCameraLinker] Nessun player registrato: camera non agganciata.");
				yield break;
			}
			if (cam == null)
			{
				Debug.LogWarning("[PlayerCameraLinker] vThirdPersonCamera non trovata: aggancio saltato.");
				yield break;
			}
			cam.mainTarget = PlayerRegistry.Player;
			cam.Init();
			if (this.enableDebugLogs)
			{
				Debug.Log("[PlayerCameraLinker] Camera agganciata al player '" + PlayerRegistry.Player.name + "'.");
			}
			yield break;
		}

		// Token: 0x04004F74 RID: 20340
		[Tooltip("Secondi massimi di attesa che player e camera siano disponibili.")]
		[SerializeField]
		private float waitTimeout = 5f;

		// Token: 0x04004F75 RID: 20341
		[SerializeField]
		private bool enableDebugLogs = true;
	}
}
