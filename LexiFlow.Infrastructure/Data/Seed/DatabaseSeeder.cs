using LexiFlow.Models.Learning.Vocabulary;
using LexiFlow.Models.User;
using LexiFlow.Models.User.UserRelations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data.Seed
{
    /// <summary>
    /// Database seeding utility
    /// </summary>
    public class DatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly LexiFlowContext _context;

        public DatabaseSeeder(
            ILogger<DatabaseSeeder> logger,
            LexiFlowContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Seed initial data to the database
        /// </summary>
        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");

                await SeedRolesAndPermissionsAsync();
                await SeedAdminUserAsync();
                await SeedCategoriesAsync();
                // Thêm các phương thức seed khác theo nhu cầu

                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        /// <summary>
        /// Seed roles and permissions
        /// </summary>
        private async Task SeedRolesAndPermissionsAsync()
        {
            if (!await _context.Roles.AnyAsync())
            {
                _logger.LogInformation("Seeding roles...");

                var roles = new List<Role>
        {
            // Vai trò hệ thống
            new Role { RoleName = "Administrator", Description = "Quản trị viên hệ thống - toàn quyền", IsActive = true },
            new Role { RoleName = "ContentManager", Description = "Quản lý nội dung tổng thể", IsActive = true },
            new Role { RoleName = "Teacher", Description = "Giáo viên/Người tạo nội dung", IsActive = true },
            new Role { RoleName = "Student", Description = "Học viên/Người học", IsActive = true },
            new Role { RoleName = "Guest", Description = "Người dùng khách với quyền truy cập hạn chế", IsActive = true },
            
            // Vai trò theo bộ phận công ty
            new Role { RoleName = "DepartmentManager", Description = "Quản lý bộ phận", IsActive = true },
            new Role { RoleName = "LearningDepartment", Description = "Nhân viên bộ phận học tập", IsActive = true },
            new Role { RoleName = "NonLearningDepartment", Description = "Nhân viên bộ phận không học tập", IsActive = true },
            new Role { RoleName = "ProgressTracker", Description = "Nhân viên theo dõi tiến độ học tập", IsActive = true },
            
            // Vai trò theo nhu cầu học tập
            new Role { RoleName = "FullLearner", Description = "Người học toàn diện", IsActive = true },
            new Role { RoleName = "SelfGuidedLearner", Description = "Người học không cần lộ trình", IsActive = true },
            new Role { RoleName = "CasualLearner", Description = "Người học lướt qua", IsActive = true },
            new Role { RoleName = "ViewOnlyUser", Description = "Người dùng chỉ xem, không ghi nhớ", IsActive = true },
            
            // Vai trò quản lý nội dung
            new Role { RoleName = "VocabularyManagerA", Description = "Quản lý từ vựng nhóm A (JLPT N4-N5)", IsActive = true },
            new Role { RoleName = "VocabularyManagerB", Description = "Quản lý từ vựng nhóm B (JLPT N1-N3)", IsActive = true },
            new Role { RoleName = "GrammarManager", Description = "Quản lý ngữ pháp", IsActive = true },
            new Role { RoleName = "KanjiManager", Description = "Quản lý Kanji", IsActive = true },
            new Role { RoleName = "ExamManager", Description = "Quản lý kỳ thi", IsActive = true },
            new Role { RoleName = "PracticeManager", Description = "Quản lý bài tập thực hành", IsActive = true },
            new Role { RoleName = "TechnicalTermManager", Description = "Quản lý thuật ngữ chuyên ngành", IsActive = true },

             // Vai trò cấp bậc trong công ty
            new Role { RoleName = "Management", Description = "Cấp quản lý cao cấp, có quyền giám sát và quản lý toàn công ty", IsActive = true },
            new Role { RoleName = "Leader", Description = "Trưởng nhóm, quản lý trực tiếp nhóm nhỏ", IsActive = true },
            new Role { RoleName = "Engineer", Description = "Kỹ sư/Chuyên viên, nhân viên chuyên môn", IsActive = true },
            new Role { RoleName = "Staff", Description = "Nhân viên chính thức, thực hiện các công việc thông thường", IsActive = true },
            new Role { RoleName = "Junior", Description = "Nhân viên mới/Junior, đang trong quá trình học tập và phát triển", IsActive = true },
            new Role { RoleName = "Star", Description = "Nhân viên xuất sắc, có thành tích và kỹ năng nổi bật", IsActive = true }
        };

                await _context.Roles.AddRangeAsync(roles);
                await _context.SaveChangesAsync();
            }

            if (!await _context.Permissions.AnyAsync())
            {
                _logger.LogInformation("Seeding permissions...");

                var permissions = new List<Permission>
        {
            // Quyền quản lý người dùng
            new Permission { PermissionName = "user.view", Description = "Xem thông tin người dùng", IsActive = true },
            new Permission { PermissionName = "user.create", Description = "Tạo người dùng mới", IsActive = true },
            new Permission { PermissionName = "user.edit", Description = "Chỉnh sửa thông tin người dùng", IsActive = true },
            new Permission { PermissionName = "user.delete", Description = "Xóa người dùng", IsActive = true },
            new Permission { PermissionName = "user.manage_permissions", Description = "Quản lý quyền của người dùng", IsActive = true },
            new Permission { PermissionName = "user.view_progress", Description = "Xem tiến độ học tập của người dùng", IsActive = true },
            new Permission { PermissionName = "user.reset_password", Description = "Đặt lại mật khẩu cho người dùng", IsActive = true },
            
            // Quyền quản lý vai trò
            new Permission { PermissionName = "role.view", Description = "Xem vai trò", IsActive = true },
            new Permission { PermissionName = "role.create", Description = "Tạo vai trò mới", IsActive = true },
            new Permission { PermissionName = "role.edit", Description = "Chỉnh sửa vai trò", IsActive = true },
            new Permission { PermissionName = "role.delete", Description = "Xóa vai trò", IsActive = true },
            new Permission { PermissionName = "role.assign", Description = "Gán vai trò cho người dùng", IsActive = true },
            
            // Quyền quản lý bộ phận
            new Permission { PermissionName = "department.view", Description = "Xem bộ phận", IsActive = true },
            new Permission { PermissionName = "department.create", Description = "Tạo bộ phận mới", IsActive = true },
            new Permission { PermissionName = "department.edit", Description = "Chỉnh sửa bộ phận", IsActive = true },
            new Permission { PermissionName = "department.delete", Description = "Xóa bộ phận", IsActive = true },
            new Permission { PermissionName = "department.assign_user", Description = "Gán người dùng vào bộ phận", IsActive = true },
            
            // Quyền quản lý nội dung chung
            new Permission { PermissionName = "content.view", Description = "Xem nội dung học tập", IsActive = true },
            new Permission { PermissionName = "content.create", Description = "Tạo nội dung học tập", IsActive = true },
            new Permission { PermissionName = "content.edit", Description = "Chỉnh sửa nội dung học tập", IsActive = true },
            new Permission { PermissionName = "content.delete", Description = "Xóa nội dung học tập", IsActive = true },
            new Permission { PermissionName = "content.approve", Description = "Phê duyệt nội dung học tập", IsActive = true },
            new Permission { PermissionName = "content.reject", Description = "Từ chối nội dung học tập", IsActive = true },
            new Permission { PermissionName = "content.publish", Description = "Xuất bản nội dung học tập", IsActive = true },
            
            // Quyền quản lý từ vựng
            new Permission { PermissionName = "vocabulary.view", Description = "Xem từ vựng", IsActive = true },
            new Permission { PermissionName = "vocabulary.create", Description = "Tạo từ vựng mới", IsActive = true },
            new Permission { PermissionName = "vocabulary.edit", Description = "Chỉnh sửa từ vựng", IsActive = true },
            new Permission { PermissionName = "vocabulary.delete", Description = "Xóa từ vựng", IsActive = true },
            new Permission { PermissionName = "vocabulary.import", Description = "Nhập từ vựng từ tệp", IsActive = true },
            new Permission { PermissionName = "vocabulary.export", Description = "Xuất từ vựng ra tệp", IsActive = true },
            new Permission { PermissionName = "vocabulary.group_a", Description = "Quản lý từ vựng nhóm A (N4-N5)", IsActive = true },
            new Permission { PermissionName = "vocabulary.group_b", Description = "Quản lý từ vựng nhóm B (N1-N3)", IsActive = true },
            
            // Quyền quản lý Kanji
            new Permission { PermissionName = "kanji.view", Description = "Xem Kanji", IsActive = true },
            new Permission { PermissionName = "kanji.create", Description = "Tạo Kanji mới", IsActive = true },
            new Permission { PermissionName = "kanji.edit", Description = "Chỉnh sửa Kanji", IsActive = true },
            new Permission { PermissionName = "kanji.delete", Description = "Xóa Kanji", IsActive = true },
            new Permission { PermissionName = "kanji.import", Description = "Nhập Kanji từ tệp", IsActive = true },
            new Permission { PermissionName = "kanji.export", Description = "Xuất Kanji ra tệp", IsActive = true },
            
            // Quyền quản lý ngữ pháp
            new Permission { PermissionName = "grammar.view", Description = "Xem ngữ pháp", IsActive = true },
            new Permission { PermissionName = "grammar.create", Description = "Tạo ngữ pháp mới", IsActive = true },
            new Permission { PermissionName = "grammar.edit", Description = "Chỉnh sửa ngữ pháp", IsActive = true },
            new Permission { PermissionName = "grammar.delete", Description = "Xóa ngữ pháp", IsActive = true },
            new Permission { PermissionName = "grammar.import", Description = "Nhập ngữ pháp từ tệp", IsActive = true },
            new Permission { PermissionName = "grammar.export", Description = "Xuất ngữ pháp ra tệp", IsActive = true },
            
            // Quyền quản lý thuật ngữ chuyên ngành
            new Permission { PermissionName = "term.view", Description = "Xem thuật ngữ chuyên ngành", IsActive = true },
            new Permission { PermissionName = "term.create", Description = "Tạo thuật ngữ chuyên ngành mới", IsActive = true },
            new Permission { PermissionName = "term.edit", Description = "Chỉnh sửa thuật ngữ chuyên ngành", IsActive = true },
            new Permission { PermissionName = "term.delete", Description = "Xóa thuật ngữ chuyên ngành", IsActive = true },
            new Permission { PermissionName = "term.import", Description = "Nhập thuật ngữ chuyên ngành từ tệp", IsActive = true },
            new Permission { PermissionName = "term.export", Description = "Xuất thuật ngữ chuyên ngành ra tệp", IsActive = true },
            
            // Quyền quản lý kỳ thi
            new Permission { PermissionName = "exam.view", Description = "Xem kỳ thi", IsActive = true },
            new Permission { PermissionName = "exam.create", Description = "Tạo kỳ thi mới", IsActive = true },
            new Permission { PermissionName = "exam.edit", Description = "Chỉnh sửa kỳ thi", IsActive = true },
            new Permission { PermissionName = "exam.delete", Description = "Xóa kỳ thi", IsActive = true },
            new Permission { PermissionName = "exam.assign", Description = "Gán kỳ thi cho người dùng", IsActive = true },
            new Permission { PermissionName = "exam.grade", Description = "Chấm điểm kỳ thi", IsActive = true },
            new Permission { PermissionName = "exam.view_results", Description = "Xem kết quả kỳ thi", IsActive = true },
            
            // Quyền quản lý bài tập thực hành
            new Permission { PermissionName = "practice.view", Description = "Xem bài tập thực hành", IsActive = true },
            new Permission { PermissionName = "practice.create", Description = "Tạo bài tập thực hành mới", IsActive = true },
            new Permission { PermissionName = "practice.edit", Description = "Chỉnh sửa bài tập thực hành", IsActive = true },
            new Permission { PermissionName = "practice.delete", Description = "Xóa bài tập thực hành", IsActive = true },
            new Permission { PermissionName = "practice.assign", Description = "Gán bài tập thực hành cho người dùng", IsActive = true },
            
            // Quyền học tập
            new Permission { PermissionName = "learning.access", Description = "Truy cập tính năng học tập", IsActive = true },
            new Permission { PermissionName = "learning.vocabulary", Description = "Học từ vựng", IsActive = true },
            new Permission { PermissionName = "learning.kanji", Description = "Học Kanji", IsActive = true },
            new Permission { PermissionName = "learning.grammar", Description = "Học ngữ pháp", IsActive = true },
            new Permission { PermissionName = "learning.term", Description = "Học thuật ngữ chuyên ngành", IsActive = true },
            new Permission { PermissionName = "learning.practice", Description = "Thực hành bài tập", IsActive = true },
            new Permission { PermissionName = "learning.test", Description = "Làm bài kiểm tra", IsActive = true },
            new Permission { PermissionName = "learning.track_progress", Description = "Theo dõi tiến độ học tập cá nhân", IsActive = true },
            new Permission { PermissionName = "learning.create_plan", Description = "Tạo kế hoạch học tập cá nhân", IsActive = true },
            new Permission { PermissionName = "learning.memorize", Description = "Sử dụng tính năng ghi nhớ", IsActive = true },
            
            // Quyền quản lý tiến độ học tập
            new Permission { PermissionName = "progress.view_own", Description = "Xem tiến độ học tập cá nhân", IsActive = true },
            new Permission { PermissionName = "progress.view_department", Description = "Xem tiến độ học tập của bộ phận", IsActive = true },
            new Permission { PermissionName = "progress.view_all", Description = "Xem tiến độ học tập của tất cả người dùng", IsActive = true },
            new Permission { PermissionName = "progress.report", Description = "Tạo báo cáo tiến độ học tập", IsActive = true },
            
            // Quyền quản lý gamification
            new Permission { PermissionName = "gamification.manage", Description = "Quản lý hệ thống gamification", IsActive = true },
            new Permission { PermissionName = "gamification.create_challenge", Description = "Tạo thử thách mới", IsActive = true },
            new Permission { PermissionName = "gamification.create_badge", Description = "Tạo huy hiệu mới", IsActive = true },
            new Permission { PermissionName = "gamification.award", Description = "Trao thưởng cho người dùng", IsActive = true },
            
            // Quyền quản lý hệ thống
            new Permission { PermissionName = "system.settings", Description = "Quản lý cài đặt hệ thống", IsActive = true },
            new Permission { PermissionName = "system.backup", Description = "Sao lưu hệ thống", IsActive = true },
            new Permission { PermissionName = "system.restore", Description = "Khôi phục hệ thống", IsActive = true },
            new Permission { PermissionName = "system.logs", Description = "Xem nhật ký hệ thống", IsActive = true },
            new Permission { PermissionName = "system.maintenance", Description = "Thực hiện bảo trì hệ thống", IsActive = true }
        };

                await _context.Permissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();
            }

            if (!await _context.RolePermissions.AnyAsync())
            {
                _logger.LogInformation("Assigning permissions to roles...");

                // Lấy danh sách vai trò
                var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Administrator");
                var contentManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "ContentManager");
                var teacherRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Teacher");
                var studentRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Student");
                var guestRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Guest");
                var departmentManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "DepartmentManager");
                var learningDeptRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "LearningDepartment");
                var nonLearningDeptRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "NonLearningDepartment");
                var progressTrackerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "ProgressTracker");
                var fullLearnerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "FullLearner");
                var selfGuidedLearnerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "SelfGuidedLearner");
                var casualLearnerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "CasualLearner");
                var viewOnlyRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "ViewOnlyUser");
                var vocabManagerARole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "VocabularyManagerA");
                var vocabManagerBRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "VocabularyManagerB");
                var grammarManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "GrammarManager");
                var kanjiManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "KanjiManager");
                var examManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "ExamManager");
                var practiceManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "PracticeManager");
                var termManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "TechnicalTermManager");

                // Lấy các vai trò theo cấp bậc công ty
                var managementRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Management");
                var leaderRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Leader");
                var engineerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Engineer");
                var staffRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Staff");
                var juniorRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Junior");
                var starRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Star");

                // Lấy danh sách quyền
                var permissions = await _context.Permissions.ToListAsync();
                var rolePermissions = new List<RolePermission>();

                // Gán tất cả quyền cho Admin
                if (adminRole != null)
                {
                    foreach (var permission in permissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = adminRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Management
                if (managementRole != null)
                {
                    var managementPermissions = permissions.Where(p =>
                        p.PermissionName == "user.view" ||
                        p.PermissionName == "user.create" ||
                        p.PermissionName == "user.edit" ||
                        p.PermissionName == "role.view" ||
                        p.PermissionName == "role.assign" ||
                        p.PermissionName == "department.view" ||
                        p.PermissionName == "department.create" ||
                        p.PermissionName == "department.edit" ||
                        p.PermissionName == "department.assign_user" ||
                        p.PermissionName.StartsWith("progress.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.approve" ||
                        p.PermissionName == "content.publish" ||
                        p.PermissionName == "learning.access" ||
                        p.PermissionName == "system.settings" ||
                        p.PermissionName == "system.logs" ||
                        p.PermissionName == "gamification.manage" ||
                        p.PermissionName == "gamification.award");

                    foreach (var permission in managementPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = managementRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Leader
                if (leaderRole != null)
                {
                    var leaderPermissions = permissions.Where(p =>
                        p.PermissionName == "user.view" ||
                        p.PermissionName == "department.view" ||
                        p.PermissionName == "progress.view_department" ||
                        p.PermissionName == "progress.report" ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit" ||
                        p.PermissionName.StartsWith("learning.") ||
                        p.PermissionName == "exam.view" ||
                        p.PermissionName == "exam.assign" ||
                        p.PermissionName == "exam.view_results" ||
                        p.PermissionName == "practice.view" ||
                        p.PermissionName == "practice.assign" ||
                        p.PermissionName == "gamification.create_challenge");

                    foreach (var permission in leaderPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = leaderRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Engineer
                if (engineerRole != null)
                {
                    var engineerPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName.StartsWith("learning.") ||
                        p.PermissionName == "progress.view_own" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "kanji.view" ||
                        p.PermissionName == "grammar.view" ||
                        p.PermissionName == "term.view" ||
                        p.PermissionName == "exam.view" ||
                        p.PermissionName == "practice.view" ||
                        p.PermissionName == "learning.create_plan");

                    foreach (var permission in engineerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = engineerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Staff (Nhân viên chính thức)
                if (staffRole != null)
                {
                    var staffPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "learning.access" ||
                        p.PermissionName == "learning.vocabulary" ||
                        p.PermissionName == "learning.kanji" ||
                        p.PermissionName == "learning.grammar" ||
                        p.PermissionName == "learning.term" ||
                        p.PermissionName == "learning.practice" ||
                        p.PermissionName == "learning.test" ||
                        p.PermissionName == "learning.track_progress" ||
                        p.PermissionName == "learning.memorize" ||
                        p.PermissionName == "progress.view_own" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "kanji.view" ||
                        p.PermissionName == "grammar.view" ||
                        p.PermissionName == "term.view" ||
                        p.PermissionName == "exam.view" ||
                        p.PermissionName == "practice.view");

                    foreach (var permission in staffPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = staffRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Junior
                if (juniorRole != null)
                {
                    var juniorPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "learning.access" ||
                        p.PermissionName == "learning.vocabulary" ||
                        p.PermissionName == "learning.kanji" ||
                        p.PermissionName == "learning.grammar" ||
                        p.PermissionName == "learning.term" ||
                        p.PermissionName == "learning.practice" ||
                        p.PermissionName == "learning.test" ||
                        p.PermissionName == "learning.track_progress" ||
                        p.PermissionName == "progress.view_own" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "kanji.view" ||
                        p.PermissionName == "grammar.view" ||
                        p.PermissionName == "term.view");

                    foreach (var permission in juniorPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = juniorRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Star
                if (starRole != null)
                {
                    var starPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName.StartsWith("learning.") ||
                        p.PermissionName == "progress.view_own" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "kanji.view" ||
                        p.PermissionName == "grammar.view" ||
                        p.PermissionName == "term.view" ||
                        p.PermissionName == "exam.view" ||
                        p.PermissionName == "practice.view" ||
                        p.PermissionName == "progress.view_department" ||
                        p.PermissionName == "gamification.create_challenge");

                    foreach (var permission in starPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = starRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Content Manager
                if (contentManagerRole != null)
                {
                    var contentManagerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("content.") ||
                        p.PermissionName.StartsWith("vocabulary.") ||
                        p.PermissionName.StartsWith("kanji.") ||
                        p.PermissionName.StartsWith("grammar.") ||
                        p.PermissionName.StartsWith("term.") ||
                        p.PermissionName.StartsWith("exam.") ||
                        p.PermissionName.StartsWith("practice.") ||
                        p.PermissionName == "user.view" ||
                        p.PermissionName == "progress.view_all" ||
                        p.PermissionName == "progress.report");

                    foreach (var permission in contentManagerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = contentManagerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Teacher
                if (teacherRole != null)
                {
                    var teacherPermissions = permissions.Where(p =>
                        p.PermissionName == "user.view" ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit" ||
                        p.PermissionName.StartsWith("vocabulary.view") ||
                        p.PermissionName.StartsWith("kanji.view") ||
                        p.PermissionName.StartsWith("grammar.view") ||
                        p.PermissionName.StartsWith("term.view") ||
                        p.PermissionName.StartsWith("exam.") ||
                        p.PermissionName.StartsWith("practice.") ||
                        p.PermissionName.StartsWith("learning.") ||
                        p.PermissionName == "progress.view_own" ||
                        p.PermissionName == "progress.view_department" ||
                        p.PermissionName == "progress.report");

                    foreach (var permission in teacherPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = teacherRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Student
                if (studentRole != null)
                {
                    var studentPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName.StartsWith("learning.") ||
                        p.PermissionName == "progress.view_own");

                    foreach (var permission in studentPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = studentRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Guest
                if (guestRole != null)
                {
                    var guestPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "learning.access" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "kanji.view" ||
                        p.PermissionName == "grammar.view");

                    foreach (var permission in guestPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = guestRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Department Manager
                if (departmentManagerRole != null)
                {
                    var departmentManagerPermissions = permissions.Where(p =>
                        p.PermissionName == "user.view" ||
                        p.PermissionName == "department.view" ||
                        p.PermissionName == "department.edit" ||
                        p.PermissionName == "department.assign_user" ||
                        p.PermissionName == "progress.view_department" ||
                        p.PermissionName == "progress.report" ||
                        p.PermissionName == "role.view" ||
                        p.PermissionName == "role.assign");

                    foreach (var permission in departmentManagerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = departmentManagerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Learning Department
                if (learningDeptRole != null)
                {
                    var learningDeptPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("learning.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "progress.view_own" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "kanji.view" ||
                        p.PermissionName == "grammar.view" ||
                        p.PermissionName == "term.view" ||
                        p.PermissionName == "exam.view" ||
                        p.PermissionName == "exam.view_results" ||
                        p.PermissionName == "practice.view");

                    foreach (var permission in learningDeptPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = learningDeptRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Non-Learning Department
                if (nonLearningDeptRole != null)
                {
                    var nonLearningDeptPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "term.view");

                    foreach (var permission in nonLearningDeptPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = nonLearningDeptRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Progress Tracker
                if (progressTrackerRole != null)
                {
                    var progressTrackerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("progress.") ||
                        p.PermissionName == "user.view" ||
                        p.PermissionName == "department.view");

                    foreach (var permission in progressTrackerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = progressTrackerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Full Learner
                if (fullLearnerRole != null)
                {
                    var fullLearnerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("learning.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "progress.view_own");

                    foreach (var permission in fullLearnerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = fullLearnerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Self-Guided Learner
                if (selfGuidedLearnerRole != null)
                {
                    var selfGuidedLearnerPermissions = permissions.Where(p =>
                        p.PermissionName == "learning.access" ||
                        p.PermissionName == "learning.vocabulary" ||
                        p.PermissionName == "learning.kanji" ||
                        p.PermissionName == "learning.grammar" ||
                        p.PermissionName == "learning.term" ||
                        p.PermissionName == "learning.practice" ||
                        p.PermissionName == "learning.test" ||
                        p.PermissionName == "learning.memorize" ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "progress.view_own");

                    foreach (var permission in selfGuidedLearnerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = selfGuidedLearnerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Casual Learner
                if (casualLearnerRole != null)
                {
                    var casualLearnerPermissions = permissions.Where(p =>
                        p.PermissionName == "learning.access" ||
                        p.PermissionName == "learning.vocabulary" ||
                        p.PermissionName == "learning.kanji" ||
                        p.PermissionName == "learning.grammar" ||
                        p.PermissionName == "content.view");

                    foreach (var permission in casualLearnerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = casualLearnerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho View-Only User
                if (viewOnlyRole != null)
                {
                    var viewOnlyPermissions = permissions.Where(p =>
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "vocabulary.view" ||
                        p.PermissionName == "kanji.view" ||
                        p.PermissionName == "grammar.view" ||
                        p.PermissionName == "term.view");

                    foreach (var permission in viewOnlyPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = viewOnlyRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Vocabulary Manager A
                if (vocabManagerARole != null)
                {
                    var vocabManagerAPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("vocabulary.") ||
                        p.PermissionName == "vocabulary.group_a" ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit");

                    foreach (var permission in vocabManagerAPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = vocabManagerARole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Vocabulary Manager B
                if (vocabManagerBRole != null)
                {
                    var vocabManagerBPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("vocabulary.") ||
                        p.PermissionName == "vocabulary.group_b" ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit");

                    foreach (var permission in vocabManagerBPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = vocabManagerBRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Grammar Manager
                if (grammarManagerRole != null)
                {
                    var grammarManagerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("grammar.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit");

                    foreach (var permission in grammarManagerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = grammarManagerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Kanji Manager
                if (kanjiManagerRole != null)
                {
                    var kanjiManagerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("kanji.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit");

                    foreach (var permission in kanjiManagerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = kanjiManagerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Exam Manager
                if (examManagerRole != null)
                {
                    var examManagerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("exam.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit");

                    foreach (var permission in examManagerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = examManagerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Practice Manager
                if (practiceManagerRole != null)
                {
                    var practiceManagerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("practice.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit");

                    foreach (var permission in practiceManagerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = practiceManagerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                // Gán quyền cho Technical Term Manager
                if (termManagerRole != null)
                {
                    var termManagerPermissions = permissions.Where(p =>
                        p.PermissionName.StartsWith("term.") ||
                        p.PermissionName == "content.view" ||
                        p.PermissionName == "content.create" ||
                        p.PermissionName == "content.edit");

                    foreach (var permission in termManagerPermissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = termManagerRole.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                await _context.RolePermissions.AddRangeAsync(rolePermissions);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Seed admin user
        /// </summary>
        private async Task SeedAdminUserAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Seeding admin user...");

                var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Administrator");
                if (adminRole != null)
                {
                    // Create admin user
                    var adminUser = new User
                    {
                        Username = "admin",
                        Email = "admin@lexiflow.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        PreferredLanguage = "en",
                        TimeZone = "UTC",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _context.Users.AddAsync(adminUser);
                    await _context.SaveChangesAsync();

                    // Assign admin role
                    var userRole = new UserRole
                    {
                        UserId = adminUser.UserId,
                        RoleId = adminRole.RoleId
                    };

                    await _context.UserRoles.AddAsync(userRole);
                    await _context.SaveChangesAsync();

                    // Create user profile
                    var userProfile = new UserProfile
                    {
                        UserId = adminUser.UserId,
                        FirstName = "Admin",
                        LastName = "User",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _context.UserProfiles.AddAsync(userProfile);
                    await _context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Seed categories
        /// </summary>
        private async Task SeedCategoriesAsync()
        {
            if (!await _context.Categories.AnyAsync())
            {
                _logger.LogInformation("Seeding categories...");

                var categories = new List<Category>
                {
                   
                    new Category
                    {
                        CategoryName = "JLPT N1",
                        Description = "Japanese Language Proficiency Test Level N1",
                        Level = 1.ToString(),
                        CategoryType = "Vocabulary",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        CategoryName = "JLPT N2",
                        Description = "Japanese Language Proficiency Test Level N2",
                        Level = 2.ToString(),
                        CategoryType = "Vocabulary",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        CategoryName = "JLPT N3",
                        Description = "Japanese Language Proficiency Test Level N3",
                        Level = 3.ToString(),
                        CategoryType = "Vocabulary",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        CategoryName = "JLPT N4",
                        Description = "Japanese Language Proficiency Test Level N4",
                        Level = 4.ToString(),
                        CategoryType = "Vocabulary",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        CategoryName = "JLPT N5",
                        Description = "Japanese Language Proficiency Test Level N5",
                        Level = 5.ToString(),
                        CategoryType = "Vocabulary",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                };

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }
    }
}
