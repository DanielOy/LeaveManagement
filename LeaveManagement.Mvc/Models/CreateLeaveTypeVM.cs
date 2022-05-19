using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Mvc.Models
{
    public class CreateLeaveTypeVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Default Number Of Days")]
        public int DefaultDays { get; set; }
    }
}
