namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Klientas' entity.
/// </summary>
public class Klientas
{
	[Display(Name="Asmens kodas")]
	[Required]
	public string AsmensKodas { get; set; }
	
	[Display(Name="Vardas")]
	[Required]
	public string Vardas { get; set; }

	[Display(Name="Pavardė")]
	[Required]
	public string Pavarde { get; set; }

	[Display(Name="Gimimo data")]
	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	[Required]
	public DateTime? GimimoData { get; set; }

	[Display(Name="Telefonas")]
	[Required]
	public string Telefonas { get; set; }

	[Display(Name="Elektroninis paštas")]
	[EmailAddress]
	[Required]
	public string Epastas { get; set; }
}
