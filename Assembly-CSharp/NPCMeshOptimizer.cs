using System;
using Bozo.AnimeCharacters;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000C2 RID: 194
public class NPCMeshOptimizer : MonoBehaviour
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060007AC RID: 1964 RVA: 0x00036365 File Offset: 0x00034565
	public bool IsOptimized
	{
		get
		{
			return this._isOptimized;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060007AD RID: 1965 RVA: 0x0003636D File Offset: 0x0003456D
	public bool IsOptimizing
	{
		get
		{
			return this._isOptimizing;
		}
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00036375 File Offset: 0x00034575
	private void Start()
	{
		this._outfitSystem = base.GetComponentInChildren<OutfitSystem>();
		if (this.autoOptimize && this._outfitSystem != null)
		{
			base.Invoke("OptimizeMesh", this.autoOptimizeDelay);
		}
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000363AC File Offset: 0x000345AC
	[ContextMenu("Optimize Mesh")]
	public void OptimizeMesh()
	{
		if (this._isOptimized || this._isOptimizing)
		{
			if (this.enableDebugLogs)
			{
				Debug.Log("[NPCMeshOptimizer] " + base.name + " already optimized or optimizing");
			}
			return;
		}
		if (this._outfitSystem == null)
		{
			this._outfitSystem = base.GetComponentInChildren<OutfitSystem>();
			if (this._outfitSystem == null)
			{
				Debug.LogWarning("[NPCMeshOptimizer] No OutfitSystem found on " + base.name);
				return;
			}
		}
		if (this._outfitSystem.mergeMaterial == null)
		{
			Debug.LogWarning("[NPCMeshOptimizer] No mergeMaterial assigned on OutfitSystem for " + base.name + ". Skipping optimization.");
			return;
		}
		this._isOptimizing = true;
		try
		{
			this._outfitSystem.MergeCharacter();
			this._isOptimized = true;
			base.Invoke("CleanupOrphanClones", this.cleanupDelay);
			if (this.enableDebugLogs)
			{
				Debug.Log("[NPCMeshOptimizer] " + base.name + " optimized successfully using OutfitSystem.MergeCharacter()");
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Concat(new string[] { "[NPCMeshOptimizer] Failed to optimize ", base.name, ": ", ex.Message, "\n", ex.StackTrace }));
		}
		finally
		{
			this._isOptimizing = false;
		}
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00036510 File Offset: 0x00034710
	public void OptimizeMeshWithDelay(float delay)
	{
		if (this._isOptimized || this._isOptimizing)
		{
			return;
		}
		if (delay <= 0f)
		{
			this.OptimizeMesh();
			return;
		}
		base.Invoke("OptimizeMesh", delay);
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0003653E File Offset: 0x0003473E
	public void CancelPendingOptimization()
	{
		base.CancelInvoke("OptimizeMesh");
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0003654C File Offset: 0x0003474C
	private void CleanupOrphanClones()
	{
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		int num = 0;
		foreach (GameObject gameObject in rootGameObjects)
		{
			if (gameObject.name.Contains("BMAC_MergedCharacterBase") && gameObject.name.Contains("Clone") && gameObject.GetComponent<NPCMeshOptimizer>() == null)
			{
				if (this.enableDebugLogs)
				{
					Debug.Log("[NPCMeshOptimizer] Cleaning up orphan clone: " + gameObject.name);
				}
				Object.Destroy(gameObject);
				num++;
			}
		}
		if (num > 0 && this.enableDebugLogs)
		{
			Debug.Log(string.Format("[NPCMeshOptimizer] Cleaned up {0} orphan clones", num));
		}
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x000365FC File Offset: 0x000347FC
	public static void CleanupAllOrphanClones()
	{
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		int num = 0;
		foreach (GameObject gameObject in rootGameObjects)
		{
			if (gameObject.name.Contains("BMAC_MergedCharacterBase") && gameObject.name.Contains("Clone") && gameObject.GetComponent<NPCMeshOptimizer>() == null)
			{
				Debug.Log("[NPCMeshOptimizer] Cleaning up orphan clone: " + gameObject.name);
				Object.Destroy(gameObject);
				num++;
			}
		}
		Debug.Log(string.Format("[NPCMeshOptimizer] Cleaned up {0} orphan clones total", num));
	}

	// Token: 0x04000695 RID: 1685
	[Header("Settings")]
	[Tooltip("Automatically optimize when enabled")]
	[SerializeField]
	private bool autoOptimize = true;

	// Token: 0x04000696 RID: 1686
	[Tooltip("Delay before auto-optimization (allows outfit loading to complete)")]
	[SerializeField]
	private float autoOptimizeDelay = 0.5f;

	// Token: 0x04000697 RID: 1687
	[Tooltip("Delay after merge to cleanup orphan clones")]
	[SerializeField]
	private float cleanupDelay = 1f;

	// Token: 0x04000698 RID: 1688
	[Header("Debug")]
	[SerializeField]
	private bool enableDebugLogs;

	// Token: 0x04000699 RID: 1689
	private bool _isOptimized;

	// Token: 0x0400069A RID: 1690
	private bool _isOptimizing;

	// Token: 0x0400069B RID: 1691
	private OutfitSystem _outfitSystem;
}
