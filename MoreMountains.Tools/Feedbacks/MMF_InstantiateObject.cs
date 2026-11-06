using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F1 RID: 497
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to instantiate the object specified in its inspector, at the feedback's position (plus an optional offset). You can also optionally (and automatically) create an object pool at initialization to save on performance. In that case you'll need to specify a pool size (usually the maximum amount of these instantiated objects you plan on having in your scene at each given time).")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("GameObject/Instantiate Object")]
	public class MMF_InstantiateObject : MMF_Feedback
	{
		// Token: 0x06000CE6 RID: 3302 RVA: 0x00037BB8 File Offset: 0x00035DB8
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.CreateObjectPool && !this._poolCreatedOrFound)
			{
				if (this._objectPooler != null)
				{
					this._objectPooler.DestroyObjectPool();
					owner.ProxyDestroy(this._objectPooler.gameObject);
				}
				GameObject gameObject = new GameObject();
				gameObject.name = this.Owner.name + "_ObjectPooler";
				this._objectPooler = gameObject.AddComponent<MMMiniObjectPooler>();
				this._objectPooler.GameObjectToPool = this.GameObjectToInstantiate;
				this._objectPooler.PoolSize = this.ObjectPoolSize;
				if (this.PoolParentTransform != null)
				{
					this._objectPooler.transform.SetParent(this.PoolParentTransform);
				}
				this._objectPooler.MutualizeWaitingPools = this.MutualizePools;
				this._objectPooler.FillObjectPool();
				if (this.Owner != null && gameObject.transform.parent == null)
				{
					SceneManager.MoveGameObjectToScene(gameObject, this.Owner.gameObject.scene);
				}
				this._poolCreatedOrFound = true;
			}
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00037CE4 File Offset: 0x00035EE4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_InstantiateObject.FeedbackTypeAuthorized || this.GameObjectToInstantiate == null)
			{
				return;
			}
			if (this._objectPooler != null)
			{
				this._newGameObject = this._objectPooler.GetPooledGameObject();
				if (this._newGameObject != null)
				{
					this.PositionObject(position);
					this._newGameObject.SetActive(true);
					return;
				}
			}
			else
			{
				this._newGameObject = Object.Instantiate<GameObject>(this.GameObjectToInstantiate);
				if (this._newGameObject != null)
				{
					SceneManager.MoveGameObjectToScene(this._newGameObject, this.Owner.gameObject.scene);
					this.PositionObject(position);
				}
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00037D94 File Offset: 0x00035F94
		protected virtual void PositionObject(Vector3 position)
		{
			this._newGameObject.transform.position = this.GetPosition(position);
			if (this.AlsoApplyRotation)
			{
				this._newGameObject.transform.rotation = this.GetRotation();
			}
			if (this.AlsoApplyScale)
			{
				this._newGameObject.transform.localScale = this.GetScale();
			}
			if (this.ParentTransform != null)
			{
				this._newGameObject.transform.SetParent(this.ParentTransform);
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00037E18 File Offset: 0x00036018
		protected virtual Vector3 GetPosition(Vector3 position)
		{
			if (this.RandomizePosition)
			{
				this._randomizedPosition.x = Random.Range(this.RandomizedPositionMin.x, this.RandomizedPositionMax.x);
				this._randomizedPosition.y = Random.Range(this.RandomizedPositionMin.y, this.RandomizedPositionMax.y);
				this._randomizedPosition.z = Random.Range(this.RandomizedPositionMin.z, this.RandomizedPositionMax.z);
			}
			switch (this.PositionMode)
			{
			case MMF_InstantiateObject.PositionModes.FeedbackPosition:
				return this.Owner.transform.position + this.PositionOffset + this._randomizedPosition;
			case MMF_InstantiateObject.PositionModes.Transform:
				return this.TargetTransform.position + this.PositionOffset + this._randomizedPosition;
			case MMF_InstantiateObject.PositionModes.WorldPosition:
				return this.TargetPosition + this.PositionOffset + this._randomizedPosition;
			case MMF_InstantiateObject.PositionModes.Script:
				return position + this.PositionOffset + this._randomizedPosition;
			default:
				return position + this.PositionOffset + this._randomizedPosition;
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00037F54 File Offset: 0x00036154
		protected virtual Quaternion GetRotation()
		{
			switch (this.PositionMode)
			{
			case MMF_InstantiateObject.PositionModes.FeedbackPosition:
				return this.Owner.transform.rotation;
			case MMF_InstantiateObject.PositionModes.Transform:
				return this.TargetTransform.rotation;
			case MMF_InstantiateObject.PositionModes.WorldPosition:
				return Quaternion.identity;
			case MMF_InstantiateObject.PositionModes.Script:
				return this.Owner.transform.rotation;
			default:
				return this.Owner.transform.rotation;
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00037FC4 File Offset: 0x000361C4
		protected virtual Vector3 GetScale()
		{
			switch (this.PositionMode)
			{
			case MMF_InstantiateObject.PositionModes.FeedbackPosition:
				return this.Owner.transform.localScale;
			case MMF_InstantiateObject.PositionModes.Transform:
				return this.TargetTransform.localScale;
			case MMF_InstantiateObject.PositionModes.WorldPosition:
				return this.Owner.transform.localScale;
			case MMF_InstantiateObject.PositionModes.Script:
				return this.Owner.transform.localScale;
			default:
				return this.Owner.transform.localScale;
			}
		}

		// Token: 0x04000EA0 RID: 3744
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000EA1 RID: 3745
		[MMFInspectorGroup("Instantiate Object", true, 37, true, false)]
		[Tooltip("the object to instantiate")]
		[FormerlySerializedAs("VfxToInstantiate")]
		public GameObject GameObjectToInstantiate;

		// Token: 0x04000EA2 RID: 3746
		[MMFInspectorGroup("Position", true, 39, false, false)]
		[Tooltip("the chosen way to position the object")]
		public MMF_InstantiateObject.PositionModes PositionMode;

		// Token: 0x04000EA3 RID: 3747
		[Tooltip("the chosen way to position the object")]
		public bool AlsoApplyRotation;

		// Token: 0x04000EA4 RID: 3748
		[Tooltip("the chosen way to position the object")]
		public bool AlsoApplyScale;

		// Token: 0x04000EA5 RID: 3749
		[Tooltip("the transform at which to instantiate the object")]
		[MMFEnumCondition("PositionMode", new int[] { 1 })]
		public Transform TargetTransform;

		// Token: 0x04000EA6 RID: 3750
		[Tooltip("the transform at which to instantiate the object")]
		[MMFEnumCondition("PositionMode", new int[] { 2 })]
		public Vector3 TargetPosition;

		// Token: 0x04000EA7 RID: 3751
		[Tooltip("the position offset at which to instantiate the object")]
		[FormerlySerializedAs("VfxPositionOffset")]
		public Vector3 PositionOffset;

		// Token: 0x04000EA8 RID: 3752
		[Tooltip("if this is true, instantiation position will be randomized between RandomizeMin and RandomizeMax")]
		public bool RandomizePosition;

		// Token: 0x04000EA9 RID: 3753
		[Tooltip("the minimum value we'll randomize our position with")]
		[MMFCondition("RandomizePosition", true)]
		public Vector3 RandomizedPositionMin = Vector3.zero;

		// Token: 0x04000EAA RID: 3754
		[Tooltip("the maximum value we'll randomize our position with")]
		[MMFCondition("RandomizePosition", true)]
		public Vector3 RandomizedPositionMax = Vector3.one;

		// Token: 0x04000EAB RID: 3755
		[MMFInspectorGroup("Parent", true, 47, false, false)]
		[Tooltip("if specified, the instantiated object will be parented to this transform ")]
		public Transform ParentTransform;

		// Token: 0x04000EAC RID: 3756
		[MMFInspectorGroup("Object Pool", true, 40, false, false)]
		[Tooltip("whether or not we should create automatically an object pool for this object")]
		[FormerlySerializedAs("VfxCreateObjectPool")]
		public bool CreateObjectPool;

		// Token: 0x04000EAD RID: 3757
		[Tooltip("the initial and planned size of this object pool")]
		[MMFCondition("CreateObjectPool", true)]
		[FormerlySerializedAs("VfxObjectPoolSize")]
		public int ObjectPoolSize = 5;

		// Token: 0x04000EAE RID: 3758
		[Tooltip("whether or not to create a new pool even if one already exists for that same prefab")]
		[MMFCondition("CreateObjectPool", true)]
		public bool MutualizePools;

		// Token: 0x04000EAF RID: 3759
		[Tooltip("the transform the pool of objects will be parented to")]
		[MMFCondition("CreateObjectPool", true)]
		public Transform PoolParentTransform;

		// Token: 0x04000EB0 RID: 3760
		protected MMMiniObjectPooler _objectPooler;

		// Token: 0x04000EB1 RID: 3761
		protected GameObject _newGameObject;

		// Token: 0x04000EB2 RID: 3762
		protected bool _poolCreatedOrFound;

		// Token: 0x04000EB3 RID: 3763
		protected Vector3 _randomizedPosition = Vector3.zero;

		// Token: 0x020004C1 RID: 1217
		public enum PositionModes
		{
			// Token: 0x04002220 RID: 8736
			FeedbackPosition,
			// Token: 0x04002221 RID: 8737
			Transform,
			// Token: 0x04002222 RID: 8738
			WorldPosition,
			// Token: 0x04002223 RID: 8739
			Script
		}
	}
}
