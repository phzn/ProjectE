using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED6 RID: 3798
	public sealed class NPCCustomRuntimeResources : MonoBehaviour
	{
		// Token: 0x060071BC RID: 29116 RVA: 0x002E3690 File Offset: 0x002E1890
		public void Capture()
		{
			Outfit[] componentsInChildren = base.GetComponentsInChildren<Outfit>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].PreserveStateWhilePooled = true;
			}
			Renderer[] componentsInChildren2 = base.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				foreach (Material material in componentsInChildren2[i].sharedMaterials)
				{
					if (material != null && material.name.EndsWith(" (Instance)", StringComparison.Ordinal))
					{
						this._materials.Add(material);
					}
				}
			}
		}

		// Token: 0x060071BD RID: 29117 RVA: 0x002E3720 File Offset: 0x002E1920
		private void OnDestroy()
		{
			this.Capture();
			foreach (Material material in this._materials)
			{
				if (material != null)
				{
					Object.Destroy(material);
				}
			}
			this._materials.Clear();
		}

		// Token: 0x040074A7 RID: 29863
		private readonly HashSet<Material> _materials = new HashSet<Material>();
	}
}
