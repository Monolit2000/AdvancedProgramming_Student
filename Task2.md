# Grading

* 'dst' - tasks from 1 to 5.
* 'dst plus' - tasks from 1 to 6.
* 'db' - tasks from 1 to 7.
* 'db plus' - tasks from 1 to 8.
* 'bdb' - tasks from 1 to 9.

-----------------------------------

# Solutions

**The code should produce no warnings at all.**

Solutions should be sent to email address as links to a repository in GitHub before 2023/09/15: 

leslaw dot pawlaczyk at chorzow dot wsb dot pl

Soution for Task1 should be placed in a separate folder to other tasks (task2 and task3) which are specified in:

* https://github.com/palles77/AdvancedProgramming2/blob/main/Task1.md, 
* https://github.com/palles77/AdvancedProgramming2/blob/main/Task3.md

-----------------------------------

# Assumptions

To start developing solution for Task 2 you need to finish Task 1 first.

-----------------------------------

1. Develop a new service called DataAccessService.cs which will be responsible for data manipulation on the database. 
* the service should allow saving data in a JSON file
* the service should allow reading data from a JSON file
* The service should be based on a new interface called IDataAccessService

-----------------------------------

2. Write unit tests for DataAccessService in a separate library called University.Services.Tests
* You should use mocking framework of your choice. More information about mocking can be found in the literature further down this document.

-----------------------------------

3. Extract interfaces for all of the entity types in the solution. The interfaces should be placed in a separate library University.Interfaces.

-----------------------------------

4. Modify existing ViewModels to use DataAccessService instead of performing business logic on database directly in ViewModels. 
* Use dependency injection for that purpose.
* More information about dependency injection can be found in literature further down this document

-----------------------------------

5. Write unit tests for the modified ViewModels from point 4.
* The unit tests should be placed in a separate library called Univerity.ViewModels.Tests
* Use mocking framework of your choice.

-----------------------------------

6. Extract validation of PESEL and date of birth into a separate service called ValidationService. Add interface for it. Use dependency injection.

-----------------------------------

7. Write unit tests for point 6.

-----------------------------------

8. Move all of the hardcoded texts associated with GUI into a separate library called University.Internationalization
* write down the English names in a separate file
* write down the Polish names in a separate file.
* develop a separate service for this purpose

-----------------------------------

9. Add a settings service which will be saving program settings in a separate file called settings.json
* the service should have an interface
* the service should have unit tests.

-----------------------------------

# Literature:

* [WPF-Polish](https://github.com/palles77/AdvancedProgramming2/blob/main/wpf-en-7.0.pdf)
* [WPF-English](https://github.com/palles77/AdvancedProgramming2/blob/main/wpf-en-7.0.pdf)
* [.NET-Polish](https://github.com/palles77/AdvancedProgramming2/blob/main/dotnet-fundamentals-pl.pdf)
* [.NET-English](https://github.com/palles77/AdvancedProgramming2/blob/main/dotnet-fundamentals-en.pdf)
* https://learn.microsoft.com/pl-pl/dotnet/core/testing/unit-testing-with-dotnet-test
* https://learn.microsoft.com/pl-pl/dotnet/architecture/modern-web-apps-azure/architectural-principles
* https://learn.microsoft.com/pl-pl/dotnet/communitytoolkit/mvvm/
* https://learn.microsoft.com/pl-pl/ef/

# Last Modified

2023/04/15

