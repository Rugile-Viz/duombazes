namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories
{
    using Newtonsoft.Json;
    using Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF2;

    public class SutartisF2Repo : RepoBase
    {
        public static List<SutartisL> ListSutartis()
		{
    	var query =
        $@"SELECT
            s.numeris as nr,
            s.pirkimo_data as data,
            s.kaina,
            s.pristatymo_kaina,
            s.`užsakymo_būsena` as busena
        FROM
            `sutartis` s
        ORDER BY s.numeris DESC";

    var drc = Sql.Query(query);

    var result =
        Sql.MapAll<SutartisL>(drc, (dre, t) => {
            t.Nr = dre.From<int>("nr");
            t.Data = dre.From<DateTime>("data");
            t.Kaina = dre.From<decimal>("kaina");
            t.PristatymoKaina = dre.From<decimal>("pristatymo_kaina");                  
            t.Busena = dre.From<string>("busena");
        });

    return result;
}

        public static SutartisCE FindSutartisCE(int nr)
        {
            var query = $@"SELECT * FROM `sutartis` WHERE numeris=?nr";
            var drc =
                Sql.Query(query, args => {
                    args.Add("?nr", nr);
                });

            var result =
                Sql.MapOne<SutartisCE>(drc, (dre, t) => {
                    var sut = t.Sutartis;

                    sut.Nr = dre.From<int>("numeris");
                    sut.SutartiesData = dre.From<DateTime>("pirkimo_data");
                    sut.Kaina = dre.From<decimal>("pristatymo_ar_kiti_mokesčiai");
                    sut.GrazinimoAdresas = dre.From<string>("grąžinimo_adresas");
                    sut.PristatymoAdresas = dre.From<string>("pristatymo_adresas");
                    sut.UzsakymoBusena = dre.From<string>("užsakymo_būsena");
                    sut.PristatymoKaina = dre.From<decimal>("pristatymo_kaina");
                });

            return result;
        }

        public static int InsertSutartis(SutartisCE sutCE)
        {
            var query =
                $@"INSERT INTO `sutartis`
                (
                    `pirkimo_data`,
                    `pristatymo_ar_kiti_mokesčiai`,
                    `kaina`,
                    `grąžinimo_adresas`,
                    `pristatymo_adresas`,
                    `pristatymo_kaina`,
                    `užsakymo_būsena`
                )
                VALUES(
                    ?pirkimo_data,
                    ?mokesciai,
                    ?kaina,
                    ?grazinimo_adresas,
                    ?pristatymo_adresas,
                    ?pristatymo_kaina,
                    ?busena
                )";

            var nr =
                Sql.Insert(query, args => {
                    var sut = sutCE.Sutartis;

                    args.Add("?pirkimo_data", sut.SutartiesData);
                    args.Add("?kaina", sut.Kaina);
                    args.Add("?mokesciai", sut.PristatymoKaina);
                    args.Add("?grazinimo_adresas", sut.GrazinimoAdresas ?? string.Empty);
                    args.Add("?pristatymo_adresas", sut.PristatymoAdresas ?? string.Empty);
                    args.Add("?pristatymo_kaina", sut.PristatymoKaina);
                    args.Add("?busena", sut.UzsakymoBusena ?? "-nepristatyta");
                });

            return (int)nr;
        }

        public static void UpdateSutartis(SutartisCE sutCE)
        {
            var query =
                $@"UPDATE `sutartis`
                SET
                    `pirkimo_data` = ?pirkimo_data,
                    `pristatymo_ar_kiti_mokesčiai` = ?mokesciai,
                    `kaina` = ?kaina,
                    `grąžinimo_adresas` = ?grazinimo_adresas,
                    `pristatymo_adresas` = ?pristatymo_adresas,
                    `pristatymo_kaina` = ?pristatymo_kaina,
                    `užsakymo_būsena` = ?busena
                WHERE numeris=?nr";

            Sql.Update(query, args => {
                var sut = sutCE.Sutartis;

                args.Add("?pirkimo_data", sut.SutartiesData);
                args.Add("?kaina", sut.Kaina);
                args.Add("?mokesciai", sut.PristatymoKaina);
                args.Add("?grazinimo_adresas", sut.GrazinimoAdresas ?? string.Empty);
                args.Add("?pristatymo_adresas", sut.PristatymoAdresas ?? string.Empty);
                args.Add("?pristatymo_kaina", sut.PristatymoKaina);
                args.Add("?busena", sut.UzsakymoBusena ?? "-nepristatyta");
                args.Add("?nr", sut.Nr);
            });
        }

        public static void DeleteSutartis(int nr)
        {
            var query = $@"DELETE FROM `sutartis` WHERE numeris=?nr";
            Sql.Delete(query, args => {
                args.Add("?nr", nr);
            });
        }

        public static List<SutartisCE.UzsakytaPaslaugaM> ListUzsakytaPaslauga(int sutartisId)
        {
            var query =
                $@"SELECT 
                    up.id_UŽSAKYTA_PREKĖ as id,
                    up.kiekis,
                    up.kaina,
                    p.id_PREKĖ as prekes_id,
                    p.Filmo_pavadinimas,
                    p.būklė,
                    p.DVD_specifinis_kodas,
                    p.tipas
                FROM `užsakyta_prekė` up
                JOIN `prekė` p ON up.fk_PREKĖid_PREKĖ = p.id_PREKĖ
                WHERE up.fk_SUTARTISnumeris = ?sutartisId";

            var drc =
                Sql.Query(query, args => {
                    args.Add("?sutartisId", sutartisId);
                });

            var result =
                Sql.MapAll<SutartisCE.UzsakytaPaslaugaM>(drc, (dre, t) => {
                    t.InListId = dre.From<int>("id");
                    t.Paslauga = dre.From<int>("prekes_id").ToString();
                    t.Kiekis = dre.From<int>("kiekis");
                    t.Kaina = dre.From<decimal>("kaina");
                    t.FilmoPavadinimas = dre.From<string>("Filmo_pavadinimas");
                    t.FilmoBukle = dre.From<string>("būklė");
                    t.DVD_specifinis_kodas = dre.From<string>("DVD_specifinis_kodas");
                    t.DVD_standartas = dre.From<string>("tipas");
                });

            return result;
        }

			public static void InsertUzsakytaPaslauga(int sutartisId, SutartisCE.UzsakytaPaslaugaM up)
		{
			var query =
				$@"INSERT INTO `užsakyta_prekė`
				(
					`kiekis`,
					`kaina`,
					`fk_PREKĖid_PREKĖ`,
					`fk_SUTARTISnumeris`
				)
				VALUES(
					?kiekis,
					?kaina,
					?preke,
					?sutartis
				)";

			Sql.Insert(query, args => {
				args.Add("?kiekis", up.Kiekis);
				args.Add("?kaina", up.Kaina);
				args.Add("?preke", int.Parse(up.Paslauga));
				args.Add("?sutartis", sutartisId);
			});
		}

        public static void DeleteUzsakytaPaslaugaForSutartis(int sutartisId)
        {
            var query = $@"DELETE FROM `užsakyta_prekė` WHERE fk_SUTARTISnumeris=?sutartisId";
            Sql.Delete(query, args => {
                args.Add("?sutartisId", sutartisId);
            });
        }

        public static List<PrekeL> ListPreke()
        {
            var query = $@"SELECT * FROM `prekė` ORDER BY Filmo_pavadinimas";
            var drc = Sql.Query(query);

            var result =
                Sql.MapAll<PrekeL>(drc, (dre, t) => {
                    t.id_PREKĖ = dre.From<int>("id_PREKĖ");
                    t.Filmo_pavadinimas = dre.From<string>("Filmo_pavadinimas");
                    t.kaina = dre.From<decimal>("kaina");
                    t.būklė = dre.From<string>("būklė");
                    t.DVD_specifinis_kodas = dre.From<string>("DVD_specifinis_kodas");
                    t.tipas = dre.From<string>("tipas");
                });

            return result;
        }
    }

    public class PrekeL
    {
        public int id_PREKĖ { get; set; }
        public string Filmo_pavadinimas { get; set; }
        public decimal kaina { get; set; }
        public string būklė { get; set; }
        public string DVD_specifinis_kodas { get; set; }
        public string tipas { get; set; }
    }
}
