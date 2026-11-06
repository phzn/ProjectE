using System;

namespace System.Resources
{
	// Token: 0x02000873 RID: 2163
	internal class NameOrId
	{
		// Token: 0x0600480D RID: 18445 RVA: 0x000ECFEB File Offset: 0x000EB1EB
		public NameOrId(string name)
		{
			this.name = name;
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x000ECFFA File Offset: 0x000EB1FA
		public NameOrId(int id)
		{
			this.id = id;
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x0600480F RID: 18447 RVA: 0x000ED009 File Offset: 0x000EB209
		public bool IsName
		{
			get
			{
				return this.name != null;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06004810 RID: 18448 RVA: 0x000ED014 File Offset: 0x000EB214
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06004811 RID: 18449 RVA: 0x000ED01C File Offset: 0x000EB21C
		public int Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x000ED024 File Offset: 0x000EB224
		public override string ToString()
		{
			if (this.name != null)
			{
				return "Name(" + this.name + ")";
			}
			return "Id(" + this.id.ToString() + ")";
		}

		// Token: 0x04002E28 RID: 11816
		private string name;

		// Token: 0x04002E29 RID: 11817
		private int id;
	}
}
