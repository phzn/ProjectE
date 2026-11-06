using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x0200005D RID: 93
	[RequireComponent(typeof(Collider))]
	public class MMRandomBoundsInstantiator : MonoBehaviour
	{
		// Token: 0x0600020D RID: 525 RVA: 0x00009D28 File Offset: 0x00007F28
		protected virtual void Awake()
		{
			this._collider = base.gameObject.GetComponent<Collider>();
			if (this.StartMode == MMRandomBoundsInstantiator.StartModes.Awake)
			{
				this.Instantiate();
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00009D49 File Offset: 0x00007F49
		protected virtual void Start()
		{
			if (this.StartMode == MMRandomBoundsInstantiator.StartModes.Start)
			{
				this.Instantiate();
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00009D5C File Offset: 0x00007F5C
		protected virtual void Instantiate()
		{
			if (this._instantiatedGameObjects == null)
			{
				this._instantiatedGameObjects = new List<GameObject>();
			}
			if (this.DestroyPreviouslyInstantiatedObjects)
			{
				foreach (GameObject gameObject in this._instantiatedGameObjects)
				{
					Object.DestroyImmediate(gameObject);
				}
				this._instantiatedGameObjects.Clear();
			}
			int num = Random.Range(this.Quantity.x, this.Quantity.y);
			for (int i = 0; i < num; i++)
			{
				this.InstantiateRandomObject();
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009E00 File Offset: 0x00008000
		public virtual void InstantiateRandomObject()
		{
			if (this.RandomPool.Count == 0)
			{
				return;
			}
			int num = Random.Range(0, this.RandomPool.Count);
			GameObject gameObject = Object.Instantiate<GameObject>(this.RandomPool[num], base.transform.position, base.transform.rotation);
			SceneManager.MoveGameObjectToScene(gameObject.gameObject, base.gameObject.scene);
			gameObject.transform.position = MMBoundsExtensions.MMRandomPointInBounds(this._collider.bounds);
			gameObject.transform.position = this._collider.ClosestPoint(gameObject.transform.position);
			gameObject.name = this.InstantiatedObjectName;
			if (this.ParentInstantiatedToThisObject)
			{
				gameObject.transform.SetParent(base.transform);
			}
			MMRandomBoundsInstantiator.ScaleModes scaleMode = this.ScaleMode;
			if (scaleMode != MMRandomBoundsInstantiator.ScaleModes.Uniform)
			{
				if (scaleMode == MMRandomBoundsInstantiator.ScaleModes.Vector3)
				{
					this._newScale = MMMaths.RandomVector3(this.MinVectorScale, this.MaxVectorScale);
					gameObject.transform.localScale = this._newScale;
				}
			}
			else
			{
				float num2 = Random.Range(this.MinScale, this.MaxScale);
				gameObject.transform.localScale = Vector3.one * num2;
			}
			this._instantiatedGameObjects.Add(gameObject);
		}

		// Token: 0x040002C7 RID: 711
		[Header("Random instantiation")]
		public MMRandomBoundsInstantiator.StartModes StartMode;

		// Token: 0x040002C8 RID: 712
		public string InstantiatedObjectName = "RandomInstantiated";

		// Token: 0x040002C9 RID: 713
		public bool ParentInstantiatedToThisObject = true;

		// Token: 0x040002CA RID: 714
		public bool DestroyPreviouslyInstantiatedObjects = true;

		// Token: 0x040002CB RID: 715
		[Header("Spawn")]
		public List<GameObject> RandomPool;

		// Token: 0x040002CC RID: 716
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2Int Quantity = new Vector2Int(1, 1);

		// Token: 0x040002CD RID: 717
		[Header("Scale")]
		public MMRandomBoundsInstantiator.ScaleModes ScaleMode;

		// Token: 0x040002CE RID: 718
		[MMEnumCondition("ScaleMode", new int[] { 0 })]
		public float MinScale = 1f;

		// Token: 0x040002CF RID: 719
		[MMEnumCondition("ScaleMode", new int[] { 0 })]
		public float MaxScale = 1f;

		// Token: 0x040002D0 RID: 720
		[MMEnumCondition("ScaleMode", new int[] { 1 })]
		public Vector3 MinVectorScale = Vector3.one;

		// Token: 0x040002D1 RID: 721
		[MMEnumCondition("ScaleMode", new int[] { 1 })]
		public Vector3 MaxVectorScale = Vector3.one;

		// Token: 0x040002D2 RID: 722
		[Header("Test")]
		[MMInspectorButton("Instantiate")]
		public bool InstantiateButton;

		// Token: 0x040002D3 RID: 723
		protected Collider _collider;

		// Token: 0x040002D4 RID: 724
		protected List<GameObject> _instantiatedGameObjects;

		// Token: 0x040002D5 RID: 725
		protected Vector3 _newScale = Vector3.zero;

		// Token: 0x020003CB RID: 971
		public enum StartModes
		{
			// Token: 0x04001DEF RID: 7663
			Awake,
			// Token: 0x04001DF0 RID: 7664
			Start,
			// Token: 0x04001DF1 RID: 7665
			None
		}

		// Token: 0x020003CC RID: 972
		public enum ScaleModes
		{
			// Token: 0x04001DF3 RID: 7667
			Uniform,
			// Token: 0x04001DF4 RID: 7668
			Vector3
		}
	}
}
