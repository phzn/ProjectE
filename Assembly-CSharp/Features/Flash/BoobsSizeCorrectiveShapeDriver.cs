using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using UnityEngine;

namespace Features.Flash
{
	// Token: 0x02001204 RID: 4612
	public class BoobsSizeCorrectiveShapeDriver : MonoBehaviour
	{
		// Token: 0x06008E63 RID: 36451 RVA: 0x003A3990 File Offset: 0x003A1B90
		public static BoobsSizeCorrectiveShapeDriver EnsureOn(GameObject host)
		{
			if (host == null)
			{
				return null;
			}
			BoobsSizeCorrectiveShapeDriver boobsSizeCorrectiveShapeDriver = host.GetComponent<BoobsSizeCorrectiveShapeDriver>();
			if (boobsSizeCorrectiveShapeDriver == null)
			{
				boobsSizeCorrectiveShapeDriver = host.AddComponent<BoobsSizeCorrectiveShapeDriver>();
			}
			return boobsSizeCorrectiveShapeDriver;
		}

		// Token: 0x06008E64 RID: 36452 RVA: 0x003A39C0 File Offset: 0x003A1BC0
		private void OnEnable()
		{
			if (!this._scanned)
			{
				this.ScanCorrectiveShapes();
			}
		}

		// Token: 0x06008E65 RID: 36453 RVA: 0x003A39D0 File Offset: 0x003A1BD0
		private void ScanCorrectiveShapes()
		{
			this._scanned = true;
			this._groups.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
				if (!(sharedMesh == null) && sharedMesh.blendShapeCount != 0)
				{
					Dictionary<string, BoobsSizeCorrectiveShapeDriver.CorrectiveGroup> dictionary = null;
					for (int j = 0; j < sharedMesh.blendShapeCount; j++)
					{
						string text = sharedMesh.GetBlendShapeName(j).Trim();
						for (int k = 0; k < BoobsSizeCorrectiveShapeDriver.SuffixByBoobsType.Length; k++)
						{
							string text2 = BoobsSizeCorrectiveShapeDriver.SuffixByBoobsType[k];
							if (text2 != null)
							{
								string text3 = "_" + text2 + "_Corr";
								if (text.EndsWith(text3))
								{
									string text4 = text.Substring(0, text.Length - text3.Length);
									int num = BoobsSizeCorrectiveShapeDriver.FindBaseIndexTrimmed(sharedMesh, text4);
									if (num < 0)
									{
										Debug.LogWarning(string.Concat(new string[] { "[BoobsSizeCorrectiveShapeDriver] Corrective '", text, "' on '", skinnedMeshRenderer.name, "' has no base shape '", text4, "': ignored." }), this);
										break;
									}
									if (dictionary == null)
									{
										dictionary = new Dictionary<string, BoobsSizeCorrectiveShapeDriver.CorrectiveGroup>();
									}
									BoobsSizeCorrectiveShapeDriver.CorrectiveGroup correctiveGroup;
									if (!dictionary.TryGetValue(text4, out correctiveGroup))
									{
										correctiveGroup = new BoobsSizeCorrectiveShapeDriver.CorrectiveGroup
										{
											renderer = skinnedMeshRenderer,
											baseIndex = num
										};
										dictionary.Add(text4, correctiveGroup);
										this._groups.Add(correctiveGroup);
									}
									correctiveGroup.correctiveIndexBySize[k] = j;
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06008E66 RID: 36454 RVA: 0x003A3B6C File Offset: 0x003A1D6C
		private static int FindBaseIndexTrimmed(Mesh mesh, string baseName)
		{
			int blendShapeIndex = mesh.GetBlendShapeIndex(baseName);
			if (blendShapeIndex >= 0)
			{
				return blendShapeIndex;
			}
			for (int i = 0; i < mesh.blendShapeCount; i++)
			{
				if (mesh.GetBlendShapeName(i).Trim() == baseName)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06008E67 RID: 36455 RVA: 0x003A3BB0 File Offset: 0x003A1DB0
		private void LateUpdate()
		{
			if (this._groups.Count == 0)
			{
				return;
			}
			int num = this.ResolveActiveSize();
			for (int i = 0; i < this._groups.Count; i++)
			{
				BoobsSizeCorrectiveShapeDriver.CorrectiveGroup correctiveGroup = this._groups[i];
				float blendShapeWeight = correctiveGroup.renderer.GetBlendShapeWeight(correctiveGroup.baseIndex);
				for (int j = 0; j < correctiveGroup.correctiveIndexBySize.Length; j++)
				{
					int num2 = correctiveGroup.correctiveIndexBySize[j];
					if (num2 >= 0)
					{
						correctiveGroup.renderer.SetBlendShapeWeight(num2, (j == num) ? blendShapeWeight : 0f);
					}
				}
			}
		}

		// Token: 0x06008E68 RID: 36456 RVA: 0x003A3C48 File Offset: 0x003A1E48
		private int ResolveActiveSize()
		{
			if (!this._controllerSearched)
			{
				this._controllerSearched = true;
				this._boobsController = base.GetComponentInParent<BoobsController>();
			}
			if (this._boobsController == null)
			{
				return 3;
			}
			return Mathf.Clamp(this._boobsController.CurrentBoobsTypeValue, 0, BoobsSizeCorrectiveShapeDriver.SuffixByBoobsType.Length - 1);
		}

		// Token: 0x040091E5 RID: 37349
		private static readonly string[] SuffixByBoobsType = new string[] { "00_Flat", "02_Smaller", "01_Small", null, "03_Natural", "04_Balloon" };

		// Token: 0x040091E6 RID: 37350
		private const int DefaultBoobsType = 3;

		// Token: 0x040091E7 RID: 37351
		private readonly List<BoobsSizeCorrectiveShapeDriver.CorrectiveGroup> _groups = new List<BoobsSizeCorrectiveShapeDriver.CorrectiveGroup>();

		// Token: 0x040091E8 RID: 37352
		private BoobsController _boobsController;

		// Token: 0x040091E9 RID: 37353
		private bool _controllerSearched;

		// Token: 0x040091EA RID: 37354
		private bool _scanned;

		// Token: 0x02001205 RID: 4613
		private class CorrectiveGroup
		{
			// Token: 0x040091EB RID: 37355
			public SkinnedMeshRenderer renderer;

			// Token: 0x040091EC RID: 37356
			public int baseIndex;

			// Token: 0x040091ED RID: 37357
			public readonly int[] correctiveIndexBySize = new int[] { -1, -1, -1, -1, -1, -1 };
		}
	}
}
