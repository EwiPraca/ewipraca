using EwiPraca.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwiPraca.Importers.Importers
{
    public static class EmployeeImportRowValidator
    {
        public static bool IsValid(this EmployeeImportRow row)
        {
            var properties = row.GetType().GetProperties().ToList();

            foreach(var property in properties.Where(x => x.Name != "PlaceNumber"))
            {
                var val = row.GetType().GetProperty(property.Name).GetValue(row, null);

                if(val == null || string.IsNullOrEmpty(val.ToString()))
                {
                    return false;
                }

                if(property.Name == "BirthDay")
                {
                    DateTime date;

                    if(!DateTime.TryParse(val.ToString(), out date))
                    {
                        return false;
                    }
                }

                if (property.Name == "PESEL")
                {
                    if (!val.ToString().IsValidPESEL())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
