using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Feel
{
	// Token: 0x02000380 RID: 896
	[AddComponentMenu("")]
	public class SnakeFoodSpawner : MonoBehaviour
	{
		// Token: 0x0600188C RID: 6284 RVA: 0x00068308 File Offset: 0x00066508
		protected virtual void Start()
		{
			this._mainCamera = Camera.main;
			this.Foods = new List<SnakeFood>();
			for (int i = 0; i < this.AmountOfFood; i++)
			{
				SnakeFood snakeFood = Object.Instantiate<SnakeFood>(this.SnakeFoodPrefab);
				SceneManager.MoveGameObjectToScene(snakeFood.gameObject, base.gameObject.scene);
				snakeFood.transform.position = this.DetermineSpawnPosition();
				snakeFood.Spawner = this;
				this.Foods.Add(snakeFood);
			}
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00068384 File Offset: 0x00066584
		public virtual Vector3 DetermineSpawnPosition()
		{
			Vector3 vector = MMMaths.RandomVector2(this.MinRandom, this.MaxRandom);
			vector.z = 10f;
			return this._mainCamera.ViewportToWorldPoint(vector);
		}

		// Token: 0x04001C94 RID: 7316
		public SnakeFood SnakeFoodPrefab;

		// Token: 0x04001C95 RID: 7317
		public int AmountOfFood = 3;

		// Token: 0x04001C96 RID: 7318
		public Vector2 MinRandom = new Vector2(0.1f, 0.1f);

		// Token: 0x04001C97 RID: 7319
		public Vector2 MaxRandom = new Vector2(0.9f, 0.9f);

		// Token: 0x04001C98 RID: 7320
		protected List<SnakeFood> Foods;

		// Token: 0x04001C99 RID: 7321
		protected Camera _mainCamera;
	}
}
