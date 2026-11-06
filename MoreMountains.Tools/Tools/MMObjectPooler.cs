using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x0200010F RID: 271
	public abstract class MMObjectPooler : MonoBehaviour
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x0001D0A8 File Offset: 0x0001B2A8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void InitializeStatics()
		{
			MMObjectPooler.Instance = null;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001D0B0 File Offset: 0x0001B2B0
		public static void AddPool(MMObjectPool pool)
		{
			if (MMObjectPooler._pools == null)
			{
				MMObjectPooler._pools = new List<MMObjectPool>(5);
			}
			if (!MMObjectPooler._pools.Contains(pool))
			{
				MMObjectPooler._pools.Add(pool);
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001D0DC File Offset: 0x0001B2DC
		public static void RemovePool(MMObjectPool pool)
		{
			List<MMObjectPool> pools = MMObjectPooler._pools;
			if (pools == null)
			{
				return;
			}
			pools.Remove(pool);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001D0EF File Offset: 0x0001B2EF
		protected virtual void Awake()
		{
			MMObjectPooler.Instance = this;
			this.FillObjectPool();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001D100 File Offset: 0x0001B300
		protected virtual bool CreateWaitingPool()
		{
			if (!this.MutualizeWaitingPools)
			{
				this._waitingPool = new GameObject(this.DetermineObjectPoolName());
				SceneManager.MoveGameObjectToScene(this._waitingPool, base.gameObject.scene);
				this._objectPool = this._waitingPool.AddComponent<MMObjectPool>();
				this._objectPool.PooledGameObjects = new List<GameObject>();
				this.ApplyNesting();
				return true;
			}
			MMObjectPool mmobjectPool = this.ExistingPool(this.DetermineObjectPoolName());
			if (mmobjectPool != null)
			{
				this._objectPool = mmobjectPool;
				this._waitingPool = mmobjectPool.gameObject;
				return false;
			}
			this._waitingPool = new GameObject(this.DetermineObjectPoolName());
			SceneManager.MoveGameObjectToScene(this._waitingPool, base.gameObject.scene);
			this._objectPool = this._waitingPool.AddComponent<MMObjectPool>();
			this._objectPool.PooledGameObjects = new List<GameObject>();
			this.ApplyNesting();
			MMObjectPooler.AddPool(this._objectPool);
			return true;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001D1EC File Offset: 0x0001B3EC
		public virtual MMObjectPool ExistingPool(string poolName)
		{
			if (MMObjectPooler._pools == null)
			{
				MMObjectPooler._pools = new List<MMObjectPool>(5);
			}
			if (MMObjectPooler._pools.Count == 0)
			{
				MMObjectPool[] array = Object.FindObjectsOfType<MMObjectPool>();
				if (array.Length != 0)
				{
					MMObjectPooler._pools.AddRange(array);
				}
			}
			foreach (MMObjectPool mmobjectPool in MMObjectPooler._pools)
			{
				if (mmobjectPool != null && mmobjectPool.name == poolName)
				{
					return mmobjectPool;
				}
			}
			return null;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001D288 File Offset: 0x0001B488
		protected virtual void ApplyNesting()
		{
			if (this.NestWaitingPool && this.NestUnderThis && this._waitingPool != null)
			{
				this._waitingPool.transform.SetParent(base.transform);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001D2BE File Offset: 0x0001B4BE
		protected virtual string DetermineObjectPoolName()
		{
			return "[ObjectPooler] " + base.name;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001D2D0 File Offset: 0x0001B4D0
		public virtual void FillObjectPool()
		{
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001D2D2 File Offset: 0x0001B4D2
		public virtual GameObject GetPooledGameObject()
		{
			return null;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001D2D5 File Offset: 0x0001B4D5
		public virtual void DestroyObjectPool()
		{
			if (this._waitingPool != null)
			{
				Object.Destroy(this._waitingPool.gameObject);
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001D2F5 File Offset: 0x0001B4F5
		protected virtual void OnEnable()
		{
			if (!this._onSceneLoadedRegistered)
			{
				SceneManager.sceneLoaded += this.OnSceneLoaded;
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001D310 File Offset: 0x0001B510
		private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (this == null)
			{
				return;
			}
			if ((this._objectPool == null || this._waitingPool == null) && this != null)
			{
				this.FillObjectPool();
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001D348 File Offset: 0x0001B548
		private void OnDestroy()
		{
			if (this._objectPool != null && this.NestUnderThis)
			{
				MMObjectPooler.RemovePool(this._objectPool);
			}
			if (this._onSceneLoadedRegistered)
			{
				SceneManager.sceneLoaded -= this.OnSceneLoaded;
				this._onSceneLoadedRegistered = false;
			}
		}

		// Token: 0x04000711 RID: 1809
		public static MMObjectPooler Instance;

		// Token: 0x04000712 RID: 1810
		public bool MutualizeWaitingPools;

		// Token: 0x04000713 RID: 1811
		public bool NestWaitingPool = true;

		// Token: 0x04000714 RID: 1812
		[MMCondition("NestWaitingPool", true)]
		public bool NestUnderThis;

		// Token: 0x04000715 RID: 1813
		protected GameObject _waitingPool;

		// Token: 0x04000716 RID: 1814
		protected MMObjectPool _objectPool;

		// Token: 0x04000717 RID: 1815
		protected const int _initialPoolsListCapacity = 5;

		// Token: 0x04000718 RID: 1816
		protected bool _onSceneLoadedRegistered;

		// Token: 0x04000719 RID: 1817
		public static List<MMObjectPool> _pools = new List<MMObjectPool>(5);
	}
}
