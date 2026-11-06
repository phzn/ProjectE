using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x0200137D RID: 4989
	public class CielOSColorPicker : MonoBehaviour
	{
		// Token: 0x06009A84 RID: 39556 RVA: 0x003F3430 File Offset: 0x003F1630
		private void Awake()
		{
			this.EnsureBuilt();
		}

		// Token: 0x06009A85 RID: 39557 RVA: 0x003F3438 File Offset: 0x003F1638
		private void EnsureBuilt()
		{
			if (this._built)
			{
				return;
			}
			this._built = true;
			if (this.root != null)
			{
				foreach (TMP_Text tmp_Text in this.root.GetComponentsInChildren<TMP_Text>(true))
				{
					if (tmp_Text.text == "Edit Color" || tmp_Text.text == "OK" || tmp_Text.text == "Cancel")
					{
						CielOSContentText.Bind(tmp_Text, tmp_Text.text);
					}
				}
			}
			this.BuildHueTexture();
			this._svTex = new Texture2D(128, 128, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Bilinear
			};
			if (this.svImage != null)
			{
				this.svImage.texture = this._svTex;
			}
			this.HookDrag(this.svImage, new Action<PointerEventData>(this.OnSvPointer));
			this.HookDrag(this.hueImage, new Action<PointerEventData>(this.OnHuePointer));
			if (this.okButton != null)
			{
				this.okButton.onClick.AddListener(new UnityAction(this.Confirm));
			}
			if (this.cancelButton != null)
			{
				this.cancelButton.onClick.AddListener(new UnityAction(this.Cancel));
			}
		}

		// Token: 0x06009A86 RID: 39558 RVA: 0x003F358C File Offset: 0x003F178C
		public void Open(Color start, Action<Color> onChange)
		{
			this.EnsureBuilt();
			if (this.root != null)
			{
				this.root.SetActive(true);
				((RectTransform)this.root.transform).SetAsLastSibling();
			}
			this._startColor = start;
			this._onChange = onChange;
			Color.RGBToHSV(start, out this._h, out this._s, out this._v);
			this.RebuildSv();
			this.UpdateCursors();
			this.UpdatePreviewAndNotify();
		}

		// Token: 0x06009A87 RID: 39559 RVA: 0x003F3606 File Offset: 0x003F1806
		private void Confirm()
		{
			if (this.root != null)
			{
				this.root.SetActive(false);
			}
		}

		// Token: 0x06009A88 RID: 39560 RVA: 0x003F3622 File Offset: 0x003F1822
		private void Cancel()
		{
			Action<Color> onChange = this._onChange;
			if (onChange != null)
			{
				onChange(this._startColor);
			}
			if (this.root != null)
			{
				this.root.SetActive(false);
			}
		}

		// Token: 0x06009A89 RID: 39561 RVA: 0x003F3655 File Offset: 0x003F1855
		private void HookDrag(RawImage target, Action<PointerEventData> handler)
		{
			if (target == null)
			{
				return;
			}
			EventTrigger eventTrigger = target.gameObject.GetComponent<EventTrigger>() ?? target.gameObject.AddComponent<EventTrigger>();
			CielOSColorPicker.AddEntry(eventTrigger, EventTriggerType.PointerDown, handler);
			CielOSColorPicker.AddEntry(eventTrigger, EventTriggerType.Drag, handler);
		}

		// Token: 0x06009A8A RID: 39562 RVA: 0x003F368C File Offset: 0x003F188C
		private static void AddEntry(EventTrigger trig, EventTriggerType type, Action<PointerEventData> handler)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry
			{
				eventID = type
			};
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				handler((PointerEventData)data);
			});
			trig.triggers.Add(entry);
		}

		// Token: 0x06009A8B RID: 39563 RVA: 0x003F36D8 File Offset: 0x003F18D8
		private void OnSvPointer(PointerEventData e)
		{
			Vector2 vector;
			if (!CielOSColorPicker.LocalNorm((RectTransform)this.svImage.transform, e, out vector))
			{
				return;
			}
			this._s = Mathf.Clamp01(vector.x);
			this._v = Mathf.Clamp01(vector.y);
			this.UpdateCursors();
			this.UpdatePreviewAndNotify();
		}

		// Token: 0x06009A8C RID: 39564 RVA: 0x003F3730 File Offset: 0x003F1930
		private void OnHuePointer(PointerEventData e)
		{
			Vector2 vector;
			if (!CielOSColorPicker.LocalNorm((RectTransform)this.hueImage.transform, e, out vector))
			{
				return;
			}
			this._h = Mathf.Clamp01(vector.y);
			this.RebuildSv();
			this.UpdateCursors();
			this.UpdatePreviewAndNotify();
		}

		// Token: 0x06009A8D RID: 39565 RVA: 0x003F377C File Offset: 0x003F197C
		private static bool LocalNorm(RectTransform rt, PointerEventData e, out Vector2 norm)
		{
			norm = Vector2.zero;
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, e.position, e.pressEventCamera, out vector))
			{
				return false;
			}
			Rect rect = rt.rect;
			norm = new Vector2((vector.x - rect.x) / rect.width, (vector.y - rect.y) / rect.height);
			return true;
		}

		// Token: 0x06009A8E RID: 39566 RVA: 0x003F37EC File Offset: 0x003F19EC
		private void BuildHueTexture()
		{
			this._hueTex = new Texture2D(1, 128, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Bilinear
			};
			for (int i = 0; i < 128; i++)
			{
				this._hueTex.SetPixel(0, i, Color.HSVToRGB((float)i / 127f, 1f, 1f));
			}
			this._hueTex.Apply();
			if (this.hueImage != null)
			{
				this.hueImage.texture = this._hueTex;
			}
		}

		// Token: 0x06009A8F RID: 39567 RVA: 0x003F3874 File Offset: 0x003F1A74
		private void RebuildSv()
		{
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					this._svTex.SetPixel(j, i, Color.HSVToRGB(this._h, (float)j / 127f, (float)i / 127f));
				}
			}
			this._svTex.Apply();
		}

		// Token: 0x06009A90 RID: 39568 RVA: 0x003F38D4 File Offset: 0x003F1AD4
		private void UpdateCursors()
		{
			if (this.svCursor != null && this.svImage != null)
			{
				Rect rect = ((RectTransform)this.svImage.transform).rect;
				this.svCursor.anchoredPosition = new Vector2(rect.x + this._s * rect.width, rect.y + this._v * rect.height);
			}
			if (this.hueCursor != null && this.hueImage != null)
			{
				Rect rect2 = ((RectTransform)this.hueImage.transform).rect;
				this.hueCursor.anchoredPosition = new Vector2(0f, rect2.y + this._h * rect2.height);
			}
		}

		// Token: 0x06009A91 RID: 39569 RVA: 0x003F39AC File Offset: 0x003F1BAC
		private void UpdatePreviewAndNotify()
		{
			Color color = Color.HSVToRGB(this._h, this._s, this._v);
			color.a = 1f;
			if (this.preview != null)
			{
				this.preview.color = color;
			}
			Action<Color> onChange = this._onChange;
			if (onChange == null)
			{
				return;
			}
			onChange(color);
		}

		// Token: 0x04009F50 RID: 40784
		[Header("Root")]
		[SerializeField]
		private GameObject root;

		// Token: 0x04009F51 RID: 40785
		[Header("SV square")]
		[SerializeField]
		private RawImage svImage;

		// Token: 0x04009F52 RID: 40786
		[SerializeField]
		private RectTransform svCursor;

		// Token: 0x04009F53 RID: 40787
		[Header("Hue bar")]
		[SerializeField]
		private RawImage hueImage;

		// Token: 0x04009F54 RID: 40788
		[SerializeField]
		private RectTransform hueCursor;

		// Token: 0x04009F55 RID: 40789
		[Header("Preview / buttons")]
		[SerializeField]
		private Image preview;

		// Token: 0x04009F56 RID: 40790
		[SerializeField]
		private Button okButton;

		// Token: 0x04009F57 RID: 40791
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04009F58 RID: 40792
		private const int SvSize = 128;

		// Token: 0x04009F59 RID: 40793
		private const int HueSize = 128;

		// Token: 0x04009F5A RID: 40794
		private Texture2D _svTex;

		// Token: 0x04009F5B RID: 40795
		private Texture2D _hueTex;

		// Token: 0x04009F5C RID: 40796
		private float _h;

		// Token: 0x04009F5D RID: 40797
		private float _s;

		// Token: 0x04009F5E RID: 40798
		private float _v;

		// Token: 0x04009F5F RID: 40799
		private Action<Color> _onChange;

		// Token: 0x04009F60 RID: 40800
		private Color _startColor;

		// Token: 0x04009F61 RID: 40801
		private bool _built;
	}
}
