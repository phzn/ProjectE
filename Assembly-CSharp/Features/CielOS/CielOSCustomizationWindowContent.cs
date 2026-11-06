using System;
using Features.CielOS.WindowManager;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x0200136C RID: 4972
	public class CielOSCustomizationWindowContent : MonoBehaviour
	{
		// Token: 0x060099F8 RID: 39416 RVA: 0x003F0D6C File Offset: 0x003EEF6C
		private void OnApplicationQuit()
		{
			this._quitting = true;
		}

		// Token: 0x060099F9 RID: 39417 RVA: 0x003F0D78 File Offset: 0x003EEF78
		private void Start()
		{
			this._takeover = Object.FindObjectOfType<CielOSCustomizationTakeover>(true);
			this._window = base.GetComponentInParent<CielOSWindow>();
			if (this._takeover == null)
			{
				Debug.LogError("[CustomizationWindow] Nessun CielOSCustomizationTakeover in scena.", this);
				this.CloseWindow();
				return;
			}
			if (this._window != null)
			{
				this._window.Closed += this.HandleWindowClosed;
				this.MaximizeWindow();
			}
			this._takeover.WindowCloseRequested += this.HandleCustomizationCloseRequested;
			this._opened = this._takeover.OpenWindowed((RectTransform)base.transform, this.stageView);
			if (!this._opened)
			{
				Debug.LogWarning("[CustomizationWindow] OpenWindowed fallito: chiudo la finestra.", this);
				this.CloseWindow();
			}
		}

		// Token: 0x060099FA RID: 39418 RVA: 0x003F0E3A File Offset: 0x003EF03A
		private void LateUpdate()
		{
			if (this._opened)
			{
				this._takeover.SyncWindowedLayout();
			}
		}

		// Token: 0x060099FB RID: 39419 RVA: 0x003F0E50 File Offset: 0x003EF050
		private void MaximizeWindow()
		{
			if (this._window == null || this._window.Rect == null)
			{
				return;
			}
			RectTransform rect = this._window.Rect;
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			CielOSWindowResizeHandle[] componentsInChildren = this._window.GetComponentsInChildren<CielOSWindowResizeHandle>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			CielOSWindowDragHandle[] componentsInChildren2 = this._window.GetComponentsInChildren<CielOSWindowDragHandle>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
		}

		// Token: 0x060099FC RID: 39420 RVA: 0x003F0F18 File Offset: 0x003EF118
		private void OnDestroy()
		{
			if (this._window != null)
			{
				this._window.Closed -= this.HandleWindowClosed;
			}
			if (this._takeover != null)
			{
				this._takeover.WindowCloseRequested -= this.HandleCustomizationCloseRequested;
			}
			this.ReleaseTakeover();
		}

		// Token: 0x060099FD RID: 39421 RVA: 0x003F0F75 File Offset: 0x003EF175
		private void HandleWindowClosed(CielOSWindow _)
		{
			this.ReleaseTakeover();
		}

		// Token: 0x060099FE RID: 39422 RVA: 0x003F0F7D File Offset: 0x003EF17D
		private void HandleCustomizationCloseRequested()
		{
			this.CloseWindow();
		}

		// Token: 0x060099FF RID: 39423 RVA: 0x003F0F85 File Offset: 0x003EF185
		private void ReleaseTakeover()
		{
			if (!this._opened)
			{
				return;
			}
			this._opened = false;
			if (this._quitting || !Application.isPlaying || this._takeover == null)
			{
				return;
			}
			this._takeover.CloseWindowed();
		}

		// Token: 0x06009A00 RID: 39424 RVA: 0x003F0FC0 File Offset: 0x003EF1C0
		private void CloseWindow()
		{
			if (this._window != null)
			{
				this._window.Close();
				return;
			}
			this.ReleaseTakeover();
			Object.Destroy(base.gameObject);
		}

		// Token: 0x04009EF6 RID: 40694
		[Tooltip("RawImage di sfondo che mostra la RenderTexture dello stage 3D.")]
		[SerializeField]
		private RawImage stageView;

		// Token: 0x04009EF7 RID: 40695
		private CielOSCustomizationTakeover _takeover;

		// Token: 0x04009EF8 RID: 40696
		private CielOSWindow _window;

		// Token: 0x04009EF9 RID: 40697
		private bool _opened;

		// Token: 0x04009EFA RID: 40698
		private bool _quitting;
	}
}
