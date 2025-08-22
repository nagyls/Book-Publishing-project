using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BookPublishing
{
    public class Book
    {
        public int publishingYear;
        public int publishingQuarter;
        public bool originHUN;
        public string name;
        public int copyNumber;

        public Book(string line)
        {
            string[] parts = line.Split(';');
            publishingYear = int.Parse(parts[0]);
            publishingQuarter = int.Parse(parts[1]);
            originHUN = parts[2] == "ma";
            name = parts[3];
            copyNumber = int.Parse(parts[4]);
        }
        public Book() { }
    }
    internal class Program
    {
        public static List<Book> bookList = new List<Book>();
        static void Main(string[] args)
        {
            ReadFile();
            CheckName();
            CheckCopyNumber();
            CheckCountry();
            StatsTable();
            Console.ReadKey();
        }
        static void ReadFile()
        {
            string[] lines = File.ReadAllLines("kiadas.txt", Encoding.UTF8);
            
            for (int i = 0; i < lines.Length; i++)
            {
                bookList.Add(new Book(lines[i]));
            }
        }
        static void CheckName()
        {
            Console.Write("2. feladat:\r\nSzerző: ");
            string author = Console.ReadLine();
            int count = 0;
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].name.Contains(author))
                {
                    count++;
                }
            }
            
            if (count == 0)
            {
                Console.WriteLine("Nem adtak ki");
            } else
            {
                Console.WriteLine($"{count} könyvkiadás");
            }
        }
        static void CheckCopyNumber()
        {
            Console.WriteLine("3. feladat:");
            int count = 0;
            int occurred = 0;
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].copyNumber > count)
                {
                    count = bookList[i].copyNumber;
                    occurred = 0;
                }
                if (bookList[i].copyNumber == count)
                {
                    occurred++;
                }
            }
            Console.WriteLine($"Legnagyobb példányszám: {count}, előfordult: {occurred} alkalommal");
        }
        static void CheckCountry()
        {
            Console.WriteLine("4. feladat:");
            for (int i = 0; i < bookList.Count; i++)
            {
                Book book = bookList[i];
                if (!book.originHUN && book.copyNumber >= 40000)
                {
                    Console.WriteLine($"{book.publishingYear}/{book.publishingQuarter}. {book.name}");
                }
            }
        }
        static void StatsTable()
        {
            Console.WriteLine("5. feladat:");
            Dictionary<int, BookEdition> bookStats = new Dictionary<int, BookEdition>();

            for (int i = 0; i < bookList.Count; i++)
            {
                Book book = bookList[i];

                if (!bookStats.ContainsKey(book.publishingYear))
                {
                    bookStats.Add(book.publishingYear, new BookEdition(book.publishingYear));
                }
                if (book.originHUN)
                {
                    bookStats[book.publishingYear].huEdition++;
                    bookStats[book.publishingYear].huPublishing += book.copyNumber;
                } else
                {
                    bookStats[book.publishingYear].abroadEdition++;
                    bookStats[book.publishingYear].abroadPublishing += book.copyNumber;
                }
            }
            Console.WriteLine($"{"Év",-8} {"Magyar kiadás",-20} {"Magyar pédányszám",-20} {"Külföldi kiadás",-20} Külföldi példányszám");
            foreach (var ele in bookStats)
            {
                Console.WriteLine($"{ele.Key,-8} {ele.Value.huEdition, +10} {ele.Value.huPublishing, +20} {ele.Value.abroadEdition,+13} {ele.Value.abroadPublishing,+23}");
            }

            string[] strings = new string[bookStats.Count + 3];
            strings[0] = "<table>";
            strings[1] = "<tr><th>Év</th><th>Magyar kiadás</th><th>Magyar példányszám</th><th>Külföldi\r\nkiadás</th><th>Külföldi példányszám</th></tr>";
            int j = 2;
            foreach (var ele in bookStats)
            {
                strings[j] = $"<tr><td>{ele.Key}</td><td>{ele.Value.huEdition}</td><td>{ele.Value.huPublishing}</td><td>{ele.Value.abroadEdition}</td><td>{ele.Value.abroadPublishing}</td></tr> ";
                j++;
            }
            strings[strings.Length - 1] = "</table>";
            File.WriteAllLines("tabla.html", strings, Encoding.UTF8);
        }
    }
}
