using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A75 RID: 2677
	public sealed class SceneUiSuppressionScope
	{
		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06004C20 RID: 19488 RVA: 0x001D9C1F File Offset: 0x001D7E1F
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x001D9C28 File Offset: 0x001D7E28
		public void Suppress(params Transform[] protectedRuntimeRoots)
		{
			this._protectedRuntimeRoots.Clear();
			if (protectedRuntimeRoots != null)
			{
				for (int i = 0; i < protectedRuntimeRoots.Length; i++)
				{
					this.AddProtectedRootInternal(protectedRuntimeRoots[i]);
				}
			}
			this._active = true;
			this._nextDiscoveryTime = 0f;
			this.Refresh();
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x001D9C72 File Offset: 0x001D7E72
		public void AddProtectedRoot(Transform protectedRuntimeRoot)
		{
			this.AddProtectedRootInternal(protectedRuntimeRoot);
			if (this._active)
			{
				this.Refresh();
			}
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x001D9C8C File Offset: 0x001D7E8C
		public void Refresh()
		{
			if (!this._active)
			{
				return;
			}
			this._protectedDescendantCache.Clear();
			this._protectedNodeCache.Clear();
			Canvas[] array = null;
			if (Time.unscaledTime >= this._nextDiscoveryTime)
			{
				this._nextDiscoveryTime = Time.unscaledTime + 1.5f;
				array = Object.FindObjectsOfType<Canvas>(true);
				this.FindLoadedUiRoots(array);
			}
			foreach (KeyValuePair<GameObject, bool> keyValuePair in this._originalStates)
			{
				GameObject key = keyValuePair.Key;
				if (key != null && key.activeSelf)
				{
					key.SetActive(false);
				}
			}
			foreach (Transform transform in this._uiRoots)
			{
				if (!(transform == null))
				{
					for (int i = 0; i < transform.childCount; i++)
					{
						this.SuppressBranch(transform.GetChild(i));
					}
				}
			}
			if (array == null)
			{
				return;
			}
			foreach (Canvas canvas in array)
			{
				if (!(canvas == null) && SceneUiSuppressionScope.IsLoadedSceneObject(canvas.gameObject))
				{
					Transform transform2 = canvas.transform;
					if (!this._originalStates.ContainsKey(canvas.gameObject) && !this.HasSuppressedAncestor(transform2) && !this.IsInsideProtectedHierarchy(transform2))
					{
						this.SuppressBranch(transform2);
					}
				}
			}
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x001D9E24 File Offset: 0x001D8024
		public void Restore()
		{
			if (!this._active && this._originalStates.Count == 0)
			{
				return;
			}
			this._active = false;
			foreach (KeyValuePair<GameObject, bool> keyValuePair in this._originalStates)
			{
				GameObject key = keyValuePair.Key;
				if (key != null && key.activeSelf != keyValuePair.Value)
				{
					key.SetActive(keyValuePair.Value);
				}
			}
			this._originalStates.Clear();
			this._uiRoots.Clear();
			this._protectedRuntimeRoots.Clear();
			this.ClearDiscoveryCaches();
			this._nextDiscoveryTime = 0f;
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x001D9EEC File Offset: 0x001D80EC
		public void Abandon()
		{
			this._active = false;
			this._originalStates.Clear();
			this._uiRoots.Clear();
			this._protectedRuntimeRoots.Clear();
			this.ClearDiscoveryCaches();
			this._nextDiscoveryTime = 0f;
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x001D9F28 File Offset: 0x001D8128
		private void SuppressBranch(Transform branch)
		{
			if (branch == null)
			{
				return;
			}
			if (this.IsProtectedNode(branch))
			{
				return;
			}
			if (this.ContainsProtectedDescendant(branch))
			{
				for (int i = 0; i < branch.childCount; i++)
				{
					this.SuppressBranch(branch.GetChild(i));
				}
				return;
			}
			GameObject gameObject = branch.gameObject;
			if (!this._originalStates.ContainsKey(gameObject))
			{
				this._originalStates.Add(gameObject, gameObject.activeSelf);
			}
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x001D9FA8 File Offset: 0x001D81A8
		private bool IsProtectedNode(Transform node)
		{
			if (node == null)
			{
				return false;
			}
			bool flag;
			if (this._protectedNodeCache.TryGetValue(node, out flag))
			{
				return flag;
			}
			for (int i = 0; i < this._protectedRuntimeRoots.Count; i++)
			{
				if (this._protectedRuntimeRoots[i] != null && node == this._protectedRuntimeRoots[i])
				{
					this._protectedNodeCache[node] = true;
					return true;
				}
			}
			EventSystem eventSystem;
			bool flag2 = node.TryGetComponent<EventSystem>(out eventSystem) || string.Equals(node.name, "EventSystem", StringComparison.OrdinalIgnoreCase);
			this._protectedNodeCache[node] = flag2;
			return flag2;
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x001DA04C File Offset: 0x001D824C
		private bool ContainsProtectedDescendant(Transform node)
		{
			bool flag;
			if (this._protectedDescendantCache.TryGetValue(node, out flag))
			{
				return flag;
			}
			for (int i = 0; i < node.childCount; i++)
			{
				Transform child = node.GetChild(i);
				if (this.IsProtectedNode(child) || this.ContainsProtectedDescendant(child))
				{
					this._protectedDescendantCache[node] = true;
					return true;
				}
			}
			this._protectedDescendantCache[node] = false;
			return false;
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001DA0B4 File Offset: 0x001D82B4
		private bool IsInsideProtectedHierarchy(Transform node)
		{
			Transform transform = node;
			while (transform != null)
			{
				if (this.IsProtectedNode(transform))
				{
					return true;
				}
				transform = transform.parent;
			}
			return this.ContainsProtectedDescendant(node);
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x001DA0E8 File Offset: 0x001D82E8
		private bool HasSuppressedAncestor(Transform node)
		{
			Transform transform = node.parent;
			while (transform != null)
			{
				if (this._originalStates.ContainsKey(transform.gameObject))
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x001DA124 File Offset: 0x001D8324
		private void FindLoadedUiRoots(Canvas[] canvases)
		{
			this._uiRoots.Clear();
			this._discoveredScenes.Clear();
			this._discoveredUiRoots.Clear();
			this._discoveredUiAncestors.Clear();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				this.FindUiRootsInScene(SceneManager.GetSceneAt(i));
			}
			foreach (Transform transform in this._protectedRuntimeRoots)
			{
				if (!(transform == null))
				{
					this.FindUiRootsInScene(transform.gameObject.scene);
					this.FindUiRootsInAncestors(transform);
				}
			}
			foreach (Canvas canvas in canvases)
			{
				if (!(canvas == null) && SceneUiSuppressionScope.IsLoadedSceneObject(canvas.gameObject))
				{
					this.FindUiRootsInScene(canvas.gameObject.scene);
					this.FindUiRootsInAncestors(canvas.transform);
				}
			}
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x001DA22C File Offset: 0x001D842C
		private void FindUiRootsInScene(Scene scene)
		{
			if (!scene.IsValid() || !scene.isLoaded || !this._discoveredScenes.Add(scene.handle))
			{
				return;
			}
			this._sceneRoots.Clear();
			scene.GetRootGameObjects(this._sceneRoots);
			foreach (GameObject gameObject in this._sceneRoots)
			{
				this.AddUiRootIfNamed(gameObject.transform);
			}
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x001DA2C4 File Offset: 0x001D84C4
		private void FindUiRootsInAncestors(Transform candidate)
		{
			Transform transform = candidate;
			while (transform != null && this._discoveredUiAncestors.Add(transform))
			{
				this.AddUiRootIfNamed(transform);
				transform = transform.parent;
			}
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x001DA2FA File Offset: 0x001D84FA
		private void AddUiRootIfNamed(Transform candidate)
		{
			if (string.Equals(candidate.name, "---UI", StringComparison.Ordinal) && this._discoveredUiRoots.Add(candidate))
			{
				this._uiRoots.Add(candidate);
			}
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x001DA32C File Offset: 0x001D852C
		private void ClearDiscoveryCaches()
		{
			this._protectedDescendantCache.Clear();
			this._protectedNodeCache.Clear();
			this._discoveredScenes.Clear();
			this._discoveredUiRoots.Clear();
			this._discoveredUiAncestors.Clear();
			this._sceneRoots.Clear();
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x001DA37C File Offset: 0x001D857C
		private static bool IsLoadedSceneObject(GameObject target)
		{
			return target != null && target.scene.IsValid() && target.scene.isLoaded;
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x001DA3B2 File Offset: 0x001D85B2
		private void AddProtectedRootInternal(Transform protectedRuntimeRoot)
		{
			if (protectedRuntimeRoot != null && !this._protectedRuntimeRoots.Contains(protectedRuntimeRoot))
			{
				this._protectedRuntimeRoots.Add(protectedRuntimeRoot);
			}
		}

		// Token: 0x04004DB2 RID: 19890
		private const string UiRootName = "---UI";

		// Token: 0x04004DB3 RID: 19891
		private const string EventSystemName = "EventSystem";

		// Token: 0x04004DB4 RID: 19892
		private const float CanvasDiscoveryInterval = 1.5f;

		// Token: 0x04004DB5 RID: 19893
		private readonly Dictionary<GameObject, bool> _originalStates = new Dictionary<GameObject, bool>();

		// Token: 0x04004DB6 RID: 19894
		private readonly List<Transform> _uiRoots = new List<Transform>();

		// Token: 0x04004DB7 RID: 19895
		private readonly List<Transform> _protectedRuntimeRoots = new List<Transform>();

		// Token: 0x04004DB8 RID: 19896
		private readonly Dictionary<Transform, bool> _protectedDescendantCache = new Dictionary<Transform, bool>();

		// Token: 0x04004DB9 RID: 19897
		private readonly Dictionary<Transform, bool> _protectedNodeCache = new Dictionary<Transform, bool>();

		// Token: 0x04004DBA RID: 19898
		private readonly HashSet<int> _discoveredScenes = new HashSet<int>();

		// Token: 0x04004DBB RID: 19899
		private readonly HashSet<Transform> _discoveredUiRoots = new HashSet<Transform>();

		// Token: 0x04004DBC RID: 19900
		private readonly HashSet<Transform> _discoveredUiAncestors = new HashSet<Transform>();

		// Token: 0x04004DBD RID: 19901
		private readonly List<GameObject> _sceneRoots = new List<GameObject>();

		// Token: 0x04004DBE RID: 19902
		private bool _active;

		// Token: 0x04004DBF RID: 19903
		private float _nextDiscoveryTime;
	}
}
