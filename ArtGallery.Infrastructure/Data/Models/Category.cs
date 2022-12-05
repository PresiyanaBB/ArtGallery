using ArtGallery.Infrastructure.Data.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Infrastructure.Data.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(ValidationConstants.CategoryNameMaxLength)]
        public string? Name { get; set; }
        public IList<Painting> Painting { get; set; } = new List<Painting>();
    }
}
