using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000006 RID: 6
public class BirthdayInput : MonoBehaviour
{
	// Token: 0x0600001C RID: 28 RVA: 0x00002458 File Offset: 0x00000658
	private void Start()
	{
		this.SetupInputFields();
		this.SetupEventListeners();
		this.HideWarning();
		if (!this.hasAssignedBirthday && string.IsNullOrEmpty(this.yearInput.text) && string.IsNullOrEmpty(this.monthInput.text) && string.IsNullOrEmpty(this.dayInput.text))
		{
			this.SetDefaultBirthday();
			return;
		}
		this.ValidateInput();
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000024C4 File Offset: 0x000006C4
	private void SetDefaultBirthday()
	{
		string text = DateTime.Today.AddYears(-21).ToString("yyyyMMdd");
		this.SetBirthday(text);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000024F8 File Offset: 0x000006F8
	private void SetupInputFields()
	{
		this.yearInput.contentType = TMP_InputField.ContentType.IntegerNumber;
		this.yearInput.characterLimit = 4;
		this.monthInput.contentType = TMP_InputField.ContentType.IntegerNumber;
		this.monthInput.characterLimit = 2;
		this.dayInput.contentType = TMP_InputField.ContentType.IntegerNumber;
		this.dayInput.characterLimit = 2;
		this.SetPlaceholder(this.yearInput, "YYYY");
		this.SetPlaceholder(this.monthInput, "MM");
		this.SetPlaceholder(this.dayInput, "DD");
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002580 File Offset: 0x00000780
	private void SetPlaceholder(TMP_InputField field, string text)
	{
		if (field.placeholder != null)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource((TMP_Text)field.placeholder, "cielos_apps", text, null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000025B4 File Offset: 0x000007B4
	private void SetupEventListeners()
	{
		this.yearInput.onValueChanged.AddListener(new UnityAction<string>(this.OnYearChanged));
		this.monthInput.onValueChanged.AddListener(new UnityAction<string>(this.OnMonthChanged));
		this.dayInput.onValueChanged.AddListener(new UnityAction<string>(this.OnDayChanged));
		this.yearInput.onEndEdit.AddListener(new UnityAction<string>(this.OnYearComplete));
		this.monthInput.onEndEdit.AddListener(new UnityAction<string>(this.OnMonthComplete));
		this.dayInput.onEndEdit.AddListener(new UnityAction<string>(this.OnDayComplete));
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002669 File Offset: 0x00000869
	private void OnYearChanged(string value)
	{
		if (value.Length == 4)
		{
			this.ValidateYear(value);
			this.monthInput.Select();
		}
		else
		{
			this.ValidateInput();
		}
		Action<string> onBirthdayChanged = this.OnBirthdayChanged;
		if (onBirthdayChanged == null)
		{
			return;
		}
		onBirthdayChanged(this.GetBirthdayOrDefault());
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000026A4 File Offset: 0x000008A4
	private void OnMonthChanged(string value)
	{
		int num;
		if (value.Length == 2)
		{
			this.ValidateMonth(value);
			this.dayInput.Select();
		}
		else if (value.Length == 1 && int.TryParse(value, out num) && num > 1)
		{
			this.monthInput.text = "0" + value;
			this.dayInput.Select();
		}
		this.ValidateInput();
		Action<string> onBirthdayChanged = this.OnBirthdayChanged;
		if (onBirthdayChanged == null)
		{
			return;
		}
		onBirthdayChanged(this.GetBirthdayOrDefault());
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002724 File Offset: 0x00000924
	private void OnDayChanged(string value)
	{
		int num;
		if (value.Length == 2)
		{
			this.ValidateDay(value);
		}
		else if (value.Length == 1 && int.TryParse(value, out num) && num > 3)
		{
			this.dayInput.text = "0" + value;
		}
		this.ValidateInput();
		Action<string> onBirthdayChanged = this.OnBirthdayChanged;
		if (onBirthdayChanged == null)
		{
			return;
		}
		onBirthdayChanged(this.GetBirthdayOrDefault());
	}

	// Token: 0x06000024 RID: 36 RVA: 0x0000278C File Offset: 0x0000098C
	private void OnYearComplete(string value)
	{
		if (value.Length < 4)
		{
			this.yearInput.text = value.PadLeft(4, '0');
		}
		this.ValidateInput();
		Action<string> onBirthdayChanged = this.OnBirthdayChanged;
		if (onBirthdayChanged == null)
		{
			return;
		}
		onBirthdayChanged(this.GetBirthdayOrDefault());
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000027C7 File Offset: 0x000009C7
	private void OnMonthComplete(string value)
	{
		if (value.Length == 1)
		{
			this.monthInput.text = "0" + value;
		}
		this.ValidateInput();
		Action<string> onBirthdayChanged = this.OnBirthdayChanged;
		if (onBirthdayChanged == null)
		{
			return;
		}
		onBirthdayChanged(this.GetBirthdayOrDefault());
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002804 File Offset: 0x00000A04
	private void OnDayComplete(string value)
	{
		if (value.Length == 1)
		{
			this.dayInput.text = "0" + value;
		}
		this.ValidateInput();
		Action<string> onBirthdayChanged = this.OnBirthdayChanged;
		if (onBirthdayChanged == null)
		{
			return;
		}
		onBirthdayChanged(this.GetBirthdayOrDefault());
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002844 File Offset: 0x00000A44
	private void ValidateYear(string yearStr)
	{
		int num;
		if (int.TryParse(yearStr, out num))
		{
			int num2 = DateTime.Now.Year - 18;
			if (num > num2)
			{
				this.ShowWarning("Year {0} indicates age under 18. Must be 18 or older.", new object[] { num });
				return;
			}
			if (num < 1900)
			{
				this.ShowWarning("Please enter a valid birth year (1900 or later).", Array.Empty<object>());
			}
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000028A4 File Offset: 0x00000AA4
	private void ValidateMonth(string monthStr)
	{
		int num;
		if (int.TryParse(monthStr, out num) && (num < 1 || num > 12))
		{
			this.ShowWarning("Month must be between 01 and 12.", Array.Empty<object>());
			this.monthInput.text = "";
			this.monthInput.Select();
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000028F0 File Offset: 0x00000AF0
	private void ValidateDay(string dayStr)
	{
		int num;
		if (int.TryParse(dayStr, out num) && (num < 1 || num > 31))
		{
			this.ShowWarning("Day must be between 01 and 31.", Array.Empty<object>());
			this.dayInput.text = "";
			this.dayInput.Select();
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x0000293C File Offset: 0x00000B3C
	private void ValidateInput()
	{
		if (this.IsAllFieldsComplete())
		{
			string birthdayString = this.GetBirthdayString();
			if (!this.IsValidDate(birthdayString))
			{
				this.ShowWarning("Please enter a valid date for someone 18 or older.", Array.Empty<object>());
				return;
			}
			this.HideWarning();
			return;
		}
		else
		{
			if (this.yearInput.text.Length == 4)
			{
				this.ValidateYear(this.yearInput.text);
				return;
			}
			this.HideWarning();
			return;
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000029A4 File Offset: 0x00000BA4
	private bool IsAllFieldsComplete()
	{
		return this.yearInput.text.Length == 4 && this.monthInput.text.Length == 2 && this.dayInput.text.Length == 2;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000029E4 File Offset: 0x00000BE4
	private bool IsValidDate(string yyyymmdd)
	{
		bool flag;
		try
		{
			if (yyyymmdd.Length != 8)
			{
				flag = false;
			}
			else
			{
				int num = int.Parse(yyyymmdd.Substring(0, 4));
				int num2 = int.Parse(yyyymmdd.Substring(4, 2));
				int num3 = int.Parse(yyyymmdd.Substring(6, 2));
				if (num < 1900 || num > DateTime.Now.Year)
				{
					flag = false;
				}
				else if (num2 < 1 || num2 > 12)
				{
					flag = false;
				}
				else if (num3 < 1 || num3 > 31)
				{
					flag = false;
				}
				else
				{
					DateTime dateTime = new DateTime(num, num2, num3);
					if (dateTime > DateTime.Today)
					{
						flag = false;
					}
					else
					{
						int num4 = DateTime.Today.Year - dateTime.Year;
						if (dateTime > DateTime.Today.AddYears(-num4))
						{
							num4--;
						}
						flag = num4 >= 18;
					}
				}
			}
		}
		catch
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002AE4 File Offset: 0x00000CE4
	private void ShowWarning(string message = null, params object[] arguments)
	{
		if (this.warningMessage != null)
		{
			this.warningMessage.SetActive(true);
		}
		if (this.warningText != null && !string.IsNullOrEmpty(message))
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(this.warningText, "cielos_apps", message, arguments);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002B3E File Offset: 0x00000D3E
	private void HideWarning()
	{
		if (this.warningMessage != null)
		{
			this.warningMessage.SetActive(false);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002B5C File Offset: 0x00000D5C
	public void ClearInput()
	{
		this.yearInput.text = "";
		this.monthInput.text = "";
		this.dayInput.text = "";
		this.HideWarning();
		this.yearInput.Select();
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002BAA File Offset: 0x00000DAA
	public string GetBirthday()
	{
		if (this.IsAllFieldsComplete())
		{
			return this.GetBirthdayString();
		}
		return null;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002BBC File Offset: 0x00000DBC
	public string GetBirthdayOrDefault()
	{
		string text = (string.IsNullOrEmpty(this.yearInput.text) ? DateTime.Today.AddYears(-21).Year.ToString() : this.yearInput.text.PadLeft(4, '0'));
		string text2 = (string.IsNullOrEmpty(this.monthInput.text) ? "01" : this.monthInput.text.PadLeft(2, '0'));
		string text3 = (string.IsNullOrEmpty(this.dayInput.text) ? "01" : this.dayInput.text.PadLeft(2, '0'));
		return text + text2 + text3;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002C70 File Offset: 0x00000E70
	private string GetBirthdayString()
	{
		return this.yearInput.text + this.monthInput.text + this.dayInput.text;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002C98 File Offset: 0x00000E98
	public bool IsInputValid()
	{
		string birthday = this.GetBirthday();
		return birthday != null && this.IsValidDate(birthday);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002CB8 File Offset: 0x00000EB8
	public bool TryGetValidBirthday(out string birthday)
	{
		birthday = this.GetBirthday();
		if (birthday != null && this.IsValidDate(birthday))
		{
			this.HideWarning();
			return true;
		}
		birthday = null;
		this.ShowWarning("Please enter a valid date for someone 18 or older.", Array.Empty<object>());
		return false;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002CEC File Offset: 0x00000EEC
	public void SetBirthday(string yyyymmdd)
	{
		this.hasAssignedBirthday = true;
		if (yyyymmdd != null && yyyymmdd.Length == 8)
		{
			this.yearInput.SetTextWithoutNotify(yyyymmdd.Substring(0, 4));
			this.monthInput.SetTextWithoutNotify(yyyymmdd.Substring(4, 2));
			this.dayInput.SetTextWithoutNotify(yyyymmdd.Substring(6, 2));
		}
		else
		{
			this.yearInput.SetTextWithoutNotify(string.Empty);
			this.monthInput.SetTextWithoutNotify(string.Empty);
			this.dayInput.SetTextWithoutNotify(string.Empty);
		}
		this.ValidateInput();
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002D80 File Offset: 0x00000F80
	public void FocusFirstEmptyField()
	{
		if (string.IsNullOrEmpty(this.yearInput.text))
		{
			this.yearInput.Select();
			return;
		}
		if (string.IsNullOrEmpty(this.monthInput.text))
		{
			this.monthInput.Select();
			return;
		}
		if (string.IsNullOrEmpty(this.dayInput.text))
		{
			this.dayInput.Select();
		}
	}

	// Token: 0x04000009 RID: 9
	[Header("Input Fields")]
	public TMP_InputField yearInput;

	// Token: 0x0400000A RID: 10
	public TMP_InputField monthInput;

	// Token: 0x0400000B RID: 11
	public TMP_InputField dayInput;

	// Token: 0x0400000C RID: 12
	[Header("UI Elements")]
	public GameObject warningMessage;

	// Token: 0x0400000D RID: 13
	public TMP_Text warningText;

	// Token: 0x0400000E RID: 14
	public Action<string> OnBirthdayChanged;

	// Token: 0x0400000F RID: 15
	private bool hasAssignedBirthday;
}
