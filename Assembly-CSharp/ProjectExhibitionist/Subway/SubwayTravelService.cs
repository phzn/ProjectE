using System;
using Features.RenderingPrecision;
using Features.RunModes;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200093E RID: 2366
	public static class SubwayTravelService
	{
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x0017E274 File Offset: 0x0017C474
		// (set) Token: 0x0600405E RID: 16478 RVA: 0x0017E27B File Offset: 0x0017C47B
		public static float LastTripMeters { get; private set; }

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x0017E283 File Offset: 0x0017C483
		// (set) Token: 0x06004060 RID: 16480 RVA: 0x0017E28A File Offset: 0x0017C48A
		public static Vector2Int LastTripCells { get; private set; }

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x0017E292 File Offset: 0x0017C492
		// (set) Token: 0x06004062 RID: 16482 RVA: 0x0017E299 File Offset: 0x0017C499
		public static float LastBoardingAdjustmentMeters { get; private set; }

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06004063 RID: 16483 RVA: 0x0017E2A4 File Offset: 0x0017C4A4
		// (remove) Token: 0x06004064 RID: 16484 RVA: 0x0017E2D8 File Offset: 0x0017C4D8
		public static event Action<Transform> WorldRelocated;

		// Token: 0x06004065 RID: 16485 RVA: 0x0017E30B File Offset: 0x0017C50B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			SubwayTravelService._departure = null;
			SubwayTravelService._pendingMeters = (SubwayTravelService.LastTripMeters = 0f);
			SubwayTravelService.LastTripCells = Vector2Int.zero;
			SubwayTravelService.LastBoardingAdjustmentMeters = 0f;
			SubwayTravelService.WorldRelocated = null;
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0017E340 File Offset: 0x0017C540
		public static void BeginTrip(Vector3 worldTravelDirection, float meters, Transform keepAlive = null)
		{
			SubwayTravelService.CompleteTrip();
			SubwayTravelService.LastTripMeters = 0f;
			SubwayTravelService.LastTripCells = Vector2Int.zero;
			SubwayTravelService.LastBoardingAdjustmentMeters = 0f;
			if (RunModeService.IsLongWayHome || meters <= 0f)
			{
				return;
			}
			SubwayRideDirector subwayRideDirector = ((keepAlive != null) ? keepAlive.GetComponent<SubwayRideDirector>() : null);
			ChunkManager instance = ChunkManager.Instance;
			Transform player = PlayerRegistry.Player;
			Vector3 normalized = Vector3.ProjectOnPlane(worldTravelDirection, Vector3.up).normalized;
			ChunkInstance chunkInstance;
			ChunkSpawnResolver.SpawnEntry spawnEntry;
			Vector3 vector;
			Vector3 vector2;
			float num;
			if (subwayRideDirector == null || player == null || instance == null || instance.CellSize <= 0.01f || normalized.sqrMagnitude < 0.5f || !instance.TryGetSubwayTravelLayout(keepAlive, out chunkInstance, out spawnEntry) || !subwayRideDirector.TryGetArrivalCabin(out vector, out vector2, out num))
			{
				Debug.LogWarning("[SubwayTravel] No valid departure station: staying at the original platform.");
				return;
			}
			Vector3 vector3 = WorldOrigin.ToLogical(player.position);
			Vector3 vector4 = WorldOrigin.ToLogical(vector);
			vector4.y = vector3.y;
			if (!SubwayTravelService.CabinContains(vector3, WorldOrigin.ToLogical(vector), vector2, num))
			{
				vector3 = vector4;
			}
			SubwayRideDirector[] componentsInChildren = chunkInstance.chunkRoot.GetComponentsInChildren<SubwayRideDirector>(true);
			ChunkManager.SubwayConnection subwayConnection;
			if (instance.TryGetSubwayConnection(chunkInstance.originCell, normalized, out subwayConnection) && SubwayTravelService.CabinContains(subwayConnection.boardingLogical, WorldOrigin.ToLogical(vector), vector2, num) && instance.TryReserveSubwayDestination(subwayRideDirector, subwayConnection.destination, spawnEntry))
			{
				Vector3 vector5 = WorldOrigin.ToUnity(subwayConnection.boardingLogical);
				SubwayTravelService.LastBoardingAdjustmentMeters = Vector3.Distance(player.position, vector5);
				SubwayRideDirector.PlaceConnectingPassenger(player, vector5);
				SubwayTravelService.ActivateTrip(subwayRideDirector, chunkInstance.originCell, normalized, subwayConnection, default(ChunkManager.SubwayConnection), false, meters);
				return;
			}
			int num2 = Mathf.CeilToInt(meters / instance.CellSize) + 2;
			float num3 = float.PositiveInfinity;
			Vector2Int vector2Int = default(Vector2Int);
			Vector3 vector6 = normalized;
			Vector3 vector7 = default(Vector3);
			ChunkManager.SubwayConnection subwayConnection2 = default(ChunkManager.SubwayConnection);
			int num4 = 0;
			while (num4 < 2 && float.IsPositiveInfinity(num3))
			{
				if (num4 == 1)
				{
					vector3 = vector4;
				}
				for (int i = -num2; i <= num2; i++)
				{
					for (int j = -num2; j <= num2; j++)
					{
						Vector2Int vector2Int2 = chunkInstance.originCell + new Vector2Int(i, j);
						if (!(vector2Int2 == chunkInstance.originCell))
						{
							Vector3 vector8 = instance.CellToWorld(vector2Int2, spawnEntry.chunkData.gridSize) - chunkInstance.worldPosition;
							if (instance.CanReserveSubwayDestination(vector2Int2, spawnEntry))
							{
								foreach (SubwayRideDirector subwayRideDirector2 in componentsInChildren)
								{
									if (instance.CanCreateSubwayConnection(chunkInstance.originCell, normalized, vector2Int2, subwayRideDirector2.TravelDirection) && subwayRideDirector2.TryGetArrivalCabin(out vector, out vector2, out num))
									{
										Vector3 vector9 = WorldOrigin.ToLogical(vector + vector8);
										Vector3 vector10;
										if (SubwayTravelService.TrySolveArrivalPoint(vector3, vector9, vector2, num, meters, out vector10))
										{
											float num5 = Vector3.Dot((vector10 - vector3).normalized, normalized);
											if (num5 >= 0.5f)
											{
												float num6 = (1f - num5) * 1000f + Vector3.Distance(vector10, vector9) * 0.01f;
												ChunkManager.SubwayConnection subwayConnection3;
												if (num6 < num3 && SubwayTravelService.TryPlanReverse(componentsInChildren, vector8, normalized, subwayRideDirector2.TravelDirection, vector3.y, meters, chunkInstance.originCell, out subwayConnection3))
												{
													num3 = num6;
													vector2Int = vector2Int2;
													vector6 = subwayRideDirector2.TravelDirection;
													vector7 = vector10;
													subwayConnection2 = subwayConnection3;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				num4++;
			}
			if (float.IsPositiveInfinity(num3) || !instance.TryReserveSubwayDestination(subwayRideDirector, vector2Int, spawnEntry))
			{
				Debug.LogWarning("[SubwayTravel] No safe connecting platform is available; no distance credited.");
				return;
			}
			Vector3 vector11 = WorldOrigin.ToUnity(vector3);
			SubwayTravelService.LastBoardingAdjustmentMeters = Vector3.Distance(player.position, vector11);
			if (SubwayTravelService.LastBoardingAdjustmentMeters > 0.001f)
			{
				SubwayRideDirector.PlaceConnectingPassenger(player, vector11);
			}
			ChunkManager.SubwayConnection subwayConnection4 = new ChunkManager.SubwayConnection(vector2Int, vector3, vector7, vector6);
			SubwayTravelService.ActivateTrip(subwayRideDirector, chunkInstance.originCell, normalized, subwayConnection4, subwayConnection2, true, meters);
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0017E738 File Offset: 0x0017C938
		private static void ActivateTrip(SubwayRideDirector departure, Vector2Int sourceCell, Vector3 direction, ChunkManager.SubwayConnection outward, ChunkManager.SubwayConnection reverse, bool isNew, float meters)
		{
			SubwayTravelService._departure = departure;
			SubwayTravelService._direction = outward.arrivalDirection;
			SubwayTravelService._departureDirection = direction;
			SubwayTravelService._departureCell = sourceCell;
			SubwayTravelService._departureLogicalPosition = outward.boardingLogical;
			SubwayTravelService._destinationCell = outward.destination;
			SubwayTravelService._plannedOutward = outward;
			SubwayTravelService._plannedReverse = reverse;
			SubwayTravelService._newConnection = isNew;
			SubwayTravelService._pendingMeters = meters;
			SubwayTravelService.LastTripCells = outward.destination - sourceCell;
			Action<Transform> worldRelocated = SubwayTravelService.WorldRelocated;
			if (worldRelocated == null)
			{
				return;
			}
			worldRelocated(departure.transform);
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0017E7BC File Offset: 0x0017C9BC
		private static bool CabinContains(Vector3 point, Vector3 center, Vector3 axis, float halfLength)
		{
			Vector3 vector = point - center;
			vector.y = 0f;
			float num = Vector3.Dot(vector, axis);
			return Mathf.Abs(num) <= halfLength + 0.02f && (vector - axis * num).sqrMagnitude <= 0.49f;
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0017E818 File Offset: 0x0017CA18
		private static bool TryPlanReverse(SubwayRideDirector[] platforms, Vector3 destinationOffset, Vector3 departureDirection, Vector3 arrivalDirection, float passengerY, float meters, Vector2Int originalCell, out ChunkManager.SubwayConnection reverse)
		{
			reverse = default(ChunkManager.SubwayConnection);
			float num = float.PositiveInfinity;
			foreach (SubwayRideDirector subwayRideDirector in platforms)
			{
				Vector3 vector;
				Vector3 vector2;
				float num2;
				if (Vector3.Dot(subwayRideDirector.TravelDirection, -arrivalDirection) >= 0.99f && subwayRideDirector.TryGetArrivalCabin(out vector, out vector2, out num2))
				{
					vector = WorldOrigin.ToLogical(vector + destinationOffset);
					vector.y = passengerY;
					foreach (SubwayRideDirector subwayRideDirector2 in platforms)
					{
						Vector3 vector3;
						Vector3 vector4;
						float num3;
						if (Vector3.Dot(subwayRideDirector2.TravelDirection, -departureDirection) >= 0.99f && subwayRideDirector2.TryGetArrivalCabin(out vector3, out vector4, out num3))
						{
							vector3 = WorldOrigin.ToLogical(vector3);
							for (int k = 0; k < 3; k++)
							{
								float num4 = ((k == 0) ? 0f : ((k == 1) ? (-num2) : num2));
								Vector3 vector5 = vector + vector2 * num4;
								Vector3 vector6;
								if (SubwayTravelService.TrySolveArrivalPoint(vector5, vector3, vector4, num3, meters, out vector6))
								{
									if (k != 0)
									{
										float num5 = 0f;
										float num6 = num4;
										for (int l = 0; l < 12; l++)
										{
											float num7 = (num5 + num6) * 0.5f;
											Vector3 vector7;
											if (SubwayTravelService.TrySolveArrivalPoint(vector + vector2 * num7, vector3, vector4, num3 - 0.05f, meters, out vector7))
											{
												num6 = num7;
											}
											else
											{
												num5 = num7;
											}
										}
										Vector3 vector8;
										if (SubwayTravelService.TrySolveArrivalPoint(vector + vector2 * num6, vector3, vector4, num3, meters, out vector8))
										{
											num4 = num6;
											vector5 = vector + vector2 * num6;
											vector6 = vector8;
										}
									}
									float num8 = Mathf.Abs(num4);
									if (num8 < num)
									{
										num = num8;
										reverse = new ChunkManager.SubwayConnection(originalCell, vector5, vector6, subwayRideDirector2.TravelDirection);
									}
								}
							}
						}
					}
				}
			}
			return !float.IsPositiveInfinity(num);
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0017EA14 File Offset: 0x0017CC14
		public static bool TrySolveArrivalPoint(Vector3 departure, Vector3 cabinCenter, Vector3 cabinAxis, float safeHalfLength, float meters, out Vector3 arrival)
		{
			arrival = default(Vector3);
			if (meters <= 0f || safeHalfLength < 0f)
			{
				return false;
			}
			cabinAxis = Vector3.ProjectOnPlane(cabinAxis, Vector3.up).normalized;
			if (cabinAxis.sqrMagnitude < 0.5f)
			{
				return false;
			}
			cabinCenter.y = departure.y;
			Vector3 vector = cabinCenter - departure;
			float num = Vector3.Dot(vector, cabinAxis);
			float num2 = num * num - vector.sqrMagnitude + meters * meters;
			if (num2 < 0f)
			{
				return false;
			}
			float num3 = Mathf.Sqrt(num2);
			float num4 = -num + num3;
			float num5 = -num - num3;
			float num6 = ((Mathf.Abs(num4) <= Mathf.Abs(num5)) ? num4 : num5);
			if (Mathf.Abs(num6) > safeHalfLength)
			{
				return false;
			}
			arrival = cabinCenter + cabinAxis * num6;
			return true;
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x0600406B RID: 16491 RVA: 0x0017EAE8 File Offset: 0x0017CCE8
		public static bool HasPendingTrip
		{
			get
			{
				return SubwayTravelService._departure != null && SubwayTravelService._pendingMeters > 0f;
			}
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0017EB05 File Offset: 0x0017CD05
		public static bool IsPreparingDepartureChunk(ChunkInstance chunk)
		{
			return SubwayTravelService.HasPendingTrip && ((chunk != null) ? chunk.chunkRoot : null) != null && SubwayTravelService._departure.transform.IsChildOf(chunk.chunkRoot.transform);
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0017EB40 File Offset: 0x0017CD40
		public static bool TryGetArrival(out SubwayRideDirector arrival, out Vector3 position)
		{
			arrival = null;
			position = default(Vector3);
			ChunkManager instance = ChunkManager.Instance;
			Vector3 vector;
			Vector3 vector2;
			float num;
			if (!SubwayTravelService.HasPendingTrip || instance == null || !instance.TryGetReadySubwayDestination(SubwayTravelService._destinationCell, SubwayTravelService._direction, out arrival) || !arrival.TryGetArrivalCabin(out vector, out vector2, out num) || !SubwayTravelService.CabinContains(SubwayTravelService._plannedOutward.arrivalLogical, WorldOrigin.ToLogical(vector), vector2, num))
			{
				return false;
			}
			position = WorldOrigin.ToUnity(SubwayTravelService._plannedOutward.arrivalLogical);
			return true;
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0017EBC0 File Offset: 0x0017CDC0
		public static void RecordPhysicalArrival(Transform destination)
		{
			Transform player = PlayerRegistry.Player;
			if (!SubwayTravelService.HasPendingTrip || player == null)
			{
				return;
			}
			Vector3 vector = WorldOrigin.ToLogical(player.position) - SubwayTravelService._departureLogicalPosition;
			vector.y = 0f;
			SubwayTravelService.LastTripMeters = vector.magnitude;
			if (SubwayTravelService._newConnection)
			{
				ChunkManager instance = ChunkManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.RememberSubwayConnection(SubwayTravelService._departureCell, SubwayTravelService._departureDirection, SubwayTravelService._plannedOutward, SubwayTravelService._plannedReverse);
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0017EC3D File Offset: 0x0017CE3D
		public static void CompleteTrip()
		{
			if (SubwayTravelService._departure != null && ChunkManager.Instance != null)
			{
				ChunkManager.Instance.ReleasePreparationFocus(SubwayTravelService._departure);
			}
			SubwayTravelService._departure = null;
			SubwayTravelService._pendingMeters = 0f;
		}

		// Token: 0x04004033 RID: 16435
		private static SubwayRideDirector _departure;

		// Token: 0x04004034 RID: 16436
		private static Vector2Int _destinationCell;

		// Token: 0x04004035 RID: 16437
		private static Vector3 _departureLogicalPosition;

		// Token: 0x04004036 RID: 16438
		private static Vector3 _direction;

		// Token: 0x04004037 RID: 16439
		private static Vector3 _departureDirection;

		// Token: 0x04004038 RID: 16440
		private static Vector2Int _departureCell;

		// Token: 0x04004039 RID: 16441
		private static ChunkManager.SubwayConnection _plannedOutward;

		// Token: 0x0400403A RID: 16442
		private static ChunkManager.SubwayConnection _plannedReverse;

		// Token: 0x0400403B RID: 16443
		private static bool _newConnection;

		// Token: 0x0400403D RID: 16445
		private static float _pendingMeters;
	}
}
