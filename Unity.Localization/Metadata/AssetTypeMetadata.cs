using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000D9 RID: 217
	[HideInInspector]
	[Serializable]
	internal class AssetTypeMetadata : SharedTableCollectionMetadata
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00014CC2 File Offset: 0x00012EC2
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x00014CCA File Offset: 0x00012ECA
		public Type Type { get; set; }

		// Token: 0x060005F0 RID: 1520 RVA: 0x00014CD3 File Offset: 0x00012ED3
		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();
			Type type = this.Type;
			this.m_TypeString = ((type != null) ? type.AssemblyQualifiedName : null);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00014CF3 File Offset: 0x00012EF3
		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();
			if (!string.IsNullOrEmpty(this.m_TypeString))
			{
				this.Type = Type.GetType(this.m_TypeString);
			}
		}

		// Token: 0x0400020E RID: 526
		[SerializeField]
		[HideInInspector]
		internal string m_TypeString;
	}
}
