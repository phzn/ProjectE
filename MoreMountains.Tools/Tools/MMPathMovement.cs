using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000067 RID: 103
	[AddComponentMenu("More Mountains/Tools/Movement/MMPathMovement")]
	public class MMPathMovement : MonoBehaviour
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000B23C File Offset: 0x0000943C
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000B244 File Offset: 0x00009444
		public virtual Vector3 CurrentSpeed { get; protected set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000B24D File Offset: 0x0000944D
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000B255 File Offset: 0x00009455
		public virtual bool CanMove { get; set; }

		// Token: 0x06000256 RID: 598 RVA: 0x0000B25E File Offset: 0x0000945E
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000B266 File Offset: 0x00009466
		protected virtual void Start()
		{
			this._originalTransformPosition = base.transform.position;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000B279 File Offset: 0x00009479
		public virtual void ResetPath()
		{
			this.Initialization();
			this.CanMove = false;
			base.transform.position = this._originalTransformPosition;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000B29C File Offset: 0x0000949C
		protected virtual void Initialization()
		{
			this._active = true;
			this._endReached = false;
			this.CanMove = true;
			if (this.PathElements == null || this.PathElements.Count < 1)
			{
				return;
			}
			if (this.LoopInitialMovementDirection == MMPathMovement.MovementDirection.Ascending)
			{
				this._direction = 1;
			}
			else
			{
				this._direction = -1;
			}
			this._currentPoint = this.GetPathEnumerator();
			this._previousPoint = this._currentPoint.Current;
			this._currentPoint.MoveNext();
			if (!this._originalTransformPositionStatus)
			{
				this._originalTransformPositionStatus = true;
				this._originalTransformPosition = base.transform.position;
			}
			base.transform.position = this.PointPosition(this._currentPoint.Current);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000B352 File Offset: 0x00009552
		protected virtual void FixedUpdate()
		{
			if (this.UpdateMode == MMPathMovement.UpdateModes.FixedUpdate)
			{
				this.ExecuteUpdate();
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000B363 File Offset: 0x00009563
		protected virtual void LateUpdate()
		{
			if (this.UpdateMode == MMPathMovement.UpdateModes.LateUpdate)
			{
				this.ExecuteUpdate();
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000B374 File Offset: 0x00009574
		protected virtual void Update()
		{
			if (this.UpdateMode == MMPathMovement.UpdateModes.Update)
			{
				this.ExecuteUpdate();
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000B384 File Offset: 0x00009584
		protected virtual void PointReached()
		{
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000B386 File Offset: 0x00009586
		protected virtual void EndReached()
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000B388 File Offset: 0x00009588
		protected virtual void ExecuteUpdate()
		{
			if (this.PathElements == null || this.PathElements.Count < 1 || this._endReached || !this.CanMove)
			{
				this.CurrentSpeed = this._vector3Zero;
				return;
			}
			this.Move();
			this._positionLastFrame = base.transform.position;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000B3E0 File Offset: 0x000095E0
		protected virtual void Move()
		{
			this._waiting -= Time.deltaTime;
			if (this._waiting > 0f)
			{
				this.CurrentSpeed = Vector3.zero;
				return;
			}
			this._initialPosition = base.transform.position;
			this.MoveAlongThePath();
			this._distanceToNextPoint = (base.transform.position - this.PointPosition(this._currentPoint.Current)).magnitude;
			if (this._distanceToNextPoint < this.MinDistanceToGoal)
			{
				if (this.PathElements.Count > this._currentIndex)
				{
					this._waiting = this.PathElements[this._currentIndex].Delay;
				}
				this.PointReached();
				this._previousPoint = this._currentPoint.Current;
				this._currentPoint.MoveNext();
			}
			this._finalPosition = base.transform.position;
			if (Time.deltaTime != 0f)
			{
				this.CurrentSpeed = (this._finalPosition - this._initialPosition) / Time.deltaTime;
			}
			if (this._endReached)
			{
				this.EndReached();
				this.CurrentSpeed = Vector3.zero;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000B518 File Offset: 0x00009718
		public virtual void MoveAlongThePath()
		{
			switch (this.AccelerationType)
			{
			case MMPathMovement.PossibleAccelerationType.ConstantSpeed:
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.PointPosition(this._currentPoint.Current), Time.deltaTime * this.MovementSpeed);
				return;
			case MMPathMovement.PossibleAccelerationType.EaseOut:
				base.transform.position = Vector3.Lerp(base.transform.position, this.PointPosition(this._currentPoint.Current), Time.deltaTime * this.MovementSpeed);
				return;
			case MMPathMovement.PossibleAccelerationType.AnimationCurve:
			{
				float num = Vector3.Distance(this._previousPoint, this._currentPoint.Current);
				if (num <= 0f)
				{
					return;
				}
				float num2 = 1f - MMMaths.Remap(this._distanceToNextPoint, 0f, num, 0f, 1f);
				float num3 = this.Acceleration.Evaluate(num2);
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.PointPosition(this._currentPoint.Current), Time.deltaTime * this.MovementSpeed * num3);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B63A File Offset: 0x0000983A
		public virtual IEnumerator<Vector3> GetPathEnumerator()
		{
			if (this.PathElements == null || this.PathElements.Count < 1)
			{
				yield break;
			}
			int index = 0;
			this._currentIndex = index;
			for (;;)
			{
				this._currentIndex = index;
				yield return this.PathElements[index].PathElementPosition;
				if (this.PathElements.Count > 1)
				{
					switch (this.CycleOption)
					{
					case MMPathMovement.CycleOptions.BackAndForth:
						if (index <= 0)
						{
							this._direction = 1;
						}
						else if (index >= this.PathElements.Count - 1)
						{
							this._direction = -1;
						}
						index += this._direction;
						break;
					case MMPathMovement.CycleOptions.Loop:
						index += this._direction;
						if (index < 0)
						{
							index = this.PathElements.Count - 1;
						}
						else if (index > this.PathElements.Count - 1)
						{
							index = 0;
						}
						break;
					case MMPathMovement.CycleOptions.OnlyOnce:
						if (index <= 0)
						{
							this._direction = 1;
						}
						else if (index >= this.PathElements.Count - 1)
						{
							this._direction = 0;
							this.CurrentSpeed = Vector3.zero;
							this._endReached = true;
						}
						index += this._direction;
						break;
					case MMPathMovement.CycleOptions.StopAtBounds:
						if (index <= 0)
						{
							if (this._direction == -1)
							{
								this.CurrentSpeed = Vector3.zero;
								this._endReached = true;
							}
							this._direction = 1;
						}
						else if (index >= this.PathElements.Count - 1)
						{
							if (this._direction == 1)
							{
								this.CurrentSpeed = Vector3.zero;
								this._endReached = true;
							}
							this._direction = -1;
						}
						index += this._direction;
						break;
					case MMPathMovement.CycleOptions.Random:
					{
						int num = index;
						if (this.PathElements.Count > 1)
						{
							while (num == index)
							{
								num = Random.Range(0, this.PathElements.Count);
							}
						}
						index = num;
						break;
					}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B649 File Offset: 0x00009849
		public virtual void ChangeDirection()
		{
			this._direction = -this._direction;
			this._currentPoint.MoveNext();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B664 File Offset: 0x00009864
		protected virtual void OnDrawGizmos()
		{
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B666 File Offset: 0x00009866
		public virtual Vector3 PointPosition(int index)
		{
			return this.PointPosition(this.PathElements[index].PathElementPosition);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B680 File Offset: 0x00009880
		public virtual Vector3 PointPosition(Vector3 relativePointPosition)
		{
			switch (this.AlignmentMode)
			{
			case MMPathMovement.AlignmentModes.None:
				return this._originalTransformPosition + relativePointPosition;
			case MMPathMovement.AlignmentModes.ThisRotation:
				return this._originalTransformPosition + base.transform.rotation * relativePointPosition;
			case MMPathMovement.AlignmentModes.ParentRotation:
				return this._originalTransformPosition + base.transform.parent.rotation * relativePointPosition;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B6F9 File Offset: 0x000098F9
		public virtual void UpdateOriginalTransformPosition(Vector3 newOriginalTransformPosition)
		{
			this._originalTransformPosition = newOriginalTransformPosition;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B702 File Offset: 0x00009902
		public virtual Vector3 GetOriginalTransformPosition()
		{
			return this._originalTransformPosition;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000B70A File Offset: 0x0000990A
		public virtual void SetOriginalTransformPositionStatus(bool status)
		{
			this._originalTransformPositionStatus = status;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000B713 File Offset: 0x00009913
		public virtual bool GetOriginalTransformPositionStatus()
		{
			return this._originalTransformPositionStatus;
		}

		// Token: 0x04000344 RID: 836
		[Header("Path")]
		[MMInformation("Here you can select the '<b>Cycle Option</b>'. Back and Forth will have your object follow the path until its end, and go back to the original point. If you select Loop, the path will be closed and the object will move along it until told otherwise. If you select Only Once, the object will move along the path from the first to the last point, and remain there forever.", MMInformationAttribute.InformationType.Info, false)]
		public MMPathMovement.CycleOptions CycleOption;

		// Token: 0x04000345 RID: 837
		[MMInformation("Add points to the <b>Path</b> (set the size of the path first), then position the points using either the inspector or by moving the handles directly in scene view. For each path element you can specify a delay (in seconds). The order of the points will be the order the object follows.\nFor looping paths, you can then decide if the object will go through the points in the Path in Ascending (1, 2, 3...) or Descending (Last, Last-1, Last-2...) order.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the initial movement direction : ascending > will go from the points 0 to 1, 2, etc ; descending > will go from the last point to last-1, last-2, etc")]
		public MMPathMovement.MovementDirection LoopInitialMovementDirection;

		// Token: 0x04000346 RID: 838
		[Tooltip("the points that make up the path the object will follow")]
		public List<MMPathMovementElement> PathElements;

		// Token: 0x04000347 RID: 839
		[Header("Path Alignment")]
		[Tooltip("whether to align the path on nothing, this object's rotation, or this object's parent's rotation")]
		public MMPathMovement.AlignmentModes AlignmentMode;

		// Token: 0x04000348 RID: 840
		[Header("Movement")]
		[MMInformation("Set the <b>speed</b> at which the path will be crawled, and if the movement should be constant or eased.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the movement speed")]
		public float MovementSpeed = 1f;

		// Token: 0x0400034A RID: 842
		[Tooltip("the movement type of the object")]
		public MMPathMovement.PossibleAccelerationType AccelerationType;

		// Token: 0x0400034B RID: 843
		[Tooltip("the acceleration to apply to an object traveling between two points of the path.")]
		public AnimationCurve Acceleration = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400034C RID: 844
		[Tooltip("the chosen update mode (update, fixed update, late update)")]
		public MMPathMovement.UpdateModes UpdateMode;

		// Token: 0x0400034D RID: 845
		[Header("Settings")]
		[MMInformation("The <b>MinDistanceToGoal</b> is used to check if we've (almost) reached a point in the Path. The 2 other settings here are for debug only, don't change them.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the minimum distance to a point at which we'll arbitrarily decide the point's been reached")]
		public float MinDistanceToGoal = 0.1f;

		// Token: 0x0400034E RID: 846
		[Tooltip("the original position of the transform, hidden and shouldn't be accessed")]
		protected Vector3 _originalTransformPosition;

		// Token: 0x04000350 RID: 848
		protected bool _originalTransformPositionStatus;

		// Token: 0x04000351 RID: 849
		protected bool _active;

		// Token: 0x04000352 RID: 850
		protected IEnumerator<Vector3> _currentPoint;

		// Token: 0x04000353 RID: 851
		protected int _direction = 1;

		// Token: 0x04000354 RID: 852
		protected Vector3 _initialPosition;

		// Token: 0x04000355 RID: 853
		protected Vector3 _finalPosition;

		// Token: 0x04000356 RID: 854
		protected Vector3 _previousPoint = Vector3.zero;

		// Token: 0x04000357 RID: 855
		protected float _waiting;

		// Token: 0x04000358 RID: 856
		protected int _currentIndex;

		// Token: 0x04000359 RID: 857
		protected float _distanceToNextPoint;

		// Token: 0x0400035A RID: 858
		protected bool _endReached;

		// Token: 0x0400035B RID: 859
		protected Vector3 _positionLastFrame;

		// Token: 0x0400035C RID: 860
		protected Vector3 _vector3Zero = Vector3.zero;

		// Token: 0x020003D9 RID: 985
		public enum PossibleAccelerationType
		{
			// Token: 0x04001E26 RID: 7718
			ConstantSpeed,
			// Token: 0x04001E27 RID: 7719
			EaseOut,
			// Token: 0x04001E28 RID: 7720
			AnimationCurve
		}

		// Token: 0x020003DA RID: 986
		public enum CycleOptions
		{
			// Token: 0x04001E2A RID: 7722
			BackAndForth,
			// Token: 0x04001E2B RID: 7723
			Loop,
			// Token: 0x04001E2C RID: 7724
			OnlyOnce,
			// Token: 0x04001E2D RID: 7725
			StopAtBounds,
			// Token: 0x04001E2E RID: 7726
			Random
		}

		// Token: 0x020003DB RID: 987
		public enum MovementDirection
		{
			// Token: 0x04001E30 RID: 7728
			Ascending,
			// Token: 0x04001E31 RID: 7729
			Descending
		}

		// Token: 0x020003DC RID: 988
		public enum UpdateModes
		{
			// Token: 0x04001E33 RID: 7731
			Update,
			// Token: 0x04001E34 RID: 7732
			FixedUpdate,
			// Token: 0x04001E35 RID: 7733
			LateUpdate
		}

		// Token: 0x020003DD RID: 989
		public enum AlignmentModes
		{
			// Token: 0x04001E37 RID: 7735
			None,
			// Token: 0x04001E38 RID: 7736
			ThisRotation,
			// Token: 0x04001E39 RID: 7737
			ParentRotation
		}
	}
}
