using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CielOS
{
	// Token: 0x0200136D RID: 4973
	[CreateAssetMenu(fileName = "CielOSAppCatalog", menuName = "CielOS/App Catalog", order = 1)]
	public class CielOSAppCatalogSO : ScriptableObject
	{
		// Token: 0x06009A02 RID: 39426 RVA: 0x003F0FED File Offset: 0x003EF1ED
		public IEnumerable<CielOSAppDefinition> GetMenuApps()
		{
			foreach (CielOSAppDefinition cielOSAppDefinition in this.apps)
			{
				if (cielOSAppDefinition != null && !cielOSAppDefinition.hidden && cielOSAppDefinition.showInAppMenu && cielOSAppDefinition.IsVisible)
				{
					yield return cielOSAppDefinition;
				}
			}
			List<CielOSAppDefinition>.Enumerator enumerator = default(List<CielOSAppDefinition>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06009A03 RID: 39427 RVA: 0x003F0FFD File Offset: 0x003EF1FD
		public IEnumerable<CielOSAppDefinition> GetDesktopApps()
		{
			foreach (CielOSAppDefinition cielOSAppDefinition in this.apps)
			{
				if (cielOSAppDefinition != null && !cielOSAppDefinition.hidden && cielOSAppDefinition.showOnDesktop && cielOSAppDefinition.IsVisible)
				{
					yield return cielOSAppDefinition;
				}
			}
			List<CielOSAppDefinition>.Enumerator enumerator = default(List<CielOSAppDefinition>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06009A04 RID: 39428 RVA: 0x003F100D File Offset: 0x003EF20D
		public IEnumerable<CielOSAppDefinition> GetCustomApps()
		{
			foreach (CielOSAppDefinition cielOSAppDefinition in this.apps)
			{
				if (cielOSAppDefinition != null && !cielOSAppDefinition.hidden && cielOSAppDefinition.isCustomApp && cielOSAppDefinition.IsVisible)
				{
					yield return cielOSAppDefinition;
				}
			}
			List<CielOSAppDefinition>.Enumerator enumerator = default(List<CielOSAppDefinition>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06009A05 RID: 39429 RVA: 0x003F1020 File Offset: 0x003EF220
		public CielOSAppDefinition FindById(string appId)
		{
			if (string.IsNullOrEmpty(appId))
			{
				return null;
			}
			foreach (CielOSAppDefinition cielOSAppDefinition in this.apps)
			{
				if (cielOSAppDefinition != null && cielOSAppDefinition.appId == appId)
				{
					return cielOSAppDefinition;
				}
			}
			return null;
		}

		// Token: 0x04009EFB RID: 40699
		[Tooltip("Installed apps, in menu order.")]
		public List<CielOSAppDefinition> apps = new List<CielOSAppDefinition>();
	}
}
