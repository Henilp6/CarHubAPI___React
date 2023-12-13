﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarHub_Web.Models.Dto
{
    public class StateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("State Name")]
        public string StateName { get; set; }
        public int CountryId { get; set; }
        [ValidateNever]
        public CountryDTO Country { get; set; }
		[DisplayName("Is Active")]
		public bool IsActive { get; set; }


    }
}