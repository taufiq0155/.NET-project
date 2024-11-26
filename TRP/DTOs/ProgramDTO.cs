using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TRP.EF;
using System.Web.Mvc;

namespace TRP.DTOs
{
    public class ProgramDTO
    {
        
        public int ProgramId { get; set; }

        [Required(ErrorMessage = "Program name is required.")]
        [StringLength(150, ErrorMessage = "Program name cannot exceed 150 characters.")]
        public string ProgramName { get; set; }

        [Required(ErrorMessage = "TRP score is required.")]
        [Range(0.0, 10.0, ErrorMessage = "TRP score must be between 0.0 and 10.0.")]
        public decimal TRPScore { get; set; }

        [Required(ErrorMessage = "Channel ID is required.")]
        [ForeignKey("Channel")] // Indicates a foreign key relationship
        public int ChannelId { get; set; }


        [Required(ErrorMessage = "Air time is required.")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$", ErrorMessage = "Air time must be in HH:mm:ss format.")]
        public TimeSpan AirTime { get; set; }

        public IEnumerable<SelectListItem> Channels { get; set; }
        public virtual Channel Channel { get; set; }
    }
}