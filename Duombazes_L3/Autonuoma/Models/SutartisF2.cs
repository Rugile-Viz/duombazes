namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF2;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

/// <summary>
/// 'Sutartis' in list form.
/// </summary>
public class SutartisL
{
    [Display(Name = "Nr.")]
    public int Nr { get; set; }

    [Display(Name = "Data")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Data { get; set; }  // Changed from SutartiesData to Data

    [Display(Name = "Kaina")]
    public decimal Kaina { get; set; }

    [Display(Name = "Pristatymo kaina")]
    public decimal PristatymoKaina { get; set; }  // Added this field

    [Display(Name = "Būsena")]
    public string Busena { get; set; }  // Changed from UzsakymoBusena to Busena
}

/// <summary>
/// 'Sutartis' in create and edit forms.
/// </summary>
public class SutartisCE
{
    /// <summary>
    /// Entity data.
    /// </summary>
    public class SutartisM
{
    [Display(Name = "Nr")]
    public int Nr { get; set; }

    [Display(Name = "Sutarties data")]
    [DataType(DataType.Date)]
    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime SutartiesData { get; set; }

    [Display(Name = "Kaina")]
    [Required]
    public decimal Kaina { get; set; }

    [Display(Name = "Grąžinimo adresas")]
    public string GrazinimoAdresas { get; set; }

    [Display(Name = "Pristatymo adresas")]
    public string PristatymoAdresas { get; set; }

    [Display(Name = "Užsakymo būsena")]
    [Required]
    public string UzsakymoBusena { get; set; }

    [Display(Name = "Pristatymo kaina")]
    public decimal PristatymoKaina { get; set; }
}

    /// <summary>
    /// Representation of 'UzsakytaPaslauga' entity in 'Sutartis' edit form.
    /// </summary>
    public class UzsakytaPaslaugaM
{
    public int InListId { get; set; }

    [Display(Name = "Paslauga")]
    [Required]
    public string Paslauga { get; set; }

    [Display(Name = "Kiekis")]
    [Required]
    [Range(1, int.MaxValue)]
    public int Kiekis { get; set; }

    [Display(Name = "Kaina")]
    [Required]
    public decimal Kaina { get; set; }

    [Display(Name = "Filmo pavadinimas")]
    public string FilmoPavadinimas { get; set; }

    [Display(Name = "Būklė")]
    public string FilmoBukle { get; set; }

    [Display(Name = "DVD kodas")]
    public string DVD_specifinis_kodas { get; set; }

    [Display(Name = "Tipas")]
    public string DVD_standartas { get; set; }
}

    /// <summary>
    /// Select lists for making drop downs for choosing values of entity fields.
    /// </summary>
    public class ListsM
    {
        public IList<SelectListItem> Busenos { get; set; }
        public IList<SelectListItem> FilmoPavadinimas { get; set; }
        public IList<SelectListItem> FilmoBukle { get; set; }
        public IList<SelectListItem> DVD_Tipas { get; set; }
        public IList<SelectListItem> Paslaugos { get; set; }
    }

    /// <summary>
    /// Sutartis.
    /// </summary>
    public SutartisM Sutartis { get; set; } = new SutartisM();

    /// <summary>
    /// Related 'UzsakytaPaslauga' records.
    /// </summary>
    public IList<UzsakytaPaslaugaM> UzsakytosPaslaugos { get; set; } = new List<UzsakytaPaslaugaM>();

    /// <summary>
    /// Lists for drop down controls.
    /// </summary>
    public ListsM Lists { get; set; } = new ListsM();
}

/// <summary>
/// 'SutartiesBusena' enumerator in lists.
/// </summary>
public class SutartiesBusena
{
    public int Id { get; set; }
    public string Name { get; set; }
}