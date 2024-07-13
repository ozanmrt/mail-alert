# C# Mail Alert Program

![ma1](https://github.com/user-attachments/assets/2141368e-372a-406a-aeb2-0f4ae1392acb)

![ma2](https://github.com/user-attachments/assets/6b7a2874-2bbd-406b-886a-88576fcf3098)


### Working Logic

- If the subject of an incoming email contains any word from the Critical Words screen, the email is marked as critical.
- When a critical email arrives, an alarm starts to sound and continues until the email is read.
- If the critical email is not viewed within 30 minutes, a warning email is sent to a specified email group. (**Who the email is sent to and the duration before it is sent can be configured in the program's source code.**)
- After the critical email is read, its status is automatically changed to "In Progress."
- After the issue is resolved, the Responsible Person and Description fields must be filled out, and the status is changed to "Completed." The Responsible Person and Description fields are mandatory. (Responsible persons can be configured in the program's source code.)
- Once the necessary information is filled out, the Report button is clicked to save the report to the database.



> [!WARNING]
> For the program to function, Outlook must be open, and the necessary files for the SQL Database must be present.



### for MsSQL;
```
CREATE DATABASE MailUyari;
GO
```

```
USE MailUyari;
GO

CREATE TABLE kritik (
    kritikKelime NVARCHAR(255) NOT NULL
);

CREATE TABLE onemli (
    onemliKelime NVARCHAR(255) NOT NULL
);
```

```
INSERT INTO kritik (kritikKelime) VALUES ('import');
INSERT INTO kritik (kritikKelime) VALUES ('critical');

INSERT INTO onemli (onemliKelime) VALUES ('meeting');
INSERT INTO onemli (onemliKelime) VALUES ('rapor');
```

```
CREATE TABLE Mails (
    ID INT PRIMARY KEY IDENTITY(1,1),
    OkunduBilgisi BIT,
    Konu NVARCHAR(MAX),
    Gonderen NVARCHAR(MAX),
    Govde NVARCHAR(MAX),
    Tarih NVARCHAR(MAX),
    Calisma_Durumu NVARCHAR(MAX),
    OtuzDK BIT
);
```


> [!NOTE]
> Finally, you can use the program by customizing the **baglanti** variable and **MailGonder** method.
