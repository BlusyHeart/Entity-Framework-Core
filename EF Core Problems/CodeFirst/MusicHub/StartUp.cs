namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            int duration = int.Parse(Console.ReadLine());

            string result = ExportSongsAboveDuration(context, duration);

            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Albums.Where(a => a.ProducerId.Value == producerId)
                .Include(a => a.Producer)
                .Include(a => a.Songs)
                .ThenInclude(s => s.Writer)
                .ToArray()
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    AllSongs = a.Songs.Select(s => new
                    {
                        s.Name,
                        s.Price,
                        Writer = s.Writer.Name
                    })
                        .OrderByDescending(s => s.Name)
                        .ThenBy(s => s.Writer)
                        .ToArray(),
                    TotalPrice = a.Price
                })                  
                .ToArray();

            StringBuilder output = new StringBuilder();
           
            foreach (var a in albums)
            {
                output.AppendLine($"-AlbumName: {a.Name}");
                output.AppendLine($"-ReleaseDate: {a.ReleaseDate}");
                output.AppendLine($"-ProducerName: {a.ProducerName}");
                output.AppendLine("-Songs:");

                int index = 0;
                foreach (var s in a.AllSongs)
                {                    
                    output.AppendLine($"#{++index}");
                    output.AppendLine($"---SongName: {s.Name}");
                    output.AppendLine($"---Price: {s.Price:f2}");
                    output.AppendLine($"---Writer: {s.Writer}");                    
                }

                output.AppendLine($"-AlbumPrice: {a.TotalPrice:f2}");
            }

            return output.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context
                .Songs
                .Include(s => s.Performers)
                .ThenInclude(sp => sp.Performer) 
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .ThenInclude(a => a.Producer)                
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    WriterName = s.Writer.Name,
                    Performer = s.Performers
                    .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}").FirstOrDefault(),
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)                
                .ThenBy(s => s.Performer)
                .ToArray();

            StringBuilder output = new StringBuilder();

            int index = 0;
            foreach (var s in songs)
            {
                output.AppendLine($"-Song #{++index}");
                output.AppendLine($"---SongName: {s.Name}");
                output.AppendLine($"---Writer: {s.WriterName}");
                output.AppendLine($"---Performer: {s.Performer}");
                output.AppendLine($"---AlbumProducer: {s.AlbumProducer}");
                output.AppendLine($"---Duration: {s.Duration}");
            }

            return output.ToString().TrimEnd();
        }
    }
}
