using System;
using UnityEngine.Serialization;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200001D RID: 29
	[ExcludeFromObjectFactory]
	[Serializable]
	public abstract class TextAsset : ScriptableObject
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000845C File Offset: 0x0000665C
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00008474 File Offset: 0x00006674
		public string version
		{
			get
			{
				return this.m_Version;
			}
			internal set
			{
				this.m_Version = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00008480 File Offset: 0x00006680
		public int instanceID
		{
			get
			{
				bool flag = this.m_InstanceID == 0;
				if (flag)
				{
					this.m_InstanceID = base.GetInstanceID();
				}
				return this.m_InstanceID;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000084B4 File Offset: 0x000066B4
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000084EA File Offset: 0x000066EA
		public int hashCode
		{
			get
			{
				bool flag = this.m_HashCode == 0;
				if (flag)
				{
					this.m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(base.name);
				}
				return this.m_HashCode;
			}
			set
			{
				this.m_HashCode = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000084F3 File Offset: 0x000066F3
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000084FB File Offset: 0x000066FB
		public Material material
		{
			get
			{
				return this.m_Material;
			}
			set
			{
				this.m_Material = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00008504 File Offset: 0x00006704
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00008555 File Offset: 0x00006755
		public int materialHashCode
		{
			get
			{
				bool flag = this.m_MaterialHashCode == 0;
				if (flag)
				{
					bool flag2 = this.m_Material == null;
					if (flag2)
					{
						return 0;
					}
					this.m_MaterialHashCode = TextUtilities.GetHashCodeCaseInSensitive(this.m_Material.name);
				}
				return this.m_MaterialHashCode;
			}
			set
			{
				this.m_MaterialHashCode = value;
			}
		}

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		internal string m_Version;

		// Token: 0x040000C9 RID: 201
		internal int m_InstanceID;

		// Token: 0x040000CA RID: 202
		internal int m_HashCode;

		// Token: 0x040000CB RID: 203
		[FormerlySerializedAs("material")]
		[SerializeField]
		internal Material m_Material;

		// Token: 0x040000CC RID: 204
		internal int m_MaterialHashCode;
	}
}
