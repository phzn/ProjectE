using System;
using Features.MatchRecap;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000CA3 RID: 3235
	public sealed class ClassicHomeReturnRuntime : MonoBehaviour
	{
		// Token: 0x06005DEA RID: 24042 RVA: 0x002520B7 File Offset: 0x002502B7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			if (Object.FindObjectOfType<ClassicHomeReturnRuntime>() != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[Classic Home Return]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<ClassicHomeReturnRuntime>();
		}

		// Token: 0x06005DEB RID: 24043 RVA: 0x002520E0 File Offset: 0x002502E0
		private void Update()
		{
			if (this._portal != null)
			{
				return;
			}
			if (!RunModeService.HomeReturnEnabled || SceneManager.GetActiveScene().name != "Game - Current")
			{
				return;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance == null || !instance.IsRunTrackingActive || instance.IsMocked)
			{
				return;
			}
			if (Time.unscaledTime < this._nextResolve)
			{
				return;
			}
			this._nextResolve = Time.unscaledTime + 0.5f;
			Vector3 vector;
			Vector3 vector2;
			if (!LongWayHomeWorldRoute.TryResolveHomeEntrance(ChunkManager.Instance, out vector, out vector2))
			{
				return;
			}
			if (this._portal == null)
			{
				GameObject gameObject = new GameObject("Classic Home Return Doorway");
				SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
				gameObject.transform.position = vector;
				this._portal = gameObject.AddComponent<PoiScenePortal>();
				this._portal.Configure(PoiScenePortal.PortalMode.ReturnHome, "CielHome", "Home", null, 2.25f);
			}
			this._portal.transform.position = vector;
			PoiPortalPresentation component = this._portal.GetComponent<PoiPortalPresentation>();
			if (component == null)
			{
				return;
			}
			component.SetHomeCardAnchor(vector2);
		}

		// Token: 0x04005EF5 RID: 24309
		private PoiScenePortal _portal;

		// Token: 0x04005EF6 RID: 24310
		private float _nextResolve;
	}
}
