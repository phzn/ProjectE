using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000139 RID: 313
[RequireComponent(typeof(Text))]
public class vScorePointDisplay : MonoBehaviour
{
	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00046750 File Offset: 0x00044950
	public Text display
	{
		get
		{
			if (this.withOutDisplay)
			{
				return null;
			}
			if (this._display == null && !this.withOutDisplay)
			{
				this._display = base.GetComponent<Text>();
			}
			if (this._display == null)
			{
				this.withOutDisplay = true;
			}
			return this._display;
		}
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x000467A4 File Offset: 0x000449A4
	public void ShowValue(float value)
	{
		if (string.IsNullOrEmpty(this.stringFormat))
		{
			this.stringFormat = "{0}";
		}
		string text = string.Format(this.stringFormat, value.ToString());
		if (this.display)
		{
			this.display.text = text;
		}
		this.onChangeDisplayText.Invoke(text);
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00046801 File Offset: 0x00044A01
	public void ShowValue(int value)
	{
		this.ShowValue((float)value);
	}

	// Token: 0x0400096E RID: 2414
	[SerializeField]
	protected Text _display;

	// Token: 0x0400096F RID: 2415
	public string stringFormat;

	// Token: 0x04000970 RID: 2416
	public InputField.OnChangeEvent onChangeDisplayText;

	// Token: 0x04000971 RID: 2417
	private bool withOutDisplay;

	// Token: 0x04000972 RID: 2418
	private const string StringFormatDefault = "{0}";
}
