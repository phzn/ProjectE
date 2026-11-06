using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000749 RID: 1865
	public class DemoSceneController : MonoBehaviour
	{
		// Token: 0x0600304D RID: 12365 RVA: 0x001119E2 File Offset: 0x0010FBE2
		public void Awake()
		{
			DemoSceneController.instance = this;
			this.currentSurface = this.Surfaces[0];
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x001119F8 File Offset: 0x0010FBF8
		public void ChangeEnvironment()
		{
			this.environmentIndex++;
			if (this.environmentIndex >= this.Environments.Length)
			{
				this.environmentIndex = 0;
			}
			GameObject[] environments = this.Environments;
			for (int i = 0; i < environments.Length; i++)
			{
				environments[i].SetActive(false);
			}
			this.currentSurface = this.Surfaces[this.environmentIndex];
			this.Environments[this.environmentIndex].SetActive(true);
			this.RubberStampPrefab.SetCurrentDMS(this.currentSurface);
			this.RemoveSpawnedProjectiles();
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x00111A85 File Offset: 0x0010FC85
		public void SetProjectile(int index)
		{
			this.projectileIndex = index;
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x00111A90 File Offset: 0x0010FC90
		public void SpawnProjectile()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.Projectiles[this.projectileIndex]);
			gameObject.transform.position = new Vector3(Random.Range(-4f, 8f), Random.Range(2f, 4f), Random.Range(8f, -5f));
			this.spawnedProjectiles.Add(gameObject);
			if (this.projectileIndex > 0)
			{
				gameObject.transform.localEulerAngles = new Vector3((float)Random.Range(0, 360), (float)Random.Range(0, 360), (float)Random.Range(0, 360));
			}
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x00111B38 File Offset: 0x0010FD38
		public void Clear()
		{
			this.RemoveSpawnedProjectiles();
			DynamicMudSurface[] surfaces = this.Surfaces;
			for (int i = 0; i < surfaces.Length; i++)
			{
				surfaces[i].ClearStampTexture();
			}
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x00111B68 File Offset: 0x0010FD68
		public void SpawnBulletHit()
		{
			RaycastHit[] array = Physics.RaycastAll(new Vector3(Random.Range(-1f, -6f), 1f, Random.Range(-3f, 3f)), Vector3.down, 2f);
			Vector3 point = new Vector3(-4f, 1f, 0f);
			foreach (RaycastHit raycastHit in array)
			{
				DynamicMudSurface dynamicMudSurface;
				if (raycastHit.collider.gameObject.TryGetComponent<DynamicMudSurface>(out dynamicMudSurface))
				{
					point = raycastHit.point;
				}
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.BulletHitPrefab);
			gameObject.GetComponent<MudBulletHit>().SpawnBulletHit(point, this.currentSurface);
			this.spawnedProjectiles.Add(gameObject);
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x00111C24 File Offset: 0x0010FE24
		public void SpawnSplat()
		{
			Vector3 vector = new Vector3(Random.Range(-1f, -5f), 1f, Random.Range(-3f, 3f));
			GameObject gameObject = Object.Instantiate<GameObject>(this.MudSplatPrefab);
			this.spawnedProjectiles.Add(gameObject);
			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.one * Random.Range(0f, 0.5f));
			gameObject.transform.position = vector;
			this.decalMaterialIndex++;
			if (this.decalMaterialIndex >= this.SplatDecalMaterials.Length)
			{
				this.decalMaterialIndex = 0;
			}
			gameObject.GetComponent<MudSplatOnImpact>().SetDecalMaterial(this.SplatDecalMaterials[this.decalMaterialIndex]);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x00111CE0 File Offset: 0x0010FEE0
		private void RemoveSpawnedProjectiles()
		{
			foreach (GameObject gameObject in this.spawnedProjectiles)
			{
				Object.Destroy(gameObject);
			}
		}

		// Token: 0x040030E1 RID: 12513
		public static DemoSceneController instance;

		// Token: 0x040030E2 RID: 12514
		public GameObject[] Environments;

		// Token: 0x040030E3 RID: 12515
		public GameObject[] Projectiles;

		// Token: 0x040030E4 RID: 12516
		public DynamicMudSurface[] Surfaces;

		// Token: 0x040030E5 RID: 12517
		public Material[] SplatDecalMaterials;

		// Token: 0x040030E6 RID: 12518
		public GameObject BulletHitPrefab;

		// Token: 0x040030E7 RID: 12519
		public GameObject MudSplatPrefab;

		// Token: 0x040030E8 RID: 12520
		public RubberStampExample RubberStampPrefab;

		// Token: 0x040030E9 RID: 12521
		private List<GameObject> spawnedProjectiles = new List<GameObject>();

		// Token: 0x040030EA RID: 12522
		private int environmentIndex;

		// Token: 0x040030EB RID: 12523
		private int projectileIndex;

		// Token: 0x040030EC RID: 12524
		private int decalMaterialIndex;

		// Token: 0x040030ED RID: 12525
		private DynamicMudSurface currentSurface;
	}
}
