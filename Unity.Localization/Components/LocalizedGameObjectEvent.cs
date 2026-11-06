using System;
using UnityEngine.Localization.Events;

namespace UnityEngine.Localization.Components
{
	// Token: 0x020000D0 RID: 208
	[AddComponentMenu("Localization/Asset/Localize Prefab Event")]
	public class LocalizedGameObjectEvent : LocalizedAssetEvent<GameObject, LocalizedGameObject, UnityEventGameObject>
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x00014968 File Offset: 0x00012B68
		protected override void UpdateAsset(GameObject localizedAsset)
		{
			if (this.m_Current != null)
			{
				Object.Destroy(this.m_Current);
				this.m_Current = null;
			}
			if (localizedAsset != null)
			{
				this.m_Current = Object.Instantiate<GameObject>(localizedAsset, base.transform);
				this.m_Current.hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
			}
			base.OnUpdateAsset.Invoke(this.m_Current);
		}

		// Token: 0x040001F4 RID: 500
		private GameObject m_Current;
	}
}
