namespace TitanicExplorer.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using TitanicExplorer.Data;
    using System.IO;
    using static TitanicExplorer.Data.Passenger;
    using System.Linq.Expressions;
    using AgileObjects.NetStandardPolyfills;
    using AgileObjects.ReadableExpressions;
    using System.Linq.Dynamic.Core;

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            var sampleDataPath = Path.GetTempFileName();

            System.IO.File.WriteAllText(sampleDataPath, DataFiles.passengers);

            this.Passengers = Passenger.LoadFromFile(sampleDataPath);
        }

        public IEnumerable<Passenger> Passengers
        {
            get; private set;
        }

        public void OnGet()
        {

        }
        public string? query { get; set; }

        public void OnPost()
        {
            var survived = Request.Form["survived"] != "" ? ParseSurvived(Request.Form["survived"]) : null;
            var pClass = ParseNullInt(Request.Form["pClass"]);
            var sex = Request.Form["sex"] != "" ? ParseSex(Request.Form["sex"]) : null;
            var age = ParseNullInt(Request.Form["age"]);
            var minimumFare = ParseNullDecimal(Request.Form["minimumFare"]);
            query = Request.Form["query"];

            this.Passengers = FilterPassengers(survived, pClass, sex, age, minimumFare);
        }

        private IEnumerable<Passenger> FilterPassengers(bool? survived, int? pClass, SexValue? sex, int? age, decimal? minimumFare)
        {
            var parameter = Expression.Parameter(typeof(Passenger));

            if (!string.IsNullOrWhiteSpace(query))
            {
                var expr = DynamicExpressionParser.ParseLambda<Passenger, bool>(new ParsingConfig(), true, query);
                var func = expr.Compile();
                return this.Passengers.Where(func);
            }

            Expression? currentExpression = null;
            if (survived != null)
                currentExpression = CreateExpression(survived.Value, currentExpression, "Survived", parameter);

            if (pClass != null)
                currentExpression = CreateExpression(pClass.Value, currentExpression, "PClass", parameter);

            if (sex != null)
                currentExpression = CreateExpression(sex.Value, currentExpression, "Sex", parameter);

            if (age != null)
                currentExpression = CreateExpression(Convert.ToDecimal(age.Value), currentExpression, "Age", parameter);

            if (minimumFare != null)
                currentExpression = CreateExpression(minimumFare.Value, currentExpression, "Fare", parameter, ">=");

            if (currentExpression != null)
            {
                var lambda = Expression.Lambda<Func<Passenger, bool>>(currentExpression, parameter);
                query = lambda.ToReadableString();
                return this.Passengers.Where(lambda.Compile());
            }

            return this.Passengers;
        }

        private Expression? CreateExpression<T>(T value, Expression? currentExpression, string propertyName, ParameterExpression parameter, string operatorType = "=")
        {
            var constant = Expression.Constant(value);
            var passengerToCall = Expression.Property(parameter, propertyName);

            Expression operatorExpression;
            switch (operatorType)
            {
                case "<":
                    operatorExpression = Expression.LessThan(passengerToCall, constant);
                    break;
                case "<=":
                    operatorExpression = Expression.LessThanOrEqual(passengerToCall, constant);
                    break;
                case ">":
                    operatorExpression = Expression.GreaterThan(passengerToCall, constant);
                    break;
                case ">=":
                    operatorExpression = Expression.GreaterThanOrEqual(passengerToCall, constant);
                    break;
                default:
                    operatorExpression = Expression.Equal(passengerToCall, constant);
                    break;
            }

            return currentExpression != null
                ? Expression.And(currentExpression, operatorExpression)
                : operatorExpression;
        }

        public decimal? ParseNullDecimal(string value)
        {
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }

            return null;
        }

        public int? ParseNullInt(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }

            return null;
        }

        public SexValue? ParseSex(string value)
        {
            return value == "male" ? SexValue.Male : SexValue.Female;
        }

        public bool? ParseSurvived(string value)
        {
            return value == "Survived" ? true : false;
        }
    }
}