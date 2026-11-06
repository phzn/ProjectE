using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core.SceneManagement
{
	// Token: 0x02000A7B RID: 2683
	[DisallowMultipleComponent]
	public class PlayerRegistry : MonoBehaviour
	{
		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x001DAAEC File Offset: 0x001D8CEC
		// (set) Token: 0x06004C51 RID: 19537 RVA: 0x001DAAF3 File Offset: 0x001D8CF3
		public static Transform Player { get; private set; }

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004C52 RID: 19538 RVA: 0x001DAAFB File Offset: 0x001D8CFB
		// (set) Token: 0x06004C53 RID: 19539 RVA: 0x001DAB03 File Offset: 0x001D8D03
		public Scene OriginalScene { get; private set; }

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06004C54 RID: 19540 RVA: 0x001DAB0C File Offset: 0x001D8D0C
		// (remove) Token: 0x06004C55 RID: 19541 RVA: 0x001DAB40 File Offset: 0x001D8D40
		public static event Action<Transform> PlayerChanged;

		// Token: 0x06004C56 RID: 19542 RVA: 0x001DAB73 File Offset: 0x001D8D73
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			PlayerRegistry.Player = null;
			PlayerRegistry.PlayerChanged = null;
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004C57 RID: 19543 RVA: 0x001DAB81 File Offset: 0x001D8D81
		public static GameObject PlayerObject
		{
			get
			{
				if (!(PlayerRegistry.Player != null))
				{
					return null;
				}
				return PlayerRegistry.Player.gameObject;
			}
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x001DAB9C File Offset: 0x001D8D9C
		private void Awake()
		{
			this.OriginalScene = base.gameObject.scene;
			if (PlayerRegistry.Player != null && PlayerRegistry.Player != base.transform)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[PlayerRegistry] Esiste già un player registrato ('",
					PlayerRegistry.Player.name,
					"'). Questo ('",
					base.name,
					"') lo sostituisce. Dovrebbe essercene uno solo (nel _Core)."
				}));
			}
			PlayerRegistry.Player = base.transform;
			Action<Transform> playerChanged = PlayerRegistry.PlayerChanged;
			if (playerChanged != null)
			{
				playerChanged(PlayerRegistry.Player);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[PlayerRegistry] Player registrato: '" + base.name + "'.");
			}
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x001DAC5C File Offset: 0x001D8E5C
		public bool TryRestoreSceneBeforeUnload(Scene leavingScene, Scene persistentScene)
		{
			if (!leavingScene.IsValid() || !leavingScene.isLoaded || !persistentScene.IsValid() || !persistentScene.isLoaded || leavingScene == persistentScene || this.OriginalScene != persistentScene || base.gameObject.scene != leavingScene)
			{
				return false;
			}
			string text = ((base.transform.parent != null) ? base.transform.parent.name : "<root>");
			base.transform.SetParent(null, true);
			SceneManager.MoveGameObjectToScene(base.gameObject, persistentScene);
			Debug.LogWarning(string.Concat(new string[] { "[PlayerRegistry] Restored persistent player '", base.name, "' from scene '", leavingScene.name, "' (parent '", text, "') to '", persistentScene.name, "' before unload." }), this);
			return true;
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x001DAD59 File Offset: 0x001D8F59
		private void OnDestroy()
		{
			if (PlayerRegistry.Player == base.transform)
			{
				PlayerRegistry.Player = null;
				Action<Transform> playerChanged = PlayerRegistry.PlayerChanged;
				if (playerChanged == null)
				{
					return;
				}
				playerChanged(null);
			}
		}

		// Token: 0x04004DD3 RID: 19923
		[SerializeField]
		private bool enableDebugLogs = true;
	}
}
