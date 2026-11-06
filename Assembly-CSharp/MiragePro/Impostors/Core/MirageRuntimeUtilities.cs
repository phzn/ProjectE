using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiragePro.Impostors.Core
{
	// Token: 0x0200027B RID: 635
	public class MirageRuntimeUtilities
	{
		// Token: 0x06001086 RID: 4230 RVA: 0x0006DBB8 File Offset: 0x0006BDB8
		public static bool IsPartOfLODGroup(Renderer renderer)
		{
			LODGroup componentInParent = renderer.GetComponentInParent<LODGroup>();
			if (componentInParent != null)
			{
				LOD[] lods = componentInParent.GetLODs();
				for (int i = 0; i < lods.Length; i++)
				{
					Renderer[] renderers = lods[i].renderers;
					for (int j = 0; j < renderers.Length; j++)
					{
						if (renderers[j] == renderer)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0006DC20 File Offset: 0x0006BE20
		public static LODGroup GetSingleLODGroup(List<MeshFilter> meshFilters, out bool valid)
		{
			valid = false;
			if (meshFilters.Count == 0)
			{
				return null;
			}
			LODGroup lodgroup = null;
			int? num = null;
			foreach (MeshFilter meshFilter in meshFilters)
			{
				if (!(meshFilter == null))
				{
					Renderer component = meshFilter.GetComponent<Renderer>();
					if (!(component == null))
					{
						LODGroup componentInParent = component.GetComponentInParent<LODGroup>();
						if (!(componentInParent == null))
						{
							int lodindex = MirageRuntimeUtilities.GetLODIndex(componentInParent, component);
							if (!(lodgroup == null) || num != null)
							{
								if (!(componentInParent != lodgroup))
								{
									int? num2 = num;
									int num3 = lodindex;
									if ((num2.GetValueOrDefault() == num3) & (num2 != null))
									{
										continue;
									}
								}
								return lodgroup;
							}
							lodgroup = componentInParent;
							num = new int?(lodindex);
						}
					}
				}
			}
			valid = true;
			return lodgroup;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0006DD10 File Offset: 0x0006BF10
		public static void CleanLODGroup(LODGroup lodGroup)
		{
			LOD[] lods = lodGroup.GetLODs();
			List<LOD> list = new List<LOD>();
			for (int i = 0; i < lods.Length; i++)
			{
				if (lods[i].renderers != null && lods[i].renderers != null && lods[i].renderers.Length != 0 && lods[i].renderers[0] != null)
				{
					list.Add(lods[i]);
				}
			}
			lodGroup.SetLODs(list.ToArray());
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0006DD94 File Offset: 0x0006BF94
		private static int GetLODIndex(LODGroup lodGroup, Renderer renderer)
		{
			LOD[] lods = lodGroup.GetLODs();
			for (int i = 0; i < lods.Length; i++)
			{
				for (int j = 0; j < lods[i].renderers.Length; j++)
				{
					if (lods[i].renderers[j] == renderer)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0006DDE8 File Offset: 0x0006BFE8
		public static List<MeshFilter> GetAllDescendantMeshFilters(GameObject ancestor)
		{
			List<MeshFilter> list = new List<MeshFilter>();
			MirageRuntimeUtilities.GetMeshFiltersRecursive(ancestor.transform, list);
			return list;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0006DE08 File Offset: 0x0006C008
		private static void GetMeshFiltersRecursive(Transform current, List<MeshFilter> meshFilters)
		{
			MeshFilter component = current.GetComponent<MeshFilter>();
			if (component != null)
			{
				bool flag;
				MirageRuntimeUtilities.ValidateCandidate(component, out flag);
				if (flag)
				{
					meshFilters.Add(component);
				}
			}
			foreach (object obj in current)
			{
				MirageRuntimeUtilities.GetMeshFiltersRecursive((Transform)obj, meshFilters);
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0006DE80 File Offset: 0x0006C080
		public static GameObject GetCommonAncestor(List<MeshFilter> meshFilters)
		{
			if (meshFilters.Count == 0)
			{
				return null;
			}
			Transform transform = null;
			int num = 0;
			while (transform == null && num < meshFilters.Count)
			{
				transform = ((meshFilters[num] != null) ? meshFilters[num].gameObject.transform : null);
				num++;
			}
			if (transform == null)
			{
				return null;
			}
			while (transform != null)
			{
				bool flag = true;
				foreach (MeshFilter meshFilter in meshFilters)
				{
					if (meshFilter != null)
					{
						Transform transform2 = meshFilter.gameObject.transform;
						bool flag2 = false;
						while (transform2 != null)
						{
							if (transform2 == transform)
							{
								flag2 = true;
								break;
							}
							transform2 = transform2.parent;
						}
						if (!flag2)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					return transform.gameObject;
				}
				transform = transform.parent;
			}
			return null;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0006DF84 File Offset: 0x0006C184
		public static bool IsEmpty(List<MeshFilter> filters)
		{
			using (List<MeshFilter>.Enumerator enumerator = filters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0006DFDC File Offset: 0x0006C1DC
		public static string ValidateCandidate(MeshFilter filter, out bool valid)
		{
			valid = false;
			if (!(filter != null))
			{
				return "Mesh filter is null";
			}
			MeshRenderer component = filter.GetComponent<MeshRenderer>();
			if (!(component != null))
			{
				return "No mesh renderer attached";
			}
			if (!(component.sharedMaterial != null) || !(component.sharedMaterial.shader != null))
			{
				return "The mesh renderer has no valid material";
			}
			string name = component.sharedMaterial.shader.name;
			if (!name.Contains("Mirage") || !name.Contains("Impostor"))
			{
				valid = true;
				return "";
			}
			return "The attached renderer is already an impostor";
		}
	}
}
