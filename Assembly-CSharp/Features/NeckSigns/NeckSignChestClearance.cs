using System;
using RayznGames.DressRigger;
using UnityEngine;

namespace Features.NeckSigns
{
	// Token: 0x02000F73 RID: 3955
	[DisallowMultipleComponent]
	public sealed class NeckSignChestClearance : MonoBehaviour
	{
		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x0600770E RID: 30478 RVA: 0x003067CB File Offset: 0x003049CB
		// (set) Token: 0x0600770F RID: 30479 RVA: 0x003067D3 File Offset: 0x003049D3
		public float FitSize { get; private set; } = 3f;

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06007710 RID: 30480 RVA: 0x003067DC File Offset: 0x003049DC
		// (set) Token: 0x06007711 RID: 30481 RVA: 0x003067E4 File Offset: 0x003049E4
		public Vector2 AdditionalFitWeights { get; private set; }

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06007712 RID: 30482 RVA: 0x003067ED File Offset: 0x003049ED
		// (set) Token: 0x06007713 RID: 30483 RVA: 0x003067F5 File Offset: 0x003049F5
		public Quaternion FitRotation { get; private set; } = Quaternion.identity;

		// Token: 0x06007714 RID: 30484 RVA: 0x00306800 File Offset: 0x00304A00
		public void ConfigureBody(SkinnedMeshRenderer renderer)
		{
			this.ReleaseFit();
			this.body = renderer;
			this.configuredBody = true;
			this.owner = ((this.body != null) ? ((this.body.transform.parent != null) ? this.body.transform.parent : this.body.transform) : null);
		}

		// Token: 0x06007715 RID: 30485 RVA: 0x00306870 File Offset: 0x00304A70
		public void RefreshFit(bool force = true)
		{
			if (!this.ResolveBody(force) || this.body.sharedMesh == null)
			{
				this.FitSize = 3f;
				this.AdditionalFitWeights = Vector2.zero;
				this.FitRotation = base.transform.rotation;
				return;
			}
			if (this.indexedMesh != this.body.sharedMesh)
			{
				this.CacheShapeIndicesAndFrame();
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < NeckSignChestClearance.SizeValues.Length; i++)
			{
				float num3 = this.ReadWeight(i);
				num += num3;
				num2 += num3 * NeckSignChestClearance.SizeValues[i];
			}
			this.FitSize = Mathf.Clamp((num <= 1f) ? (num2 + 3f * (1f - num)) : (num2 / num), 0f, 5f);
			this.AdditionalFitWeights = new Vector2(this.ReadWeight(5), this.ReadWeight(6));
			this.FitRotation = ((this.chest != null) ? (this.chest.rotation * this.chestAxes) : ((this.owner != null) ? this.owner.rotation : base.transform.rotation));
		}

		// Token: 0x06007716 RID: 30486 RVA: 0x003069B4 File Offset: 0x00304BB4
		private float ReadWeight(int index)
		{
			if (this.shapeIndices[index] < 0)
			{
				return 0f;
			}
			float blendShapeWeight = this.body.GetBlendShapeWeight(this.shapeIndices[index]);
			if (!float.IsNaN(blendShapeWeight) && !float.IsInfinity(blendShapeWeight))
			{
				return Mathf.Clamp01(blendShapeWeight * 0.01f);
			}
			return 0f;
		}

		// Token: 0x06007717 RID: 30487 RVA: 0x00306A08 File Offset: 0x00304C08
		private bool ResolveBody(bool force)
		{
			if (this.body != null && this.owner != null)
			{
				return true;
			}
			if (this.configuredBody)
			{
				return false;
			}
			if (!force && this.resolvedBody && Time.frameCount < this.nextResolveFrame)
			{
				return false;
			}
			this.resolvedBody = true;
			this.nextResolveFrame = Time.frameCount + 30;
			PlayerDresser componentInParent = base.GetComponentInParent<PlayerDresser>();
			if (componentInParent == null)
			{
				return false;
			}
			this.body = componentInParent.CharacterBodyRenderer;
			this.owner = componentInParent.transform;
			return this.body != null;
		}

		// Token: 0x06007718 RID: 30488 RVA: 0x00306AA4 File Offset: 0x00304CA4
		private void CacheShapeIndicesAndFrame()
		{
			this.indexedMesh = this.body.sharedMesh;
			for (int i = 0; i < NeckSignChestClearance.ShapeNames.Length; i++)
			{
				this.shapeIndices[i] = this.indexedMesh.GetBlendShapeIndex(NeckSignChestClearance.ShapeNames[i]);
			}
			this.chest = null;
			this.chestAxes = Quaternion.identity;
			Transform[] bones = this.body.bones;
			for (int j = 0; j < bones.Length; j++)
			{
				if (!(bones[j] == null) && !(bones[j].name != "spine_02.x"))
				{
					this.chest = bones[j];
					Matrix4x4[] bindposes = this.indexedMesh.bindposes;
					this.chestAxes = ((j < bindposes.Length) ? (bindposes[j] * this.body.transform.worldToLocalMatrix * this.owner.localToWorldMatrix).rotation : (Quaternion.Inverse(this.chest.rotation) * this.owner.rotation));
					return;
				}
			}
		}

		// Token: 0x06007719 RID: 30489 RVA: 0x00306BBC File Offset: 0x00304DBC
		public void ReleaseFit()
		{
			this.indexedMesh = null;
			this.chest = null;
			this.resolvedBody = false;
			this.nextResolveFrame = 0;
			this.FitSize = 3f;
			this.AdditionalFitWeights = Vector2.zero;
			this.FitRotation = base.transform.rotation;
		}

		// Token: 0x04007A50 RID: 31312
		private static readonly string[] ShapeNames = new string[] { "Breast_Size_00_Flat", "Breast_Size_02_Smaller", "Breast_Size_01_Small", "Breast_Size_03_Natural", "Breast_Size_04_Balloon", "Upper Torso Thickness OUT", "Breast Vertical Position" };

		// Token: 0x04007A51 RID: 31313
		private static readonly float[] SizeValues = new float[] { 0f, 1f, 2f, 4f, 5f };

		// Token: 0x04007A52 RID: 31314
		private readonly int[] shapeIndices = new int[NeckSignChestClearance.ShapeNames.Length];

		// Token: 0x04007A53 RID: 31315
		private SkinnedMeshRenderer body;

		// Token: 0x04007A54 RID: 31316
		private Mesh indexedMesh;

		// Token: 0x04007A55 RID: 31317
		private Transform owner;

		// Token: 0x04007A56 RID: 31318
		private Transform chest;

		// Token: 0x04007A57 RID: 31319
		private Quaternion chestAxes = Quaternion.identity;

		// Token: 0x04007A58 RID: 31320
		private bool configuredBody;

		// Token: 0x04007A59 RID: 31321
		private bool resolvedBody;

		// Token: 0x04007A5A RID: 31322
		private int nextResolveFrame;
	}
}
