-- Create CourseAuditLog table if not exists
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CourseAuditLog' AND xtype='U')
BEGIN
    CREATE TABLE CourseAuditLog (
        Id INT IDENTITY PRIMARY KEY,
        CourseId INT,
        Name NVARCHAR(100),
        DeletedAt DATETIME,
        DeletedBy NVARCHAR(450)
    );
END;

GO

-- Create or alter the sp_Course_Manage procedure
CREATE OR ALTER PROCEDURE sp_Course_Manage
    @Id INT,
    @Name NVARCHAR(100),
    @Description NVARCHAR(1000),
    @TeacherId NVARCHAR(450),
    @IsDelete BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF @Id = 0
    BEGIN
        INSERT INTO Courses (Name, Description, CreatedDate, TeacherId)
        VALUES (@Name, @Description, GETUTCDATE(), @TeacherId);
    END
    ELSE
    BEGIN
        IF @IsDelete = 1
        BEGIN
            DELETE FROM Courses WHERE Id = @Id;
        END
        ELSE
        BEGIN
            UPDATE Courses
            SET Name = @Name,
                Description = @Description,
                TeacherId = @TeacherId
            WHERE Id = @Id;
        END
    END
END;

GO

-- Function
CREATE OR ALTER FUNCTION fn_GetChapterCount (@CourseId INT)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) FROM Chapters WHERE CourseId = @CourseId;
    RETURN @Count;
END;

GO

-- Trigger
CREATE OR ALTER TRIGGER trg_CourseDeleteLog
ON Courses
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO CourseAuditLog (CourseId, Name, DeletedAt, DeletedBy)
    SELECT 
        d.Id, d.Name, GETUTCDATE(), SYSTEM_USER
    FROM deleted d;
END;

GO

-- View
CREATE OR ALTER VIEW vw_CourseSummary AS
SELECT
    c.Id AS CourseId,
    c.Name AS CourseName,
    c.Description,
    c.TeacherId,
    u.UserName AS TeacherName,
    dbo.fn_GetChapterCount(c.Id) AS ChapterCount,
    (
        SELECT COUNT(*) 
        FROM UserCourses uc 
        WHERE uc.CourseId = c.Id
    ) AS EnrollmentCount
FROM Courses c
LEFT JOIN AspNetUsers u ON u.Id = c.TeacherId;

GO