using System;
using System.ComponentModel;

namespace UnityEngine
{
	// Token: 0x0200002F RID: 47
	[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[ExcludeFromObjectFactory]
	[ExcludeFromPreset]
	public sealed class GUITexture
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0000C5B5 File Offset: 0x0000A7B5
		private static void FeatureRemoved()
		{
			throw new Exception("GUITexture has been removed from Unity. Use UI.Image instead.");
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
		// (set) Token: 0x0600037A RID: 890 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
		public Color color
		{
			get
			{
				GUITexture.FeatureRemoved();
				return new Color(0f, 0f, 0f);
			}
			set
			{
				GUITexture.FeatureRemoved();
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000C5FC File Offset: 0x0000A7FC
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
		public Texture texture
		{
			get
			{
				GUITexture.FeatureRemoved();
				return null;
			}
			set
			{
				GUITexture.FeatureRemoved();
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0000C618 File Offset: 0x0000A818
		// (set) Token: 0x0600037E RID: 894 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
		public Rect pixelInset
		{
			get
			{
				GUITexture.FeatureRemoved();
				return default(Rect);
			}
			set
			{
				GUITexture.FeatureRemoved();
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000C63C File Offset: 0x0000A83C
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
		public RectOffset border
		{
			get
			{
				GUITexture.FeatureRemoved();
				return null;
			}
			set
			{
				GUITexture.FeatureRemoved();
			}
		}
	}
}
