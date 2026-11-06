using System;
using System.Collections;
using Bozo.AnimeCharacters;
using Features.NPC;
using UnityEngine;

// Token: 0x020000E3 RID: 227
[DisallowMultipleComponent]
public class PoolNpcConfigurator : MonoBehaviour, INpcPostSpawnConfigurator
{
	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000908 RID: 2312 RVA: 0x00041DE6 File Offset: 0x0003FFE6
	// (set) Token: 0x06000909 RID: 2313 RVA: 0x00041DED File Offset: 0x0003FFED
	public static bool IsActive { get; private set; }

	// Token: 0x0600090A RID: 2314 RVA: 0x00041DF5 File Offset: 0x0003FFF5
	private void OnEnable()
	{
		PoolNpcConfigurator.IsActive = true;
		this._retrofitRoutine = base.StartCoroutine(this.RetrofitSwimmersLoop());
		this._hookupRoutine = base.StartCoroutine(this.HookupWhenSpawnerReady());
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00041E21 File Offset: 0x00040021
	private IEnumerator HookupWhenSpawnerReady()
	{
		float elapsed = 0f;
		while (this._spawner == null)
		{
			this._spawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
			if (this._spawner != null)
			{
				break;
			}
			elapsed += Time.unscaledDeltaTime;
			if (elapsed >= this.spawnerWaitTimeout)
			{
				Debug.LogWarning(string.Format("[PoolNpcConfigurator] No NPCRuntimeSpawner found after {0:F0}s — Pool NPCs will not get dive/swim/climb behaviour.", this.spawnerWaitTimeout));
				this._hookupRoutine = null;
				yield break;
			}
			yield return null;
		}
		this._spawner.RegisterConfigurator(this);
		this._spawner.ApplyConfiguratorToExisting(this);
		this._hookupRoutine = null;
		Debug.Log("[PoolNpcConfigurator] Registered with NPCRuntimeSpawner — Pool NPCs will get dive/swim/climb behaviour.");
		yield break;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x00041E30 File Offset: 0x00040030
	private IEnumerator RetrofitSwimmersLoop()
	{
		WaitForSeconds wait = new WaitForSeconds(Mathf.Max(1f, this.retrofitScanInterval));
		while (base.isActiveAndEnabled)
		{
			this.RetrofitSwimmersOnce();
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x00041E40 File Offset: 0x00040040
	private void RetrofitSwimmersOnce()
	{
		foreach (AStarNPCController astarNPCController in Object.FindObjectsOfType<AStarNPCController>())
		{
			GameObject gameObject = astarNPCController.gameObject;
			if (gameObject.GetComponent<NPCPoolRecovery>() == null)
			{
				this.ConfigureRecovery(gameObject, astarNPCController);
			}
			if (PoolNpcConfigurator.IsSwimmerName(gameObject.name) && !(gameObject.GetComponent<NPCDiveController>() != null))
			{
				this.ConfigureSwimmer(gameObject, astarNPCController);
			}
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00041EA5 File Offset: 0x000400A5
	private static bool IsSwimmerName(string n)
	{
		return !string.IsNullOrEmpty(n) && (n.Contains("Swimmer") || n.Contains("Lifeguard"));
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00041ECC File Offset: 0x000400CC
	private void ConfigureSwimmer(GameObject npc, AStarNPCController controller)
	{
		if (npc.GetComponent<NPCDiveController>() != null)
		{
			return;
		}
		NPCWanderController componentInChildren = npc.GetComponentInChildren<NPCWanderController>(true);
		Animator animator = ((controller != null) ? controller.animator : npc.GetComponentInChildren<Animator>());
		if (this.poolMasterController != null && animator != null)
		{
			animator.runtimeAnimatorController = this.poolMasterController;
		}
		else if (this.poolMasterController == null)
		{
			Debug.LogWarning("[PoolNpcConfigurator] " + npc.name + ": pool swimmer but poolMasterController not assigned — dive/swim/climb animations will be missing.");
		}
		NPCDiveController npcdiveController = npc.AddComponent<NPCDiveController>();
		if (controller != null && componentInChildren != null)
		{
			npcdiveController.Initialize(controller, componentInChildren);
		}
		else
		{
			Debug.LogWarning("[PoolNpcConfigurator] " + npc.name + ": NPCDiveController added but AStar/Wander not found — dive will stay idle.");
		}
		if (this.poolSplashClip != null || this.poolUnderwaterClip != null || this.poolExitClip != null)
		{
			npcdiveController.AssignWaterAudioClips(this.poolSplashClip, this.poolUnderwaterClip, this.poolExitClip);
		}
		Debug.Log("[PoolNpcConfigurator] Retrofitted swimmer '" + npc.name + "' with NPCDiveController.");
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00041FF0 File Offset: 0x000401F0
	private void OnDisable()
	{
		PoolNpcConfigurator.IsActive = false;
		if (this._retrofitRoutine != null)
		{
			base.StopCoroutine(this._retrofitRoutine);
			this._retrofitRoutine = null;
		}
		if (this._hookupRoutine != null)
		{
			base.StopCoroutine(this._hookupRoutine);
			this._hookupRoutine = null;
		}
		if (this._spawner != null)
		{
			this._spawner.UnregisterConfigurator(this);
			this._spawner = null;
		}
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0004205C File Offset: 0x0004025C
	public void Configure(GameObject npc, CharacterObject characterData)
	{
		if (npc != null)
		{
			this.ConfigureRecovery(npc, npc.GetComponentInChildren<AStarNPCController>(true));
		}
		if (npc == null || characterData == null || string.IsNullOrEmpty(this.poolNpcPrefix))
		{
			return;
		}
		if (!characterData.name.StartsWith(this.poolNpcPrefix))
		{
			return;
		}
		NPCDiveController npcdiveController = npc.GetComponent<NPCDiveController>();
		if (npcdiveController == null)
		{
			npcdiveController = npc.AddComponent<NPCDiveController>();
		}
		AStarNPCController componentInChildren = npc.GetComponentInChildren<AStarNPCController>(true);
		NPCWanderController componentInChildren2 = npc.GetComponentInChildren<NPCWanderController>(true);
		Animator animator = ((componentInChildren != null) ? componentInChildren.animator : npc.GetComponentInChildren<Animator>());
		if (this.poolMasterController != null && animator != null)
		{
			animator.runtimeAnimatorController = this.poolMasterController;
		}
		else if (this.poolMasterController == null)
		{
			Debug.LogWarning("[PoolNpcConfigurator] " + npc.name + ": Pool NPC but poolMasterController not assigned — dive/swim/climb animations will be missing.");
		}
		if (componentInChildren != null && componentInChildren2 != null)
		{
			npcdiveController.Initialize(componentInChildren, componentInChildren2);
		}
		else
		{
			Debug.LogWarning("[PoolNpcConfigurator] " + npc.name + ": NPCDiveController added but AStarNPCController/NPCWanderController not found — dive will stay idle until initialized.");
		}
		if (this.poolSplashClip != null || this.poolUnderwaterClip != null || this.poolExitClip != null)
		{
			npcdiveController.AssignWaterAudioClips(this.poolSplashClip, this.poolUnderwaterClip, this.poolExitClip);
		}
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x000421B8 File Offset: 0x000403B8
	private void ConfigureRecovery(GameObject npc, AStarNPCController controller)
	{
		if (controller == null || this.poolMasterController == null)
		{
			return;
		}
		if (this._recoverySwimClip == null || this._recoveryClimbClip == null)
		{
			foreach (AnimationClip animationClip in this.poolMasterController.animationClips)
			{
				if (animationClip.name == "Swimming_Above")
				{
					this._recoverySwimClip = animationClip;
				}
				if (animationClip.name == "ClimbUp")
				{
					this._recoveryClimbClip = animationClip;
				}
			}
		}
		NPCPoolRecovery npcpoolRecovery = controller.GetComponent<NPCPoolRecovery>();
		if (npcpoolRecovery == null)
		{
			npcpoolRecovery = controller.gameObject.AddComponent<NPCPoolRecovery>();
		}
		npcpoolRecovery.Configure(this._recoverySwimClip, this._recoveryClimbClip);
	}

	// Token: 0x04000823 RID: 2083
	[Header("Pool NPCs (a tema piscina)")]
	[Tooltip("Animator controller dedicato assegnato ai soli NPC con nome 'Pool_*' (duplicato con stati dive/swim/climb). Generato da Tools/NPC/Build Pool Animator Controller.")]
	[SerializeField]
	private RuntimeAnimatorController poolMasterController;

	// Token: 0x04000824 RID: 2084
	[Tooltip("Prefisso che identifica gli NPC della piscina (ricevono controller dedicato + NPCDiveController).")]
	[SerializeField]
	private string poolNpcPrefix = "Pool_";

	// Token: 0x04000825 RID: 2085
	[Header("Pool NPC water audio (spaziale, assegnabili per il build)")]
	[Tooltip("Splash al tuffo (Invector waterSplash).")]
	[SerializeField]
	private AudioClip poolSplashClip;

	// Token: 0x04000826 RID: 2086
	[Tooltip("Loop sott'acqua mentre nuota (Invector underwater).")]
	[SerializeField]
	private AudioClip poolUnderwaterClip;

	// Token: 0x04000827 RID: 2087
	[Tooltip("One-shot all'uscita dall'acqua (Invector exitWater).")]
	[SerializeField]
	private AudioClip poolExitClip;

	// Token: 0x04000828 RID: 2088
	[Header("Hookup")]
	[Tooltip("Quanto attendere (s) che l'NPCRuntimeSpawner del _Core compaia prima di rinunciare. Il _Core viene caricato in additive: lo spawner può non esistere ancora all'OnEnable di questa scena.")]
	[SerializeField]
	private float spawnerWaitTimeout = 15f;

	// Token: 0x04000829 RID: 2089
	private NPCRuntimeSpawner _spawner;

	// Token: 0x0400082A RID: 2090
	private Coroutine _hookupRoutine;

	// Token: 0x0400082B RID: 2091
	private Coroutine _retrofitRoutine;

	// Token: 0x0400082C RID: 2092
	private AnimationClip _recoverySwimClip;

	// Token: 0x0400082D RID: 2093
	private AnimationClip _recoveryClimbClip;

	// Token: 0x0400082F RID: 2095
	[Header("Retrofit scan")]
	[Tooltip("Intervallo di controllo degli NPC spawnati dopo la registrazione (minimo un secondo).")]
	[SerializeField]
	private float retrofitScanInterval = 0.5f;
}
