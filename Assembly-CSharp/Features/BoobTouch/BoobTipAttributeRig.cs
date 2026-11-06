using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using MagicaCloth2;
using UnityEngine;

namespace Features.BoobTouch
{
	// Token: 0x020014D2 RID: 5330
	[DefaultExecutionOrder(-500)]
	public class BoobTipAttributeRig : MonoBehaviour
	{
		// Token: 0x0600A3EF RID: 41967 RVA: 0x00441EE8 File Offset: 0x004400E8
		private void Awake()
		{
			vThirdPersonInput componentInParent = base.GetComponentInParent<vThirdPersonInput>();
			foreach (MagicaCloth magicaCloth in ((componentInParent != null) ? componentInParent.transform : base.transform.root).GetComponentsInChildren<MagicaCloth>(true))
			{
				if (Array.IndexOf<string>(this.clothNames, magicaCloth.gameObject.name) >= 0)
				{
					Dictionary<Transform, VertexAttribute> boneAttributeDict = magicaCloth.GetSerializeData2().boneAttributeDict;
					foreach (Transform transform in magicaCloth.SerializeData.rootBones)
					{
						if (!(transform == null))
						{
							boneAttributeDict[transform] = VertexAttribute.Fixed;
							for (int j = 0; j < transform.childCount; j++)
							{
								boneAttributeDict[transform.GetChild(j)] = VertexAttribute.Fixed;
							}
						}
					}
				}
			}
		}

		// Token: 0x0400A7CC RID: 42956
		[SerializeField]
		private string[] clothNames = new string[] { "BoobL", "BoobR", "ButtL", "ButtR" };
	}
}
