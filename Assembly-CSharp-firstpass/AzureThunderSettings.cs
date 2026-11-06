using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x0200001F RID: 31
	[Serializable]
	public sealed class AzureThunderSettings
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00004DAE File Offset: 0x00002FAE
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00004DB6 File Offset: 0x00002FB6
		public Transform transform
		{
			get
			{
				return this.m_transform;
			}
			set
			{
				this.m_transform = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00004DBF File Offset: 0x00002FBF
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00004DC7 File Offset: 0x00002FC7
		public Vector3 position
		{
			get
			{
				return this.m_position;
			}
			set
			{
				this.m_position = value;
			}
		}

		// Token: 0x0400011B RID: 283
		[SerializeField]
		private Transform m_transform;

		// Token: 0x0400011C RID: 284
		[SerializeField]
		private Vector3 m_position = Vector3.zero;
	}
}
