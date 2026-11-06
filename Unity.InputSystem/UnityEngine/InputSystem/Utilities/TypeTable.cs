using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200014E RID: 334
	internal struct TypeTable
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x00056B38 File Offset: 0x00054D38
		public IEnumerable<string> names
		{
			get
			{
				return this.table.Keys.Select<InternedString, string>((InternedString x) => x.ToString());
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001245 RID: 4677 RVA: 0x00056B69 File Offset: 0x00054D69
		public IEnumerable<InternedString> internedNames
		{
			get
			{
				return this.table.Keys;
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00056B76 File Offset: 0x00054D76
		public void Initialize()
		{
			this.table = new Dictionary<InternedString, Type>();
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00056B84 File Offset: 0x00054D84
		public InternedString FindNameForType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			foreach (KeyValuePair<InternedString, Type> keyValuePair in this.table)
			{
				if (keyValuePair.Value == type)
				{
					return keyValuePair.Key;
				}
			}
			return default(InternedString);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00056C08 File Offset: 0x00054E08
		public void AddTypeRegistration(string name, Type type)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Name cannot be null or empty", "name");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			InternedString internedString = new InternedString(name);
			this.table[internedString] = type;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00056C58 File Offset: 0x00054E58
		public Type LookupTypeRegistration(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			if (this.table == null)
			{
				throw new InvalidOperationException("Input System not yet initialized");
			}
			InternedString internedString = new InternedString(name);
			Type type;
			if (this.table.TryGetValue(internedString, out type))
			{
				return type;
			}
			return null;
		}

		// Token: 0x0400073F RID: 1855
		public Dictionary<InternedString, Type> table;
	}
}
