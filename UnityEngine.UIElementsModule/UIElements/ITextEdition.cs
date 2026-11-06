using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000373 RID: 883
	public interface ITextEdition
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001DB6 RID: 7606
		// (set) Token: 0x06001DB7 RID: 7607
		bool multiline { get; set; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001DB8 RID: 7608
		// (set) Token: 0x06001DB9 RID: 7609
		bool isReadOnly { get; set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001DBA RID: 7610
		// (set) Token: 0x06001DBB RID: 7611
		int maxLength { get; set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001DBC RID: 7612
		// (set) Token: 0x06001DBD RID: 7613
		bool isDelayed { get; set; }

		// Token: 0x06001DBE RID: 7614
		void ResetValueAndText();

		// Token: 0x06001DBF RID: 7615
		void SaveValueAndText();

		// Token: 0x06001DC0 RID: 7616
		void RestoreValueAndText();

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001DC1 RID: 7617
		// (set) Token: 0x06001DC2 RID: 7618
		Func<char, bool> AcceptCharacter { get; set; }

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001DC3 RID: 7619
		// (set) Token: 0x06001DC4 RID: 7620
		Action<bool> UpdateScrollOffset { get; set; }

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001DC5 RID: 7621
		// (set) Token: 0x06001DC6 RID: 7622
		Action UpdateValueFromText { get; set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001DC7 RID: 7623
		// (set) Token: 0x06001DC8 RID: 7624
		Action UpdateTextFromValue { get; set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001DC9 RID: 7625
		// (set) Token: 0x06001DCA RID: 7626
		Action MoveFocusToCompositeRoot { get; set; }

		// Token: 0x06001DCB RID: 7627
		void UpdateText(string value);

		// Token: 0x06001DCC RID: 7628
		string CullString(string s);

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001DCD RID: 7629
		// (set) Token: 0x06001DCE RID: 7630
		char maskChar { get; set; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001DCF RID: 7631
		// (set) Token: 0x06001DD0 RID: 7632
		bool isPassword { get; set; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x00073FF8 File Offset: 0x000721F8
		// (set) Token: 0x06001DD2 RID: 7634 RVA: 0x0007402B File Offset: 0x0007222B
		bool autoCorrection
		{
			get
			{
				Debug.Log("Type " + base.GetType().Name + " implementing interface ITextEdition is missing the implementation for autoCorrection. Calling ITextEdition.autoCorrection of this type will always return false.");
				return false;
			}
			set
			{
				Debug.Log("Type " + base.GetType().Name + " implementing interface ITextEdition is missing the implementation for autoCorrection. Assigning a value to ITextEdition.autoCorrection will not update its value.");
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x00074050 File Offset: 0x00072250
		// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x00074083 File Offset: 0x00072283
		bool hideMobileInput
		{
			get
			{
				Debug.Log("Type " + base.GetType().Name + " implementing interface ITextEdition is missing the implementation for hideMobileInput. Calling ITextEdition.hideMobileInput of this type will always return false.");
				return false;
			}
			set
			{
				Debug.Log("Type " + base.GetType().Name + " implementing interface ITextEdition is missing the implementation for hideMobileInput. Assigning a value to ITextEdition.hideMobileInput will not update its value.");
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x000740A8 File Offset: 0x000722A8
		TouchScreenKeyboard touchScreenKeyboard
		{
			get
			{
				Debug.Log("Type " + base.GetType().Name + " implementing interface ITextEdition is missing the implementation for touchScreenKeyboard. Calling ITextEdition.touchScreenKeyboard of this type will always return null.");
				return null;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x000740DC File Offset: 0x000722DC
		// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x0007410F File Offset: 0x0007230F
		TouchScreenKeyboardType keyboardType
		{
			get
			{
				Debug.Log("Type " + base.GetType().Name + " implementing interface ITextEdition is missing the implementation for keyboardType. Calling ITextEdition.keyboardType of this type will always return Default.");
				return TouchScreenKeyboardType.Default;
			}
			set
			{
				Debug.Log("Type " + base.GetType().Name + " implementing interface ITextEdition is missing the implementation for keyboardType. Assigning a value to ITextEdition.keyboardType will not update its value.");
			}
		}
	}
}
