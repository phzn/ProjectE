using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public struct TableReference : ISerializationCallbackReceiver, IEquatable<TableReference>
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00006B25 File Offset: 0x00004D25
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00006B2D File Offset: 0x00004D2D
		public TableReference.Type ReferenceType { readonly get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00006B36 File Offset: 0x00004D36
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00006B3E File Offset: 0x00004D3E
		public Guid TableCollectionNameGuid { readonly get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00006B47 File Offset: 0x00004D47
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00006B6A File Offset: 0x00004D6A
		public string TableCollectionName
		{
			get
			{
				if (this.ReferenceType == TableReference.Type.Name)
				{
					return this.m_TableCollectionName;
				}
				SharedTableData sharedTableData = this.SharedTableData;
				if (sharedTableData == null)
				{
					return null;
				}
				return sharedTableData.TableCollectionName;
			}
			private set
			{
				this.m_TableCollectionName = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00006B74 File Offset: 0x00004D74
		internal SharedTableData SharedTableData
		{
			get
			{
				if (this.ReferenceType == TableReference.Type.Empty || !LocalizationSettings.HasSettings)
				{
					return null;
				}
				if (this.ReferenceType == TableReference.Type.Guid)
				{
					AsyncOperationHandle<SharedTableData> asyncOperationHandle;
					if (LocalizationSettings.StringDatabase != null && LocalizationSettings.StringDatabase.SharedTableDataOperations.TryGetValue(this.TableCollectionNameGuid, out asyncOperationHandle))
					{
						return asyncOperationHandle.Result;
					}
					if (LocalizationSettings.AssetDatabase != null && LocalizationSettings.AssetDatabase.SharedTableDataOperations.TryGetValue(this.TableCollectionNameGuid, out asyncOperationHandle))
					{
						return asyncOperationHandle.Result;
					}
				}
				else if (this.ReferenceType == TableReference.Type.Name)
				{
					LocalizedStringDatabase stringDatabase = LocalizationSettings.StringDatabase;
					foreach (KeyValuePair<Guid, AsyncOperationHandle<SharedTableData>> keyValuePair in ((stringDatabase != null) ? stringDatabase.SharedTableDataOperations : null))
					{
						SharedTableData result = keyValuePair.Value.Result;
						if (((result != null) ? result.TableCollectionName : null) == this.m_TableCollectionName)
						{
							return keyValuePair.Value.Result;
						}
					}
					LocalizedAssetDatabase assetDatabase = LocalizationSettings.AssetDatabase;
					foreach (KeyValuePair<Guid, AsyncOperationHandle<SharedTableData>> keyValuePair2 in ((assetDatabase != null) ? assetDatabase.SharedTableDataOperations : null))
					{
						SharedTableData result2 = keyValuePair2.Value.Result;
						if (((result2 != null) ? result2.TableCollectionName : null) == this.m_TableCollectionName)
						{
							return keyValuePair2.Value.Result;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006D0C File Offset: 0x00004F0C
		public static implicit operator TableReference(string tableCollectionName)
		{
			return new TableReference
			{
				TableCollectionName = tableCollectionName,
				ReferenceType = (string.IsNullOrWhiteSpace(tableCollectionName) ? TableReference.Type.Empty : TableReference.Type.Name)
			};
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00006D40 File Offset: 0x00004F40
		public static implicit operator TableReference(Guid tableCollectionNameGuid)
		{
			return new TableReference
			{
				TableCollectionNameGuid = tableCollectionNameGuid,
				ReferenceType = ((tableCollectionNameGuid == Guid.Empty) ? TableReference.Type.Empty : TableReference.Type.Guid)
			};
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006D76 File Offset: 0x00004F76
		public static implicit operator string(TableReference tableReference)
		{
			return tableReference.TableCollectionName;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006D7F File Offset: 0x00004F7F
		public static implicit operator Guid(TableReference tableReference)
		{
			return tableReference.TableCollectionNameGuid;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006D88 File Offset: 0x00004F88
		internal void Validate()
		{
			if (this.m_Valid)
			{
				return;
			}
			switch (this.ReferenceType)
			{
			case TableReference.Type.Empty:
				throw new ArgumentException("Empty Table Reference. Must contain a Guid or Table Collection Name");
			case TableReference.Type.Guid:
				if (this.TableCollectionNameGuid == Guid.Empty)
				{
					throw new ArgumentException("Must use a valid Table Collection Name Guid, can not be Empty.");
				}
				break;
			case TableReference.Type.Name:
				if (string.IsNullOrWhiteSpace(this.TableCollectionName))
				{
					throw new ArgumentException("Table Collection Name can not be null or empty.");
				}
				break;
			}
			this.m_Valid = true;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00006E00 File Offset: 0x00005000
		internal string GetSerializedString()
		{
			TableReference.Type referenceType = this.ReferenceType;
			if (referenceType == TableReference.Type.Guid)
			{
				return "GUID:" + TableReference.StringFromGuid(this.TableCollectionNameGuid);
			}
			if (referenceType != TableReference.Type.Name)
			{
				return string.Empty;
			}
			return this.TableCollectionName;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00006E40 File Offset: 0x00005040
		public override string ToString()
		{
			if (this.ReferenceType == TableReference.Type.Guid)
			{
				return string.Format("{0}({1} - {2})", "TableReference", this.TableCollectionNameGuid, this.TableCollectionName);
			}
			if (this.ReferenceType == TableReference.Type.Name)
			{
				return "TableReference(" + this.TableCollectionName + ")";
			}
			return "TableReference(Empty)";
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006E9C File Offset: 0x0000509C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is TableReference)
			{
				TableReference tableReference = (TableReference)obj;
				return this.Equals(tableReference);
			}
			return false;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006EC8 File Offset: 0x000050C8
		public override int GetHashCode()
		{
			if (this.ReferenceType == TableReference.Type.Guid)
			{
				return this.TableCollectionNameGuid.GetHashCode();
			}
			if (this.ReferenceType == TableReference.Type.Name)
			{
				return this.TableCollectionName.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006F18 File Offset: 0x00005118
		public bool Equals(TableReference other)
		{
			if (this.ReferenceType != other.ReferenceType)
			{
				return false;
			}
			if (this.ReferenceType == TableReference.Type.Guid)
			{
				return this.TableCollectionNameGuid == other.TableCollectionNameGuid;
			}
			return this.ReferenceType != TableReference.Type.Name || this.TableCollectionName == other.TableCollectionName;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00006F70 File Offset: 0x00005170
		internal static string StringFromGuid(Guid value)
		{
			string text;
			if (TableReference.s_GuidToStringCache.TryGetValue(value, out text))
			{
				return text;
			}
			string text2 = value.ToString("N");
			TableReference.s_GuidToStringCache.TryAdd(value, text2);
			return text2;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00006FAC File Offset: 0x000051AC
		internal static Guid GuidFromString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return Guid.Empty;
			}
			Guid guid;
			if (TableReference.s_StringToGuidCache.TryGetValue(value, out guid))
			{
				return guid;
			}
			Guid guid2;
			if (Guid.TryParse(value.Substring("GUID:".Length), out guid2))
			{
				TableReference.s_StringToGuidCache.TryAdd(value, guid2);
				return guid2;
			}
			TableReference.s_StringToGuidCache.TryAdd(value, Guid.Empty);
			return Guid.Empty;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00007016 File Offset: 0x00005216
		internal static TableReference TableReferenceFromString(string value)
		{
			if (TableReference.IsGuid(value))
			{
				return TableReference.GuidFromString(value);
			}
			return value;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00007032 File Offset: 0x00005232
		internal static bool IsGuid(string value)
		{
			return !string.IsNullOrEmpty(value) && value.StartsWith("GUID:", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000704A File Offset: 0x0000524A
		public void OnBeforeSerialize()
		{
			this.m_TableCollectionName = this.GetSerializedString();
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00007058 File Offset: 0x00005258
		public void OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(this.m_TableCollectionName))
			{
				this.ReferenceType = TableReference.Type.Empty;
				return;
			}
			if (TableReference.IsGuid(this.m_TableCollectionName))
			{
				this.TableCollectionNameGuid = TableReference.GuidFromString(this.m_TableCollectionName);
				this.ReferenceType = TableReference.Type.Guid;
				return;
			}
			this.ReferenceType = TableReference.Type.Name;
		}

		// Token: 0x0400006B RID: 107
		private static readonly ConcurrentDictionary<Guid, string> s_GuidToStringCache = new ConcurrentDictionary<Guid, string>();

		// Token: 0x0400006C RID: 108
		private static readonly ConcurrentDictionary<string, Guid> s_StringToGuidCache = new ConcurrentDictionary<string, Guid>();

		// Token: 0x0400006D RID: 109
		[SerializeField]
		[FormerlySerializedAs("m_TableName")]
		private string m_TableCollectionName;

		// Token: 0x0400006E RID: 110
		private bool m_Valid;

		// Token: 0x0400006F RID: 111
		private const string k_GuidTag = "GUID:";

		// Token: 0x0200011E RID: 286
		public enum Type
		{
			// Token: 0x04000274 RID: 628
			Empty,
			// Token: 0x04000275 RID: 629
			Guid,
			// Token: 0x04000276 RID: 630
			Name
		}
	}
}
