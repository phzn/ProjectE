using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F2E RID: 3886
	public class SceneContext
	{
		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x060074A0 RID: 29856 RVA: 0x002F4FE0 File Offset: 0x002F31E0
		// (set) Token: 0x060074A1 RID: 29857 RVA: 0x002F4FE8 File Offset: 0x002F31E8
		public SceneDefinitionSO Definition { get; private set; }

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x060074A2 RID: 29858 RVA: 0x002F4FF1 File Offset: 0x002F31F1
		// (set) Token: 0x060074A3 RID: 29859 RVA: 0x002F4FF9 File Offset: 0x002F31F9
		public SceneTrigger Trigger { get; private set; }

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x060074A4 RID: 29860 RVA: 0x002F5002 File Offset: 0x002F3202
		// (set) Token: 0x060074A5 RID: 29861 RVA: 0x002F500A File Offset: 0x002F320A
		public Dictionary<string, GameObject> SpawnedNPCs { get; private set; } = new Dictionary<string, GameObject>();

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x060074A6 RID: 29862 RVA: 0x002F5013 File Offset: 0x002F3213
		// (set) Token: 0x060074A7 RID: 29863 RVA: 0x002F501B File Offset: 0x002F321B
		public Dictionary<string, AStarNPCController> NPCControllers { get; private set; } = new Dictionary<string, AStarNPCController>();

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x060074A8 RID: 29864 RVA: 0x002F5024 File Offset: 0x002F3224
		// (set) Token: 0x060074A9 RID: 29865 RVA: 0x002F502C File Offset: 0x002F322C
		public Dictionary<string, Animator> NPCAnimators { get; private set; } = new Dictionary<string, Animator>();

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x060074AA RID: 29866 RVA: 0x002F5035 File Offset: 0x002F3235
		// (set) Token: 0x060074AB RID: 29867 RVA: 0x002F503D File Offset: 0x002F323D
		public Dictionary<string, Transform> Waypoints { get; private set; } = new Dictionary<string, Transform>();

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x060074AC RID: 29868 RVA: 0x002F5046 File Offset: 0x002F3246
		// (set) Token: 0x060074AD RID: 29869 RVA: 0x002F504E File Offset: 0x002F324E
		public Dictionary<string, Transform> SpawnPoints { get; private set; } = new Dictionary<string, Transform>();

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x060074AE RID: 29870 RVA: 0x002F5057 File Offset: 0x002F3257
		// (set) Token: 0x060074AF RID: 29871 RVA: 0x002F505F File Offset: 0x002F325F
		public bool IsInterrupted { get; set; }

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x060074B0 RID: 29872 RVA: 0x002F5068 File Offset: 0x002F3268
		// (set) Token: 0x060074B1 RID: 29873 RVA: 0x002F5070 File Offset: 0x002F3270
		public Transform PlayerTransform { get; set; }

		// Token: 0x060074B2 RID: 29874 RVA: 0x002F507C File Offset: 0x002F327C
		public SceneContext(SceneDefinitionSO definition, SceneTrigger trigger)
		{
			this.Definition = definition;
			this.Trigger = trigger;
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x002F50D4 File Offset: 0x002F32D4
		public void RegisterNPC(string slotId, GameObject npc)
		{
			this.SpawnedNPCs[slotId] = npc;
			AStarNPCController component = npc.GetComponent<AStarNPCController>();
			if (component != null)
			{
				this.NPCControllers[slotId] = component;
			}
			Animator componentInChildren = npc.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				this.NPCAnimators[slotId] = componentInChildren;
			}
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x002F5128 File Offset: 0x002F3328
		public GameObject GetNPC(string slotId)
		{
			GameObject gameObject;
			this.SpawnedNPCs.TryGetValue(slotId, out gameObject);
			return gameObject;
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x002F5148 File Offset: 0x002F3348
		public AStarNPCController GetController(string slotId)
		{
			AStarNPCController astarNPCController;
			this.NPCControllers.TryGetValue(slotId, out astarNPCController);
			return astarNPCController;
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x002F5168 File Offset: 0x002F3368
		public Animator GetAnimator(string slotId)
		{
			Animator animator;
			this.NPCAnimators.TryGetValue(slotId, out animator);
			return animator;
		}

		// Token: 0x060074B7 RID: 29879 RVA: 0x002F5188 File Offset: 0x002F3388
		public Transform GetWaypoint(string name)
		{
			Transform transform;
			this.Waypoints.TryGetValue(name, out transform);
			return transform;
		}

		// Token: 0x060074B8 RID: 29880 RVA: 0x002F51A8 File Offset: 0x002F33A8
		public Transform GetSpawnPoint(string name)
		{
			Transform transform;
			this.SpawnPoints.TryGetValue(name, out transform);
			return transform;
		}
	}
}
