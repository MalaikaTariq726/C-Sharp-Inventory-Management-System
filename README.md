# C-Sharp-Inventory-Management-System
C Sharp Windows Form Project with customer and admin portal whole project is in master branch 
Replace the connection string in admin.cs, customer.cs and Program.cs file with your oracle connection string Project Instructions

This project is built on the Visual Studio (Csharp ) .NET 4.7.2 framework to ensure compatibility with Oracle Database 11g. 
The primary functionality involves storing images in the database. 
Follow the steps below to set up and configure the project:

Grant Necessary Privileges:

If encountering any privileges issues, open the command prompt and connect to the database as the system administrator using the following commands:
```bash
sqlplus sys as sysdba
```
Enter your password and execute the command: 
```bash
GRANT ALL PRIVILEGES TO DBFINAL;
```
**Replace dbfinal with your oracle username.**


Uncomment the createDirectory function in Form1.cs class in constructor Change the connection string with your connection string of oracle in program.cs,Admin.cs and CustomerPage.cs file with your oracle connection string. Open the sendPassword function in the Admin.cs file and the sendotp function in the SignUp.cs file. Replace the network credentials with your specific credentials. Address Image Path Issues:

Execution:

Execute the project after completing the above steps. Images will be retrieved from the specified directory and stored in the Oracle Database. Note: Always exercise caution when working with sensitive privileges, and ensure that you have the necessary permissions on the operating system to create directories and access files.

![image](https://github.com/user-attachments/assets/c34560f7-6b90-4fa2-b250-1ff57bd60777)
