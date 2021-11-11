using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FastReport.Web;
using Microsoft.Extensions.Hosting;
using System.IO;
using FastReport;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using JYTGameStore.Data;
using JYTGameStore.Models;
using FastReport.Data;
using FastReport.Export.PdfSimple;

namespace JYTGameStore.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db_a7aedd_jytstoreContext;

        public ReportsController(ILogger<ReportsController> logger,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration,
            ApplicationDbContext db_a7aedd_jytstoreContext)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _db_a7aedd_jytstoreContext = db_a7aedd_jytstoreContext;
        }

        public IActionResult Index()
        {
            //var webReport = Report();
            //ViewBag.WebReport = webReport;
            return View();
        }
        public IList<Game> GetEventList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var gamelist = db.Events.ToList();
            return (IList<Game>)gamelist;
        }

        public IActionResult ReportGames()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPGames.frx"));
            var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            webReport.Report.RegisterData(games, "Games");

            return View(webReport);

        }
        public IActionResult PdfGame()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPGames.frx"));
            var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            webReport.Report.RegisterData(games, "Game");
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Game Report") + ".pdf");
            }
        }
        public IActionResult PdfEvent()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPEvents.frx"));
            var events = GetTable<Event>(_db_a7aedd_jytstoreContext.Events, "Events");
            webReport.Report.RegisterData(events, "events");

            webReport.Report.Prepare();
            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Event Report") + ".pdf");
            }
        }
        public IActionResult ReportEvents()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPEvents.frx"));
            var events = GetTable<Event>(_db_a7aedd_jytstoreContext.Events, "Events");
            webReport.Report.RegisterData(events, "events");

            return View(webReport);
        }

        //public IActionResult ReportMembers()
        //{
        //    var webReport = new WebReport();
        //    var mssqlDataConnection = new MsSqlDataConnection();
        //    mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        //    webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
        //    webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPMembers.frx"));
        //    var member = GetTable<Game>(_db_a7aedd_jytstoreContext.Member, "Games");
        //    webReport.Report.RegisterData(member, "Games");

        //    return View(webReport);

        //}
        public IActionResult ReportMembers()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPMembers.frx"));
            //var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            //webReport.Report.RegisterData(games, "Game");
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Members Report") + ".pdf");
            }
        }
        public IActionResult ReportWishlist()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPWishlist.frx"));
            //var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            //webReport.Report.RegisterData(games, "Game");
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Wishlist Report") + ".pdf");
            }
        }
        public IActionResult ReportOrders()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPOrders.frx"));
            //var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            //webReport.Report.RegisterData(games, "Game");
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Orders Report") + ".pdf");
            }
        }
        public IActionResult ReportMemberDetails()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPMemberDetail.frx"));
            //var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            //webReport.Report.RegisterData(games, "Game");
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Member Detail Report") + ".pdf");
            }
        }
        public IActionResult ReportGameDetails()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPGameDetail.frx"));
            //var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            //webReport.Report.RegisterData(games, "Game");
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Game Detail Report") + ".pdf");
            }
        }
        public IActionResult ReportSales()
        {
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(_hostEnvironment.ContentRootPath, "reports", "RPSales.frx"));
            //var games = GetTable<Game>(_db_a7aedd_jytstoreContext.Game, "Games");
            //webReport.Report.RegisterData(games, "Game");
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "wwwroot/pdf", Path.GetFileNameWithoutExtension("Sales Report") + ".pdf");
            }
        }
        static DataTable GetTable<TEntity>(IEnumerable<TEntity> table, string name) where TEntity : class
        {
            var offset = 78;
            DataTable result = new DataTable(name);
            PropertyInfo[] infos = typeof(TEntity).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (info.PropertyType.IsGenericType
                && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    result.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType)));
                }
                else
                {
                    result.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
            }
            foreach (var el in table)
            {
                DataRow row = result.NewRow();
                foreach (PropertyInfo info in infos)
                {
                    if (info.PropertyType.IsGenericType &&
                        info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        object t = info.GetValue(el);
                        if (t == null)
                        {
                            t = Activator.CreateInstance(Nullable.GetUnderlyingType(info.PropertyType));
                        }

                        row[info.Name] = t;
                    }
                    else
                    {
                        if (info.PropertyType == typeof(byte[]))
                        {
                            var imageData = (byte[])info.GetValue(el);
                            var bytes = new byte[imageData.Length - offset];
                            Array.Copy(imageData, offset, bytes, 0, bytes.Length);
                            row[info.Name] = bytes;
                        }
                        else
                        {
                            row[info.Name] = info.GetValue(el);
                        }
                    }
                }
                result.Rows.Add(row);
            }

            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
