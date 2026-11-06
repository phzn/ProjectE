using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000172 RID: 370
	[ExecuteInEditMode]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/unityreferencehelper.html")]
	public class UnityReferenceHelper : MonoBehaviour
	{
		// Token: 0x06000BA1 RID: 2977 RVA: 0x00040A00 File Offset: 0x0003EC00
		public string GetGUID()
		{
			return this.guid;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00040A08 File Offset: 0x0003EC08
		public void Awake()
		{
			this.Reset();
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00040A10 File Offset: 0x0003EC10
		public void Reset()
		{
			if (string.IsNullOrEmpty(this.guid))
			{
				this.guid = Guid.NewGuid().ToString();
				Debug.Log("Created new GUID - " + this.guid, this);
				return;
			}
			if (base.gameObject.scene.name != null)
			{
				foreach (UnityReferenceHelper unityReferenceHelper in UnityCompatibility.FindObjectsByTypeUnsorted<UnityReferenceHelper>())
				{
					if (unityReferenceHelper != this && this.guid == unityReferenceHelper.guid)
					{
						this.guid = Guid.NewGuid().ToString();
						Debug.Log("Created new GUID - " + this.guid, this);
						return;
					}
				}
			}
		}

		// Token: 0x04000792 RID: 1938
		[HideInInspector]
		[SerializeField]
		private string guid;
	}
}
