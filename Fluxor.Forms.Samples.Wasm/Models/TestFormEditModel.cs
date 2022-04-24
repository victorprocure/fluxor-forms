using System;
using System.ComponentModel.DataAnnotations;

namespace Fluxor.Forms.Samples.Wasm.Models
{
    public class TestFormEditModel
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? FavoriteColour { get; set; }

        public Guid ModelId { get; set; } = Guid.NewGuid();
    }
}