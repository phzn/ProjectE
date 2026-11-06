using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.Serialization;

namespace UnityEngine.AddressableAssets.Initialization
{
	// Token: 0x0200002A RID: 42
	[Serializable]
	public class ResourceManagerRuntimeData
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000901B File Offset: 0x0000721B
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00009023 File Offset: 0x00007223
		public string BuildTarget
		{
			get
			{
				return this.m_buildTarget;
			}
			set
			{
				this.m_buildTarget = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000902C File Offset: 0x0000722C
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00009034 File Offset: 0x00007234
		public string SettingsHash
		{
			get
			{
				return this.m_SettingsHash;
			}
			set
			{
				this.m_SettingsHash = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000903D File Offset: 0x0000723D
		public List<ResourceLocationData> CatalogLocations
		{
			get
			{
				return this.m_CatalogLocations;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00009045 File Offset: 0x00007245
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000904D File Offset: 0x0000724D
		public bool ProfileEvents
		{
			get
			{
				return this.m_ProfileEvents;
			}
			set
			{
				this.m_ProfileEvents = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00009056 File Offset: 0x00007256
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000905E File Offset: 0x0000725E
		public bool LogResourceManagerExceptions
		{
			get
			{
				return this.m_LogResourceManagerExceptions;
			}
			set
			{
				this.m_LogResourceManagerExceptions = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00009067 File Offset: 0x00007267
		public List<ObjectInitializationData> InitializationObjects
		{
			get
			{
				return this.m_ExtraInitializationData;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000906F File Offset: 0x0000726F
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00009077 File Offset: 0x00007277
		public bool DisableCatalogUpdateOnStartup
		{
			get
			{
				return this.m_DisableCatalogUpdateOnStart;
			}
			set
			{
				this.m_DisableCatalogUpdateOnStart = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00009080 File Offset: 0x00007280
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00009088 File Offset: 0x00007288
		public bool IsLocalCatalogInBundle
		{
			get
			{
				return this.m_IsLocalCatalogInBundle;
			}
			set
			{
				this.m_IsLocalCatalogInBundle = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00009091 File Offset: 0x00007291
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000909E File Offset: 0x0000729E
		public Type CertificateHandlerType
		{
			get
			{
				return this.m_CertificateHandlerType.Value;
			}
			set
			{
				this.m_CertificateHandlerType.Value = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000090AC File Offset: 0x000072AC
		// (set) Token: 0x06000202 RID: 514 RVA: 0x000090B4 File Offset: 0x000072B4
		public string AddressablesVersion
		{
			get
			{
				return this.m_AddressablesVersion;
			}
			set
			{
				this.m_AddressablesVersion = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000090BD File Offset: 0x000072BD
		// (set) Token: 0x06000204 RID: 516 RVA: 0x000090C5 File Offset: 0x000072C5
		public int MaxConcurrentWebRequests
		{
			get
			{
				return this.m_maxConcurrentWebRequests;
			}
			set
			{
				this.m_maxConcurrentWebRequests = Mathf.Clamp(value, 1, 1024);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000090D9 File Offset: 0x000072D9
		// (set) Token: 0x06000206 RID: 518 RVA: 0x000090E1 File Offset: 0x000072E1
		public int CatalogRequestsTimeout
		{
			get
			{
				return this.m_CatalogRequestsTimeout;
			}
			set
			{
				this.m_CatalogRequestsTimeout = ((value < 0) ? 0 : value);
			}
		}

		// Token: 0x04000086 RID: 134
		public const string kCatalogAddress = "AddressablesMainContentCatalog";

		// Token: 0x04000087 RID: 135
		[SerializeField]
		private string m_buildTarget;

		// Token: 0x04000088 RID: 136
		[FormerlySerializedAs("m_settingsHash")]
		[SerializeField]
		private string m_SettingsHash;

		// Token: 0x04000089 RID: 137
		[FormerlySerializedAs("m_catalogLocations")]
		[SerializeField]
		private List<ResourceLocationData> m_CatalogLocations = new List<ResourceLocationData>();

		// Token: 0x0400008A RID: 138
		[FormerlySerializedAs("m_profileEvents")]
		[SerializeField]
		private bool m_ProfileEvents;

		// Token: 0x0400008B RID: 139
		[FormerlySerializedAs("m_logResourceManagerExceptions")]
		[SerializeField]
		private bool m_LogResourceManagerExceptions = true;

		// Token: 0x0400008C RID: 140
		[FormerlySerializedAs("m_extraInitializationData")]
		[SerializeField]
		private List<ObjectInitializationData> m_ExtraInitializationData = new List<ObjectInitializationData>();

		// Token: 0x0400008D RID: 141
		[SerializeField]
		private bool m_DisableCatalogUpdateOnStart;

		// Token: 0x0400008E RID: 142
		[SerializeField]
		private bool m_IsLocalCatalogInBundle;

		// Token: 0x0400008F RID: 143
		[SerializeField]
		private SerializedType m_CertificateHandlerType;

		// Token: 0x04000090 RID: 144
		[SerializeField]
		private string m_AddressablesVersion;

		// Token: 0x04000091 RID: 145
		[SerializeField]
		private int m_maxConcurrentWebRequests = 500;

		// Token: 0x04000092 RID: 146
		[SerializeField]
		private int m_CatalogRequestsTimeout;
	}
}
