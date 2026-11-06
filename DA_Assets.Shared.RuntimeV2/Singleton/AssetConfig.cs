using System;
using UnityEngine;

namespace DA_Assets.Singleton
{
	// Token: 0x02000003 RID: 3
	public class AssetConfig<T> : SingletonScriptableObject<T> where T : ScriptableObject
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020BF File Offset: 0x000002BF
		public string ProductVersion
		{
			get
			{
				return this.productVersion;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020C7 File Offset: 0x000002C7
		public InternalLocalizator Localizator
		{
			get
			{
				return this.localizator.Init();
			}
		}

		// Token: 0x04000001 RID: 1
		[SerializeField]
		private string productVersion;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		private InternalLocalizator localizator;
	}
}
