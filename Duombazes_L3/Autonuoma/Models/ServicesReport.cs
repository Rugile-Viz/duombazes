namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.ServicesReport;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;



/// <summary>
/// View model for a single service in services report.
/// </summary>
public class Paslauga
{
	[Display(Name="Id")]
	public int Id { get; set; }

	[Display(Name="Pavadinimas")]
	public string Pavadinimas { get; set; }

	[Display(Name="Kiekis")]
	public int Kiekis { get; set; }

	[Display(Name="Suma")]
	public decimal Suma { get; set; }
}

/// <summary>
/// View model of the whole report.
/// </summary>
public class Report
{
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateFrom { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateTo { get; set; }

	public List<Paslauga> Paslaugos { get; set; }

	public int VisoUzsakyta { get; set; }

	public decimal BendraSuma { get; set; }
}
