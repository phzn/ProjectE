using System;
using System.Collections.Generic;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013D8 RID: 5080
	public class CielOSDock : MonoBehaviour
	{
		// Token: 0x06009CE3 RID: 40163 RVA: 0x0040629C File Offset: 0x0040449C
		private void Start()
		{
			this.Build();
		}

		// Token: 0x06009CE4 RID: 40164 RVA: 0x004062A4 File Offset: 0x004044A4
		public void RefreshAvailability()
		{
			if (this.buttonParent != null)
			{
				for (int i = this.buttonParent.childCount - 1; i >= 0; i--)
				{
					GameObject gameObject = this.buttonParent.GetChild(i).gameObject;
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
			this._indicatorByApp.Clear();
			this._appIdByTitle.Clear();
			this._openCountByApp.Clear();
			this._built = false;
			this.Build();
			if (this.windowManager != null)
			{
				foreach (CielOSWindow cielOSWindow in this.windowManager.Windows)
				{
					this.OnWindowOpened(cielOSWindow);
				}
			}
		}

		// Token: 0x06009CE5 RID: 40165 RVA: 0x00406388 File Offset: 0x00404588
		private void OnEnable()
		{
			if (this.windowManager != null)
			{
				this.windowManager.WindowOpened += this.OnWindowOpened;
				this.windowManager.WindowClosed += this.OnWindowClosed;
			}
		}

		// Token: 0x06009CE6 RID: 40166 RVA: 0x004063C6 File Offset: 0x004045C6
		private void OnDisable()
		{
			if (this.windowManager != null)
			{
				this.windowManager.WindowOpened -= this.OnWindowOpened;
				this.windowManager.WindowClosed -= this.OnWindowClosed;
			}
		}

		// Token: 0x06009CE7 RID: 40167 RVA: 0x00406404 File Offset: 0x00404604
		private void Build()
		{
			if (this._built || this.catalog == null || this.buttonParent == null)
			{
				return;
			}
			this._built = true;
			Image component = base.GetComponent<Image>();
			if (component != null)
			{
				component.color = new Color(0f, 0f, 0f, 0f);
				component.sprite = null;
				component.raycastTarget = false;
			}
			Transform transform = base.transform.Find("ThemeBorder");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			foreach (CielOSAppDefinition cielOSAppDefinition in this.catalog.apps)
			{
				if (!(cielOSAppDefinition == null) && !cielOSAppDefinition.hidden && cielOSAppDefinition.IsVisible && !cielOSAppDefinition.isCustomApp)
				{
					string id = cielOSAppDefinition.appId;
					CielOSTileFactory.Tile tile = CielOSTileFactory.BuildAppTile(this.buttonParent, id, cielOSAppDefinition.displayName, 36f, false, true, false, !cielOSAppDefinition.IsPlatformSupported, false);
					LayoutElement component2 = tile.Root.GetComponent<LayoutElement>();
					if (component2 != null)
					{
						component2.minHeight = 52f;
						component2.preferredHeight = 52f;
					}
					tile.Button.onClick.AddListener(delegate
					{
						if (this.windowManager != null)
						{
							this.windowManager.LaunchApp(id);
						}
					});
					if (tile.Indicator != null)
					{
						this._indicatorByApp[id] = tile.Indicator.gameObject;
					}
					if (!string.IsNullOrEmpty(cielOSAppDefinition.displayName))
					{
						this._appIdByTitle[cielOSAppDefinition.displayName] = id;
					}
				}
			}
		}

		// Token: 0x06009CE8 RID: 40168 RVA: 0x0040660C File Offset: 0x0040480C
		private void OnWindowOpened(CielOSWindow window)
		{
			string text = this.ResolveAppId(window);
			if (text == null)
			{
				return;
			}
			int num;
			this._openCountByApp.TryGetValue(text, out num);
			this._openCountByApp[text] = num + 1;
			this.SetIndicator(text, true);
		}

		// Token: 0x06009CE9 RID: 40169 RVA: 0x0040664C File Offset: 0x0040484C
		private void OnWindowClosed(CielOSWindow window)
		{
			string text = this.ResolveAppId(window);
			if (text == null)
			{
				return;
			}
			int num;
			if (this._openCountByApp.TryGetValue(text, out num))
			{
				num = Mathf.Max(0, num - 1);
				this._openCountByApp[text] = num;
				if (num == 0)
				{
					this.SetIndicator(text, false);
				}
			}
		}

		// Token: 0x06009CEA RID: 40170 RVA: 0x00406698 File Offset: 0x00404898
		private string ResolveAppId(CielOSWindow window)
		{
			if (window == null)
			{
				return null;
			}
			string text;
			if (!this._appIdByTitle.TryGetValue(window.WindowTitle ?? string.Empty, out text))
			{
				return null;
			}
			return text;
		}

		// Token: 0x06009CEB RID: 40171 RVA: 0x004066D4 File Offset: 0x004048D4
		private void SetIndicator(string appId, bool active)
		{
			GameObject gameObject;
			if (this._indicatorByApp.TryGetValue(appId, out gameObject) && gameObject != null)
			{
				gameObject.SetActive(active);
			}
		}

		// Token: 0x0400A16F RID: 41327
		[SerializeField]
		private CielOSAppCatalogSO catalog;

		// Token: 0x0400A170 RID: 41328
		[SerializeField]
		private CielOSWindowManager windowManager;

		// Token: 0x0400A171 RID: 41329
		[Tooltip("Parent (VerticalLayoutGroup) for dock buttons.")]
		[SerializeField]
		private RectTransform buttonParent;

		// Token: 0x0400A172 RID: 41330
		private readonly Dictionary<string, GameObject> _indicatorByApp = new Dictionary<string, GameObject>();

		// Token: 0x0400A173 RID: 41331
		private readonly Dictionary<string, string> _appIdByTitle = new Dictionary<string, string>();

		// Token: 0x0400A174 RID: 41332
		private readonly Dictionary<string, int> _openCountByApp = new Dictionary<string, int>();

		// Token: 0x0400A175 RID: 41333
		private bool _built;
	}
}
