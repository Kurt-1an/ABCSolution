using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ABC.Client.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Privacy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VissionMission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    returnRefund = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<long>(type: "bigint", nullable: false),
                    ApSuUn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetorSubd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Barangay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    storeContactNumber = table.Column<long>(type: "bigint", nullable: false),
                    storeEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    storeStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    storeUnitNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    storeStreetSubdv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    storeBarangay = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    storeCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    storeProvince = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    storeZipCode = table.Column<int>(type: "int", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplierCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierContactNumber = table.Column<long>(type: "bigint", nullable: false),
                    supplierEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierUnitNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierStreetSubdv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierBarangay = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    supplierCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierProvince = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    supplierZipCode = table.Column<int>(type: "int", nullable: false),
                    supplierNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartSalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSalesDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<long>(type: "bigint", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostPrice = table.Column<float>(type: "real", nullable: false),
                    RetailPrice = table.Column<float>(type: "real", nullable: false),
                    WarrantyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<long>(type: "bigint", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentTerm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OrderTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Failed = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierAudits_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderTotal = table.Column<double>(type: "float", nullable: false),
                    AmountTendered = table.Column<double>(type: "float", nullable: true),
                    OfficialReceipt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefundedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderHeaders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransferCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceStoreId = table.Column<int>(type: "int", nullable: false),
                    DestinationStoreId = table.Column<int>(type: "int", nullable: false),
                    applicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferRemarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransfers_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Stores_DestinationStoreId",
                        column: x => x.DestinationStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Stores_SourceStoreId",
                        column: x => x.SourceStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Failed = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAudits_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockPerStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Store1StockQty = table.Column<int>(type: "int", nullable: false),
                    MinimumStore1StockQty = table.Column<int>(type: "int", nullable: false),
                    Store2StockQty = table.Column<int>(type: "int", nullable: false),
                    MinimumStore2StockQty = table.Column<int>(type: "int", nullable: false),
                    TotalStocks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPerStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockPerStores_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Charge = table.Column<double>(type: "float", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    RetailPrice = table.Column<double>(type: "float", nullable: false),
                    CostPrice = table.Column<double>(type: "float", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    remarkQty = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferItemDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockTransferId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    subTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferItemDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransferItemDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransferItemDetails_StockTransfers_StockTransferId",
                        column: x => x.StockTransferId,
                        principalTable: "StockTransfers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockTransferAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Failed = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StockPerStoreId = table.Column<int>(type: "int", nullable: false),
                    SourceStoreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescitnationStoreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferredStocks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransferAudit_StockPerStores_StockPerStoreId",
                        column: x => x.StockPerStoreId,
                        principalTable: "StockPerStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7ed397ef-a19d-4a20-a59c-9fd97cd6e9da", null, "Admin", "ADMIN" },
                    { "a5916c5a-4609-436a-9bbc-d450be6d3c42", null, "Manager", "MANAGER" },
                    { "f451925b-5627-4a0c-a3e5-9ee8b158edfc", null, "Customer", "CUSTOMER" },
                    { "f5818c2c-0543-43ad-a7a3-18316244dcd4", null, "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "Province", "SecurityStamp", "StoreId", "StoreName", "TimeStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0e9f9406-7100-496c-946f-2055b08ab847", 0, null, null, "6e0b1d16-edf3-400c-91b2-8589dc21fef3", "cust@abc.com", true, "Ej Customer", "Esan", false, null, null, "CUST@ABC.COM", "AQAAAAIAAYagAAAAEC6ylsRIVhqGkmGgI4N8L38JCQsEFnnP0/GwxVrgS6FFq1yubaWQ4aRtJ8J2siFM1Q==", null, false, null, null, "4052c534-9e3f-4ea7-8c18-33109eebcfff", null, null, new DateTime(2024, 5, 3, 4, 17, 47, 402, DateTimeKind.Utc).AddTicks(9941), false, "cust@abc.com" },
                    { "41f6010f-3a33-4c67-a759-be56331d018e", 0, null, null, "629efbbd-a4ae-406b-ba49-f7c39d13ceda", "emp@abc.com", true, "Ej Employee", "Esan", false, null, null, "EMP@ABC.COM", "AQAAAAIAAYagAAAAEFRJm5YpTd1/pvcQHaB1EWJdyeLka5dsht9l0FwgqnPkKkhnQ/RlmijzT0BFL0jSZw==", null, false, null, null, "bc2fde3a-4b7b-4d45-97b9-7dbe61180a68", null, null, new DateTime(2024, 5, 3, 4, 17, 47, 329, DateTimeKind.Utc).AddTicks(3063), false, "emp@abc.com" },
                    { "dda73560-4332-4ddd-bba6-2de302fed50a", 0, null, null, "e9387b07-cd69-4bcc-a45b-bf79df214748", "manager@abc.com", true, "Ej Manager", "Esan", false, null, null, "MANAGER@ABC.COM", "AQAAAAIAAYagAAAAEJKG2f+u+TiIfwizAT9jXaxHFzKcdBj0YDTjwDmPEpicHNU8n8VI9Z4YUOQC80bmtg==", null, false, null, null, "63e78ba3-46c6-4132-a5ba-3899a44bf48a", null, null, new DateTime(2024, 5, 3, 4, 17, 47, 254, DateTimeKind.Utc).AddTicks(9138), false, "manager@abc.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "status" },
                values: new object[,]
                {
                    { 1, "CCTV", null },
                    { 2, "Printers", null },
                    { 3, "Computer Accesories", null },
                    { 4, "Cables & Tools", null }
                });

            migrationBuilder.InsertData(
                table: "Contents",
                columns: new[] { "Id", "About", "Privacy", "TermsPolicy", "VissionMission", "returnRefund" },
                values: new object[] { 1, "Our company formerly known as ABC Computer was established year 1990. At present our new company ABC Company (Ahead Biz & Addsome Business Corp).  We are Located at Unit 303 3F., Vmall Greenhills San Juan who has experience more than two decades in sales and services in a well-known and best computer company in V-mall formerly Virramall in Greenhills San Juan Metro Manila. Which specializes on major sales and services on branded and various models of Laptops as well as the branded and clone personal computers either brand new or old model units. \r\n\r\nAside from the above, we are likewise specialized in repairing any kind of printers and other computer peripherals. \r\n\r\nToday, our business is aiming to continue and handling CCTV or Closed Circuit Television to give support to our values clients and enhance the personnel's skill with regards to the best performance in installing CCTV's and latest features of CCTV's to satisfy the great demands in efficient and high tech tools. \r\n\r\nWhile there is an increasing demand for such equipment in the SME (small and medium enterprise) scale industries, CCTV has became the main tools of more companies to facilitate faster service in information storage, thus we also aim to give our clients the best benefits in using our products.", "At ABC Company, we value your privacy and are committed to protecting your personal information. Our privacy policy describes how we collect, use and protect the data you provide to us. We only collect information necessary to process your order, provide customer support, and improve our services. Please rest assured that we do not share your personal information with third parties without your consent, unless required by law. We use industry standard security measures to protect your data from unauthorized access or disclosure. For more details about our privacy practices, please refer to our Privacy Policy.", "By accessing and using ABC Company's website, you agree to comply with our Terms of Use. These Terms govern your use of our website, including browsing, purchasing products and interacting with our content. We strive to provide accurate and up-to-date information but we cannot guarantee the completeness or accuracy of the content on our site. Any reliance on the information provided is at your own risk. We reserve the right to modify or discontinue our website, products or services at any time without notice. For a detailed understanding of our terms of use, please refer to the dedicated section of our website.", "Vision: \r\nOur company is dedicated to analyze and provide suggestions and recommendations, install and maintain the comprehensive solutions to our clients concerns. However, partnership is also encouraged to interested parties including colleagues to penetrate the huge information technology market. \r\n\r\n\r\nMission: Our mission is to deliver superior electronic gadgets and repair solutions, backed by excellent customer service. We aim to exceed customer expectations by offering a wide range of high-quality products, employing skilled technicians, and continuously improving our services to stay at the forefront of technology.\r\n\r\n\r\nAt ABC Company, we are driven by our vision and mission to create a positive impact in the lives of our customers. Join us on this journey as we strive to make technology accessible and reliable for everyone.", "\r\nAt ABC Company, we want you to be completely satisfied with your purchase. If you are not satisfied with your product, we offer a return/replace policy. You may return the item within 7 days of purchase for a replacement, as long as the item is in its original packaging and condition. Please note that some products may be subject to specific return policies, such as software licenses or personalized items. For more information about our return policy and steps to initiate a return, please visit our website or contact our customer support team.\r\n" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "ApSuUn", "Barangay", "City", "ContactNumber", "EmailAddress", "FirstName", "LastName", "Province", "StreetorSubd", "ZipCode" },
                values: new object[] { new Guid("2b2068a8-30c0-49f9-9f8f-86e384138d9a"), "Unit 1234", "Batman", "Antipolo", 9568271611L, "neiljejomar@gmail.com", "Kurt", "Betonio", "Rizal", "Jasmine St.", 1870 });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "storeBarangay", "storeCity", "storeContactNumber", "storeEmail", "storeName", "storeProvince", "storeStatus", "storeStreetSubdv", "storeUnitNumber", "storeZipCode" },
                values: new object[,]
                {
                    { 1, "Maybancal", "Tanay", 9651232235L, "abiz214@gmail.com", "Addsome Business Corporation", "Rizal", "Active", "E. Corazon", "c4 l5", 1870 },
                    { 2, "Maybancal", "Tanay", 9651232235L, "abiz214@gmail.com", "Ahead Biz Computers", "Rizal", "Active", "E. Corazon", "c4 l5", 1870 }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Timestamp", "supplierBarangay", "supplierCity", "supplierCompanyName", "supplierContactNumber", "supplierEmail", "supplierNote", "supplierProvince", "supplierStatus", "supplierStreetSubdv", "supplierUnitNumber", "supplierZipCode" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 3, 4, 17, 47, 476, DateTimeKind.Utc).AddTicks(3426), "Maybancal", "Tanay", "Addvert", 9651232235L, "addvert214@gmail.com", "My supplier", "Rizal", "Active", "E. Corazon", "c4 l5", 1870 },
                    { 2, new DateTime(2024, 5, 3, 4, 17, 47, 476, DateTimeKind.Utc).AddTicks(3430), "Maybancal", "Tanay", "Addvert", 9651232235L, "addvert214@gmail.com", "My supplier", "Rizal", "Active", "E. Corazon", "c4 l5", 1870 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f451925b-5627-4a0c-a3e5-9ee8b158edfc", "0e9f9406-7100-496c-946f-2055b08ab847" },
                    { "f5818c2c-0543-43ad-a7a3-18316244dcd4", "41f6010f-3a33-4c67-a759-be56331d018e" },
                    { "a5916c5a-4609-436a-9bbc-d450be6d3c42", "dda73560-4332-4ddd-bba6-2de302fed50a" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "Province", "SecurityStamp", "StoreId", "StoreName", "TimeStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "60d36645-b307-452a-b852-56f383b7b828", 0, null, null, "3ba770e0-35c8-44ab-9f7f-02b8bf75ca1f", "admin@abc.com", true, "Ej Admin", "Esan", false, null, null, "ADMIN@ABC.COM", "AQAAAAIAAYagAAAAENlbPCd5hlyt08w+LLFoadwM068ZSdecwPGLQYWD/0YZjMrkVPjMxxxh/IPeXW0d3A==", null, false, null, null, "d8d94ae5-fa34-4009-b48e-f292ffc80bad", 1, "Addsome Business Corporation", new DateTime(2024, 5, 3, 4, 17, 47, 183, DateTimeKind.Utc).AddTicks(393), false, "admin@abc.com" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "Brand", "CategoryId", "CostPrice", "Description", "Duration", "EndSalesDate", "ImageUrl", "IsRemoved", "RetailPrice", "SKU", "StartSalesDate", "SupplierId", "WarrantyType", "productName", "status" },
                values: new object[,]
                {
                    { 1, 832175698L, "HP", 1, 800f, "Versatile all-in-one printer for printing, copying, and scanning", "12 months from date of purchase", null, "", false, 1299f, "printer-AllInOne-XYZ123", new DateTime(2024, 5, 3, 4, 17, 47, 476, DateTimeKind.Utc).AddTicks(3334), 2, "Extended Warranty", "XYZ123 All-in-One Printer", "In Stock" },
                    { 2, 954532414L, "Samsung", 2, 1200f, "Panoramic view with motion detection", "7 days from date of purchase", null, "", false, 1999f, "cctv-SmartCam-360", new DateTime(2024, 5, 3, 4, 17, 47, 476, DateTimeKind.Utc).AddTicks(3339), 1, "Manufacturers Warranty", "SmartCam 360 Security Camera", "In Stock" },
                    { 3, 123456789013L, "Dell", 4, 600f, "Lightweight 13-inch laptop with SSD and 8GB RAM", "12 months from date of purchase", null, "", false, 899f, "laptop-ultrabook-ABC789", new DateTime(2024, 5, 3, 4, 17, 47, 476, DateTimeKind.Utc).AddTicks(3341), 2, "Extended Warranty", "ABC789 13-inch Laptop", "In Stock" },
                    { 4, 123456789014L, "Apple", 4, 500f, "5.8-inch OLED smartphone with dual camera", "24 months from date of purchase", null, "", false, 999f, "phone-smartphone-XYZ101", new DateTime(2024, 5, 3, 4, 17, 47, 476, DateTimeKind.Utc).AddTicks(3344), 1, "Extended Warranty", "XYZ101 Smartphone", "In Stock" },
                    { 5, 123456789015L, "Bose", 4, 150f, "Noise cancelling wireless over-ear headphones", "12 months from date of purchase", null, "", false, 249f, "headphones-wireless-XYZ222", new DateTime(2024, 5, 3, 4, 17, 47, 476, DateTimeKind.Utc).AddTicks(3346), 2, "Extended Warranty", "XYZ222 Wireless Headphones", "In Stock" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7ed397ef-a19d-4a20-a59c-9fd97cd6e9da", "60d36645-b307-452a-b852-56f383b7b828" });

            migrationBuilder.InsertData(
                table: "StockPerStores",
                columns: new[] { "Id", "MinimumStore1StockQty", "MinimumStore2StockQty", "ProductId", "Store1StockQty", "Store2StockQty", "TotalStocks" },
                values: new object[,]
                {
                    { 1, 2, 2, 1, 15, 5, 20 },
                    { 2, 2, 2, 2, 7, 8, 15 },
                    { 3, 2, 2, 3, 5, 3, 8 },
                    { 4, 2, 2, 4, 6, 6, 12 },
                    { 5, 2, 2, 5, 17, 3, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StoreId",
                table: "AspNetUsers",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_CustomerId",
                table: "OrderHeaders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAudits_ProductId",
                table: "ProductAudits",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_ProductId",
                table: "PurchaseOrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_StoreId",
                table: "PurchaseOrders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ApplicationUserId",
                table: "ShoppingCarts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPerStores_ProductId",
                table: "StockPerStores",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferAudit_StockPerStoreId",
                table: "StockTransferAudit",
                column: "StockPerStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferItemDetails_ProductId",
                table: "StockTransferItemDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferItemDetails_StockTransferId",
                table: "StockTransferItemDetails",
                column: "StockTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_applicationUserId",
                table: "StockTransfers",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_DestinationStoreId",
                table: "StockTransfers",
                column: "DestinationStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_SourceStoreId",
                table: "StockTransfers",
                column: "SourceStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierAudits_SupplierId",
                table: "SupplierAudits",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductAudits");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "StockTransferAudit");

            migrationBuilder.DropTable(
                name: "StockTransferItemDetails");

            migrationBuilder.DropTable(
                name: "SupplierAudits");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "StockPerStores");

            migrationBuilder.DropTable(
                name: "StockTransfers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
