using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets.Utility;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.Serialization;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
	// Token: 0x02000024 RID: 36
	[Serializable]
	public class ResourceLocationData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00007D26 File Offset: 0x00005F26
		public string[] Keys
		{
			get
			{
				return this.m_Keys;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00007D2E File Offset: 0x00005F2E
		public string InternalId
		{
			get
			{
				return this.m_InternalId;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00007D36 File Offset: 0x00005F36
		public string Provider
		{
			get
			{
				return this.m_Provider;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007D3E File Offset: 0x00005F3E
		public string[] Dependencies
		{
			get
			{
				return this.m_Dependencies;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007D46 File Offset: 0x00005F46
		public Type ResourceType
		{
			get
			{
				return this.m_ResourceType.Value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007D53 File Offset: 0x00005F53
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00007D88 File Offset: 0x00005F88
		public object Data
		{
			get
			{
				if (this._Data == null)
				{
					if (this.SerializedData == null || this.SerializedData.Length == 0)
					{
						return null;
					}
					this._Data = SerializationUtilities.ReadObjectFromByteArray(this.SerializedData, 0);
				}
				return this._Data;
			}
			set
			{
				List<byte> list = new List<byte>();
				SerializationUtilities.WriteObjectToByteList(value, list);
				this.SerializedData = list.ToArray();
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007DB0 File Offset: 0x00005FB0
		public ResourceLocationData(string[] keys, string id, Type provider, Type t, string[] dependencies = null)
		{
			this.m_Keys = keys;
			this.m_InternalId = id;
			this.m_Provider = ((provider == null) ? "" : provider.FullName);
			this.m_Dependencies = ((dependencies == null) ? new string[0] : dependencies);
			this.m_ResourceType = new SerializedType
			{
				Value = t
			};
		}

		// Token: 0x0400006E RID: 110
		[FormerlySerializedAs("m_keys")]
		[SerializeField]
		private string[] m_Keys;

		// Token: 0x0400006F RID: 111
		[FormerlySerializedAs("m_internalId")]
		[SerializeField]
		private string m_InternalId;

		// Token: 0x04000070 RID: 112
		[FormerlySerializedAs("m_provider")]
		[SerializeField]
		private string m_Provider;

		// Token: 0x04000071 RID: 113
		[FormerlySerializedAs("m_dependencies")]
		[SerializeField]
		private string[] m_Dependencies;

		// Token: 0x04000072 RID: 114
		[SerializeField]
		private SerializedType m_ResourceType;

		// Token: 0x04000073 RID: 115
		[SerializeField]
		private byte[] SerializedData;

		// Token: 0x04000074 RID: 116
		private object _Data;
	}
}
