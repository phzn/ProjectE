using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class NamingStrategy
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00022D44 File Offset: 0x00020F44
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x00022D4C File Offset: 0x00020F4C
		public bool ProcessDictionaryKeys { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00022D55 File Offset: 0x00020F55
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x00022D5D File Offset: 0x00020F5D
		public bool ProcessExtensionDataNames { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x00022D66 File Offset: 0x00020F66
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00022D6E File Offset: 0x00020F6E
		public bool OverrideSpecifiedNames { get; set; }

		// Token: 0x0600080C RID: 2060 RVA: 0x00022D77 File Offset: 0x00020F77
		public virtual string GetPropertyName(string name, bool hasSpecifiedName)
		{
			if (hasSpecifiedName && !this.OverrideSpecifiedNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00022D8D File Offset: 0x00020F8D
		public virtual string GetExtensionDataName(string name)
		{
			if (!this.ProcessExtensionDataNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00022DA0 File Offset: 0x00020FA0
		public virtual string GetDictionaryKey(string key)
		{
			if (!this.ProcessDictionaryKeys)
			{
				return key;
			}
			return this.ResolvePropertyName(key);
		}

		// Token: 0x0600080F RID: 2063
		protected abstract string ResolvePropertyName(string name);

		// Token: 0x06000810 RID: 2064 RVA: 0x00022DB4 File Offset: 0x00020FB4
		public override int GetHashCode()
		{
			return (((((base.GetType().GetHashCode() * 397) ^ this.ProcessDictionaryKeys.GetHashCode()) * 397) ^ this.ProcessExtensionDataNames.GetHashCode()) * 397) ^ this.OverrideSpecifiedNames.GetHashCode();
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00022E0B File Offset: 0x0002100B
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NamingStrategy);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00022E1C File Offset: 0x0002101C
		[NullableContext(2)]
		protected bool Equals(NamingStrategy other)
		{
			return other != null && (base.GetType() == other.GetType() && this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames) && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
		}
	}
}
