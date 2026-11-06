using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000150 RID: 336
[DefaultExecutionOrder(100)]
[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
public class ElevatorSimple : MonoBehaviour
{
	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00047D2A File Offset: 0x00045F2A
	// (set) Token: 0x06000A8A RID: 2698 RVA: 0x00047D32 File Offset: 0x00045F32
	public bool SimulationPaused
	{
		get
		{
			return this._simulationPaused;
		}
		set
		{
			this._simulationPaused = value;
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00047D3C File Offset: 0x00045F3C
	private void Start()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		this._length = base.transform.TransformVector(Vector3.right * component.size.x).magnitude;
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00047D80 File Offset: 0x00045F80
	private void OnTriggerEnter(Collider other)
	{
		if (this._simulationPaused)
		{
			return;
		}
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		if (attachedRigidbody == null)
		{
			return;
		}
		ElevatorSimple.RiderState riderState;
		if (this._riders.TryGetValue(attachedRigidbody, out riderState))
		{
			riderState.ContactCount++;
			return;
		}
		riderState = new ElevatorSimple.RiderState
		{
			ContactCount = 1,
			WasKinematic = attachedRigidbody.isKinematic,
			Velocity = attachedRigidbody.velocity,
			AngularVelocity = attachedRigidbody.angularVelocity
		};
		this._riders.Add(attachedRigidbody, riderState);
		if (!attachedRigidbody.isKinematic)
		{
			attachedRigidbody.isKinematic = true;
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00047E14 File Offset: 0x00046014
	private void OnTriggerExit(Collider other)
	{
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		if (attachedRigidbody == null)
		{
			return;
		}
		ElevatorSimple.RiderState riderState;
		if (!this._riders.TryGetValue(attachedRigidbody, out riderState))
		{
			return;
		}
		riderState.ContactCount--;
		if (riderState.ContactCount > 0)
		{
			return;
		}
		ElevatorSimple.RestoreRider(attachedRigidbody, riderState);
		this._riders.Remove(attachedRigidbody);
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x00047E70 File Offset: 0x00046070
	private void FixedUpdate()
	{
		if (this._simulationPaused || this._riders.Count == 0)
		{
			return;
		}
		Vector3 vector = (this._goesDown ? Vector3.left : Vector3.right);
		Vector3 normalized = base.transform.TransformVector(vector).normalized;
		float num = (Mathf.Approximately(this._travelTime, 0f) ? 7f : Mathf.Abs(this._travelTime));
		Vector3 vector2 = normalized * (this._length / num) * Time.fixedDeltaTime;
		this._staleRiders.Clear();
		foreach (KeyValuePair<Rigidbody, ElevatorSimple.RiderState> keyValuePair in this._riders)
		{
			Rigidbody key = keyValuePair.Key;
			if (key == null)
			{
				this._staleRiders.Add(key);
			}
			else
			{
				key.MovePosition(key.position + vector2);
			}
		}
		foreach (Rigidbody rigidbody in this._staleRiders)
		{
			this._riders.Remove(rigidbody);
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x00047FC8 File Offset: 0x000461C8
	private void OnDisable()
	{
		if (!this._simulationPaused)
		{
			this.RestoreAllRiders();
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00047FD8 File Offset: 0x000461D8
	private void OnDestroy()
	{
		this.RestoreAllRiders();
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x00047FE0 File Offset: 0x000461E0
	private void RestoreAllRiders()
	{
		foreach (KeyValuePair<Rigidbody, ElevatorSimple.RiderState> keyValuePair in this._riders)
		{
			if (keyValuePair.Key != null)
			{
				ElevatorSimple.RestoreRider(keyValuePair.Key, keyValuePair.Value);
			}
		}
		this._riders.Clear();
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0004805C File Offset: 0x0004625C
	private static void RestoreRider(Rigidbody rigidBody, ElevatorSimple.RiderState state)
	{
		if (rigidBody == null)
		{
			return;
		}
		rigidBody.isKinematic = state.WasKinematic;
		if (!state.WasKinematic)
		{
			rigidBody.velocity = state.Velocity;
			rigidBody.angularVelocity = state.AngularVelocity;
		}
	}

	// Token: 0x040009D2 RID: 2514
	private const float DEFAULT_TRAVEL_TIME = 7f;

	// Token: 0x040009D3 RID: 2515
	[SerializeField]
	[Tooltip("The amount of time it takes to travel across the escalator in seconds.")]
	private float _travelTime = 7f;

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	[Tooltip("Should the escalator move downwards?")]
	private bool _goesDown;

	// Token: 0x040009D5 RID: 2517
	private float _length;

	// Token: 0x040009D6 RID: 2518
	private readonly Dictionary<Rigidbody, ElevatorSimple.RiderState> _riders = new Dictionary<Rigidbody, ElevatorSimple.RiderState>();

	// Token: 0x040009D7 RID: 2519
	private readonly List<Rigidbody> _staleRiders = new List<Rigidbody>();

	// Token: 0x040009D8 RID: 2520
	private bool _simulationPaused;

	// Token: 0x02000151 RID: 337
	private sealed class RiderState
	{
		// Token: 0x040009D9 RID: 2521
		public int ContactCount;

		// Token: 0x040009DA RID: 2522
		public bool WasKinematic;

		// Token: 0x040009DB RID: 2523
		public Vector3 Velocity;

		// Token: 0x040009DC RID: 2524
		public Vector3 AngularVelocity;
	}
}
