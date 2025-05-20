namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model for 'Marke' entity.
/// </summary>
public class Marke
{
	[Display(Name="Id")]
	public int Id { get; set; }

	[Display(Name="Pavadinimas")]
	[Required]
	public string Pavadinimas { get; set; }
}
