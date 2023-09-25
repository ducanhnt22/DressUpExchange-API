using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public  class FeedbackRequest
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        [Range(1,10)]
        public int Rating { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Comment { get; set; }
    }
}
