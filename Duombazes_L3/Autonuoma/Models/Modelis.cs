namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Modelis;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// Model of 'Modelis' entity.
/// </summary>
public class Modelis
{
	[Display(Name="Id")]
	public int Id { get; set; }

	[Display(Name="Pavadinimas")]
	public string Pavadinimas { get; set; }

	//Markė
	[Display(Name="Markė")]
	public int FkMarke { get; set; }
}


/// <summary>
/// Model of 'Modelis' entity used in lists.
/// </summary>
public class ModelisL
{
	[Display(Name="Id")]
	public int Id { get; set; }

	[Display(Name="Pavadinimas")]
	public string Pavadinimas { get; set; }		

	[Display(Name="Markė")]
	public string Marke { get; set; }
}


/// <summary>
/// Model of 'Modelis' entity used in creation and editing forms.
/// </summary>
public class ModelisCE
{
	/// <summary>
	/// Entity data
	/// </summary>
	public class ModelM
	{
		[Display(Name="Id")]
		public int Id { get; set; }

		[Display(Name="Pavadinimas")]
		[MaxLength(20)]
		[Required]
		public string Pavadinimas { get; set; }

		[Display(Name="Markė")]
		[Required]
		public int FkMarke { get; set; }
	}

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Markes { get; set; }
	}

	/// <summary>
	/// Entity view.
	/// </summary>
	public ModelM Model { get; set; } = new ModelM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}

