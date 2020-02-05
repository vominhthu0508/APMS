using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XT.Model;
using XT.BusinessService;
using XT.Utilities;

namespace XT.Web.Models
{
    public class QuarterModel
    {
        public string Name { get; set; }

        public DateTime First_Date { get; set; }

        public DateTime Last_Date { get; set; }

        public static IEnumerable<QuarterModel> GetQuarters(int year)
        {
            var lstItems = new List<QuarterModel>();

            for (int i = 1; i <= 4; i++)
            {
                var month = i * 3 - 2;
                var firstDayOfQuarter = new DateTime(year, month, 1);
                var lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);

                lstItems.Add(new QuarterModel
                {
                    Name = "Quý " + i + "/" + year,
                    First_Date = firstDayOfQuarter,
                    Last_Date = lastDayOfQuarter
                });
            }

            return lstItems;
        }

        public static IEnumerable<QuarterModel> GetQuarters()
        {
            var lstItems = new List<QuarterModel>();

            var today = DateTime.Today;
            var thisYear = today.Year;
            var lastYear = today.AddYears(-1);

            lstItems.AddRange(GetQuarters(thisYear));
            lstItems.Add(new QuarterModel
            {
                Name = "Cả năm " + thisYear,
                First_Date = today.StartOfYear(),
                Last_Date = today//.EndOfYear()
            });
            lstItems.AddRange(GetQuarters(lastYear.Year));
            lstItems.Add(new QuarterModel
            {
                Name = "Cả năm " + lastYear.Year,
                First_Date = lastYear.StartOfYear(),
                Last_Date = lastYear.EndOfYear()
            });

            return lstItems;
        }
    }
}