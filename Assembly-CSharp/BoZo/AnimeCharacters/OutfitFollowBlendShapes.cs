using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008BE RID: 2238
	public class OutfitFollowBlendShapes : MonoBehaviour
	{
		// Token: 0x06003C25 RID: 15397 RVA: 0x00165CDC File Offset: 0x00163EDC
		public void Start()
		{
			OutfitSystem componentInParent = base.GetComponentInParent<OutfitSystem>();
			this.mesh = base.GetComponentInChildren<SkinnedMeshRenderer>();
			if (componentInParent == null)
			{
				return;
			}
			this.character = componentInParent.GetCharacterBody();
			string text = this.character.sharedMesh.GetBlendShapeName(0).Split('.', StringSplitOptions.None)[0];
			this.shapes.Clear();
			for (int i = 0; i < this.mesh.sharedMesh.blendShapeCount; i++)
			{
				string text2 = this.mesh.sharedMesh.GetBlendShapeName(i).Split(".", StringSplitOptions.None)[1];
				int blendShapeIndex = this.character.sharedMesh.GetBlendShapeIndex(text + "." + text2);
				if (blendShapeIndex != -1)
				{
					this.shapes.Add(blendShapeIndex);
				}
			}
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x00165DA4 File Offset: 0x00163FA4
		private void Update()
		{
			for (int i = 0; i < this.shapes.Count; i++)
			{
				this.mesh.SetBlendShapeWeight(i, this.character.GetBlendShapeWeight(this.shapes[i]));
			}
		}

		// Token: 0x04003C3A RID: 15418
		private SkinnedMeshRenderer mesh;

		// Token: 0x04003C3B RID: 15419
		private SkinnedMeshRenderer character;

		// Token: 0x04003C3C RID: 15420
		[SerializeField]
		private List<int> shapes = new List<int>();
	}
}
