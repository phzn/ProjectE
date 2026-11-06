using System;
using System.Collections.Generic;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C3F RID: 3135
	[DefaultExecutionOrder(50)]
	public class StadiumController : MonoBehaviour
	{
		// Token: 0x06005A5E RID: 23134 RVA: 0x002360E4 File Offset: 0x002342E4
		private void OnEnable()
		{
			if (this.fieldZone != null)
			{
				this.fieldZone.PlayerEntered += this.OnPlayerEnteredField;
				this.fieldZone.PlayerExited += this.OnPlayerExitedField;
			}
			if (this.zoomController != null)
			{
				this.zoomController.ZoomEntered += this.OnZoomEntered;
				this.zoomController.ZoomExited += this.OnZoomExited;
				this.zoomController.enabled = false;
			}
			if (this.hypeTrigger != null)
			{
				this.hypeTrigger.enabled = false;
			}
			if (this.commentaryController != null)
			{
				this.commentaryController.enabled = false;
			}
			if (this.fieldZone != null && this.fieldZone.IsPlayerInside)
			{
				this.OnPlayerEnteredField(this.fieldZone.CurrentPlayer);
			}
		}

		// Token: 0x06005A5F RID: 23135 RVA: 0x002361D8 File Offset: 0x002343D8
		private void OnDisable()
		{
			this._playerInField = false;
			if (this.fieldZone != null)
			{
				this.fieldZone.PlayerEntered -= this.OnPlayerEnteredField;
				this.fieldZone.PlayerExited -= this.OnPlayerExitedField;
			}
			if (this.zoomController != null)
			{
				this.zoomController.ZoomEntered -= this.OnZoomEntered;
				this.zoomController.ZoomExited -= this.OnZoomExited;
				this.zoomController.enabled = false;
			}
			if (this.hypeTrigger != null)
			{
				this.hypeTrigger.enabled = false;
			}
			if (this.commentaryController != null)
			{
				this.commentaryController.enabled = false;
			}
			this.SetHudHiddenDuringZoom(false);
			this.HideLocationHud();
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x002362B4 File Offset: 0x002344B4
		private void LateUpdate()
		{
			if (this.hypeTrigger != null)
			{
				StadiumParachuteController activeFlight = StadiumHypeTrigger.ActiveFlight;
				this.hypeTrigger.enabled = this._playerInField || (activeFlight != null && activeFlight.Zone != null && activeFlight.Zone.gameObject.scene == base.gameObject.scene);
			}
			if (this._playerInField)
			{
				this.ShowLocationHud();
			}
		}

		// Token: 0x06005A61 RID: 23137 RVA: 0x00236334 File Offset: 0x00234534
		private void OnPlayerEnteredField(Transform player)
		{
			this._playerInField = true;
			if (this.debugLogs)
			{
				Debug.Log("[StadiumController] Player ENTERED field", this);
			}
			if (this.screenController != null)
			{
				this.screenController.SwitchToPlayerFocus(player);
			}
			if (this.zoomController != null)
			{
				this.zoomController.enabled = true;
			}
			if (this.hypeTrigger != null)
			{
				this.hypeTrigger.enabled = true;
			}
			if (this.commentaryController != null)
			{
				this.commentaryController.enabled = true;
			}
			this.ShowLocationHud();
		}

		// Token: 0x06005A62 RID: 23138 RVA: 0x002363CC File Offset: 0x002345CC
		private void OnPlayerExitedField(Transform player)
		{
			this._playerInField = false;
			if (this.debugLogs)
			{
				Debug.Log("[StadiumController] Player EXITED field", this);
			}
			if (this.hypeTrigger != null)
			{
				this.hypeTrigger.enabled = false;
			}
			if (this.commentaryController != null)
			{
				this.commentaryController.enabled = false;
			}
			if (this.zoomController != null)
			{
				this.zoomController.enabled = false;
			}
			if (this.screenController != null)
			{
				this.screenController.SwitchToAmbient();
			}
			this.HideLocationHud();
		}

		// Token: 0x06005A63 RID: 23139 RVA: 0x00236460 File Offset: 0x00234660
		private void OnZoomEntered()
		{
			this.ShowLocationHud();
			this.SetHudHiddenDuringZoom(true);
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x0023646F File Offset: 0x0023466F
		private void OnZoomExited()
		{
			this.SetHudHiddenDuringZoom(false);
			if (this._playerInField)
			{
				this.ShowLocationHud();
			}
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x00236488 File Offset: 0x00234688
		private void SetHudHiddenDuringZoom(bool hidden)
		{
			if (!hidden)
			{
				foreach (KeyValuePair<GameObject, bool> keyValuePair in this._hiddenHudStates)
				{
					if (keyValuePair.Key != null)
					{
						keyValuePair.Key.SetActive(keyValuePair.Value);
					}
				}
				this._hiddenHudStates.Clear();
				return;
			}
			if (this.hudToHideDuringZoom != null)
			{
				for (int i = 0; i < this.hudToHideDuringZoom.Length; i++)
				{
					GameObject gameObject = this.hudToHideDuringZoom[i];
					this.HideHudContainer(gameObject);
				}
			}
			GameObject[] array = this.ResolveHudByName();
			if (array != null)
			{
				foreach (GameObject gameObject2 in array)
				{
					this.HideHudContainer(gameObject2);
				}
			}
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x0023655C File Offset: 0x0023475C
		private void HideHudContainer(GameObject go)
		{
			if (go == null || this._hiddenHudStates.ContainsKey(go))
			{
				return;
			}
			this._hiddenHudStates.Add(go, go.activeSelf);
			go.SetActive(false);
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x00236590 File Offset: 0x00234790
		private GameObject[] ResolveHudByName()
		{
			if (this._resolvedHudByName != null)
			{
				return this._resolvedHudByName;
			}
			if (this.hudContainerNamesToHide == null || this.hudContainerNamesToHide.Length == 0)
			{
				return null;
			}
			List<GameObject> list = new List<GameObject>(this.hudContainerNamesToHide.Length);
			Transform[] array = Object.FindObjectsOfType<Transform>(true);
			for (int i = 0; i < this.hudContainerNamesToHide.Length; i++)
			{
				string text = this.hudContainerNamesToHide[i];
				if (!string.IsNullOrEmpty(text))
				{
					GameObject gameObject = null;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].name == text)
						{
							gameObject = array[j].gameObject;
							break;
						}
					}
					if (gameObject != null)
					{
						list.Add(gameObject);
					}
					else if (this.debugLogs)
					{
						Debug.LogWarning("[StadiumController] HUD container '" + text + "' not found to hide during zoom.", this);
					}
				}
			}
			this._resolvedHudByName = list.ToArray();
			return this._resolvedHudByName;
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x00236674 File Offset: 0x00234874
		private void ShowLocationHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (this._presentedHud != instance)
			{
				this.HideLocationHud();
				this._presentedHud = instance;
			}
			if (instance == null)
			{
				return;
			}
			InteractionConfigSO interactionConfigSO = ((this.zoomController != null && this.zoomController.IsZoomActive) ? this.zoomHudConfig : this.fieldHudConfig);
			instance.ShowSupplemental(this, interactionConfigSO);
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x002366DE File Offset: 0x002348DE
		private void HideLocationHud()
		{
			if (this._presentedHud != null)
			{
				this._presentedHud.HideSupplemental(this);
			}
			this._presentedHud = null;
		}

		// Token: 0x04005B54 RID: 23380
		[Header("Sub-controllers")]
		[SerializeField]
		private StadiumFieldZone fieldZone;

		// Token: 0x04005B55 RID: 23381
		[SerializeField]
		private StadiumScreenController screenController;

		// Token: 0x04005B56 RID: 23382
		[SerializeField]
		private StadiumScreenZoomController zoomController;

		// Token: 0x04005B57 RID: 23383
		[SerializeField]
		private StadiumHypeTrigger hypeTrigger;

		// Token: 0x04005B58 RID: 23384
		[SerializeField]
		private StadiumCommentaryController commentaryController;

		// Token: 0x04005B59 RID: 23385
		[Header("HUD")]
		[Tooltip("HUD config shown while the player is in the field but not zoomed (T=Zoom, G=Tifa).")]
		[SerializeField]
		private InteractionConfigSO fieldHudConfig;

		// Token: 0x04005B5A RID: 23386
		[Tooltip("HUD config shown while the player is zoomed on the maxi screen (C=Change Camera, T=Exit Zoom).")]
		[SerializeField]
		private InteractionConfigSO zoomHudConfig;

		// Token: 0x04005B5B RID: 23387
		[Tooltip("Game HUD elements to hide while zoom is active (and re-show on exit). Do NOT include the InteractionHUDController root — the zoom HUD must stay visible. Use this when the HUD lives in the same scene (e.g. the Stadium). When the HUD lives in the persistent _Core (POI scenes like the Pool), it can't be wired here — use 'hudContainerNamesToHide' instead, which resolves by name at runtime.")]
		[SerializeField]
		private GameObject[] hudToHideDuringZoom;

		// Token: 0x04005B5C RID: 23388
		[Tooltip("Names of HUD container GameObjects to hide during zoom, resolved by name at runtime (works across scenes — e.g. the _Core's HUDCanvas children). Everything NOT listed stays visible: leave the actions container (UIMiddle) out so the executable actions remain on screen, exactly like the Stadium hides only UITop/UIBottom.")]
		[SerializeField]
		private string[] hudContainerNamesToHide = new string[] { "UITop", "UIBottom" };

		// Token: 0x04005B5D RID: 23389
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005B5E RID: 23390
		private GameObject[] _resolvedHudByName;

		// Token: 0x04005B5F RID: 23391
		private readonly Dictionary<GameObject, bool> _hiddenHudStates = new Dictionary<GameObject, bool>();

		// Token: 0x04005B60 RID: 23392
		private bool _playerInField;

		// Token: 0x04005B61 RID: 23393
		private InteractionHUDController _presentedHud;
	}
}
