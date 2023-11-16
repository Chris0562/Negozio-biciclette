
using Betacomio.Authentication;
using Betacomio.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Text.Json.Serialization;

namespace Betacomio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AdventureWorksLt2019Context>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorks")));
            builder.Services.AddDbContext<UsersBetacomioContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Betacomio")));
            builder.Services.AddControllers().AddJsonOptions(jsOpt => jsOpt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", opt => { });
            builder.Services.AddAuthorization(opt => opt.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build()));



        builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(
                    build =>
                    {
                        build.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                        //build.WithOrigins("http://localhost:4200")
                        //.AllowAnyHeader()
                        //.AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();            

            app.Run();
        }
    }
}