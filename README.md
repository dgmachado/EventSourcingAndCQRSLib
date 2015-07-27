# The Event Sourcing And CQRS Library

This project is focused on implementation of a library to help you to implement the patterns Command & Query Responsibility Segregation and the Event Sourcing. It can be very useful to implement the pattern DDD and your boundaries contexts.

The goal of Domain-Driven Design (DDD) is to decompose a complex business domain into manageable components taking into account scalability and consistency requirements, and is combined with CQRS great for building software based on concepts like bounded contexts, transaction boundaries and event based communication.

To understand better the themes DDD, CQRS and Event Sourcing, please read the following articles:

- [Link1](http://cqrs.nu/)
- [Link2](http://www.kenneth-truyers.net/2013/12/05/introduction-to-domain-driven-design-cqrs-and-event-sourcing/)

##When to use it

Like any pattern, CQRS and Event Sourcing is useful in some places, but not in others. Many systems do fit a CRUD mental model, and so should be done in that style. CQRS and Event Sourcing is a significant mental leap for all concerned, so shouldn't be tackled unless the benefit is worth the jump. 

In particular this should only be used on specific portions of a system (a Bounded Context in DDD lingo) and not the system as a whole. In this way of thinking, each Bounded Context needs its own decisions on how it should be modeled.

##Installation
  TODO

##How to use
  TODO
