
CREATE TABLE Qualifications
(
 QualificationId INT IDENTITY PRIMARY KEY,
 QualificationName VARCHAR(50),
 Status BIT ,
 IsDeleted BIT DEFAULT 0,
 DeletedAt DATETIME DEFAULT NULL
);



Create PROCEDURE usp_CreateUpdateDeleteRestore_Qualifications
(
    @Type VARCHAR(10),   
    @QualificationId INT = NULL,   
    @QualificationName VARCHAR(50) = NULL,  
    @Status BIT = NULL,
		@IsDeleted BIT=NULL,

    @LastInsertedId INT OUTPUT -- Output parameter for the last inserted ID
 )  
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO Qualifications ( 
			QualificationName,
			Status,
			IsDeleted
		)
        VALUES (
			@QualificationName,
			@Status
			,0
		);
		SET @LastInsertedId = SCOPE_IDENTITY();
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @QualificationId IS NOT NULL
        BEGIN
            UPDATE Qualifications
            SET 
				QualificationName = @QualificationName,
                Status = @Status
            WHERE QualificationId = @QualificationId AND IsDeleted = 0;
			SET @LastInsertedId = @QualificationId;
        END
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @QualificationId IS NOT NULL
        BEGIN
            UPDATE Qualifications
            SET DeletedAt = GETDATE(),IsDeleted=1
            WHERE QualificationId = @QualificationId AND IsDeleted =0;
			SET @LastInsertedId = @QualificationId;

        END
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @QualificationId IS NOT NULL
        BEGIN
            UPDATE Qualifications
            SET DeletedAt = NULL ,IsDeleted = 0
            WHERE QualificationId = @QualificationId AND IsDeleted =1;
			SET @LastInsertedId = @QualificationId;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Invalid action specified. Valid actions are: INSERT, UPDATE, DELETE, RESTORE.', 16, 1);
    END
END;




CREATE PROCEDURE usp_GetAll_Qualifications
    @QualificationId INT = NULL,
    @QualificationName VARCHAR(255) = NULL, -- Specify the size for VARCHAR
    @Status BIT = NULL
AS
BEGIN
    SELECT 
        QualificationId, 
        QualificationName, 
        Status, 
		IsDeleted
    FROM Qualifications
    WHERE 
        (@QualificationId IS NULL OR QualificationId = @QualificationId)
        AND (@QualificationName IS NULL OR QualificationName = @QualificationName)
        AND (@Status IS NULL OR Status = @Status)
        AND DeletedAt IS NULL  AND IsDeleted=0;
END;


Create Table Documents
(
  DocumentId INT IDENTITY PRIMARY KEY,
  DocumentName VARCHAR(100) ,
  Status BIT DEFAULT 0,
  DeletedAt DATETIME DEFAULT NULL
);





Create PROCEDURE usp_CreateUpd	ateDeleteRestore_Documents
(
	@Type VARCHAR(10),
	@DocumentId INT = NULL,
    @DocumentName VARCHAR(100) =NULL,
	@Status BIT = NULL,
	@LastInsertedId INT OUTPUT
)
AS
BEGIN
		    IF @Type = 'INSERT'
			BEGIN
				INSERT INTO Documents(
					DocumentName,
					Status
				) values (
					@DocumentName,
					@Status
				);
				SET @LastInsertedId = SCOPE_IDENTITY();
			END

		    IF @Type = 'UPDATE'			-- UPDATE BY DocumentId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Documents WHERE DocumentId  = @DocumentId AND DeletedAt IS NULL)
					BEGIN
						RAISERROR('Invalid DocumentId. The specified DocumentId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				BEGIN
					UPDATE Documents SET 
						DocumentName = @DocumentName,
						Status = @Status
					WHERE 
						DocumentId = @DocumentId 
						AND  DeletedAt IS NULL;
					SET 
						@LastInsertedId = SCOPE_IDENTITY();

				END
			END

		    IF @Type = 'DELETE'			-- DELETE BY DocumentId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Documents WHERE DocumentId = @DocumentId AND DeletedAt IS NULL)
					BEGIN
						RAISERROR('Invalid DocumentId. The specified DocumentId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				UPDATE Documents SET 
					DeletedAt = GETDATE() 
				WHERE 
					DocumentId = @DocumentId 
					AND DeletedAt IS NULL
			END

		    IF @Type = 'RESTORE'			-- RESTORE BY DocumentId
			BEGIN
				UPDATE Documents SET
					DeletedAt = NULL 
				WHERE 
					DocumentId = @DocumentId;
				SET @LastInsertedId = SCOPE_IDENTITY();
			END
END;

EXEC sp_rename 'sp_GetAll_Documents', 'usp_GetAll_Documents';

CREATE PROCEDURE usp_GetAll_Documents
(
	@DocumentId INT = NULL,
	@DocumentName VARCHAR(100)=NULL ,
    @Status BIT =NULL
)
AS
	BEGIN
		SELECT 
        DocumentId, 
        DocumentName, 
        Status, 
        DeletedAt
    FROM Documents
    WHERE 
        (@DocumentId IS NULL OR DocumentId = @DocumentId)
        AND (@DocumentName IS NULL OR DocumentName = @DocumentName)
        AND (@Status IS NULL OR Status = @Status)
        AND DeletedAt IS NULL;
END;



CREATE TABLE Assets
(
	AssetId INT PRIMARY KEY IDENTITY,
	AssetName VARCHAR(50) NOT NULL,
	Status BIT DEFAULT 0,
	DeletedAt DATETIME DEFAULT NULL
);

CREATE PROCEDURE sp_CreateUpdateDeleteRestore_Assets
(
	@Type VARCHAR(10),
	@AssetId INT = NULL,
    @AssetName VARCHAR(50) =NULL,
	@Status BIT =NULL,
	@LastInsertedId INT OUTPUT -- Output parameter for the last inserted ID
)
AS
BEGIN
		    IF @Type = 'INSERT'
			BEGIN
				INSERT INTO Assets(AssetName, Status) values ( @AssetName, @Status)
				SET @LastInsertedId = SCOPE_IDENTITY();
			END

		    IF @Type = 'UPDATE'			-- UPDATE BY AssetId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Assets WHERE AssetId = @AssetId AND DeletedAt IS NULL)
					BEGIN
						RAISERROR('Invalid AssetId. The specified AssetId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				BEGIN
					UPDATE Assets SET 
					AssetName = @AssetName,
					Status = @Status
					WHERE AssetId = @AssetId AND  DeletedAt IS NULL
				END
				SET @LastInsertedId = @AssetId;


			END

		    IF @Type = 'DELETE'			-- DELETE BY AssetId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Assets WHERE AssetId = @AssetId AND DeletedAt IS NULL)
					BEGIN
						RAISERROR('Invalid AssetId. The specified AssetId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				UPDATE Assets SET DeletedAt = GETDATE() WHERE AssetId = @AssetId AND DeletedAt IS NULL
				SET @LastInsertedId = @AssetId;
			END

		    IF @Type = 'RESTORE'			-- RESTORE BY AssetId
			BEGIN
				UPDATE Assets SET DeletedAt = NULL WHERE AssetId = @AssetId
				SET @LastInsertedId = @AssetId;
			END	
END;



CREATE PROCEDURE sp_GetAll_Assets
    @AssetId INT = NULL,
    @AssetName VARCHAR(255) = NULL, -- Specify the size for VARCHAR
    @Status BIT = NULL
AS
BEGIN
    SELECT 
        AssetId, 
        AssetName, 
        Status, 
        DeletedAt
    FROM Assets
    WHERE 
        (@AssetId IS NULL OR AssetId = @AssetId)
        AND (@AssetName IS NULL OR AssetName = @AssetName)
        AND (@Status IS NULL OR Status = @Status)
        AND DeletedAt IS NULL;
END;


CREATE TABLE Projects
(
	ProjectId INT PRIMARY KEY IDENTITY,
	ProjectName VARCHAR(100) NOT NULL,
	ProjectDiscription NVARCHAR(300),
	TotalManPower INT NOT NULL,
	ProjectBudget FLOAT NOT NULL,
	Status VARCHAR(20) NOT NULL,
	Remark VARCHAR(100) DEFAULT NULL,
	ProjectStartDate DATETIME DEFAULT  NULL,
	ProjectEndDate DATETIME DEFAULT  NULL,
	IsDeleted BIT DEFAULT 0,
	DeletedAt DATETIME DEFAULT NULL,
	CONSTRAINT ENUM_Projects_Status CHECK (Status in ('Upcomping', 'Running','Completed','Cancelled'))
);


CREATE PROCEDURE usp_CreateUpdateDeleteRestore_Projects
(
	@Type VARCHAR(10),
	@ProjectId INT = 0,
    @ProjectName VARCHAR(100) =NULL,
    @ProjectDiscription NVARCHAR(300) =NULL,
	@TotalManPower INT  =NULL,
	@ProjectBudget FLOAT  =NULL,
	@Status	VARCHAR(20)  =NULL,
	@Remark VARCHAR(100) =NULL,
	@ProjectStartDate DATETIME =NULL,
	@ProjectEndDate DATETIME =NULL,
	@LastInsertedId INT OUTPUT -- Output parameter for the last inserted ID
)
AS
BEGIN
		    IF @Type = 'INSERT'
			BEGIN
				INSERT INTO Projects(ProjectName,ProjectDiscription,TotalManPower,ProjectBudget,Status,Remark,ProjectStartDate,ProjectEndDate,IsDeleted) 
				VALUES (@ProjectName,@ProjectDiscription,@TotalManPower,@ProjectBudget,@Status,@Remark,@ProjectStartDate,@ProjectEndDate,0)
				SET @LastInsertedId = SCOPE_IDENTITY();

				END

		    IF @Type = 'UPDATE'
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Projects WHERE ProjectId = @ProjectId AND IsDeleted = 0)
					BEGIN
						RAISERROR('Invalid ProjectId. The specified ProjectId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				BEGIN
					UPDATE Projects SET 
					ProjectName=@ProjectName,
					ProjectDiscription=@ProjectDiscription,
					TotalManPower=@TotalManPower,
					ProjectBudget=@ProjectBudget,
					Status=@Status,
					Remark=@Remark,
					ProjectStartDate=@ProjectStartDate,
					ProjectEndDate=@ProjectEndDate
					WHERE ProjectId = @ProjectId AND  IsDeleted = 0
					SET @LastInsertedId = @ProjectId;
				END
				

			END

		    IF @Type = 'DELETE'			-- DELETE BY ProjectId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Projects WHERE ProjectId = @ProjectId AND DeletedAt IS NULL)
					BEGIN
						RAISERROR('Invalid ProjectId. The specified ProjectId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				UPDATE Projects SET 
				DeletedAt = GETDATE(),
				IsDeleted=1
				WHERE ProjectId = @ProjectId AND IsDeleted = 0
				SET @LastInsertedId = @ProjectId;

			END

		    IF @Type = 'RESTORE'			-- RESTORE BY ProjectId
			BEGIN
				UPDATE Projects SET 
				DeletedAt = NULL,
				IsDeleted=0
				WHERE ProjectId = @ProjectId AND IsDeleted = 1
				SET @LastInsertedId = @ProjectId;

			END	
END;


CREATE PROCEDURE usp_GetAll_Projects
(
	@ProjectId INT =NULL,
    @ProjectName VARCHAR(100)=NULL,
    @ProjectDiscription NVARCHAR(300) =NULL,
	@TotalManPower INT  =NULL,
	@ProjectBudget FLOAT  =NULL,
	@Status	VARCHAR(20)  =NULL,
	@Remark VARCHAR(100) =NULL,
	@ProjectStartDate DATETIME =NULL,
	@ProjectEndDate DATETIME =NULL
)
AS
BEGIN
	
		SELECT 
			ProjectId,
			ProjectName,
			ProjectDiscription,
			TotalManPower,
			ProjectBudget,
			Status,
			Remark,
			ProjectStartDate,
			ProjectEndDate,
			IsDeleted
		FROM Projects 
		WHERE 
			 (@ProjectId IS NULL OR ProjectId = @ProjectId)
        AND (@ProjectName IS NULL OR ProjectName = @ProjectName)
        AND (@ProjectDiscription IS NULL OR ProjectDiscription = @ProjectDiscription) 
		AND (@ProjectName IS NULL OR ProjectName = @ProjectName)
		AND (@TotalManPower IS NULL OR TotalManPower = @TotalManPower)
		AND (@ProjectBudget IS NULL OR ProjectBudget = @ProjectBudget)
		AND (@Status IS NULL OR Status = @Status)
		AND (@Remark IS NULL OR Remark = @Remark)
		AND (@ProjectStartDate IS NULL OR ProjectStartDate = @ProjectStartDate)
		AND (@ProjectEndDate IS NULL OR ProjectEndDate = @ProjectEndDate)
        AND IsDeleted = 0;	
END;



CREATE TABLE Sites
(
 SiteId INT IDENTITY PRIMARY KEY,
 ProjectId INT,
 SiteName VARCHAR(50),
 SiteLocation VARCHAR(100) DEFAULT NULL,
 GpsLocation VARCHAR(100) DEFAULT NULL,
 CreatedAt DATETIME,
 Status BIT DEFAULT 1,
 IsDeleted BIT DEFAULT 0,
 DeletedAt DATETIME DEFAULT NULL
);



CREATE PROCEDURE usp_CreateUpdateDeleteRestore_Sites
    @Type VARCHAR(10),   
    @SiteId INT = NULL,  
    @ProjectId INT = NULL, 
    @SiteName VARCHAR(50) = NULL,  
    @SiteLocation VARCHAR(100) = NULL, 
    @GpsLocation VARCHAR(100) = NULL,  
    @Status BIT = NULL,
	@IsDeleted BIT=NULL,
	@LastInsertedId INT OUTPUT
   
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO Sites (ProjectId, SiteName, SiteLocation, GpsLocation, CreatedAt, Status,IsDeleted)
        VALUES (@ProjectId, @SiteName, @SiteLocation, @GpsLocation, GETDATE(), @Status,0);
		SET @LastInsertedId = SCOPE_IDENTITY();
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @SiteId IS NOT NULL
        BEGIN
            UPDATE Sites
            SET ProjectId = @ProjectId,
                SiteName = @SiteName,
                SiteLocation = @SiteLocation,
                GpsLocation = @GpsLocation,
                Status = @Status
            WHERE SiteId = @SiteId AND IsDeleted = 0;
			SET @LastInsertedId = @SiteId;
        END
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @SiteId IS NOT NULL
        BEGIN
            UPDATE Sites
            SET DeletedAt = GETDATE(),IsDeleted=1
            WHERE SiteId = @SiteId AND IsDeleted =0;
			SET @LastInsertedId = @SiteId;
        END
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @SiteId IS NOT NULL
        BEGIN
            UPDATE Sites
            SET DeletedAt = NULL,IsDeleted = 0
            WHERE SiteId = @SiteId AND IsDeleted =1;
				SET @LastInsertedId = @SiteId;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Invalid action specified. Valid actions are: INSERT, UPDATE, DELETE, RESTORE.', 16, 1);
    END
END;

=================================================== Sites GetAll Procedure ========================================

select * from projects

-- usp_GetAll_Project_Sites
CREATE PROCEDURE usp_GetAll_Sites
    @SiteId INT = NULL,
    @ProjectId INT = NULL,
    @Status BIT = NULL
AS
BEGIN
    SELECT 
        p.ProjectId,
        p.ProjectName,
        p.ProjectDiscription,
        p.TotalManPower,
        p.ProjectBudget,
        p.Status AS ProjectStatus,
        p.Remark,
        p.ProjectStartDate,
        p.ProjectEndDate,
        p.DeletedAt AS ProjectDeletedAt,
        s.SiteId,
        s.SiteName,
        s.SiteLocation,
        s.GpsLocation,
        s.CreatedAt AS SiteCreatedAt,
        s.Status AS SiteStatus
    FROM 
        Projects p
    JOIN 
        Sites s ON p.ProjectId = s.ProjectId
    WHERE
        (@SiteId IS NULL OR SiteId = @SiteId)
        AND (@ProjectId IS NULL OR p.ProjectId = @ProjectId)
        AND (@Status IS NULL OR s.Status = @Status)
END;


CREATE TABLE Departments
(
 DepartmentId INT IDENTITY PRIMARY KEY,
 DepartmentName VARCHAR(50),
 Status BIT ,	
 IsDeleted BIT DEFAULT 0,
 DeletedAt DATETIME DEFAULT NULL
);



Create PROCEDURE usp_CreateUpdateDeleteRestore_Departments
    @Type VARCHAR(10),   
    @DepartmentId INT = NULL,   
    @DepartmentName VARCHAR(50) = NULL,  
    @Status BIT = NULL,
	@IsDeleted BIT=NULL,

	@LastInsertedId INT OUTPUT
   
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO Departments ( DepartmentName,Status,IsDeleted)
        VALUES ( @DepartmentName, @Status,0);
		SET @LastInsertedId = SCOPE_IDENTITY();
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @DepartmentId IS NOT NULL
        BEGIN
            UPDATE Departments
            SET DepartmentName = @DepartmentName,
                Status = @Status
            WHERE DepartmentId = @DepartmentId AND IsDeleted = 0;
			SET @LastInsertedId = @DepartmentId;
        END
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @DepartmentId IS NOT NULL
        BEGIN
            UPDATE Departments
            SET DeletedAt = GETDATE(),IsDeleted=1
            WHERE DepartmentId = @DepartmentId AND IsDeleted =0;
			SET @LastInsertedId = @DepartmentId;
        END
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @DepartmentId IS NOT NULL
        BEGIN
            UPDATE Departments
            SET DeletedAt = NULL,IsDeleted = 0
            WHERE DepartmentId = @DepartmentId AND IsDeleted =1 ;
			SET @LastInsertedId = @DepartmentId;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Invalid action specified. Valid actions are: INSERT, UPDATE, DELETE, RESTORE.', 16, 1);
    END
END;



CREATE PROCEDURE usp_GetAll_Departments
    @DepartmentId INT = NULL,
    @DepartmentName VARCHAR(50) = NULL, -- Specify the size for VARCHAR
    @Status BIT = NULL
AS
BEGIN
    SELECT 
          DepartmentId , 
        DepartmentName, 
        Status,
		IsDeleted
    FROM Departments
    WHERE 
        (@DepartmentId IS NULL OR DepartmentId = @DepartmentId)
        AND (@DepartmentName IS NULL OR DepartmentName = @DepartmentName)
        AND (@Status IS NULL OR Status = @Status)
		AND DeletedAt IS NULL  AND IsDeleted=0

        
END;


CREATE TABLE Attendance
(
    AttendanceId INT IDENTITY PRIMARY KEY,  
    EmployeeId INT DEFAULT NULL,                       
    WorkerId INT DEFAULT NULL,                       
    CheckInTime DATETIME NOT NULL,                 
    CheckOutTime DATETIME NULL,                    
    Date DATE NOT NULL,                            
	SelfiPath VARCHAR(500),
	GeoLocation varchar(50),
	LateIn int,
	EarlyOut int,
	Remark varchar(50),
	Status VARCHAR(10),
	CONSTRAINT chk_Status CHECK (Status IN ('Absent', 'Present', 'Leave', 'Miss')),
	CreatedAt DATETIME DEFAULT NULL,
	IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL   
);


Create PROCEDURE usp_CreateUpdateDeleteRestore_Attendance
    @Type VARCHAR(10),   
    @AttendanceId INT = NULL, 
	@EmployeeId INT =  NULL,
	@CheckInTime DATETIME = NULL,
	@CheckOutTime DATETIME = NULL,
	@Date DATE = NULL,
	@SelfiPath VARCHAR(150) = NULL,
	@GeoLocation varchar(50)= NULL,
	@LateIn INT = NULL,
	@EarlyOut INT = NULL,
	@Remark VARCHAR(50) = NULL,
    @Status VARCHAR(10) = NULL,
	@IsDeleted BIT=NULL,
	@CreatedAt DATETIME = NULL,
	 @LastInsertedId INT OUTPUT
   
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO Attendance (EmployeeId,CheckInTime,CheckOutTime,Date,SelfiPath,GeoLocation,LateIn,EarlyOut,Remark,Status,CreatedAt,IsDeleted)
        VALUES ( @EmployeeId,@CheckInTime,@CheckOutTime,@Date,@SelfiPath,@GeoLocation,@LateIn,@EarlyOut,@Remark,@Status,@CreatedAt,0);
		SET @LastInsertedId = SCOPE_IDENTITY();
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @AttendanceId IS NOT NULL
        BEGIN
            UPDATE Attendance
            SET EmployeeId = @EmployeeId,
			CheckInTime=@CheckInTime,
			CheckOutTime=@CheckOutTime,
			Date=@Date,
			SelfiPath=@SelfiPath,
			GeoLocation=@GeoLocation,
			LateIn=@LateIn,
			EarlyOut=@EarlyOut,
			Remark=@Remark,
            Status = @Status,
			CreatedAt = @CreatedAt
            WHERE AttendanceId = @AttendanceId  AND IsDeleted = 0;
			SET @LastInsertedId = @AttendanceId;
        END
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @AttendanceId IS NOT NULL
        BEGIN
            UPDATE Attendance
            SET DeletedAt = GETDATE(),IsDeleted=1
            WHERE AttendanceId = @AttendanceId AND IsDeleted =0;
			SET @LastInsertedId = @AttendanceId;
        END
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @AttendanceId IS NOT NULL
        BEGIN
            UPDATE Attendance
            SET DeletedAt = NULL,IsDeleted = 0
            WHERE AttendanceId = @AttendanceId AND IsDeleted =1;
			SET @LastInsertedId = @AttendanceId;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Invalid action specified. Valid actions are: INSERT, UPDATE, DELETE, RESTORE.', 16, 1);
    END
END;

Create PROCEDURE usp_GelAll_Attendance
    @AttendanceId INT = NULL,
    @EmployeeId INT = NULL,
    @Date DATE = NULL,
    @Status VARCHAR(10) = NULL
AS
BEGIN
    SELECT 
        AttendanceId,
        EmployeeId,
        CheckInTime,
        CheckOutTime,
        Date,
        SelfiPath,
        GeoLocation,
        LateIn,
        EarlyOut,
        Remark,
        Status,
		IsDeleted
    FROM 
        Attendance
    WHERE
        (@AttendanceId IS NULL OR AttendanceId = @AttendanceId)
        AND (@EmployeeId IS NULL OR EmployeeId = @EmployeeId)
        AND (@Date IS NULL OR Date = @Date)
        AND (@Status IS NULL OR Status = @Status)AND IsDeleted=0
    ORDER BY Date DESC;
END;


create TABLE Assets
(
	AssetId INT PRIMARY KEY IDENTITY,
	AssetName VARCHAR(50) NOT NULL,
	Status BIT DEFAULT 0,
	IsDeleted BIT DEFAULT 0,
	DeletedAt DATETIME DEFAULT NULL
);


CREATE PROCEDURE usp_CreateUpdateDeleteRestore_Assets
(
	@Type VARCHAR(10),
	@AssetId INT ,
    @AssetName VARCHAR(50) =NULL,
	@Status BIT =NULL,
	@IsDeleted BIT =NULL,
	@LastInsertedId INT OUTPUT -- Output parameter for the last inserted ID
)
AS
BEGIN
		    IF @Type = 'INSERT'
			BEGIN
				INSERT INTO Assets(AssetName, Status, IsDeleted) values ( @AssetName, @Status,0)
				SET @LastInsertedId = SCOPE_IDENTITY();
			END

		    IF @Type = 'UPDATE'			-- UPDATE BY AssetId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Assets WHERE AssetId = @AssetId AND IsDeleted = 0)
					BEGIN
						RAISERROR('Invalid AssetId. The specified AssetId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				BEGIN
					UPDATE Assets SET 
					AssetName = @AssetName,
					Status = @Status
					WHERE AssetId = @AssetId  AND  IsDeleted = 0
				END
				SET @LastInsertedId = @AssetId;


			END

		    IF @Type = 'DELETE'			-- DELETE BY AssetId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Assets WHERE AssetId = @AssetId AND IsDeleted = 0)
					BEGIN
						RAISERROR('Invalid AssetId. The specified AssetId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				UPDATE Assets SET 
				DeletedAt = GETDATE(), 
				IsDeleted=1 
				WHERE AssetId = @AssetId AND IsDeleted = 0
				SET @LastInsertedId = @AssetId;
			END

		    IF @Type = 'RESTORE'			-- RESTORE BY AssetId
			BEGIN
				UPDATE Assets SET 
				DeletedAt = NULL , 
				IsDeleted=0 
				WHERE AssetId = @AssetId AND IsDeleted = 1
				SET @LastInsertedId = @AssetId;
			END	
END;


CREATE PROCEDURE usp_GetAll_Assets
    @AssetId INT = NULL,
    @AssetName VARCHAR(255) = NULL, -- Specify the size for VARCHAR
    @Status BIT = NULL
AS
BEGIN
    SELECT 
        AssetId, 
        AssetName, 
        Status, 
		IsDeleted
    FROM Assets
    WHERE 
        (@AssetId IS NULL OR AssetId = @AssetId)
        AND (@AssetName IS NULL OR AssetName = @AssetName)
        AND (@Status IS NULL OR Status = @Status)
        AND IsDeleted = 0
END;



CREATE TABLE AssetsIssues
(
 AssetsIssuesId INT IDENTITY PRIMARY KEY,
 EmployeeId INT NULL DEFAULT NULL,
 AssetId INT NULL DEFAULT NULL,
 IssuesBy INT NULL DEFAULT NULL,
 IssuesAt DATETIME,
 ReturnTo INT NULL DEFAULT NULL,
 ReturnAt DATETIME,
 IsReturnAble BIT DEFAULT 1,
 CreatedAt DATETIME,
 CreatedBy INT NULL DEFAULT NULL,
 UpdatedAt DATETIME,
 UpdatedBy INT NULL DEFAULT NULL,
 DeletedAt DATETIME,
 Remark VARCHAR(50),
 Status BIT DEFAULT 1,
 IsDeleted BIT DEFAULT 0,
 DeletedBy INT NULL DEFAULT NULL
);



Create PROCEDURE usp_CreateUpdateDeleteRestore_AssetsIssues
 @Type VARCHAR(10),   
 @AssetsIssuesId INT = NULL, 
 @EmployeeId INT= NULL,
 @AssetId INT =NULL ,
 @IssuesBy INT= NULL ,
 @IssuesAt DATETIME=NULL,
 @ReturnTo INT= NULL ,
 @ReturnAt DATETIME=NULL,
 @IsReturnAble BIT =NULL,
 @Remark VARCHAR(50)=NULL,
 @Status BIT=NULL,
 @IsDeleted BIT=NULL,
 @LastInsertedId INT OUTPUT
   
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO AssetsIssues (EmployeeId,AssetId,IssuesBy,IssuesAt,ReturnTo,ReturnAt,IsReturnAble,Remark,Status,IsDeleted)
        VALUES ( @EmployeeId,@AssetId,@IssuesBy,@IssuesAt,@ReturnTo,@ReturnAt,@IsReturnAble,@Remark,@Status,0);
		SET @LastInsertedId = SCOPE_IDENTITY();
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @AssetsIssuesId IS NOT NULL
        BEGIN
            UPDATE AssetsIssues
            SET EmployeeId = @EmployeeId,
			AssetId=@AssetId,
			IssuesBy=@IssuesBy,
			IssuesAt=@IssuesAt,
			ReturnTo=@ReturnTo,
			ReturnAt=@ReturnAt,
			IsReturnAble=@IsReturnAble,
			Remark=@Remark,
            Status = @Status
            WHERE AssetsIssuesId = @AssetsIssuesId AND IsDeleted = 0;
			SET @LastInsertedId = @AssetsIssuesId;
        END
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @AssetsIssuesId IS NOT NULL
        BEGIN
            UPDATE AssetsIssues
            SET DeletedAt = GETDATE(),IsDeleted=1
            WHERE AssetsIssuesId = @AssetsIssuesId  AND IsDeleted =0;

			SET @LastInsertedId = @AssetsIssuesId;
        END
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @AssetsIssuesId IS NOT NULL
        BEGIN
            UPDATE AssetsIssues
            SET DeletedAt = NULL ,IsDeleted = 0
            WHERE AssetsIssuesId = @AssetsIssuesId AND IsDeleted =1;
			SET @LastInsertedId = @AssetsIssuesId;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Invalid action specified. Valid actions are: INSERT, UPDATE, DELETE, RESTORE.', 16, 1);
    END
END;


CREATE PROCEDURE usp_GelAll_AssetsIssues
    @AssetsIssuesId INT = NULL, 
 @EmployeeId INT= NULL,
 @AssetId INT =NULL ,
 @IssuesBy INT= NULL ,
 @IssuesAt DATETIME=NULL,
 @ReturnTo INT= NULL ,
 @ReturnAt DATETIME=NULL,
 @IsReturnAble BIT =NULL,
 @Remark VARCHAR(50)=NULL,
 @Status BIT=NULL
AS
BEGIN
    SELECT 
 AssetsIssuesId, 
 EmployeeId ,
 AssetId,
 IssuesBy,
 IssuesAt,
 ReturnTo,
 ReturnAt,
 IsReturnAble,
 CreatedAt,
 CreatedBy,
 UpdatedAt,
 UpdatedBy,
 DeletedAt,
 DeletedBy,
 Remark,
 Status,
 IsDeleted

    FROM 
        AssetsIssues
    WHERE
        (@AssetsIssuesId IS NULL OR AssetsIssuesId = @AssetsIssuesId)
        AND (@Status IS NULL OR Status = @Status)  AND IsDeleted=0
    ORDER BY Status DESC;
END;


select * from Workers

CREATE TABLE Workers
(
	WorkerId INT IDENTITY PRIMARY KEY,
    WorkmanId VARCHAR(50) UNIQUE NOT NULL,
    FirstName VARCHAR(30),
    MiddleName VARCHAR(30) DEFAULT NULL,
    LastName VARCHAR(30),
	DepartmentId INT DEFAULT NULL,
    MarritalStatus VARCHAR(15),
	DateofBirth DATE NULL DEFAULT NULL,
	Age INT  NULL DEFAULT NULL,
	Gender varchar(8) NULL DEFAULT NULL,
	SpouseName VARCHAR(50) NULL DEFAULT NULL,
    DateofJoining DATE NULL DEFAULT NULL,
    Status BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL
);


ALTER PROCEDURE usp_CreateUpdateDeleteRestore_Workers
(
	@Type VARCHAR(10),
	@WorkerId INT =NULL,
	@WorkmanId VARCHAR(50) =NULL,
	@FirstName VARCHAR(30) =NULL,
    @MiddleName VARCHAR(30) =NULL,
    @LastName VARCHAR(30) =NULL,
	@DepartmentId INT =NULL,
    @MarritalStatus VARCHAR(15) =NULL,
	@DateofBirth DATE =NULL,
	@Age INT =NULL,
	@Gender varchar(8) =NULL,
	@SpouseName VARCHAR(50) =NULL,
    @DateofJoining DATE =NULL,
    @IsDeleted BIT =NULL,
   @LastInsertedId INT OUTPUT -- Output parameter for the last inserted ID
)
AS
BEGIN
		    IF @Type = 'INSERT'
			BEGIN
				INSERT INTO Workers (WorkmanId, FirstName,MiddleName,LastName,DepartmentId,MarritalStatus,DateofBirth,Age,Gender,SpouseName,DateofJoining,IsDeleted) 
				VALUES (@WorkmanId,@FirstName,@MiddleName, @LastName,@DepartmentId,@MarritalStatus,@DateofBirth,@Age,@Gender,@SpouseName,@DateofJoining,0)
				SET @LastInsertedId = SCOPE_IDENTITY();
			END

		    IF @Type = 'UPDATE'			-- UPDATE BY WorkerId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Workers WHERE WorkerId = @WorkerId AND IsDeleted = 0)
					BEGIN
						RAISERROR('Invalid WorkerId. The specified WorkerId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				BEGIN
					UPDATE Workers SET 
					FirstName = @FirstName,
					MiddleName = @MiddleName,
					LastName = @LastName,
					DepartmentId = @DepartmentId,
					MarritalStatus = @MarritalStatus,
					DateofBirth = @DateofBirth,
					Age = @Age,
					Gender = @Gender,
					SpouseName = @SpouseName,
					DateofJoining = @DateofJoining
					WHERE WorkerId = @WorkerId AND  IsDeleted = 0
				END
				SET @LastInsertedId = @WorkerId;


			END

		    IF @Type = 'DELETE'			-- DELETE BY WorkerId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Workers WHERE WorkerId = @WorkerId AND IsDeleted = 0)
					BEGIN
						RAISERROR('Invalid WorkerId. The specified WorkerId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				UPDATE Workers SET 
				DeletedAt = GETDATE(), 
				IsDeleted=1 
				WHERE WorkerId = @WorkerId AND IsDeleted = 0
				SET @LastInsertedId = @WorkerId;
			END

		    IF @Type = 'RESTORE'			-- RESTORE BY RoleId
			BEGIN
				UPDATE Workers SET 
				DeletedAt = NULL , 
				IsDeleted=0 
				WHERE WorkerId = @WorkerId AND IsDeleted = 1
				SET @LastInsertedId = @WorkerId;
			END	
END;


ALTER PROCEDURE usp_GetAll_Workers
(
	@WorkerId INT =NULL,
	@WorkmanId VARCHAR(50) =NULL,
	@DepartmentId INT =NULL
--	@FirstName VARCHAR(30) =NULL,
--  @MiddleName VARCHAR(30) =NULL,
--  @LastName VARCHAR(30) =NULL,
--  @MarritalStatus VARCHAR(15) =NULL,
--	@DateofBirth DATE =NULL,
--	@Age INT =NULL,
--	@Gender varchar(8) =NULL,
--	@SpouseName VARCHAR(50) =NULL,
 -- @DateofJoining DATE =NULL
)
AS
BEGIN
	SELECT 
		w.WorkerId,
		w.WorkmanId,
		w.FirstName, 
		w.MiddleName, 
		w.LastName, 
		w.DepartmentId, 
		d.DepartmentName, 
		w.MarritalStatus, 
		w.DateofBirth, 
		w.Age, 
		w.Gender, 
		w.SpouseName, 
		w.DateofJoining, 
		w.Status
	FROM Workers w 
	JOIN Departments d 
	ON w.DepartmentId=d.DepartmentId
	WHERE 
			 (@WorkerId IS NULL OR WorkerId = @WorkerId)
        AND (@WorkmanId IS NULL OR WorkmanId = @WorkmanId)
--        AND (@FirstName IS NULL OR FirstName = @FirstName) 
--		AND (@MiddleName IS NULL OR MiddleName = @MiddleName)
--		AND (@LastName IS NULL OR LastName = @LastName)
		AND (@DepartmentId IS NULL OR w.DepartmentId = @DepartmentId)
--		AND (@MarritalStatus IS NULL OR MarritalStatus = @MarritalStatus)
--		AND (@DateofBirth IS NULL OR DateofBirth = @DateofBirth)
--		AND (@Age IS NULL OR Age = @Age)
--		AND (@Gender IS NULL OR Gender = @Gender)
--		AND (@SpouseName IS NULL OR SpouseName = @SpouseName)
--		AND (@DateofJoining IS NULL OR DateofJoining = @DateofJoining)
        AND w.IsDeleted = 0
END;


CREATE TABLE WorkerProjectSites
(
	WorkerProjectSitesId INT IDENTITY PRIMARY KEY,
	WorkerId INT,
	ProjectId INT,
	SiteId INT,
    Status BIT,
    CreatedAt DATETIME DEFAULT NULL,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL
);


CREATE PROCEDURE usp_CreateUpdateDeleteRestore_WorkerProjectSite
    @Type VARCHAR(10),           
    @WorkerProjectSitesId INT = NULL,  
    @WorkerId INT = NULL,             
    @ProjectId INT = NULL,             
    @SiteId INT = NULL,                
    @Status BIT = NULL,
	@LastInsertedId INT OUTPUT
AS
BEGIN
    -- Create
    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO WorkerProjectSites (WorkerId, ProjectId, SiteId, Status, CreatedAt, IsDeleted)
        VALUES (@WorkerId, @ProjectId, @SiteId, @Status, GETDATE(), 0)
		SET @LastInsertedId = Scope_Identity();
    END

    -- Update
    ELSE IF @Type = 'Update'
    BEGIN
        UPDATE WorkerProjectSites
        SET 
            ProjectId = @ProjectId,
            SiteId = @SiteId,
            Status = @Status
        WHERE 
			WorkerProjectSitesId = @WorkerProjectSitesId 
			AND IsDeleted = 0 ;
		SET @LastInsertedId =@WorkerProjectSitesId;
    END
	

    -- Soft Delete
    ELSE IF @Type = 'Delete'
    BEGIN
        UPDATE WorkerProjectSites
        SET 
            IsDeleted = 1
        WHERE
			WorkerProjectSitesId = @WorkerProjectSitesId 
			AND IsDeleted = 0;
			SET @LastInsertedId =@WorkerProjectSitesId;
    END

    -- Restore Soft Deleted Record
    ELSE IF @Type = 'Restore'
    BEGIN
        UPDATE WorkerProjectSites
        SET 
            IsDeleted = 0
        WHERE 
			WorkerProjectSitesId = @WorkerProjectSitesId
			AND IsDeleted = 1;
			SET @LastInsertedId =@WorkerProjectSitesId;
    END
END;


CREATE PROCEDURE usp_GetAll_WorkerProjectSites
(
 @WorkerProjectSitesId INT = NULL,
 @WorkerId INT = NULL,
 @ProjectId INT = NULL,
 @SiteId INT = NULL
)
AS
BEGIN
    SELECT 
        wps.WorkerProjectSitesId,
        w.WorkerId,
		CONCAT("FirstName",' ', "MiddleName",' ',"LastName") AS WorkerName,
        wps.ProjectId,
        p.ProjectName, 
        wps.SiteId,
        s.SiteName,   
        wps.Status,
        wps.CreatedAt
    FROM 
        WorkerProjectSites wps
     JOIN 
        Workers w ON wps.WorkerId = w.WorkerId
     JOIN 
        Projects p ON wps.ProjectId = p.ProjectId
     JOIN 
        Sites s ON wps.SiteId = s.SiteId
    WHERE 
        wps.IsDeleted = 0;
END;


CREATE TABLE WorkerMobileNumbers
(
    WorkerMobileNumberId INT IDENTITY PRIMARY KEY,
    WorkerId INT NOT NULL,
    MobileNumber VARCHAR(13),
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL
);


CREATE PROCEDURE usp_CreateUpdateDeleteRestore_WorkerMobileNumbers
(
	@Type VARCHAR(10),
	@WorkerMobileNumberId INT =NULL,
	@WorkerId INT =NULL,
	@MobileNumber VARCHAR(13) =NULL,
   @LastInsertedId INT OUTPUT
)
AS
BEGIN
		    IF @Type = 'INSERT'
			BEGIN
				INSERT INTO WorkerMobileNumbers (WorkerId, MobileNumber,IsDeleted) 
				VALUES (@WorkerId,@MobileNumber,0)
				SET @LastInsertedId = SCOPE_IDENTITY();
			END

		    IF @Type = 'UPDATE'			-- UPDATE BY WorkerId
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM WorkerMobileNumbers WHERE WorkerMobileNumberId = @WorkerMobileNumberId AND IsDeleted = 0)
					BEGIN
						RAISERROR('Invalid WorkerMobileNumberId. The specified WorkerId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				BEGIN
					UPDATE WorkerMobileNumbers SET 
					MobileNumber = @MobileNumber
					WHERE 
						WorkerMobileNumberId = @WorkerMobileNumberId
						AND  IsDeleted = 0
				END
				SET @LastInsertedId = @WorkerId;


			END

		    IF @Type = 'DELETE'			
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM WorkerMobileNumbers WHERE WorkerMobileNumberId = @WorkerMobileNumberId AND IsDeleted = 0)
					BEGIN
						RAISERROR('Invalid WorkerId. The specified WorkerId does not exist.', 16, 1)
						RETURN
					END
				ELSE
				UPDATE WorkerMobileNumbers SET 
				DeletedAt = GETDATE(), 
				IsDeleted=1 
				 WHERE 
					WorkerMobileNumberId = @WorkerMobileNumberId
					AND IsDeleted = 0
				SET @LastInsertedId = @WorkerId;
			END

		    IF @Type = 'RESTORE'		
			BEGIN
				UPDATE WorkerMobileNumbers SET 
				DeletedAt = NULL , 
				IsDeleted=0 
				 WHERE
					WorkerMobileNumberId = @WorkerMobileNumberId
					AND IsDeleted = 0
				SET @LastInsertedId = @WorkerId;
			END	
END;




CREATE PROCEDURE usp_GetAll_WorkerMobileNumbers
(
 @WorkerMobileNumberId INT = NULL,
 @WorkerId INT =NULL
)
AS
BEGIN
    SELECT 
        wmn.WorkerMobileNumberId,
        w.WorkerId,
		CONCAT("FirstName",' ', "MiddleName",' ',"LastName") AS WorkerName,
        wmn.MobileNumber
    FROM 
        WorkerMobileNumbers wmn
     JOIN 
        Workers w ON wmn.WorkerId = w.WorkerId
    WHERE 
        wmn.IsDeleted = 0;
END;


CREATE TABLE WorkerQualifications
(
    WorkerQualificationId INT IDENTITY PRIMARY KEY,
    WorkerId INT NOT NULL,
    QualificationId INT NOT NULL,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL,
    
);



CREATE PROCEDURE usp_CreateUpdateDeleteRestore_WorkerQualifications
(
    @Type VARCHAR(10),
    @WorkerQualificationId INT = NULL,
    @WorkerId INT = NULL,
    @QualificationId INT = NULL,
    @LastInsertedId INT OUTPUT
)
AS
BEGIN
    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO WorkerQualifications (WorkerId, QualificationId, IsDeleted)
        VALUES (@WorkerId, @QualificationId, 0);
        SET @LastInsertedId = SCOPE_IDENTITY();
    END

    IF @Type = 'UPDATE'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM WorkerQualifications WHERE WorkerQualificationId = @WorkerQualificationId AND IsDeleted = 0)
        BEGIN
            RAISERROR('Invalid WorkerQualificationId. The specified WorkerQualificationId does not exist.', 16, 1);
            RETURN;
        END
        ELSE
        BEGIN
            UPDATE WorkerQualifications
            SET 
                QualificationId = @QualificationId
            WHERE WorkerQualificationId = @WorkerQualificationId AND IsDeleted = 0;
            SET @LastInsertedId = SCOPE_IDENTITY();
        END
    END

    IF @Type = 'DELETE'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM WorkerQualifications WHERE WorkerQualificationId = @WorkerQualificationId AND IsDeleted = 0)
        BEGIN
            RAISERROR('Invalid WorkerQualificationId. The specified WorkerQualificationId does not exist.', 16, 1);
            RETURN;
        END
        ELSE
        BEGIN
            UPDATE WorkerQualifications
            SET 
                DeletedAt = GETDATE(),
                IsDeleted = 1
            WHERE WorkerQualificationId = @WorkerQualificationId AND IsDeleted = 0;
        END
    END

    IF @Type = 'RESTORE'
        BEGIN
            UPDATE WorkerQualifications
            SET 
                DeletedAt = NULL,
                IsDeleted = 0
            WHERE WorkerQualificationId = @WorkerQualificationId AND IsDeleted = 1;
            SET @LastInsertedId = SCOPE_IDENTITY();
        END
    
END;


CREATE PROCEDURE usp_GetAll_WorkerQualifications
(
    @WorkerQualificationId INT = NULL,
    @WorkerId INT = NULL,
    @QualificationId INT = NULL
)
AS
BEGIN
    SELECT 
        wq.WorkerQualificationId,
        wq.WorkerId,
        w.WorkmanId,
        w.FirstName + ' ' + w.LastName AS WorkerName,
        wq.QualificationId,
        q.QualificationName,
        wq.IsDeleted,
        wq.DeletedAt
    FROM WorkerQualifications wq
    JOIN Workers w 
        ON wq.WorkerId = w.WorkerId
    JOIN Qualifications q 
        ON wq.QualificationId = q.QualificationId
    WHERE 
        (@WorkerQualificationId IS NULL OR wq.WorkerQualificationId = @WorkerQualificationId)
        AND (@WorkerId IS NULL OR wq.WorkerId = @WorkerId)
        AND (@QualificationId IS NULL OR wq.QualificationId = @QualificationId)
        AND wq.IsDeleted = 0
    ORDER BY wq.WorkerQualificationId DESC;
END;

select * from SiteShifts

CREATE TABLE SiteShifts
(
    SiteShiftId INT IDENTITY PRIMARY KEY,
    SiteId INT NOT NULL,
    ShiftName VARCHAR(50) NOT NULL,
    StartTime Time NOT NULL,
    EndTime Time NOT NULL,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL    
);

alter PROCEDURE usp_CreateUpdateDeleteRestore_SiteShifts
    @Type VARCHAR(10),
    @SiteShiftId INT = NULL,
    @SiteId INT = NULL,
    @ShiftName VARCHAR(50) = NULL,
    @StartTime TIME = NULL,
    @EndTime TIME = NULL,
    @LastInsertedId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'INSERT'
    BEGIN
        INSERT INTO SiteShifts (SiteId, ShiftName, StartTime, EndTime, IsDeleted)
        VALUES (@SiteId, @ShiftName, @StartTime, @EndTime, 0);
        SET @LastInsertedId = SCOPE_IDENTITY();
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @SiteShiftId IS NOT NULL
        BEGIN
            UPDATE SiteShifts
            SET SiteId = @SiteId,
                ShiftName = @ShiftName,
                StartTime = @StartTime,
                EndTime = @EndTime
            WHERE SiteShiftId = @SiteShiftId AND IsDeleted = 0;
            SET @LastInsertedId = @SiteShiftId;
        END
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @SiteShiftId IS NOT NULL
        BEGIN
            UPDATE SiteShifts
            SET DeletedAt = GETDATE(), IsDeleted = 1
            WHERE SiteShiftId = @SiteShiftId AND IsDeleted = 0;
            SET @LastInsertedId = @SiteShiftId;
        END
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @SiteShiftId IS NOT NULL
        BEGIN
            UPDATE SiteShifts
            SET DeletedAt = NULL, IsDeleted = 0
            WHERE SiteShiftId = @SiteShiftId AND IsDeleted = 1;
            SET @LastInsertedId = @SiteShiftId;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Invalid action specified. Valid actions are: INSERT, UPDATE, DELETE, RESTORE.', 16, 1);
    END
END;

CREATE PROCEDURE usp_GetAll_SiteShifts
    @SiteShiftId INT = NULL,
    @SiteId INT = NULL
AS
BEGIN
    SELECT 
    SiteShiftId,
    SiteId,
    ShiftName,
    StartTime,
    EndTime
    FROM SiteShifts
    WHERE 
        (@SiteShiftId IS NULL OR SiteShiftId = @SiteShiftId)
        AND (@SiteId IS NULL OR SiteId = @SiteId)
		AND DeletedAt IS NULL  AND IsDeleted=0        
END;






CREATE TABLE WorkerShifts
(
    WorkerShiftId INT IDENTITY PRIMARY KEY,
    WorkerId INT NOT NULL,
    ShiftName VARCHAR(25) NOT NULL,
    StartTime Time NOT NULL,
    EndTime Time NOT NULL,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL    
);


CREATE PROCEDURE usp_CreateUpdateDeleteRestore_WorkerShifts
    @Type VARCHAR(10),
    @WorkerShiftId INT = NULL,
    @WorkerId INT = NULL,
    @ShiftName VARCHAR(25) = NULL,
    @StartTime TIME = NULL,
    @EndTime TIME = NULL,
    @LastInsertedId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Type = 'CREATE'
    BEGIN
        INSERT INTO WorkerShifts (WorkerId, ShiftName, StartTime, EndTime, IsDeleted)
        VALUES (@WorkerId, @ShiftName, @StartTime, @EndTime, 0);
        SET @LastInsertedId = SCOPE_IDENTITY();
    END
    ELSE IF @Type = 'UPDATE'
    BEGIN
        IF @WorkerShiftId IS NOT NULL
        BEGIN
            UPDATE WorkerShifts
            SET WorkerId = @WorkerId,
                ShiftName = @ShiftName,
                StartTime = @StartTime,
                EndTime = @EndTime
            WHERE WorkerShiftId = @WorkerShiftId AND IsDeleted = 0;
            SET @LastInsertedId = @WorkerShiftId;
        END
    END
    ELSE IF @Type = 'DELETE'
    BEGIN
        IF @WorkerShiftId IS NOT NULL
        BEGIN
            UPDATE WorkerShifts
            SET DeletedAt = GETDATE(), IsDeleted = 1
            WHERE WorkerShiftId = @WorkerShiftId AND IsDeleted = 0;
            SET @LastInsertedId = @WorkerShiftId;
        END
    END
    ELSE IF @Type = 'RESTORE'
    BEGIN
        IF @WorkerShiftId IS NOT NULL
        BEGIN
            UPDATE WorkerShifts
            SET DeletedAt = NULL, IsDeleted = 0
            WHERE WorkerShiftId = @WorkerShiftId AND IsDeleted = 1;
            SET @LastInsertedId = @WorkerShiftId;
        END
    END
    ELSE
    BEGIN
        RAISERROR('Invalid action specified. Valid actions are: INSERT, UPDATE, DELETE, RESTORE.', 16, 1);
    END
END;



CREATE PROCEDURE usp_GetAll_WorkerShifts
    @WorkerShiftId INT = NULL
AS
BEGIN
    SELECT 
    WorkerShiftId,
    WorkerId,
    ShiftName,
    StartTime,
    EndTime
    FROM WorkerShifts
    WHERE 
        (@WorkerShiftId IS NULL OR WorkerShiftId = @WorkerShiftId)
		AND DeletedAt IS NULL  AND IsDeleted=0        
END;



CREATE TABLE WorkerAttendances
(
	WorkerAttendanceId INT IDENTITY PRIMARY KEY,
    WorkerId INT NOT NULL,
    VerifyBy INT DEFAULT NULL,
	SiteId INT NOT NULL,
	ShiftId INT NOT NULL,
	InTime	Datetime DEFAULT NULL,
	OutTime	Datetime DEFAULT NULL,
	InSelfiPath VARCHAR(500) DEFAULT NULL,
	OutSelfiPath VARCHAR(500) DEFAULT NULL,
	InGeoLocation varchar(50) DEFAULT NULL,
	OutGeoLocation varchar(50) DEFAULT NULL,
	LateIn INT DEFAULT 0,
	EarlyOut INT DEFAULT 0,
	Status VARCHAR(20),
	CONSTRAINT chk_Status CHECK (Status IN ('Absent', 'Present', 'Leave', 'Miss')),
	CreatedAt DATETIME DEFAULT NULL,
	IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME DEFAULT NULL   
);





select * from workers;
select * from WorkerQualifications;
select * from WorkerMobileNumbers;
select * from WorkerProjectSites;
exec usp_GetAll_WorkerProfile 12

ALTER PROCEDURE usp_GetAll_WorkerProfile
(
    @WorkerId INT = NULL,
    @WorkmanId VARCHAR(20) = NULL
)
AS
BEGIN
	SELECT
		w.WorkerId, w.WorkmanId, w.FirstName, w.MiddleName,w.LastName, d.DepartmentId, d.DepartmentName,
		wq.WorkerQualificationId, q.QualificationName,
		wm.WorkerMobileNumberId, wm.MobileNumber,
		p.ProjectId, p.ProjectName,
		s.SiteId, s.SiteName, s.SiteLocation, s.GpsLocation
	FROM workers as w
	JOIN Departments d ON d.DepartmentId = w.DepartmentId
	JOIN WorkerQualifications wq ON wq.WorkerId = w.WorkerId
	JOIN Qualifications q ON q.QualificationId = wq.QualificationId
	JOIN WorkerMobileNumbers wm ON wm.WorkerId = w.WorkerId
	JOIN WorkerProjectSites wps ON wps.WorkerId = w.WorkerId
	JOIN Projects p ON p.ProjectId = wps.ProjectId
	JOIN Sites s ON s.SiteId = wps.SiteId

	WHERE w.WorkerId = @WorkerId
		OR w.WorkmanId = 	@WorkmanId
END

