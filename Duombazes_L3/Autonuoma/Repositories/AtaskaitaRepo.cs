namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using LateContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.LateContractsReport;
using ContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;
using ServicesReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ServicesReport;


/// <summary>
/// Database operations related to reports.
/// </summary>
public class AtaskaitaRepo : RepoBase
{
    public static List<ServicesReport.Paslauga> GetServicesOrdered(DateTime? dateFrom, DateTime? dateTo)
    {
        var query =
            $@"SELECT
                dp.Paslaugos_kodas as id,
                dp.Paslaugos_pavadinimas as pavadinimas,
                COUNT(*) as kiekis,
                SUM(dp.Kaina) as suma,
                AVG(dp.Kaina) as vidurkis
            FROM `dronų_priežiūra` dp
            INNER JOIN `drono_gamintojas` dg ON dp.Paslaugos_kodas = dg.fk_Dronų_priežiūraPaslaugos_kodas
            WHERE
                dg.Pavadinimas IS NOT NULL
            GROUP BY
                dp.Paslaugos_kodas, dp.Paslaugos_pavadinimas
            ORDER BY
                suma DESC";

        var drc =
            Sql.Query(query, args => {
                // No date parameters in this simplified version
            });

        var result = 
            Sql.MapAll<ServicesReport.Paslauga>(drc, (dre, t) => {
                t.Id = dre.From<int>("id");
                t.Pavadinimas = dre.From<string>("pavadinimas");
                t.Kiekis = dre.From<int>("kiekis");
                t.Suma = dre.From<decimal>("suma");
                //t.Vidurkis = dre.From<decimal>("vidurkis");
            });

        return result;
    }

    public static ServicesReport.Report GetTotalServicesOrdered(DateTime? dateFrom, DateTime? dateTo)
    {
        var query =
            $@"SELECT
                COUNT(*) as kiekis,
                SUM(dp.Kaina) as suma,
                AVG(dp.Kaina) as vidurkis
            FROM `dronų_priežiūra` dp
            INNER JOIN `drono_gamintojas` dg ON dp.Paslaugos_kodas = dg.fk_Dronų_priežiūraPaslaugos_kodas";

        var drc =
            Sql.Query(query, args => {
                // No date parameters in this simplified version
            });

        var result = 
            Sql.MapOne<ServicesReport.Report>(drc, (dre, t) => {
                t.VisoUzsakyta = dre.From<int?>("kiekis") ?? 0;
                t.BendraSuma = dre.From<decimal?>("suma") ?? 0;
               // t.VidutineKaina = dre.From<decimal?>("vidurkis") ?? 0;
            });

        return result;
    }

   public static List<ContractsReport.Sutartis> GetContracts(
    DateTime? dateFrom = null, 
    DateTime? dateTo = null, 
    string sortOrder = "date_desc",
    string searchVardas = null,
    string searchPavarde = null)
{
    var orderByClause = sortOrder switch
    {
        "date_asc" => "u.Data ASC",
        "price_asc" => "u.Kaina ASC",
        "price_desc" => "u.Kaina DESC",
        _ => "u.Data DESC"
    };

    var query = $@"
        SELECT
            u.Identifikacijos_numeris as nr,
            DATE_FORMAT(u.Data, '%Y-%m-%d %H:%i') as sutarties_data,
            p.Vardas,
            p.Pavardė as pavarde,
            p.Tel_nr as asmens_kodas,
            u.Kaina,
            0 as paslaugu_kaina,
            1 as sutarciu_kiekis,
            u.Kaina as vidutine_kaina,
            u.Kaina as bendra_suma,
            0 as bendra_suma_paslaugu
        FROM
            `užsakymas` u
            LEFT JOIN `pirkėjas` p ON u.fk_PirkėjasIndividualus_nr = p.Individualus_nr
        WHERE 1=1";

    if (dateFrom.HasValue)
        query += " AND u.Data >= ?dateFrom";
    
    if (dateTo.HasValue)
        query += " AND u.Data <= ?dateTo";
    
    if (!string.IsNullOrWhiteSpace(searchVardas))
        query += " AND p.Vardas LIKE ?searchVardas";
    
    if (!string.IsNullOrWhiteSpace(searchPavarde))
        query += " AND p.Pavardė LIKE ?searchPavarde";

    query += $"\nORDER BY {orderByClause}, p.Pavardė ASC";

    var drc = Sql.Query(query, args => {
        if (dateFrom.HasValue)
            args.Add("?dateFrom", dateFrom);
        
        if (dateTo.HasValue)
            args.Add("?dateTo", dateTo);
        
        if (!string.IsNullOrWhiteSpace(searchVardas))
            args.Add("?searchVardas", $"%{searchVardas}%");
        
        if (!string.IsNullOrWhiteSpace(searchPavarde))
            args.Add("?searchPavarde", $"%{searchPavarde}%");
    });

    var result = 
        Sql.MapAll<ContractsReport.Sutartis>(drc, (dre, t) => {
            t.Nr = dre.From<int>("nr");
            t.SutartiesData = dre.From<DateTime>("sutarties_data");
            t.AsmensKodas = dre.From<string>("asmens_kodas");
            t.Vardas = dre.From<string>("vardas");
            t.Pavarde = dre.From<string>("pavarde");
            t.Kaina = dre.From<decimal>("kaina");
            t.PaslauguKaina = dre.From<decimal>("paslaugu_kaina");
            t.SutarciuKiekis = dre.From<int>("sutarciu_kiekis");
           // t.VidutineKaina = dre.From<decimal>("vidutine_kaina");
            t.BendraSuma = dre.From<decimal>("bendra_suma");
            t.BendraSumaPaslaug = dre.From<decimal>("bendra_suma_paslaugu");
        });

    return result;
}

    public static List<LateContractsReport.Sutartis> GetLateReturnContracts(DateTime? dateFrom, DateTime? dateTo)
    {
        // Since your schema doesn't have return dates, this will return empty
        var query = @"SELECT 1 as nr, '2000-01-01' as sutarties_data, 'N/A' as klientas, 
                     '2000-01-01' as planuojama_grazinimo_data_laikas, 'negražinta' as grazinimo_data
                     WHERE 1=0";  // Empty result

        var drc = Sql.Query(query);

        var result = 
            Sql.MapAll<LateContractsReport.Sutartis>(drc, (dre, t) => {
                t.Nr = dre.From<int>("nr");
                t.SutartiesData = dre.From<DateTime>("sutarties_data");
                t.Klientas = dre.From<string>("klientas");
                t.PlanuojamaGrData = dre.From<DateTime>("planuojama_grazinimo_data_laikas");
                t.FaktineGrData = dre.From<string>("grazinimo_data");
            });

        return result;
    }
}