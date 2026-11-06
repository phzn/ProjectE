using System;
using Features.CensorBars;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x02001390 RID: 5008
	[DefaultExecutionOrder(-800)]
	public class CielOSScreensaver : MonoBehaviour
	{
		// Token: 0x06009AFC RID: 39676 RVA: 0x003F6608 File Offset: 0x003F4808
		private void Awake()
		{
			if (this.controller == null)
			{
				this.controller = base.GetComponentInParent<CielOSController>();
			}
			this._cg = ((this.root != null) ? this.root.GetComponent<CanvasGroup>() : null);
			if (this.root != null && this._cg == null)
			{
				this._cg = this.root.AddComponent<CanvasGroup>();
			}
			this.Dismiss();
			this._lastMouse = Input.mousePosition;
		}

		// Token: 0x06009AFD RID: 39677 RVA: 0x003F668F File Offset: 0x003F488F
		private void OnEnable()
		{
			CielOSWallpaperService.ScreensaverChanged += this.RefreshSelectedImage;
		}

		// Token: 0x06009AFE RID: 39678 RVA: 0x003F66A2 File Offset: 0x003F48A2
		private void OnDisable()
		{
			CielOSWallpaperService.ScreensaverChanged -= this.RefreshSelectedImage;
			this.Dismiss();
		}

		// Token: 0x06009AFF RID: 39679 RVA: 0x003F66A2 File Offset: 0x003F48A2
		private void OnDestroy()
		{
			CielOSWallpaperService.ScreensaverChanged -= this.RefreshSelectedImage;
			this.Dismiss();
		}

		// Token: 0x06009B00 RID: 39680 RVA: 0x003F66BC File Offset: 0x003F48BC
		private void Update()
		{
			if (!(this.controller == null) && !this.controller.IsOpen)
			{
				if (this._showing)
				{
					this.Dismiss();
				}
				this._idleTimer = 0f;
				return;
			}
			bool flag = CielOSScreensaver.HasControllerActivity();
			bool flag2 = flag || ControllerTextKeyboard.IsOpen || (Input.mousePosition - this._lastMouse).sqrMagnitude > 4f || Input.anyKeyDown || Input.GetAxis("Mouse ScrollWheel") != 0f;
			this._lastMouse = Input.mousePosition;
			if (this._showing)
			{
				if (this._cg != null && this._cg.alpha < 1f)
				{
					this._cg.alpha = Mathf.MoveTowards(this._cg.alpha, 1f, Time.unscaledDeltaTime / Mathf.Max(0.01f, this.fadeDuration));
				}
				if (flag2)
				{
					if (flag)
					{
						ControllerInput.ConsumeUIInput();
					}
					this.Dismiss();
				}
				return;
			}
			if (flag2)
			{
				this._idleTimer = 0f;
				return;
			}
			this._idleTimer += Time.unscaledDeltaTime;
			if (this._idleTimer >= this.idleSeconds)
			{
				this.Show();
			}
		}

		// Token: 0x06009B01 RID: 39681 RVA: 0x003F6800 File Offset: 0x003F4A00
		private static bool HasControllerActivity()
		{
			if (!ControllerInput.HasGamepad)
			{
				return false;
			}
			if (ControllerInput.Move.sqrMagnitude > 0.09f || ControllerInput.Look.sqrMagnitude > 0.09f || ControllerInput.Navigate.sqrMagnitude > 0.09f)
			{
				return true;
			}
			for (int i = 0; i <= 15; i++)
			{
				if (ControllerInput.GetButton((ControllerButton)i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009B02 RID: 39682 RVA: 0x003F686C File Offset: 0x003F4A6C
		private void Show()
		{
			if (!this.LoadSelectedImage())
			{
				this.Dismiss();
				return;
			}
			if (this._cg != null)
			{
				this._cg.alpha = 0f;
			}
			if (this.root != null)
			{
				this.root.SetActive(true);
				((RectTransform)this.root.transform).SetAsLastSibling();
			}
			this._showing = true;
		}

		// Token: 0x06009B03 RID: 39683 RVA: 0x003F68DC File Offset: 0x003F4ADC
		private void RefreshSelectedImage()
		{
			if (!this._showing)
			{
				this.ReleaseImage();
				return;
			}
			if (!this.LoadSelectedImage())
			{
				this.Dismiss();
			}
		}

		// Token: 0x06009B04 RID: 39684 RVA: 0x003F68FC File Offset: 0x003F4AFC
		private bool LoadSelectedImage()
		{
			this.ReleaseImage();
			if (this.image == null)
			{
				return false;
			}
			CielOSWallpaperService instance = CielOSWallpaperService.Instance;
			if (instance == null || string.IsNullOrEmpty(instance.ScreensaverPath))
			{
				return false;
			}
			Sprite sprite = instance.LoadScreensaverSprite(0);
			if (sprite == null)
			{
				return false;
			}
			this._runtimeSprite = sprite;
			this._runtimeTexture = sprite.texture;
			this.image.sprite = sprite;
			StreamerCensorImage.Bind(this.image, false);
			this.image.preserveAspect = true;
			this.image.color = Color.white;
			return true;
		}

		// Token: 0x06009B05 RID: 39685 RVA: 0x003F6998 File Offset: 0x003F4B98
		private void Dismiss()
		{
			this._showing = false;
			this._idleTimer = 0f;
			this.ReleaseImage();
			if (this._cg != null)
			{
				this._cg.alpha = 0f;
			}
			if (this.root != null)
			{
				this.root.SetActive(false);
			}
		}

		// Token: 0x06009B06 RID: 39686 RVA: 0x003F69F8 File Offset: 0x003F4BF8
		private void ReleaseImage()
		{
			if (this.image != null)
			{
				this.image.overrideSprite = null;
				this.image.sprite = null;
			}
			CielOSScreensaver.DestroyRuntimeObject(this._runtimeSprite);
			CielOSScreensaver.DestroyRuntimeObject(this._runtimeTexture);
			this._runtimeSprite = null;
			this._runtimeTexture = null;
		}

		// Token: 0x06009B07 RID: 39687 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyRuntimeObject(Object target)
		{
			if (target == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x04009FEE RID: 40942
		[SerializeField]
		private GameObject root;

		// Token: 0x04009FEF RID: 40943
		[SerializeField]
		private Image image;

		// Token: 0x04009FF0 RID: 40944
		[SerializeField]
		private CielOSController controller;

		// Token: 0x04009FF1 RID: 40945
		[Tooltip("Idle time (seconds) before the screensaver appears.")]
		[SerializeField]
		private float idleSeconds = 30f;

		// Token: 0x04009FF2 RID: 40946
		[Tooltip("Fade-in duration.")]
		[SerializeField]
		private float fadeDuration = 1.5f;

		// Token: 0x04009FF3 RID: 40947
		private float _idleTimer;

		// Token: 0x04009FF4 RID: 40948
		private Vector3 _lastMouse;

		// Token: 0x04009FF5 RID: 40949
		private bool _showing;

		// Token: 0x04009FF6 RID: 40950
		private CanvasGroup _cg;

		// Token: 0x04009FF7 RID: 40951
		private Sprite _runtimeSprite;

		// Token: 0x04009FF8 RID: 40952
		private Texture2D _runtimeTexture;
	}
}
