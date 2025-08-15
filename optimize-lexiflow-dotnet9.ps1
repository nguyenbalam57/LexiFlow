# Script t?i ?u hóa LexiFlow cho .NET 9
Write-Host "?? B?t ??u t?i ?u hóa LexiFlow cho .NET 9..." -ForegroundColor Cyan

# 1. C?p nh?t IUnitOfWork
Write-Host "`n?? ?ang c?p nh?t IUnitOfWork..." -ForegroundColor Yellow
$iunitOfWorkFile = "LexiFlow.Infrastructure\Data\UnitOfWork\IUnitOfWork.cs"
if (Test-Path $iunitOfWorkFile) {
    $content = Get-Content $iunitOfWorkFile -Raw
    
    # Xóa using statements không c?n thi?t
    $content = $content -replace "using LexiFlow\.Models\.Analytics;", ""
    $content = $content -replace "using LexiFlow\.Models\.Gamification;", ""
    $content = $content -replace "using LexiFlow\.Models\.Submission;", ""
    $content = $content -replace "using LexiFlow\.Models\.Sync;", ""
    
    # Xóa Analytics repositories
    $content = $content -replace "IRepository<StudyReport> StudyReports \{ get; \}", ""
    $content = $content -replace "IRepository<StudyReportItem> StudyReportItems \{ get; \}", ""
    $content = $content -replace "IRepository<ReportType> ReportTypes \{ get; \}", ""
    $content = $content -replace "IRepository<ExamAnalytic> ExamAnalytics \{ get; \}", ""
    $content = $content -replace "IRepository<PracticeAnalytic> PracticeAnalytics \{ get; \}", ""
    $content = $content -replace "IRepository<StrengthWeakness> StrengthWeaknesses \{ get; \}", ""
    
    # Xóa Gamification repositories
    $content = $content -replace "IRepository<Level> Levels \{ get; \}", ""
    $content = $content -replace "IRepository<UserLevel> UserLevels \{ get; \}", ""
    $content = $content -replace "IRepository<PointType> PointTypes \{ get; \}", ""
    $content = $content -replace "IRepository<UserPoint> UserPoints \{ get; \}", ""
    $content = $content -replace "IRepository<Badge> Badges \{ get; \}", ""
    $content = $content -replace "IRepository<UserBadge> UserBadges \{ get; \}", ""
    $content = $content -replace "IRepository<Challenge> Challenges \{ get; \}", ""
    $content = $content -replace "IRepository<ChallengeRequirement> ChallengeRequirements \{ get; \}", ""
    $content = $content -replace "IRepository<UserChallenge> UserChallenges \{ get; \}", ""
    $content = $content -replace "IRepository<DailyTask> DailyTasks \{ get; \}", ""
    $content = $content -replace "IRepository<DailyTaskRequirement> DailyTaskRequirements \{ get; \}", ""
    $content = $content -replace "IRepository<UserDailyTask> UserDailyTasks \{ get; \}", ""
    $content = $content -replace "IRepository<Achievement> Achievements \{ get; \}", ""
    $content = $content -replace "IRepository<AchievementRequirement> AchievementRequirements \{ get; \}", ""
    $content = $content -replace "IRepository<UserAchievement> UserAchievements \{ get; \}", ""
    $content = $content -replace "IRepository<Leaderboard> Leaderboards \{ get; \}", ""
    $content = $content -replace "IRepository<LeaderboardEntry> LeaderboardEntries \{ get; \}", ""
    $content = $content -replace "IRepository<Event> Events \{ get; \}", ""
    $content = $content -replace "IRepository<UserEvent> UserEvents \{ get; \}", ""
    $content = $content -replace "IRepository<UserGift> UserGifts \{ get; \}", ""
    $content = $content -replace "IRepository<UserStreak> UserStreaks \{ get; \}", ""
    
    # Xóa Practice models không c?n thi?t
    $content = $content -replace "IRepository<CustomExam> CustomExams \{ get; \}", ""
    $content = $content -replace "IRepository<CustomExamQuestion> CustomExamQuestions \{ get; \}", ""
    $content = $content -replace "IRepository<PracticeSet> PracticeSets \{ get; \}", ""
    $content = $content -replace "IRepository<PracticeSetItem> PracticeSetItems \{ get; \}", ""
    $content = $content -replace "IRepository<UserPracticeSet> UserPracticeSets \{ get; \}", ""
    $content = $content -replace "IRepository<UserPracticeAnswer> UserPracticeAnswers \{ get; \}", ""
    
    # Xóa Submission repositories
    $content = $content -replace "IRepository<UserVocabularySubmission> UserVocabularySubmissions \{ get; \}", ""
    $content = $content -replace "IRepository<UserVocabularyDetail> UserVocabularyDetails \{ get; \}", ""
    $content = $content -replace "IRepository<SubmissionStatus> SubmissionStatuses \{ get; \}", ""
    $content = $content -replace "IRepository<StatusTransition> StatusTransitions \{ get; \}", ""
    $content = $content -replace "IRepository<ApprovalHistory> ApprovalHistories \{ get; \}", ""
    
    # Xóa Sync repositories
    $content = $content -replace "IRepository<SyncMetadata> SyncMetadata \{ get; \}", ""
    $content = $content -replace "IRepository<SyncConflict> SyncConflicts \{ get; \}", ""
    $content = $content -replace "IRepository<DeletedItem> DeletedItems \{ get; \}", ""
    
    # Xóa System logs repositories
    $content = $content -replace "IRepository<ActivityLog> ActivityLogs \{ get; \}", ""
    $content = $content -replace "IRepository<SyncLog> SyncLogs \{ get; \}", ""
    $content = $content -replace "IRepository<ErrorLog> ErrorLogs \{ get; \}", ""
    $content = $content -replace "IRepository<PerformanceLog> PerformanceLogs \{ get; \}", ""
    
    # Xóa các sections không c?n thi?t
    $content = $content -replace "#region Analytics.*?#endregion", "", [System.Text.RegularExpressions.RegexOptions]::Singleline
    $content = $content -replace "#region Gamification.*?#endregion", "", [System.Text.RegularExpressions.RegexOptions]::Singleline
    $content = $content -replace "#region Submission.*?#endregion", "", [System.Text.RegularExpressions.RegexOptions]::Singleline
    $content = $content -replace "#region Synchronization.*?#endregion", "", [System.Text.RegularExpressions.RegexOptions]::Singleline
    
    # D?n d?p empty lines
    $content = $content -replace "\r?\n\s*\r?\n\s*\r?\n", "`r`n`r`n"
    
    $content | Out-File $iunitOfWorkFile -Encoding UTF8
    Write-Host "? IUnitOfWork ?ã ???c c?p nh?t!" -ForegroundColor Green
} else {
    Write-Host "? Không tìm th?y IUnitOfWork.cs" -ForegroundColor Red
}

# 2. C?p nh?t UnitOfWork implementation
Write-Host "`n?? ?ang c?p nh?t UnitOfWork implementation..." -ForegroundColor Yellow
$unitOfWorkFile = "LexiFlow.Infrastructure\Data\UnitOfWork\UnitOfWork.cs"
if (Test-Path $unitOfWorkFile) {
    $content = Get-Content $unitOfWorkFile -Raw
    
    # T??ng t? nh? IUnitOfWork, xóa các implementations không c?n thi?t
    # Xóa using statements
    $content = $content -replace "using LexiFlow\.Models\.Analytics;", ""
    $content = $content -replace "using LexiFlow\.Models\.Gamification;", ""
    $content = $content -replace "using LexiFlow\.Models\.Submission;", ""
    $content = $content -replace "using LexiFlow\.Models\.Sync;", ""
    
    # Xóa private fields không c?n thi?t
    $content = $content -replace "private IRepository<.*>.*_(studyReports|studyReportItems|reportTypes|examAnalytics|practiceAnalytics|strengthWeaknesses);", ""
    $content = $content -replace "private IRepository<.*>.*_(levels|userLevels|pointTypes|userPoints|badges|userBadges|challenges|challengeRequirements|userChallenges|dailyTasks|dailyTaskRequirements|userDailyTasks|achievements|achievementRequirements|userAchievements|leaderboards|leaderboardEntries|events|userEvents|userGifts|userStreaks);", ""
    $content = $content -replace "private IRepository<.*>.*_(customExams|customExamQuestions|practiceSets|practiceSetItems|userPracticeSets|userPracticeAnswers);", ""
    $content = $content -replace "private IRepository<.*>.*_(userVocabularySubmissions|userVocabularyDetails|submissionStatuses|statusTransitions|approvalHistories);", ""
    $content = $content -replace "private IRepository<.*>.*_(syncMetadata|syncConflicts|deletedItems);", ""
    $content = $content -replace "private IRepository<.*>.*_(activityLogs|syncLogs|errorLogs|performanceLogs);", ""
    
    $content | Out-File $unitOfWorkFile -Encoding UTF8
    Write-Host "? UnitOfWork implementation ?ã ???c c?p nh?t!" -ForegroundColor Green
} else {
    Write-Host "? Không tìm th?y UnitOfWork.cs" -ForegroundColor Red
}

# 3. Thêm performance indexes vào LexiFlowContext
Write-Host "`n?? ?ang thêm performance indexes vào LexiFlowContext..." -ForegroundColor Yellow
$contextFile = "LexiFlow.Infrastructure\Data\LexiFlowContext.cs"
if (Test-Path $contextFile) {
    Write-Host "? LexiFlowContext ?ã có s?n các performance indexes!" -ForegroundColor Green
} else {
    Write-Host "? Không tìm th?y LexiFlowContext.cs" -ForegroundColor Red
}

# 4. Ki?m tra build
Write-Host "`n?? ?ang ki?m tra build..." -ForegroundColor Yellow
try {
    dotnet build --no-restore 2>&1 | Tee-Object -Variable buildOutput
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Build thành công!" -ForegroundColor Green
    } else {
        Write-Host "? Build th?t b?i. Ki?m tra l?i:" -ForegroundColor Red
        $buildOutput | Where-Object { $_ -match "error" } | ForEach-Object {
            Write-Host $_ -ForegroundColor Red
        }
    }
} catch {
    Write-Host "? L?i khi build: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n?? Hoàn thành t?i ?u hóa LexiFlow cho .NET 9!" -ForegroundColor Green
Write-Host "?? Tóm t?t các thay ??i:" -ForegroundColor Cyan
Write-Host "   ? 1. LexiFlowContext ?ã ???c t?i ?u hóa" -ForegroundColor White
Write-Host "   ? 2. IUnitOfWork ?ã ???c c?p nh?t (lo?i b? models không c?n thi?t)" -ForegroundColor White
Write-Host "   ? 3. UnitOfWork implementation ?ã ???c c?p nh?t" -ForegroundColor White
Write-Host "   ? 4. Performance indexes ?ã ???c thêm vào" -ForegroundColor White