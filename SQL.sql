CREATE DATABASE ERP_System;
GO

USE ERP_System;
GO

--������� ���������
CREATE TABLE Unit (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

--������
CREATE TABLE Resource (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

--�������� �����������
CREATE TABLE ReceiptDocument (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Number NVARCHAR(50) NOT NULL UNIQUE,
    Date DATE NOT NULL
);


--������ �����������
CREATE TABLE ReceiptResource (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ResourceId INT NOT NULL,
    UnitId INT NOT NULL,
    Quantity DECIMAL(15, 3) NOT NULL CHECK (Quantity > 0),
    ReceiptDocumentId INT NOT NULL,

    CONSTRAINT FK_ReceiptResource_Resource FOREIGN KEY (ResourceId)
        REFERENCES Resource(Id),

    CONSTRAINT FK_ReceiptResource_Unit FOREIGN KEY (UnitId)
        REFERENCES Unit(Id),

    CONSTRAINT FK_ReceiptResource_ReceiptDocument FOREIGN KEY (ReceiptDocumentId)
        REFERENCES ReceiptDocument(Id)
        ON DELETE CASCADE
);