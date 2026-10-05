# WebShopping — Website bán hàng thời trang

Đồ án ASP.NET Core MVC: cửa hàng thời trang **Karl** (client) + quản trị **Startmin** (admin).
Sinh viên: **Tạ Nguyễn Đăng Khoa** — MSSV `0306241457`

## Công nghệ

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core + SQL Server (LocalDB)
- Cookie Authentication
- Session (giỏ hàng)

## Chạy dự án

1. Cài [.NET SDK](https://dotnet.microsoft.com/download) và SQL Server LocalDB.
2. Mở thư mục `WebShopping`:

```bash
cd WebShopping
dotnet restore
dotnet ef database update
dotnet run
```

Lần chạy đầu, ứng dụng tự `Migrate()` và seed dữ liệu mẫu (danh mục, sản phẩm, tài khoản admin).
Mở trình duyệt theo cổng trong `Properties/launchSettings.json` (mặc định `http://localhost:5015`).

## Tài khoản admin

| Username | Password | Vai trò |
| -------- | -------- | ------- |
| `admin`  | `123456` | Admin   |

Đăng nhập: `/Account/Login`. Khu vực `/Admin` bắt buộc đăng nhập.

## Chức năng

**Client**

- Trang chủ: 9 sản phẩm mới nhất
- Danh sách / lọc theo danh mục
- Chi tiết sản phẩm + sản phẩm liên quan
- Giỏ hàng (Session): thêm, sửa số lượng, xóa
- Đặt hàng: lưu `Order` + `OrderDetail` (đơn giá snapshot lúc đặt)
  **Admin**
- CRUD danh mục
- CRUD sản phẩm (upload ảnh vào `wwwroot/client/img/product-img/`)
- Danh sách đơn hàng, chi tiết, cập nhật trạng thái (`Pending` / `Shipping` / `Completed` / `Cancelled`)

## Cơ sở dữ liệu

Chuỗi kết nối trong `appsettings.json`:

```
Server=(localdb)\MSSQLLocalDB;Database=WebShopping_Db;Trusted_Connection=True;TrustServerCertificate=True;
```

Script SQL đầy đủ: `Scripts/WebShopping_Db.sql`.

## Cấu trúc

```
Areas/Admin/     Layout Startmin, CRUD Category/Product/Order
Controllers/     Home, Product, Cart, Account
Models/          Category, Product, Order, OrderDetail, User
wwwroot/client   Template Karl
wwwroot/admin    Template Startmin
```

## Ghi chú

Mật khẩu admin đang lưu dạng chữ (theo yêu cầu bài lab). Khi triển khai thật nên băm mật khẩu.
