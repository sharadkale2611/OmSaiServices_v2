CREATE TABLE Projects (
    ProjectId INT IDENTITY(1,1) PRIMARY KEY, -- Unique identifier for each project
    ProjectName NVARCHAR(255) NOT NULL, -- Name of the project
    RequiredManPower INT NOT NULL, -- Required man power for the project
    ProjectBudget DECIMAL(18,2) NOT NULL, -- Budget for the project
    Remark NVARCHAR(500) NULL, -- Optional remarks about the project or site
    Status BIT DEFAULT 1, -- Status: 1 for active, 0 for inactive
    CreatedAt DATETIME DEFAULT GETDATE(), -- Timestamp when the record is created
    IsDeleted BIT DEFAULT 0,
	DeletedAt DATETIME NULL -- Timestamp when the record is marked deleted
);


CREATE PROCEDURE usp_CreateUpdateDeleteRestore_Projects
    @Type NVARCHAR(20), -- Action to perform: 'CREATE', 'UPDATE', 'DELETE', 'RESTORE'
    @ProjectId INT = NULL, -- Project ID (required for UPDATE, DELETE, RESTORE)
    @ProjectName NVARCHAR(255) = NULL, -- Project Name (required for CREATE, optional for UPDATE)
    @RequiredManPower INT = NULL, -- Required manpower (required for CREATE, optional for UPDATE)
    @ProjectBudget DECIMAL(18,2) = NULL, -- Project budget (required for CREATE, optional for UPDATE)
    @Remark NVARCHAR(500) = NULL -- Remark (optional for CREATE or UPDATE)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'CREATE'
    BEGIN
        INSERT INTO Projects (ProjectName, RequiredManPower, ProjectBudget, Remark, Status, CreatedAt, IsDeleted)
        VALUES (@ProjectName, @RequiredManPower, @ProjectBudget, @Remark, 1, GETDATE(), 0);

        PRINT 'Project created successfully.';
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @ProjectId IS NULL
        BEGIN
            PRINT 'ProjectId is required for UPDATE.';
            RETURN;
        END

        UPDATE Projects
        SET 
            ProjectName = COALESCE(@ProjectName, ProjectName),
            RequiredManPower = COALESCE(@RequiredManPower, RequiredManPower),
            ProjectBudget = COALESCE(@ProjectBudget, ProjectBudget),
            Remark = COALESCE(@Remark, Remark),
            Status = 1 -- Ensure the project remains active on update
        WHERE ProjectId = @ProjectId AND IsDeleted = 0;

        PRINT 'Project updated successfully.';
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @ProjectId IS NULL
        BEGIN
            PRINT 'ProjectId is required for DELETE.';
            RETURN;
        END

        UPDATE Projects
        SET 
            IsDeleted = 1,
            DeletedAt = GETDATE(),
            Status = 0 -- Mark as inactive
        WHERE ProjectId = @ProjectId AND IsDeleted = 0;

        PRINT 'Project deleted (soft delete) successfully.';
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @ProjectId IS NULL
        BEGIN
            PRINT 'ProjectId is required for RESTORE.';
            RETURN;
        END

        UPDATE Projects
        SET 
            IsDeleted = 0,
            DeletedAt = NULL,
            Status = 1 -- Mark as active
        WHERE ProjectId = @ProjectId AND IsDeleted = 1;

        PRINT 'Project restored successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Invalid Action. Please specify one of the following: CREATE, UPDATE, DELETE, RESTORE.';
    END
END;



CREATE PROCEDURE usp_GetAll_Projects
    @ProjectId INT = NULL -- Optional parameter for filtering by ID
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN
        -- Retrieve all records
        SELECT 
            ProjectID,
            ProjectName,
            RequiredManPower,
            ProjectBudget,
            Remark,
            Status,
            CreatedAt
        FROM Projects
        WHERE IsDeleted = 0 -- Include only active records
			AND (@ProjectId IS NULL OR ProjectId = @ProjectId);
    END
END;


