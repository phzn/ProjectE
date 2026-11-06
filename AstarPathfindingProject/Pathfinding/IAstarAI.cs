using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200001B RID: 27
	public interface IAstarAI
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600019D RID: 413
		// (set) Token: 0x0600019E RID: 414
		float radius { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600019F RID: 415
		// (set) Token: 0x060001A0 RID: 416
		float height { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001A1 RID: 417
		Vector3 position { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001A2 RID: 418
		// (set) Token: 0x060001A3 RID: 419
		Quaternion rotation { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001A4 RID: 420
		// (set) Token: 0x060001A5 RID: 421
		float maxSpeed { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001A6 RID: 422
		Vector3 velocity { get; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A7 RID: 423
		Vector3 desiredVelocity { get; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A8 RID: 424
		// (set) Token: 0x060001A9 RID: 425
		Vector3 desiredVelocityWithoutLocalAvoidance { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001AA RID: 426
		float remainingDistance { get; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AB RID: 427
		bool reachedDestination { get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001AC RID: 428
		bool reachedEndOfPath { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001AD RID: 429
		Vector3 endOfPath { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001AE RID: 430
		// (set) Token: 0x060001AF RID: 431
		Vector3 destination { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001B0 RID: 432
		// (set) Token: 0x060001B1 RID: 433
		bool canSearch { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B2 RID: 434
		// (set) Token: 0x060001B3 RID: 435
		bool simulateMovement { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00008211 File Offset: 0x00006411
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00008219 File Offset: 0x00006419
		[Obsolete("Renamed to simulateMovement")]
		bool canMove
		{
			get
			{
				return this.simulateMovement;
			}
			set
			{
				this.simulateMovement = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B6 RID: 438
		bool hasPath { get; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001B7 RID: 439
		bool pathPending { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B8 RID: 440
		// (set) Token: 0x060001B9 RID: 441
		bool updatePosition { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001BA RID: 442
		// (set) Token: 0x060001BB RID: 443
		bool updateRotation { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001BC RID: 444
		// (set) Token: 0x060001BD RID: 445
		bool isStopped { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001BE RID: 446
		Vector3 steeringTarget { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001BF RID: 447
		// (set) Token: 0x060001C0 RID: 448
		Action onSearchPath { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C1 RID: 449
		NativeMovementPlane movementPlane { get; }

		// Token: 0x060001C2 RID: 450
		void GetRemainingPath(List<Vector3> buffer, out bool stale);

		// Token: 0x060001C3 RID: 451
		void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, out bool stale);

		// Token: 0x060001C4 RID: 452
		void SearchPath();

		// Token: 0x060001C5 RID: 453
		void SetPath(Path path, bool updateDestinationFromPath = true);

		// Token: 0x060001C6 RID: 454
		void Teleport(Vector3 newPosition, bool clearPath = true);

		// Token: 0x060001C7 RID: 455
		void Move(Vector3 deltaPosition);

		// Token: 0x060001C8 RID: 456
		void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation);

		// Token: 0x060001C9 RID: 457
		void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation);
	}
}
