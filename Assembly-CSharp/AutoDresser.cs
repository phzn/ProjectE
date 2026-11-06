using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200001F RID: 31
[ExecuteInEditMode]
[AddComponentMenu("Auto Dresser/Auto Dresser")]
public class AutoDresser : MonoBehaviour
{
	// Token: 0x060000EF RID: 239 RVA: 0x00008250 File Offset: 0x00006450
	private void Start()
	{
		if (base.transform != base.transform.root)
		{
			base.transform.root.gameObject.AddComponent<AutoDresser>();
			Object.DestroyImmediate(this);
			return;
		}
		if (this.cloth_hip_bone != null)
		{
			return;
		}
		try
		{
			this.cloth_hip_bone = base.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);
			this.cloth_bone_array = this.cloth_hip_bone.gameObject.GetComponentsInChildren<Transform>();
		}
		catch (MissingComponentException)
		{
			this.cloth_not_humanoid = true;
			this.cloth_not_humanoid_hipbone_set = false;
		}
		catch (NullReferenceException)
		{
			this.cloth_not_humanoid = true;
			this.cloth_not_humanoid_hipbone_set = false;
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000830C File Offset: 0x0000650C
	private void OnValidate()
	{
		if (!this.cloth_not_humanoid_hipbone_set && this.cloth_hip_bone != null)
		{
			this.cloth_bone_array = this.cloth_hip_bone.gameObject.GetComponentsInChildren<Transform>();
			this.cloth_not_humanoid_hipbone_set = true;
		}
		else if (this.cloth_not_humanoid_hipbone_set && this.cloth_hip_bone == null)
		{
			this.cloth_not_humanoid_hipbone_set = false;
		}
		if (this.body_object != null && this.cloth_hip_bone != null)
		{
			try
			{
				this.body_hip_bone = this.body_object.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips);
			}
			catch (MissingComponentException)
			{
				this.body_object = null;
			}
			catch (NullReferenceException)
			{
				this.body_object = null;
			}
			if (this.body_object != null)
			{
				string[] array = new string[] { this.body_hip_bone.name };
				string[] array2 = this.cloth_hip_bone.name.Split(array, StringSplitOptions.None);
				if (array2.Length == 2)
				{
					this.prefix = array2[0];
					this.suffix = array2[1];
				}
				else
				{
					this.prefix = null;
					this.suffix = null;
				}
			}
		}
		if (this.cloth_hip_bone == this.body_hip_bone && this.cloth_hip_bone != null)
		{
			this.body_object = null;
			this.body_hip_bone = null;
		}
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00008464 File Offset: 0x00006664
	public void Kiseru()
	{
		if (this.body_object == null)
		{
			this.toggle = false;
			return;
		}
		Transform[] componentsInChildren = this.body_hip_bone.gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform transform in this.cloth_bone_array)
		{
			foreach (Transform transform2 in componentsInChildren)
			{
				if (transform.name == this.prefix + transform2.name + this.suffix)
				{
					this.parent_list_k.Add(transform.name);
					this.parent_list_v.Add(transform.parent);
					transform.parent = transform2;
					this.toggle = true;
					break;
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = base.transform.root.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		if (this.toggle)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren2)
			{
				if (skinnedMeshRenderer.gameObject.transform.parent == base.transform.root)
				{
					this.cloth_object_list.Add(skinnedMeshRenderer.gameObject.transform);
					skinnedMeshRenderer.gameObject.transform.parent = this.body_object.transform;
				}
			}
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000085C0 File Offset: 0x000067C0
	public void Modosu()
	{
		Dictionary<string, Transform> dictionary = Enumerable.Range(0, this.parent_list_k.Count<string>()).ToDictionary<int, string, Transform>((int i) => this.parent_list_k[i], (int i) => this.parent_list_v[i]);
		foreach (Transform transform in this.cloth_bone_array)
		{
			Transform transform2;
			if (dictionary.TryGetValue(transform.name, out transform2))
			{
				transform.parent = transform2;
			}
		}
		foreach (Transform transform3 in this.cloth_object_list)
		{
			transform3.parent = this.cloth_hip_bone.root;
		}
		this.parent_list_k.Clear();
		this.parent_list_v.Clear();
		this.cloth_object_list.Clear();
		this.toggle = false;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x000086A8 File Offset: 0x000068A8
	public bool Toggle_Check()
	{
		return this.toggle;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000086B0 File Offset: 0x000068B0
	public bool CNH_Check()
	{
		return this.cloth_not_humanoid;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x000086B8 File Offset: 0x000068B8
	public bool CNHHS_Check()
	{
		return this.cloth_not_humanoid_hipbone_set;
	}

	// Token: 0x040000BB RID: 187
	public Transform cloth_hip_bone;

	// Token: 0x040000BC RID: 188
	public GameObject body_object;

	// Token: 0x040000BD RID: 189
	public Transform body_hip_bone;

	// Token: 0x040000BE RID: 190
	public List<string> parent_list_k = new List<string>();

	// Token: 0x040000BF RID: 191
	public List<Transform> parent_list_v = new List<Transform>();

	// Token: 0x040000C0 RID: 192
	public Transform[] cloth_bone_array;

	// Token: 0x040000C1 RID: 193
	public string prefix;

	// Token: 0x040000C2 RID: 194
	public string suffix;

	// Token: 0x040000C3 RID: 195
	public bool toggle;

	// Token: 0x040000C4 RID: 196
	public bool cloth_not_humanoid;

	// Token: 0x040000C5 RID: 197
	public bool cloth_not_humanoid_hipbone_set = true;

	// Token: 0x040000C6 RID: 198
	public List<Transform> cloth_object_list = new List<Transform>();
}
