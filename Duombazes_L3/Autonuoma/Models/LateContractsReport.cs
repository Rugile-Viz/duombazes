namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.LateContractsReport;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// View model for single contract in late contracts report.
/// </summary>
public class Sutartis
{
	[Display(Name="Sutartis")]
	public int Nr { get; set; }

	[Display(Name="Sudarymo data")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime SutartiesData { get; set; }

	[Display(Name="Klientas")]
	public string Klientas { get; set; }

	[Display(Name="Planuota grąžinti")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
	public DateTime PlanuojamaGrData { get; set; }

	[Display(Name="Grąžinta")]
	public string FaktineGrData { get; set; }
}

/// <summary>
/// View model for late contracts report.
/// </summary>
public class Report
{
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	public DateTime? DateFrom { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	public DateTime? DateTo { get; set; }

	public List<Sutartis> Sutartys { get; set; }
}
