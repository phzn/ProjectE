using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x0200010C RID: 268
	[AddComponentMenu("More Mountains/Tools/Object Pool/MMMultipleObjectPooler")]
	public class MMMultipleObjectPooler : MMObjectPooler
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001C379 File Offset: 0x0001A579
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0001C381 File Offset: 0x0001A581
		public virtual List<MMMultipleObjectPooler> Owner { get; set; }

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001C38A File Offset: 0x0001A58A
		private void OnDestroy()
		{
			List<MMMultipleObjectPooler> owner = this.Owner;
			if (owner == null)
			{
				return;
			}
			owner.Remove(this);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001C39E File Offset: 0x0001A59E
		protected override string DetermineObjectPoolName()
		{
			if (this.MutualizedPoolName == null || this.MutualizedPoolName == "")
			{
				return "[MultipleObjectPooler] " + base.name;
			}
			return "[MultipleObjectPooler] " + this.MutualizedPoolName;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001C3DC File Offset: 0x0001A5DC
		public override void FillObjectPool()
		{
			if (this.Pool == null || this.Pool.Count == 0)
			{
				return;
			}
			if (!this.CreateWaitingPool())
			{
				return;
			}
			if (this.Pool.Count <= 1)
			{
				this.CanPoolSameObjectTwice = true;
			}
			MMPoolingMethods poolingMethod = this.PoolingMethod;
			if (poolingMethod != MMPoolingMethods.OriginalOrder)
			{
				if (poolingMethod == MMPoolingMethods.OriginalOrderSequential)
				{
					using (List<MMMultipleObjectPoolerObject>.Enumerator enumerator = this.Pool.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MMMultipleObjectPoolerObject mmmultipleObjectPoolerObject = enumerator.Current;
							for (int i = 0; i < mmmultipleObjectPoolerObject.PoolSize; i++)
							{
								this.AddOneObjectToThePool(mmmultipleObjectPoolerObject.GameObjectToPool);
							}
						}
						return;
					}
				}
				int num = 0;
				foreach (MMMultipleObjectPoolerObject mmmultipleObjectPoolerObject2 in this.Pool)
				{
					if (num > this.Pool.Count)
					{
						break;
					}
					for (int j = 0; j < this.Pool[num].PoolSize; j++)
					{
						this.AddOneObjectToThePool(mmmultipleObjectPoolerObject2.GameObjectToPool);
					}
					num++;
				}
				return;
			}
			bool flag = true;
			int[] array = new int[this.Pool.Count];
			for (int k = 0; k < this.Pool.Count; k++)
			{
				array[k] = this.Pool[k].PoolSize;
			}
			while (flag)
			{
				flag = false;
				for (int l = 0; l < this.Pool.Count; l++)
				{
					if (array[l] > 0)
					{
						this.AddOneObjectToThePool(this.Pool[l].GameObjectToPool);
						array[l]--;
						flag = true;
					}
				}
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001C5B0 File Offset: 0x0001A7B0
		protected virtual GameObject AddOneObjectToThePool(GameObject typeOfObject)
		{
			if (typeOfObject == null)
			{
				return null;
			}
			bool activeSelf = typeOfObject.activeSelf;
			typeOfObject.SetActive(false);
			GameObject gameObject = Object.Instantiate<GameObject>(typeOfObject);
			typeOfObject.SetActive(activeSelf);
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			if (this.NestWaitingPool)
			{
				gameObject.transform.SetParent(this._waitingPool.transform);
			}
			gameObject.name = typeOfObject.name;
			this._objectPool.PooledGameObjects.Add(gameObject);
			return gameObject;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001C634 File Offset: 0x0001A834
		public override GameObject GetPooledGameObject()
		{
			GameObject gameObject;
			switch (this.PoolingMethod)
			{
			case MMPoolingMethods.OriginalOrder:
				gameObject = this.GetPooledGameObjectOriginalOrder();
				break;
			case MMPoolingMethods.OriginalOrderSequential:
				gameObject = this.GetPooledGameObjectOriginalOrderSequential();
				break;
			case MMPoolingMethods.RandomBetweenObjects:
				gameObject = this.GetPooledGameObjectRandomBetweenObjects();
				break;
			case MMPoolingMethods.RandomPoolSizeBased:
				gameObject = this.GetPooledGameObjectPoolSizeBased();
				break;
			default:
				gameObject = null;
				break;
			}
			if (gameObject != null)
			{
				this._lastPooledObject = gameObject;
			}
			else
			{
				this._lastPooledObject = null;
			}
			return gameObject;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001C6A0 File Offset: 0x0001A8A0
		protected virtual GameObject GetPooledGameObjectOriginalOrder()
		{
			if (this._currentIndex >= this.Pool.Count)
			{
				this.ResetCurrentIndex();
			}
			MMMultipleObjectPoolerObject poolObject = this.GetPoolObject(this.Pool[this._currentIndex].GameObjectToPool);
			if (this._currentIndex >= this._objectPool.PooledGameObjects.Count)
			{
				return null;
			}
			if (!poolObject.Enabled)
			{
				this._currentIndex++;
				return null;
			}
			if (!this._objectPool.PooledGameObjects[this._currentIndex].gameObject.activeInHierarchy)
			{
				int currentIndex = this._currentIndex;
				this._currentIndex++;
				return this._objectPool.PooledGameObjects[currentIndex];
			}
			GameObject gameObject = this.FindInactiveObject(this._objectPool.PooledGameObjects[this._currentIndex].gameObject.name, this._objectPool.PooledGameObjects);
			if (gameObject != null)
			{
				this._currentIndex++;
				return gameObject;
			}
			if (poolObject.PoolCanExpand)
			{
				this._currentIndex++;
				return this.AddOneObjectToThePool(poolObject.GameObjectToPool);
			}
			return null;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001C7D0 File Offset: 0x0001A9D0
		protected virtual GameObject GetPooledGameObjectOriginalOrderSequential()
		{
			if (this._currentIndex >= this.Pool.Count)
			{
				this._currentCount = 0;
				this.ResetCurrentIndex();
			}
			MMMultipleObjectPoolerObject poolObject = this.GetPoolObject(this.Pool[this._currentIndex].GameObjectToPool);
			if (this._currentIndex >= this._objectPool.PooledGameObjects.Count)
			{
				return null;
			}
			if (!poolObject.Enabled)
			{
				this._currentIndex++;
				this._currentCount = 0;
				return null;
			}
			if (!this._objectPool.PooledGameObjects[this._currentIndex].gameObject.activeInHierarchy)
			{
				this._currentCount++;
				this.OrderSequentialResetCounter(poolObject);
				return this._objectPool.PooledGameObjects[this._currentIndex];
			}
			GameObject gameObject = this.FindInactiveObject(this.Pool[this._currentIndex].GameObjectToPool.name, this._objectPool.PooledGameObjects);
			if (gameObject != null)
			{
				this._currentCount++;
				this.OrderSequentialResetCounter(poolObject);
				return gameObject;
			}
			if (poolObject.PoolCanExpand)
			{
				this._currentCount++;
				this.OrderSequentialResetCounter(poolObject);
				return this.AddOneObjectToThePool(poolObject.GameObjectToPool);
			}
			this._currentIndex++;
			this._currentCount = 0;
			return null;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001C931 File Offset: 0x0001AB31
		protected virtual void OrderSequentialResetCounter(MMMultipleObjectPoolerObject searchedObject)
		{
			if (this._currentCount >= searchedObject.PoolSize)
			{
				this._currentIndex++;
				this._currentCount = 0;
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001C958 File Offset: 0x0001AB58
		protected virtual GameObject GetPooledGameObjectPoolSizeBased()
		{
			int num = Random.Range(0, this._objectPool.PooledGameObjects.Count);
			int num2 = 0;
			while (!this.PoolObjectEnabled(this._objectPool.PooledGameObjects[num]) && num2 < this._objectPool.PooledGameObjects.Count)
			{
				num = Random.Range(0, this._objectPool.PooledGameObjects.Count);
				num2++;
			}
			if (!this.PoolObjectEnabled(this._objectPool.PooledGameObjects[num]))
			{
				return null;
			}
			num2 = 0;
			if (this._lastPooledObject != null)
			{
				while (!this.CanPoolSameObjectTwice && (this._objectPool.PooledGameObjects[num].name == this._lastPooledObject.name || !this.PoolObjectEnabled(this._objectPool.PooledGameObjects[num])) && (float)num2 < (float)this._objectPool.PooledGameObjects.Count * this.OverflowAmount)
				{
					num = Random.Range(0, this._objectPool.PooledGameObjects.Count);
					num2++;
				}
			}
			if (!this._objectPool.PooledGameObjects[num].gameObject.activeInHierarchy)
			{
				return this._objectPool.PooledGameObjects[num];
			}
			GameObject gameObject = this.FindInactiveObject(this._objectPool.PooledGameObjects[num].gameObject.name, this._objectPool.PooledGameObjects);
			if (gameObject != null)
			{
				return gameObject;
			}
			MMMultipleObjectPoolerObject poolObject = this.GetPoolObject(this._objectPool.PooledGameObjects[num].gameObject);
			if (poolObject == null)
			{
				return null;
			}
			if (poolObject.PoolCanExpand)
			{
				return this.AddOneObjectToThePool(poolObject.GameObjectToPool);
			}
			return null;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001CB18 File Offset: 0x0001AD18
		protected virtual GameObject GetPooledGameObjectRandomBetweenObjects()
		{
			int num = Random.Range(0, this.Pool.Count);
			int num2 = 0;
			if (this._lastPooledObject != null)
			{
				while (!this.CanPoolSameObjectTwice && (this.Pool[num].GameObjectToPool.name == this._lastPooledObject.name || !this.PoolObjectEnabled(this.Pool[num].GameObjectToPool)) && (float)num2 < (float)this._objectPool.PooledGameObjects.Count * this.OverflowAmount)
				{
					num = Random.Range(0, this.Pool.Count);
					num2++;
				}
			}
			int num3 = num + 1;
			bool flag = false;
			num2 = 0;
			while (!flag && num != num3 && num2 < this._objectPool.PooledGameObjects.Count)
			{
				if (num >= this.Pool.Count)
				{
					num = 0;
				}
				if (!this.Pool[num].Enabled)
				{
					num++;
					num2++;
				}
				else
				{
					GameObject gameObject = this.FindInactiveObject(this.Pool[num].GameObjectToPool.name, this._objectPool.PooledGameObjects);
					if (gameObject != null)
					{
						return gameObject;
					}
					if (this.Pool[num].PoolCanExpand)
					{
						return this.AddOneObjectToThePool(this.Pool[num].GameObjectToPool);
					}
					num++;
					num2++;
				}
			}
			return null;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001CC87 File Offset: 0x0001AE87
		public virtual GameObject GetPooledGamObjectAtIndex(int index)
		{
			if (index < 0 || index >= this.Pool.Count)
			{
				return null;
			}
			this._tempSearchedName = this.Pool[index].GameObjectToPool.name;
			return this.GetPooledGameObjectOfType(this._tempSearchedName);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
		public virtual GameObject GetPooledGameObjectOfType(string searchedName)
		{
			GameObject gameObject = this.FindInactiveObject(searchedName, this._objectPool.PooledGameObjects);
			if (gameObject != null)
			{
				return gameObject;
			}
			GameObject gameObject2 = this.FindObject(searchedName, this._objectPool.PooledGameObjects);
			if (gameObject2 == null)
			{
				return null;
			}
			if (this.GetPoolObject(this.FindObject(searchedName, this._objectPool.PooledGameObjects)).PoolCanExpand)
			{
				return this.AddOneObjectToThePool(gameObject2);
			}
			return null;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001CD3C File Offset: 0x0001AF3C
		protected virtual GameObject FindInactiveObject(string searchedName, List<GameObject> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].name.Equals(searchedName) && !list[i].gameObject.activeInHierarchy)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001CD8C File Offset: 0x0001AF8C
		protected virtual GameObject FindAnyInactiveObject(List<GameObject> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].gameObject.activeInHierarchy)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001CDC8 File Offset: 0x0001AFC8
		protected virtual GameObject FindObject(string searchedName, List<GameObject> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].name.Equals(searchedName))
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001CE04 File Offset: 0x0001B004
		protected virtual MMMultipleObjectPoolerObject GetPoolObject(GameObject testedObject)
		{
			if (testedObject == null)
			{
				return null;
			}
			int num = 0;
			foreach (MMMultipleObjectPoolerObject mmmultipleObjectPoolerObject in this.Pool)
			{
				if (testedObject.name.Equals(mmmultipleObjectPoolerObject.GameObjectToPool.name))
				{
					return mmmultipleObjectPoolerObject;
				}
				num++;
			}
			return null;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001CE80 File Offset: 0x0001B080
		protected virtual bool PoolObjectEnabled(GameObject testedObject)
		{
			MMMultipleObjectPoolerObject poolObject = this.GetPoolObject(testedObject);
			return poolObject != null && poolObject.Enabled;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001CEA0 File Offset: 0x0001B0A0
		public virtual void EnableObjects(string name, bool newStatus)
		{
			foreach (MMMultipleObjectPoolerObject mmmultipleObjectPoolerObject in this.Pool)
			{
				if (name.Equals(mmmultipleObjectPoolerObject.GameObjectToPool.name))
				{
					mmmultipleObjectPoolerObject.Enabled = newStatus;
				}
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001CF08 File Offset: 0x0001B108
		public virtual void ResetCurrentIndex()
		{
			this._currentIndex = 0;
			this._currentIndexCounter = 0;
		}

		// Token: 0x04000703 RID: 1795
		[Tooltip("the list of objects to pool")]
		public List<MMMultipleObjectPoolerObject> Pool;

		// Token: 0x04000704 RID: 1796
		[MMInformation("A MultipleObjectPooler is a reserve of objects, to be used by a Spawner. When asked, it will return an object from the pool (ideally an inactive one) chosen based on the pooling method you've chosen.\n- OriginalOrder will spawn objects in the order you've set them in the inspector (from top to bottom)\n- OriginalOrderSequential will do the same, but will empty each pool before moving to the next object\n- RandomBetweenObjects will pick one object from the pool, at random, but ignoring its pool size, each object has equal chances to get picked\n- PoolSizeBased randomly choses one object from the pool, based on its pool size probability (the larger the pool size, the higher the chances it'll get picked)'...", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the chosen pooling method")]
		public MMPoolingMethods PoolingMethod = MMPoolingMethods.RandomPoolSizeBased;

		// Token: 0x04000705 RID: 1797
		[MMInformation("If you set CanPoolSameObjectTwice to false, the Pooler will try to prevent the same object from being pooled twice to avoid repetition. This will only affect random pooling methods, not ordered pooling.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("whether or not the same object can be pooled twice in a row. If you set CanPoolSameObjectTwice to false, the Pooler will try to prevent the same object from being pooled twice to avoid repetition. This will only affect random pooling methods, not ordered pooling.")]
		public bool CanPoolSameObjectTwice = true;

		// Token: 0x04000706 RID: 1798
		[Tooltip("a unique name that should match on all MMMultipleObjectPoolers you want to use together")]
		[MMCondition("MutualizeWaitingPools", true)]
		public string MutualizedPoolName = "";

		// Token: 0x04000707 RID: 1799
		[Tooltip("if CanPoolSameObjectTwice is set to false, this determines up to how many times we'll iterate to try and find a different object")]
		[MMCondition("CanPoolSameObjectTwice", true, true)]
		public float OverflowAmount = 10f;

		// Token: 0x04000709 RID: 1801
		protected GameObject _lastPooledObject;

		// Token: 0x0400070A RID: 1802
		protected int _currentIndex;

		// Token: 0x0400070B RID: 1803
		protected int _currentIndexCounter;

		// Token: 0x0400070C RID: 1804
		protected int _currentCount;

		// Token: 0x0400070D RID: 1805
		protected string _tempSearchedName;
	}
}
