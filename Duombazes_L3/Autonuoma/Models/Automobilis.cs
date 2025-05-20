namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Automobilis;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


/// <summary>
/// 'Automobilis' in list form.
/// </summary>
public class AutomobilisL
{
	[Display(Name="Id")]
	public int Id { get; set; }

	[Display(Name="Valstybinis Nr.")]
	public string ValstybinisNr { get; set; }

	[Display(Name="Būsena")]
	public string Busena { get; set; }

	[Display(Name="Modelis")]
	public string Modelis { get; set; }

	[Display(Name="Markė")]
	public string Marke { get; set; }
}

/// <summary>
/// 'Automobilis' in create and edit forms.
/// </summary>
public class AutomobilisCE
{
	/// <summary>
	/// Automobilis.
	/// </summary>
	public class AutomobilisM
	{
		[Display(Name="Id")]
		[Required]
		public int Id { get; set; }


		[Display(Name="Valstybinis Nr.")]
		[MaxLength(6)]
		[Required]
		public string ValstybinisNr { get; set; }

		[Display(Name="Pagaminimo data")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required]
		public DateTime? PagaminimoData { get; set; }

		[Display(Name="Rida")]
		[Required]
		public int Rida { get; set; }

		[Display(Name="Radijas")]
		[Required]
		public bool Radijas { get; set; }

		[Display(Name="Grotuvas")]
		[Required]
		public bool Grotuvas { get; set; }

		[Display(Name="Kondicionierius")]
		[Required]
		public bool Kondicionierius { get; set; }

		[Display(Name="Vietų skaičius")]
		[Required]
		public int VietuSkaicius { get; set; }

		[Display(Name="Registravimo data")]
		[Required]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? RegistravimoData { get; set; }

		[Display(Name="Vertė")]
		[Required]
		[DataType(DataType.Currency)]
		public decimal Verte { get; set; }



		[Display(Name="Būsena")]
		public int? FkBusena { get; set; }

		[Display(Name="Modelis")]
		[Required]
		public int FkModelis { get; set; }

		[Display(Name="Pavarų dėžė")]
		[Required]
		public int FkPavaruDeze { get; set; }

		[Display(Name="Degalų tipas")]
		[Required]
		public int FkDegaluTipas { get; set; }

		[Display(Name="Kėbulo tipas")]
		[Required]
		public int FkKebuloTipas { get; set; }

		[Display(Name="Bagažo dydis")]
		[Required]
		public int FkLagaminas { get; set; }
	}

	/// <summary>
	/// Select lists for making drop downs for choosing values of entity fields.
	/// </summary>
	public class ListsM
	{
		public IList<SelectListItem> Busenos { get; set; }
		public IList<SelectListItem> Modeliai { get; set; }
		public IList<SelectListItem> PavaruDezes { get; set; }
		public IList<SelectListItem> KebuloTipai { get; set; }
		public IList<SelectListItem> DegaluTipai { get; set; }
		public IList<SelectListItem> Lagaminai { get; set; }

	}


	/// <summary>
	/// Automobilis.
	/// </summary>
	public AutomobilisM Automobilis { get ; set; } = new AutomobilisM();

	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();
}


/// <summary>
/// 'AutoBusena' enumerator in lists.
/// </summary>
public class AutoBusena
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'PavaruDeze' enumerator in lists.
/// </summary>
public class PavaruDeze
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'KebuloTipas' enumerator in lists.
/// </summary>
public class KebuloTipas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'DegaluTipas' enumerator in lists.
/// </summary>
public class DegaluTipas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}

/// <summary>
/// 'Lagaminas' enumerator in lists.
/// </summary>
public class Lagaminas
{
	public int Id { get; set; }

	public string Pavadinimas { get; set; }
}
