using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000014 RID: 20
	[RequireComponent(typeof(GPUIPrefabManager))]
	[DefaultExecutionOrder(-1000)]
	public class GPUIRuntimePrototypeDefiner : MonoBehaviour
	{
		// Token: 0x0600004C RID: 76 RVA: 0x000036D8 File Offset: 0x000018D8
		private void OnEnable()
		{
			this._prefabManager = base.GetComponent<GPUIPrefabManager>();
			if (this.prefabs == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.prefabs)
			{
				if (!(gameObject == null))
				{
					int num = this._prefabManager.GetPrototypeIndex(gameObject);
					if (num < 0)
					{
						num = this._prefabManager.AddPrototype(gameObject);
						if (num < 0)
						{
							string log_PREFIX = GPUIConstants.LOG_PREFIX;
							string text = "Add Prototype operation failed for prefab: ";
							GameObject gameObject2 = gameObject;
							Debug.LogError(log_PREFIX + text + ((gameObject2 != null) ? gameObject2.ToString() : null), gameObject);
							continue;
						}
					}
					this._prefabManager.GetPrototypeData(num).isAutoUpdateTransformData = this.enableTransformUpdates;
				}
			}
		}

		// Token: 0x04000063 RID: 99
		public List<GameObject> prefabs;

		// Token: 0x04000064 RID: 100
		public bool enableTransformUpdates;

		// Token: 0x04000065 RID: 101
		private GPUIPrefabManager _prefabManager;
	}
}
