using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B5 RID: 181
[CreateAssetMenu(fileName = "NPCActionRegistry", menuName = "NPC/Action Registry")]
public class NPCActionRegistry : ScriptableObject
{
	// Token: 0x0600072E RID: 1838 RVA: 0x00032AE4 File Offset: 0x00030CE4
	public void BuildCache()
	{
		this._byAnimatorIndex = new Dictionary<int, NPCActionDefinition>();
		this._byName = new Dictionary<string, NPCActionDefinition>();
		this._locomotionActions = new List<NPCActionDefinition>();
		this._totalLocomotionWeight = 0f;
		foreach (NPCActionDefinition npcactionDefinition in this.actions)
		{
			if (!(npcactionDefinition == null))
			{
				this._byAnimatorIndex[npcactionDefinition.animatorIndex] = npcactionDefinition;
				if (!string.IsNullOrEmpty(npcactionDefinition.displayName))
				{
					this._byName[npcactionDefinition.displayName.ToLower()] = npcactionDefinition;
				}
				if (!string.IsNullOrEmpty(npcactionDefinition.name))
				{
					this._byName[npcactionDefinition.name.ToLower()] = npcactionDefinition;
				}
				if (npcactionDefinition.canPerformWhileMoving)
				{
					this._locomotionActions.Add(npcactionDefinition);
					float num = ((npcactionDefinition.locomotionSpawnWeight > 0f) ? npcactionDefinition.locomotionSpawnWeight : this.defaultLocomotionWeight);
					this._totalLocomotionWeight += num;
				}
			}
		}
		this._isCacheBuilt = true;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x00032C10 File Offset: 0x00030E10
	private void EnsureCache()
	{
		if (!this._isCacheBuilt)
		{
			this.BuildCache();
		}
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00032C20 File Offset: 0x00030E20
	public NPCActionDefinition GetByAnimatorIndex(int index)
	{
		this.EnsureCache();
		NPCActionDefinition npcactionDefinition;
		if (!this._byAnimatorIndex.TryGetValue(index, out npcactionDefinition))
		{
			return this.defaultAction;
		}
		return npcactionDefinition;
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x00032C4C File Offset: 0x00030E4C
	public NPCActionDefinition GetByName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return this.defaultAction;
		}
		this.EnsureCache();
		NPCActionDefinition npcactionDefinition;
		if (!this._byName.TryGetValue(name.ToLower(), out npcactionDefinition))
		{
			return this.defaultAction;
		}
		return npcactionDefinition;
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00032C8C File Offset: 0x00030E8C
	public float GetAttentionLevel(int animatorIndex)
	{
		NPCActionDefinition byAnimatorIndex = this.GetByAnimatorIndex(animatorIndex);
		if (!(byAnimatorIndex != null))
		{
			return 0.5f;
		}
		return byAnimatorIndex.attentionLevel;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x00032CB8 File Offset: 0x00030EB8
	public bool CanPerformWhileMoving(int animatorIndex)
	{
		NPCActionDefinition byAnimatorIndex = this.GetByAnimatorIndex(animatorIndex);
		return byAnimatorIndex != null && byAnimatorIndex.canPerformWhileMoving;
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00032CDE File Offset: 0x00030EDE
	public string[] GetRequiredProps(int animatorIndex)
	{
		NPCActionDefinition byAnimatorIndex = this.GetByAnimatorIndex(animatorIndex);
		return ((byAnimatorIndex != null) ? byAnimatorIndex.requiredProps : null) ?? Array.Empty<string>();
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00032CFC File Offset: 0x00030EFC
	public bool IsLookAtAllowed(int animatorIndex)
	{
		NPCActionDefinition byAnimatorIndex = this.GetByAnimatorIndex(animatorIndex);
		return byAnimatorIndex == null || byAnimatorIndex.allowLookAt;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00032D22 File Offset: 0x00030F22
	public void InvalidateCache()
	{
		this._isCacheBuilt = false;
		this._byAnimatorIndex = null;
		this._byName = null;
		this._locomotionActions = null;
		this._totalLocomotionWeight = 0f;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00032D4B File Offset: 0x00030F4B
	public List<NPCActionDefinition> GetLocomotionActions()
	{
		this.EnsureCache();
		return this._locomotionActions;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00032D5C File Offset: 0x00030F5C
	public NPCActionDefinition GetRandomLocomotionAction()
	{
		this.EnsureCache();
		if (Random.value > this.locomotionActionChance)
		{
			return this.defaultAction;
		}
		if (this._locomotionActions == null || this._locomotionActions.Count == 0 || this._totalLocomotionWeight <= 0f)
		{
			return this.defaultAction;
		}
		float num = Random.value * this._totalLocomotionWeight;
		float num2 = 0f;
		foreach (NPCActionDefinition npcactionDefinition in this._locomotionActions)
		{
			float num3 = ((npcactionDefinition.locomotionSpawnWeight > 0f) ? npcactionDefinition.locomotionSpawnWeight : this.defaultLocomotionWeight);
			num2 += num3;
			if (num <= num2)
			{
				return npcactionDefinition;
			}
		}
		if (this._locomotionActions.Count <= 0)
		{
			return this.defaultAction;
		}
		return this._locomotionActions[this._locomotionActions.Count - 1];
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00032E5C File Offset: 0x0003105C
	private void OnEnable()
	{
		this._isCacheBuilt = false;
	}

	// Token: 0x040005FC RID: 1532
	[Header("Azioni Registrate")]
	[Tooltip("Lista di tutte le azioni NPC disponibili nel gioco")]
	public List<NPCActionDefinition> actions = new List<NPCActionDefinition>();

	// Token: 0x040005FD RID: 1533
	[Header("Azione Default")]
	[Tooltip("Azione da usare quando nessuna è specificata (tipicamente 'None/Idle')")]
	public NPCActionDefinition defaultAction;

	// Token: 0x040005FE RID: 1534
	[Header("Locomozione")]
	[Tooltip("Probabilità che venga scelta un'azione diversa da quella default durante la locomozione (0-1)")]
	[Range(0f, 1f)]
	public float locomotionActionChance = 0.3f;

	// Token: 0x040005FF RID: 1535
	[Tooltip("Peso default per azioni che non hanno un peso specifico configurato")]
	[Range(0.1f, 10f)]
	public float defaultLocomotionWeight = 1f;

	// Token: 0x04000600 RID: 1536
	private Dictionary<int, NPCActionDefinition> _byAnimatorIndex;

	// Token: 0x04000601 RID: 1537
	private Dictionary<string, NPCActionDefinition> _byName;

	// Token: 0x04000602 RID: 1538
	private List<NPCActionDefinition> _locomotionActions;

	// Token: 0x04000603 RID: 1539
	private float _totalLocomotionWeight;

	// Token: 0x04000604 RID: 1540
	private bool _isCacheBuilt;
}
