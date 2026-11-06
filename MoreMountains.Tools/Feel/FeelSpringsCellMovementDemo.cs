using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000382 RID: 898
	[AddComponentMenu("")]
	public class FeelSpringsCellMovementDemo : MonoBehaviour
	{
		// Token: 0x06001894 RID: 6292 RVA: 0x00068630 File Offset: 0x00066830
		protected virtual void Update()
		{
			this.UpdateSliderValues();
			this.HandleInput();
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00068640 File Offset: 0x00066840
		public virtual void MoveRandomly()
		{
			int num = Random.Range(0, 4);
			this.Move((FeelSpringsCellMovementDemo.Directions)num);
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x0006865C File Offset: 0x0006685C
		protected virtual void Move(FeelSpringsCellMovementDemo.Directions direction)
		{
			this.ComputeNewGridPosition(direction);
			this._movementPosition.x = this._currentPosition.x * this._cellWidth;
			this._movementPosition.y = this._currentPosition.y * this._cellWidth;
			this._movementPosition.z = this.MovementSpring.Target.transform.localPosition.z;
			this.MovementSpring.MoveTo(this._movementPosition);
			this.ScaleSpring.Bump(5f * Vector3.one);
			MMFeedbacks moveFeedback = this.MoveFeedback;
			if (moveFeedback == null)
			{
				return;
			}
			moveFeedback.PlayFeedbacks();
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0006870C File Offset: 0x0006690C
		protected virtual void Bump(FeelSpringsCellMovementDemo.Directions direction)
		{
			switch (direction)
			{
			case FeelSpringsCellMovementDemo.Directions.Left:
				this.RotationSpring.Bump(new Vector3(0f, 0f, -900f));
				this.MovementSpring.Bump(new Vector3(4f, 0f, 0f));
				return;
			case FeelSpringsCellMovementDemo.Directions.Right:
				this.RotationSpring.Bump(new Vector3(0f, 0f, 900f));
				this.MovementSpring.Bump(new Vector3(-4f, 0f, 0f));
				return;
			case FeelSpringsCellMovementDemo.Directions.Up:
				this.RotationSpring.Bump(new Vector3(0f, 0f, 450f));
				this.MovementSpring.Bump(new Vector3(0f, -4f, 0f));
				return;
			case FeelSpringsCellMovementDemo.Directions.Down:
				this.RotationSpring.Bump(new Vector3(0f, 0f, -450f));
				this.MovementSpring.Bump(new Vector3(0f, 4f, 0f));
				return;
			default:
				return;
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0006882C File Offset: 0x00066A2C
		protected virtual void ComputeNewGridPosition(FeelSpringsCellMovementDemo.Directions direction)
		{
			switch (direction)
			{
			case FeelSpringsCellMovementDemo.Directions.Left:
				this._currentPosition.x = this._currentPosition.x - 1f;
				break;
			case FeelSpringsCellMovementDemo.Directions.Right:
				this._currentPosition.x = this._currentPosition.x + 1f;
				break;
			case FeelSpringsCellMovementDemo.Directions.Up:
				this._currentPosition.y = this._currentPosition.y + 1f;
				break;
			case FeelSpringsCellMovementDemo.Directions.Down:
				this._currentPosition.y = this._currentPosition.y - 1f;
				break;
			}
			if (this._currentPosition.x < -3f)
			{
				this._currentPosition.x = -3f;
				this.Bump(FeelSpringsCellMovementDemo.Directions.Left);
				return;
			}
			if (this._currentPosition.x > 3f)
			{
				this._currentPosition.x = 3f;
				this.Bump(FeelSpringsCellMovementDemo.Directions.Right);
				return;
			}
			if (this._currentPosition.y < -3f)
			{
				this._currentPosition.y = -3f;
				this.Bump(FeelSpringsCellMovementDemo.Directions.Down);
				return;
			}
			if (this._currentPosition.y > 3f)
			{
				this._currentPosition.y = 3f;
				this.Bump(FeelSpringsCellMovementDemo.Directions.Up);
				return;
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00068950 File Offset: 0x00066B50
		protected virtual void HandleInput()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.Move(FeelSpringsCellMovementDemo.Directions.Left);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.Move(FeelSpringsCellMovementDemo.Directions.Right);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.Move(FeelSpringsCellMovementDemo.Directions.Down);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.Move(FeelSpringsCellMovementDemo.Directions.Up);
			}
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000689AC File Offset: 0x00066BAC
		protected virtual void UpdateSliderValues()
		{
			this.MovementSpring.SpringVector3.SetDamping(this.DampingSlider.value * Vector3.one);
			this.MovementSpring.SpringVector3.SetFrequency(this.FrequencySlider.value * Vector3.one);
		}

		// Token: 0x04001CA8 RID: 7336
		[Header("Spring")]
		public MMSpringPosition MovementSpring;

		// Token: 0x04001CA9 RID: 7337
		public MMSpringRotation RotationSpring;

		// Token: 0x04001CAA RID: 7338
		public MMSpringScale ScaleSpring;

		// Token: 0x04001CAB RID: 7339
		[Header("Bindings")]
		public FeelSpringsDemoSlider DampingSlider;

		// Token: 0x04001CAC RID: 7340
		public FeelSpringsDemoSlider FrequencySlider;

		// Token: 0x04001CAD RID: 7341
		public MMFeedbacks MoveFeedback;

		// Token: 0x04001CAE RID: 7342
		protected Vector3 _newPosition;

		// Token: 0x04001CAF RID: 7343
		protected float _cellWidth = 0.125f;

		// Token: 0x04001CB0 RID: 7344
		protected Vector2 _currentPosition;

		// Token: 0x04001CB1 RID: 7345
		protected Vector3 _movementPosition;

		// Token: 0x0200059B RID: 1435
		protected enum Directions
		{
			// Token: 0x0400251B RID: 9499
			Left,
			// Token: 0x0400251C RID: 9500
			Right,
			// Token: 0x0400251D RID: 9501
			Up,
			// Token: 0x0400251E RID: 9502
			Down
		}
	}
}
