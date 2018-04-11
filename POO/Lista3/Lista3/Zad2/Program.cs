using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    public class ReportPrinter
    {
        string document;
        public string GetData()
        {
            document = "Jakiś dokument";
            return document;
        }
        public void FormatDocument()
        {
            document = document + " sformatowany ";
        }
        public void PrintReport()
        {
            Console.WriteLine(document);
        }
    }

    public class ReportPrinter2 : IReportPrinter
    {
        public void PrintReport(string document)
        {
            Console.WriteLine(document);
        }
    }

    public class ReportRepository2 : IReportRepository
    {
        public string GetData()
        {
            return "Jakiś dokument";
        }
    }

    public class ReportFormatter2 : IReportFormatter
    {
        public string FormatDocument(string document)
        {
            return document + " sformatowany ";
        }
    }

    public class ReportPrinter3
    {
        ReportFormatter2 rf = new ReportFormatter2();
        ReportRepository2 rr = new ReportRepository2();

        public void PrintReport()
        {
            string document = rr.GetData();
            document = rf.FormatDocument(document);
            Console.WriteLine(document);
        }
    }



    public class ReportComposer
    {
        IReportRepository _repository;
        IReportPrinter _printer;
        IReportFormatter _formatter;

        public ReportComposer(IReportRepository repo, IReportPrinter printer, IReportFormatter formatter)
        {
            _formatter = formatter;
            _printer = printer;
            _repository = repo;
        }

        public void PrintReport()
        {
            _printer.PrintReport(_formatter.FormatDocument(_repository.GetData()));
        }
    }

    public interface IReportFormatter
    {
        string FormatDocument(string document);
    }

    public interface IReportPrinter
    {
        void PrintReport(string document);
    }

    public interface IReportRepository
    {
        string GetData();
    }

    class Program
    {
        static void Main(string[] args)
        {
            //1
            ReportPrinter rp1 = new ReportPrinter();
            rp1.GetData();
            rp1.FormatDocument();
            rp1.PrintReport();

            ReportPrinter2 rp2 = new ReportPrinter2();
            ReportFormatter2 rf2 = new ReportFormatter2();
            ReportRepository2 rr2 = new ReportRepository2();
            rp2.PrintReport(rf2.FormatDocument(rr2.GetData()));

            ReportPrinter3 rp3 = new ReportPrinter3();
            rp3.PrintReport();

            ReportComposer rc = new ReportComposer(rr2, rp2, rf2);
            rc.PrintReport();
            Console.Read();


        }
    }
}
