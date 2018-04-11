using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    public class TaxCalculator
    {
        public Decimal CalculateTax(Decimal Price) { return Price * 0.22m; }
    }
    public class Item
    {
        public Item(string name, Decimal price)
        {
            _name = name;
            _price = price;
        }

        Decimal _price;
        string _name;
        public Decimal Price { get { return _price; } }
        public string Name { get { return _name; } }
    }
    public class CashRegister
    {
        public TaxCalculator taxCalc = new TaxCalculator();
        public Decimal CalculatePrice(Item[] Items)
        {
            Decimal _price = 0;
            foreach (Item item in Items)
            {
            _price += item.Price + taxCalc.CalculateTax(item.Price);
            }
            return _price;
        }
        public void PrintBill(Item[] Items)
        {
            foreach (var item in Items)
                Console.WriteLine("towar {0} : cena {1} + podatek {2}",
                item.Name, item.Price, taxCalc.CalculateTax(item.Price));
        }
    }



    public interface ITaxCalculator
    {
        Decimal CalculateTax(Decimal Price);
    }

    public interface IBillPrinter
    {
        void PrintBill(Item[] Items, ITaxCalculator TaxCalculator);
    }

    public class CashRegister2
    {
        ITaxCalculator _taxCalculator;
        IBillPrinter _printer;
        public CashRegister2(ITaxCalculator calc, IBillPrinter printer )
        {
            _taxCalculator = calc;
            _printer = printer;
        }

        public Decimal CalculatePrice(Item[] Items)
        {
            Decimal _price = 0;
            foreach (Item item in Items)
            {
                _price += item.Price + _taxCalculator.CalculateTax(item.Price);
            }
            return _price;
        }
        public void PrintBill(Item[] Items)
        {
            _printer.PrintBill(Items, _taxCalculator);
        }
    }

    class SortedByPriceBillPrinter : IBillPrinter
    {
        public void PrintBill(Item[] Items, ITaxCalculator _taxCalculator)
        {
            foreach (var item in Items.OrderBy(x => x.Price ))
                Console.WriteLine("towar {0} : cena {1} + podatek {2}",
                item.Name, item.Price, _taxCalculator.CalculateTax(item.Price));
        }
    }

    class BillPrinter : IBillPrinter
    {
        public void PrintBill(Item[] Items, ITaxCalculator _taxCalculator)
        {
            foreach (var item in Items)
                Console.WriteLine("towar {0} : cena {1} + podatek {2}",
                item.Name, item.Price, _taxCalculator.CalculateTax(item.Price));
        }
    }

    class VATBillPrinter : IBillPrinter
    {
        public void PrintBill(Item[] Items, ITaxCalculator _taxCalculator)
        {
            Console.WriteLine("Faktura VAT nr 63556");
            foreach (var item in Items)
                Console.WriteLine("towar {0} : cena {1} + podatek {2}",
                item.Name, item.Price, _taxCalculator.CalculateTax(item.Price));
        }
    }

    class TaxCalculator2 : ITaxCalculator
    {
        public Decimal CalculateTax(Decimal Price) { return Price * 0.22m; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Item item1 = new Item("mleko", 2.50m);
            Item item2 = new Item("chleb", 3.50m);
            Item item3 = new Item("masło", 5.50m);
            Item item4 = new Item("woda", 1.50m);

            Item[] items = new Item[]
            {
                item1,
                item2,
                item3,
                item4
            };
            

            CashRegister cr1 = new CashRegister();
            Console.WriteLine(string.Format("Wersja pierwsza, cena produktów: {0}", cr1.CalculatePrice(items)));
            cr1.PrintBill(items);

            TaxCalculator2 tc2 = new TaxCalculator2();
            BillPrinter bp2 = new BillPrinter();
            SortedByPriceBillPrinter sortedbp2 = new SortedByPriceBillPrinter();
            VATBillPrinter vatbp2 = new VATBillPrinter();

            CashRegister2 cr2 = new CashRegister2(tc2, bp2);
            Console.WriteLine(string.Format("Wersja druga, cena produktów: {0}", cr2.CalculatePrice(items)));
            cr2.PrintBill(items);

            Console.WriteLine("Rachunek wg. ceny:");
            cr2 = new CashRegister2(tc2, sortedbp2);
            cr2.PrintBill(items);

            Console.WriteLine("Rachunek VAT:");
            cr2 = new CashRegister2(tc2, vatbp2);
            cr2.PrintBill(items);

            Console.Read();
        }
    }
}
