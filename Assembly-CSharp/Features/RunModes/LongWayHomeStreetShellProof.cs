using System;
using System.Collections.Generic;
using Features.Detection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D1B RID: 3355
	public sealed class LongWayHomeStreetShellProof : IDisposable
	{
		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x060061EF RID: 25071 RVA: 0x0026FBCC File Offset: 0x0026DDCC
		public int CheckedShellCount
		{
			get
			{
				return this._cache.Count;
			}
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x0026FBD9 File Offset: 0x0026DDD9
		public LongWayHomeStreetShellProof(float cellSize)
		{
			this._cellSize = cellSize;
		}

		// Token: 0x060061F1 RID: 25073 RVA: 0x0026FC00 File Offset: 0x0026DE00
		public bool IsSupported(GameObject prefab, int rotation)
		{
			if (prefab == null || this._cellSize < 2f)
			{
				return false;
			}
			rotation = (rotation % 4 + 4) % 4;
			ValueTuple<GameObject, int> valueTuple = new ValueTuple<GameObject, int>(prefab, rotation);
			bool flag;
			if (this._cache.TryGetValue(valueTuple, out flag))
			{
				return flag;
			}
			if (!this._scene.IsValid())
			{
				this._scene = SceneManager.CreateScene("LongWayHomeStreetShellProof", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
				this._physics = this._scene.GetPhysicsScene();
			}
			GameObject gameObject = new GameObject("Road shell collider proof");
			SceneManager.MoveGameObjectToScene(gameObject, this._scene);
			gameObject.transform.rotation = Quaternion.Euler(0f, (float)rotation * 90f, 0f);
			gameObject.transform.localScale = prefab.transform.lossyScale;
			bool flag2 = true;
			int num = 0;
			foreach (Collider collider in prefab.GetComponentsInChildren<Collider>(true))
			{
				if (collider.enabled && !collider.isTrigger && LongWayHomeStreetShellProof.IsAuthoredActive(collider.transform, prefab.transform) && (!prefab.scene.IsValid() || (!(collider.attachedRigidbody != null) && !(collider.GetComponentInParent<NPCSenses>(true) != null))))
				{
					GameObject gameObject2 = new GameObject(collider.name);
					gameObject2.layer = collider.gameObject.layer;
					gameObject2.transform.SetParent(gameObject.transform, false);
					Matrix4x4 matrix4x = prefab.transform.worldToLocalMatrix * collider.transform.localToWorldMatrix;
					gameObject2.transform.localPosition = matrix4x.GetColumn(3);
					gameObject2.transform.localRotation = matrix4x.rotation;
					gameObject2.transform.localScale = matrix4x.lossyScale;
					flag2 &= LongWayHomeStreetShellProof.CopyCollider(collider, gameObject2);
					num++;
				}
			}
			Physics.SyncTransforms();
			this._physics.Simulate(0.02f);
			bool flag3 = flag2 && num > 0 && this.CheckRing();
			gameObject.SetActive(false);
			if (Application.isPlaying)
			{
				Object.Destroy(gameObject);
			}
			else
			{
				Object.DestroyImmediate(gameObject);
			}
			this._cache[valueTuple] = flag3;
			return flag3;
		}

		// Token: 0x060061F2 RID: 25074 RVA: 0x0026FE58 File Offset: 0x0026E058
		private static bool IsAuthoredActive(Transform node, Transform root)
		{
			Transform transform = node;
			while (transform != null)
			{
				if (!transform.gameObject.activeSelf)
				{
					return false;
				}
				if (transform == root)
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x060061F3 RID: 25075 RVA: 0x0026FE94 File Offset: 0x0026E094
		private static bool CopyCollider(Collider source, GameObject target)
		{
			MeshCollider meshCollider = source as MeshCollider;
			if (meshCollider != null)
			{
				if (meshCollider.sharedMesh == null)
				{
					return false;
				}
				MeshCollider meshCollider2 = target.AddComponent<MeshCollider>();
				meshCollider2.cookingOptions = meshCollider.cookingOptions;
				meshCollider2.sharedMesh = meshCollider.sharedMesh;
				meshCollider2.convex = meshCollider.convex;
			}
			else
			{
				BoxCollider boxCollider = source as BoxCollider;
				if (boxCollider != null)
				{
					BoxCollider boxCollider2 = target.AddComponent<BoxCollider>();
					boxCollider2.center = boxCollider.center;
					boxCollider2.size = boxCollider.size;
				}
				else
				{
					SphereCollider sphereCollider = source as SphereCollider;
					if (sphereCollider != null)
					{
						SphereCollider sphereCollider2 = target.AddComponent<SphereCollider>();
						sphereCollider2.center = sphereCollider.center;
						sphereCollider2.radius = sphereCollider.radius;
					}
					else
					{
						CapsuleCollider capsuleCollider = source as CapsuleCollider;
						if (capsuleCollider != null)
						{
							CapsuleCollider capsuleCollider2 = target.AddComponent<CapsuleCollider>();
							capsuleCollider2.center = capsuleCollider.center;
							capsuleCollider2.radius = capsuleCollider.radius;
							capsuleCollider2.height = capsuleCollider.height;
							capsuleCollider2.direction = capsuleCollider.direction;
						}
						else
						{
							TerrainCollider terrainCollider = source as TerrainCollider;
							if (terrainCollider == null)
							{
								return false;
							}
							target.AddComponent<TerrainCollider>().terrainData = terrainCollider.terrainData;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x0026FFA4 File Offset: 0x0026E1A4
		private bool CheckRing()
		{
			float num = this._cellSize * 0.5f - 0.1f;
			Vector3[] array = new Vector3[]
			{
				new Vector3(-num, 0f, -num),
				new Vector3(num, 0f, -num),
				new Vector3(num, 0f, num),
				new Vector3(-num, 0f, num)
			};
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector = array[i];
				Vector3 vector2 = array[(i + 1) % array.Length];
				int num2 = Mathf.CeilToInt(Vector3.Distance(vector, vector2) / 0.25f);
				Vector3 vector3 = default(Vector3);
				for (int j = 0; j <= num2; j++)
				{
					Vector3 vector4 = Vector3.Lerp(vector, vector2, (float)j / (float)num2);
					RaycastHit raycastHit;
					if (!this._physics.Raycast(vector4 + Vector3.up * 1.2f, Vector3.down, out raycastHit, 2.4f, -5, QueryTriggerInteraction.Ignore) || raycastHit.normal.y < 0.85f || Mathf.Abs(raycastHit.point.y) > 0.3f || (j > 0 && Mathf.Abs(raycastHit.point.y - vector3.y) > 0.3f))
					{
						return false;
					}
					if (this._physics.OverlapCapsule(raycastHit.point + Vector3.up * 0.4f, raycastHit.point + Vector3.up * 1.45f, 0.3f, this._overlaps, -5, QueryTriggerInteraction.Ignore) != 0)
					{
						return false;
					}
					vector3 = raycastHit.point;
				}
			}
			return true;
		}

		// Token: 0x060061F5 RID: 25077 RVA: 0x0027016C File Offset: 0x0026E36C
		public void Dispose()
		{
			this._cache.Clear();
			if (this._scene.IsValid() && this._scene.isLoaded)
			{
				SceneManager.UnloadSceneAsync(this._scene);
			}
			this._scene = default(Scene);
		}

		// Token: 0x04006420 RID: 25632
		private readonly Dictionary<ValueTuple<GameObject, int>, bool> _cache = new Dictionary<ValueTuple<GameObject, int>, bool>();

		// Token: 0x04006421 RID: 25633
		private readonly float _cellSize;

		// Token: 0x04006422 RID: 25634
		private Scene _scene;

		// Token: 0x04006423 RID: 25635
		private PhysicsScene _physics;

		// Token: 0x04006424 RID: 25636
		private readonly Collider[] _overlaps = new Collider[16];
	}
}
