#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Android.Runtime;

/// <summary>
/// Abstraction over the typemap dictionary that handles alias resolution.
/// Both Debug (per-assembly universes) and Release (single merged universe)
/// go through this interface, so <see cref="TrimmableTypeMap"/> doesn't
/// need to know about aliasing mechanics.
/// </summary>
interface ITypeMapWithAliasing
{
	/// <summary>
	/// Returns all types mapped to a JNI name, resolving alias holders.
	/// For non-alias entries this yields a single type. For alias groups
	/// it follows each alias key and yields the surviving target types.
	/// </summary>
	IEnumerable<Type> GetTypes (string jniName);

	/// <summary>
	/// Resolves a managed type to its proxy type (the generated type that
	/// carries the <see cref="JavaPeerProxy"/> attribute).
	/// </summary>
	bool TryGetProxyType (Type managedType, [NotNullWhen (true)] out Type? proxyType);

	/// <summary>
	/// Raw lookup of a managed type by JNI name, without the
	/// <see cref="JavaPeerProxy"/> attribute filtering applied by
	/// <see cref="GetTypes"/>. Used by the AOT-safe array creation path,
	/// where the typemap entry points directly at a closed array
	/// <see cref="Type"/> (e.g. <c>typeof(byte[][])</c>) that has no
	/// proxy attribute.
	/// </summary>
	bool TryGetType (string jniName, [NotNullWhen (true)] out Type? type);
}
