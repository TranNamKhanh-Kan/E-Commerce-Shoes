# E-Commerce Shoes API

ASP.NET Core 8 Web API (3 lớp: API / BAL / DAL) cho cửa hàng bán giày.

## Setup

1. Chạy script SQL: `Database/Schema.sql` trên SQL Server (tạo DB, bảng User/Product/Order/Cart + seed Role).
2. Kiểm tra connection string trong `API-E-Commerce-Shoes/appsettings.json`.
3. Chạy project:

```bash
dotnet run --project API-E-Commerce-Shoes
```

4. Mở Swagger: `http://localhost:5285/swagger`

## Roles

| RoleId | Name |
|--------|------|
| 1 | Admin |
| 2 | Staff |
| 3 | Customer (mặc định khi register) |

## API Endpoints

### User (`/api/User`)

| Method | Path | Auth | Mô tả |
|--------|------|------|-------|
| GET | `/role` | Public | Danh sách role |
| POST | `/register` | Public | Đăng ký |
| POST | `/login` | Public | Đăng nhập + JWT |
| GET | `/me` | JWT | User hiện tại |
| GET | `/get-user-by-email?email=` | JWT | Tìm theo email |
| GET | `/get-user-by-id/{id}` | JWT | Tìm theo id |
| PUT | `/update-user` | JWT | Cập nhật user |
| GET | `/get-all-user` | Admin (1) | Tất cả user |

### Product (`/api/Product`)

| Method | Path | Auth | Mô tả |
|--------|------|------|-------|
| GET | `/get-all-product` | Public | Tất cả sản phẩm |
| GET | `/get-product-by-id/{id}` | Public | Chi tiết |
| GET | `/search?keyword=&type=&status=` | Public | Tìm kiếm / lọc |
| POST | `/create-product` | Admin/Staff | Tạo sản phẩm |
| PUT | `/update-product/{id}` | Admin/Staff | Cập nhật |
| DELETE | `/delete-product/{id}` | Admin | Xóa / soft-delete |

### Cart (`/api/Cart`)

| Method | Path | Auth | Mô tả |
|--------|------|------|-------|
| GET | `/get-cart?userId=` | JWT | Xem giỏ |
| POST | `/add-to-cart` | JWT | Thêm vào giỏ |
| PUT | `/update-item` | JWT | Đổi số lượng |
| DELETE | `/remove-item?userId=&productId=` | JWT | Xóa item |
| DELETE | `/clear-cart?userId=` | JWT | Xóa hết giỏ |
| POST | `/checkout` | JWT | Thanh toán → tạo Order |

### Order (`/api/Order`)

| Method | Path | Auth | Mô tả |
|--------|------|------|-------|
| GET | `/get-all-order` | Admin/Staff | Tất cả đơn |
| GET | `/get-order-by-id/{id}` | JWT | Chi tiết đơn |
| GET | `/get-order-by-user-id?id=` | JWT | Đơn theo user |
| POST | `/create-order` | JWT | Tạo đơn trực tiếp |
| PUT | `/update-order/{id}` | Admin/Staff | Cập nhật status/address |
| PUT | `/cancel-order/{id}` | JWT | Hủy đơn (hoàn stock) |

## Order status gợi ý

`Pending` → `Confirmed` → `Shipping` → `Completed` | `Cancelled`

## Product status gợi ý

`Active` | `Inactive` | `OutOfStock`

## Auth Swagger

Sau khi login, copy token và dán vào Authorize dạng: `Bearer {token}`
