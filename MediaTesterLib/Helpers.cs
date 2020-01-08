namespace MediaTesterLib
{
	public class Helpers
	{
		public static void UpdateAverage(ref decimal runningAverage, ref long totalSamples, ref long newValue)
		{
			if (totalSamples > 0)
			{
				runningAverage = ((decimal) totalSamples * runningAverage + (decimal) newValue) / (decimal) (++totalSamples);
			}
			else
			{
				runningAverage = newValue;
				totalSamples++;
			}
		}
	}
}
