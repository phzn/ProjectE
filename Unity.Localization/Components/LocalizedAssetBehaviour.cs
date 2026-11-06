using System;

namespace UnityEngine.Localization.Components
{
	// Token: 0x020000CE RID: 206
	[ExecuteAlways]
	public abstract class LocalizedAssetBehaviour<TObject, TReference> : LocalizedMonoBehaviour where TObject : Object where TReference : LocalizedAsset<TObject>, new()
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00014858 File Offset: 0x00012A58
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x00014860 File Offset: 0x00012A60
		public TReference AssetReference
		{
			get
			{
				return this.m_LocalizedAssetReference;
			}
			set
			{
				this.ClearChangeHandler();
				this.m_LocalizedAssetReference = value;
				if (base.isActiveAndEnabled)
				{
					this.RegisterChangeHandler();
				}
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001487D File Offset: 0x00012A7D
		protected virtual void OnEnable()
		{
			this.RegisterChangeHandler();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00014885 File Offset: 0x00012A85
		protected virtual void OnDisable()
		{
			this.ClearChangeHandler();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001488D File Offset: 0x00012A8D
		private void OnDestroy()
		{
			this.ClearChangeHandler();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00014895 File Offset: 0x00012A95
		private void OnValidate()
		{
			TReference treference = this.AssetReference;
			if (treference == null)
			{
				return;
			}
			treference.ForceUpdate();
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000148AC File Offset: 0x00012AAC
		internal virtual void RegisterChangeHandler()
		{
			if (this.AssetReference == null)
			{
				return;
			}
			if (this.m_ChangeHandler == null)
			{
				this.m_ChangeHandler = new LocalizedAsset<TObject>.ChangeHandler(this.UpdateAsset);
			}
			this.AssetReference.AssetChanged += this.m_ChangeHandler;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000148F8 File Offset: 0x00012AF8
		internal virtual void ClearChangeHandler()
		{
			if (this.AssetReference != null)
			{
				this.AssetReference.AssetChanged -= this.m_ChangeHandler;
			}
		}

		// Token: 0x060005BF RID: 1471
		protected abstract void UpdateAsset(TObject localizedAsset);

		// Token: 0x040001F1 RID: 497
		[SerializeField]
		private TReference m_LocalizedAssetReference = new TReference();

		// Token: 0x040001F2 RID: 498
		private LocalizedAsset<TObject>.ChangeHandler m_ChangeHandler;
	}
}
