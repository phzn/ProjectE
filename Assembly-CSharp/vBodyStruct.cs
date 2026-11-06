using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000105 RID: 261
[CreateAssetMenu(menuName = "Invector/SnapBody/New Body Struct")]
public class vBodyStruct : ScriptableObject
{
	// Token: 0x0600097F RID: 2431 RVA: 0x00043EE9 File Offset: 0x000420E9
	protected virtual void Reset()
	{
		this.bones.Clear();
		this.bones = vBodyStruct.GetHumanBones();
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00043F04 File Offset: 0x00042104
	public static List<vBodyStruct.Bone> GetHumanBones()
	{
		List<vBodyStruct.Bone> list = new List<vBodyStruct.Bone>();
		string[] names = Enum.GetNames(typeof(HumanBodyBones));
		for (int i = 0; i < names.Length; i++)
		{
			if (!vBodyStruct.IsIgnoredBone(names[i]))
			{
				HumanBodyBones humanBodyBones = HumanBodyBones.Chest;
				if (names[i].ToEnum<HumanBodyBones>(ref humanBodyBones))
				{
					list.Add(new vBodyStruct.Bone
					{
						isHuman = true,
						name = names[i],
						genericBone = names[i],
						humanBone = humanBodyBones
					});
				}
			}
		}
		return (from x in list
			orderby x.name.ToUpper().Contains("LEFT"), x.name.ToUpper().Contains("RIGHT")
			select x).ToList<vBodyStruct.Bone>();
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x00043FD0 File Offset: 0x000421D0
	private static string[] ignoreBones
	{
		get
		{
			return new string[] { "Thumb", "Distal", "Little", "Middle", "Index", "Ring", "Eye", "Toes", "Jaw", "LastBone" };
		}
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00044038 File Offset: 0x00042238
	private static bool IsIgnoredBone(string bone)
	{
		bool flag = false;
		for (int i = 0; i < vBodyStruct.ignoreBones.Length; i++)
		{
			if (bone.Contains(vBodyStruct.ignoreBones[i]))
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x040008C0 RID: 2240
	public List<vBodyStruct.Bone> bones = new List<vBodyStruct.Bone>();

	// Token: 0x02000106 RID: 262
	[Serializable]
	public class Bone
	{
		// Token: 0x040008C1 RID: 2241
		public string name;

		// Token: 0x040008C2 RID: 2242
		public HumanBodyBones humanBone;

		// Token: 0x040008C3 RID: 2243
		public string genericBone;

		// Token: 0x040008C4 RID: 2244
		public bool isHuman = true;
	}
}
