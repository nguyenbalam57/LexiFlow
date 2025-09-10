# ?? H??NG D?N T?O MIGRATION CHO LEXIFLOW

## T?ng quan các Script

### ?? Danh sách Scripts

1. **`validate-lexiflow-context.ps1`** - Ki?m tra và s?a l?i LexiFlowContext
2. **`fix-lexiflow-migration-errors.ps1`** - X? lý l?i migration chuyên sâu
3. **`create-lexiflow-migration.ps1`** - T?o migration hoàn ch?nh
4. **`create-lexiflow-migration.bat`** - Version batch file ??n gi?n

---

## ?? Quy trình s? d?ng khuy?n ngh?

### B??c 1: Validation tr??c khi t?o Migration

```powershell
# Ki?m tra LexiFlowContext có l?i không
.\validate-lexiflow-context.ps1

# N?u có l?i, t? ??ng s?a
.\validate-lexiflow-context.ps1 -AutoFix

# Xem báo cáo chi ti?t
.\validate-lexiflow-context.ps1 -DetailedReport
```

### B??c 2: X? lý l?i Migration (n?u c?n)

```powershell
# S?a l?i Foreign Key constraints
.\fix-lexiflow-migration-errors.ps1 -FixForeignKeys -AutoFix

# S?a l?i Duplicate entities
.\fix-lexiflow-migration-errors.ps1 -FixDuplicates -AutoFix

# Reset toàn b? migrations (C?NH BÁO: Xóa t?t c?)
.\fix-lexiflow-migration-errors.ps1 -ResetMigrations

# Ch?y t?t c? fixes
.\fix-lexiflow-migration-errors.ps1 -AutoFix -FixForeignKeys -FixDuplicates
```

### B??c 3: T?o Migration

#### Option A: PowerShell Script (Khuy?n ngh?)
```powershell
# T?o migration v?i tên m?c ??nh
.\create-lexiflow-migration.ps1

# T?o migration v?i tên tùy ch?nh
.\create-lexiflow-migration.ps1 -MigrationName "MyCustomMigration"

# T?o migration và update database ngay
.\create-lexiflow-migration.ps1 -MigrationName "InitialCreate" -UpdateDatabase

# T?o migration, update database và seed data
.\create-lexiflow-migration.ps1 -MigrationName "InitialCreate" -UpdateDatabase -SeedData

# B? qua validation (nhanh h?n)
.\create-lexiflow-migration.ps1 -SkipValidation

# Force th?c hi?n dù có l?i
.\create-lexiflow-migration.ps1 -Force
```

#### Option B: Batch File (Windows)
```batch
REM T?o migration v?i tên m?c ??nh
create-lexiflow-migration.bat

REM T?o migration v?i tên tùy ch?nh
create-lexiflow-migration.bat "MyCustomMigration"

REM B? qua build check
create-lexiflow-migration.bat "MyMigration" skipbuild
```

---

## ?? Các tình hu?ng th??ng g?p

### ? Tình hu?ng 1: L?n ??u t?o Migration

```powershell
# 1. Validate context
.\validate-lexiflow-context.ps1 -AutoFix

# 2. T?o initial migration
.\create-lexiflow-migration.ps1 -MigrationName "InitialCreate" -UpdateDatabase -SeedData
```

### ? Tình hu?ng 2: Migration b? l?i Foreign Key

```powershell
# 1. S?a l?i foreign key
.\fix-lexiflow-migration-errors.ps1 -FixForeignKeys -AutoFix

# 2. T?o migration m?i
.\create-lexiflow-migration.ps1 -MigrationName "FixForeignKeys"
```

### ? Tình hu?ng 3: Có Migration c? gây xung ??t

```powershell
# Option A: Reset toàn b? (C?NH BÁO: M?t d? li?u)
.\fix-lexiflow-migration-errors.ps1 -ResetMigrations
.\create-lexiflow-migration.ps1 -MigrationName "FreshStart"

# Option B: Backup và t?o m?i
.\create-lexiflow-migration.ps1 -MigrationName "NewMigration" -Force
```

### ? Tình hu?ng 4: Database Connection Error

```powershell
# Ki?m tra và s?a connection issues
.\fix-lexiflow-migration-errors.ps1

# Ki?m tra SQL LocalDB
sqllocaldb info

# Start LocalDB n?u c?n
sqllocaldb start MSSQLLocalDB
```

### ? Tình hu?ng 5: Build Error tr??c Migration

```powershell
# Ki?m tra và s?a build errors
.\validate-lexiflow-context.ps1 -AutoFix

# Build manual ?? xem l?i chi ti?t
dotnet build LexiFlow.Infrastructure --verbosity detailed

# T?o migration v?i skip validation
.\create-lexiflow-migration.ps1 -SkipValidation
```

---

## ?? Các Parameters quan tr?ng

### validate-lexiflow-context.ps1
- `-AutoFix`: T? ??ng s?a các l?i phát hi?n
- `-DetailedReport`: Hi?n th? báo cáo chi ti?t

### fix-lexiflow-migration-errors.ps1
- `-AutoFix`: T? ??ng áp d?ng fixes
- `-ResetMigrations`: Xóa t?t c? migrations (NGUY HI?M)
- `-FixForeignKeys`: S?a l?i foreign key constraints
- `-FixDuplicates`: S?a l?i duplicate entities

### create-lexiflow-migration.ps1
- `-MigrationName`: Tên migration (m?c ??nh: "LexiFlowInitialMigration")
- `-SkipValidation`: B? qua validation ?? t?ng t?c
- `-Force`: Ti?p t?c dù có l?i
- `-UpdateDatabase`: T? ??ng update database sau khi t?o migration
- `-SeedData`: Seed data sau khi update database

---

## ?? Debugging và Troubleshooting

### Ki?m tra Log chi ti?t

```powershell
# Migration v?i verbose logging
dotnet ef migrations add "TestMigration" --project LexiFlow.Infrastructure --startup-project LexiFlow.API --verbose

# Database update v?i verbose logging
dotnet ef database update --project LexiFlow.Infrastructure --startup-project LexiFlow.API --verbose
```

### Ki?m tra Database hi?n t?i

```powershell
# Xem migrations ?ã áp d?ng
dotnet ef migrations list --project LexiFlow.Infrastructure --startup-project LexiFlow.API

# Xem SQL s? ???c generate
dotnet ef migrations script --project LexiFlow.Infrastructure --startup-project LexiFlow.API
```

### Rollback Migration

```powershell
# Rollback v? migration tr??c
dotnet ef database update PreviousMigrationName --project LexiFlow.Infrastructure --startup-project LexiFlow.API

# Remove migration ch?a apply
dotnet ef migrations remove --project LexiFlow.Infrastructure --startup-project LexiFlow.API
```

---

## ?? C?u trúc Files sau khi Migration

```
LexiFlow.Infrastructure/
??? Migrations/
?   ??? 20241204120000_InitialCreate.cs
?   ??? 20241204120000_InitialCreate.Designer.cs
?   ??? LexiFlowContextModelSnapshot.cs
??? Data/
?   ??? LexiFlowContext.cs
?   ??? Seed/
??? ...
```

---

## ?? C?nh báo quan tr?ng

### ?? NGUY HI?M - Các l?nh có th? m?t d? li?u
```powershell
# XÓA T?T C? MIGRATIONS
.\fix-lexiflow-migration-errors.ps1 -ResetMigrations

# XÓA DATABASE
dotnet ef database drop --force --project LexiFlow.Infrastructure --startup-project LexiFlow.API
```

### ? AN TOÀN - Luôn backup tr??c
- Scripts t? ??ng t?o backup v?i timestamp
- Backup location: `*.backup.yyyyMMdd_HHmmss`
- Test trên database development tr??c

---

## ?? H? tr? và FAQ

### Q: Migration t?o thành công nh?ng không update ???c database?
```powershell
# Ki?m tra connection string
.\fix-lexiflow-migration-errors.ps1

# Update database manual
dotnet ef database update --project LexiFlow.Infrastructure --startup-project LexiFlow.API --verbose
```

### Q: L?i "pending model changes"?
```powershell
# Reset và t?o l?i
.\fix-lexiflow-migration-errors.ps1 -ResetMigrations
.\create-lexiflow-migration.ps1 -MigrationName "FreshMigration"
```

### Q: L?i foreign key constraint?
```powershell
# T? ??ng s?a
.\fix-lexiflow-migration-errors.ps1 -FixForeignKeys -AutoFix
```

### Q: Build error tr??c khi t?o migration?
```powershell
# Validate và fix
.\validate-lexiflow-context.ps1 -AutoFix

# Ho?c skip validation
.\create-lexiflow-migration.ps1 -SkipValidation
```

---

## ?? Best Practices

1. **Luôn validate tr??c khi t?o migration**
2. **Backup d? li?u quan tr?ng**
3. **Test migration trên development database**
4. **Review migration code tr??c khi deploy**
5. **S? d?ng meaningful migration names**
6. **Keep migrations nh? và focused**

---

## ?? Migration Success Checklist

- [ ] ? LexiFlowContext validation passed
- [ ] ? Build successful
- [ ] ? Migration created without errors
- [ ] ? Database updated successfully
- [ ] ? Seed data completed
- [ ] ? Application starts without errors
- [ ] ? Swagger UI accessible
- [ ] ? Basic API calls working

---

**Happy Migrating! ??**