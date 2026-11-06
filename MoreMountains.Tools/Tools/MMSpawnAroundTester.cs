using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x02000061 RID: 97
	public class MMSpawnAroundTester : MonoBehaviour
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0000A430 File Offset: 0x00008630
		public virtual void DebugSpawn()
		{
			for (int i = 0; i < this.DebugQuantity; i++)
			{
				this.Spawn();
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A454 File Offset: 0x00008654
		public virtual void Spawn()
		{
			this._gameObject = Object.Instantiate<GameObject>(this.ObjectToInstantiate);
			SceneManager.MoveGameObjectToScene(this._gameObject, base.gameObject.scene);
			MMSpawnAround.ApplySpawnAroundProperties(this._gameObject, this.SpawnProperties, base.transform.position);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A4A4 File Offset: 0x000086A4
		protected virtual void OnDrawGizmos()
		{
			if (this.DrawGizmos)
			{
				MMSpawnAround.DrawGizmos(this.SpawnProperties, base.transform.position, this.GizmosQuantity, this.GizmosSize, Color.gray);
			}
		}

		// Token: 0x040002EF RID: 751
		public GameObject ObjectToInstantiate;

		// Token: 0x040002F0 RID: 752
		public MMSpawnAroundProperties SpawnProperties;

		// Token: 0x040002F1 RID: 753
		[Header("Debug")]
		public int DebugQuantity = 10000;

		// Token: 0x040002F2 RID: 754
		[MMInspectorButton("DebugSpawn")]
		public bool DebugSpawnButton;

		// Token: 0x040002F3 RID: 755
		[Header("Gizmos")]
		public bool DrawGizmos;

		// Token: 0x040002F4 RID: 756
		public int GizmosQuantity = 1000;

		// Token: 0x040002F5 RID: 757
		public float GizmosSize = 1f;

		// Token: 0x040002F6 RID: 758
		protected GameObject _gameObject;
	}
}
