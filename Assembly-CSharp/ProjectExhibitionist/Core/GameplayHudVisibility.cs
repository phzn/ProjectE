using System;
using System.Collections.Generic;
using ExhibitionistDemo.UI;
using Features.Detection;
using Features.Loading;
using Features.MainMenu;
using Features.Score;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Subway;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A54 RID: 2644
	public sealed class GameplayHudVisibility : MonoBehaviour
	{
		// Token: 0x06004B40 RID: 19264 RVA: 0x001D4861 File Offset: 0x001D2A61
		public static void RegisterCanvas(Canvas canvas)
		{
			if (!Application.isPlaying || canvas == null)
			{
				return;
			}
			GameplayHudVisibility.Bootstrap();
			GameplayHudVisibility._instance.RegisterCanvasForDiscovery(canvas);
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x001D4884 File Offset: 0x001D2A84
		private void RegisterCanvasForDiscovery(Canvas canvas)
		{
			if (canvas == null)
			{
				return;
			}
			this._renderCanvases.RemoveAll((Canvas knownCanvas) => knownCanvas == null);
			this.TrackCanvas(canvas);
			this.TrackCanvas(canvas.rootCanvas);
			this.ForgetCanvasGraphics(canvas);
			if (canvas.rootCanvas != canvas)
			{
				this.ForgetCanvasGraphics(canvas.rootCanvas);
			}
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x001D48FC File Offset: 0x001D2AFC
		private void ForgetCanvasGraphics(Canvas canvas)
		{
			if (canvas == null)
			{
				return;
			}
			IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
			for (int i = 0; i < graphicsForCanvas.Count; i++)
			{
				Graphic graphic = graphicsForCanvas[i];
				if (!(graphic == null))
				{
					this._knownGraphics.Remove(graphic);
					UnityAction unityAction;
					if (this._dirtyCallbacks.TryGetValue(graphic, out unityAction))
					{
						graphic.UnregisterDirtyMaterialCallback(unityAction);
						this._dirtyCallbacks.Remove(graphic);
					}
				}
			}
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x001D496D File Offset: 0x001D2B6D
		private void TrackCanvas(Canvas canvas)
		{
			if (canvas != null && !this._renderCanvases.Contains(canvas))
			{
				this._renderCanvases.Add(canvas);
			}
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x001D4994 File Offset: 0x001D2B94
		private void DiscoverCanvases()
		{
			this._renderCanvases.RemoveAll((Canvas canvas) => canvas == null);
			foreach (Canvas canvas2 in Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				this.TrackCanvas(canvas2);
			}
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x001D49ED File Offset: 0x001D2BED
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (GameplayHudVisibility._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[Gameplay HUD Visibility]");
			Object.DontDestroyOnLoad(gameObject);
			GameplayHudVisibility._instance = gameObject.AddComponent<GameplayHudVisibility>();
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x001D4A18 File Offset: 0x001D2C18
		private void OnEnable()
		{
			this._inMainMenu = SceneManager.GetActiveScene().name == "MainMenu";
			Canvas.preWillRenderCanvases += this.Refresh;
			Canvas.willRenderCanvases += this.RefreshLateGraphics;
			GameSettingsService.SettingsChanged += this.Invalidate;
			SceneManager.activeSceneChanged += this.SceneChanged;
			SceneManager.sceneLoaded += this.SceneLoaded;
			this.DiscoverCanvases();
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x001D4AA0 File Offset: 0x001D2CA0
		private void OnDisable()
		{
			Canvas.preWillRenderCanvases -= this.Refresh;
			Canvas.willRenderCanvases -= this.RefreshLateGraphics;
			GameSettingsService.SettingsChanged -= this.Invalidate;
			SceneManager.activeSceneChanged -= this.SceneChanged;
			SceneManager.sceneLoaded -= this.SceneLoaded;
			this._renderCanvases.Clear();
			this.ClearDiscovery();
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x001D4B13 File Offset: 0x001D2D13
		private void SceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.DiscoverCanvases();
			this.ClearDiscovery();
			this.Refresh();
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x001D4B27 File Offset: 0x001D2D27
		private void SceneChanged(Scene before, Scene after)
		{
			this._inMainMenu = after.name == "MainMenu";
			this.ClearDiscovery();
			this.Refresh();
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x001D4B4C File Offset: 0x001D2D4C
		private void ClearDiscovery()
		{
			foreach (KeyValuePair<Graphic, UnityAction> keyValuePair in this._dirtyCallbacks)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.UnregisterDirtyMaterialCallback(keyValuePair.Value);
				}
			}
			this._dirtyCallbacks.Clear();
			this._knownGraphics.Clear();
			this._attachedRoots.Clear();
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x001D4BDC File Offset: 0x001D2DDC
		private void RevisitWhenMaterialChanges(Graphic graphic)
		{
			if (this._dirtyCallbacks.ContainsKey(graphic))
			{
				return;
			}
			UnityAction callback = null;
			callback = delegate
			{
				this._knownGraphics.Remove(graphic);
				this._dirtyCallbacks.Remove(graphic);
				if (graphic != null)
				{
					graphic.UnregisterDirtyMaterialCallback(callback);
				}
			};
			this._dirtyCallbacks.Add(graphic, callback);
			graphic.RegisterDirtyMaterialCallback(callback);
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x001D4C54 File Offset: 0x001D2E54
		private void Invalidate()
		{
			bool flag = !GameSettingsService.ShowFaceCamera;
			bool flag2 = !GameSettingsService.ShowDistanceAndScore;
			bool flag3 = !GameSettingsService.ShowGameplayUI;
			if (this._lastHideFace == flag && this._lastHideScore == flag2 && this._lastHideAll == flag3)
			{
				return;
			}
			this._lastHideFace = flag;
			this._lastHideScore = flag2;
			this._lastHideAll = flag3;
			this.ClearDiscovery();
			this.Refresh();
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x001D4CBC File Offset: 0x001D2EBC
		public void Refresh()
		{
			if (this._refreshing || this._inMainMenu)
			{
				return;
			}
			bool flag = !GameSettingsService.ShowFaceCamera;
			bool flag2 = !GameSettingsService.ShowDistanceAndScore;
			bool flag3 = !GameSettingsService.ShowGameplayUI;
			if (!flag && !flag2 && !flag3)
			{
				return;
			}
			this._refreshing = true;
			try
			{
				if (Time.unscaledTime >= this._nextPrune)
				{
					this._knownGraphics.RemoveWhere((Graphic graphic) => graphic == null);
					this._attachedRoots.RemoveWhere((GameObject root) => root == null);
					this._renderCanvases.RemoveAll((Canvas canvas) => canvas == null);
					this._deadCallbacks.Clear();
					foreach (KeyValuePair<Graphic, UnityAction> keyValuePair in this._dirtyCallbacks)
					{
						if (keyValuePair.Key == null)
						{
							this._deadCallbacks.Add(keyValuePair.Key);
						}
					}
					foreach (Graphic graphic2 in this._deadCallbacks)
					{
						this._dirtyCallbacks.Remove(graphic2);
					}
					this._deadCallbacks.Clear();
					this._nextPrune = Time.unscaledTime + 10f;
				}
				foreach (Canvas canvas2 in this._renderCanvases)
				{
					this.RefreshCanvasGraphics(canvas2, flag, flag2, flag3);
				}
			}
			finally
			{
				this._refreshing = false;
			}
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x001D4EF8 File Offset: 0x001D30F8
		private void RefreshLateGraphics()
		{
			bool flag = !GameSettingsService.ShowFaceCamera;
			bool flag2 = !GameSettingsService.ShowDistanceAndScore;
			bool flag3 = !GameSettingsService.ShowGameplayUI;
			if (this._refreshing || this._inMainMenu || (!flag && !flag2 && !flag3))
			{
				return;
			}
			this._refreshing = true;
			try
			{
				GameplayHudVisualMask.RefreshPendingChildren();
				foreach (Canvas canvas in this._renderCanvases)
				{
					this.RefreshCanvasGraphics(canvas, flag, flag2, flag3);
				}
			}
			finally
			{
				this._refreshing = false;
			}
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x001D4FA8 File Offset: 0x001D31A8
		private void RefreshCanvas(Canvas canvas)
		{
			this.RefreshCanvasGraphics(canvas, !GameSettingsService.ShowFaceCamera, !GameSettingsService.ShowDistanceAndScore, !GameSettingsService.ShowGameplayUI);
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x001D4FC9 File Offset: 0x001D31C9
		private void AttachRoot(GameObject root, HudDisplayPart part)
		{
			if (this._attachedRoots.Add(root))
			{
				GameplayHudVisualMask.Attach(root, part);
			}
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x001D4FE4 File Offset: 0x001D31E4
		private void RefreshCanvasGraphics(Canvas canvas, bool hideFace, bool hideScore, bool hideAll)
		{
			if (canvas == null || this._inMainMenu)
			{
				return;
			}
			IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
			for (int i = 0; i < graphicsForCanvas.Count; i++)
			{
				Graphic graphic = graphicsForCanvas[i];
				if (!(graphic == null) && this._knownGraphics.Add(graphic))
				{
					if (GameplayHudVisibility.IsProtected(graphic.transform))
					{
						this.RevisitWhenMaterialChanges(graphic);
					}
					else
					{
						RawImage rawImage = graphic as RawImage;
						if (rawImage != null && rawImage.texture == null)
						{
							this.RevisitWhenMaterialChanges(graphic);
						}
						if (hideFace)
						{
							RawImage rawImage2 = graphic as RawImage;
							if (rawImage2 != null && rawImage2.texture != null && rawImage2.texture.name == "CielFaceCam")
							{
								Transform parent = rawImage2.transform.parent;
								this.AttachRoot((parent != null && parent.name == "FaceCamContainer") ? parent.gameObject : rawImage2.gameObject, HudDisplayPart.FaceCamera);
							}
						}
						if (hideScore)
						{
							DistanceHUD componentInParent = graphic.GetComponentInParent<DistanceHUD>();
							RunScoreHUD componentInParent2 = graphic.GetComponentInParent<RunScoreHUD>();
							if (componentInParent != null)
							{
								this.AttachRoot(componentInParent.gameObject, HudDisplayPart.DistanceAndScore);
							}
							if (componentInParent2 != null)
							{
								this.AttachRoot(componentInParent2.gameObject, HudDisplayPart.DistanceAndScore);
							}
						}
						if ((hideAll || graphic.GetComponentInParent<GameplayHudVisualMask>(true) != null) && graphic.GetComponent<GameplayHudVisualMask>() == null)
						{
							GameplayHudVisualMask.Attach(graphic.gameObject, HudDisplayPart.Inherit);
						}
					}
				}
			}
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x001D5168 File Offset: 0x001D3368
		public static bool IsProtected(Transform target)
		{
			return target.GetComponentInParent<MainMenuSettingsPanel>(true) != null || target.GetComponentInParent<GraphicsSettingsRuntimeUI>(true) != null || target.GetComponentInParent<LoadingScreenController>(true) != null || target.GetComponentInParent<SubwayScreenFade>(true) != null || target.GetComponentInParent<ScreenFadeController>(true) != null;
		}

		// Token: 0x04004D1B RID: 19739
		private static GameplayHudVisibility _instance;

		// Token: 0x04004D1C RID: 19740
		private bool _refreshing;

		// Token: 0x04004D1D RID: 19741
		private readonly HashSet<Graphic> _knownGraphics = new HashSet<Graphic>();

		// Token: 0x04004D1E RID: 19742
		private readonly HashSet<GameObject> _attachedRoots = new HashSet<GameObject>();

		// Token: 0x04004D1F RID: 19743
		private readonly Dictionary<Graphic, UnityAction> _dirtyCallbacks = new Dictionary<Graphic, UnityAction>();

		// Token: 0x04004D20 RID: 19744
		private readonly List<Graphic> _deadCallbacks = new List<Graphic>();

		// Token: 0x04004D21 RID: 19745
		private float _nextPrune;

		// Token: 0x04004D22 RID: 19746
		private readonly List<Canvas> _renderCanvases = new List<Canvas>();

		// Token: 0x04004D23 RID: 19747
		private bool _lastHideFace;

		// Token: 0x04004D24 RID: 19748
		private bool _lastHideScore;

		// Token: 0x04004D25 RID: 19749
		private bool _lastHideAll;

		// Token: 0x04004D26 RID: 19750
		private bool _inMainMenu;
	}
}
