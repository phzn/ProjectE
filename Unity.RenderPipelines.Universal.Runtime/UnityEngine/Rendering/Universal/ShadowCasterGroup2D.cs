using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200003A RID: 58
	[MovedFrom("UnityEngine.Experimental.Rendering.Universal")]
	public abstract class ShadowCasterGroup2D : MonoBehaviour
	{
		// Token: 0x06000247 RID: 583 RVA: 0x00011F38 File Offset: 0x00010138
		internal virtual void CacheValues()
		{
			if (this.m_ShadowCasters != null)
			{
				for (int i = 0; i < this.m_ShadowCasters.Count; i++)
				{
					this.m_ShadowCasters[i].CacheValues();
				}
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00011F74 File Offset: 0x00010174
		public List<ShadowCaster2D> GetShadowCasters()
		{
			return this.m_ShadowCasters;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00011F7C File Offset: 0x0001017C
		public int GetShadowGroup()
		{
			return this.m_ShadowGroup;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00011F84 File Offset: 0x00010184
		public void RegisterShadowCaster2D(ShadowCaster2D shadowCaster2D)
		{
			if (this.m_ShadowCasters == null)
			{
				this.m_ShadowCasters = new List<ShadowCaster2D>();
			}
			this.m_ShadowCasters.Add(shadowCaster2D);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00011FA5 File Offset: 0x000101A5
		public void UnregisterShadowCaster2D(ShadowCaster2D shadowCaster2D)
		{
			if (this.m_ShadowCasters != null)
			{
				this.m_ShadowCasters.Remove(shadowCaster2D);
			}
		}

		// Token: 0x04000192 RID: 402
		[SerializeField]
		internal int m_ShadowGroup;

		// Token: 0x04000193 RID: 403
		private List<ShadowCaster2D> m_ShadowCasters;
	}
}
