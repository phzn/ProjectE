using System;
using UnityEngine.Events;

namespace UnityEngine.Localization.Components
{
	// Token: 0x020000CF RID: 207
	public class LocalizedAssetEvent<TObject, TReference, TEvent> : LocalizedAssetBehaviour<TObject, TReference> where TObject : Object where TReference : LocalizedAsset<TObject>, new() where TEvent : UnityEvent<TObject>, new()
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00014930 File Offset: 0x00012B30
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x00014938 File Offset: 0x00012B38
		public TEvent OnUpdateAsset
		{
			get
			{
				return this.m_UpdateAsset;
			}
			set
			{
				this.m_UpdateAsset = value;
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00014941 File Offset: 0x00012B41
		protected override void UpdateAsset(TObject localizedAsset)
		{
			this.OnUpdateAsset.Invoke(localizedAsset);
		}

		// Token: 0x040001F3 RID: 499
		[SerializeField]
		private TEvent m_UpdateAsset = new TEvent();
	}
}
