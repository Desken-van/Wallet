using Wallet.Data.Helpers;

namespace Wallet.Data.Factory
{
    public interface IStoryPointCalc
    {
        string Calculate(DateTime date, Season season);
    }
}
