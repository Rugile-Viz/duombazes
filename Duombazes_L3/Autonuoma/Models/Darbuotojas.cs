namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Darbuotojas' entity.
/// </summary>
public class Darbuotojas
{
	[Display(Name="Tabelio Nr.")]
	[MaxLength(10)]
	[Required]
	public string Tabelis { get; set; }

	[Display(Name="Vardas")]
	[MaxLength(20)]
	[Required]
	public string Vardas { get; set; }

	[Display(Name="Pavardė")]
	[MaxLength(20)]
	[Required]
	public string Pavarde { get; set; }
}
