using NextIssueWeb.Data;
using NextIssueWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// ŧ����¹��ԡ�áѺ DI
builder.Services.AddControllersWithViews();
// ŧ����¹ DbContext �ͧ�س
builder.Services.AddDbContext<NextIssueContext>();
// ŧ����¹ AccountSv ��� LoggerSv
builder.Services.AddScoped<AccountSv>();
builder.Services.AddScoped<LoggerSv>();
builder.Services.AddScoped<ProjectSv>();
builder.Services.AddScoped<StatusSv>();


builder.Services.AddDistributedMemoryCache(); // ������Ѻ�红����� Session �˹��¤�����

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ��駤������������آͧ Session
    options.Cookie.HttpOnly = true; // ��ͧ�ѹ�����Ҷ֧ Session ��ҹ JavaScript
    options.Cookie.IsEssential = true; // ����� Session ����觨���
});
builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// ������õ�駤�� Status Code Pages
app.UseStatusCodePagesWithReExecute("/Home/NOTFOUND");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ���¡�� Session middleware
app.UseSession(); // ������÷Ѵ��������Դ��ҹ Session

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
