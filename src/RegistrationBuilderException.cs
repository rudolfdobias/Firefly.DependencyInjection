using System;

namespace Firefly.DependencyInjection;

public class RegistrationBuilderException : Exception
{
	public RegistrationBuilderException(string message, Exception outer) : base(message, outer)
	{
	}
}