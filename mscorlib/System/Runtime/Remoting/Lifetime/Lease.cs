using System;
using System.Collections;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x02000585 RID: 1413
	internal class Lease : MarshalByRefObject, ILease
	{
		// Token: 0x0600375D RID: 14173 RVA: 0x000C79B4 File Offset: 0x000C5BB4
		public Lease()
		{
			this._currentState = LeaseState.Initial;
			this._initialLeaseTime = LifetimeServices.LeaseTime;
			this._renewOnCallTime = LifetimeServices.RenewOnCallTime;
			this._sponsorshipTimeout = LifetimeServices.SponsorshipTimeout;
			this._leaseExpireTime = DateTime.UtcNow + this._initialLeaseTime;
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x000C7A05 File Offset: 0x000C5C05
		public TimeSpan CurrentLeaseTime
		{
			get
			{
				return this._leaseExpireTime - DateTime.UtcNow;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x000C7A17 File Offset: 0x000C5C17
		public LeaseState CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x000C7A1F File Offset: 0x000C5C1F
		public void Activate()
		{
			this._currentState = LeaseState.Active;
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06003761 RID: 14177 RVA: 0x000C7A28 File Offset: 0x000C5C28
		// (set) Token: 0x06003762 RID: 14178 RVA: 0x000C7A30 File Offset: 0x000C5C30
		public TimeSpan InitialLeaseTime
		{
			get
			{
				return this._initialLeaseTime;
			}
			set
			{
				if (this._currentState != LeaseState.Initial)
				{
					throw new RemotingException("InitialLeaseTime property can only be set when the lease is in initial state; state is " + this._currentState.ToString() + ".");
				}
				this._initialLeaseTime = value;
				this._leaseExpireTime = DateTime.UtcNow + this._initialLeaseTime;
				if (value == TimeSpan.Zero)
				{
					this._currentState = LeaseState.Null;
				}
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06003763 RID: 14179 RVA: 0x000C7A9D File Offset: 0x000C5C9D
		// (set) Token: 0x06003764 RID: 14180 RVA: 0x000C7AA5 File Offset: 0x000C5CA5
		public TimeSpan RenewOnCallTime
		{
			get
			{
				return this._renewOnCallTime;
			}
			set
			{
				if (this._currentState != LeaseState.Initial)
				{
					throw new RemotingException("RenewOnCallTime property can only be set when the lease is in initial state; state is " + this._currentState.ToString() + ".");
				}
				this._renewOnCallTime = value;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06003765 RID: 14181 RVA: 0x000C7ADD File Offset: 0x000C5CDD
		// (set) Token: 0x06003766 RID: 14182 RVA: 0x000C7AE5 File Offset: 0x000C5CE5
		public TimeSpan SponsorshipTimeout
		{
			get
			{
				return this._sponsorshipTimeout;
			}
			set
			{
				if (this._currentState != LeaseState.Initial)
				{
					throw new RemotingException("SponsorshipTimeout property can only be set when the lease is in initial state; state is " + this._currentState.ToString() + ".");
				}
				this._sponsorshipTimeout = value;
			}
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x000C7B1D File Offset: 0x000C5D1D
		public void Register(ISponsor obj)
		{
			this.Register(obj, TimeSpan.Zero);
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x000C7B2C File Offset: 0x000C5D2C
		public void Register(ISponsor obj, TimeSpan renewalTime)
		{
			lock (this)
			{
				if (this._sponsors == null)
				{
					this._sponsors = new ArrayList();
				}
				this._sponsors.Add(obj);
			}
			if (renewalTime != TimeSpan.Zero)
			{
				this.Renew(renewalTime);
			}
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x000C7B98 File Offset: 0x000C5D98
		public TimeSpan Renew(TimeSpan renewalTime)
		{
			DateTime dateTime = DateTime.UtcNow + renewalTime;
			if (dateTime > this._leaseExpireTime)
			{
				this._leaseExpireTime = dateTime;
			}
			return this.CurrentLeaseTime;
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000C7BCC File Offset: 0x000C5DCC
		public void Unregister(ISponsor obj)
		{
			lock (this)
			{
				if (this._sponsors != null)
				{
					for (int i = 0; i < this._sponsors.Count; i++)
					{
						if (this._sponsors[i] == obj)
						{
							this._sponsors.RemoveAt(i);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000C7C40 File Offset: 0x000C5E40
		internal void UpdateState()
		{
			if (this._currentState != LeaseState.Active)
			{
				return;
			}
			if (this.CurrentLeaseTime > TimeSpan.Zero)
			{
				return;
			}
			if (this._sponsors != null)
			{
				this._currentState = LeaseState.Renewing;
				lock (this)
				{
					this._renewingSponsors = new Queue(this._sponsors);
				}
				this.CheckNextSponsor();
				return;
			}
			this._currentState = LeaseState.Expired;
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x000C7CC0 File Offset: 0x000C5EC0
		private void CheckNextSponsor()
		{
			if (this._renewingSponsors.Count == 0)
			{
				this._currentState = LeaseState.Expired;
				this._renewingSponsors = null;
				return;
			}
			ISponsor sponsor = (ISponsor)this._renewingSponsors.Peek();
			this._renewalDelegate = new Lease.RenewalDelegate(sponsor.Renewal);
			IAsyncResult asyncResult = this._renewalDelegate.BeginInvoke(this, null, null);
			ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, new WaitOrTimerCallback(this.ProcessSponsorResponse), asyncResult, this._sponsorshipTimeout, true);
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x000C7D3C File Offset: 0x000C5F3C
		private void ProcessSponsorResponse(object state, bool timedOut)
		{
			if (!timedOut)
			{
				try
				{
					IAsyncResult asyncResult = (IAsyncResult)state;
					TimeSpan timeSpan = this._renewalDelegate.EndInvoke(asyncResult);
					if (timeSpan != TimeSpan.Zero)
					{
						this.Renew(timeSpan);
						this._currentState = LeaseState.Active;
						this._renewingSponsors = null;
						return;
					}
				}
				catch
				{
				}
			}
			this.Unregister((ISponsor)this._renewingSponsors.Dequeue());
			this.CheckNextSponsor();
		}

		// Token: 0x04002589 RID: 9609
		private DateTime _leaseExpireTime;

		// Token: 0x0400258A RID: 9610
		private LeaseState _currentState;

		// Token: 0x0400258B RID: 9611
		private TimeSpan _initialLeaseTime;

		// Token: 0x0400258C RID: 9612
		private TimeSpan _renewOnCallTime;

		// Token: 0x0400258D RID: 9613
		private TimeSpan _sponsorshipTimeout;

		// Token: 0x0400258E RID: 9614
		private ArrayList _sponsors;

		// Token: 0x0400258F RID: 9615
		private Queue _renewingSponsors;

		// Token: 0x04002590 RID: 9616
		private Lease.RenewalDelegate _renewalDelegate;

		// Token: 0x02000586 RID: 1414
		// (Invoke) Token: 0x0600376F RID: 14191
		private delegate TimeSpan RenewalDelegate(ILease lease);
	}
}
