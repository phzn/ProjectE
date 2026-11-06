using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x02000111 RID: 273
	[AddComponentMenu("More Mountains/Tools/Object Pool/MMSimpleObjectPooler")]
	public class MMSimpleObjectPooler : MMObjectPooler
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001D4C0 File Offset: 0x0001B6C0
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0001D4C8 File Offset: 0x0001B6C8
		public virtual List<MMSimpleObjectPooler> Owner { get; set; }

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001D4D1 File Offset: 0x0001B6D1
		private void OnDestroy()
		{
			List<MMSimpleObjectPooler> owner = this.Owner;
			if (owner == null)
			{
				return;
			}
			owner.Remove(this);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
		public override void FillObjectPool()
		{
			if (this.GameObjectToPool == null)
			{
				return;
			}
			if (this._objectPool != null && this._objectPool.PooledGameObjects.Count > this.PoolSize)
			{
				return;
			}
			this.CreateWaitingPool();
			int num = this.PoolSize;
			if (this._objectPool != null)
			{
				num -= this._objectPool.PooledGameObjects.Count;
			}
			for (int i = 0; i < num; i++)
			{
				this.AddOneObjectToThePool();
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001D56D File Offset: 0x0001B76D
		protected override string DetermineObjectPoolName()
		{
			return "[SimpleObjectPooler] " + this.GameObjectToPool.name;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001D584 File Offset: 0x0001B784
		public override GameObject GetPooledGameObject()
		{
			for (int i = 0; i < this._objectPool.PooledGameObjects.Count; i++)
			{
				if (!this._objectPool.PooledGameObjects[i].gameObject.activeInHierarchy)
				{
					return this._objectPool.PooledGameObjects[i];
				}
			}
			if (this.PoolCanExpand)
			{
				return this.AddOneObjectToThePool();
			}
			return null;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001D5EC File Offset: 0x0001B7EC
		protected virtual GameObject AddOneObjectToThePool()
		{
			if (this.GameObjectToPool == null)
			{
				Debug.LogWarning("The " + base.gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", base.gameObject);
				return null;
			}
			bool activeSelf = this.GameObjectToPool.activeSelf;
			this.GameObjectToPool.SetActive(false);
			GameObject gameObject = Object.Instantiate<GameObject>(this.GameObjectToPool);
			this.GameObjectToPool.SetActive(activeSelf);
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			if (this.NestWaitingPool)
			{
				gameObject.transform.SetParent(this._waitingPool.transform);
			}
			gameObject.name = this.GameObjectToPool.name + "-" + this._objectPool.PooledGameObjects.Count.ToString();
			this._objectPool.PooledGameObjects.Add(gameObject);
			return gameObject;
		}

		// Token: 0x0400071E RID: 1822
		public GameObject GameObjectToPool;

		// Token: 0x0400071F RID: 1823
		public int PoolSize = 20;

		// Token: 0x04000720 RID: 1824
		public bool PoolCanExpand = true;
	}
}
