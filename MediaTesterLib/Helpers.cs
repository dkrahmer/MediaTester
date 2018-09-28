using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTesterLib
{
	public class Helpers
	{
		static public void UpdateAverage(ref decimal runningAverage, ref long totalSamples, ref long newValue)
		{
			if (totalSamples > 0)
			{
				runningAverage = ((decimal)totalSamples * runningAverage + (decimal)newValue) / (decimal)(++totalSamples);
			}
			else
			{
				runningAverage = newValue;
				totalSamples++;
			}
		}
	}
}
