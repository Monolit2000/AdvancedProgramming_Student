# Grading

* 'dst' - tasks from 1 to 3.
* 'dst plus' - tasks from 1 to 4.
* 'db' - tasks from 1 to 5.
* 'db plus' - tasks from 1 to 6.
* 'bdb' - tasks from 1 to 7.

-----------------------------------

# Solutions

**The code should produce no warnings at all.**

Solutions should be sent to email address as links to a repository in GitHub before 2023/09/15: 

leslaw dot pawlaczyk at chorzow dot wsb dot pl

Soution for Task1 should be placed in a separate folder to other tasks (task2 and task3) which are specified in:

* https://github.com/palles77/AdvancedProgramming2/blob/main/Task2.md, 
* https://github.com/palles77/AdvancedProgramming2/blob/main/Task3.md

-----------------------------------

1. Add four choces new properties to the Student entity:
* Gender
* Place of Birth
* Place of Residence (town, city or village data would be entered here)
* Address Line 1
* Address Line 2
* Postal Code
These properties should be rendered in Student related controls and views.

-----------------------------------

2. Rename Subject entity to Course and modify it such that it has the following properties:

* Course Code (string)
* Title (string)
* Instructor (string)
* Schedule (string)
* Description (string)
* Credits (integer)
* Department (string)
* Prerequisites (list of strings)

-----------------------------------

3. Add two chosen of the following entities into database and the GUI. 
Imporant notes:
* You should base your code on existing examples (it could be a Student entity, views, and view models).
* All the data types and services should have corresponding interfaces.

## FacultyMember:
* faculty_id (string)
* name (string)
* age (integer)
* gender (string)
* department (string)
* position (string)
* email (string)
* office_room_number (string)

## Book:
* book_id (string)
* title (string)
* author (string)
* publisher (string)
* publication_date (date)
* isbn (string)
* genre (string)
* description (string)

## ResearchProject:
* project_id (string)
* title (string)
* description (string)
* team_members (list of strings)
* supervisor (string)
* start_date (date)
* end_date (date)
* budget (float)

## Classroom:
* classroom_id (string)
* location (string)
* capacity (integer)
* available_seats (integer)
* projector (boolean)
* whiteboard (boolean)
* microphone (boolean)
* description (string)

## Library:
* name (string)
* address (string)
* number_of_floors (integer)
* number_of_rooms (integer)
* description (string)
* facilities (list of strings)
* librarian (string)

## AthleticsFacility:
* facility_id (string)
* name (string)
* location (string)
* type (string)
* description (string)
* availability (list of strings)
* equipment (list of strings)
* capacity (integer)

## StudentOrganization:
* org_id (string)
* name (string)
* advisor (string)
* president (string)
* description (string)
* meeting_schedule (string)
* membership (list of strings)
* email (string)

## Exam:
* exam_id (string)
* course_code (string)
* date (date)
* start_time (time)
* end_time (time)
* location (string)
* description (string)
* professor (string)

-----------------------------------

4. Add unit tests for the services created in task 1, 2, 3.
* Unit tests should be written in existing library called University.Tests.
* You can base your code on existing example.

-----------------------------------

5. Add support for one of the chosen many to many relationships. Make sure you have a support for the entities used in such mapping.

* FacultyMembers and ResearchProjects (a faculty member could supervise multiple research projects, and a research project could have multiple faculty members on its team)
* Books and Courses (a book could be used in multiple courses, and a course could require multiple books)
* StudentOrganizations and Students (an organization could have multiple members, and a student could belong to multiple organizations)
* Exams and Courses (a course could have multiple exams, and an exam could be given in multiple courses)
* AthleticsFacilities and FacultyMembers (a faculty member could use multiple facilities for their research or recreation, and a facility could be used by multiple faculty members)
* ResearchProjects and Students (a research project could have multiple undergraduate students working on it, and a student could work on multiple research projects)
* Books and Libraries (a library could have multiple copies of the same book, and a book could be found in multiple libraries)
* StudentOrganizations and Students (a graduate student could belong to multiple organizations, and an organization could have multiple graduate student members)
* Exams and Students (a graduate student could take multiple exams, and an exam could be taken by multiple graduate students)
* AthleticsFacilities and Students (an undergraduate student could use multiple facilities for their recreation, and a facility could be used by multiple undergraduate students)

-----------------------------------

6. Add support for one more entity from task 3.

-----------------------------------

7. Add support for one more entity from task 5.

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

