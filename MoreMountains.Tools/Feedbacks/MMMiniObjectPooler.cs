using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C9 RID: 457
	public class MMMiniObjectPooler : MonoBehaviour
	{
		// Token: 0x06000B7C RID: 2940 RVA: 0x00032AD4 File Offset: 0x00030CD4
		public static void AddPool(MMMiniObjectPool pool)
		{
			if (MMMiniObjectPooler._pools == null)
			{
				MMMiniObjectPooler._pools = new List<MMMiniObjectPool>(5);
			}
			if (!MMMiniObjectPooler._pools.Contains(pool))
			{
				MMMiniObjectPooler._pools.Add(pool);
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00032B00 File Offset: 0x00030D00
		public static void RemovePool(MMMiniObjectPool pool)
		{
			List<MMMiniObjectPool> pools = MMMiniObjectPooler._pools;
			if (pools == null)
			{
				return;
			}
			pools.Remove(pool);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00032B13 File Offset: 0x00030D13
		protected virtual void Awake()
		{
			this.FillObjectPool();
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00032B1B File Offset: 0x00030D1B
		private void OnDestroy()
		{
			if (this._objectPool != null)
			{
				MMMiniObjectPooler.RemovePool(this._objectPool);
			}
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00032B38 File Offset: 0x00030D38
		public virtual MMMiniObjectPool ExistingPool(string poolName)
		{
			if (MMMiniObjectPooler._pools == null)
			{
				MMMiniObjectPooler._pools = new List<MMMiniObjectPool>(5);
			}
			if (MMMiniObjectPooler._pools.Count == 0)
			{
				MMMiniObjectPool[] array = Object.FindObjectsOfType<MMMiniObjectPool>();
				if (array.Length != 0)
				{
					MMMiniObjectPooler._pools.AddRange(array);
				}
			}
			foreach (MMMiniObjectPool mmminiObjectPool in MMMiniObjectPooler._pools)
			{
				if (mmminiObjectPool != null && mmminiObjectPool.name == poolName)
				{
					return mmminiObjectPool;
				}
			}
			return null;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00032BD4 File Offset: 0x00030DD4
		protected virtual void CreateWaitingPool()
		{
			if (!this.MutualizeWaitingPools)
			{
				this._objectPool = base.gameObject.AddComponent<MMMiniObjectPool>();
				this._objectPool.PooledGameObjects = new List<GameObject>();
				return;
			}
			MMMiniObjectPool mmminiObjectPool = this.ExistingPool(MMMiniObjectPooler.DetermineObjectPoolName(this.GameObjectToPool));
			if (mmminiObjectPool != null)
			{
				this._waitingPool = mmminiObjectPool.gameObject;
				this._objectPool = mmminiObjectPool;
				return;
			}
			GameObject gameObject = new GameObject();
			gameObject.name = MMMiniObjectPooler.DetermineObjectPoolName(this.GameObjectToPool);
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			this._objectPool = gameObject.AddComponent<MMMiniObjectPool>();
			this._objectPool.PooledGameObjects = new List<GameObject>();
			MMMiniObjectPooler.AddPool(this._objectPool);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00032C89 File Offset: 0x00030E89
		public static string DetermineObjectPoolName(GameObject gameObjectToPool)
		{
			return gameObjectToPool.name + "_pool";
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00032C9C File Offset: 0x00030E9C
		public virtual void FillObjectPool()
		{
			if (this.GameObjectToPool == null)
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

		// Token: 0x06000B84 RID: 2948 RVA: 0x00032CFC File Offset: 0x00030EFC
		public virtual GameObject GetPooledGameObject()
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

		// Token: 0x06000B85 RID: 2949 RVA: 0x00032D64 File Offset: 0x00030F64
		protected virtual GameObject AddOneObjectToThePool()
		{
			if (this.GameObjectToPool == null)
			{
				Debug.LogWarning("The " + base.gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", base.gameObject);
				return null;
			}
			this.GameObjectToPool.gameObject.SetActive(false);
			GameObject gameObject = Object.Instantiate<GameObject>(this.GameObjectToPool);
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			if (this.NestWaitingPool)
			{
				gameObject.transform.SetParent(this._objectPool.transform);
			}
			gameObject.name = this.GameObjectToPool.name + "-" + this._objectPool.PooledGameObjects.Count.ToString();
			this._objectPool.PooledGameObjects.Add(gameObject);
			return gameObject;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00032E37 File Offset: 0x00031037
		public virtual void DestroyObjectPool()
		{
			if (this._waitingPool != null)
			{
				Object.Destroy(this._waitingPool.gameObject);
			}
		}

		// Token: 0x04000D25 RID: 3365
		public GameObject GameObjectToPool;

		// Token: 0x04000D26 RID: 3366
		public int PoolSize = 20;

		// Token: 0x04000D27 RID: 3367
		public bool PoolCanExpand = true;

		// Token: 0x04000D28 RID: 3368
		public bool MutualizeWaitingPools;

		// Token: 0x04000D29 RID: 3369
		public bool NestWaitingPool = true;

		// Token: 0x04000D2A RID: 3370
		protected GameObject _waitingPool;

		// Token: 0x04000D2B RID: 3371
		protected MMMiniObjectPool _objectPool;

		// Token: 0x04000D2C RID: 3372
		protected const int _initialPoolsListCapacity = 5;

		// Token: 0x04000D2D RID: 3373
		private static List<MMMiniObjectPool> _pools = new List<MMMiniObjectPool>(5);
	}
}
