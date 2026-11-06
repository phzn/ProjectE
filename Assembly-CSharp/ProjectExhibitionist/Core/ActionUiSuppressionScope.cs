using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Features.MasturbationSystem;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A31 RID: 2609
	public sealed class ActionUiSuppressionScope
	{
		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004A1D RID: 18973 RVA: 0x001CABA4 File Offset: 0x001C8DA4
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x001CABAC File Offset: 0x001C8DAC
		public void Begin(params Transform[] extraProtectedRoots)
		{
			ActionUiSuppressionScope.RemoveDestroyedGroups();
			this._hierarchyDirty = true;
			this._extraProtectedRoots.Clear();
			if (extraProtectedRoots != null)
			{
				foreach (Transform transform in extraProtectedRoots)
				{
					if (transform != null)
					{
						this._extraProtectedRoots.Add(transform);
					}
				}
			}
			if (!this._active)
			{
				this._active = true;
				ActionUiSuppressionScope.ActiveScopes.Add(this);
				Canvas.willRenderCanvases += this.RefreshPresentation;
			}
			this.Refresh();
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x001CAC2E File Offset: 0x001C8E2E
		public void Refresh()
		{
			if (!this._active)
			{
				return;
			}
			this.RefreshPresentation();
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x001CAC40 File Offset: 0x001C8E40
		public static bool IsProtectedCanvas(Canvas canvas)
		{
			if (canvas == null || ActionUiSuppressionScope.ActiveScopes.Count == 0)
			{
				return false;
			}
			foreach (ActionUiSuppressionScope actionUiSuppressionScope in ActionUiSuppressionScope.ActiveScopes)
			{
				foreach (Transform transform in actionUiSuppressionScope._protectedRoots)
				{
					if (transform != null && (transform.IsChildOf(canvas.transform) || canvas.transform.IsChildOf(transform)))
					{
						return true;
					}
				}
			}
			return canvas.GetComponentInParent<FlyCameraPhotoViewfinder>(true) != null || canvas.GetComponentInParent<ScreenFadeController>(true) != null;
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x001CAD24 File Offset: 0x001C8F24
		public void Restore()
		{
			this.EndScope();
			foreach (KeyValuePair<GameObject, ActionUiSuppressionScope.SharedSuppression> keyValuePair in this._suppressionGroups)
			{
				ActionUiSuppressionScope.ReleaseSuppression(keyValuePair.Key, keyValuePair.Value);
			}
			this._suppressionGroups.Clear();
			this._protectedRoots.Clear();
			this._extraProtectedRoots.Clear();
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x001CADAC File Offset: 0x001C8FAC
		public void Abandon()
		{
			this.EndScope();
			foreach (KeyValuePair<GameObject, ActionUiSuppressionScope.SharedSuppression> keyValuePair in this._suppressionGroups)
			{
				ActionUiSuppressionScope.SharedSuppression value = keyValuePair.Value;
				ActionUiSuppressionScope.SharedSuppression sharedSuppression = value;
				int num = sharedSuppression.Owners - 1;
				sharedSuppression.Owners = num;
				if (num <= 0)
				{
					ActionUiSuppressionScope.SharedGroups.Remove(keyValuePair.Key);
					if (value.Created && value.Group != null)
					{
						ActionUiSuppressionScope.DestroyRuntimeObject(value.Group);
					}
				}
			}
			this._suppressionGroups.Clear();
			this._protectedRoots.Clear();
			this._extraProtectedRoots.Clear();
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x001CAE70 File Offset: 0x001C9070
		private void EndScope()
		{
			this._active = false;
			ActionUiSuppressionScope.ActiveScopes.Remove(this);
			Canvas.willRenderCanvases -= this.RefreshPresentation;
			this._canvases = null;
			this._canvasHierarchyRoots.Clear();
			this._hierarchyCounts.Clear();
			this._protectedAncestors.Clear();
			this._protectedParents.Clear();
			this._scenes.Clear();
			this._sceneRootCounts.Clear();
			this._uiComponents.Clear();
			this._hierarchyDirty = true;
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x001CAEFC File Offset: 0x001C90FC
		private static void RemoveDestroyedGroups()
		{
			List<GameObject> list = new List<GameObject>();
			foreach (KeyValuePair<GameObject, ActionUiSuppressionScope.SharedSuppression> keyValuePair in ActionUiSuppressionScope.SharedGroups)
			{
				if (keyValuePair.Value.Owners == 0 && (keyValuePair.Key == null || keyValuePair.Value.Group == null))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (GameObject gameObject in list)
			{
				ActionUiSuppressionScope.SharedGroups.Remove(gameObject);
			}
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x001CAFD0 File Offset: 0x001C91D0
		private void RefreshPresentation()
		{
			if (!this._active || this._refreshing)
			{
				return;
			}
			this._refreshing = true;
			try
			{
				if (!this.HierarchyChanged())
				{
					using (Dictionary<GameObject, ActionUiSuppressionScope.SharedSuppression>.Enumerator enumerator = this._suppressionGroups.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<GameObject, ActionUiSuppressionScope.SharedSuppression> keyValuePair = enumerator.Current;
							keyValuePair.Value.ObserveAndSuppress();
						}
						return;
					}
				}
				this._canvases = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
				this.DiscoverProtectedRoots();
				this._targets.Clear();
				foreach (Canvas canvas in this._canvases)
				{
					if (!(canvas == null) && canvas.gameObject.scene.IsValid() && canvas.gameObject.scene.isLoaded)
					{
						this.CollectSuppressedBranches(canvas.transform);
					}
				}
				this._releasedTargets.Clear();
				foreach (KeyValuePair<GameObject, ActionUiSuppressionScope.SharedSuppression> keyValuePair2 in this._suppressionGroups)
				{
					if (!this._targets.Contains(keyValuePair2.Key))
					{
						this._releasedTargets.Add(keyValuePair2.Key);
					}
				}
				foreach (GameObject gameObject in this._releasedTargets)
				{
					ActionUiSuppressionScope.ReleaseSuppression(gameObject, this._suppressionGroups[gameObject]);
					this._suppressionGroups.Remove(gameObject);
				}
				foreach (GameObject gameObject2 in this._targets)
				{
					ActionUiSuppressionScope.SharedSuppression sharedSuppression;
					if (!this._suppressionGroups.TryGetValue(gameObject2, out sharedSuppression))
					{
						if (!ActionUiSuppressionScope.SharedGroups.TryGetValue(gameObject2, out sharedSuppression))
						{
							CanvasGroup canvasGroup = gameObject2.GetComponent<CanvasGroup>();
							bool flag = canvasGroup == null;
							if (flag)
							{
								canvasGroup = gameObject2.AddComponent<CanvasGroup>();
								canvasGroup.hideFlags = HideFlags.DontSave;
							}
							sharedSuppression = new ActionUiSuppressionScope.SharedSuppression
							{
								Group = canvasGroup,
								Created = flag,
								Alpha = canvasGroup.alpha,
								Interactable = canvasGroup.interactable,
								BlocksRaycasts = canvasGroup.blocksRaycasts,
								Enabled = canvasGroup.enabled
							};
							ActionUiSuppressionScope.SharedGroups.Add(gameObject2, sharedSuppression);
						}
						else if (sharedSuppression.Owners == 0 && sharedSuppression.Group != null)
						{
							sharedSuppression.Alpha = sharedSuppression.Group.alpha;
							sharedSuppression.Interactable = sharedSuppression.Group.interactable;
							sharedSuppression.BlocksRaycasts = sharedSuppression.Group.blocksRaycasts;
							sharedSuppression.Enabled = sharedSuppression.Group.enabled;
						}
						sharedSuppression.Owners++;
						this._suppressionGroups.Add(gameObject2, sharedSuppression);
					}
					sharedSuppression.ObserveAndSuppress();
				}
				this.CaptureHierarchy();
			}
			finally
			{
				this._refreshing = false;
			}
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x001CB364 File Offset: 0x001C9564
		private bool HierarchyChanged()
		{
			if (this._hierarchyDirty || this._canvases == null || SceneManager.sceneCount != this._scenes.Count)
			{
				return true;
			}
			for (int i = 0; i < this._scenes.Count; i++)
			{
				if (!this._scenes[i].IsValid() || this._scenes[i] != SceneManager.GetSceneAt(i) || this._scenes[i].rootCount != this._sceneRootCounts[i])
				{
					return true;
				}
			}
			for (int j = 0; j < this._canvasHierarchyRoots.Count; j++)
			{
				if (this._canvasHierarchyRoots[j] == null || this._canvasHierarchyRoots[j].hierarchyCount != this._hierarchyCounts[j])
				{
					return true;
				}
			}
			for (int k = 0; k < this._protectedAncestors.Count; k++)
			{
				if (this._protectedAncestors[k] == null || this._protectedAncestors[k].parent != this._protectedParents[k])
				{
					return true;
				}
			}
			for (int l = 0; l < this._canvases.Length; l++)
			{
				if (this._canvases[l] == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x001CB4C4 File Offset: 0x001C96C4
		private void CaptureHierarchy()
		{
			this._scenes.Clear();
			this._sceneRootCounts.Clear();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				this._scenes.Add(sceneAt);
				this._sceneRootCounts.Add(sceneAt.rootCount);
			}
			this._canvasHierarchyRoots.Clear();
			this._hierarchyCounts.Clear();
			foreach (Canvas canvas in this._canvases)
			{
				if (!(canvas == null))
				{
					Transform root = canvas.transform.root;
					if (!this._canvasHierarchyRoots.Contains(root))
					{
						this._canvasHierarchyRoots.Add(root);
						this._hierarchyCounts.Add(root.hierarchyCount);
					}
				}
			}
			this._protectedParents.Clear();
			this._protectedAncestors.Clear();
			foreach (Transform transform in this._protectedRoots)
			{
				while (transform != null && !this._protectedAncestors.Contains(transform))
				{
					this._protectedAncestors.Add(transform);
					this._protectedParents.Add(transform.parent);
					transform = transform.parent;
				}
			}
			this._hierarchyDirty = false;
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x001CB634 File Offset: 0x001C9834
		private static void ReleaseSuppression(GameObject target, ActionUiSuppressionScope.SharedSuppression state)
		{
			int num = state.Owners - 1;
			state.Owners = num;
			if (num > 0)
			{
				return;
			}
			if (state.Group == null)
			{
				ActionUiSuppressionScope.SharedGroups.Remove(target);
				return;
			}
			state.ObserveAndSuppress();
			CanvasGroup group = state.Group;
			group.alpha = state.Alpha;
			group.interactable = state.Interactable;
			group.blocksRaycasts = state.BlocksRaycasts;
			group.enabled = state.Enabled;
			if (!state.Created || !Application.isPlaying)
			{
				ActionUiSuppressionScope.SharedGroups.Remove(target);
			}
			if (state.Created && !Application.isPlaying)
			{
				ActionUiSuppressionScope.DestroyRuntimeObject(group);
			}
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x001CB6E0 File Offset: 0x001C98E0
		private void DiscoverProtectedRoots()
		{
			this._protectedRoots.Clear();
			this._protectedRoots.AddRange(this._extraProtectedRoots);
			foreach (Canvas canvas in this._canvases)
			{
				if (!(canvas == null) && canvas.gameObject.scene.IsValid() && canvas.gameObject.scene.isLoaded && (!(canvas.transform.parent != null) || !(canvas.transform.parent.GetComponentInParent<Canvas>(true) != null)))
				{
					canvas.GetComponentsInChildren<MonoBehaviour>(true, this._uiComponents);
					foreach (MonoBehaviour monoBehaviour in this._uiComponents)
					{
						if (monoBehaviour is OrgasmMeterHUD || monoBehaviour is MasturbationSpeedWidgetLink || monoBehaviour is InteractionHUDController || monoBehaviour is FlyCameraPhotoViewfinder || monoBehaviour is ScreenFadeController)
						{
							this._protectedRoots.Add(monoBehaviour.transform);
						}
					}
					ScreenFadeController componentInParent = canvas.GetComponentInParent<ScreenFadeController>(true);
					if (componentInParent != null && !this._protectedRoots.Contains(componentInParent.transform))
					{
						this._protectedRoots.Add(componentInParent.transform);
					}
					FlyCameraPhotoViewfinder componentInParent2 = canvas.GetComponentInParent<FlyCameraPhotoViewfinder>(true);
					if (componentInParent2 != null && !this._protectedRoots.Contains(componentInParent2.transform))
					{
						this._protectedRoots.Add(componentInParent2.transform);
					}
				}
			}
			this._uiComponents.Clear();
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x001CB89C File Offset: 0x001C9A9C
		private void CollectSuppressedBranches(Transform node)
		{
			bool flag = false;
			foreach (Transform transform in this._protectedRoots)
			{
				if (!(transform == null))
				{
					if (node.IsChildOf(transform))
					{
						return;
					}
					if (transform.IsChildOf(node))
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this._targets.Add(node.gameObject);
				return;
			}
			for (int i = 0; i < node.childCount; i++)
			{
				this.CollectSuppressedBranches(node.GetChild(i));
			}
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyRuntimeObject(Object target)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x04004C4F RID: 19535
		private static readonly HashSet<ActionUiSuppressionScope> ActiveScopes = new HashSet<ActionUiSuppressionScope>();

		// Token: 0x04004C50 RID: 19536
		private static readonly Dictionary<GameObject, ActionUiSuppressionScope.SharedSuppression> SharedGroups = new Dictionary<GameObject, ActionUiSuppressionScope.SharedSuppression>();

		// Token: 0x04004C51 RID: 19537
		private readonly Dictionary<GameObject, ActionUiSuppressionScope.SharedSuppression> _suppressionGroups = new Dictionary<GameObject, ActionUiSuppressionScope.SharedSuppression>();

		// Token: 0x04004C52 RID: 19538
		private readonly List<Transform> _extraProtectedRoots = new List<Transform>();

		// Token: 0x04004C53 RID: 19539
		private readonly List<Transform> _protectedRoots = new List<Transform>();

		// Token: 0x04004C54 RID: 19540
		private readonly HashSet<GameObject> _targets = new HashSet<GameObject>();

		// Token: 0x04004C55 RID: 19541
		private readonly List<GameObject> _releasedTargets = new List<GameObject>();

		// Token: 0x04004C56 RID: 19542
		private Canvas[] _canvases;

		// Token: 0x04004C57 RID: 19543
		private readonly List<Transform> _canvasHierarchyRoots = new List<Transform>();

		// Token: 0x04004C58 RID: 19544
		private readonly List<int> _hierarchyCounts = new List<int>();

		// Token: 0x04004C59 RID: 19545
		private readonly List<Transform> _protectedAncestors = new List<Transform>();

		// Token: 0x04004C5A RID: 19546
		private readonly List<Transform> _protectedParents = new List<Transform>();

		// Token: 0x04004C5B RID: 19547
		private readonly List<Scene> _scenes = new List<Scene>();

		// Token: 0x04004C5C RID: 19548
		private readonly List<int> _sceneRootCounts = new List<int>();

		// Token: 0x04004C5D RID: 19549
		private readonly List<MonoBehaviour> _uiComponents = new List<MonoBehaviour>();

		// Token: 0x04004C5E RID: 19550
		private bool _hierarchyDirty = true;

		// Token: 0x04004C5F RID: 19551
		private bool _active;

		// Token: 0x04004C60 RID: 19552
		private bool _refreshing;

		// Token: 0x02000A32 RID: 2610
		private sealed class SharedSuppression
		{
			// Token: 0x06004A2E RID: 18990 RVA: 0x001CB9F4 File Offset: 0x001C9BF4
			public void ObserveAndSuppress()
			{
				if (this.Group == null)
				{
					return;
				}
				if (this.Group.alpha != 0f)
				{
					this.Alpha = this.Group.alpha;
					this.Group.alpha = 0f;
				}
				if (this.Group.interactable)
				{
					this.Interactable = true;
					this.Group.interactable = false;
				}
				if (this.Group.blocksRaycasts)
				{
					this.BlocksRaycasts = true;
					this.Group.blocksRaycasts = false;
				}
				if (!this.Group.enabled)
				{
					this.Enabled = false;
					this.Group.enabled = true;
				}
			}

			// Token: 0x04004C61 RID: 19553
			public CanvasGroup Group;

			// Token: 0x04004C62 RID: 19554
			public bool Created;

			// Token: 0x04004C63 RID: 19555
			public int Owners;

			// Token: 0x04004C64 RID: 19556
			public float Alpha;

			// Token: 0x04004C65 RID: 19557
			public bool Interactable;

			// Token: 0x04004C66 RID: 19558
			public bool BlocksRaycasts;

			// Token: 0x04004C67 RID: 19559
			public bool Enabled;
		}
	}
}
