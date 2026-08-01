using System.Security.Claims;
using System.Text;
using System.Text.Json;
using API.Middlewares;
using Application.Commands.Login;
using Application.Commands.Logout;
using Application.Commands.RefreshToken;
using Application.Commands.Register;
using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using Domain.Interfaces;
using IdentityDBService.Infrastructure.Repositories;
using IdentityDBService.src.Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//DI services controller
builder.Services.AddControllers();

//DI Services swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "nhập vào token JWT định dạng Bearer {token của bạn}."
    });

    //hiện button đăng nhập(ổ khoá nhập token) trên từng api endpoint của swagger
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

//DI Service EF-context
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionstring"));
});

//DI Repository 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// DI Service
builder.Services.AddScoped<IAuthService, AuthService>();

//UnitOfWork
builder.Services.AddScoped<UnitOfWork>();

//DI JwtService
builder.Services.AddScoped<IJwtService,JwtService>();

//DI Service BlockIpMiddleware
builder.Services.AddTransient<BlockIpMiddleware>();

//DI Handler
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddScoped<LogoutHandler>();


// MediatR (CQRS)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterHandler).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RefreshTokenHandler).Assembly);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(LogoutHandler).Assembly);
});

// DI AutoMapper
builder.Services.AddAutoMapper(cf=>{}, typeof(AuthProfile));

//JWT service
//Thêm middleware authentication
var PrivateKey = builder.Configuration["Jwt:Secret-Key"];
var Issuer = builder.Configuration["Jwt:Issuer"];
var Audience = builder.Configuration["Jwt:Audience"];
// Thêm dịch vụ Authentication vào ứng dụng, sử dụng JWT Bearer làm phương thức xác thực
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // Thiết lập các tham số xác thực token
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // Kiểm tra và xác nhận Issuer (nguồn phát hành token)
        ValidateIssuer = true,
        ValidIssuer = Issuer, // Biến `Issuer` chứa giá trị của Issuer hợp lệ
                              // Kiểm tra và xác nhận Audience (đối tượng nhận token)
        ValidateAudience = true,
        ValidAudience = Audience, // Biến `Audience` chứa giá trị của Audience hợp lệ
                                  // Kiểm tra và xác nhận khóa bí mật được sử dụng để ký token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PrivateKey)),
        // Sử dụng khóa bí mật (`privateKey`) để tạo SymmetricSecurityKey nhằm xác thực chữ ký của token
        // Giảm độ trễ (skew time) của token xuống 0, đảm bảo token hết hạn chính xác
        ClockSkew = TimeSpan.Zero,
        // Xác định claim chứa vai trò của user (để phân quyền)
        RoleClaimType = ClaimTypes.Role,
        // Xác định claim chứa tên của user
        NameClaimType = ClaimTypes.Name,
        // Kiểm tra thời gian hết hạn của token, không cho phép sử dụng token hết hạn
        ValidateLifetime = true
    };

    //Doạn phân quyền cho phép client gửi token lên khi kết nối signalR (mặc định signalR không hỗ trợ gửi token lên header như các request http thông thường mà sẽ gửi token lên query string)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var path = context.HttpContext.Request.Path;
            var accessToken = context.Request.Query["access_token"];
            var authHeader = context.Request.Headers["Authorization"];


            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/cart-hub"))
            {
                context.Token = accessToken.FirstOrDefault();
            }

            return Task.CompletedTask;
        }
    };
});

//di phân quyền
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseExceptionHandler(er =>
{
    er.Run(async context =>
    {
        //can thiệp lỗi theo format chuẩn
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = errorFeature?.Error;

        var errorResponse = new
        {
            Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau.",
            Detail = exception?.Message
        };

        var jsonRes = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(jsonRes);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Apply các middleware
app.UseAuthentication();
app.UseAuthorization();

// app.UseMiddleware<BlockIpMiddleware>();

// Tự động chuyển hướng HTTP sang HTTPS (bảo mật)
app.UseHttpsRedirection();

// Cho phép truy cập các file tĩnh (CSS, JS, ảnh, ...)
app.UseStaticFiles();

// Tự động chuyển hướng HTTP sang HTTPS (bảo mật)
app.UseHttpsRedirection();

//Sử dụng middleware map controller
app.MapControllers();

app.Run();