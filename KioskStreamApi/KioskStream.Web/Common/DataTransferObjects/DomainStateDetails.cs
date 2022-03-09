using System;
using System.ComponentModel.DataAnnotations;

namespace KioskStream.Web.Common.DataTransferObjects
{
    public class DomainStateDetails
    {
        [Required]
        [Range(minimum: 1, maximum: 5, ErrorMessage = "Id of Domain State is incorrect.")]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 7, ErrorMessage = "Description of Domain State is incorrect.")]
        public string Description { get; set; }
    }
}
