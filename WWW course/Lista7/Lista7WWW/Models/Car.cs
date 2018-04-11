using System;
using System.ComponentModel.DataAnnotations;

namespace Lista7WWW.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj numer rejestracyjny samochodu.")]
        [RegularExpression(@"^\D{2,3}\d{4,6}$", ErrorMessage = "Format numeru rejestracyjnego: 2-3 liter, 4-6 cyfr")]
        public string Registration { get; set; }

        [Required(ErrorMessage = "Podaj datę pierwszej rejestracji samochodu.")]
        [DataType(DataType.Date, ErrorMessage = "Niepoprawny format daty")]
        public DateTime FirstRegistration { get; set; }

        [Required(ErrorMessage = "Podaj markę samochodu")]
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Niepoprawna nazwa marki.")]
        public string Automaker { get; set; }

        [Required(ErrorMessage = "Podaj rok produkcji samochodu")]
        [RegularExpression(@"\d{4}", ErrorMessage = "Niepoprawny format roku produkcji.")]
        public int YearOfProduction { get; set; }

        [Required(ErrorMessage = "Podaj rodzaj paliwa samochodu.")]
        [RegularExpression("ON|LPG|EE|P", ErrorMessage = "Niepoprawny format rodzaju paliwa. Wpisz jedno z : ON, LPG, EE, P")]
        public string FuelType { get; set; }
    }
}
