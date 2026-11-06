using System;
using System.Collections;
using System.Collections.Generic;
using DA_Assets.CR;
using Features.Localization;
using Features.Workshop;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x020010A4 RID: 4260
	public sealed class MainMenuWorkshopPanel : MonoBehaviour
	{
		// Token: 0x060080F8 RID: 33016 RVA: 0x0034991E File Offset: 0x00347B1E
		private void OnEnable()
		{
			if (this._gridRoot == null)
			{
				return;
			}
			WorkshopService.ItemsChanged += this.RefreshStatuses;
			GameLocalization.LocaleChanged += this.RefreshLanguage;
			this.Query(0);
		}

		// Token: 0x060080F9 RID: 33017 RVA: 0x00349958 File Offset: 0x00347B58
		private void OnDisable()
		{
			WorkshopService.ItemsChanged -= this.RefreshStatuses;
			GameLocalization.LocaleChanged -= this.RefreshLanguage;
			this._generation++;
			IDisposable queryRequest = this._queryRequest;
			if (queryRequest != null)
			{
				queryRequest.Dispose();
			}
			this._queryRequest = null;
			IDisposable operation = this._operation;
			if (operation != null)
			{
				operation.Dispose();
			}
			this._operation = null;
			this._working = false;
			this.ClearCards();
			this._selected = null;
		}

		// Token: 0x060080FA RID: 33018 RVA: 0x003499DC File Offset: 0x00347BDC
		private void Update()
		{
			if (this._viewport == null)
			{
				return;
			}
			float width = this._viewport.rect.width;
			if (width > 0f && Mathf.Abs(width - this._lastGridWidth) > 1f)
			{
				this._lastGridWidth = width;
				int num = ((width >= 850f) ? 3 : ((width >= 500f) ? 2 : 1));
				float num2 = (width - (float)(num - 1) * this._grid.spacing.x) / (float)num;
				this._grid.constraintCount = num;
				this._grid.cellSize = new Vector2(num2, (num2 - 16f) * 0.5625f + 88f);
			}
			if (Time.unscaledTime < this._nextStatusRefresh)
			{
				return;
			}
			this._nextStatusRefresh = Time.unscaledTime + 0.5f;
			this.RefreshStatuses();
		}

		// Token: 0x060080FB RID: 33019 RVA: 0x00349AB7 File Offset: 0x00347CB7
		private void ChangeScope(WorkshopScope scope)
		{
			if (this._scope == scope && this._page != null)
			{
				return;
			}
			this._scope = scope;
			this.Query(0);
		}

		// Token: 0x060080FC RID: 33020 RVA: 0x00349ADC File Offset: 0x00347CDC
		private void Query(int pageIndex)
		{
			MainMenuWorkshopPanel.<>c__DisplayClass20_0 CS$<>8__locals1 = new MainMenuWorkshopPanel.<>c__DisplayClass20_0();
			CS$<>8__locals1.<>4__this = this;
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this._pageIndex = Mathf.Max(0, pageIndex);
			MainMenuWorkshopPanel.<>c__DisplayClass20_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this._generation + 1;
			this._generation = num;
			CS$<>8__locals2.generation = num;
			IDisposable queryRequest = this._queryRequest;
			if (queryRequest != null)
			{
				queryRequest.Dispose();
			}
			this._queryRequest = null;
			this.ClearCards();
			this.Select(null);
			this._searchRoot.SetActive(this._scope == WorkshopScope.Available);
			MainMenuWorkshopPanel.SetTabColor(this._browse, this._scope == WorkshopScope.Available);
			MainMenuWorkshopPanel.SetTabColor(this._subscribed, this._scope == WorkshopScope.Subscribed);
			this._scroll.StopMovement();
			this._scroll.verticalNormalizedPosition = 1f;
			this._queryRequest = WorkshopService.RequestPage(this._scope, this._pageIndex, (this._scope == WorkshopScope.Available) ? this._search.text.Trim() : string.Empty, delegate(WorkshopPage result)
			{
				if (CS$<>8__locals1.<>4__this == null || !CS$<>8__locals1.<>4__this.isActiveAndEnabled || CS$<>8__locals1.generation != CS$<>8__locals1.<>4__this._generation)
				{
					return;
				}
				CS$<>8__locals1.<>4__this._page = result;
				CS$<>8__locals1.<>4__this.RenderPage();
			});
		}

		// Token: 0x060080FD RID: 33021 RVA: 0x00349BE4 File Offset: 0x00347DE4
		private void RenderPage()
		{
			if (this._page == null)
			{
				return;
			}
			bool flag = this._page.State == WorkshopPageState.Ready;
			bool flag2 = this._page.State == WorkshopPageState.Loading;
			if (flag && this._pageIndex > 0 && (long)this._pageIndex * 50L >= (long)this._page.TotalCount)
			{
				this.Query(Math.Max(0, (this._page.TotalCount - 1) / 50));
				return;
			}
			this._previous.interactable = flag && this._pageIndex > 0;
			this._next.interactable = flag && this._page.HasNextPage;
			this._emptyRoot.SetActive(!flag || this._page.Items.Count == 0);
			this._retry.gameObject.SetActive(!flag && !flag2);
			this._pageLabel.text = (flag ? GameLocalization.Format("workshop.page", "Page {0} / {1}", new object[]
			{
				this._pageIndex + 1,
				Math.Max(1, (int)Math.Ceiling((double)this._page.TotalCount / 50.0))
			}) : string.Empty);
			this._emptyText.text = (flag2 ? MainMenuWorkshopPanel.T("loading", "Loading Workshop...") : ((!flag) ? MainMenuWorkshopPanel.Error(this._page.ErrorCode) : (MainMenuWorkshopPanel.T((this._scope == WorkshopScope.Subscribed) ? "empty.subscribed" : "empty.browse", (this._scope == WorkshopScope.Subscribed) ? "You haven't subscribed to any mods yet." : "No mods found.") + "\n\n" + MainMenuWorkshopPanel.T("empty.hint", "Try another search or explore the Workshop on Steam."))));
			if (!flag)
			{
				return;
			}
			if (this._cards.Count == 0)
			{
				foreach (WorkshopItem workshopItem in this._page.Items)
				{
					MainMenuWorkshopPanel.Card card = this.BuildCard(workshopItem);
					this._cards.Add(card);
					if (MainMenuWorkshopPanel.IsAllowedPreviewUrl(workshopItem.PreviewUrl))
					{
						this._previewQueue.Enqueue(card);
					}
				}
				for (int i = 0; i < Math.Min(3, this._previewQueue.Count); i++)
				{
					base.StartCoroutine(this.LoadPreviews(this._generation));
				}
				if (this._page.Items.Count > 0)
				{
					this.Select(this._page.Items[0]);
				}
			}
			this.RefreshStatuses();
		}

		// Token: 0x060080FE RID: 33022 RVA: 0x00349E94 File Offset: 0x00348094
		private void RefreshLanguage()
		{
			this.RenderPage();
			if (this._selected != null)
			{
				this.Select(this._selected);
			}
		}

		// Token: 0x060080FF RID: 33023 RVA: 0x00349EB0 File Offset: 0x003480B0
		private void Select(WorkshopItem item)
		{
			this._selected = item;
			this._detailContent.SetActive(item != null);
			this._detailEmpty.SetActive(item == null);
			if (item == null)
			{
				return;
			}
			this._detailTitle.text = (string.IsNullOrWhiteSpace(item.Title) ? MainMenuWorkshopPanel.T("untitled", "Untitled mod") : item.Title);
			this._description.text = (string.IsNullOrWhiteSpace(item.Description) ? MainMenuWorkshopPanel.T("no_description", "The creator hasn't added a description.") : item.Description);
			ScrollRect componentInParent = this._description.GetComponentInParent<ScrollRect>();
			if (componentInParent != null)
			{
				componentInParent.verticalNormalizedPosition = 1f;
			}
			this._operationError.text = string.Empty;
			Texture2D texture2D;
			MainMenuWorkshopPanel.ApplyPreview(this._detailPreview, this._detailPreviewPlaceholder, this._previews.TryGetValue(item.Id, out texture2D) ? texture2D : null);
			this.RefreshStatuses();
		}

		// Token: 0x06008100 RID: 33024 RVA: 0x00349FA4 File Offset: 0x003481A4
		private void RefreshStatuses()
		{
			if (!base.isActiveAndEnabled || this._subscribe == null)
			{
				return;
			}
			this._modNotice.text = (WorkshopModManager.RequiresRestart ? MainMenuWorkshopPanel.T("mod.restart", "Restart the game to apply mod changes.") : string.Empty);
			if (WorkshopModManager.HasModdedSession)
			{
				TMP_Text modNotice = this._modNotice;
				modNotice.text = modNotice.text + ((this._modNotice.text.Length > 0) ? "\n" : string.Empty) + MainMenuWorkshopPanel.T("mod.ranked", "Steam score submissions are disabled while mods are active.");
			}
			foreach (MainMenuWorkshopPanel.Card card in this._cards)
			{
				if (!(card.Root == null))
				{
					WorkshopInstallState installState = WorkshopService.GetInstallState(card.Item.Id);
					card.Status.text = MainMenuWorkshopPanel.InstallLabel(installState);
					card.Status.color = (installState.IsSubscribed ? MainMenuWorkshopPanel.Pink : MainMenuWorkshopPanel.Muted);
					MainMenuWorkshopPanel.SetTabColor(card.Button, this._selected != null && card.Item.Id == this._selected.Id);
				}
			}
			if (this._selected == null)
			{
				return;
			}
			WorkshopInstallState installState2 = WorkshopService.GetInstallState(this._selected.Id);
			this._detailStatus.text = (this._working ? MainMenuWorkshopPanel.T("status.working", "Updating subscription...") : MainMenuWorkshopPanel.InstallLabel(installState2));
			WorkshopModStatus status = WorkshopModManager.GetStatus(this._selected.Id);
			TMP_Text detailStatus = this._detailStatus;
			detailStatus.text = detailStatus.text + "\n" + GameLocalization.Get(status.StatusKey, status.StatusFallback);
			LocalizedGameText.Bind(this._enableModLabel, status.Enabled ? "workshop.mod.disable" : "workshop.mod.enable", status.Enabled ? "DISABLE MOD" : "ENABLE MOD", null).FitTranslationToRectangle(true);
			this._enableMod.interactable = !this._working && (status.Enabled || (status.Supported && installState2.IsInstalled && !installState2.IsDownloading && !installState2.IsDownloadPending && !installState2.NeedsUpdate));
			this._subscribe.interactable = !this._working && WorkshopService.IsAvailable;
			LocalizedGameText.Bind(this._subscribeLabel, installState2.IsSubscribed ? "workshop.unsubscribe" : "workshop.subscribe", installState2.IsSubscribed ? "UNSUBSCRIBE" : "SUBSCRIBE", null).FitTranslationToRectangle(true);
			this._download.interactable = !this._working && WorkshopService.IsAvailable && installState2.IsSubscribed && !installState2.IsDownloading && !installState2.IsDownloadPending && (!installState2.IsInstalled || installState2.NeedsUpdate);
			this._itemSteam.interactable = true;
		}

		// Token: 0x06008101 RID: 33025 RVA: 0x0034A2BC File Offset: 0x003484BC
		private static string InstallLabel(WorkshopInstallState state)
		{
			if (state.ErrorCode == WorkshopErrorCode.DownloadFailed)
			{
				return MainMenuWorkshopPanel.Error(state.ErrorCode);
			}
			if (state.IsDownloading)
			{
				return GameLocalization.Format("workshop.status.downloading", "Downloading {0}%", new object[] { Mathf.RoundToInt(state.DownloadProgress * 100f) });
			}
			if (state.IsDownloadPending)
			{
				return MainMenuWorkshopPanel.T("status.pending", "Download queued");
			}
			if (state.NeedsUpdate)
			{
				return MainMenuWorkshopPanel.T("status.update", "Update available");
			}
			if (state.IsInstalled)
			{
				return MainMenuWorkshopPanel.T("status.installed", "Downloaded");
			}
			if (!state.IsSubscribed)
			{
				return MainMenuWorkshopPanel.T("status.available", "Available");
			}
			return MainMenuWorkshopPanel.T("status.subscribed", "Subscribed");
		}

		// Token: 0x06008102 RID: 33026 RVA: 0x0034A38C File Offset: 0x0034858C
		private void ToggleSubscription()
		{
			if (this._selected == null || this._working)
			{
				return;
			}
			ulong id = this._selected.Id;
			bool subscribed = WorkshopService.GetInstallState(id).IsSubscribed;
			this._working = true;
			this._operationError.text = string.Empty;
			this.RefreshStatuses();
			IDisposable operation = this._operation;
			if (operation != null)
			{
				operation.Dispose();
			}
			this._operation = WorkshopService.SetSubscribed(id, !subscribed, delegate(WorkshopOperationResult result)
			{
				if (this == null || !this.isActiveAndEnabled)
				{
					return;
				}
				this._working = false;
				if (!result.Success)
				{
					this._operationError.text = MainMenuWorkshopPanel.Error(result.ErrorCode);
					this.RefreshStatuses();
					return;
				}
				string text;
				if (subscribed && WorkshopModManager.GetStatus(id).Enabled && !WorkshopModManager.SetEnabled(id, false, out text))
				{
					this._operationError.text = MainMenuWorkshopPanel.T("mod.unsubscribeFailed", "Unsubscribed, but local activation could not be saved. Try disabling the mod again.");
					Debug.LogWarning("[Workshop] Could not clear activation after unsubscribe: " + text);
					this.RefreshStatuses();
					return;
				}
				if (this._scope == WorkshopScope.Subscribed)
				{
					this.Query(this._pageIndex);
					return;
				}
				this.RefreshStatuses();
			});
		}

		// Token: 0x06008103 RID: 33027 RVA: 0x0034A435 File Offset: 0x00348635
		private void DownloadSelected()
		{
			if (this._selected == null)
			{
				return;
			}
			this._operationError.text = (WorkshopService.Download(this._selected.Id) ? string.Empty : MainMenuWorkshopPanel.Error(WorkshopErrorCode.DownloadFailed));
			this.RefreshStatuses();
		}

		// Token: 0x06008104 RID: 33028 RVA: 0x0034A470 File Offset: 0x00348670
		private void ToggleMod()
		{
			if (this._selected == null || this._working)
			{
				return;
			}
			WorkshopModStatus status = WorkshopModManager.GetStatus(this._selected.Id);
			string text;
			if (!WorkshopModManager.SetEnabled(this._selected.Id, !status.Enabled, out text))
			{
				this._operationError.text = MainMenuWorkshopPanel.T("mod.actionFailed", "We couldn't change this mod. Check its compatibility and try again.");
				Debug.LogWarning("[Workshop] " + text);
			}
			else
			{
				this._operationError.text = string.Empty;
			}
			this.RefreshStatuses();
		}

		// Token: 0x06008105 RID: 33029 RVA: 0x0034A4FE File Offset: 0x003486FE
		private IEnumerator LoadPreviews(int generation)
		{
			while (this._previewQueue.Count > 0 && generation == this._generation)
			{
				MainMenuWorkshopPanel.Card card = this._previewQueue.Dequeue();
				using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(card.Item.PreviewUrl, true))
				{
					request.timeout = 12;
					request.redirectLimit = 2;
					this._imageRequests.Add(request);
					UnityWebRequestAsyncOperation operation = request.SendWebRequest();
					while (!operation.isDone)
					{
						if (generation != this._generation || request.downloadedBytes > 8388608UL)
						{
							request.Abort();
							break;
						}
						yield return null;
					}
					this._imageRequests.Remove(request);
					if (generation != this._generation || card.Root == null)
					{
						yield break;
					}
					if (request.result != UnityWebRequest.Result.Success || request.downloadedBytes > 8388608UL)
					{
						continue;
					}
					Texture2D content = DownloadHandlerTexture.GetContent(request);
					if (content == null)
					{
						continue;
					}
					long num = (long)content.width * (long)content.height * 4L;
					if (content.width > 4096 || content.height > 4096 || this._previewBytes + num > 67108864L)
					{
						Object.Destroy(content);
						continue;
					}
					this._previewBytes += num;
					this._previews[card.Item.Id] = content;
					MainMenuWorkshopPanel.ApplyPreview(card.Image, card.Placeholder, content);
					if (this._selected != null && this._selected.Id == card.Item.Id)
					{
						MainMenuWorkshopPanel.ApplyPreview(this._detailPreview, this._detailPreviewPlaceholder, content);
					}
					operation = null;
				}
				UnityWebRequest request = null;
				card = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008106 RID: 33030 RVA: 0x0034A514 File Offset: 0x00348714
		public static bool IsAllowedPreviewUrl(string url)
		{
			Uri uri;
			if (!Uri.TryCreate(url, UriKind.Absolute, out uri) || uri.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(uri.UserInfo) || !uri.IsDefaultPort)
			{
				return false;
			}
			string dnsSafeHost = uri.DnsSafeHost;
			return dnsSafeHost.Equals("steamusercontent.com", StringComparison.OrdinalIgnoreCase) || dnsSafeHost.EndsWith(".steamusercontent.com", StringComparison.OrdinalIgnoreCase) || dnsSafeHost.Equals("steamstatic.com", StringComparison.OrdinalIgnoreCase) || dnsSafeHost.EndsWith(".steamstatic.com", StringComparison.OrdinalIgnoreCase) || dnsSafeHost.Equals("steamuserimages-a.akamaihd.net", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06008107 RID: 33031 RVA: 0x0034A5A4 File Offset: 0x003487A4
		private static void ApplyPreview(RawImage image, TMP_Text placeholder, Texture2D texture)
		{
			image.texture = texture;
			image.gameObject.SetActive(texture != null);
			placeholder.gameObject.SetActive(texture == null);
			if (texture != null)
			{
				image.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / (float)texture.height;
			}
		}

		// Token: 0x06008108 RID: 33032 RVA: 0x0034A600 File Offset: 0x00348800
		private void ClearCards()
		{
			foreach (UnityWebRequest unityWebRequest in this._imageRequests)
			{
				unityWebRequest.Abort();
			}
			base.StopAllCoroutines();
			foreach (UnityWebRequest unityWebRequest2 in this._imageRequests)
			{
				unityWebRequest2.Dispose();
			}
			this._imageRequests.Clear();
			this._previewQueue.Clear();
			if (this._detailPreview != null)
			{
				this._detailPreview.texture = null;
			}
			foreach (MainMenuWorkshopPanel.Card card in this._cards)
			{
				if (!(card.Root == null))
				{
					card.Root.SetActive(false);
					Object.Destroy(card.Root);
				}
			}
			this._cards.Clear();
			foreach (Texture2D texture2D in this._previews.Values)
			{
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
			}
			this._previews.Clear();
			this._previewBytes = 0L;
		}

		// Token: 0x06008109 RID: 33033 RVA: 0x0034A798 File Offset: 0x00348998
		private static void SetTabColor(Button button, bool selected)
		{
			ColorBlock colors = button.colors;
			colors.normalColor = (selected ? new Color(0.35f, 0.14f, 0.27f, 1f) : MainMenuWorkshopPanel.Row);
			button.colors = colors;
		}

		// Token: 0x0600810A RID: 33034 RVA: 0x0034A7DD File Offset: 0x003489DD
		private static string T(string suffix, string fallback)
		{
			return GameLocalization.Get("workshop." + suffix, fallback);
		}

		// Token: 0x0600810B RID: 33035 RVA: 0x0034A7F0 File Offset: 0x003489F0
		private static string Error(WorkshopErrorCode code)
		{
			switch (code)
			{
			case WorkshopErrorCode.SteamUnavailable:
				return MainMenuWorkshopPanel.T("error.unavailable", "Launch the Steam version of the game with Steam online to use the Workshop.");
			case WorkshopErrorCode.TimedOut:
				return MainMenuWorkshopPanel.T("error.timeout", "Steam took too long to respond. Please try again.");
			case WorkshopErrorCode.SubscriptionFailed:
				return MainMenuWorkshopPanel.T("error.subscription", "We couldn't update this subscription. Please try again.");
			case WorkshopErrorCode.DownloadFailed:
				return MainMenuWorkshopPanel.T("error.download", "The download couldn't start. Please try again.");
			case WorkshopErrorCode.Busy:
				return MainMenuWorkshopPanel.T("error.busy", "Steam is processing another request. Please try again shortly.");
			}
			return MainMenuWorkshopPanel.T("error.query", "We couldn't load the Workshop. Please try again.");
		}

		// Token: 0x0600810C RID: 33036 RVA: 0x0034A880 File Offset: 0x00348A80
		public static MainMenuWorkshopPanel Build(Transform parent, TMP_FontAsset font, Action close)
		{
			RectTransform rectTransform = MainMenuWorkshopPanel.Rect("WorkshopPanel", parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			rectTransform.gameObject.SetActive(false);
			MainMenuWorkshopPanel.Fill(rectTransform, MainMenuWorkshopPanel.Overlay, 0f);
			rectTransform.gameObject.AddComponent<CanvasGroup>();
			MainMenuWorkshopPanel mainMenuWorkshopPanel = rectTransform.gameObject.AddComponent<MainMenuWorkshopPanel>();
			mainMenuWorkshopPanel._font = font;
			mainMenuWorkshopPanel._close = close;
			mainMenuWorkshopPanel.BuildLayout();
			rectTransform.gameObject.AddComponent<MainMenuPanelIntroAnimator>().Configure(Array.Empty<RectTransform>(), null);
			return mainMenuWorkshopPanel;
		}

		// Token: 0x0600810D RID: 33037 RVA: 0x0034A90C File Offset: 0x00348B0C
		private void BuildLayout()
		{
			this.Text("Kicker", base.transform, "workshop.kicker", "COMMUNITY CREATIONS", 18f, MainMenuWorkshopPanel.Pink, new Vector2(0f, 1f), new Vector2(0.7f, 1f), new Vector2(64f, -68f), new Vector2(-24f, -34f), false);
			this.Text("Title", base.transform, "workshop.title", "WORKSHOP", 64f, MainMenuWorkshopPanel.White, new Vector2(0f, 1f), new Vector2(0.7f, 1f), new Vector2(60f, -142f), new Vector2(-24f, -70f), true);
			this.Text("Subtitle", base.transform, "workshop.subtitle", "Explore mods from the community.", 22f, MainMenuWorkshopPanel.Muted, new Vector2(0f, 1f), new Vector2(0.7f, 1f), new Vector2(64f, -180f), new Vector2(-24f, -143f), false);
			this.ActionButton("Back", base.transform, "workshop.back", "BACK", Vector2.one, Vector2.one, new Vector2(-226f, -101f), new Vector2(-64f, -47f)).onClick.AddListener(delegate
			{
				Action close = this._close;
				if (close == null)
				{
					return;
				}
				close();
			});
			RectTransform rectTransform = MainMenuWorkshopPanel.Rect("Toolbar", base.transform, new Vector2(0f, 1f), Vector2.one, new Vector2(64f, -248f), new Vector2(-64f, -194f));
			this._browse = this.ActionButton("Browse", rectTransform, "workshop.browse", "BROWSE", Vector2.zero, new Vector2(0.16f, 1f), Vector2.zero, new Vector2(-8f, 0f));
			this._subscribed = this.ActionButton("Subscribed", rectTransform, "workshop.subscribed", "SUBSCRIBED", new Vector2(0.16f, 0f), new Vector2(0.35f, 1f), Vector2.zero, new Vector2(-8f, 0f));
			this._browse.onClick.AddListener(delegate
			{
				this.ChangeScope(WorkshopScope.Available);
			});
			this._subscribed.onClick.AddListener(delegate
			{
				this.ChangeScope(WorkshopScope.Subscribed);
			});
			RectTransform rectTransform2 = MainMenuWorkshopPanel.Rect("SearchControls", rectTransform, new Vector2(0.37f, 0f), new Vector2(0.84f, 1f), Vector2.zero, new Vector2(-10f, 0f));
			this._searchRoot = rectTransform2.gameObject;
			RectTransform rectTransform3 = MainMenuWorkshopPanel.Rect("SearchInput", rectTransform2, Vector2.zero, new Vector2(0.71f, 1f), Vector2.zero, new Vector2(-8f, 0f));
			MainMenuWorkshopPanel.Fill(rectTransform3, MainMenuWorkshopPanel.Surface, 12f);
			RectTransform rectTransform4 = MainMenuWorkshopPanel.Rect("Viewport", rectTransform3, Vector2.zero, Vector2.one, new Vector2(18f, 4f), new Vector2(-18f, -4f));
			rectTransform4.gameObject.AddComponent<RectMask2D>();
			TMP_Text tmp_Text = this.Text("Text", rectTransform4, null, "", 20f, MainMenuWorkshopPanel.White, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, false);
			tmp_Text.richText = false;
			tmp_Text.enableWordWrapping = false;
			TMP_Text tmp_Text2 = this.Text("Placeholder", rectTransform4, "workshop.search.placeholder", "Search mods...", 20f, MainMenuWorkshopPanel.Muted, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, false);
			this._search = rectTransform3.gameObject.AddComponent<TMP_InputField>();
			this._search.textViewport = rectTransform4;
			this._search.textComponent = tmp_Text;
			this._search.placeholder = tmp_Text2;
			this._search.characterLimit = 128;
			this._search.lineType = TMP_InputField.LineType.SingleLine;
			this._search.selectionColor = new Color(1f, 0.31f, 0.61f, 0.35f);
			this._search.onSubmit.AddListener(delegate(string _)
			{
				this.Query(0);
			});
			rectTransform3.gameObject.AddComponent<MainMenuUiSoundRelay>();
			this.ActionButton("Search", rectTransform2, "workshop.search", "SEARCH", new Vector2(0.71f, 0f), Vector2.one, Vector2.zero, Vector2.zero).onClick.AddListener(delegate
			{
				this.Query(0);
			});
			this.ActionButton("Refresh", rectTransform, "workshop.refresh", "REFRESH", new Vector2(0.84f, 0f), Vector2.one, Vector2.zero, Vector2.zero).onClick.AddListener(delegate
			{
				this.Query(this._pageIndex);
			});
			RectTransform rectTransform5 = MainMenuWorkshopPanel.Rect("Gallery", base.transform, Vector2.zero, new Vector2(0.66f, 1f), new Vector2(64f, 98f), new Vector2(-16f, -272f));
			this._viewport = MainMenuWorkshopPanel.Rect("Viewport", rectTransform5, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-12f, 0f));
			this._viewport.gameObject.AddComponent<RectMask2D>();
			this._gridRoot = MainMenuWorkshopPanel.Rect("Cards", this._viewport, new Vector2(0f, 1f), Vector2.one, Vector2.zero, Vector2.zero);
			this._gridRoot.pivot = new Vector2(0.5f, 1f);
			this._grid = this._gridRoot.gameObject.AddComponent<GridLayoutGroup>();
			this._grid.spacing = new Vector2(16f, 16f);
			this._grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			this._grid.constraintCount = 3;
			this._grid.cellSize = new Vector2(300f, 256f);
			this._gridRoot.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._scroll = rectTransform5.gameObject.AddComponent<ScrollRect>();
			this._scroll.viewport = this._viewport;
			this._scroll.content = this._gridRoot;
			this._scroll.horizontal = false;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			this._scroll.scrollSensitivity = 45f;
			this._scroll.verticalScrollbar = MainMenuWorkshopPanel.Scrollbar(rectTransform5);
			this._scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
			RectTransform rectTransform6 = MainMenuWorkshopPanel.Rect("EmptyState", rectTransform5, Vector2.zero, Vector2.one, new Vector2(25f, 30f), new Vector2(-25f, -30f));
			this._emptyRoot = rectTransform6.gameObject;
			this._emptyText = this.Text("Message", rectTransform6, null, "", 27f, MainMenuWorkshopPanel.White, new Vector2(0.06f, 0.4f), new Vector2(0.94f, 0.76f), Vector2.zero, Vector2.zero, false);
			this._emptyText.alignment = TextAlignmentOptions.Center;
			this._retry = this.ActionButton("Retry", rectTransform6, "workshop.retry", "TRY AGAIN", new Vector2(0.3f, 0.24f), new Vector2(0.7f, 0.24f), new Vector2(0f, -27f), new Vector2(0f, 27f));
			this._retry.onClick.AddListener(delegate
			{
				this.Query(this._pageIndex);
			});
			RectTransform rectTransform7 = MainMenuWorkshopPanel.Rect("Detail", base.transform, new Vector2(0.66f, 0f), Vector2.one, new Vector2(10f, 98f), new Vector2(-64f, -272f));
			MainMenuWorkshopPanel.Fill(rectTransform7, MainMenuWorkshopPanel.Surface, 20f);
			TMP_Text tmp_Text3 = this.Text("SelectMod", rectTransform7, "workshop.select", "Select a mod to see its details.", 25f, MainMenuWorkshopPanel.Muted, new Vector2(0.08f, 0.3f), new Vector2(0.92f, 0.7f), Vector2.zero, Vector2.zero, false);
			tmp_Text3.alignment = TextAlignmentOptions.Center;
			this._detailEmpty = tmp_Text3.gameObject;
			this._detailContent = MainMenuWorkshopPanel.Rect("Content", rectTransform7, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero).gameObject;
			RectTransform rectTransform8 = MainMenuWorkshopPanel.Rect("PreviewBox", this._detailContent.transform, new Vector2(0f, 1f), Vector2.one, new Vector2(20f, -190f), new Vector2(-20f, -20f));
			MainMenuWorkshopPanel.Fill(rectTransform8, MainMenuWorkshopPanel.Row, 12f);
			this._detailPreviewPlaceholder = this.Text("NoPreview", rectTransform8, "workshop.no_preview", "No preview available", 18f, MainMenuWorkshopPanel.Muted, Vector2.zero, Vector2.one, new Vector2(10f, 10f), new Vector2(-10f, -10f), false);
			this._detailPreviewPlaceholder.alignment = TextAlignmentOptions.Center;
			this._detailPreview = MainMenuWorkshopPanel.Preview(rectTransform8);
			this._detailTitle = this.Text("ModTitle", this._detailContent.transform, null, "", 28f, MainMenuWorkshopPanel.White, new Vector2(0f, 1f), Vector2.one, new Vector2(24f, -246f), new Vector2(-24f, -202f), true);
			this._detailTitle.richText = false;
			this._detailTitle.overflowMode = TextOverflowModes.Ellipsis;
			this._detailStatus = this.Text("Status", this._detailContent.transform, null, "", 18f, MainMenuWorkshopPanel.Pink, new Vector2(0f, 1f), Vector2.one, new Vector2(24f, -302f), new Vector2(-24f, -250f), false);
			RectTransform rectTransform9 = MainMenuWorkshopPanel.Rect("DescriptionScroll", this._detailContent.transform, Vector2.zero, Vector2.one, new Vector2(24f, 282f), new Vector2(-24f, -310f));
			RectTransform rectTransform10 = MainMenuWorkshopPanel.Rect("Viewport", rectTransform9, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			MainMenuWorkshopPanel.Fill(rectTransform10, Color.clear, 0f);
			rectTransform10.gameObject.AddComponent<RectMask2D>();
			this._description = this.Text("Description", rectTransform10, null, "", 20f, MainMenuWorkshopPanel.Muted, new Vector2(0f, 1f), Vector2.one, Vector2.zero, Vector2.zero, false);
			this._description.rectTransform.pivot = new Vector2(0.5f, 1f);
			this._description.richText = false;
			this._description.alignment = TextAlignmentOptions.TopLeft;
			this._description.GetComponent<LocalizedGameText>().SetSourceLayout(false, true, 20f, 20f);
			this._description.overflowMode = TextOverflowModes.Overflow;
			this._description.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			ScrollRect scrollRect = rectTransform9.gameObject.AddComponent<ScrollRect>();
			scrollRect.viewport = rectTransform10;
			scrollRect.content = this._description.rectTransform;
			scrollRect.horizontal = false;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.scrollSensitivity = 30f;
			this._operationError = this.Text("OperationError", this._detailContent.transform, null, "", 17f, MainMenuWorkshopPanel.Pink, Vector2.zero, Vector2.right, new Vector2(24f, 186f), new Vector2(-24f, 227f), false);
			this.Text("TrustNotice", this._detailContent.transform, "workshop.mod.trust", "Mods can run code on your computer. Enable only mods you trust.", 16f, MainMenuWorkshopPanel.Muted, Vector2.zero, Vector2.right, new Vector2(24f, 232f), new Vector2(-24f, 276f), false);
			this._enableMod = this.ActionButton("EnableMod", this._detailContent.transform, "workshop.mod.enable", "ENABLE MOD", Vector2.zero, Vector2.right, new Vector2(24f, 140f), new Vector2(-24f, 182f));
			this._enableModLabel = this._enableMod.GetComponentInChildren<TMP_Text>();
			this._enableMod.onClick.AddListener(new UnityAction(this.ToggleMod));
			this._subscribe = this.ActionButton("Subscribe", this._detailContent.transform, "workshop.subscribe", "SUBSCRIBE", Vector2.zero, new Vector2(0.64f, 0f), new Vector2(24f, 78f), new Vector2(-6f, 132f));
			this._subscribeLabel = this._subscribe.GetComponentInChildren<TMP_Text>();
			this._subscribe.onClick.AddListener(new UnityAction(this.ToggleSubscription));
			this._download = this.ActionButton("Download", this._detailContent.transform, "workshop.download", "DOWNLOAD", new Vector2(0.64f, 0f), Vector2.right, new Vector2(6f, 78f), new Vector2(-24f, 132f));
			this._download.onClick.AddListener(new UnityAction(this.DownloadSelected));
			this._itemSteam = this.ActionButton("OpenItem", this._detailContent.transform, "workshop.open", "OPEN IN STEAM", Vector2.zero, Vector2.right, new Vector2(24f, 20f), new Vector2(-24f, 66f));
			this._itemSteam.onClick.AddListener(delegate
			{
				if (this._selected != null)
				{
					WorkshopService.OpenItem(this._selected.Id);
				}
			});
			this._previous = this.ActionButton("Previous", base.transform, "workshop.previous", "PREVIOUS", Vector2.zero, Vector2.zero, new Vector2(64f, 30f), new Vector2(238f, 76f));
			this._previous.onClick.AddListener(delegate
			{
				this.Query(this._pageIndex - 1);
			});
			this._pageLabel = this.Text("Page", base.transform, null, "", 19f, MainMenuWorkshopPanel.Muted, Vector2.zero, Vector2.zero, new Vector2(250f, 30f), new Vector2(440f, 76f), false);
			this._pageLabel.alignment = TextAlignmentOptions.Center;
			this._next = this.ActionButton("Next", base.transform, "workshop.next", "NEXT", Vector2.zero, Vector2.zero, new Vector2(452f, 30f), new Vector2(626f, 76f));
			this._next.onClick.AddListener(delegate
			{
				this.Query(this._pageIndex + 1);
			});
			this._modNotice = this.Text("ModNotice", base.transform, null, "", 16f, MainMenuWorkshopPanel.Muted, Vector2.zero, Vector2.right, new Vector2(650f, 24f), new Vector2(-405f, 80f), false);
			this.ActionButton("OpenWorkshop", base.transform, "workshop.open", "OPEN IN STEAM", Vector2.right, Vector2.right, new Vector2(-380f, 30f), new Vector2(-64f, 76f)).onClick.AddListener(new UnityAction(WorkshopService.OpenWorkshop));
			ControllerNavigationScope.Configure(base.gameObject, delegate
			{
				Action close2 = this._close;
				if (close2 == null)
				{
					return;
				}
				close2();
			}, this._browse.gameObject, 100, false);
			this._detailContent.SetActive(false);
		}

		// Token: 0x0600810E RID: 33038 RVA: 0x0034B988 File Offset: 0x00349B88
		private MainMenuWorkshopPanel.Card BuildCard(WorkshopItem item)
		{
			RectTransform rectTransform = MainMenuWorkshopPanel.Rect("Mod_" + item.Id.ToString(), this._gridRoot, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image = MainMenuWorkshopPanel.Fill(rectTransform, MainMenuWorkshopPanel.Row, 14f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			MainMenuWorkshopPanel.StyleButton(button, image);
			button.onClick.AddListener(delegate
			{
				this.Select(item);
			});
			rectTransform.gameObject.AddComponent<MainMenuUiSoundRelay>();
			RectTransform rectTransform2 = MainMenuWorkshopPanel.Rect("PreviewBox", rectTransform, Vector2.zero, Vector2.one, new Vector2(8f, 80f), new Vector2(-8f, -8f));
			MainMenuWorkshopPanel.Fill(rectTransform2, MainMenuWorkshopPanel.Surface, 8f);
			TMP_Text tmp_Text = this.Text("NoPreview", rectTransform2, "workshop.no_preview", "No preview available", 16f, MainMenuWorkshopPanel.Muted, Vector2.zero, Vector2.one, new Vector2(8f, 8f), new Vector2(-8f, -8f), false);
			tmp_Text.alignment = TextAlignmentOptions.Center;
			RawImage rawImage = MainMenuWorkshopPanel.Preview(rectTransform2);
			TMP_Text tmp_Text2 = this.Text("Title", rectTransform, null, string.IsNullOrWhiteSpace(item.Title) ? GameLocalization.Get("workshop.untitled", "Untitled mod") : item.Title, 22f, MainMenuWorkshopPanel.White, Vector2.zero, Vector2.right, new Vector2(16f, 36f), new Vector2(-16f, 74f), true);
			tmp_Text2.richText = false;
			tmp_Text2.enableWordWrapping = false;
			tmp_Text2.overflowMode = TextOverflowModes.Ellipsis;
			TMP_Text tmp_Text3 = this.Text("Status", rectTransform, null, "", 16f, MainMenuWorkshopPanel.Muted, Vector2.zero, Vector2.right, new Vector2(16f, 9f), new Vector2(-16f, 34f), false);
			return new MainMenuWorkshopPanel.Card
			{
				Root = rectTransform.gameObject,
				Item = item,
				Button = button,
				Image = rawImage,
				Placeholder = tmp_Text,
				Status = tmp_Text3
			};
		}

		// Token: 0x0600810F RID: 33039 RVA: 0x0034BBDC File Offset: 0x00349DDC
		private static RawImage Preview(Transform parent)
		{
			RectTransform rectTransform = MainMenuWorkshopPanel.Rect("Preview", parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-50f, -50f), new Vector2(50f, 50f));
			RawImage rawImage = rectTransform.gameObject.AddComponent<RawImage>();
			rawImage.raycastTarget = false;
			AspectRatioFitter aspectRatioFitter = rectTransform.gameObject.AddComponent<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
			aspectRatioFitter.aspectRatio = 1.7777778f;
			rawImage.gameObject.SetActive(false);
			return rawImage;
		}

		// Token: 0x06008110 RID: 33040 RVA: 0x0034BC6C File Offset: 0x00349E6C
		private Button ActionButton(string name, Transform parent, string key, string label, Vector2 min, Vector2 max, Vector2 lo, Vector2 hi)
		{
			RectTransform rectTransform = MainMenuWorkshopPanel.Rect(name, parent, min, max, lo, hi);
			Image image = MainMenuWorkshopPanel.Fill(rectTransform, MainMenuWorkshopPanel.Row, 12f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			MainMenuWorkshopPanel.StyleButton(button, image);
			this.Text("Label", rectTransform, key, label, 20f, MainMenuWorkshopPanel.White, Vector2.zero, Vector2.one, new Vector2(12f, 3f), new Vector2(-12f, -3f), true).alignment = TextAlignmentOptions.Center;
			rectTransform.gameObject.AddComponent<MainMenuUiSoundRelay>();
			return button;
		}

		// Token: 0x06008111 RID: 33041 RVA: 0x0034BD04 File Offset: 0x00349F04
		private static void StyleButton(Button button, Image image)
		{
			image.color = Color.white;
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.normalColor = MainMenuWorkshopPanel.Row;
			colors.highlightedColor = new Color(0.27f, 0.17f, 0.31f, 1f);
			colors.selectedColor = colors.highlightedColor;
			colors.pressedColor = new Color(0.39f, 0.2f, 0.32f, 1f);
			colors.disabledColor = new Color(0.1f, 0.075f, 0.12f, 0.75f);
			colors.fadeDuration = 0.1f;
			button.colors = colors;
		}

		// Token: 0x06008112 RID: 33042 RVA: 0x0034BDB8 File Offset: 0x00349FB8
		private TMP_Text Text(string name, Transform parent, string key, string value, float size, Color color, Vector2 min, Vector2 max, Vector2 lo, Vector2 hi, bool bold = false)
		{
			TextMeshProUGUI textMeshProUGUI = MainMenuWorkshopPanel.Rect(name, parent, min, max, lo, hi).gameObject.AddComponent<TextMeshProUGUI>();
			if (this._font != null)
			{
				textMeshProUGUI.font = this._font;
			}
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontSizeMax = size;
			textMeshProUGUI.fontSizeMin = size * 0.72f;
			textMeshProUGUI.enableAutoSizing = true;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.fontStyle = (bold ? FontStyles.Bold : FontStyles.Normal);
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.raycastTarget = false;
			if (key != null)
			{
				LocalizedGameText.Bind(textMeshProUGUI, key, value, null).FitTranslationToRectangle(false);
			}
			else
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			return textMeshProUGUI;
		}

		// Token: 0x06008113 RID: 33043 RVA: 0x0034BE74 File Offset: 0x0034A074
		private static RectTransform Rect(string name, Transform parent, Vector2 min, Vector2 max, Vector2 lo, Vector2 hi)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.gameObject.layer = parent.gameObject.layer;
			component.anchorMin = min;
			component.anchorMax = max;
			component.offsetMin = lo;
			component.offsetMax = hi;
			return component;
		}

		// Token: 0x06008114 RID: 33044 RVA: 0x0034BEDB File Offset: 0x0034A0DB
		private static Image Fill(RectTransform rect, Color color, float radius)
		{
			Image image = rect.gameObject.AddComponent<Image>();
			image.color = color;
			if (radius > 0f)
			{
				rect.gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
			}
			return image;
		}

		// Token: 0x06008115 RID: 33045 RVA: 0x0034BF10 File Offset: 0x0034A110
		private static Scrollbar Scrollbar(Transform parent)
		{
			RectTransform rectTransform = MainMenuWorkshopPanel.Rect("Scrollbar", parent, Vector2.right, Vector2.one, new Vector2(-5f, 0f), Vector2.zero);
			MainMenuWorkshopPanel.Fill(rectTransform, MainMenuWorkshopPanel.Surface, 2f);
			RectTransform rectTransform2 = MainMenuWorkshopPanel.Rect("Handle", rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			Image image = MainMenuWorkshopPanel.Fill(rectTransform2, MainMenuWorkshopPanel.Muted, 2f);
			Scrollbar scrollbar = rectTransform.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform2;
			scrollbar.targetGraphic = image;
			scrollbar.direction = global::UnityEngine.UI.Scrollbar.Direction.BottomToTop;
			scrollbar.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			return scrollbar;
		}

		// Token: 0x04008439 RID: 33849
		private readonly List<MainMenuWorkshopPanel.Card> _cards = new List<MainMenuWorkshopPanel.Card>();

		// Token: 0x0400843A RID: 33850
		private readonly Dictionary<ulong, Texture2D> _previews = new Dictionary<ulong, Texture2D>();

		// Token: 0x0400843B RID: 33851
		private readonly List<UnityWebRequest> _imageRequests = new List<UnityWebRequest>();

		// Token: 0x0400843C RID: 33852
		private readonly Queue<MainMenuWorkshopPanel.Card> _previewQueue = new Queue<MainMenuWorkshopPanel.Card>();

		// Token: 0x0400843D RID: 33853
		private IDisposable _queryRequest;

		// Token: 0x0400843E RID: 33854
		private IDisposable _operation;

		// Token: 0x0400843F RID: 33855
		private WorkshopScope _scope;

		// Token: 0x04008440 RID: 33856
		private WorkshopPage _page;

		// Token: 0x04008441 RID: 33857
		private WorkshopItem _selected;

		// Token: 0x04008442 RID: 33858
		private int _pageIndex;

		// Token: 0x04008443 RID: 33859
		private int _generation;

		// Token: 0x04008444 RID: 33860
		private bool _working;

		// Token: 0x04008445 RID: 33861
		private float _nextStatusRefresh;

		// Token: 0x04008446 RID: 33862
		private float _lastGridWidth;

		// Token: 0x04008447 RID: 33863
		private long _previewBytes;

		// Token: 0x04008448 RID: 33864
		private static readonly Color Overlay = new Color(0.025f, 0.012f, 0.03f, 0.98f);

		// Token: 0x04008449 RID: 33865
		private static readonly Color Surface = new Color(0.075f, 0.035f, 0.075f, 1f);

		// Token: 0x0400844A RID: 33866
		private static readonly Color Row = new Color(0.165f, 0.118f, 0.2f, 1f);

		// Token: 0x0400844B RID: 33867
		private static readonly Color Pink = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x0400844C RID: 33868
		private static readonly Color White = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x0400844D RID: 33869
		private static readonly Color Muted = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x0400844E RID: 33870
		private TMP_FontAsset _font;

		// Token: 0x0400844F RID: 33871
		private Action _close;

		// Token: 0x04008450 RID: 33872
		private Button _browse;

		// Token: 0x04008451 RID: 33873
		private Button _subscribed;

		// Token: 0x04008452 RID: 33874
		private Button _previous;

		// Token: 0x04008453 RID: 33875
		private Button _next;

		// Token: 0x04008454 RID: 33876
		private Button _retry;

		// Token: 0x04008455 RID: 33877
		private Button _subscribe;

		// Token: 0x04008456 RID: 33878
		private Button _download;

		// Token: 0x04008457 RID: 33879
		private Button _itemSteam;

		// Token: 0x04008458 RID: 33880
		private Button _enableMod;

		// Token: 0x04008459 RID: 33881
		private TMP_InputField _search;

		// Token: 0x0400845A RID: 33882
		private GameObject _searchRoot;

		// Token: 0x0400845B RID: 33883
		private GameObject _emptyRoot;

		// Token: 0x0400845C RID: 33884
		private GameObject _detailContent;

		// Token: 0x0400845D RID: 33885
		private GameObject _detailEmpty;

		// Token: 0x0400845E RID: 33886
		private TMP_Text _emptyText;

		// Token: 0x0400845F RID: 33887
		private TMP_Text _pageLabel;

		// Token: 0x04008460 RID: 33888
		private TMP_Text _detailTitle;

		// Token: 0x04008461 RID: 33889
		private TMP_Text _description;

		// Token: 0x04008462 RID: 33890
		private TMP_Text _detailStatus;

		// Token: 0x04008463 RID: 33891
		private TMP_Text _operationError;

		// Token: 0x04008464 RID: 33892
		private TMP_Text _subscribeLabel;

		// Token: 0x04008465 RID: 33893
		private TMP_Text _detailPreviewPlaceholder;

		// Token: 0x04008466 RID: 33894
		private TMP_Text _enableModLabel;

		// Token: 0x04008467 RID: 33895
		private TMP_Text _modNotice;

		// Token: 0x04008468 RID: 33896
		private RawImage _detailPreview;

		// Token: 0x04008469 RID: 33897
		private RectTransform _gridRoot;

		// Token: 0x0400846A RID: 33898
		private RectTransform _viewport;

		// Token: 0x0400846B RID: 33899
		private GridLayoutGroup _grid;

		// Token: 0x0400846C RID: 33900
		private ScrollRect _scroll;

		// Token: 0x020010A5 RID: 4261
		private sealed class Card
		{
			// Token: 0x0400846D RID: 33901
			public GameObject Root;

			// Token: 0x0400846E RID: 33902
			public WorkshopItem Item;

			// Token: 0x0400846F RID: 33903
			public Button Button;

			// Token: 0x04008470 RID: 33904
			public RawImage Image;

			// Token: 0x04008471 RID: 33905
			public TMP_Text Placeholder;

			// Token: 0x04008472 RID: 33906
			public TMP_Text Status;
		}
	}
}
