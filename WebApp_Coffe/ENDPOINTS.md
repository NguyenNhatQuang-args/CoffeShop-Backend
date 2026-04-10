## Tổng quan
- Tổng số endpoints: 26
- Public: 11 | Admin: 15
- Có implement đầy đủ: 26 | Còn thiếu: 0

## Chi tiết theo nhóm

### 1. Auth
| Method | Route | Auth Required? | Request | Response | Description |
| :--- | :--- | :--- | :--- | :--- | :--- |
| POST | `/api/auth/login` | Public | Body: `LoginRequest` | `ApiResponse<AuthResponse>` | Đăng nhập và set JWT vào HttpOnly Cookie |
| POST | `/api/auth/refresh` | Public | Cookie: `refreshToken` | `ApiResponse<AuthResponse>` | Làm mới Access/Refresh token dựa trên refresh token cookie |
| POST | `/api/auth/logout` | Public | N/A | `ApiResponse<object>` | Xóa Access/Refresh Cookie khỏi trình duyệt |

### 2. Category
| Method | Route | Auth Required? | Request | Response | Description |
| :--- | :--- | :--- | :--- | :--- | :--- |
| GET | `/api/categories` | Public | N/A | `ApiResponse<List<CategoryResponse>>` | Lấy danh sách danh mục (chỉ các mục Active) |
| GET | `/api/categories/{slug}` | Public | Path: `slug` | `ApiResponse<CategoryDetailResponse>` | Lấy chi tiết danh mục kèm sản phẩm |
| POST | `/api/admin/categories` | Admin | Form: `CategoryRequest` (kèm file) | `ApiResponse<CategoryResponse>` | Tạo mới danh mục và upload ảnh |
| PUT | `/api/admin/categories/{id}` | Admin | Path: `id`, Form: `CategoryRequest` | `ApiResponse<CategoryResponse>` | Cập nhật thông tin/ảnh danh mục |
| DELETE | `/api/admin/categories/{id}` | Admin | Path: `id` | `ApiResponse<bool>` | Xóa danh mục |

### 3. Product
| Method | Route | Auth Required? | Request | Response | Description |
| :--- | :--- | :--- | :--- | :--- | :--- |
| GET | `/api/products` | Public | Query: `categoryId`, `tag`, `search`, `isFeature`, `page`, `pageSize` | `ApiResponse<PagedResult<ProductResponse>>` | Lấy danh sách sản phẩm có phân trang và lọc |
| GET | `/api/products/featured` | Public | N/A | `ApiResponse<List<ProductResponse>>` | Lấy 10 sản phẩm nổi bật mới nhất |
| GET | `/api/products/{slug}` | Public | Path: `slug` | `ApiResponse<ProductDetailResponse>` | Lấy chi tiết sản phẩm kèm variants và tags |
| POST | `/api/admin/products` | Admin | Form: `ProductRequest` (kèm file) | `ApiResponse<ProductResponse>` | Tạo mới sản phẩm và upload ảnh |
| PUT | `/api/admin/products/{id}` | Admin | Path: `id`, Form: `ProductRequest` | `ApiResponse<ProductResponse>` | Cập nhật thông tin/ảnh sản phẩm |
| DELETE | `/api/admin/products/{id}` | Admin | Path: `id` | `ApiResponse<bool>` | Xóa sản phẩm |

### 4. Blog
| Method | Route | Auth Required? | Request | Response | Description |
| :--- | :--- | :--- | :--- | :--- | :--- |
| GET | `/api/blog` | Public | Query: `page`, `pageSize` | `ApiResponse<PagedResult<BlogPostResponse>>` | Lấy danh sách bài viết đã publish (có phân trang) |
| GET | `/api/blog/{slug}` | Public | Path: `slug` | `ApiResponse<BlogPostDetailResponse>` | Lấy chi tiết bài viết |
| POST | `/api/admin/blog` | Admin | Form: `BlogPostRequest` (kèm file) | `ApiResponse<BlogPostResponse>` | Tạo mới bài viết và upload thumbnail |
| PUT | `/api/admin/blog/{id}` | Admin | Path: `id`, Form: `BlogPostRequest` | `ApiResponse<BlogPostResponse>` | Cập nhật bài viết |
| DELETE | `/api/admin/blog/{id}` | Admin | Path: `id` | `ApiResponse<bool>` | Xóa bài viết |

### 5. Store
| Method | Route | Auth Required? | Request | Response | Description |
| :--- | :--- | :--- | :--- | :--- | :--- |
| GET | `/api/stores` | Public | N/A | `ApiResponse<List<StoreResponse>>` | Lấy danh sách các chi nhánh (chỉ Active) |
| POST | `/api/admin/stores` | Admin | Body: `StoreRequest` | `ApiResponse<StoreResponse>` | Tạo mới chi nhánh |
| PUT | `/api/admin/stores/{id}` | Admin | Path: `id`, Body: `StoreRequest` | `ApiResponse<StoreResponse>` | Cập nhật thông tin chi nhánh |
| DELETE | `/api/admin/stores/{id}` | Admin | Path: `id` | `ApiResponse<bool>` | Xóa chi nhánh |

### 6. Upload (Manual)
| Method | Route | Auth Required? | Request | Response | Description |
| :--- | :--- | :--- | :--- | :--- | :--- |
| POST | `/api/admin/upload/product` | Admin | Form: `file` | `ApiResponse<string>` | Tải ảnh sản phẩm rời lên storage |
| POST | `/api/admin/upload/category` | Admin | Form: `file` | `ApiResponse<string>` | Tải ảnh danh mục rời lên storage |
| POST | `/api/admin/upload/blog` | Admin | Form: `file` | `ApiResponse<string>` | Tải ảnh blog thumbnail rời lên storage |
| DELETE | `/api/admin/upload` | Admin | Query: `fileUrl` | `ApiResponse<bool>` | Xóa file trực tiếp theo đường dẫn |

## Danh sách vấn đề phát hiện & Đã xử lý (Fixes Applied)
- [x] Đã xử lý: **Thiếu tính năng DELETE cho Blog**: Đã tạo `[HttpDelete("{id}")]` trong `AdminBlogController` và `DeleteAsync` trong `BlogService`.
- [x] Đã xử lý: **Thiếu tính năng Quản lý Store**: Đã bổ sung `AdminStoresController` kèm 3 hàm C.R.U.D: POST, PUT, DELETE và tạo mới `StoreRequest` DTO. Đã cập nhật `IStoreService` và `StoreService`.
- [x] Đã xử lý: **Refresh Token logic**: Đã cập nhật `User` entity có column `RefreshToken` + `RefreshTokenExpiryTime`. Implement logic tạo token, xác thực qua database, renew Access Token và Refresh Token, sau đó đẩy xuống trình duyệt dưới dạng `HttpOnly Cookie`.

## Recommended fix order
Tất cả các vấn đề ưu tiên trong Static Analysis đều đã được fix hoàn thiện 100%.
