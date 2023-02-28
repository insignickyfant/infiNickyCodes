using Microsoft.EntityFrameworkCore;
using CamerasWebApp.Data;
using CamerasWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using InfiNickyCodes;

namespace CamerasWebApp
{
    public class Program
    {
        public static CSVDataHandler dataHandler;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? 
                throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string csv_file_path = "wwwroot/csv/cameras-defb.csv";
            dataHandler = new CSVDataHandler(csv_file_path);
            //"Server=(localdb)\\mssqllocaldb;Database=CamerasWebApp.Data;Trusted_Connection=True;MultipleActiveResultSets=true"
            SqlConnection connection = new SqlConnection(builder.Configuration.GetConnectionString("ApplicationDbContext"));
            DataTable table = ToDataTable<InfiNickyCodes.Camera>(dataHandler.CreateCamerasFromCSV());
            connection.Open();
            SqlBulkCopy bulkCopy = new(connection);
            bulkCopy.DestinationTableName = "Camera";
            try
            {
                bulkCopy.WriteToServer(table);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //SqlDataAdapter adapter = new ("SELECT * FROM " + table.TableName, connection);
            //adapter.Fill(table);



            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="IQueryable"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<IQueryable>(List<IQueryable> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(IQueryable));
            DataTable table = new();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (IQueryable item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            table.TableName = "CSVData";
            return table;
        }
    }
}