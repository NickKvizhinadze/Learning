using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace Dometrain.EFCore.API.Data.ValueConverters
{
    public class DateTimeToChar8Converter : ValueConverter<DateTime, string>
    {
        public DateTimeToChar8Converter(): base(
            dateTimeValue => dateTimeValue.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
            stringValue => DateTime.ParseExact(stringValue, "yyyyMMdd", CultureInfo.InvariantCulture))
        {
        }
    }
}
