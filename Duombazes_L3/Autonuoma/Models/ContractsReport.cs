namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// View model for single contract in a report.
/// </summary>
public class Sutartis
{
    [Display(Name="Sutartis")]
    public int Nr { get; set; }

    [Display(Name="Data")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime SutartiesData { get; set; }

    public string Vardas { get; set; }

    public string Pavarde { get; set; }

    public string AsmensKodas { get; set; }

    //[Display(Name="Sudarytų sutarčių vertė")]
    public decimal Kaina { get; set; }

    [Display(Name="Užsakytų paslaugų vertė")]
    public decimal PaslauguKaina { get; set; }

    public decimal BendraSuma { get; set; }

    public decimal BendraSumaPaslaug { get; set; }

    public int SutarciuKiekis { get; set; }

   // public decimal VidutineKaina { get; set; }

	
}

/// <summary>
/// View model for whole report.
/// </summary>
public class Report
{
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? DateFrom { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? DateTo { get; set; }

    public List<Sutartis> Sutartys { get; set; }

    public decimal VisoSumaSutartciu { get; set; }

    public decimal VisoSumaPaslaugu { get; set; }

   // public decimal VidutineSutartiesKaina { get; set; }

    public int VisoSutarciu { get; set; }
}