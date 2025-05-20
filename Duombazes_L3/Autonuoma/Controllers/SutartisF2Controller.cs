namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
    using Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF2;

    public class SutartisF2Controller : Controller
    {
        /// <summary>
        /// Shows list of contracts
        /// </summary>
        /// <returns>Contracts list view</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View(SutartisF2Repo.ListSutartis());
        }

        /// <summary>
        /// Shows creation form
        /// </summary>
        /// <returns>Creation form view</returns>
        [HttpGet]
        public ActionResult Create()
        {
            var sutCE = new SutartisCE();
            sutCE.Sutartis.SutartiesData = DateTime.Now;
            PopulateLists(sutCE);
            return View(sutCE);
        }

        /// <summary>
        /// Handles form submission
        /// </summary>
        [HttpPost]
        public ActionResult Create(int? save, int? add, int? remove, SutartisCE sutCE)
        {
            if (add != null)
            {
                var up = new SutartisCE.UzsakytaPaslaugaM
                {
                    InListId = sutCE.UzsakytosPaslaugos.Count > 0 ? 
                        sutCE.UzsakytosPaslaugos.Max(it => it.InListId) + 1 : 0,
                    Paslauga = string.Empty,
                    Kiekis = 1,
                    Kaina = 0
                };
                sutCE.UzsakytosPaslaugos.Add(up);

                ModelState.Clear();
                PopulateLists(sutCE);
                return View(sutCE);
            }

            if (remove != null)
            {
                sutCE.UzsakytosPaslaugos = sutCE.UzsakytosPaslaugos
                    .Where(it => it.InListId != remove.Value)
                    .ToList();

                ModelState.Clear();
                PopulateLists(sutCE);
                return View(sutCE);
            }

            if (save != null)
            {
                // Patikrina ar nėra dubliuotų prekių pagal ID
                for (var i = 0; i < sutCE.UzsakytosPaslaugos.Count - 1; i++)
                {
                    var refUp = sutCE.UzsakytosPaslaugos[i];
                    for (var j = i + 1; j < sutCE.UzsakytosPaslaugos.Count; j++)
                    {
                        var testUp = sutCE.UzsakytosPaslaugos[j];
                        if (testUp.Paslauga == refUp.Paslauga)
                            ModelState.AddModelError($"UzsakytosPaslaugos[{j}].Paslauga", 
                                "Nepasirinkta prekė jau pridėta prie užsakymo.");
                    }
                }

                // Patikrina ar visos prekės pasirinktos
                for (int i = 0; i < sutCE.UzsakytosPaslaugos.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(sutCE.UzsakytosPaslaugos[i].Paslauga))
                    {
                        ModelState.AddModelError($"UzsakytosPaslaugos[{i}].Paslauga", "Pasirinkite prekę.");
                    }
                }

                if (ModelState.IsValid)
                {
                    // Žemėlapiuoja būseną į DB reikšmę
                    sutCE.Sutartis.UzsakymoBusena = MapBusenaToDbValue(sutCE.Sutartis.UzsakymoBusena);

                    // Sukuria naują sutartį ir gauna numerį
                    sutCE.Sutartis.Nr = SutartisF2Repo.InsertSutartis(sutCE);

                    // Įrašo kiekvieną užsakytą prekę
                    foreach (var upVm in sutCE.UzsakytosPaslaugos)
                    {
                        SutartisF2Repo.InsertUzsakytaPaslauga(sutCE.Sutartis.Nr, upVm);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    PopulateLists(sutCE);
                    return View(sutCE);
                }
            }

            throw new Exception("Should not reach here.");
        }

        /// <summary>
        /// Shows edit form
        /// </summary>
        /// <param name="id">Contract ID</param>
        /// <returns>Edit form view</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sutCE = SutartisF2Repo.FindSutartisCE(id);
            sutCE.UzsakytosPaslaugos = SutartisF2Repo.ListUzsakytaPaslauga(id);
            
            // Map status from DB to form value
            sutCE.Sutartis.UzsakymoBusena = MapBusenaFromDbValue(sutCE.Sutartis.UzsakymoBusena);
            
            PopulateLists(sutCE);
            return View(sutCE);
        }

        /// <summary>
        /// Handles edit form submission
        /// </summary>
        [HttpPost]
        public ActionResult Edit(int id, int? save, int? add, int? remove, SutartisCE sutCE)
        {
            if (add != null)
            {
                var up = new SutartisCE.UzsakytaPaslaugaM
                {
                    InListId = sutCE.UzsakytosPaslaugos.Count > 0 ? 
                        sutCE.UzsakytosPaslaugos.Max(it => it.InListId) + 1 : 0,
                    Paslauga = string.Empty,
                    Kiekis = 1,
                    Kaina = 0
                };
                sutCE.UzsakytosPaslaugos.Add(up);

                ModelState.Clear();
                PopulateLists(sutCE);
                return View(sutCE);
            }

            if (remove != null)
            {
                sutCE.UzsakytosPaslaugos = sutCE.UzsakytosPaslaugos
                    .Where(it => it.InListId != remove.Value)
                    .ToList();

                ModelState.Clear();
                PopulateLists(sutCE);
                return View(sutCE);
            }

            if (save != null)
            {
                // Validate for duplicate services
                for (var i = 0; i < sutCE.UzsakytosPaslaugos.Count - 1; i++)
                {
                    var refUp = sutCE.UzsakytosPaslaugos[i];
                    for (var j = i + 1; j < sutCE.UzsakytosPaslaugos.Count; j++)
                    {
                        var testUp = sutCE.UzsakytosPaslaugos[j];
                        if (testUp.Paslauga == refUp.Paslauga)
                            ModelState.AddModelError($"UzsakytosPaslaugos[{j}].Paslauga", 
                                "Duplicate DVD product added.");
                    }
                }

                if (ModelState.IsValid)
                {
                    // Map status to database values
                    sutCE.Sutartis.UzsakymoBusena = MapBusenaToDbValue(sutCE.Sutartis.UzsakymoBusena);

                    // Update contract
                    SutartisF2Repo.UpdateSutartis(sutCE);

                    // Delete old ordered DVD products
                    SutartisF2Repo.DeleteUzsakytaPaslaugaForSutartis(sutCE.Sutartis.Nr);

                    // Create new ordered DVD products
                    foreach (var upVm in sutCE.UzsakytosPaslaugos)
                        SutartisF2Repo.InsertUzsakytaPaslauga(sutCE.Sutartis.Nr, upVm);

                    return RedirectToAction("Index");
                }
                else
                {
                    PopulateLists(sutCE);
                    return View(sutCE);
                }
            }

            throw new Exception("Should not reach here.");
        }

        /// <summary>
        /// Shows delete confirmation
        /// </summary>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sutCE = SutartisF2Repo.FindSutartisCE(id);
            return View(sutCE);
        }

        /// <summary>
        /// Handles deletion
        /// </summary>
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            var sutCE = SutartisF2Repo.FindSutartisCE(id);

            // Check if contract can be deleted
            if (sutCE.Sutartis.UzsakymoBusena == "-nepristatyta" || 
                sutCE.Sutartis.UzsakymoBusena == "-neapmokėta")
            {
                SutartisF2Repo.DeleteUzsakytaPaslaugaForSutartis(id);
                SutartisF2Repo.DeleteSutartis(id);
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["deletionNotPermitted"] = true;
                return View("Delete", sutCE);
            }
        }

        /// <summary>
        /// Populates select lists
        /// </summary>
        private void PopulateLists(SutartisCE sutCE)
        {
            // DVD products
            sutCE.Lists.Paslaugos = SutartisF2Repo.ListPreke()
                .Select(it => new SelectListItem
                {
                    Value = it.id_PREKĖ.ToString(),
                    Text = $"{it.Filmo_pavadinimas} (Kaina: {it.kaina} EUR, Būklė: {it.būklė})"
                })
                .ToList();

            // Contract statuses
            sutCE.Lists.Busenos = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Pristatyta" },
                new SelectListItem { Value = "2", Text = "Nepristatyta" },
                new SelectListItem { Value = "3", Text = "Apmokėta" },
                new SelectListItem { Value = "4", Text = "Neapmokėta" },
                new SelectListItem { Value = "5", Text = "Išsiųsta" }
            };

            // DVD conditions
            sutCE.Lists.FilmoBukle = new List<SelectListItem>
            {
                new SelectListItem { Value = "nauja", Text = "Nauja" },
                new SelectListItem { Value = "naudota", Text = "Naudota" },
                new SelectListItem { Value = "gera", Text = "Gera" },
                new SelectListItem { Value = "sena", Text = "Sena" }
            };

            // DVD types
            sutCE.Lists.DVD_Tipas = new List<SelectListItem>
            {
                new SelectListItem { Value = "DVD-ROM", Text = "DVD-ROM" },
                new SelectListItem { Value = "DVD-R", Text = "DVD-R" },
                new SelectListItem { Value = "DVD-R_DL", Text = "DVD-R DL" },
                new SelectListItem { Value = "DVD+R_DL", Text = "DVD+R DL" }
            };
        }

        /// <summary>
        /// Maps form status value to database value
        /// </summary>
        private string MapBusenaToDbValue(string formValue)
        {
            return formValue switch
            {
                "1" => "-pristatyta",
                "2" => "-nepristatyta",
                "3" => "-apmokėta",
                "4" => "-neapmokėta",
                "5" => "-išsiųsta",
                _ => "-nepristatyta"
            };
        }

        /// <summary>
        /// Maps database status value to form value
        /// </summary>
        private string MapBusenaFromDbValue(string dbValue)
        {
            return dbValue switch
            {
                "-pristatyta" => "1",
                "-nepristatyta" => "2",
                "-apmokėta" => "3",
                "-neapmokėta" => "4",
                "-išsiųsta" => "5",
                _ => "2"
            };
        }
    }
}