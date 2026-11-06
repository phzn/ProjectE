using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000035 RID: 53
	public class DC_Controller : MonoBehaviour
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000708C File Offset: 0x0000528C
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00007094 File Offset: 0x00005294
		public int ControllerID
		{
			get
			{
				return this._controllerID;
			}
			set
			{
				this._controllerID = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000709D File Offset: 0x0000529D
		// (set) Token: 0x0600013B RID: 315 RVA: 0x000070A5 File Offset: 0x000052A5
		public float ObjectsLifetime
		{
			get
			{
				return this._objectsLifetime;
			}
			set
			{
				this._objectsLifetime = Mathf.Max(0.1f, value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000070B8 File Offset: 0x000052B8
		// (set) Token: 0x0600013D RID: 317 RVA: 0x000070C0 File Offset: 0x000052C0
		public bool MergeInGroups
		{
			get
			{
				return this._mergeInGroups;
			}
			set
			{
				if (this._sourcesProvider != null)
				{
					Debug.Log("You can set 'MergeInGroups' option only before initialized");
					return;
				}
				this._mergeInGroups = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000070DC File Offset: 0x000052DC
		// (set) Token: 0x0600013F RID: 319 RVA: 0x000070E4 File Offset: 0x000052E4
		public float CellSize
		{
			get
			{
				return this._cellSize;
			}
			set
			{
				if (this._sourcesProvider != null)
				{
					Debug.Log("You can set 'Cell Size' option only before initialized");
					return;
				}
				this._cellSize = Mathf.Max(value, 0.1f);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000710A File Offset: 0x0000530A
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00007112 File Offset: 0x00005312
		public CullingMethod DefaultCullingMethod
		{
			get
			{
				return this._defaultCullingMethod;
			}
			set
			{
				this._defaultCullingMethod = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000711B File Offset: 0x0000531B
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00007123 File Offset: 0x00005323
		public bool DrawGizmos { get; set; }

		// Token: 0x06000144 RID: 324 RVA: 0x0000712C File Offset: 0x0000532C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ReloadDomain()
		{
			DC_Controller._controllersDic = null;
			DC_Controller._hitablesDic = null;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000713C File Offset: 0x0000533C
		private void Awake()
		{
			if (DC_Controller._controllersDic == null)
			{
				DC_Controller._controllersDic = new Dictionary<int, DC_Controller>();
			}
			if (DC_Controller._hitablesDic == null)
			{
				DC_Controller._hitablesDic = new Dictionary<Collider, IHitable>();
				SceneManager.sceneUnloaded += delegate(Scene s)
				{
					DC_Controller.ClearEmptyHitables();
				};
			}
			if (!DC_Controller._controllersDic.ContainsKey(this._controllerID))
			{
				DC_Controller._controllersDic.Add(this._controllerID, this);
			}
			else
			{
				Debug.Log("DynamicCullingController with id : " + this._controllerID.ToString() + " already exists!");
			}
			if (this._mergeInGroups)
			{
				this._sourcesProvider = new DC_SourcesTree(this._cellSize);
				this._treeDrawer = new BinaryTreeDrawer();
				return;
			}
			this._sourcesProvider = new DC_SingleSourcesProvider();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007203 File Offset: 0x00005403
		private void OnDestroy()
		{
			DC_Controller._controllersDic.Remove(this._controllerID);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007218 File Offset: 0x00005418
		private void OnDrawGizmosSelected()
		{
			if (!this.DrawGizmos)
			{
				return;
			}
			if (this._treeDrawer == null)
			{
				return;
			}
			DC_SourcesTree dc_SourcesTree = this._sourcesProvider as DC_SourcesTree;
			if (dc_SourcesTree.Root == null)
			{
				return;
			}
			this._treeDrawer.Color = Color.white;
			this._treeDrawer.DrawTreeGizmos(dc_SourcesTree.Root);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007270 File Offset: 0x00005470
		public DC_Camera AddCamera(Camera camera, int raysPerFrame)
		{
			DC_Camera dc_Camera;
			if (camera.TryGetComponent<DC_Camera>(out dc_Camera))
			{
				Debug.Log(camera.name + " already has DynamicCullingCamera component");
			}
			else
			{
				dc_Camera = camera.gameObject.AddComponent<DC_Camera>();
				dc_Camera.SetRaysCount(raysPerFrame);
			}
			return dc_Camera;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000072B2 File Offset: 0x000054B2
		public DC_SourceSettings AddObjectForCulling(MeshRenderer renderer)
		{
			return this.AddObjectForCulling(renderer, this._defaultCullingMethod);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000072C1 File Offset: 0x000054C1
		public DC_SourceSettings AddObjectForCulling(MeshRenderer renderer, CullingMethod cullingMethod)
		{
			DC_SourceSettings dc_SourceSettings = renderer.gameObject.AddComponent<DC_SourceSettings>();
			dc_SourceSettings.ControllerID = this._controllerID;
			dc_SourceSettings.SourceType = SourceType.SingleMesh;
			dc_SourceSettings.GetStrategy<DC_RendererSourceSettingsStrategy>().CullingMethod = cullingMethod;
			return dc_SourceSettings;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000072ED File Offset: 0x000054ED
		public DC_SourceSettings AddObjectForCulling(LODGroup lodGroup)
		{
			return this.AddObjectForCulling(lodGroup, this._defaultCullingMethod);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000072FC File Offset: 0x000054FC
		public DC_SourceSettings AddObjectForCulling(LODGroup lodGroup, CullingMethod cullingMethod)
		{
			DC_SourceSettings dc_SourceSettings = lodGroup.gameObject.AddComponent<DC_SourceSettings>();
			dc_SourceSettings.ControllerID = this._controllerID;
			dc_SourceSettings.SourceType = SourceType.LODGroup;
			dc_SourceSettings.GetStrategy<DC_LODGroupSourceSettingsStrategy>().CullingMethod = cullingMethod;
			return dc_SourceSettings;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007328 File Offset: 0x00005528
		public void AddObjectForCulling(ICullingTarget cullingTarget, IEnumerable<Collider> colliders)
		{
			DC_Source source = this._sourcesProvider.GetSource(cullingTarget);
			source.Lifetime = this._objectsLifetime;
			source.transform.parent = base.transform;
			cullingTarget.GameObject.AddComponent<DC_CullingTargetObserver>().Initialize(source, cullingTarget);
			foreach (Collider collider in colliders)
			{
				DC_Controller._hitablesDic.Add(collider, source);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000073B4 File Offset: 0x000055B4
		public static DC_Controller GetById(int id)
		{
			return DC_Controller._controllersDic[id];
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000073C1 File Offset: 0x000055C1
		public static int GetCullingLayer()
		{
			return LayerMask.NameToLayer(DC_Controller.GetCullingLayerName());
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000560D File Offset: 0x0000380D
		public static string GetCullingLayerName()
		{
			return "ACSCulling";
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000073D0 File Offset: 0x000055D0
		public static void ClearEmptyHitables()
		{
			List<Collider> list = new List<Collider>(DC_Controller._hitablesDic.Count);
			List<IHitable> list2 = new List<IHitable>(DC_Controller._hitablesDic.Count);
			foreach (KeyValuePair<Collider, IHitable> keyValuePair in DC_Controller._hitablesDic)
			{
				if (!(keyValuePair.Key == null))
				{
					list.Add(keyValuePair.Key);
					list2.Add(keyValuePair.Value);
				}
			}
			DC_Controller._hitablesDic.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				DC_Controller._hitablesDic.Add(list[i], list2[i]);
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000074A0 File Offset: 0x000056A0
		public static IReadOnlyDictionary<Collider, IHitable> GetHitables()
		{
			return DC_Controller._hitablesDic;
		}

		// Token: 0x040000F5 RID: 245
		private static Dictionary<int, DC_Controller> _controllersDic;

		// Token: 0x040000F6 RID: 246
		private static Dictionary<Collider, IHitable> _hitablesDic;

		// Token: 0x040000F8 RID: 248
		[SerializeField]
		private int _controllerID;

		// Token: 0x040000F9 RID: 249
		[SerializeField]
		[Min(0.1f)]
		private float _objectsLifetime = 2f;

		// Token: 0x040000FA RID: 250
		[SerializeField]
		private bool _mergeInGroups = true;

		// Token: 0x040000FB RID: 251
		[SerializeField]
		private float _cellSize = 10f;

		// Token: 0x040000FC RID: 252
		[SerializeField]
		private CullingMethod _defaultCullingMethod;

		// Token: 0x040000FD RID: 253
		private IDC_SourcesProvider _sourcesProvider;

		// Token: 0x040000FE RID: 254
		private BinaryTreeDrawer _treeDrawer;
	}
}
