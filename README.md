# Food Delivery System: A simplistic food delivery, built with a microservices pattern.

The purpose of this project is educational and throughout implementing this project, we will be able to learn and practice all studied concepts of microservices architecture as well as a few common design patterns.

## 1. Introduction

This simple food delivery project is intended to offer an integrated system in which restaurant owners can create menus
and then users can order food and get the food delivered to them.

The restaurant owners will have an exclusive panel for managing food availability and monitor orders.

On the other hand, users also have a different panel for placing order and reviewing the history of their orders.

## 2. Tech Stacks and Decided Infrastructres

This is a .Net based project but it is likely to include some other technologies thank to the microservices platform independent feature.

 - The main tech stack used in this project is **Microsoft .Net** with the latest version possible.

 - Most of the APIs and services will be built with **ASP.Net core** and will be containerized with **Docker containers**.

 - Orchestration will be possible using **Kubernetes**.

 - The message broker will be **RabbitMQ**.

 - As of now, relational database will be **PostgreSQL** and document base database will be **MongoDB**. (TBD)

## 3. Road Map

To make the progress simple, the project is split into a few phases.

**Note:** This road map is highly possible to change in the future

### Phase I
 - [ ] Restaurant owner panel for managing supply.
 - [ ] User portal for placing order and manage their data.
 - [ ] Ordering.
 - [ ] Public web page for searching restaurants and see menus
 - [ ] Generic static file manager.

### Phase II
 - [ ] Management of delivery vehicles.
 - [ ] Handling deliveries.
 - [ ] Payments and invoicing.

### Phase III
 - [ ] Rating and reviews.
 - [ ] Analytics for admin.

### Phase IV
 - [ ] Blogging and content building system.

## 4. Why Microservices?

Microservices architecture concepts are both complex and complicated. You might heard this famous quote that says:

> The best advice on microservices, is not to go microservices.

The reason is quite simple; The architecture introduces many complexities and new challenges while they are nonexistent in a monolithic system.

Each design pattern and architecture are like medicines I believe; They have some side effects while solving a problem.
We need to make sure what are we trading off when implementing those.
Of course, it wouldn't make sense to implement a design pattern if we don't have any problem that they solve.
This way we gain nothing and at the same time, we are creating new problems.

Starting a project with a microservices pattern from scratch is also considered as a foul.

With all being said, Microservices are still a powerful tool for large-scale projects.

Adding this powerful tool to our toolbox requires practice and some trials and errors.

Starting educational projects like this can assist us to grasp the concepts by trying them out, facing the challenges, and solving them.
