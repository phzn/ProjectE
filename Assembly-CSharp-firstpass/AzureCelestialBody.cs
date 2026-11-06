using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public sealed class AzureCelestialBody
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000022D4 File Offset: 0x000004D4
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000022DC File Offset: 0x000004DC
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000022E5 File Offset: 0x000004E5
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000022ED File Offset: 0x000004ED
		public AzureCelestialBodyType type
		{
			get
			{
				return this.m_type;
			}
			set
			{
				this.m_type = value;
			}
		}

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private Transform m_transform;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private AzureCelestialBodyType m_type;
	}
}
