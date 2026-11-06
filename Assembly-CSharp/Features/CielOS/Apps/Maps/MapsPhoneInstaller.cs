using System;
using Features.CielOS.UI;
using Features.Localization;
using Features.PhoneCompanion;
using Features.RunModes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x02001481 RID: 5249
	[DisallowMultipleComponent]
	public sealed class MapsPhoneInstaller : MonoBehaviour
	{
		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x0600A1CC RID: 41420 RVA: 0x004332F0 File Offset: 0x004314F0
		public static bool IsMapUnlocked
		{
			get
			{
				return RunModeService.IsLongWayHome;
			}
		}

		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x0600A1CD RID: 41421 RVA: 0x004332F7 File Offset: 0x004314F7
		public static bool IsAvailable
		{
			get
			{
				return MapsPhoneInstaller.IsMapUnlocked;
			}
		}

		// Token: 0x0600A1CE RID: 41422 RVA: 0x00433300 File Offset: 0x00431500
		public static void Ensure(GameObject phone, SmartphoneController owner)
		{
			if (phone == null || phone.GetComponentInChildren<MapsPhoneApp>(true) != null)
			{
				return;
			}
			Transform transform = phone.transform.Find("Canvas/Panel");
			Transform transform2 = ((transform != null) ? transform.Find("Home") : null);
			Transform transform3 = ((transform2 != null) ? transform2.Find("ICONS/Map") : null);
			if (transform == null || transform3 == null)
			{
				Debug.LogWarning("[Maps] Native phone Map icon was not found.", phone);
				return;
			}
			TMP_Text componentInChildren = transform3.GetComponentInChildren<TMP_Text>(true);
			TMP_FontAsset tmp_FontAsset = ((componentInChildren != null) ? LocalizedGameText.SourceFont(componentInChildren) : TMP_Settings.defaultFontAsset);
			GameObject gameObject = new GameObject("MapsPanel", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(Mask),
				typeof(Animator)
			});
			gameObject.layer = transform.gameObject.layer;
			gameObject.SetActive(false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(transform, false);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = new Vector2(2.0443f, 4.3207f);
			rectTransform.anchoredPosition3D = Vector3.zero;
			rectTransform.localScale = Vector3.one * 0.20591347f;
			gameObject.GetComponent<Image>().color = new Color(0.035f, 0.06f, 0.09f);
			gameObject.GetComponent<Mask>().showMaskGraphic = true;
			Animator component = gameObject.GetComponent<Animator>();
			foreach (object obj in transform)
			{
				Transform transform4 = (Transform)obj;
				if (!(transform4 == rectTransform))
				{
					Animator component2 = transform4.GetComponent<Animator>();
					if (!(component2 == null) && !(component2.runtimeAnimatorController == null))
					{
						component.runtimeAnimatorController = component2.runtimeAnimatorController;
						break;
					}
				}
			}
			MapsPhoneApp mapsPhoneApp = gameObject.AddComponent<MapsPhoneApp>();
			mapsPhoneApp.Initialize(owner, tmp_FontAsset);
			Button button = transform3.GetComponent<Button>();
			if (button == null)
			{
				button = transform3.gameObject.AddComponent<Button>();
			}
			button.onClick = new Button.ButtonClickedEvent();
			button.onClick.AddListener(new UnityAction(mapsPhoneApp.OpenApp));
			Image component3 = transform3.GetComponent<Image>();
			if (component3 != null)
			{
				component3.sprite = CielOSIconLibrary.GetBadgedSprite(CielOSIconId.Maps, 128);
				component3.type = Image.Type.Simple;
				button.targetGraphic = component3;
			}
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(componentInChildren, "maps_ui", "Maps", null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			MapsPhoneInstaller mapsPhoneInstaller = transform3.GetComponent<MapsPhoneInstaller>();
			if (mapsPhoneInstaller == null)
			{
				mapsPhoneInstaller = transform3.gameObject.AddComponent<MapsPhoneInstaller>();
			}
			mapsPhoneInstaller._button = button;
			mapsPhoneInstaller._image = component3;
			transform3.gameObject.SetActive(true);
			mapsPhoneInstaller.RefreshAvailability();
			PhoneHomeGridReorder componentInParent = transform3.GetComponentInParent<PhoneHomeGridReorder>();
			if (componentInParent != null)
			{
				componentInParent.RefreshChildren();
			}
			if (componentInParent != null)
			{
				componentInParent.ApplySavedOrder();
			}
			Transform transform5 = transform.Find("mMap");
			if (transform5 != null)
			{
				transform5.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600A1CF RID: 41423 RVA: 0x00433664 File Offset: 0x00431864
		private void OnEnable()
		{
			RunModeService.Changed += this.RefreshAvailability;
			this.RefreshAvailability();
		}

		// Token: 0x0600A1D0 RID: 41424 RVA: 0x0043367D File Offset: 0x0043187D
		private void OnDisable()
		{
			RunModeService.Changed -= this.RefreshAvailability;
		}

		// Token: 0x0600A1D1 RID: 41425 RVA: 0x00433690 File Offset: 0x00431890
		private void OnDestroy()
		{
			if (this._phoneIcon == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._phoneIcon);
				return;
			}
			Object.DestroyImmediate(this._phoneIcon);
		}

		// Token: 0x0600A1D2 RID: 41426 RVA: 0x004336BF File Offset: 0x004318BF
		private void RefreshAvailability()
		{
			this.ApplyIconPresentation();
			if (this._button != null)
			{
				PhoneAppAccessGate.Bind(base.transform, "Map");
			}
		}

		// Token: 0x0600A1D3 RID: 41427 RVA: 0x004336E8 File Offset: 0x004318E8
		private void ApplyIconPresentation()
		{
			if (this._image == null)
			{
				return;
			}
			Sprite badgedSprite = CielOSIconLibrary.GetBadgedSprite(CielOSIconId.Maps, 128);
			if (this._phoneIcon == null)
			{
				this._phoneIcon = MapsPhoneInstaller.CreatePhoneIcon(badgedSprite);
			}
			this._image.sprite = this._phoneIcon;
		}

		// Token: 0x0600A1D4 RID: 41428 RVA: 0x0043373C File Offset: 0x0043193C
		private static Sprite CreatePhoneIcon(Sprite source)
		{
			Texture2D texture = source.texture;
			Color32[] pixels = texture.GetPixels32();
			Rect rect = source.rect;
			int num = Mathf.FloorToInt(rect.xMin);
			int num2 = Mathf.FloorToInt(rect.yMin);
			int num3 = Mathf.CeilToInt(rect.xMax) - 1;
			int num4 = Mathf.CeilToInt(rect.yMax) - 1;
			int num5 = num3;
			int num6 = num4;
			int num7 = num;
			int num8 = num2;
			for (int i = num2; i <= num4; i++)
			{
				for (int j = num; j <= num3; j++)
				{
					if (pixels[i * texture.width + j].a > 6)
					{
						num5 = Mathf.Min(num5, j);
						num6 = Mathf.Min(num6, i);
						num7 = Mathf.Max(num7, j);
						num8 = Mathf.Max(num8, i);
					}
				}
			}
			num5 = Mathf.Max(num, num5 - 1);
			num6 = Mathf.Max(num2, num6 - 1);
			num7 = Mathf.Min(num3, num7 + 1);
			num8 = Mathf.Min(num4, num8 + 1);
			Sprite sprite = Sprite.Create(texture, new Rect((float)num5, (float)num6, (float)(num7 - num5 + 1), (float)(num8 - num6 + 1)), new Vector2(0.5f, 0.5f), source.pixelsPerUnit, 0U, SpriteMeshType.FullRect);
			sprite.name = "LongWayHomeMapsPhoneIcon";
			return sprite;
		}

		// Token: 0x0400A576 RID: 42358
		private Button _button;

		// Token: 0x0400A577 RID: 42359
		private Image _image;

		// Token: 0x0400A578 RID: 42360
		private Sprite _phoneIcon;
	}
}
