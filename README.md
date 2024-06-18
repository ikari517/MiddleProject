### Practice Project ###
+ Must read before Proejct running
  1. Edit DB Connection
     + (Root -> Practice -> App.Config -> MyDatabase.connectionString)
  3. Make Same Table
      + DDL (for MariaDB)
        ``` sql
        CREATE TABLE `tb_employee` (
          `id` int(11) NOT NULL AUTO_INCREMENT,
          `name` varchar(10) NOT NULL,
          `position` varchar(10) NOT NULL,
          `gender` tinyint(1) NOT NULL COMMENT 'Male-0, Femal-1',
          PRIMARY KEY (`id`)
        )
        ```
  

+ Project Info
  + Subject: Employee Manage
  + Using Tech Stack
    + WPF(.NET 8.0)
    + Entity Framework
    + Maria DB(11.3.2)
  + Design pattern: Mvvm
  + Action: CRUD
  + Etc: DB Connection(Entity Framework) / Exception Handling
