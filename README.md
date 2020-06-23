# FolderBackup

![.NET Core](https://github.com/DuAell/FolderBackup/workflows/.NET%20Core/badge.svg) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=DuAell_FolderBackup&metric=alert_status)](https://sonarcloud.io/dashboard?id=DuAell_FolderBackup)

Backup folders and restore them easily using pre-defined path configurations

## Usage
Create a file config.json inside execution directory.
File content should be like : 
```
{
    "BackupConfigurations":[
       {
          "Name":"FolderA",
          "OriginalPath":"C:\\MyFolder"
       },
       {
          "Name":"FolderB",
          "OriginalPath":"C:\\AnotherFolder"
       }
    ]
 }
 ```
