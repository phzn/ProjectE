using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x020004A4 RID: 1188
	internal class StylePropertyValueParser
	{
		// Token: 0x0600251F RID: 9503 RVA: 0x0009C8AC File Offset: 0x0009AAAC
		public string[] Parse(string propertyValue)
		{
			this.m_PropertyValue = propertyValue;
			this.m_ValueList.Clear();
			this.m_StringBuilder.Remove(0, this.m_StringBuilder.Length);
			this.m_ParseIndex = 0;
			while (this.m_ParseIndex < this.m_PropertyValue.Length)
			{
				char c = this.m_PropertyValue[this.m_ParseIndex];
				char c2 = c;
				char c3 = c2;
				if (c3 != ' ')
				{
					if (c3 != '(')
					{
						if (c3 != ',')
						{
							this.m_StringBuilder.Append(c);
						}
						else
						{
							this.EatSpace();
							this.AddValuePart();
							this.m_ValueList.Add(",");
						}
					}
					else
					{
						this.AppendFunction();
					}
				}
				else
				{
					this.EatSpace();
					this.AddValuePart();
				}
				this.m_ParseIndex++;
			}
			string text = this.m_StringBuilder.ToString();
			bool flag = !string.IsNullOrEmpty(text);
			if (flag)
			{
				this.m_ValueList.Add(text);
			}
			return this.m_ValueList.ToArray();
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x0009C9CC File Offset: 0x0009ABCC
		private void AddValuePart()
		{
			string text = this.m_StringBuilder.ToString();
			this.m_StringBuilder.Remove(0, this.m_StringBuilder.Length);
			this.m_ValueList.Add(text);
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x0009CA0C File Offset: 0x0009AC0C
		private void AppendFunction()
		{
			while (this.m_ParseIndex < this.m_PropertyValue.Length && this.m_PropertyValue[this.m_ParseIndex] != ')')
			{
				this.m_StringBuilder.Append(this.m_PropertyValue[this.m_ParseIndex]);
				this.m_ParseIndex++;
			}
			this.m_StringBuilder.Append(this.m_PropertyValue[this.m_ParseIndex]);
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x0009CA98 File Offset: 0x0009AC98
		private void EatSpace()
		{
			while (this.m_ParseIndex + 1 < this.m_PropertyValue.Length && this.m_PropertyValue[this.m_ParseIndex + 1] == ' ')
			{
				this.m_ParseIndex++;
			}
		}

		// Token: 0x040011DA RID: 4570
		private string m_PropertyValue;

		// Token: 0x040011DB RID: 4571
		private List<string> m_ValueList = new List<string>();

		// Token: 0x040011DC RID: 4572
		private StringBuilder m_StringBuilder = new StringBuilder();

		// Token: 0x040011DD RID: 4573
		private int m_ParseIndex = 0;
	}
}
