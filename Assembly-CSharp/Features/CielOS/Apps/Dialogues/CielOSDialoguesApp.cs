using System;
using System.Collections.Generic;
using System.Linq;
using Bozo.AnimeCharacters;
using Features.CielOS.WindowManager;
using Features.Detection;
using Features.Detection.Customization;
using Michsky.MUIP;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x02001489 RID: 5257
	public class CielOSDialoguesApp : MonoBehaviour
	{
		// Token: 0x0600A20C RID: 41484 RVA: 0x00434CB8 File Offset: 0x00432EB8
		private void Awake()
		{
			DialogueTextLocalization.BindPrefab(base.transform);
			DialogueLibraryService.EnsureLoaded();
			this.HidePrefabTemplates();
			this.HookStaticControls();
			this.BuildPillRows();
			this.SetupHints();
			if (this.wizardView != null)
			{
				this.wizardView.gameObject.SetActive(false);
			}
			if (this.detailView != null)
			{
				this.detailView.gameObject.SetActive(false);
			}
			if (this.confirmDialog != null)
			{
				this.confirmDialog.gameObject.SetActive(false);
			}
			if (this.mainView != null)
			{
				this.mainView.SetActive(true);
			}
			(base.GetComponent<DialogueResponsiveLayout>() ?? base.gameObject.AddComponent<DialogueResponsiveLayout>()).Initialize();
		}

		// Token: 0x0600A20D RID: 41485 RVA: 0x00434D7E File Offset: 0x00432F7E
		private void Start()
		{
			this.MaximizeWindow();
		}

		// Token: 0x0600A20E RID: 41486 RVA: 0x00434D86 File Offset: 0x00432F86
		private void OnEnable()
		{
			DialogueLibraryService.Changed += this.OnLibraryChanged;
			this.RefreshList();
		}

		// Token: 0x0600A20F RID: 41487 RVA: 0x00434D9F File Offset: 0x00432F9F
		private void OnDisable()
		{
			DialogueLibraryService.Changed -= this.OnLibraryChanged;
		}

		// Token: 0x0600A210 RID: 41488 RVA: 0x00434DB2 File Offset: 0x00432FB2
		private void OnLibraryChanged()
		{
			if (this == null || !base.isActiveAndEnabled)
			{
				return;
			}
			if (this._suppressListRefresh)
			{
				this._pendingRefresh = true;
				return;
			}
			this.RefreshList();
		}

		// Token: 0x0600A211 RID: 41489 RVA: 0x00434DDC File Offset: 0x00432FDC
		private void OnWeightDragBegin()
		{
			this._suppressListRefresh = true;
		}

		// Token: 0x0600A212 RID: 41490 RVA: 0x00434DE5 File Offset: 0x00432FE5
		private void OnWeightDragEnd()
		{
			this._suppressListRefresh = false;
			if (this._pendingRefresh)
			{
				this._pendingRefresh = false;
				this.RefreshList();
			}
		}

		// Token: 0x0600A213 RID: 41491 RVA: 0x00434E03 File Offset: 0x00433003
		private void HidePrefabTemplates()
		{
			if (this.docRowTemplate != null)
			{
				this.docRowTemplate.gameObject.SetActive(false);
			}
			if (this.pageNumberTemplate != null)
			{
				this.pageNumberTemplate.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600A214 RID: 41492 RVA: 0x00434E44 File Offset: 0x00433044
		private void HookStaticControls()
		{
			if (this.resetFiltersButton != null)
			{
				this.resetFiltersButton.onClick.AddListener(new UnityAction(this.ResetFilters));
			}
			if (this.headerSaveButton != null)
			{
				this.headerSaveButton.onClick.AddListener(delegate
				{
					DialogueTextLocalization.Toast("All changes are saved.", "Saved");
				});
			}
			if (this.newLineButton != null)
			{
				this.newLineButton.onClick.AddListener(new UnityAction(this.OpenCreate));
			}
			if (this.sortButton != null)
			{
				this.sortButton.onClick.AddListener(new UnityAction(this.CycleSort));
			}
			if (this.searchInput != null)
			{
				this.searchInput.onValueChanged.AddListener(delegate(string s)
				{
					this._search = s;
					this._page = 0;
					this.RefreshList();
				});
			}
			if (this.firstPageButton != null)
			{
				this.firstPageButton.onClick.AddListener(delegate
				{
					this.GoToPage(0);
				});
			}
			if (this.prevPageButton != null)
			{
				this.prevPageButton.onClick.AddListener(delegate
				{
					this.GoToPage(this._page - 1);
				});
			}
			if (this.nextPageButton != null)
			{
				this.nextPageButton.onClick.AddListener(delegate
				{
					this.GoToPage(this._page + 1);
				});
			}
			if (this.lastPageButton != null)
			{
				this.lastPageButton.onClick.AddListener(delegate
				{
					this.GoToPage(int.MaxValue);
				});
			}
			if (this.hintPrevButton != null)
			{
				this.hintPrevButton.onClick.AddListener(delegate
				{
					this.StepHint(-1);
				});
			}
			if (this.hintNextButton != null)
			{
				this.hintNextButton.onClick.AddListener(delegate
				{
					this.StepHint(1);
				});
			}
			if (this.sortLabel != null)
			{
				DialogueTextLocalization.Bind(this.sortLabel, CielOSDialoguesApp.SortLabels[this._sortIndex]);
			}
		}

		// Token: 0x0600A215 RID: 41493 RVA: 0x00435057 File Offset: 0x00433257
		private void SetupHints()
		{
			this._hintIndex = 0;
			this._hintTimer = 0f;
			this.ShowHint(0);
		}

		// Token: 0x0600A216 RID: 41494 RVA: 0x00435074 File Offset: 0x00433274
		private void StepHint(int delta)
		{
			if (CielOSDialoguesApp.Hints.Length == 0)
			{
				return;
			}
			this._hintIndex = ((this._hintIndex + delta) % CielOSDialoguesApp.Hints.Length + CielOSDialoguesApp.Hints.Length) % CielOSDialoguesApp.Hints.Length;
			this._hintTimer = 0f;
			this.ShowHint(this._hintIndex);
		}

		// Token: 0x0600A217 RID: 41495 RVA: 0x004350C8 File Offset: 0x004332C8
		private void ShowHint(int index)
		{
			if (CielOSDialoguesApp.Hints.Length == 0)
			{
				return;
			}
			index = (index % CielOSDialoguesApp.Hints.Length + CielOSDialoguesApp.Hints.Length) % CielOSDialoguesApp.Hints.Length;
			this._hintIndex = index;
			if (this.hintText != null)
			{
				DialogueTextLocalization.Bind(this.hintText, CielOSDialoguesApp.Hints[index]);
			}
			if (this.hintCounterText != null)
			{
				this.hintCounterText.text = string.Format("{0} / {1}", index + 1, CielOSDialoguesApp.Hints.Length);
			}
		}

		// Token: 0x0600A218 RID: 41496 RVA: 0x00435158 File Offset: 0x00433358
		private void Update()
		{
			if (this.hintText == null || this.hintRotateSeconds <= 0f || CielOSDialoguesApp.Hints.Length <= 1)
			{
				return;
			}
			this._hintTimer += Time.unscaledDeltaTime;
			if (this._hintTimer >= this.hintRotateSeconds)
			{
				this.StepHint(1);
			}
		}

		// Token: 0x0600A219 RID: 41497 RVA: 0x004351B4 File Offset: 0x004333B4
		private void BuildPillRows()
		{
			this.WirePillRow(this.groupButtonsParent, CielOSDialoguesApp.GroupOrder.Length, new Action<int>(this.SetGroup));
			this.WirePillRow(this.personalityPillsParent, CielOSDialoguesApp.PersonalityFilterLabels.Length, delegate(int i)
			{
				this._personalityFilter = i;
				this._page = 0;
				this.RefreshPills();
				this.RefreshList();
			});
			this.WirePillRow(this.genderPillsParent, CielOSDialoguesApp.GenderFilterLabels.Length, delegate(int i)
			{
				this._genderFilter = i;
				this._page = 0;
				this.RefreshPills();
				this.RefreshList();
			});
			this.WirePillRow(this.typePillsParent, CielOSDialoguesApp.TypeFilterLabels.Length, delegate(int i)
			{
				this._typeFilter = i;
				this._page = 0;
				this.RefreshPills();
				this.RefreshList();
			});
			if (this.categoryTabsParent != null)
			{
				this.categoryTabsParent.gameObject.SetActive(false);
			}
			this.RefreshPills();
		}

		// Token: 0x0600A21A RID: 41498 RVA: 0x00435262 File Offset: 0x00433462
		private void SetGroup(int index)
		{
			this._groupIndex = Mathf.Clamp(index, 0, CielOSDialoguesApp.GroupOrder.Length - 1);
			this._page = 0;
			this.RefreshPills();
			this.RefreshList();
		}

		// Token: 0x0600A21B RID: 41499 RVA: 0x00435290 File Offset: 0x00433490
		private void WirePillRow(Transform parent, int count, Action<int> onClick)
		{
			if (parent == null)
			{
				return;
			}
			int num = Mathf.Min(count, parent.childCount);
			for (int i = 0; i < num; i++)
			{
				int captured = i;
				Transform child = parent.GetChild(i);
				ButtonManager component = child.GetComponent<ButtonManager>();
				if (component != null)
				{
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(delegate
					{
						onClick(captured);
					});
				}
				else
				{
					Button component2 = child.GetComponent<Button>();
					if (!(component2 == null))
					{
						component2.onClick.RemoveAllListeners();
						component2.onClick.AddListener(delegate
						{
							onClick(captured);
						});
					}
				}
			}
		}

		// Token: 0x0600A21C RID: 41500 RVA: 0x00435360 File Offset: 0x00433560
		private void RefreshPills()
		{
			this.PaintPillRow(this.groupButtonsParent, this._groupIndex);
			this.PaintPillRow(this.personalityPillsParent, this._personalityFilter);
			this.PaintPillRow(this.genderPillsParent, this._genderFilter);
			this.PaintPillRow(this.typePillsParent, this._typeFilter);
			this.RefreshGroupSummary();
		}

		// Token: 0x0600A21D RID: 41501 RVA: 0x004353BC File Offset: 0x004335BC
		private void RefreshGroupSummary()
		{
			DialogueActorGroup dialogueActorGroup = CielOSDialoguesApp.GroupOrder[Mathf.Clamp(this._groupIndex, 0, CielOSDialoguesApp.GroupOrder.Length - 1)];
			string text = CielOSDialoguesApp.GroupLabels[Mathf.Clamp(this._groupIndex, 0, CielOSDialoguesApp.GroupLabels.Length - 1)];
			if (this.editingGroupLabel != null)
			{
				DialogueTextLocalization.Bind(this.editingGroupLabel, text);
			}
			ValueTuple<int, int, int> dialogueDocCounts = DialogueLibraryService.GetDialogueDocCounts(dialogueActorGroup);
			int item = dialogueDocCounts.Item1;
			int item2 = dialogueDocCounts.Item2;
			int item3 = dialogueDocCounts.Item3;
			if (this.groupTotalLabel != null)
			{
				this.groupTotalLabel.text = item.ToString();
			}
			if (this.groupSystemLabel != null)
			{
				this.groupSystemLabel.text = item2.ToString();
			}
			if (this.groupCustomLabel != null)
			{
				this.groupCustomLabel.text = item3.ToString();
			}
			Transform transform = ((this.groupCountLabelsParent != null) ? this.groupCountLabelsParent : this.groupButtonsParent);
			if (transform != null)
			{
				int num = Mathf.Min(CielOSDialoguesApp.GroupOrder.Length, transform.childCount);
				for (int i = 0; i < num; i++)
				{
					Transform child = transform.GetChild(i);
					TextMeshProUGUI textMeshProUGUI;
					if (!(this.groupCountLabelsParent != null))
					{
						Transform transform2 = child.Find("Count");
						textMeshProUGUI = ((transform2 != null) ? transform2.GetComponent<TextMeshProUGUI>() : null);
					}
					else
					{
						textMeshProUGUI = child.GetComponent<TextMeshProUGUI>();
					}
					TextMeshProUGUI textMeshProUGUI2 = textMeshProUGUI;
					if (textMeshProUGUI2 != null)
					{
						textMeshProUGUI2.text = DialogueLibraryService.GetDialogueDocCounts(CielOSDialoguesApp.GroupOrder[i]).Item1.ToString();
					}
				}
			}
		}

		// Token: 0x0600A21E RID: 41502 RVA: 0x0043554C File Offset: 0x0043374C
		private void PaintPillRow(Transform parent, int selectedIndex)
		{
			if (parent == null)
			{
				return;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				bool flag = i == selectedIndex;
				Transform transform = child.Find("Underline");
				if (transform != null)
				{
					transform.gameObject.SetActive(flag);
					Transform transform2 = child.Find("Label");
					TextMeshProUGUI textMeshProUGUI = ((transform2 != null) ? transform2.GetComponent<TextMeshProUGUI>() : null);
					if (textMeshProUGUI != null)
					{
						textMeshProUGUI.color = (flag ? this.pillSelectedText : this.pillNormalText);
					}
				}
				else
				{
					Image component = child.GetComponent<Image>();
					if (component != null)
					{
						component.color = (flag ? this.pillSelectedColor : this.pillNormalColor);
					}
					TextMeshProUGUI componentInChildren = child.GetComponentInChildren<TextMeshProUGUI>(true);
					if (componentInChildren != null)
					{
						componentInChildren.color = (flag ? this.pillSelectedText : this.pillNormalText);
					}
				}
			}
		}

		// Token: 0x0600A21F RID: 41503 RVA: 0x0043563C File Offset: 0x0043383C
		private void CycleSort()
		{
			this._sortIndex = (this._sortIndex + 1) % CielOSDialoguesApp.SortLabels.Length;
			if (this.sortLabel != null)
			{
				DialogueTextLocalization.Bind(this.sortLabel, CielOSDialoguesApp.SortLabels[this._sortIndex]);
			}
			this.RefreshList();
		}

		// Token: 0x0600A220 RID: 41504 RVA: 0x0043568C File Offset: 0x0043388C
		private void ResetFilters()
		{
			this._personalityFilter = 0;
			this._genderFilter = 0;
			this._typeFilter = 0;
			this._search = "";
			this._sortIndex = 0;
			this._page = 0;
			if (this.searchInput != null)
			{
				this.searchInput.SetTextWithoutNotify("");
			}
			if (this.sortLabel != null)
			{
				DialogueTextLocalization.Bind(this.sortLabel, CielOSDialoguesApp.SortLabels[0]);
			}
			this.RefreshPills();
			this.RefreshList();
			DialogueTextLocalization.Toast("Filters reset.", "Filters updated");
		}

		// Token: 0x0600A221 RID: 41505 RVA: 0x00435720 File Offset: 0x00433920
		private List<DialogueDocView> QueryDocs()
		{
			PersonalityType? personalityType = CielOSDialoguesApp.PersonalityFilterValues[this._personalityFilter];
			DialogueGender? dialogueGender = CielOSDialoguesApp.GenderFilterValues[this._genderFilter];
			List<DialogueDocView> list = DialogueLibraryService.GetDialogueDocs(CielOSDialoguesApp.GroupOrder[Mathf.Clamp(this._groupIndex, 0, CielOSDialoguesApp.GroupOrder.Length - 1)], personalityType, dialogueGender);
			bool? typeFilter = CielOSDialoguesApp.TypeFilterValues[Mathf.Clamp(this._typeFilter, 0, CielOSDialoguesApp.TypeFilterValues.Length - 1)];
			if (typeFilter != null)
			{
				list = list.Where<DialogueDocView>((DialogueDocView d) => d.isSystem == typeFilter.Value).ToList<DialogueDocView>();
			}
			if (!string.IsNullOrWhiteSpace(this._search))
			{
				string q = this._search.Trim().ToLowerInvariant();
				list = list.Where<DialogueDocView>(delegate(DialogueDocView d)
				{
					Func<DialogueLine, bool> <>9__4;
					Func<DialogueAltView, bool> <>9__3;
					return d.DisplayLabel.ToLowerInvariant().Contains(q) || (d.isSystem && DialogueTextLocalization.DocumentTitle(d).ToLowerInvariant().Contains(q)) || d.phases.Any<DialoguePhaseView>(delegate(DialoguePhaseView ph)
					{
						IEnumerable<DialogueAltView> alternatives = ph.alternatives;
						Func<DialogueAltView, bool> func;
						if ((func = <>9__3) == null)
						{
							func = (<>9__3 = delegate(DialogueAltView a)
							{
								if (a.lines != null)
								{
									IEnumerable<DialogueLine> lines = a.lines;
									Func<DialogueLine, bool> func2;
									if ((func2 = <>9__4) == null)
									{
										func2 = (<>9__4 = (DialogueLine l) => l != null && !string.IsNullOrEmpty(l.text) && (l.text.ToLowerInvariant().Contains(q) || (d.isSystem && DialogueTextLocalization.SystemContent(l.text).ToLowerInvariant().Contains(q))));
									}
									return lines.Any<DialogueLine>(func2);
								}
								return false;
							});
						}
						return alternatives.Any<DialogueAltView>(func);
					});
				}).ToList<DialogueDocView>();
			}
			switch (this._sortIndex)
			{
			case 0:
				list = (from d in list
					orderby d.macroWeight descending, d.DisplayLabel
					select d).ToList<DialogueDocView>();
				break;
			case 1:
				list = (from d in list
					orderby d.macroWeight, d.DisplayLabel
					select d).ToList<DialogueDocView>();
				break;
			case 2:
				list = (from d in list
					orderby d.isSystem descending, d.DisplayLabel
					select d).ToList<DialogueDocView>();
				break;
			case 3:
				list = list.OrderByDescending<DialogueDocView, int>((DialogueDocView d) => d.TotalLines).ToList<DialogueDocView>();
				break;
			}
			return list;
		}

		// Token: 0x0600A222 RID: 41506 RVA: 0x0043594C File Offset: 0x00433B4C
		public void RefreshList()
		{
			if (this.listContent == null || this.docRowTemplate == null)
			{
				return;
			}
			List<DialogueDocView> list = this.QueryDocs();
			this._totalCount = list.Count;
			int num = Mathf.Max(1, Mathf.CeilToInt((float)this._totalCount / 8f));
			this._page = Mathf.Clamp(this._page, 0, num - 1);
			List<DialogueDocView> list2 = list.Skip<DialogueDocView>(this._page * 8).Take<DialogueDocView>(8).ToList<DialogueDocView>();
			this.EnsureRowPool(list2.Count);
			for (int i = 0; i < this._rowPool.Count; i++)
			{
				if (i < list2.Count)
				{
					DialogueDocView d = list2[i];
					bool flag = !d.isSystem && !DialogueLibraryService.IsCustomDocValid(d.id);
					this._rowPool[i].Bind(d, () => CielOSDialoguesApp.BindingLabel(d), new Action<string>(this.OpenDetail), delegate(string pid, int w)
					{
						DialogueLibraryService.SetMacroWeight(pid, w);
					}, new Action<string>(this.OpenEdit), new Action<string>(this.RequestDelete), flag, new Action(this.OnWeightDragBegin), new Action(this.OnWeightDragEnd));
				}
				else
				{
					this._rowPool[i].gameObject.SetActive(false);
				}
			}
			if (this.emptyState != null)
			{
				this.emptyState.SetActive(list2.Count == 0);
			}
			this.RenderPagination(num);
			this.UpdateRangeLabel();
			this.ResetScrollToTop();
		}

		// Token: 0x0600A223 RID: 41507 RVA: 0x00435B14 File Offset: 0x00433D14
		private static string BindingLabel(DialogueDocView d)
		{
			string text = DialogueTextLocalization.Get((d.gender == DialogueGender.Any) ? "All Genders" : d.gender.ToString());
			if (d.actorGroup != DialogueActorGroup.Civilians)
			{
				return text;
			}
			return DialogueTextLocalization.Get(d.personality.ToString()) + " · " + text;
		}

		// Token: 0x0600A224 RID: 41508 RVA: 0x00435B74 File Offset: 0x00433D74
		private void EnsureRowPool(int needed)
		{
			while (this._rowPool.Count < needed)
			{
				DialogueDocRowWidget dialogueDocRowWidget = Object.Instantiate<DialogueDocRowWidget>(this.docRowTemplate, this.listContent);
				dialogueDocRowWidget.gameObject.SetActive(false);
				this._rowPool.Add(dialogueDocRowWidget);
			}
		}

		// Token: 0x0600A225 RID: 41509 RVA: 0x00435BBC File Offset: 0x00433DBC
		private void ResetScrollToTop()
		{
			Canvas.ForceUpdateCanvases();
			if (this.listContent != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.listContent);
			}
			if (this.listScrollRect != null)
			{
				this.listScrollRect.verticalNormalizedPosition = 1f;
			}
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x0600A226 RID: 41510 RVA: 0x00435C0A File Offset: 0x00433E0A
		private void RebuildListLayout()
		{
			Canvas.ForceUpdateCanvases();
			if (this.listContent != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.listContent);
			}
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x0600A227 RID: 41511 RVA: 0x00435C30 File Offset: 0x00433E30
		private void RenderPagination(int pageCount)
		{
			if (this.pagesParent == null || this.pageNumberTemplate == null)
			{
				return;
			}
			List<int> list = CielOSDialoguesApp.BuildPageList(pageCount, this._page, 4);
			this.EnsurePageButtonPool(list.Count);
			for (int i = 0; i < this._pageButtonPool.Count; i++)
			{
				Button button = this._pageButtonPool[i];
				if (i >= list.Count)
				{
					button.gameObject.SetActive(false);
				}
				else
				{
					int num = list[i];
					button.onClick.RemoveAllListeners();
					TextMeshProUGUI componentInChildren = button.GetComponentInChildren<TextMeshProUGUI>(true);
					if (num < 0)
					{
						if (componentInChildren != null)
						{
							DialogueTextLocalization.Bind(componentInChildren, "…");
						}
						button.interactable = false;
					}
					else
					{
						if (componentInChildren != null)
						{
							DialogueTextLocalization.UserText(componentInChildren, (num + 1).ToString());
						}
						button.interactable = true;
						int captured = num;
						button.onClick.AddListener(delegate
						{
							this.GoToPage(captured);
						});
						Image component = button.GetComponent<Image>();
						if (component != null)
						{
							component.color = ((num == this._page) ? this.pillSelectedColor : this.pillNormalColor);
						}
						if (componentInChildren != null)
						{
							componentInChildren.color = ((num == this._page) ? this.pillSelectedText : this.pillNormalText);
						}
					}
					button.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600A228 RID: 41512 RVA: 0x00435DB0 File Offset: 0x00433FB0
		private void EnsurePageButtonPool(int needed)
		{
			while (this._pageButtonPool.Count < needed)
			{
				Button button = Object.Instantiate<Button>(this.pageNumberTemplate, this.pagesParent);
				button.gameObject.SetActive(false);
				this._pageButtonPool.Add(button);
			}
		}

		// Token: 0x0600A229 RID: 41513 RVA: 0x00435DF8 File Offset: 0x00433FF8
		private static List<int> BuildPageList(int pageCount, int current, int maxButtons)
		{
			List<int> list = new List<int>();
			if (pageCount <= maxButtons + 2)
			{
				for (int i = 0; i < pageCount; i++)
				{
					list.Add(i);
				}
				return list;
			}
			list.Add(0);
			int num = Mathf.Max(1, current - 1);
			int num2 = Mathf.Min(pageCount - 2, current + 1);
			if (num > 1)
			{
				list.Add(-1);
			}
			for (int j = num; j <= num2; j++)
			{
				list.Add(j);
			}
			if (num2 < pageCount - 2)
			{
				list.Add(-1);
			}
			list.Add(pageCount - 1);
			return list;
		}

		// Token: 0x0600A22A RID: 41514 RVA: 0x00435E78 File Offset: 0x00434078
		private void GoToPage(int page)
		{
			int num = Mathf.Max(1, Mathf.CeilToInt((float)this._totalCount / 8f));
			this._page = Mathf.Clamp(page, 0, num - 1);
			this.RefreshList();
		}

		// Token: 0x0600A22B RID: 41515 RVA: 0x00435EB4 File Offset: 0x004340B4
		private void UpdateRangeLabel()
		{
			if (this.rangeLabel == null)
			{
				return;
			}
			if (this._totalCount == 0)
			{
				DialogueTextLocalization.Bind(this.rangeLabel, "0 dialogues");
				return;
			}
			int num = this._page * 8 + 1;
			int num2 = Mathf.Min(this._totalCount, (this._page + 1) * 8);
			DialogueTextLocalization.BindFormat(this.rangeLabel, (this._totalCount == 1) ? "{0}–{1} of {2} dialogue" : "{0}–{1} of {2} dialogues", new object[] { num, num2, this._totalCount });
		}

		// Token: 0x0600A22C RID: 41516 RVA: 0x00435F50 File Offset: 0x00434150
		private void OpenDetail(string profileId)
		{
			if (this.detailView == null)
			{
				return;
			}
			DialogueDocView? dialogueDocById = DialogueLibraryService.GetDialogueDocById(profileId);
			if (dialogueDocById == null)
			{
				return;
			}
			this._openDetailId = profileId;
			if (this.wizardView != null)
			{
				this.wizardView.gameObject.SetActive(false);
			}
			if (this.mainView != null)
			{
				this.mainView.SetActive(false);
			}
			bool flag = !dialogueDocById.Value.isSystem;
			this.detailView.Open(dialogueDocById.Value, new Action(this.CloseDetail), new Action<string>(this.RequestDelete), new Action<string>(this.RequestDuplicate), delegate(string setId, int w)
			{
				DialogueLibraryService.SetWeight(setId, w);
			}, delegate(string setId)
			{
				DialogueLibraryService.ResetWeight(setId);
				this.OpenDetail(this._openDetailId);
			}, flag ? new Action<string, string>(this.RenameDoc) : null, flag ? new Action<string, DialogueCategory>(this.AddSetToDoc) : null, flag ? new Action<string>(this.RequestDeleteSet) : null, flag ? new Action<string, int>(this.EditLine) : null, flag ? new Action<string, int>(this.DeleteLineFromSet) : null, flag ? new Action<string, string[]>(this.ReorderLines) : null, flag ? new Action<string>(this.SaveDoc) : null, flag ? new Action<string, PersonalityType, DialogueGender, DialogueActorGroup>(this.SetDocBinding) : null, flag ? DialogueLibraryService.ExpectedPhases(dialogueDocById.Value.actorGroup) : null);
		}

		// Token: 0x0600A22D RID: 41517 RVA: 0x004360D8 File Offset: 0x004342D8
		private void CloseDetail()
		{
			if (this.detailView != null)
			{
				this.detailView.gameObject.SetActive(false);
			}
			if (this.mainView != null)
			{
				this.mainView.SetActive(true);
			}
			this.RefreshList();
		}

		// Token: 0x0600A22E RID: 41518 RVA: 0x00436124 File Offset: 0x00434324
		private void OpenEdit(string profileId)
		{
			this.OpenDetail(profileId);
		}

		// Token: 0x0600A22F RID: 41519 RVA: 0x00436130 File Offset: 0x00434330
		private void OpenCreate()
		{
			if (this.wizardView == null)
			{
				DialogueTextLocalization.Toast("Wizard unavailable.", "Error");
				return;
			}
			DialogueActorGroup dialogueActorGroup = CielOSDialoguesApp.GroupOrder[Mathf.Clamp(this._groupIndex, 0, CielOSDialoguesApp.GroupOrder.Length - 1)];
			if (this.detailView != null)
			{
				this.detailView.gameObject.SetActive(false);
			}
			if (this.mainView != null)
			{
				this.mainView.SetActive(false);
			}
			this.wizardView.Open(dialogueActorGroup, new Action<DialogueActorGroup, PersonalityType, DialogueGender, string>(this.CreateFromWizard), new Action(this.CloseWizard));
		}

		// Token: 0x0600A230 RID: 41520 RVA: 0x004361D4 File Offset: 0x004343D4
		private void CreateFromWizard(DialogueActorGroup actor, PersonalityType personality, DialogueGender gender, string name)
		{
			string text = DialogueLibraryService.CreateCustomDoc(name, personality, gender, actor, 10);
			int num = Array.IndexOf<DialogueActorGroup>(CielOSDialoguesApp.GroupOrder, actor);
			if (num >= 0)
			{
				this._groupIndex = num;
				this._page = 0;
				this.RefreshPills();
			}
			this.OpenDetail(text);
			DialogueTextLocalization.Toast("Dialogue created. Add a set with lines to each phase, then Save.", "Draft created");
		}

		// Token: 0x0600A231 RID: 41521 RVA: 0x00436228 File Offset: 0x00434428
		private void CloseWizard()
		{
			if (this.wizardView != null)
			{
				this.wizardView.Hide();
			}
			if (this.mainView != null)
			{
				this.mainView.SetActive(true);
			}
			this.RefreshList();
		}

		// Token: 0x0600A232 RID: 41522 RVA: 0x00436263 File Offset: 0x00434463
		private void RenameDoc(string docId, string newName)
		{
			DialogueLibraryService.RenameCustomDoc(docId, newName);
		}

		// Token: 0x0600A233 RID: 41523 RVA: 0x0043626D File Offset: 0x0043446D
		private void AddSetToDoc(string docId, DialogueCategory category)
		{
			DialogueLibraryService.AddSet(docId, category, 10);
			this.OpenDetail(docId);
		}

		// Token: 0x0600A234 RID: 41524 RVA: 0x00436280 File Offset: 0x00434480
		private void RequestDeleteSet(string setId)
		{
			Func<string> func = () => "";
			DialogueDocView? dialogueDocById = DialogueLibraryService.GetDialogueDocById(this._openDetailId);
			if (dialogueDocById != null)
			{
				foreach (DialoguePhaseView dialoguePhaseView in dialogueDocById.Value.phases)
				{
					for (int j = 0; j < dialoguePhaseView.alternatives.Length; j++)
					{
						if (dialoguePhaseView.alternatives[j].id == setId)
						{
							int lines = ((dialoguePhaseView.alternatives[j].lines != null) ? dialoguePhaseView.alternatives[j].lines.Length : 0);
							int ordinal = j + 1;
							func = () => DialogueTextLocalization.Format((lines == 1) ? "Set {0} · {1} line" : "Set {0} · {1} lines", new object[] { ordinal, lines });
						}
					}
				}
			}
			if (this.confirmDialog != null)
			{
				this.confirmDialog.Show("Delete this set?", func(), "This removes the set and all its lines.", delegate
				{
					DialogueLibraryService.DeleteSet(this._openDetailId, setId);
					this.OpenDetail(this._openDetailId);
				}, func);
				return;
			}
			DialogueLibraryService.DeleteSet(this._openDetailId, setId);
			this.OpenDetail(this._openDetailId);
		}

		// Token: 0x0600A235 RID: 41525 RVA: 0x004363E6 File Offset: 0x004345E6
		private void DeleteLineFromSet(string setId, int lineIndex)
		{
			DialogueLibraryService.DeleteLine(this._openDetailId, setId, lineIndex);
			this.OpenDetail(this._openDetailId);
		}

		// Token: 0x0600A236 RID: 41526 RVA: 0x00436404 File Offset: 0x00434604
		private void ApplySetLines(string docId, string setId, DialogueLine[] newLines)
		{
			DialogueDocView? dialogueDocById = DialogueLibraryService.GetDialogueDocById(docId);
			if (dialogueDocById != null)
			{
				int num = 0;
				DialoguePhaseView[] phases = dialogueDocById.Value.phases;
				for (int i = 0; i < phases.Length; i++)
				{
					foreach (DialogueAltView dialogueAltView in phases[i].alternatives)
					{
						if (dialogueAltView.id == setId)
						{
							num = ((dialogueAltView.lines != null) ? dialogueAltView.lines.Length : 0);
						}
					}
				}
				for (int k = num - 1; k >= 0; k--)
				{
					DialogueLibraryService.DeleteLine(docId, setId, k);
				}
			}
			if (newLines != null)
			{
				foreach (DialogueLine dialogueLine in newLines)
				{
					DialogueLibraryService.AddLine(docId, setId, (dialogueLine != null) ? dialogueLine.text : "");
				}
			}
		}

		// Token: 0x0600A237 RID: 41527 RVA: 0x004364E4 File Offset: 0x004346E4
		private void EditLine(string setId, int lineIndex)
		{
			if (this.lineDialog == null)
			{
				if (lineIndex < 0)
				{
					DialogueLibraryService.AddLine(this._openDetailId, setId, "");
					this.OpenDetail(this._openDetailId);
				}
				return;
			}
			string text3 = "";
			if (lineIndex >= 0)
			{
				DialogueDocView? dialogueDocById = DialogueLibraryService.GetDialogueDocById(this._openDetailId);
				if (dialogueDocById != null)
				{
					DialoguePhaseView[] phases = dialogueDocById.Value.phases;
					for (int i = 0; i < phases.Length; i++)
					{
						foreach (DialogueAltView dialogueAltView in phases[i].alternatives)
						{
							if (dialogueAltView.id == setId && dialogueAltView.lines != null && lineIndex < dialogueAltView.lines.Length)
							{
								text3 = ((dialogueAltView.lines[lineIndex] != null) ? dialogueAltView.lines[lineIndex].text : "");
							}
						}
					}
				}
			}
			string text2 = ((lineIndex < 0) ? "Add line" : "Edit line");
			this.lineDialog.Show(text2, text3, delegate(string text)
			{
				if (lineIndex < 0)
				{
					DialogueLibraryService.AddLine(this._openDetailId, setId, text);
				}
				else
				{
					DialogueLibraryService.UpdateLine(this._openDetailId, setId, lineIndex, text);
				}
				this.OpenDetail(this._openDetailId);
			});
		}

		// Token: 0x0600A238 RID: 41528 RVA: 0x00436650 File Offset: 0x00434850
		private void ReorderLines(string setId, string[] orderedTexts)
		{
			List<DialogueLine> list = new List<DialogueLine>();
			if (orderedTexts != null)
			{
				foreach (string text in orderedTexts)
				{
					list.Add(new DialogueLine
					{
						text = text
					});
				}
			}
			this.ApplySetLines(this._openDetailId, setId, list.ToArray());
			this.OpenDetail(this._openDetailId);
		}

		// Token: 0x0600A239 RID: 41529 RVA: 0x004366AC File Offset: 0x004348AC
		private void SetDocBinding(string docId, PersonalityType personality, DialogueGender gender, DialogueActorGroup actor)
		{
			if (!DialogueLibraryService.SetCustomDocBinding(docId, personality, gender, new DialogueActorGroup?(actor)))
			{
				return;
			}
			this.OpenDetail(docId);
			ToastService.ShowFormat("dialogues_ui", "Now speaks for: {0}", new object[]
			{
				new DialogueTextLocalization.Argument(() => CielOSDialoguesApp.BindingSummary(actor, personality, gender))
			}, "Binding updated", null, -1f);
		}

		// Token: 0x0600A23A RID: 41530 RVA: 0x00436730 File Offset: 0x00434930
		private static string BindingSummary(DialogueActorGroup actor, PersonalityType personality, DialogueGender gender)
		{
			string text = DialogueTextLocalization.Get((gender == DialogueGender.Any) ? "All Genders" : gender.ToString());
			if (actor != DialogueActorGroup.Civilians)
			{
				return DialogueTextLocalization.Get(actor.ToString()) + " · " + text;
			}
			return DialogueTextLocalization.Get(personality.ToString()) + " · " + text;
		}

		// Token: 0x0600A23B RID: 41531 RVA: 0x00436798 File Offset: 0x00434998
		private void SaveDoc(string docId)
		{
			if (!DialogueLibraryService.IsCustomDocValid(docId))
			{
				DialogueLibraryService.GetMissingRequiredPhases(docId);
				DialogueTextLocalization.Toast("Add a set with at least one line in every required phase.", "Still incomplete");
				return;
			}
			this.CloseDetail();
		}

		// Token: 0x0600A23C RID: 41532 RVA: 0x004367C0 File Offset: 0x004349C0
		private void RequestDelete(string profileId)
		{
			DialogueDocView? dialogueDocById = DialogueLibraryService.GetDialogueDocById(profileId);
			if (dialogueDocById != null && dialogueDocById.Value.isSystem)
			{
				DialogueTextLocalization.Toast("System dialogues can't be deleted.", "Read-only");
				return;
			}
			if (this.confirmDialog != null)
			{
				this.confirmDialog.Show("Delete this dialogue?", (dialogueDocById != null) ? dialogueDocById.Value.DisplayLabel : "", "This permanently removes your custom dialogue.", delegate
				{
					DialogueLibraryService.DeleteCustomDoc(profileId);
					this.CloseDetail();
				}, null);
				return;
			}
			DialogueLibraryService.DeleteCustomDoc(profileId);
			this.CloseDetail();
		}

		// Token: 0x0600A23D RID: 41533 RVA: 0x00436878 File Offset: 0x00434A78
		private void RequestDuplicate(string profileId)
		{
			string text = DialogueLibraryService.DuplicateToCustomDoc(profileId, null);
			if (string.IsNullOrEmpty(text))
			{
				DialogueTextLocalization.Toast("Couldn't duplicate this dialogue.", "Error");
				return;
			}
			this.OpenDetail(text);
			DialogueTextLocalization.Toast("Editable copy created.", "Duplicated");
		}

		// Token: 0x0600A23E RID: 41534 RVA: 0x004368BC File Offset: 0x00434ABC
		private void MaximizeWindow()
		{
			CielOSWindow componentInParent = base.GetComponentInParent<CielOSWindow>();
			if (componentInParent == null || componentInParent.Rect == null)
			{
				return;
			}
			RectTransform rect = componentInParent.Rect;
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			CielOSWindowResizeHandle[] componentsInChildren = componentInParent.GetComponentsInChildren<CielOSWindowResizeHandle>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			CielOSWindowDragHandle[] componentsInChildren2 = componentInParent.GetComponentsInChildren<CielOSWindowDragHandle>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
		}

		// Token: 0x0400A5AF RID: 42415
		public static readonly string[] PersonalityFilterLabels = new string[] { "All", "Shy", "Bold", "Curious", "Hostile", "Friendly", "Neutral" };

		// Token: 0x0400A5B0 RID: 42416
		private static readonly PersonalityType?[] PersonalityFilterValues = new PersonalityType?[]
		{
			null,
			new PersonalityType?(PersonalityType.Shy),
			new PersonalityType?(PersonalityType.Bold),
			new PersonalityType?(PersonalityType.Curious),
			new PersonalityType?(PersonalityType.Hostile),
			new PersonalityType?(PersonalityType.Friendly),
			new PersonalityType?(PersonalityType.Neutral)
		};

		// Token: 0x0400A5B1 RID: 42417
		public static readonly string[] GenderFilterLabels = new string[] { "All", "Female", "Male" };

		// Token: 0x0400A5B2 RID: 42418
		private static readonly DialogueGender?[] GenderFilterValues = new DialogueGender?[]
		{
			null,
			new DialogueGender?(DialogueGender.Female),
			new DialogueGender?(DialogueGender.Male)
		};

		// Token: 0x0400A5B3 RID: 42419
		public static readonly string[] TypeFilterLabels = new string[] { "All", "System", "Custom" };

		// Token: 0x0400A5B4 RID: 42420
		private static readonly bool?[] TypeFilterValues = new bool?[]
		{
			null,
			new bool?(true),
			new bool?(false)
		};

		// Token: 0x0400A5B5 RID: 42421
		public static readonly string[] CategoryTabLabels = new string[] { "Reaction", "Shame", "Exhibition", "Exit", "Other" };

		// Token: 0x0400A5B6 RID: 42422
		private static readonly DialogueCategory[] CategoryTabValues = new DialogueCategory[]
		{
			DialogueCategory.Reaction,
			DialogueCategory.Shame,
			DialogueCategory.Exhibit,
			DialogueCategory.Exit,
			DialogueCategory.Chasing
		};

		// Token: 0x0400A5B7 RID: 42423
		private const int DefaultCategoryTab = 0;

		// Token: 0x0400A5B8 RID: 42424
		public static readonly string[] SortLabels = new string[] { "Weight (desc)", "Weight (asc)", "Source", "Length" };

		// Token: 0x0400A5B9 RID: 42425
		public static readonly DialogueActorGroup[] GroupOrder = new DialogueActorGroup[]
		{
			DialogueActorGroup.Civilians,
			DialogueActorGroup.Police,
			DialogueActorGroup.Journalists,
			DialogueActorGroup.Referee
		};

		// Token: 0x0400A5BA RID: 42426
		public static readonly string[] GroupLabels = new string[] { "Civilians", "Police", "Journalists", "Referee" };

		// Token: 0x0400A5BB RID: 42427
		private const int PageSize = 8;

		// Token: 0x0400A5BC RID: 42428
		[Header("NPC Groups (actor databases)")]
		[Tooltip("Parent of the NPC-Group nav buttons. Children order MUST match GroupOrder below.")]
		[SerializeField]
		private Transform groupButtonsParent;

		// Token: 0x0400A5BD RID: 42429
		[Tooltip("Label showing which group is being edited (main header 'Editing: <group>').")]
		[SerializeField]
		private TextMeshProUGUI editingGroupLabel;

		// Token: 0x0400A5BE RID: 42430
		[Tooltip("Summary labels under the selected group.")]
		[SerializeField]
		private TextMeshProUGUI groupTotalLabel;

		// Token: 0x0400A5BF RID: 42431
		[SerializeField]
		private TextMeshProUGUI groupSystemLabel;

		// Token: 0x0400A5C0 RID: 42432
		[SerializeField]
		private TextMeshProUGUI groupCustomLabel;

		// Token: 0x0400A5C1 RID: 42433
		[Tooltip("Optional per-group count labels shown inside each nav button (children order = GroupOrder).")]
		[SerializeField]
		private Transform groupCountLabelsParent;

		// Token: 0x0400A5C2 RID: 42434
		[Header("Filters sidebar")]
		[Tooltip("Parent of the Personality pill buttons (children order matches PersonalityFilterLabels).")]
		[SerializeField]
		private Transform personalityPillsParent;

		// Token: 0x0400A5C3 RID: 42435
		[SerializeField]
		private Transform genderPillsParent;

		// Token: 0x0400A5C4 RID: 42436
		[Tooltip("Parent of the Type (All/System/Custom) pill buttons (children order matches TypeFilterLabels).")]
		[SerializeField]
		private Transform typePillsParent;

		// Token: 0x0400A5C5 RID: 42437
		[SerializeField]
		private Button resetFiltersButton;

		// Token: 0x0400A5C6 RID: 42438
		[Header("Hints panel (rotating tips)")]
		[Tooltip("Label that shows the current hint. Hints auto-rotate and can be stepped with the arrows.")]
		[SerializeField]
		private TextMeshProUGUI hintText;

		// Token: 0x0400A5C7 RID: 42439
		[Tooltip("Optional 'previous hint' button.")]
		[SerializeField]
		private Button hintPrevButton;

		// Token: 0x0400A5C8 RID: 42440
		[Tooltip("Optional 'next hint' button.")]
		[SerializeField]
		private Button hintNextButton;

		// Token: 0x0400A5C9 RID: 42441
		[Tooltip("Optional label like '2 / 5' showing which hint is displayed.")]
		[SerializeField]
		private TextMeshProUGUI hintCounterText;

		// Token: 0x0400A5CA RID: 42442
		[Tooltip("Seconds each hint stays on screen before auto-advancing (0 = no auto-rotation).")]
		[SerializeField]
		private float hintRotateSeconds = 8f;

		// Token: 0x0400A5CB RID: 42443
		[Header("Header / toolbar")]
		[SerializeField]
		private Button headerSaveButton;

		// Token: 0x0400A5CC RID: 42444
		[SerializeField]
		private Transform categoryTabsParent;

		// Token: 0x0400A5CD RID: 42445
		[SerializeField]
		private TMP_InputField searchInput;

		// Token: 0x0400A5CE RID: 42446
		[SerializeField]
		private Button sortButton;

		// Token: 0x0400A5CF RID: 42447
		[SerializeField]
		private TextMeshProUGUI sortLabel;

		// Token: 0x0400A5D0 RID: 42448
		[SerializeField]
		private Button newLineButton;

		// Token: 0x0400A5D1 RID: 42449
		[Header("List")]
		[SerializeField]
		private RectTransform listContent;

		// Token: 0x0400A5D2 RID: 42450
		[SerializeField]
		private ScrollRect listScrollRect;

		// Token: 0x0400A5D3 RID: 42451
		[SerializeField]
		private DialogueDocRowWidget docRowTemplate;

		// Token: 0x0400A5D4 RID: 42452
		[SerializeField]
		private GameObject emptyState;

		// Token: 0x0400A5D5 RID: 42453
		[Header("Detail (phase view)")]
		[SerializeField]
		private DialogueDetailWidget detailView;

		// Token: 0x0400A5D6 RID: 42454
		[Header("Pagination")]
		[SerializeField]
		private Transform pagesParent;

		// Token: 0x0400A5D7 RID: 42455
		[SerializeField]
		private Button firstPageButton;

		// Token: 0x0400A5D8 RID: 42456
		[SerializeField]
		private Button prevPageButton;

		// Token: 0x0400A5D9 RID: 42457
		[SerializeField]
		private Button nextPageButton;

		// Token: 0x0400A5DA RID: 42458
		[SerializeField]
		private Button lastPageButton;

		// Token: 0x0400A5DB RID: 42459
		[SerializeField]
		private Button pageNumberTemplate;

		// Token: 0x0400A5DC RID: 42460
		[SerializeField]
		private TextMeshProUGUI rangeLabel;

		// Token: 0x0400A5DD RID: 42461
		[Header("Overlays")]
		[SerializeField]
		private GameObject mainView;

		// Token: 0x0400A5DE RID: 42462
		[Tooltip("Full-page 'New Dialogue' wizard — the only place the binding is configured.")]
		[SerializeField]
		private DialogueWizardWidget wizardView;

		// Token: 0x0400A5DF RID: 42463
		[SerializeField]
		private DialogueConfirmWidget confirmDialog;

		// Token: 0x0400A5E0 RID: 42464
		[Tooltip("Small modal for editing a single line's text (custom authoring).")]
		[SerializeField]
		private DialogueLineEditWidget lineDialog;

		// Token: 0x0400A5E1 RID: 42465
		[Header("Pill visuals")]
		[Tooltip("Sprite tint applied to a selected pill (mockup pink). Unselected pills use white.")]
		[SerializeField]
		private Color pillSelectedColor = new Color(0.86f, 0.34f, 0.44f, 1f);

		// Token: 0x0400A5E2 RID: 42466
		[SerializeField]
		private Color pillNormalColor = Color.white;

		// Token: 0x0400A5E3 RID: 42467
		[SerializeField]
		private Color pillSelectedText = Color.white;

		// Token: 0x0400A5E4 RID: 42468
		[SerializeField]
		private Color pillNormalText = new Color(0.92f, 0.9f, 0.95f, 1f);

		// Token: 0x0400A5E5 RID: 42469
		private int _personalityFilter;

		// Token: 0x0400A5E6 RID: 42470
		private int _genderFilter;

		// Token: 0x0400A5E7 RID: 42471
		private int _typeFilter;

		// Token: 0x0400A5E8 RID: 42472
		private int _sortIndex;

		// Token: 0x0400A5E9 RID: 42473
		private int _groupIndex;

		// Token: 0x0400A5EA RID: 42474
		private string _search = "";

		// Token: 0x0400A5EB RID: 42475
		private int _page;

		// Token: 0x0400A5EC RID: 42476
		private int _totalCount;

		// Token: 0x0400A5ED RID: 42477
		private int _hintIndex;

		// Token: 0x0400A5EE RID: 42478
		private float _hintTimer;

		// Token: 0x0400A5EF RID: 42479
		private readonly List<DialogueDocRowWidget> _rowPool = new List<DialogueDocRowWidget>();

		// Token: 0x0400A5F0 RID: 42480
		private readonly List<Button> _pageButtonPool = new List<Button>();

		// Token: 0x0400A5F1 RID: 42481
		private bool _suppressListRefresh;

		// Token: 0x0400A5F2 RID: 42482
		private bool _pendingRefresh;

		// Token: 0x0400A5F3 RID: 42483
		private static readonly string[] Hints = new string[] { "Every encounter flows the same way. The NPC <b>reacts</b> first, then you pick to <b>show off</b> or get <b>ashamed</b>, and there's an <b>exit</b> line as they walk away.", "Special NPCs get extra moments. A <b>Journalist</b>, for instance, also talks while <b>chasing</b> you, which happens before they actually catch you.", "Tune the <b>weight</b> of each dialogue. Raise the ones you like (or wrote yourself) so they show up more often, and lower the ones you'd rather rarely hear.", "Inside a single dialogue you can add <b>random variants</b> for each phase, and every variant has its own chance of being picked. Mix things up so encounters never feel repetitive!", "The <b>System</b> badge marks the built-in dialogues. Set their weight to 1 to let your <b>custom</b> dialogues take the spotlight during the run." };

		// Token: 0x0400A5F4 RID: 42484
		private string _openDetailId;
	}
}
