using HSDraft.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HSDraft.Models.Draft
{
    public class DraftIndex
    {
        public List<Draft> Draft { get; set; }

        [Required]
        [Display(Name = "Players count")]
        [RegularExpression("^[2-8]{1}$", ErrorMessage = "Player count must be 2-8")]
        public string PlayerCount { get; set; }
    }
}