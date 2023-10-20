namespace Wallet.Data.Helpers
{
    public class YearTable
    {
        public Season Spring {  get; set; }
        public Season Summer { get; set; }
        public Season Autumn { get; set; }
        public Season Winter { get; set; }

        public YearTable(DateTime date) 
        { 
            Spring = new Season() 
            {
                StartPeriod = new DateTime(date.Year, 3, 1),
                EndPeriod = new DateTime(date.Year, 5, 31), 
            };

            Summer = new Season()
            {
                StartPeriod = new DateTime(date.Year, 6, 1),
                EndPeriod = new DateTime(date.Year, 8, 31),
            };

            Autumn = new Season()
            {
                StartPeriod = new DateTime(date.Year, 9, 1),
                EndPeriod = new DateTime(date.Year, 11, 30),
            };

            Winter = new Season()
            {
                StartPeriod = new DateTime(date.Year + 1, 12, 1),
                EndPeriod = new DateTime(date.Year + 1, 2, 28),
            };
        }

        public Season? CheckDate(DateTime date)
        {
            if(date >= Spring.StartPeriod && date <= Spring.EndPeriod)
            {
                return Spring;
            }
            if (date >= Summer.StartPeriod && date <= Summer.EndPeriod)
            {
                return Summer;
            }
            if (date >= Autumn.StartPeriod && date <= Autumn.EndPeriod)
            {
                return Autumn;
            }
            if (date >= Winter.StartPeriod && date <= Winter.EndPeriod)
            {
                return Winter;
            }

            return null;
        }
    }
}
