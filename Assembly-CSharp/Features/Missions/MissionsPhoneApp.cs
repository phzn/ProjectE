using System;
using Features.Localization;
using Features.OxGram;
using Features.PhoneCompanion;
using ProjektSumperk;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000FB3 RID: 4019
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public sealed class MissionsPhoneApp : MonoBehaviour
	{
		// Token: 0x06007956 RID: 31062 RVA: 0x0031881C File Offset: 0x00316A1C
		internal void Initialize(TMP_FontAsset font)
		{
			if (this._content != null)
			{
				return;
			}
			this._font = ((font != null) ? font : TMP_Settings.defaultFontAsset);
			this._animator = base.GetComponent<Animator>();
			this._island = base.transform.root.GetComponentInChildren<DynamicIslandController>(true);
			Transform transform = base.transform.Find("TrophiesPanel");
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
			Transform transform2 = base.transform.Find("Trophies");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(false);
			}
			Transform transform3 = base.transform.Find("Back");
			if (transform3 != null)
			{
				transform3.gameObject.SetActive(false);
			}
			this._content = OxGramUI.Rect("Missions Content", base.transform);
			this._content.anchorMin = (this._content.anchorMax = Vector2.one * 0.5f);
			this._content.sizeDelta = new Vector2(480f, 1014f);
			RectTransform rectTransform = OxGramUI.Rect("Completed Missions", this._content);
			MissionsPhoneApp.Position(rectTransform, 18f, 99f, 444f, 880f);
			rectTransform.gameObject.SetActive(false);
			rectTransform.gameObject.AddComponent<CompletedMissionsPanel>().Initialize(this._font);
			this._completedPage = rectTransform.gameObject;
			this._title = this.Text("Title", this._content, "MISSIONS", 28f, MissionPanelStyle.Text);
			MissionsPhoneApp.Position(this._title.rectTransform, 112f, 25f, 224f, 50f);
			MissionsPhoneApp.Position((RectTransform)this.Button("Back", "< Back", new UnityAction(this.CloseApp)).transform, 12f, 25f, 100f, 50f);
			Button button = this.Button("Completed Tab", "COMPLETED", new UnityAction(this.ToggleCompleted));
			MissionsPhoneApp.Position((RectTransform)button.transform, 338f, 25f, 130f, 50f);
			this._tabLabel = button.GetComponentInChildren<TMP_Text>();
			button.interactable = this._completedPage != null;
			RectTransform rectTransform2 = OxGramUI.Rect("Run Missions", this._content);
			MissionsPhoneApp.Position(rectTransform2, 18f, 99f, 444f, 880f);
			rectTransform2.gameObject.SetActive(false);
			this._runPage = rectTransform2.gameObject.AddComponent<RunMissionsPanel>();
			this._runPage.Initialize(this._font);
			this.ShowCompleted(false);
			this.FitContent();
			Transform transform4 = base.transform.Find("APanel");
			if (transform4 != null)
			{
				transform4.SetAsLastSibling();
			}
		}

		// Token: 0x06007957 RID: 31063 RVA: 0x00318B10 File Offset: 0x00316D10
		public void OpenApp()
		{
			if (!PhoneAppAccessPolicy.TryOpen("Missions"))
			{
				return;
			}
			if (this._content == null)
			{
				return;
			}
			PhoneAppAutoHide component = base.GetComponent<PhoneAppAutoHide>();
			if (component != null && component.Pending)
			{
				base.gameObject.SetActive(false);
			}
			this.EnsureHomeUnderneath();
			base.gameObject.SetActive(true);
			this.ShowCompleted(false);
			this.FitContent();
			base.transform.SetAsLastSibling();
			if (this._animator != null && this._animator.isActiveAndEnabled && this._animator.runtimeAnimatorController != null)
			{
				this._animator.Play("BackIdleAnimation", -1, 0f);
			}
			if (!this._registered)
			{
				SmartphoneController.NotifyCustomAppOpened(new Action(this.CloseApp));
				this._registered = true;
			}
		}

		// Token: 0x06007958 RID: 31064 RVA: 0x00318BEC File Offset: 0x00316DEC
		public void CloseApp()
		{
			this.Unregister();
			if (this._island != null && this._animator != null && this._animator.runtimeAnimatorController != null)
			{
				this._island.UniversalBack(this._animator);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x00318C4C File Offset: 0x00316E4C
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.RefreshLocale;
			if (this._content != null)
			{
				this.ShowCompleted(false);
			}
		}

		// Token: 0x0600795A RID: 31066 RVA: 0x00318C74 File Offset: 0x00316E74
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocale;
			this.Unregister();
		}

		// Token: 0x0600795B RID: 31067 RVA: 0x00318C8D File Offset: 0x00316E8D
		private void RefreshLocale()
		{
			this.ShowCompleted(this._showingCompleted);
		}

		// Token: 0x0600795C RID: 31068 RVA: 0x00318C9B File Offset: 0x00316E9B
		private void Unregister()
		{
			if (!this._registered)
			{
				return;
			}
			SmartphoneController.NotifyCustomAppClosed();
			this._registered = false;
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x00318CB2 File Offset: 0x00316EB2
		private void LateUpdate()
		{
			if (this._content != null)
			{
				this.FitContent();
			}
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x00318CC8 File Offset: 0x00316EC8
		private void ToggleCompleted()
		{
			this.ShowCompleted(!this._showingCompleted);
		}

		// Token: 0x0600795F RID: 31071 RVA: 0x00318CDC File Offset: 0x00316EDC
		private void ShowCompleted(bool completed)
		{
			this._showingCompleted = completed && this._completedPage != null;
			if (this._completedPage != null)
			{
				this._completedPage.SetActive(this._showingCompleted);
			}
			this._runPage.gameObject.SetActive(!this._showingCompleted);
			MissionLocalization.Bind(this._title, this._showingCompleted ? "COMPLETED" : "MISSIONS", false);
			float num = (this._showingCompleted ? 22f : 28f);
			this._title.enableAutoSizing = GameLocalization.CurrentCode != "en";
			this._title.fontSizeMin = num * 0.68f;
			this._title.fontSizeMax = num;
			this._title.fontSize = num;
			MissionLocalization.Bind(this._tabLabel, this._showingCompleted ? "ACTIVE" : "COMPLETED", true);
		}

		// Token: 0x06007960 RID: 31072 RVA: 0x00318DD4 File Offset: 0x00316FD4
		private void FitContent()
		{
			Vector2 size = ((RectTransform)base.transform).rect.size;
			if (size == this._lastRootSize)
			{
				return;
			}
			this._lastRootSize = size;
			float num = ((size.x > 0f && size.y > 0f) ? Mathf.Min(size.x / 480f, size.y / 1014f) : 0.0053659766f);
			this._content.localScale = new Vector3(num, num, 1f);
		}

		// Token: 0x06007961 RID: 31073 RVA: 0x00318E66 File Offset: 0x00317066
		private TMP_Text Text(string name, Transform parent, string value, float size, Color color)
		{
			TMP_Text tmp_Text = OxGramUI.Text(name, parent, value, this._font, size, color, TextAlignmentOptions.Center);
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			tmp_Text.fontStyle = FontStyles.Bold;
			return tmp_Text;
		}

		// Token: 0x06007962 RID: 31074 RVA: 0x00318E94 File Offset: 0x00317094
		private Button Button(string name, string label, UnityAction onClick)
		{
			Image image = OxGramUI.Image(name, this._content, Color.clear, null);
			Button button = OxGramUI.Button(image.gameObject);
			TMP_Text tmp_Text = this.Text("Label", image.transform, label, 18f, MissionPanelStyle.Accent);
			OxGramUI.Stretch(tmp_Text.rectTransform, 0f, 0f, 0f, 0f);
			MissionLocalization.Bind(tmp_Text, label, true);
			button.onClick.AddListener(onClick);
			return button;
		}

		// Token: 0x06007963 RID: 31075 RVA: 0x00318F10 File Offset: 0x00317110
		private static void Position(RectTransform rect, float left, float top, float width, float height)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0f, 1f);
			rect.anchoredPosition = new Vector2(left, -top);
			rect.sizeDelta = new Vector2(width, height);
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x00318F70 File Offset: 0x00317170
		private void EnsureHomeUnderneath()
		{
			if (base.transform.parent == null)
			{
				return;
			}
			foreach (object obj in base.transform.parent)
			{
				Transform transform = (Transform)obj;
				if (!(transform == base.transform))
				{
					if (transform.name == "Home")
					{
						transform.gameObject.SetActive(true);
					}
					else if (transform.name.StartsWith("m", StringComparison.Ordinal) || transform.GetComponent<Animator>() != null || transform.GetComponent<PhotoGallery>() != null || transform.GetComponent<OxGramApp>() != null)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x04007CAD RID: 31917
		private RectTransform _content;

		// Token: 0x04007CAE RID: 31918
		private GameObject _completedPage;

		// Token: 0x04007CAF RID: 31919
		private RunMissionsPanel _runPage;

		// Token: 0x04007CB0 RID: 31920
		private TMP_Text _title;

		// Token: 0x04007CB1 RID: 31921
		private TMP_Text _tabLabel;

		// Token: 0x04007CB2 RID: 31922
		private TMP_FontAsset _font;

		// Token: 0x04007CB3 RID: 31923
		private Animator _animator;

		// Token: 0x04007CB4 RID: 31924
		private DynamicIslandController _island;

		// Token: 0x04007CB5 RID: 31925
		private Vector2 _lastRootSize;

		// Token: 0x04007CB6 RID: 31926
		private bool _showingCompleted;

		// Token: 0x04007CB7 RID: 31927
		private bool _registered;
	}
}
