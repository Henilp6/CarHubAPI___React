using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHub_API.Models.Dto
{
    public class CarSpecificationCreateDTO
    {
      public int Id { get; set; }
        [Required]
        [Display(Name = "Car Name")]
        public int CarId { get; set; }
      
        [Required]
        [Display(Name = "Displacement(Engine in CC)")]
        public string Displacement { get; set; }
        [Required]
        [Display(Name = "Max Power")]

        public string MaxPower { get; set; }
        [Required]
        [Display(Name = "Max Torque")]
        public string MaxTorque { get; set; }
        [Required]
        [Display(Name = "No Of Cylinder")]
        public int Cylinder { get; set; }
        [Required]
        [Display(Name = "Front Suspension")]
        public string FrontSuspension { get; set; }
        [Required]
        [Display(Name = "Rear Suspension")]
        public string RearSuspension { get; set; }
        [Required]
        [Display(Name = "Shock Absorbers")]
        public string ShockAbsorbers { get; set; }
        [Display(Name = "No Of AirBag")]
        public string AirbagNo { get; set; }
        [Required]
        [Display(Name = "Length(mm)")]
        public int Length { get; set; }
        [Required]
        [Display(Name = "Width(mm)")]
        public int Width { get; set; }
        [Required]
        [Display(Name = "Height")]
        public int Height { get; set; }
        [Required]
        [Display(Name = "Boot Space(Litres)")]
        public int BootSpace { get; set; }
        [Required]
        [Display(Name = "Seating Capacity")]
        public int SeatingCapacity { get; set; }
        [Required]
        [Display(Name = "WheelBase(mm) ")]
        public int WheelBase { get; set; }
        [Required]
        [Display(Name = "GearBox(Speed)")]
        public int GearBox { get; set; }
        [Display(Name = "DriveType(EX:4*4)")]
        public string DriveType { get; set; }
    }
}
