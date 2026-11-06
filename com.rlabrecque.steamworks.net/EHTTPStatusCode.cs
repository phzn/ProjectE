using System;

namespace Steamworks
{
	// Token: 0x02000152 RID: 338
	public enum EHTTPStatusCode
	{
		// Token: 0x0400098D RID: 2445
		k_EHTTPStatusCodeInvalid,
		// Token: 0x0400098E RID: 2446
		k_EHTTPStatusCode100Continue = 100,
		// Token: 0x0400098F RID: 2447
		k_EHTTPStatusCode101SwitchingProtocols,
		// Token: 0x04000990 RID: 2448
		k_EHTTPStatusCode200OK = 200,
		// Token: 0x04000991 RID: 2449
		k_EHTTPStatusCode201Created,
		// Token: 0x04000992 RID: 2450
		k_EHTTPStatusCode202Accepted,
		// Token: 0x04000993 RID: 2451
		k_EHTTPStatusCode203NonAuthoritative,
		// Token: 0x04000994 RID: 2452
		k_EHTTPStatusCode204NoContent,
		// Token: 0x04000995 RID: 2453
		k_EHTTPStatusCode205ResetContent,
		// Token: 0x04000996 RID: 2454
		k_EHTTPStatusCode206PartialContent,
		// Token: 0x04000997 RID: 2455
		k_EHTTPStatusCode300MultipleChoices = 300,
		// Token: 0x04000998 RID: 2456
		k_EHTTPStatusCode301MovedPermanently,
		// Token: 0x04000999 RID: 2457
		k_EHTTPStatusCode302Found,
		// Token: 0x0400099A RID: 2458
		k_EHTTPStatusCode303SeeOther,
		// Token: 0x0400099B RID: 2459
		k_EHTTPStatusCode304NotModified,
		// Token: 0x0400099C RID: 2460
		k_EHTTPStatusCode305UseProxy,
		// Token: 0x0400099D RID: 2461
		k_EHTTPStatusCode307TemporaryRedirect = 307,
		// Token: 0x0400099E RID: 2462
		k_EHTTPStatusCode308PermanentRedirect,
		// Token: 0x0400099F RID: 2463
		k_EHTTPStatusCode400BadRequest = 400,
		// Token: 0x040009A0 RID: 2464
		k_EHTTPStatusCode401Unauthorized,
		// Token: 0x040009A1 RID: 2465
		k_EHTTPStatusCode402PaymentRequired,
		// Token: 0x040009A2 RID: 2466
		k_EHTTPStatusCode403Forbidden,
		// Token: 0x040009A3 RID: 2467
		k_EHTTPStatusCode404NotFound,
		// Token: 0x040009A4 RID: 2468
		k_EHTTPStatusCode405MethodNotAllowed,
		// Token: 0x040009A5 RID: 2469
		k_EHTTPStatusCode406NotAcceptable,
		// Token: 0x040009A6 RID: 2470
		k_EHTTPStatusCode407ProxyAuthRequired,
		// Token: 0x040009A7 RID: 2471
		k_EHTTPStatusCode408RequestTimeout,
		// Token: 0x040009A8 RID: 2472
		k_EHTTPStatusCode409Conflict,
		// Token: 0x040009A9 RID: 2473
		k_EHTTPStatusCode410Gone,
		// Token: 0x040009AA RID: 2474
		k_EHTTPStatusCode411LengthRequired,
		// Token: 0x040009AB RID: 2475
		k_EHTTPStatusCode412PreconditionFailed,
		// Token: 0x040009AC RID: 2476
		k_EHTTPStatusCode413RequestEntityTooLarge,
		// Token: 0x040009AD RID: 2477
		k_EHTTPStatusCode414RequestURITooLong,
		// Token: 0x040009AE RID: 2478
		k_EHTTPStatusCode415UnsupportedMediaType,
		// Token: 0x040009AF RID: 2479
		k_EHTTPStatusCode416RequestedRangeNotSatisfiable,
		// Token: 0x040009B0 RID: 2480
		k_EHTTPStatusCode417ExpectationFailed,
		// Token: 0x040009B1 RID: 2481
		k_EHTTPStatusCode4xxUnknown,
		// Token: 0x040009B2 RID: 2482
		k_EHTTPStatusCode421MisdirectedRequest = 421,
		// Token: 0x040009B3 RID: 2483
		k_EHTTPStatusCode422UnprocessableContent,
		// Token: 0x040009B4 RID: 2484
		k_EHTTPStatusCode423Locked,
		// Token: 0x040009B5 RID: 2485
		k_EHTTPStatusCode424FailedDependency,
		// Token: 0x040009B6 RID: 2486
		k_EHTTPStatusCode425TooEarly,
		// Token: 0x040009B7 RID: 2487
		k_EHTTPStatusCode426UpgradeRequired,
		// Token: 0x040009B8 RID: 2488
		k_EHTTPStatusCode428PreconditionRequired = 428,
		// Token: 0x040009B9 RID: 2489
		k_EHTTPStatusCode429TooManyRequests,
		// Token: 0x040009BA RID: 2490
		k_EHTTPStatusCode431RequestHeaderFieldsTooLarge = 431,
		// Token: 0x040009BB RID: 2491
		k_EHTTPStatusCode444ConnectionClosed = 444,
		// Token: 0x040009BC RID: 2492
		k_EHTTPStatusCode451UnavailableForLegalReasons = 451,
		// Token: 0x040009BD RID: 2493
		k_EHTTPStatusCode500InternalServerError = 500,
		// Token: 0x040009BE RID: 2494
		k_EHTTPStatusCode501NotImplemented,
		// Token: 0x040009BF RID: 2495
		k_EHTTPStatusCode502BadGateway,
		// Token: 0x040009C0 RID: 2496
		k_EHTTPStatusCode503ServiceUnavailable,
		// Token: 0x040009C1 RID: 2497
		k_EHTTPStatusCode504GatewayTimeout,
		// Token: 0x040009C2 RID: 2498
		k_EHTTPStatusCode505HTTPVersionNotSupported,
		// Token: 0x040009C3 RID: 2499
		k_EHTTPStatusCode506VariantAlsoNegotiates,
		// Token: 0x040009C4 RID: 2500
		k_EHTTPStatusCode507InsufficientStorage,
		// Token: 0x040009C5 RID: 2501
		k_EHTTPStatusCode508LoopDetected,
		// Token: 0x040009C6 RID: 2502
		k_EHTTPStatusCode510NotExtended = 510,
		// Token: 0x040009C7 RID: 2503
		k_EHTTPStatusCode511NetworkAuthenticationRequired,
		// Token: 0x040009C8 RID: 2504
		k_EHTTPStatusCode5xxUnknown = 599
	}
}
