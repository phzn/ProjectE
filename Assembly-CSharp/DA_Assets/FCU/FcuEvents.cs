using System;
using DA_Assets.DAI;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.Events;

namespace DA_Assets.FCU
{
	// Token: 0x020002C5 RID: 709
	[Serializable]
	public class FcuEvents : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x0007FA26 File Offset: 0x0007DC26
		[SerializeProperty("onProjectDownloadFail")]
		public UnityEvent<FigmaConverterUnity> OnProjectDownloadFail
		{
			get
			{
				return this.onProjectDownloadFail;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x0007FA2E File Offset: 0x0007DC2E
		[SerializeProperty("onProjectDownloadStart")]
		public UnityEvent<FigmaConverterUnity> OnProjectDownloadStart
		{
			get
			{
				return this.onProjectDownloadStart;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x0007FA36 File Offset: 0x0007DC36
		[SerializeProperty("onProjectDownloaded")]
		public UnityEvent<FigmaConverterUnity> OnProjectDownloaded
		{
			get
			{
				return this.onProjectDownloaded;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x0007FA3E File Offset: 0x0007DC3E
		[SerializeProperty("onImportStart")]
		public UnityEvent<FigmaConverterUnity> OnImportStart
		{
			get
			{
				return this.onImportStart;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0007FA46 File Offset: 0x0007DC46
		[SerializeProperty("onImportComplete")]
		public UnityEvent<FigmaConverterUnity> OnImportComplete
		{
			get
			{
				return this.onImportComplete;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0007FA4E File Offset: 0x0007DC4E
		[SerializeProperty("onImportFail")]
		public UnityEvent<FigmaConverterUnity> OnImportFail
		{
			get
			{
				return this.onImportFail;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0007FA56 File Offset: 0x0007DC56
		[SerializeProperty("onObjectInstantiate")]
		public UnityEvent<FigmaConverterUnity, FObject> OnObjectInstantiate
		{
			get
			{
				return this.onObjectInstantiate;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x0007FA5E File Offset: 0x0007DC5E
		[SerializeProperty("onAddComponent")]
		public UnityEvent<FigmaConverterUnity, FObject, FcuTag> OnAddComponent
		{
			get
			{
				return this.onAddComponent;
			}
		}

		// Token: 0x04001595 RID: 5525
		[SerializeField]
		private UnityEvent<FigmaConverterUnity> onProjectDownloadFail;

		// Token: 0x04001596 RID: 5526
		[SerializeField]
		private UnityEvent<FigmaConverterUnity> onProjectDownloadStart;

		// Token: 0x04001597 RID: 5527
		[SerializeField]
		private UnityEvent<FigmaConverterUnity> onProjectDownloaded;

		// Token: 0x04001598 RID: 5528
		[SerializeField]
		private UnityEvent<FigmaConverterUnity> onImportStart;

		// Token: 0x04001599 RID: 5529
		[SerializeField]
		private UnityEvent<FigmaConverterUnity> onImportComplete;

		// Token: 0x0400159A RID: 5530
		[SerializeField]
		private UnityEvent<FigmaConverterUnity> onImportFail;

		// Token: 0x0400159B RID: 5531
		[SerializeField]
		private UnityEvent<FigmaConverterUnity, FObject> onObjectInstantiate;

		// Token: 0x0400159C RID: 5532
		[SerializeField]
		private UnityEvent<FigmaConverterUnity, FObject, FcuTag> onAddComponent;
	}
}
