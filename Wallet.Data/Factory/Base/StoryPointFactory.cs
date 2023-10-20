using Wallet.Data.Factory.Implementation;
using Wallet.Data.Helpers;

namespace Wallet.Data.Factory.Base
{
    public static class StoryPointFactory
    {
        public static IStoryPointCalc CreateStoryPoint(DateTime date, Season season)
        {
            if(date.ToString("MM/dd/yyyy") == season.StartPeriod.ToString("MM/dd/yyyy"))
            {
                return new FirstOption();
            }
            else if (date.ToString("MM/dd/yyyy") == $"{season.StartPeriod.Month}/02/{season.StartPeriod.Year}")
            {
                return new SecondOption();
            }
            else
            {
                return new DefaultOption();
            }
        }
    }
        
}
