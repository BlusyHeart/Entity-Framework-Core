namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Security.Authentication.ExtendedProtection;
    using System.Text;
    using System.Xml;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(db);    

            Stopwatch sp = new Stopwatch();
            sp.Start();
            int countRemoved = RemoveBooks(context);
            Console.WriteLine(countRemoved);

            Console.WriteLine(sp.ElapsedMilliseconds);
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context
                .Books.Where(b => b.Copies < 5000)
                .Delete();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            int tagetYear = 2010;

            var books = context
                .Books.Where(b => b.ReleaseDate.Value.Year < tagetYear)
                .Update(b => new Book() { Price = b.Price + 5 });
        }
            
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context
                .Categories
                .OrderBy(c => c.Name)
                .Take(3)
                .Select(c => new
                {
                    c.Name,
                    AllTitles = c.CategoryBooks.Select(cb => new
                    {
                        cb.Book.Title,
                        cb.Book.ReleaseDate

                    })
                    .OrderByDescending(cb => cb.ReleaseDate)
                    .ToArray()

                })                              
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var c in categories)
            {
                sb.AppendLine($"--{c.Name}");
                sb.AppendLine(string.Join(Environment.NewLine, c.AllTitles.Select(t => $"{t.Title} ({t.ReleaseDate})")));
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var category = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var c in category)
            {
                sb.AppendLine($"{c.Name} ${c.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }   

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var books = context
                .Authors.Select(a => new
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.TotalCopies)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.AuthorName} - {b.TotalCopies}"));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context
                .Books.Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.Title)
                .Count();

            return books;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context
                .Books.Where(b => EF.Functions.Like(b.Author.LastName, $"{input}%"))
                .Select(b => new { b.Title, AuthorName = $"{b.Author.FirstName} {b.Author.LastName}", b.BookId })
                .OrderBy(b => b.BookId)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.AuthorName})"));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context
                .Books.Where(b => EF.Functions.Like(b.Title, $"%{input}%"))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context
                .Authors.Where(a => EF.Functions.Like(a.FirstName, $"%{input}"))
                .Select(a => new { Output = $"{a.FirstName} {a.LastName}" })
                .ToArray();

            return string.Join(Environment.NewLine, authors.Select(a => a.Output));

        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime datetime;
            string dateFormat = "dd-MM-yyyy";

            datetime = DateTime.ParseExact(date, dateFormat,
               CultureInfo.InvariantCulture);

            var titles = context
                .Books.Where(x => x.ReleaseDate < datetime)
                .Select(b => new
                {
                    b.Title,
                    EditionType = b.EditionType.ToString(),
                    b.Price,
                    ReleaseDate = b.ReleaseDate.Value

                }).OrderByDescending(b => b.ReleaseDate)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var t in titles)
            {
                sb.AppendLine($"{t.Title} - {t.EditionType} - {t.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ');

            var titles = context
                .Books.Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name)))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context
                .Books.Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context
                .Books.Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - {b.Price}");
            }

            return sb.ToString().TrimEnd();
        }


        public static string GetGoldenBooks(BookShopContext context)
        {
            /*bool parseSuccess = Enum.TryParse<EditionType>("Gold", out EditionType goldType);

            if (!parseSuccess)
            {
                return string.Empty;
            }*/

            var goldenTitles = context
                .Books.Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, goldenTitles);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string input)
        {
            bool parseSuccess = Enum.TryParse<AgeRestriction>(input, true, out AgeRestriction ageRestriction);

            if (!parseSuccess)
            {
                return String.Empty;
            }

            var titles = context
                .Books.Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, titles);
        }
    }
}
