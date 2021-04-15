using System;
using System.Collections.Generic;
using Validation;
using Bogus;
using System.Linq;
using ServiceStack.Text;
using ServiceStack;

namespace task4_1
{
    class Program
    {
        public class Person
        {
            public string FullName { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
        }
        static void Main(string[] args)
        {
            var incomingParametersСorrectnessConditions = new List<ValidationCondition>()
            {
                new ValidationCondition(args.Length != 0, "Error: You entered no one parameter. Please, enter two parameters - number of records (int) and region (string)."),
                new ValidationCondition(args.Length == 2, "Error: You entered more or less than two parameters. Please, enter two parameters - number of records (int) and region (string)."),
            };
            if (ValidationCondition.AreValidated(incomingParametersСorrectnessConditions))
            {
                int numberOfRecords = int.Parse(args[0]);
                string region = args[1];
                var regions = new List<string>() { "en_US", "ru_RU", "uk_UK" };
                var receivedDataСorrectnessConditions = new List<ValidationCondition>()
                {
                new ValidationCondition(numberOfRecords > 0, "Error: You entered negative number of records. Number of records must be positive."),
                new ValidationCondition(regions.Contains(region), "Error: Unidentified region. Please, enter region in format an example 'en_US'. Avialable regions:\n1. en_US\n2. ru_RU\n3. uk_UK"),
                };
                if (ValidationCondition.AreValidated(receivedDataСorrectnessConditions))
                {
                    SetCSVFormatting();
                    string locale = region.Split("_")[0];
                    GetFakePersons(numberOfRecords, locale);
                }
                else ValidationCondition.DisplayValidationErrors(receivedDataСorrectnessConditions);
            }
            else ValidationCondition.DisplayValidationErrors(incomingParametersСorrectnessConditions);
        }
        private static Faker<Person> setGeneratingRules(string locale)
        {
            return new Faker<Person>(locale)
                .RuleFor(x => x.FullName, f => f.Name.FullName())
                .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber());
        }
        public static void SetCSVFormatting()
        {
            CsvConfig.ItemSeperatorString = ";";
            CsvConfig<Person>.OmitHeaders = true;
        }

        public static void GetFakePersons(int personsAmount, string locale)
        {
            var generator = new Faker<Person>();
            generator = setGeneratingRules(locale);
            for (int i = 0; i < personsAmount; i++)
            {
                generator = i % 2 == 0 ? generator.RuleFor(x => x.Address, f => f.Address.FullAddress()) : generator.RuleFor(x => x.Address, f => f.Address.StreetAddress());
                Console.Write(generator.Generate().ToCsv());
            }
        }
    }
}
