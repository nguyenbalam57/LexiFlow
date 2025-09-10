# LexiFlowContext - Complete Entity Framework Context

## T?ng quan
?ã t?o file `LexiFlowContext.cs` hoàn ch?nh cho Entity Framework v?i t?t c? các tính n?ng c?n thi?t cho ?ng d?ng LexiFlow.

## ??c ?i?m chính

### 1. DbSets - Các b?ng d? li?u
File context bao g?m t?t c? các DbSet cho:

#### User Management
- `Users` - Ng??i dùng
- `UserProfiles` - Thông tin h? s? ng??i dùng
- `UserLearningPreferences` - Thi?t l?p h?c t?p cá nhân
- `UserNotificationSettings` - Cài ??t thông báo
- `Roles` - Vai trò
- `Permissions` - Quy?n h?n
- `UserRoles`, `UserPermissions`, `RolePermissions` - B?ng liên k?t
- `Departments`, `Teams`, `Groups` - T? ch?c

#### Learning Content
- `Categories` - Danh m?c t? v?ng
- `Vocabularies` - T? v?ng
- `Definitions`, `Examples`, `Translations` - Chi ti?t t? v?ng
- `Kanjis` - Ch? Hán và các b?ng liên quan
- `Grammars` - Ng? pháp và các b?ng liên quan
- `TechnicalTerms` - Thu?t ng? k? thu?t

#### Media Management
- `MediaFiles` - File media
- `MediaCategories` - Danh m?c media
- `MediaProcessingHistories` - L?ch s? x? lý

#### Progress & Learning
- `LearningProgresses` - Ti?n trình h?c t?p
- `LearningSessions` - Phiên h?c t?p
- `PersonalWordLists` - Danh sách t? cá nhân
- Các b?ng ti?n trình khác

#### Study Planning
- `StudyPlans` - K? ho?ch h?c t?p
- `StudyGoals` - M?c tiêu h?c t?p
- `StudyTasks` - Nhi?m v? h?c t?p
- Các b?ng l?p k? ho?ch khác

#### Exams & Practice
- `Questions` - Câu h?i
- `TestResults` - K?t qu? bài test
- `JLPTExams` - K? thi JLPT
- Các b?ng thi c? khác

#### Notifications
- `Notifications` - Thông báo
- `NotificationRecipients` - Ng??i nh?n thông báo
- Các b?ng thông báo khác

#### Scheduling
- `Schedules` - L?ch trình
- `ScheduleItems` - M?c l?ch trình
- Các b?ng l?p l?ch khác

#### System & Sync
- `Settings` - Cài ??t h? th?ng
- `DeletedItems` - L?u tr? d? li?u ?ã xóa
- `SyncMetadata` - Metadata ??ng b?
- `SyncConflicts` - Xung ??t ??ng b?

### 2. Tính n?ng Audit Trail
- T? ??ng c?p nh?t `CreatedAt`, `UpdatedAt` cho t?t c? entities
- T? ??ng set `CreatedBy`, `ModifiedBy` cho `AuditableEntity`
- Ph??ng th?c `SetCurrentUserId()` ?? thi?t l?p user hi?n t?i

### 3. Tính n?ng Soft Delete
- Query filters t? ??ng cho các entities có soft delete
- Ph??ng th?c `BulkSoftDeleteAsync()` ?? xóa m?m hàng lo?t
- Ph??ng th?c `GetActiveEntities<T>()` ?? l?y entities còn ho?t ??ng

### 4. C?u hình Model
- **T?t cascade delete** toàn c?c ?? tránh xung ??t
- **C?u hình indexes** cho các tr??ng quan tr?ng
- **C?u hình relationships** ?úng theo thi?t k?
- **C?u hình entity configurations** cho t?ng module

### 5. Data Seeding
- Kh?i t?o d? li?u m?u cho Roles
- Kh?i t?o Categories m?c ??nh
- Kh?i t?o Settings h? th?ng
- Ph??ng th?c `SeedDataAsync()` có th? m? r?ng

### 6. Performance & Logging
- **Debug logging** cho development
- **Sensitive data logging** khi debug
- **Warning configuration** ?? gi?m log noise
- **Bulk operations** support

## Cách s? d?ng

### 1. Dependency Injection
```csharp
services.AddDbContext<LexiFlowContext>(options =>
    options.UseSqlServer(connectionString));
```

### 2. S? d?ng trong Controller
```csharp
public class VocabularyController : ControllerBase
{
    private readonly LexiFlowContext _context;
    
    public VocabularyController(LexiFlowContext context)
    {
        _context = context;
        _context.SetCurrentUserId(currentUserId);
    }
}
```

### 3. Migration
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Data Seeding
```csharp
await _context.SeedDataAsync();
```

## Tính t??ng thích
- **.NET 9** compatible
- **Entity Framework Core 9** support
- **SQL Server** optimized
- **Async/await** throughout

## Ki?n trúc
- **Clean Architecture** principles
- **Domain-driven design** approach
- **Repository pattern** ready
- **Unit of Work** compatible

## B?o trì
- **Type-safe** configuration
- **Strongly-typed** entities
- **Compile-time** error checking
- **IntelliSense** support

## Tóm l?i
LexiFlowContext ?ã ???c t?o hoàn ch?nh v?i:
? T?t c? DbSets c?n thi?t
? Relationships ???c c?u hình ?úng
? Audit trail t? ??ng
? Soft delete support
? Performance optimizations
? Data seeding
? Error-free compilation
? .NET 9 ready

Context này s?n sàng ?? s? d?ng cho vi?c phát tri?n ?ng d?ng LexiFlow v?i Entity Framework Core.