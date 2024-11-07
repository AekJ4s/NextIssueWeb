using NextIssueWeb.Data;
using NextIssueWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// ลงทะเบียนบริการกับ DI
builder.Services.AddControllersWithViews();
// ลงทะเบียน DbContext ของคุณ
builder.Services.AddDbContext<NextIssueContext>();
// ลงทะเบียน AccountSv และ LoggerSv
builder.Services.AddScoped<AccountSv>();
builder.Services.AddScoped<LoggerSv>();
builder.Services.AddScoped<ProjectSv>();
builder.Services.AddScoped<StatusSv>();


builder.Services.AddDistributedMemoryCache(); // ใช้สำหรับเก็บข้อมูล Session ในหน่วยความจำ

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ตั้งค่าเวลาหมดอายุของ Session
    options.Cookie.HttpOnly = true; // ป้องกันการเข้าถึง Session ผ่าน JavaScript
    options.Cookie.IsEssential = true; // ทำให้ Session เป็นสิ่งจำเป็น
});
builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// เพิ่มการตั้งค่า Status Code Pages
app.UseStatusCodePagesWithReExecute("/Home/NOTFOUND");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// เรียกใช้ Session middleware
app.UseSession(); // เพิ่มบรรทัดนี้เพื่อเปิดใช้งาน Session

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
