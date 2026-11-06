using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x0200000E RID: 14
	public class GPUIInstanceSpawner : MonoBehaviour
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002B5B File Offset: 0x00000D5B
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002B64 File Offset: 0x00000D64
		public float TargetInstanceCount
		{
			get
			{
				return (float)this._targetInstanceCount;
			}
			set
			{
				this._targetInstanceCount = (int)value;
				if (this._targetInstanceCount < 0)
				{
					this._targetInstanceCount = 0;
				}
				if (this.instanceCountText != null)
				{
					this.instanceCountText.text = this._targetInstanceCount.ToString();
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002BA4 File Offset: 0x00000DA4
		private void Awake()
		{
			if (this.prefabObjects == null || this.prefabObjects.Count == 0)
			{
				base.enabled = false;
				return;
			}
			if (this.startInstanceCount < 0)
			{
				this.startInstanceCount = 0;
			}
			this._instances = new List<GameObject>();
			this._instancesToRemove = new List<GameObject>();
			this._addedInstances = new List<GPUIPrefab>();
			this._addedGOs = new List<GameObject>[this.prefabObjects.Count];
			for (int i = 0; i < this._addedGOs.Length; i++)
			{
				this._addedGOs[i] = new List<GameObject>();
			}
			this.TargetInstanceCount = (float)this.startInstanceCount;
			if (this.instanceCountSlider != null)
			{
				this.instanceCountSlider.value = (float)this._targetInstanceCount;
			}
			if (this.instantiatedPrefabEvent == null)
			{
				this.instantiatedPrefabEvent = new GPUIInstanceSpawner.GPUIInstanceSpawnerInstantiatedPrefabEvent();
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002C78 File Offset: 0x00000E78
		private void OnEnable()
		{
			global::UnityEngine.Random.InitState(this.isRandomSeed ? global::UnityEngine.Random.Range(100, 100000) : this.seed);
			if (this._prefabManager == null)
			{
				this._prefabManager = Object.FindAnyObjectByType<GPUIPrefabManager>();
			}
			this._lastAddTime = float.MinValue;
			if (!this.addInstantly && this.addSpeed > 0f)
			{
				this._addWaitTime = 1f / this.addSpeed;
				return;
			}
			this._addWaitTime = 0f;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002D00 File Offset: 0x00000F00
		private void Update()
		{
			if (this._instances.Count > this._targetInstanceCount)
			{
				this.RemoveInstance();
			}
			else if (this._instances.Count < this._targetInstanceCount)
			{
				this.AddInstance();
			}
			else if (this.loadingPanel != null && this.loadingPanel.gameObject.activeSelf)
			{
				this.loadingPanel.gameObject.SetActive(false);
			}
			this.ApplyDelayedRemoval();
			this.UpdateCurrentInstanceCount();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D80 File Offset: 0x00000F80
		[ContextMenu("Spawn Instances")]
		private void SpawnInstances()
		{
			if (!Application.isPlaying)
			{
				this.Awake();
			}
			if (this._instances == null)
			{
				return;
			}
			while (this._instances.Count > this._targetInstanceCount || this._instances.Count < this._targetInstanceCount)
			{
				this.Update();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002DD0 File Offset: 0x00000FD0
		private void AddInstance()
		{
			float time = Time.time;
			if (this._addWaitTime > 0f && this._addWaitTime > time - this._lastAddTime)
			{
				return;
			}
			int num = 0;
			Transform transform = base.transform;
			Vector3 position = transform.position;
			Quaternion quaternion = transform.rotation;
			Vector3 lossyScale = transform.lossyScale;
			bool flag = this._prefabManager != null;
			do
			{
				int count = this._instances.Count;
				Vector3 vector = Vector3.zero;
				Vector3 eulerAngles = this.rotation.eulerAngles;
				if (this.randomRotation.x)
				{
					eulerAngles.x = global::UnityEngine.Random.Range(0f, 360f);
				}
				if (this.randomRotation.y)
				{
					eulerAngles.y = global::UnityEngine.Random.Range(0f, 360f);
				}
				if (this.randomRotation.z)
				{
					eulerAngles.z = global::UnityEngine.Random.Range(0f, 360f);
				}
				Quaternion quaternion2 = Quaternion.Euler(eulerAngles);
				switch (this.spawnMode)
				{
				case GPUIInstanceSpawner.SpawnMode.Sphere:
					vector = global::UnityEngine.Random.insideUnitSphere * this.radius + this.center;
					break;
				case GPUIInstanceSpawner.SpawnMode.Grid:
				{
					int num2 = Mathf.FloorToInt(Mathf.Sqrt((float)count));
					int num3 = num2 * num2 + num2;
					vector.x = (float)((count >= num3) ? (count - num3) : num2);
					vector.z = (float)((count >= num3) ? num2 : (count - num2 * num2));
					vector.Scale(this.spacing);
					vector = quaternion * vector;
					vector += position + new Vector3(global::UnityEngine.Random.Range(-this.randomPosition.x, this.randomPosition.x), global::UnityEngine.Random.Range(-this.randomPosition.y, this.randomPosition.y), global::UnityEngine.Random.Range(-this.randomPosition.z, this.randomPosition.z));
					break;
				}
				case GPUIInstanceSpawner.SpawnMode.Ring:
				{
					vector = global::UnityEngine.Random.insideUnitSphere * this.radius + this.center;
					float num4 = global::UnityEngine.Random.value * 360f;
					vector.x += Mathf.Cos(num4) * this.distanceFromCenter;
					vector.z += Mathf.Sin(num4) * this.distanceFromCenter;
					vector.Scale(lossyScale);
					vector = quaternion * vector;
					vector += position;
					break;
				}
				}
				int num5 = global::UnityEngine.Random.Range(0, this.prefabObjects.Count);
				GameObject gameObject = Object.Instantiate<GameObject>(this.prefabObjects[num5], vector, quaternion2, this.addAsChildGameObject ? transform : null);
				this.instantiatedPrefabEvent.Invoke(gameObject);
				GameObject gameObject2 = gameObject;
				gameObject2.name = gameObject2.name + " [" + count.ToString() + "]";
				gameObject.transform.localScale = Vector3.one * (global::UnityEngine.Random.value * (this.minMaxScale.y - this.minMaxScale.x) + this.minMaxScale.x);
				this._instances.Add(gameObject);
				GPUIPrefab gpuiprefab;
				if (gameObject.TryGetComponent<GPUIPrefab>(out gpuiprefab))
				{
					this._addedInstances.Add(gpuiprefab);
				}
				else if (flag)
				{
					this._addedGOs[num5].Add(gameObject);
				}
				num++;
				this._lastAddTime = time;
			}
			while (this.addInstantly && this._instances.Count < this._targetInstanceCount && num < this.maxAddCount);
			if (this._addedInstances.Count > 0)
			{
				GPUIPrefabAPI.AddPrefabInstances(this._addedInstances);
			}
			this._addedInstances.Clear();
			if (flag)
			{
				for (int i = 0; i < this._addedGOs.Length; i++)
				{
					if (this._addedGOs[i].Count > 0)
					{
						int prototypeIndex = this._prefabManager.GetPrototypeIndex(this.prefabObjects[i]);
						if (prototypeIndex >= 0)
						{
							this._prefabManager.AddPrefabInstances(this._addedGOs[i], prototypeIndex);
						}
					}
					this._addedGOs[i].Clear();
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000031F0 File Offset: 0x000013F0
		private void RemoveInstance()
		{
			if (this.removeSpeed <= 0f)
			{
				for (int i = this._instances.Count - 1; i >= this._targetInstanceCount; i--)
				{
					GameObject gameObject = this._instances[i];
					this._instances.RemoveAt(i);
					if (!(gameObject == null))
					{
						GPUIPrefab gpuiprefab;
						if (gameObject.TryGetComponent<GPUIPrefab>(out gpuiprefab) && gpuiprefab.IsInstanced)
						{
							GPUIPrefabAPI.RemovePrefabInstance(gpuiprefab);
						}
						Object.Destroy(gameObject);
					}
				}
				return;
			}
			this._instancesToRemove.Add(this._instances[this._instances.Count - 1]);
			this._instances.RemoveAt(this._instances.Count - 1);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000032A4 File Offset: 0x000014A4
		private void ApplyDelayedRemoval()
		{
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < this._instancesToRemove.Count; i++)
			{
				GameObject gameObject = this._instancesToRemove[i];
				if (gameObject == null)
				{
					this._instancesToRemove.RemoveAt(i);
					i--;
				}
				else
				{
					gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Vector3.zero, this.removeSpeed * deltaTime * 100f);
					if (Vector3.Distance(gameObject.transform.position, Vector3.zero) < 0.5f)
					{
						GPUIPrefab gpuiprefab;
						if (gameObject.TryGetComponent<GPUIPrefab>(out gpuiprefab) && gpuiprefab.IsInstanced)
						{
							gpuiprefab.RemovePrefabInstance();
						}
						Object.Destroy(gameObject);
						this._instancesToRemove.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003374 File Offset: 0x00001574
		private void UpdateCurrentInstanceCount()
		{
			if (this._currentInstanceCount != this._instances.Count + this._instancesToRemove.Count)
			{
				this._currentInstanceCount = this._instances.Count + this._instancesToRemove.Count;
				if (this.currentInstanceCountText != null)
				{
					this.currentInstanceCountText.text = this._currentInstanceCount.ToString();
				}
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000033E1 File Offset: 0x000015E1
		public void AddInstances(int amount)
		{
			this.TargetInstanceCount += (float)amount;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000033F2 File Offset: 0x000015F2
		public void RemoveInstances(int amount)
		{
			this.TargetInstanceCount -= (float)amount;
		}

		// Token: 0x04000036 RID: 54
		public bool isRandomSeed = true;

		// Token: 0x04000037 RID: 55
		public int seed = 42;

		// Token: 0x04000038 RID: 56
		public GPUIInstanceSpawner.SpawnMode spawnMode = GPUIInstanceSpawner.SpawnMode.Grid;

		// Token: 0x04000039 RID: 57
		public int startInstanceCount;

		// Token: 0x0400003A RID: 58
		public List<GameObject> prefabObjects;

		// Token: 0x0400003B RID: 59
		public bool addInstantly = true;

		// Token: 0x0400003C RID: 60
		public float addSpeed;

		// Token: 0x0400003D RID: 61
		public float removeSpeed;

		// Token: 0x0400003E RID: 62
		public int maxAddCount = 1000;

		// Token: 0x0400003F RID: 63
		public bool3 randomRotation;

		// Token: 0x04000040 RID: 64
		public Quaternion rotation = Quaternion.identity;

		// Token: 0x04000041 RID: 65
		public Vector3 spacing = Vector3.one;

		// Token: 0x04000042 RID: 66
		public Vector3 randomPosition = Vector3.zero;

		// Token: 0x04000043 RID: 67
		public Vector3 center;

		// Token: 0x04000044 RID: 68
		public float distanceFromCenter = 100f;

		// Token: 0x04000045 RID: 69
		public float radius = 1f;

		// Token: 0x04000046 RID: 70
		public bool addAsChildGameObject = true;

		// Token: 0x04000047 RID: 71
		public Vector2 minMaxScale = Vector2.one;

		// Token: 0x04000048 RID: 72
		public GPUIInstanceSpawner.GPUIInstanceSpawnerInstantiatedPrefabEvent instantiatedPrefabEvent;

		// Token: 0x04000049 RID: 73
		public Text instanceCountText;

		// Token: 0x0400004A RID: 74
		public Text currentInstanceCountText;

		// Token: 0x0400004B RID: 75
		public Slider instanceCountSlider;

		// Token: 0x0400004C RID: 76
		public RectTransform loadingPanel;

		// Token: 0x0400004D RID: 77
		private int _currentInstanceCount;

		// Token: 0x0400004E RID: 78
		private float _lastAddTime;

		// Token: 0x0400004F RID: 79
		private float _addWaitTime;

		// Token: 0x04000050 RID: 80
		private int _targetInstanceCount;

		// Token: 0x04000051 RID: 81
		private List<GameObject> _instances;

		// Token: 0x04000052 RID: 82
		private List<GameObject> _instancesToRemove;

		// Token: 0x04000053 RID: 83
		private List<GPUIPrefab> _addedInstances;

		// Token: 0x04000054 RID: 84
		private List<GameObject>[] _addedGOs;

		// Token: 0x04000055 RID: 85
		private GPUIPrefabManager _prefabManager;

		// Token: 0x0200000F RID: 15
		public enum SpawnMode
		{
			// Token: 0x04000057 RID: 87
			Sphere,
			// Token: 0x04000058 RID: 88
			Grid,
			// Token: 0x04000059 RID: 89
			Ring
		}

		// Token: 0x02000010 RID: 16
		[Serializable]
		public class GPUIInstanceSpawnerInstantiatedPrefabEvent : UnityEvent<GameObject>
		{
		}
	}
}
