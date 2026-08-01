using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Application.Commands.AddRoom;
using Application.Commands.AddRoomType;
using Application.Commands.RemoveRoom;
using Application.Commands.RemoveRoomType;
using Application.Commands.UpdateRoom;
using Application.Commands.UpdateRoomStatus;
using Application.Commands.UpdateRoomType;
using Application.Interfaces;
using Application.Mapping;
using Application.Queries.GetAllRooms;
using Application.Queries.GetAvailableRooms;
using Application.Queries.GetRoomById;
using Application.Queries.SeachRoom;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RoomDBService.src.Infrastructure.Data;

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
builder.Services.AddDbContext<RoomDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionstring"));
});


// DI Repository 
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IRoomImageRepository, RoomImageRepository>();

// DI Service
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();

//UnitOfWork
builder.Services.AddScoped<UnitOfWork>();

// DI Handler
builder.Services.AddScoped<GetAllRoomsHandler>();
builder.Services.AddScoped<GetRoomByIdHandler>();
builder.Services.AddScoped<AddRoomTypeHandler>();
builder.Services.AddScoped<AddRoomHandler>();
builder.Services.AddScoped<UpdateRoomHandler>();
builder.Services.AddScoped<UpdateRoomTypeHandler>();
builder.Services.AddScoped<RemoveRoomHandler>();
builder.Services.AddScoped<RemoveRoomTypeHandler>();
builder.Services.AddScoped<SeachRoomHandler>();
builder.Services.AddScoped<GetAvailableRoomsHandler>();
builder.Services.AddScoped<UpdateRoomStatusHandler>();

// MediatR (CQRS)
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(GetAllRoomsHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(GetRoomByIdHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(AddRoomHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(AddRoomTypeHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(UpdateRoomHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(UpdateRoomTypeHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(RemoveRoomHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(RemoveRoomTypeHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(SeachRoomHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(GetAvailableRoomsHandler).Assembly);
});

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(UpdateRoomStatusHandler).Assembly);
});

//AutoMapper
builder.Services.AddAutoMapper(cfg=>{},typeof(RoomProfile));

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

// Tự động chuyển hướng HTTP sang HTTPS (bảo mật)
app.UseHttpsRedirection();

// Cho phép truy cập các file tĩnh (CSS, JS, ảnh, ...)
app.UseStaticFiles();

// Tự động chuyển hướng HTTP sang HTTPS (bảo mật)
app.UseHttpsRedirection();

//Sử dụng middleware map controller
app.MapControllers();

app.Run();