# Script ?? s?a l?i navigation properties
Write-Host "Fixing navigation property errors..." -ForegroundColor Cyan

# 1. Fix MediaFile.cs - X�a UserVocabularyDetail reference
$mediaFilePath = "LexiFlow.Models\Media\MediaFile.cs"
if (Test-Path $mediaFilePath) {
    Write-Host "Fixing MediaFile.cs..." -ForegroundColor Yellow
    $content = Get-Content $mediaFilePath -Raw -Encoding UTF8
    
    # X�a UserVocabularyDetaillId property
    $content = $content -replace "public int\? UserVocabularyDetaillId \{ get; set; \}", ""
    
    # X�a navigation property
    $content = $content -replace "\[ForeignKey\(""UserVocabularyDetaillId""\)\]\s*public virtual .*?UserVocabularyDetail \{ get; set; \}", ""
    
    $content | Out-File $mediaFilePath -Encoding UTF8
    Write-Host "MediaFile.cs fixed!" -ForegroundColor Green
}

# 2. Fix StudyGoal.cs - X�a Analytics references
$studyGoalPath = "LexiFlow.Models\Planning\StudyGoal.cs" 
if (Test-Path $studyGoalPath) {
    Write-Host "Fixing StudyGoal.cs..." -ForegroundColor Yellow
    $content = Get-Content $studyGoalPath -Raw -Encoding UTF8
    
    # X�a StudyReportItem v� StrengthWeakness navigation properties
    $content = $content -replace "public virtual ICollection<Analytics\.StudyReportItem> StudyReportItems \{ get; set; \}", ""
    $content = $content -replace "public virtual ICollection<Analytics\.StrengthWeakness> StrengthWeaknesses \{ get; set; \}", ""
    
    $content | Out-File $studyGoalPath -Encoding UTF8
    Write-Host "StudyGoal.cs fixed!" -ForegroundColor Green
}

# 3. Fix Category.cs - X�a PracticeSet reference
$categoryPath = "LexiFlow.Models\Learning\Vocabulary\Category.cs"
if (Test-Path $categoryPath) {
    Write-Host "Fixing Category.cs..." -ForegroundColor Yellow
    $content = Get-Content $categoryPath -Raw -Encoding UTF8
    
    # X�a PracticeSet navigation property
    $content = $content -replace "public virtual ICollection<Practice\.PracticeSet> PracticeSets \{ get; set; \}", ""
    
    $content | Out-File $categoryPath -Encoding UTF8
    Write-Host "Category.cs fixed!" -ForegroundColor Green
}

# 4. Fix UserExam.cs - X�a CustomExam v� ExamAnalytics references
$userExamPath = "LexiFlow.Models\Exam\UserExam.cs"
if (Test-Path $userExamPath) {
    Write-Host "Fixing UserExam.cs..." -ForegroundColor Yellow
    $content = Get-Content $userExamPath -Raw -Encoding UTF8
    
    # X�a CustomExam property
    $content = $content -replace "public virtual Practice\.CustomExam CustomExam \{ get; set; \}", ""
    
    # X�a ExamAnalytics navigation property 
    $content = $content -replace "public virtual ICollection<Analytics\.ExamAnalytic> ExamAnalytics \{ get; set; \}", ""
    
    # X�a CustomExamId property n?u c?n
    $content = $content -replace "public int\? CustomExamId \{ get; set; \}", ""
    
    $content | Out-File $userExamPath -Encoding UTF8
    Write-Host "UserExam.cs fixed!" -ForegroundColor Green
}

Write-Host "All navigation property errors fixed successfully!" -ForegroundColor Green