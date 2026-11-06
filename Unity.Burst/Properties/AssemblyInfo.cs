using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Unity.Burst;
using Unity.Burst.Intrinsics;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: InternalsVisibleTo("Unity.Burst.CodeGen")]
[assembly: InternalsVisibleTo("Unity.Burst.Editor")]
[assembly: InternalsVisibleTo("Unity.Burst.Tests.UnitTests")]
[assembly: InternalsVisibleTo("Unity.Burst.Editor.Tests")]
[assembly: InternalsVisibleTo("Unity.Burst.Benchmarks")]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(BurstCompiler.BurstCompilerHelper.IsBurstEnabled_00000145$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(X86.DoSetCSRTrampoline_00000129$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(X86.DoGetCSRTrampoline_0000012A$BurstDirectCall))]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
