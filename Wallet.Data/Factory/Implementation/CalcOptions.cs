using Wallet.Data.Helpers;

namespace Wallet.Data.Factory.Implementation
{
    public class FirstOption : IStoryPointCalc
    {
        public string Calculate(DateTime date, Season season)
        {
            return "2";
        }
    }
    public class SecondOption : IStoryPointCalc
    {
        public string Calculate(DateTime date, Season season)
        {
            return "3";
        }
    }
    public class DefaultOption : IStoryPointCalc
    {
        public string Calculate(DateTime date, Season season)
        {
            var totalCount = 0;

            if(date.Month == season.StartPeriod.Month)
            {
                totalCount = date.Day;
            }

            if (date.Month == season.StartPeriod.Month + 1)
            {
                totalCount = date.Day + 30;
            }

            if (date.Month == season.StartPeriod.Month + 2)
            {
                totalCount = date.Day + 60;
            }

            var mas = new int[totalCount];

            mas[0] = 2;
            mas[1] = 3;

            for (int i = 2; i < mas.Length; i++)
            {
                var sixty = (double)(60 * mas[i - 1] / 100);
                mas[i] = mas[i - 2] + (int)Math.Round(sixty);
            }

            var result = 0;

            foreach (var item in mas)
            {
                result += item;
            }

            if(result > 1000)
            {
                return $"{(int)Math.Round((double)(result / 1000))}K";
            }
            else
            {
                return result.ToString();
            }
        }
    }
}
