using MusicHub.Commons;
using MusicHub.Data.Models.Enums;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Song
    {

        public Song()
        {
            Performers = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxSongNameLength)]
        public string Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
   
        public Genre Genre { get; set; }

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }

        public virtual Album Album { get; set; }

        [ForeignKey(nameof(Writer))]
        [Required]
        public int WriterId { get; set; }
       
        public Writer Writer { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> Performers { get; set; }

    }
}
