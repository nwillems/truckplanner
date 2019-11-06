# truckplanner
A small system to plan trucks and extract data

# Run the stuff
The project has mainly been tested on unixy systems, and will run the included
tests by running the following command:

```
dotnet test
```

On the todo list is to dockerize the setup and add a "mocked"
CountryService(expect python).

**DISCLAMER** The code currently throws a rather large exception. Thats because
it can't find a country service. Once dockerized, the country service will be
added in a docker-compose setup.

# Description 
The problem: A TruckPlan describes a single Driver driving a Truck for a
continuous period. Each Truck has a GPS device, that provides updates
approximately every 5 minutes.

The solution tries to solve three problems, by designing an appropriate data
model:
* Calculate distance driven for a single TruckPlan
* Allow the data model to have an "external" dependency, specifically categories
  locations with their country
* Allow advanced querying over the model

## Design

It was chosen to separate the model classes out into a separate module, such
that it can be shared across the solution. This could also have been achieved
by placing the files into the same project as "the application", however, great
plans lies ahead for the TruckPlan service, such as WebService, VC funding and finally
IPO, hence it seemed ideal to separate it.

A lack of recent experience with dotNet and visual studio, has however hampered
the effort, since it was chosen to use "-" instead of "." in the project names...

The data model, is implemented as straightforward as possible from the problem
description. The coordinates submitted for each truck, was decided to naturally
be stored with the truck, as it is where they belong. This will also in the
future allow us to extract Trucks from this context and into it's own context.

To ensure durability, EntityFrameworkCore was chosen, which has also influenced
some of the design choices for the model. Eg. the model was originally intended
to be based on immutable objects and therefore also be append only, this was
not initially feasible with EFc.

Distance calculation and country categorising, has been made as Services and
put into the business module. This felt naturally, as they are neither specific
for the application(user facing) nor for the data model itself. Rather they
present business objectives that we would like to achieve, using the model, and
present in the App.

A small emphasis has been put on testing, or at least providing reasonable
examples of the individual classes usage. As a next step, it would seem obvious
to add an integration test between a "real" country-service and the
truckplanner, this could eg be done with contract testing using
pact(https://docs.pact.io/).

As the code for the app, doing the final "advanced querying" shows, a general
functional approach has been taken. In some ways its a little iffy, but it
mostly seems like a shorter and more integrated way of querying the data model,
rather than having to express it directly in SQL queries. Raw queries however
could provide the speed needed, at a later stage.

Generally notes on future plans, and ways of improving the code are sprinkled
throughout. Feel free to raise an issue with questions :-)


