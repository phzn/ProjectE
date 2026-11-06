using System;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public struct TableEntryReference : ISerializationCallbackReceiver, IEquatable<TableEntryReference>
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000070BD File Offset: 0x000052BD
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x000070C5 File Offset: 0x000052C5
		public TableEntryReference.Type ReferenceType { readonly get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000070CE File Offset: 0x000052CE
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x000070D6 File Offset: 0x000052D6
		public long KeyId
		{
			get
			{
				return this.m_KeyId;
			}
			private set
			{
				this.m_KeyId = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000070DF File Offset: 0x000052DF
		// (set) Token: 0x060001EB RID: 491 RVA: 0x000070E7 File Offset: 0x000052E7
		public string Key
		{
			get
			{
				return this.m_Key;
			}
			private set
			{
				this.m_Key = value;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000070F0 File Offset: 0x000052F0
		public static implicit operator TableEntryReference(string key)
		{
			if (!string.IsNullOrWhiteSpace(key))
			{
				return new TableEntryReference
				{
					Key = key,
					ReferenceType = TableEntryReference.Type.Name
				};
			}
			return default(TableEntryReference);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007128 File Offset: 0x00005328
		public static implicit operator TableEntryReference(long keyId)
		{
			if (keyId != 0L)
			{
				return new TableEntryReference
				{
					KeyId = keyId,
					ReferenceType = TableEntryReference.Type.Id
				};
			}
			return default(TableEntryReference);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000715B File Offset: 0x0000535B
		public static implicit operator string(TableEntryReference tableEntryReference)
		{
			return tableEntryReference.Key;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007164 File Offset: 0x00005364
		public static implicit operator long(TableEntryReference tableEntryReference)
		{
			return tableEntryReference.KeyId;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007170 File Offset: 0x00005370
		internal void Validate()
		{
			if (this.m_Valid)
			{
				return;
			}
			switch (this.ReferenceType)
			{
			case TableEntryReference.Type.Empty:
				throw new ArgumentException("Empty Table Entry Reference. Must contain a Name or Key Id");
			case TableEntryReference.Type.Name:
				if (string.IsNullOrWhiteSpace(this.Key))
				{
					throw new ArgumentException("Must use a valid Key, can not be null or Empty.");
				}
				break;
			case TableEntryReference.Type.Id:
				if (this.KeyId == 0L)
				{
					throw new ArgumentException("Key Id can not be empty.");
				}
				break;
			}
			this.m_Valid = true;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000071E0 File Offset: 0x000053E0
		public string ResolveKeyName(SharedTableData sharedData)
		{
			if (this.ReferenceType == TableEntryReference.Type.Name)
			{
				return this.Key;
			}
			if (this.ReferenceType != TableEntryReference.Type.Id)
			{
				return null;
			}
			if (!(sharedData != null))
			{
				return string.Format("Key Id {0}", this.KeyId);
			}
			return sharedData.GetKey(this.KeyId);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007234 File Offset: 0x00005434
		public override string ToString()
		{
			TableEntryReference.Type referenceType = this.ReferenceType;
			if (referenceType == TableEntryReference.Type.Name)
			{
				return "TableEntryReference(" + this.Key + ")";
			}
			if (referenceType != TableEntryReference.Type.Id)
			{
				return "TableEntryReference(Empty)";
			}
			return string.Format("{0}({1})", "TableEntryReference", this.KeyId);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007288 File Offset: 0x00005488
		public string ToString(TableReference tableReference)
		{
			SharedTableData sharedTableData = tableReference.SharedTableData;
			if (sharedTableData != null)
			{
				string text;
				long num;
				if (this.ReferenceType == TableEntryReference.Type.Name)
				{
					text = this.Key;
					num = sharedTableData.GetId(text);
				}
				else
				{
					if (this.ReferenceType != TableEntryReference.Type.Id)
					{
						return this.ToString();
					}
					num = this.KeyId;
					text = sharedTableData.GetKey(num);
				}
				return string.Format("{0}({1} - {2})", "TableEntryReference", num, text);
			}
			return this.ToString();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000730C File Offset: 0x0000550C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is TableEntryReference)
			{
				TableEntryReference tableEntryReference = (TableEntryReference)obj;
				return this.Equals(tableEntryReference);
			}
			return false;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007338 File Offset: 0x00005538
		public bool Equals(TableEntryReference other)
		{
			if (this.ReferenceType != other.ReferenceType)
			{
				return false;
			}
			if (this.ReferenceType == TableEntryReference.Type.Name)
			{
				return this.Key == other.Key;
			}
			return this.ReferenceType != TableEntryReference.Type.Id || this.KeyId == other.KeyId;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000738C File Offset: 0x0000558C
		public override int GetHashCode()
		{
			if (this.ReferenceType == TableEntryReference.Type.Name)
			{
				return this.Key.GetHashCode();
			}
			if (this.ReferenceType == TableEntryReference.Type.Id)
			{
				return this.KeyId.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000073D6 File Offset: 0x000055D6
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000073D8 File Offset: 0x000055D8
		public void OnAfterDeserialize()
		{
			if (this.KeyId != 0L)
			{
				this.ReferenceType = TableEntryReference.Type.Id;
				return;
			}
			if (string.IsNullOrEmpty(this.m_Key))
			{
				this.ReferenceType = TableEntryReference.Type.Empty;
				return;
			}
			this.ReferenceType = TableEntryReference.Type.Name;
		}

		// Token: 0x04000072 RID: 114
		[SerializeField]
		private long m_KeyId;

		// Token: 0x04000073 RID: 115
		[SerializeField]
		private string m_Key;

		// Token: 0x04000074 RID: 116
		private bool m_Valid;

		// Token: 0x0200011F RID: 287
		public enum Type
		{
			// Token: 0x04000278 RID: 632
			Empty,
			// Token: 0x04000279 RID: 633
			Name,
			// Token: 0x0400027A RID: 634
			Id
		}
	}
}
