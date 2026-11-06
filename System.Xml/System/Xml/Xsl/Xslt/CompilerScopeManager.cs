using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020003DB RID: 987
	internal sealed class CompilerScopeManager<V>
	{
		// Token: 0x06002765 RID: 10085 RVA: 0x000EAC6C File Offset: 0x000E8E6C
		public CompilerScopeManager()
		{
			this.records[0].flags = CompilerScopeManager<V>.ScopeFlags.NsDecl;
			this.records[0].ncName = "xml";
			this.records[0].nsUri = "http://www.w3.org/XML/1998/namespace";
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000EACCC File Offset: 0x000E8ECC
		public CompilerScopeManager(KeywordsTable atoms)
		{
			this.records[0].flags = CompilerScopeManager<V>.ScopeFlags.NsDecl;
			this.records[0].ncName = atoms.Xml;
			this.records[0].nsUri = atoms.UriXml;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000EAD2D File Offset: 0x000E8F2D
		public void EnterScope()
		{
			this.lastScopes++;
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000EAD40 File Offset: 0x000E8F40
		public void ExitScope()
		{
			if (0 < this.lastScopes)
			{
				this.lastScopes--;
				return;
			}
			CompilerScopeManager<V>.ScopeRecord[] array;
			int num;
			do
			{
				array = this.records;
				num = this.lastRecord - 1;
				this.lastRecord = num;
			}
			while (array[num].scopeCount == 0);
			this.lastScopes = this.records[this.lastRecord].scopeCount;
			this.lastScopes--;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000EADB2 File Offset: 0x000E8FB2
		[Conditional("DEBUG")]
		public void CheckEmpty()
		{
			this.ExitScope();
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000EADBC File Offset: 0x000E8FBC
		public bool EnterScope(NsDecl nsDecl)
		{
			this.lastScopes++;
			bool flag = false;
			bool flag2 = false;
			while (nsDecl != null)
			{
				if (nsDecl.NsUri == null)
				{
					flag2 = true;
				}
				else if (nsDecl.Prefix == null)
				{
					this.AddExNamespace(nsDecl.NsUri);
				}
				else
				{
					flag = true;
					this.AddNsDeclaration(nsDecl.Prefix, nsDecl.NsUri);
				}
				nsDecl = nsDecl.Prev;
			}
			if (flag2)
			{
				this.AddExNamespace(null);
			}
			return flag;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000EAE2C File Offset: 0x000E902C
		private void AddRecord()
		{
			this.records[this.lastRecord].scopeCount = this.lastScopes;
			int num = this.lastRecord + 1;
			this.lastRecord = num;
			if (num == this.records.Length)
			{
				CompilerScopeManager<V>.ScopeRecord[] array = new CompilerScopeManager<V>.ScopeRecord[this.lastRecord * 2];
				Array.Copy(this.records, 0, array, 0, this.lastRecord);
				this.records = array;
			}
			this.lastScopes = 0;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000EAEA0 File Offset: 0x000E90A0
		private void AddRecord(CompilerScopeManager<V>.ScopeFlags flag, string ncName, string uri, V value)
		{
			CompilerScopeManager<V>.ScopeFlags scopeFlags = this.records[this.lastRecord].flags;
			if (this.lastScopes != 0 || (scopeFlags & CompilerScopeManager<V>.ScopeFlags.ExclusiveFlags) != (CompilerScopeManager<V>.ScopeFlags)0)
			{
				this.AddRecord();
				scopeFlags &= CompilerScopeManager<V>.ScopeFlags.InheritedFlags;
			}
			this.records[this.lastRecord].flags = scopeFlags | flag;
			this.records[this.lastRecord].ncName = ncName;
			this.records[this.lastRecord].nsUri = uri;
			this.records[this.lastRecord].value = value;
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000EAF44 File Offset: 0x000E9144
		private void SetFlag(CompilerScopeManager<V>.ScopeFlags flag, bool value)
		{
			CompilerScopeManager<V>.ScopeFlags scopeFlags = this.records[this.lastRecord].flags;
			if ((scopeFlags & flag) > (CompilerScopeManager<V>.ScopeFlags)0 != value)
			{
				if (this.lastScopes != 0)
				{
					this.AddRecord();
					scopeFlags &= CompilerScopeManager<V>.ScopeFlags.InheritedFlags;
				}
				if (flag == CompilerScopeManager<V>.ScopeFlags.CanHaveApplyImports)
				{
					scopeFlags ^= flag;
				}
				else
				{
					scopeFlags &= (CompilerScopeManager<V>.ScopeFlags)(-4);
					if (value)
					{
						scopeFlags |= flag;
					}
				}
				this.records[this.lastRecord].flags = scopeFlags;
			}
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000EAFB3 File Offset: 0x000E91B3
		public void AddVariable(QilName varName, V value)
		{
			this.AddRecord(CompilerScopeManager<V>.ScopeFlags.Variable, varName.LocalName, varName.NamespaceUri, value);
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000EAFCC File Offset: 0x000E91CC
		private string LookupNamespace(string prefix, int from, int to)
		{
			int num = from;
			while (to <= num)
			{
				string text;
				string text2;
				if ((CompilerScopeManager<V>.GetName(ref this.records[num], out text, out text2) & CompilerScopeManager<V>.ScopeFlags.NsDecl) != (CompilerScopeManager<V>.ScopeFlags)0 && text == prefix)
				{
					return text2;
				}
				num--;
			}
			return null;
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000EB00B File Offset: 0x000E920B
		public string LookupNamespace(string prefix)
		{
			return this.LookupNamespace(prefix, this.lastRecord, 0);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000EB01B File Offset: 0x000E921B
		private static CompilerScopeManager<V>.ScopeFlags GetName(ref CompilerScopeManager<V>.ScopeRecord re, out string prefix, out string nsUri)
		{
			prefix = re.ncName;
			nsUri = re.nsUri;
			return re.flags;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000EB034 File Offset: 0x000E9234
		public void AddNsDeclaration(string prefix, string nsUri)
		{
			this.AddRecord(CompilerScopeManager<V>.ScopeFlags.NsDecl, prefix, nsUri, default(V));
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000EB054 File Offset: 0x000E9254
		public void AddExNamespace(string nsUri)
		{
			this.AddRecord(CompilerScopeManager<V>.ScopeFlags.NsExcl, null, nsUri, default(V));
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000EB074 File Offset: 0x000E9274
		public bool IsExNamespace(string nsUri)
		{
			int num = 0;
			int num2 = this.lastRecord;
			while (0 <= num2)
			{
				string text;
				string text2;
				CompilerScopeManager<V>.ScopeFlags name = CompilerScopeManager<V>.GetName(ref this.records[num2], out text, out text2);
				if ((name & CompilerScopeManager<V>.ScopeFlags.NsExcl) != (CompilerScopeManager<V>.ScopeFlags)0)
				{
					if (text2 == nsUri)
					{
						return true;
					}
					if (text2 == null)
					{
						num = num2;
					}
				}
				else if (num != 0 && (name & CompilerScopeManager<V>.ScopeFlags.NsDecl) != (CompilerScopeManager<V>.ScopeFlags)0 && text2 == nsUri)
				{
					bool flag = false;
					for (int i = num2 + 1; i < num; i++)
					{
						string text3;
						string text4;
						CompilerScopeManager<V>.GetName(ref this.records[i], out text3, out text4);
						if ((name & CompilerScopeManager<V>.ScopeFlags.NsDecl) != (CompilerScopeManager<V>.ScopeFlags)0 && text3 == text)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return true;
					}
				}
				num2--;
			}
			return false;
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000EB128 File Offset: 0x000E9328
		private int SearchVariable(string localName, string uri)
		{
			int num = this.lastRecord;
			while (0 <= num)
			{
				string text;
				string text2;
				if ((CompilerScopeManager<V>.GetName(ref this.records[num], out text, out text2) & CompilerScopeManager<V>.ScopeFlags.Variable) != (CompilerScopeManager<V>.ScopeFlags)0 && text == localName && text2 == uri)
				{
					return num;
				}
				num--;
			}
			return -1;
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000EB178 File Offset: 0x000E9378
		public V LookupVariable(string localName, string uri)
		{
			int num = this.SearchVariable(localName, uri);
			if (num >= 0)
			{
				return this.records[num].value;
			}
			return default(V);
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000EB1B0 File Offset: 0x000E93B0
		public bool IsLocalVariable(string localName, string uri)
		{
			int num = this.SearchVariable(localName, uri);
			while (0 <= --num)
			{
				if (this.records[num].scopeCount != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x000EB1E6 File Offset: 0x000E93E6
		// (set) Token: 0x06002779 RID: 10105 RVA: 0x000EB203 File Offset: 0x000E9403
		public bool ForwardCompatibility
		{
			get
			{
				return (this.records[this.lastRecord].flags & CompilerScopeManager<V>.ScopeFlags.ForwardCompatibility) > (CompilerScopeManager<V>.ScopeFlags)0;
			}
			set
			{
				this.SetFlag(CompilerScopeManager<V>.ScopeFlags.ForwardCompatibility, value);
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x000EB20D File Offset: 0x000E940D
		// (set) Token: 0x0600277B RID: 10107 RVA: 0x000EB22A File Offset: 0x000E942A
		public bool BackwardCompatibility
		{
			get
			{
				return (this.records[this.lastRecord].flags & CompilerScopeManager<V>.ScopeFlags.BackwardCompatibility) > (CompilerScopeManager<V>.ScopeFlags)0;
			}
			set
			{
				this.SetFlag(CompilerScopeManager<V>.ScopeFlags.BackwardCompatibility, value);
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x000EB234 File Offset: 0x000E9434
		// (set) Token: 0x0600277D RID: 10109 RVA: 0x000EB251 File Offset: 0x000E9451
		public bool CanHaveApplyImports
		{
			get
			{
				return (this.records[this.lastRecord].flags & CompilerScopeManager<V>.ScopeFlags.CanHaveApplyImports) > (CompilerScopeManager<V>.ScopeFlags)0;
			}
			set
			{
				this.SetFlag(CompilerScopeManager<V>.ScopeFlags.CanHaveApplyImports, value);
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000EB25B File Offset: 0x000E945B
		internal IEnumerable<CompilerScopeManager<V>.ScopeRecord> GetActiveRecords()
		{
			int currentRecord = this.lastRecord + 1;
			for (;;)
			{
				int num = 0;
				int num2 = currentRecord - 1;
				currentRecord = num2;
				if (num >= num2)
				{
					break;
				}
				if (!this.records[currentRecord].IsNamespace || this.LookupNamespace(this.records[currentRecord].ncName, this.lastRecord, currentRecord + 1) == null)
				{
					yield return this.records[currentRecord];
				}
			}
			yield break;
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000EB26B File Offset: 0x000E946B
		public CompilerScopeManager<V>.NamespaceEnumerator GetEnumerator()
		{
			return new CompilerScopeManager<V>.NamespaceEnumerator(this);
		}

		// Token: 0x04001EE4 RID: 7908
		private const int LastPredefRecord = 0;

		// Token: 0x04001EE5 RID: 7909
		private CompilerScopeManager<V>.ScopeRecord[] records = new CompilerScopeManager<V>.ScopeRecord[32];

		// Token: 0x04001EE6 RID: 7910
		private int lastRecord;

		// Token: 0x04001EE7 RID: 7911
		private int lastScopes;

		// Token: 0x020003DC RID: 988
		public enum ScopeFlags
		{
			// Token: 0x04001EE9 RID: 7913
			BackwardCompatibility = 1,
			// Token: 0x04001EEA RID: 7914
			ForwardCompatibility,
			// Token: 0x04001EEB RID: 7915
			CanHaveApplyImports = 4,
			// Token: 0x04001EEC RID: 7916
			NsDecl = 16,
			// Token: 0x04001EED RID: 7917
			NsExcl = 32,
			// Token: 0x04001EEE RID: 7918
			Variable = 64,
			// Token: 0x04001EEF RID: 7919
			CompatibilityFlags = 3,
			// Token: 0x04001EF0 RID: 7920
			InheritedFlags = 7,
			// Token: 0x04001EF1 RID: 7921
			ExclusiveFlags = 112
		}

		// Token: 0x020003DD RID: 989
		public struct ScopeRecord
		{
			// Token: 0x170007C4 RID: 1988
			// (get) Token: 0x06002780 RID: 10112 RVA: 0x000EB273 File Offset: 0x000E9473
			public bool IsVariable
			{
				get
				{
					return (this.flags & CompilerScopeManager<V>.ScopeFlags.Variable) > (CompilerScopeManager<V>.ScopeFlags)0;
				}
			}

			// Token: 0x170007C5 RID: 1989
			// (get) Token: 0x06002781 RID: 10113 RVA: 0x000EB281 File Offset: 0x000E9481
			public bool IsNamespace
			{
				get
				{
					return (this.flags & CompilerScopeManager<V>.ScopeFlags.NsDecl) > (CompilerScopeManager<V>.ScopeFlags)0;
				}
			}

			// Token: 0x04001EF2 RID: 7922
			public int scopeCount;

			// Token: 0x04001EF3 RID: 7923
			public CompilerScopeManager<V>.ScopeFlags flags;

			// Token: 0x04001EF4 RID: 7924
			public string ncName;

			// Token: 0x04001EF5 RID: 7925
			public string nsUri;

			// Token: 0x04001EF6 RID: 7926
			public V value;
		}

		// Token: 0x020003DE RID: 990
		internal struct NamespaceEnumerator
		{
			// Token: 0x06002782 RID: 10114 RVA: 0x000EB28F File Offset: 0x000E948F
			public NamespaceEnumerator(CompilerScopeManager<V> scope)
			{
				this.scope = scope;
				this.lastRecord = scope.lastRecord;
				this.currentRecord = this.lastRecord + 1;
			}

			// Token: 0x06002783 RID: 10115 RVA: 0x000EB2B2 File Offset: 0x000E94B2
			public void Reset()
			{
				this.currentRecord = this.lastRecord + 1;
			}

			// Token: 0x06002784 RID: 10116 RVA: 0x000EB2C4 File Offset: 0x000E94C4
			public bool MoveNext()
			{
				do
				{
					int num = 0;
					int num2 = this.currentRecord - 1;
					this.currentRecord = num2;
					if (num >= num2)
					{
						return false;
					}
				}
				while (!this.scope.records[this.currentRecord].IsNamespace || this.scope.LookupNamespace(this.scope.records[this.currentRecord].ncName, this.lastRecord, this.currentRecord + 1) != null);
				return true;
			}

			// Token: 0x170007C6 RID: 1990
			// (get) Token: 0x06002785 RID: 10117 RVA: 0x000EB33D File Offset: 0x000E953D
			public CompilerScopeManager<V>.ScopeRecord Current
			{
				get
				{
					return this.scope.records[this.currentRecord];
				}
			}

			// Token: 0x04001EF7 RID: 7927
			private CompilerScopeManager<V> scope;

			// Token: 0x04001EF8 RID: 7928
			private int lastRecord;

			// Token: 0x04001EF9 RID: 7929
			private int currentRecord;
		}
	}
}
