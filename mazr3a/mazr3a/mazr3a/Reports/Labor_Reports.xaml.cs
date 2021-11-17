using Npgsql;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using System.Data;
using Microcharts;
using SkiaSharp;
using Microcharts.Forms;
using Entry = Microcharts.ChartEntry;
using System.Linq;

namespace mazr3a
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Labor_Reports : ContentPage
    {
        public Labor_Reports()
        {
            InitializeComponent();

        }

        private async void Generate_Labor_Excel_Clicked(object sender, EventArgs e)
        {

            // Create Lists of Table Columns
            var DistributionDate = new List<string>() { "Date" };
            var DistributionActivityCode = new List<string>() { "Activity Code" };
            var DistributionActivity = new List<string>() { "Activity" };
            var DistributionFarmName = new List<string>() { "Farm Name" };
            var DistributionSector = new List<string>() { "Sector" };
            var DistributionNoOfLabor = new List<string>() { "No.Of Labor" };
            var DistributionUnitRate = new List<string>() { "Unit Rate" };
            var DistributionTotal = new List<string>() { "Total" };

            //Connect To DB and append data to lists
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"Date\", \"Activity_Code\", \"Activity\", \"Farm_Name\", \"Sector\", \"No.of_labor\", \"Unit_Rate\", \"Total\" FROM public.\"Distribution\" WHERE \"Date\" BETWEEN @DateFrom AND @DateTo ORDER BY \"Date\" desc;";
                command.Parameters.AddWithValue("DateFrom", Convert.ToDateTime(DateFrom.Date.ToShortDateString()));
                command.Parameters.AddWithValue("DateTo", Convert.ToDateTime(DateTo.Date.ToShortDateString()));
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    DistributionDate.Add(reader[0].ToString());
                    DistributionActivityCode.Add(reader[1].ToString());
                    DistributionActivity.Add(reader[2].ToString());
                    DistributionFarmName.Add(reader[3].ToString());
                    DistributionSector.Add(reader[4].ToString());
                    DistributionNoOfLabor.Add(reader[5].ToString());
                    DistributionUnitRate.Add(reader[6].ToString());
                    DistributionTotal.Add(reader[7].ToString());


                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
                await DisplayAlert("Error", Error, "OK");

            }

            //Save the stream as a file in the device and invoke it for viewing
            MemoryStream memoryStream = new MemoryStream();
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.
                GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet1"
            };
            sheets.Append(sheet);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Create Excel Table
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            DataTable table = new DataTable();
            table.Columns.Add("Date", typeof(string));
            table.Columns.Add("ActivityCode", typeof(string));
            table.Columns.Add("Activity", typeof(string));
            table.Columns.Add("FarmName", typeof(string));
            table.Columns.Add("Sector", typeof(string));
            table.Columns.Add("No.OfLabor", typeof(string));
            table.Columns.Add("UnitRate", typeof(string));
            table.Columns.Add("Total", typeof(string));


            for (int i = 0; i < DistributionDate.Count; i++)
            {
                DataRow row = table.NewRow();
                row["Date"] = DistributionDate[i];
                row["ActivityCode"] = DistributionActivityCode[i];
                row["Activity"] = DistributionActivity[i];
                row["FarmName"] = DistributionFarmName[i];
                row["Sector"] = DistributionSector[i];
                row["No.OfLabor"] = DistributionNoOfLabor[i];
                row["UnitRate"] = DistributionUnitRate[i];
                row["Total"] = DistributionTotal[i];
                table.Rows.Add(row);
            }
            foreach (DataRow item in table.Rows)
            {
                Row r = new Row();
                for (int i = 0; i < item.ItemArray.Length; i++)
                {
                    Cell c = new Cell()
                    {
                        CellValue = new CellValue(item[i].ToString()),
                        DataType = CellValues.String
                    };
                    r.Append(c);

                }

                sheetData.Append(r);
            }
            worksheetPart.Worksheet.Save();
            workbookpart.Workbook.Save();
            // Close the document.
            spreadsheetDocument.Close();
            memoryStream.Seek(0, SeekOrigin.Begin);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Mazr3a.xlsx", "application/msexcel", memoryStream);
            await DisplayAlert("Message", "Generation Done", "OK");
        }

        private void Generate_Labor_Charts_Clicked(object sender, EventArgs e)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            string ConnectionString = "Server=mazr3a.cc5lkrzrthkd.us-east-1.rds.amazonaws.com; Port=5432; User Id=postgres; Password=postgres; Database = postgres";
            var Total = new List<string>() {};
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SUM(\"Total\") FROM public.\"Distribution\" WHERE \"Date\" BETWEEN @DateFrom AND @DateTo ;";
                command.Parameters.AddWithValue("DateFrom", Convert.ToDateTime(DateFrom.Date.ToShortDateString()));
                command.Parameters.AddWithValue("DateTo", Convert.ToDateTime(DateTo.Date.ToShortDateString()));
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Total.Add(reader[0].ToString());
                    
                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
                DisplayAlert("Error", Error, "OK");

            }
            Tot.Text = Total[0].ToString()+" EGP";
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            var Farm_Sector = new List<string>();
            var Total_Farm_Sector = new List<string>();
            var random = new Random();
            var entries = new List<Entry>();

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"Farm_Sector\",SUM(\"Total\") AS \"SUM\" From(SELECT \"Date\", concat(\"Farm_Name\", ' - ', \"Sector\") AS \"Farm_Sector\", \"Total\" FROM public.\"Distribution\" WHERE \"Date\" BETWEEN @DateFrom AND @DateTo) TT GROUP BY \"Farm_Sector\" ORDER BY \"SUM\" DESC;";
                command.Parameters.AddWithValue("DateFrom", Convert.ToDateTime(DateFrom.Date.ToShortDateString()));
                command.Parameters.AddWithValue("DateTo", Convert.ToDateTime(DateTo.Date.ToShortDateString()));
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Farm_Sector.Add(reader[0].ToString());
                    Total_Farm_Sector.Add(reader[1].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
                DisplayAlert("Error", Error, "OK");

            }




            
            for (int i = 0;i< Farm_Sector.Count;i++)
            {
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                entries.Add(new Entry(float.Parse(Total_Farm_Sector[i]))
                {
                    Color = SKColor.Parse(color),
                    Label = i.ToString(),
                    ValueLabel = Total_Farm_Sector[i].ToString(),
                    
                });
                Label lbl = new Label();
                lbl.Text = i.ToString() + " -- "+ Farm_Sector[i].ToString();
                Chart1name.Children.Add(lbl);
               

            }

            Chart1.Chart = new BarChart() { Entries = entries };
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var Farm_Sector_labor = new List<string>();
            var Sum_labor = new List<string>();
            
            var entries2 = new List<Entry>();

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                NpgsqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT \"Farm_Sector\",SUM(\"No.of_labor\") AS \"SUM\"  From(SELECT \"Date\", concat(\"Farm_Name\", \' - ', \"Sector\") AS \"Farm_Sector\", \"No.of_labor\" FROM public.\"Distribution\" WHERE \"Date\" BETWEEN @DateFrom AND @DateTo) TT GROUP BY \"Farm_Sector\" ORDER BY \"SUM\" DESC;";
                command.Parameters.AddWithValue("DateFrom", Convert.ToDateTime(DateFrom.Date.ToShortDateString()));
                command.Parameters.AddWithValue("DateTo", Convert.ToDateTime(DateTo.Date.ToShortDateString()));
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Farm_Sector_labor.Add(reader[0].ToString());
                    Sum_labor.Add(reader[1].ToString());

                }
                connection.Close();


            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
                DisplayAlert("Error", Error, "OK");

            }





            for (int i = 0; i < Farm_Sector_labor.Count; i++)
            {
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                entries2.Add(new Entry(float.Parse(Sum_labor[i]))
                {
                    Color = SKColor.Parse(color),
                    Label = i.ToString(),
                    ValueLabel = Sum_labor[i].ToString(),
                    ValueLabelColor = SKColor.Parse(color),

                });
                Label lbl = new Label();
                lbl.Text = i.ToString() + " -- " + Farm_Sector_labor[i].ToString();
                Chart2name.Children.Add(lbl);


            }

            Chart2.Chart = new PointChart() { Entries = entries2 };
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }
    }
}