using MusicHub.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Producer
    {
        public Producer()
        {
            Albums = new HashSet<Album>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ProducerNameMaxLength)]
        public string Name { get; set; }

        public string Pseudonym  { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
