# ATHEA - Thời Trang Nữ Cao Cấp

Chào mừng bạn đến với kho lưu trữ mã nguồn của dự án **ATHEA** - Nền tảng thương mại điện tử chuyên cung cấp các sản phẩm thời trang nữ cao cấp. Ứng dụng cung cấp trải nghiệm mua sắm tuyệt vời cho khách hàng và hệ thống quản lý toàn diện cho ban quản trị.

## 🌟 Tổng Quan Dự Án

**Kho lưu trữ mã nguồn:** [Athea-FashionShop trên GitHub](https://github.com/meou2k4/Athea-FashionShop)

Dự án được xây dựng theo kiến trúc hiện đại, phân tách rõ ràng giữa Frontend (Client) và Backend (API).

- **Kiến trúc:** Client-Server, Web API
- **Phân hệ Backend:** ASP.NET Core Web API (C#)
- **Phân hệ Frontend:** React (JavaScript/JSX) kết hợp Vite
- **Hệ cơ sở dữ liệu:** Microsoft SQL Server (FashionDb) bằng Entity Framework Core
- **Xác thực:** JSON Web Token (JWT) cho cả User và Admin

## 📁 Cấu Trúc Mã Nguồn (Repository Structure)

Solution bao gồm 3 dự án chính:

1. **`FashionShop.Api`**: Dự án RESTful Web API đóng vai trò là Backend. Xử lý logic nghiệp vụ, quản lý xác thực bằng JWT, cung cấp dữ liệu qua API cho Client và gửi Email tự động.
2. **`FashionShop.Data`**: Dự án Class Library chứa mã thao tác với cơ sở dữ liệu. Định nghĩa các Entities (Models) và DBContext thông qua Entity Framework Core.
3. **`FashionShop.Web`**: Dự án Frontend SPA (Single Page Application) sử dụng React. Giao diện người dùng dành cho đối tượng khách hàng tham quan, mua sắm và trang Admin Dashboards dành cho người quản trị cửa hàng.

## 🚀 Các Tính Năng Nổi Bật

**Phía Khách Hàng (Public 面):**
- **Trang chủ tĩnh mượt mà**: Banner quảng bá (New Arrivals, Sale Off), phần hiển thị danh sách sản phẩm đẹp mắt.
- **Duyệt và tìm kiếm sản phẩm**: Xem chi tiết, thuộc tính màu sắc, kích cỡ, phân loại sản phẩm.
- **Giỏ hàng & Thanh toán**: Trải nghiệm thêm/xóa giỏ hàng và thanh toán tiện lợi (Hỗ trợ tích hợp PayPal/VNPay tương lai).
- **Trang giới thiệu / Liên hệ**: Thông tin thương hiệu ATHEA, vị trí cửa hàng, hotline, tích hợp trực tiếp link email/tổng đài.

**Phía Quản Trị Hệ Thống (Admin 面):**
- **Quản lý danh mục & Sản phẩm**: Thêm mới, chỉnh sửa thông tin, giá, thuộc tính cũng như cập nhật hình ảnh.
- **Quản lý biến thể**: Quản lý song song size và màu sắc trên cùng một sản phẩm cha.
- **Cấu hình cửa hàng (Settings)**: Cho phép thay đổi hotline, email liên hệ, đường dẫn Zalo, Facebook động không cần can thiệp source code.
- **Quản lý đơn hàng**: Giao diện cập nhật tiến độ đơn hàng và theo dõi doanh thu.

## 🛠️ Yêu Cầu Cài Đặt (Prerequisites)

- [Node.js](https://nodejs.org/) (Khuyến nghị bản LTS)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (Hoặc version phù hợp tùy theo `.csproj`)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express hoặc Developer)
- Trình quản lý gói: `npm` hoặc `yarn`

## ⚙️ Hướng Dẫn Chạy Dự Án (Getting Started)

### Bước 1: Thiết lập Database
Dự án sử dụng EF Core Code-First. Hãy đảm bảo bạn đã cài đặt sẵn SQL Server trên máy.
1. Mở Package Manager Console trong Visual Studio.
2. Thiết lập Default project thành `FashionShop.Data` (hoặc `FashionShop.Api` nếu config migration ở đây).
3. Chạy lệnh: `Update-Database`. Hệ thống sẽ tự tạo DB `FashionDb`.

### Bước 2: Chạy Backend (API)
Mở dự án `FashionShop.Api` qua Visual Studio và chọn cấu hình chạy (HTTP hoặc HTTPS).
```bash
cd FashionShop.Api
dotnet run
```
API sẽ khởi chạy trên port mặc định, ví dụ: `http://localhost:5086`.

**Lưu ý:** Bạn cần tùy chỉnh file `appsettings.Development.json` (Không push lên Git) chứa tài khoản cấu hình Mail Sender thật và JWT Key của riêng bạn.

### Bước 3: Chạy Frontend (Web)
Chuyển hướng terminal vào thư mục web và khởi chạy NPM.
```bash
cd FashionShop.Web
npm install
npm run dev
```
Mở trình duyệt và truy cập vào đường dẫn thiết lập (thường là `http://localhost:5173` do Vite cấp hoặc `3000`).

## 🛡️ Bảo Mật & Đóng Góp
- File `appsettings.json` hiện tại đã được tách và sử dụng placeholder để đảm bảo không rò rỉ JWT Key hay thông tin Password. Đừng quên thiết lập thông tin bí mật vào `appsettings.Development.json` thuộc máy cá nhân hoặc biến môi trường ở Server.
- File rác `node_modules` và `.vs` đã được che giấu an toàn bằng `.gitignore`.

---
*Dự án ATHEA - Nâng tầm nét đẹp thời trang*
