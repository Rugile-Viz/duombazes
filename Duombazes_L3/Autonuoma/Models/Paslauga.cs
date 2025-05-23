﻿namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Paslauga;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Paslauga' entity.
/// </summary>
public class Paslauga
{
	[Display(Name="Id")]
	public int Id { get; set; }

	[Display(Name="Pavadinimas")]
	[Required]
	public string Pavadinimas { get; set; }

	[Display(Name="Aprašymas")]
	public string Aprasymas { get; set; }
}


/// <summary>
/// View model for editing data of 'Paslauga' entity.
/// </summary>
public class PaslaugaCE
{
	/// <summary>
	/// Entity data.
	/// </summary>
	public Paslauga Paslauga { get; set; }

	/// <summary>
	/// View models of related 'PaslaugosKaina' entities
	/// </summary>
	public List<PaslaugosKaina> Kainos { get; set; } = new List<PaslaugosKaina>();
}
