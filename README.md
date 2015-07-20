# Event Sourcing And CQRS

This project is focused on implementation of the patterns Command & Query Responsibility Segregation and the 
Event Sourcing in a unique library.


###CQRS

CQRS stands for Command Query Responsibility Segregation. It's a pattern that I first heard described by Greg Young. 
At its heart is the notion that you can use a different model to update information than the model you use to read information. 
For some situations, this separation can be valuable, but beware that for most systems CQRS adds risky complexity.

The mainstream approach people use for interacting with an information system is to treat it as a CRUD datastore.
By this I mean that we have mental model of some record structure where we can create new records, read records, 
update existing records, and delete records when we're done with them. In the simplest case, our interactions are all about 
storing and retrieving these records.



###Event Sourcing

Use an append-only store to record the full series of events that describe actions taken on data in a domain, rather than storing just the current state, so that the store can be used to materialize the domain objects. This pattern can simplify tasks in complex domains by avoiding the requirement to synchronize the data model and the business domain; improve performance, scalability, and responsiveness; provide consistency for transactional data; and maintain full audit trails and history that may enable compensating actions.

##When to use it

Like any pattern, CQRS is useful in some places, but not in others. Many systems do fit a CRUD mental model, and so should be 
done in that style. CQRS is a significant mental leap for all concerned, so shouldn't be tackled unless the benefit is worth 
the jump. While I have come across successful uses of CQRS, so far the majority of cases I've run into have not been so good, 
with CQRS seen as a significant force for getting a software system into serious difficulties.

In particular CQRS should only be used on specific portions of a system (a Bounded Context in DDD lingo) and not the system as
a whole. In this way of thinking, each Bounded Context needs its own decisions on how it should be modeled.

##Installation
  TODO

##How to use
  TODO
