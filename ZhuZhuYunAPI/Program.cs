using JwtWebApiTutorial.Middleware;
using JwtWebApiTutorial.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using ZhuZhuYunAPI.Models;
//点击工具->NUGET包管理器->程序包管理器控制台
// 依次执行下列迁移命令，不要执行Remove - Migration
// 如果更改了models，需要删除迁移，然后重新迁移

//Get-help Migration    //查询命令
// Add-Migration init    //迁移
// Remove-Migration      //迁移删除
// Update-Database       //更新到数据库  22222

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey

    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWTConfig"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    byte[] key = System.Text.Encoding.ASCII.GetBytes(builder.Configuration["JWTConfig:Key"]);
    string isSuer = builder.Configuration["JWTConfig:Issuer"];
    string audience = builder.Configuration["JWTConfig:Audience"];

    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidIssuer = isSuer,
        ValidAudience = audience
    };
});
WebApiUtils.AppConfigHelper.InitAppConfig(builder.Configuration);
//MySql
builder.Services.AddDbContext<PanoUserContext>(opt =>
{
    string connectionString = WebApiUtils.AppConfigHelper.GetDBStrCon("MySQLConnection");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    opt.UseMySql(connectionString, serverVersion);
});

var app = builder.Build();

DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(defaultFilesOptions);

StaticFileOptions staticFileOptions = new StaticFileOptions();
//staticFileOptions.FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(@"C:\zhuzhuyun\publish\");
Console.WriteLine(app.Environment.ContentRootPath+"   55555");
//Console.WriteLine(app.Environment.WebRootPath+"   55555");
//Console.WriteLine(app.Environment.EnvironmentName+"   55555");
//Console.WriteLine(app.Environment.ApplicationName+"   55555");

if (app.Environment.IsDevelopment())
{
    staticFileOptions.FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(app.Environment.ContentRootPath + @"\bin\Release\net6.0\publish");
    //staticFileOptions.FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(@"E:\WEB\ZhuZhuYunAPI\ZhuZhuYunAPI\bin\Release\net6.0\publish");
    //staticFileOptions.FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(@"D:\YZJ\VSProject\ZhuZhuYunAPI\ZhuZhuYunAPI\bin\Release\net6.0\publish");
}
else
{
    staticFileOptions.FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(@"C:\zhuzhuyun\publish\");
}
app.UseStaticFiles(staticFileOptions);

//开启认证
app.UseAuthentication();
//开启授权
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();

app.UseSwaggerUI();
/// <summary>
/// 中间件
/// </summary>
app.UseCustomExceptionMiddleware();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
