using System;
using System.Collections;
using Features.CielOS.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E40 RID: 3648
	[DisallowMultipleComponent]
	public sealed class PhoneExperienceCoordinator : MonoBehaviour
	{
		// Token: 0x06006D07 RID: 27911 RVA: 0x002BBCD0 File Offset: 0x002B9ED0
		public static PhoneExperienceCoordinator Ensure(GameObject phoneRoot)
		{
			if (phoneRoot == null)
			{
				return null;
			}
			PhoneExperienceCoordinator component = phoneRoot.GetComponent<PhoneExperienceCoordinator>();
			if (!(component != null))
			{
				return phoneRoot.AddComponent<PhoneExperienceCoordinator>();
			}
			return component;
		}

		// Token: 0x06006D08 RID: 27912 RVA: 0x002BBD00 File Offset: 0x002B9F00
		private IEnumerator Start()
		{
			yield return null;
			this.ConfigureHome();
			yield break;
		}

		// Token: 0x06006D09 RID: 27913 RVA: 0x002BBD10 File Offset: 0x002B9F10
		private void ConfigureHome()
		{
			Transform transform = base.transform.Find("Canvas/Panel/Home/ICONS");
			if (transform != null)
			{
				Transform transform2 = transform.Find("Map");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(true);
				}
				Transform transform3 = transform.Find("Clock");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(false);
				}
				Transform transform4 = transform.Find("CielGram");
				if (transform4 != null)
				{
					Image component = transform4.GetComponent<Image>();
					if (component != null)
					{
						PhoneExperienceCoordinator.ApplyCielGramPhoneIcon(component);
					}
				}
				foreach (object obj in transform)
				{
					Transform transform5 = (Transform)obj;
					PhoneAppAccessGate.Bind(transform5, transform5.name);
				}
				PhoneHomeGridReorder component2 = transform.GetComponent<PhoneHomeGridReorder>();
				if (component2 != null)
				{
					component2.RefreshChildren();
				}
			}
			Transform transform6 = base.transform.Find("Canvas/Panel/Home/Footer");
			if (transform6 != null)
			{
				foreach (object obj2 in transform6)
				{
					Transform transform7 = (Transform)obj2;
					PhoneAppAccessGate.Bind(transform7, transform7.name);
				}
			}
			PhoneAppAccessGate.Bind(base.transform.Find("Canvas/Panel/Home/Widget 2"), "Missions");
			Transform transform8 = base.transform.Find("Canvas/Panel/mMap");
			if (transform8 != null)
			{
				transform8.gameObject.SetActive(false);
			}
			Transform transform9 = base.transform.Find("Canvas/Panel/mClock");
			if (transform9 != null)
			{
				transform9.gameObject.SetActive(false);
			}
			Transform transform10 = base.transform.Find("Canvas/Panel/mMusic/Panel/StopMusic");
			if (transform10 != null)
			{
				transform10.gameObject.SetActive(false);
			}
			PhonePrefabLocalization.Bind(base.transform);
		}

		// Token: 0x06006D0A RID: 27914 RVA: 0x002BBF20 File Offset: 0x002BA120
		private static void ApplyCielGramPhoneIcon(Image image)
		{
			if (PhoneExperienceCoordinator._cielGramPhoneIcon == null)
			{
				PhoneExperienceCoordinator._cielGramPhoneIcon = PhoneExperienceCoordinator.CropTransparentBadgeMargin(CielOSIconLibrary.GetBadgedSprite(CielOSIconId.Photos, 128));
			}
			image.sprite = PhoneExperienceCoordinator._cielGramPhoneIcon;
			image.type = Image.Type.Simple;
			image.preserveAspect = false;
			image.color = Color.white;
		}

		// Token: 0x06006D0B RID: 27915 RVA: 0x002BBF74 File Offset: 0x002BA174
		private static Sprite CropTransparentBadgeMargin(Sprite source)
		{
			if (source == null || source.texture == null)
			{
				return source;
			}
			Texture2D texture = source.texture;
			Color32[] pixels;
			try
			{
				pixels = texture.GetPixels32();
			}
			catch (UnityException)
			{
				return source;
			}
			Rect rect = source.rect;
			int num = Mathf.Max(0, Mathf.FloorToInt(rect.xMin));
			int num2 = Mathf.Max(0, Mathf.FloorToInt(rect.yMin));
			int num3 = Mathf.Min(texture.width - 1, Mathf.CeilToInt(rect.xMax) - 1);
			int num4 = Mathf.Min(texture.height - 1, Mathf.CeilToInt(rect.yMax) - 1);
			int num5 = num3;
			int num6 = num4;
			int num7 = num;
			int num8 = num2;
			bool flag = false;
			for (int i = num2; i <= num4; i++)
			{
				int num9 = i * texture.width;
				for (int j = num; j <= num3; j++)
				{
					if (pixels[num9 + j].a > 6)
					{
						flag = true;
						num5 = Mathf.Min(num5, j);
						num6 = Mathf.Min(num6, i);
						num7 = Mathf.Max(num7, j);
						num8 = Mathf.Max(num8, i);
					}
				}
			}
			if (!flag)
			{
				return source;
			}
			num5 = Mathf.Max(num, num5 - 1);
			num6 = Mathf.Max(num2, num6 - 1);
			num7 = Mathf.Min(num3, num7 + 1);
			num8 = Mathf.Min(num4, num8 + 1);
			Rect rect2 = new Rect((float)num5, (float)num6, (float)(num7 - num5 + 1), (float)(num8 - num6 + 1));
			Sprite sprite = Sprite.Create(texture, rect2, new Vector2(0.5f, 0.5f), source.pixelsPerUnit, 0U, SpriteMeshType.FullRect);
			sprite.name = "CielGramPhoneIcon";
			return sprite;
		}

		// Token: 0x04006FB9 RID: 28601
		private static Sprite _cielGramPhoneIcon;
	}
}
