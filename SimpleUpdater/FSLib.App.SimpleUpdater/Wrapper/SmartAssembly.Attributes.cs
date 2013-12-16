using System;

namespace SimpleUpdater.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Struct)]
	public sealed class DoNotCaptureVariablesAttribute : Attribute
	{
	}

	[DoNotPrune]
	[DoNotObfuscate]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public sealed class DoNotCaptureFieldsAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Module | AttributeTargets.Struct)]
	public sealed class DoNotObfuscateAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct)]
	public sealed class DoNotObfuscateTypeAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Module | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Struct)]
	public sealed class DoNotPruneAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct)]
	public sealed class DoNotPruneTypeAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DoNotSealTypeAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ReportExceptionAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Struct)]
	public sealed class ObfuscateControlFlowAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Struct)]
	public sealed class DoNotObfuscateControlFlowAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Struct)]
	public sealed class ObfuscateToAttribute : Attribute
	{
		public ObfuscateToAttribute(string newName)
		{
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct)]
	public sealed class ObfuscateNamespaceToAttribute : Attribute
	{
		public ObfuscateNamespaceToAttribute(string newName)
		{
		}
	}

	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Module | AttributeTargets.Struct)]
	public sealed class DoNotEncodeStringsAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Struct)]
	public sealed class EncodeStringsAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Module | AttributeTargets.Struct)]
	public sealed class ExcludeFromMemberRefsProxyAttribute : Attribute
	{
	}
}
