using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ABC.Client.Components;
using ABC.Client.Components.Account;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using ABC.Shared.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<POSService_SQL>();
builder.Services.AddScoped<ProductService_SQL>();
builder.Services.AddScoped<CategoryService_SQL>();
builder.Services.AddScoped<SupplierService_SQL>();
builder.Services.AddScoped<StoreService_SQL>();
builder.Services.AddScoped<ShoppingCartService_SQL>();
builder.Services.AddScoped<ApplicationUserService_SQL>();
builder.Services.AddScoped<OrderHeaderService_SQL>();
builder.Services.AddScoped<CustomerService_SQL>();
builder.Services.AddScoped<ContentService_SQL>();
builder.Services.AddScoped<AuditService_SQL>();
builder.Services.AddScoped<PurchaseOrderService_SQL>();
builder.Services.AddScoped<StockTransferService_SQL>();
builder.Services.AddScoped<PdfService>();
//builder.Services.AddTransient<EmailSender>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

AppSettingsHelper.SetConfig(builder.Configuration);
AppSettingsHelper.EnableLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
