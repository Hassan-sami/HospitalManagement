using Hospital.BLL.Helpers;
using Hospital.BLL.Mappers;
using Hospital.BLL.Services.Abstraction;
using Hospital.BLL.Services.Implementation;
using Hospital.DAL.DataBase;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using Hospital.DAL.Repository.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IPatientService, PatientService>();

            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

            builder.Services.AddDbContext<HospitalDbContext>(options =>
            options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<HospitalDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddAutoMapper(option => option.AddProfile<DomainProfile>());

            

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
