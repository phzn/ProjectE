using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000066 RID: 102
	[AddComponentMenu("More Mountains/Tools/Movement/MMPath")]
	public class MMPath : MonoBehaviour
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000ADC7 File Offset: 0x00008FC7
		public bool EndReached
		{
			get
			{
				return this._endReached;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000ADCF File Offset: 0x00008FCF
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000ADD7 File Offset: 0x00008FD7
		public virtual bool CanMove { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000ADE8 File Offset: 0x00008FE8
		public virtual bool Initialized { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000ADF1 File Offset: 0x00008FF1
		public virtual int Direction
		{
			get
			{
				return this._direction;
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000ADF9 File Offset: 0x00008FF9
		protected virtual void Start()
		{
			if (!this.Initialized)
			{
				this.Initialization();
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000AE0C File Offset: 0x0000900C
		public virtual void Initialization()
		{
			this._active = true;
			this._endReached = false;
			this.CanMove = true;
			if (this.ReferenceMMPath != null && (this.ReferenceMMPath.PathElements != null || this.ReferenceMMPath.PathElements.Count > 0))
			{
				if (this.AbsoluteReferencePath)
				{
					base.transform.position = this.ReferenceMMPath.transform.position;
				}
				this.PathElements = this.ReferenceMMPath.PathElements;
			}
			if (this.PathElements == null || this.PathElements.Count < 1)
			{
				return;
			}
			if (this.PathElements[0].PathElementPosition != Vector3.zero)
			{
				Vector3 pathElementPosition = this.PathElements[0].PathElementPosition;
				base.transform.position += pathElementPosition;
				foreach (MMPathMovementElement mmpathMovementElement in this.PathElements)
				{
					mmpathMovementElement.PathElementPosition -= pathElementPosition;
				}
			}
			if (this.LoopInitialMovementDirection == MMPath.MovementDirection.Ascending)
			{
				this._direction = 1;
			}
			else
			{
				this._direction = -1;
			}
			this._initialPosition = base.transform.position;
			this._currentPoint = this.GetPathEnumerator();
			this._previousPoint = this._currentPoint.Current;
			this._currentPoint.MoveNext();
			if (!this._originalTransformPositionStatus)
			{
				this._originalTransformPositionStatus = true;
				this._originalTransformPosition = base.transform.position;
			}
			base.transform.position = this._originalTransformPosition + this._currentPoint.Current;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public int CurrentIndex()
		{
			return this._currentIndex;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000AFD8 File Offset: 0x000091D8
		public Vector3 CurrentPoint()
		{
			return this._initialPosition + this._currentPoint.Current;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000AFF0 File Offset: 0x000091F0
		public Vector3 CurrentPositionRelative()
		{
			return this._currentPoint.Current;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000AFFD File Offset: 0x000091FD
		protected virtual void Update()
		{
			if (this.PathElements == null || this.PathElements.Count < 1 || this._endReached || !this.CanMove)
			{
				return;
			}
			this.ComputePath();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B02C File Offset: 0x0000922C
		protected virtual void ComputePath()
		{
			this._initialPositionThisFrame = base.transform.position;
			this._distanceToNextPoint = (base.transform.position - (this._originalTransformPosition + this._currentPoint.Current)).magnitude;
			if (this._distanceToNextPoint < this.MinDistanceToGoal)
			{
				this._previousPoint = this._currentPoint.Current;
				this._currentPoint.MoveNext();
			}
			this._finalPosition = base.transform.position;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000B0BA File Offset: 0x000092BA
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
					if (this.CycleOption == MMPath.CycleOptions.Loop)
					{
						index += this._direction;
						if (index < 0)
						{
							index = this.PathElements.Count - 1;
						}
						else if (index > this.PathElements.Count - 1)
						{
							index = 0;
						}
					}
					if (this.CycleOption == MMPath.CycleOptions.BackAndForth)
					{
						if (index <= 0)
						{
							this._direction = 1;
						}
						else if (index >= this.PathElements.Count - 1)
						{
							this._direction = -1;
						}
						index += this._direction;
					}
					if (this.CycleOption == MMPath.CycleOptions.OnlyOnce)
					{
						if (index <= 0)
						{
							this._direction = 1;
						}
						else if (index >= this.PathElements.Count - 1)
						{
							this._direction = 0;
							this._endReached = true;
						}
						index += this._direction;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000B0C9 File Offset: 0x000092C9
		public virtual void ChangeDirection()
		{
			this._direction = -this._direction;
			this._currentPoint.MoveNext();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B0E4 File Offset: 0x000092E4
		protected virtual void OnDrawGizmos()
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B0E6 File Offset: 0x000092E6
		public virtual void UpdateOriginalTransformPosition(Vector3 newOriginalTransformPosition)
		{
			this._originalTransformPosition = newOriginalTransformPosition;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000B0EF File Offset: 0x000092EF
		public virtual Vector3 GetOriginalTransformPosition()
		{
			return this._originalTransformPosition;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000B0F7 File Offset: 0x000092F7
		public virtual void SetOriginalTransformPositionStatus(bool status)
		{
			this._originalTransformPositionStatus = status;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000B100 File Offset: 0x00009300
		public virtual bool GetOriginalTransformPositionStatus()
		{
			return this._originalTransformPositionStatus;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000B108 File Offset: 0x00009308
		public void SetPath(in MMPath.Data configuration)
		{
			if (configuration.Offsets == null)
			{
				return;
			}
			this._active = true;
			this._endReached = false;
			this.CanMove = true;
			this.PathElements = this.PathElements ?? new List<MMPathMovementElement>(configuration.Offsets.Length);
			this.PathElements.Clear();
			foreach (Vector3 vector in configuration.Offsets)
			{
				this.PathElements.Add(new MMPathMovementElement
				{
					Delay = configuration.Delay,
					PathElementPosition = vector
				});
			}
			if (this.PathElements == null || this.PathElements.Count < 1)
			{
				return;
			}
			this.CycleOption = configuration.Cycle;
			if (configuration.Direction == MMPath.MovementDirection.Ascending)
			{
				this._direction = 1;
			}
			else
			{
				this._direction = -1;
			}
			this._initialPosition = configuration.Center;
			this._originalTransformPosition = configuration.Center;
			this._currentPoint = this.GetPathEnumerator();
			this._previousPoint = this._currentPoint.Current;
			this._currentPoint.MoveNext();
		}

		// Token: 0x0400032D RID: 813
		[Header("Path")]
		[MMInformation("Here you can select the '<b>Cycle Option</b>'. Back and Forth will have your object follow the path until its end, and go back to the original point. If you select Loop, the path will be closed and the object will move along it until told otherwise. If you select Only Once, the object will move along the path from the first to the last point, and remain there forever.", MMInformationAttribute.InformationType.Info, false)]
		public MMPath.CycleOptions CycleOption;

		// Token: 0x0400032E RID: 814
		[MMInformation("Add points to the <b>Path</b> (set the size of the path first), then position the points using either the inspector or by moving the handles directly in scene view. For each path element you can specify a delay (in seconds). The order of the points will be the order the object follows.\nFor looping paths, you can then decide if the object will go through the points in the Path in Ascending (1, 2, 3...) or Descending (Last, Last-1, Last-2...) order.", MMInformationAttribute.InformationType.Info, false)]
		public MMPath.MovementDirection LoopInitialMovementDirection;

		// Token: 0x0400032F RID: 815
		public List<MMPathMovementElement> PathElements;

		// Token: 0x04000330 RID: 816
		public MMPath ReferenceMMPath;

		// Token: 0x04000331 RID: 817
		public bool AbsoluteReferencePath;

		// Token: 0x04000332 RID: 818
		public float MinDistanceToGoal = 0.1f;

		// Token: 0x04000333 RID: 819
		[Header("Gizmos")]
		public bool LockHandlesOnXAxis;

		// Token: 0x04000334 RID: 820
		public bool LockHandlesOnYAxis;

		// Token: 0x04000335 RID: 821
		public bool LockHandlesOnZAxis;

		// Token: 0x04000336 RID: 822
		protected Vector3 _originalTransformPosition;

		// Token: 0x04000337 RID: 823
		protected bool _originalTransformPositionStatus;

		// Token: 0x0400033A RID: 826
		protected bool _active;

		// Token: 0x0400033B RID: 827
		protected IEnumerator<Vector3> _currentPoint;

		// Token: 0x0400033C RID: 828
		protected int _direction = 1;

		// Token: 0x0400033D RID: 829
		protected Vector3 _initialPosition;

		// Token: 0x0400033E RID: 830
		protected Vector3 _initialPositionThisFrame;

		// Token: 0x0400033F RID: 831
		protected Vector3 _finalPosition;

		// Token: 0x04000340 RID: 832
		protected Vector3 _previousPoint = Vector3.zero;

		// Token: 0x04000341 RID: 833
		protected int _currentIndex;

		// Token: 0x04000342 RID: 834
		protected float _distanceToNextPoint;

		// Token: 0x04000343 RID: 835
		protected bool _endReached;

		// Token: 0x020003D5 RID: 981
		public enum CycleOptions
		{
			// Token: 0x04001E16 RID: 7702
			BackAndForth,
			// Token: 0x04001E17 RID: 7703
			Loop,
			// Token: 0x04001E18 RID: 7704
			OnlyOnce
		}

		// Token: 0x020003D6 RID: 982
		public enum MovementDirection
		{
			// Token: 0x04001E1A RID: 7706
			Ascending,
			// Token: 0x04001E1B RID: 7707
			Descending
		}

		// Token: 0x020003D7 RID: 983
		[Serializable]
		public struct Data
		{
			// Token: 0x0600192F RID: 6447 RVA: 0x0006A564 File Offset: 0x00068764
			public static MMPath.Data ForwardLoopingPath(Vector3 ctr, Vector3[] vtx, float wait)
			{
				return new MMPath.Data
				{
					Center = ctr,
					Offsets = vtx,
					Delay = wait,
					Cycle = MMPath.CycleOptions.Loop,
					Direction = MMPath.MovementDirection.Ascending
				};
			}

			// Token: 0x06001930 RID: 6448 RVA: 0x0006A5A4 File Offset: 0x000687A4
			public static MMPath.Data ForwardBackAndForthPath(Vector3 ctr, Vector3[] vtx, float wait)
			{
				return new MMPath.Data
				{
					Center = ctr,
					Offsets = vtx,
					Delay = wait,
					Cycle = MMPath.CycleOptions.BackAndForth,
					Direction = MMPath.MovementDirection.Ascending
				};
			}

			// Token: 0x06001931 RID: 6449 RVA: 0x0006A5E4 File Offset: 0x000687E4
			public static MMPath.Data ForwardOnlyOncePath(Vector3 ctr, Vector3[] vtx, float wait)
			{
				return new MMPath.Data
				{
					Center = ctr,
					Offsets = vtx,
					Delay = wait,
					Cycle = MMPath.CycleOptions.OnlyOnce,
					Direction = MMPath.MovementDirection.Ascending
				};
			}

			// Token: 0x04001E1C RID: 7708
			public Vector3 Center;

			// Token: 0x04001E1D RID: 7709
			public Vector3[] Offsets;

			// Token: 0x04001E1E RID: 7710
			public float Delay;

			// Token: 0x04001E1F RID: 7711
			public MMPath.CycleOptions Cycle;

			// Token: 0x04001E20 RID: 7712
			public MMPath.MovementDirection Direction;
		}
	}
}
