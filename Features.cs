using System;

namespace CDForestFull
{
	[Flags]
	public enum Features
	{
		Hash = 1,
		Frequency = 2,
		Distances = 4,
		MedianFilter = 8
	}
}
