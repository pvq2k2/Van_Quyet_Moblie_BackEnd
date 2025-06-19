# Văn Quyết Mobile Project

Văn Quyết Mobile là một dự án Web API sử dụng ASP.NET Core, được xây dựng với kiến trúc **Clean Architecture**. Dự án sử dụng các lớp phân tách rõ ràng theo các tầng **Domain**, **Application**, **Infrastructure**, và **WebApi**, giúp duy trì tính mở rộng và dễ bảo trì.

---

## Kiến trúc dự án

Dự án được xây dựng dựa trên mô hình **Clean Architecture** (hay Onion Architecture), tách biệt các thành phần chính của hệ thống thành các layer độc lập.

---

## Công nghệ sử dụng

- **.NET 9**: Framework chính
- **Entity Framework Core**: ORM
- **ASP.NET Core**: Phát triển Web API
- **Swagger**: Tài liệu hóa API
- **IdentityServer4 / ASP.NET Core Identity**: Xác thực và phân quyền
- **AutoMapper**: Object mapping

---

## Các tính năng chính

- **Clean Architecture**: Tách biệt rõ layer
- **Modular**: Dễ dàng mở rộng
- **Authentication & Authorization**: JWT, IdentityServer4
- **Logging & Caching**: Serilog, MemoryCache, Redis
- **Testable**: Dễ viết unit test

---

## Cài đặt

### 1. Clone dự án

```bash
git clone https://github.com/pvq2k2/Van_Quyet_Moblie_BackEnd
cd Van_Quyet_Moblie_BackEnd
```

### 2. Cài đặt các dependency

```bash
dotnet restore
```

### 3. Chạy ứng dụng

```bash
dotnet run --project src/WebApi
```

### 4. Kiểm tra

Chạy unit tests:

```bash
dotnet test
```

---

## Đóng góp

Nếu bạn muốn đóng góp vào dự án này:

1. Fork dự án
2. Tạo branch mới cho tính năng/sửa lỗi
3. Gửi Pull Request
