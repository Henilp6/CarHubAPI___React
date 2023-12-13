
using CarHub_Web;
using CarHub_Web.Service;
using CarHub_Web.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddHttpClient<IApplicationRoleService, ApplicationRoleService>();
builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
builder.Services.AddHttpClient<IApplicationUserRoleService, ApplicationUserRoleService>();
builder.Services.AddScoped<IApplicationUserRoleService, ApplicationUserRoleService>();

builder.Services.AddHttpClient<ICarTypeService, CarTypeService>();
builder.Services.AddScoped<ICarTypeService, CarTypeService>();
builder.Services.AddHttpClient<IBrandService, BrandService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddHttpClient<IColorService, ColorService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddHttpClient<ICarXColorService, CarXColorService>();
builder.Services.AddScoped<ICarXColorService, CarXColorService>();
builder.Services.AddHttpClient<ICarService, CarService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddHttpClient<ICarSpecificationService, CarSpecificationService>();
builder.Services.AddScoped<ICarSpecificationService, CarSpecificationService>();
builder.Services.AddHttpClient<IFeatureTypeService, FeatureTypeService>();
builder.Services.AddScoped<IFeatureTypeService, FeatureTypeService>();
builder.Services.AddHttpClient<IFeatureService, FeatureService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddHttpClient<IDealerService, DealerService>();
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddHttpClient<IVariantService, VariantService>();
builder.Services.AddScoped<IVariantService, VariantService>();
builder.Services.AddHttpClient<ICityService, CityService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddHttpClient<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddHttpClient<IStateService, StateService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddHttpClient<ICarXFeatureService, CarXFeatureService>();
builder.Services.AddScoped<ICarXFeatureService, CarXFeatureService>();
builder.Services.AddHttpClient<IMileageService, MileageService>();
builder.Services.AddScoped<IMileageService, MileageService>();
builder.Services.AddHttpClient<ICarImageService, CarImageService>();
builder.Services.AddScoped<ICarImageService, CarImageService>();
builder.Services.AddHttpClient<IFeatureXFeaturetypeService, FeatureXFeaturetypeService>();
builder.Services.AddScoped<IFeatureXFeaturetypeService, FeatureXFeaturetypeService>();
builder.Services.AddHttpClient<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddHttpClient<IReviewXCommentService, ReviewXCommentService>();
builder.Services.AddScoped<IReviewXCommentService, ReviewXCommentService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			  .AddCookie(options =>
			  {
				  options.Cookie.HttpOnly = true;
				  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				  options.LoginPath = "/Customer/Auth/Login";
				  options.AccessDeniedPath = "/Customer/Auth/AccessDenied";
				  options.SlidingExpiration = true;
			  });

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(100);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

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
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();