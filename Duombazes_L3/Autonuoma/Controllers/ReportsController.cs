namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using LateContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.LateContractsReport;
using ContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;
using ServicesReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ServicesReport;


/// <summary>
/// Controller for producing reports.
/// </summary>
public class ReportsController : ControllerBase
{
    /// <summary>
    /// Produces services report.
    /// </summary>
    /// <param name="dateFrom">Starting date. Can be null.</param>
    /// <param name="dateTo">Ending date. Can be null.</param>
    /// <returns>Report view.</returns>
    [HttpGet]
    public ActionResult Services(DateTime? dateFrom, DateTime? dateTo)
    {
        var report = AtaskaitaRepo.GetTotalServicesOrdered(dateFrom, dateTo);
        report.DateFrom = dateFrom;
        report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); //move time of end date to end of day

        report.Paslaugos = AtaskaitaRepo.GetServicesOrdered(dateFrom, dateTo);

        return View(report);
    }

    /// <summary>
    /// Produces contracts report.
    /// </summary>
    /// <param name="dateFrom">Starting date. Can be null.</param>
    /// <param name="dateTo">Ending date. Can be null.</param>
    /// <returns>Report view.</returns>
    [HttpGet]
 public ActionResult Contracts(DateTime? dateFrom, DateTime? dateTo, string sortOrder = "date_desc", string searchVardas = null, string searchPavarde = null)
{
    var report = new ContractsReport.Report();
    report.DateFrom = dateFrom;
    report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59);

    report.Sutartys = AtaskaitaRepo.GetContracts(report.DateFrom, report.DateTo, sortOrder, searchVardas, searchPavarde);

    foreach (var item in report.Sutartys)
    {
        report.VisoSumaSutartciu += item.Kaina;
        report.VisoSumaPaslaugu += item.PaslauguKaina;
    }

    ViewBag.SortOrder = sortOrder;
    ViewBag.SearchVardas = searchVardas;
    ViewBag.SearchPavarde = searchPavarde;
    
    return View(report);
}
    /// <summary>
    /// Produces late contracts reports.
    /// </summary>
    /// <param name="dateFrom">Starting date. Can be null.</param>
    /// <param name="dateTo">Ending date. Can be null.</param>
    /// <returns>Report view.</returns>
    [HttpGet]
    public ActionResult LateContracts(DateTime? dateFrom, DateTime? dateTo)
    {
        var report = new LateContractsReport.Report();
        report.DateFrom = dateFrom;
        report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); //move time of end date to end of day

        report.Sutartys = AtaskaitaRepo.GetLateReturnContracts(report.DateFrom, report.DateTo);

        return View(report);
    }
}