using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x020000FA RID: 250
	public class MMDebugOnScreenConsole : MonoBehaviour
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0001ABAC File Offset: 0x00018DAC
		public virtual void Toggle()
		{
			if (this.ConsoleText.enabled)
			{
				this._openBackgroundWidth = this.BackgroundImage.rectTransform.sizeDelta;
				this.BackgroundImage.rectTransform.sizeDelta = this._closedSize;
			}
			else
			{
				this.BackgroundImage.rectTransform.sizeDelta = this._openBackgroundWidth;
			}
			this.ConsoleText.enabled = !this.ConsoleText.isActiveAndEnabled;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001AC23 File Offset: 0x00018E23
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001AC2B File Offset: 0x00018E2B
		protected virtual void Initialization()
		{
			this.ConsoleText.color = this.LabelColor;
			this._stringBuilder = new StringBuilder();
			this._rectTransform = base.gameObject.GetComponent<RectTransform>();
			this._valueTagEnd = "</size></color>";
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001AC68 File Offset: 0x00018E68
		protected virtual void SetFontSize(int fontSize)
		{
			if (fontSize == this.ConsoleText.fontSize)
			{
				return;
			}
			this.ConsoleText.fontSize = fontSize;
			this._valueTagStart = string.Concat(new string[]
			{
				"<color=",
				this.ValueColor,
				"><size=",
				((float)this.ConsoleText.fontSize * this.ValueSizeRatio).ToString(),
				">"
			});
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001ACE0 File Offset: 0x00018EE0
		protected virtual void LateUpdate()
		{
			this._messageStackHasBeenDisplayed = true;
			if (!this._newMessageThisFrame && this.ConsoleText.isActiveAndEnabled)
			{
				base.gameObject.SetActive(false);
			}
			this._newMessageThisFrame = false;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001AD11 File Offset: 0x00018F11
		public virtual void SetScreenOffset(int top = 10, int left = 10)
		{
			this.Container.MMSetTop((float)top);
			this.Container.MMSetLeft((float)left);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001AD2D File Offset: 0x00018F2D
		public virtual void SetMessage(string newMessage)
		{
			this.AddMessage(newMessage, "", 30);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001AD40 File Offset: 0x00018F40
		public virtual void AddMessage(string label, object value, int fontSize)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(true);
			}
			int frameCount = Time.frameCount;
			if (!this.ConsoleText.isActiveAndEnabled)
			{
				return;
			}
			this._newMessageThisFrame = true;
			this.SetFontSize(fontSize);
			if (this._last_append_at_frame != frameCount)
			{
				this._stringBuilder.Clear();
				this._messageStackHasBeenDisplayed = false;
				this._numberOfMessages = 0;
				this._largestMessageLength = 0;
			}
			this._last_append_at_frame = Time.frameCount;
			if (this._stringBuilder.Length != 0)
			{
				this._stringBuilder.Append(Environment.NewLine);
			}
			this._stringBuilder.Append(label.ToUpper());
			this._stringBuilder.Append(" ");
			this._stringBuilder.Append(this._valueTagStart);
			this._stringBuilder.Append(value);
			this._stringBuilder.Append(this._valueTagEnd);
			if (label.Length > this._largestMessageLength)
			{
				this._largestMessageLength = label.Length;
			}
			this._numberOfMessages++;
			this.ConsoleText.text = this._stringBuilder.ToString();
		}

		// Token: 0x040006DF RID: 1759
		[Header("Bindings")]
		public RectTransform Container;

		// Token: 0x040006E0 RID: 1760
		public Image BackgroundImage;

		// Token: 0x040006E1 RID: 1761
		public Text ConsoleText;

		// Token: 0x040006E2 RID: 1762
		[Header("Label")]
		public Color LabelColor = Color.white;

		// Token: 0x040006E3 RID: 1763
		[Header("Value")]
		public string ValueColor = "#FFC400";

		// Token: 0x040006E4 RID: 1764
		public float ValueSizeRatio = 1.35f;

		// Token: 0x040006E5 RID: 1765
		protected RectTransform _rectTransform;

		// Token: 0x040006E6 RID: 1766
		protected int _numberOfMessages;

		// Token: 0x040006E7 RID: 1767
		protected bool _messageStackHasBeenDisplayed;

		// Token: 0x040006E8 RID: 1768
		protected bool _newMessageThisFrame;

		// Token: 0x040006E9 RID: 1769
		protected int _largestMessageLength;

		// Token: 0x040006EA RID: 1770
		protected StringBuilder _stringBuilder;

		// Token: 0x040006EB RID: 1771
		protected string _valueTagStart;

		// Token: 0x040006EC RID: 1772
		protected string _valueTagEnd;

		// Token: 0x040006ED RID: 1773
		protected const string space = " ";

		// Token: 0x040006EE RID: 1774
		protected Vector2 _closedSize = new Vector2(60f, 80f);

		// Token: 0x040006EF RID: 1775
		protected Vector2 _openBackgroundWidth;

		// Token: 0x040006F0 RID: 1776
		protected int _last_append_at_frame = -1;
	}
}
