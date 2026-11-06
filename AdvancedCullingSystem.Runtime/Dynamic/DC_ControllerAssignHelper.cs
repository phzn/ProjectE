using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000056 RID: 86
	public static class DC_ControllerAssignHelper
	{
		// Token: 0x0600021E RID: 542 RVA: 0x000093F8 File Offset: 0x000075F8
		public static void AssignSources(this DC_Controller controller, GameObject parent = null, CullingMethod cullingMethod = CullingMethod.KeepShadows)
		{
			LODGroup[] array = DC_ControllerAssignHelper.FindLODGroups(parent);
			MeshRenderer[] array2 = DC_ControllerAssignHelper.FindMeshRenderers(parent);
			HashSet<Renderer> hashSet;
			DC_ControllerAssignHelper.ProcessLODGroups(controller, array, cullingMethod, out hashSet);
			DC_ControllerAssignHelper.ProcessMeshRenderers(controller, array2, hashSet, cullingMethod);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009428 File Offset: 0x00007628
		public static void AssignSourcesFast(this DC_Controller controller, GameObject parent = null, CullingMethod cullingMethod = CullingMethod.KeepShadows)
		{
			LODGroup[] array = DC_ControllerAssignHelper.FindLODGroups(parent);
			MeshRenderer[] array2 = DC_ControllerAssignHelper.FindMeshRenderers(parent);
			DC_ControllerAssignHelper.ProcessLODGroupsFast(controller, array, cullingMethod);
			DC_ControllerAssignHelper.ProcessMeshRenderersFast(controller, array2, cullingMethod);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00009453 File Offset: 0x00007653
		private static LODGroup[] FindLODGroups(GameObject parent)
		{
			if (parent == null)
			{
				return UnityAPI.FindObjectsOfType<LODGroup>();
			}
			return parent.GetComponentsInChildren<LODGroup>();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000946A File Offset: 0x0000766A
		private static MeshRenderer[] FindMeshRenderers(GameObject parent)
		{
			if (parent == null)
			{
				return UnityAPI.FindObjectsOfType<MeshRenderer>();
			}
			return parent.GetComponentsInChildren<MeshRenderer>();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00009484 File Offset: 0x00007684
		private static void ProcessLODGroups(DC_Controller controller, LODGroup[] groups, CullingMethod cullingMethod, out HashSet<Renderer> lodRenderers)
		{
			lodRenderers = new HashSet<Renderer>();
			foreach (LODGroup lodgroup in groups)
			{
				LOD[] lods = lodgroup.GetLODs();
				for (int j = 0; j < lods.Length; j++)
				{
					foreach (Renderer renderer in lods[j].renderers)
					{
						lodRenderers.Add(renderer);
					}
				}
				if (DC_ControllerAssignHelper.CheckLODGroup(lodgroup))
				{
					controller.AddObjectForCulling(lodgroup, cullingMethod);
				}
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000950C File Offset: 0x0000770C
		private static void ProcessLODGroupsFast(DC_Controller controller, LODGroup[] groups, CullingMethod cullingMethod)
		{
			foreach (LODGroup lodgroup in groups)
			{
				if (DC_ControllerAssignHelper.CheckLODGroup(lodgroup))
				{
					controller.AddObjectForCulling(lodgroup, cullingMethod);
				}
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00009540 File Offset: 0x00007740
		private static bool CheckLODGroup(LODGroup group)
		{
			return group.gameObject.activeInHierarchy && !(group.GetComponent<DC_IgnoreByAssign>() != null) && !(group.GetComponent<DC_SourceSettings>() != null) && !(group.GetComponent<DC_Occluder>() != null);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009590 File Offset: 0x00007790
		private static void ProcessMeshRenderers(DC_Controller controller, MeshRenderer[] renderers, HashSet<Renderer> lodRenderers, CullingMethod cullingMethod)
		{
			foreach (MeshRenderer meshRenderer in renderers)
			{
				if (DC_ControllerAssignHelper.CheckMeshRenderer(meshRenderer) && !lodRenderers.Contains(meshRenderer))
				{
					controller.AddObjectForCulling(meshRenderer, cullingMethod);
				}
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000095CC File Offset: 0x000077CC
		private static void ProcessMeshRenderersFast(DC_Controller controller, MeshRenderer[] renderers, CullingMethod cullingMethod)
		{
			foreach (MeshRenderer meshRenderer in renderers)
			{
				if (DC_ControllerAssignHelper.CheckMeshRenderer(meshRenderer) && !(meshRenderer.GetComponentInParent<LODGroup>() != null))
				{
					controller.AddObjectForCulling(meshRenderer, cullingMethod);
				}
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000960C File Offset: 0x0000780C
		private static bool CheckMeshRenderer(MeshRenderer renderer)
		{
			return renderer.gameObject.activeInHierarchy && !(renderer.GetComponent<DC_IgnoreByAssign>() != null) && !(renderer.GetComponent<DC_SourceSettings>() != null) && !(renderer.GetComponent<DC_Occluder>() != null);
		}
	}
}
