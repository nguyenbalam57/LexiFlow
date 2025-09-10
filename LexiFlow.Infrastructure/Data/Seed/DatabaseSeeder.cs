using LexiFlow.Infrastructure.Data;
using LexiFlow.Models.Learning.Vocabulary;
using LexiFlow.Models.Learning.Kanji;
using LexiFlow.Models.Learning.Grammar;
using LexiFlow.Models.User;
using LexiFlow.Models.User.UserRelations;
using LexiFlow.Models.Progress;
using LexiFlow.Models.Planning;
using LexiFlow.Models.Exam;
using LexiFlow.Models.Practice;
using LexiFlow.Models.Notification;
using LexiFlow.Models.Media;
using LexiFlow.Models.System;
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
                
                // Basic Categories - Cải tiến: thêm nhiều categories hơn để test
                await SeedCategoriesAsync();
                
                // Basic Vocabularies - Tạm thời comment out để tránh FK constraints
                // await VocabularySeedData.SeedVocabularyAsync(_context, _logger);

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
                    new Role { RoleName = "Administrator", Description = "Quản trị viên hệ thống - toàn quyền", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "ContentManager", Description = "Quản lý nội dung tổng thể", IsActive = true, Metadata = "{}"},
                    new Role { RoleName = "Teacher", Description = "Giáo viên/Người tạo nội dung", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "Student", Description = "Học viên/Người học", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "Guest", Description = "Người dùng khách với quyền truy cập hạn chế", IsActive = true, Metadata = "{}" },
                    
                    // Vai trò theo bộ phận công ty
                    new Role { RoleName = "DepartmentManager", Description = "Quản lý bộ phận", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "LearningDepartment", Description = "Nhân viên bộ phận học tập", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "NonLearningDepartment", Description = "Nhân viên bộ phận không học tập", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "ProgressTracker", Description = "Nhân viên theo dõi tiến độ học tập", IsActive = true, Metadata = "{}" },
                    
                    // Vai trò theo nhu cầu học tập
                    new Role { RoleName = "FullLearner", Description = "Người học toàn diện", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "SelfGuidedLearner", Description = "Người học không cần lộ trình", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "CasualLearner", Description = "Người học lướt qua", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "ViewOnlyUser", Description = "Người dùng chỉ xem, không ghi nhớ", IsActive = true, Metadata = "{}" },
                    
                    // Vai trò quản lý nội dung
                    new Role { RoleName = "VocabularyManagerA", Description = "Quản lý từ vựng nhóm A (JLPT N4-N5)", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "VocabularyManagerB", Description = "Quản lý từ vựng nhóm B (JLPT N1-N3)", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "GrammarManager", Description = "Quản lý ngữ pháp", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "KanjiManager", Description = "Quản lý Kanji", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "ExamManager", Description = "Quản lý kỳ thi", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "PracticeManager", Description = "Quản lý bài tập thực hành", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "TechnicalTermManager", Description = "Quản lý thuật ngữ chuyên ngành", IsActive = true, Metadata = "{}" },

                     // Vai trò cấp bậc trong công ty
                    new Role { RoleName = "Management", Description = "Cấp quản lý cao cấp, có quyền giám sát và quản lý toàn công ty", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "Leader", Description = "Trưởng nhóm, quản lý trực tiếp nhóm nhỏ", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "Engineer", Description = "Kỹ sư/Chuyên viên, nhân viên chuyên môn", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "Staff", Description = "Nhân viên chính thức, thực hiện các công việc thông thường", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "Junior", Description = "Nhân viên mới/Junior, đang trong quá trình học tập và phát triển", IsActive = true, Metadata = "{}" },
                    new Role { RoleName = "Star", Description = "Nhân viên xuất sắc, có thành tích và kỹ năng nổi bật", IsActive = true, Metadata = "{}" }
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
                    new Permission { PermissionName = "user.view", Description = "Xem thông tin người dùng", IsActive = true, Metadata = "{}", Module = "User", ResourceType = "User", Action = "View", Category = "User" },
                    new Permission { PermissionName = "user.create", Description = "Tạo người dùng mới", IsActive = true, Metadata = "{}", Module = "User", ResourceType = "User", Action = "Create", Category = "User" },
                    new Permission { PermissionName = "user.edit", Description = "Chỉnh sửa thông tin người dùng", IsActive = true, Metadata = "{}", Module = "User", ResourceType = "User", Action = "Edit", Category = "User" },
                    new Permission { PermissionName = "user.delete", Description = "Xóa người dùng", IsActive = true, Metadata = "{}", Module = "User", ResourceType = "User", Action = "Delete", Category = "User" },
                    new Permission { PermissionName = "user.manage_permissions", Description = "Quản lý quyền của người dùng", IsActive = true, Metadata = "{}", Module = "User", ResourceType = "User", Action = "ManagePermissions", Category = "User" },
                    new Permission { PermissionName = "user.view_progress", Description = "Xem tiến độ học tập của người dùng", IsActive = true, Metadata = "{}", Module = "User", ResourceType = "User", Action = "ViewProgress", Category = "User" },
                    new Permission { PermissionName = "user.reset_password", Description = "Đặt lại mật khẩu cho người dùng", IsActive = true, Metadata = "{}", Module = "User", ResourceType = "User", Action = "ResetPassword", Category = "User" },

                    // Quyền quản lý vai trò
                    new Permission { PermissionName = "role.view", Description = "Xem vai trò", IsActive = true, Metadata = "{}", Module = "Role", ResourceType = "Role", Action = "View", Category = "Role" },
                    new Permission { PermissionName = "role.create", Description = "Tạo vai trò mới", IsActive = true, Metadata = "{}", Module = "Role", ResourceType = "Role", Action = "Create", Category = "Role" },
                    new Permission { PermissionName = "role.edit", Description = "Chỉnh sửa vai trò", IsActive = true, Metadata = "{}", Module = "Role", ResourceType = "Role", Action = "Edit", Category = "Role" },
                    new Permission { PermissionName = "role.delete", Description = "Xóa vai trò", IsActive = true, Metadata = "{}", Module = "Role", ResourceType = "Role", Action = "Delete", Category = "Role" },
                    new Permission { PermissionName = "role.assign", Description = "Gán vai trò cho người dùng", IsActive = true, Metadata = "{}", Module = "Role", ResourceType = "Role", Action = "Assign", Category = "Role" },

                    // Quyền quản lý bộ phận
                    new Permission { PermissionName = "department.view", Description = "Xem bộ phận", IsActive = true, Metadata = "{}", Module = "Department", ResourceType = "Department", Action = "View", Category = "Department" },
                    new Permission { PermissionName = "department.create", Description = "Tạo bộ phận mới", IsActive = true, Metadata = "{}", Module = "Department", ResourceType = "Department", Action = "Create", Category = "Department" },
                    new Permission { PermissionName = "department.edit", Description = "Chỉnh sửa bộ phận", IsActive = true, Metadata = "{}", Module = "Department", ResourceType = "Department", Action = "Edit", Category = "Department" },
                    new Permission { PermissionName = "department.delete", Description = "Xóa bộ phận", IsActive = true, Metadata = "{}", Module = "Department", ResourceType = "Department", Action = "Delete", Category = "Department" },
                    new Permission { PermissionName = "department.assign_user", Description = "Gán người dùng vào bộ phận", IsActive = true, Metadata = "{}", Module = "Department", ResourceType = "Department", Action = "AssignUser", Category = "Department" },

                    // Quyền quản lý nội dung chung
                    new Permission { PermissionName = "content.view", Description = "Xem nội dung học tập", IsActive = true, Metadata = "{}", Module = "Content", ResourceType = "Content", Action = "View", Category = "Content" },
                    new Permission { PermissionName = "content.create", Description = "Tạo nội dung học tập", IsActive = true, Metadata = "{}", Module = "Content", ResourceType = "Content", Action = "Create", Category = "Content" },
                    new Permission { PermissionName = "content.edit", Description = "Chỉnh sửa nội dung học tập", IsActive = true, Metadata = "{}", Module = "Content", ResourceType = "Content", Action = "Edit", Category = "Content" },
                    new Permission { PermissionName = "content.delete", Description = "Xóa nội dung học tập", IsActive = true, Metadata = "{}", Module = "Content", ResourceType = "Content", Action = "Delete", Category = "Content" },
                    new Permission { PermissionName = "content.approve", Description = "Phê duyệt nội dung học tập", IsActive = true, Metadata = "{}", Module = "Content", ResourceType = "Content", Action = "Approve", Category = "Content" },
                    new Permission { PermissionName = "content.reject", Description = "Từ chối nội dung học tập", IsActive = true, Metadata = "{}", Module = "Content", ResourceType = "Content", Action = "Reject", Category = "Content" },
                    new Permission { PermissionName = "content.publish", Description = "Xuất bản nội dung học tập", IsActive = true, Metadata = "{}", Module = "Content", ResourceType = "Content", Action = "Publish", Category = "Content" },

                    // Quyền quản lý từ vựng
                    new Permission { PermissionName = "vocabulary.view", Description = "Xem từ vựng", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "View", Category = "Vocabulary" },
                    new Permission { PermissionName = "vocabulary.create", Description = "Tạo từ vựng mới", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "Create", Category = "Vocabulary" },
                    new Permission { PermissionName = "vocabulary.edit", Description = "Chỉnh sửa từ vựng", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "Edit", Category = "Vocabulary" },
                    new Permission { PermissionName = "vocabulary.delete", Description = "Xóa từ vựng", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "Delete", Category = "Vocabulary" },
                    new Permission { PermissionName = "vocabulary.import", Description = "Nhập từ vựng từ tệp", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "Import", Category = "Vocabulary" },
                    new Permission { PermissionName = "vocabulary.export", Description = "Xuất từ vựng ra tệp", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "Export", Category = "Vocabulary" },
                    new Permission { PermissionName = "vocabulary.group_a", Description = "Quản lý từ vựng nhóm A (N4-N5)", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "GroupA", Category = "Vocabulary" },
                    new Permission { PermissionName = "vocabulary.group_b", Description = "Quản lý từ vựng nhóm B (N1-N3)", IsActive = true, Metadata = "{}", Module = "Vocabulary", ResourceType = "Vocabulary", Action = "GroupB", Category = "Vocabulary" },

                    // Quyền quản lý Kanji
                    new Permission { PermissionName = "kanji.view", Description = "Xem Kanji", IsActive = true, Metadata = "{}", Module = "Kanji", ResourceType = "Kanji", Action = "View", Category = "Kanji" },
                    new Permission { PermissionName = "kanji.create", Description = "Tạo Kanji mới", IsActive = true, Metadata = "{}", Module = "Kanji", ResourceType = "Kanji", Action = "Create", Category = "Kanji" },
                    new Permission { PermissionName = "kanji.edit", Description = "Chỉnh sửa Kanji", IsActive = true, Metadata = "{}", Module = "Kanji", ResourceType = "Kanji", Action = "Edit", Category = "Kanji" },
                    new Permission { PermissionName = "kanji.delete", Description = "Xóa Kanji", IsActive = true, Metadata = "{}", Module = "Kanji", ResourceType = "Kanji", Action = "Delete", Category = "Kanji" },
                    new Permission { PermissionName = "kanji.import", Description = "Nhập Kanji từ tệp", IsActive = true, Metadata = "{}", Module = "Kanji", ResourceType = "Kanji", Action = "Import", Category = "Kanji" },
                    new Permission { PermissionName = "kanji.export", Description = "Xuất Kanji ra tệp", IsActive = true, Metadata = "{}", Module = "Kanji", ResourceType = "Kanji", Action = "Export", Category = "Kanji" },

                    // Quyền quản lý ngữ pháp
                    new Permission { PermissionName = "grammar.view", Description = "Xem ngữ pháp", IsActive = true, Metadata = "{}", Module = "Grammar", ResourceType = "Grammar", Action = "View", Category = "Grammar" },
                    new Permission { PermissionName = "grammar.create", Description = "Tạo ngữ pháp mới", IsActive = true, Metadata = "{}", Module = "Grammar", ResourceType = "Grammar", Action = "Create", Category = "Grammar" },
                    new Permission { PermissionName = "grammar.edit", Description = "Chỉnh sửa ngữ pháp", IsActive = true, Metadata = "{}", Module = "Grammar", ResourceType = "Grammar", Action = "Edit", Category = "Grammar" },
                    new Permission { PermissionName = "grammar.delete", Description = "Xóa ngữ pháp", IsActive = true, Metadata = "{}", Module = "Grammar", ResourceType = "Grammar", Action = "Delete", Category = "Grammar" },
                    new Permission { PermissionName = "grammar.import", Description = "Nhập ngữ pháp từ tệp", IsActive = true, Metadata = "{}", Module = "Grammar", ResourceType = "Grammar", Action = "Import", Category = "Grammar" },
                    new Permission { PermissionName = "grammar.export", Description = "Xuất ngữ pháp ra tệp", IsActive = true, Metadata = "{}", Module = "Grammar", ResourceType = "Grammar", Action = "Export", Category = "Grammar" },

                    // Quyền quản lý thuật ngữ chuyên ngành
                    new Permission { PermissionName = "term.view", Description = "Xem thuật ngữ chuyên ngành", IsActive = true, Metadata = "{}", Module = "Term", ResourceType = "Term", Action = "View", Category = "Term" },
                    new Permission { PermissionName = "term.create", Description = "Tạo thuật ngữ chuyên ngành mới", IsActive = true, Metadata = "{}", Module = "Term", ResourceType = "Term", Action = "Create", Category = "Term" },
                    new Permission { PermissionName = "term.edit", Description = "Chỉnh sửa thuật ngữ chuyên ngành", IsActive = true, Metadata = "{}", Module = "Term", ResourceType = "Term", Action = "Edit", Category = "Term" },
                    new Permission { PermissionName = "term.delete", Description = "Xóa thuật ngữ chuyên ngành", IsActive = true, Metadata = "{}", Module = "Term", ResourceType = "Term", Action = "Delete", Category = "Term" },
                    new Permission { PermissionName = "term.import", Description = "Nhập thuật ngữ chuyên ngành từ tệp", IsActive = true, Metadata = "{}", Module = "Term", ResourceType = "Term", Action = "Import", Category = "Term" },
                    new Permission { PermissionName = "term.export", Description = "Xuất thuật ngữ chuyên ngành ra tệp", IsActive = true, Metadata = "{}", Module = "Term", ResourceType = "Term", Action = "Export", Category = "Term" },

                    // Quyền quản lý kỳ thi
                    new Permission { PermissionName = "exam.view", Description = "Xem kỳ thi", IsActive = true, Metadata = "{}", Module = "Exam", ResourceType = "Exam", Action = "View", Category = "Exam" },
                    new Permission { PermissionName = "exam.create", Description = "Tạo kỳ thi mới", IsActive = true, Metadata = "{}", Module = "Exam", ResourceType = "Exam", Action = "Create", Category = "Exam" },
                    new Permission { PermissionName = "exam.edit", Description = "Chỉnh sửa kỳ thi", IsActive = true, Metadata = "{}", Module = "Exam", ResourceType = "Exam", Action = "Edit", Category = "Exam" },
                    new Permission { PermissionName = "exam.delete", Description = "Xóa kỳ thi", IsActive = true, Metadata = "{}", Module = "Exam", ResourceType = "Exam", Action = "Delete", Category = "Exam" },
                    new Permission { PermissionName = "exam.assign", Description = "Gán kỳ thi cho người dùng", IsActive = true, Metadata = "{}", Module = "Exam", ResourceType = "Exam", Action = "Assign", Category = "Exam" },
                    new Permission { PermissionName = "exam.grade", Description = "Chấm điểm kỳ thi", IsActive = true, Metadata = "{}", Module = "Exam", ResourceType = "Exam", Action = "Grade", Category = "Exam" },
                    new Permission { PermissionName = "exam.view_results", Description = "Xem kết quả kỳ thi", IsActive = true, Metadata = "{}", Module = "Exam", ResourceType = "Exam", Action = "ViewResults", Category = "Exam" },

                    // Quyền quản lý bài tập thực hành
                    new Permission { PermissionName = "practice.view", Description = "Xem bài tập thực hành", IsActive = true, Metadata = "{}", Module = "Practice", ResourceType = "Practice", Action = "View", Category = "Practice" },
                    new Permission { PermissionName = "practice.create", Description = "Tạo bài tập thực hành mới", IsActive = true, Metadata = "{}", Module = "Practice", ResourceType = "Practice", Action = "Create", Category = "Practice" },
                    new Permission { PermissionName = "practice.edit", Description = "Chỉnh sửa bài tập thực hành", IsActive = true, Metadata = "{}", Module = "Practice", ResourceType = "Practice", Action = "Edit", Category = "Practice" },
                    new Permission { PermissionName = "practice.delete", Description = "Xóa bài tập thực hành", IsActive = true, Metadata = "{}", Module = "Practice", ResourceType = "Practice", Action = "Delete", Category = "Practice" },
                    new Permission { PermissionName = "practice.assign", Description = "Gán bài tập thực hành cho người dùng", IsActive = true, Metadata = "{}", Module = "Practice", ResourceType = "Practice", Action = "Assign", Category = "Practice" },

                    // Quyền học tập
                    new Permission { PermissionName = "learning.access", Description = "Truy cập tính năng học tập", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Access", Category = "Learning" },
                    new Permission { PermissionName = "learning.vocabulary", Description = "Học từ vựng", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Vocabulary", Category = "Learning" },
                    new Permission { PermissionName = "learning.kanji", Description = "Học Kanji", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Kanji", Category = "Learning" },
                    new Permission { PermissionName = "learning.grammar", Description = "Học ngữ pháp", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Grammar", Category = "Learning" },
                    new Permission { PermissionName = "learning.term", Description = "Học thuật ngữ chuyên ngành", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Term", Category = "Learning" },
                    new Permission { PermissionName = "learning.practice", Description = "Thực hành bài tập", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Practice", Category = "Learning" },
                    new Permission { PermissionName = "learning.test", Description = "Làm bài kiểm tra", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Test", Category = "Learning" },
                    new Permission { PermissionName = "learning.track_progress", Description = "Theo dõi tiến độ học tập cá nhân", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "TrackProgress", Category = "Learning" },
                    new Permission { PermissionName = "learning.create_plan", Description = "Tạo kế hoạch học tập cá nhân", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "CreatePlan", Category = "Learning" },
                    new Permission { PermissionName = "learning.memorize", Description = "Sử dụng tính năng ghi nhớ", IsActive = true, Metadata = "{}", Module = "Learning", ResourceType = "Learning", Action = "Memorize", Category = "Learning" },

                    // Quyền quản lý tiến độ học tập
                    new Permission { PermissionName = "progress.view_own", Description = "Xem tiến độ học tập cá nhân", IsActive = true, Metadata = "{}", Module = "Progress", ResourceType = "Progress", Action = "ViewOwn", Category = "Progress" },
                    new Permission { PermissionName = "progress.view_department", Description = "Xem tiến độ học tập của bộ phận", IsActive = true, Metadata = "{}", Module = "Progress", ResourceType = "Progress", Action = "ViewDepartment", Category = "Progress" },
                    new Permission { PermissionName = "progress.view_all", Description = "Xem tiến độ học tập của tất cả người dùng", IsActive = true, Metadata = "{}", Module = "Progress", ResourceType = "Progress", Action = "ViewAll", Category = "Progress" },
                    new Permission { PermissionName = "progress.report", Description = "Tạo báo cáo tiến độ học tập", IsActive = true, Metadata = "{}", Module = "Progress", ResourceType = "Progress", Action = "Report", Category = "Progress" },

                    // Quyền quản lý gamification
                    new Permission { PermissionName = "gamification.manage", Description = "Quản lý hệ thống gamification", IsActive = true, Metadata = "{}", Module = "Gamification", ResourceType = "Gamification", Action = "Manage", Category = "Gamification" },
                    new Permission { PermissionName = "gamification.create_challenge", Description = "Tạo thử thách mới", IsActive = true, Metadata = "{}", Module = "Gamification", ResourceType = "Gamification", Action = "CreateChallenge", Category = "Gamification" },
                    new Permission { PermissionName = "gamification.create_badge", Description = "Tạo huy hiệu mới", IsActive = true, Metadata = "{}", Module = "Gamification", ResourceType = "Gamification", Action = "CreateBadge", Category = "Gamification" },
                    new Permission { PermissionName = "gamification.award", Description = "Trao thưởng cho người dùng", IsActive = true, Metadata = "{}", Module = "Gamification", ResourceType = "Gamification", Action = "Award", Category = "Gamification" },

                    // Quyền quản lý hệ thống
                    new Permission { PermissionName = "system.settings", Description = "Quản lý cài đặt hệ thống", IsActive = true, Metadata = "{}", Module = "System", ResourceType = "System", Action = "Settings", Category = "System" },
                    new Permission { PermissionName = "system.backup", Description = "Sao lưu hệ thống", IsActive = true, Metadata = "{}", Module = "System", ResourceType = "System", Action = "Backup", Category = "System" },
                    new Permission { PermissionName = "system.restore", Description = "Khôi phục hệ thống", IsActive = true, Metadata = "{}", Module = "System", ResourceType = "System", Action = "Restore", Category = "System" },
                    new Permission { PermissionName = "system.logs", Description = "Xem nhật ký hệ thống", IsActive = true, Metadata = "{}", Module = "System", ResourceType = "System", Action = "Logs", Category = "System" },
                    new Permission { PermissionName = "system.maintenance", Description = "Thực hiện bảo trì hệ thống", IsActive = true, Metadata = "{}", Module = "System", ResourceType = "System", Action = "Maintenance", Category = "System" }
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

                // Gán tất cả quyền cho Admin với scope Global
                if (adminRole != null)
                {
                    foreach (var permission in permissions)
                    {
                        rolePermissions.Add(new RolePermission
                        {
                            RoleId = adminRole.RoleId,
                            PermissionId = permission.PermissionId,
                            Scope = "Global",
                            IsOverride = true,
                            GrantedBy = null, // System granted
                            Description = $"Quyền toàn hệ thống cho Admin: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Management với scope Company
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
                            PermissionId = permission.PermissionId,
                            Scope = "Company",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý cấp công ty: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Leader với scope Department
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
                            PermissionId = permission.PermissionId,
                            Scope = "Department",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền trưởng nhóm phòng ban: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Engineer với scope Personal
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
                            PermissionId = permission.PermissionId,
                            Scope = "Personal",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền kỹ sư cá nhân: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Staff với scope Personal
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
                            PermissionId = permission.PermissionId,
                            Scope = "Personal",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền nhân viên cơ bản: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Junior với scope Limited
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
                            PermissionId = permission.PermissionId,
                            Scope = "Limited",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền nhân viên mới với giới hạn: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Star với scope Extended
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
                            PermissionId = permission.PermissionId,
                            Scope = "Extended",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền nhân viên xuất sắc mở rộng: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Content Manager với scope Global
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
                            PermissionId = permission.PermissionId,
                            Scope = "Global",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý nội dung toàn cục: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Teacher với scope Department
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
                            PermissionId = permission.PermissionId,
                            Scope = "Department",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền giáo viên phòng ban: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Student với scope Personal
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
                            PermissionId = permission.PermissionId,
                            Scope = "Personal",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền học viên cá nhân: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Guest với scope Public
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
                            PermissionId = permission.PermissionId,
                            Scope = "Public",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền khách truy cập công khai: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Department Manager với scope Department
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
                            PermissionId = permission.PermissionId,
                            Scope = "Department",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền trưởng phòng quản lý: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Learning Department với scope Personal
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
                            PermissionId = permission.PermissionId,
                            Scope = "Personal",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền phòng ban học tập: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Non-Learning Department với scope Limited
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
                            PermissionId = permission.PermissionId,
                            Scope = "Limited",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền phòng ban ngoài học tập giới hạn: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Progress Tracker với scope Company
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
                            PermissionId = permission.PermissionId,
                            Scope = "Company",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền theo dõi tiến độ công ty: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Full Learner với scope Extended
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
                            PermissionId = permission.PermissionId,
                            Scope = "Extended",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền học viên toàn diện mở rộng: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Self-Guided Learner với scope Personal
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
                            PermissionId = permission.PermissionId,
                            Scope = "Personal",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền học viên tự học cá nhân: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Casual Learner với scope Limited
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
                            PermissionId = permission.PermissionId,
                            Scope = "Limited",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền học viên thường xuyên giới hạn: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho View-Only User với scope Public
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
                            PermissionId = permission.PermissionId,
                            Scope = "Public",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền chỉ xem công khai: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Vocabulary Manager A với scope GroupA
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
                            PermissionId = permission.PermissionId,
                            Scope = "GroupA",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý từ vựng nhóm A: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Vocabulary Manager B với scope GroupB
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
                            PermissionId = permission.PermissionId,
                            Scope = "GroupB",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý từ vựng nhóm B: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Grammar Manager với scope Module
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
                            PermissionId = permission.PermissionId,
                            Scope = "Module",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý ngữ pháp module: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Kanji Manager với scope Module
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
                            PermissionId = permission.PermissionId,
                            Scope = "Module",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý kanji module: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Exam Manager với scope Module
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
                            PermissionId = permission.PermissionId,
                            Scope = "Module",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý thi cử module: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Practice Manager với scope Module
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
                            PermissionId = permission.PermissionId,
                            Scope = "Module",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý thực hành module: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // Gán quyền cho Technical Term Manager với scope Module
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
                            PermissionId = permission.PermissionId,
                            Scope = "Module",
                            IsOverride = false,
                            GrantedBy = null,
                            Description = $"Quyền quản lý thuật ngữ kỹ thuật module: {permission.Module ?? permission.PermissionName}",
                            IsActive = true,
                            Metadata = "{}",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
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
                        Description = "Admin toàn quyền thực hiện thiện",
                        LastLoginIP = "",
                        LastLoginAt = DateTime.UtcNow,
                        Metadata = "{}",
                        MfaKey = "",
                        RefreshToken = "",
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
                        RoleId = adminRole.RoleId,
                        Description = "Admin được gán quyền",
                        Metadata = "{}"

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
        /// Seed test users with different roles
        /// </summary>
        private async Task SeedTestUsersAsync()
        {
            _logger.LogInformation("Seeding test users...");

            var roles = await _context.Roles.ToListAsync();
            var testUsers = new List<(string username, string email, string firstName, string lastName, string roleName)>
            {
                ("management01", "management@lexiflow.com", "Management", "User", "Management"),
                ("leader01", "leader@lexiflow.com", "Team", "Leader", "Leader"),
                ("engineer01", "engineer@lexiflow.com", "Software", "Engineer", "Engineer"),
                ("staff01", "staff@lexiflow.com", "Regular", "Staff", "Staff"),
                ("junior01", "junior@lexiflow.com", "Junior", "Developer", "Junior"),
                ("star01", "star@lexiflow.com", "Star", "Employee", "Star"),
                ("teacher01", "teacher@lexiflow.com", "Japanese", "Teacher", "Teacher"),
                ("student01", "student@lexiflow.com", "Test", "Student", "Student"),
                ("content01", "content@lexiflow.com", "Content", "Manager", "ContentManager"),
                ("vocab_a01", "vocaba@lexiflow.com", "Vocabulary", "Manager A", "VocabularyManagerA"),
                ("vocab_b01", "vocabb@lexiflow.com", "Vocabulary", "Manager B", "VocabularyManagerB"),
                ("grammar01", "grammar@lexiflow.com", "Grammar", "Manager", "GrammarManager"),
                ("kanji01", "kanji@lexiflow.com", "Kanji", "Manager", "KanjiManager"),
                ("exam01", "exam@lexiflow.com", "Exam", "Manager", "ExamManager"),
                ("practice01", "practice@lexiflow.com", "Practice", "Manager", "PracticeManager"),
                ("term01", "term@lexiflow.com", "Term", "Manager", "TechnicalTermManager")
            };

            foreach (var (username, email, firstName, lastName, roleName) in testUsers)
            {
                if (!await _context.Users.AnyAsync(u => u.Username == username))
                {
                    var role = roles.FirstOrDefault(r => r.RoleName == roleName);
                    if (role != null)
                    {
                        // Create user
                        var user = new User
                        {
                            Username = username,
                            Email = email,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                            PreferredLanguage = "vi",
                            TimeZone = "Asia/Ho_Chi_Minh",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();

                        // Assign role
                        var userRole = new UserRole
                        {
                            UserId = user.UserId,
                            RoleId = role.RoleId
                        };

                        await _context.UserRoles.AddAsync(userRole);

                        // Create profile
                        var userProfile = new UserProfile
                        {
                            UserId = user.UserId,
                            FirstName = firstName,
                            LastName = lastName,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await _context.UserProfiles.AddAsync(userProfile);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Seed categories
        /// </summary>
        private async Task SeedCategoriesAsync()
        {
            if (!await _context.Categories.AnyAsync())
            {
                _logger.LogInformation("Seeding categories...");

                // Tìm user đầu tiên
                var firtUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                int createdById = firtUser.UserId;

                var categories = new List<Category>
                {
                    // Các danh mục JLPT
                    new Category
                    {
                        CategoryName = "JLPT N1",
                        Description = "Kỳ thi năng lực tiếng Nhật cấp độ N1 - Trình độ cao cấp",
                        Level = "1",
                        CategoryType = "JLPT",
                        ColorCode = "#D32F2F", // Đỏ đậm - cấp độ khó nhất
                        IconPath = "/images/icons/jlpt-n1.svg",
                        Tags = "jlpt,advanced,n1,difficult",
                        DisplayOrder = 1,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "JLPT N2",
                        Description = "Kỳ thi năng lực tiếng Nhật cấp độ N2 - Trình độ trung cấp cao",
                        Level = "2",
                        CategoryType = "JLPT",
                        ColorCode = "#F57C00", // Cam - cấp độ khá khó
                        IconPath = "/images/icons/jlpt-n2.svg",
                        Tags = "jlpt,upper-intermediate,n2",
                        DisplayOrder = 2,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "JLPT N3",
                        Description = "Kỳ thi năng lực tiếng Nhật cấp độ N3 - Trình độ trung cấp",
                        Level = "3",
                        CategoryType = "JLPT",
                        ColorCode = "#FBC02D", // Vàng - cấp độ trung bình
                        IconPath = "/images/icons/jlpt-n3.svg",
                        Tags = "jlpt,intermediate,n3",
                        DisplayOrder = 3,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "JLPT N4",
                        Description = "Kỳ thi năng lực tiếng Nhật cấp độ N4 - Trình độ sơ cấp",
                        Level = "4",
                        CategoryType = "JLPT",
                        ColorCode = "#689F38", // Xanh lá nhạt - cấp độ dễ
                        IconPath = "/images/icons/jlpt-n4.svg",
                        Tags = "jlpt,elementary,n4",
                        DisplayOrder = 4,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "JLPT N5",
                        Description = "Kỳ thi năng lực tiếng Nhật cấp độ N5 - Trình độ cơ bản",
                        Level = "5",
                        CategoryType = "JLPT",
                        ColorCode = "#4CAF50", // Xanh lá - cấp độ dễ nhất
                        IconPath = "/images/icons/jlpt-n5.svg",
                        Tags = "jlpt,beginner,n5,basic",
                        DisplayOrder = 5,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },

                    // Các danh mục thuật ngữ kỹ thuật
                    new Category
                    {
                        CategoryName = "Thuật ngữ IT",
                        Description = "Các thuật ngữ về công nghệ thông tin và lập trình",
                        Level = "1",
                        CategoryType = "Technical",
                        ColorCode = "#2196F3", // Xanh dương - IT
                        IconPath = "/images/icons/it-terms.svg",
                        Tags = "it,technology,programming,technical",
                        DisplayOrder = 10,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "Thuật ngữ Kinh doanh",
                        Description = "Các thuật ngữ về kinh doanh, tài chính và quản lý",
                        Level = "1",
                        CategoryType = "Technical",
                        ColorCode = "#9C27B0", // Tím - Business
                        IconPath = "/images/icons/business-terms.svg",
                        Tags = "business,finance,management,economy",
                        DisplayOrder = 11,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "Thuật ngữ Kỹ thuật",
                        Description = "Các thuật ngữ về kỹ thuật, công nghệ và sản xuất",
                        Level = "1",
                        CategoryType = "Technical",
                        ColorCode = "#FF5722", // Cam đỏ - Engineering
                        IconPath = "/images/icons/engineering-terms.svg",
                        Tags = "engineering,manufacturing,technology",
                        DisplayOrder = 12,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },

                    // Các danh mục lộ trình học tập
                    new Category
                    {
                        CategoryName = "Lộ trình Người mới bắt đầu",
                        Description = "Lộ trình học tập dành cho người mới bắt đầu học tiếng Nhật",
                        Level = "1",
                        CategoryType = "LearningPath",
                        ColorCode = "#8BC34A", // Xanh lá nhạt - Beginner
                        IconPath = "/images/icons/beginner-path.svg",
                        Tags = "beginner,learning-path,starter",
                        DisplayOrder = 20,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "Lộ trình Trung cấp",
                        Description = "Lộ trình học tập dành cho người đã có kiến thức cơ bản",
                        Level = "2",
                        CategoryType = "LearningPath",
                        ColorCode = "#FF9800", // Cam - Intermediate
                        IconPath = "/images/icons/intermediate-path.svg",
                        Tags = "intermediate,learning-path,progress",
                        DisplayOrder = 21,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "Lộ trình Nâng cao",
                        Description = "Lộ trình học tập dành cho người có trình độ cao",
                        Level = "3",
                        CategoryType = "LearningPath",
                        ColorCode = "#E91E63", // Hồng đậm - Advanced
                        IconPath = "/images/icons/advanced-path.svg",
                        Tags = "advanced,learning-path,expert",
                        DisplayOrder = 22,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },

                    // Các danh mục nội dung theo chủ đề
                    new Category
                    {
                        CategoryName = "Hội thoại Hàng ngày",
                        Description = "Từ vựng và cụm từ sử dụng trong cuộc sống hàng ngày",
                        Level = "1",
                        CategoryType = "Content",
                        ColorCode = "#00BCD4", // Xanh cyan - Daily
                        IconPath = "/images/icons/daily-conversation.svg",
                        Tags = "daily,conversation,practical,life",
                        DisplayOrder = 30,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "Tiếng Nhật Trang trọng",
                        Description = "Các cách diễn đạt lịch sự và trang trọng trong tiếng Nhật",
                        Level = "2",
                        CategoryType = "Content",
                        ColorCode = "#3F51B5", // Xanh indigo - Formal
                        IconPath = "/images/icons/formal-japanese.svg",
                        Tags = "formal,polite,business,keigo",
                        DisplayOrder = 31,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    },
                    new Category
                    {
                        CategoryName = "Tiếng Nhật Học thuật",
                        Description = "Tiếng Nhật sử dụng trong môi trường học thuật và nghiên cứu",
                        Level = "3",
                        CategoryType = "Content",
                        ColorCode = "#795548", // Nâu - Academic
                        IconPath = "/images/icons/academic-japanese.svg",
                        Tags = "academic,scholarly,research,university",
                        DisplayOrder = 32,
                        IsActive = true,
                        IsRestricted = false,
                        AllowedRoles = "",
                        AllowedUserIds = "",
                        Metadata = "",
                        ChangeReason = "",
                        ItemCount = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = createdById,
                    }
                };

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Seed sample vocabulary data
        /// </summary>
        private async Task SeedSampleVocabularyAsync()
        {
            if (!await _context.Vocabularies.AnyAsync())
            {
                _logger.LogInformation("Seeding sample vocabulary...");
                var n5Category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "JLPT N5");
                var n4Category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "JLPT N4");

                if (n5Category != null && n4Category != null)
                {
                    var vocabularies = new List<Vocabulary>
            {
                // N5 Level Vocabularies
                new Vocabulary
                {
                    Term = "こんにちは",
                    LanguageCode = "ja",
                    Reading = "こんにちは",
                    Level = "N5",
                    CategoryId = n5Category.CategoryId,
                    DifficultyLevel = 1,
                    FrequencyRank = 10,
                    PartOfSpeech = "Expression",
                    DetailedPartOfSpeech = "Greeting",
                    PolitenessLevel = "Polite",
                    UsageContext = "Daily conversation",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 150,
                    FavoriteCount = 75,
                    AverageRating = 4.8,
                    RatingCount = 45,
                    PopularityScore = 375,
                    ContentVersion = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "ありがとう",
                    LanguageCode = "ja",
                    Reading = "ありがとう",
                    Level = "N5",
                    CategoryId = n5Category.CategoryId,
                    DifficultyLevel = 1,
                    FrequencyRank = 10,
                    PartOfSpeech = "Expression",
                    DetailedPartOfSpeech = "Gratitude expression",
                    PolitenessLevel = "Casual",
                    UsageContext = "Daily conversation",
                    IsCommon = true,
                    IsFormal = false,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 200,
                    FavoriteCount = 90,
                    AverageRating = 4.9,
                    RatingCount = 60,
                    PopularityScore = 474,
                    ContentVersion = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "学校",
                    LanguageCode = "ja",
                    Reading = "がっこう",
                    Level = "N5",
                    CategoryId = n5Category.CategoryId,
                    DifficultyLevel = 2,
                    FrequencyRank = 9,
                    PartOfSpeech = "Noun",
                    DetailedPartOfSpeech = "Common noun",
                    PolitenessLevel = "Neutral",
                    UsageContext = "Education, daily life",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 180,
                    FavoriteCount = 65,
                    AverageRating = 4.5,
                    RatingCount = 40,
                    PopularityScore = 310,
                    ContentVersion = 1,
                    SynonymsJson = "[\"学園\", \"スクール\"]",
                    RelatedWordsJson = "[\"先生\", \"学生\", \"勉強\"]",
                    Tags = "[\"Education\", \"Institution\", \"N5\"]",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "水",
                    LanguageCode = "ja",
                    Reading = "みず",
                    Level = "N5",
                    CategoryId = n5Category.CategoryId,
                    DifficultyLevel = 1,
                    FrequencyRank = 9,
                    PartOfSpeech = "Noun",
                    DetailedPartOfSpeech = "Common noun",
                    PolitenessLevel = "Neutral",
                    UsageContext = "Daily life, food, drinks",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 160,
                    FavoriteCount = 55,
                    AverageRating = 4.3,
                    RatingCount = 35,
                    PopularityScore = 270,
                    ContentVersion = 1,
                    RelatedWordsJson = "[\"お茶\", \"コーヒー\", \"ジュース\"]",
                    Tags = "[\"Drinks\", \"Daily life\", \"N5\"]",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "食べる",
                    LanguageCode = "ja",
                    Reading = "たべる",
                    Level = "N5",
                    CategoryId = n5Category.CategoryId,
                    DifficultyLevel = 2,
                    FrequencyRank = 9,
                    PartOfSpeech = "Verb",
                    DetailedPartOfSpeech = "Ichidan verb, transitive",
                    PolitenessLevel = "Neutral",
                    UsageContext = "Daily life, food",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 190,
                    FavoriteCount = 70,
                    AverageRating = 4.6,
                    RatingCount = 50,
                    PopularityScore = 330,
                    ContentVersion = 1,
                    SynonymsJson = "[\"いただく\"]",
                    AntonymsJson = "[\"吐く\"]",
                    RelatedWordsJson = "[\"飲む\", \"料理\", \"レストラン\"]",
                    Tags = "[\"Verb\", \"Food\", \"Daily actions\", \"N5\"]",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },

                // N4 Level Vocabularies
                new Vocabulary
                {
                    Term = "勉強",
                    LanguageCode = "ja",
                    Reading = "べんきょう",
                    Level = "N4",
                    CategoryId = n4Category.CategoryId,
                    DifficultyLevel = 3,
                    FrequencyRank = 8,
                    PartOfSpeech = "Noun, Verb",
                    DetailedPartOfSpeech = "Suru verb, noun",
                    PolitenessLevel = "Neutral",
                    UsageContext = "Education, academic",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 220,
                    FavoriteCount = 85,
                    AverageRating = 4.7,
                    RatingCount = 65,
                    PopularityScore = 390,
                    ContentVersion = 1,
                    SynonymsJson = "[\"学習\", \"研究\"]",
                    RelatedWordsJson = "[\"学校\", \"先生\", \"本\", \"試験\"]",
                    Tags = "[\"Education\", \"Study\", \"Academic\", \"N4\"]",
                    UsageNotes = "Can be used as both noun and verb with する",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "会社",
                    LanguageCode = "ja",
                    Reading = "かいしゃ",
                    Level = "N4",
                    CategoryId = n4Category.CategoryId,
                    DifficultyLevel = 3,
                    FrequencyRank = 8,
                    PartOfSpeech = "Noun",
                    DetailedPartOfSpeech = "Common noun",
                    PolitenessLevel = "Neutral",
                    UsageContext = "Business, work",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 170,
                    FavoriteCount = 60,
                    AverageRating = 4.4,
                    RatingCount = 42,
                    PopularityScore = 290,
                    ContentVersion = 1,
                    SynonymsJson = "[\"企業\", \"法人\"]",
                    RelatedWordsJson = "[\"仕事\", \"社員\", \"オフィス\", \"ビジネス\"]",
                    Tags = "[\"Business\", \"Work\", \"Organization\", \"N4\"]",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "時間",
                    LanguageCode = "ja",
                    Reading = "じかん",
                    Level = "N4",
                    CategoryId = n4Category.CategoryId,
                    DifficultyLevel = 3,
                    FrequencyRank = 9,
                    PartOfSpeech = "Noun",
                    DetailedPartOfSpeech = "Common noun",
                    PolitenessLevel = "Neutral",
                    UsageContext = "Time, schedule, daily life",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 200,
                    FavoriteCount = 75,
                    AverageRating = 4.5,
                    RatingCount = 55,
                    PopularityScore = 350,
                    ContentVersion = 1,
                    SynonymsJson = "[\"タイム\"]",
                    RelatedWordsJson = "[\"分\", \"秒\", \"時\", \"日\", \"スケジュール\"]",
                    Tags = "[\"Time\", \"Schedule\", \"Measurement\", \"N4\"]",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "天気",
                    LanguageCode = "ja",
                    Reading = "てんき",
                    Level = "N4",
                    CategoryId = n4Category.CategoryId,
                    DifficultyLevel = 3,
                    FrequencyRank = 7,
                    PartOfSpeech = "Noun",
                    DetailedPartOfSpeech = "Common noun",
                    PolitenessLevel = "Neutral",
                    UsageContext = "Weather, daily conversation",
                    IsCommon = true,
                    IsFormal = true,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 140,
                    FavoriteCount = 50,
                    AverageRating = 4.2,
                    RatingCount = 30,
                    PopularityScore = 240,
                    ContentVersion = 1,
                    RelatedWordsJson = "[\"雨\", \"晴れ\", \"曇り\", \"雪\", \"風\"]",
                    Tags = "[\"Weather\", \"Nature\", \"Daily conversation\", \"N4\"]",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                },
                new Vocabulary
                {
                    Term = "友達",
                    LanguageCode = "ja",
                    Reading = "ともだち",
                    Level = "N4",
                    CategoryId = n4Category.CategoryId,
                    DifficultyLevel = 2,
                    FrequencyRank = 8,
                    PartOfSpeech = "Noun",
                    DetailedPartOfSpeech = "Common noun",
                    PolitenessLevel = "Casual",
                    UsageContext = "Relationships, social",
                    IsCommon = true,
                    IsFormal = false,
                    IsActive = true,
                    Status = "Active",
                    Source = "Dictionary",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow,
                    StudyCount = 185,
                    FavoriteCount = 80,
                    AverageRating = 4.8,
                    RatingCount = 48,
                    PopularityScore = 345,
                    ContentVersion = 1,
                    SynonymsJson = "[\"仲間\", \"親友\"]",
                    RelatedWordsJson = "[\"家族\", \"恋人\", \"同級生\", \"友情\"]",
                    Tags = "[\"Relationships\", \"Social\", \"People\", \"N4\"]",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    ModifiedBy = 1
                }
            };

                    await _context.Vocabularies.AddRangeAsync(vocabularies);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Seeded {vocabularies.Count} sample vocabularies");
                }
                else
                {
                    _logger.LogWarning("Could not find N4 or N5 categories for vocabulary seeding");
                }
            }
            else
            {
                _logger.LogInformation("Vocabularies already exist, skipping seeding");
            }
        }

        /// <summary>
        /// Comprehensive seeding with all data
        /// </summary>
        public async Task SeedAllAsync()
        {
            try
            {
                _logger.LogInformation("Starting comprehensive database seeding...");

                await SeedRolesAndPermissionsAsync();
                await SeedAdminUserAsync();
                await SeedTestUsersAsync();
                await SeedCategoriesAsync();
                await SeedSampleVocabularyAsync();

                _logger.LogInformation("Comprehensive database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during comprehensive database seeding.");
                throw;
            }
        }
    }
}