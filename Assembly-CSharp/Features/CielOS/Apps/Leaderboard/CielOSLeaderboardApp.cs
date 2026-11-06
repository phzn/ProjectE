using System;
using System.Collections.Generic;
using System.Globalization;
using Features.Localization;
using Features.RunModes;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Leaderboard
{
	// Token: 0x02001486 RID: 5254
	public sealed class CielOSLeaderboardApp : MonoBehaviour
	{
		// Token: 0x0600A1E7 RID: 41447 RVA: 0x00433D08 File Offset: 0x00431F08
		private void Awake()
		{
			foreach (TMP_Text tmp_Text in base.GetComponentsInChildren<TMP_Text>(true))
			{
				LocalizedGameText.BindSource(tmp_Text, "leaderboard_ui", tmp_Text.text, null).FitTranslationToRectangle(false);
			}
			if (this.rowTemplate != null)
			{
				this.rowTemplate.SetActive(false);
			}
			CielOSLeaderboardApp.Bind(this.globalButton, new UnityAction(this.ShowGlobal));
			CielOSLeaderboardApp.Bind(this.friendsButton, new UnityAction(this.ShowFriends));
			CielOSLeaderboardApp.Bind(this.refreshButton, new UnityAction(this.Refresh));
			CielOSLeaderboardApp.Bind(this.previousButton, new UnityAction(this.PreviousPage));
			CielOSLeaderboardApp.Bind(this.nextButton, new UnityAction(this.NextPage));
			CielOSLeaderboardApp.Bind(this.resetRecordButton, new UnityAction(this.ShowResetConfirmation));
			CielOSLeaderboardApp.Bind(this.cancelResetButton, new UnityAction(this.CancelReset));
			CielOSLeaderboardApp.Bind(this.confirmResetButton, new UnityAction(this.ConfirmReset));
		}

		// Token: 0x0600A1E8 RID: 41448 RVA: 0x00433E19 File Offset: 0x00432019
		private void OnEnable()
		{
			RankedLeaderboardService.AvailabilityChanged += this.Refresh;
			RankedLeaderboardService.EntriesChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x0600A1E9 RID: 41449 RVA: 0x00433E43 File Offset: 0x00432043
		private void OnDisable()
		{
			RankedLeaderboardService.AvailabilityChanged -= this.Refresh;
			RankedLeaderboardService.EntriesChanged -= this.Refresh;
			this.CancelRequest();
			this.HideResetConfirmation(false);
		}

		// Token: 0x0600A1EA RID: 41450 RVA: 0x00433E74 File Offset: 0x00432074
		private static void Bind(Button button, UnityAction action)
		{
			if (button == null)
			{
				return;
			}
			button.onClick.AddListener(action);
			CielOSSfx.Attach(button);
		}

		// Token: 0x0600A1EB RID: 41451 RVA: 0x00433E92 File Offset: 0x00432092
		private void ShowGlobal()
		{
			this.SelectScope(RankedLeaderboardScope.Global);
		}

		// Token: 0x0600A1EC RID: 41452 RVA: 0x00433E9B File Offset: 0x0043209B
		private void ShowFriends()
		{
			this.SelectScope(RankedLeaderboardScope.Friends);
		}

		// Token: 0x0600A1ED RID: 41453 RVA: 0x00433EA4 File Offset: 0x004320A4
		private void SelectScope(RankedLeaderboardScope scope)
		{
			if (this._scope == scope)
			{
				return;
			}
			this._scope = scope;
			this._page = 0;
			this.Refresh();
		}

		// Token: 0x0600A1EE RID: 41454 RVA: 0x00433EC4 File Offset: 0x004320C4
		private void PreviousPage()
		{
			if (this._result == null || !this._result.HasPreviousPage)
			{
				return;
			}
			this._page = Mathf.Max(0, this._page - 1);
			this.Refresh();
		}

		// Token: 0x0600A1EF RID: 41455 RVA: 0x00433EF6 File Offset: 0x004320F6
		private void NextPage()
		{
			if (this._result == null || !this._result.HasNextPage)
			{
				return;
			}
			this._page++;
			this.Refresh();
		}

		// Token: 0x0600A1F0 RID: 41456 RVA: 0x00433F22 File Offset: 0x00432122
		private void CancelRequest()
		{
			this._requestVersion++;
			IDisposable request = this._request;
			this._request = null;
			if (request == null)
			{
				return;
			}
			request.Dispose();
		}

		// Token: 0x0600A1F1 RID: 41457 RVA: 0x00433F4C File Offset: 0x0043214C
		public void Refresh()
		{
			if (this == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this.CancelRequest();
			this.UpdateSelection();
			int version = this._requestVersion;
			bool completed = false;
			IDisposable disposable = RankedLeaderboardService.RequestEntries(this._scope, this._page, this.pageSize, delegate(RankedLeaderboardResult result)
			{
				if (this == null || !this.isActiveAndEnabled || version != this._requestVersion)
				{
					return;
				}
				completed = result.State > RankedLeaderboardState.Loading;
				this.RenderResult(result);
			});
			if ((version != this._requestVersion || !base.isActiveAndEnabled) | completed)
			{
				if (disposable != null)
				{
					disposable.Dispose();
					return;
				}
			}
			else
			{
				this._request = disposable;
			}
		}

		// Token: 0x0600A1F2 RID: 41458 RVA: 0x00433FF0 File Offset: 0x004321F0
		private void UpdateSelection()
		{
			if (this.globalSelection != null)
			{
				this.globalSelection.SetActive(this._scope == RankedLeaderboardScope.Global);
			}
			if (this.friendsSelection != null)
			{
				this.friendsSelection.SetActive(this._scope == RankedLeaderboardScope.Friends);
			}
		}

		// Token: 0x0600A1F3 RID: 41459 RVA: 0x00434044 File Offset: 0x00432244
		private void RenderResult(RankedLeaderboardResult result)
		{
			this._result = result;
			this._scope = result.Scope;
			this._page = result.Page;
			this.UpdateSelection();
			RankedLeaderboardState rankedLeaderboardState = ((result.State == RankedLeaderboardState.Ready && result.Entries.Count == 0) ? RankedLeaderboardState.Empty : result.State);
			bool flag = rankedLeaderboardState == RankedLeaderboardState.Ready;
			bool flag2 = rankedLeaderboardState == RankedLeaderboardState.Empty;
			bool flag3 = rankedLeaderboardState == RankedLeaderboardState.Loading;
			this.ClearRows();
			if (flag && result.Entries != null)
			{
				for (int i = 0; i < result.Entries.Count; i++)
				{
					this.AddRow(result.Entries[i], i);
				}
			}
			if (this.entriesScroll != null)
			{
				this.entriesScroll.StopMovement();
				this.entriesScroll.verticalNormalizedPosition = 1f;
				if (this.entriesScroll.verticalScrollbar != null)
				{
					this.entriesScroll.verticalScrollbar.gameObject.SetActive(flag);
				}
			}
			if (this.statePanel != null)
			{
				this.statePanel.SetActive(!flag);
			}
			CielOSLeaderboardApp.SetInteractable(this.previousButton, (flag || flag2) && result.HasPreviousPage);
			CielOSLeaderboardApp.SetInteractable(this.nextButton, flag && result.HasNextPage);
			CielOSLeaderboardApp.SetInteractable(this.refreshButton, !flag3);
			CielOSLeaderboardApp.SetText(this.pageLabel, (flag || (flag2 && result.Page > 0)) ? CielOSLeaderboardApp.GetPageCaption(result) : string.Empty);
			string text = ((result.Scope == RankedLeaderboardScope.Friends) ? ((result.TotalEntries == 1) ? "{0} FRIEND" : "{0} FRIENDS") : ((result.TotalEntries == 1) ? "{0} PLAYER" : "{0} PLAYERS"));
			if ((flag || flag2) && result.TotalEntries >= 0)
			{
				LocalizedGameText.BindFormat(this.entryCountLabel, "leaderboard_ui", text, new object[] { CielOSLeaderboardApp.FormatNumber(result.TotalEntries) });
			}
			else
			{
				CielOSLeaderboardApp.SetText(this.entryCountLabel, string.Empty);
			}
			this.UpdatePersonalBest(result, flag || flag2);
			this.UpdateResetControls();
			switch (rankedLeaderboardState)
			{
			case RankedLeaderboardState.Loading:
				this.SetState("Loading...", string.Empty);
				return;
			case RankedLeaderboardState.Ready:
				break;
			case RankedLeaderboardState.Empty:
				if (result.Page > 0)
				{
					this.SetState("No records on this page", string.Empty);
					return;
				}
				if (result.Scope == RankedLeaderboardScope.AroundPlayer)
				{
					this.SetState("Unranked", "No Classic Ranked record yet.");
					return;
				}
				if (result.Scope == RankedLeaderboardScope.Friends)
				{
					this.SetState("No friends ranked yet", string.Empty);
					return;
				}
				this.SetState("No records yet", string.Empty);
				return;
			case RankedLeaderboardState.Unavailable:
				this.SetState("Steam offline", "Records are unavailable.");
				return;
			case RankedLeaderboardState.Error:
				this.SetState("Could not load records", string.IsNullOrWhiteSpace(result.ErrorMessage) ? "Steam did not respond." : result.ErrorMessage);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600A1F4 RID: 41460 RVA: 0x0043430C File Offset: 0x0043250C
		private void UpdatePersonalBest(RankedLeaderboardResult result, bool loaded)
		{
			RankedLeaderboardEntry rankedLeaderboardEntry = (loaded ? result.LocalPlayerEntry : null);
			CielOSLeaderboardApp.SetText(this.personalRankLabel, (rankedLeaderboardEntry != null) ? ("#" + CielOSLeaderboardApp.FormatNumber(rankedLeaderboardEntry.GlobalRank)) : "--");
			CielOSLeaderboardApp.SetText(this.personalDistanceLabel, (rankedLeaderboardEntry != null) ? CielOSLeaderboardApp.FormatDistance(rankedLeaderboardEntry.DistanceMeters) : "--");
			CielOSLeaderboardApp.SetText(this.personalScoreLabel, CielOSLeaderboardApp.FormatScore((rankedLeaderboardEntry != null) ? rankedLeaderboardEntry.Score : null));
		}

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x0600A1F5 RID: 41461 RVA: 0x00434394 File Offset: 0x00432594
		private bool ResetBusy
		{
			get
			{
				return this._resetInFlight || RankedLeaderboardService.IsResettingOwnRecord;
			}
		}

		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x0600A1F6 RID: 41462 RVA: 0x004343A8 File Offset: 0x004325A8
		private bool CanRequestReset
		{
			get
			{
				if (RankedLeaderboardService.CanResetOwnRecord && !this.ResetBusy)
				{
					RankedLeaderboardResult result = this._result;
					return ((result != null) ? result.LocalPlayerEntry : null) != null || RankedLeaderboardService.HasPendingRecordReset || (this._resetFailedAccountId != null && string.Equals(this._resetFailedAccountId, RankedLeaderboardService.CurrentAccountId, StringComparison.Ordinal));
				}
				return false;
			}
		}

		// Token: 0x0600A1F7 RID: 41463 RVA: 0x00434400 File Offset: 0x00432600
		private void ShowResetConfirmation()
		{
			if (!this.CanRequestReset || this.resetConfirmation == null)
			{
				return;
			}
			ControllerNavigationScope.Configure(this.resetConfirmation, new Action(this.CancelReset), (this.cancelResetButton != null) ? this.cancelResetButton.gameObject : null, 100, false);
			this._resetAccountId = RankedLeaderboardService.CurrentAccountId;
			this.resetConfirmation.SetActive(true);
			this.resetConfirmation.transform.SetAsLastSibling();
			this.UpdateResetControls();
			if (this.cancelResetButton != null)
			{
				this.cancelResetButton.Select();
			}
		}

		// Token: 0x0600A1F8 RID: 41464 RVA: 0x004344A1 File Offset: 0x004326A1
		private void CancelReset()
		{
			if (!this.ResetBusy)
			{
				this.HideResetConfirmation(true);
			}
		}

		// Token: 0x0600A1F9 RID: 41465 RVA: 0x004344B4 File Offset: 0x004326B4
		private void ConfirmReset()
		{
			if (this.resetConfirmation == null || !this.resetConfirmation.activeSelf || !this.CanRequestReset)
			{
				return;
			}
			if (!string.Equals(this._resetAccountId, RankedLeaderboardService.CurrentAccountId, StringComparison.Ordinal))
			{
				this.HideResetConfirmation(true);
				this.Refresh();
				return;
			}
			string accountId = this._resetAccountId;
			this._resetInFlight = true;
			this.UpdateResetControls();
			RankedLeaderboardService.ResetOwnRecord(delegate(RankedRecordResetResult result)
			{
				if (this == null)
				{
					return;
				}
				this._resetInFlight = false;
				this._resetFailedAccountId = (result.Success ? null : accountId);
				this.HideResetConfirmation(this.isActiveAndEnabled);
				if (!result.Success && Application.isPlaying)
				{
					ToastService.Show(string.IsNullOrWhiteSpace(result.ErrorMessage) ? "Steam could not reset your record." : result.ErrorMessage, "Record not reset", null, 7f, "leaderboard_ui");
				}
				if (this.isActiveAndEnabled)
				{
					this.Refresh();
				}
			});
		}

		// Token: 0x0600A1FA RID: 41466 RVA: 0x00434540 File Offset: 0x00432740
		private void HideResetConfirmation(bool restoreFocus)
		{
			if (this.resetConfirmation != null)
			{
				this.resetConfirmation.SetActive(false);
			}
			this._resetAccountId = null;
			this.UpdateResetControls();
			if (restoreFocus && this.resetRecordButton != null && this.resetRecordButton.IsInteractable())
			{
				this.resetRecordButton.Select();
			}
		}

		// Token: 0x0600A1FB RID: 41467 RVA: 0x004345A0 File Offset: 0x004327A0
		private void UpdateResetControls()
		{
			bool resetBusy = this.ResetBusy;
			bool flag = this.resetConfirmation != null && this.resetConfirmation.activeSelf;
			CielOSLeaderboardApp.SetInteractable(this.resetRecordButton, this.CanRequestReset);
			CielOSLeaderboardApp.SetInteractable(this.cancelResetButton, !resetBusy);
			CielOSLeaderboardApp.SetInteractable(this.confirmResetButton, this.CanRequestReset);
			if (this.confirmResetButton != null)
			{
				CielOSLeaderboardApp.SetText(this.confirmResetButton.GetComponentInChildren<TMP_Text>(), resetBusy ? "RESETTING..." : "RESET");
			}
			if (this.browseControls != null)
			{
				this.browseControls.interactable = !flag;
			}
		}

		// Token: 0x0600A1FC RID: 41468 RVA: 0x0043464C File Offset: 0x0043284C
		private static void SetInteractable(Button button, bool interactable)
		{
			if (button == null)
			{
				return;
			}
			button.interactable = interactable;
			TMP_Text componentInChildren = button.GetComponentInChildren<TMP_Text>();
			if (componentInChildren != null)
			{
				componentInChildren.color = (interactable ? new Color32(243, 238, byte.MaxValue, byte.MaxValue) : new Color32(133, 118, 152, byte.MaxValue));
			}
		}

		// Token: 0x0600A1FD RID: 41469 RVA: 0x004346BC File Offset: 0x004328BC
		private static string GetPageCaption(RankedLeaderboardResult result)
		{
			if (result.Scope == RankedLeaderboardScope.AroundPlayer)
			{
				return string.Empty;
			}
			if (result.TotalEntries < 0)
			{
				return (result.Page + 1).ToString(CultureInfo.InvariantCulture);
			}
			int num = Mathf.Max(1, Mathf.CeilToInt((float)result.TotalEntries / (float)Mathf.Max(1, result.PageSize)));
			if (result.Page >= num)
			{
				return (result.Page + 1).ToString(CultureInfo.InvariantCulture);
			}
			return (result.Page + 1).ToString(CultureInfo.InvariantCulture) + " / " + num.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600A1FE RID: 41470 RVA: 0x00434764 File Offset: 0x00432964
		private void AddRow(RankedLeaderboardEntry entry, int index)
		{
			if (this.rowTemplate == null || this.rowsParent == null || entry == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.rowTemplate, this.rowsParent, false);
			gameObject.name = "Entry_" + entry.GlobalRank.ToString(CultureInfo.InvariantCulture);
			gameObject.SetActive(true);
			this._rows.Add(gameObject);
			Image component = gameObject.GetComponent<Image>();
			if (component != null)
			{
				component.color = (entry.IsLocalPlayer ? this.localRowColor : ((index % 2 == 0) ? this.rowColor : this.alternateRowColor));
			}
			TMP_Text tmp_Text = CielOSLeaderboardApp.FindText(gameObject, "Position");
			TMP_Text tmp_Text2 = CielOSLeaderboardApp.FindText(gameObject, "Distance");
			TMP_Text tmp_Text3 = CielOSLeaderboardApp.FindText(gameObject, "Score");
			CielOSLeaderboardApp.SetText(tmp_Text, "#" + CielOSLeaderboardApp.FormatNumber(entry.GlobalRank));
			bool flag = entry.GlobalRank >= 1 && entry.GlobalRank <= 3;
			if (entry.IsLocalPlayer || flag)
			{
				Color color = (entry.IsLocalPlayer ? this.localTextColor : this.podiumTextColor);
				if (tmp_Text != null)
				{
					tmp_Text.color = color;
				}
				if (tmp_Text2 != null)
				{
					tmp_Text2.color = color;
				}
			}
			TMP_Text tmp_Text4 = CielOSLeaderboardApp.FindText(gameObject, "PlayerName");
			if (tmp_Text4 != null)
			{
				tmp_Text4.richText = false;
				tmp_Text4.text = CielOSLeaderboardApp.NormalizePersonaName(entry.DisplayName);
				if (string.IsNullOrWhiteSpace(entry.DisplayName))
				{
					LocalizedGameText.BindSource(tmp_Text4, "leaderboard_ui", "Steam player", null);
				}
				else
				{
					LocalizedGameText.BindFontOnly(tmp_Text4);
				}
			}
			CielOSLeaderboardApp.SetText(tmp_Text2, CielOSLeaderboardApp.FormatDistance(entry.DistanceMeters));
			CielOSLeaderboardApp.SetText(tmp_Text3, CielOSLeaderboardApp.FormatScore(entry.Score));
			CielOSLeaderboardApp.SetText(CielOSLeaderboardApp.FindText(gameObject, "You"), entry.IsLocalPlayer ? "YOU" : string.Empty);
			Transform transform = gameObject.transform.Find("LocalIndicator");
			if (transform != null)
			{
				transform.gameObject.SetActive(entry.IsLocalPlayer);
			}
		}

		// Token: 0x0600A1FF RID: 41471 RVA: 0x0043497F File Offset: 0x00432B7F
		public static string NormalizePersonaName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return "Steam player";
			}
			return value.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
		}

		// Token: 0x0600A200 RID: 41472 RVA: 0x004349AC File Offset: 0x00432BAC
		private void ClearRows()
		{
			foreach (GameObject gameObject in this._rows)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(gameObject);
					}
					else
					{
						Object.DestroyImmediate(gameObject);
					}
				}
			}
			this._rows.Clear();
		}

		// Token: 0x0600A201 RID: 41473 RVA: 0x00434A28 File Offset: 0x00432C28
		private void SetState(string title, string message)
		{
			CielOSLeaderboardApp.SetText(this.stateTitle, title);
			CielOSLeaderboardApp.SetText(this.stateMessage, message);
			if (this.stateMessage != null)
			{
				this.stateMessage.gameObject.SetActive(!string.IsNullOrWhiteSpace(message));
			}
		}

		// Token: 0x0600A202 RID: 41474 RVA: 0x00434A74 File Offset: 0x00432C74
		private static TMP_Text FindText(GameObject row, string name)
		{
			Transform transform = row.transform.Find(name);
			if (!(transform != null))
			{
				return null;
			}
			return transform.GetComponent<TMP_Text>();
		}

		// Token: 0x0600A203 RID: 41475 RVA: 0x00434A9F File Offset: 0x00432C9F
		private static void SetText(TMP_Text target, string value)
		{
			LocalizedGameText.BindSource(target, "leaderboard_ui", value, null);
		}

		// Token: 0x0600A204 RID: 41476 RVA: 0x00434AAF File Offset: 0x00432CAF
		private static string FormatNumber(int value)
		{
			return value.ToString("N0", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600A205 RID: 41477 RVA: 0x00434AC2 File Offset: 0x00432CC2
		private static string FormatDistance(int distanceMeters)
		{
			return CielOSLeaderboardApp.FormatNumber(distanceMeters) + " m";
		}

		// Token: 0x0600A206 RID: 41478 RVA: 0x00434AD4 File Offset: 0x00432CD4
		private static string FormatScore(int? score)
		{
			if (score == null)
			{
				return "--";
			}
			return CielOSLeaderboardApp.FormatNumber(score.Value);
		}

		// Token: 0x0400A584 RID: 42372
		[Header("Navigation")]
		[SerializeField]
		private Button globalButton;

		// Token: 0x0400A585 RID: 42373
		[SerializeField]
		private Button friendsButton;

		// Token: 0x0400A586 RID: 42374
		[SerializeField]
		private GameObject globalSelection;

		// Token: 0x0400A587 RID: 42375
		[SerializeField]
		private GameObject friendsSelection;

		// Token: 0x0400A588 RID: 42376
		[SerializeField]
		private Button refreshButton;

		// Token: 0x0400A589 RID: 42377
		[SerializeField]
		private Button previousButton;

		// Token: 0x0400A58A RID: 42378
		[SerializeField]
		private Button nextButton;

		// Token: 0x0400A58B RID: 42379
		[SerializeField]
		private TMP_Text pageLabel;

		// Token: 0x0400A58C RID: 42380
		[SerializeField]
		[Range(5f, 100f)]
		private int pageSize = 20;

		// Token: 0x0400A58D RID: 42381
		[Header("Summary")]
		[SerializeField]
		private TMP_Text entryCountLabel;

		// Token: 0x0400A58E RID: 42382
		[SerializeField]
		private TMP_Text personalRankLabel;

		// Token: 0x0400A58F RID: 42383
		[SerializeField]
		private TMP_Text personalDistanceLabel;

		// Token: 0x0400A590 RID: 42384
		[SerializeField]
		private TMP_Text personalScoreLabel;

		// Token: 0x0400A591 RID: 42385
		[Header("Reset record")]
		[SerializeField]
		private Button resetRecordButton;

		// Token: 0x0400A592 RID: 42386
		[SerializeField]
		private GameObject resetConfirmation;

		// Token: 0x0400A593 RID: 42387
		[SerializeField]
		private Button cancelResetButton;

		// Token: 0x0400A594 RID: 42388
		[SerializeField]
		private Button confirmResetButton;

		// Token: 0x0400A595 RID: 42389
		[SerializeField]
		private CanvasGroup browseControls;

		// Token: 0x0400A596 RID: 42390
		[Header("Entries")]
		[SerializeField]
		private RectTransform rowsParent;

		// Token: 0x0400A597 RID: 42391
		[SerializeField]
		private GameObject rowTemplate;

		// Token: 0x0400A598 RID: 42392
		[SerializeField]
		private ScrollRect entriesScroll;

		// Token: 0x0400A599 RID: 42393
		[SerializeField]
		private GameObject statePanel;

		// Token: 0x0400A59A RID: 42394
		[SerializeField]
		private TMP_Text stateTitle;

		// Token: 0x0400A59B RID: 42395
		[SerializeField]
		private TMP_Text stateMessage;

		// Token: 0x0400A59C RID: 42396
		[SerializeField]
		private Color rowColor = new Color(0.13f, 0.09f, 0.18f);

		// Token: 0x0400A59D RID: 42397
		[SerializeField]
		private Color alternateRowColor = new Color(0.16f, 0.11f, 0.22f);

		// Token: 0x0400A59E RID: 42398
		[SerializeField]
		private Color localRowColor = new Color(0.26f, 0.16f, 0.33f);

		// Token: 0x0400A59F RID: 42399
		[SerializeField]
		private Color podiumTextColor = new Color32(222, 194, 243, byte.MaxValue);

		// Token: 0x0400A5A0 RID: 42400
		[SerializeField]
		private Color localTextColor = new Color32(243, 143, 168, byte.MaxValue);

		// Token: 0x0400A5A1 RID: 42401
		private readonly List<GameObject> _rows = new List<GameObject>();

		// Token: 0x0400A5A2 RID: 42402
		private RankedLeaderboardScope _scope;

		// Token: 0x0400A5A3 RID: 42403
		private RankedLeaderboardResult _result;

		// Token: 0x0400A5A4 RID: 42404
		private IDisposable _request;

		// Token: 0x0400A5A5 RID: 42405
		private int _requestVersion;

		// Token: 0x0400A5A6 RID: 42406
		private int _page;

		// Token: 0x0400A5A7 RID: 42407
		private bool _resetInFlight;

		// Token: 0x0400A5A8 RID: 42408
		private string _resetFailedAccountId;

		// Token: 0x0400A5A9 RID: 42409
		private string _resetAccountId;
	}
}
