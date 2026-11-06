using System;
using System.Collections.Generic;

namespace DA_Assets.CssNet
{
	// Token: 0x0200000A RID: 10
	public class CssSelector
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000026AC File Offset: 0x000008AC
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000026B4 File Offset: 0x000008B4
		public string Name { get; set; }

		// Token: 0x06000030 RID: 48 RVA: 0x000026C0 File Offset: 0x000008C0
		public CssSelector(string name, string block)
		{
			this.Name = name;
			string[] array = block.Split(';', StringSplitOptions.None);
			if (array.Length < 1)
			{
				CssRule cssRule = new CssRule(block + ";");
				this.rules.Add(cssRule);
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				CssRule cssRule2 = new CssRule(array2[i] + ";");
				this.rules.Add(cssRule2);
			}
		}

		// Token: 0x04000017 RID: 23
		public List<CssRule> rules = new List<CssRule>();
	}
}
