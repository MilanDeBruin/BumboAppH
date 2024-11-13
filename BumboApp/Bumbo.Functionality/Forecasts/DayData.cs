﻿using System.ComponentModel.DataAnnotations;

namespace Bumbo.Functionality.Forecasts;

public class DayData
{
    
    public string DayName { get; set; }
    [Required(ErrorMessage = "Het veld Verwachte aantal collies is verplicht.")]
    public int Collies { get; set; }
    [Required(ErrorMessage = "Het veld Verwachte aantal klanten is verplicht.")]
    public int ExpectedCustomers { get; set; }
   
}