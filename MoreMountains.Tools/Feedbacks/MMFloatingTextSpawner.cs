using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200021B RID: 539
	public class MMFloatingTextSpawner : MMMonoBehaviour
	{
		// Token: 0x06000ED6 RID: 3798 RVA: 0x00042020 File Offset: 0x00040220
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00042028 File Offset: 0x00040228
		protected virtual void Start()
		{
			this.GrabMainCamera();
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00042030 File Offset: 0x00040230
		protected virtual void Initialization()
		{
			this.InstantiateObjectPool();
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00042038 File Offset: 0x00040238
		protected virtual void InstantiateObjectPool()
		{
			if (this._pooler == null)
			{
				if (this.PoolerMode == MMFloatingTextSpawner.PoolerModes.Simple)
				{
					this.InstantiateSimplePool();
					return;
				}
				this.InstantiateMultiplePool();
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00042060 File Offset: 0x00040260
		protected virtual void InstantiateSimplePool()
		{
			if (this.PooledSimpleMMFloatingText == null)
			{
				Debug.LogError(base.name + " : no PooledSimpleMMFloatingText prefab has been set.");
				return;
			}
			GameObject gameObject = new GameObject();
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			gameObject.name = this.PooledSimpleMMFloatingText.name + "_Pooler";
			gameObject.transform.SetParent(base.transform);
			MMSimpleObjectPooler mmsimpleObjectPooler = gameObject.AddComponent<MMSimpleObjectPooler>();
			mmsimpleObjectPooler.PoolSize = this.PoolSize;
			mmsimpleObjectPooler.GameObjectToPool = this.PooledSimpleMMFloatingText.gameObject;
			mmsimpleObjectPooler.NestWaitingPool = this.NestWaitingPool;
			mmsimpleObjectPooler.MutualizeWaitingPools = this.MutualizeWaitingPools;
			mmsimpleObjectPooler.PoolCanExpand = this.PoolCanExpand;
			mmsimpleObjectPooler.FillObjectPool();
			this._pooler = mmsimpleObjectPooler;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00042128 File Offset: 0x00040328
		protected virtual void InstantiateMultiplePool()
		{
			GameObject gameObject = new GameObject();
			SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			gameObject.name = base.name + "_Pooler";
			gameObject.transform.SetParent(base.transform);
			MMMultipleObjectPooler mmmultipleObjectPooler = gameObject.AddComponent<MMMultipleObjectPooler>();
			mmmultipleObjectPooler.Pool = new List<MMMultipleObjectPoolerObject>();
			foreach (MMFloatingText mmfloatingText in this.PooledMultipleMMFloatingText)
			{
				MMMultipleObjectPoolerObject mmmultipleObjectPoolerObject = new MMMultipleObjectPoolerObject();
				mmmultipleObjectPoolerObject.GameObjectToPool = mmfloatingText.gameObject;
				mmmultipleObjectPoolerObject.PoolCanExpand = this.PoolCanExpand;
				mmmultipleObjectPoolerObject.PoolSize = this.PoolSize;
				mmmultipleObjectPoolerObject.Enabled = true;
				mmmultipleObjectPooler.Pool.Add(mmmultipleObjectPoolerObject);
			}
			mmmultipleObjectPooler.NestWaitingPool = this.NestWaitingPool;
			mmmultipleObjectPooler.MutualizeWaitingPools = this.MutualizeWaitingPools;
			mmmultipleObjectPooler.FillObjectPool();
			this._pooler = mmmultipleObjectPooler;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00042224 File Offset: 0x00040424
		protected virtual void GrabMainCamera()
		{
			if (this.AutoGrabMainCameraOnStart)
			{
				this.TargetCamera = Camera.main;
			}
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0004223C File Offset: 0x0004043C
		protected virtual void Spawn(string value, Vector3 position, Vector3 direction, float intensity = 1f, bool forceLifetime = false, float lifetime = 1f, bool forceColor = false, Gradient animateColorGradient = null)
		{
			if (!this.CanSpawn)
			{
				return;
			}
			this._direction = ((direction != Vector3.zero) ? (direction + base.transform.up) : base.transform.up);
			base.transform.position = position;
			GameObject pooledGameObject = this._pooler.GetPooledGameObject();
			float num = (this.IntensityImpactsLifetime ? (intensity * this.IntensityLifetimeMultiplier) : 1f);
			float num2 = (this.IntensityImpactsMovement ? (intensity * this.IntensityMovementMultiplier) : 1f);
			float num3 = (this.IntensityImpactsScale ? (intensity * this.IntensityScaleMultiplier) : 1f);
			this._lifetime = Random.Range(this.Lifetime.x, this.Lifetime.y) * num;
			this._spawnOffset = MMMaths.RandomVector3(this.SpawnOffsetMin, this.SpawnOffsetMax);
			this._animateColor = this.AnimateColor;
			this._colorGradient = this.AnimateColorGradient;
			float num4 = Random.Range(this.RemapXZero.x, this.RemapXZero.y);
			float num5 = Random.Range(this.RemapXOne.x, this.RemapXOne.y) * num2;
			float num6 = Random.Range(this.RemapYZero.x, this.RemapYZero.y);
			float num7 = Random.Range(this.RemapYOne.x, this.RemapYOne.y) * num2;
			float num8 = Random.Range(this.RemapZZero.x, this.RemapZZero.y);
			float num9 = Random.Range(this.RemapZOne.x, this.RemapZOne.y) * num2;
			float num10 = Random.Range(this.RemapOpacityZero.x, this.RemapOpacityZero.y);
			float num11 = Random.Range(this.RemapOpacityOne.x, this.RemapOpacityOne.y);
			float num12 = Random.Range(this.RemapScaleZero.x, this.RemapOpacityZero.y);
			float num13 = Random.Range(this.RemapScaleOne.x, this.RemapScaleOne.y) * num3;
			if (forceLifetime)
			{
				this._lifetime = lifetime;
			}
			if (forceColor)
			{
				this._animateColor = true;
				this._colorGradient = animateColorGradient;
			}
			if (pooledGameObject == null)
			{
				return;
			}
			pooledGameObject.gameObject.SetActive(true);
			pooledGameObject.gameObject.MMGetComponentNoAlloc<MMPoolableObject>().TriggerOnSpawnComplete();
			pooledGameObject.transform.position = base.transform.position + this._spawnOffset;
			this._floatingText = pooledGameObject.MMGetComponentNoAlloc<MMFloatingText>();
			this._floatingText.SetUseUnscaledTime(this.UseUnscaledTime, true);
			this._floatingText.ResetPosition();
			this._floatingText.SetProperties(value, this._lifetime, this._direction, this.AnimateMovement, this.AlignmentMode, this.FixedAlignment, this.AlwaysFaceCamera, this.TargetCamera, this.AnimateX, this.AnimateXCurve, num4, num5, this.AnimateY, this.AnimateYCurve, num6, num7, this.AnimateZ, this.AnimateZCurve, num8, num9, this.AnimateOpacity, this.AnimateOpacityCurve, num10, num11, this.AnimateScale, this.AnimateScaleCurve, num12, num13, this._animateColor, this._colorGradient);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00042584 File Offset: 0x00040784
		public virtual void OnMMFloatingTextSpawnEvent(MMChannelData channelData, Vector3 spawnPosition, string value, Vector3 direction, float intensity, bool forceLifetime = false, float lifetime = 1f, bool forceColor = false, Gradient animateColorGradient = null, bool useUnscaledTime = false)
		{
			if (!MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition))
			{
				return;
			}
			this.UseUnscaledTime = useUnscaledTime;
			this.Spawn(value, spawnPosition, direction, intensity, forceLifetime, lifetime, forceColor, animateColorGradient);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x000425C8 File Offset: 0x000407C8
		protected virtual void OnEnable()
		{
			MMFloatingTextSpawnEvent.Register(new MMFloatingTextSpawnEvent.Delegate(this.OnMMFloatingTextSpawnEvent));
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000425DC File Offset: 0x000407DC
		protected virtual void OnDisable()
		{
			MMFloatingTextSpawnEvent.Unregister(new MMFloatingTextSpawnEvent.Delegate(this.OnMMFloatingTextSpawnEvent));
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x000425F0 File Offset: 0x000407F0
		protected virtual void TestSpawnOne()
		{
			string text = Random.Range(this.DebugRandomValue.x, this.DebugRandomValue.y).ToString();
			this.Spawn(text, base.transform.position, Vector3.zero, 1f, false, 1f, false, null);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00042645 File Offset: 0x00040845
		protected virtual void TestSpawnMany()
		{
			if (this._testSpawnCoroutine == null)
			{
				this._testSpawnCoroutine = base.StartCoroutine(this.TestSpawnManyCo());
				return;
			}
			base.StopCoroutine(this._testSpawnCoroutine);
			this._testSpawnCoroutine = null;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00042675 File Offset: 0x00040875
		protected virtual IEnumerator TestSpawnManyCo()
		{
			float lastSpawnAt = Time.time;
			float interval = Random.Range(this.DebugInterval.x, this.DebugInterval.y);
			for (;;)
			{
				if (Time.time - lastSpawnAt > interval)
				{
					this.TestSpawnOne();
					lastSpawnAt = Time.time;
					interval = Random.Range(this.DebugInterval.x, this.DebugInterval.y);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400113F RID: 4415
		[MMInspectorGroup("General Settings", true, 10, false)]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001140 RID: 4416
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04001141 RID: 4417
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04001142 RID: 4418
		[Tooltip("whether or not this spawner can spawn at this time")]
		public bool CanSpawn = true;

		// Token: 0x04001143 RID: 4419
		[Tooltip("whether or not this spawner should spawn objects on unscaled time")]
		public bool UseUnscaledTime;

		// Token: 0x04001144 RID: 4420
		[MMInspectorGroup("Pooler", true, 24, false)]
		[Tooltip("the selected pooler mode (single prefab or multiple ones)")]
		public MMFloatingTextSpawner.PoolerModes PoolerMode;

		// Token: 0x04001145 RID: 4421
		[Tooltip("the prefab to spawn (ignored if in multiple mode)")]
		public MMFloatingText PooledSimpleMMFloatingText;

		// Token: 0x04001146 RID: 4422
		[Tooltip("the prefabs to spawn (ignored if in simple mode)")]
		public List<MMFloatingText> PooledMultipleMMFloatingText;

		// Token: 0x04001147 RID: 4423
		[Tooltip("the amount of objects to pool to avoid having to instantiate them at runtime. Should be bigger than the max amount of texts you plan on having on screen at any given moment")]
		public int PoolSize = 20;

		// Token: 0x04001148 RID: 4424
		[Tooltip("whether or not to nest the waiting pools")]
		public bool NestWaitingPool = true;

		// Token: 0x04001149 RID: 4425
		[Tooltip("whether or not to mutualize the waiting pools")]
		public bool MutualizeWaitingPools = true;

		// Token: 0x0400114A RID: 4426
		[Tooltip("whether or not the text pool can expand if the pool is empty")]
		public bool PoolCanExpand = true;

		// Token: 0x0400114B RID: 4427
		[MMInspectorGroup("Spawn Settings", true, 14, false)]
		[Tooltip("the random min and max lifetime duration for the spawned texts (in seconds)")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Lifetime = Vector2.one;

		// Token: 0x0400114C RID: 4428
		[Header("Spawn Position Offset")]
		[Tooltip("the random min position at which to spawn the text, relative to its intended spawn position")]
		public Vector3 SpawnOffsetMin = Vector3.zero;

		// Token: 0x0400114D RID: 4429
		[Tooltip("the random max position at which to spawn the text, relative to its intended spawn position")]
		public Vector3 SpawnOffsetMax = Vector3.zero;

		// Token: 0x0400114E RID: 4430
		[MMInspectorGroup("Animate Position", true, 15, false)]
		[Header("Movement")]
		[Tooltip("whether or not to animate the movement of spawned texts")]
		public bool AnimateMovement = true;

		// Token: 0x0400114F RID: 4431
		[Tooltip("whether or not to animate the X movement of spawned texts")]
		public bool AnimateX;

		// Token: 0x04001150 RID: 4432
		[Tooltip("the value to which the x movement curve's zero should be remapped to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMCondition("AnimateX", true)]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 RemapXZero = Vector2.zero;

		// Token: 0x04001151 RID: 4433
		[Tooltip("the value to which the x movement curve's one should be remapped to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMCondition("AnimateX", true)]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 RemapXOne = Vector2.one;

		// Token: 0x04001152 RID: 4434
		[Tooltip("the curve on which to animate the x movement")]
		[MMCondition("AnimateX", true)]
		public AnimationCurve AnimateXCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04001153 RID: 4435
		[Tooltip("whether or not to animate the Y movement of spawned texts")]
		public bool AnimateY = true;

		// Token: 0x04001154 RID: 4436
		[Tooltip("the value to which the y movement curve's zero should be remapped to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMCondition("AnimateY", true)]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 RemapYZero = Vector2.zero;

		// Token: 0x04001155 RID: 4437
		[Tooltip("the value to which the y movement curve's one should be remapped to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMCondition("AnimateY", true)]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 RemapYOne = new Vector2(5f, 5f);

		// Token: 0x04001156 RID: 4438
		[Tooltip("the curve on which to animate the y movement")]
		[MMCondition("AnimateY", true)]
		public AnimationCurve AnimateYCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04001157 RID: 4439
		[Tooltip("whether or not to animate the Z movement of spawned texts")]
		public bool AnimateZ;

		// Token: 0x04001158 RID: 4440
		[Tooltip("the value to which the z movement curve's zero should be remapped to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMCondition("AnimateZ", true)]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZZero = Vector2.zero;

		// Token: 0x04001159 RID: 4441
		[Tooltip("the value to which the z movement curve's one should be remapped to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMCondition("AnimateZ", true)]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZOne = Vector2.one;

		// Token: 0x0400115A RID: 4442
		[Tooltip("the curve on which to animate the z movement")]
		[MMCondition("AnimateZ", true)]
		public AnimationCurve AnimateZCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400115B RID: 4443
		[MMInspectorGroup("Facing Directions", true, 16, false)]
		[Header("Alignment")]
		[Tooltip("the selected alignment mode (whether the spawned text should have a fixed alignment, orient to match the initial spawn direction, or its movement curve)")]
		public MMFloatingTextSpawner.AlignmentModes AlignmentMode;

		// Token: 0x0400115C RID: 4444
		[Tooltip("when in fixed mode, the direction in which to keep the spawned texts")]
		[MMEnumCondition("AlignmentMode", new int[] { 0 })]
		public Vector3 FixedAlignment = Vector3.up;

		// Token: 0x0400115D RID: 4445
		[Header("Billboard")]
		[Tooltip("whether or not spawned texts should always face the camera")]
		public bool AlwaysFaceCamera;

		// Token: 0x0400115E RID: 4446
		[Tooltip("whether or not this spawner should automatically grab the main camera on start")]
		[MMCondition("AlwaysFaceCamera", true)]
		public bool AutoGrabMainCameraOnStart = true;

		// Token: 0x0400115F RID: 4447
		[Tooltip("if not in auto grab mode, the camera to use for billboards")]
		[MMCondition("AlwaysFaceCamera", true)]
		public Camera TargetCamera;

		// Token: 0x04001160 RID: 4448
		[MMInspectorGroup("Animate Scale", true, 46, false)]
		[Tooltip("whether or not to animate the scale of spawned texts")]
		public bool AnimateScale = true;

		// Token: 0x04001161 RID: 4449
		[Tooltip("the value to which the scale curve's zero should be remapped to")]
		[MMCondition("AnimateScale", true)]
		public Vector2 RemapScaleZero = Vector2.zero;

		// Token: 0x04001162 RID: 4450
		[Tooltip("the value to which the scale curve's one should be remapped to")]
		[MMCondition("AnimateScale", true)]
		public Vector2 RemapScaleOne = Vector2.one;

		// Token: 0x04001163 RID: 4451
		[Tooltip("the curve on which to animate the scale")]
		[MMCondition("AnimateScale", true)]
		public AnimationCurve AnimateScaleCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.15f, 1f),
			new Keyframe(0.85f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001164 RID: 4452
		[MMInspectorGroup("Animate Color", true, 55, false)]
		[Tooltip("whether or not to animate the spawned text's color over time")]
		public bool AnimateColor;

		// Token: 0x04001165 RID: 4453
		[Tooltip("the gradient over which to animate the spawned text's color over time")]
		[GradientUsage(true)]
		public Gradient AnimateColorGradient = new Gradient();

		// Token: 0x04001166 RID: 4454
		[MMInspectorGroup("Animate Opacity", true, 45, false)]
		[Tooltip("whether or not to animate the opacity of the spawned texts")]
		public bool AnimateOpacity = true;

		// Token: 0x04001167 RID: 4455
		[Tooltip("the value to which the opacity curve's zero should be remapped to")]
		[MMCondition("AnimateOpacity", true)]
		public Vector2 RemapOpacityZero = Vector2.zero;

		// Token: 0x04001168 RID: 4456
		[Tooltip("the value to which the opacity curve's one should be remapped to")]
		[MMCondition("AnimateOpacity", true)]
		public Vector2 RemapOpacityOne = Vector2.one;

		// Token: 0x04001169 RID: 4457
		[Tooltip("the curve on which to animate the opacity")]
		[MMCondition("AnimateOpacity", true)]
		public AnimationCurve AnimateOpacityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.2f, 1f),
			new Keyframe(0.8f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400116A RID: 4458
		[MMInspectorGroup("Intensity Multipliers", true, 45, false)]
		[Tooltip("whether or not the intensity multiplier should impact lifetime")]
		public bool IntensityImpactsLifetime;

		// Token: 0x0400116B RID: 4459
		[Tooltip("when getting an intensity multiplier, the value by which to multiply the lifetime")]
		[MMCondition("IntensityImpactsLifetime", true)]
		public float IntensityLifetimeMultiplier = 1f;

		// Token: 0x0400116C RID: 4460
		[Tooltip("whether or not the intensity multiplier should impact movement")]
		public bool IntensityImpactsMovement;

		// Token: 0x0400116D RID: 4461
		[Tooltip("when getting an intensity multiplier, the value by which to multiply the movement values")]
		[MMCondition("IntensityImpactsMovement", true)]
		public float IntensityMovementMultiplier = 1f;

		// Token: 0x0400116E RID: 4462
		[Tooltip("whether or not the intensity multiplier should impact scale")]
		public bool IntensityImpactsScale;

		// Token: 0x0400116F RID: 4463
		[Tooltip("when getting an intensity multiplier, the value by which to multiply the scale values")]
		[MMCondition("IntensityImpactsScale", true)]
		public float IntensityScaleMultiplier = 1f;

		// Token: 0x04001170 RID: 4464
		[MMInspectorGroup("Debug", true, 12, false)]
		[Tooltip("a random value to display when pressing the TestSpawnOne button")]
		public Vector2Int DebugRandomValue = new Vector2Int(100, 500);

		// Token: 0x04001171 RID: 4465
		[Tooltip("the min and max bounds within which to pick a value to output when pressing the TestSpawnMany button")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 DebugInterval = new Vector2(0.3f, 0.5f);

		// Token: 0x04001172 RID: 4466
		[Tooltip("a button used to test the spawn of one text")]
		[MMInspectorButton("TestSpawnOne")]
		public bool TestSpawnOneBtn;

		// Token: 0x04001173 RID: 4467
		[Tooltip("a button used to start/stop the spawn of texts at regular intervals")]
		[MMInspectorButton("TestSpawnMany")]
		public bool TestSpawnManyBtn;

		// Token: 0x04001174 RID: 4468
		protected MMObjectPooler _pooler;

		// Token: 0x04001175 RID: 4469
		protected MMFloatingText _floatingText;

		// Token: 0x04001176 RID: 4470
		protected Coroutine _testSpawnCoroutine;

		// Token: 0x04001177 RID: 4471
		protected float _lifetime;

		// Token: 0x04001178 RID: 4472
		protected float _speed;

		// Token: 0x04001179 RID: 4473
		protected Vector3 _spawnOffset;

		// Token: 0x0400117A RID: 4474
		protected Vector3 _direction;

		// Token: 0x0400117B RID: 4475
		protected Gradient _colorGradient;

		// Token: 0x0400117C RID: 4476
		protected bool _animateColor;

		// Token: 0x02000507 RID: 1287
		public enum PoolerModes
		{
			// Token: 0x04002384 RID: 9092
			Simple,
			// Token: 0x04002385 RID: 9093
			Multiple
		}

		// Token: 0x02000508 RID: 1288
		public enum AlignmentModes
		{
			// Token: 0x04002387 RID: 9095
			Fixed,
			// Token: 0x04002388 RID: 9096
			MatchInitialDirection,
			// Token: 0x04002389 RID: 9097
			MatchMovementDirection
		}
	}
}
