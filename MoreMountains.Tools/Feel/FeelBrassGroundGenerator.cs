using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Feel
{
	// Token: 0x02000376 RID: 886
	[AddComponentMenu("")]
	public class FeelBrassGroundGenerator : MonoBehaviour
	{
		// Token: 0x06001854 RID: 6228 RVA: 0x000673A7 File Offset: 0x000655A7
		protected virtual void Awake()
		{
			if (this.GenerateOnAwake)
			{
				this.GenerateGround();
			}
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x000673B8 File Offset: 0x000655B8
		protected virtual void GenerateGround()
		{
			int num = 0;
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.ParentContainer.transform.childCount; i++)
			{
				list.Add(this.ParentContainer.transform.GetChild(i).gameObject);
			}
			foreach (GameObject gameObject in list)
			{
				num++;
				if (Application.isPlaying)
				{
					Object.Destroy(gameObject.gameObject);
				}
				else
				{
					Object.DestroyImmediate(gameObject.gameObject);
				}
			}
			this._counter = 0;
			for (int j = 0; j < this.NumberOfRows; j++)
			{
				for (int k = 0; k < this.NumberOfColumns; k++)
				{
					this._wipPosition.x = (float)j * this.Width;
					this._wipPosition.y = 0f;
					this._wipPosition.z = (float)k * this.Depth;
					this._wipPosition += this.Offset;
					this._wipName = "GroundBlock_" + this._counter.ToString();
					this.InstantiateBlock(this._wipPosition, this._wipName);
					this._counter++;
				}
			}
			for (int l = 0; l < this.NumberOfRows; l++)
			{
				for (int m = 0; m < this.NumberOfColumns; m++)
				{
					this._wipPosition.x = (float)l * this.Width;
					this._wipPosition.y = Random.Range(this.MinHeight, this.MaxHeight);
					this._wipPosition.z = (float)m * this.Depth;
					this._wipPosition += this.Offset;
					if (MMMaths.Chance(this.FloatingBlockChance) && Vector3.Distance(this._wipPosition, this.Dancer.transform.position) > this.MinDistanceToDancer)
					{
						this._wipName = "AirBlock_" + this._counter.ToString();
						this._receiver = this.InstantiateBlock(this._wipPosition, this._wipName);
						this._receiver.transform.localScale = this._receiver.transform.localScale * Random.Range(this.MinScale, this.MaxScale);
						this._receiver.MinRandomLevelMultiplier *= 3f;
						this._receiver.MaxRandomLevelMultiplier *= 3f;
						this._receiver.gameObject.AddComponent<MMAutoRotate>().RotationSpeed = new Vector3(0f, 100f, 0f);
						this._counter++;
					}
				}
			}
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x000676C8 File Offset: 0x000658C8
		protected virtual MMRadioReceiver InstantiateBlock(Vector3 newPosition, string newName)
		{
			this._receiver = Object.Instantiate<MMRadioReceiver>(this.GroundPrefabToInstantiate, newPosition, Quaternion.identity, this.ParentContainer);
			if (this.ParentContainer == null)
			{
				SceneManager.MoveGameObjectToScene(this._receiver.gameObject, base.gameObject.scene);
			}
			this._receiver.name = newName;
			float num = Vector3.Distance(this.Dancer.transform.position + this.DancerOffset, newPosition);
			float num2 = Mathf.Max((float)this.NumberOfColumns * this.Depth, (float)this.NumberOfRows * this.Width) / 2f;
			float num3 = MMMaths.Remap(num, 0f, num2, 0f, 1f);
			float num4 = this.Amplitude.Evaluate(num3);
			float num5 = Random.Range(this.MinRandom, this.MaxRandom);
			num4 *= num5;
			num4 *= this.AmplitudeMultiplier;
			int num6 = Random.Range(0, 2);
			this._receiver.MinRandomLevelMultiplier = num4;
			this._receiver.MaxRandomLevelMultiplier = num4;
			this._receiver.GenerateRandomLevelMultiplier();
			this._receiver.Channel = num6;
			float num7 = Random.Range(0f, 100f);
			if (num7 < 80f)
			{
				this._receiver.GetComponent<MeshRenderer>().material = this.GroundMaterial;
			}
			else if (num7 < 90f)
			{
				this._receiver.GetComponent<MeshRenderer>().material = this.GroundMaterialAlt1;
			}
			else
			{
				this._receiver.GetComponent<MeshRenderer>().material = this.GroundMaterialAlt2;
			}
			this._receiver.transform.position = newPosition;
			return this._receiver;
		}

		// Token: 0x04001C37 RID: 7223
		[Header("Dimensions")]
		public int NumberOfRows = 10;

		// Token: 0x04001C38 RID: 7224
		public int NumberOfColumns = 10;

		// Token: 0x04001C39 RID: 7225
		public Vector3 Offset;

		// Token: 0x04001C3A RID: 7226
		public Vector3 DancerOffset;

		// Token: 0x04001C3B RID: 7227
		public AnimationCurve Amplitude;

		// Token: 0x04001C3C RID: 7228
		public float Width = 0.5f;

		// Token: 0x04001C3D RID: 7229
		public float Depth = 0.5f;

		// Token: 0x04001C3E RID: 7230
		public float MinRandom = 1f;

		// Token: 0x04001C3F RID: 7231
		public float MaxRandom = 2f;

		// Token: 0x04001C40 RID: 7232
		public float AmplitudeMultiplier = 2f;

		// Token: 0x04001C41 RID: 7233
		public int FloatingCubesAmount = 20;

		// Token: 0x04001C42 RID: 7234
		[Header("Air Cubes")]
		public int FloatingBlockChance = 3;

		// Token: 0x04001C43 RID: 7235
		public float MinHeight = 1f;

		// Token: 0x04001C44 RID: 7236
		public float MaxHeight = 5f;

		// Token: 0x04001C45 RID: 7237
		public float MinDistanceToDancer = 2f;

		// Token: 0x04001C46 RID: 7238
		public float MinScale = 0.5f;

		// Token: 0x04001C47 RID: 7239
		public float MaxScale = 2f;

		// Token: 0x04001C48 RID: 7240
		[Header("Materials")]
		public Material GroundMaterial;

		// Token: 0x04001C49 RID: 7241
		public Material GroundMaterialAlt1;

		// Token: 0x04001C4A RID: 7242
		public Material GroundMaterialAlt2;

		// Token: 0x04001C4B RID: 7243
		[Header("Bindings")]
		public MMRadioReceiver GroundPrefabToInstantiate;

		// Token: 0x04001C4C RID: 7244
		public Transform ParentContainer;

		// Token: 0x04001C4D RID: 7245
		public Transform Dancer;

		// Token: 0x04001C4E RID: 7246
		[Header("Behaviour")]
		public bool GenerateOnAwake;

		// Token: 0x04001C4F RID: 7247
		[Header("Debug")]
		[MMInspectorButton("GenerateGround")]
		public bool GenerateGroundBtn;

		// Token: 0x04001C50 RID: 7248
		protected MMRadioReceiver _receiver;

		// Token: 0x04001C51 RID: 7249
		protected Vector3 _wipPosition;

		// Token: 0x04001C52 RID: 7250
		protected string _wipName;

		// Token: 0x04001C53 RID: 7251
		protected int _counter;
	}
}
