# LexiFlowContext - Complete Entity Framework Context

## T?ng quan
?� t?o file `LexiFlowContext.cs` ho�n ch?nh cho Entity Framework v?i t?t c? c�c t�nh n?ng c?n thi?t cho ?ng d?ng LexiFlow.

## ??c ?i?m ch�nh

### 1. DbSets - C�c b?ng d? li?u
File context bao g?m t?t c? c�c DbSet cho:

#### User Management
- `Users` - Ng??i d�ng
- `UserProfiles` - Th�ng tin h? s? ng??i d�ng
- `UserLearningPreferences` - Thi?t l?p h?c t?p c� nh�n
- `UserNotificationSettings` - C�i ??t th�ng b�o
- `Roles` - Vai tr�
- `Permissions` - Quy?n h?n
- `UserRoles`, `UserPermissions`, `RolePermissions` - B?ng li�n k?t
- `Departments`, `Teams`, `Groups` - T? ch?c

#### Learning Content
- `Categories` - Danh m?c t? v?ng
- `Vocabularies` - T? v?ng
- `Definitions`, `Examples`, `Translations` - Chi ti?t t? v?ng
- `Kanjis` - Ch? H�n v� c�c b?ng li�n quan
- `Grammars` - Ng? ph�p v� c�c b?ng li�n quan
- `TechnicalTerms` - Thu?t ng? k? thu?t

#### Media Management
- `MediaFiles` - File media
- `MediaCategories` - Danh m?c media
- `MediaProcessingHistories` - L?ch s? x? l�

#### Progress & Learning
- `LearningProgresses` - Ti?n tr�nh h?c t?p
- `LearningSessions` - Phi�n h?c t?p
- `PersonalWordLists` - Danh s�ch t? c� nh�n
- C�c b?ng ti?n tr�nh kh�c

#### Study Planning
- `StudyPlans` - K? ho?ch h?c t?p
- `StudyGoals` - M?c ti�u h?c t?p
- `StudyTasks` - Nhi?m v? h?c t?p
- C�c b?ng l?p k? ho?ch kh�c

#### Exams & Practice
- `Questions` - C�u h?i
- `TestResults` - K?t qu? b�i test
- `JLPTExams` - K? thi JLPT
- C�c b?ng thi c? kh�c

#### Notifications
- `Notifications` - Th�ng b�o
- `NotificationRecipients` - Ng??i nh?n th�ng b�o
- C�c b?ng th�ng b�o kh�c

#### Scheduling
- `Schedules` - L?ch tr�nh
- `ScheduleItems` - M?c l?ch tr�nh
- C�c b?ng l?p l?ch kh�c

#### System & Sync
- `Settings` - C�i ??t h? th?ng
- `DeletedItems` - L?u tr? d? li?u ?� x�a
- `SyncMetadata` - Metadata ??ng b?
- `SyncConflicts` - Xung ??t ??ng b?

### 2. T�nh n?ng Audit Trail
- T? ??ng c?p nh?t `CreatedAt`, `UpdatedAt` cho t?t c? entities
- T? ??ng set `CreatedBy`, `ModifiedBy` cho `AuditableEntity`
- Ph??ng th?c `SetCurrentUserId()` ?? thi?t l?p user hi?n t?i

### 3. T�nh n?ng Soft Delete
- Query filters t? ??ng cho c�c entities c� soft delete
- Ph??ng th?c `BulkSoftDeleteAsync()` ?? x�a m?m h�ng lo?t
- Ph??ng th?c `GetActiveEntities<T>()` ?? l?y entities c�n ho?t ??ng

### 4. C?u h�nh Model
- **T?t cascade delete** to�n c?c ?? tr�nh xung ??t
- **C?u h�nh indexes** cho c�c tr??ng quan tr?ng
- **C?u h�nh relationships** ?�ng theo thi?t k?
- **C?u h�nh entity configurations** cho t?ng module

### 5. Data Seeding
- Kh?i t?o d? li?u m?u cho Roles
- Kh?i t?o Categories m?c ??nh
- Kh?i t?o Settings h? th?ng
- Ph??ng th?c `SeedDataAsync()` c� th? m? r?ng

### 6. Performance & Logging
- **Debug logging** cho development
- **Sensitive data logging** khi debug
- **Warning configuration** ?? gi?m log noise
- **Bulk operations** support

## C�ch s? d?ng

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

## T�nh t??ng th�ch
- **.NET 9** compatible
- **Entity Framework Core 9** support
- **SQL Server** optimized
- **Async/await** throughout

## Ki?n tr�c
- **Clean Architecture** principles
- **Domain-driven design** approach
- **Repository pattern** ready
- **Unit of Work** compatible

## B?o tr�
- **Type-safe** configuration
- **Strongly-typed** entities
- **Compile-time** error checking
- **IntelliSense** support

## T�m l?i
LexiFlowContext ?� ???c t?o ho�n ch?nh v?i:
? T?t c? DbSets c?n thi?t
? Relationships ???c c?u h�nh ?�ng
? Audit trail t? ??ng
? Soft delete support
? Performance optimizations
? Data seeding
? Error-free compilation
? .NET 9 ready

Context n�y s?n s�ng ?? s? d?ng cho vi?c ph�t tri?n ?ng d?ng LexiFlow v?i Entity Framework Core.