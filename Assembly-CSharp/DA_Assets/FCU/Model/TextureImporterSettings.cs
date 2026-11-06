using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000376 RID: 886
	[FcuPropertyHeader(FcuLocKey.label_texture_importer_settings, FcuLocKey.tooltip_texture_importer_settings)]
	[Serializable]
	public class TextureImporterSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00096D23 File Offset: 0x00094F23
		// (set) Token: 0x06001723 RID: 5923 RVA: 0x00096D2B File Offset: 0x00094F2B
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_crunched_compression, FcuLocKey.tooltip_crunched_compression, 0f, 1f)]
		public bool CrunchedCompression
		{
			get
			{
				return this.crunchedCompression;
			}
			set
			{
				base.SetValue<bool>(ref this.crunchedCompression, value);
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00096D3A File Offset: 0x00094F3A
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x00096D42 File Offset: 0x00094F42
		[FcuInspectorProperty(ComponentType.SliderField, FcuLocKey.label_compression_quality, FcuLocKey.tooltip_compression_quality, 0f, 100f)]
		public int CompressionQuality
		{
			get
			{
				return this.compressionQuality;
			}
			set
			{
				base.SetValue<int>(ref this.compressionQuality, value);
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x00096D51 File Offset: 0x00094F51
		// (set) Token: 0x06001727 RID: 5927 RVA: 0x00096D59 File Offset: 0x00094F59
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_is_readable, FcuLocKey.tooltip_is_readable, 0f, 1f)]
		public bool IsReadable
		{
			get
			{
				return this.isReadable;
			}
			set
			{
				base.SetValue<bool>(ref this.isReadable, value);
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x00096D68 File Offset: 0x00094F68
		// (set) Token: 0x06001729 RID: 5929 RVA: 0x00096D70 File Offset: 0x00094F70
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_mipmap_enabled, FcuLocKey.tooltip_mipmap_enabled, 0f, 1f)]
		public bool MipmapEnabled
		{
			get
			{
				return this.mipmapEnabled;
			}
			set
			{
				base.SetValue<bool>(ref this.mipmapEnabled, value);
			}
		}

		// Token: 0x040018D6 RID: 6358
		[SerializeField]
		private bool crunchedCompression;

		// Token: 0x040018D7 RID: 6359
		[SerializeField]
		private int compressionQuality = 100;

		// Token: 0x040018D8 RID: 6360
		[SerializeField]
		private bool isReadable = true;

		// Token: 0x040018D9 RID: 6361
		[SerializeField]
		private bool mipmapEnabled;
	}
}
